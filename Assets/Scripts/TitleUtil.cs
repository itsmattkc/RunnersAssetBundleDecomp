using SaveData;
using System;

public class TitleUtil
{
	private static bool s_initUser;

	private static readonly string FirstGameId = "0";

	public static bool initUser
	{
		get
		{
			return !TitleUtil.IsExistSaveDataGameId() || TitleUtil.s_initUser;
		}
	}

	public static string GetSystemSaveDataGameId()
	{
		string gameID = SystemSaveManager.GetGameID();
		if (!string.IsNullOrEmpty(gameID))
		{
			return gameID;
		}
		return TitleUtil.FirstGameId;
	}

	public static bool IsExistSaveDataGameId()
	{
		string systemSaveDataGameId = TitleUtil.GetSystemSaveDataGameId();
		if (systemSaveDataGameId == TitleUtil.FirstGameId)
		{
			TitleUtil.s_initUser = true;
			return false;
		}
		return true;
	}

	public static bool SetSystemSaveDataGameId(string gameId)
	{
		bool result = false;
		string gameID = SystemSaveManager.GetGameID();
		if (string.IsNullOrEmpty(gameID) || gameID == TitleUtil.FirstGameId)
		{
			SystemSaveManager.SetGameID(gameId);
			result = true;
		}
		return result;
	}

	public static string GetSystemSaveDataPassword()
	{
		string text = SystemSaveManager.GetGamePassword();
		if (text == null)
		{
			text = string.Empty;
		}
		return text;
	}

	public static bool SetSystemSaveDataPassword(string password)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(password))
		{
			result = SystemSaveManager.SetGamePassword(password);
		}
		return result;
	}
}
