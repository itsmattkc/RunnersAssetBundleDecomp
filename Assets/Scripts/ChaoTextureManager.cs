using DataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChaoTextureManager : MonoBehaviour
{
	public class TextureData
	{
		public int chao_id;

		public Texture tex;

		public TextureData()
		{
		}

		public TextureData(Texture tex, int chao_id)
		{
			this.tex = tex;
			this.chao_id = chao_id;
		}
	}

	public class CallbackInfo
	{
		public delegate void LoadFinishCallback(Texture tex);

		private Texture m_texture;

		private UITexture m_uiTex;

		private bool m_nguiRebuild;

		private ChaoTextureManager.CallbackInfo.LoadFinishCallback m_callback;

		public Texture Texture
		{
			get
			{
				return this.m_texture;
			}
			private set
			{
			}
		}

		public bool LoadEnded
		{
			get
			{
				return this.m_texture != null;
			}
		}

		public CallbackInfo(UITexture uiTex, ChaoTextureManager.CallbackInfo.LoadFinishCallback callback = null, bool nguiRebuild = false)
		{
			this.m_uiTex = uiTex;
			this.m_callback = callback;
			this.m_nguiRebuild = nguiRebuild;
			if (this.m_uiTex != null)
			{
				this.m_uiTex.enabled = true;
				this.m_uiTex.SetTexture(ChaoTextureManager.Instance.m_defaultTexture);
			}
		}

		public void Disable()
		{
			if (this.m_uiTex != null)
			{
				this.m_uiTex.enabled = false;
				this.m_uiTex.SetTexture(null);
			}
			if (this.m_callback != null)
			{
				this.m_callback(null);
			}
		}

		public void LoadDone(Texture tex)
		{
			this.m_texture = tex;
			if (this.m_uiTex != null)
			{
				this.m_uiTex.enabled = true;
				if (this.m_nguiRebuild)
				{
					this.m_uiTex.mainTexture = tex;
				}
				else
				{
					this.m_uiTex.SetTexture(tex);
				}
			}
			if (this.m_callback != null)
			{
				this.m_callback(this.m_texture);
			}
		}
	}

	public class LoadRequestData
	{
		public enum LoadType
		{
			Default,
			Event
		}

		private enum RequestMode
		{
			IDLE,
			LOAD,
			LOAD_WAIT,
			SET_TEXTURE,
			END
		}

		private ChaoTextureManager.LoadRequestData.RequestMode m_requestMode;

		private ResourceSceneLoader m_sceneLoader;

		private GameObject m_managerObj;

		private bool m_cancel;

		public int m_chaoId;

		public ChaoTextureManager.LoadRequestData.LoadType m_type;

		private List<ChaoTextureManager.CallbackInfo> m_infoList;

		public bool Loaded
		{
			get
			{
				return this.m_requestMode == ChaoTextureManager.LoadRequestData.RequestMode.END;
			}
		}

		public LoadRequestData()
		{
		}

		public LoadRequestData(GameObject managerObj, int chaoId, ChaoTextureManager.CallbackInfo info, ChaoTextureManager.LoadRequestData.LoadType type = ChaoTextureManager.LoadRequestData.LoadType.Default)
		{
			this.m_managerObj = managerObj;
			this.m_chaoId = chaoId;
			this.m_type = type;
			this.m_infoList = new List<ChaoTextureManager.CallbackInfo>();
			this.m_infoList.Add(info);
		}

		public void AddCallback(ChaoTextureManager.CallbackInfo info)
		{
			if (info == null)
			{
				return;
			}
			if (this.m_infoList.Contains(info))
			{
				return;
			}
			this.m_infoList.Add(info);
		}

		public void StartLoad()
		{
			this.m_requestMode = ChaoTextureManager.LoadRequestData.RequestMode.LOAD;
		}

		public void Cancel()
		{
			this.m_cancel = true;
		}

		public void Update()
		{
			switch (this.m_requestMode)
			{
			case ChaoTextureManager.LoadRequestData.RequestMode.LOAD:
			{
				GameObject gameObject = new GameObject("SceneLoader");
				this.m_sceneLoader = gameObject.AddComponent<ResourceSceneLoader>();
				string scenename = "ui_tex_chao_" + this.m_chaoId.ToString("0000");
				ResourceSceneLoader.ResourceInfo resourceInfo = new ResourceSceneLoader.ResourceInfo();
				resourceInfo.m_scenename = scenename;
				resourceInfo.m_isAssetBundle = true;
				resourceInfo.m_onlyDownload = false;
				resourceInfo.m_isAsycnLoad = true;
				resourceInfo.m_category = ResourceCategory.UI;
				this.m_sceneLoader.AddLoadAndResourceManager(resourceInfo);
				this.m_requestMode = ChaoTextureManager.LoadRequestData.RequestMode.LOAD_WAIT;
				break;
			}
			case ChaoTextureManager.LoadRequestData.RequestMode.LOAD_WAIT:
				if (this.m_sceneLoader != null)
				{
					if (this.m_sceneLoader.Loaded)
					{
						this.m_requestMode = ChaoTextureManager.LoadRequestData.RequestMode.SET_TEXTURE;
					}
				}
				else
				{
					this.m_requestMode = ChaoTextureManager.LoadRequestData.RequestMode.SET_TEXTURE;
				}
				break;
			case ChaoTextureManager.LoadRequestData.RequestMode.SET_TEXTURE:
				if (this.m_cancel)
				{
					string name = "ui_tex_chao_" + this.m_chaoId.ToString("0000");
					GameObject gameObject2 = GameObject.Find(name);
					if (gameObject2 != null)
					{
						UnityEngine.Object.Destroy(gameObject2);
					}
				}
				else
				{
					string name2 = "ui_tex_chao_" + this.m_chaoId.ToString("0000");
					GameObject gameObject3 = GameObject.Find(name2);
					if (gameObject3 != null)
					{
						ChaoTextureManager.LoadRequestData.LoadType type = this.m_type;
						if (type != ChaoTextureManager.LoadRequestData.LoadType.Default)
						{
							if (type == ChaoTextureManager.LoadRequestData.LoadType.Event)
							{
								GameObject gameObject4 = GameObjectUtil.FindChildGameObject(this.m_managerObj, "Event");
								gameObject3.transform.parent = gameObject4.transform;
							}
						}
						else
						{
							GameObject gameObject5 = GameObjectUtil.FindChildGameObject(this.m_managerObj, "Texture");
							gameObject3.transform.parent = gameObject5.transform;
						}
						gameObject3.SetActive(false);
					}
					GameObject gameObject6 = GameObjectUtil.FindChildGameObject(this.m_managerObj, name2);
					AssetBundleTexture component = gameObject6.GetComponent<AssetBundleTexture>();
					Texture tex = component.m_tex;
					if (tex != null)
					{
						for (int i = 0; i < this.m_infoList.Count; i++)
						{
							ChaoTextureManager.CallbackInfo callbackInfo = this.m_infoList[i];
							if (callbackInfo != null)
							{
								callbackInfo.LoadDone(tex);
							}
						}
						this.m_infoList.Clear();
					}
				}
				UnityEngine.Object.Destroy(this.m_sceneLoader.gameObject);
				this.m_requestMode = ChaoTextureManager.LoadRequestData.RequestMode.END;
				break;
			}
		}

		public Texture GetTexture(int chao_id)
		{
			string name = "ui_tex_chao_" + this.m_chaoId.ToString("0000");
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_managerObj, name);
			if (gameObject != null)
			{
				AssetBundleTexture component = gameObject.GetComponent<AssetBundleTexture>();
				if (component != null)
				{
					return component.m_tex;
				}
			}
			return null;
		}
	}

	private sealed class _WaitUnloadUnusedAssets_c__Iterator7 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _waite_frame___0;

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
				this._waite_frame___0 = 1;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._waite_frame___0 > 0)
			{
				this._waite_frame___0--;
				this._current = null;
				this._PC = 1;
				return true;
			}
			Resources.UnloadUnusedAssets();
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

	private sealed class _LoadNext_c__Iterator8 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal float _startTime___0;

		internal float _spendTime___1;

		internal float _currentTime___2;

		internal int _PC;

		internal object _current;

		internal ChaoTextureManager __f__this;

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
				this._startTime___0 = Time.realtimeSinceStartup;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			this._spendTime___1 = 0f;
			this._currentTime___2 = Time.realtimeSinceStartup;
			this._spendTime___1 = this._currentTime___2 - this._startTime___0;
			if (this._spendTime___1 < 0.1f)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this.__f__this.m_loadingList.Count > 0)
			{
				this.__f__this.m_currentRequest = this.__f__this.m_loadingList[0];
				this.__f__this.m_currentRequest.StartLoad();
				this.__f__this.m_loadingList.Remove(this.__f__this.m_loadingList[0]);
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

	private const string TEX_FOLDER = "Texture";

	private const string EVENT_FOLDER = "Event";

	[SerializeField]
	public Texture m_defaultTexture;

	private List<ChaoTextureManager.LoadRequestData> m_loadingList = new List<ChaoTextureManager.LoadRequestData>();

	private ChaoTextureManager.LoadRequestData m_currentRequest;

	private GameObject m_texObj;

	private int m_loadingChaoId = -1;

	private static ChaoTextureManager instance;

	public static ChaoTextureManager Instance
	{
		get
		{
			return ChaoTextureManager.instance;
		}
	}

	public int LoadingChaoId
	{
		get
		{
			return this.m_loadingChaoId;
		}
	}

	public Texture GetLoadedTexture(int chao_id)
	{
		if (chao_id < 0)
		{
			return null;
		}
		Texture result = null;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, this.GetChaoTexName(chao_id));
		if (gameObject != null)
		{
			AssetBundleTexture component = gameObject.GetComponent<AssetBundleTexture>();
			if (component != null)
			{
				result = component.m_tex;
			}
		}
		return result;
	}

	public void GetTexture(int chao_id, ChaoTextureManager.CallbackInfo info)
	{
		if (info == null)
		{
			return;
		}
		if (chao_id < 0)
		{
			info.Disable();
			return;
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, this.GetChaoTexName(chao_id));
		if (gameObject != null)
		{
			AssetBundleTexture component = gameObject.GetComponent<AssetBundleTexture>();
			if (component != null)
			{
				info.LoadDone(component.m_tex);
			}
		}
		else
		{
			bool flag = true;
			foreach (ChaoTextureManager.LoadRequestData current in this.m_loadingList)
			{
				if (current.m_chaoId == chao_id)
				{
					if (info != null)
					{
						current.AddCallback(info);
					}
					flag = false;
					break;
				}
			}
			if (flag)
			{
				if (this.m_currentRequest != null && this.m_currentRequest.m_chaoId == chao_id)
				{
					this.m_currentRequest.AddCallback(info);
					flag = false;
				}
				if (flag)
				{
					ChaoTextureManager.LoadRequestData loadRequestData = new ChaoTextureManager.LoadRequestData(base.gameObject, chao_id, info, ChaoTextureManager.LoadRequestData.LoadType.Default);
					if (this.m_currentRequest == null)
					{
						this.m_currentRequest = loadRequestData;
						this.m_currentRequest.StartLoad();
					}
					else
					{
						this.m_loadingList.Add(loadRequestData);
					}
					base.enabled = true;
				}
			}
		}
	}

	public void RequestLoadingPageChaoTexture()
	{
		ChaoData loadingChao = ChaoTable.GetLoadingChao();
		if (loadingChao != null)
		{
			this.m_loadingChaoId = loadingChao.id;
			ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(null, null, false);
			this.GetTexture(loadingChao.id, info);
		}
	}

	public void RequestTitleLoadChaoTexture()
	{
		this.RequestLoadingPageChaoTexture();
	}

	public void RemoveChaoTexture(int chao_id)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_texObj, this.GetChaoTexName(chao_id));
		if (gameObject != null)
		{
			string name = gameObject.name;
			int num = -1;
			int num2 = -1;
			SaveDataManager saveDataManager = SaveDataManager.Instance;
			if (saveDataManager != null)
			{
				num = saveDataManager.PlayerData.MainChaoID;
				num2 = saveDataManager.PlayerData.SubChaoID;
			}
			List<string> list = new List<string>();
			if (num >= 0)
			{
				list.Add(this.GetChaoTexName(num));
			}
			if (num2 >= 0)
			{
				list.Add(this.GetChaoTexName(num2));
			}
			if (this.m_loadingChaoId >= 0)
			{
				list.Add(this.GetChaoTexName(this.m_loadingChaoId));
			}
			if (!list.Contains(name))
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			base.StartCoroutine(this.WaitUnloadUnusedAssets());
		}
	}

	public void RemoveChaoTextureForMainMenuEnd()
	{
		int num = -1;
		int num2 = -1;
		SaveDataManager saveDataManager = SaveDataManager.Instance;
		if (saveDataManager != null)
		{
			num = saveDataManager.PlayerData.MainChaoID;
			num2 = saveDataManager.PlayerData.SubChaoID;
		}
		List<string> list = new List<string>();
		if (num >= 0)
		{
			list.Add(this.GetChaoTexName(num));
		}
		if (num2 >= 0)
		{
			list.Add(this.GetChaoTexName(num2));
		}
		if (this.m_loadingChaoId >= 0)
		{
			list.Add(this.GetChaoTexName(this.m_loadingChaoId));
		}
		if (EventManager.Instance != null)
		{
			RewardChaoData rewardChaoData = EventManager.Instance.GetRewardChaoData();
			if (rewardChaoData != null && rewardChaoData.chao_id >= 0)
			{
				list.Add(this.GetChaoTexName(rewardChaoData.chao_id));
			}
		}
		List<GameObject> list2 = new List<GameObject>();
		int childCount = this.m_texObj.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = this.m_texObj.transform.GetChild(i);
			string name = child.name;
			if (!list.Contains(name))
			{
				list2.Add(child.gameObject);
			}
		}
		foreach (GameObject current in list2)
		{
			UnityEngine.Object.Destroy(current);
		}
		this.CancelLoad();
	}

	public void RemoveChaoTexture()
	{
		int num = -1;
		int num2 = -1;
		SaveDataManager saveDataManager = SaveDataManager.Instance;
		if (saveDataManager != null)
		{
			num = saveDataManager.PlayerData.MainChaoID;
			num2 = saveDataManager.PlayerData.SubChaoID;
		}
		List<string> list = new List<string>();
		if (num >= 0)
		{
			list.Add(this.GetChaoTexName(num));
		}
		if (num2 >= 0)
		{
			list.Add(this.GetChaoTexName(num2));
		}
		if (this.m_loadingChaoId >= 0)
		{
			list.Add(this.GetChaoTexName(this.m_loadingChaoId));
		}
		List<GameObject> list2 = new List<GameObject>();
		int childCount = this.m_texObj.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = this.m_texObj.transform.GetChild(i);
			string name = child.name;
			if (!list.Contains(name))
			{
				list2.Add(child.gameObject);
			}
		}
		foreach (GameObject current in list2)
		{
			UnityEngine.Object.Destroy(current);
		}
		this.CancelLoad();
		base.StartCoroutine(this.WaitUnloadUnusedAssets());
	}

	private IEnumerator WaitUnloadUnusedAssets()
	{
		return new ChaoTextureManager._WaitUnloadUnusedAssets_c__Iterator7();
	}

	public bool IsLoaded()
	{
		return this.m_currentRequest == null && this.m_loadingList.Count <= 0;
	}

	protected void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "Event";
		gameObject.transform.parent = base.transform;
		this.m_texObj = new GameObject();
		this.m_texObj.name = "Texture";
		this.m_texObj.transform.parent = base.transform;
		base.enabled = false;
	}

	private void OnDestroy()
	{
		if (ChaoTextureManager.instance == this)
		{
			ChaoTextureManager.instance = null;
		}
	}

	private void SetInstance()
	{
		if (ChaoTextureManager.instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			ChaoTextureManager.instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (this.m_currentRequest != null)
		{
			this.m_currentRequest.Update();
			if (this.m_currentRequest.Loaded)
			{
				this.m_currentRequest = null;
				if (this.m_loadingList.Count > 0)
				{
					base.StartCoroutine(this.LoadNext());
				}
				else
				{
					base.enabled = false;
				}
			}
		}
	}

	private IEnumerator LoadNext()
	{
		ChaoTextureManager._LoadNext_c__Iterator8 _LoadNext_c__Iterator = new ChaoTextureManager._LoadNext_c__Iterator8();
		_LoadNext_c__Iterator.__f__this = this;
		return _LoadNext_c__Iterator;
	}

	private void CancelLoad()
	{
		if (this.m_currentRequest != null)
		{
			this.m_currentRequest.Cancel();
		}
		this.m_loadingList.Clear();
	}

	private string GetChaoTexName(int chao_id)
	{
		return "ui_tex_chao_" + chao_id.ToString("0000");
	}
}
