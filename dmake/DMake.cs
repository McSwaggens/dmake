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
			Project project = new Project (Util.CurrentPath.TrimEnd ('/').TrimEnd ('\\'));

			bool analizeSucceded = await project.Analize ();

			if (!analizeSucceded)
			{
				return;
			}

			project.script.CallFunction ("Load");

			Logger.ioSchedulerRunning = true;

			Task.Run (Logger.IOScheduler);

			foreach (Stage stage in stages)
			{
				currentStage = stage;
				if (stage == Stage.COMPILE)
				{
					Logger.Normal ("Compiling project...");
					await Compiler.CompileProject (project);

					Logger.Normal ("Linking project...");
					await Compiler.LinkProject (project);
				}
				else if (stage == Stage.INSTALL)
				{
					Logger.Normal ("Installing project into system binary directory...");
					await Installer.Install (project);
				}
				else if (stage == Stage.RUN)
				{
					await Runner.Run (project);
				}
			}

			Logger.ioSchedulerRunning = false;
			Thread.Sleep (1);
		}
	}
}