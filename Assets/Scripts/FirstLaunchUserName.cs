using System;
using UnityEngine;

public class FirstLaunchUserName : MonoBehaviour
{
	private SettingUserName m_settingName;

	public static bool IsFirstLaunch
	{
		get
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				ServerSettingState settingState = ServerInterface.SettingState;
				if (settingState.m_userName != string.Empty)
				{
					return false;
				}
			}
			return true;
		}
		private set
		{
		}
	}

	public bool IsEndPlay
	{
		get
		{
			return this.m_settingName == null || this.m_settingName.IsEndPlay();
		}
		private set
		{
		}
	}

	public void Setup(string anchorName)
	{
		if (anchorName == null)
		{
			return;
		}
		if (!FirstLaunchUserName.IsFirstLaunch)
		{
			return;
		}
		this.m_settingName = base.gameObject.GetComponent<SettingUserName>();
		if (this.m_settingName == null)
		{
			this.m_settingName = base.gameObject.AddComponent<SettingUserName>();
		}
		this.m_settingName.SetCancelButtonUseFlag(false);
		this.m_settingName.Setup(anchorName);
	}

	public void PlayStart()
	{
		if (!FirstLaunchUserName.IsFirstLaunch)
		{
			return;
		}
		if (this.m_settingName == null)
		{
			return;
		}
		this.m_settingName.PlayStart();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
