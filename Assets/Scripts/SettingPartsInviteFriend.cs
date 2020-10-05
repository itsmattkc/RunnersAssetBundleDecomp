using System;

public class SettingPartsInviteFriend : SettingBase
{
	private enum State
	{
		STATE_IDLE,
		STATE_WAIT,
		STATE_LOGIN_SETUP,
		STATE_LOGIN,
		STATE_INVITE_FRIEND,
		STATE_END
	}

	private SettingPartsInviteFriendUI m_inviteFriend;

	private SettingPartsSnsLogin m_login;

	private string m_anchorPath;

	private readonly string ExcludePathName = "Camera/Anchor_5_MC";

	private SettingPartsInviteFriend.State m_state;

	private void Start()
	{
	}

	protected override void OnSetup(string anthorPath)
	{
		this.m_anchorPath = this.ExcludePathName;
		if (this.m_inviteFriend == null)
		{
			this.m_inviteFriend = base.gameObject.AddComponent<SettingPartsInviteFriendUI>();
		}
		if (this.m_login == null)
		{
			this.m_login = base.gameObject.AddComponent<SettingPartsSnsLogin>();
		}
		if (this.m_login != null)
		{
			this.m_login.Setup(this.m_anchorPath);
		}
	}

	protected override void OnPlayStart()
	{
		this.m_state = SettingPartsInviteFriend.State.STATE_WAIT;
	}

	protected override bool OnIsEndPlay()
	{
		return !(this.m_inviteFriend != null) || this.m_inviteFriend.IsEndPlay();
	}

	protected override void OnUpdate()
	{
		switch (this.m_state)
		{
		case SettingPartsInviteFriend.State.STATE_WAIT:
			this.m_state = SettingPartsInviteFriend.State.STATE_LOGIN_SETUP;
			break;
		case SettingPartsInviteFriend.State.STATE_LOGIN_SETUP:
			if (this.m_login != null)
			{
				this.m_login.PlayStart();
			}
			this.m_state = SettingPartsInviteFriend.State.STATE_LOGIN;
			break;
		case SettingPartsInviteFriend.State.STATE_LOGIN:
			if (this.m_login != null && this.m_login.IsEnd)
			{
				this.m_inviteFriend.Setup(this.m_anchorPath);
				this.m_inviteFriend.PlayStart();
				this.m_state = SettingPartsInviteFriend.State.STATE_INVITE_FRIEND;
			}
			break;
		case SettingPartsInviteFriend.State.STATE_INVITE_FRIEND:
			if (this.m_inviteFriend.IsEndPlay())
			{
				this.m_state = SettingPartsInviteFriend.State.STATE_END;
			}
			break;
		}
	}
}
