using System;
using UnityEngine;

public class OptionFriendInvitation : MonoBehaviour
{
	private const string ATTACH_ANTHOR_NAME = "Camera/menu_Anim/OptionUI/Anchor_5_MC";

	private SettingPartsInviteFriend m_inviteFriend;

	private EasySnsFeed m_easySnsFeed;

	private bool m_loginFlag;

	private ui_option_scroll m_ui_option_scroll;

	public void Setup(ui_option_scroll scroll)
	{
		if (scroll != null)
		{
			this.m_ui_option_scroll = scroll;
		}
		this.m_loginFlag = false;
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			this.m_loginFlag = socialInterface.IsLoggedIn;
		}
		if (this.m_loginFlag)
		{
			this.PlayInvite();
		}
		else
		{
			this.m_easySnsFeed = new EasySnsFeed(base.gameObject, "Camera/Anchor_5_MC");
		}
		base.enabled = true;
	}

	private void SetInvite()
	{
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			this.m_loginFlag = socialInterface.IsLoggedIn;
		}
		if (this.m_loginFlag)
		{
			if (this.m_inviteFriend != null)
			{
				this.m_inviteFriend.PlayStart();
			}
			else
			{
				this.m_inviteFriend = base.gameObject.AddComponent<SettingPartsInviteFriend>();
				if (this.m_inviteFriend != null)
				{
					this.m_inviteFriend.Setup("Camera/menu_Anim/OptionUI/Anchor_5_MC");
					this.m_inviteFriend.PlayStart();
				}
			}
		}
	}

	private void PlayInvite()
	{
		if (this.m_inviteFriend != null)
		{
			this.m_inviteFriend.PlayStart();
		}
		else
		{
			this.m_inviteFriend = base.gameObject.AddComponent<SettingPartsInviteFriend>();
			if (this.m_inviteFriend != null)
			{
				this.m_inviteFriend.Setup("Camera/menu_Anim/OptionUI/Anchor_5_MC");
				this.m_inviteFriend.PlayStart();
			}
		}
	}

	public void Update()
	{
		if (this.m_loginFlag)
		{
			if (this.m_inviteFriend != null && this.m_inviteFriend.IsEndPlay())
			{
				if (this.m_ui_option_scroll != null)
				{
					this.m_ui_option_scroll.OnEndChildPage();
				}
				base.enabled = false;
			}
		}
		else if (this.m_easySnsFeed != null)
		{
			EasySnsFeed.Result result = this.m_easySnsFeed.Update();
			if (result != EasySnsFeed.Result.COMPLETED)
			{
				if (result == EasySnsFeed.Result.FAILED)
				{
					this.m_easySnsFeed = null;
					if (this.m_ui_option_scroll != null)
					{
						this.m_ui_option_scroll.OnEndChildPage();
					}
					base.enabled = false;
				}
			}
			else
			{
				this.SetInvite();
				this.m_easySnsFeed = null;
				if (!this.m_loginFlag)
				{
					if (this.m_ui_option_scroll != null)
					{
						this.m_ui_option_scroll.OnEndChildPage();
					}
					base.enabled = false;
				}
			}
		}
	}
}
