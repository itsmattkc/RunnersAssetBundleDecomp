using AnimationOrTween;
using Message;
using System;
using System.Collections.Generic;
using Text;
using UI;
using UnityEngine;

public class HudEventResultRaidBoss : HudEventResultParts
{
	private enum RaidResultState
	{
		INIT,
		IN_BG,
		WAIT_HELP_REQUEST,
		WAIT_HELP,
		WAIT_HELP_FAILURE,
		DAMAGE_WINDOW,
		OUT,
		END
	}

	private GameObject m_resultRootObject;

	private HudEventResult.AnimationEndCallback m_callback;

	private Animation m_animation;

	private Animation m_helpRequestAnimation;

	private HudEventResult.AnimType m_currentAnimType;

	private RaidBossInfo m_info;

	private GameResultScoreInterporate m_score;

	private GameResultScoreInterporate m_redRingScore;

	private UIImageButton m_DamageDetailsButton;

	private UIToggle m_helpRequestToggle;

	private RaidBossDamageRewardWindow m_raidBossDamageWindow;

	private RaidBosshelpRequestWindow m_helpRequestWindow;

	private bool m_isDamageDetailsWindowOpen;

	private bool m_isHelpRequestWindowOpen;

	private bool m_isHelpRequestIn;

	private bool m_isHelpRequestReady;

	private List<ServerEventRaidBossDesiredState> m_desiredList;

	private HudEventResultRaidBoss.RaidResultState m_raidResultState;

	private bool m_isBackkeyEnable = true;

	public override bool IsBackkeyEnable()
	{
		return this.m_isBackkeyEnable;
	}

