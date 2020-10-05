using Message;
using SaveData;
using System;
using UnityEngine;

public class AchievementIncentive : MonoBehaviour
{
	public enum State
	{
		Idle,
		Request,
		Succeeded,
		Failed
	}

	private AchievementIncentive.State m_state;

	private void Start()
	{
	}

	public AchievementIncentive.State GetState()
	{
		return this.m_state;
	}

	public void RequestServer()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			int achievementIncentiveCount = AchievementIncentive.GetAchievementIncentiveCount();
			if (achievementIncentiveCount > 0)
			{
				loggedInServerInterface.RequestServerGetFacebookIncentive(3, achievementIncentiveCount, base.gameObject);
				this.m_state = AchievementIncentive.State.Request;
			}
		}
	}

	private void ServerGetFacebookIncentive_Succeeded(MsgGetNormalIncentiveSucceed msg)
	{
		AchievementIncentive.ResetAchievementIncentiveCount();
		if (SaveDataManager.Instance != null && SaveDataManager.Instance.ConnectData != null)
		{
			SaveDataManager.Instance.ConnectData.ReplaceMessageBox = true;
		}
		this.m_state = AchievementIncentive.State.Succeeded;
	}

	private void ServerGetFacebookIncentive_Failed(MsgServerConnctFailed msg)
	{
		this.m_state = AchievementIncentive.State.Failed;
	}

	private static SystemData GetSystemSaveData()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			return instance.GetSystemdata();
		}
		return null;
	}

	private static int GetAchievementIncentiveCount()
	{
		SystemData systemSaveData = AchievementIncentive.GetSystemSaveData();
		if (systemSaveData != null)
		{
			return systemSaveData.achievementIncentiveCount;
		}
		return 0;
	}

	public static void AddAchievementIncentiveCount(int add)
	{
		if (add > 0)
		{
			SystemData systemSaveData = AchievementIncentive.GetSystemSaveData();
			if (systemSaveData != null)
			{
				systemSaveData.achievementIncentiveCount += add;
				AchievementIncentive.Save();
			}
		}
	}

	private static void ResetAchievementIncentiveCount()
	{
		SystemData systemSaveData = AchievementIncentive.GetSystemSaveData();
		if (systemSaveData != null)
		{
			systemSaveData.achievementIncentiveCount = 0;
			AchievementIncentive.Save();
		}
	}

	private static void Save()
	{
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			instance.SaveSystemData();
		}
	}
}
