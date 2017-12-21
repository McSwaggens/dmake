using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmake
{
	internal class Project
	{
		public static List<string> sourceDirectoryNames = new List<string> ()
		{
			"src",
			"source",
			"sources",
			"code",
			"scripts"
		};

		public static List<string> outputDirectoryNames = new List<string> ()
		{
			"bin",
			"output",
			"out"
		};

		public string name;
		public string path;
		public string sourceDirectory;
		public string outputDirectory;
		public string outputFile;
		public List<SourceFile> files = new List<SourceFile> ();
		public Script script = null;
		public string cxxFlags = "";
		public string linkerFlags = "";
		public List<string> libraries = new List<string> ();

		public string CombineLibraries ()
		{
			string libs = "";
			libraries.ForEach (s => libs += $"-l{s} ");
			libs = libs.TrimEnd (' ');
			return libs;
		}

		public Project ( string path )
		{
			this.path = path;
			name = Path.GetFileName (path);
		}

		private bool FindLoader ()
		{
			string filePath = path + "/dmake.lua";
			if (File.Exists (filePath))
			{
				script = new Script (filePath, this);
				return true;
			}

			return false;
		}

		private bool FindSourceDirectory ()
		{
			foreach (string dir in Directory.GetDirectories (path))
			{
				if (sourceDirectoryNames.Contains (Path.GetFileName (dir)))
				{
					sourceDirectory = dir;
					return true;
				}
			}

			return false;
		}

		private bool FindOutputDirectory ()
		{
			foreach (string dir in Directory.GetDirectories (path))
			{
				if (outputDirectoryNames.Contains (Path.GetFileName (dir)))
				{
					outputDirectory = dir;
					return true;
				}
			}

			return false;
		}

		private List<SourceFile> RecursiveAnalizeDirectory ( string inputDir )
		{
			Logger.Verbose ($"Searching: '{inputDir}'");
			List<SourceFile> files = new List<SourceFile> ();

			foreach (string file in Directory.GetFiles (inputDir))
			{
				string name = Path.GetFileNameWithoutExtension (file);
				string extension = Path.GetExtension (file);

				SourceType type = SourceFile.GetSourceType (extension);

				SourceFile newSource = new SourceFile (name, file, type);

				files.Add (newSource);
			}

			foreach (string dir in Directory.GetDirectories (inputDir))
			{
				files.AddRange (RecursiveAnalizeDirectory (dir));
			}

			return files;
		}

		/// <summary>
		/// Finds everything useful in the project
		/// </summary>
		/// <returns>Whether or not the directory can properly be used.</returns>
		public async Task<bool> Analize ()
		{
			if (!FindSourceDirectory ())
			{
				Logger.Warning ("Unable to find source directory...");
				return false;
			}

			if (!FindOutputDirectory ())
			{
				Logger.Warning ("Output directory missing... Creating one for you...");

				string newOutPath = path + "/bin";
				Directory.CreateDirectory (newOutPath);
				outputDirectory = newOutPath;
			}

			if (!FindLoader ())
			{
				Logger.Verbose ("'dmake.lua' not found, consider making one if you want to add custom arguments to the compiler or add libraries, etc.");
			}

			outputFile = $"{outputDirectory}/{name}{Platform.executableExtension}";

			files = RecursiveAnalizeDirectory (sourceDirectory);

			if (files.Count == 0)
			{
				Logger.Warning ("No files found to compile.");

				return false;
			}

			return true;
		}
	}
}