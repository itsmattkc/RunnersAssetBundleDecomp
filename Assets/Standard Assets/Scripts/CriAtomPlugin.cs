using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class CriAtomPlugin
{
	public const string criAtomUnityEditorVersion = "Ver.0.21.03";

	private static int initializationCount;

	public static bool isInitialized
	{
		get
		{
			return CriAtomPlugin.initializationCount > 0;
		}
	}

	public static void Log(string log)
	{
	}

	public static void SetConfigParameters(int max_virtual_voices, int num_standard_memory_voices, int num_standard_streaming_voices, int num_hca_mx_memory_voices, int num_hca_mx_streaming_voices, int output_sampling_rate, bool uses_in_game_preview, float server_frequency)
	{
		CriAtomPlugin.criAtomUnity_SetConfigParameters(max_virtual_voices, num_standard_memory_voices, num_standard_streaming_voices, num_hca_mx_memory_voices, num_hca_mx_streaming_voices, output_sampling_rate, uses_in_game_preview, server_frequency);
	}

	public static void SetConfigAdditionalParameters_IOS(uint buffering_time_ios, bool override_ipod_music_ios)
	{
		CriAtomPlugin.criAtomUnity_SetConfigAdditionalParameters_IOS(buffering_time_ios, override_ipod_music_ios);
	}

	public static void SetConfigAdditionalParameters_ANDROID(int num_low_delay_memory_voices, int num_low_delay_streaming_voices, int sound_buffering_time, int sound_start_buffering_time)
	{
		CriAtomPlugin.criAtomUnity_SetConfigAdditionalParameters_ANDROID(num_low_delay_memory_voices, num_low_delay_streaming_voices, sound_buffering_time, sound_start_buffering_time);
	}

	public static void SetConfigAdditionalParameters_VITA(int num_atrac9_memory_voices, int num_atrac9_streaming_voices, int num_mana_decoders)
	{
	}

	public static void SetConfigAdditionalParameters_PS4(int num_atrac9_memory_voices, int num_atrac9_streaming_voices)
	{
	}

	public static void SetMaxSamplingRateForStandardVoicePool(int sampling_rate_for_memory, int sampling_rate_for_streaming)
	{
		CriAtomPlugin.criAtomUnity_SetMaxSamplingRateForStandardVoicePool(sampling_rate_for_memory, sampling_rate_for_streaming);
	}

	public static int GetRequiredMaxVirtualVoices(CriAtomConfig atomConfig)
	{
		int num = 0;
		num += atomConfig.standardVoicePoolConfig.memoryVoices;
		num += atomConfig.standardVoicePoolConfig.streamingVoices;
		num += atomConfig.hcaMxVoicePoolConfig.memoryVoices;
		num += atomConfig.hcaMxVoicePoolConfig.streamingVoices;
		num += atomConfig.androidLowLatencyStandardVoicePoolConfig.memoryVoices;
		return num + atomConfig.androidLowLatencyStandardVoicePoolConfig.streamingVoices;
	}

	public static void InitializeLibrary()
	{
		CriAtomPlugin.initializationCount++;
		if (CriAtomPlugin.initializationCount != 1)
		{
			return;
		}
		if (!CriWareInitializer.IsInitialized())
		{
			UnityEngine.Debug.Log("[CRIWARE] CriWareInitializer is not working. Initializes Atom by default parameters.");
		}
		CriFsPlugin.InitializeLibrary();
		CriAtomPlugin.criAtomUnity_Initialize();
		CriAtomServer.CreateInstance();
	}

	public static void FinalizeLibrary()
	{
		CriAtomPlugin.initializationCount--;
		if (CriAtomPlugin.initializationCount < 0)
		{
			UnityEngine.Debug.LogError("[CRIWARE] ERROR: Atom library is already finalized.");
			return;
		}
		if (CriAtomPlugin.initializationCount != 0)
		{
			return;
		}
		CriAtomServer.DestroyInstance();
		CriAtomPlugin.criAtomUnity_Finalize();
		CriFsPlugin.FinalizeLibrary();
	}

	public static void Pause(bool pause)
	{
		if (CriAtomPlugin.isInitialized)
		{
			CriAtomPlugin.criAtomUnity_Pause(pause);
		}
	}

	public static CriWare.CpuUsage GetCpuUsage()
	{
		CriWare.CpuUsage result = default(CriWare.CpuUsage);
		CriAtomPlugin.criAtomUnity_GetCpuUsage(out result.last, out result.average, out result.peak);
		return result;
	}

	[DllImport("cri_ware_unity")]
	private static extern void criAtomUnity_SetConfigParameters(int max_virtual_voices, int num_standard_memory_voices, int num_standard_streaming_voices, int num_hca_mx_memory_voices, int num_hca_mx_streaming_voices, int output_sampling_rate, bool uses_in_game_preview, float server_frequency);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomUnity_SetConfigAdditionalParameters_IOS(uint buffering_time_ios, bool override_ipod_music_ios);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomUnity_SetConfigAdditionalParameters_ANDROID(int num_low_delay_memory_voices, int num_low_delay_streaming_voices, int sound_buffering_time, int sound_start_buffering_time);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomUnity_Initialize();

	[DllImport("cri_ware_unity")]
	private static extern void criAtomUnity_Finalize();

	[DllImport("cri_ware_unity")]
	private static extern void criAtomUnity_Pause(bool pause);

	[DllImport("cri_ware_unity")]
	public static extern void criAtomUnity_GetCpuUsage(out float last, out float average, out float peak);

	[DllImport("cri_ware_unity")]
	public static extern uint criAtomUnity_GetAllocatedHeapSize();

	[DllImport("cri_ware_unity")]
	public static extern void criAtomUnity_ControlDataCompatibility(int code);

	[DllImport("cri_ware_unity")]
	public static extern void criAtomUnitySeqencer_SetEventCallback(IntPtr cbfunc, string gameobj_name, string func_name, string separator_string);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomUnity_SetMaxSamplingRateForStandardVoicePool(int sampling_rate_for_memory, int sampling_rate_for_streaming);
}
