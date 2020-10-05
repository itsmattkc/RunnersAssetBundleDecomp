using System;
using UnityEngine;

public class RaidEnergyStorage : MonoBehaviour
{
	private const float REFRESH_TIME = 30f;

	private const float UPDATE_TIME = 0.2f;

	private const uint FILL_UP_BORDER_COUNT = 3u;

	[SerializeField]
	private UILabel m_lblEnergy;

	[SerializeField]
	private UILabel m_lblOverEnergy;

	[SerializeField]
	private UILabel m_lblTime;

	private DateTime m_renew_at_time = DateTime.MinValue;

	private TimeSpan m_refresh_time = new TimeSpan(0, 0, 1);

	private TimeSpan m_current_time = new TimeSpan(0, 0, 0);

	private uint m_energy_count = 3u;

	private uint m_energyStock_count;

	private uint m_energyAdd_count;

	private uint m_energyAdd_max;

	private int m_last_count = -1;

	private long m_count_upd;

	private float m_update_time;

	private float m_time;

	public uint energyCount
	{
		get
		{
			return this.m_energy_count + this.m_energyStock_count + this.m_energyAdd_count;
		}
	}

	public static bool CheckEnergy(ref int energyFree, ref int energyBuy, ref DateTime atTime)
	{
		if ((long)(energyFree + energyBuy) < 3L)
		{
			bool flag = atTime <= NetBase.GetCurrentTime();
			int num = 0;
			if (flag)
			{
				while (flag)
				{
					num++;
					if ((long)(energyFree + energyBuy + num) >= 3L)
					{
						atTime = DateTime.MinValue;
						flag = false;
					}
					else
					{
						atTime += new TimeSpan(0, 0, 1800);
						flag = (atTime <= NetBase.GetCurrentTime());
					}
				}
				if (num > 0)
				{
					energyFree += num;
					if ((long)(energyFree + energyBuy) >= 3L)
					{
						atTime = DateTime.MinValue;
					}
				}
				return true;
			}
		}
		return false;
	}

	private bool CanAddEnergy()
	{
		return !this.IsFillUpCount() && this.m_renew_at_time <= this.GetCurrentTime();
	}

	public bool IsFillUpCount()
	{
		return this.m_refresh_time.Ticks <= 0L;
	}

	public void Init()
	{
		this.OnUpdateSaveDataDisplay();
		this.m_count_upd = 0L;
		if (!this.IsFillUpCount())
		{
			this.m_current_time = this.GetRestTimeForRenew();
			if (this.m_lblEnergy != null)
			{
				this.m_lblEnergy.text = this.energyCount.ToString();
				if ((long)this.m_last_count <= (long)((ulong)this.energyCount) || this.m_last_count < 0)
				{
					this.m_lblEnergy.gameObject.SetActive(true);
					this.m_last_count = (int)this.energyCount;
				}
			}
			if (this.m_lblOverEnergy != null)
			{
				this.m_lblOverEnergy.gameObject.SetActive(false);
			}
			if (this.m_lblTime != null)
			{
				if (this.m_current_time.Minutes >= 0 && this.m_current_time.Seconds >= 0)
				{
					this.m_lblTime.text = string.Format("{0:D2}:{1:D2}", this.m_current_time.Minutes, this.m_current_time.Seconds);
				}
				this.m_lblTime.gameObject.SetActive(true);
			}
			this.m_update_time = 0.2f;
		}
		else
		{
			if (this.m_lblEnergy != null)
			{
				this.m_lblEnergy.gameObject.SetActive(false);
			}
			if (this.m_lblOverEnergy != null)
			{
				this.m_lblOverEnergy.gameObject.SetActive(true);
				if ((long)this.m_last_count <= (long)((ulong)this.energyCount) || this.m_last_count < 0)
				{
					this.m_lblOverEnergy.text = this.energyCount.ToString();
					this.m_last_count = (int)this.energyCount;
				}
			}
			if (this.m_lblTime != null)
			{
				this.m_lblTime.gameObject.SetActive(false);
			}
			this.m_update_time = 1f;
		}
	}

	private void InitEnergy()
	{
		if (!this.IsFillUpCount())
		{
			this.m_current_time = this.GetRestTimeForRenew();
			if (this.m_lblEnergy != null)
			{
				this.m_lblEnergy.gameObject.SetActive(true);
				this.m_lblEnergy.text = this.energyCount.ToString();
				this.m_last_count = (int)this.energyCount;
			}
			if (this.m_lblOverEnergy != null)
			{
				this.m_lblOverEnergy.gameObject.SetActive(false);
			}
			if (this.m_lblTime != null)
			{
				this.m_lblTime.gameObject.SetActive(true);
				if (this.m_current_time.Minutes >= 0 && this.m_current_time.Seconds >= 0)
				{
					this.m_lblTime.text = string.Format("{0:D2}:{1:D2}", this.m_current_time.Minutes, this.m_current_time.Seconds);
				}
			}
		}
		else
		{
			if (this.m_lblEnergy != null)
			{
				this.m_lblEnergy.gameObject.SetActive(false);
			}
			if (this.m_lblOverEnergy != null)
			{
				this.m_lblOverEnergy.gameObject.SetActive(true);
				this.m_lblOverEnergy.text = this.energyCount.ToString();
				this.m_last_count = (int)this.energyCount;
			}
			if (this.m_lblTime != null)
			{
				this.m_lblTime.gameObject.SetActive(false);
			}
		}
	}

