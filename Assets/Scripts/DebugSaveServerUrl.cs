using System;

public class DebugSaveServerUrl
{
	private const string SaveURLName = "DebugServerURL.txt";

	private static string s_Url;

	public static string Url
	{
		get
		{
			return DebugSaveServerUrl.s_Url;
		}
	}

	public static void LoadURL()
	{
	}

	public static void SaveURL(string url)
	{
	}
}
