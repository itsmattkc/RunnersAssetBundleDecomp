using SaveData;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class SpecialStageStartEvent : MonoBehaviour
{
	private enum Mode
	{
		Idle,
		Load,
		WaitLoad,
		Start,
		WaitEnd,
		NextPicture,
		DailyMission,
		End
	}

	private SpecialStageStartEvent.Mode m_mode;

	private ResourceSceneLoader m_sceneLoader;

	private int m_textWindowId = -1;

	private List<string> m_loadTexList = new List<string>();

	private WindowEventData m_windowEventData;

	private DailyWindowUI m_dailyWindowUI;

	private bool m_isNotPlaybackDefaultBgm;

	private bool m_isEnd;

	private bool m_alertFlag;

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
		if (this.m_dailyWindowUI != null)
		{
			this.m_dailyWindowUI = null;
		}
	}

	public static SpecialStageStartEvent Create(GameObject obj)
	{
		if (obj != null)
		{
			SpecialStageStartEvent specialStageStartEvent = obj.GetComponent<SpecialStageStartEvent>();
			if (specialStageStartEvent == null)
			{
				specialStageStartEvent = obj.AddComponent<SpecialStageStartEvent>();
			}
			else if (GeneralWindow.IsCreated("SpecialStageStartEvent"))
			{
				GeneralWindow.Close();
			}
			if (specialStageStartEvent != null)
			{
				specialStageStartEvent.m_mode = SpecialStageStartEvent.Mode.Load;
				specialStageStartEvent.SetColision(true);
			}
			return specialStageStartEvent;
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
				name = "SpecialStageStartEvent",
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

	private void SetTextureData()
	{
		GameObject gameObject = new GameObject("FaceTextures");
		if (gameObject != null)
		{
			if (gameObject != null)
			{
				gameObject.transform.parent = base.transform;
			}
			foreach (string current in this.m_loadTexList)
			{
				GameObject gameObject2 = GameObject.Find(current);
				if (gameObject2 != null)
				{
					gameObject2.transform.parent = gameObject.transform;
				}
			}
		}
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

	private string GetResourceName()
	{
		return "EventResourcePictureCardShowTextures" + this.m_textWindowId.ToString("D2");
	}

	public void SavePictureEvent()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				systemdata.pictureShowEventId = EventManager.Instance.Id;
				systemdata.pictureShowProgress = this.m_textWindowId;
			}
			instance.SaveSystemData();
		}
	}

	public void StartPlayDailyMissionResult()
	{
		GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
		if (menuAnimUIObject != null)
		{
			this.m_dailyWindowUI = GameObjectUtil.FindChildGameObjectComponent<DailyWindowUI>(menuAnimUIObject, "DailyWindowUI");
			if (this.m_dailyWindowUI != null)
			{
				this.m_dailyWindowUI.gameObject.SetActive(true);
				this.m_dailyWindowUI.PlayStart();
			}
		}
	}

	private int GetNextWindowId()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null && EventManager.Instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			EventProductionData puductionData = EventManager.Instance.GetPuductionData();
			if (systemdata != null && puductionData != null)
			{
				int pictureShowProgress = systemdata.pictureShowProgress;
				for (int i = 0; i < puductionData.startCollectCount.Length; i++)
				{
					int num = puductionData.startCollectCount[i];
					if (EventManager.Instance.CollectCount >= (long)num && i > pictureShowProgress)
					{
						return i;
					}
				}
			}
		}
		return -1;
	}

	public void Update()
	{
		switch (this.m_mode)
		{
		case SpecialStageStartEvent.Mode.Idle:
			base.enabled = false;
			break;
		case SpecialStageStartEvent.Mode.Load:
			this.SetColision(true);
			this.m_textWindowId = this.GetNextWindowId();
			if (this.m_textWindowId == -1)
			{
				if (this.IsDailyMissionComplete())
				{
					this.StartPlayDailyMissionResult();
					this.m_mode = SpecialStageStartEvent.Mode.DailyMission;
				}
				else
				{
					this.m_mode = SpecialStageStartEvent.Mode.End;
				}
			}
			else if (this.LoadTexture())
			{
				this.m_mode = SpecialStageStartEvent.Mode.WaitLoad;
			}
			else
			{
				this.m_mode = SpecialStageStartEvent.Mode.Start;
			}
			break;
		case SpecialStageStartEvent.Mode.WaitLoad:
			if (this.m_sceneLoader != null && this.m_sceneLoader.Loaded)
			{
				this.SetTextureData();
				UnityEngine.Object.Destroy(this.m_sceneLoader.gameObject);
				this.m_mode = SpecialStageStartEvent.Mode.Start;
			}
			break;
		case SpecialStageStartEvent.Mode.Start:
			if (this.StartEvent())
			{
				this.m_mode = SpecialStageStartEvent.Mode.WaitEnd;
			}
			else
			{
				this.m_mode = SpecialStageStartEvent.Mode.End;
			}
			break;
		case SpecialStageStartEvent.Mode.WaitEnd:
			if (GeneralWindow.IsCreated("SpecialStageStartEvent") && GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				this.DestroyTextureData();
				this.SavePictureEvent();
				if (SpecialStageStartEvent.IsPictureEvent())
				{
					this.m_mode = SpecialStageStartEvent.Mode.NextPicture;
				}
				else if (this.IsDailyMissionComplete())
				{
					this.StartPlayDailyMissionResult();
					this.m_mode = SpecialStageStartEvent.Mode.DailyMission;
				}
				else
				{
					this.m_mode = SpecialStageStartEvent.Mode.End;
				}
			}
			break;
		case SpecialStageStartEvent.Mode.NextPicture:
			this.m_mode = SpecialStageStartEvent.Mode.Load;
			break;
		case SpecialStageStartEvent.Mode.DailyMission:
			if (this.m_dailyWindowUI != null && this.m_dailyWindowUI.IsEnd)
			{
				this.m_dailyWindowUI = null;
				this.m_mode = SpecialStageStartEvent.Mode.End;
			}
			break;
		case SpecialStageStartEvent.Mode.End:
			this.m_isEnd = true;
			this.m_mode = SpecialStageStartEvent.Mode.Idle;
			break;
		}
	}

	private void SetColision(bool flag)
	{
		if (flag)
		{
			if (!this.m_alertFlag)
			{
				HudMenuUtility.SetConnectAlertSimpleUI(true);
			}
		}
		else if (this.m_alertFlag)
		{
			HudMenuUtility.SetConnectAlertSimpleUI(false);
		}
		this.m_alertFlag = flag;
	}

	private bool IsDailyMissionComplete()
	{
		return false;
	}

	public void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (!this.m_isEnd && msg != null)
		{
			msg.StaySequence();
		}
	}

	public static bool IsPictureEvent()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null && EventManager.Instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				if (systemdata.pictureShowEventId != EventManager.Instance.Id)
				{
					return true;
				}
				int pictureShowProgress = systemdata.pictureShowProgress;
				EventProductionData puductionData = EventManager.Instance.GetPuductionData();
				if (puductionData != null)
				{
					int num = 0;
					int[] startCollectCount = puductionData.startCollectCount;
					for (int i = 0; i < startCollectCount.Length; i++)
					{
						int num2 = startCollectCount[i];
						if (EventManager.Instance.CollectCount >= (long)num2 && num > pictureShowProgress)
						{
							return true;
						}
						num++;
					}
				}
			}
		}
		return false;
	}

	public static List<int> GetProductFaceIdList()
	{
		List<int> list = new List<int>();
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null && EventManager.Instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			EventProductionData puductionData = EventManager.Instance.GetPuductionData();
			if (systemdata != null && puductionData != null)
			{
				int pictureShowProgress = systemdata.pictureShowProgress;
				for (int i = 0; i < puductionData.startCollectCount.Length; i++)
				{
					int num = puductionData.startCollectCount[i];
					if (EventManager.Instance.CollectCount >= (long)num && i > pictureShowProgress)
					{
						list.Add(i);
					}
				}
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
					for (int j = 0; j < body.Length; j++)
					{
						WindowBodyData windowBodyData = body[j];
						for (int k = 0; k < windowBodyData.face_count; k++)
						{
							if (k < windowBodyData.product.Length && !list2.Contains(windowBodyData.product[k].face_id))
							{
								list2.Add(windowBodyData.product[k].face_id);
							}
						}
					}
				}
			}
		}
		return list2;
	}
}
