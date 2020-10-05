using DataTable;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class SpecialStageInfo : EventBaseInfo
{
	private string m_eventCaption;

	private string m_eventText;

	public string eventCaption
	{
		get
		{
			return this.m_eventCaption;
		}
	}

	public string eventText
	{
		get
		{
			return this.m_eventText;
		}
	}

	public override void Init()
	{
		if (this.m_init)
		{
			return;
		}
		this.m_eventName = HudUtility.GetEventStageName();
		this.m_totalPoint = EventManager.Instance.CollectCount;
		this.m_totalPointTarget = EventBaseInfo.EVENT_AGGREGATE_TARGET.SP_CRYSTAL;
		this.m_eventCaption = this.m_eventName;
		this.m_eventText = "スペシャルステージイベント説明（デバック）\n\n\u3000あいうえお\n\u30001234567890\n\u3000ABCDEFG\n\n\u3000期間: XX/XX  XX:XX  -  XX/XX  XX:XX";
		this.m_eventMission = new List<EventMission>();
		string text = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Result", "ui_Lbl_word_get_total");
		List<ServerEventReward> rewardList = EventManager.Instance.RewardList;
		if (rewardList != null)
		{
			for (int i = 0; i < rewardList.Count; i++)
			{
				ServerEventReward serverEventReward = rewardList[i];
				string eventSpObjectName = HudUtility.GetEventSpObjectName();
				string name = TextUtility.Replace(text, "{PARAM_OBJ}", eventSpObjectName);
				this.m_eventMission.Add(new EventMission(name, serverEventReward.Param, serverEventReward.m_itemId, serverEventReward.m_num));
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
		this.m_rightTitle = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_get_total");
		this.m_rightTitleIcon = "ui_event_object_icon";
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
		this.m_totalPointTarget = EventBaseInfo.EVENT_AGGREGATE_TARGET.SP_CRYSTAL;
		this.m_dummyData = true;
		this.m_eventName = "SpecialStage";
		this.m_eventCaption = "スペシャルステージイベント（デバック）";
		this.m_eventText = "スペシャルステージイベント説明（デバック）\n\n\u3000あいうえお\n\u30001234567890\n\u3000ABCDEFG\n\n\u3000期間: XX/XX  XX:XX  -  XX/XX  XX:XX";
		this.m_rewardChao = new List<ChaoData>();
		List<ChaoData> dataTable = ChaoTable.GetDataTable(ChaoData.Rarity.NORMAL);
		List<ChaoData> dataTable2 = ChaoTable.GetDataTable(ChaoData.Rarity.RARE);
		List<ChaoData> dataTable3 = ChaoTable.GetDataTable(ChaoData.Rarity.SRARE);
		bool flag = false;
		if (dataTable.Count > 0 && dataTable2.Count > 0 && dataTable3.Count > 0)
		{
			switch (UnityEngine.Random.Range(1, 3))
			{
			case 0:
				this.m_rewardChao.Add(dataTable[UnityEngine.Random.Range(0, dataTable.Count - 1)]);
				break;
			case 1:
				this.m_rewardChao.Add(dataTable2[UnityEngine.Random.Range(0, dataTable2.Count - 1)]);
				break;
			case 2:
				this.m_rewardChao.Add(dataTable3[UnityEngine.Random.Range(0, dataTable3.Count - 1)]);
				break;
			default:
				this.m_rewardChao.Add(dataTable3[UnityEngine.Random.Range(0, dataTable3.Count - 1)]);
				break;
			}
		}
		else
		{
			flag = true;
			ChaoData chaoData = new ChaoData();
			chaoData.SetDummyData();
			this.m_rewardChao.Add(chaoData);
		}
		this.m_eventMission = new List<EventMission>();
		List<int> list = new List<int>();
		list.Add(400000 + this.m_rewardChao[0].id);
		list.Add(120100);
		list.Add(121000);
		list.Add(120101);
		list.Add(121001);
		list.Add(400000 + this.m_rewardChao[0].id);
		list.Add(120102);
		list.Add(121002);
		list.Add(120103);
		list.Add(121003);
		list.Add(400000 + this.m_rewardChao[0].id);
		list.Add(120104);
		list.Add(121004);
		list.Add(120105);
		list.Add(121005);
		list.Add(120106);
		list.Add(121006);
		list.Add(120107);
		list.Add(400000 + this.m_rewardChao[0].id);
		for (int i = 0; i < 10; i++)
		{
			long point = (long)((i + 1) * (i + 1) * (i + 1) * (i + 1) * (i + 1) * (i + 1) * (i + 1) * (i + 1));
			this.m_eventMission.Add(new EventMission("累計SPクリスタル_" + (i + 1), point, list[i % list.Count], i));
		}
		int chaoLevel = ChaoTable.ChaoMaxLevel();
		this.m_leftTitle = "今週の目玉";
		if (this.m_rewardChao.Count > 0)
		{
			this.m_leftName = this.m_rewardChao[0].nameTwolines;
			this.m_leftText = ((!flag) ? this.m_rewardChao[0].GetDetailsLevel(chaoLevel) : "dummy text");
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
		this.m_caption = "スペシャルステージ報酬(デバック)";
		this.m_rightTitle = "○○を集めろ";
		this.m_rightTitleIcon = "ui_event_object_icon";
		this.m_init = true;
	}

	public static SpecialStageInfo CreateData()
	{
		SpecialStageInfo specialStageInfo = new SpecialStageInfo();
		specialStageInfo.Init();
		return specialStageInfo;
	}

	public static SpecialStageInfo CreateDummyData()
	{
		SpecialStageInfo result = null;
		global::Debug.LogWarning("SpecialStageInfo:DummyDataCreate  not create!!!");
		return result;
	}
}
