using DataTable;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class RaidBossInfo : EventBaseInfo
{
	public delegate void CallbackRaidBossInfoUpdate(RaidBossInfo info);

	private static RaidBossData m_currentRaidData;

	private string m_bossName;

	private long m_raidRing;

	private long m_raidRingOffset;

	private List<RaidBossData> m_raidData;

	private RaidBossInfo.CallbackRaidBossInfoUpdate m_callback;

	public static RaidBossData currentRaidData
	{
		get
		{
			return RaidBossInfo.m_currentRaidData;
		}
		set
		{
			RaidBossInfo.m_currentRaidData = value;
		}
	}

	public string bossName
	{
		get
		{
			return this.m_bossName;
		}
	}

	public long totalDestroyCount
	{
		get
		{
			return this.m_totalPoint;
		}
		set
		{
			this.m_totalPoint = value;
		}
	}

	public long raidRing
	{
		get
		{
			return this.m_raidRing + this.m_raidRingOffset;
		}
		set
		{
			this.m_raidRing = value;
			this.m_raidRingOffset = 0L;
		}
	}

	public long raidRingOffset
	{
		get
		{
			return this.m_raidRingOffset;
		}
		set
		{
			this.m_raidRingOffset = value;
		}
	}

	public List<RaidBossData> raidData
	{
		get
		{
			return this.m_raidData;
		}
	}

	public RaidBossInfo.CallbackRaidBossInfoUpdate callback
	{
		set
		{
			this.m_callback = value;
		}
	}

	public int raidNumActive
	{
		get
		{
			if (this.m_raidData == null)
			{
				return 0;
			}
			int num = 0;
			foreach (RaidBossData current in this.m_raidData)
			{
				if (current != null && (!current.end || current.IsDiscoverer() || !current.participation))
				{
					num++;
				}
			}
			return num;
		}
	}

	public int raidNumLost
	{
		get
		{
			if (this.m_raidData == null)
			{
				return 0;
			}
			int num = 0;
			foreach (RaidBossData current in this.m_raidData)
			{
				if (current != null && current.end && !current.IsDiscoverer() && current.participation)
				{
					num++;
				}
			}
			return num;
		}
	}

	public override void Init()
	{
		if (this.m_init)
		{
			return;
		}
		this.m_eventName = "RaidBoss(正式なテキストを追加してください)";
		string text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Result", "ui_Lbl_word_boss_destroy");
		List<ServerEventReward> rewardList = EventManager.Instance.RewardList;
		this.m_eventMission = new List<EventMission>();
		if (rewardList != null)
		{
			for (int i = 0; i < rewardList.Count; i++)
			{
				ServerEventReward serverEventReward = rewardList[i];
				this.m_eventMission.Add(new EventMission(text, serverEventReward.Param, serverEventReward.m_itemId, serverEventReward.m_num));
			}
		}
		this.m_rewardChao = new List<ChaoData>();
		RewardChaoData rewardChaoData = EventManager.Instance.GetRewardChaoData();
		if (rewardChaoData != null)
		{
			ChaoData chaoData = ChaoTable.GetChaoData(rewardChaoData.chao_id);
			if (chaoData != null)
			{
				this.m_rewardChao.Add(chaoData);
			}
		}
		EyeCatcherChaoData[] eyeCatcherChaoDatas = EventManager.Instance.GetEyeCatcherChaoDatas();
		if (eyeCatcherChaoDatas != null)
		{
			EyeCatcherChaoData[] array = eyeCatcherChaoDatas;
			for (int j = 0; j < array.Length; j++)
			{
				EyeCatcherChaoData eyeCatcherChaoData = array[j];
				ChaoData chaoData2 = ChaoTable.GetChaoData(eyeCatcherChaoData.chao_id);
				if (chaoData2 != null)
				{
					this.m_rewardChao.Add(chaoData2);
				}
			}
		}
		int chaoLevel = ChaoTable.ChaoMaxLevel();
		this.m_leftTitle = TextUtility.GetCommonText("Roulette", "ui_Lbl_word_best_chao");
		if (this.m_rewardChao.Count > 0)
		{
			this.m_leftName = this.m_rewardChao[0].nameTwolines;
			this.m_leftText = this.m_rewardChao[0].GetDetailsLevel(chaoLevel);
			switch (this.m_rewardChao[0].rarity)
			{
			case ChaoData.Rarity.NORMAL:
				this.m_leftBg = "ui_tex_chao_bg_0";
				break;
			case ChaoData.Rarity.RARE:
				this.m_leftBg = "ui_tex_chao_bg_1";
				break;
			case ChaoData.Rarity.SRARE:
				this.m_leftBg = "ui_tex_chao_bg_2";
				break;
			}
			switch (this.m_rewardChao[0].charaAtribute)
			{
			case CharacterAttribute.SPEED:
				this.m_chaoTypeIcon = "ui_chao_set_type_icon_speed";
				break;
			case CharacterAttribute.FLY:
				this.m_chaoTypeIcon = "ui_chao_set_type_icon_fly";
				break;
			case CharacterAttribute.POWER:
				this.m_chaoTypeIcon = "ui_chao_set_type_icon_power";
				break;
			}
		}
		this.m_caption = TextUtility.GetCommonText("Event", "ui_Lbl_event_reward_list");
		this.m_rightTitle = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_destroy_total");
		this.m_rightTitleIcon = "ui_event_object_icon";
		this.m_init = true;
	}

	public override void UpdateData(MonoBehaviour obj)
	{
		if (!this.m_init)
		{
			this.Init();
		}
		else
		{
			this.m_callback(this);
		}
	}

	public bool IsAttention()
	{
		bool result = false;
		if (this.m_raidData != null && this.m_raidData.Count > 0)
		{
			foreach (RaidBossData current in this.m_raidData)
			{
				if (!current.end && current.id != 0L)
				{
					result = true;
					break;
				}
				if (current.participation)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	public static RaidBossInfo CreateData(List<RaidBossData> raidBossDatas)
	{
		RaidBossInfo raidBossInfo = new RaidBossInfo();
		raidBossInfo.Init();
		raidBossInfo.m_raidData = new List<RaidBossData>();
		if (raidBossDatas != null)
		{
			foreach (RaidBossData current in raidBossDatas)
			{
				raidBossInfo.m_raidData.Add(current);
			}
		}
		return raidBossInfo;
	}

	public static RaidBossInfo CreateDataForDebugData(List<RaidBossData> raidBossDatas)
	{
		RaidBossInfo result = null;
		global::Debug.LogWarning("RaidBossInfo:DummyDataCreate  not create!!!");
		return result;
	}
}
