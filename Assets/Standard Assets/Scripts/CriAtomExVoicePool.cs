using System;
using System.Runtime.InteropServices;

public class CriAtomExVoicePool
{
	public enum VoicePoolId
	{
		StandardMemory,
		StandardStreaming,
		HcaMxMemory = 4,
		HcaMxStreaming,
		LowLatencyMemory = 2,
		LowLatencyStreaming
	}

	public struct UsedVoicesInfo
	{
		public int numUsedVoices;

		public int numPoolVoices;
	}

	public const int StandardMemoryAsrVoicePoolId = 0;

	public const int StandardStreamingAsrVoicePoolId = 1;

	public const int StandardMemoryNsrVoicePoolId = 2;

	public const int StandardStreamingNsrVoicePoolId = 3;

	public static CriAtomExVoicePool.UsedVoicesInfo GetNumUsedVoices(CriAtomExVoicePool.VoicePoolId voicePoolId)
	{
		CriAtomExVoicePool.UsedVoicesInfo result;
		CriAtomExVoicePool.criAtomUnity_GetNumUsedVoices((int)voicePoolId, out result.numUsedVoices, out result.numPoolVoices);
		return result;
	}

	[DllImport("cri_ware_unity")]
	private static extern void criAtomUnity_GetNumUsedVoices(int voice_pool_id, out int num_used_voices, out int num_pool_voices);
}
