using AnimationOrTween;
using DataTable;
using System;
using Text;
using UnityEngine;

public class ChaoMergeWindow : WindowBase
{
	private bool m_isSetup;

	private int m_chaoId;

	private int m_level;

	private int m_rarity;

	private bool m_backKeyVaild;

	private bool m_isEnd;

	private RouletteUtility.AchievementType m_achievementType;

	private static readonly string[] ChaoIconNameList = new string[]
	{
		"img_chao_1",
		"img_chao_2",
		"img_chao_3"
	};

	private HudFlagWatcher m_SeFlagFog;

	public bool isSetuped
	{
		get
		{
			return this.m_isSetup;
		}
		set
		{
			this.m_isSetup = false;
		}
	}

	public bool IsPlayEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	public void PlayStart(int chaoId, int level, int rarity, RouletteUtility.AchievementType achievement = RouletteUtility.AchievementType.NONE)
	{
		RouletteManager.OpenRouletteWindow();
		this.m_chaoId = chaoId;
		this.m_level = level;
		this.m_backKeyVaild = false;
		this.m_isEnd = false;
		this.m_achievementType = achievement;
		if (!this.m_isSetup)
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
			this.m_SeFlagFog = new HudFlagWatcher();
			GameObject watchObject = GameObjectUtil.FindChildGameObject(base.gameObject, "SE_flag_fog");
			this.m_SeFlagFog.Setup(watchObject, new HudFlagWatcher.ValueChangeCallback(this.SeFlagFogCallback));
			this.m_isSetup = true;
		}
		base.gameObject.SetActive(true);
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "skip_collider");
		if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
		Animation component = base.gameObject.GetComponent<Animation>();
		if (component != null)
		{
			component.Stop();
			ActiveAnimation activeAnimation = ActiveAnimation.Play(component, "ui_menu_chao_merge_Window_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.InAnimationEndCallback), true);
			UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, "Btn_ok");
			if (uIImageButton != null)
			{
				uIImageButton.isEnabled = false;
			}
		}
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_chao_bg_rare");
		if (uISprite != null)
		{
			ChaoWindowUtility.ChangeRaritySpriteFromServerChaoId(uISprite, this.m_chaoId);
		}
		int idFromServerId = ChaoWindowUtility.GetIdFromServerId(this.m_chaoId);
		string[] chaoIconNameList = ChaoMergeWindow.ChaoIconNameList;
		for (int i = 0; i < chaoIconNameList.Length; i++)
		{
			string name = chaoIconNameList[i];
			UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(base.gameObject, name);
			if (uITexture != null)
			{
				ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(uITexture, null, true);
				ChaoTextureManager.Instance.GetTexture(idFromServerId, info);
			}
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_chao_1_lv");
		if (uILabel != null)
		{
			uILabel.text = TextUtility.GetTextLevel((this.m_level - 1).ToString());
		}
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_chao_2_lv");
		if (uILabel2 != null)
		{
			uILabel2.text = TextUtility.GetTextLevel("0");
		}
		UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_chao_3_lv");
		if (uILabel3 != null)
		{
			uILabel3.text = TextUtility.GetTextLevel(this.m_level.ToString());
		}
		UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_chao_name");
		if (uILabel4 != null)
		{
			string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_CHAO_TEXT, "Chao", ChaoWindowUtility.GetChaoLabelName(this.m_chaoId)).text;
			uILabel4.text = text;
		}
		UILabel uILabel5 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_chao_lv");
		if (uILabel5 != null)
		{
			uILabel5.text = TextUtility.GetTextLevel(this.m_level.ToString());
		}
		UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_type_icon");
		if (uISprite2 != null)
		{
			ChaoData chaoData = ChaoTable.GetChaoData(idFromServerId);
			if (chaoData != null)
			{
				CharacterAttribute charaAtribute = chaoData.charaAtribute;
				string spriteName = "ui_chao_set_type_icon_" + charaAtribute.ToString().ToLower();
				uISprite2.spriteName = spriteName;
			}
		}
		UILabel uILabel6 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_type");
		if (uILabel6 != null)
		{
			ChaoData chaoData2 = ChaoTable.GetChaoData(idFromServerId);
			if (chaoData2 != null)
			{
				CharacterAttribute charaAtribute2 = chaoData2.charaAtribute;
				string cellName = charaAtribute2.ToString().ToLower();
				string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaAtribute", cellName).text;
				uILabel6.text = text2;
			}
		}
		UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_bonus_icon");
		if (uISprite3 != null)
		{
			uISprite3.enabled = false;
		}
		UILabel uILabel7 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_bonus");
		if (uILabel7 != null)
		{
			uILabel7.text = HudUtility.GetChaoGrowAbilityText(idFromServerId, -1);
		}
		SoundManager.SePlay("sys_window_open", "SE");
		UIDraggablePanel uIDraggablePanel = GameObjectUtil.FindChildGameObjectComponent<UIDraggablePanel>(base.gameObject, "window_chaoset_alpha_clip");
		if (uIDraggablePanel != null)
		{
			uIDraggablePanel.ResetPosition();
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "eff_4");
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "eff_6");
		GameObject gameObject4 = GameObjectUtil.FindChildGameObject(base.gameObject, "eff_7");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(true);
		}
		if (gameObject3 != null)
		{
			gameObject3.SetActive(true);
		}
		if (gameObject4 != null)
		{
			gameObject4.SetActive(true);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_SeFlagFog != null)
		{
			this.m_SeFlagFog.Update();
		}
	}

	private void SeFlagFogCallback(float newValue, float prevValue)
	{
		if (newValue == 1f)
		{
			ChaoWindowUtility.PlaySEChaoUnite(this.m_chaoId);
		}
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
			ActiveAnimation activeAnimation = ActiveAnimation.Play(component, "ui_menu_chao_merge_Window_outro_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OutAnimationEndCallback), true);
		}
		this.m_backKeyVaild = false;
	}

	private void InAnimationEndCallback()
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
		this.m_backKeyVaild = true;
	}

	private void OutAnimationEndCallback()
	{
		this.DeleteChaoTexture();
		base.gameObject.SetActive(false);
		this.m_isEnd = true;
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
		string[] chaoIconNameList = ChaoMergeWindow.ChaoIconNameList;
		for (int i = 0; i < chaoIconNameList.Length; i++)
		{
			string name = chaoIconNameList[i];
			UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(base.gameObject, name);
			if (uITexture != null)
			{
				uITexture.enabled = true;
				uITexture.mainTexture = data.tex;
			}
		}
	}

	private void DeleteChaoTexture()
	{
		string[] chaoIconNameList = ChaoMergeWindow.ChaoIconNameList;
		for (int i = 0; i < chaoIconNameList.Length; i++)
		{
			string name = chaoIconNameList[i];
			UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(base.gameObject, name);
			if (uITexture != null)
			{
				uITexture.enabled = false;
				uITexture.mainTexture = null;
			}
		}
		ChaoTextureManager.Instance.RemoveChaoTexture(this.m_chaoId);
	}

	public override void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (msg != null)
		{
			msg.StaySequence();
		}
		if (this.m_backKeyVaild)
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
	}
}
