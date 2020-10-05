using App;
using SaveData;
using System;

internal class NetBaseUtil
{
	private static string m_debugServerUrl = null;

	private static string m_assetServerVersionUrl = string.Empty;

	private static string[] mActionServerUrlTable = new string[]
	{
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/"
	};

	private static string[] mSecureActionServerUrlTable = new string[]
	{
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/",
		"http://sanic.uk.to:9001/"
	};

	private static string[] mServerTypeStringTable = new string[]
	{
		"_L1",
		"_L2",
		"_L3",
		"_L4",
		"_L5",
		"_D1",
		"_D2",
		"_D3",
		"_S",
		string.Empty,
		"a"
	};

	private static string[] mAssetURLTable = new string[]
	{
		"http://sanic.uk.to:9002/assets/",
		"http://sanic.uk.to:9002/assets/",
		"http://sanic.uk.to:9002/assets/",
		"http://sanic.uk.to:9002/assets/",
		"http://sanic.uk.to:9002/assets/",
		"http://sanic.uk.to:9002/assets/",
		"http://sanic.uk.to:9002/assets/",
		"http://sanic.uk.to:9002/assets/",
		"http://sanic.uk.to:9002/assets/",
		"http://sanic.uk.to:9002/assets/",
		"http://sanic.uk.to:9002/assets/"
	};

	private static string[] mInformationURLTable = new string[]
	{
		"http://sanic.uk.to:9002/information/",
		"http://sanic.uk.to:9002/information/",
		"http://sanic.uk.to:9002/information/",
		"http://sanic.uk.to:9002/information/",
		"http://sanic.uk.to:9002/information/",
		"http://sanic.uk.to:9002/information/",
		"http://sanic.uk.to:9002/information/",
		"http://sanic.uk.to:9002/information/",
		"http://sanic.uk.to:9002/information/",
		"http://sanic.uk.to:9002/information/",
		"http://sanic.uk.to:9002/information/"
	};

	private static string mRedirectInstallPageUrl = "https://play.google.com/store/apps/details?id=com.sega.sonicrunners";

	public static string DebugServerUrl
	{
		get
		{
			return NetBaseUtil.m_debugServerUrl;
		}
		set
		{
			NetBaseUtil.m_debugServerUrl = value;
		}
	}

	public static bool IsDebugServer
	{
		get
		{
			return NetBaseUtil.m_debugServerUrl != null;
		}
		private set
		{
		}
	}

	public static string ActionServerURL
	{
		get
		{
			if (NetBaseUtil.IsDebugServer)
			{
				return NetBaseUtil.DebugServerUrl;
			}
			return NetBaseUtil.mActionServerUrlTable[(int)Env.actionServerType];
		}
	}

	public static string SecureActionServerURL
	{
		get
		{
			if (NetBaseUtil.IsDebugServer)
			{
				return NetBaseUtil.DebugServerUrl;
			}
			return NetBaseUtil.mSecureActionServerUrlTable[(int)Env.actionServerType];
		}
	}

	public static string ServerTypeString
	{
		get
		{
			if (NetBaseUtil.IsDebugServer)
			{
				return "i";
			}
			return NetBaseUtil.mServerTypeStringTable[(int)Env.actionServerType];
		}
	}

	public static string AssetServerURL
	{
		get
		{
			if (NetBaseUtil.m_assetServerVersionUrl != string.Empty)
			{
				return NetBaseUtil.m_assetServerVersionUrl;
			}
			return NetBaseUtil.mAssetURLTable[(int)Env.actionServerType];
		}
	}

	public static string InformationServerURL
	{
		get
		{
			ServerLoginState loginState = ServerInterface.LoginState;
			string text = string.Empty;
			if (loginState != null)
			{
				text = loginState.InfoVersionString;
			}
			string text2 = NetBaseUtil.mInformationURLTable[(int)Env.actionServerType];
			if (text != string.Empty)
			{
				return text2 + text + "/";
			}
			return text2;
		}
	}

	public static string RedirectInstallPageUrl
	{
		get
		{
			return NetBaseUtil.mRedirectInstallPageUrl;
		}
		private set
		{
		}
	}

	public static string RedirectTrmsOfServicePageUrlForTitle
	{
		get
		{
			if (Env.language == Env.Language.JAPANESE)
			{
				return "http://sonicrunners.sega-net.com/jp/rule/";
			}
			return "http://www.sega.com/legal";
		}
	}

	public static string RedirectTrmsOfServicePageUrl
	{
		get
		{
			if (RegionManager.Instance != null && RegionManager.Instance.IsJapan())
			{
				return "http://sonicrunners.sega-net.com/jp/rule/";
			}
			return "http://sonicrunners.sega-net.com/rule.html";
		}
	}

	public static void SetAssetServerURL()
	{
		ServerLoginState loginState = ServerInterface.LoginState;
		string text = string.Empty;
		if (loginState != null)
		{
			text = loginState.AssetsVersionString;
		}
		string text2 = NetBaseUtil.mAssetURLTable[(int)Env.actionServerType];
		if (text != string.Empty)
		{
			NetBaseUtil.m_assetServerVersionUrl = string.Concat(new string[]
			{
				text2,
				CurrentBundleVersion.version,
				"_",
				text,
				"/"
			});
		}
		else
		{
			NetBaseUtil.m_assetServerVersionUrl = text2;
		}
		if (SystemSaveManager.GetSystemSaveData() != null && SystemSaveManager.GetSystemSaveData().highTexture)
		{
			NetBaseUtil.m_assetServerVersionUrl += "tablet/";
		}
	}

	public static int GetVersionValue(string versionString, int scaleOffset)
	{
		string[] array = versionString.Split(new char[]
		{
			'.'
		});
		int num = array.Length;
		int[] array2 = new int[num];
		bool flag = true;
		for (int i = 0; i < num; i++)
		{
			if (!int.TryParse(array[i], out array2[i]))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			int num2 = 0;
			int num3 = 1;
			for (int j = num - 1; j >= 0; j--)
			{
				num2 += array2[j] * num3;
				num3 *= scaleOffset;
			}
			return num2;
		}
		return -1;
	}

	public static int GetVersionValue(string versionString)
	{
		return NetBaseUtil.GetVersionValue(versionString, 10);
	}
}
