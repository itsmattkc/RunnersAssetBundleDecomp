using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ButtonEventResourceLoader : MonoBehaviour
{
	public delegate void CallbackIfNotLoaded();

	private sealed class _LoadPageResourceIfNotLoadedSync_c__Iterator3B : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal ButtonInfoTable.PageType pageType;

		internal ButtonEventResourceLoader.CallbackIfNotLoaded callbackIfNotLoaded;

		internal int _PC;

		internal object _current;

		internal ButtonInfoTable.PageType ___pageType;

		internal ButtonEventResourceLoader.CallbackIfNotLoaded ___callbackIfNotLoaded;

		internal ButtonEventResourceLoader __f__this;

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
				this._current = this.__f__this.StartCoroutine(this.__f__this.LoadPageResourceIfNotLoadedCoroutine(this.pageType, this.callbackIfNotLoaded));
				this._PC = 1;
				return true;
			case 1u:
				this._PC = -1;
				break;
			}
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

	private sealed class _LoadResourceIfNotLoadedSync_c__Iterator3C : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal string resourceName;

		internal ButtonEventResourceLoader.CallbackIfNotLoaded callbackIfNotLoaded;

		internal int _PC;

		internal object _current;

		internal string ___resourceName;

		internal ButtonEventResourceLoader.CallbackIfNotLoaded ___callbackIfNotLoaded;

		internal ButtonEventResourceLoader __f__this;

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
				this._current = this.__f__this.StartCoroutine(this.__f__this.LoadPageResourceIfNotLoadedCoroutine(this.resourceName, this.callbackIfNotLoaded));
				this._PC = 1;
				return true;
			case 1u:
				this._PC = -1;
				break;
			}
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

	private sealed class _LoadAtlasResourceIfNotLoaded_c__Iterator3D : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal AtlasManager _atlasManager___0;

		internal int _PC;

		internal object _current;

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
				this._atlasManager___0 = AtlasManager.Instance;
				if (!(this._atlasManager___0 != null))
				{
					goto IL_75;
				}
				this._atlasManager___0.StartLoadAtlasForDividedMenu();
				break;
			case 1u:
				if (this._atlasManager___0.IsLoadAtlas())
				{
					goto IL_75;
				}
				break;
			default:
				return false;
			}
			this._current = null;
			this._PC = 1;
			return true;
			IL_75:
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

	private sealed class _LoadPageResourceIfNotLoadedCoroutine_c__Iterator3E : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal ButtonInfoTable.PageType pageType;

		internal GameObject _mainMenuPagesObject___0;

		internal bool _isNeedCallback___1;

		internal List<string> _resourceNameList___2;

		internal List<string>.Enumerator __s_570___3;

		internal string _resourceName___4;

		internal bool _isExistResource___5;

		internal ButtonEventResourceLoader.CallbackIfNotLoaded callbackIfNotLoaded;

		internal int _PC;

		internal object _current;

		internal ButtonInfoTable.PageType ___pageType;

		internal ButtonEventResourceLoader.CallbackIfNotLoaded ___callbackIfNotLoaded;

		internal ButtonEventResourceLoader __f__this;

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
			bool flag = false;
			switch (num)
			{
			case 0u:
				this.__f__this.m_isLoaded = false;
				if (this.pageType == ButtonInfoTable.PageType.NON)
				{
					this.__f__this.m_isLoaded = true;
					return false;
				}
				this._mainMenuPagesObject___0 = GameObject.Find("UI Root (2D)/Camera/menu_Anim");
				if (this._mainMenuPagesObject___0 == null)
				{
					this.__f__this.m_isLoaded = true;
					return false;
				}
				this._isNeedCallback___1 = false;
				if (!this.__f__this.m_resourceMap.ContainsKey(this.pageType))
				{
					return false;
				}
				this.__f__this.m_resourceMap.TryGetValue(this.pageType, out this._resourceNameList___2);
				this.__s_570___3 = this._resourceNameList___2.GetEnumerator();
				num = 4294967293u;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			try
			{
				switch (num)
				{
				case 1u:
					this._isNeedCallback___1 = true;
					break;
				}
				while (this.__s_570___3.MoveNext())
				{
					this._resourceName___4 = this.__s_570___3.Current;
					this._isExistResource___5 = this.__f__this.IsExistResource(this._resourceName___4, this._mainMenuPagesObject___0);
					if (!this._isExistResource___5)
					{
						this._current = this.__f__this.StartCoroutine(this.__f__this.LoadResourceRequest(this._resourceName___4, this._mainMenuPagesObject___0));
						this._PC = 1;
						flag = true;
						return true;
					}
				}
			}
			finally
			{
				if (!flag)
				{
					((IDisposable)this.__s_570___3).Dispose();
				}
			}
			this.__f__this.m_isLoaded = true;
			if (this._isNeedCallback___1 && this.callbackIfNotLoaded != null)
			{
				this.callbackIfNotLoaded();
			}
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 1u:
				try
				{
				}
				finally
				{
					((IDisposable)this.__s_570___3).Dispose();
				}
				break;
			}
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _LoadPageResourceIfNotLoadedCoroutine_c__Iterator3F : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal GameObject _mainMenuPagesObject___0;

		internal bool _isNeedCallback___1;

		internal string resourceName;

		internal bool _isExistResource___2;

		internal ButtonEventResourceLoader.CallbackIfNotLoaded callbackIfNotLoaded;

		internal int _PC;

		internal object _current;

		internal string ___resourceName;

		internal ButtonEventResourceLoader.CallbackIfNotLoaded ___callbackIfNotLoaded;

		internal ButtonEventResourceLoader __f__this;

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
				this.__f__this.m_isLoaded = false;
				this._mainMenuPagesObject___0 = GameObject.Find("UI Root (2D)/Camera/menu_Anim");
				if (this._mainMenuPagesObject___0 == null)
				{
					this.__f__this.m_isLoaded = true;
				}
				else
				{
					this._isNeedCallback___1 = false;
					this._isExistResource___2 = this.__f__this.IsExistResource(this.resourceName, this._mainMenuPagesObject___0);
					if (!this._isExistResource___2)
					{
						this._current = this.__f__this.StartCoroutine(this.__f__this.LoadResourceRequest(this.resourceName, this._mainMenuPagesObject___0));
						this._PC = 1;
						return true;
					}
					this.__f__this.m_isLoaded = true;
				}
				break;
			case 1u:
				this._isNeedCallback___1 = true;
				this.__f__this.m_isLoaded = true;
				if (this._isNeedCallback___1 && this.callbackIfNotLoaded != null)
				{
					this.callbackIfNotLoaded();
				}
				this._PC = -1;
				break;
			}
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

	private sealed class _LoadResourceRequest_c__Iterator40 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal HudNetworkConnect _hudConnect___0;

		internal GameObject _sceneLoaderObject___1;

		internal string resourceName;

		internal GameObject _resourceObject___2;

		internal ResourceFolderMarker _marker___3;

		internal IEnumerator __s_571___4;

		internal Transform _child___5;

		internal Vector3 _localPosition___6;

		internal Vector3 _localScale___7;

		internal GameObject attachObject;

		internal int _PC;

		internal object _current;

		internal string ___resourceName;

		internal GameObject ___attachObject;

		internal ButtonEventResourceLoader __f__this;

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
				this._hudConnect___0 = NetMonitor.Instance.GetComponent<HudNetworkConnect>();
				if (this._hudConnect___0 != null)
				{
					this._hudConnect___0.Setup();
					this._hudConnect___0.PlayStart(HudNetworkConnect.DisplayType.LOADING);
				}
				this._sceneLoaderObject___1 = GameObject.Find("ButtonEventSceneLoader");
				if (this._sceneLoaderObject___1 == null)
				{
					this._sceneLoaderObject___1 = new GameObject("ButtonEventSceneLoader");
				}
				this.__f__this.m_sceneLoader = this._sceneLoaderObject___1.AddComponent<ResourceSceneLoader>();
				this.__f__this.m_sceneLoader.AddLoadAndResourceManager(this.resourceName, true, ResourceCategory.UI, false, false, null);
				break;
			case 1u:
				if (this.__f__this.m_sceneLoader.Loaded)
				{
					UnityEngine.Object.Destroy(this.__f__this.m_sceneLoader);
					this.__f__this.m_sceneLoader = null;
					this._current = null;
					this._PC = 2;
					return true;
				}
				break;
			case 2u:
				this._resourceObject___2 = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, this.resourceName);
				if (this._resourceObject___2 != null)
				{
					this._marker___3 = this._resourceObject___2.GetComponent<ResourceFolderMarker>();
					if (this._marker___3 == null)
					{
						this._resourceObject___2.SetActive(false);
						this.__s_571___4 = this._resourceObject___2.transform.GetEnumerator();
						try
						{
							while (this.__s_571___4.MoveNext())
							{
								this._child___5 = (Transform)this.__s_571___4.Current;
								this._child___5.gameObject.SetActive(true);
							}
						}
						finally
						{
							IDisposable disposable = this.__s_571___4 as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					this._localPosition___6 = this._resourceObject___2.transform.localPosition;
					this._localScale___7 = this._resourceObject___2.transform.localScale;
					this._resourceObject___2.transform.parent = this.attachObject.transform;
					this._resourceObject___2.transform.localPosition = this._localPosition___6;
					this._resourceObject___2.transform.localScale = this._localScale___7;
				}
				if (this._hudConnect___0 != null)
				{
					this._hudConnect___0.PlayEnd();
				}
				this._current = null;
				this._PC = 3;
				return true;
			case 3u:
				this._PC = -1;
				return false;
			default:
				return false;
			}
			this._current = null;
			this._PC = 1;
			return true;
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

	private ResourceSceneLoader m_sceneLoader;

	private bool m_isLoaded;

	private Dictionary<ButtonInfoTable.PageType, List<string>> m_resourceMap = new Dictionary<ButtonInfoTable.PageType, List<string>>
	{
		{
			ButtonInfoTable.PageType.PRESENT_BOX,
			new List<string>
			{
				"item_get_Window",
				"PlayerSetWindowUI",
				"ChaoSetWindowUI",
				"PresentBoxUI",
				"ChaoWindows"
			}
		},
		{
			ButtonInfoTable.PageType.ROULETTE,
			new List<string>
			{
				"item_get_Window",
				"PlayerSetWindowUI",
				"ChaoSetWindowUI",
				"RouletteTopUI",
				"ChaoWindows",
				"NewsWindow"
			}
		},
		{
			ButtonInfoTable.PageType.DAILY_BATTLE,
			new List<string>
			{
				"PlayerSetWindowUI",
				"ChaoSetWindowUI",
				"RankingWindowUI",
				"DailyBattleDetailWindow",
				"DailyInfoUI"
			}
		},
		{
			ButtonInfoTable.PageType.CHAO,
			new List<string>
			{
				"PlayerSetWindowUI",
				"ChaoSetWindowUI",
				"DeckViewWindow",
				"ChaoSetUIPage"
			}
		},
		{
			ButtonInfoTable.PageType.OPTION,
			new List<string>
			{
				"OptionUI",
				"window_name_setting",
				"OptionWindows"
			}
		},
		{
			ButtonInfoTable.PageType.SHOP_RSR,
			new List<string>
			{
				"item_get_Window",
				"ShopPage"
			}
		},
		{
			ButtonInfoTable.PageType.SHOP_RING,
			new List<string>
			{
				"item_get_Window",
				"ShopPage"
			}
		},
		{
			ButtonInfoTable.PageType.SHOP_ENERGY,
			new List<string>
			{
				"item_get_Window",
				"ShopPage"
			}
		},
		{
			ButtonInfoTable.PageType.INFOMATION,
			new List<string>
			{
				"NewsWindow",
				"WorldRankingWindowUI",
				"LeagueResultWindowUI",
				"InformationUI"
			}
		},
		{
			ButtonInfoTable.PageType.EPISODE_RANKING,
			new List<string>
			{
				"PlayerSetWindowUI",
				"ChaoSetWindowUI",
				"RankingFriendOptionWindow",
				"RankingResultBitWindow",
				"RankingWindowUI",
				"ui_mm_ranking_page"
			}
		},
		{
			ButtonInfoTable.PageType.QUICK_RANKING,
			new List<string>
			{
				"PlayerSetWindowUI",
				"ChaoSetWindowUI",
				"RankingFriendOptionWindow",
				"RankingResultBitWindow",
				"RankingWindowUI",
				"ui_mm_ranking_page"
			}
		},
		{
			ButtonInfoTable.PageType.QUICK,
			new List<string>
			{
				"PlayerSetWindowUI",
				"ChaoSetWindowUI",
				"DeckViewWindow",
				"ItemSet_3_UI"
			}
		},
		{
			ButtonInfoTable.PageType.EPISODE,
			new List<string>
			{
				"item_get_Window",
				"Mileage_rankup",
				"ui_mm_mileage2_page"
			}
		},
		{
			ButtonInfoTable.PageType.EPISODE_PLAY,
			new List<string>
			{
				"PlayerSetWindowUI",
				"ChaoSetWindowUI",
				"DeckViewWindow",
				"ItemSet_3_UI"
			}
		},
		{
			ButtonInfoTable.PageType.PLAY_AT_EPISODE_PAGE,
			new List<string>
			{
				"PlayerSetWindowUI",
				"ChaoSetWindowUI",
				"DeckViewWindow",
				"ItemSet_3_UI"
			}
		},
		{
			ButtonInfoTable.PageType.PLAYER_MAIN,
			new List<string>
			{
				"PlayerSetWindowUI",
				"ChaoSetWindowUI",
				"DeckViewWindow",
				"PlayerSet_3_UI"
			}
		},
		{
			ButtonInfoTable.PageType.DAILY_CHALLENGE,
			new List<string>
			{
				"DailyWindowUI"
			}
		}
	};

	public bool IsLoaded
	{
		get
		{
			return this.m_isLoaded;
		}
		private set
		{
		}
	}

	public Dictionary<ButtonInfoTable.PageType, List<string>> ResourceMap
	{
		get
		{
			return this.m_resourceMap;
		}
		private set
		{
		}
	}

	public IEnumerator LoadPageResourceIfNotLoadedSync(ButtonInfoTable.PageType pageType, ButtonEventResourceLoader.CallbackIfNotLoaded callbackIfNotLoaded)
	{
		ButtonEventResourceLoader._LoadPageResourceIfNotLoadedSync_c__Iterator3B _LoadPageResourceIfNotLoadedSync_c__Iterator3B = new ButtonEventResourceLoader._LoadPageResourceIfNotLoadedSync_c__Iterator3B();
		_LoadPageResourceIfNotLoadedSync_c__Iterator3B.pageType = pageType;
		_LoadPageResourceIfNotLoadedSync_c__Iterator3B.callbackIfNotLoaded = callbackIfNotLoaded;
		_LoadPageResourceIfNotLoadedSync_c__Iterator3B.___pageType = pageType;
		_LoadPageResourceIfNotLoadedSync_c__Iterator3B.___callbackIfNotLoaded = callbackIfNotLoaded;
		_LoadPageResourceIfNotLoadedSync_c__Iterator3B.__f__this = this;
		return _LoadPageResourceIfNotLoadedSync_c__Iterator3B;
	}

	public void LoadResourceIfNotLoadedAsync(ButtonInfoTable.PageType pageType, ButtonEventResourceLoader.CallbackIfNotLoaded callbackIfNotLoaded)
	{
		base.StartCoroutine(this.LoadPageResourceIfNotLoadedCoroutine(pageType, callbackIfNotLoaded));
	}

	public IEnumerator LoadResourceIfNotLoadedSync(string resourceName, ButtonEventResourceLoader.CallbackIfNotLoaded callbackIfNotLoaded)
	{
		ButtonEventResourceLoader._LoadResourceIfNotLoadedSync_c__Iterator3C _LoadResourceIfNotLoadedSync_c__Iterator3C = new ButtonEventResourceLoader._LoadResourceIfNotLoadedSync_c__Iterator3C();
		_LoadResourceIfNotLoadedSync_c__Iterator3C.resourceName = resourceName;
		_LoadResourceIfNotLoadedSync_c__Iterator3C.callbackIfNotLoaded = callbackIfNotLoaded;
		_LoadResourceIfNotLoadedSync_c__Iterator3C.___resourceName = resourceName;
		_LoadResourceIfNotLoadedSync_c__Iterator3C.___callbackIfNotLoaded = callbackIfNotLoaded;
		_LoadResourceIfNotLoadedSync_c__Iterator3C.__f__this = this;
		return _LoadResourceIfNotLoadedSync_c__Iterator3C;
	}

	public void LoadResourceIfNotLoadedAsync(string resourceName, ButtonEventResourceLoader.CallbackIfNotLoaded callbackIfNotLoaded)
	{
		base.StartCoroutine(this.LoadPageResourceIfNotLoadedCoroutine(resourceName, callbackIfNotLoaded));
	}

	public IEnumerator LoadAtlasResourceIfNotLoaded()
	{
		return new ButtonEventResourceLoader._LoadAtlasResourceIfNotLoaded_c__Iterator3D();
	}

	private IEnumerator LoadPageResourceIfNotLoadedCoroutine(ButtonInfoTable.PageType pageType, ButtonEventResourceLoader.CallbackIfNotLoaded callbackIfNotLoaded)
	{
		ButtonEventResourceLoader._LoadPageResourceIfNotLoadedCoroutine_c__Iterator3E _LoadPageResourceIfNotLoadedCoroutine_c__Iterator3E = new ButtonEventResourceLoader._LoadPageResourceIfNotLoadedCoroutine_c__Iterator3E();
		_LoadPageResourceIfNotLoadedCoroutine_c__Iterator3E.pageType = pageType;
		_LoadPageResourceIfNotLoadedCoroutine_c__Iterator3E.callbackIfNotLoaded = callbackIfNotLoaded;
		_LoadPageResourceIfNotLoadedCoroutine_c__Iterator3E.___pageType = pageType;
		_LoadPageResourceIfNotLoadedCoroutine_c__Iterator3E.___callbackIfNotLoaded = callbackIfNotLoaded;
		_LoadPageResourceIfNotLoadedCoroutine_c__Iterator3E.__f__this = this;
		return _LoadPageResourceIfNotLoadedCoroutine_c__Iterator3E;
	}

	private IEnumerator LoadPageResourceIfNotLoadedCoroutine(string resourceName, ButtonEventResourceLoader.CallbackIfNotLoaded callbackIfNotLoaded)
	{
		ButtonEventResourceLoader._LoadPageResourceIfNotLoadedCoroutine_c__Iterator3F _LoadPageResourceIfNotLoadedCoroutine_c__Iterator3F = new ButtonEventResourceLoader._LoadPageResourceIfNotLoadedCoroutine_c__Iterator3F();
		_LoadPageResourceIfNotLoadedCoroutine_c__Iterator3F.resourceName = resourceName;
		_LoadPageResourceIfNotLoadedCoroutine_c__Iterator3F.callbackIfNotLoaded = callbackIfNotLoaded;
		_LoadPageResourceIfNotLoadedCoroutine_c__Iterator3F.___resourceName = resourceName;
		_LoadPageResourceIfNotLoadedCoroutine_c__Iterator3F.___callbackIfNotLoaded = callbackIfNotLoaded;
		_LoadPageResourceIfNotLoadedCoroutine_c__Iterator3F.__f__this = this;
		return _LoadPageResourceIfNotLoadedCoroutine_c__Iterator3F;
	}

	private bool IsExistResource(string resourceName, GameObject parentObject)
	{
		int childCount = parentObject.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = parentObject.transform.GetChild(i).gameObject;
			if (!(gameObject == null))
			{
				if (gameObject.name == resourceName)
				{
					return true;
				}
			}
		}
		return false;
	}

	public IEnumerator LoadResourceRequest(string resourceName, GameObject attachObject)
	{
		ButtonEventResourceLoader._LoadResourceRequest_c__Iterator40 _LoadResourceRequest_c__Iterator = new ButtonEventResourceLoader._LoadResourceRequest_c__Iterator40();
		_LoadResourceRequest_c__Iterator.resourceName = resourceName;
		_LoadResourceRequest_c__Iterator.attachObject = attachObject;
		_LoadResourceRequest_c__Iterator.___resourceName = resourceName;
		_LoadResourceRequest_c__Iterator.___attachObject = attachObject;
		_LoadResourceRequest_c__Iterator.__f__this = this;
		return _LoadResourceRequest_c__Iterator;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
