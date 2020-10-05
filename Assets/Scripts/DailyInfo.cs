using AnimationOrTween;
using System;
using Text;
using UnityEngine;

public class DailyInfo : MonoBehaviour
{
	[SerializeField]
	private Animation m_animation;

	[SerializeField]
	private UIPanel m_mainPanel;

	[SerializeField]
	private GameObject m_battleObject;

	[SerializeField]
	private GameObject m_historyObject;

	private bool m_isClickClose;

	private bool m_isEnd;

	private bool m_isHistory;

	private bool m_isToggleLock;

	private void Start()
	{
	}

	public static bool Open()
	{
		bool result = false;
		GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
		if (menuAnimUIObject != null)
		{
			DailyInfo dailyInfo = GameObjectUtil.FindChildGameObjectComponent<DailyInfo>(menuAnimUIObject, "DailyInfoUI");
			if (dailyInfo != null)
			{
				dailyInfo.Setup();
				result = true;
			}
		}
		return result;
	}

	private void Setup()
	{
		if (this.m_mainPanel != null)
		{
			this.m_mainPanel.alpha = 0.1f;
		}
		this.m_isEnd = false;
		this.m_isClickClose = false;
		this.m_isHistory = false;
		if (this.m_animation != null)
		{
			ActiveAnimation.Play(this.m_animation, "ui_daily_challenge_infomation_intro_Anim", Direction.Forward);
		}
		GeneralUtil.SetButtonFunc(base.gameObject, "Btn_today", base.gameObject, "OnClickToggleToday");
		GeneralUtil.SetButtonFunc(base.gameObject, "Btn_history", base.gameObject, "OnClickToggleHistory");
		GeneralUtil.SetButtonFunc(base.gameObject, "Btn_help", base.gameObject, "OnClickHelp");
		this.SetupToggleBtn();
		this.ChangeInfo();
		base.gameObject.SetActive(true);
	}

	private void SetupToggleBtn()
	{
		this.m_isToggleLock = true;
		UIToggle uIToggle;
		if (this.m_isHistory)
		{
			uIToggle = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(base.gameObject, "Btn_history");
			UIToggle uIToggle2 = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(base.gameObject, "Btn_today");
			if (uIToggle2 != null)
			{
				uIToggle2.startsActive = false;
			}
		}
		else
		{
			uIToggle = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(base.gameObject, "Btn_today");
			UIToggle uIToggle3 = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(base.gameObject, "Btn_history");
			if (uIToggle3 != null)
			{
				uIToggle3.startsActive = false;
			}
		}
		uIToggle.startsActive = true;
		if (uIToggle != null)
		{
			uIToggle.SendMessage("Start");
		}
		this.m_isToggleLock = false;
	}

	private void OnClickBack()
	{
		this.m_isClickClose = true;
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickToggleToday()
	{
		if (!this.m_isToggleLock && this.m_isHistory)
		{
			if (GeneralUtil.IsNetwork())
			{
				this.m_isHistory = false;
				this.ChangeInfo();
			}
			else
			{
				this.SetupToggleBtn();
				GeneralUtil.ShowNoCommunication("ShowNoCommunicationDailyInfo");
			}
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void OnClickToggleHistory()
	{
		if (!this.m_isToggleLock && !this.m_isHistory)
		{
			if (GeneralUtil.IsNetwork())
			{
				this.m_isHistory = true;
				this.ChangeInfo();
			}
			else
			{
				this.SetupToggleBtn();
				GeneralUtil.ShowNoCommunication("ShowNoCommunicationDailyInfo");
			}
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void OnClickHelp()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "ShowDailyBattleHelp",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_help_caption"),
			message = TextUtility.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "battle_help_text")
		});
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void ChangeInfo()
	{
		if (!this.m_isHistory)
		{
			this.SetupMainObject();
		}
		else
		{
			this.SetupHistoryObject();
		}
	}

	private void SetupMainObject()
	{
		if (this.m_battleObject == null)
		{
			this.m_battleObject = GameObjectUtil.FindChildGameObject(base.gameObject, "battle_set");
		}
		if (this.m_historyObject == null)
		{
			this.m_historyObject = GameObjectUtil.FindChildGameObject(base.gameObject, "history_set");
		}
		if (this.m_battleObject != null)
		{
			this.m_battleObject.SetActive(true);
			DailyInfoBattle dailyInfoBattle = GameObjectUtil.FindChildGameObjectComponent<DailyInfoBattle>(this.m_battleObject, "battle_set");
			if (dailyInfoBattle == null)
			{
				dailyInfoBattle = this.m_battleObject.AddComponent<DailyInfoBattle>();
			}
			if (dailyInfoBattle != null)
			{
				dailyInfoBattle.Setup(this);
			}
		}
		if (this.m_historyObject != null)
		{
			this.m_historyObject.SetActive(false);
		}
	}

	private void SetupHistoryObject()
	{
		if (this.m_battleObject == null)
		{
			this.m_battleObject = GameObjectUtil.FindChildGameObject(base.gameObject, "battle_set");
		}
		if (this.m_historyObject == null)
		{
			this.m_historyObject = GameObjectUtil.FindChildGameObject(base.gameObject, "history_set");
		}
		if (this.m_battleObject != null)
		{
			this.m_battleObject.SetActive(false);
		}
		if (this.m_historyObject != null)
		{
			this.m_historyObject.SetActive(true);
			DailyInfoHistory dailyInfoHistory = GameObjectUtil.FindChildGameObjectComponent<DailyInfoHistory>(this.m_historyObject, "history_set");
			if (dailyInfoHistory == null)
			{
				dailyInfoHistory = this.m_historyObject.AddComponent<DailyInfoHistory>();
			}
			if (dailyInfoHistory != null)
			{
				dailyInfoHistory.Setup(this);
			}
		}
	}

	public void OnClosedWindowAnim()
	{
		this.m_isEnd = true;
		this.m_isHistory = false;
		if (this.m_mainPanel != null)
		{
			this.m_mainPanel.alpha = 0f;
		}
		this.ChangeInfo();
		base.gameObject.SetActive(false);
	}

	public void OnClickBackButton()
	{
		if (this.m_isEnd)
		{
			return;
		}
		if (!this.m_isClickClose)
		{
			this.m_isClickClose = true;
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_animation, "ui_daily_challenge_infomation_intro_Anim", Direction.Reverse);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnClosedWindowAnim), true);
			}
		}
	}
}
