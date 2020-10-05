using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CriFsServer : MonoBehaviour
{
	private static CriFsServer _instance;

	private List<CriFsRequest> requestList;

	private int _installBufferSize_k__BackingField;

	private static Predicate<CriFsRequest> __f__am_cache3;

	public static CriFsServer instance
	{
		get
		{
			CriFsServer.CreateInstance();
			return CriFsServer._instance;
		}
	}

	public int installBufferSize
	{
		get;
		private set;
	}

	public static void CreateInstance()
	{
		if (CriFsServer._instance == null)
		{
			CriWare.managerObject.AddComponent<CriFsServer>();
			CriFsServer._instance.installBufferSize = CriFsPlugin.installBufferSize;
		}
	}

	public static void DestroyInstance()
	{
		if (CriFsServer._instance != null)
		{
			UnityEngine.Object.Destroy(CriFsServer._instance);
		}
	}

	private void Awake()
	{
		if (CriFsServer._instance == null)
		{
			CriFsServer._instance = this;
			this.requestList = new List<CriFsRequest>();
			CriFsRequest item = new CriFsRequest();
			this.requestList.Add(item);
			this.requestList.RemoveAt(0);
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void OnDestroy()
	{
		if (CriFsServer._instance == this)
		{
			foreach (CriFsRequest current in this.requestList)
			{
				current.Dispose();
			}
			CriFsServer._instance = null;
		}
	}

	private void Update()
	{
		CriFsInstaller.ExecuteMain();
		foreach (CriFsRequest current in this.requestList)
		{
			current.Update();
		}
		this.requestList.RemoveAll((CriFsRequest request) => request.isDone || request.isDisposed);
	}

	public void AddRequest(CriFsRequest request)
	{
		this.requestList.Add(request);
	}

	public CriFsLoadFileRequest LoadFile(CriFsBinder binder, string path, CriFsRequest.DoneDelegate doneDelegate, int readUnitSize)
	{
		CriFsLoadFileRequest criFsLoadFileRequest = new CriFsLoadFileRequest(binder, path, doneDelegate, readUnitSize);
		this.AddRequest(criFsLoadFileRequest);
		return criFsLoadFileRequest;
	}

	public CriFsLoadAssetBundleRequest LoadAssetBundle(CriFsBinder binder, string path, int readUnitSize)
	{
		CriFsLoadAssetBundleRequest criFsLoadAssetBundleRequest = new CriFsLoadAssetBundleRequest(binder, path, readUnitSize);
		this.AddRequest(criFsLoadAssetBundleRequest);
		return criFsLoadAssetBundleRequest;
	}

	public CriFsInstallRequest Install(CriFsBinder srcBinder, string srcPath, string dstPath, CriFsRequest.DoneDelegate doneDelegate)
	{
		CriFsInstallRequest criFsInstallRequest = new CriFsInstallRequest(srcBinder, srcPath, dstPath, doneDelegate, this.installBufferSize);
		this.requestList.Add(criFsInstallRequest);
		return criFsInstallRequest;
	}

	public CriFsBindRequest BindCpk(CriFsBinder targetBinder, CriFsBinder srcBinder, string path)
	{
		CriFsBindRequest criFsBindRequest = new CriFsBindRequest(CriFsBindRequest.BindType.Cpk, targetBinder, srcBinder, path);
		this.AddRequest(criFsBindRequest);
		return criFsBindRequest;
	}

	public CriFsBindRequest BindDirectory(CriFsBinder targetBinder, CriFsBinder srcBinder, string path)
	{
		CriFsBindRequest criFsBindRequest = new CriFsBindRequest(CriFsBindRequest.BindType.Directory, targetBinder, srcBinder, path);
		this.AddRequest(criFsBindRequest);
		return criFsBindRequest;
	}

	public CriFsBindRequest BindFile(CriFsBinder targetBinder, CriFsBinder srcBinder, string path)
	{
		CriFsBindRequest criFsBindRequest = new CriFsBindRequest(CriFsBindRequest.BindType.File, targetBinder, srcBinder, path);
		this.AddRequest(criFsBindRequest);
		return criFsBindRequest;
	}
}
