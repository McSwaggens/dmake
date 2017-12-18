using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmake
{
	internal class Program
	{
		private static void Main ( string[] args )
		{
			List<Stage> stages = Parser.ParseInput (args);

			// Start DMake async
			// wait until it's finished.
			DMake.Start (stages).GetAwaiter ().GetResult ();

			Console.WriteLine ("DMake finished.");
			Console.ReadLine ();
		}
	}
}