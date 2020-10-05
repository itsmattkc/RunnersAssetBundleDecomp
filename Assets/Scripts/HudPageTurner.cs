using AnimationOrTween;
using App;
using Message;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HudPageTurner : MonoBehaviour
{
	private const string LEFT_BUTTON_PATH = "Anchor_4_ML/mainmenu_grip_L/";

	private const string RIGHT_BUTTON_PATH = "Anchor_6_MR/mainmenu_grip_R/";

	private const string SCROLL_BAR_PATH = "Anchor_5_MC/mainmenu_grip/mainmenu_SB";

	private const string SLIDER_PATH = "Anchor_5_MC/mainmenu_grip/mainmenu_Slider";

	private const string CONTENTS_PATH = "Anchor_5_MC/mainmenu_contents";

	private const string LEFT_BUTTON_NAME = "Btn_mainmenu_pager_L";

	private const string RIGHT_BUTTON_NAME = "Btn_mainmenu_pager_R";

	private const string LEFT_ICON_NAME = "img_pager_icon_l";

	private const string RIGHT_ICON_NAME = "img_pager_icon_r";

	private const string PAGE_BTN_PATH = "Anchor_5_MC/mainmenu_grip/Btn_pager/";

	private const string LEFT_PAGE_BTN_NAME = "Btn_mainmenu_pager_L";

	private const string RIGHT_PAGE_BTN_NAME = "Btn_mainmenu_pager_R";

	private const string SE_NAME = "sys_page_skip";

	private const string BG_PATH = "Anchor_5_MC/mainmenu_grip/custom_bg";

	private const string BG_ANIM = "ui_mm_mileage_bg_Anim";

	private const float PAGE_STEP_VALUE = 0.5f;

	private GameObject m_left_button;

	private GameObject m_right_button;

	private GameObject m_leftPageBtn;

	private GameObject m_rightPageBtn;

	private GameObject m_bg;

	private UITexture m_bg_ui_tex;

	private Animation m_bg_animation;

	private UIScrollBar m_scroll_bar;

	private UISlider m_slider;

	private UIPanel m_contents_panel;

	private UISprite m_left_icon;

	private UISprite m_right_icon;

	private uint m_page_number;

	private bool m_bg_anim;

	private bool m_initEnd;

	private bool _TutorialFlag_k__BackingField;

	public bool TutorialFlag
	{
		get;
		set;
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		if (this.m_bg_ui_tex != null)
		{
			this.m_bg_ui_tex.material.mainTexture = null;
			this.m_bg_ui_tex.material = null;
			this.m_bg_ui_tex.mainTexture = null;
		}
	}

	private void Initialize()
	{
		if (this.m_initEnd)
		{
			return;
		}
		GameObject mainMenuUIObject = HudMenuUtility.GetMainMenuUIObject();
		if (mainMenuUIObject != null)
		{
			Transform transform = mainMenuUIObject.transform.FindChild("Anchor_4_ML/mainmenu_grip_L/Btn_mainmenu_pager_L");
			if (transform != null)
			{
				this.m_left_button = transform.gameObject;
				this.SetButtonCommponent(this.m_left_button);
			}
			Transform transform2 = mainMenuUIObject.transform.FindChild("Anchor_5_MC/mainmenu_grip/Btn_pager/Btn_mainmenu_pager_L");
			if (transform2 != null)
			{
				this.m_leftPageBtn = transform2.gameObject;
				this.SetButtonCommponent(this.m_leftPageBtn);
				Transform transform3 = transform2.FindChild("img_pager_icon_l");
				if (transform3 != null)
				{
					this.m_left_icon = transform3.gameObject.GetComponent<UISprite>();
				}
			}
			Transform transform4 = mainMenuUIObject.transform.FindChild("Anchor_6_MR/mainmenu_grip_R/Btn_mainmenu_pager_R");
			if (transform4 != null)
			{
				this.m_right_button = transform4.gameObject;
				this.SetButtonCommponent(this.m_right_button);
			}
			Transform transform5 = mainMenuUIObject.transform.FindChild("Anchor_5_MC/mainmenu_grip/Btn_pager/Btn_mainmenu_pager_R");
			if (transform5 != null)
			{
				this.m_rightPageBtn = transform5.gameObject;
				this.SetButtonCommponent(this.m_rightPageBtn);
				Transform transform6 = transform5.FindChild("img_pager_icon_r");
				if (transform6 != null)
				{
					this.m_right_icon = transform6.gameObject.GetComponent<UISprite>();
				}
			}
			GameObject gameObject = mainMenuUIObject.transform.FindChild("Anchor_5_MC/mainmenu_grip/mainmenu_SB").gameObject;
			if (gameObject != null)
			{
				this.m_scroll_bar = gameObject.GetComponent<UIScrollBar>();
				if (this.m_scroll_bar != null)
				{
					EventDelegate.Add(this.m_scroll_bar.onChange, new EventDelegate.Callback(this.OnChangeScrollBarValue));
				}
			}
			GameObject gameObject2 = mainMenuUIObject.transform.FindChild("Anchor_5_MC/mainmenu_grip/mainmenu_Slider").gameObject;
			if (gameObject2 != null)
			{
				this.m_slider = gameObject2.GetComponent<UISlider>();
			}
			Transform transform7 = mainMenuUIObject.transform.FindChild("Anchor_5_MC/mainmenu_contents");
			if (transform7 != null)
			{
				GameObject gameObject3 = transform7.gameObject;
				if (gameObject3 != null)
				{
					this.m_contents_panel = gameObject3.GetComponent<UIPanel>();
				}
			}
			this.m_bg = mainMenuUIObject.transform.FindChild("Anchor_5_MC/mainmenu_grip/custom_bg").gameObject;
			if (this.m_bg != null)
			{
				this.m_bg_animation = this.m_bg.GetComponent<Animation>();
				GameObject gameObject4 = this.m_bg.transform.FindChild("img_stage_tex").gameObject;
				if (gameObject4 != null)
				{
					this.m_bg_ui_tex = gameObject4.GetComponent<UITexture>();
				}
			}
		}
		this.SetHeader(this.m_page_number);
		this.SetIcon(this.m_page_number);
		this.m_initEnd = true;
	}

	private void SetButtonCommponent(GameObject obj)
	{
		if (obj != null)
		{
			UIButtonMessage uIButtonMessage = obj.GetComponent<UIButtonMessage>();
			if (uIButtonMessage == null)
			{
				uIButtonMessage = obj.AddComponent<UIButtonMessage>();
			}
			if (uIButtonMessage != null)
			{
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OnClickButtonCallBack";
			}
		}
	}

	private void ChangeNextPage(bool left_page_flag)
	{
		if (this.m_scroll_bar != null)
		{
			if (left_page_flag)
			{
				if (this.m_page_number == 2u)
				{
					this.m_scroll_bar.value = 0.5f;
				}
				else if (this.m_page_number == 1u)
				{
					this.m_scroll_bar.value = 0f;
					this.SetVisible(this.m_left_button, false);
					this.SetVisible(this.m_leftPageBtn, false);
				}
				this.SetVisible(this.m_right_button, true);
				this.SetVisible(this.m_rightPageBtn, true);
			}
			else
			{
				if (this.m_page_number == 0u)
				{
					this.m_scroll_bar.value = 0.5f;
				}
				else if (this.m_page_number == 1u)
				{
					this.m_scroll_bar.value = 1f;
					this.SetVisible(this.m_right_button, false);
					this.SetVisible(this.m_rightPageBtn, false);
				}
				this.SetVisible(this.m_left_button, true);
				this.SetVisible(this.m_leftPageBtn, true);
			}
			if (this.m_scroll_bar.onDragFinished != null)
			{
				this.m_scroll_bar.onDragFinished();
			}
		}
	}

	private void OnChangeScrollBarValue()
	{
		if (this.m_slider != null)
		{
			float value = this.m_scroll_bar.value;
			if (App.Math.NearZero(value, 1E-06f))
			{
				if (this.m_page_number != 0u)
				{
					this.m_page_number = 0u;
					this.SetHeader(this.m_page_number);
					this.SetIcon(this.m_page_number);
					this.SetBG(this.m_page_number);
					this.m_slider.value = 0f;
					SoundManager.SePlay("sys_page_skip", "SE");
				}
				this.SetVisible(this.m_left_button, false);
				this.SetVisible(this.m_leftPageBtn, false);
				this.SetVisible(this.m_right_button, true);
				this.SetVisible(this.m_rightPageBtn, true);
			}
			else if (Mathf.Abs(value - 0.5f) < 0.02f)
			{
				if (this.m_page_number != 1u)
				{
					this.m_page_number = 1u;
					this.SetHeader(this.m_page_number);
					this.SetIcon(this.m_page_number);
					this.SetBG(this.m_page_number);
					this.m_slider.value = 0.5f;
					this.m_scroll_bar.value = 0.5f;
					SoundManager.SePlay("sys_page_skip", "SE");
				}
				this.SetVisible(this.m_left_button, true);
				this.SetVisible(this.m_leftPageBtn, true);
				this.SetVisible(this.m_right_button, true);
				this.SetVisible(this.m_rightPageBtn, true);
			}
			else if (App.Math.NearZero(value - 1f, 1E-06f))
			{
				if (this.m_page_number != 2u)
				{
					this.m_page_number = 2u;
					this.SetHeader(this.m_page_number);
					this.SetIcon(this.m_page_number);
					this.SetBG(this.m_page_number);
					this.m_slider.value = 1f;
					SoundManager.SePlay("sys_page_skip", "SE");
				}
				this.SetVisible(this.m_left_button, true);
				this.SetVisible(this.m_leftPageBtn, true);
				this.SetVisible(this.m_right_button, false);
				this.SetVisible(this.m_rightPageBtn, false);
			}
			if (this.m_contents_panel != null)
			{
				Vector4 clipRange = this.m_contents_panel.clipRange;
				clipRange.y = 0f;
				this.m_contents_panel.clipRange = clipRange;
			}
		}
	}

	private void OnClickButtonCallBack(GameObject obj)
	{
		if (obj.name == "Btn_mainmenu_pager_L" || obj.name == "Btn_mainmenu_pager_L")
		{
			this.ChangeNextPage(true);
		}
		else if (obj.name == "Btn_mainmenu_pager_R" || obj.name == "Btn_mainmenu_pager_R")
		{
			this.ChangeNextPage(false);
			if (this.TutorialFlag)
			{
				HudMenuUtility.SendMsgMenuSequenceToMainMenu(MsgMenuSequence.SequeneceType.TUTORIAL_PAGE_MOVE);
			}
		}
	}

	private void SetVisible(GameObject obj, bool flag)
	{
		if (obj != null)
		{
			obj.SetActive(flag);
		}
	}

	private void SetPage(uint page)
	{
		this.SetHeader(page);
		this.SetBG(page);
		this.SetIcon(page);
	}

	private void SetHeader(uint page)
	{
		if (page >= 0u && page <= 2u)
		{
			uint num = page + 1u;
			HudMenuUtility.SendChangeHeaderText("ui_Header_MainPage" + num);
		}
	}

	private void SetBG(uint page)
	{
		if (this.m_bg != null && this.m_bg_animation != null && this.m_bg_ui_tex != null)
		{
			if (page == 1u)
			{
				if (!this.m_bg_anim)
				{
					this.m_bg.SetActive(true);
					this.m_bg_ui_tex.material.mainTexture = MileageMapUtility.GetBGTexture();
					ActiveAnimation.Play(this.m_bg_animation, "ui_mm_mileage_bg_Anim", Direction.Forward);
					this.m_bg_anim = true;
				}
			}
			else if (this.m_bg_anim)
			{
				ActiveAnimation.Play(this.m_bg_animation, "ui_mm_mileage_bg_Anim", Direction.Reverse);
				this.m_bg_anim = false;
			}
		}
	}

	private void SetIcon(uint page)
	{
		switch (page)
		{
		case 0u:
			this.SetIcon(this.m_right_icon, "ui_mm_pager_icon_1");
			break;
		case 1u:
			this.SetIcon(this.m_left_icon, "ui_mm_pager_icon_0");
			this.SetIcon(this.m_right_icon, "ui_mm_pager_icon_2");
			break;
		case 2u:
			this.SetIcon(this.m_left_icon, "ui_mm_pager_icon_1");
			break;
		}
	}

	private void SetIcon(UISprite uiSprite, string name)
	{
		if (uiSprite != null)
		{
			uiSprite.spriteName = name;
		}
	}

	private void OnSendChangeMainPageHeaderText()
	{
		this.SetPage(this.m_page_number);
	}

	private void SetMileageMapProduction()
	{
		this.m_page_number = 1u;
		this.SetPage(this.m_page_number);
		this.m_scroll_bar.value = 0.5f;
		this.m_slider.value = 0.5f;
		if (this.m_scroll_bar.onDragFinished != null)
		{
			this.m_scroll_bar.onDragFinished();
		}
	}

	public void OnSetBGTexture()
	{
		if (this.m_bg_ui_tex != null)
		{
			this.m_bg_ui_tex.material.mainTexture = MileageMapUtility.GetBGTexture();
		}
	}

	public void OnNormalDisplay()
	{
		this.Initialize();
		this.SetMileageMapProduction();
	}

	private void OnStartMileageMapProduction()
	{
		this.Initialize();
		this.SetMileageMapProduction();
		this.SetColliderTrigger(this.m_left_button, true);
		this.SetColliderTrigger(this.m_right_button, true);
	}

	private void OnStartRankingProduction()
	{
		this.Initialize();
		this.m_page_number = 0u;
		this.SetPage(this.m_page_number);
		this.m_scroll_bar.value = 0f;
		this.m_slider.value = 0f;
		this.SetVisible(this.m_left_button, false);
		this.SetVisible(this.m_leftPageBtn, false);
		this.SetVisible(this.m_right_button, false);
		this.SetVisible(this.m_rightPageBtn, false);
		if (this.m_scroll_bar.onDragFinished != null)
		{
			this.m_scroll_bar.onDragFinished();
		}
		this.SetColliderTrigger(this.m_left_button, true);
		this.SetColliderTrigger(this.m_right_button, true);
	}

	private void OnSetPlayerChaoSetPage()
	{
		this.Initialize();
		this.m_scroll_bar.value = 1f;
		if (this.m_bg)
		{
			this.m_bg.SetActive(false);
		}
		if (this.m_scroll_bar.onDragFinished != null)
		{
			this.m_scroll_bar.onDragFinished();
		}
	}

	private void OnEndMileageMapProduction()
	{
		this.SetColliderTrigger(this.m_left_button, false);
		this.SetColliderTrigger(this.m_right_button, false);
	}

	private void SetColliderTrigger(GameObject obj, bool trigger)
	{
		if (obj != null)
		{
			BoxCollider component = obj.GetComponent<BoxCollider>();
			if (component != null)
			{
				component.isTrigger = trigger;
			}
		}
	}
}
