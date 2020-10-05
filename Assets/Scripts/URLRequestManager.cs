using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class URLRequestManager : MonoBehaviour
{
	private enum CancelState
	{
		IDLE,
		CANCELING,
		CANCELED
	}

	private sealed class _ExecuteRequest_c__IteratorB9 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _count___0;

		internal int _i___1;

		internal URLRequest _request___2;

		internal bool _cancel___3;

		internal bool _exec___4;

		internal WWWForm _form___5;

		internal string _param___6;

		internal float _startTime___7;

		internal float _spendTime___8;

		internal float _waitTime___9;

		internal float _startTime___10;

		internal int _PC;

		internal object _current;

		internal URLRequestManager __f__this;

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
				goto IL_2D4;
			case 1u:
				if (Time.realtimeSinceStartup - this._startTime___7 >= 1f)
				{
					this._request___2.DidReceiveSuccess(null);
					goto IL_21B;
				}
				goto IL_138;
			case 2u:
				this._spendTime___8 = Time.realtimeSinceStartup - this._startTime___10;
				if (this._spendTime___8 >= this._waitTime___9)
				{
					goto IL_20B;
				}
				goto IL_1D5;
			default:
				return false;
			}
			IL_2AB:
			while (this._exec___4)
			{
				if (!this.__f__this.Emulation && !this._request___2.Emulation)
				{
					this.__f__this.mCurrentRequest = this._request___2;
					this._request___2.Begin();
					this._spendTime___8 = 0f;
					goto IL_20B;
				}
				this._request___2.PreBegin();
				this._form___5 = this._request___2.CreateWWWForm();
				if (this._form___5 != null)
				{
					this._param___6 = Encoding.ASCII.GetString(this._form___5.data);
					global::Debug.Log(string.Concat(new string[]
					{
						"URLRequest Emulation : [",
						this._request___2.url,
						"] [",
						this._param___6,
						"]"
					}), DebugTraceManager.TraceType.SERVER);
					this._startTime___7 = Time.realtimeSinceStartup;
					goto IL_138;
				}
			}
			if (this._cancel___3)
			{
				goto IL_2E5;
			}
			goto IL_2C6;
			IL_138:
			this._current = null;
			this._PC = 1;
			return true;
			IL_1D5:
			this._current = null;
			this._PC = 2;
			return true;
			IL_20B:
			if (!this._request___2.IsDone())
			{
				this._waitTime___9 = this._request___2.UpdateElapsedTime(this._spendTime___8);
				if (!this._request___2.IsTimeOut())
				{
					this._startTime___10 = Time.realtimeSinceStartup;
					goto IL_1D5;
				}
			}
			IL_21B:
			if (this.__f__this.mCancelState == URLRequestManager.CancelState.CANCELING)
			{
				global::Debug.Log("-------------- AssetBundleRequest.ExecuteRequest Cancel --------------", DebugTraceManager.TraceType.SERVER);
				this.__f__this.mCancelState = URLRequestManager.CancelState.CANCELED;
				this.__f__this.mExecuting = false;
				this._exec___4 = false;
				this._cancel___3 = true;
				goto IL_2AB;
			}
			if (!this.__f__this.Emulation && !this._request___2.Emulation)
			{
				this._request___2.Result();
			}
			this._exec___4 = false;
			this.__f__this.mRemainingList.Remove(this._request___2);
			goto IL_2AB;
			IL_2C6:
			this._i___1++;
			IL_2D4:
			if (this._i___1 < this._count___0)
			{
				this._request___2 = this.__f__this.mExecuteList[this._i___1];
				if (this._request___2 == null)
				{
					goto IL_2C6;
				}
				this._cancel___3 = false;
				this._exec___4 = true;
				goto IL_2AB;
			}
			IL_2E5:
			this.__f__this.mExecuteList.Clear();
			this.__f__this.mRemainingList.Clear();
			this.__f__this.mCurrentRequest = null;
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

	private const float EMULATION_WAIT_TIME = 1f;

	public bool mEmulation;

	private List<URLRequest> mRequestList;

	private List<URLRequest> mExecuteList;

	private List<URLRequest> mRemainingList;

	private bool mExecuting;

	private static URLRequestManager mInstance;

	private URLRequest mCurrentRequest;

	private URLRequestManager.CancelState mCancelState;

	private bool mCancelRequest;

	public static URLRequestManager Instance
	{
		get
		{
			return URLRequestManager.mInstance;
		}
	}

	public bool Emulation
	{
		get
		{
			return this.mEmulation;
		}
		set
		{
			this.mEmulation = value;
		}
	}

	private void Awake()
	{
		if (URLRequestManager.mInstance == null)
		{
			URLRequestManager.mInstance = this;
			this.mEmulation = false;
			UnityEngine.Object.DontDestroyOnLoad(this);
			this.mRequestList = new List<URLRequest>();
			this.mExecuteList = new List<URLRequest>();
			this.mRemainingList = new List<URLRequest>();
			this.mExecuting = false;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.mExecuting)
		{
			return;
		}
		if (0 < this.mRequestList.Count)
		{
			this.mExecuteList.Clear();
			this.mRemainingList.Clear();
			int count = this.mRequestList.Count;
			for (int i = 0; i < count; i++)
			{
				URLRequest item = this.mRequestList[i];
				this.mExecuteList.Add(item);
				this.mRemainingList.Add(item);
			}
			this.mRequestList.Clear();
			this.mExecuting = true;
			base.StartCoroutine("ExecuteRequest");
		}
	}

	private IEnumerator ExecuteRequest()
	{
		URLRequestManager._ExecuteRequest_c__IteratorB9 _ExecuteRequest_c__IteratorB = new URLRequestManager._ExecuteRequest_c__IteratorB9();
		_ExecuteRequest_c__IteratorB.__f__this = this;
		return _ExecuteRequest_c__IteratorB;
	}

	public void Request(URLRequest request)
	{
		this.mRequestList.Add(request);
	}

	public void Cancel()
	{
		if (0 >= this.mExecuteList.Count)
		{
			this.mCancelState = URLRequestManager.CancelState.IDLE;
			return;
		}
		this.mCancelRequest = true;
		this.mCancelState = URLRequestManager.CancelState.CANCELING;
	}

	public bool IsCanceled()
	{
		return this.mCancelState == URLRequestManager.CancelState.IDLE || URLRequestManager.CancelState.CANCELED == this.mCancelState;
	}

	public bool IsCancelRequest()
	{
		return this.mCancelRequest;
	}

	public void ClearCancel()
	{
		this.mCancelRequest = false;
		this.mCancelState = URLRequestManager.CancelState.IDLE;
	}

	public int GetRemainingCount()
	{
		if (this.mRemainingList == null)
		{
			return 0;
		}
		return this.mRemainingList.Count;
	}

	public URLRequest GetRemainingRequest(int index)
	{
		if (this.mRemainingList != null && 0 <= index && this.mRemainingList.Count > index)
		{
			return this.mRemainingList[index];
		}
		return null;
	}
}
