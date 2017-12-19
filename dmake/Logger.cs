using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmake
{
	internal static class Logger
	{
		public static Dictionary<string, ConsoleColor> outputStack = new Dictionary<string, ConsoleColor> ();
		private static Object ioLock = new Object ();

		public static bool verbose = true;

		public static void Normal ( object str )
		{
			lock (ioLock)
			{
				outputStack.Add ($"{str.ToString ()}", ConsoleColor.Gray);
			}
		}

		public static void Verbose ( object str )
		{
			lock (ioLock)
			{
				if (verbose)
				{
					outputStack.Add ($"[Verbose] {str.ToString ()}", ConsoleColor.Magenta);
				}
			}
		}

		public static void Warning ( object str )
		{
			lock (ioLock)
			{
				outputStack.Add ($"[Warning] {str.ToString ()}", ConsoleColor.Yellow);
			}
		}

		public static void ProgressInfo ( object str )
		{
			lock (ioLock)
			{
				outputStack.Add ($"{str.ToString ()}", ConsoleColor.Green);
			}
		}

		internal static bool ioSchedulerRunning = false;

		internal static async Task IOScheduler ()
		{
			while (ioSchedulerRunning)
			{
				if (outputStack.Count > 0)
				{
					lock (ioLock)
					{
						foreach (var log in outputStack)
						{
							Console.ForegroundColor = log.Value;
							Console.WriteLine (log.Key);
							Console.ResetColor ();
						}

						outputStack.Clear ();
					}
				}
			}
		}
	}
}