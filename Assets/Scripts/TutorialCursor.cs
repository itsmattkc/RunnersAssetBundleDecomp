using System;
using UnityEngine;

public class TutorialCursor : MonoBehaviour
{
	public enum Type
	{
		CHAOSELECT_CHAO,
		CHAOSELECT_MAIN,
		ITEMSELECT_LASER,
		MAINMENU_CHAO,
		MAINMENU_PLAY,
		MAINMENU_PAGE,
		MAINMENU_ROULETTE,
		MAINMENU_BOSS_PLAY,
		ROULETTE_SPIN,
		ROULETTE_OK,
		BACK,
		OPTION,
		CHARASELECT_CHARA,
		CHARASELECT_MAIN,
		CHARASELECT_LEVEL_UP,
		ITEMSELECT_SUBCHARA,
		ROULETTE_TOP_PAGE,
		STAGE_ITEM,
		NUM,
		NONE
	}

	private static readonly TutorialCursorParam[] ParamTable = new TutorialCursorParam[]
	{
		new TutorialCursorParam("Anchor_5_MC/pattern_ch_chao"),
		new TutorialCursorParam("Anchor_5_MC/pattern_ch_main"),
		new TutorialCursorParam("Anchor_5_MC/pattern_it_laser"),
		new TutorialCursorParam("Anchor_5_MC/pattern_mm_chao"),
		new TutorialCursorParam("Anchor_9_BR/pattern_mm_play"),
		new TutorialCursorParam("Anchor_8_BC/pattern_mm_page3"),
		new TutorialCursorParam("Anchor_8_BC/pattern_mm_roulette"),
		new TutorialCursorParam("Anchor_5_MC/pattern_mm_boss_play"),
		new TutorialCursorParam("Anchor_6_MR/pattern_ro_spin"),
		new TutorialCursorParam("Anchor_5_MC/pattern_ro_ok"),
		new TutorialCursorParam("Anchor_7_BL/pattern_back"),
		new TutorialCursorParam("Anchor_5_MC/pattern_option"),
		new TutorialCursorParam("Anchor_5_MC/pattern_pl_main"),
		new TutorialCursorParam("Anchor_5_MC/pattern_pl_mainsub"),
		new TutorialCursorParam("Anchor_5_MC/pattern_pl_levelup"),
		new TutorialCursorParam("Anchor_5_MC/pattern_pl_change"),
		new TutorialCursorParam("Anchor_5_MC/pattern_ro_premium"),
		new TutorialCursorParam("Anchor_8_BC/pattern_st_item")
	};

	private GameObject[] m_cursorObj = new GameObject[18];

	private UIDraggablePanel m_draggablePanel;

	private TutorialCursor.Type m_type = TutorialCursor.Type.NONE;

	private bool m_optionTouch;

	private bool m_created;

	private static TutorialCursor m_instance = null;

	public static TutorialCursor Instance
	{
		get
		{
			return TutorialCursor.m_instance;
		}
	}

