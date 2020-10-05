using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AssetBundleManager : MonoBehaviour
{
	private class WaitInfo
	{
		public global::AssetBundleRequest mRequest;
	}

	private enum CancelState
	{
		IDLE,
		CANCELING,
		CANCELED
	}

	private sealed class _ExecuteRequest_c__IteratorBF : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _count___0;

		internal int _i___1;

		internal global::AssetBundleRequest _req___2;

		internal bool _cancel___3;

		internal bool _exec___4;

		internal float _spendTime___5;

		internal float _waitTime___6;

		internal float _startTime___7;

		internal int _PC;

		internal object _current;

		internal AssetBundleManager __f__this;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._count___0 = this.__f__this.mExecuteList.Count;
				this._i___1 = 0;
				goto IL_293;
			case 1u:
				this._spendTime___5 = Time.realtimeSinceStartup - this._startTime___7;
				if (this._spendTime___5 >= this._waitTime___6)
				{
					goto IL_125;
				}
				break;
			case 2u:
				global::Debug.LogWarning(string.Concat(new object[]
				{
					"!!!!!! Load Retry [",
					this._req___2.TryCount,
					"/",
					this._req___2.MaxTryCount,
					"] : ",
					this._req___2.Url
				}), DebugTraceManager.TraceType.ASSETBUNDLE);
				this._req___2.Load();
				goto IL_26A;
			default:
				return false;
			}
			IL_EF:
			this._current = null;
			this._PC = 1;
			return true;
			IL_125:
			if (!this._req___2.IsLoaded())
			{
				this._waitTime___6 = this._req___2.UpdateElapsedTime(this._spendTime___5);
				if (!this._req___2.IsTimeOut())
				{
					this._startTime___7 = Time.realtimeSinceStartup;
					goto IL_EF;
				}
				global::Debug.Log("!!!!!!!!! AssetBundle TimeOut !!!!!!!!!", DebugTraceManager.TraceType.ASSETBUNDLE);
			}
			if (this.__f__this.mCancelState == AssetBundleManager.CancelState.CANCELING)
			{
				global::Debug.LogWarning("-------------- AssetBundleRequest.ExecuteRequest Cancel --------------", DebugTraceManager.TraceType.ASSETBUNDLE);
				this.__f__this.mCancelState = AssetBundleManager.CancelState.CANCELED;
				this.__f__this.mExecuting = false;
				this._exec___4 = false;
				this._cancel___3 = true;
			}
			else
			{
				this._req___2.Result();
				if (this._req___2.IsRetry())
				{
					this._current = new WaitForSeconds(2f);
					this._PC = 2;
					return true;
				}
				if (this._req___2.IsFailed())
				{
					this.__f__this.mAssetDic.Remove(this._req___2.path);
				}
				this._exec___4 = false;
				this.__f__this.mRemainingList.Remove(this._req___2);
			}
			IL_26A:
			if (this._exec___4)
			{
				this._spendTime___5 = 0f;
				goto IL_125;
			}
			if (this._cancel___3)
			{
				goto IL_2A4;
			}
			this._i___1++;
			IL_293:
			if (this._i___1 < this._count___0)
			{
				this._req___2 = this.__f__this.mExecuteList[this._i___1];
				this._cancel___3 = false;
				this._exec___4 = true;
				if (this._req___2.isCancel)
				{
					this._exec___4 = false;
				}
				else
				{
					this._req___2.Load();
				}
				goto IL_26A;
			}
			IL_2A4:
			this.__f__this.mExecuteList.Clear();
			this.__f__this.mRemainingList.Clear();
			this.__f__this.mExecuting = false;
			this.__f__this.ClearCancel();
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _WaitAsset_c__IteratorC0 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal AssetBundleManager.WaitInfo info;

		internal AssetBundleResult _result___0;

		internal MsgAssetBundleResponseSucceed _msg___1;

		internal int _PC;

		internal object _current;

		internal AssetBundleManager.WaitInfo ___info;

		internal AssetBundleManager __f__this;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!this.__f__this.IsAssetStandby(this.info.mRequest))
			{
				this._current = new WaitForSeconds(0.1f);
				this._PC = 1;
				return true;
			}
			this._result___0 = this.info.mRequest.CreateResult();
			if (this.info.mRequest.returnObject != null)
			{
				this._msg___1 = new MsgAssetBundleResponseSucceed(this.info.mRequest, this._result___0);
				this.info.mRequest.returnObject.SendMessage("AssetBundleResponseSucceed", this._msg___1, SendMessageOptions.DontRequireReceiver);
			}
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private Dictionary<string, global::AssetBundleRequest> mAssetDic = new Dictionary<string, global::AssetBundleRequest>();

	private static AssetBundleManager mInstance;

	private bool mExecuting;

	private List<global::AssetBundleRequest> mRequestList = new List<global::AssetBundleRequest>();

	private List<global::AssetBundleRequest> mExecuteList = new List<global::AssetBundleRequest>();

	private List<global::AssetBundleRequest> mRemainingList = new List<global::AssetBundleRequest>();

	private List<string> mReqUnloadList = new List<string>();

	public int mAllLoadedAssetCount;

	public int mLoadedTextureAssetCount;

	private AssetBundleManager.CancelState mCancelState;

	private bool mCancelRequest;

	public int Count
	{
		get
		{
			return this.mAssetDic.Count;
		}
	}

	public static AssetBundleManager Instance
	{
		get
		{
			return AssetBundleManager.mInstance;
		}
	}

	public int RequestCount
	{
		get
		{
			return this.mRequestList.Count;
		}
	}

	public bool Executing
	{
		get
		{
			return this.mExecuting;
		}
	}

	protected void Awake()
	{
		this.CheckInstance();
	}

	private void OnDestroy()
	{
		if (AssetBundleManager.mInstance == this)
		{
			AssetBundleManager.mInstance = null;
		}
	}

	protected bool CheckInstance()
	{
		if (AssetBundleManager.mInstance == null)
		{
			AssetBundleManager.mInstance = this;
			return true;
		}
		if (this == AssetBundleManager.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(this);
		return false;
	}

	public static void Create()
	{
		if (AssetBundleManager.mInstance == null)
		{
			GameObject gameObject = new GameObject("AssetBundleManager");
			gameObject.AddComponent<AssetBundleManager>();
			global::Debug.Log("AssetBundleManager.Create", DebugTraceManager.TraceType.ASSETBUNDLE);
		}
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if (this.mReqUnloadList.Count > 0)
		{
			foreach (string current in this.mReqUnloadList)
			{
				this.Unload(current);
			}
			this.mReqUnloadList.Clear();
		}
		if (this.mExecuting)
		{
			return;
		}
		int count = this.mRequestList.Count;
		if (0 < count)
		{
			this.mExecuteList.Clear();
			this.mRemainingList.Clear();
			for (int i = 0; i < count; i++)
			{
				this.mExecuteList.Add(this.mRequestList[i]);
				this.mRemainingList.Add(this.mRequestList[i]);
			}
			this.mRequestList.Clear();
			this.mExecuting = true;
			base.StartCoroutine("ExecuteRequest");
		}
	}

	public global::AssetBundleRequest RequestNoCache(string path, global::AssetBundleRequest.Type type, GameObject returnObject)
	{
		return this.Request(path, 0, 0u, type, returnObject, false);
	}

	public global::AssetBundleRequest Request(string path, int version, uint crc, global::AssetBundleRequest.Type type, GameObject returnObject, bool useCache)
	{
		if (this.IsCancelRequest())
		{
			return null;
		}
		global::AssetBundleRequest assetBundleRequest = null;
		if (this.mAssetDic.TryGetValue(path, out assetBundleRequest))
		{
			global::Debug.Log("AssetBundleManager.Request : already exist : " + path, DebugTraceManager.TraceType.ASSETBUNDLE);
			assetBundleRequest.Result();
			if (returnObject != null)
			{
				MsgAssetBundleResponseSucceed value = new MsgAssetBundleResponseSucceed(assetBundleRequest, assetBundleRequest.assetbundleResult);
				assetBundleRequest.returnObject.SendMessage("AssetBundleResponseSucceed", value, SendMessageOptions.DontRequireReceiver);
			}
			return assetBundleRequest;
		}
		assetBundleRequest = new global::AssetBundleRequest(path, version, crc, type, returnObject, useCache);
		assetBundleRequest.TimeOut = global::AssetBundleRequest.DefaultTimeOut;
		this.mAssetDic[assetBundleRequest.path] = assetBundleRequest;
		this.mRequestList.Add(assetBundleRequest);
		return assetBundleRequest;
	}

	public global::AssetBundleRequest ReRequest(global::AssetBundleRequest request)
	{
		if (this.IsCancelRequest())
		{
			return null;
		}
		global::AssetBundleRequest assetBundleRequest = new global::AssetBundleRequest(request);
		request.TimeOut += global::AssetBundleRequest.DefaultTimeOut;
		this.mAssetDic[request.path] = assetBundleRequest;
		this.mRequestList.Add(assetBundleRequest);
		return assetBundleRequest;
	}

	public global::AssetBundleRequest GetResource(string path)
	{
		global::AssetBundleRequest result = null;
		if (this.mAssetDic.TryGetValue(path, out result))
		{
			return result;
		}
		return null;
	}

	public void Unload(string url)
	{
		global::AssetBundleRequest assetBundleRequest = null;
		bool flag = false;
		if (this.mAssetDic.TryGetValue(url, out assetBundleRequest))
		{
			assetBundleRequest.Unload();
			if (this.mAssetDic.Remove(url))
			{
				flag = true;
			}
		}
		if (flag)
		{
			global::Debug.Log("AssetBundleManager.Unload() : " + url, DebugTraceManager.TraceType.ASSETBUNDLE);
		}
		else
		{
			global::Debug.LogWarning("AssetBundleManager.Unload : But [" + url + "] is not exist", DebugTraceManager.TraceType.ASSETBUNDLE);
		}
	}

	public void UnloadWithCancel(string url)
	{
		global::AssetBundleRequest assetBundleRequest = null;
		if (this.mAssetDic.TryGetValue(url, out assetBundleRequest))
		{
			if (assetBundleRequest.IsLoading())
			{
				assetBundleRequest.Cancel();
			}
			else
			{
				assetBundleRequest.Unload();
			}
			if (this.mAssetDic.Remove(url))
			{
			}
		}
	}

	public void RemoveAll()
	{
		foreach (KeyValuePair<string, global::AssetBundleRequest> current in this.mAssetDic)
		{
			current.Value.Unload();
		}
		this.mAssetDic.Clear();
		global::Debug.Log("Remove all asset bundles", DebugTraceManager.TraceType.ASSETBUNDLE);
	}

	private IEnumerator ExecuteRequest()
	{
		AssetBundleManager._ExecuteRequest_c__IteratorBF _ExecuteRequest_c__IteratorBF = new AssetBundleManager._ExecuteRequest_c__IteratorBF();
		_ExecuteRequest_c__IteratorBF.__f__this = this;
		return _ExecuteRequest_c__IteratorBF;
	}

	public bool IsAssetStandby(global::AssetBundleRequest request)
	{
		return request.assetbundleResult != null;
	}

	public void RequestWaitAsset(global::AssetBundleRequest request)
	{
		if (this.IsAssetStandby(request))
		{
			AssetBundleResult assetbundleResult = request.assetbundleResult;
			if (request.returnObject != null)
			{
				MsgAssetBundleResponseSucceed value = new MsgAssetBundleResponseSucceed(request, assetbundleResult);
				request.returnObject.SendMessage("AssetBundleResponseSucceed", value, SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			base.StartCoroutine("WaitAsset", new AssetBundleManager.WaitInfo
			{
				mRequest = request
			});
		}
	}

	private IEnumerator WaitAsset(AssetBundleManager.WaitInfo info)
	{
		AssetBundleManager._WaitAsset_c__IteratorC0 _WaitAsset_c__IteratorC = new AssetBundleManager._WaitAsset_c__IteratorC0();
		_WaitAsset_c__IteratorC.info = info;
		_WaitAsset_c__IteratorC.___info = info;
		_WaitAsset_c__IteratorC.__f__this = this;
		return _WaitAsset_c__IteratorC;
	}

	public void Cancel()
	{
		if (0 >= this.mExecuteList.Count)
		{
			this.mCancelState = AssetBundleManager.CancelState.IDLE;
			return;
		}
		this.mCancelRequest = true;
		this.mCancelState = AssetBundleManager.CancelState.CANCELING;
	}

	public bool IsCanceled()
	{
		return this.mCancelState == AssetBundleManager.CancelState.IDLE || AssetBundleManager.CancelState.CANCELED == this.mCancelState;
	}

	public bool IsCancelRequest()
	{
		return this.mCancelRequest;
	}

	public void ClearCancel()
	{
		this.mCancelRequest = false;
		this.mCancelState = AssetBundleManager.CancelState.IDLE;
	}

	public int GetRemainingCount()
	{
		if (this.mRemainingList == null)
		{
			return 0;
		}
		return this.mRemainingList.Count;
	}

	public global::AssetBundleRequest GetRemainingRequest(int index)
	{
		if (this.mRemainingList != null && 0 <= index && this.mRemainingList.Count > index)
		{
			return this.mRemainingList[index];
		}
		return null;
	}

	public void DebugPrintLoadedList()
	{
		string text = string.Empty;
		foreach (string current in this.mAssetDic.Keys)
		{
			text = text + current + "\n";
		}
		global::Debug.Log(this + ".DebugPrintLoadedList : \n" + text, DebugTraceManager.TraceType.ASSETBUNDLE);
	}

	public void RequestUnload(string url)
	{
		this.mReqUnloadList.Add(url);
	}
}
