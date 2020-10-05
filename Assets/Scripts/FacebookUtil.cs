using LitJson;
using SaveData;
using System;
using System.Collections.Generic;

public class FacebookUtil
{
	public static readonly string EnergyMarkString = "SonicRunnersEnergy";

	public static readonly string SonicRunnersId = "203227836537595";

	public static readonly string FBVersionString = "/v2.1/";

	public static readonly int MaxFBFriends = 5000;

	public static readonly int MaxFBRankingFriends = 50;

	public static readonly string[] PermissionString = new string[]
	{
		"public_profile",
		"user_friends"
	};

	public static void SaveFriendIdList(List<SocialUserData> friends)
	{
		if (friends == null)
		{
			return;
		}
		if (friends.Count > FacebookUtil.MaxFBRankingFriends)
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (SocialUserData current in friends)
		{
			if (current != null)
			{
				list.Add(current.Id);
			}
		}
		SystemData systemSaveData = SystemSaveManager.GetSystemSaveData();
		if (systemSaveData != null)
		{
			systemSaveData.fbFriends = list;
		}
		SystemSaveManager.Instance.SaveSystemData();
	}

	public static SocialUserData GetUserData(JsonData jdata)
	{
		bool flag = false;
		return FacebookUtil.GetUserData(jdata, ref flag);
	}

	public static SocialUserData GetUserData(JsonData jdata, ref bool isInstalled)
	{
		SocialUserData socialUserData = new SocialUserData();
		string jsonString = NetUtil.GetJsonString(jdata, "id");
		if (jsonString == null)
		{
			return socialUserData;
		}
		socialUserData.Id = jsonString;
		socialUserData.Name = NetUtil.GetJsonString(jdata, "name");
		isInstalled = NetUtil.GetJsonBoolean(jdata, "installed");
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, "picture");
		if (jsonObject == null)
		{
			return socialUserData;
		}
		JsonData jsonObject2 = NetUtil.GetJsonObject(jsonObject, "data");
		if (jsonObject2 == null)
		{
			return socialUserData;
		}
		string jsonString2 = NetUtil.GetJsonString(jsonObject2, "url");
		if (jsonString2 == null)
		{
			return socialUserData;
		}
		socialUserData.IsSilhouette = NetUtil.GetJsonBoolean(jsonObject2, "is_silhouette");
		socialUserData.Url = jsonString2;
		return socialUserData;
	}

	public static void GetDefaultPicture(SocialUserData userData)
	{
	}

	public static string GetActionId(string text)
	{
		if (text == null)
		{
			return null;
		}
		UnityEngine.Debug.Log("Facebook.GetAction:" + text);
		JsonData jsonData = JsonMapper.ToObject(text);
		if (jsonData == null)
		{
			UnityEngine.Debug.Log("Failed transform plainText to Json");
			return null;
		}
		JsonData jsonArray = NetUtil.GetJsonArray(jsonData, "data");
		if (jsonArray == null)
		{
			UnityEngine.Debug.Log("Not found user data in json data");
			return null;
		}
		if (jsonArray.Count == 0)
		{
			return null;
		}
		return NetUtil.GetJsonString(jsonArray[0], "id");
	}

	public static string GetObjectIdFromAction(string text, string objectName)
	{
		if (text == null)
		{
			return null;
		}
		UnityEngine.Debug.Log("Facebook.GetAction:" + text);
		JsonData jsonData = JsonMapper.ToObject(text);
		if (jsonData == null)
		{
			UnityEngine.Debug.Log("Failed transform plainText to Json");
			return null;
		}
		JsonData jsonArray = NetUtil.GetJsonArray(jsonData, "data");
		if (jsonArray == null)
		{
			UnityEngine.Debug.Log("Not found user data in json data");
			return null;
		}
		if (jsonArray.Count == 0)
		{
			return null;
		}
		JsonData jsonObject = NetUtil.GetJsonObject(jsonArray[0], "data");
		if (jsonObject == null)
		{
			UnityEngine.Debug.Log("Not found object in json data");
			return null;
		}
		JsonData jsonObject2 = NetUtil.GetJsonObject(jsonObject, objectName);
		if (jsonObject2 == null)
		{
			UnityEngine.Debug.Log("Not found sigoto object in json data");
			return null;
		}
		string jsonString = NetUtil.GetJsonString(jsonObject2, "id");
		if (jsonString == null)
		{
			UnityEngine.Debug.Log("Not found object's id in json data");
			return null;
		}
		return jsonString;
	}

	public static void UpdatePermissionInfo(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		JsonData jsonData = JsonMapper.ToObject(text);
		if (jsonData == null)
		{
			UnityEngine.Debug.Log("Failed transform plainText to Json");
			return;
		}
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null)
		{
			JsonData jsonArray = NetUtil.GetJsonArray(jsonData, "data");
			if (jsonArray != null)
			{
				for (int i = 0; i < jsonArray.Count; i++)
				{
					string jsonString = NetUtil.GetJsonString(jsonArray[i], "permission");
					if (!string.IsNullOrEmpty(jsonString))
					{
						string jsonString2 = NetUtil.GetJsonString(jsonArray[i], "status");
						if (!string.IsNullOrEmpty(jsonString2))
						{
							for (int j = 0; j < 2; j++)
							{
								string text2 = FacebookUtil.PermissionString[j];
								if (!string.IsNullOrEmpty(text2))
								{
									if (jsonString == text2)
									{
										if (jsonString2 == "granted")
										{
											socialInterface.IsGrantedPermission[j] = true;
											UnityEngine.Debug.Log("FB permission:" + text2 + " is granted");
										}
										else
										{
											socialInterface.IsGrantedPermission[j] = false;
											UnityEngine.Debug.Log("FB permission:" + text2 + " is not granted");
										}
										break;
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public static bool IsLoggedIn()
	{
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		return !(socialInterface == null) && socialInterface.IsLoggedIn;
	}

	public static List<SocialUserData> GetInvitedFriendList(string text)
	{
		List<SocialUserData> list = new List<SocialUserData>();
		if (text == null)
		{
			return list;
		}
		JsonData jsonData = JsonMapper.ToObject(text);
		if (jsonData == null)
		{
			UnityEngine.Debug.Log("Failed transform plainText to Json");
			return list;
		}
		JsonData jsonData2 = null;
		try
		{
			jsonData2 = NetUtil.GetJsonArray(jsonData, "data");
			if (jsonData2 == null)
			{
				UnityEngine.Debug.Log("Not found user data in json data");
				List<SocialUserData> result = list;
				return result;
			}
		}
		catch (Exception var_3_52)
		{
			List<SocialUserData> result = list;
			return result;
		}
		int count = jsonData2.Count;
		List<string> list2 = new List<string>();
		for (int i = 0; i < count; i++)
		{
			JsonData jsonObject = NetUtil.GetJsonObject(jsonData2[i], "application");
			if (jsonObject != null)
			{
				string jsonString = NetUtil.GetJsonString(jsonObject, "id");
				if (!(jsonString != FacebookUtil.SonicRunnersId))
				{
					string jsonString2 = NetUtil.GetJsonString(jsonData2[i], "data");
					list2.Add(jsonString2);
				}
			}
		}
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface == null)
		{
			return list;
		}
		foreach (string current in list2)
		{
			if (!string.IsNullOrEmpty(current))
			{
				foreach (SocialUserData current2 in socialInterface.FriendList)
				{
					if (current2 != null)
					{
						if (current2.CustomData.GameId == current)
						{
							list.Add(current2);
							break;
						}
					}
				}
			}
		}
		return list;
	}
}
