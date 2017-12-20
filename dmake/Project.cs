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

		public Project ( string path )
		{
			this.path = path;
			name = Path.GetFileName(path);
		}

		private bool FindSourceDirectory ( out string directory )
		{
			directory = "";

			foreach (string dir in Directory.GetDirectories (path))
			{
				if (sourceDirectoryNames.Contains (Path.GetFileName (dir)))
				{
					directory = dir;
					return true;
				}
			}

			return false;
		}

		private bool FindOutputDirectory ( out string directory )
		{
			directory = "";

			foreach (string dir in Directory.GetDirectories (path))
			{
				if (outputDirectoryNames.Contains (Path.GetFileName (dir)))
				{
					directory = dir;
					return true;
				}
			}

			return false;
		}

		private List<SourceFile> RecursiveAnalizeDirectory ( string inputDir )
		{
			Logger.Verbose ($"Searching: {inputDir}");
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
			if (!FindSourceDirectory (out sourceDirectory))
			{
				Logger.Warning ("Unable to find source directory...");
				return false;
			}

			if (!FindOutputDirectory (out outputDirectory))
			{
				Logger.Warning ("Output directory missing... Creating one for you...");
				Directory.CreateDirectory (path + "/bin");
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