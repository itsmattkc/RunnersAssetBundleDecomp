using System;
using Text;
using UnityEngine;

public class MainMenuWindow : MonoBehaviour
{
	public enum WindowType
	{
		ChallengeGoShop,
		ChallengeGoShopFromItem,
		EventStart,
		EventOutOfTime,
		EventLastPlay,
		NUM,
		UNKNOWN = -1
	}

	public class WindowInfo
	{
		private string m_captionGroup;

		private string m_captionCell;

		private string m_messageGroup;

		private string m_messageCell;

		private GeneralWindow.ButtonType m_buttonType;

		private bool m_errorSe;

		public string CaptionGroup
		{
			get
			{
				return this.m_captionGroup;
			}
		}

		public string CaptionCell
		{
			get
			{
				return this.m_captionCell;
			}
		}

		public string MessageGroup
		{
			get
			{
				return this.m_messageGroup;
			}
		}

		public string MessageCell
		{
			get
			{
				return this.m_messageCell;
			}
		}

		public GeneralWindow.ButtonType ButtonType
		{
			get
			{
				return this.m_buttonType;
			}
		}

		public bool ErrorSe
		{
			get
			{
				return this.m_errorSe;
			}
		}

		public WindowInfo()
		{
		}

		public WindowInfo(string captionGroup, string captionCell, string messageGroup, string messageCell, GeneralWindow.ButtonType buttonType, bool errorSe)
		{
			this.m_captionGroup = captionGroup;
			this.m_captionCell = captionCell;
			this.m_messageGroup = messageGroup;
			this.m_messageCell = messageCell;
			this.m_buttonType = buttonType;
			this.m_errorSe = errorSe;
		}
	}

	public delegate void ButtonClickedCallback(bool yesButtonClicked);

	private readonly MainMenuWindow.WindowInfo[] m_windowInfo = new MainMenuWindow.WindowInfo[]
	{
		new MainMenuWindow.WindowInfo("MainMenu", "no_challenge_count", "MainMenu", "no_challenge_count_info", GeneralWindow.ButtonType.ShopCancel, true),
		new MainMenuWindow.WindowInfo("MainMenu", "no_challenge_count", "MainMenu", "no_challenge_count_info", GeneralWindow.ButtonType.ShopCancel, true),
		new MainMenuWindow.WindowInfo("ItemRoulette", "gw_remain_caption", "Event", "ui_Lbl_new_event_start", GeneralWindow.ButtonType.Ok, true),
		new MainMenuWindow.WindowInfo("ItemRoulette", "gw_remain_caption", "Event", "ui_Lbl_event_out_of_time_2", GeneralWindow.ButtonType.Ok, true),
		new MainMenuWindow.WindowInfo("ItemRoulette", "gw_remain_caption", "Event", "ui_Lbl_event_last_time", GeneralWindow.ButtonType.Ok, false)
	};

	private MainMenuWindow.WindowType m_window_type = MainMenuWindow.WindowType.UNKNOWN;

	private MainMenuWindow.ButtonClickedCallback m_buttonClickedCallback;

	private void Start()
	{
		base.enabled = false;
	}

	public void CreateWindow(MainMenuWindow.WindowType window_type, MainMenuWindow.ButtonClickedCallback callback = null)
	{
		if (window_type < MainMenuWindow.WindowType.NUM)
		{
			this.m_window_type = window_type;
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				buttonType = this.m_windowInfo[(int)this.m_window_type].ButtonType,
				caption = TextUtility.GetCommonText(this.m_windowInfo[(int)this.m_window_type].CaptionGroup, this.m_windowInfo[(int)this.m_window_type].CaptionCell),
				message = TextUtility.GetCommonText(this.m_windowInfo[(int)this.m_window_type].MessageGroup, this.m_windowInfo[(int)this.m_window_type].MessageCell),
				isPlayErrorSe = this.m_windowInfo[(int)this.m_window_type].ErrorSe,
				name = "MainMenuWindow"
			});
			base.enabled = true;
			this.m_buttonClickedCallback = callback;
		}
	}

	public void Update()
	{
		if (GeneralWindow.IsCreated("MainMenuWindow") && GeneralWindow.IsButtonPressed)
		{
			if (MainMenuWindow.WindowType.ChallengeGoShop <= this.m_window_type && this.m_window_type < MainMenuWindow.WindowType.NUM && this.m_windowInfo[(int)this.m_window_type].ButtonType == GeneralWindow.ButtonType.ShopCancel)
			{
				bool isYesButtonPressed = GeneralWindow.IsYesButtonPressed;
			}
			if (this.m_buttonClickedCallback != null)
			{
				this.m_buttonClickedCallback(GeneralWindow.IsYesButtonPressed);
				this.m_buttonClickedCallback = null;
			}
			GeneralWindow.Close();
			base.enabled = false;
			this.m_window_type = MainMenuWindow.WindowType.UNKNOWN;
		}
	}
}
