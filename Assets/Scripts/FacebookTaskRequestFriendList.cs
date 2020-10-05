using Facebook;
using LitJson;
using Message;
using SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FacebookTaskRequestFriendList : SocialTaskBase
{
	private readonly string TaskName = "FacebookTaskRequestFriendList";

	private GameObject m_callbackObject;

	private bool m_isEndProcess;

	public FacebookTaskRequestFriendList()
	{
		this.m_callbackObject = null;
		this.m_isEndProcess = false;
	}

	public void Request(GameObject callbackObject)
	{
		this.m_callbackObject = callbackObject;
	}

	protected override void OnStartProcess()
	{
		bool flag = true;
		if (!FacebookUtil.IsLoggedIn())
		{
			flag = false;
		}
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null && !socialInterface.IsGrantedPermission[1])
		{
			flag = false;
		}
		if (!flag)
		{
			this.m_isEndProcess = true;
			MsgSocialFriendListResponse msgSocialFriendListResponse = new MsgSocialFriendListResponse();
			msgSocialFriendListResponse.m_result = null;
			msgSocialFriendListResponse.m_friends = null;
			this.m_callbackObject.SendMessage("RequestFriendListEndCallback", msgSocialFriendListResponse, SendMessageOptions.DontRequireReceiver);
			return;
		}
		string str = FacebookUtil.MaxFBFriends.ToString();
		string text = FacebookUtil.FBVersionString + "me?fields=friends.limit(" + str + "){installed,id,name,picture}";
		text = Uri.EscapeUriString(text);
		FB.API(text, HttpMethod.GET, new FacebookDelegate(this.RequestFriendListEndCallback), null);
	}

	protected override void OnUpdate()
	{
	}

	protected override bool OnIsEndProcess()
	{
		return this.m_isEndProcess;
	}

	protected override string OnGetTaskName()
	{
		return this.TaskName;
	}

	private void RequestFriendListEndCallback(FBResult fbResult)
	{
		this.m_isEndProcess = true;
		if (this.m_callbackObject == null)
		{
			return;
		}
		if (fbResult == null)
		{
			return;
		}
		string text = fbResult.Text;
		global::Debug.Log("Facebook.RequestFriendListResult:" + text);
		JsonData jsonData = null;
		JsonData jsonData2 = JsonMapper.ToObject(text);
		if (jsonData2 != null)
		{
			try
			{
				JsonData jsonObject = NetUtil.GetJsonObject(jsonData2, "friends");
				if (jsonObject != null)
				{
					jsonData = NetUtil.GetJsonArray(jsonObject, "data");
				}
			}
			catch (Exception var_4_69)
			{
				MsgSocialFriendListResponse msgSocialFriendListResponse = new MsgSocialFriendListResponse();
				msgSocialFriendListResponse.m_result = new SocialResult
				{
					ResultId = 0,
					Result = fbResult.Text,
					IsError = true
				};
				msgSocialFriendListResponse.m_friends = null;
				this.m_callbackObject.SendMessage("RequestFriendListEndCallback", msgSocialFriendListResponse, SendMessageOptions.DontRequireReceiver);
				this.m_callbackObject = null;
			}
		}
		if (jsonData == null)
		{
			MsgSocialFriendListResponse msgSocialFriendListResponse2 = new MsgSocialFriendListResponse();
			msgSocialFriendListResponse2.m_result = new SocialResult();
			msgSocialFriendListResponse2.m_friends = null;
			this.m_callbackObject.SendMessage("RequestFriendListEndCallback", msgSocialFriendListResponse2, SendMessageOptions.DontRequireReceiver);
			this.m_callbackObject = null;
			return;
		}
		List<SocialUserData> list = new List<SocialUserData>();
		List<SocialUserData> list2 = new List<SocialUserData>();
		int count = jsonData.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jsonData3 = jsonData[i];
			if (jsonData3 != null)
			{
				bool flag = false;
				SocialUserData userData = FacebookUtil.GetUserData(jsonData3, ref flag);
				if (userData != null)
				{
					if (flag)
					{
						list.Add(userData);
					}
					else
					{
						list2.Add(userData);
					}
				}
			}
		}
		global::Debug.Log("FacebookTaskRequestFriendList.InstalledFriendList.Count = " + list.Count.ToString());
		SocialResult socialResult = new SocialResult();
		socialResult.ResultId = 0;
		socialResult.Result = fbResult.Text;
		socialResult.IsError = false;
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			socialInterface.AllFriendList = list;
			SystemData systemdata = SystemSaveManager.Instance.GetSystemdata();
			if (!systemdata.IsFlagStatus(SystemData.FlagStatus.FACEBOOK_FRIEND_INIT))
			{
				systemdata.SetFlagStatus(SystemData.FlagStatus.FACEBOOK_FRIEND_INIT, true);
				List<SocialUserData> list3 = new List<SocialUserData>();
				for (int j = 0; j < FacebookUtil.MaxFBRankingFriends; j++)
				{
					if (socialInterface.AllFriendList.Count <= j)
					{
						break;
					}
					SocialUserData socialUserData = list[j];
					if (socialUserData != null)
					{
						list3.Add(socialUserData);
					}
				}
				FacebookUtil.SaveFriendIdList(list3);
			}
			List<SocialUserData> list4 = new List<SocialUserData>();
			if (systemdata != null && systemdata.fbFriends != null)
			{
				List<string> fbFriends = systemdata.fbFriends;
				foreach (string current in fbFriends)
				{
					if (current != null)
					{
						foreach (SocialUserData current2 in list)
						{
							if (current2 != null)
							{
								if (current2.Id == current)
								{
									list4.Add(current2);
								}
							}
						}
					}
				}
			}
			socialInterface.FriendList = list4;
			socialInterface.NotInstalledFriendList = list2;
		}
		MsgSocialFriendListResponse msgSocialFriendListResponse3 = new MsgSocialFriendListResponse();
		msgSocialFriendListResponse3.m_result = socialResult;
		msgSocialFriendListResponse3.m_friends = list;
		this.m_callbackObject.SendMessage("RequestFriendListEndCallback", msgSocialFriendListResponse3, SendMessageOptions.DontRequireReceiver);
		this.m_callbackObject = null;
		global::Debug.Log("Facebook Request FriendList is finished");
	}
}