	private void Awake()
	{
		if (TutorialCursor.m_instance == null)
		{
			TutorialCursor.m_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		this.EntryBackKeyCallBack();
	}

	private void OnDestroy()
	{
		if (TutorialCursor.m_instance == this)
		{
			TutorialCursor.m_instance.RemoveBackKeyCallBack();
			TutorialCursor.m_instance = null;
		}
	}

	public void Setup()
	{
		for (int i = 0; i < 18; i++)
		{
			Transform transform = base.gameObject.transform.FindChild(TutorialCursor.ParamTable[i].m_name);
			if (transform != null)
			{
				this.m_cursorObj[i] = transform.gameObject;
				if (this.m_cursorObj[i] != null)
				{
					this.m_cursorObj[i].SetActive(false);
				}
			}
		}
		this.SetUIButtonMessage(TutorialCursor.Type.OPTION);
		this.SetUIButtonMessage(TutorialCursor.Type.ITEMSELECT_SUBCHARA);
		this.m_draggablePanel = HudMenuUtility.GetMainMenuDraggablePanel();
	}

	private void SetUIButtonMessage(TutorialCursor.Type type)
	{
		if (this.m_cursorObj[(int)type] != null)
		{
			Transform transform = this.m_cursorObj[(int)type].transform.FindChild("blinder/0_all");
			if (transform != null)
			{
				GameObject gameObject = transform.gameObject;
				UIButtonMessage uIButtonMessage = gameObject.GetComponent<UIButtonMessage>();
				if (uIButtonMessage == null)
				{
					uIButtonMessage = gameObject.AddComponent<UIButtonMessage>();
				}
				if (uIButtonMessage != null)
				{
					uIButtonMessage.enabled = true;
					uIButtonMessage.trigger = UIButtonMessage.Trigger.OnClick;
					uIButtonMessage.target = base.gameObject;
					uIButtonMessage.functionName = "OnOptionTouchScreen";
				}
			}
		}
	}

	public void OnStartTutorialCursor(TutorialCursor.Type type)
	{
		this.m_optionTouch = false;
		this.m_created = true;
		BackKeyManager.AddWindowCallBack(base.gameObject);
		this.SetTutorialCursor(type, true);
	}

	public void OnEndTutorialCursor(TutorialCursor.Type type)
	{
		this.m_optionTouch = false;
		this.m_created = false;
		BackKeyManager.RemoveWindowCallBack(base.gameObject);
		this.SetTutorialCursor(type, false);
	}

	public bool IsOptionTouchScreen()
	{
		return this.m_optionTouch;
	}

	public bool IsCreated()
	{
		return this.m_created;
	}

	private void SetTutorialCursor(TutorialCursor.Type type, bool active)
	{
		if (type < TutorialCursor.Type.NUM)
		{
			if (active)
			{
				for (int i = 0; i < 18; i++)
				{
					if (this.m_cursorObj[i] != null)
					{
						if (type == (TutorialCursor.Type)i)
						{
							this.m_cursorObj[i].SetActive(true);
						}
						else
						{
							this.m_cursorObj[i].SetActive(false);
						}
					}
				}
				this.m_type = type;
			}
			else
			{
				if (this.m_cursorObj[(int)type] != null)
				{
					this.m_cursorObj[(int)type].SetActive(false);
				}
				this.m_type = TutorialCursor.Type.NONE;
			}
			this.SetDraggablePanel(!active);
		}
	}

	private void SetDraggablePanel(bool on)
	{
		if (this.m_draggablePanel != null)
		{
			this.m_draggablePanel.enabled = on;
		}
	}

	private void OnOptionTouchScreen()
	{
		this.m_optionTouch = true;
	}

	private void EntryBackKeyCallBack()
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
	}

	private void RemoveBackKeyCallBack()
	{
		BackKeyManager.RemoveWindowCallBack(base.gameObject);
	}

	public void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (this.m_created)
		{
			bool flag = false;
			switch (this.m_type)
			{
			case TutorialCursor.Type.OPTION:
				flag = true;
				break;
			case TutorialCursor.Type.CHARASELECT_LEVEL_UP:
				flag = true;
				break;
			case TutorialCursor.Type.ITEMSELECT_SUBCHARA:
				flag = true;
				break;
			}
			if (flag)
			{
				this.OnOptionTouchScreen();
				msg.StaySequence();
			}
		}
	}

	public static TutorialCursor GetTutorialCursor()
	{
		TutorialCursor instance = TutorialCursor.Instance;
		if (instance == null)
		{
			GameObject gameObject = Resources.Load("Prefabs/UI/tutorial_sign") as GameObject;
			GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
			if (gameObject != null && cameraUIObject != null)
			{
				UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
				instance = TutorialCursor.Instance;
				if (instance != null)
				{
					instance.Setup();
					Vector3 localPosition = instance.gameObject.transform.localPosition;
					Vector3 localScale = instance.gameObject.transform.localScale;
					instance.gameObject.transform.parent = cameraUIObject.transform;
					instance.gameObject.transform.localPosition = localPosition;
					instance.gameObject.transform.localScale = localScale;
				}
			}
		}
		return instance;
	}

	public static void StartTutorialCursor(TutorialCursor.Type type)
	{
		TutorialCursor tutorialCursor = TutorialCursor.GetTutorialCursor();
		if (tutorialCursor != null)
		{
			tutorialCursor.OnStartTutorialCursor(type);
		}
	}

	public static void EndTutorialCursor(TutorialCursor.Type type)
	{
		TutorialCursor tutorialCursor = TutorialCursor.GetTutorialCursor();
		if (tutorialCursor != null)
		{
			tutorialCursor.OnEndTutorialCursor(type);
		}
	}

	public static bool IsTouchScreen()
	{
		TutorialCursor tutorialCursor = TutorialCursor.GetTutorialCursor();
		return tutorialCursor != null && tutorialCursor.IsOptionTouchScreen();
	}

	public static void DestroyTutorialCursor()
	{
		TutorialCursor instance = TutorialCursor.Instance;
		if (instance != null)
		{
			instance.m_created = false;
			instance.SetDraggablePanel(true);
			UnityEngine.Object.Destroy(instance.gameObject);
		}
	}
}
