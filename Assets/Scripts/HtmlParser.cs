using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HtmlParser : MonoBehaviour
{
	public enum SyncType
	{
		TYPE_SYNC,
		TYPE_ASYNC
	}

	private sealed class _ParseASync_c__Iterator29 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal string htmlString;

		internal bool _isEndParse___0;

		internal int _PC;

		internal object _current;

		internal string ___htmlString;

		internal HtmlParser __f__this;

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
				this.__f__this.BeginParse(this.htmlString);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			this._isEndParse___0 = this.__f__this.Parse();
			if (!this._isEndParse___0)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			this.__f__this.EndParse();
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

	private HtmlLoader m_loader;

	private HtmlParser.SyncType m_loadSyncType;

	private HtmlParser.SyncType m_parseSyncType;

	private TinyFsmBehavior m_fsm;

	private bool m_isEndParse;

	private string m_parsedString;

	private string m_url;

	public bool IsEndParse
	{
		get
		{
			return this.m_isEndParse;
		}
	}

	public string ParsedString
	{
		get
		{
			return this.m_parsedString;
		}
	}

	public void Setup(string url, HtmlParser.SyncType loadSyncType, HtmlParser.SyncType parseSyncType)
	{
		this.m_loadSyncType = loadSyncType;
		this.m_parseSyncType = parseSyncType;
		this.m_fsm = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		if (this.m_fsm == null)
		{
			return;
		}
		TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
		description.ignoreDeltaTime = true;
		this.m_isEndParse = false;
		HtmlParser.SyncType loadSyncType2 = this.m_loadSyncType;
		if (loadSyncType2 != HtmlParser.SyncType.TYPE_SYNC)
		{
			if (loadSyncType2 == HtmlParser.SyncType.TYPE_ASYNC)
			{
				HtmlParser.SyncType parseSyncType2 = this.m_parseSyncType;
				if (parseSyncType2 != HtmlParser.SyncType.TYPE_SYNC)
				{
					if (parseSyncType2 == HtmlParser.SyncType.TYPE_ASYNC)
					{
						this.m_loader = base.gameObject.AddComponent<HtmlLoaderASync>();
						this.m_loader.Setup(url);
						description.initState = new TinyFsmState(new EventFunction(this.StateLoadHtml));
					}
				}
				else
				{
					this.m_loader = base.gameObject.AddComponent<HtmlLoaderASync>();
					this.m_loader.Setup(url);
					description.initState = new TinyFsmState(new EventFunction(this.StateLoadHtml));
				}
			}
		}
		else
		{
			HtmlParser.SyncType parseSyncType2 = this.m_parseSyncType;
			if (parseSyncType2 != HtmlParser.SyncType.TYPE_SYNC)
			{
				if (parseSyncType2 == HtmlParser.SyncType.TYPE_ASYNC)
				{
					this.m_loader = base.gameObject.AddComponent<HtmlLoaderSync>();
					this.m_loader.Setup(url);
					description.initState = new TinyFsmState(new EventFunction(this.StateParseHtml));
				}
			}
			else
			{
				this.m_loader = base.gameObject.AddComponent<HtmlLoaderSync>();
				this.m_loader.Setup(url);
				this.ParseSync(this.m_loader.GetUrlContentsText());
				description.initState = new TinyFsmState(new EventFunction(this.StateIdle));
			}
		}
		this.m_loader.Setup(url);
		this.m_fsm.SetUp(description);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private TinyFsmState StateLoadHtml(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			if (this.m_loader != null && this.m_loader.IsEndLoad)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateParseHtml)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateParseHtml(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			HtmlParser.SyncType parseSyncType = this.m_parseSyncType;
			if (parseSyncType != HtmlParser.SyncType.TYPE_SYNC)
			{
				if (parseSyncType == HtmlParser.SyncType.TYPE_ASYNC)
				{
					base.StartCoroutine(this.ParseASync(this.m_loader.GetUrlContentsText()));
				}
			}
			else
			{
				this.ParseSync(this.m_loader.GetUrlContentsText());
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_isEndParse)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateIdle(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void ParseSync(string htmlString)
	{
		this.BeginParse(htmlString);
		bool flag;
		do
		{
			flag = this.Parse();
		}
		while (!flag);
		this.EndParse();
	}

	private IEnumerator ParseASync(string htmlString)
	{
		HtmlParser._ParseASync_c__Iterator29 _ParseASync_c__Iterator = new HtmlParser._ParseASync_c__Iterator29();
		_ParseASync_c__Iterator.htmlString = htmlString;
		_ParseASync_c__Iterator.___htmlString = htmlString;
		_ParseASync_c__Iterator.__f__this = this;
		return _ParseASync_c__Iterator;
	}

	private void BeginParse(string htmlString)
	{
		if (htmlString == null)
		{
			return;
		}
		string text = "<body>";
		int num = htmlString.IndexOf(text);
		this.m_parsedString = htmlString.Remove(0, num + text.Length);
	}

	private bool Parse()
	{
		int num = this.m_parsedString.IndexOf("<");
		int num2 = this.m_parsedString.IndexOf(">");
		if (num < 0 || num2 < 0)
		{
			return true;
		}
		int count = num2 + 1 - num;
		this.m_parsedString = this.m_parsedString.Remove(num, count);
		return false;
	}

	private void EndParse()
	{
		this.m_isEndParse = true;
	}
}
