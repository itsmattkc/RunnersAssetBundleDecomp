using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static class CriHcaDecoder
{
	public static CriPcmData Decode(byte[] data)
	{
		GCHandle gCHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
		IntPtr intPtr = gCHandle.AddrOfPinnedObject();
		CriPcmData criPcmData = new CriPcmData();
		if (!CriHcaDecoder.criAtomDecHca_GetInfo(intPtr, data.Length, out criPcmData.samplingRate, out criPcmData.numChannels, out criPcmData.numSamples, out criPcmData.loopStart, out criPcmData.loopLength))
		{
			gCHandle.Free();
			return null;
		}
		criPcmData.data = new float[criPcmData.numSamples * criPcmData.numChannels];
		GCHandle gCHandle2 = GCHandle.Alloc(criPcmData.data, GCHandleType.Pinned);
		IntPtr out_buf = gCHandle2.AddrOfPinnedObject();
		CriHcaDecoder.criAtomDecHca_DecodeFloatInterleaved(intPtr, data.Length, out_buf, criPcmData.data.Length * 4);
		gCHandle2.Free();
		gCHandle.Free();
		return criPcmData;
	}

	public static CriPcmData Decode(string path)
	{
		byte[] data = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, path));
		return CriHcaDecoder.Decode(data);
	}

	public static AudioClip CreateAudioClip(string name, string path)
	{
		CriPcmData criPcmData = CriHcaDecoder.Decode(path);
		if (criPcmData == null)
		{
			return null;
		}
		AudioClip audioClip = AudioClip.Create(name, criPcmData.numSamples, criPcmData.numChannels, criPcmData.samplingRate, false, false);
		audioClip.SetData(criPcmData.data, 0);
		return audioClip;
	}

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomDecHca_GetInfo(IntPtr data, int nbyte, out int sampling_rate, out int num_channels, out int num_samples, out int loop_start, out int loop_length);

	[DllImport("cri_ware_unity")]
	private static extern int criAtomDecHca_DecodeShortInterleaved(IntPtr in_data, int inbyte, IntPtr out_buf, int out_nbyte);

	[DllImport("cri_ware_unity")]
	private static extern int criAtomDecHca_DecodeFloatInterleaved(IntPtr in_data, int in_nbyte, IntPtr out_buf, int out_nbyte);
}
