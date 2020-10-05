using System;
using System.Collections.Generic;
using UnityEngine;

public class SocialInterface : MonoBehaviour
{
	public enum Permission
	{
		PUBLIC_PROFILE,
		USER_FRIENDS,
		NUM
	}

	private SocialPlatform m_platform;

	private bool m_isInitialized;

	private bool m_isLoggedIn;

	private SocialUserData m_myProfile = new SocialUserData();

	private bool m_isEnableFriendInfo;

	private List<SocialUserData> m_allFriendList = new List<SocialUserData>();

	private List<SocialUserData> m_friendList;

	private List<SocialUserData> m_notInstalledFriendList = new List<SocialUserData>();

	private List<SocialUserData> m_invitedFriendList;

	private bool[] m_isGrantedPermission = new bool[2];

	private static SocialInterface m_instance;

	public static SocialInterface Instance
	{
		get
		{
			return SocialInterface.m_instance;
		}
	}

	public bool IsInitialized
	{
		get
		{
			return this.m_isInitialized;
		}
		set
		{
			this.m_isInitialized = value;
		}
	}

	public bool IsLoggedIn
	{
		get
		{
			return this.m_isLoggedIn;
		}
		set
		{
			this.m_isLoggedIn = value;
		}
	}

	public SocialUserData MyProfile
	{
		get
		{
			return this.m_myProfile;
		}
		set
		{
			this.m_myProfile = value;
		}
	}

	public bool IsEnableFriendInfo
	{
		get
		{
			return this.m_isEnableFriendInfo;
		}
		set
		{
			this.m_isEnableFriendInfo = value;
		}
	}

	public List<SocialUserData> FriendList
	{
		get
		{
			return this.m_friendList;
		}
		set
		{
			if (value == null)
			{
				return;
			}
			if (value.Count > FacebookUtil.MaxFBRankingFriends)
			{
				return;
			}
			this.m_friendList = value;
		}
	}

	public List<SocialUserData> AllFriendList
	{
		get
		{
			return this.m_allFriendList;
		}
		set
		{
			this.m_allFriendList = value;
		}
	}

	public List<SocialUserData> NotInstalledFriendList
	{
		get
		{
			return this.m_notInstalledFriendList;
		}
		set
		{
			this.m_notInstalledFriendList = value;
		}
	}

	public List<SocialUserData> InvitedFriendList
	{
		get
		{
			return this.m_invitedFriendList;
		}
		set
		{
			this.m_invitedFriendList = value;
		}
	}

	public bool[] IsGrantedPermission
	{
		get
		{
			return this.m_isGrantedPermission;
		}
		set
		{
			this.m_isGrantedPermission = value;
		}
	}

	public List<SocialUserData> FriendWithMeList
	{
		get
		{
			List<SocialUserData> list = new List<SocialUserData>();
			if (this.m_friendList != null)
			{
				foreach (SocialUserData current in this.m_friendList)
				{
					list.Add(current);
				}
			}
			if (this.m_myProfile != null)
			{
				list.Add(this.m_myProfile);
			}
			return list;
		}
	}

	public static List<string> GetGameIdList(List<SocialUserData> socialUserDataList)
	{
		List<string> list = new List<string>();
		if (socialUserDataList == null)
		{
			return list;
		}
		foreach (SocialUserData current in socialUserDataList)
		{
			string gameId = current.CustomData.GameId;
			if (!string.IsNullOrEmpty(gameId))
			{
				list.Add(gameId);
			}
		}
		return list;
	}

	public static SocialUserData GetSocialUserDataFromGameId(List<SocialUserData> socialUserDataList, string gameId)
	{
		if (socialUserDataList != null)
		{
			foreach (SocialUserData current in socialUserDataList)
			{
				if (current.CustomData.GameId == gameId)
				{
					return current;
				}
			}
		}
		return null;
	}

	private void Awake()
	{
		if (SocialInterface.m_instance == null)
		{
			this.m_platform = base.gameObject.AddComponent<SocialPlatformFacebook>();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			SocialInterface.m_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
	}

	public void Initialize(GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.Initialize(callbackObject);
	}

	public void Login(GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.Login(callbackObject);
	}

	public void Logout()
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.Logout();
	}

	public void RequestMyProfile(GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.RequestMyProfile(callbackObject);
	}

	public void RequestFriendList(GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.RequestFriendList(callbackObject);
	}

	public void SetScore(SocialDefine.ScoreType type, int score, GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.SetScore(type, score, callbackObject);
	}

	public void CreateMyGameData(string gameId, GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.CreateMyGameData(gameId, callbackObject);
	}

	public void RequestGameData(string userId, GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.RequestGameData(userId, callbackObject);
	}

	public void DeleteGameData(GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.DeleteGameData(callbackObject);
	}

	public void InviteFriend(GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.InviteFriend(callbackObject);
	}

	public void SendEnergy(SocialUserData userData, GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.SendEnergy(userData, callbackObject);
	}

	public void Feed(string feedCaption, string feedText, GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.Feed(feedCaption, feedText, callbackObject);
	}

	public void RequestInvitedFriend(GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.RequestInvitedFriend(callbackObject);
	}

	public void RequestPermission(GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.RequestPermission(callbackObject);
	}

	public void AddPermission(List<SocialInterface.Permission> permissions, GameObject callbackObject)
	{
		if (this.m_platform == null)
		{
			return;
		}
		this.m_platform.AddPermission(permissions, callbackObject);
	}

	public void RequestFriendRankingInfoSet(string gameObjectName, string functionName, SettingPartsSnsAdditional.Mode mode)
	{
		global::Debug.Log("RequestFriendRankingInfoSet");
		SettingPartsSnsAdditional settingPartsSnsAdditional = base.gameObject.GetComponent<SettingPartsSnsAdditional>();
		if (settingPartsSnsAdditional == null)
		{
			settingPartsSnsAdditional = base.gameObject.AddComponent<SettingPartsSnsAdditional>();
		}
		settingPartsSnsAdditional.PlayStart(gameObjectName, functionName, mode);
	}
}
