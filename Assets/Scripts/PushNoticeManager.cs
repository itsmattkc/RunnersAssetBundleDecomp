using SaveData;
using System;
using System.Diagnostics;
using Text;
using UnityEngine;

public class PushNoticeManager : MonoBehaviour
{
	private static PushNoticeManager instance;

	public static PushNoticeManager Instance
	{
		get
		{
			if (PushNoticeManager.instance == null)
			{
				PushNoticeManager.instance = (UnityEngine.Object.FindObjectOfType(typeof(PushNoticeManager)) as PushNoticeManager);
			}
			return PushNoticeManager.instance;
		}
	}

	private void Awake()
	{
		this.CheckInstance();
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		LocalNotification.Initialize();
	}

	private bool CheckInstance()
	{
		if (PushNoticeManager.instance == null)
		{
			PushNoticeManager.instance = this;
			return true;
		}
		if (this == PushNoticeManager.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}

	private void OnDestroy()
	{
		if (PushNoticeManager.instance == this)
		{
			PushNoticeManager.instance = null;
		}
	}

	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			LocalNotification.OnActive();
		}
		else
		{
			this.PushNotice(PushNoticeManager.GetSecondsToFullChallenge());
		}
	}

	private void OnApplicationQuit()
	{
		this.PushNotice(PushNoticeManager.GetSecondsToFullChallenge());
	}

	private void PushNotice(int secondsToFullChallenge)
	{
		if (SystemSaveManager.Instance != null && SystemSaveManager.Instance.GetSystemdata().pushNoticeFlags.Test(1) && secondsToFullChallenge > 0)
		{
			string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "PushNotice", "challenge_notification").text;
			LocalNotification.RegisterNotification((float)secondsToFullChallenge, text);
		}
	}

	private static int GetSecondsToFullChallenge()
	{
		int result = -1;
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			ServerSettingState settingState = ServerInterface.SettingState;
			ServerPlayerState playerState = ServerInterface.PlayerState;
			if (settingState != null && playerState != null)
			{
				int energyRecoveryMax = settingState.m_energyRecoveryMax;
				result = 0;
				if (NetUtil.GetUnixTime(playerState.m_energyRenewsAt) != 0L && playerState.m_numEnergy < energyRecoveryMax)
				{
					DateTime d = playerState.m_energyRenewsAt.AddSeconds((double)(settingState.m_energyRefreshTime * (long)(energyRecoveryMax - playerState.m_numEnergy - 1)));
					result = (int)(d - NetUtil.GetCurrentTime()).TotalSeconds;
				}
			}
		}
		return result;
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLog(string s)
	{
		global::Debug.Log("@ms " + s);
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLogWarning(string s)
	{
		global::Debug.LogWarning("@ms " + s);
	}
}
