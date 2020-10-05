using System;
using System.Runtime.CompilerServices;

public class CriFsLoadFileRequest : CriFsRequest
{
	private enum Phase
	{
		Stop,
		Bind,
		Load,
		Done,
		Error
	}

	private CriFsLoadFileRequest.Phase phase;

	private CriFsBinder refBinder;

	private CriFsBinder newBinder;

	private uint bindId;

	private CriFsLoader loader;

	private int readUnitSize;

	private long fileSize;

	private string _path_k__BackingField;

	private byte[] _bytes_k__BackingField;

	public string path
	{
		get;
		private set;
	}

	public byte[] bytes
	{
		get;
		private set;
	}

	public CriFsLoadFileRequest(CriFsBinder srcBinder, string path, CriFsRequest.DoneDelegate doneDelegate, int readUnitSize)
	{
		this.path = path;
		base.doneDelegate = doneDelegate;
		this.readUnitSize = readUnitSize;
		if (srcBinder == null)
		{
			this.newBinder = new CriFsBinder();
			this.refBinder = this.newBinder;
			this.bindId = this.newBinder.BindFile(srcBinder, path);
			this.phase = CriFsLoadFileRequest.Phase.Bind;
		}
		else
		{
			this.newBinder = null;
			this.refBinder = srcBinder;
			this.fileSize = srcBinder.GetFileSize(path);
			if (this.fileSize < 0L)
			{
				this.phase = CriFsLoadFileRequest.Phase.Error;
			}
			else
			{
				this.phase = CriFsLoadFileRequest.Phase.Load;
			}
		}
	}

	public override void Dispose()
	{
		if (base.isDisposed)
		{
			return;
		}
		if (this.loader != null)
		{
			this.loader.Dispose();
			this.loader = null;
		}
		if (this.newBinder != null)
		{
			this.newBinder.Dispose();
			this.newBinder = null;
		}
		this.bytes = null;
		GC.SuppressFinalize(this);
		base.isDisposed = true;
	}

	public override void Stop()
	{
		if (this.phase == CriFsLoadFileRequest.Phase.Load && this.loader != null)
		{
			this.loader.Stop();
		}
	}

	public override void Update()
	{
		if (this.phase == CriFsLoadFileRequest.Phase.Bind)
		{
			this.UpdateBinder();
		}
		if (this.phase == CriFsLoadFileRequest.Phase.Load)
		{
			this.UpdateLoader();
		}
		if (this.phase == CriFsLoadFileRequest.Phase.Error)
		{
			this.OnError();
		}
	}

	private void UpdateBinder()
	{
		CriFsBinder.Status status = CriFsBinder.GetStatus(this.bindId);
		if (status == CriFsBinder.Status.Analyze)
		{
			return;
		}
		CriFsBinder.Status status2 = status;
		if (status2 != CriFsBinder.Status.Complete)
		{
			this.fileSize = -1L;
		}
		else
		{
			this.fileSize = this.refBinder.GetFileSize(this.path);
		}
		if (this.fileSize < 0L)
		{
			this.phase = CriFsLoadFileRequest.Phase.Error;
			return;
		}
		this.phase = CriFsLoadFileRequest.Phase.Load;
	}

	private void UpdateLoader()
	{
		if (this.loader == null)
		{
			this.loader = new CriFsLoader();
			this.loader.SetReadUnitSize(this.readUnitSize);
			this.bytes = new byte[this.fileSize];
			this.loader.Load(this.refBinder, this.path, 0L, this.fileSize, this.bytes);
		}
		CriFsLoader.Status status = this.loader.GetStatus();
		if (status == CriFsLoader.Status.Loading)
		{
			return;
		}
		switch (status)
		{
		case CriFsLoader.Status.Stop:
			this.bytes = null;
			break;
		case CriFsLoader.Status.Error:
			this.phase = CriFsLoadFileRequest.Phase.Error;
			return;
		}
		this.phase = CriFsLoadFileRequest.Phase.Done;
		this.loader.Dispose();
		this.loader = null;
		if (this.newBinder != null)
		{
			this.newBinder.Dispose();
			this.newBinder = null;
		}
		base.Done();
	}

	private void OnError()
	{
		this.bytes = null;
		base.error = "Error occurred.";
		this.refBinder = null;
		if (this.newBinder != null)
		{
			this.newBinder.Dispose();
			this.newBinder = null;
		}
		if (this.loader != null)
		{
			this.loader.Dispose();
			this.loader = null;
		}
		this.phase = CriFsLoadFileRequest.Phase.Done;
		base.Done();
	}
}
