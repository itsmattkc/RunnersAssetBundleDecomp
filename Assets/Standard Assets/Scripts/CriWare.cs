using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class CriWare
{
	public struct CpuUsage
	{
		public float last;

		public float average;

		public float peak;
	}

	private const string scriptVersionString = "2.10.00";

	private const int scriptVersionNumber = 34603008;

	public const bool supportsCriFsInstaller = true;

	public const string pluginName = "cri_ware_unity";

	private static GameObject _managerObject;

	public static string streamingAssetsPath
	{
		get
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				return string.Empty;
			}
			return Application.streamingAssetsPath;
		}
	}

	public static string installTargetPath
	{
		get
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return Application.temporaryCachePath;
			}
			return Application.persistentDataPath;
		}
	}

	public static GameObject managerObject
	{
		get
		{
			if (CriWare._managerObject == null)
			{
				CriWare._managerObject = GameObject.Find("/CRIWARE");
				if (CriWare._managerObject == null)
				{
					CriWare._managerObject = new GameObject("CRIWARE");
				}
			}
			return CriWare._managerObject;
		}
	}

	public static bool IsStreamingAssetsPath(string path)
	{
		return !Path.IsPathRooted(path) && path.IndexOf(':') < 0;
	}

	[DllImport("cri_ware_unity")]
	public static extern int criWareUnity_SetDecryptionKey(ulong key, string authentication_file, bool enable_atom_decryption, bool enable_mana_decryption);

	[DllImport("cri_ware_unity")]
	public static extern int criWareUnity_GetVersionNumber();

	public static string GetScriptVersionString()
	{
		return "2.10.00";
	}

	public static int GetScriptVersionNumber()
	{
		return 34603008;
	}

	public static int GetBinaryVersionNumber()
	{
		return CriWare.criWareUnity_GetVersionNumber();
	}

	public static int GetRequiredBinaryVersionNumber()
	{
		return 34603008;
	}

	public static bool CheckBinaryVersionCompatibility()
	{
		if (CriWare.GetBinaryVersionNumber() == CriWare.GetRequiredBinaryVersionNumber())
		{
			return true;
		}
		UnityEngine.Debug.LogError("CRI runtime library is not compatible. Confirm the version number.");
		return false;
	}

	public static uint GetFsMemoryUsage()
	{
		return CriFsPlugin.criFsUnity_GetAllocatedHeapSize();
	}

	public static uint GetAtomMemoryUsage()
	{
		return CriAtomPlugin.criAtomUnity_GetAllocatedHeapSize();
	}

	public static uint GetManaMemoryUsage()
	{
		return CriManaPlugin.criManaUnity_GetAllocatedHeapSize();
	}

	public static CriWare.CpuUsage GetAtomCpuUsage()
	{
		return CriAtomPlugin.GetCpuUsage();
	}
}
