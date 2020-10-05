using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CriFsLoadAssetBundleRequest : CriFsRequest
{
	private CriFsLoadFileRequest loadFileReq;

	private AssetBundleCreateRequest assetBundleReq;

	private string _path_k__BackingField;

	private AssetBundle _assetBundle_k__BackingField;

	public string path
	{
		get;
		private set;
	}

	public AssetBundle assetBundle
	{
		get;
		private set;
	}

	public CriFsLoadAssetBundleRequest(CriFsBinder binder, string path, int readUnitSize)
	{
		this.path = path;
		this.loadFileReq = CriFsUtility.LoadFile(binder, path, readUnitSize);
	}

	public override void Update()
	{
		if (this.loadFileReq != null)
		{
			if (this.loadFileReq.isDone)
			{
				if (this.loadFileReq.error != null)
				{
					base.error = "Error occurred.";
					base.Done();
				}
				else
				{
					this.assetBundleReq = AssetBundle.CreateFromMemory(this.loadFileReq.bytes);
				}
				this.loadFileReq = null;
			}
		}
		else if (this.assetBundleReq != null)
		{
			if (this.assetBundleReq.isDone)
			{
				this.assetBundle = this.assetBundleReq.assetBundle;
				base.Done();
			}
		}
		else
		{
			base.Done();
		}
	}

	public override void Dispose()
	{
		if (base.isDisposed)
		{
			return;
		}
		if (this.loadFileReq != null)
		{
			this.loadFileReq.Dispose();
			this.loadFileReq = null;
		}
		GC.SuppressFinalize(this);
		base.isDisposed = true;
	}
}
