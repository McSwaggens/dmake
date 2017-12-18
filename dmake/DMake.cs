using dmake.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmake
{
	internal static class DMake
	{
		public static Stage currentStage;

		public static async Task Start ( List<Stage> stages )
		{
			Project project = Analizer.GetProject (Util.CurrentPath);

			bool analizeSucceded = await project.Analize ();

			foreach (Stage stage in stages)
			{
				if (stage == Stage.COMPILE)
				{
					Console.WriteLine ("Compiling project...");
					await Compiler.CompileProject (project);

					Console.WriteLine ("Linking project...");
					await Compiler.LinkProject (project);
				}
			}
		}
	}
}