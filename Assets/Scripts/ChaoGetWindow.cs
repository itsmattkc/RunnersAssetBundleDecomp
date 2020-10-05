using System;
using UnityEngine;

public class ChaoGetWindow : WindowBase
{
	private enum State
	{
		NONE = -1,
		PLAYING,
		WAIT_CLICK_NEXT_BUTTON,
		CLICKED_NEXT_BUTTON,
		END,
		NUM
	}

	private bool m_isSetuped;

	private bool m_isClickedEquip;

	private bool m_isTutorial;

	private bool m_disabledEqip;

	private bool m_backKeyVaildNextBtn;

	private bool m_backKyeVaildOKBtn;

	private ChaoGetPartsBase m_chaoGetParts;

	private HudFlagWatcher m_SeFlagHatch;

	private HudFlagWatcher m_SeFlagBreak;

	private ChaoGetPartsBase.BtnType m_btnType;

	private RouletteUtility.AchievementType m_achievementType;

	private GameObject[] m_buttons = new GameObject[3];

	private ChaoGetWindow.State m_state = ChaoGetWindow.State.END;

	public bool isSetuped
	{
		get
		{
			return this.m_isSetuped;
		}
		set
		{
			this.m_isSetuped = false;
		}
	}

	public bool IsPlayEnd
	{
		get
		{
			return this.m_state == ChaoGetWindow.State.END;
		}
	}

