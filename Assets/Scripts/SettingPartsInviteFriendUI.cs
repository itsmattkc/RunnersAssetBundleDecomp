using Message;
using System;
using UnityEngine;

public class SettingPartsInviteFriendUI : SettingBase
{
	private bool m_isEnd;

	protected override void OnSetup(string anthorPath)
	{
	}

	protected override void OnPlayStart()
	{
		GameObject gameObject = GameObject.Find("SocialInterface");
		if (gameObject == null)
		{
			return;
		}
		SocialInterface component = gameObject.GetComponent<SocialInterface>();
		if (component == null)
		{
			return;
		}
		component.InviteFriend(base.gameObject);
	}

	protected override bool OnIsEndPlay()
	{
		return this.m_isEnd;
	}

	protected override void OnUpdate()
	{
	}

	private void InviteFriendEndCallback(MsgSocialNormalResponse msg)
	{
		this.m_isEnd = true;
	}
}
