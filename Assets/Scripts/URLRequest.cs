using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class URLRequest
{
	private bool mEmulation;

	private string mURL;

	private List<URLRequestParam> mParamList;

	private Action mDelegateRequest;

	private Action<WWW> mDelegateSuccess;

	private Action<WWW, bool, bool> mDelegateFailure;

	private bool mCompleted;

	private bool mNotReachability;

	private float mElapsedTime;

	private WWW mWWW;

	private string mFormString;

	public bool Completed
	{
		get
		{
			return this.mCompleted;
		}
	}

	public float ElapsedTime
	{
		get
		{
			return this.mElapsedTime;
		}
	}

	public List<URLRequestParam> ParamList
	{
		get
		{
			return this.mParamList;
		}
	}

	public string FormString
	{
		get
		{
			return this.mFormString;
		}
	}

	public string url
	{
		get
		{
			return this.mURL;
		}
		set
		{
			this.mURL = value;
		}
	}

	public Action beginRequest
	{
		get
		{
			return this.mDelegateRequest;
		}
		set
		{
			this.mDelegateRequest = value;
		}
	}

	public Action<WWW> success
	{
		get
		{
			return this.mDelegateSuccess;
		}
		set
		{
			this.mDelegateSuccess = value;
		}
	}

	public Action<WWW, bool, bool> failure
	{
		get
		{
			return this.mDelegateFailure;
		}
		set
		{
			this.mDelegateFailure = value;
		}
	}

	public float TimeOut
	{
		get
		{
			return NetUtil.ConnectTimeOut;
		}
		private set
		{
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

	public URLRequest() : this(string.Empty, null, null, null)
	{
	}

	public URLRequest(string url) : this(url, null, null, null)
	{
	}

	public URLRequest(string url, Action begin, Action<WWW> success, Action<WWW, bool, bool> failure)
	{
		this.mEmulation = URLRequestManager.Instance.Emulation;
		this.mURL = url;
		this.mParamList = new List<URLRequestParam>();
		this.mDelegateRequest = begin;
		this.mDelegateSuccess = success;
		this.mDelegateFailure = failure;
		this.mCompleted = false;
		this.mNotReachability = false;
		this.mElapsedTime = 0f;
	}

	public void AddParam(string propertyName, string value)
	{
		URLRequestParam item = new URLRequestParam(propertyName, value);
		this.mParamList.Add(item);
	}

	public void Add1stParam(string propertyName, string value)
	{
		URLRequestParam item = new URLRequestParam(propertyName, value);
		this.mParamList.Insert(0, item);
	}

	public WWWForm CreateWWWForm()
	{
		if (this.mParamList.Count == 0)
		{
			return null;
		}
		WWWForm wWWForm = new WWWForm();
		int count = this.mParamList.Count;
		for (int i = 0; i < count; i++)
		{
			URLRequestParam uRLRequestParam = this.mParamList[i];
			if (uRLRequestParam != null)
			{
				wWWForm.AddField(uRLRequestParam.propertyName, uRLRequestParam.value);
			}
		}
		return wWWForm;
	}

	public void DidReceiveSuccess(WWW www)
	{
		if (this.mDelegateSuccess != null)
		{
			this.mDelegateSuccess(www);
		}
	}

	public void DidReceiveFailure(WWW www)
	{
		if (this.mDelegateFailure != null)
		{
			this.mDelegateFailure(www, this.IsTimeOut(), this.IsNotReachability());
		}
	}

	public void PreBegin()
	{
		if (this.mDelegateRequest != null)
		{
			this.mDelegateRequest();
		}
	}

	public void Begin()
	{
		this.PreBegin();
		this.mElapsedTime = 0f;
		WWWForm wWWForm = this.CreateWWWForm();
		if (wWWForm == null)
		{
			this.mWWW = new WWW(this.mURL);
			global::Debug.Log("URLRequestManager.ExecuteRequest:" + URLRequest.UriDecode(this.mURL), DebugTraceManager.TraceType.SERVER);
			this.mFormString = null;
		}
		else
		{
			this.mWWW = new WWW(this.mURL, wWWForm);
			this.mFormString = Encoding.ASCII.GetString(wWWForm.data);
			global::Debug.Log("URLRequestManager.ExecuteRequest:" + URLRequest.UriDecode(this.mURL) + "  params:" + URLRequest.UriDecode(this.mFormString), DebugTraceManager.TraceType.SERVER);
		}
	}

	public float UpdateElapsedTime(float addElapsedTime)
	{
		this.mElapsedTime += addElapsedTime;
		return 0.1f;
	}

	public bool IsDone()
	{
		return this.mWWW.isDone;
	}

	public bool IsTimeOut()
	{
		return NetUtil.ConnectTimeOut <= this.mElapsedTime;
	}

	public bool IsNotReachability()
	{
		return this.mNotReachability;
	}

	public void Result()
	{
		if (this.IsTimeOut())
		{
			global::Debug.Log("Request : TimeOut : " + this.mURL, DebugTraceManager.TraceType.SERVER);
			this.DidReceiveFailure(null);
			this.mWWW = null;
			return;
		}
		if (!this.mWWW.isDone)
		{
			global::Debug.Log("WWW doesn't begin yet.", DebugTraceManager.TraceType.SERVER);
		}
		bool flag = null == this.mWWW.error;
		if (flag)
		{
			this.DidReceiveSuccess(this.mWWW);
		}
		else
		{
			this.DidReceiveFailure(this.mWWW);
		}
	}

	private static string UriDecode(string stringToUnescape)
	{
		return Uri.UnescapeDataString(stringToUnescape.Replace("+", "%20"));
	}
}
