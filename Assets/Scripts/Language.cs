using App;
using System;
using UnityEngine;

public class Language
{
	public static string GetLocalLanguage()
	{
		string result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.util.Locale"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getDefault", new object[0]))
			{
				string text = androidJavaObject.Call<string>("getLanguage", new object[0]);
				result = text;
			}
		}
		return result;
	}

	private static string GetLocale()
	{
		string result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.util.Locale"))
		{
			using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getDefault", new object[0]))
			{
				result = androidJavaObject.Call<string>("toString", new object[0]);
			}
		}
		return result;
	}

	private static void SetEditorLanguage()
	{
	}

	private static void SetIPhoneLanguage()
	{
		SystemLanguage systemLanguage = Application.systemLanguage;
		switch (systemLanguage)
		{
		case SystemLanguage.Italian:
			Env.language = Env.Language.ITALIAN;
			return;
		case SystemLanguage.Japanese:
			Env.language = Env.Language.JAPANESE;
			return;
		case SystemLanguage.Korean:
			Env.language = Env.Language.KOREAN;
			return;
		case SystemLanguage.Latvian:
		case SystemLanguage.Lithuanian:
		case SystemLanguage.Norwegian:
		case SystemLanguage.Polish:
		case SystemLanguage.Romanian:
			IL_37:
			switch (systemLanguage)
			{
			case SystemLanguage.English:
				Env.language = Env.Language.ENGLISH;
				return;
			case SystemLanguage.Estonian:
			case SystemLanguage.Faroese:
			case SystemLanguage.Finnish:
				IL_58:
				if (systemLanguage == SystemLanguage.Chinese)
				{
					string localLanguage = Language.GetLocalLanguage();
					global::Debug.Log("Language.InitAppEnvLanguage() GetLocalLanguage = " + localLanguage);
					if (localLanguage.Contains("zh-Hans"))
					{
						Env.language = Env.Language.CHINESE_ZHJ;
					}
					else
					{
						Env.language = Env.Language.CHINESE_ZH;
					}
					return;
				}
				if (systemLanguage != SystemLanguage.Spanish)
				{
					Env.language = Env.Language.ENGLISH;
					return;
				}
				Env.language = Env.Language.SPANISH;
				return;
			case SystemLanguage.French:
				Env.language = Env.Language.FRENCH;
				return;
			case SystemLanguage.German:
				Env.language = Env.Language.GERMAN;
				return;
			}
			goto IL_58;
		case SystemLanguage.Portuguese:
			Env.language = Env.Language.PORTUGUESE;
			return;
		case SystemLanguage.Russian:
			Env.language = Env.Language.RUSSIAN;
			return;
		}
		goto IL_37;
	}

	private static void SetAndroidLanguage()
	{
		string localLanguage = Language.GetLocalLanguage();
		if (localLanguage == "ja")
		{
			Env.language = Env.Language.JAPANESE;
		}
		else if (localLanguage == "en")
		{
			Env.language = Env.Language.ENGLISH;
		}
		else if (localLanguage == "de")
		{
			Env.language = Env.Language.GERMAN;
		}
		else if (localLanguage == "es")
		{
			Env.language = Env.Language.SPANISH;
		}
		else if (localLanguage == "fr")
		{
			Env.language = Env.Language.FRENCH;
		}
		else if (localLanguage == "it")
		{
			Env.language = Env.Language.ITALIAN;
		}
		else if (localLanguage == "ko")
		{
			Env.language = Env.Language.KOREAN;
		}
		else if (localLanguage == "pt")
		{
			Env.language = Env.Language.PORTUGUESE;
		}
		else if (localLanguage == "ru")
		{
			Env.language = Env.Language.RUSSIAN;
		}
		else if (localLanguage == "zh")
		{
			Env.language = Env.Language.CHINESE_ZHJ;
			string locale = Language.GetLocale();
			if (locale != null && locale == "zh_TW")
			{
				Env.language = Env.Language.CHINESE_ZH;
			}
		}
		else
		{
			Env.language = Env.Language.ENGLISH;
		}
	}

	public static void InitAppEnvLanguage()
	{
		Language.SetAndroidLanguage();
	}
}
