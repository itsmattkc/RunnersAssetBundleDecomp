using AnimationOrTween;
using Message;
using SaveData;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class SettingPartsAcceptInvite : SettingBase
{
	private enum State
	{
		IDLE,
		WAIT_SNS_RESPONSE,
		SETUP_BEFORE,
		WAIT_SETUP,
		SETUP,
		UPDATE,
		DECIDE_FRIEND,
		WAIT_SERVER_RESPONSE,
		WAIT_END_WINDOW_CLOSE
	}

	private bool m_isValid = true;

	private bool m_isEnd;

	private bool m_isSetup;

	private GameObject m_object;

	private UIPlayAnimation m_uiAnimation;

	private List<SettingPartsInviteButton> m_buttons;

	private UIRectItemStorage m_itemStorage;

	private string m_anchorPath;

	private SettingPartsAcceptInvite.State m_state;

	private SocialUserData m_decidedFriendData;

	private MsgSocialFriendListResponse m_msg;

	private readonly string ExcludePathName = "UI Root (2D)/Camera/Anchor_5_MC";

	private void Start()
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
	}

	private void OnDestroy()
	{
		BackKeyManager.RemoveWindowCallBack(base.gameObject);
	}

	protected override void OnSetup(string anthorPath)
	{
		this.m_anchorPath = this.ExcludePathName;
	}

	protected override void OnPlayStart()
	{
		this.m_isEnd = false;
		bool flag = true;
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface == null)
		{
			flag = false;
		}
		else if (!socialInterface.IsLoggedIn)
		{
			flag = false;
		}
		if (!flag)
		{
			this.m_isValid = false;
			this.m_isEnd = true;
			return;
		}
		socialInterface.RequestInvitedFriend(base.gameObject);
	}

	protected override bool OnIsEndPlay()
	{
		return this.m_isEnd;
	}

	protected override void OnUpdate()
	{
		if (!this.m_isValid)
		{
			return;
		}
		if (this.m_isEnd)
		{
			return;
		}
		switch (this.m_state)
		{
		case SettingPartsAcceptInvite.State.SETUP_BEFORE:
			if (this.m_isSetup)
			{
				this.m_state = SettingPartsAcceptInvite.State.WAIT_SETUP;
			}
			else
			{
				this.SetupWindowData();
				this.m_isSetup = true;
				this.m_state = SettingPartsAcceptInvite.State.WAIT_SETUP;
			}
			break;
		case SettingPartsAcceptInvite.State.WAIT_SETUP:
			this.m_state = SettingPartsAcceptInvite.State.SETUP;
			break;
		case SettingPartsAcceptInvite.State.SETUP:
		{
			if (this.m_itemStorage != null && this.m_msg != null)
			{
				this.m_itemStorage.maxRows = this.m_msg.m_friends.Count;
				this.m_itemStorage.Restart();
			}
			List<GameObject> list = GameObjectUtil.FindChildGameObjects(this.m_object, "ui_option_window_invite_scroll(Clone)");
			if (list == null)
			{
				return;
			}
			this.m_buttons.Clear();
			if (this.m_msg != null)
			{
				List<SocialUserData> friends = this.m_msg.m_friends;
				for (int i = 0; i < friends.Count; i++)
				{
					SocialUserData socialUserData = friends[i];
					if (socialUserData != null)
					{
						GameObject gameObject = list[i];
						if (!(gameObject == null))
						{
							SettingPartsInviteButton settingPartsInviteButton = gameObject.AddComponent<SettingPartsInviteButton>();
							settingPartsInviteButton.Setup(socialUserData, new SettingPartsInviteButton.ButtonPressedCallback(this.InviteButtonPressedCallback));
							this.m_buttons.Add(settingPartsInviteButton);
						}
					}
				}
			}
			if (this.m_object != null)
			{
				this.m_object.SetActive(true);
			}
			if (this.m_uiAnimation != null)
			{
				this.m_uiAnimation.Play(true);
			}
			SoundManager.SePlay("sys_window_open", "SE");
			this.m_state = SettingPartsAcceptInvite.State.UPDATE;
			break;
		}
		case SettingPartsAcceptInvite.State.DECIDE_FRIEND:
			if (GeneralWindow.IsYesButtonPressed)
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					string gameId = this.m_decidedFriendData.CustomData.GameId;
					loggedInServerInterface.RequestServerSetInviteCode(gameId, base.gameObject);
				}
				this.m_decidedFriendData = null;
				this.m_state = SettingPartsAcceptInvite.State.WAIT_SERVER_RESPONSE;
				GeneralWindow.Close();
			}
			else if (GeneralWindow.IsNoButtonPressed)
			{
				this.m_decidedFriendData = null;
				this.m_state = SettingPartsAcceptInvite.State.UPDATE;
				GeneralWindow.Close();
			}
			break;
		case SettingPartsAcceptInvite.State.WAIT_END_WINDOW_CLOSE:
			if (GeneralWindow.IsCreated("CreateEndAcceptWindow") && GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
				this.OnClickCancelButton();
				this.m_state = SettingPartsAcceptInvite.State.UPDATE;
			}
			break;
		}
	}

	protected void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (this.m_object != null && this.m_object.activeSelf && this.m_state == SettingPartsAcceptInvite.State.UPDATE)
		{
			if (msg != null)
			{
				msg.StaySequence();
			}
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_object, "Btn_close");
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

	private void OnClickCancelButton()
	{
		if (this.m_object != null)
		{
			Animation component = this.m_object.GetComponent<Animation>();
			if (component != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(component, Direction.Reverse);
				if (activeAnimation != null)
				{
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinishedAnimationCallback), true);
				}
			}
		}
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void InviteButtonPressedCallback(SocialUserData friendData)
	{
		GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
		info.buttonType = GeneralWindow.ButtonType.YesNo;
		TextManager.TextType type = TextManager.TextType.TEXTTYPE_FIXATION_TEXT;
		info.caption = TextUtility.GetText(type, "FaceBook", "ui_Lbl_verification");
		info.message = TextUtility.GetText(type, "FaceBook", "ui_Lbl_accept_invite_text", "{FRIEND_NAME}", friendData.Name);
		GeneralWindow.Create(info);
		this.m_decidedFriendData = friendData;
		this.m_state = SettingPartsAcceptInvite.State.DECIDE_FRIEND;
	}

	private void RequestInviteListEndCallback(MsgSocialFriendListResponse msg)
	{
		if (msg == null)
		{
			return;
		}
		if (msg.m_result.IsError)
		{
			this.m_isEnd = true;
			return;
		}
		this.m_msg = msg;
		this.m_state = SettingPartsAcceptInvite.State.SETUP_BEFORE;
	}

	private void ServerSetInviteCode_Succeeded(MsgGetNormalIncentiveSucceed msg)
	{
		HudMenuUtility.SaveSystemDataFlagStatus(SystemData.FlagStatus.FRIEDN_ACCEPT_INVITE);
		this.CreateEndAcceptWindow();
		this.m_state = SettingPartsAcceptInvite.State.WAIT_END_WINDOW_CLOSE;
	}

	private void ServerSetInviteCode_Failed(MsgServerConnctFailed msg)
	{
		this.m_state = SettingPartsAcceptInvite.State.UPDATE;
	}

	private void OnFinishedAnimationCallback()
	{
		this.m_isEnd = true;
		this.m_object.SetActive(false);
	}

	private void SetupWindowData()
	{
		this.m_object = HudMenuUtility.GetLoadMenuChildObject("window_invite", true);
		if (this.m_object != null)
		{
			GameObject gameObject = GameObject.Find(this.m_anchorPath);
			if (gameObject != null)
			{
				Vector3 localPosition = this.m_object.transform.localPosition;
				Vector3 localScale = this.m_object.transform.localScale;
				this.m_object.transform.parent = gameObject.transform;
				this.m_object.transform.localPosition = localPosition;
				this.m_object.transform.localScale = localScale;
			}
			this.m_uiAnimation = base.gameObject.AddComponent<UIPlayAnimation>();
			if (this.m_uiAnimation != null)
			{
				Animation component = this.m_object.GetComponent<Animation>();
				this.m_uiAnimation.target = component;
				this.m_uiAnimation.clipName = "ui_menu_option_window_Anim";
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_object, "Btn_close");
			if (gameObject2 != null)
			{
				UIButtonMessage uIButtonMessage = gameObject2.AddComponent<UIButtonMessage>();
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OnClickCancelButton";
			}
			this.m_itemStorage = GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(this.m_object, "slot");
			if (this.m_itemStorage != null && this.m_msg != null)
			{
				this.m_itemStorage.maxRows = this.m_msg.m_friends.Count;
			}
			this.m_buttons = new List<SettingPartsInviteButton>();
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(this.m_object, "Lbl_invite");
			if (gameObject3 != null)
			{
				UILabel component2 = gameObject3.GetComponent<UILabel>();
				if (component2 != null)
				{
					TextUtility.SetCommonText(component2, "Option", "acceptance_of_invite");
				}
			}
			GameObject gameObject4 = GameObjectUtil.FindChildGameObject(this.m_object, "Lbl_invite_sub");
			if (gameObject4 != null)
			{
				UILabel component3 = gameObject4.GetComponent<UILabel>();
				if (component3 != null)
				{
					TextUtility.SetCommonText(component3, "Option", "acceptance_of_invite_info");
				}
			}
			UIPanel component4 = this.m_object.GetComponent<UIPanel>();
			if (component4 != null)
			{
				component4.alpha = 0f;
			}
			this.m_object.SetActive(true);
		}
	}

	private void CreateEndAcceptWindow()
	{
		GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
		info.buttonType = GeneralWindow.ButtonType.Ok;
		TextManager.TextType type = TextManager.TextType.TEXTTYPE_FIXATION_TEXT;
		info.caption = TextUtility.GetText(type, "FaceBook", "ui_Lbl_ask_accept_invite_caption");
		info.message = TextUtility.GetText(type, "FaceBook", "ui_Lbl_accept_invite_end_text");
		info.name = "CreateEndAcceptWindow";
		GeneralWindow.Create(info);
	}
}
