using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResourceSceneLoader : MonoBehaviour
{
	public class ResourceInfo
	{
		public bool m_isAssetBundle;

		public bool m_onlyDownload;

		public bool m_isloaded;

		public string m_scenename;

		public ResourceCategory m_category = ResourceCategory.UNKNOWN;

		public bool m_dontDestroyOnChangeScene;

		public string m_rootObjectName;

		public bool m_isAsycnLoad;

		public ResourceInfo()
		{
		}

		public ResourceInfo(ResourceCategory cate, string name, bool assetbundle, bool onlyDownload, bool dontdestroyOnScene, string rootObjectName = null, bool isAsyncLoad = false)
		{
			this.m_scenename = name;
			this.m_category = cate;
			this.m_dontDestroyOnChangeScene = (!onlyDownload && dontdestroyOnScene);
			this.m_isAssetBundle = assetbundle;
			this.m_onlyDownload = onlyDownload;
			this.m_rootObjectName = ((rootObjectName != null) ? rootObjectName : this.m_scenename);
			this.m_isAsycnLoad = isAsyncLoad;
			this.m_isloaded = false;
		}
	}

	private sealed class _LoadScene_c__IteratorC1 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal float _loadStartTime___0;

		internal List<ResourceSceneLoader.ResourceInfo>.Enumerator __s_979___1;

		internal ResourceSceneLoader.ResourceInfo _loadInfo___2;

		internal float _oldTime___3;

		internal AsyncOperation _operation___4;

		internal float _loadTime___5;

		internal float _loadEndTime___6;

		internal int _PC;

		internal object _current;

		internal ResourceSceneLoader __f__this;

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
				this.__f__this.m_loadEndCount = 0;
				this._loadStartTime___0 = Time.realtimeSinceStartup;
				this.__s_979___1 = this.__f__this.m_loadInfos.GetEnumerator();
				num = 4294967293u;
				break;
			case 1u:
			case 2u:
				break;
			default:
				return false;
			}
			try
			{
				switch (num)
				{
				case 1u:
					goto IL_157;
				case 2u:
					goto IL_157;
				}
				IL_1D6:
				while (this.__s_979___1.MoveNext())
				{
					this._loadInfo___2 = this.__s_979___1.Current;
					if (this._loadInfo___2.m_scenename != null)
					{
						this._oldTime___3 = Time.realtimeSinceStartup;
						if (this.__f__this.m_checkTime)
						{
							global::Debug.Log("start load scene " + this._loadInfo___2.m_scenename);
						}
						if (this._loadInfo___2.m_isAsycnLoad)
						{
							this._operation___4 = Application.LoadLevelAdditiveAsync(this._loadInfo___2.m_scenename);
							this._current = this.__f__this.StartCoroutine(this.__f__this.WaitLoard(this._operation___4));
							this._PC = 1;
							flag = true;
							return true;
						}
						Application.LoadLevelAdditive(this._loadInfo___2.m_scenename);
						this._current = this.__f__this.StartCoroutine(this.__f__this.WaitLoard());
						this._PC = 2;
						flag = true;
						return true;
					}
				}
				goto IL_200;
				IL_157:
				this.__f__this.RegisterResourceManager(this._loadInfo___2);
				this._loadInfo___2.m_isloaded = true;
				this.__f__this.m_loadEndCount++;
				if (this.__f__this.m_checkTime)
				{
					this._loadTime___5 = Time.realtimeSinceStartup;
					global::Debug.Log("LS:Load File " + this._loadInfo___2.m_scenename + " Time is " + (this._loadTime___5 - this._oldTime___3).ToString());
					goto IL_1D6;
				}
				goto IL_1D6;
			}
			finally
			{
				if (!flag)
				{
					((IDisposable)this.__s_979___1).Dispose();
				}
			}
			IL_200:
			if (this.__f__this.m_checkTime)
			{
				this._loadEndTime___6 = Time.realtimeSinceStartup;
				global::Debug.Log("LS:All Loading Time is " + (this._loadEndTime___6 - this._loadStartTime___0).ToString());
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
			case 2u:
				try
				{
				}
				finally
				{
					((IDisposable)this.__s_979___1).Dispose();
				}
				break;
			}
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _LoadSceneAssetBundle_c__IteratorC2 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal float _oldTime___0;

		internal ResourceSceneLoader.ResourceInfo loadInfo;

		internal AsyncOperation _operation___1;

		internal float _loadTime___2;

		internal AssetBundleResult result;

		internal int _PC;

		internal object _current;

		internal ResourceSceneLoader.ResourceInfo ___loadInfo;

		internal AssetBundleResult ___result;

		internal ResourceSceneLoader __f__this;

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
				this._oldTime___0 = Time.realtimeSinceStartup;
				if (this.__f__this.m_checkTime)
				{
					global::Debug.Log("start load scene " + this.loadInfo.m_scenename);
				}
				if (this.loadInfo.m_isAsycnLoad)
				{
					this._operation___1 = Application.LoadLevelAdditiveAsync(this.loadInfo.m_scenename);
					this._current = this.__f__this.StartCoroutine(this.__f__this.WaitLoard(this._operation___1));
					this._PC = 1;
				}
				else
				{
					Application.LoadLevelAdditive(this.loadInfo.m_scenename);
					this._current = this.__f__this.StartCoroutine(this.__f__this.WaitLoard());
					this._PC = 2;
				}
				return true;
			case 1u:
				break;
			case 2u:
				break;
			default:
				return false;
			}
			this.__f__this.RegisterResourceManager(this.loadInfo);
			this.loadInfo.m_isloaded = true;
			this.__f__this.m_loadEndCount++;
			if (this.__f__this.m_checkTime)
			{
				this._loadTime___2 = Time.realtimeSinceStartup;
				global::Debug.Log("LS:Load File " + this.loadInfo.m_scenename + " Time is " + (this._loadTime___2 - this._oldTime___0).ToString());
			}
			if (this.result != null && AssetBundleManager.Instance)
			{
				AssetBundleManager.Instance.RequestUnload(this.result.Path);
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

	private sealed class _WaitLoard_c__IteratorC3 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal AsyncOperation async;

		internal int _PC;

		internal object _current;

		internal AsyncOperation ___async;

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
			if (this.async.progress < 0.9f)
			{
				this._current = null;
				this._PC = 1;
				return true;
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

	private sealed class _WaitLoard_c__IteratorC4 : IDisposable, IEnumerator, IEnumerator<object>
	{
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
				this._current = null;
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

	private bool m_checkTime = true;

	private List<ResourceSceneLoader.ResourceInfo> m_loadInfos = new List<ResourceSceneLoader.ResourceInfo>();

	private List<ResourceSceneLoader.ResourceInfo> m_assetLoadInfos = new List<ResourceSceneLoader.ResourceInfo>();

	private bool m_isloaded;

	private bool m_pause;

	private int m_loadEndCount;

	public int LoadEndCount
	{
		get
		{
			return this.m_loadEndCount;
		}
		private set
		{
		}
	}

	public int RequestedLoadCount
	{
		get
		{
			return this.m_loadInfos.Count;
		}
		private set
		{
		}
	}

	public bool Loaded
	{
		get
		{
			return this.m_isloaded;
		}
	}

	private void Start()
	{
		this.m_checkTime = false;
		this.StartAssetBundleLoad();
		base.StartCoroutine(this.LoadScene());
	}

	private void Update()
	{
		foreach (ResourceSceneLoader.ResourceInfo current in this.m_loadInfos)
		{
			if (!current.m_isloaded)
			{
				this.m_isloaded = false;
				return;
			}
		}
		foreach (ResourceSceneLoader.ResourceInfo current2 in this.m_assetLoadInfos)
		{
			if (!current2.m_isloaded)
			{
				this.m_isloaded = false;
				return;
			}
		}
		this.m_isloaded = true;
	}

	public void Pause(bool value)
	{
		this.m_pause = value;
		base.enabled = !this.m_pause;
	}

	public bool AddLoad(string scenename, bool onAssetBundle, bool onlyDownload)
	{
		if (ResourceManager.Instance && ResourceManager.Instance.IsExistContainer(scenename))
		{
			return false;
		}
		ResourceSceneLoader.ResourceInfo resourceInfo = new ResourceSceneLoader.ResourceInfo();
		resourceInfo.m_scenename = scenename;
		resourceInfo.m_isAssetBundle = onAssetBundle;
		resourceInfo.m_isloaded = false;
		resourceInfo.m_category = ResourceCategory.UNKNOWN;
		resourceInfo.m_onlyDownload = onlyDownload;
		if (onAssetBundle && AssetBundleLoader.Instance != null)
		{
			if (onlyDownload && AssetBundleLoader.Instance.IsDownloaded(scenename))
			{
				return false;
			}
			bool flag = true;
			foreach (ResourceSceneLoader.ResourceInfo current in this.m_assetLoadInfos)
			{
				if (current.m_scenename == scenename)
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.m_assetLoadInfos.Add(resourceInfo);
			}
		}
		else
		{
			if (onlyDownload)
			{
				return false;
			}
			bool flag2 = true;
			foreach (ResourceSceneLoader.ResourceInfo current2 in this.m_assetLoadInfos)
			{
				if (current2.m_scenename == scenename)
				{
					flag2 = false;
				}
			}
			if (flag2)
			{
				this.m_loadInfos.Add(resourceInfo);
			}
		}
		return true;
	}

	public bool AddLoadAndResourceManager(string scenename, bool onAssetBundle, ResourceCategory category, bool dontDestroyOnChangeScene, bool onlyDownload, string rootObjectName)
	{
		if (ResourceManager.Instance && ResourceManager.Instance.IsExistContainer(scenename))
		{
			return false;
		}
		this.AddLoadAndResourceManager(new ResourceSceneLoader.ResourceInfo
		{
			m_scenename = scenename,
			m_isAssetBundle = onAssetBundle,
			m_isloaded = false,
			m_category = category,
			m_dontDestroyOnChangeScene = dontDestroyOnChangeScene,
			m_rootObjectName = ((rootObjectName == null) ? scenename : rootObjectName),
			m_onlyDownload = onlyDownload
		});
		return true;
	}

	public bool AddLoadAndResourceManager(ResourceSceneLoader.ResourceInfo info)
	{
		if (ResourceManager.Instance && ResourceManager.Instance.IsExistContainer(info.m_scenename))
		{
			return false;
		}
		bool isAssetBundle = info.m_isAssetBundle;
		bool onlyDownload = info.m_onlyDownload;
		if (isAssetBundle && AssetBundleLoader.Instance != null)
		{
			if (onlyDownload && AssetBundleLoader.Instance.IsDownloaded(info.m_scenename))
			{
				return false;
			}
			this.m_assetLoadInfos.Add(info);
		}
		else
		{
			if (onlyDownload)
			{
				return false;
			}
			this.m_loadInfos.Add(info);
		}
		return true;
	}

	private IEnumerator LoadScene()
	{
		ResourceSceneLoader._LoadScene_c__IteratorC1 _LoadScene_c__IteratorC = new ResourceSceneLoader._LoadScene_c__IteratorC1();
		_LoadScene_c__IteratorC.__f__this = this;
		return _LoadScene_c__IteratorC;
	}

	private IEnumerator LoadSceneAssetBundle(ResourceSceneLoader.ResourceInfo loadInfo, AssetBundleResult result)
	{
		ResourceSceneLoader._LoadSceneAssetBundle_c__IteratorC2 _LoadSceneAssetBundle_c__IteratorC = new ResourceSceneLoader._LoadSceneAssetBundle_c__IteratorC2();
		_LoadSceneAssetBundle_c__IteratorC.loadInfo = loadInfo;
		_LoadSceneAssetBundle_c__IteratorC.result = result;
		_LoadSceneAssetBundle_c__IteratorC.___loadInfo = loadInfo;
		_LoadSceneAssetBundle_c__IteratorC.___result = result;
		_LoadSceneAssetBundle_c__IteratorC.__f__this = this;
		return _LoadSceneAssetBundle_c__IteratorC;
	}

	private void StartAssetBundleLoad()
	{
		foreach (ResourceSceneLoader.ResourceInfo current in this.m_assetLoadInfos)
		{
			AssetBundleLoader.Instance.RequestLoadScene(current.m_scenename, true, base.gameObject);
		}
	}

	private void RegisterResourceManager(ResourceSceneLoader.ResourceInfo loadInfo)
	{
		if (loadInfo.m_category != ResourceCategory.UNKNOWN && ResourceManager.Instance != null)
		{
			GameObject gameObject = GameObject.Find(loadInfo.m_rootObjectName);
			if (gameObject != null)
			{
				ResourceManager.Instance.AddCategorySceneObjects(loadInfo.m_category, loadInfo.m_scenename, gameObject, loadInfo.m_dontDestroyOnChangeScene);
			}
		}
	}

	private IEnumerator WaitLoard(AsyncOperation async)
	{
		ResourceSceneLoader._WaitLoard_c__IteratorC3 _WaitLoard_c__IteratorC = new ResourceSceneLoader._WaitLoard_c__IteratorC3();
		_WaitLoard_c__IteratorC.async = async;
		_WaitLoard_c__IteratorC.___async = async;
		return _WaitLoard_c__IteratorC;
	}

	private IEnumerator WaitLoard()
	{
		return new ResourceSceneLoader._WaitLoard_c__IteratorC4();
	}

	private void AssetBundleResponseSucceed(MsgAssetBundleResponseSucceed msg)
	{
		string fileName = msg.m_request.FileName;
		AssetBundleResult result = msg.m_result;
		foreach (ResourceSceneLoader.ResourceInfo current in this.m_assetLoadInfos)
		{
			if (current.m_scenename.Equals(fileName))
			{
				if (current.m_onlyDownload)
				{
					current.m_isloaded = true;
					this.m_loadEndCount++;
					if (result != null && AssetBundleManager.Instance)
					{
						AssetBundleManager.Instance.RequestUnload(result.Path);
					}
				}
				else
				{
					base.StartCoroutine(this.LoadSceneAssetBundle(current, result));
				}
				break;
			}
		}
	}
}
