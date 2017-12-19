using dmake.Stages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dmake
{
	internal static class DMake
	{
		public static Stage currentStage;

		public static async Task Start ( List<Stage> stages )
		{
			Project project = new Project (Util.CurrentPath);

			bool analizeSucceded = await project.Analize ();

			if (!analizeSucceded)
			{
				return;
			}

			Logger.ioSchedulerRunning = true;

			Task.Run (Logger.IOScheduler);

			foreach (Stage stage in stages)
			{
				currentStage = stage;
				if (stage == Stage.COMPILE)
				{
					Console.WriteLine ("Compiling project...");
					await Compiler.CompileProject (project);

					Console.WriteLine ("Linking project...");
					await Compiler.LinkProject (project);
				}
			}

			Logger.ioSchedulerRunning = false;
			Thread.Sleep (1);
		}
	}
}