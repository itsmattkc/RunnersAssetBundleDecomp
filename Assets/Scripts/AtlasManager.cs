using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class AtlasManager : MonoBehaviour
{
	private const string m_loadingLangAtlasName = "ui_load_word_Atlas";

	private const string m_eventDummyAtlasName = "ui_event_reference_Atlas";

	private const string m_playerAtlasName = "ui_cmn_player_bundle_Atlas";

	private const string m_chaoAtlasName = "ChaoTextures";

	private const string m_resultAtlasName = "ui_result_Atlas";

	private const string m_itemAtlasName = "ui_cmn_item_Atlas";

	private readonly string[] m_menuLangAtlasName = new string[]
	{
		"ui_item_set_2_word_Atlas",
		"ui_mm_contents_word_Atlas",
		"ui_ranking_word_Atlas",
		"ui_mm_info_page_word_Atlas"
	};

	private readonly string[] m_dividedMenuLangAtlasName = new string[]
	{
		"ui_player_set_2_word_Atlas",
		"ui_shop_word_Atlas",
		"ui_roulette_word_Atlas"
	};

	private readonly string[] m_stageLangAtlasName = new string[]
	{
		"ui_gp_bit_word_Atlas",
		"ui_result_word_Atlas",
		"ui_tutrial_word_Atlas",
		"ui_shop_word_Atlas"
	};

	private ResourceSceneLoader.ResourceInfo m_loadInfoForEvent = new ResourceSceneLoader.ResourceInfo(ResourceCategory.EVENT_RESOURCE, "EventResourceCommon", true, false, true, "EventResourceCommon", false);

	private UIAtlas m_itemAtlas;

	private ResourceSceneLoader m_sceneLoader;

	private bool m_loadedAtlas;

	private string m_eventLangDummyAtlasName = string.Empty;

	private string m_eventLangAtlasName = string.Empty;

	private static AtlasManager instance;

	private List<string> m_dontDestryAtlasList = new List<string>();

	public static AtlasManager Instance
	{
		get
		{
			return AtlasManager.instance;
		}
	}

	public UIAtlas ItemAtlas
	{
		get
		{
			return this.m_itemAtlas;
		}
	}

	public string EventLangAtlasName
	{
		get
		{
			return this.m_eventLangAtlasName;
		}
	}

	public void StartLoadAtlasForMenu()
	{
		if (this.m_sceneLoader == null)
		{
			GameObject gameObject = new GameObject("SceneLoader");
			if (gameObject != null)
			{
				this.m_sceneLoader = gameObject.AddComponent<ResourceSceneLoader>();
				this.AddLoadAtlasForMenu();
				base.enabled = true;
				this.m_loadedAtlas = false;
			}
		}
	}

	public void StartLoadAtlasForEventMenu()
	{
		if (this.m_sceneLoader == null)
		{
			GameObject gameObject = new GameObject("SceneLoader");
			if (gameObject != null)
			{
				this.m_sceneLoader = gameObject.AddComponent<ResourceSceneLoader>();
				this.AddLoadAtlasForTitle();
				base.enabled = true;
				this.m_loadedAtlas = false;
			}
		}
	}

	public void StartLoadAtlasForDividedMenu()
	{
		if (this.m_sceneLoader == null)
		{
			GameObject gameObject = new GameObject("SceneLoader");
			if (gameObject != null)
			{
				this.m_sceneLoader = gameObject.AddComponent<ResourceSceneLoader>();
				this.AddLoadAtlasForDividedMenu();
				base.enabled = true;
				this.m_loadedAtlas = false;
			}
		}
	}

	public void StartLoadAtlasForStage()
	{
		if (this.m_sceneLoader == null)
		{
			GameObject gameObject = new GameObject("SceneLoader");
			if (gameObject != null)
			{
				this.m_sceneLoader = gameObject.AddComponent<ResourceSceneLoader>();
				this.AddLoadAtlasForStage();
				base.enabled = true;
				this.m_loadedAtlas = false;
			}
		}
	}

	public void StartLoadAtlasForTitle()
	{
		if (this.m_sceneLoader == null)
		{
			GameObject gameObject = new GameObject("SceneLoader");
			if (gameObject != null)
			{
				this.m_sceneLoader = gameObject.AddComponent<ResourceSceneLoader>();
				this.AddLoadAtlasForTitle();
				base.enabled = true;
				this.m_loadedAtlas = false;
			}
		}
	}

	public void ResetReplaceAtlas()
	{
		if (this.m_itemAtlas != null)
		{
			this.m_itemAtlas = null;
		}
		UIAtlas[] array = Resources.FindObjectsOfTypeAll(typeof(UIAtlas)) as UIAtlas[];
		for (int i = 0; i < array.Length; i++)
		{
			array[i].replacement = null;
		}
	}

	public void ResetEventRelaceAtlas()
	{
		UIAtlas[] array = Resources.FindObjectsOfTypeAll(typeof(UIAtlas)) as UIAtlas[];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name == "ui_event_reference_Atlas")
			{
				array[i].replacement = null;
			}
		}
	}

	public void ReplaceAtlasForMenu(bool isReplaceDividedMenu)
	{
		if (this.m_loadedAtlas)
		{
			string str = "_" + TextUtility.GetSuffixe();
			UIAtlas[] array = Resources.FindObjectsOfTypeAll(typeof(UIAtlas)) as UIAtlas[];
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, "ui_cmn_item_Atlas");
			if (gameObject != null)
			{
				this.m_itemAtlas = gameObject.GetComponent<UIAtlas>();
			}
			for (int i = 0; i < this.m_menuLangAtlasName.Length; i++)
			{
				string name = this.m_menuLangAtlasName[i] + str;
				GameObject gameObject2 = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, name);
				if (gameObject2 != null)
				{
					UIAtlas component = gameObject2.GetComponent<UIAtlas>();
					if (component != null)
					{
						UIAtlas[] array2 = array;
						for (int j = 0; j < array2.Length; j++)
						{
							UIAtlas uIAtlas = array2[j];
							if (uIAtlas.name == this.m_menuLangAtlasName[i])
							{
								uIAtlas.replacement = component;
							}
						}
					}
				}
			}
			if (isReplaceDividedMenu)
			{
				for (int k = 0; k < this.m_dividedMenuLangAtlasName.Length; k++)
				{
					string name2 = this.m_dividedMenuLangAtlasName[k] + str;
					GameObject gameObject3 = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, name2);
					if (gameObject3 != null)
					{
						UIAtlas component2 = gameObject3.GetComponent<UIAtlas>();
						if (component2 != null)
						{
							UIAtlas[] array3 = array;
							for (int l = 0; l < array3.Length; l++)
							{
								UIAtlas uIAtlas2 = array3[l];
								if (uIAtlas2.name == this.m_dividedMenuLangAtlasName[k])
								{
									uIAtlas2.replacement = component2;
								}
							}
						}
					}
				}
			}
			GameObject gameObject4 = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, "ui_cmn_player_bundle_Atlas");
			if (gameObject4 != null)
			{
				UIAtlas component3 = gameObject4.GetComponent<UIAtlas>();
				UIAtlas[] array4 = array;
				for (int m = 0; m < array4.Length; m++)
				{
					UIAtlas uIAtlas3 = array4[m];
					if (uIAtlas3 != null && uIAtlas3.name == "ui_cmn_player_Atlas" && uIAtlas3 != component3)
					{
						uIAtlas3.replacement = component3;
					}
				}
			}
			this.ReplaceEventAtlas(array);
			this.ReplaceItemAtlas(array);
		}
	}

	public void ReplaceAtlasForStage()
	{
		if (this.m_loadedAtlas)
		{
			string str = "_" + TextUtility.GetSuffixe();
			UIAtlas[] array = Resources.FindObjectsOfTypeAll(typeof(UIAtlas)) as UIAtlas[];
			for (int i = 0; i < this.m_stageLangAtlasName.Length; i++)
			{
				string name = this.m_stageLangAtlasName[i] + str;
				GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, name);
				if (gameObject != null)
				{
					UIAtlas component = gameObject.GetComponent<UIAtlas>();
					if (component != null)
					{
						UIAtlas[] array2 = array;
						for (int j = 0; j < array2.Length; j++)
						{
							UIAtlas uIAtlas = array2[j];
							if (uIAtlas.name == this.m_stageLangAtlasName[i])
							{
								uIAtlas.replacement = component;
							}
						}
					}
				}
			}
			this.ReplaceEventAtlas(array);
			GameObject gameObject2 = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, "ui_result_Atlas");
			if (gameObject2 != null)
			{
				UIAtlas component2 = gameObject2.GetComponent<UIAtlas>();
				UIAtlas[] array3 = array;
				for (int k = 0; k < array3.Length; k++)
				{
					UIAtlas uIAtlas2 = array3[k];
					if (uIAtlas2 != null && uIAtlas2.name == "ui_result_reference_Atlas")
					{
						uIAtlas2.replacement = component2;
					}
				}
			}
			this.ReplaceItemAtlas(array);
		}
	}

	public void ReplaceAtlasForMenuLoading(UIAtlas[] atlasArray)
	{
		if (atlasArray != null)
		{
			this.ReplaceEventAtlas(atlasArray);
		}
	}

	public void ReplaceAtlasForLoading(UIAtlas referenceLoadingAtlas)
	{
		if (referenceLoadingAtlas != null)
		{
			string str = "_" + TextUtility.GetSuffixe();
			string name = "ui_load_word_Atlas" + str;
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, name);
			if (gameObject != null)
			{
				UIAtlas component = gameObject.GetComponent<UIAtlas>();
				if (component != null)
				{
					referenceLoadingAtlas.replacement = component;
				}
			}
		}
	}

	private void ReplaceEventAtlas(UIAtlas[] atlasList)
	{
		if (!string.IsNullOrEmpty(this.m_eventLangDummyAtlasName))
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, this.m_eventLangAtlasName);
			if (gameObject != null)
			{
				UIAtlas component = gameObject.GetComponent<UIAtlas>();
				for (int i = 0; i < atlasList.Length; i++)
				{
					UIAtlas uIAtlas = atlasList[i];
					if (uIAtlas.name == this.m_eventLangDummyAtlasName || uIAtlas.name == "ui_event_00000_Atlas")
					{
						Transform parent = uIAtlas.gameObject.transform.parent;
						if (!(parent != null) || !(parent.name == "EventResourceAtlas"))
						{
							uIAtlas.replacement = component;
						}
					}
				}
			}
		}
		this.ReplaceEventCommonAtlas(atlasList);
	}

	private UIAtlas GetEventCommonAtlas()
	{
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.EVENT_RESOURCE, "EventResourceCommon");
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "EventResourceAtlas");
			if (gameObject2 != null)
			{
				for (int i = 0; i < gameObject2.transform.childCount; i++)
				{
					UIAtlas component = gameObject2.transform.GetChild(i).GetComponent<UIAtlas>();
					if (component != null)
					{
						return component;
					}
				}
			}
		}
		return null;
	}

	private void ReplaceEventCommonAtlas(UIAtlas[] atlasList)
	{
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.EVENT_RESOURCE, "EventResourceCommon");
		if (gameObject == null)
		{
			gameObject = GameObject.Find("EventResourceCommon");
		}
		if (gameObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "EventResourceAtlas");
			if (gameObject2 != null)
			{
				UIAtlas uIAtlas = null;
				for (int i = 0; i < gameObject2.transform.childCount; i++)
				{
					uIAtlas = gameObject2.transform.GetChild(i).GetComponent<UIAtlas>();
					if (uIAtlas != null)
					{
						break;
					}
				}
				if (uIAtlas != null)
				{
					for (int j = 0; j < atlasList.Length; j++)
					{
						UIAtlas uIAtlas2 = atlasList[j];
						if (uIAtlas2.name == "ui_event_reference_Atlas")
						{
							uIAtlas2.replacement = uIAtlas;
						}
					}
				}
			}
		}
	}

	public void ReplacePlayerAtlasForRaidResult(UIAtlas referenceAtlas)
	{
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, "ui_result_Atlas");
		if (gameObject != null)
		{
			UIAtlas component = gameObject.GetComponent<UIAtlas>();
			if (referenceAtlas != null && referenceAtlas.name == "ui_cmn_player_Atlas" && referenceAtlas != component)
			{
				referenceAtlas.replacement = component;
			}
		}
	}

	private void ReplaceItemAtlas(UIAtlas[] atlasList)
	{
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, "ui_cmn_item_Atlas");
		if (gameObject != null)
		{
			UIAtlas component = gameObject.GetComponent<UIAtlas>();
			for (int i = 0; i < atlasList.Length; i++)
			{
				UIAtlas uIAtlas = atlasList[i];
				if (uIAtlas != null && uIAtlas.name == "ui_cmn_item_reference_Atlas")
				{
					uIAtlas.replacement = component;
				}
			}
		}
	}

	public void ClearAllAtlas()
	{
		List<UIAtlas> list = new List<UIAtlas>();
		foreach (string current in this.m_dontDestryAtlasList)
		{
			if (!string.IsNullOrEmpty(current))
			{
				GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, current);
				if (!(gameObject == null))
				{
					UIAtlas component = gameObject.GetComponent<UIAtlas>();
					if (!(component == null))
					{
						list.Add(component);
					}
				}
			}
		}
		UIAtlas eventCommonAtlas = this.GetEventCommonAtlas();
		if (eventCommonAtlas != null)
		{
			list.Add(eventCommonAtlas);
		}
		UIAtlas[] array = Resources.FindObjectsOfTypeAll<UIAtlas>();
		UIAtlas[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			UIAtlas uIAtlas = array2[i];
			if (!(uIAtlas == null))
			{
				bool flag = true;
				foreach (UIAtlas current2 in list)
				{
					if (list != null)
					{
						if (uIAtlas.name == current2.name)
						{
							flag = false;
							break;
						}
						if (uIAtlas.texture != null && uIAtlas.texture.name == current2.name)
						{
							flag = false;
							break;
						}
						if (uIAtlas.spriteMaterial != null && uIAtlas.spriteMaterial.name == current2.name)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					Resources.UnloadAsset(uIAtlas.texture);
					Resources.UnloadAsset(uIAtlas.spriteMaterial);
				}
			}
		}
	}

	public bool IsLoadAtlas()
	{
		return this.m_loadedAtlas;
	}

	protected void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
		base.enabled = false;
	}

	private void OnDestroy()
	{
		if (AtlasManager.instance == this)
		{
			if (this.m_itemAtlas != null)
			{
				this.m_itemAtlas = null;
			}
			AtlasManager.instance = null;
		}
	}

	private void SetInstance()
	{
		if (AtlasManager.instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			AtlasManager.instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (this.m_sceneLoader != null && this.m_sceneLoader.Loaded)
		{
			this.m_loadedAtlas = true;
			UnityEngine.Object.Destroy(this.m_sceneLoader.gameObject);
			this.m_sceneLoader = null;
			base.enabled = false;
		}
	}

	private void AddLoadEventLangAtlas()
	{
		if (EventManager.Instance != null)
		{
			this.m_eventLangDummyAtlasName = string.Empty;
			if (EventManager.Instance.IsInEvent())
			{
				switch (EventManager.GetType(EventManager.Instance.Id))
				{
				case EventManager.EventType.SPECIAL_STAGE:
					this.m_eventLangDummyAtlasName = "ui_event_10000_Atlas";
					break;
				case EventManager.EventType.RAID_BOSS:
					this.m_eventLangDummyAtlasName = "ui_event_20000_Atlas";
					break;
				case EventManager.EventType.COLLECT_OBJECT:
					this.m_eventLangDummyAtlasName = "ui_event_30000_Atlas";
					break;
				case EventManager.EventType.GACHA:
					this.m_eventLangDummyAtlasName = "ui_event_40000_Atlas";
					break;
				case EventManager.EventType.ADVERT:
					this.m_eventLangDummyAtlasName = "ui_event_50000_Atlas";
					break;
				case EventManager.EventType.QUICK:
					this.m_eventLangDummyAtlasName = "ui_event_60000_Atlas";
					break;
				case EventManager.EventType.BGM:
					this.m_eventLangDummyAtlasName = "ui_event_70000_Atlas";
					break;
				}
				int specificId = EventManager.GetSpecificId();
				if (specificId > 0)
				{
					this.m_eventLangAtlasName = "ui_event_" + specificId.ToString() + "_Atlas_" + TextUtility.GetSuffixe();
					ResourceSceneLoader.ResourceInfo resInfo = this.CreateResourceSceneLoader(this.m_eventLangAtlasName, true);
					this.AddSceneLoaderAndResourceManager(resInfo);
				}
			}
		}
	}

	private void AddLoadAtlasForMenu()
	{
		for (int i = 0; i < this.m_menuLangAtlasName.Length; i++)
		{
			string sceneName = this.m_menuLangAtlasName[i] + "_" + TextUtility.GetSuffixe();
			ResourceSceneLoader.ResourceInfo resInfo = this.CreateResourceSceneLoader(sceneName, false);
			this.AddSceneLoaderAndResourceManager(resInfo);
		}
		string sceneName2 = "ui_load_word_Atlas_" + TextUtility.GetSuffixe();
		ResourceSceneLoader.ResourceInfo resInfo2 = this.CreateResourceSceneLoader(sceneName2, true);
		this.AddSceneLoaderAndResourceManager(resInfo2);
		this.AddLoadEventLangAtlas();
		ResourceSceneLoader.ResourceInfo resInfo3 = this.CreateResourceSceneLoader("ui_cmn_player_bundle_Atlas", false);
		this.AddSceneLoaderAndResourceManager(resInfo3);
		ResourceSceneLoader.ResourceInfo resInfo4 = this.CreateResourceSceneLoader("ui_cmn_item_Atlas", true);
		this.AddSceneLoaderAndResourceManager(resInfo4);
	}

	private void AddLoadAtlasForDividedMenu()
	{
		for (int i = 0; i < this.m_dividedMenuLangAtlasName.Length; i++)
		{
			string sceneName = this.m_dividedMenuLangAtlasName[i] + "_" + TextUtility.GetSuffixe();
			ResourceSceneLoader.ResourceInfo resInfo = this.CreateResourceSceneLoader(sceneName, false);
			this.AddSceneLoaderAndResourceManager(resInfo);
		}
	}

	private void AddLoadAtlasForStage()
	{
		for (int i = 0; i < this.m_stageLangAtlasName.Length; i++)
		{
			string sceneName = this.m_stageLangAtlasName[i] + "_" + TextUtility.GetSuffixe();
			ResourceSceneLoader.ResourceInfo resInfo = this.CreateResourceSceneLoader(sceneName, false);
			this.AddSceneLoaderAndResourceManager(resInfo);
		}
		this.AddLoadEventLangAtlas();
		ResourceSceneLoader.ResourceInfo resInfo2 = this.CreateResourceSceneLoader("ui_result_Atlas", false);
		this.AddSceneLoaderAndResourceManager(resInfo2);
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, "ui_cmn_item_Atlas");
		if (gameObject == null)
		{
			ResourceSceneLoader.ResourceInfo resInfo3 = this.CreateResourceSceneLoader("ui_cmn_item_Atlas", true);
			this.AddSceneLoaderAndResourceManager(resInfo3);
		}
	}

	private void AddLoadAtlasForTitle()
	{
		this.AddLoadEventLangAtlas();
	}

	private void AddSceneLoaderAndResourceManager(ResourceSceneLoader.ResourceInfo resInfo)
	{
		if (this.m_sceneLoader == null)
		{
			return;
		}
		bool flag = this.m_sceneLoader.AddLoadAndResourceManager(resInfo);
		if (flag && resInfo.m_dontDestroyOnChangeScene)
		{
			this.m_dontDestryAtlasList.Add(resInfo.m_scenename);
		}
	}

	private ResourceSceneLoader.ResourceInfo CreateResourceSceneLoader(string sceneName, bool dontDestroy = false)
	{
		return new ResourceSceneLoader.ResourceInfo(ResourceCategory.UI, sceneName, true, false, dontDestroy, null, false);
	}
}
