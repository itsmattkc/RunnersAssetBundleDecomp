using System;
using System.Runtime.CompilerServices;

public class CriFsInstallRequest : CriFsRequest
{
	private CriFsInstaller installer;

	private string _sourcePath_k__BackingField;

	private string _destinationPath_k__BackingField;

	private float _progress_k__BackingField;

	public string sourcePath
	{
		get;
		private set;
	}

	public string destinationPath
	{
		get;
		private set;
	}

	public float progress
	{
		get;
		private set;
	}

	public CriFsInstallRequest(CriFsBinder srcBinder, string srcPath, string dstPath, CriFsRequest.DoneDelegate doneDelegate, int installBufferSize)
	{
		this.sourcePath = srcPath;
		this.destinationPath = dstPath;
		base.doneDelegate = doneDelegate;
		this.progress = 0f;
		this.installer = new CriFsInstaller();
		this.installer.Copy(srcBinder, srcPath, dstPath, installBufferSize);
	}

	public override void Stop()
	{
		if (this.installer != null)
		{
			this.installer.Stop();
		}
	}

	public override void Update()
	{
		if (this.installer == null)
		{
			return;
		}
		this.progress = this.installer.GetProgress();
		CriFsInstaller.Status status = this.installer.GetStatus();
		if (status == CriFsInstaller.Status.Busy)
		{
			return;
		}
		if (status == CriFsInstaller.Status.Error)
		{
			this.progress = -1f;
			base.error = "Error occurred.";
		}
		this.installer.Dispose();
		this.installer = null;
		base.Done();
	}

	public override void Dispose()
	{
		if (base.isDisposed)
		{
			return;
		}
		if (this.installer != null)
		{
			this.installer.Dispose();
			this.installer = null;
		}
		GC.SuppressFinalize(this);
		base.isDisposed = true;
	}
}
