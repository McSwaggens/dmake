using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmake
{
	internal static class Parser
	{
		private static Dictionary<List<string>, Stage> stagePairs = new Dictionary<List<string>, Stage> ()
		{
			{
				new List<string>() { "compile", "c" },
				Stage.COMPILE
			},
			{
				new List<string>() { "install", "i" },
				Stage.INSTALL
			},
			{
				new List<string>() { "run", "r" },
				Stage.RUN
			}
		};

		private static Stage? GetStage ( string str )
		{
			foreach (var pair in stagePairs)
			{
				if (pair.Key.Contains (str))
				{
					return pair.Value;
				}
			}

			return null;
		}

		public static List<Stage> ParseInput ( string[] args )
		{
			if (args.Length == 0)
			{
				return new List<Stage> () { Stage.COMPILE };
			}

			List<Stage> stages = new List<Stage> ();

			foreach (string str in args)
			{
				Stage? stage = GetStage (str);

				if (stage != null)
				{
					stages.Add ((Stage)stage);
				}
				else
				{
					Console.WriteLine ($"[DMAKE WARNING] Unknown stage called \"{str}\"");
				}
			}

			return stages;
		}
	}
}