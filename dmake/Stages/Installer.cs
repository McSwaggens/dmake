using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmake.Stages
{
	internal static class Installer
	{
		public static async Task Install ( Project project )
		{
			if (File.Exists (project.outputFile))
			{
				string destinationFilePath = Platform.outputBinDir + Path.GetFileName (project.outputFile);
				if (File.Exists (destinationFilePath))
				{
					Logger.Verbose ("Binary file already found in system bin directory, deleting it...");

					try
					{
						File.Delete (destinationFilePath);
					}
					catch (UnauthorizedAccessException e)
					{
						Logger.Warning ("Unable to copy file to system binary directory, does this process have the correct permissions to install this application?");
						return;
					}
				}

				try
				{
					File.Copy (project.outputFile, destinationFilePath, true);
				}
				catch (UnauthorizedAccessException e)
				{
					Logger.Warning ("Unable to copy file to system binary directory, does this process have the correct permissions to install this application?");
					return;
				}

				if (!File.Exists (destinationFilePath))
				{
					Logger.Warning ("Unable to verify existance of installed binary file, make sure dmake has the correct permissions to read the system binary directory.");
					return;
				}
			}
			else
			{
				Logger.Warning ("Nothing binary found to install...");
				return;
			}
		}
	}
}