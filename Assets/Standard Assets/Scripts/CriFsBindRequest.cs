using System;
using System.Runtime.CompilerServices;

public class CriFsBindRequest : CriFsRequest
{
	public enum BindType
	{
		Cpk,
		Directory,
		File
	}

	private string _path_k__BackingField;

	private uint _bindId_k__BackingField;

	public string path
	{
		get;
		private set;
	}

	public uint bindId
	{
		get;
		private set;
	}

	public CriFsBindRequest(CriFsBindRequest.BindType type, CriFsBinder targetBinder, CriFsBinder srcBinder, string path)
	{
		this.path = path;
		switch (type)
		{
		case CriFsBindRequest.BindType.Cpk:
			this.bindId = targetBinder.BindCpk(srcBinder, path);
			break;
		case CriFsBindRequest.BindType.Directory:
			this.bindId = targetBinder.BindDirectory(srcBinder, path);
			break;
		case CriFsBindRequest.BindType.File:
			this.bindId = targetBinder.BindFile(srcBinder, path);
			break;
		default:
			throw new Exception("Invalid bind type.");
		}
	}

	public override void Stop()
	{
	}

	public override void Update()
	{
		if (base.isDone)
		{
			return;
		}
		CriFsBinder.Status status = CriFsBinder.GetStatus(this.bindId);
		if (status == CriFsBinder.Status.Analyze)
		{
			return;
		}
		if (status == CriFsBinder.Status.Error)
		{
			base.error = "Error occurred.";
		}
		base.Done();
	}

	public override void Dispose()
	{
		if (base.isDisposed)
		{
			return;
		}
		GC.SuppressFinalize(this);
		base.isDisposed = true;
	}
}
