using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class ServerWheelOptionsOrgGen : ServerWheelOptionsOrg
{
	private ServerWheelOptionsGeneral m_orgData;

	public override bool isValid
	{
		get
		{
			return true;
		}
	}

	public override bool isRemainingRefresh
	{
		get
		{
			bool result = false;
			if (this.m_orgData != null && this.m_orgData.type == RouletteUtility.WheelType.Rankup && RouletteManager.Instance != null)
			{
				result = RouletteManager.Instance.currentRankup;
			}
			return result;
		}
	}

	public override int itemWon
	{
		get
		{
			return -1;
		}
	}

	public override ServerItem itemWonData
	{
		get
		{
			return default(ServerItem);
		}
	}

	public override int rouletteId
	{
		get
		{
			return this.m_orgData.rouletteId;
		}
	}

	public override int multi
	{
		get
		{
			return this.m_orgData.multi;
		}
	}

	public override int numJackpotRing
	{
		get
		{
			return this.m_orgData.jackpotRing;
		}
	}

	public ServerWheelOptionsOrgGen(ServerWheelOptionsGeneral data)
	{
		if (data == null)
		{
			return;
		}
		this.m_category = RouletteUtility.GetRouletteCategory(data);
		this.m_init = true;
		this.m_type = RouletteUtility.WheelType.Rankup;
		if (this.m_orgData == null)
		{
			this.m_orgData = new ServerWheelOptionsGeneral();
		}
		data.CopyTo(this.m_orgData);
		this.UpdateItemWeights();
	}

	public override void Setup(ServerChaoWheelOptions data)
	{
	}

	public override void Setup(ServerWheelOptions data)
	{
	}

	public override void Setup(ServerWheelOptionsGeneral data)
	{
		if (data == null)
		{
			return;
		}
		this.m_category = RouletteUtility.GetRouletteCategory(data);
		this.m_init = true;
		this.m_type = RouletteUtility.WheelType.Rankup;
		int selectIndex = 0;
		int multi = 1;
		if (this.m_orgData == null)
		{
			this.m_orgData = new ServerWheelOptionsGeneral();
		}
		else
		{
			selectIndex = this.m_orgData.currentCostSelect;
			multi = this.m_orgData.multi;
		}
		data.CopyTo(this.m_orgData);
		data.ChangeCostItem(selectIndex);
		if (!data.ChangeMulti(multi) || data.rank != RouletteUtility.WheelRank.Normal)
		{
			data.ChangeMulti(1);
		}
		this.UpdateItemWeights();
	}

	public override bool ChangeMulti(int multi)
	{
		return this.m_orgData != null && this.m_orgData.ChangeMulti(multi);
	}

	public override bool IsMulti(int multi)
	{
		return this.m_orgData != null && this.m_orgData.IsMulti(multi);
	}

	public override int GetRouletteBoardPattern()
	{
		int result = 0;
		if (this.m_init)
		{
			result = this.m_orgData.patternType;
		}
		return result;
	}

	public override string GetRouletteArrowSprite()
	{
		if (this.m_orgData != null)
		{
			return this.m_orgData.spriteNameArrow;
		}
		return null;
	}

	public override string GetRouletteBgSprite()
	{
		if (this.m_orgData != null)
		{
			return this.m_orgData.spriteNameBg;
		}
		return null;
	}

	public override string GetRouletteBoardSprite()
	{
		if (this.m_orgData != null)
		{
			return this.m_orgData.spriteNameBoard;
		}
		return null;
	}

	public override string GetRouletteTicketSprite()
	{
		if (this.m_orgData != null)
		{
			return this.m_orgData.spriteNameCostItem;
		}
		return null;
	}

	public override RouletteUtility.WheelRank GetRouletteRank()
	{
		RouletteUtility.WheelRank result = RouletteUtility.WheelRank.Normal;
		if (this.m_init && this.m_orgData != null)
		{
			result = this.m_orgData.rank;
		}
		return result;
	}

	public override float GetCellWeight(int cellIndex)
	{
		float result = 0f;
		if (this.m_orgData != null && this.m_orgData.itemLenght > cellIndex)
		{
			result = this.m_orgData.GetCellWeight(cellIndex);
		}
		return result;
	}

	public override int GetCellEgg(int cellIndex)
	{
		int result = -1;
		if (this.m_orgData != null && this.m_orgData.itemLenght > cellIndex)
		{
			int id = 0;
			int num = 0;
			float num2 = 0f;
			this.m_orgData.GetCell(cellIndex, out id, out num, out num2);
			ServerItem serverItem = new ServerItem((ServerItem.Id)id);
			if (serverItem.idType == ServerItem.IdType.CHAO)
			{
				result = (int)(serverItem.id / (ServerItem.Id)1000 % (ServerItem.Id)10);
			}
		}
		return result;
	}

	public override ServerItem GetCellItem(int cellIndex, out int num)
	{
		ServerItem result = default(ServerItem);
		num = 1;
		if (this.m_orgData != null && this.m_orgData.itemLenght > cellIndex)
		{
			int id = 0;
			float num2 = 0f;
			this.m_orgData.GetCell(cellIndex, out id, out num, out num2);
			result = new ServerItem((ServerItem.Id)id);
		}
		else
		{
			num = -1;
		}
		return result;
	}

	public override void PlayBgm(float delay = 0f)
	{
		if (this.m_init && EventManager.Instance != null)
		{
			EventManager.EventType type = EventManager.Instance.Type;
			string text = null;
			string cueSheetName = "BGM";
			if ((!RouletteUtility.isTutorial || this.m_category != RouletteCategory.PREMIUM) && type != EventManager.EventType.NUM && type != EventManager.EventType.UNKNOWN && EventManager.Instance.IsInEvent() && EventCommonDataTable.Instance != null)
			{
				string data;
				switch (base.category)
				{
				case RouletteCategory.SPECIAL:
					data = EventCommonDataTable.Instance.GetData(EventCommonDataItem.RouletteS_BgmName);
					goto IL_CB;
				}
				data = EventCommonDataTable.Instance.GetData(EventCommonDataItem.Roulette_BgmName);
				IL_CB:
				if (!string.IsNullOrEmpty(data))
				{
					cueSheetName = "BGM_" + EventManager.GetEventTypeName(EventManager.Instance.Type);
					text = data;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				switch (base.category)
				{
				case RouletteCategory.SPECIAL:
					text = "bgm_sys_s_roulette";
					goto IL_149;
				}
				text = "bgm_sys_roulette";
			}
			IL_149:
			if (!string.IsNullOrEmpty(text))
			{
				RouletteManager.PlayBgm(text, delay, cueSheetName, false);
			}
		}
	}

	public override void PlaySe(ServerWheelOptionsData.SE_TYPE seType, float delay = 0f)
	{
		if (this.m_init && EventManager.Instance != null)
		{
			string text = null;
			string cueSheetName = "SE";
			EventManager.EventType type = EventManager.Instance.Type;
			switch (seType)
			{
			case ServerWheelOptionsData.SE_TYPE.Open:
				text = "sys_window_open";
				break;
			case ServerWheelOptionsData.SE_TYPE.Close:
				text = "sys_window_close";
				break;
			case ServerWheelOptionsData.SE_TYPE.Click:
				text = "sys_menu_decide";
				break;
			case ServerWheelOptionsData.SE_TYPE.Spin:
				if (!RouletteUtility.isTutorial || this.m_category != RouletteCategory.PREMIUM)
				{
					switch (base.category)
					{
					case RouletteCategory.PREMIUM:
					case RouletteCategory.SPECIAL:
						if ((base.category == RouletteCategory.PREMIUM || base.category == RouletteCategory.SPECIAL) && type != EventManager.EventType.NUM && type != EventManager.EventType.UNKNOWN && EventManager.Instance.IsInEvent())
						{
							string data = EventCommonDataTable.Instance.GetData(EventCommonDataItem.RouletteDecide_SeCueName);
							if (!string.IsNullOrEmpty(data))
							{
								cueSheetName = "SE_" + EventManager.GetEventTypeName(EventManager.Instance.Type);
								text = data;
							}
						}
						goto IL_158;
					}
					text = "sys_menu_decide";
				}
				IL_158:
				if (string.IsNullOrEmpty(text))
				{
					text = "sys_menu_decide";
				}
				break;
			case ServerWheelOptionsData.SE_TYPE.SpinError:
				text = "sys_error";
				break;
			case ServerWheelOptionsData.SE_TYPE.Arrow:
				text = "sys_roulette_arrow";
				break;
			case ServerWheelOptionsData.SE_TYPE.Skip:
				text = "sys_page_skip";
				break;
			case ServerWheelOptionsData.SE_TYPE.GetItem:
				text = "sys_roulette_itemget";
				break;
			case ServerWheelOptionsData.SE_TYPE.GetRare:
				text = "sys_roulette_itemget_rare";
				break;
			case ServerWheelOptionsData.SE_TYPE.GetRankup:
				text = "sys_roulette_levelup";
				break;
			case ServerWheelOptionsData.SE_TYPE.GetJackpot:
				text = "sys_roulette_jackpot";
				break;
			case ServerWheelOptionsData.SE_TYPE.Multi:
				text = "boss_scene_change";
				break;
			case ServerWheelOptionsData.SE_TYPE.Change:
				if (!RouletteUtility.isTutorial || this.m_category != RouletteCategory.PREMIUM)
				{
					switch (base.category)
					{
					case RouletteCategory.PREMIUM:
					case RouletteCategory.SPECIAL:
						if ((base.category == RouletteCategory.PREMIUM || base.category == RouletteCategory.SPECIAL) && type != EventManager.EventType.NUM && type != EventManager.EventType.UNKNOWN && EventManager.Instance.IsInEvent())
						{
							string data2 = EventCommonDataTable.Instance.GetData(EventCommonDataItem.RouletteChange_SeCueName);
							if (!string.IsNullOrEmpty(data2))
							{
								cueSheetName = "SE_" + EventManager.GetEventTypeName(EventManager.Instance.Type);
								text = data2;
							}
						}
						goto IL_28D;
					}
					text = "sys_roulette_change";
				}
				IL_28D:
				if (string.IsNullOrEmpty(text))
				{
					text = "sys_roulette_change";
				}
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (delay <= 0f)
				{
					SoundManager.SePlay(text, cueSheetName);
				}
				else
				{
					RouletteManager.PlaySe(text, delay, cueSheetName);
				}
			}
		}
	}

	public override ServerWheelOptionsData.SPIN_BUTTON GetSpinButtonSeting(out int count, out bool btnActive)
	{
		ServerWheelOptionsData.SPIN_BUTTON sPIN_BUTTON = ServerWheelOptionsData.SPIN_BUTTON.NONE;
		count = 0;
		btnActive = false;
		if (this.m_init && SaveDataManager.Instance != null && SaveDataManager.Instance.ItemData != null && this.m_orgData != null)
		{
			if (RouletteUtility.isTutorial && this.m_category == RouletteCategory.PREMIUM)
			{
				count = -1;
				sPIN_BUTTON = ServerWheelOptionsData.SPIN_BUTTON.FREE;
				btnActive = true;
			}
			else
			{
				int currentCostItemId = this.m_orgData.GetCurrentCostItemId();
				int multi = this.m_orgData.multi;
				int costItemNum = this.m_orgData.GetCostItemNum(currentCostItemId);
				count = this.m_orgData.GetCostItemCost(currentCostItemId) * multi;
				sPIN_BUTTON = this.m_orgData.GetSpinButton();
				if (costItemNum >= count)
				{
					btnActive = true;
				}
				if (sPIN_BUTTON == ServerWheelOptionsData.SPIN_BUTTON.FREE)
				{
					btnActive = true;
					count = this.m_orgData.remainingFree;
				}
			}
		}
		return sPIN_BUTTON;
	}

	public override ServerWheelOptionsData.SPIN_BUTTON GetSpinButtonSeting()
	{
		ServerWheelOptionsData.SPIN_BUTTON result = ServerWheelOptionsData.SPIN_BUTTON.NONE;
		if (this.m_init && SaveDataManager.Instance != null && SaveDataManager.Instance.ItemData != null && this.m_orgData != null)
		{
			if (RouletteUtility.isTutorial && this.m_category == RouletteCategory.PREMIUM)
			{
				result = ServerWheelOptionsData.SPIN_BUTTON.FREE;
			}
			else
			{
				result = this.m_orgData.GetSpinButton();
			}
		}
		return result;
	}

	public override List<int> GetSpinCostItemIdList()
	{
		List<int> result = null;
		if (this.m_orgData != null)
		{
			result = this.m_orgData.GetCostItemList();
		}
		return result;
	}

	public override int GetSpinCostItemCost(int costItemId)
	{
		int result = 0;
		if (this.m_orgData != null)
		{
			if (costItemId <= 0)
			{
				result = 1;
			}
			else
			{
				result = this.m_orgData.GetCostItemCost(costItemId);
			}
		}
		return result;
	}

	public override int GetSpinCostItemNum(int costItemId)
	{
		int result = 0;
		if (this.m_orgData != null)
		{
			if (costItemId <= 0)
			{
				result = this.m_orgData.remainingFree;
			}
			else
			{
				result = this.m_orgData.GetCostItemNum(costItemId);
			}
		}
		return result;
	}

	public override bool ChangeSpinCost(int selectIndex)
	{
		bool result = false;
		if (this.IsChangeSpinCost())
		{
			result = this.m_orgData.ChangeCostItem(selectIndex);
		}
		return result;
	}

	public override bool IsChangeSpinCost()
	{
		bool result = false;
		if (this.m_orgData.GetSpinButton() != ServerWheelOptionsData.SPIN_BUTTON.FREE)
		{
			List<int> spinCostItemIdList = this.GetSpinCostItemIdList();
			if (spinCostItemIdList.Count > 1)
			{
				result = true;
			}
		}
		return result;
	}

	public override int GetSpinCostCurrentIndex()
	{
		return this.m_orgData.currentCostSelect;
	}

	public override bool GetEggSeting(out int count)
	{
		bool result = false;
		count = RouletteManager.Instance.specialEgg;
		if (this.m_init && this.m_orgData != null && base.category != RouletteCategory.SPECIAL && base.category != RouletteCategory.RAID)
		{
			result = true;
			count = RouletteManager.Instance.specialEgg;
		}
		return result;
	}

	public override ServerWheelOptions GetOrgRankupData()
	{
		return null;
	}

	public override ServerChaoWheelOptions GetOrgNormalData()
	{
		return null;
	}

	public override ServerWheelOptionsGeneral GetOrgGeneralData()
	{
		return this.m_orgData;
	}

	public override Dictionary<long, string[]> UpdateItemWeights()
	{
		if (this.m_itemOdds != null)
		{
			this.m_itemOdds.Clear();
		}
		this.m_itemOdds = new Dictionary<long, string[]>();
		List<long> list = new List<long>();
		Dictionary<long, float> dictionary = new Dictionary<long, float>();
		Dictionary<long, int> dictionary2 = new Dictionary<long, int>();
		Dictionary<long, float> dictionary3 = new Dictionary<long, float>();
		bool flag = false;
		ServerCampaignState serverCampaignState = null;
		if (this.m_orgData != null)
		{
			int num = 0;
			if (base.IsCampaign(Constants.Campaign.emType.PremiumRouletteOdds) && base.category == RouletteCategory.PREMIUM)
			{
				flag = true;
				serverCampaignState = ServerInterface.CampaignState;
				for (int i = 0; i < this.m_orgData.itemLenght; i++)
				{
					ServerCampaignData campaignInSession = serverCampaignState.GetCampaignInSession(Constants.Campaign.emType.PremiumRouletteOdds, i);
					if (campaignInSession != null)
					{
						num += campaignInSession.iContent;
					}
				}
			}
			for (int j = 0; j < this.m_orgData.itemLenght; j++)
			{
				int num2;
				int num3;
				float num4;
				this.m_orgData.GetCell(j, out num2, out num3, out num4);
				long num5 = (long)num2;
				float num6 = num4;
				float num7 = num4;
				int num8 = num3;
				num5 = num5 * 100000L + (long)num8;
				if (flag && flag && serverCampaignState != null)
				{
					float num9 = -1f;
					ServerCampaignData campaignInSession2 = serverCampaignState.GetCampaignInSession(Constants.Campaign.emType.PremiumRouletteOdds, j);
					if (campaignInSession2 != null)
					{
						num9 = (float)campaignInSession2.iContent;
					}
					if (num9 >= 0f)
					{
						num7 = (float)Mathf.RoundToInt(num9 / (float)num * 10000f) / 100f;
					}
				}
				if (!list.Contains(num5))
				{
					list.Add(num5);
				}
				if (!dictionary.ContainsKey(num5))
				{
					dictionary.Add(num5, num7);
				}
				else
				{
					Dictionary<long, float> dictionary4;
					Dictionary<long, float> expr_17E = dictionary4 = dictionary;
					long key;
					long expr_183 = key = num5;
					float num10 = dictionary4[key];
					expr_17E[expr_183] = num10 + num7;
				}
				if (!dictionary3.ContainsKey(num5))
				{
					dictionary3.Add(num5, num6);
				}
				else
				{
					Dictionary<long, float> dictionary5;
					Dictionary<long, float> expr_1B8 = dictionary5 = dictionary3;
					long key;
					long expr_1BD = key = num5;
					float num10 = dictionary5[key];
					expr_1B8[expr_1BD] = num10 + num6;
				}
				if (!dictionary2.ContainsKey(num5))
				{
					dictionary2.Add(num5, num8);
				}
				else
				{
					dictionary2[num5] = num8;
				}
			}
			list.Sort();
			for (int k = 0; k < list.Count; k++)
			{
				List<string> list2 = new List<string>();
				long num11 = list[k];
				float num12 = dictionary3[num11];
				float num13 = dictionary[num11];
				int num14 = dictionary2[num11];
				int id = (int)(num11 / 100000L);
				ServerItem serverItem = new ServerItem((ServerItem.Id)id);
				string str = string.Empty;
				string str2 = string.Empty;
				if (serverItem.idType == ServerItem.IdType.CHARA)
				{
					str = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Roulette", "ui_Lbl_rarity_100").text;
				}
				else if (serverItem.idType == ServerItem.IdType.CHAO)
				{
					int id2 = (int)serverItem.id;
					int num15 = id2 / 1000 % 10;
					str = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Roulette", "ui_Lbl_rarity_" + num15).text;
				}
				else
				{
					switch (serverItem.id)
					{
					case ServerItem.Id.BIG:
					case ServerItem.Id.SUPER:
					case ServerItem.Id.JACKPOT:
						str = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "reward_" + serverItem.id.ToString().ToLower()).text;
						break;
					default:
						str = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "RewardType", "reward_type_" + (int)serverItem.rewardType).text;
						str2 = " x " + num14;
						break;
					}
				}
				list2.Add(str + str2);
				string format = "F" + RouletteUtility.OddsDisplayDecimal.ToString();
				string text = string.Empty;
				if (num13 != num12)
				{
					text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "odds").text.Replace("{ODDS}", num13.ToString(format));
					float num16 = num13 - num12;
					string text2 = num16.ToString(format);
					string cellName = string.Empty;
					if (num16 > 0f)
					{
						text2 = "+" + text2;
						cellName = "campaign_odds_up";
					}
					else
					{
						cellName = "campaign_odds_down";
					}
					string str3 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", cellName).text.Replace("{ODDS}", text2);
					text += str3;
				}
				else
				{
					text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "odds").text.Replace("{ODDS}", num13.ToString(format));
				}
				list2.Add(text);
				this.m_itemOdds.Add(num11, list2.ToArray());
			}
		}
		return this.m_itemOdds;
	}

	public override string ShowSpinErrorWindow()
	{
		ServerWheelOptionsData.SPIN_BUTTON spinButtonSeting = this.GetSpinButtonSeting();
		string result = null;
		switch (spinButtonSeting)
		{
		case ServerWheelOptionsData.SPIN_BUTTON.FREE:
			result = "SpinRemainingError";
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "SpinRemainingError",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "gw_remain_caption").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "gw_remain_text").text,
				isPlayErrorSe = true
			});
			return result;
		case ServerWheelOptionsData.SPIN_BUTTON.RING:
			result = "SpinCostErrorRing";
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				caption = TextUtility.GetCommonText("ItemRoulette", "gw_cost_caption"),
				message = TextUtility.GetCommonText("ItemRoulette", "gw_cost_text"),
				buttonType = GeneralWindow.ButtonType.ShopCancel,
				name = "SpinCostErrorRing",
				isPlayErrorSe = true
			});
			return result;
		case ServerWheelOptionsData.SPIN_BUTTON.RSRING:
		{
			result = "SpinCostErrorRSRing";
			bool flag = ServerInterface.IsRSREnable();
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				caption = TextUtility.GetCommonText("ChaoRoulette", "gw_cost_caption"),
				message = ((!flag) ? TextUtility.GetCommonText("ChaoRoulette", "gw_cost_caption_text_2") : TextUtility.GetCommonText("ChaoRoulette", "gw_cost_caption_text")),
				buttonType = ((!flag) ? GeneralWindow.ButtonType.Ok : GeneralWindow.ButtonType.ShopCancel),
				name = "SpinCostErrorRSRing",
				isPlayErrorSe = true
			});
			return result;
		}
		case ServerWheelOptionsData.SPIN_BUTTON.RAID:
			result = "SpinCostErrorRaidRing";
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				caption = TextUtility.GetCommonText("Roulette", "gw_raid_cost_caption"),
				message = TextUtility.GetCommonText("Roulette", "gw_raid_cost_caption_text"),
				buttonType = GeneralWindow.ButtonType.Ok,
				name = "SpinCostErrorRaidRing",
				isPlayErrorSe = true
			});
			return result;
		}
		global::Debug.Log("ServerWheelOptionsRankup ShowSpinErrorWindow error !!!");
		return result;
	}

	public override List<ServerItem> GetAttentionItemList()
	{
		List<ServerItem> result = null;
		if (base.category == RouletteCategory.RAID && RouletteManager.Instance != null)
		{
			ServerPrizeState prizeList = RouletteManager.Instance.GetPrizeList(base.category);
			if (prizeList != null)
			{
				result = prizeList.GetAttentionList();
			}
		}
		return result;
	}

	public override bool IsPrizeDataList()
	{
		return true;
	}
}
