using System;

public class SocialDefine
{
	public enum ScoreType
	{
		TYPE_NONE = -1,
		TYPE_GAMEDATA,
		TYPE_NUM
	}

	public static readonly string[] ScoreQueryName = new string[]
	{
		"/testrunners:store"
	};

	public static readonly string[] ScoreJsonKeyName = new string[]
	{
		"gamedata"
	};
}
