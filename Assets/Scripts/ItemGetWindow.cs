using AnimationOrTween;
using DataTable;
using System;
using Text;
using UnityEngine;

public class ItemGetWindow : WindowBase
{
	public enum ButtonType
	{
		Ok,
		TweetCancel
	}

	public class CInfo
	{
		public delegate void FinishedCloseDelegate();

		public bool disableButton;

		public ItemGetWindow.ButtonType buttonType;

		public string name;

		public string caption;

		public int serverItemId = -1;

		public string imageCount;

		public ItemGetWindow.CInfo.FinishedCloseDelegate finishedCloseDelegate;
	}

	private struct Pressed
	{
		public bool m_isButtonPressed;

		public bool m_isOkButtonPressed;

		public bool m_isYesButtonPressed;

		public bool m_isNoButtonPressed;
	}

	private ItemGetWindow.CInfo m_info = new ItemGetWindow.CInfo();

	private ItemGetWindow.Pressed m_pressed;

	[SerializeField]
	private GameObject m_imgView;

	[SerializeField]
	private GameObject m_imgEventTex;

	[SerializeField]
	private UITexture m_uiTexture;

	[SerializeField]
	private GameObject m_imgItem;

	[SerializeField]
	private UISprite m_imgItemSprite;

	[SerializeField]
	private UILabel m_imgName;

	[SerializeField]
	private UILabel m_imgCount;

	[SerializeField]
	private GameObject m_imgDecoEff;

	private string m_itemImageSpriteName;

	private bool m_disableButton;

	private bool m_isEnd = true;

	private bool m_isOpened;

	public ItemGetWindow.CInfo Info
	{
		get
		{
			return this.m_info;
		}
	}

	public bool IsOkButtonPressed
	{
		get
		{
			return this.m_pressed.m_isOkButtonPressed;
		}
	}

	public bool IsYesButtonPressed
	{
		get
		{
			return this.m_pressed.m_isYesButtonPressed;
		}
	}