	public override void Init(GameObject resultRootObject, long beforeTotalPoint, HudEventResult.AnimationEndCallback callback)
	{
		global::Debug.Log("HudEventResultRaidBoss:Init");
		this.m_resultRootObject = resultRootObject;
		this.m_callback = callback;
		this.m_isDamageDetailsWindowOpen = false;
		this.m_isHelpRequestWindowOpen = false;
		this.m_isHelpRequestReady = false;
		this.m_isBackkeyEnable = true;
		this.m_info = EventManager.Instance.RaidBossInfo;
		if (this.m_info != null)
		{
			this.m_animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "EventResult_Anim");
			if (this.m_animation == null)
			{
				return;
			}
			this.m_helpRequestAnimation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "help_request");
			if (this.m_helpRequestAnimation != null)
			{
				this.m_helpRequestAnimation.gameObject.SetActive(false);
			}
			this.m_DamageDetailsButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_resultRootObject, "Btn_damage_details");
			if (this.m_DamageDetailsButton != null)
			{
				this.m_DamageDetailsButton.isEnabled = false;
				UIButtonMessage uIButtonMessage = this.m_DamageDetailsButton.gameObject.GetComponent<UIButtonMessage>();
				if (uIButtonMessage == null)
				{
					uIButtonMessage = this.m_DamageDetailsButton.gameObject.AddComponent<UIButtonMessage>();
				}
				if (uIButtonMessage != null)
				{
					uIButtonMessage.enabled = true;
					uIButtonMessage.trigger = UIButtonMessage.Trigger.OnClick;
					uIButtonMessage.target = base.gameObject;
					uIButtonMessage.functionName = "OnClickDamageDetailsButton";
				}
				if (GameResultUtility.GetBossDestroyFlag())
				{
					string text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_reward_get");
					UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_DamageDetailsButton.gameObject, "Lbl_word_damage_details");
					if (uILabel != null)
					{
						uILabel.text = text;
						UILocalizeText component = uILabel.gameObject.GetComponent<UILocalizeText>();
						if (component != null)
						{
							component.enabled = false;
						}
					}
					UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_DamageDetailsButton.gameObject, "Lbl_word_damage_details_sh");
					if (uILabel2 != null)
					{
						uILabel2.text = text;
						UILocalizeText component2 = uILabel2.gameObject.GetComponent<UILocalizeText>();
						if (component2 != null)
						{
							component2.enabled = false;
						}
					}
				}
			}
			this.m_helpRequestToggle = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(this.m_resultRootObject, "img_check_box_0");
			if (this.m_helpRequestToggle != null)
			{
				UIButtonMessage uIButtonMessage2 = this.m_helpRequestToggle.gameObject.AddComponent<UIButtonMessage>();
				if (uIButtonMessage2 != null)
				{
					uIButtonMessage2.enabled = true;
					uIButtonMessage2.trigger = UIButtonMessage.Trigger.OnClick;
					uIButtonMessage2.target = base.gameObject;
					uIButtonMessage2.functionName = "OnClickHelpRequestButton";
				}
			}
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_resultRootObject, "object_get");
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
	}

	private bool isHelpRequest()
	{
		bool flag = true;
		if (this.m_info != null && RaidBossInfo.currentRaidData != null && RaidBossInfo.currentRaidData.IsDiscoverer())
		{
			flag = GameResultUtility.GetBossDestroyFlag();
			flag |= RaidBossInfo.currentRaidData.crowded;
		}
		return flag;
	}

	public override void PlayAnimation(HudEventResult.AnimType animType)
	{
		this.m_currentAnimType = animType;
		global::Debug.Log("HudEventResultRaidBoss:PlayAnimation >> " + this.m_currentAnimType);
		switch (animType)
		{
		case HudEventResult.AnimType.IN:
			this.m_isHelpRequestIn = false;
			if (!this.isHelpRequest())
			{
				if (this.m_helpRequestAnimation != null)
				{
					this.m_helpRequestAnimation.gameObject.SetActive(true);
					ActiveAnimation activeAnimation = ActiveAnimation.Play(this.m_helpRequestAnimation, "ui_EventResult_raidboss_help_request_intro_Anim", Direction.Forward, true);
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.AnimationFinishCallback), true);
					this.m_isHelpRequestIn = true;
				}
				else
				{
					global::Debug.Log("HudEventResultRaidBoss:PlayAnimation >> help request animation not found!!");
					this.m_callback(HudEventResult.AnimType.OUT_WAIT);
				}
			}
			else
			{
				this.m_callback(HudEventResult.AnimType.OUT_WAIT);
			}
			break;
		case HudEventResult.AnimType.IN_BONUS:
			this.AnimationFinishCallback();
			break;
		case HudEventResult.AnimType.WAIT_ADD_COLLECT_OBJECT:
			this.SetEnableDamageDetailsButton(true);
			break;
		case HudEventResult.AnimType.ADD_COLLECT_OBJECT:
			this.AnimationFinishCallback();
			break;
		case HudEventResult.AnimType.SHOW_QUOTA_LIST:
			this.AnimationFinishCallback();
			break;
		case HudEventResult.AnimType.OUT:
			if (this.m_isHelpRequestIn)
			{
				ActiveAnimation activeAnimation2 = ActiveAnimation.Play(this.m_helpRequestAnimation, "ui_EventResult_raidboss_help_request_outro_Anim", Direction.Forward, true);
				if (this.m_helpRequestToggle != null)
				{
					if (this.m_helpRequestToggle.value)
					{
						if (this.m_info != null && !this.m_isHelpRequestReady)
						{
							ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
							if (loggedInServerInterface != null)
							{
								List<string> list = new List<string>();
								SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
								if (socialInterface != null && socialInterface.IsLoggedIn)
								{
									List<SocialUserData> friendList = socialInterface.FriendList;
									foreach (SocialUserData current in friendList)
									{
										string gameId = current.CustomData.GameId;
										list.Add(gameId);
									}
								}
								loggedInServerInterface.RequestServerGetEventRaidBossDesiredList(EventManager.Instance.Id, RaidBossInfo.currentRaidData.id, list, base.gameObject);
								this.m_raidResultState = HudEventResultRaidBoss.RaidResultState.WAIT_HELP_REQUEST;
								this.m_isHelpRequestReady = true;
							}
						}
					}
					else
					{
						this.m_raidResultState = HudEventResultRaidBoss.RaidResultState.OUT;
						if (activeAnimation2 != null)
						{
							EventDelegate.Add(activeAnimation2.onFinished, new EventDelegate.Callback(this.AnimationFinishCallback), true);
						}
					}
				}
			}
			else
			{
				this.AnimationFinishCallback();
				this.m_raidResultState = HudEventResultRaidBoss.RaidResultState.OUT;
			}
			if (this.m_raidBossDamageWindow != null)
			{
				this.m_raidBossDamageWindow.OnClickClose();
			}
			this.SetEnableDamageDetailsButton(false);
			break;
		}
	}

	private void Update()
	{
		switch (this.m_raidResultState)
		{
		case HudEventResultRaidBoss.RaidResultState.WAIT_HELP:
			if (this.m_helpRequestWindow != null && this.m_isHelpRequestWindowOpen && this.m_helpRequestWindow.isFinished())
			{
				this.m_isHelpRequestWindowOpen = false;
				this.m_isHelpRequestIn = false;
				this.m_raidResultState = HudEventResultRaidBoss.RaidResultState.OUT;
				this.AnimationFinishCallback();
			}
			break;
		case HudEventResultRaidBoss.RaidResultState.WAIT_HELP_FAILURE:
			if (GeneralWindow.IsCreated("HelpRequestFailure") && GeneralWindow.IsButtonPressed)
			{
				this.m_isHelpRequestWindowOpen = false;
				this.m_isHelpRequestIn = false;
				this.m_raidResultState = HudEventResultRaidBoss.RaidResultState.OUT;
				this.AnimationFinishCallback();
				GeneralWindow.Close();
			}
			break;
		}
		if (!this.m_isBackkeyEnable && this.m_raidBossDamageWindow != null && !RaidBossDamageRewardWindow.IsEnabled())
		{
			this.m_isBackkeyEnable = true;
		}
	}

	private void AnimationFinishCallback()
	{
		if (this.m_callback != null)
		{
			this.m_callback(this.m_currentAnimType);
		}
	}

	private void QuotaPlayEndCallback()
	{
		if (this.m_callback != null)
		{
			this.m_callback(this.m_currentAnimType);
		}
	}

	private void OnClickDamageDetailsButton()
	{
		global::Debug.Log("HudEventResultRaidBoss:OnClickDamageDetailsButton");
		SoundManager.SePlay("sys_menu_decide", "SE");
		if (this.m_info != null)
		{
			if (!this.m_isDamageDetailsWindowOpen)
			{
				this.m_isDamageDetailsWindowOpen = true;
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					int eventId = -1;
					if (EventManager.Instance != null)
					{
						eventId = EventManager.Instance.Id;
					}
					loggedInServerInterface.RequestServerGetEventRaidBossUserList(eventId, RaidBossInfo.currentRaidData.id, base.gameObject);
				}
				else
				{
					this.ServerGetEventRaidBossUserList_Succeeded(null);
				}
			}
			else
			{
				this.DamageDetailsWindowOpen();
			}
			this.m_isBackkeyEnable = false;
		}
	}

	private void DamageDetailsWindowOpen()
	{
		this.m_raidBossDamageWindow = RaidBossDamageRewardWindow.Create(RaidBossInfo.currentRaidData, null);
		if (this.m_raidBossDamageWindow != null)
		{
			GameObject gameObject = this.m_raidBossDamageWindow.gameObject;
			if (gameObject != null)
			{
				Vector3 localPosition = gameObject.transform.localPosition;
				Vector3 localScale = gameObject.transform.localScale;
				gameObject.transform.parent = this.m_resultRootObject.transform;
				gameObject.transform.localPosition = localPosition;
				gameObject.transform.localScale = localScale;
			}
			this.m_raidBossDamageWindow.useResult = true;
		}
	}

	private void OnClickHelpRequestButton()
	{
		global::Debug.Log("HudEventResultRaidBoss:OnClickHelpRequestButton");
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void ServerGetEventRaidBossUserList_Succeeded(MsgGetEventRaidBossUserListSucceed msg)
	{
		this.DamageDetailsWindowOpen();
	}

	private void ServerGetEventRaidBossDesiredList_Succeeded(MsgEventRaidBossDesiredListSucceed msg)
	{
		this.m_desiredList = msg.m_desiredList;
		if (this.m_desiredList != null)
		{
			if (this.m_desiredList.Count > 0)
			{
				this.m_raidResultState = HudEventResultRaidBoss.RaidResultState.WAIT_HELP;
				this.m_helpRequestWindow = RaidBosshelpRequestWindow.Create(this.m_desiredList);
				if (this.m_helpRequestWindow != null)
				{
					GameObject gameObject = this.m_helpRequestWindow.gameObject;
					if (gameObject != null)
					{
						Vector3 localPosition = gameObject.transform.localPosition;
						Vector3 localScale = gameObject.transform.localScale;
						gameObject.transform.parent = this.m_resultRootObject.transform;
						gameObject.transform.localPosition = localPosition;
						gameObject.transform.localScale = localScale;
					}
					this.m_isHelpRequestWindowOpen = true;
				}
			}
			else
			{
				this.m_raidResultState = HudEventResultRaidBoss.RaidResultState.WAIT_HELP_FAILURE;
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					name = "HelpRequestFailure",
					buttonType = GeneralWindow.ButtonType.Ok,
					caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Result", "Lbl_caption_help_request_failure").text,
					message = TextManager.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Result", "Lbl_help_request_failure_text").text
				});
			}
		}
	}

	public void SetEnableDamageDetailsButton(bool flag)
	{
		if (this.m_DamageDetailsButton != null)
		{
			this.m_DamageDetailsButton.isEnabled = flag;
		}
	}
}
