using System;
using System.Runtime.InteropServices;

public class CriFsInstaller : IDisposable
{
	public enum Status
	{
		Stop,
		Busy,
		Complete,
		Error
	}

	private enum CopyPolicy
	{
		Always
	}

	private byte[] installBuffer;

	private GCHandle installBufferGch;

	private IntPtr handle;

	public CriFsInstaller()
	{
		CriFsPlugin.InitializeLibrary();
		this.handle = IntPtr.Zero;
		CriFsInstaller.criFsInstaller_Create(out this.handle, CriFsInstaller.CopyPolicy.Always);
		if (this.handle == IntPtr.Zero)
		{
			CriFsPlugin.FinalizeLibrary();
			throw new Exception("criFsInstaller_Create() failed.");
		}
	}

	public void Dispose()
	{
		if (this.handle == IntPtr.Zero)
		{
			return;
		}
		CriFsInstaller.criFsInstaller_Destroy(this.handle);
		this.handle = IntPtr.Zero;
		if (this.installBuffer != null)
		{
			this.installBufferGch.Free();
			this.installBuffer = null;
		}
		CriFsPlugin.FinalizeLibrary();
		GC.SuppressFinalize(this);
	}

	public void Copy(CriFsBinder binder, string srcPath, string dstPath, int installBufferSize)
	{
		string text = srcPath;
		if (text.StartsWith("http:") || text.StartsWith("https:"))
		{
			text = "net2:" + text;
		}
		if (installBufferSize > 0)
		{
			this.installBuffer = new byte[installBufferSize];
			this.installBufferGch = GCHandle.Alloc(this.installBuffer, GCHandleType.Pinned);
			CriFsInstaller.criFsInstaller_Copy(this.handle, (binder == null) ? IntPtr.Zero : binder.nativeHandle, text, dstPath, this.installBufferGch.AddrOfPinnedObject(), (long)this.installBuffer.Length);
		}
		else
		{
			CriFsInstaller.criFsInstaller_Copy(this.handle, (binder == null) ? IntPtr.Zero : binder.nativeHandle, text, dstPath, IntPtr.Zero, 0L);
		}
	}

	public void Stop()
	{
		CriFsInstaller.criFsInstaller_Stop(this.handle);
	}

	public CriFsInstaller.Status GetStatus()
	{
		CriFsInstaller.Status result;
		CriFsInstaller.criFsInstaller_GetStatus(this.handle, out result);
		return result;
	}

	public float GetProgress()
	{
		float result;
		CriFsInstaller.criFsInstaller_GetProgress(this.handle, out result);
		return result;
	}

	public static void ExecuteMain()
	{
		CriFsInstaller.criFsInstaller_ExecuteMain();
	}

	~CriFsInstaller()
	{
		this.Dispose();
	}

	[DllImport("cri_ware_unity")]
	private static extern void criFsInstaller_ExecuteMain();

	[DllImport("cri_ware_unity")]
	private static extern int criFsInstaller_Create(out IntPtr installer, CriFsInstaller.CopyPolicy option);

	[DllImport("cri_ware_unity")]
	private static extern int criFsInstaller_Destroy(IntPtr installer);

	[DllImport("cri_ware_unity")]
	private static extern int criFsInstaller_Copy(IntPtr installer, IntPtr binder, string src_path, string dst_path, IntPtr buffer, long buffer_size);

	[DllImport("cri_ware_unity")]
	private static extern int criFsInstaller_Stop(IntPtr installer);

	[DllImport("cri_ware_unity")]
	private static extern int criFsInstaller_GetStatus(IntPtr installer, out CriFsInstaller.Status status);

	[DllImport("cri_ware_unity")]
	private static extern int criFsInstaller_GetProgress(IntPtr installer, out float progress);
}
