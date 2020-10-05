using System;
using System.Runtime.InteropServices;

public class CriFsLoader : IDisposable
{
	public enum Status
	{
		Stop,
		Loading,
		Complete,
		Error
	}

	private IntPtr handle;

	private GCHandle gch;

	public CriFsLoader()
	{
		CriFsPlugin.InitializeLibrary();
		this.handle = IntPtr.Zero;
		CriFsLoader.criFsLoader_Create(out this.handle);
		if (this.handle == IntPtr.Zero)
		{
			CriFsPlugin.FinalizeLibrary();
			throw new Exception("criFsLoader_Create() failed.");
		}
	}

	public void Dispose()
	{
		if (this.handle == IntPtr.Zero)
		{
			return;
		}
		CriFsLoader.criFsLoader_Destroy(this.handle);
		this.handle = IntPtr.Zero;
		if (this.gch.IsAllocated)
		{
			this.gch.Free();
		}
		CriFsPlugin.FinalizeLibrary();
		GC.SuppressFinalize(this);
	}

	public void Load(CriFsBinder binder, string path, long fileOffset, long loadSize, byte[] buffer)
	{
		this.gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
		CriFsLoader.criFsLoader_Load(this.handle, (binder == null) ? IntPtr.Zero : binder.nativeHandle, path, fileOffset, loadSize, this.gch.AddrOfPinnedObject(), (long)buffer.Length);
	}

	public void LoadById(CriFsBinder binder, int id, long fileOffset, long loadSize, byte[] buffer)
	{
		this.gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
		CriFsLoader.criFsLoader_LoadById(this.handle, (binder == null) ? IntPtr.Zero : binder.nativeHandle, id, fileOffset, loadSize, this.gch.AddrOfPinnedObject(), (long)buffer.Length);
	}

	public void Stop()
	{
		CriFsLoader.criFsLoader_Stop(this.handle);
	}

	public CriFsLoader.Status GetStatus()
	{
		CriFsLoader.Status status = CriFsLoader.Status.Stop;
		CriFsLoader.criFsLoader_GetStatus(this.handle, out status);
		if (status != CriFsLoader.Status.Loading && this.gch.IsAllocated)
		{
			this.gch.Free();
		}
		return status;
	}

	public void SetReadUnitSize(int unit_size)
	{
		CriFsLoader.criFsLoader_SetReadUnitSize(this.handle, (long)unit_size);
	}

	~CriFsLoader()
	{
		this.Dispose();
	}

	[DllImport("cri_ware_unity")]
	private static extern int criFsLoader_Create(out IntPtr loader);

	[DllImport("cri_ware_unity")]
	private static extern int criFsLoader_Destroy(IntPtr loader);

	[DllImport("cri_ware_unity")]
	private static extern int criFsLoader_Load(IntPtr loader, IntPtr binder, string path, long offset, long load_size, IntPtr buffer, long buffer_size);

	[DllImport("cri_ware_unity")]
	private static extern int criFsLoader_LoadById(IntPtr loader, IntPtr binder, int id, long offset, long load_size, IntPtr buffer, long buffer_size);

	[DllImport("cri_ware_unity")]
	private static extern int criFsLoader_Stop(IntPtr loader);

	[DllImport("cri_ware_unity")]
	private static extern int criFsLoader_GetStatus(IntPtr loader, out CriFsLoader.Status status);

	[DllImport("cri_ware_unity")]
	private static extern int criFsLoader_SetReadUnitSize(IntPtr loader, long unit_size);
}
