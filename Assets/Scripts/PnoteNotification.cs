using App.Utility;
using SaveData;
using System;

public class PnoteNotification
{
	public enum LaunchOption
	{
		None,
		SendEnergy,
		NoLogin
	}

	private static readonly string[] LaunchString = new string[]
	{
		"None",
		"SendEnergy",
		"nologin"
	};

	public static void RequestRegister()
	{
		if (SystemSaveManager.Instance != null)
		{
			string gameID = SystemSaveManager.GetGameID();
			if (!string.IsNullOrEmpty(gameID) && !gameID.Equals("0"))
			{
				Binding.Instance.RegistPnote(gameID);
			}
		}
	}

	public static void RequestUnregister()
	{
		Binding.Instance.UnregistPnote();
	}

	public static void SendMessage(string message, string reciever, PnoteNotification.LaunchOption option)
	{
		if (SystemSaveManager.Instance != null)
		{
			string gameID = SystemSaveManager.GetGameID();
			if (!string.IsNullOrEmpty(gameID) && !gameID.Equals("0"))
			{
				string launchOption = PnoteNotification.LaunchString[(int)option];
				Binding.Instance.SendMessagePnote(message, gameID, reciever, launchOption);
			}
		}
	}

	public static bool CheckEnableGetNoLoginIncentive()
	{
		string text = Binding.Instance.GetPnoteLaunchString();
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		text = text.ToLower();
		return text.Contains(PnoteNotification.LaunchString[2]);
	}

	public static void RegistTagsPnote(Bitset32 tag_bit)
	{
		string gameID = SystemSaveManager.GetGameID();
		string text = string.Empty;
		if (tag_bit.Test(0))
		{
			text = "1";
		}
		else
		{
			text = "0";
		}
		for (int i = 1; i < 5; i++)
		{
			if (tag_bit.Test(i))
			{
				text += ",1";
			}
			else
			{
				text += ",0";
			}
		}
		Binding.Instance.RegistTagsPnote(text, gameID);
	}
}
