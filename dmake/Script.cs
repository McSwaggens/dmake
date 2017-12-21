using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LUA = MoonSharp.Interpreter;

namespace dmake
{
	internal class Script
	{
		/// <summary>
		/// Contains the script instance
		/// <code>null</code> if no script was found.
		/// </summary>
		public static Script script = null;

		/// <summary>
		/// Whether or not the script was found.
		/// </summary>
		public static bool hasScript = script != null;

		public string file;
		public LUA.Script luaScript;

		public Script ( string file, Project project )
		{
			this.file = file;

			luaScript = new LUA.Script ();

			// Types
			LUA.UserData.RegisterType (typeof (Project));
			LUA.UserData.RegisterType (typeof (SourceFile));
			LUA.UserData.RegisterType (typeof (List<string>));

			// Variables
			luaScript.Globals["project"] = project;

			luaScript.Globals["Windows"] = Platform.isWin32;
			luaScript.Globals["Unix"] = Platform.isUnix;
			luaScript.Globals["OSX"] = Platform.isOSX;
			luaScript.Globals["Linux"] = Platform.isLinux;

			// Functions
			luaScript.Globals["Print"] = (Action<object>)( Logger.Normal );
			luaScript.Globals["Warning"] = (Action<object>)( Logger.Warning );
			luaScript.Globals["AddLib"] = (Action<string>)( ( slib ) =>
			{
				project.libraries.Add (slib);
			} );

			luaScript.Globals["AddFlag"] = (Action<string>)( ( sopt ) =>
			{
				project.cxxFlags += $"{sopt} ";
			} );

			luaScript.Globals["AddLibIW"] = (Action<string>)( ( slib ) =>
			{
				if (Platform.isWin32)
				{
					project.libraries.Add (slib);
				}
			} );

			luaScript.Globals["AddLibMP"] = (Action<string>)( ( slib ) =>
			{
				slib = Platform.isWin32 ? $"{slib}32" : slib;
				project.libraries.Add (slib);
			} );

			luaScript.DoFile (file);

			script = this;
		}

		public void CallFunction ( string func )
		{
			if (luaScript.Globals[func] != null)
			{
				luaScript.Call (luaScript.Globals[func]);
			}
		}
	}
}