using SaveData;
using System;
using Text;
using UnityEngine;

public class OptionAcceptInvite : MonoBehaviour
{
	private const string ATTACH_ANTHOR_NAME = "UI Root (2D)/Camera/menu_Anim/OptionUI/Anchor_5_MC";

	private SettingPartsAcceptInvite m_acceptInvite;

	private EasySnsFeed m_easySnsFeed;

	private bool m_loginFlag;

	private bool m_acceptedFlag;

	private ui_option_scroll m_ui_option_scroll;

	public void Setup(ui_option_scroll scroll)
	{
		if (scroll != null)
		{
			this.m_ui_option_scroll = scroll;
		}
		if (HudMenuUtility.IsSystemDataFlagStatus(SystemData.FlagStatus.FRIEDN_ACCEPT_INVITE))
		{
			this.m_acceptedFlag = true;
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "AcceptedInvite",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextUtility.GetCommonText("Option", "accepted_invite_caption"),
				message = TextUtility.GetCommonText("Option", "accepted_invite_text")
			});
		}
		else
		{
			this.m_loginFlag = false;
			SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
			if (socialInterface != null)
			{
				this.m_loginFlag = socialInterface.IsLoggedIn;
			}
			if (this.m_loginFlag)
			{
				this.PlayAcceptInvite();
			}
			else
			{
				this.m_easySnsFeed = new EasySnsFeed(base.gameObject, "Camera/Anchor_5_MC");
			}
		}
		base.enabled = true;
	}

	private void SetAcceptInvite()
	{
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			this.m_loginFlag = socialInterface.IsLoggedIn;
		}
		if (this.m_loginFlag)
		{
			this.PlayAcceptInvite();
		}
	}

	private void PlayAcceptInvite()
	{
		this.m_acceptInvite = base.gameObject.GetComponent<SettingPartsAcceptInvite>();
		if (this.m_acceptInvite == null)
		{
			this.m_acceptInvite = base.gameObject.AddComponent<SettingPartsAcceptInvite>();
			this.m_acceptInvite.Setup("UI Root (2D)/Camera/menu_Anim/OptionUI/Anchor_5_MC");
			this.m_acceptInvite.PlayStart();
		}
		else
		{
			this.m_acceptInvite.PlayStart();
		}
	}

	public void Update()
	{
		if (this.m_acceptedFlag)
		{
			if (GeneralWindow.IsCreated("AcceptedInvite") && GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				if (this.m_ui_option_scroll != null)
				{
					this.m_ui_option_scroll.OnEndChildPage();
				}
				base.enabled = false;
			}
		}
		else if (this.m_loginFlag)
		{
			if (this.m_acceptInvite != null && this.m_acceptInvite.IsEndPlay())
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
				this.SetAcceptInvite();
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
