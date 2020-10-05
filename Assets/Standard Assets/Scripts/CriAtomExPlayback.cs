using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public struct CriAtomExPlayback
{
	public enum Status
	{
		Prep = 1,
		Playing,
		Removed
	}

	private uint _id_k__BackingField;

	public uint id
	{
		get;
		private set;
	}

	public CriAtomExPlayback.Status status
	{
		get
		{
			return this.GetStatus();
		}
	}

	public long time
	{
		get
		{
			return this.GetTime();
		}
	}

	public CriAtomExPlayback(uint id)
	{
		this.id = id;
	}

	public void Stop(bool ignoresReleaseTime)
	{
		if (!ignoresReleaseTime)
		{
			CriAtomExPlayback.criAtomExPlayback_Stop(this.id);
		}
		else
		{
			CriAtomExPlayback.criAtomExPlayback_StopWithoutReleaseTime(this.id);
		}
	}

	public void Pause()
	{
		CriAtomExPlayback.criAtomExPlayback_Pause(this.id, true);
	}

	public void Resume(CriAtomEx.ResumeMode mode)
	{
		CriAtomExPlayback.criAtomExPlayback_Resume(this.id, mode);
	}

	public bool IsPaused()
	{
		return CriAtomExPlayback.criAtomExPlayback_IsPaused(this.id);
	}

	public bool GetFormatInfo(out CriAtomEx.FormatInfo info)
	{
		return CriAtomExPlayback.criAtomExPlayback_GetFormatInfo(this.id, out info);
	}

	public CriAtomExPlayback.Status GetStatus()
	{
		return CriAtomExPlayback.criAtomExPlayback_GetStatus(this.id);
	}

	public long GetTime()
	{
		return CriAtomExPlayback.criAtomExPlayback_GetTime(this.id);
	}

	public bool GetNumPlayedSamples(out long numSamples, out int samplingRate)
	{
		return CriAtomExPlayback.criAtomExPlayback_GetNumPlayedSamples(this.id, out numSamples, out samplingRate);
	}

	public int GetCurrentBlockIndex()
	{
		return CriAtomExPlayback.criAtomExPlayback_GetCurrentBlockIndex(this.id);
	}

	public void SetNextBlockIndex(int index)
	{
		CriAtomExPlayback.criAtomExPlayback_SetNextBlockIndex(this.id, index);
	}

	public void Stop()
	{
		CriAtomExPlayback.criAtomExPlayback_Stop(this.id);
	}

	public void StopWithoutReleaseTime()
	{
		CriAtomExPlayback.criAtomExPlayback_StopWithoutReleaseTime(this.id);
	}

	public void Pause(bool sw)
	{
		CriAtomExPlayback.criAtomExPlayback_Pause(this.id, sw);
	}

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayback_Stop(uint id);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayback_StopWithoutReleaseTime(uint id);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayback_Pause(uint id, bool sw);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayback_Resume(uint id, CriAtomEx.ResumeMode mode);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExPlayback_IsPaused(uint id);

	[DllImport("cri_ware_unity")]
	private static extern CriAtomExPlayback.Status criAtomExPlayback_GetStatus(uint id);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExPlayback_GetFormatInfo(uint id, out CriAtomEx.FormatInfo info);

	[DllImport("cri_ware_unity")]
	private static extern long criAtomExPlayback_GetTime(uint id);

	[DllImport("cri_ware_unity")]
	private static extern bool criAtomExPlayback_GetNumPlayedSamples(uint id, out long num_samples, out int sampling_rate);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayback_SetNextBlockIndex(uint id, int index);

	[DllImport("cri_ware_unity")]
	private static extern int criAtomExPlayback_GetCurrentBlockIndex(uint id);
}
