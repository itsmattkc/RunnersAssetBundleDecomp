using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class HudLoadingCharaInfo : MonoBehaviour
{
	private struct CharaInfo
	{
		public bool isReady;

		public string type;

		public string name;

		public Texture2D picture;

		public string explain;

		public string explainCaption;
	}

	private sealed class _LoadWWW_c__Iterator2A : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal string _url___0;

		internal string _serverUrl___1;

		internal GameObject _gameObjectParser___2;

		internal HtmlParser _parser___3;

		internal string _result___4;

		internal string[] _contents___5;

		internal int _index___6;

		internal int _paramCountMax___7;

		internal bool _typeKeyFound___8;

		internal int _index___9;

		internal int _contentsLength___10;

		internal int _index___11;

		internal HudLoadingCharaInfo.CharaInfo _charaInfo___12;

		internal int _contentsIndex___13;

		internal string _pictureUrl___14;

		internal bool _loadingFlg___15;

		internal WWW _wwwPicture___16;

		internal int _PC;

		internal object _current;

		internal HudLoadingCharaInfo __f__this;

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
				this.__f__this.m_loopCount = 0;
				if (TitleUtil.initUser)
				{
					this.__f__this.m_currentCharaIndex = 0;
				}
				else
				{
					this.__f__this.m_currentCharaIndex = 1;
				}
				this._url___0 = string.Empty;
				this._serverUrl___1 = NetBaseUtil.InformationServerURL;
				this._url___0 = this._serverUrl___1 + "title_load/title_load_index_" + TextUtility.GetSuffixe() + ".html";
				global::Debug.Log("HudLoadingCharaInfo LoadWWW url:" + this._url___0 + " !!!!");
				this._gameObjectParser___2 = HtmlParserFactory.Create(this._url___0, HtmlParser.SyncType.TYPE_ASYNC, HtmlParser.SyncType.TYPE_ASYNC);
				if (!(this._gameObjectParser___2 != null))
				{
					goto IL_546;
				}
				this._parser___3 = this._gameObjectParser___2.GetComponent<HtmlParser>();
				if (!(this._parser___3 != null))
				{
					goto IL_546;
				}
				break;
			case 1u:
				break;
			case 2u:
				this._charaInfo___12.picture = ((!(this._wwwPicture___16.texture != null)) ? null : this._wwwPicture___16.texture);
				this.__f__this.m_pictureStack.Add(this._pictureUrl___14, this._wwwPicture___16.texture);
				this._wwwPicture___16.Dispose();
				goto IL_496;
			case 3u:
				this._charaInfo___12.picture = this.__f__this.m_pictureStack[this._pictureUrl___14];
				goto IL_496;
			default:
				return false;
			}
			if (this._parser___3.IsEndParse)
			{
				this._result___4 = this._parser___3.ParsedString;
				this._contents___5 = this._result___4.Split(new char[]
				{
					']'
				});
				this._index___6 = 0;
				while (this._index___6 < this._contents___5.Length)
				{
					this._contents___5[this._index___6] = this._contents___5[this._index___6].Remove(0, 2);
					this._index___6++;
				}
				this._paramCountMax___7 = 5;
				if (this._contents___5.Length < 5)
				{
					this._paramCountMax___7 = 4;
				}
				else
				{
					this._typeKeyFound___8 = false;
					this._index___9 = 2;
					while (this._index___9 < 5)
					{
						if (this._contents___5[this._index___9] == "howtoplay" || this._contents___5[this._index___9] == "Howtoplay")
						{
							this._typeKeyFound___8 = true;
						}
						else if (this._contents___5[this._index___9] == "player" || this._contents___5[this._index___9] == "Player")
						{
							this._typeKeyFound___8 = true;
						}
						else if (this._contents___5[this._index___9] == "chara" || this._contents___5[this._index___9] == "Chara")
						{
							this._typeKeyFound___8 = true;
						}
						else if (this._contents___5[this._index___9] == "object" || this._contents___5[this._index___9] == "Object")
						{
							this._typeKeyFound___8 = true;
						}
						this._index___9++;
					}
					if (!this._typeKeyFound___8)
					{
						this._paramCountMax___7 = 4;
					}
				}
				this._contentsLength___10 = this._contents___5.Length / this._paramCountMax___7;
				this._index___11 = 0;
				goto IL_535;
			}
			this._current = null;
			this._PC = 1;
			return true;
			IL_496:
			this._charaInfo___12.explain = this._contents___5[this._contentsIndex___13 + 2];
			this._charaInfo___12.explainCaption = this._contents___5[this._contentsIndex___13 + 3];
			if (this._paramCountMax___7 == 5)
			{
				this._charaInfo___12.type = this._contents___5[this._contentsIndex___13 + 4];
			}
			else
			{
				this._charaInfo___12.type = "player";
			}
			this._charaInfo___12.isReady = true;
			this.__f__this.m_charaInfoList.Add(this._charaInfo___12);
			this._index___11++;
			IL_535:
			if (this._index___11 < this._contentsLength___10)
			{
				this._charaInfo___12 = default(HudLoadingCharaInfo.CharaInfo);
				this._charaInfo___12.isReady = false;
				this._contentsIndex___13 = this._paramCountMax___7 * this._index___11;
				this._charaInfo___12.name = this._contents___5[this._contentsIndex___13];
				this._pictureUrl___14 = this._serverUrl___1 + "pictures/title/" + this._contents___5[this._contentsIndex___13 + 1];
				this._loadingFlg___15 = true;
				if (this.__f__this.m_pictureStack.Count > 0 && this.__f__this.m_pictureStack.ContainsKey(this._pictureUrl___14))
				{
					this._loadingFlg___15 = false;
				}
				if (this._loadingFlg___15)
				{
					this._wwwPicture___16 = new WWW(this._pictureUrl___14);
					this._current = this._wwwPicture___16;
					this._PC = 2;
					return true;
				}
				this._current = null;
				this._PC = 3;
				return true;
			}
			IL_546:
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

	private const int WebParamCount = 5;

	private List<HudLoadingCharaInfo.CharaInfo> m_charaInfoList;

	private Dictionary<string, Texture2D> m_pictureStack;

	private int m_currentCharaIndex;

	private int m_loopCount;

	private bool m_isStartCoroutine;

	private bool m_isErrorRestartCoroutine;

	private void Awake()
	{
		this.m_charaInfoList = new List<HudLoadingCharaInfo.CharaInfo>();
		this.m_pictureStack = new Dictionary<string, Texture2D>();
		this.m_loopCount = 0;
		this.m_currentCharaIndex = 0;
		this.m_isStartCoroutine = false;
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		this.m_charaInfoList.Clear();
		this.m_pictureStack.Clear();
		this.m_loopCount = 0;
		this.m_currentCharaIndex = 0;
		this.m_isStartCoroutine = false;
	}

	private void Update()
	{
		if (!this.m_isStartCoroutine)
		{
			base.StartCoroutine(this.LoadWWW());
			this.m_isStartCoroutine = true;
			this.m_isErrorRestartCoroutine = false;
		}
		else if (NetworkErrorWindow.IsCreated("NetworkErrorReload") || NetworkErrorWindow.IsCreated("NetworkErrorRetry"))
		{
			this.m_isErrorRestartCoroutine = true;
		}
		else
		{
			if (this.m_isErrorRestartCoroutine)
			{
				this.m_isStartCoroutine = false;
			}
			this.m_isErrorRestartCoroutine = false;
		}
	}

	private IEnumerator LoadWWW()
	{
		HudLoadingCharaInfo._LoadWWW_c__Iterator2A _LoadWWW_c__Iterator2A = new HudLoadingCharaInfo._LoadWWW_c__Iterator2A();
		_LoadWWW_c__Iterator2A.__f__this = this;
		return _LoadWWW_c__Iterator2A;
	}

	public int GetCharaCount()
	{
		return this.m_charaInfoList.Count;
	}

	public bool IsReady()
	{
		return this.m_currentCharaIndex < this.m_charaInfoList.Count && this.m_charaInfoList[this.m_currentCharaIndex].isReady;
	}

	public void GoNext()
	{
		this.m_currentCharaIndex++;
		if (this.m_currentCharaIndex >= this.m_charaInfoList.Count && this.m_charaInfoList.Count > 0)
		{
			this.m_currentCharaIndex = 0;
			this.m_loopCount++;
		}
		if (this.m_loopCount > 0)
		{
			for (int i = 0; i < this.m_charaInfoList.Count; i++)
			{
				HudLoadingCharaInfo.CharaInfo charaInfo = this.m_charaInfoList[(this.m_currentCharaIndex + i) % this.m_charaInfoList.Count];
				if (charaInfo.type != "howtoplay" && charaInfo.type != "Howtoplay")
				{
					this.m_currentCharaIndex = (this.m_currentCharaIndex + i) % this.m_charaInfoList.Count;
					break;
				}
			}
		}
	}

	public string GetCharaName()
	{
		if (this.m_currentCharaIndex >= this.m_charaInfoList.Count)
		{
			return string.Empty;
		}
		return this.m_charaInfoList[this.m_currentCharaIndex].name;
	}

	public string GetTypeName()
	{
		if (this.m_currentCharaIndex >= this.m_charaInfoList.Count)
		{
			return string.Empty;
		}
		return this.m_charaInfoList[this.m_currentCharaIndex].type;
	}

	public Texture2D GetCharaPicture()
	{
		if (this.m_currentCharaIndex >= this.m_charaInfoList.Count)
		{
			return null;
		}
		return this.m_charaInfoList[this.m_currentCharaIndex].picture;
	}

	public string GetCharaExplain()
	{
		if (this.m_currentCharaIndex >= this.m_charaInfoList.Count)
		{
			return string.Empty;
		}
		return this.m_charaInfoList[this.m_currentCharaIndex].explain;
	}

	public string GetCharaExplainCaption()
	{
		if (this.m_currentCharaIndex >= this.m_charaInfoList.Count)
		{
			return string.Empty;
		}
		return this.m_charaInfoList[this.m_currentCharaIndex].explainCaption;
	}
}
