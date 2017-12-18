using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmake
{
	internal static class Analizer
	{
		public static Project GetProject ( string dir )
		{
			Project project = new Project (dir);

			return project;
		}
	}
}