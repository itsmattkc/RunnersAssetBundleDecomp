using AnimationOrTween;
using System;
using System.Diagnostics;
using UnityEngine;

public class HudLoading : MonoBehaviour
{
	[SerializeField]
	private GameObject m_loadingGameObject;

	[SerializeField]
	private Animation m_screenAnimation;

	[SerializeField]
	private UILabel m_titleLabel;

	[SerializeField]
	private UILabel m_tipsLabel;

	[SerializeField]
	private UITexture m_texture;

	[SerializeField]
	private GameObject m_Chao1GameObject;

	[SerializeField]
	private UILabel m_chao1BonusNameLabel;

	[SerializeField]
	private GameObject m_Chao1SPGameObject;

	[SerializeField]
	private UILabel m_chao1SPBonusNameLabel;

	[SerializeField]
	private UITexture m_chao1Texture;

	[SerializeField]
	private GameObject m_Chao2GameObject;

	[SerializeField]
	private UILabel m_chao2BonusNameLabel;

	[SerializeField]
	private GameObject m_Chao2SPGameObject;

	[SerializeField]
	private UILabel m_chao2SPBonusNameLabel;

	[SerializeField]
	private UITexture m_chao2Texture;

	[SerializeField]
	private GameObject m_ChaoCountGameObject;

	[SerializeField]
	private UILabel m_chaoCountLabel;

	[SerializeField]
	private UILabel m_chaoCountLabel2;

	[SerializeField]
	private UILabel m_chaoCountScoreLabel;

	[SerializeField]
	private GameObject m_campaignBonusGameObject;

	[SerializeField]
	private UILabel m_campaignBonus;

	private static HudLoading s_instance;

	private Action m_onFinishedStartAnimationAction;

	private Action m_onFinishedEndAnimationAction;

	private bool m_optionTutorial;

	private void Awake()
	{
		HudLoading.s_instance = this;
	}

