using App;
using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AssetBundleLoader : MonoBehaviour
{
	public class CachedFileInfo
	{
		public string name;

		public int version;

		public uint crc;

		public bool downloaded;
	}

	public class LoadingDataInfo
	{
		public string name;

		public string filePath;

		public string fullPath;

		public bool cashed;

		public AssetBundleLoader.CachedFileInfo cashedInfo;

		public GameObject returnObject;
	}

	private enum Mode
	{
		Non,
		DownloadList,
		EnableLoad
	}

	private sealed class _WaitCachingReady_c__IteratorBC : IDisposable, IEnumerator, IEnumerator<object>
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
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (!Caching.ready)
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

	private Dictionary<string, AssetBundleLoader.CachedFileInfo> m_bundleInfoList;

	private Dictionary<string, AssetBundleLoader.LoadingDataInfo> m_loadingDataList = new Dictionary<string, AssetBundleLoader.LoadingDataInfo>();

	private List<global::AssetBundleRequest> m_retryRequest = new List<global::AssetBundleRequest>();

	private AssetBundleLoader.Mode m_mode;

	private bool m_isConnecting;

	private float m_connectUIDisplayTime;

	private static AssetBundleLoader instance;

	public static AssetBundleLoader Instance
	{
		get
		{
			return AssetBundleLoader.instance;
		}
	}

	public string[] GetChaoTextureList()
	{
		Dictionary<string, AssetBundleLoader.CachedFileInfo>.KeyCollection keys = this.m_bundleInfoList.Keys;
		List<string> list = new List<string>();
		foreach (string current in keys)
		{
			if (current.Contains("ui_tex_chao_"))
			{
				list.Add(current);
			}
			else if (current.Contains("ui_tex_player_"))
			{
				list.Add(current);
			}
			else if (current.Contains("ui_tex_mile_w"))
			{
				list.Add(current);
			}
		}
		return list.ToArray();
	}

	private void Start()
	{
		if (AssetBundleManager.Instance == null)
		{
			AssetBundleManager.Create();
		}
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Update()
	{
		AssetBundleManager assetBundleManager = AssetBundleManager.Instance;
		if (assetBundleManager != null)
		{
			bool flag = assetBundleManager.Executing || assetBundleManager.RequestCount > 0;
			bool flag2 = this.m_retryRequest != null && this.m_retryRequest.Count > 0;
			if (!flag && !flag2)
			{
				AssetBundleLoader.LoadingDataInfo loadingDataInfo = null;
				foreach (KeyValuePair<string, AssetBundleLoader.LoadingDataInfo> current in this.m_loadingDataList)
				{
					loadingDataInfo = current.Value;
				}
				if (loadingDataInfo != null)
				{
					global::Debug.Log("ExecuteRequest:" + loadingDataInfo.fullPath);
					this.ExecuteLoadScene(loadingDataInfo);
				}
			}
		}
	}

	public void Initialize()
	{
		if (Env.useAssetBundle)
		{
			string path = NetUtil.GetAssetBundleUrl() + "ablist.txt";
			AssetBundleManager.Instance.RequestNoCache(path, global::AssetBundleRequest.Type.TEXT, base.gameObject);
			if (NetMonitor.Instance != null)
			{
				this.m_connectUIDisplayTime = 0.01f;
				NetMonitor.Instance.StartMonitor(new AssetBundleRetryProcess(base.gameObject), this.m_connectUIDisplayTime, HudNetworkConnect.DisplayType.ALL);
			}
			this.m_mode = AssetBundleLoader.Mode.DownloadList;
		}
		else
		{
			this.m_mode = AssetBundleLoader.Mode.EnableLoad;
		}
	}

	public void ClearDownloadList()
	{
		this.m_mode = AssetBundleLoader.Mode.Non;
		this.m_bundleInfoList = null;
		this.m_loadingDataList.Clear();
		this.m_retryRequest.Clear();
	}

	public bool IsEnableDownlad()
	{
		return this.m_mode == AssetBundleLoader.Mode.EnableLoad;
	}

	public void RequestLoadScene(string filePath, bool cashed, GameObject returnObject)
	{
		if (this.m_mode != AssetBundleLoader.Mode.EnableLoad)
		{
			global::Debug.Log("AssetBundleLoader Not Initialized.");
			return;
		}
		if (Env.useAssetBundle)
		{
			AssetBundleLoader.LoadingDataInfo loadingDataInfo = new AssetBundleLoader.LoadingDataInfo();
			loadingDataInfo.name = Path.GetFileNameWithoutExtension(filePath);
			loadingDataInfo.filePath = filePath;
			loadingDataInfo.fullPath = this.GetDownloadURL(filePath);
			loadingDataInfo.cashed = cashed;
			loadingDataInfo.returnObject = returnObject;
			bool flag = !this.IsDownloaded(filePath);
			if (flag)
			{
				this.m_isConnecting = true;
				if (NetMonitor.Instance != null && NetMonitor.Instance.IsIdle())
				{
					this.m_connectUIDisplayTime = -0.1f;
					NetMonitor.Instance.StartMonitor(new AssetBundleRetryProcess(base.gameObject), this.m_connectUIDisplayTime, HudNetworkConnect.DisplayType.ALL);
				}
			}
			this.m_loadingDataList.Add(loadingDataInfo.name, loadingDataInfo);
		}
		else
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
			Application.LoadLevelAdditive(fileNameWithoutExtension);
		}
	}

	public void RetryLoadScene(AssetBundleRetryProcess retryProcess)
	{
		if (NetMonitor.Instance != null)
		{
			NetMonitor.Instance.StartMonitor(new AssetBundleRetryProcess(base.gameObject), this.m_connectUIDisplayTime, HudNetworkConnect.DisplayType.ALL);
		}
		foreach (global::AssetBundleRequest current in this.m_retryRequest)
		{
			AssetBundleManager.Instance.ReRequest(current);
		}
		this.m_retryRequest.Clear();
	}

	public bool IsDownloaded(string fileName)
	{
		AssetBundleLoader.CachedFileInfo fileInfo = this.GetFileInfo(fileName);
		return fileInfo != null && fileInfo.downloaded;
	}

	private void ExecuteLoadScene(AssetBundleLoader.LoadingDataInfo loadDataInfo)
	{
		string fullPath = loadDataInfo.fullPath;
		bool cashed = loadDataInfo.cashed;
		if (cashed)
		{
			string filePath = loadDataInfo.filePath;
			AssetBundleLoader.CachedFileInfo fileInfo = this.GetFileInfo(filePath);
			loadDataInfo.cashedInfo = fileInfo;
			this.LoadSceneCache(fullPath, fileInfo);
		}
		else
		{
			AssetBundleManager.Instance.RequestNoCache(fullPath, global::AssetBundleRequest.Type.SCENE, base.gameObject);
		}
	}

	private void LoadSceneCache(string fullPath, AssetBundleLoader.CachedFileInfo info)
	{
		int version = 0;
		uint crc = 0u;
		if (info != null)
		{
			version = info.version;
			crc = info.crc;
		}
		AssetBundleManager.Instance.Request(fullPath, version, crc, global::AssetBundleRequest.Type.SCENE, base.gameObject, true);
	}

	private void AssetBundleResponseSucceed(MsgAssetBundleResponseSucceed msg)
	{
		if (this.m_mode == AssetBundleLoader.Mode.DownloadList)
		{
			base.StartCoroutine(this.WaitCachingReady());
			string text = msg.m_result.Text;
			if (text != null)
			{
				this.ParseAssetBundleList(text);
			}
			AssetBundleManager.Instance.RequestUnload(msg.m_request.path);
			if (NetMonitor.Instance != null)
			{
				NetMonitor.Instance.EndMonitorForward(msg, base.gameObject, null);
				NetMonitor.Instance.EndMonitorBackward();
			}
			this.m_mode = AssetBundleLoader.Mode.EnableLoad;
		}
		else
		{
			AssetBundleResult result = msg.m_result;
			if (result != null)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(result.Path);
				AssetBundleLoader.LoadingDataInfo loadingDataInfo = null;
				if (this.m_loadingDataList.TryGetValue(fileNameWithoutExtension, out loadingDataInfo))
				{
					if (loadingDataInfo.cashedInfo != null)
					{
						loadingDataInfo.cashedInfo.downloaded = true;
					}
					if (loadingDataInfo.returnObject != null)
					{
						loadingDataInfo.returnObject.SendMessage("AssetBundleResponseSucceed", msg, SendMessageOptions.DontRequireReceiver);
					}
					this.m_loadingDataList.Remove(fileNameWithoutExtension);
					if (NetMonitor.Instance != null && this.m_isConnecting && this.m_loadingDataList.Count <= 0)
					{
						this.m_isConnecting = false;
						NetMonitor.Instance.EndMonitorForward(msg, base.gameObject, null);
						NetMonitor.Instance.EndMonitorBackward();
					}
				}
			}
		}
	}

	private void AssetBundleResponseFailed(MsgAssetBundleResponseFailed msg)
	{
		global::Debug.Log("Load Failed.");
		if (this.m_mode == AssetBundleLoader.Mode.DownloadList)
		{
			this.m_retryRequest.Add(msg.m_request);
			if (NetMonitor.Instance != null)
			{
				NetMonitor.Instance.EndMonitorForward(new MsgAssetBundleResponseFailedMonitor(), null, null);
				NetMonitor.Instance.EndMonitorBackward();
			}
		}
		else
		{
			string fileName = msg.m_request.FileName;
			AssetBundleLoader.LoadingDataInfo loadingDataInfo = null;
			if (this.m_loadingDataList.TryGetValue(fileName, out loadingDataInfo))
			{
				this.m_retryRequest.Add(msg.m_request);
				NetMonitor.Instance.EndMonitorForward(new MsgAssetBundleResponseFailedMonitor(), loadingDataInfo.returnObject, null);
				NetMonitor.Instance.EndMonitorBackward();
			}
		}
	}

	private IEnumerator WaitCachingReady()
	{
		return new AssetBundleLoader._WaitCachingReady_c__IteratorBC();
	}

	private void AssetBundleResponseFailedMonitor(MsgAssetBundleResponseFailedMonitor msg)
	{
	}

	private void ParseAssetBundleList(string str)
	{
		List<CsvParser.CsvFields> list = CsvParser.ParseCsvFromText(str);
		if (list != null && list.Count > 0)
		{
			this.m_bundleInfoList = new Dictionary<string, AssetBundleLoader.CachedFileInfo>();
			foreach (CsvParser.CsvFields current in list)
			{
				List<string> fieldList = current.FieldList;
				if (fieldList.Count >= 2)
				{
					AssetBundleLoader.CachedFileInfo cachedFileInfo = new AssetBundleLoader.CachedFileInfo();
					cachedFileInfo.name = Path.GetFileNameWithoutExtension(fieldList[0]);
					int.TryParse(fieldList[1], out cachedFileInfo.version);
					if (fieldList.Count >= 3)
					{
						uint.TryParse(fieldList[2], out cachedFileInfo.crc);
					}
					string downloadURL = this.GetDownloadURL(cachedFileInfo.name);
					if (Caching.IsVersionCached(downloadURL, cachedFileInfo.version))
					{
						cachedFileInfo.downloaded = true;
					}
					else
					{
						cachedFileInfo.downloaded = false;
					}
					this.m_bundleInfoList.Add(cachedFileInfo.name, cachedFileInfo);
				}
			}
		}
	}

	private AssetBundleLoader.CachedFileInfo GetFileInfo(string path)
	{
		if (this.m_bundleInfoList == null)
		{
			return null;
		}
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
		AssetBundleLoader.CachedFileInfo result = null;
		this.m_bundleInfoList.TryGetValue(fileNameWithoutExtension, out result);
		return result;
	}

	private string GetDownloadURL(string filePath)
	{
		string text = NetUtil.GetAssetBundleUrl() + filePath;
		string extension = Path.GetExtension(filePath);
		if (string.IsNullOrEmpty(extension))
		{
			text += ".unity3d";
		}
		return text;
	}

	protected void Awake()
	{
		this.CheckInstance();
	}

	public static void Create()
	{
		if (AssetBundleLoader.instance == null)
		{
			GameObject gameObject = new GameObject("AssetBundleLoader");
			gameObject.AddComponent<AssetBundleLoader>();
			if (AssetBundleManager.Instance == null)
			{
				AssetBundleManager.Create();
			}
		}
	}

	protected bool CheckInstance()
	{
		if (AssetBundleLoader.instance == null)
		{
			AssetBundleLoader.instance = this;
			return true;
		}
		if (this == AssetBundleLoader.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}

	private void OnDestroy()
	{
		if (this == AssetBundleLoader.instance)
		{
			AssetBundleLoader.instance = null;
		}
	}
}
