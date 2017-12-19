using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmake.Stages
{
	internal static class Runner
	{
		public static async Task Run ( Project project )
		{
			if (File.Exists (project.outputFile))
			{
				Logger.Normal ($"Running program '{Path.GetFileName (project.outputFile)}'");

				var procInfo = new ProcessStartInfo
				{
					FileName = $"\"{project.outputFile}\"",
					Arguments = $"",
					UseShellExecute = false,
					RedirectStandardOutput = false,
				};

				Logger.Verbose ($"{procInfo.FileName} {procInfo.Arguments}");

				Process process = Process.Start (procInfo);

				process.WaitForExit ();

				Logger.Normal ("Program has finished.");
			}
		}
	}
}