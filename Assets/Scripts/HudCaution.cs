using AnimationOrTween;
using Message;
using System;
using System.Diagnostics;
using UnityEngine;

public class HudCaution : MonoBehaviour
{
	public enum Type
	{
		GO,
		SPEEDUP,
		BOSS,
		COMBO_N,
		TRICK0,
		TRICK1,
		TRICK2,
		TRICK3,
		TRICK4,
		BONUS_N,
		COMBO_BONUS_N,
		TRICK_BONUS_N,
		COUNTDOWN,
		MAP_BOSS_CLEAR,
		MAP_BOSS_FAILED,
		STAGE_OUT,
		STAGE_IN,
		ZERO_POINT_TEST,
		GET_ITEM,
		DESTROY_ENEMY,
		NO_RING,
		WISPBOOST,
		EVENTBOSS,
		EXTREMEMODE,
		COMBOITEM_BONUS_N,
		GET_TIMER,
		COUNT
	}

	[Serializable]
	private class AnimInfo
	{
		[SerializeField]
		public Animation m_animation;

		[SerializeField]
		public string m_clipName;

		[SerializeField]
		public UILabel m_label;

		[SerializeField]
		public string m_labelStringFormat;

		[SerializeField]
		public UISlider m_slider;

		[SerializeField]
		public UISprite m_sprite;

		[SerializeField]
		public UISprite m_sprite2;

		[SerializeField]
		public bool m_finishDisable;
	}

	private class DelayWorldToScreenPoint
	{
		private Vector3 m_beforeTargetPosition;

		public Vector2 GetScreenPositon(GameObject targetGameObject, Camera targetCamera, Camera uiCamera)
		{
			Vector2 result = new Vector2(-100f, -100f);
			if (targetGameObject != null && targetCamera != null && uiCamera != null)
			{
				Vector3 beforeTargetPosition = this.m_beforeTargetPosition;
				this.m_beforeTargetPosition = targetGameObject.transform.localPosition;
				Vector3 position = targetCamera.WorldToScreenPoint(beforeTargetPosition);
				position.z = 0f;
				result = uiCamera.ScreenToWorldPoint(position);
			}
			return result;
		}
	}

	private static HudCaution instance;

	[SerializeField]
	private GameObject m_playerAnchorGameObject;

	[SerializeField]
	private GameObject m_enemyAnchorGameObject;

	[SerializeField]
	private HudCaution.AnimInfo[] m_animInfos = new HudCaution.AnimInfo[26];

	[SerializeField]
	private UISprite m_bossAttenion;

	[SerializeField]
	private UISprite m_raidBossAttenion;

	private Camera m_gameMainCamera;

	private StageItemManager m_stageItemManager;

	private ObjBossEventBossParameter m_bossParameter;

	private HudCaution.DelayWorldToScreenPoint m_playerDelayWorldToScreenPoint;

	private GameObject m_enemyGameObject;

	private Camera m_uiCamera;

	private UISlider m_slider;

	private UISlider m_boostSlider;

	public static HudCaution Instance
	{
		get
		{
			return HudCaution.instance;
		}
	}

	private void Awake()
	{
		this.SetInstance();
	}

	private void OnDestroy()
	{
		if (HudCaution.instance == this)
		{
			HudCaution.instance = null;
		}
	}

