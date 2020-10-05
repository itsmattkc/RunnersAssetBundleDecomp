using System;
using System.Collections.Generic;

public abstract class ServerWheelOptionsOrg
{
	protected bool m_init;

	protected RouletteUtility.WheelType m_type;

	protected RouletteCategory m_category;

	protected Dictionary<long, string[]> m_itemOdds;

	public RouletteUtility.WheelType wheelType
	{
		get
		{
			return this.m_type;
		}
	}

	public RouletteCategory category
	{
		get
		{
			return this.m_category;
		}
	}

	public virtual bool isValid
	{
		get
		{
			return false;
		}
	}

	public virtual bool isRemainingRefresh
	{
		get
		{
			return false;
		}
	}

	public virtual int itemWon
	{
		get
		{
			return -1;
		}
	}

	public virtual ServerItem itemWonData
	{
		get
		{
			return default(ServerItem);
		}
	}

	public virtual int rouletteId
	{
		get
		{
			return -1;
		}
	}

	public virtual int multi
	{
		get
		{
			return 1;
		}
	}

	public virtual int numJackpotRing
	{
		get
		{
			return 0;
		}
	}

	public abstract void Setup(ServerChaoWheelOptions data);

	public abstract void Setup(ServerWheelOptions data);

	public abstract void Setup(ServerWheelOptionsGeneral data);

	public virtual bool ChangeMulti(int multi)
	{
		return false;
	}

	public virtual bool IsMulti(int multi)
	{
		return multi == 1;
	}

	public abstract int GetRouletteBoardPattern();

	public abstract string GetRouletteArrowSprite();

	public abstract string GetRouletteBgSprite();

	public abstract string GetRouletteBoardSprite();

	public abstract string GetRouletteTicketSprite();

	public abstract RouletteUtility.WheelRank GetRouletteRank();

	public abstract float GetCellWeight(int cellIndex);

	public abstract int GetCellEgg(int cellIndex);

	public abstract ServerItem GetCellItem(int cellIndex, out int num);

	public abstract void PlayBgm(float delay = 0f);

	public abstract void PlaySe(ServerWheelOptionsData.SE_TYPE seType, float delay = 0f);

	public abstract ServerWheelOptionsData.SPIN_BUTTON GetSpinButtonSeting(out int count, out bool btnActive);

	public abstract ServerWheelOptionsData.SPIN_BUTTON GetSpinButtonSeting();

	public int GetSpinCostItemId()
	{
		int result = -1;
		switch (this.GetSpinButtonSeting())
		{
		case ServerWheelOptionsData.SPIN_BUTTON.FREE:
			result = 0;
			break;
		case ServerWheelOptionsData.SPIN_BUTTON.RING:
			result = 910000;
			break;
		case ServerWheelOptionsData.SPIN_BUTTON.RSRING:
			result = 900000;
			break;
		case ServerWheelOptionsData.SPIN_BUTTON.TICKET:
		{
			ServerWheelOptionsGeneral orgGeneralData = this.GetOrgGeneralData();
			if (orgGeneralData != null)
			{
				int currentCostItemId = orgGeneralData.GetCurrentCostItemId();
				if (currentCostItemId > 0)
				{
					int costItemNum = orgGeneralData.GetCostItemNum(currentCostItemId);
					int costItemCost = orgGeneralData.GetCostItemCost(currentCostItemId);
					if (costItemNum >= costItemCost)
					{
						result = currentCostItemId;
					}
				}
			}
			else
			{
				ServerWheelOptions orgRankupData = this.GetOrgRankupData();
				ServerChaoWheelOptions orgNormalData = this.GetOrgNormalData();
				if (orgRankupData != null)
				{
					if (orgRankupData.m_numRouletteToken > 0)
					{
						result = 240000;
					}
				}
				else if (orgNormalData != null)
				{
					result = 230000;
				}
			}
			break;
		}
		case ServerWheelOptionsData.SPIN_BUTTON.RAID:
			result = 960000;
			break;
		}
		return result;
	}

	public abstract List<int> GetSpinCostItemIdList();

	public abstract int GetSpinCostItemCost(int costItemId);

	public abstract int GetSpinCostItemNum(int costItemId);

	public virtual int GetSpinCostCurrentIndex()
	{
		return 0;
	}

	public virtual bool ChangeSpinCost(int selectIndex)
	{
		return false;
	}

	public virtual bool IsChangeSpinCost()
	{
		return false;
	}

	public abstract bool GetEggSeting(out int count);

	public abstract ServerWheelOptions GetOrgRankupData();

	public abstract ServerChaoWheelOptions GetOrgNormalData();

	public abstract ServerWheelOptionsGeneral GetOrgGeneralData();

	public List<Constants.Campaign.emType> GetCampaign()
	{
		return RouletteUtility.GetCampaign(this.category);
	}

	public bool IsCampaign()
	{
		bool result = false;
		List<Constants.Campaign.emType> campaign = this.GetCampaign();
		if (campaign != null)
		{
			result = true;
		}
		return result;
	}

	public bool IsCampaign(Constants.Campaign.emType campaign)
	{
		bool result = false;
		List<Constants.Campaign.emType> campaign2 = this.GetCampaign();
		if (campaign2 != null)
		{
			result = campaign2.Contains(campaign);
		}
		return result;
	}

	public abstract Dictionary<long, string[]> UpdateItemWeights();

	public List<string[]> GetItemOdds()
	{
		if (this.m_itemOdds == null)
		{
			this.UpdateItemWeights();
		}
		List<string[]> list = new List<string[]>();
		Dictionary<long, string[]>.KeyCollection keys = this.m_itemOdds.Keys;
		foreach (long current in keys)
		{
			list.Add(this.m_itemOdds[current]);
		}
		return list;
	}

	public abstract string ShowSpinErrorWindow();

	public virtual List<ServerItem> GetAttentionItemList()
	{
		List<ServerItem> list = null;
		EventManager instance = EventManager.Instance;
		if (instance != null)
		{
			EyeCatcherChaoData[] eyeCatcherChaoDatas = instance.GetEyeCatcherChaoDatas();
			EyeCatcherCharaData[] eyeCatcherCharaDatas = instance.GetEyeCatcherCharaDatas();
			if (eyeCatcherCharaDatas != null)
			{
				EyeCatcherCharaData[] array = eyeCatcherCharaDatas;
				for (int i = 0; i < array.Length; i++)
				{
					EyeCatcherCharaData eyeCatcherCharaData = array[i];
					ServerItem item = new ServerItem((ServerItem.Id)eyeCatcherCharaData.id);
					if (list == null)
					{
						list = new List<ServerItem>();
					}
					list.Add(item);
				}
			}
			if (eyeCatcherChaoDatas != null)
			{
				EyeCatcherChaoData[] array2 = eyeCatcherChaoDatas;
				for (int j = 0; j < array2.Length; j++)
				{
					EyeCatcherChaoData eyeCatcherChaoData = array2[j];
					ServerItem item2 = new ServerItem(eyeCatcherChaoData.chao_id + ServerItem.Id.CHAO_BEGIN);
					if (list == null)
					{
						list = new List<ServerItem>();
					}
					list.Add(item2);
				}
			}
		}
		return list;
	}

	public virtual bool IsPrizeDataList()
	{
		return false;
	}
}
