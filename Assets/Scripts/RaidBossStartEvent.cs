using SaveData;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class RaidBossStartEvent : MonoBehaviour
{
	public enum ProductType
	{
		MileagePage,
		EventTopPage
	}

	private enum Mode
	{
		Idle,
		CheckId,
		WaitCommonResource,
		Load,
		WaitLoad,
		Start,
		WaitEnd,
		Next,
		End
	}

	private const string WINDOW_NAME = "RaidBossStartEvent";

	private RaidBossStartEvent.Mode m_mode;

	private ResourceSceneLoader m_sceneLoader;

	private int m_productProgress = -1;

	private int m_textWindowId = -1;

	private List<string> m_loadTexList = new List<string>();

	private WindowEventData m_windowEventData;

	private RaidBossStartEvent.ProductType m_productType;

	private bool m_isNotPlaybackDefaultBgm;

	private bool m_isEnd;

	private bool m_alertFlag;

	private bool m_firstBattle;

	private bool m_commonResourceLoaded;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	private void Start()
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
	}

	private void OnDestroy()
	{
		this.SetColision(false);
		BackKeyManager.RemoveWindowCallBack(base.gameObject);
		if (this.m_windowEventData != null)
		{
			this.m_windowEventData = null;
		}
	}

	public static RaidBossStartEvent Create(GameObject obj, RaidBossStartEvent.ProductType type)
	{
		if (obj != null)
		{
			RaidBossStartEvent raidBossStartEvent = obj.GetComponent<RaidBossStartEvent>();
			if (raidBossStartEvent == null)
			{
				raidBossStartEvent = obj.AddComponent<RaidBossStartEvent>();
			}
			else if (GeneralWindow.IsCreated("RaidBossStartEvent"))
			{
				GeneralWindow.Close();
			}
			if (raidBossStartEvent != null)
			{
				raidBossStartEvent.ResetParam(type);
			}
			return raidBossStartEvent;
		}
		return null;
	}

	public void CloseWindow()
	{
		this.SetColision(false);
	}

	private bool StartEvent()
	{
		this.m_windowEventData = EventManager.Instance.GetWindowEvenData(this.m_textWindowId);
		this.m_isNotPlaybackDefaultBgm = false;
		this.m_isEnd = false;
		if (this.m_windowEventData != null)
		{
			GeneralWindow.CInfo.Event[] array = new GeneralWindow.CInfo.Event[this.m_windowEventData.body.Length];
			for (int i = 0; i < this.m_windowEventData.body.Length; i++)
			{
				WindowBodyData windowBodyData = this.m_windowEventData.body[i];
				GeneralWindow.CInfo.Event.FaceWindow[] array2 = null;
				if (windowBodyData.product != null)
				{
					array2 = new GeneralWindow.CInfo.Event.FaceWindow[windowBodyData.product.Length];
					for (int j = 0; j < windowBodyData.product.Length; j++)
					{
						WindowProductData windowProductData = windowBodyData.product[j];
						array2[j] = new GeneralWindow.CInfo.Event.FaceWindow
						{
							texture = this.GetTexture(windowProductData.face_id),
							name = ((windowProductData.name_cell_id == null) ? null : MileageMapText.GetName(windowProductData.name_cell_id)),
							effectType = windowProductData.effect,
							animType = windowProductData.anim,
							reverseType = windowProductData.reverse,
							showingType = windowProductData.showing
						};
					}
				}
				array[i] = new GeneralWindow.CInfo.Event
				{
					faceWindows = array2,
					arrowType = windowBodyData.arrow,
					bgmCueName = windowBodyData.bgm,
					seCueName = windowBodyData.se,
					message = this.GetText(windowBodyData.text_cell_id)
				};
			}
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "RaidBossStartEvent",
				buttonType = GeneralWindow.ButtonType.OkNextSkip,
				caption = MileageMapText.GetMapCommonText(this.m_windowEventData.title_cell_id),
				events = array,
				isNotPlaybackDefaultBgm = this.m_isNotPlaybackDefaultBgm,
				isSpecialEvent = true
			});
			return true;
		}
		return false;
	}

	private string GetText(string cellID)
	{
		string text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_SPECIFIC, "Production", cellID);
		if (text == null)
		{
			text = "NoText";
		}
		return text;
	}

	private bool LoadTexture()
	{
		this.m_loadTexList.Clear();
		this.m_loadTexList = this.GetResourceNameList();
		if (this.m_loadTexList.Count > 0)
		{
			this.CreateLoadObject();
			if (this.m_sceneLoader != null)
			{
				foreach (string current in this.m_loadTexList)
				{
					ResourceSceneLoader.ResourceInfo info = new ResourceSceneLoader.ResourceInfo(ResourceCategory.EVENT_RESOURCE, current, true, false, false, null, false);
					this.m_sceneLoader.AddLoadAndResourceManager(info);
				}
				return true;
			}
		}
		return false;
	}

	private void DestroyTextureData()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "FaceTextures");
		if (gameObject != null)
		{
			UnityEngine.Object.DestroyImmediate(gameObject);
			Resources.UnloadUnusedAssets();
		}
	}

	private void CreateLoadObject()
	{
		GameObject gameObject = new GameObject("SceneLoader");
		if (gameObject != null)
		{
			this.m_sceneLoader = gameObject.AddComponent<ResourceSceneLoader>();
		}
	}

	private Texture GetTexture(int id)
	{
		string faceTextureName = MileageMapUtility.GetFaceTextureName(id);
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.EVENT_RESOURCE, faceTextureName);
		if (!(gameObject != null))
		{
			return MileageMapUtility.GetFaceTexture(id);
		}
		AssetBundleTexture component = gameObject.GetComponent<AssetBundleTexture>();
		if (component != null)
		{
			return component.m_tex;
		}
		return null;
	}

	private List<string> GetResourceNameList()
	{
		List<string> list = new List<string>();
		List<int> list2 = new List<int>();
		if (EventManager.Instance != null)
		{
			WindowEventData windowEvenData = EventManager.Instance.GetWindowEvenData(this.m_textWindowId);
			if (windowEvenData != null)
			{
				WindowBodyData[] body = windowEvenData.body;
				for (int i = 0; i < body.Length; i++)
				{
					WindowBodyData windowBodyData = body[i];
					for (int j = 0; j < windowBodyData.face_count; j++)
					{
						if (j < windowBodyData.product.Length && !list2.Contains(windowBodyData.product[j].face_id))
						{
							list2.Add(windowBodyData.product[j].face_id);
						}
					}
				}
			}
		}
		if (list2.Count > 0)
		{
			foreach (int current in list2)
			{
				Texture faceTexture = MileageMapUtility.GetFaceTexture(current);
				if (faceTexture == null)
				{
					string faceTextureName = MileageMapUtility.GetFaceTextureName(current);
					GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.EVENT_RESOURCE, faceTextureName);
					if (gameObject == null)
					{
						list.Add(faceTextureName);
					}
				}
			}
		}
		return list;
	}

	private int GetTextWindowId()
	{
		if (EventManager.Instance != null)
		{
			EventRaidProductionData raidProductionData = EventManager.Instance.GetRaidProductionData();
			if (raidProductionData != null)
			{
				if (this.m_productType == RaidBossStartEvent.ProductType.MileagePage)
				{
					EventProductionData mileagePage = raidProductionData.mileagePage;
					if (mileagePage != null && this.m_productProgress < mileagePage.textWindowId.Length)
					{
						return mileagePage.textWindowId[this.m_productProgress];
					}
				}
				else if (this.m_firstBattle)
				{
					EventProductionData firstBattle = raidProductionData.firstBattle;
					if (firstBattle != null && 0 < firstBattle.textWindowId.Length)
					{
						return firstBattle.textWindowId[0];
					}
				}
				else
				{
					EventProductionData eventTop = raidProductionData.eventTop;
					if (eventTop != null && this.m_productProgress < eventTop.textWindowId.Length)
					{
						return eventTop.textWindowId[this.m_productProgress];
					}
				}
			}
		}
		return -1;
	}

	public void SaveEventTopPagePictureEvent()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				systemdata.pictureShowEventId = EventManager.Instance.Id;
				systemdata.pictureShowProgress = this.m_productProgress;
			}
			instance.SaveSystemData();
		}
	}

	public void SaveMileagePagePictureEvent()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				systemdata.pictureShowEventId = EventManager.Instance.Id;
				systemdata.pictureShowEmergeRaidBossProgress = this.m_productProgress;
			}
			instance.SaveSystemData();
		}
	}

	public static bool IsTopMenuProduction()
	{
		if (EventManager.Instance != null && SystemSaveManager.Instance)
		{
			ServerEventUserRaidBossState raidBossState = EventManager.Instance.RaidBossState;
			SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
			if (systemdata == null || raidBossState == null)
			{
				return false;
			}
			EventRaidProductionData raidProductionData = EventManager.Instance.GetRaidProductionData();
			if (raidProductionData != null)
			{
				EventProductionData mileagePage = raidProductionData.mileagePage;
				if (mileagePage != null)
				{
					int pictureShowEmergeRaidBossProgress = systemdata.pictureShowEmergeRaidBossProgress;
					int numRaidBossEncountered = raidBossState.NumRaidBossEncountered;
					int numBeatedEncounter = raidBossState.NumBeatedEncounter;
					for (int i = 0; i < mileagePage.startCollectCount.Length; i++)
					{
						int num = mileagePage.startCollectCount[i];
						if (numRaidBossEncountered >= num && numBeatedEncounter >= num - 1 && i > pictureShowEmergeRaidBossProgress)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	private int GetNextWindowProductIndex(RaidBossStartEvent.ProductType type, ref bool firstBattle)
	{
		int result = -1;
		if (EventManager.Instance != null && !EventManager.Instance.IsChallengeEvent())
		{
			return result;
		}
		if (SystemSaveManager.Instance == null || EventManager.Instance == null)
		{
			return result;
		}
		SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
		EventRaidProductionData raidProductionData = EventManager.Instance.GetRaidProductionData();
		ServerEventUserRaidBossState raidBossState = EventManager.Instance.RaidBossState;
		if (systemdata == null || raidProductionData == null || raidBossState == null)
		{
			return result;
		}
		if (EventManager.Instance.UserRaidBossList == null)
		{
			return result;
		}
		int numRaidBossEncountered = raidBossState.NumRaidBossEncountered;
		int numBeatedEncounter = raidBossState.NumBeatedEncounter;
		if (type == RaidBossStartEvent.ProductType.MileagePage)
		{
			EventProductionData mileagePage = raidProductionData.mileagePage;
			if (mileagePage != null)
			{
				int pictureShowEmergeRaidBossProgress = systemdata.pictureShowEmergeRaidBossProgress;
				for (int i = 0; i < mileagePage.startCollectCount.Length; i++)
				{
					int num = mileagePage.startCollectCount[i];
					if (numRaidBossEncountered >= num && numBeatedEncounter >= num - 1 && i > pictureShowEmergeRaidBossProgress)
					{
						return i;
					}
				}
			}
		}
		else if (type == RaidBossStartEvent.ProductType.EventTopPage)
		{
			if (systemdata.pictureShowRaidBossFirstBattle == 1)
			{
				firstBattle = true;
				return 0;
			}
			EventProductionData eventTop = raidProductionData.eventTop;
			if (eventTop != null)
			{
				int pictureShowProgress = systemdata.pictureShowProgress;
				for (int j = 0; j < eventTop.startCollectCount.Length; j++)
				{
					int num2 = eventTop.startCollectCount[j];
					if (numRaidBossEncountered >= num2 && numBeatedEncounter >= num2 - 1 && j > pictureShowProgress)
					{
						return j;
					}
				}
			}
		}
		return result;
	}

	public void SavePictureEvent()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				RaidBossStartEvent.ProductType productType = this.m_productType;
				if (productType != RaidBossStartEvent.ProductType.MileagePage)
				{
					if (productType == RaidBossStartEvent.ProductType.EventTopPage)
					{
						if (this.m_firstBattle)
						{
							systemdata.pictureShowEventId = EventManager.Instance.Id;
							systemdata.pictureShowRaidBossFirstBattle = 0;
						}
						else
						{
							systemdata.pictureShowEventId = EventManager.Instance.Id;
							systemdata.pictureShowProgress = this.m_productProgress;
						}
					}
				}
				else
				{
					systemdata.pictureShowEventId = EventManager.Instance.Id;
					systemdata.pictureShowEmergeRaidBossProgress = this.m_productProgress;
				}
			}
			instance.SaveSystemData();
		}
	}

	public void Update()
	{
		switch (this.m_mode)
		{
		case RaidBossStartEvent.Mode.Idle:
			base.enabled = false;
			break;
		case RaidBossStartEvent.Mode.CheckId:
			if (this.m_productProgress == -1)
			{
				this.m_mode = RaidBossStartEvent.Mode.End;
			}
			else
			{
				this.m_textWindowId = this.GetTextWindowId();
				if (!this.m_commonResourceLoaded)
				{
					GameObjectUtil.SendMessageFindGameObject("MainMenuButtonEvent", "OnMenuEventClicked", base.gameObject, SendMessageOptions.DontRequireReceiver);
				}
				this.m_mode = RaidBossStartEvent.Mode.WaitCommonResource;
			}
			break;
		case RaidBossStartEvent.Mode.WaitCommonResource:
			if (this.m_commonResourceLoaded)
			{
				if (this.LoadTexture())
				{
					this.m_mode = RaidBossStartEvent.Mode.WaitLoad;
				}
				else
				{
					this.m_mode = RaidBossStartEvent.Mode.Start;
				}
			}
			break;
		case RaidBossStartEvent.Mode.Load:
			this.SetColision(true);
			this.m_productProgress = this.GetNextWindowProductIndex(this.m_productType, ref this.m_firstBattle);
			this.m_mode = RaidBossStartEvent.Mode.CheckId;
			break;
		case RaidBossStartEvent.Mode.WaitLoad:
			if (this.m_sceneLoader != null && this.m_sceneLoader.Loaded)
			{
				UnityEngine.Object.Destroy(this.m_sceneLoader.gameObject);
				this.m_mode = RaidBossStartEvent.Mode.Start;
			}
			break;
		case RaidBossStartEvent.Mode.Start:
			if (!RaidBossWindow.IsOpenAdvent())
			{
				if (this.StartEvent())
				{
					this.m_mode = RaidBossStartEvent.Mode.WaitEnd;
				}
				else
				{
					this.m_mode = RaidBossStartEvent.Mode.End;
				}
			}
			break;
		case RaidBossStartEvent.Mode.WaitEnd:
			if (GeneralWindow.IsCreated("RaidBossStartEvent") && GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				this.DestroyTextureData();
				this.SavePictureEvent();
				this.m_firstBattle = false;
				this.m_productProgress = this.GetNextWindowProductIndex(this.m_productType, ref this.m_firstBattle);
				this.m_mode = RaidBossStartEvent.Mode.CheckId;
			}
			break;
		case RaidBossStartEvent.Mode.Next:
			this.m_mode = RaidBossStartEvent.Mode.Load;
			break;
		case RaidBossStartEvent.Mode.End:
			this.m_isEnd = true;
			this.m_mode = RaidBossStartEvent.Mode.Idle;
			break;
		}
	}

	private void ResetParam(RaidBossStartEvent.ProductType type)
	{
		this.m_mode = RaidBossStartEvent.Mode.Load;
		this.m_productType = type;
		this.SetColision(true);
		if (type == RaidBossStartEvent.ProductType.EventTopPage)
		{
			this.m_commonResourceLoaded = true;
		}
		this.m_isEnd = false;
		base.enabled = true;
	}

	private void SetColision(bool flag)
	{
		if (flag)
		{
			if (!this.m_alertFlag)
			{
				HudMenuUtility.SetConnectAlertMenuButtonUI(true);
			}
		}
		else if (this.m_alertFlag)
		{
			HudMenuUtility.SetConnectAlertMenuButtonUI(false);
		}
		this.m_alertFlag = flag;
	}

	public void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (!this.m_isEnd && msg != null)
		{
			msg.StaySequence();
		}
	}

	public void OnButtonEventCallBack()
	{
		this.m_commonResourceLoaded = true;
	}

	public static List<int> GetProductFaceIdList(bool mileageProduct)
	{
		if (SystemSaveManager.Instance == null || EventManager.Instance == null)
		{
			return null;
		}
		SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
		EventRaidProductionData raidProductionData = EventManager.Instance.GetRaidProductionData();
		ServerEventUserRaidBossState raidBossState = EventManager.Instance.RaidBossState;
		if (systemdata == null || raidProductionData == null || raidBossState == null)
		{
			return null;
		}
		if (EventManager.Instance.UserRaidBossList == null)
		{
			return null;
		}
		List<int> list = new List<int>();
		int numRaidBossEncountered = raidBossState.NumRaidBossEncountered;
		int numBeatedEncounter = raidBossState.NumBeatedEncounter;
		if (mileageProduct)
		{
			EventProductionData mileagePage = raidProductionData.mileagePage;
			if (mileagePage != null)
			{
				int pictureShowEmergeRaidBossProgress = systemdata.pictureShowEmergeRaidBossProgress;
				for (int i = 0; i < mileagePage.startCollectCount.Length; i++)
				{
					int num = mileagePage.startCollectCount[i];
					if (numRaidBossEncountered >= num && numBeatedEncounter >= num - 1 && i > pictureShowEmergeRaidBossProgress)
					{
						list.Add(mileagePage.textWindowId[i]);
					}
				}
			}
		}
		EventProductionData firstBattle = raidProductionData.firstBattle;
		if (firstBattle != null && systemdata.pictureShowRaidBossFirstBattle == 1)
		{
			list.Add(firstBattle.textWindowId[0]);
		}
		EventProductionData eventTop = raidProductionData.eventTop;
		int pictureShowProgress = systemdata.pictureShowProgress;
		for (int j = 0; j < eventTop.startCollectCount.Length; j++)
		{
			int num2 = eventTop.startCollectCount[j];
			if (numRaidBossEncountered >= num2 && numBeatedEncounter >= num2 - 1 && j > pictureShowProgress)
			{
				list.Add(eventTop.textWindowId[j]);
			}
		}
		List<int> list2 = new List<int>();
		if (list.Count > 0 && EventManager.Instance != null)
		{
			foreach (int current in list)
			{
				WindowEventData windowEvenData = EventManager.Instance.GetWindowEvenData(current);
				if (windowEvenData != null)
				{
					WindowBodyData[] body = windowEvenData.body;
					for (int k = 0; k < body.Length; k++)
					{
						WindowBodyData windowBodyData = body[k];
						for (int l = 0; l < windowBodyData.face_count; l++)
						{
							if (l < windowBodyData.product.Length && !list2.Contains(windowBodyData.product[l].face_id))
							{
								list2.Add(windowBodyData.product[l].face_id);
							}
						}
					}
				}
			}
		}
		return list2;
	}
}