	private void UpdateEnergy()
	{
		this.m_update_time -= Time.deltaTime;
		if (this.m_update_time <= 0f)
		{
			this.m_count_upd += 1L;
			if (!this.IsFillUpCount())
			{
				this.m_current_time = this.GetRestTimeForRenew();
				if (this.m_lblEnergy != null)
				{
					this.m_lblEnergy.gameObject.SetActive(true);
					if ((long)this.m_last_count <= (long)((ulong)this.energyCount) || this.m_count_upd > 3L)
					{
						this.m_lblEnergy.text = this.energyCount.ToString();
						this.m_last_count = (int)this.energyCount;
						this.m_count_upd = 0L;
					}
				}
				if (this.m_lblOverEnergy != null)
				{
					this.m_lblOverEnergy.gameObject.SetActive(false);
				}
				if (this.m_lblTime != null)
				{
					this.m_lblTime.gameObject.SetActive(true);
					if (this.m_current_time.Minutes >= 0 && this.m_current_time.Seconds >= 0)
					{
						this.m_lblTime.text = string.Format("{0:D2}:{1:D2}", this.m_current_time.Minutes, this.m_current_time.Seconds);
					}
				}
				this.m_update_time = 0.2f;
			}
			else
			{
				if (this.m_lblEnergy != null)
				{
					this.m_lblEnergy.gameObject.SetActive(false);
				}
				if (this.m_lblOverEnergy != null)
				{
					this.m_lblOverEnergy.gameObject.SetActive(true);
					if ((long)this.m_last_count <= (long)((ulong)this.energyCount) || this.m_count_upd > 3L)
					{
						this.m_lblOverEnergy.text = this.energyCount.ToString();
						this.m_last_count = (int)this.energyCount;
						this.m_count_upd = 0L;
					}
				}
				if (this.m_lblTime != null)
				{
					this.m_lblTime.gameObject.SetActive(false);
				}
				this.m_update_time = 1f;
			}
		}
	}

	private void Update()
	{
		this.m_time += Time.deltaTime;
		if (this.m_time > 0.1f)
		{
			bool flag = this.CanAddEnergy();
			if (flag)
			{
				while (flag)
				{
					this.m_energyAdd_count += 1u;
					if (this.m_energyAdd_count >= this.m_energyAdd_max)
					{
						this.m_renew_at_time = DateTime.MinValue;
						this.m_refresh_time = new TimeSpan(0, 0, 0);
						flag = false;
					}
					else
					{
						this.m_renew_at_time += this.m_refresh_time;
						flag = this.CanAddEnergy();
					}
				}
				this.ReflectChallengeCount();
			}
			this.UpdateEnergy();
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

	public void ReflectChallengeCount()
	{
		if (EventManager.Instance != null)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			global::Debug.Log(string.Concat(new object[]
			{
				"+ RaidEnergyStorage ReflectChallengeCount ChallengeCount:",
				EventManager.Instance.RaidbossChallengeCount,
				" !!!!!!!!!!!!!!!!! time:",
				realtimeSinceStartup
			}));
			if (EventManager.Instance.RaidBossState != null)
			{
				EventManager.Instance.RaidBossState.RaidBossEnergy = (int)(this.m_energy_count + this.m_energyAdd_count);
				EventManager.Instance.RaidBossState.EnergyRenewsAt = this.m_renew_at_time;
			}
			global::Debug.Log(string.Concat(new object[]
			{
				"- RaidEnergyStorage ReflectChallengeCount ChallengeCount:",
				EventManager.Instance.RaidbossChallengeCount,
				" !!!!!!!!!!!!!!!!! time:",
				realtimeSinceStartup
			}));
		}
	}

	private void OnUpdateSaveDataDisplay()
	{
		this.m_time = 0f;
		this.m_energy_count = 0u;
		this.m_energyStock_count = 0u;
		this.m_energyAdd_count = 0u;
		this.m_energyAdd_max = 0u;
		this.m_renew_at_time = DateTime.MinValue;
		this.m_refresh_time = new TimeSpan(0, 0, 1);
		this.m_last_count = -1;
		EventManager instance = EventManager.Instance;
		if (instance != null)
		{
			ServerEventUserRaidBossState raidBossState = instance.RaidBossState;
			if (raidBossState != null)
			{
				this.m_energy_count = (uint)raidBossState.RaidBossEnergy;
				this.m_energyStock_count = (uint)raidBossState.RaidbossEnergyBuy;
				if (this.m_energy_count + this.m_energyStock_count < 3u)
				{
					this.m_energyAdd_max = 3u - (this.m_energy_count + this.m_energyStock_count);
					this.m_refresh_time = new TimeSpan(0, 0, 1800);
					this.m_renew_at_time = raidBossState.EnergyRenewsAt;
				}
				else
				{
					this.m_renew_at_time = DateTime.MinValue;
					this.m_refresh_time = new TimeSpan(0, 0, 0);
				}
			}
		}
		this.InitEnergy();
	}
}
