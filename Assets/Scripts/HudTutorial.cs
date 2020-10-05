using Message;
using SaveData;
using System;
using Text;
using Tutorial;
using UnityEngine;

public class HudTutorial : MonoBehaviour
{
	private enum ExplanPatternType
	{
		TEXT_ONLY,
		IMAGE_ONLY,
		TEXT_AND_IMAGE,
		COUNT
	}

	[Serializable]
	private class ExplanPattern
	{
		[SerializeField]
		public GameObject m_gameObject;

		[SerializeField]
		public UILabel m_label;

		[SerializeField]
		public UITexture m_texture;
	}

	public enum Id
	{
		NONE = -1,
		MISSION_1,
		MISSION_2,
		MISSION_3,
		MISSION_4,
		MISSION_5,
		MISSION_6,
		MISSION_7,
		MISSION_8,
		MISSION_END,
		FEVERBOSS,
		MAPBOSS_1,
		MAPBOSS_2,
		MAPBOSS_3,
		EVENTBOSS_1,
		EVENTBOSS_2,
		ITEM_1,
		ITEM_2,
		ITEM_3,
		ITEM_4,
		ITEM_5,
		ITEM_6,
		ITEM_7,
		ITEM_8,
		CHARA_0,
		CHARA_1,
		CHARA_2,
		CHARA_3,
		CHARA_4,
		CHARA_5,
		CHARA_6,
		CHARA_7,
		CHARA_8,
		CHARA_9,
		CHARA_10,
		CHARA_11,
		CHARA_12,
		CHARA_13,
		CHARA_14,
		CHARA_15,
		CHARA_16,
		CHARA_17,
		CHARA_18,
		CHARA_19,
		CHARA_20,
		CHARA_21,
		CHARA_22,
		CHARA_23,
		CHARA_24,
		CHARA_25,
		CHARA_26,
		CHARA_27,
		CHARA_28,
		ACTION_1,
		ITEM_BUTTON_1,
		QUICK_1,
		NUM
	}

	public enum Kind
	{
		NONE = -1,
		MISSION,
		MISSION_END,
		FEVERBOSS,
		MAPBOSS,
		EVENTBOSS,
		ITEM,
		ITEM_BUTTON,
		CHARA,
		ACTION,
		QUICK
	}

	private enum Phase
	{
		NONE = -1,
		OPEN,
		OPEN_WAIT,
		WAIT,
		CLOSE_WAIT,
		CLOSE,
		PLAY,
		SUCCESS,
		RETRY,
		RESULT,
		END
	}

	public class TutorialData
	{
		public EventID m_eventID;

		public int m_textureCount;

		public int m_textureNumber1;

		public int m_textureNumber2;

		public TutorialData(EventID eventID, int count, int num1, int num2)
		{
			this.m_eventID = eventID;
			this.m_textureCount = count;
			this.m_textureNumber1 = num1;
			this.m_textureNumber2 = num2;
		}
	}

	public class ActionTutorialData
	{
		public SystemData.ActionTutorialFlagStatus m_flagStatus;

		public string m_textCategory;

		public string m_textCell;

		public ActionTutorialData(SystemData.ActionTutorialFlagStatus flagStatus, string textCategory, string textCell)
		{
			this.m_flagStatus = flagStatus;
			this.m_textCategory = textCategory;
			this.m_textCell = textCell;
		}
	}

	public class QuickModeTutorialData
	{
		public SystemData.QuickModeTutorialFlagStatus m_flagStatus;

		public QuickModeTutorialData(SystemData.QuickModeTutorialFlagStatus flagStatus)
		{
			this.m_flagStatus = flagStatus;
		}
	}

	[SerializeField]
	private GameObject m_captionGameObject;

	[SerializeField]
	private UILabel m_captionLabel;

	[SerializeField]
	private GameObject m_explanGameObject;

	[SerializeField]
	private HudTutorial.ExplanPattern[] m_explanPatterns = new HudTutorial.ExplanPattern[3];

	[SerializeField]
	private GameObject m_successGameObject;

	[SerializeField]
	private GameObject m_retryGameObject;

	[SerializeField]
	private GameObject m_anchorObj;

