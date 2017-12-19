using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dmake.Stages
{
	internal class Compiler
	{
		public static bool useMinGW = true;
		public static bool forceCPPOnC = false;
		public static int timeout = 60;

		public string name;
		public string path;

		public Compiler ( string name, string path )
		{
			this.name = name;

			if (!useMinGW)
			{
				this.path = name;
			}
			else
			{
				this.path = path;
			}
		}

		public void CompileSourceFile ( SourceFile file, string outputDirectory )
		{
			Logger.ProgressInfo ($"Compiling source file '{Path.GetFileName (file.path)}'");

			var procInfo = new ProcessStartInfo
			{
				FileName = $"\"{path}\"",
				Arguments = $"-c \"{file.path}\" -o \"{outputDirectory}/{file.name}.o\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
			};

			Logger.Verbose ($"{procInfo.FileName} {procInfo.Arguments}");

			Process process = Process.Start (procInfo);

			process.WaitForExit (1000 * timeout);

			process.Close ();
		}

		public static async Task LinkProject ( Project project )
		{
			string outputFile = project.outputFile;

			Logger.ProgressInfo ($"Linking object files -> '{Path.GetFileName (outputFile)}'");

			string filesConcat = "";

			foreach (string objFile in Directory.GetFiles (project.outputDirectory))
			{
				if (objFile.EndsWith (".o"))
				{
					filesConcat += $"\"{objFile}\" ";
				}
			}

			filesConcat.TrimEnd (' ');

			var procInfo = new ProcessStartInfo
			{
				FileName = $"\"{Platform.cpp}\"",
				Arguments = $"{filesConcat} -o \"{outputFile}\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
			};

			Logger.Verbose ($"{procInfo.FileName} {procInfo.Arguments}");

			Process process = Process.Start (procInfo);

			process.WaitForExit (1000 * timeout);

			foreach (string objFile in Directory.GetFiles (project.outputDirectory))
			{
				if (objFile.EndsWith (".o"))
				{
					File.Delete (objFile);
				}
			}
		}

		public static readonly Compiler C = new Compiler ("C", Platform.c);
		public static readonly Compiler CPP = new Compiler ("C++", Platform.cpp);

		public static int maxThreads = 4;
		public static List<Thread> threadPool = new List<Thread> ();
		private static Object threadPoolLock = new Object ();

		private static void FreeThread ( Thread thread )
		{
			lock (threadPoolLock)
			{
				Debug.Print ("Removing thread from thread pool.");
				threadPool.Remove (thread);
				Debug.Print ($"Thread pool size is now {threadPool.Count}");
			}
		}

		public static void ExecuteThreaded ( Action function )
		{
			// If there are no free threads, sleep.
			while (threadPool.Count >= maxThreads)
			{
				Thread.Sleep (10);
			}

			Thread thread = null;
			thread = new Thread (() =>
			{
				try
				{
					// Actually execute the function
					function.Invoke ();
				}
				catch (Exception e)
				{
					Logger.Warning ($"Exception in compile thread:\n\t{e.Message}");
				}
				finally
				{
					// Release the thread from the thread pool
					FreeThread (thread);
				}
			});

			// Add the thread to the thread pool
			threadPool.Add (thread);

			// Start the thread
			thread.Start ();
		}

		public static async Task CompileProject ( Project project )
		{
			if (File.Exists (project.outputFile))
			{
				Logger.Warning ("Deleting old binary file.");
				File.Delete (project.outputFile);
			}

			foreach (SourceFile source in project.files)
			{
				if (source.type == SourceType.C)
				{
					if (forceCPPOnC)
					{
						// Use C++ compiler on C code
						ExecuteThreaded (() => CPP.CompileSourceFile (source, project.outputDirectory));
					}
					else
					{
						// Use C compiler on C code
						ExecuteThreaded (() => C.CompileSourceFile (source, project.outputDirectory));
					}
				}
				else if (source.type == SourceType.CPP)
				{
					ExecuteThreaded (() => CPP.CompileSourceFile (source, project.outputDirectory));
				}
			}

			while (threadPool.Count > 0)
			{
				Thread.Sleep (10);
			}

			Logger.Verbose ($"All worker threads completed.");
		}
	}
}