	public bool IsNoButtonPressed
	{
		get
		{
			return this.m_pressed.m_isNoButtonPressed;
		}
	}

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		base.Destroy();
	}

	private void SetWindowData()
	{
		base.gameObject.SetActive(true);
		this.SetDisableButton(this.m_disableButton);
		bool active = false;
		bool active2 = false;
		ItemGetWindow.ButtonType buttonType = this.m_info.buttonType;
		if (buttonType != ItemGetWindow.ButtonType.Ok)
		{
			if (buttonType == ItemGetWindow.ButtonType.TweetCancel)
			{
				active2 = true;
			}
		}
		else
		{
			active = true;
		}
		Transform transform = base.gameObject.transform.Find("window/pattern_btn_use");
		Transform transform2 = base.gameObject.transform.Find("window/pattern_btn_use/pattern_0");
		Transform transform3 = base.gameObject.transform.Find("window/pattern_btn_use/pattern_1");
		if (transform != null)
		{
			transform.gameObject.SetActive(true);
		}
		if (transform2 != null)
		{
			transform2.gameObject.SetActive(active);
		}
		if (transform3 != null)
		{
			transform3.gameObject.SetActive(active2);
			UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(transform3.gameObject, "Btn_post");
			if (uIImageButton != null)
			{
				if (RegionManager.Instance != null)
				{
					uIImageButton.isEnabled = RegionManager.Instance.IsUseSNS();
				}
				else
				{
					uIImageButton.isEnabled = false;
				}
			}
		}
		Transform transform4 = base.gameObject.transform.Find("window/Lbl_caption");
		if (transform4 != null)
		{
			UILabel component = transform4.GetComponent<UILabel>();
			if (component != null)
			{
				component.text = this.m_info.caption;
			}
		}
		if (this.m_imgView != null)
		{
			this.m_imgView.SetActive(true);
		}
		bool active3 = true;
		bool active4 = true;
		bool active5 = true;
		string text = string.Empty;
		ServerItem serverItem = new ServerItem((ServerItem.Id)this.m_info.serverItemId);
		if (serverItem.idType == ServerItem.IdType.CHAO)
		{
			int chaoId = serverItem.chaoId;
			if (ChaoTextureManager.Instance != null)
			{
				ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(this.m_uiTexture, null, true);
				ChaoTextureManager.Instance.GetTexture(chaoId, info);
			}
			ChaoData chaoData = ChaoTable.GetChaoData(chaoId);
			if (chaoData != null)
			{
				text = chaoData.name;
				this.m_info.imageCount = TextUtility.GetTextLevel(chaoData.level.ToString());
			}
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_imgEventTex, "img_tex_flame");
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			active3 = false;
		}
		else if (serverItem.rewardType != RewardType.NONE)
		{
			if (serverItem.id == ServerItem.Id.JACKPOT)
			{
				this.m_itemImageSpriteName = "ui_cmn_icon_item_" + 8;
				active4 = false;
				text = TextUtility.GetCommonText("Item", "ring");
			}
			else
			{
				this.m_itemImageSpriteName = "ui_cmn_icon_item_" + (int)serverItem.rewardType;
				active4 = false;
				text = serverItem.serverItemName;
			}
		}
		else
		{
			this.m_itemImageSpriteName = null;
			active3 = false;
			active4 = false;
			active5 = false;
		}
		if (this.m_imgEventTex != null)
		{
			this.m_imgEventTex.SetActive(active4);
		}
		if (this.m_imgItem != null)
		{
			this.m_imgItem.SetActive(active3);
		}
		if (this.m_imgItemSprite != null)
		{
			this.m_imgItemSprite.spriteName = this.m_itemImageSpriteName;
		}
		if (this.m_imgName != null)
		{
			this.m_imgName.text = text;
		}
		if (this.m_imgCount != null)
		{
			this.m_imgCount.text = this.m_info.imageCount;
		}
		if (this.m_imgDecoEff != null)
		{
			this.m_imgDecoEff.SetActive(active5);
		}
	}

	public GameObject Create(ItemGetWindow.CInfo info)
	{
		RouletteManager.OpenRouletteWindow();
		this.m_info = info;
		this.m_disableButton = info.disableButton;
		this.m_pressed.m_isButtonPressed = false;
		this.m_pressed.m_isOkButtonPressed = false;
		this.m_pressed.m_isYesButtonPressed = false;
		this.m_pressed.m_isNoButtonPressed = false;
		this.m_isEnd = false;
		this.m_isOpened = false;
		this.SetWindowData();
		SoundManager.SePlay("sys_window_open", "SE");
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(component, Direction.Forward);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallback), true);
			}
		}
		return base.gameObject;
	}

	public void Reset()
	{
		this.m_info = new ItemGetWindow.CInfo();
		this.m_disableButton = false;
		this.m_pressed.m_isButtonPressed = false;
		this.m_pressed.m_isOkButtonPressed = false;
		this.m_pressed.m_isYesButtonPressed = false;
		this.m_pressed.m_isNoButtonPressed = false;
		this.m_isEnd = true;
		this.m_isOpened = false;
	}

	public bool IsCreated(string name)
	{
		return this.m_info.name == name;
	}

	private void OnClickOkButton()
	{
		RouletteManager.CloseRouletteWindow();
		if (!this.m_pressed.m_isButtonPressed)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			this.m_pressed.m_isOkButtonPressed = true;
			this.m_pressed.m_isButtonPressed = true;
		}
		this.m_isOpened = false;
	}

	private void OnClickYesButton()
	{
		RouletteManager.CloseRouletteWindow();
		if (!this.m_pressed.m_isButtonPressed)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			this.m_pressed.m_isYesButtonPressed = true;
			this.m_pressed.m_isButtonPressed = true;
		}
		this.m_isOpened = false;
	}

	private void OnClickNoButton()
	{
		RouletteManager.CloseRouletteWindow();
		if (!this.m_pressed.m_isButtonPressed)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			this.m_pressed.m_isNoButtonPressed = true;
			this.m_pressed.m_isButtonPressed = true;
		}
		this.m_isOpened = false;
	}

	private void OnSetChaoTexture(ChaoTextureManager.TextureData data)
	{
		if (data.tex != null && this.m_uiTexture != null)
		{
			this.m_uiTexture.enabled = true;
			this.m_uiTexture.mainTexture = data.tex;
		}
	}

	public void OnFinishedAnimationCallback()
	{
		this.m_isOpened = true;
	}

	public void OnFinishedCloseAnim()
	{
		RouletteManager.CloseRouletteWindow();
		this.m_isEnd = true;
		if (this.m_info.finishedCloseDelegate != null)
		{
			this.m_info.finishedCloseDelegate();
		}
		base.gameObject.SetActive(false);
	}

	public void SetDisableButton(bool disableButton)
	{
		this.m_disableButton = disableButton;
		UIButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<UIButton>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UIButton uIButton = componentsInChildren[i];
			uIButton.isEnabled = !this.m_disableButton;
		}
		UIImageButton[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<UIImageButton>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			UIImageButton uIImageButton = componentsInChildren2[j];
			uIImageButton.isEnabled = !this.m_disableButton;
		}
	}

	private void SendButtonMessage(string patternName, string btnName)
	{
		Transform transform = base.gameObject.transform.Find(patternName);
		if (transform != null)
		{
			UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(transform.gameObject, btnName);
			if (uIButtonMessage != null)
			{
				uIButtonMessage.SendMessage("OnClick");
			}
		}
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (this.m_isOpened)
		{
			ItemGetWindow.ButtonType buttonType = this.m_info.buttonType;
			if (buttonType != ItemGetWindow.ButtonType.Ok)
			{
				if (buttonType == ItemGetWindow.ButtonType.TweetCancel)
				{
					this.SendButtonMessage("window/pattern_btn_use/pattern_1", "Btn_ok");
				}
			}
			else
			{
				this.SendButtonMessage("window/pattern_btn_use/pattern_0", "Btn_ok");
			}
		}
		if (msg != null)
		{
			msg.StaySequence();
		}
	}
}
