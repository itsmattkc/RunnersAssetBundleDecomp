using AnimationOrTween;
using System;
using Text;
using UnityEngine;

public class SpEggGetWindow : WindowBase
{
	private enum State
	{
		NONE = -1,
		PLAYING,
		END,
		NUM
	}

	private bool m_isSetuped;

	private bool m_isOpened;

	[SerializeField]
	private UILabel m_caption;

	private SpEggGetPartsBase m_spEggGetParts;

	private HudFlagWatcher m_SeFlagHatch;

	private HudFlagWatcher m_SeFlagBreak;

	private HudFlagWatcher m_SeFlagSpEgg;

	private RouletteUtility.AchievementType m_achievementType;

	private SpEggGetWindow.State m_state = SpEggGetWindow.State.END;

	public bool IsPlayEnd
	{
		get
		{
			return this.m_state == SpEggGetWindow.State.END;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_SeFlagHatch != null)
		{
			this.m_SeFlagHatch.Update();
		}
		if (this.m_SeFlagBreak != null)
		{
			this.m_SeFlagBreak.Update();
		}
		if (this.m_SeFlagSpEgg != null)
		{
			this.m_SeFlagSpEgg.Update();
		}
	}

	public void PlayStart(SpEggGetPartsBase spEggGetParts, RouletteUtility.AchievementType achievement = RouletteUtility.AchievementType.NONE)
	{
		RouletteManager.OpenRouletteWindow();
		this.m_achievementType = achievement;
		this.m_spEggGetParts = spEggGetParts;
		this.m_state = SpEggGetWindow.State.PLAYING;
		this.m_isOpened = false;
		if (this.m_caption != null)
		{
			this.m_caption.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "gw_get_chao_caption").text;
		}
		if (!this.m_isSetuped)
		{
			UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_ok");
			if (uIButtonMessage != null)
			{
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OkButtonClickedCallback";
			}
			UIButtonMessage uIButtonMessage2 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "skip_collider");
			if (uIButtonMessage2 != null)
			{
				uIButtonMessage2.target = base.gameObject;
				uIButtonMessage2.functionName = "SkipButtonClickedCallback";
			}
			this.m_SeFlagHatch = new HudFlagWatcher();
			GameObject watchObject = GameObjectUtil.FindChildGameObject(base.gameObject, "SE_flag");
			this.m_SeFlagHatch.Setup(watchObject, new HudFlagWatcher.ValueChangeCallback(this.SeFlagHatchCallback));
			this.m_SeFlagBreak = new HudFlagWatcher();
			GameObject watchObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "SE_flag_break");
			this.m_SeFlagBreak.Setup(watchObject2, new HudFlagWatcher.ValueChangeCallback(this.SeFlagBreakCallback));
			this.m_SeFlagSpEgg = new HudFlagWatcher();
			GameObject watchObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "SE_flag_spegg");
			this.m_SeFlagSpEgg.Setup(watchObject3, new HudFlagWatcher.ValueChangeCallback(this.SeFlagSpEggCallback));
			this.m_isSetuped = true;
		}
		base.gameObject.SetActive(true);
		if (this.m_spEggGetParts != null)
		{
			this.m_spEggGetParts.Setup(base.gameObject);
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "skip_collider");
		if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(component, Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.InAnimationFinishCallback), true);
			UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_ok");
			if (uIImageButton != null)
			{
				uIImageButton.isEnabled = false;
			}
		}
		UIDraggablePanel uIDraggablePanel = GameObjectUtil.FindChildGameObjectComponent<UIDraggablePanel>(base.gameObject, "window_chaoset_alpha_clip");
		if (uIDraggablePanel != null)
		{
			uIDraggablePanel.ResetPosition();
		}
		SoundManager.SePlay("sys_window_open", "SE");
	}

	private void OkButtonClickedCallback()
	{
		RouletteManager.CloseRouletteWindow();
		SoundManager.SePlay("sys_menu_decide", "SE");
		if (this.m_achievementType != RouletteUtility.AchievementType.NONE)
		{
			RouletteManager.RouletteGetWindowClose(this.m_achievementType, RouletteUtility.NextType.NONE);
			this.m_achievementType = RouletteUtility.AchievementType.NONE;
		}
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(component, "ui_menu_chao_egg_transform_Window_outro_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OutAnimationFinishedCallback), true);
		}
	}

	private void SeFlagHatchCallback(float newValue, float prevValue)
	{
		if (newValue == 1f && this.m_spEggGetParts != null)
		{
			this.m_spEggGetParts.PlaySE(ChaoWindowUtility.SeHatch);
		}
	}

	private void SeFlagBreakCallback(float newValue, float prevValue)
	{
		if (newValue == 1f && this.m_spEggGetParts != null)
		{
			this.m_spEggGetParts.PlaySE(ChaoWindowUtility.SeBreak);
		}
	}

	private void SeFlagSpEggCallback(float newValue, float prevValue)
	{
		if (newValue == 1f)
		{
			if (this.m_spEggGetParts != null)
			{
				this.m_spEggGetParts.PlaySE(ChaoWindowUtility.SeSpEgg);
			}
			if (this.m_caption != null)
			{
				this.m_caption.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "gw_got_special_egg_caption").text;
			}
		}
	}

	private void InAnimationFinishCallback()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "skip_collider");
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_ok");
		if (uIImageButton != null)
		{
			uIImageButton.isEnabled = true;
		}
		this.m_isOpened = true;
	}

	private void OutAnimationFinishedCallback()
	{
		this.DeleteChaoTexture();
		base.gameObject.SetActive(false);
		this.m_isOpened = false;
		this.m_state = SpEggGetWindow.State.END;
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
		if (this.m_caption != null)
		{
			this.m_caption.text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "gw_got_special_egg_caption").text;
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
			ChaoTextureManager.Instance.RemoveChaoTexture(this.m_spEggGetParts.ChaoId);
		}
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (this.m_isOpened)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_ok");
			if (gameObject != null)
			{
				UIButtonMessage component = gameObject.GetComponent<UIButtonMessage>();
				if (component != null)
				{
					component.SendMessage("OnClick");
				}
			}
		}
		if (msg != null)
		{
			msg.StaySequence();
		}
	}
}
