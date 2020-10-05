using System;
using System.Runtime.InteropServices;

public class CriAtomExAsr
{
	private struct BusAnalyzerConfig
	{
		public int interval;

		public int peakHoldTime;
	}

	public struct BusAnalyzerInfo
	{
		public int numChannels;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public float[] rmsLevels;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public float[] peakLevels;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public float[] peakHoldLevels;

		public BusAnalyzerInfo(byte[] data)
		{
			if (data != null)
			{
				this.numChannels = BitConverter.ToInt32(data, 0);
				this.rmsLevels = new float[8];
				for (int i = 0; i < 8; i++)
				{
					this.rmsLevels[i] = BitConverter.ToSingle(data, 4 + i * 4);
				}
				this.peakLevels = new float[8];
				for (int j = 0; j < 8; j++)
				{
					this.peakLevels[j] = BitConverter.ToSingle(data, 36 + j * 4);
				}
				this.peakHoldLevels = new float[8];
				for (int k = 0; k < 8; k++)
				{
					this.peakHoldLevels[k] = BitConverter.ToSingle(data, 68 + k * 4);
				}
			}
			else
			{
				this.numChannels = 0;
				this.rmsLevels = new float[8];
				this.peakLevels = new float[8];
				this.peakHoldLevels = new float[8];
			}
		}
	}

	public static void AttachBusAnalyzer(int interval, int peakHoldTime)
	{
		CriAtomExAsr.BusAnalyzerConfig busAnalyzerConfig;
		busAnalyzerConfig.interval = 50;
		busAnalyzerConfig.peakHoldTime = 1000;
		for (int i = 0; i < 8; i++)
		{
			CriAtomExAsr.criAtomExAsr_AttachBusAnalyzer(i, ref busAnalyzerConfig);
		}
	}

	public static void DetachBusAnalyzer()
	{
		for (int i = 0; i < 8; i++)
		{
			CriAtomExAsr.criAtomExAsr_DetachBusAnalyzer(i);
		}
	}

	public static void GetBusAnalyzerInfo(int bus, out CriAtomExAsr.BusAnalyzerInfo info)
	{
		using (CriStructMemory<CriAtomExAsr.BusAnalyzerInfo> criStructMemory = new CriStructMemory<CriAtomExAsr.BusAnalyzerInfo>())
		{
			CriAtomExAsr.criAtomExAsr_GetBusAnalyzerInfo(bus, criStructMemory.ptr);
			info = new CriAtomExAsr.BusAnalyzerInfo(criStructMemory.bytes);
		}
	}

	public static void SetBusVolume(int bus, float volume)
	{
		CriAtomExAsr.criAtomExAsr_SetBusVolume(bus, volume);
	}

	public static void SetBusSendLevel(int bus, int sendTo, float level)
	{
		CriAtomExAsr.criAtomExAsr_SetBusSendLevel(bus, sendTo, level);
	}

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExAsr_AttachBusAnalyzer(int bus_no, ref CriAtomExAsr.BusAnalyzerConfig config);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExAsr_DetachBusAnalyzer(int bus_no);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExAsr_GetBusAnalyzerInfo(int bus_no, IntPtr info);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExAsr_SetBusVolume(int bus_no, float volume);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExAsr_SetBusSendLevel(int bus_no, int sendto_no, float level);
}