	private void SetInstance()
	{
		if (HudCaution.instance == null)
		{
			HudCaution.instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		this.m_playerDelayWorldToScreenPoint = new HudCaution.DelayWorldToScreenPoint();
		this.m_uiCamera = null;
		this.m_slider = null;
		this.m_boostSlider = null;
		if (this.m_enemyAnchorGameObject != null)
		{
			this.m_enemyAnchorGameObject.SetActive(false);
		}
		if (EventManager.Instance != null && !EventManager.Instance.IsRaidBossStage() && this.m_playerAnchorGameObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_playerAnchorGameObject, "gp_bit_WispBoost");
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
	}

	private void Update()
	{
		if (this.m_stageItemManager == null)
		{
			this.m_stageItemManager = StageItemManager.Instance;
		}
		if (this.m_uiCamera == null)
		{
			this.m_uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		if (this.m_gameMainCamera == null)
		{
			this.m_gameMainCamera = GameObjectUtil.FindGameObjectComponent<Camera>("GameMainCamera");
		}
		Vector2 screenPositon = this.m_playerDelayWorldToScreenPoint.GetScreenPositon(GameObject.FindWithTag("Player"), this.m_gameMainCamera, this.m_uiCamera);
		this.m_playerAnchorGameObject.transform.position = new Vector3(screenPositon.x, screenPositon.y, 0f);
		if (this.m_slider != null && this.m_stageItemManager != null)
		{
			float cautionItemTimeRate = this.m_stageItemManager.CautionItemTimeRate;
			if (cautionItemTimeRate > 0f)
			{
				this.m_slider.value = cautionItemTimeRate;
			}
			else
			{
				this.m_slider.value = 0f;
				this.m_slider = null;
				ActiveAnimation.Play(this.m_animInfos[12].m_animation, this.m_animInfos[12].m_clipName, Direction.Reverse);
			}
		}
		if (this.m_boostSlider != null && this.m_bossParameter != null)
		{
			float boostRatio = this.m_bossParameter.BoostRatio;
			if (boostRatio > 0f)
			{
				this.m_boostSlider.value = boostRatio;
			}
			else
			{
				this.m_boostSlider.value = 0f;
				this.m_boostSlider = null;
				ActiveAnimation.Play(this.m_animInfos[21].m_animation, this.m_animInfos[21].m_clipName, Direction.Reverse);
			}
		}
	}

	public void SetBossWord(bool bossStage)
	{
		string spriteName = (!bossStage) ? "ui_gp_bit_word_chancetime" : "ui_gp_bit_word_attention";
		if (this.m_bossAttenion != null)
		{
			this.m_bossAttenion.spriteName = spriteName;
		}
		if (this.m_raidBossAttenion != null)
		{
			this.m_raidBossAttenion.spriteName = spriteName;
		}
	}

	public void SetCaution(MsgCaution msg)
	{
		if (msg.m_cautionType < HudCaution.Type.COUNT)
		{
			HudCaution.AnimInfo animInfo = this.m_animInfos[(int)msg.m_cautionType];
			if (animInfo.m_animation != null && !string.IsNullOrEmpty(animInfo.m_clipName))
			{
				HudCaution.Type cautionType;
				if (animInfo.m_label != null)
				{
					cautionType = msg.m_cautionType;
					if (cautionType != HudCaution.Type.COMBOITEM_BONUS_N)
					{
						if (cautionType == HudCaution.Type.GET_TIMER)
						{
							string text = string.Format(animInfo.m_labelStringFormat, msg.m_second);
							if (!animInfo.m_label.enabled)
							{
								animInfo.m_label.enabled = true;
							}
							animInfo.m_label.text = text;
							goto IL_179;
						}
						if (cautionType != HudCaution.Type.BONUS_N)
						{
							if (cautionType != HudCaution.Type.GET_ITEM)
							{
								if (!string.IsNullOrEmpty(animInfo.m_labelStringFormat))
								{
									string text2 = string.Format(animInfo.m_labelStringFormat, msg.m_number);
									animInfo.m_label.text = text2;
								}
								goto IL_179;
							}
							if (animInfo.m_label.enabled)
							{
								animInfo.m_label.enabled = false;
							}
							goto IL_179;
						}
					}
					if (!string.IsNullOrEmpty(animInfo.m_labelStringFormat))
					{
						string text3 = string.Format(animInfo.m_labelStringFormat, msg.m_number);
						if (msg.m_flag)
						{
							animInfo.m_label.text = "[FF0000]" + text3;
						}
						else
						{
							animInfo.m_label.text = text3;
						}
					}
				}
				IL_179:
				float num = 0f;
				cautionType = msg.m_cautionType;
				if (cautionType != HudCaution.Type.COUNTDOWN)
				{
					if (cautionType == HudCaution.Type.WISPBOOST)
					{
						if (animInfo.m_slider != null)
						{
							this.m_bossParameter = msg.m_bossParam;
							if (this.m_bossParameter != null)
							{
								if (animInfo.m_sprite != null && this.m_bossParameter.BoostLevel != WispBoostLevel.NONE)
								{
									animInfo.m_sprite.spriteName = "ui_event_gp_gauge_power_bg_" + (int)this.m_bossParameter.BoostLevel;
								}
								if (animInfo.m_sprite2 != null)
								{
									GameObject parent = GameObjectUtil.FindChildGameObject(base.gameObject, "gp_bit_WispBoost");
									RaidBossBoostGagueColor raidBossBoostGagueColor = GameObjectUtil.FindChildGameObjectComponent<RaidBossBoostGagueColor>(parent, "img_gauge");
									Color color = animInfo.m_sprite2.color;
									switch (this.m_bossParameter.BoostLevel)
									{
									case WispBoostLevel.LEVEL1:
										color = raidBossBoostGagueColor.Level1;
										break;
									case WispBoostLevel.LEVEL2:
										color = raidBossBoostGagueColor.Level2;
										break;
									case WispBoostLevel.LEVEL3:
										color = raidBossBoostGagueColor.Level3;
										break;
									}
									animInfo.m_sprite2.color = color;
								}
								num = this.m_bossParameter.BoostRatio;
								if (num > 0f)
								{
									this.m_boostSlider = animInfo.m_slider;
								}
								if (this.m_boostSlider != null)
								{
									this.m_boostSlider.value = num;
								}
							}
						}
					}
				}
				else if (animInfo.m_slider != null)
				{
					num = msg.m_rate;
					if (num > 0f)
					{
						this.m_slider = animInfo.m_slider;
					}
					if (this.m_slider != null)
					{
						this.m_slider.value = num;
					}
				}
				if (!(animInfo.m_slider != null) || num != 0f)
				{
					cautionType = msg.m_cautionType;
					if (cautionType != HudCaution.Type.COMBO_N)
					{
						if (cautionType != HudCaution.Type.BONUS_N)
						{
							if (cautionType == HudCaution.Type.NO_RING)
							{
								animInfo.m_animation.playAutomatically = true;
								animInfo.m_animation.cullingType = AnimationCullingType.AlwaysAnimate;
								animInfo.m_animation.Rewind();
								animInfo.m_animation.Sample();
								animInfo.m_animation.Play();
								goto IL_4AA;
							}
							if (cautionType != HudCaution.Type.COMBOITEM_BONUS_N)
							{
								this.SetAnimPlay(animInfo);
								goto IL_4AA;
							}
						}
						this.SetAnimPlay(animInfo);
						float length = animInfo.m_animation[animInfo.m_clipName].length;
						animInfo.m_animation[animInfo.m_clipName].time = length * 0.01f;
					}
					else
					{
						this.SetAnimPlay(animInfo);
						float length2 = animInfo.m_animation[animInfo.m_clipName].length;
						if (msg.m_flag)
						{
							animInfo.m_animation[animInfo.m_clipName].time = length2 * 0.05f;
							animInfo.m_animation.Sample();
							animInfo.m_animation.Stop();
						}
						else
						{
							animInfo.m_animation[animInfo.m_clipName].time = length2 * 0.01f;
						}
					}
				}
				IL_4AA:
				cautionType = msg.m_cautionType;
				if (cautionType != HudCaution.Type.GET_ITEM)
				{
					if (cautionType == HudCaution.Type.GET_TIMER)
					{
						if (animInfo.m_sprite != null)
						{
							animInfo.m_sprite.spriteName = "ui_cmn_icon_item_timer_" + msg.m_number;
						}
					}
				}
				else if (animInfo.m_sprite != null)
				{
					animInfo.m_sprite.spriteName = "ui_cmn_icon_item_" + (int)msg.m_itemType;
				}
			}
		}
	}

	private void SetAnimPlay(HudCaution.AnimInfo animInfo)
	{
		if (animInfo != null)
		{
			if (animInfo.m_animation != null && animInfo.m_animation.gameObject != null && !animInfo.m_animation.gameObject.activeSelf)
			{
				animInfo.m_animation.gameObject.SetActive(true);
			}
			animInfo.m_animation.Rewind(animInfo.m_clipName);
			DisableCondition disableCondition = (!animInfo.m_finishDisable) ? DisableCondition.DoNotDisable : DisableCondition.DisableAfterForward;
			ActiveAnimation.Play(animInfo.m_animation, animInfo.m_clipName, Direction.Forward, EnableCondition.DoNothing, disableCondition, false);
		}
	}

	public void SetInvisibleCaution(MsgCaution msg)
	{
		if (msg.m_cautionType < HudCaution.Type.COUNT)
		{
			HudCaution.AnimInfo animInfo = this.m_animInfos[(int)msg.m_cautionType];
			if (animInfo.m_animation != null && !string.IsNullOrEmpty(animInfo.m_clipName))
			{
				HudCaution.Type cautionType = msg.m_cautionType;
				if (cautionType != HudCaution.Type.NO_RING)
				{
					animInfo.m_animation.Play();
					float length = animInfo.m_animation[animInfo.m_clipName].length;
					animInfo.m_animation[animInfo.m_clipName].time = length;
					animInfo.m_animation.Sample();
					animInfo.m_animation.Stop();
				}
				else
				{
					animInfo.m_animation.playAutomatically = false;
					animInfo.m_animation.cullingType = AnimationCullingType.BasedOnRenderers;
					animInfo.m_animation.Rewind();
					animInfo.m_animation.Sample();
					animInfo.m_animation.Stop();
				}
			}
		}
	}

	public void SetMsgExitStage(MsgExitStage msg)
	{
		base.enabled = false;
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
