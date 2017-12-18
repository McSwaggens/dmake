using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmake
{
	internal enum SourceType
	{
		CPP,
		C,
		HEADER,
		MISC
	}

	internal class SourceFile
	{
		private static Dictionary<string, SourceType> sourceTypeDictionary = new Dictionary<string, SourceType> ()
		{
			{   ".cpp",    SourceType.CPP      },
			{   ".hpp",    SourceType.HEADER   },
			{   ".c",      SourceType.C        },
			{   ".h",      SourceType.HEADER   }
		};

		public static SourceType GetSourceType ( string extension )
		{
			if (sourceTypeDictionary.ContainsKey (extension))
			{
				return sourceTypeDictionary[extension];
			}
			else
			{
				return SourceType.MISC;
			}
		}

		public string name;
		public string path;
		public SourceType type = SourceType.MISC;

		public SourceFile ( string name, string path, SourceType type )
		{
			this.name = name;
			this.path = path;
			this.type = type;
		}
	}
}