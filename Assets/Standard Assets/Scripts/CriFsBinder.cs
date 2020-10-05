using System;
using System.Runtime.InteropServices;

public class CriFsBinder : IDisposable
{
	public enum Status
	{
		None,
		Analyze,
		Complete,
		Unbind,
		Removed,
		Invalid,
		Error
	}

	private IntPtr handle;

	public IntPtr nativeHandle
	{
		get
		{
			return this.handle;
		}
	}

	public CriFsBinder()
	{
		CriFsPlugin.InitializeLibrary();
		this.handle = IntPtr.Zero;
		CriFsBinder.criFsBinder_Create(out this.handle);
		if (this.handle == IntPtr.Zero)
		{
			CriFsPlugin.FinalizeLibrary();
			throw new Exception("criFsBinder_Create() failed.");
		}
	}

	public void Dispose()
	{
		if (this.handle == IntPtr.Zero)
		{
			return;
		}
		CriFsBinder.criFsBinder_Destroy(this.handle);
		this.handle = IntPtr.Zero;
		CriFsPlugin.FinalizeLibrary();
		GC.SuppressFinalize(this);
	}

	public uint BindCpk(CriFsBinder srcBinder, string path)
	{
		uint result;
		CriFsBinder.criFsBinder_BindCpk(this.handle, (srcBinder == null) ? IntPtr.Zero : srcBinder.nativeHandle, path, IntPtr.Zero, 0, out result);
		return result;
	}

	public uint BindDirectory(CriFsBinder srcBinder, string path)
	{
		uint result;
		CriFsBinder.criFsBinder_BindDirectory(this.handle, (srcBinder == null) ? IntPtr.Zero : srcBinder.nativeHandle, path, IntPtr.Zero, 0, out result);
		return result;
	}

	public uint BindFile(CriFsBinder srcBinder, string path)
	{
		uint result;
		CriFsBinder.criFsBinder_BindFile(this.handle, (srcBinder == null) ? IntPtr.Zero : srcBinder.nativeHandle, path, IntPtr.Zero, 0, out result);
		return result;
	}

	public static void Unbind(uint bindId)
	{
		CriFsBinder.criFsBinder_Unbind(bindId);
	}

	public static CriFsBinder.Status GetStatus(uint bindId)
	{
		CriFsBinder.Status result;
		CriFsBinder.criFsBinder_GetStatus(bindId, out result);
		return result;
	}

	public long GetFileSize(string path)
	{
		long result;
		int num = CriFsBinder.criFsBinder_GetFileSize(this.handle, path, out result);
		if (num != 0)
		{
			return -1L;
		}
		return result;
	}

	public long GetFileSize(int id)
	{
		long result;
		int num = CriFsBinder.criFsBinder_GetFileSizeById(this.handle, id, out result);
		if (num != 0)
		{
			return -1L;
		}
		return result;
	}

	public static void SetPriority(uint bindId, int priority)
	{
		CriFsBinder.criFsBinder_SetPriority(bindId, priority);
	}

	~CriFsBinder()
	{
		this.Dispose();
	}

	[DllImport("cri_ware_unity")]
	private static extern uint criFsBinder_Create(out IntPtr binder);

	[DllImport("cri_ware_unity")]
	private static extern uint criFsBinder_Destroy(IntPtr binder);

	[DllImport("cri_ware_unity")]
	private static extern uint criFsBinder_BindCpk(IntPtr binder, IntPtr srcBinder, string path, IntPtr work, int worksize, out uint bindId);

	[DllImport("cri_ware_unity")]
	private static extern uint criFsBinder_BindDirectory(IntPtr binder, IntPtr srcBinder, string path, IntPtr work, int worksize, out uint bindId);

	[DllImport("cri_ware_unity")]
	private static extern uint criFsBinder_BindFile(IntPtr binder, IntPtr srcBinder, string path, IntPtr work, int worksize, out uint bindId);

	[DllImport("cri_ware_unity")]
	private static extern int criFsBinder_Unbind(uint bindId);

	[DllImport("cri_ware_unity")]
	private static extern int criFsBinder_GetStatus(uint bindId, out CriFsBinder.Status status);

	[DllImport("cri_ware_unity")]
	private static extern int criFsBinder_GetFileSize(IntPtr binder, string path, out long size);

	[DllImport("cri_ware_unity")]
	private static extern int criFsBinder_GetFileSizeById(IntPtr binder, int id, out long size);

	[DllImport("cri_ware_unity")]
	private static extern int criFsBinder_SetPriority(uint bindId, int priority);
}
