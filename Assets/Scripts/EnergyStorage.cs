using System;
using UnityEngine;

public class EnergyStorage : MonoBehaviour
{
	private DateTime m_renew_at_time = DateTime.MinValue;

	private TimeSpan m_refresh_time = new TimeSpan(0, 30, 0);

	private uint m_energy_count = 1u;

	private uint m_energyRecoveryMax = 1u;

	public uint Count
	{
		get
		{
			return this.m_energy_count;
		}
	}

	private void Start()
	{
		HudMenuUtility.SetTagHudSaveItem(base.gameObject);
		ServerSettingState settingState = ServerInterface.SettingState;
		if (settingState != null)
		{
			this.m_energyRecoveryMax = (uint)settingState.m_energyRecoveryMax;
		}
		this.OnUpdateSaveDataDisplay();
	}

	private bool CanAddEnergy()
	{
		return this.m_energy_count < this.m_energyRecoveryMax && this.m_renew_at_time <= this.GetCurrentTime();
	}

	public bool IsFillUpCount()
	{
		return this.m_energy_count >= this.m_energyRecoveryMax;
	}

	public void Update()
	{
		bool flag = this.CanAddEnergy();
		if (flag)
		{
			while (flag)
			{
				this.m_energy_count += 1u;
				this.m_renew_at_time += this.m_refresh_time;
				flag = this.CanAddEnergy();
			}
			this.ReflectChallengeCount();
			HudMenuUtility.SendMsgUpdateChallengeDisply();
		}
	}

	public TimeSpan GetRestTimeForRenew()
	{
		return this.m_renew_at_time - this.GetCurrentTime();
	}

	private DateTime GetCurrentTime()
	{
		return NetBase.GetCurrentTime();
	}

	private void ReflectChallengeCount()
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			instance.PlayerData.ChallengeCount = this.m_energy_count;
			instance.SavePlayerData();
		}
		if (ServerInterface.PlayerState != null)
		{
			ServerInterface.PlayerState.m_energyRenewsAt = this.m_renew_at_time;
		}
	}

	private void OnEnergyAwarded(uint energyToAward)
	{
		this.m_energy_count += energyToAward;
	}

	private void OnUpdateSaveDataDisplay()
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			this.m_energy_count = instance.PlayerData.ChallengeCount;
		}
		if (ServerInterface.PlayerState != null)
		{
			this.m_renew_at_time = ServerInterface.PlayerState.m_energyRenewsAt;
		}
		if (ServerInterface.SettingState != null)
		{
			int seconds = (int)ServerInterface.SettingState.m_energyRefreshTime;
			this.m_refresh_time = new TimeSpan(0, 0, seconds);
		}
	}
}
