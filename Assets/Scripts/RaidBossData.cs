using SaveData;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RaidBossData
{
	public delegate void CallbackRaidBossDataUpdate(RaidBossData data);

	private RaidBossUser m_myData;

	private List<RaidBossUser> m_listData;

	private ServerEventRaidBossBonus m_raidbossReward;

	private long m_id;

	private int m_lv;

	private int m_rarity;

	private string m_discoverer;

	private string m_name;

	private bool m_participation;

	private bool m_end;

	private bool m_clear;

	private bool m_encounter;

	private bool m_creowded;

	private DateTime m_limitTime;

	private long m_bossHp;

	private long m_bossHpMax;

	private RaidBossWindow m_parent;

	private RaidBossData.CallbackRaidBossDataUpdate m_callback;

	private static Comparison<RaidBossUser> __f__am_cache12;

	public RaidBossWindow parent
	{
		get
		{
			return this.m_parent;
		}
		set
		{
			this.m_parent = value;
		}
	}

	public long id
	{
		get
		{
			return this.m_id;
		}
	}

	public int rarity
	{
		get
		{
			return this.m_rarity;
		}
	}

	public int lv
	{
		get
		{
			return this.m_lv;
		}
	}

	public string discoverer
	{
		get
		{
			return this.m_discoverer;
		}
	}

	public string name
	{
		get
		{
			return this.m_name;
		}
	}

	public bool participation
	{
		get
		{
			return this.m_participation;
		}
	}

	public bool end
	{
		get
		{
			return this.m_end;
		}
	}

	public bool clear
	{
		get
		{
			return this.m_clear;
		}
	}

	public bool crowded
	{
		get
		{
			return this.m_creowded;
		}
	}

	public long hp
	{
		get
		{
			return this.m_bossHp;
		}
	}

	public long hpMax
	{
		get
		{
			return this.m_bossHpMax;
		}
	}

	public RaidBossUser myData
	{
		get
		{
			return this.m_myData;
		}
	}

	public List<RaidBossUser> listData
	{
		get
		{
			return this.m_listData;
		}
	}

	public RaidBossData(ServerEventRaidBossState state)
	{
		this.m_myData = null;
		this.m_listData = null;
		this.m_callback = null;
		this.SetData(state);
	}

	public void SetData(ServerEventRaidBossState state)
	{
		this.m_callback = null;
		this.m_id = state.Id;
		this.m_rarity = state.Rarity;
		this.m_lv = state.Level;
		this.m_encounter = state.Encounter;
		this.m_discoverer = state.EncounterName;
		this.m_name = EventUtility.GetRaidBossName(this.m_rarity);
		this.m_participation = state.Participation;
		this.m_end = false;
		this.m_clear = false;
		this.m_creowded = state.Crowded;
		switch (state.Status)
		{
		case 2:
			this.m_end = true;
			break;
		case 3:
			this.m_clear = true;
			this.m_end = true;
			break;
		case 4:
			this.m_clear = true;
			this.m_end = true;
			break;
		}
		this.m_limitTime = state.EscapeAt;
		this.m_bossHp = (long)state.HitPoint;
		this.m_bossHpMax = (long)state.MaxHitPoint;
		this.m_limitTime = this.m_limitTime.AddSeconds(1.0);
	}

	public void SetReward(ServerEventRaidBossBonus bonus)
	{
		this.m_raidbossReward = bonus;
	}

	public float GetHpRate()
	{
		if (this.m_bossHpMax < 0L && this.m_bossHp < 0L)
		{
			return 0f;
		}
		if (this.m_bossHp >= this.m_bossHpMax)
		{
			return 1f;
		}
		return (float)this.m_bossHp / (float)this.m_bossHpMax;
	}

	public TimeSpan GetTimeLimit()
	{
		DateTime currentTime = NetBase.GetCurrentTime();
		return this.m_limitTime - currentTime;
	}

	public string GetTimeLimitString(bool slightlyChangeColor = false)
	{
		string result;
		if (!this.end)
		{
			DateTime currentTime = NetBase.GetCurrentTime();
			TimeSpan timeSpan = this.m_limitTime - currentTime;
			if (timeSpan.Ticks > 0L)
			{
				if (timeSpan.TotalSeconds > 60.0 || !slightlyChangeColor)
				{
					result = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
				}
				else
				{
					result = string.Format("[ff0000]{0:D2}:{1:D2}:{2:D2}[-]", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
				}
			}
			else if (slightlyChangeColor)
			{
				result = "[ff0000]00:00:00[-]";
			}
			else
			{
				result = "00:00:00";
			}
		}
		else if (slightlyChangeColor)
		{
			result = "[ff0000]00:00:00[-]";
		}
		else
		{
			result = "00:00:00";
		}
		return result;
	}

	public bool IsLimit()
	{
		return this.GetTimeLimit().Ticks <= 0L;
	}

	public bool IsDiscoverer()
	{
		return this.m_encounter;
	}

	public bool IsList()
	{
		return this.m_listData != null || this.m_myData != null;
	}

	public bool GetListData(RaidBossData.CallbackRaidBossDataUpdate callback, MonoBehaviour obj = null)
	{
		global::Debug.Log("GetListData:" + this.IsList());
		this.m_callback = callback;
		this.m_callback(this);
		return true;
	}

	public void SetUserList(List<ServerEventRaidBossUserState> stateList)
	{
		if (this.m_listData == null)
		{
			this.m_listData = new List<RaidBossUser>();
		}
		else
		{
			this.m_listData.Clear();
		}
		if (this.m_listData != null && stateList != null)
		{
			string gameID = SystemSaveManager.GetGameID();
			foreach (ServerEventRaidBossUserState current in stateList)
			{
				RaidBossUser raidBossUser = new RaidBossUser(current);
				if (!string.IsNullOrEmpty(raidBossUser.id) && raidBossUser.id != "0000000000" && raidBossUser.destroyCount > 0L)
				{
					this.m_listData.Add(raidBossUser);
					if (!string.IsNullOrEmpty(gameID) && gameID == raidBossUser.id)
					{
						this.m_myData = raidBossUser;
					}
				}
			}
			if (this.m_listData.Count > 0)
			{
				this.m_listData.Sort((RaidBossUser userA, RaidBossUser userB) => (int)(userB.damage - userA.damage));
			}
		}
	}

	public string GetRewardText()
	{
		string text = null;
		if (this.end && this.clear && this.m_raidbossReward != null)
		{
			ServerItem serverItem = new ServerItem(ServerItem.Id.RAIDRING);
			text = serverItem.serverItemName;
			int num = 0;
			num += this.m_raidbossReward.BeatBonus;
			num += this.m_raidbossReward.DamageRateBonus;
			num += this.m_raidbossReward.DamageTopBonus;
			num += this.m_raidbossReward.EncounterBonus;
			num += this.m_raidbossReward.WrestleBonus;
			if (num > 1)
			{
				text = text + " × " + num;
			}
			else if (num <= 0)
			{
				text = null;
			}
		}
		return text;
	}
}
