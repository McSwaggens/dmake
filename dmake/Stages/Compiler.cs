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
		public static bool verbose = true;
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
			Console.WriteLine ($"Compiling source file \"{Path.GetFileName (file.path)}\"...");

			var procInfo = new ProcessStartInfo
			{
				FileName = $"\"{path}\"",
				Arguments = $"-c \"{file.path}\" -o \"{outputDirectory}/{file.name}.o\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
			};

			if (verbose)
			{
				Console.WriteLine ($"[Verbose] {procInfo.FileName} {procInfo.Arguments}");
			}

			Process process = Process.Start (procInfo);

			process.WaitForExit (1000 * timeout);
		}

		public static async Task LinkProject ( Project project )
		{
			string outputFile = $"{project.outputDirectory}/{project.name}.exe";

			Console.WriteLine ($"Linking object files -> {outputFile}");

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
				FileName = $"\"{Util.MinGWPath}/bin/g++.exe\"",
				Arguments = $"{filesConcat} -o \"{outputFile}\"",
				UseShellExecute = false,
				RedirectStandardOutput = true,
			};

			if (verbose)
			{
				Console.WriteLine ($"[Verbose] {procInfo.FileName} {procInfo.Arguments}");
			}

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

		public static readonly Compiler C = new Compiler ("C", $"{Util.MinGWPath}/bin/gcc");
		public static readonly Compiler CPP = new Compiler ("C++", $"{Util.MinGWPath}/bin/g++");

		public static int maxThreads = 4;
		public static List<Thread> threadPool = new List<Thread> ();

		public static void ExecuteThreaded ( Action function )
		{
			// If there are no free threads, sleep.
			while (threadPool.Count >= maxThreads)
			{
				Thread.Sleep (10);
			}

			Thread thread = new Thread (() =>
			{
				try
				{
					// Actually execute the function
					function.Invoke ();
				}
				catch (Exception e)
				{
					Console.WriteLine ($"[DMAKE ERROR] Exception in compile thread:\n\t{e.Message}");
				}
				finally
				{
					// Release the thread from the thread pool
					threadPool.Remove (Thread.CurrentThread);
				}
			});

			// Add the thread to the thread pool
			threadPool.Add (thread);

			// Start the thread
			thread.Start ();
		}

		public static async Task CompileProject ( Project project )
		{
			foreach (SourceFile source in project.files)
			{
				if (source.type == SourceType.C)
				{
					ExecuteThreaded (() => C.CompileSourceFile (source, project.outputDirectory));
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

			Console.WriteLine ($"All worker threads completed.");
		}
	}
}