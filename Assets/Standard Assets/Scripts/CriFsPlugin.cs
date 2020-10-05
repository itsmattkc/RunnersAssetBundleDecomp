using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class CriFsPlugin
{
	private static int initializationCount = 0;

	public static int defaultInstallBufferSize = 4194304;

	public static int installBufferSize = CriFsPlugin.defaultInstallBufferSize;

	public static bool isInitialized
	{
		get
		{
			return CriFsPlugin.initializationCount > 0;
		}
	}

	public static void SetConfigParameters(int num_loaders, int num_binders, int num_installers, int argInstallBufferSize, int max_path, bool minimize_file_descriptor_usage)
	{
		CriFsPlugin.criFsUnity_SetConfigParameters(num_loaders, num_binders, num_installers, max_path, minimize_file_descriptor_usage);
		CriFsPlugin.installBufferSize = argInstallBufferSize;
	}

	public static void SetConfigAdditionalParameters_ANDROID(int device_read_bps)
	{
		CriFsPlugin.criFsUnity_SetConfigAdditionalParameters_ANDROID(device_read_bps);
	}

	public static void InitializeLibrary()
	{
		CriFsPlugin.initializationCount++;
		if (CriFsPlugin.initializationCount != 1)
		{
			return;
		}
		if (!CriWareInitializer.IsInitialized())
		{
			UnityEngine.Debug.Log("[CRIWARE] CriWareInitializer is not working. Initializes FileSystem by default parameters.");
		}
		CriFsPlugin.criFsUnity_Initialize();
	}

	public static void FinalizeLibrary()
	{
		CriFsPlugin.initializationCount--;
		if (CriFsPlugin.initializationCount < 0)
		{
			UnityEngine.Debug.LogError("[CRIWARE] ERROR: FileSystem library is already finalized.");
			return;
		}
		if (CriFsPlugin.initializationCount != 0)
		{
			return;
		}
		CriFsServer.DestroyInstance();
		CriFsPlugin.installBufferSize = CriFsPlugin.defaultInstallBufferSize;
		CriFsPlugin.criFsUnity_Finalize();
	}

	[DllImport("cri_ware_unity")]
	private static extern void criFsUnity_SetConfigParameters(int num_loaders, int num_binders, int num_installers, int max_path, bool minimize_file_descriptor_usage);

	[DllImport("cri_ware_unity")]
	private static extern void criFsUnity_SetConfigAdditionalParameters_ANDROID(int device_read_bps);

	[DllImport("cri_ware_unity")]
	private static extern void criFsUnity_Initialize();

	[DllImport("cri_ware_unity")]
	private static extern void criFsUnity_Finalize();

	[DllImport("cri_ware_unity")]
	public static extern uint criFsUnity_GetAllocatedHeapSize();

	[DllImport("cri_ware_unity")]
	public static extern uint criFsLoader_GetRetryCount();
}