	private static readonly HudTutorial.TutorialData[] TUTORIAL_DATA_TBL = new HudTutorial.TutorialData[]
	{
		new HudTutorial.TutorialData(EventID.JUMP, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.DOUBLE_JUMP, 1, 1, 0),
		new HudTutorial.TutorialData(EventID.RING_BONUS, 2, 2, 8),
		new HudTutorial.TutorialData(EventID.ENEMY, 1, 3, 0),
		new HudTutorial.TutorialData(EventID.DAMAGE, 2, 4, 9),
		new HudTutorial.TutorialData(EventID.MISS, 1, 5, 0),
		new HudTutorial.TutorialData(EventID.PARA_LOOP, 1, 6, 0),
		new HudTutorial.TutorialData(EventID.FEVER_BOSS, 1, 7, 0),
		new HudTutorial.TutorialData(EventID.COMPLETE, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 1),
		new HudTutorial.TutorialData(EventID.NUM, 2, 0, 1),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 1, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 2, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 3, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 4, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 5, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 6, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 7, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 0, 0, 0),
		new HudTutorial.TutorialData(EventID.NUM, 1, 0, 0)
	};

	private static readonly HudTutorial.ActionTutorialData[] ACTION_TUTORIAL_DATA_TBL = new HudTutorial.ActionTutorialData[]
	{
		new HudTutorial.ActionTutorialData(SystemData.ActionTutorialFlagStatus.ACTION_1, "Item", "crystal_2")
	};

	private static readonly HudTutorial.QuickModeTutorialData[] QUICK_MODE_TUTORIAL_DATA_TBL = new HudTutorial.QuickModeTutorialData[]
	{
		new HudTutorial.QuickModeTutorialData(SystemData.QuickModeTutorialFlagStatus.QUICK_1)
	};

	private static string MISSION_SCENENAME = "ui_tex_tutorial_mission";

	private static string FEVERBOSS_SCENENAME = "ui_tex_tutorial_feverboss";

	private static string MAPBOSS_SCENENAME = "ui_tex_tutorial_mapboss_";

	private static string EVENTBOSS_SCENENAME = "ui_tex_tutorial_eventboss_";

	private static string ITEM_SCENENAME = "ui_tex_tutorial_item";

	private static string CHARA_SCENENAME = "ui_tex_tutorial_chara_";

	private static string ACTION_SCENENAME = "ui_tex_tutorial_action_";

	private static string QUICK_SCENENAME = "ui_tex_tutorial_quick_";

	private HudTutorial.ExplanPattern m_explanPattern;

	private HudTutorial.Id m_id;

	private HudTutorial.Phase m_phase;

	private int m_textureCount;

	private int m_currentTextureIndex;

	private float m_timer;

	private BossType m_bossType;

	private HudTutorial.Kind kind
	{
		get
		{
			return HudTutorial.GetKind(this.m_id);
		}
	}

	public static HudTutorial.Kind GetKind(HudTutorial.Id id)
	{
		if (id == HudTutorial.Id.NONE)
		{
			return HudTutorial.Kind.NONE;
		}
		if (id < HudTutorial.Id.MISSION_END)
		{
			return HudTutorial.Kind.MISSION;
		}
		if (id == HudTutorial.Id.MISSION_END)
		{
			return HudTutorial.Kind.MISSION_END;
		}
		if (id < HudTutorial.Id.MAPBOSS_1)
		{
			return HudTutorial.Kind.FEVERBOSS;
		}
		if (id < HudTutorial.Id.EVENTBOSS_1)
		{
			return HudTutorial.Kind.MAPBOSS;
		}
		if (id < HudTutorial.Id.ITEM_1)
		{
			return HudTutorial.Kind.EVENTBOSS;
		}
		if (id < HudTutorial.Id.CHARA_0)
		{
			return HudTutorial.Kind.ITEM;
		}
		if (id < HudTutorial.Id.ACTION_1)
		{
			return HudTutorial.Kind.CHARA;
		}
		if (id < HudTutorial.Id.ITEM_BUTTON_1)
		{
			return HudTutorial.Kind.ACTION;
		}
		if (id < HudTutorial.Id.QUICK_1)
		{
			return HudTutorial.Kind.ITEM_BUTTON;
		}
		return HudTutorial.Kind.QUICK;
	}

	private void Start()
	{
		this.Initialize();
	}

	private void SetTexture(string sceneName, int index)
	{
		this.m_explanPattern.m_texture.mainTexture = null;
		GameObject gameObject = GameObject.Find(sceneName);
		if (gameObject == null)
		{
			GameObject gameObject2 = GameObject.Find("EventResourceCommon");
			if (gameObject2 != null)
			{
				for (int i = 0; i < gameObject2.transform.childCount; i++)
				{
					GameObject gameObject3 = gameObject2.transform.GetChild(i).gameObject;
					if (gameObject3.name == sceneName)
					{
						gameObject = gameObject3;
						break;
					}
				}
			}
		}
		if (gameObject != null)
		{
			HudTutorialTexture component = gameObject.GetComponent<HudTutorialTexture>();
			if (component != null && index < component.m_texList.Length)
			{
				this.m_explanPattern.m_texture.mainTexture = component.m_texList[index];
			}
		}
		this.m_explanPattern.m_texture.enabled = true;
	}

	private void SetTexture(int index)
	{
		HudTutorial.TutorialData tutorialData = HudTutorial.GetTutorialData(this.m_id);
		if (tutorialData == null)
		{
			return;
		}
		int index2;
		if (index != 0)
		{
			if (index != 1)
			{
				return;
			}
			index2 = tutorialData.m_textureNumber2;
		}
		else
		{
			index2 = tutorialData.m_textureNumber1;
		}
		switch (this.kind)
		{
		case HudTutorial.Kind.MISSION:
			this.SetTexture(HudTutorial.MISSION_SCENENAME, index2);
			break;
		case HudTutorial.Kind.FEVERBOSS:
			this.SetTexture(HudTutorial.FEVERBOSS_SCENENAME, index2);
			break;
		case HudTutorial.Kind.MAPBOSS:
			this.SetTexture(HudTutorial.MAPBOSS_SCENENAME + (this.m_id - HudTutorial.Id.MAPBOSS_1).ToString(), index2);
			break;
		case HudTutorial.Kind.EVENTBOSS:
			this.SetTexture(HudTutorial.EVENTBOSS_SCENENAME + (this.m_id - HudTutorial.Id.EVENTBOSS_1).ToString(), index2);
			break;
		case HudTutorial.Kind.ITEM:
			this.SetTexture(HudTutorial.ITEM_SCENENAME, index2);
			break;
		case HudTutorial.Kind.CHARA:
			this.SetTexture(HudTutorial.CHARA_SCENENAME + (this.m_id - HudTutorial.Id.CHARA_0).ToString(), index2);
			break;
		case HudTutorial.Kind.ACTION:
			this.SetTexture(HudTutorial.ACTION_SCENENAME + (this.m_id - HudTutorial.Id.ACTION_1 + 1).ToString(), index2);
			break;
		case HudTutorial.Kind.QUICK:
			this.SetTexture(HudTutorial.QUICK_SCENENAME + (this.m_id - HudTutorial.Id.QUICK_1 + 1).ToString(), index2);
			break;
		}
	}

	private void Update()
	{
		if (this.m_id != HudTutorial.Id.NONE)
		{
			HudTutorial.Phase phase = this.m_phase;
			switch (phase + 1)
			{
			case HudTutorial.Phase.OPEN:
				switch (this.kind)
				{
				case HudTutorial.Kind.MISSION:
				case HudTutorial.Kind.FEVERBOSS:
				case HudTutorial.Kind.MAPBOSS:
				case HudTutorial.Kind.ITEM:
				case HudTutorial.Kind.CHARA:
				case HudTutorial.Kind.ACTION:
				case HudTutorial.Kind.QUICK:
					this.m_captionGameObject.SetActive(true);
					this.m_captionLabel.text = HudTutorial.GetCaptionText(this.m_id, 0);
					this.m_explanGameObject.SetActive(true);
					this.m_explanPattern = this.m_explanPatterns[2];
					this.m_explanPattern.m_gameObject.SetActive(true);
					this.m_explanPattern.m_label.text = HudTutorial.GetExplainText(this.m_id, 0);
					this.SetTexture(0);
					break;
				case HudTutorial.Kind.MISSION_END:
					this.m_explanGameObject.SetActive(true);
					this.m_explanPattern = this.m_explanPatterns[0];
					this.m_explanPattern.m_gameObject.SetActive(true);
					this.m_explanPattern.m_label.text = HudTutorial.GetExplainText(this.m_id, 0);
					break;
				case HudTutorial.Kind.EVENTBOSS:
					this.m_captionGameObject.SetActive(true);
					this.m_captionLabel.text = HudTutorial.GetEventBossCaptionText(this.m_id, this.m_bossType, 0);
					this.m_explanGameObject.SetActive(true);
					this.m_explanPattern = this.m_explanPatterns[2];
					this.m_explanPattern.m_gameObject.SetActive(true);
					this.m_explanPattern.m_label.text = HudTutorial.GetEventBossExplainText(this.m_id, this.m_bossType, 0);
					this.SetTexture(0);
					break;
				}
				if (this.kind == HudTutorial.Kind.ITEM_BUTTON)
				{
					this.m_phase = HudTutorial.Phase.CLOSE;
				}
				else
				{
					this.m_textureCount = HudTutorial.GetTexuterPageCount(this.m_id);
					this.m_currentTextureIndex = 0;
					this.m_timer = 1f;
					this.m_phase = HudTutorial.Phase.OPEN;
				}
				break;
			case HudTutorial.Phase.OPEN_WAIT:
				if (this.m_anchorObj != null)
				{
					this.m_anchorObj.SetActive(true);
				}
				this.m_phase = HudTutorial.Phase.OPEN_WAIT;
				break;
			case HudTutorial.Phase.WAIT:
				this.m_timer -= RealTime.deltaTime;
				if (this.m_timer <= 0f)
				{
					this.m_phase = HudTutorial.Phase.WAIT;
				}
				break;
			case HudTutorial.Phase.CLOSE:
				this.m_timer -= RealTime.deltaTime;
				if (this.m_timer <= 0f)
				{
					this.m_phase = HudTutorial.Phase.CLOSE;
				}
				break;
			case HudTutorial.Phase.PLAY:
				this.m_phase = ((this.kind != HudTutorial.Kind.MISSION) ? HudTutorial.Phase.END : HudTutorial.Phase.PLAY);
				if (this.kind == HudTutorial.Kind.ITEM_BUTTON)
				{
					GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnNextTutorial", null, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					HudTutorial.TutorialData tutorialData = HudTutorial.GetTutorialData(this.m_id);
					if (tutorialData != null)
					{
						MsgTutorialPlayAction value = new MsgTutorialPlayAction(tutorialData.m_eventID);
						GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnMsgTutorialPlayAction", value, SendMessageOptions.DontRequireReceiver);
					}
				}
				break;
			case HudTutorial.Phase.RETRY:
				if (this.m_id != HudTutorial.Id.MISSION_8)
				{
					SoundManager.SePlay("sys_clear", "SE");
					this.m_successGameObject.SetActive(true);
				}
				this.m_phase = HudTutorial.Phase.RESULT;
				break;
			case HudTutorial.Phase.RESULT:
				if (this.m_id != HudTutorial.Id.MISSION_8)
				{
					SoundManager.SePlay("sys_retry", "SE");
					this.m_retryGameObject.SetActive(true);
				}
				this.m_phase = HudTutorial.Phase.RESULT;
				break;
			case (HudTutorial.Phase)10:
				if (this.m_anchorObj != null)
				{
					this.m_anchorObj.SetActive(false);
				}
				this.m_phase = HudTutorial.Phase.NONE;
				this.m_id = HudTutorial.Id.NONE;
				break;
			}
		}
	}

	private void Initialize()
	{
		this.m_id = HudTutorial.Id.NONE;
		this.m_phase = HudTutorial.Phase.NONE;
		this.m_captionGameObject.SetActive(false);
		this.m_explanGameObject.SetActive(false);
		HudTutorial.ExplanPattern[] explanPatterns = this.m_explanPatterns;
		for (int i = 0; i < explanPatterns.Length; i++)
		{
			HudTutorial.ExplanPattern explanPattern = explanPatterns[i];
			explanPattern.m_gameObject.SetActive(false);
			if (explanPattern.m_label != null)
			{
				explanPattern.m_label.text = null;
			}
			if (explanPattern.m_texture != null)
			{
				explanPattern.m_texture.enabled = false;
			}
		}
		this.m_successGameObject.SetActive(false);
		this.m_retryGameObject.SetActive(false);
		if (this.m_anchorObj != null)
		{
			this.m_anchorObj.SetActive(false);
		}
	}

	private void OnClickScreen()
	{
		if (this.m_phase == HudTutorial.Phase.WAIT)
		{
			bool flag = false;
			if (this.m_textureCount > 1 && this.m_currentTextureIndex < this.m_textureCount - 1)
			{
				flag = true;
			}
			if (flag)
			{
				this.m_currentTextureIndex++;
				switch (this.kind)
				{
				case HudTutorial.Kind.MISSION:
					this.m_explanPattern.m_label.text = HudTutorial.GetExplainText(this.m_id, this.m_currentTextureIndex);
					break;
				case HudTutorial.Kind.MAPBOSS:
					this.m_explanPattern.m_label.text = HudTutorial.GetExplainText(this.m_id, this.m_currentTextureIndex);
					break;
				case HudTutorial.Kind.EVENTBOSS:
					this.m_explanPattern.m_label.text = HudTutorial.GetEventBossExplainText(this.m_id, this.m_bossType, this.m_currentTextureIndex);
					break;
				}
				this.SetTexture(this.m_currentTextureIndex);
				this.m_timer = 0.5f;
				this.m_phase = HudTutorial.Phase.OPEN_WAIT;
			}
			else
			{
				this.OnClose();
				if (this.kind == HudTutorial.Kind.ITEM || this.kind == HudTutorial.Kind.CHARA || this.kind == HudTutorial.Kind.ACTION || this.kind == HudTutorial.Kind.ITEM_BUTTON || this.kind == HudTutorial.Kind.QUICK)
				{
					this.m_timer = 0.3f;
				}
				else
				{
					this.m_timer = 0f;
				}
				this.m_phase = HudTutorial.Phase.CLOSE_WAIT;
			}
		}
	}

	private void OnClose()
	{
		switch (this.kind)
		{
		case HudTutorial.Kind.MISSION:
			if (this.m_id == HudTutorial.Id.MISSION_8)
			{
				this.m_captionGameObject.SetActive(false);
			}
			break;
		case HudTutorial.Kind.MAPBOSS:
		case HudTutorial.Kind.EVENTBOSS:
		case HudTutorial.Kind.ITEM:
		case HudTutorial.Kind.ITEM_BUTTON:
		case HudTutorial.Kind.CHARA:
		case HudTutorial.Kind.ACTION:
		case HudTutorial.Kind.QUICK:
			this.m_captionGameObject.SetActive(false);
			break;
		}
		this.m_explanGameObject.SetActive(false);
		this.m_explanPattern.m_gameObject.SetActive(false);
	}

	private void OnStartTutorial(MsgTutorialHudStart msg)
	{
		this.Initialize();
		if (this.m_phase == HudTutorial.Phase.NONE)
		{
			this.m_id = msg.m_id;
			this.m_bossType = msg.m_bossType;
		}
	}

	private void OnSuccessTutorial()
	{
		if (this.m_phase == HudTutorial.Phase.PLAY)
		{
			this.m_phase = HudTutorial.Phase.SUCCESS;
		}
	}

	private void OnRetryTutorial()
	{
		if (this.m_phase == HudTutorial.Phase.PLAY)
		{
			this.m_phase = HudTutorial.Phase.RETRY;
		}
	}

	private void OnEndTutorial()
	{
		this.Initialize();
	}

	private void OnPushBackKey()
	{
		this.OnClickScreen();
	}

	private static void SetUIEffect(bool flag)
	{
		if (UIEffectManager.Instance != null)
		{
			UIEffectManager.Instance.SetActiveEffect(HudMenuUtility.EffectPriority.Menu, flag);
		}
	}

	public static void StartTutorial(HudTutorial.Id id, BossType bossType)
	{
		HudTutorial.SetUIEffect(false);
		GameObjectUtil.SendMessageFindGameObject("HudTutorial", "OnStartTutorial", new MsgTutorialHudStart(id, bossType), SendMessageOptions.DontRequireReceiver);
	}

	public static void SuccessTutorial()
	{
		GameObjectUtil.SendMessageFindGameObject("HudTutorial", "OnSuccessTutorial", null, SendMessageOptions.DontRequireReceiver);
	}

	public static void RetryTutorial()
	{
		GameObjectUtil.SendMessageFindGameObject("HudTutorial", "OnRetryTutorial", null, SendMessageOptions.DontRequireReceiver);
	}

	public static void EndTutorial()
	{
		HudTutorial.SetUIEffect(true);
		GameObjectUtil.SendMessageFindGameObject("HudTutorial", "OnEndTutorial", null, SendMessageOptions.DontRequireReceiver);
	}

	public static void PushBackKey()
	{
		GameObjectUtil.SendMessageFindGameObject("HudTutorial", "OnPushBackKey", null, SendMessageOptions.DontRequireReceiver);
	}

	public static void Load(ResourceSceneLoader loaderComponent, bool turorial, bool bossStage, BossType tutorialBossType, CharaType mainChara, CharaType subChara)
	{
		bool onAssetBundle = true;
		if (loaderComponent != null)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (turorial)
			{
				flag = true;
				flag3 = true;
				loaderComponent.AddLoad(HudTutorial.FEVERBOSS_SCENENAME, onAssetBundle, false);
			}
			else if (tutorialBossType == BossType.MAP1 || tutorialBossType == BossType.MAP2 || tutorialBossType == BossType.MAP3)
			{
				flag2 = true;
				flag4 = true;
			}
			else if (bossStage)
			{
				flag4 = true;
			}
			else
			{
				flag3 = true;
				flag4 = true;
				if (StageModeManager.Instance != null && StageModeManager.Instance.IsQuickMode())
				{
					for (int i = 0; i < HudTutorial.QUICK_MODE_TUTORIAL_DATA_TBL.Length; i++)
					{
						if (HudTutorial.IsQuickModeTutorial(HudTutorial.Id.QUICK_1 + i))
						{
							loaderComponent.AddLoad(HudTutorial.QUICK_SCENENAME + (i + 1).ToString(), onAssetBundle, false);
						}
					}
				}
				for (int j = 0; j < HudTutorial.ACTION_TUTORIAL_DATA_TBL.Length; j++)
				{
					if (HudTutorial.IsActionTutorial(HudTutorial.Id.ACTION_1 + j))
					{
						loaderComponent.AddLoad(HudTutorial.ACTION_SCENENAME + (j + 1).ToString(), onAssetBundle, false);
					}
				}
				if (tutorialBossType == BossType.FEVER)
				{
					loaderComponent.AddLoad(HudTutorial.FEVERBOSS_SCENENAME, onAssetBundle, false);
				}
			}
			if (flag)
			{
				loaderComponent.AddLoad(HudTutorial.MISSION_SCENENAME, onAssetBundle, false);
			}
			if (flag2)
			{
				int num = tutorialBossType - BossType.MAP1;
				loaderComponent.AddLoad(HudTutorial.MAPBOSS_SCENENAME + num, onAssetBundle, false);
			}
			if (flag3 && HudTutorial.IsItemTutorial())
			{
				loaderComponent.AddLoad(HudTutorial.ITEM_SCENENAME, onAssetBundle, false);
			}
			if (flag4)
			{
				if (HudTutorial.IsCharaTutorial(mainChara))
				{
					int charaTutorialIndex = HudTutorial.GetCharaTutorialIndex(mainChara);
					if (charaTutorialIndex >= 0)
					{
						loaderComponent.AddLoad(HudTutorial.CHARA_SCENENAME + charaTutorialIndex, onAssetBundle, false);
					}
				}
				if (HudTutorial.IsCharaTutorial(subChara))
				{
					int charaTutorialIndex2 = HudTutorial.GetCharaTutorialIndex(subChara);
					if (charaTutorialIndex2 >= 0)
					{
						loaderComponent.AddLoad(HudTutorial.CHARA_SCENENAME + charaTutorialIndex2, onAssetBundle, false);
					}
				}
			}
		}
	}

	public static HudTutorial.TutorialData GetTutorialData(HudTutorial.Id id)
	{
		if ((ulong)id < (ulong)((long)HudTutorial.TUTORIAL_DATA_TBL.Length))
		{
			return HudTutorial.TUTORIAL_DATA_TBL[(int)id];
		}
		return null;
	}

	public static string GetLoadSceneName(BossType bossType)
	{
		if (bossType != BossType.NONE)
		{
			int num = bossType - BossType.MAP1;
			return HudTutorial.MAPBOSS_SCENENAME + num;
		}
		return string.Empty;
	}

	public static string GetLoadSceneName(CharaType charaType)
	{
		if (charaType != CharaType.UNKNOWN)
		{
			int charaTutorialIndex = HudTutorial.GetCharaTutorialIndex(charaType);
			return HudTutorial.CHARA_SCENENAME + charaTutorialIndex;
		}
		return string.Empty;
	}

	public static string GetLoadQuickModeSceneName(HudTutorial.Id quickID)
	{
		int num = quickID - HudTutorial.Id.QUICK_1;
		if (0 <= num && num < HudTutorial.QUICK_MODE_TUTORIAL_DATA_TBL.Length)
		{
			return HudTutorial.QUICK_SCENENAME + (num + 1).ToString();
		}
		return string.Empty;
	}

	public static int GetTexuterPageCount(HudTutorial.Id id)
	{
		HudTutorial.TutorialData tutorialData = HudTutorial.GetTutorialData(id);
		if (tutorialData != null)
		{
			return tutorialData.m_textureCount;
		}
		return 0;
	}

	public static string GetCaptionText(HudTutorial.Id id, int page = 0)
	{
		string result = string.Empty;
		switch (HudTutorial.GetKind(id))
		{
		case HudTutorial.Kind.MISSION:
			result = TextUtility.GetCommonText("Tutorial", "caption" + (int)(id + 1));
			break;
		case HudTutorial.Kind.FEVERBOSS:
		{
			TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Tutorial", "caption_explan2_2");
			string commonText = TextUtility.GetCommonText("BossName", "ferver");
			text.ReplaceTag("{PARAM_NAME}", commonText);
			result = text.text;
			break;
		}
		case HudTutorial.Kind.MAPBOSS:
		{
			BossType type = BossType.MAP1;
			switch (id)
			{
			case HudTutorial.Id.MAPBOSS_1:
				type = BossType.MAP1;
				break;
			case HudTutorial.Id.MAPBOSS_2:
				type = BossType.MAP2;
				break;
			case HudTutorial.Id.MAPBOSS_3:
				type = BossType.MAP3;
				break;
			}
			TextObject text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Tutorial", "caption_boss");
			text2.ReplaceTag("{PARAM_NAME}", BossTypeUtil.GetTextCommonBossName(type));
			result = text2.text;
			break;
		}
		case HudTutorial.Kind.ITEM:
		{
			string itemTutorialCaptionText = HudTutorial.GetItemTutorialCaptionText(id);
			TextObject text3 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Tutorial", itemTutorialCaptionText);
			text3.ReplaceTag("{PARAM_NAME}", HudTutorial.GetItemTutorialText(id));
			result = text3.text;
			break;
		}
		case HudTutorial.Kind.ITEM_BUTTON:
		{
			TextObject text4 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Tutorial", "caption_explan3");
			result = text4.text;
			break;
		}
		case HudTutorial.Kind.CHARA:
		{
			CharaType commonTextCharaName = HudTutorial.GetCommonTextCharaName(id);
			string text5 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaName", CharaName.Name[(int)commonTextCharaName]).text;
			TextObject text6 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Tutorial", "caption_explan1");
			text6.ReplaceTag("{PARAM_NAME}", text5);
			result = text6.text;
			break;
		}
		case HudTutorial.Kind.ACTION:
		{
			TextObject text7 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Tutorial", "caption_explan2_2");
			text7.ReplaceTag("{PARAM_NAME}", HudTutorial.GetActionTutorialText(id));
			result = text7.text;
			break;
		}
		case HudTutorial.Kind.QUICK:
		{
			string commonText2 = TextUtility.GetCommonText("Tutorial", "caption_quickmode_tutorial");
			result = TextUtility.GetCommonText("Tutorial", "caption_explan2", "{PARAM_NAME}", commonText2);
			break;
		}
		}
		return result;
	}

	public static string GetEventBossCaptionText(HudTutorial.Id id, BossType bossType, int page = 0)
	{
		string result = string.Empty;
		HudTutorial.Kind kind = HudTutorial.GetKind(id);
		if (kind == HudTutorial.Kind.EVENTBOSS)
		{
			TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Tutorial", "caption_boss");
			text.ReplaceTag("{PARAM_NAME}", BossTypeUtil.GetTextCommonBossCharaName(bossType));
			result = text.text;
		}
		return result;
	}

	public static string GetExplainText(HudTutorial.Id id, int page = 0)
	{
		string result = string.Empty;
		switch (HudTutorial.GetKind(id))
		{
		case HudTutorial.Kind.MISSION:
			if (page == 0)
			{
				result = TextUtility.GetCommonText("Tutorial", "explan" + (int)(id + 1));
			}
			else
			{
				string arg = "_" + page.ToString();
				result = TextUtility.GetCommonText("Tutorial", "explan" + (int)(id + 1) + arg);
			}
			break;
		case HudTutorial.Kind.MISSION_END:
			result = TextUtility.GetCommonText("Tutorial", "end");
			break;
		case HudTutorial.Kind.FEVERBOSS:
			result = TextUtility.GetCommonText("Tutorial", "fever_boss");
			break;
		case HudTutorial.Kind.MAPBOSS:
			if (page == 0)
			{
				result = TextUtility.GetCommonText("Tutorial", "boss" + (id - HudTutorial.Id.MAPBOSS_1 + 1));
			}
			else
			{
				string arg2 = "_" + page.ToString();
				result = TextUtility.GetCommonText("Tutorial", "boss" + (id - HudTutorial.Id.MAPBOSS_1 + 1) + arg2);
			}
			break;
		case HudTutorial.Kind.ITEM:
			result = TextUtility.GetCommonText("Tutorial", "item_" + (id - HudTutorial.Id.ITEM_1 + 1));
			break;
		case HudTutorial.Kind.ITEM_BUTTON:
			result = TextUtility.GetCommonText("Tutorial", "item_btn");
			break;
		case HudTutorial.Kind.CHARA:
			result = TextUtility.GetCommonText("Tutorial", "chara" + (id - HudTutorial.Id.CHARA_1 + 1));
			break;
		case HudTutorial.Kind.ACTION:
			result = TextUtility.GetCommonText("Tutorial", "action" + (id - HudTutorial.Id.ACTION_1 + 1));
			break;
		case HudTutorial.Kind.QUICK:
			result = TextUtility.GetCommonText("Tutorial", "quick" + (id - HudTutorial.Id.QUICK_1 + 1));
			break;
		}
		return result;
	}

	public static string GetEventBossExplainText(HudTutorial.Id id, BossType bossType, int page = 0)
	{
		string result = string.Empty;
		HudTutorial.Kind kind = HudTutorial.GetKind(id);
		if (kind == HudTutorial.Kind.EVENTBOSS)
		{
			if (page == 0)
			{
				result = TextManager.GetText(TextManager.TextType.TEXTTYPE_EVENT_SPECIFIC, "Tutorial", "boss_" + (id - HudTutorial.Id.EVENTBOSS_1 + 1)).text;
			}
			else
			{
				string text = "_" + page.ToString();
				result = TextManager.GetText(TextManager.TextType.TEXTTYPE_EVENT_SPECIFIC, "Tutorial", string.Concat(new object[]
				{
					"boss_",
					id - HudTutorial.Id.EVENTBOSS_1 + 1,
					"_",
					text
				})).text;
			}
		}
		return result;
	}

	private static bool IsItemTutorial()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				for (int i = 0; i < 8; i++)
				{
					SystemData.ItemTutorialFlagStatus itemTutorialStatus = ItemTypeName.GetItemTutorialStatus((ItemType)i);
					if (itemTutorialStatus != SystemData.ItemTutorialFlagStatus.NONE && !systemdata.IsFlagStatus(itemTutorialStatus))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private static bool IsItemTutorial(ItemType type)
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				SystemData.ItemTutorialFlagStatus itemTutorialStatus = ItemTypeName.GetItemTutorialStatus(type);
				if (itemTutorialStatus != SystemData.ItemTutorialFlagStatus.NONE && !systemdata.IsFlagStatus(itemTutorialStatus))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsCharaTutorial(CharaType type)
	{
		if (type == CharaType.SONIC)
		{
			ServerMileageMapState mileageMapState = ServerInterface.MileageMapState;
			if (mileageMapState != null && mileageMapState.m_episode > 1)
			{
				return false;
			}
		}
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				SystemData.CharaTutorialFlagStatus characterSaveDataFlagStatus = CharaTypeUtil.GetCharacterSaveDataFlagStatus(type);
				if (characterSaveDataFlagStatus != SystemData.CharaTutorialFlagStatus.NONE && !systemdata.IsFlagStatus(characterSaveDataFlagStatus))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static int GetCharaTutorialIndex(CharaType type)
	{
		HudTutorial.Id characterTutorialID = CharaTypeUtil.GetCharacterTutorialID(type);
		if (characterTutorialID != HudTutorial.Id.NONE)
		{
			return characterTutorialID - HudTutorial.Id.CHARA_0;
		}
		return -1;
	}

	public static CharaType GetCommonTextCharaName(HudTutorial.Id in_id)
	{
		for (int i = 0; i < 29; i++)
		{
			HudTutorial.Id characterTutorialID = CharaTypeUtil.GetCharacterTutorialID((CharaType)i);
			if (characterTutorialID == in_id)
			{
				return (CharaType)i;
			}
		}
		return CharaType.UNKNOWN;
	}

	private static bool IsActionTutorial(HudTutorial.Id actionID)
	{
		if (HudTutorial.GetKind(actionID) == HudTutorial.Kind.ACTION)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					SystemData.ActionTutorialFlagStatus actionTutorialSaveFlag = HudTutorial.GetActionTutorialSaveFlag(actionID);
					if (actionTutorialSaveFlag != SystemData.ActionTutorialFlagStatus.NONE && !systemdata.IsFlagStatus(actionTutorialSaveFlag))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool IsQuickModeTutorial(HudTutorial.Id actionID)
	{
		if (HudTutorial.GetKind(actionID) == HudTutorial.Kind.QUICK)
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					SystemData.QuickModeTutorialFlagStatus quickModeTutorialSaveFlag = HudTutorial.GetQuickModeTutorialSaveFlag(actionID);
					if (quickModeTutorialSaveFlag != SystemData.QuickModeTutorialFlagStatus.NONE && !systemdata.IsFlagStatus(quickModeTutorialSaveFlag))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static void SendItemTutorial(ItemType itemType)
	{
		if (HudTutorial.IsItemTutorial(itemType))
		{
			HudTutorial.Id itemTutorialID = ItemTypeName.GetItemTutorialID(itemType);
			if (itemTutorialID != HudTutorial.Id.NONE)
			{
				GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnMsgTutorialItem", new MsgTutorialItem(itemTutorialID), SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public static void SendActionTutorial(HudTutorial.Id actionID)
	{
		if (HudTutorial.GetKind(actionID) == HudTutorial.Kind.ACTION && HudTutorial.IsActionTutorial(actionID))
		{
			GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnMsgTutorialAction", new MsgTutorialAction(actionID), SendMessageOptions.DontRequireReceiver);
		}
	}

	public static SystemData.ActionTutorialFlagStatus GetActionTutorialSaveFlag(HudTutorial.Id actionID)
	{
		if (HudTutorial.GetKind(actionID) == HudTutorial.Kind.ACTION)
		{
			int num = actionID - HudTutorial.Id.ACTION_1;
			if ((ulong)num < (ulong)((long)HudTutorial.ACTION_TUTORIAL_DATA_TBL.Length))
			{
				return HudTutorial.ACTION_TUTORIAL_DATA_TBL[num].m_flagStatus;
			}
		}
		return SystemData.ActionTutorialFlagStatus.NONE;
	}

	public static SystemData.QuickModeTutorialFlagStatus GetQuickModeTutorialSaveFlag(HudTutorial.Id quickID)
	{
		if (HudTutorial.GetKind(quickID) == HudTutorial.Kind.QUICK)
		{
			int num = quickID - HudTutorial.Id.QUICK_1;
			if ((ulong)num < (ulong)((long)HudTutorial.QUICK_MODE_TUTORIAL_DATA_TBL.Length))
			{
				return HudTutorial.QUICK_MODE_TUTORIAL_DATA_TBL[num].m_flagStatus;
			}
		}
		return SystemData.QuickModeTutorialFlagStatus.NONE;
	}

	public static string GetActionTutorialText(HudTutorial.Id actionID)
	{
		if (HudTutorial.GetKind(actionID) == HudTutorial.Kind.ACTION)
		{
			int num = actionID - HudTutorial.Id.ACTION_1;
			if ((ulong)num < (ulong)((long)HudTutorial.ACTION_TUTORIAL_DATA_TBL.Length))
			{
				return TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, HudTutorial.ACTION_TUTORIAL_DATA_TBL[num].m_textCategory, HudTutorial.ACTION_TUTORIAL_DATA_TBL[num].m_textCell).text;
			}
		}
		return string.Empty;
	}

	public static string GetItemTutorialText(HudTutorial.Id itemID)
	{
		if (HudTutorial.GetKind(itemID) == HudTutorial.Kind.ITEM)
		{
			string text = "name" + (itemID - HudTutorial.Id.ITEM_1 + 1).ToString();
			if (itemID == HudTutorial.Id.ITEM_6)
			{
				text += "_2";
			}
			return TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ShopItem", text).text;
		}
		return string.Empty;
	}

	public static string GetItemTutorialCaptionText(HudTutorial.Id itemID)
	{
		if (HudTutorial.GetKind(itemID) != HudTutorial.Kind.ITEM)
		{
			return string.Empty;
		}
		if (itemID == HudTutorial.Id.ITEM_6)
		{
			return "caption_explan2_2";
		}
		return "caption_explan2";
	}
}
