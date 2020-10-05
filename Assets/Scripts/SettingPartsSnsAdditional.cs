using Message;
using SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SettingPartsSnsAdditional : MonoBehaviour
{
	public enum Mode
	{
		NONE,
		BACK_GROUND_LOAD,
		WAIT_TO_LOAD_END
	}

	private struct CallbackInfo
	{
		public string gameObjectName;

		public string functionName;
	}

	private bool m_isStart;

	private bool m_isEnd;

	private SettingPartsSnsAdditional.Mode m_mode;

	private List<SettingPartsSnsAdditional.CallbackInfo> m_callbackList = new List<SettingPartsSnsAdditional.CallbackInfo>();

	private bool m_isEndRequestMyProfile;

	private bool m_isEndRequestFriendProfile;

	public bool IsEnd
	{
		get
		{
			return this.m_isEnd;
		}
	}

	private void Start()
	{
	}

	public void PlayStart()
	{
		this.PlayStart(null, null, SettingPartsSnsAdditional.Mode.WAIT_TO_LOAD_END);
	}

	public void PlayStart(string gameObjectName, string functionName, SettingPartsSnsAdditional.Mode mode)
	{
		if (this.m_isEnd)
		{
			GameObject gameObject = GameObject.Find(gameObjectName);
			if (gameObject != null)
			{
				gameObject.SendMessage(functionName);
			}
			return;
		}
		if (this.m_isStart && !this.m_isEnd)
		{
			if (this.m_mode < mode)
			{
				this.m_mode = mode;
				if (this.m_mode == SettingPartsSnsAdditional.Mode.WAIT_TO_LOAD_END)
				{
					NetMonitor instance = NetMonitor.Instance;
					if (instance != null)
					{
						instance.StartMonitor(null);
					}
				}
			}
			SettingPartsSnsAdditional.CallbackInfo item;
			item.gameObjectName = gameObjectName;
			item.functionName = functionName;
			this.m_callbackList.Add(item);
			return;
		}
		this.m_isStart = true;
		this.m_mode = mode;
		if (this.m_mode == SettingPartsSnsAdditional.Mode.WAIT_TO_LOAD_END)
		{
			NetMonitor instance2 = NetMonitor.Instance;
			if (instance2 != null)
			{
				instance2.StartMonitor(null);
			}
		}
		SettingPartsSnsAdditional.CallbackInfo item2;
		item2.gameObjectName = gameObjectName;
		item2.functionName = functionName;
		this.m_callbackList.Add(item2);
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null && socialInterface.IsLoggedIn)
		{
			global::Debug.Log("SettingPartsSnsAdditional:PlayStart");
			socialInterface.RequestPermission(base.gameObject);
		}
		else
		{
			global::Debug.Log("SettingPartsSnsAdditional:NotLoggedIn");
			this.m_isEnd = false;
		}
	}

	private void RequestPermissionEndCallback(MsgSocialNormalResponse msg)
	{
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null && socialInterface.IsLoggedIn)
		{
			global::Debug.Log("SettingPartsSnsAdditional:PlayStart");
			socialInterface.RequestMyProfile(base.gameObject);
		}
	}

	private void RequestMyProfileEndCallback(MsgSocialMyProfileResponse msg)
	{
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null && socialInterface.IsLoggedIn)
		{
			socialInterface.RequestFriendList(base.gameObject);
		}
		global::Debug.Log("SettingPartsSnsAdditional:RequestMyProfileEndCallback");
	}

	private void RequestFriendListEndCallback(MsgSocialFriendListResponse msg)
	{
		global::Debug.Log("SettingPartsSnsAdditional:RequestFriendListEndCallback");
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		PlayerImageManager playerImageManager = GameObjectUtil.FindGameObjectComponent<PlayerImageManager>("PlayerImageManager");
		if (socialInterface != null && playerImageManager != null)
		{
			List<string> list = new List<string>();
			List<SocialUserData> friendWithMeList = socialInterface.FriendWithMeList;
			if (friendWithMeList != null)
			{
				foreach (SocialUserData current in friendWithMeList)
				{
					if (current != null)
					{
						if (!current.IsSilhouette)
						{
							playerImageManager.GetPlayerImage(current.Id, current.Url, null);
							global::Debug.Log("sns picture add: " + current.Id + ", " + current.Url);
						}
						list.Add(current.Id);
					}
				}
				global::Debug.Log("SettingPartsSnsAdditional:GetPlayerImage");
			}
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null && socialInterface != null)
			{
				loggedInServerInterface.RequestServerGetFriendUserIdList(list, base.gameObject);
			}
		}
	}

	private void RequestGameDataEndCallback(MsgSocialCustomUserDataResponse msg)
	{
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		SocialUserData myProfile = socialInterface.MyProfile;
		if (myProfile.Id == msg.m_userData.Id)
		{
			global::Debug.Log("SettingPartsSnsLogin:myProfile throwed");
			string text = string.Empty;
			if (SystemSaveManager.GetGameID() != "0")
			{
				text = SystemSaveManager.GetGameID();
			}
			bool flag = false;
			if (!msg.m_isCreated)
			{
				flag = true;
			}
			else if (text != myProfile.CustomData.GameId)
			{
				socialInterface.DeleteGameData(base.gameObject);
				flag = true;
			}
			if (flag)
			{
				global::Debug.Log("SettingPartsSnsLogin:Created Game Data");
				socialInterface.CreateMyGameData(text, base.gameObject);
			}
			else
			{
				this.CreateGameDataEndCallback(null);
			}
		}
	}

	private void CreateGameDataEndCallback(MsgSocialNormalResponse msg)
	{
		global::Debug.Log("SettingPartsSnsLogin:CreatedGameDataWasFinished");
		if (this.m_mode == SettingPartsSnsAdditional.Mode.WAIT_TO_LOAD_END)
		{
			NetMonitor instance = NetMonitor.Instance;
			if (instance != null)
			{
				instance.EndMonitorForward(null, null, null);
				instance.EndMonitorBackward();
			}
		}
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			socialInterface.IsEnableFriendInfo = true;
		}
		this.m_isEnd = true;
		foreach (SettingPartsSnsAdditional.CallbackInfo current in this.m_callbackList)
		{
			string gameObjectName = current.gameObjectName;
			if (!string.IsNullOrEmpty(gameObjectName))
			{
				string functionName = current.functionName;
				if (!string.IsNullOrEmpty(functionName))
				{
					GameObject gameObject = GameObject.Find(gameObjectName);
					if (gameObject != null)
					{
						gameObject.SendMessage(functionName);
					}
				}
			}
		}
		this.m_callbackList.Clear();
		GameObject gameObject2 = GameObject.Find("ui_mm_ranking_page(Clone)");
		if (gameObject2 != null)
		{
			gameObject2.SendMessage("OnSettingPartsSnsAdditional");
		}
	}

	private void ServerGetFriendUserIdList_Succeeded(MsgGetFriendUserIdListSucceed msg)
	{
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			string id = socialInterface.MyProfile.Id;
			global::Debug.Log("mySnsUserId = " + id);
			bool flag = false;
			List<ServerUserTransformData> transformDataList = msg.m_transformDataList;
			if (transformDataList == null)
			{
				global::Debug.Log("ServerGetFriendUserIdList_Succeeded: DataList is null");
				this.ProcessEnd();
				return;
			}
			foreach (ServerUserTransformData current in transformDataList)
			{
				if (current != null)
				{
					if (current.m_facebookId == id && current.m_userId == SystemSaveManager.GetGameID())
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				global::Debug.Log("ServerGetFriendUserIdList_Succeeded: MyId is not Registered");
				transformDataList.Add(new ServerUserTransformData
				{
					m_facebookId = id,
					m_userId = SystemSaveManager.GetGameID()
				});
				id = socialInterface.MyProfile.Id;
			}
			foreach (ServerUserTransformData current2 in transformDataList)
			{
				if (current2 != null)
				{
					foreach (SocialUserData current3 in socialInterface.FriendWithMeList)
					{
						if (current3 != null)
						{
							if (current2.m_facebookId == current3.Id && string.IsNullOrEmpty(current3.CustomData.GameId))
							{
								current3.CustomData.GameId = current2.m_userId;
							}
						}
					}
				}
			}
			if (!flag)
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null && socialInterface != null)
				{
					loggedInServerInterface.RequestServerSetFacebookScopedId(id, base.gameObject);
				}
			}
			else
			{
				global::Debug.Log("ServerGetFriendUserIdList_Succeeded: MyId is already Registered");
				this.ProcessEnd();
			}
		}
	}

	private void ServerSetFacebookScopedId_Succeeded(MsgSetFacebookScopedIdSucceed msg)
	{
		this.ProcessEnd();
	}

	private void ProcessEnd()
	{
		global::Debug.Log("SettingPartsSnsLogin:CreatedGameDataWasFinished");
		if (this.m_mode == SettingPartsSnsAdditional.Mode.WAIT_TO_LOAD_END)
		{
			NetMonitor instance = NetMonitor.Instance;
			if (instance != null)
			{
				instance.EndMonitorForward(null, null, null);
				instance.EndMonitorBackward();
			}
		}
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			socialInterface.IsEnableFriendInfo = true;
		}
		this.m_isEnd = true;
		foreach (SettingPartsSnsAdditional.CallbackInfo current in this.m_callbackList)
		{
			string gameObjectName = current.gameObjectName;
			if (!string.IsNullOrEmpty(gameObjectName))
			{
				string functionName = current.functionName;
				if (!string.IsNullOrEmpty(functionName))
				{
					GameObject gameObject = GameObject.Find(gameObjectName);
					if (gameObject != null)
					{
						gameObject.SendMessage(functionName);
					}
				}
			}
		}
		this.m_callbackList.Clear();
		GameObject gameObject2 = GameObject.Find("ui_mm_ranking_page(Clone)");
		if (gameObject2 != null)
		{
			gameObject2.SendMessage("OnSettingPartsSnsAdditional");
		}
	}
}
