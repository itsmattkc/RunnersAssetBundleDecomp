using System;

namespace GooglePlayGames.OurUtils
{
	public class Logger
	{
		private static string LOG_PREF = "[Play Games Plugin DLL] ";

		private static bool debugLogEnabled;

		public static bool DebugLogEnabled
		{
			get
			{
				return Logger.debugLogEnabled;
			}
			set
			{
				Logger.debugLogEnabled = value;
			}
		}

		public static void d(string msg)
		{
			if (Logger.debugLogEnabled)
			{
				UnityEngine.Debug.Log(Logger.LOG_PREF + msg);
			}
		}

		public static void w(string msg)
		{
			UnityEngine.Debug.LogWarning("!!! " + Logger.LOG_PREF + " WARNING: " + msg);
		}

		public static void e(string msg)
		{
			UnityEngine.Debug.LogWarning("*** " + Logger.LOG_PREF + " ERROR: " + msg);
		}

		public static string describe(byte[] b)
		{
			return (b != null) ? ("byte[" + b.Length + "]") : "(null)";
		}
	}
}
