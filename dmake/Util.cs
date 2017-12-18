using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dmake
{
	internal static class Util
	{
		public static string CurrentPath => Directory.GetCurrentDirectory ();
		public static string MinGWPath => "C:\\MinGW";
	}
}