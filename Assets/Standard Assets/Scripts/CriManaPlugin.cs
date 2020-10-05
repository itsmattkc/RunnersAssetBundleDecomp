using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class CriManaPlugin
{
	private enum GraphicsApi
	{
		Unknown,
		OpenGLES_2_0,
		OpenGLES_3_0,
		Metal
	}

	private static int initializationCount;

	public static bool isInitialized
	{
		get
		{
			return CriManaPlugin.initializationCount > 0;
		}
	}

	public static void SetConfigParameters(int num_decoders, int max_num_of_entries, bool enable_cue_point)
	{
		CriManaPlugin.GraphicsApi graphics_api = CriManaPlugin.GraphicsApi.Unknown;
		CriManaPlugin.criManaUnity_SetConfigParameters((int)graphics_api, num_decoders, max_num_of_entries, enable_cue_point);
	}

	public static void InitializeLibrary()
	{
		CriManaPlugin.initializationCount++;
		if (CriManaPlugin.initializationCount != 1)
		{
			return;
		}
		if (!CriWareInitializer.IsInitialized())
		{
			UnityEngine.Debug.Log("[CRIWARE] CriWareInitializer is not working. Initializes Mana by default parameters.");
		}
		CriFsPlugin.InitializeLibrary();
		CriAtomPlugin.InitializeLibrary();
		CriManaPlugin.criManaUnity_Initialize();
	}

	public static void FinalizeLibrary()
	{
		CriManaPlugin.initializationCount--;
		if (CriManaPlugin.initializationCount < 0)
		{
			UnityEngine.Debug.LogError("[CRIWARE] ERROR: Mana library is already finalized.");
			return;
		}
		if (CriManaPlugin.initializationCount != 0)
		{
			return;
		}
		CriManaPlugin.criManaUnity_Finalize();
		CriAtomPlugin.FinalizeLibrary();
		CriFsPlugin.FinalizeLibrary();
	}

	public static void SetCuePointFormatDelimiter(string delimiter)
	{
		CriManaPlugin.criManaUnity_SetCuePointFormatDelimiter(delimiter);
	}

	[DllImport("cri_ware_unity")]
	private static extern void criManaUnity_SetConfigParameters(int graphics_api, int num_decoders, int num_of_max_entries, bool enable_cue_point);

	[DllImport("cri_ware_unity")]
	private static extern void criManaUnity_Initialize();

	[DllImport("cri_ware_unity")]
	private static extern void criManaUnity_Finalize();

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnity_ExecuteMain();

	[DllImport("cri_ware_unity")]
	public static extern uint criManaUnity_GetAllocatedHeapSize();

	[DllImport("cri_ware_unity")]
	private static extern void criManaUnity_SetCuePointFormatDelimiter(string delimite_string);

	[DllImport("cri_ware_unity")]
	public static extern int criManaUnityPlayer_Create();

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_Destroy(int player_id);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_SetFile(int player_id, IntPtr binder, string path);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_SetContentId(int player_id, IntPtr binder, int content_id);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_SetFileRange(int player_id, string path, ulong offset, long range);

	[DllImport("cri_ware_unity")]
	public static extern bool criManaUnityPlayer_EntryFile(int player_id, IntPtr binder, string path, bool repeat);

	[DllImport("cri_ware_unity")]
	public static extern bool criManaUnityPlayer_EntryContentId(int player_id, IntPtr binder, int content_id, bool repeat);

	[DllImport("cri_ware_unity")]
	public static extern bool criManaUnityPlayer_EntryFileRange(int player_id, string path, ulong offset, long range, bool repeat);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_ClearEntry(int player_id);

	[DllImport("cri_ware_unity")]
	public static extern int criManaUnityPlayer_GetNumberOfEntry(int player_id);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_SetCuePointCallback(int player_id, IntPtr cbfunc, string gameobj_name, string func_name);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_GetMovieInfo(int player_id, out CriManaPlayer.MovieInfo movie_info);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_Update(int player_id);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_Prepare(int player_id);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_Start(int player_id);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_Stop(int player_id);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_SetSeekPosition(int player_id, int seek_frame_no);

	[DllImport("cri_ware_unity")]
	public static extern bool criManaUnityPlayer_UpdateTexture(int player_id, IntPtr texbuf, out CriManaPlayer.FrameInfo frame_info);

	[DllImport("cri_ware_unity")]
	public static extern bool criManaUnityPlayer_UpdateTextureYuvByID(int player_id, uint texid_y, uint texid_u, uint texid_v, out CriManaPlayer.FrameInfo frame_info);

	[DllImport("cri_ware_unity")]
	public static extern bool criManaUnityPlayer_UpdateTextureYuvaByID(int player_id, uint texid_y, uint texid_u, uint texid_v, uint texid_a, out CriManaPlayer.FrameInfo frame_info);

	[DllImport("cri_ware_unity")]
	public static extern bool criManaUnityPlayer_UpdateTextureYuvByPtr(int player_id, IntPtr texid_y, IntPtr texid_u, IntPtr texid_v, out CriManaPlayer.FrameInfo frame_info);

	[DllImport("cri_ware_unity")]
	public static extern bool criManaUnityPlayer_UpdateTextureYuvaByPtr(int player_id, IntPtr texid_y, IntPtr texid_u, IntPtr texid_v, IntPtr texid_a, out CriManaPlayer.FrameInfo frame_info);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_Pause(int player_id, int sw);

	[DllImport("cri_ware_unity")]
	public static extern bool criManaUnityPlayer_IsPaused(int player_id);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_Loop(int player_id, int sw);

	[DllImport("cri_ware_unity")]
	public static extern long criManaUnityPlayer_GetTime(int player_id);

	[DllImport("cri_ware_unity")]
	public static extern int criManaUnityPlayer_GetStatus(int player_id);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_SetAudioTrack(int player_id, int track);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_SetVolume(int player_id, float vol);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_SetSubAudioTrack(int player_id, int track);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_SetSubAudioVolume(int player_id, float vol);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_SetSpeed(int player_id, float speed);

	[DllImport("cri_ware_unity")]
	public static extern void criManaUnityPlayer_SetDeviceSendLevel(int player_id, int device_id, float level);

	[DllImport("cri_ware_unity")]
	public static extern bool criManaUnityPlayer_HasAlpha(int player_id);
}
