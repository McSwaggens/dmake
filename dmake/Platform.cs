using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmake
{
	public static class Platform
	{
		public static readonly bool isOSX = Environment.OSVersion.Platform == PlatformID.MacOSX;
		public static readonly bool isLinux = Environment.OSVersion.Platform == PlatformID.Unix && !isOSX;
		public static readonly bool isUnix = isOSX || isLinux;
		public static readonly bool isWin32 = !isUnix;

		public static readonly string executableExtension = isWin32 ? ".exe" : "";

		public static readonly string binDir = isUnix ? "/usr/bin/" : "C:\\MinGW\\bin\\";
		public static readonly string cpp = binDir + "g++" + executableExtension;
		public static readonly string c = binDir + "gcc" + executableExtension;

		public static readonly string outputBinDir = isUnix ? "/usr/bin/" : "C:\\Windows\\System32\\";

		public static string FPath ( string path )
		{
			if (isWin32)
			{
				return $"\"{path}\"";
			}

			return path;
		}
	}
}