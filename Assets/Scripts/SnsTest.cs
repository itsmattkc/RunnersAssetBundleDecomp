using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SnsTest : MonoBehaviour
{
	private SocialInterface m_socialInterface;

	private MsgSocialMyProfileResponse m_profileMsg;

	private MsgSocialFriendListResponse m_friendListMsg;

	private int m_score;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("SocialInterface");
		if (gameObject != null)
		{
			this.m_socialInterface = gameObject.GetComponent<SocialInterface>();
			if (this.m_socialInterface != null)
			{
				this.m_socialInterface.Initialize(base.gameObject);
			}
		}
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(100f, 100f, 300f, 150f), "Login") && this.m_socialInterface != null)
		{
			this.m_socialInterface.Login(base.gameObject);
		}
		if (GUI.Button(new Rect(100f, 300f, 300f, 150f), "Logout") && this.m_socialInterface != null)
		{
			this.m_socialInterface.Logout();
		}
		if (GUI.Button(new Rect(450f, 100f, 300f, 150f), "Feed") && this.m_socialInterface != null)
		{
			this.m_socialInterface.Feed("マイレージ達成!", "マイレージマップ10を終えて、11に進んでます。", base.gameObject);
		}
		if (GUI.Button(new Rect(450f, 300f, 300f, 150f), "GetFriendList") && this.m_socialInterface != null)
		{
			this.m_socialInterface.RequestFriendList(base.gameObject);
		}
		if (this.m_profileMsg != null)
		{
			GUI.Label(new Rect(750f, 200f, 300f, 150f), this.m_profileMsg.m_profile.Name);
			GUI.Label(new Rect(750f, 300f, 300f, 150f), this.m_profileMsg.m_profile.Url);
		}
		if (this.m_friendListMsg != null)
		{
			List<SocialUserData> friends = this.m_friendListMsg.m_friends;
			int num = 0;
			foreach (SocialUserData current in friends)
			{
				if (current != null)
				{
					GUI.Label(new Rect(750f, (float)(430 + 100 * num), 300f, 150f), current.Name);
					GUI.Label(new Rect(750f, (float)(480 + 100 * num), 300f, 150f), current.Id);
					num++;
				}
			}
			if (!GUI.Button(new Rect(100f, 500f, 300f, 150f), "InviteFriend") || this.m_socialInterface != null)
			{
			}
			if (GUI.Button(new Rect(450f, 500f, 300f, 150f), "GetFriendScore") && this.m_socialInterface != null)
			{
				this.m_socialInterface.RequestGameData(friends[0].Id, base.gameObject);
			}
		}
		if (this.m_score != 0)
		{
			GUI.Label(new Rect(850f, 400f, 300f, 150f), this.m_score.ToString());
		}
	}

	private void RequestMyProfileEndCallback(MsgSocialMyProfileResponse msg)
	{
		global::Debug.Log("RequestMyProfileEndCallback");
		if (msg == null)
		{
			return;
		}
		this.m_profileMsg = msg;
	}

	private void RequestFriendListEndCallback(MsgSocialFriendListResponse msg)
	{
		global::Debug.Log("RequestFriendListEndCallback");
		if (msg == null)
		{
			return;
		}
		this.m_friendListMsg = msg;
	}

	private void RequestGameDataEndCallback(MsgSocialCustomUserDataResponse msg)
	{
	}
}