	public void Play(string clipName, EventDelegate.Callback callback)
	{
		ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_screenAnimation, clipName, Direction.Forward);
		if (activeAnimation != null)
		{
			EventDelegate.Add(activeAnimation.onFinished, callback, true);
		}
	}

	private void SetView()
	{
		bool flag = StageModeManager.Instance != null && StageModeManager.Instance.IsQuickMode();
		this.m_optionTutorial = false;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Anchor_1_TL");
		if (gameObject != null)
		{
			bool flag2 = false;
			if (EventManager.Instance != null)
			{
				if (EventManager.Instance.Type == EventManager.EventType.QUICK)
				{
					flag2 = flag;
				}
				else if (EventManager.Instance.Type == EventManager.EventType.BGM)
				{
					EventStageData stageData = EventManager.Instance.GetStageData();
					if (stageData != null)
					{
						flag2 = ((!flag) ? stageData.IsEndlessModeBGM() : stageData.IsQuickModeBGM());
					}
				}
			}
			if (flag2 && AtlasManager.Instance != null)
			{
				AtlasManager.Instance.ReplaceAtlasForStage();
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
		GameObject gameObject2 = GameObject.Find("LoadingInfo");
		if (gameObject2 != null)
		{
			LoadingInfo component = gameObject2.GetComponent<LoadingInfo>();
			if (component != null)
			{
				this.m_titleLabel.text = component.GetInfo().m_titleText;
				this.m_tipsLabel.text = component.GetInfo().m_mainText;
				if (!flag && component.GetInfo().m_texture != null)
				{
					this.m_texture.mainTexture = component.GetInfo().m_texture;
				}
				this.m_optionTutorial = component.GetInfo().m_optionTutorial;
			}
			UnityEngine.Object.Destroy(gameObject2);
		}
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			int mainChaoID = instance.PlayerData.MainChaoID;
			if (mainChaoID >= 0)
			{
				this.m_Chao1GameObject.SetActive(true);
				this.m_chao1BonusNameLabel.text = HudUtility.GetChaoLoadingAbilityText(mainChaoID);
				string chaoSPLoadingAbilityText = HudUtility.GetChaoSPLoadingAbilityText(mainChaoID);
				if (string.IsNullOrEmpty(chaoSPLoadingAbilityText))
				{
					this.m_Chao1SPGameObject.SetActive(false);
				}
				else
				{
					this.m_Chao1SPGameObject.SetActive(true);
					this.m_chao1SPBonusNameLabel.text = chaoSPLoadingAbilityText;
				}
			}
			else
			{
				this.m_Chao1GameObject.SetActive(false);
				this.m_Chao1SPGameObject.SetActive(false);
			}
			HudUtility.SetChaoTexture(this.m_chao1Texture, mainChaoID, true);
			int subChaoID = instance.PlayerData.SubChaoID;
			if (subChaoID >= 0)
			{
				this.m_Chao2GameObject.SetActive(true);
				this.m_chao2BonusNameLabel.text = HudUtility.GetChaoLoadingAbilityText(subChaoID);
				string chaoSPLoadingAbilityText2 = HudUtility.GetChaoSPLoadingAbilityText(subChaoID);
				if (string.IsNullOrEmpty(chaoSPLoadingAbilityText2))
				{
					this.m_Chao2SPGameObject.SetActive(false);
				}
				else
				{
					this.m_Chao2SPGameObject.SetActive(true);
					this.m_chao2SPBonusNameLabel.text = chaoSPLoadingAbilityText2;
				}
			}
			else
			{
				this.m_Chao2GameObject.SetActive(false);
				this.m_Chao2SPGameObject.SetActive(false);
			}
			HudUtility.SetChaoTexture(this.m_chao2Texture, subChaoID, true);
		}
		StageAbilityManager stageAbilityManager = UnityEngine.Object.FindObjectOfType<StageAbilityManager>();
		if (stageAbilityManager != null)
		{
			stageAbilityManager.RecalcAbilityVaue();
			int chaoCount = stageAbilityManager.GetChaoCount();
			if (chaoCount > 0 && !this.m_optionTutorial)
			{
				this.m_ChaoCountGameObject.SetActive(true);
				this.m_chaoCountLabel.text = chaoCount.ToString();
				this.m_chaoCountLabel2.text = chaoCount.ToString();
				this.m_chaoCountScoreLabel.text = HudUtility.GetChaoCountBonusText(stageAbilityManager.GetChaoCountBonusValue());
			}
			else
			{
				this.m_ChaoCountGameObject.SetActive(false);
				this.m_chaoCountLabel.text = string.Empty;
				this.m_chaoCountLabel2.text = string.Empty;
				this.m_chaoCountScoreLabel.text = string.Empty;
			}
			if (this.m_optionTutorial)
			{
				this.m_campaignBonusGameObject.SetActive(false);
			}
			else
			{
				float num = stageAbilityManager.CampaignValueRate;
				if (num != 0f)
				{
					num *= 100f;
					this.m_campaignBonus.text = "＋" + num.ToString() + "％";
					if (AtlasManager.Instance != null)
					{
						UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_campaignBonusGameObject, "img_word_campaign_bonus");
						if (uISprite != null)
						{
							AtlasManager.Instance.ReplaceAtlasForLoading(uISprite.atlas);
						}
					}
					this.m_campaignBonusGameObject.SetActive(true);
				}
				else
				{
					this.m_campaignBonusGameObject.SetActive(false);
				}
			}
		}
	}

	public static void StartScreen(Action onFinishedAnimationAction = null)
	{
		if (HudLoading.s_instance != null)
		{
			HudLoading.s_instance.SetView();
			HudLoading.s_instance.m_loadingGameObject.SetActive(true);
			HudLoading.s_instance.m_onFinishedStartAnimationAction = onFinishedAnimationAction;
			HudLoading.s_instance.Play("ui_load_intro_Anim", new EventDelegate.Callback(HudLoading.s_instance.OnFinishedStartAnimation));
		}
	}

	public static void EndScreen(Action onFinishedAnimationAction = null)
	{
		if (HudLoading.s_instance != null)
		{
			HudLoading.s_instance.m_onFinishedEndAnimationAction = onFinishedAnimationAction;
			HudLoading.s_instance.Play("ui_load_outro_Anim", new EventDelegate.Callback(HudLoading.s_instance.OnFinishedEndAnimation));
		}
	}

	private void OnFinishedStartAnimation()
	{
		if (this.m_onFinishedStartAnimationAction != null)
		{
			this.m_onFinishedStartAnimationAction();
		}
	}

	private void OnFinishedEndAnimation()
	{
		if (this.m_onFinishedEndAnimationAction != null)
		{
			this.m_onFinishedEndAnimationAction();
		}
		UnityEngine.Object.Destroy(base.gameObject);
		EventUtility.DestroyLoadingFaceTexture();
		HudLoading.s_instance = null;
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLog(string s)
	{
		global::Debug.Log("@ms " + s);
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLogWarning(string s)
	{
		global::Debug.LogWarning("@ms " + s);
	}
}