	public bool IsClickedEquip
	{
		get
		{
			return this.m_isClickedEquip;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_state == ChaoGetWindow.State.CLICKED_NEXT_BUTTON)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "skip_collider");
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
			GameObject[] buttons = this.m_buttons;
			for (int i = 0; i < buttons.Length; i++)
			{
				GameObject gameObject2 = buttons[i];
				if (!(gameObject2 == null))
				{
					gameObject2.SetActive(false);
				}
			}
			if (this.m_chaoGetParts != null)
			{
				this.m_chaoGetParts.PlayGetAnimation(base.gameObject.GetComponent<Animation>());
				this.m_btnType = this.m_chaoGetParts.GetButtonType();
				this.m_buttons[(int)this.m_btnType].SetActive(true);
			}
			this.SetEnableButton(this.m_btnType, false);
			this.m_state = ChaoGetWindow.State.PLAYING;
		}
		if (this.m_SeFlagHatch != null)
		{
			this.m_SeFlagHatch.Update();
		}
		if (this.m_SeFlagBreak != null)
		{
			this.m_SeFlagBreak.Update();
		}
	}

	public void PlayStart(ChaoGetPartsBase chaoGetParts, bool isTutorial, bool disabledEqip = false, RouletteUtility.AchievementType achievement = RouletteUtility.AchievementType.NONE)
	{
		this.m_achievementType = achievement;
		RouletteManager.OpenRouletteWindow();
		this.m_chaoGetParts = chaoGetParts;
		this.m_isTutorial = isTutorial;
		this.m_backKyeVaildOKBtn = false;
		this.m_backKeyVaildNextBtn = false;
		this.m_isClickedEquip = false;
		this.m_disabledEqip = disabledEqip;
		this.m_state = ChaoGetWindow.State.PLAYING;
		if (this.m_isTutorial)
		{
			if (RouletteTop.Instance != null && RouletteTop.Instance.category == RouletteCategory.PREMIUM)
			{
				this.m_isTutorial = true;
			}
			else
			{
				this.m_isTutorial = false;
			}
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "pattern_btn_use");
		if (gameObject != null)
		{
			this.m_buttons[0] = GameObjectUtil.FindChildGameObject(gameObject, "pattern_0");
			this.m_buttons[2] = GameObjectUtil.FindChildGameObject(gameObject, "pattern_5");
			this.m_buttons[1] = GameObjectUtil.FindChildGameObject(gameObject, "pattern_6");
		}
		if (!this.m_isSetuped)
		{
			if (this.m_buttons[0] != null)
			{
				UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_buttons[0], "Btn_ok");
				if (uIButtonMessage != null)
				{
					uIButtonMessage.target = base.gameObject;
					uIButtonMessage.functionName = "OkButtonClickedCallback";
				}
			}
			if (this.m_buttons[1] != null)
			{
				UIButtonMessage uIButtonMessage2 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_buttons[1], "Btn_next");
				if (uIButtonMessage2 != null)
				{
					uIButtonMessage2.target = base.gameObject;
					uIButtonMessage2.functionName = "NextButtonClickedCallback";
				}
			}
			if (this.m_buttons[2] != null)
			{
				UIButtonMessage uIButtonMessage3 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_buttons[2], "Btn_ok");
				if (uIButtonMessage3 != null)
				{
					uIButtonMessage3.target = base.gameObject;
					uIButtonMessage3.functionName = "OkButtonClickedCallback";
				}
				UIButtonMessage uIButtonMessage4 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(this.m_buttons[2], "Btn_post");
				if (uIButtonMessage4 != null)
				{
					uIButtonMessage4.target = base.gameObject;
					uIButtonMessage4.functionName = "EquipButtonClickedCallback";
				}
			}
			UIButtonMessage uIButtonMessage5 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "skip_collider");
			if (uIButtonMessage5 != null)
			{
				uIButtonMessage5.target = base.gameObject;
				uIButtonMessage5.functionName = "SkipButtonClickedCallback";
			}
			this.m_SeFlagHatch = new HudFlagWatcher();
			GameObject watchObject = GameObjectUtil.FindChildGameObject(base.gameObject, "SE_flag");
			this.m_SeFlagHatch.Setup(watchObject, new HudFlagWatcher.ValueChangeCallback(this.SeFlagHatchCallback));
			this.m_SeFlagBreak = new HudFlagWatcher();
			GameObject watchObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "SE_flag_break");
			this.m_SeFlagBreak.Setup(watchObject2, new HudFlagWatcher.ValueChangeCallback(this.SeFlagBreakCallback));
			this.m_isSetuped = true;
		}
		base.gameObject.SetActive(true);
		GameObject[] buttons = this.m_buttons;
		for (int i = 0; i < buttons.Length; i++)
		{
			GameObject gameObject2 = buttons[i];
			if (!(gameObject2 == null))
			{
				gameObject2.SetActive(false);
			}
		}
		if (this.m_chaoGetParts != null)
		{
			this.m_chaoGetParts.SetCallback(new ChaoGetPartsBase.AnimationEndCallback(this.AnimationEndCallback));
			this.m_chaoGetParts.Setup(base.gameObject);
			Animation component = base.gameObject.GetComponent<Animation>();
			component.Stop();
			this.m_chaoGetParts.PlayGetAnimation(component);
			this.m_btnType = this.m_chaoGetParts.GetButtonType();
			if (this.m_achievementType == RouletteUtility.AchievementType.Multi || RouletteUtility.loginRoulette)
			{
				this.m_btnType = ChaoGetPartsBase.BtnType.OK;
			}
			if (this.m_btnType >= ChaoGetPartsBase.BtnType.OK && this.m_btnType < ChaoGetPartsBase.BtnType.NUM)
			{
				if (this.m_buttons[(int)this.m_btnType] != null)
				{
					this.m_buttons[(int)this.m_btnType].SetActive(true);
				}
				else if (this.m_buttons[2] != null)
				{
					GameObject gameObject3 = this.m_buttons[2];
					if (gameObject3 != null)
					{
						gameObject3.SetActive(true);
						UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(gameObject3, "Btn_post");
						if (uIImageButton != null)
						{
							uIImageButton.isEnabled = false;
						}
					}
				}
			}
		}
		GameObject gameObject4 = GameObjectUtil.FindChildGameObject(base.gameObject, "skip_collider");
		if (gameObject4 != null)
		{
			gameObject4.SetActive(true);
		}
		this.SetEnableButton(this.m_btnType, false);
		UIDraggablePanel uIDraggablePanel = GameObjectUtil.FindChildGameObjectComponent<UIDraggablePanel>(base.gameObject, "window_chaoset_alpha_clip");
		if (uIDraggablePanel != null)
		{
			uIDraggablePanel.ResetPosition();
		}
		SoundManager.SePlay("sys_window_open", "SE");
	}

	private void SetEnableButton(ChaoGetPartsBase.BtnType buttonType, bool isEnable)
	{
		UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_buttons[(int)buttonType], "Btn_ok");
		if (uIImageButton != null)
		{
			uIImageButton.isEnabled = isEnable;
		}
		UIImageButton uIImageButton2 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_buttons[(int)buttonType], "Btn_next");
		if (uIImageButton2 != null)
		{
			uIImageButton2.isEnabled = isEnable;
		}
		UIImageButton uIImageButton3 = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_buttons[(int)buttonType], "Btn_post");
		if (uIImageButton3 != null)
		{
			uIImageButton3.isEnabled = isEnable;
		}
	}

	private void AnimationEndCallback(ChaoGetPartsBase.AnimType animType)
	{
		switch (animType)
		{
		case ChaoGetPartsBase.AnimType.GET_ANIM_CONTINUE:
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "skip_collider");
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			this.SetEnableButton(this.m_btnType, true);
			this.SetEqipBtnDisabled();
			this.m_backKeyVaildNextBtn = true;
			this.m_state = ChaoGetWindow.State.WAIT_CLICK_NEXT_BUTTON;
			break;
		}
		case ChaoGetPartsBase.AnimType.GET_ANIM_FINISH:
		{
			this.SetEnableButton(this.m_btnType, true);
			this.SetEqipBtnDisabled();
			if (this.m_isTutorial)
			{
				TutorialCursor.StartTutorialCursor(TutorialCursor.Type.ROULETTE_OK);
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "skip_collider");
			if (gameObject2 != null)
			{
				gameObject2.SetActive(false);
			}
			this.m_backKyeVaildOKBtn = true;
			break;
		}
		case ChaoGetPartsBase.AnimType.OUT_ANIM:
			this.DeleteChaoTexture();
			base.gameObject.SetActive(false);
			this.m_state = ChaoGetWindow.State.END;
			break;
		}
	}

	private void SetEqipBtnDisabled()
	{
		if (this.m_disabledEqip)
		{
			GameObject gameObject = this.m_buttons[2];
			if (gameObject != null)
			{
				UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(gameObject, "Btn_post");
				if (uIImageButton != null)
				{
					uIImageButton.isEnabled = false;
				}
			}
		}
	}

	private void OkButtonClickedCallback()
	{
		RouletteManager.CloseRouletteWindow();
		this.m_isClickedEquip = false;
		SoundManager.SePlay("sys_menu_decide", "SE");
		if (this.m_achievementType != RouletteUtility.AchievementType.NONE)
		{
			RouletteManager.RouletteGetWindowClose(this.m_achievementType, RouletteUtility.NextType.NONE);
			this.m_achievementType = RouletteUtility.AchievementType.NONE;
		}
		if (this.m_chaoGetParts != null)
		{
			this.m_chaoGetParts.PlayEndAnimation(base.gameObject.GetComponent<Animation>());
		}
		this.m_backKyeVaildOKBtn = false;
		this.m_backKeyVaildNextBtn = false;
	}

	private void NextButtonClickedCallback()
	{
		RouletteManager.CloseRouletteWindow();
		this.m_backKyeVaildOKBtn = false;
		this.m_backKeyVaildNextBtn = false;
		this.m_state = ChaoGetWindow.State.CLICKED_NEXT_BUTTON;
	}

	private void EquipButtonClickedCallback()
	{
		RouletteManager.CloseRouletteWindow();
		if (this.m_achievementType != RouletteUtility.AchievementType.NONE)
		{
			if (this.m_achievementType == RouletteUtility.AchievementType.PlayerGet)
			{
				RouletteManager.RouletteGetWindowClose(this.m_achievementType, RouletteUtility.NextType.CHARA_EQUIP);
			}
			else
			{
				RouletteManager.RouletteGetWindowClose(this.m_achievementType, RouletteUtility.NextType.EQUIP);
			}
			this.m_achievementType = RouletteUtility.AchievementType.NONE;
		}
		this.m_isClickedEquip = true;
		SoundManager.SePlay("sys_menu_decide", "SE");
		if (this.m_chaoGetParts != null)
		{
			this.m_chaoGetParts.PlayEndAnimation(base.gameObject.GetComponent<Animation>());
		}
		this.m_backKyeVaildOKBtn = false;
		this.m_backKeyVaildNextBtn = false;
	}

	private void SeFlagHatchCallback(float newValue, float prevValue)
	{
		if (newValue == 1f && this.m_chaoGetParts != null)
		{
			this.m_chaoGetParts.PlaySE(ChaoWindowUtility.SeHatch);
		}
	}

	private void SeFlagBreakCallback(float newValue, float prevValue)
	{
		if (newValue == 1f && this.m_chaoGetParts != null)
		{
			this.m_chaoGetParts.PlaySE(ChaoWindowUtility.SeBreak);
		}
	}

	private void SkipButtonClickedCallback()
	{
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			foreach (AnimationState animationState in component)
			{
				if (!(animationState == null))
				{
					animationState.time = animationState.length * 0.99f;
				}
			}
		}
	}

	private void OnSetChaoTexture(ChaoTextureManager.TextureData data)
	{
		UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(base.gameObject, "img_chao_1");
		if (uITexture != null)
		{
			uITexture.mainTexture = data.tex;
			uITexture.enabled = true;
		}
	}

	private void DeleteChaoTexture()
	{
		UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(base.gameObject, "img_chao_1");
		if (uITexture != null)
		{
			uITexture.mainTexture = null;
			uITexture.enabled = false;
			ChaoTextureManager.Instance.RemoveChaoTexture(this.m_chaoGetParts.ChaoId);
		}
	}

	private void SendMessageOnClick(string btnName)
	{
		if (this.m_btnType == ChaoGetPartsBase.BtnType.NONE || this.m_btnType == ChaoGetPartsBase.BtnType.NUM)
		{
			return;
		}
		if (this.m_buttons[(int)this.m_btnType] != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_buttons[(int)this.m_btnType], btnName);
			if (gameObject != null && gameObject.activeSelf)
			{
				UIButtonMessage component = gameObject.GetComponent<UIButtonMessage>();
				if (component != null)
				{
					component.SendMessage("OnClick");
				}
			}
		}
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (this.m_backKyeVaildOKBtn)
		{
			this.SendMessageOnClick("Btn_ok");
		}
		else if (this.m_backKeyVaildNextBtn)
		{
			this.SendMessageOnClick("Btn_next");
		}
		if (msg != null)
		{
			msg.StaySequence();
		}
	}
}
