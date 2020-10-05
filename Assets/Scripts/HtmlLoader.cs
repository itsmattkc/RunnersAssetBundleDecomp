using System;
using UnityEngine;

public abstract class HtmlLoader : MonoBehaviour
{
	private WWW m_www;

	private bool m_isEndLoad;

	public bool IsEndLoad
	{
		get
		{
			return this.m_isEndLoad;
		}
		protected set
		{
			this.m_isEndLoad = value;
		}
	}

	public HtmlLoader()
	{
	}

	private void OnDestroy()
	{
		if (this.m_www != null)
		{
			this.m_www.Dispose();
		}
	}

	public void Setup(string url)
	{
		this.m_www = new WWW(url);
		this.OnSetup();
	}

	public string GetUrlContentsText()
	{
		if (this.m_www == null)
		{
			return null;
		}
		return this.m_www.text;
	}

	protected WWW GetWWW()
	{
		return this.m_www;
	}

	protected abstract void OnSetup();
}
