using System;
using UnityEngine;

internal class WWWRequest
{
	private WWW m_www;

	private bool m_checkTime;

	private float m_startTime;

	private bool m_isEnd;

	private bool m_isTimeOut;

	public static readonly float DefaultConnectTime = 60f;

	private float m_connectTime = WWWRequest.DefaultConnectTime;

	public WWWRequest(string url, bool checkTime = false)
	{
		this.m_www = new WWW(url);
		this.m_startTime = Time.realtimeSinceStartup;
		this.m_checkTime = checkTime;
	}

	public void SetConnectTime(float connectTime)
	{
		if (connectTime > 180f)
		{
			connectTime = 180f;
		}
		this.m_connectTime = connectTime;
	}

	public void Remove()
	{
		try
		{
			if (this.m_www != null)
			{
			}
		}
		catch (Exception ex)
		{
			global::Debug.Log("WWWRequest.Remove():Exception->Message = " + ex.Message + ",ToString() = " + ex.ToString());
		}
		this.m_www = null;
	}

	public void Cancel()
	{
		this.Remove();
	}

	public void Update()
	{
		if (!this.m_isTimeOut)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float num = realtimeSinceStartup - this.m_startTime;
			if (num >= this.m_connectTime)
			{
				this.m_isTimeOut = true;
			}
		}
		if (!this.m_isEnd)
		{
			if (this.m_www != null)
			{
				if (this.m_www.isDone)
				{
					this.m_isEnd = true;
				}
			}
			else
			{
				this.m_isEnd = true;
			}
			if (this.m_checkTime)
			{
				float realtimeSinceStartup2 = Time.realtimeSinceStartup;
				global::Debug.Log("LS:Load File: " + this.m_www.url + " Time is " + (realtimeSinceStartup2 - this.m_startTime).ToString());
			}
		}
	}

	public bool IsEnd()
	{
		return this.m_isEnd;
	}

	public bool IsTimeOut()
	{
		return this.m_isTimeOut;
	}

	public string GetError()
	{
		if (this.m_www != null)
		{
			return this.m_www.error;
		}
		return null;
	}

	public byte[] GetResult()
	{
		if (this.m_www != null)
		{
			return this.m_www.bytes;
		}
		return null;
	}

	public string GetResultString()
	{
		if (this.m_www != null)
		{
			return this.m_www.text;
		}
		return null;
	}

	public int GetResultSize()
	{
		if (this.m_www != null)
		{
			return this.m_www.size;
		}
		return 0;
	}
}
