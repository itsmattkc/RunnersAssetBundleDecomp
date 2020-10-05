using System;
using System.Runtime.InteropServices;

public class CriAtomExPlayerOutputAnalyzer : IDisposable
{
	public enum Type
	{
		LevelMeter
	}

	private IntPtr handle = IntPtr.Zero;

	private CriAtomExPlayer player;

	public IntPtr nativeHandle
	{
		get
		{
			return this.handle;
		}
	}

	public CriAtomExPlayerOutputAnalyzer(CriAtomExPlayerOutputAnalyzer.Type[] types)
	{
		this.handle = CriAtomExPlayerOutputAnalyzer.criAtomExPlayerOutputAnalyzer_Create(types.Length, types);
		if (this.handle == IntPtr.Zero)
		{
			throw new Exception("criAtomExPlayerOutputAnalyzer_Create() failed.");
		}
	}

	public void Dispose()
	{
		if (this.handle == IntPtr.Zero)
		{
			return;
		}
		CriAtomExPlayerOutputAnalyzer.criAtomExPlayerOutputAnalyzer_Destroy(this.handle);
		GC.SuppressFinalize(this);
	}

	public void AttachExPlayer(CriAtomExPlayer player)
	{
		if (player == null || this.handle == IntPtr.Zero)
		{
			return;
		}
		this.DetachExPlayer();
		CriAtomExPlayerOutputAnalyzer.criAtomExPlayerOutputAnalyzer_AttachExPlayer(this.handle, player.nativeHandle);
		this.player = player;
	}

	public void DetachExPlayer()
	{
		if (this.player == null || this.handle == IntPtr.Zero)
		{
			return;
		}
		CriAtomExPlayerOutputAnalyzer.criAtomExPlayerOutputAnalyzer_DetachExPlayer(this.handle, this.player.nativeHandle);
	}

	public float GetRms(int channel)
	{
		if (this.player == null || this.handle == IntPtr.Zero)
		{
			return 0f;
		}
		if (this.player.GetStatus() != CriAtomExPlayer.Status.Playing && this.player.GetStatus() != CriAtomExPlayer.Status.Prep)
		{
			return 0f;
		}
		return CriAtomExPlayerOutputAnalyzer.criAtomExPlayerOutputAnalyzer_GetRms(this.handle, channel);
	}

	~CriAtomExPlayerOutputAnalyzer()
	{
		this.Dispose();
	}

	[DllImport("cri_ware_unity")]
	private static extern IntPtr criAtomExPlayerOutputAnalyzer_Create(int numTypes, CriAtomExPlayerOutputAnalyzer.Type[] types);

	[DllImport("cri_ware_unity")]
	private static extern IntPtr criAtomExPlayerOutputAnalyzer_Destroy(IntPtr analyzer);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayerOutputAnalyzer_AttachExPlayer(IntPtr analyzer, IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern void criAtomExPlayerOutputAnalyzer_DetachExPlayer(IntPtr analyzer, IntPtr player);

	[DllImport("cri_ware_unity")]
	private static extern float criAtomExPlayerOutputAnalyzer_GetRms(IntPtr analyzer, int channel);
}
