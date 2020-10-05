using DataTable;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class EtcEventInfo : EventBaseInfo
{
	public override void Init()
	{
		if (this.m_init)
		{
			return;
		}
		if (EventManager.Instance == null)
		{
			return;
		}
		string cellID = "ui_Lbl_word_animl_get_event";
		string text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Result", "ui_Lbl_word_animal_get_total");
		this.m_totalPointTarget = EventBaseInfo.EVENT_AGGREGATE_TARGET.ANIMAL;
		this.m_rightTitle = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_animal_get_total");
		switch (EventManager.Instance.CollectType)
		{
		case EventManager.CollectEventType.GET_RING:
			this.m_totalPointTarget = EventBaseInfo.EVENT_AGGREGATE_TARGET.RING;
			this.m_rightTitle = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_ring_get_total");
			cellID = "ui_Lbl_word_ring_get_event";
			text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Result", "ui_Lbl_word_ring_get_total");
			break;
		case EventManager.CollectEventType.RUN_DISTANCE:
			this.m_totalPointTarget = EventBaseInfo.EVENT_AGGREGATE_TARGET.DISTANCE;
			this.m_rightTitle = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_run_distance_total");
			cellID = "ui_Lbl_word_run_distance_event";
			text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Result", "ui_Lbl_word_run_distance_get_total");
			break;
		}
		this.m_eventName = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", cellID);
		this.m_totalPoint = EventManager.Instance.CollectCount;
		this.m_caption = TextUtility.GetCommonText("Event", "ui_Lbl_event_reward_list");
		this.m_rightTitleIcon = "ui_event_object_icon";
		this.m_eventMission = new List<EventMission>();
		List<ServerEventReward> rewardList = EventManager.Instance.RewardList;
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
		this.m_leftTitle = TextUtility.GetCommonText("Roulette", "ui_Lbl_word_recommended_chao");
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
		this.m_init = true;
	}

	public override void UpdateData(MonoBehaviour obj)
	{
		if (!this.m_init)
		{
			this.Init();
		}
		else if (!this.m_dummyData)
		{
		}
	}

	protected override void DebugInit()
	{
		if (this.m_init)
		{
			return;
		}
		this.m_totalPoint = 123456L;
		this.m_totalPointTarget = EventBaseInfo.EVENT_AGGREGATE_TARGET.ANIMAL;
		this.m_dummyData = true;
		this.m_eventName = "EtcEvent";
		this.m_eventMission = new List<EventMission>();
		List<int> list = new List<int>();
		list.Add(120100);
		list.Add(121000);
		list.Add(120101);
		list.Add(121001);
		list.Add(120102);
		list.Add(121002);
		list.Add(120103);
		list.Add(121003);
		list.Add(120104);
		list.Add(121004);
		list.Add(120105);
		list.Add(121005);
		list.Add(120106);
		list.Add(121006);
		list.Add(120107);
		list.Add(121007);
		for (int i = 0; i < 10; i++)
		{
			long point = (long)((i + 1) * (i + 1) * (i + 1) * (i + 1) * (i + 1) * (i + 1) * (i + 1) * (i + 1));
			this.m_eventMission.Add(new EventMission("獲得動物数_" + (i + 1), point, list[i % list.Count], i));
		}
		this.m_rewardChao = new List<ChaoData>();
		List<ChaoData> dataTable = ChaoTable.GetDataTable(ChaoData.Rarity.SRARE);
		if (dataTable != null && dataTable.Count > 0)
		{
			this.m_rewardChao.Add(dataTable[UnityEngine.Random.Range(0, dataTable.Count - 1)]);
		}
		int chaoLevel = ChaoTable.ChaoMaxLevel();
		this.m_leftTitle = "今週の目玉";
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
		this.m_caption = "動物獲得イベント報酬(デバック)";
		this.m_rightTitle = "動物を集めろ";
		this.m_rightTitleIcon = "ui_event_object_icon";
		this.m_init = true;
	}

	public static EtcEventInfo CreateData()
	{
		EtcEventInfo etcEventInfo = new EtcEventInfo();
		etcEventInfo.Init();
		return etcEventInfo;
	}

	public static EtcEventInfo CreateDummyData()
	{
		EtcEventInfo result = null;
		global::Debug.LogWarning("EtcEventInfo:DummyDataCreate  not create!!!");
		return result;
	}
}
