using System;
using System.Collections.Generic;

public class ServerWheelOptionsData
{
	public enum DATA_TYPE
	{
		NONE,
		NORMAL,
		RANKUP,
		GENERAL
	}

	public enum SPIN_BUTTON
	{
		FREE,
		RING,
		RSRING,
		TICKET,
		RAID,
		NONE
	}

	public enum SE_TYPE
	{
		NONE,
		Open,
		Close,
		Click,
		Spin,
		SpinError,
		Arrow,
		Skip,
		GetItem,
		GetRare,
		GetRankup,
		GetJackpot,
		Multi,
		Change
	}

	private ServerWheelOptionsData.DATA_TYPE m_dataType;

	private ServerWheelOptionsOrg m_wheelOption;

	public RouletteUtility.WheelType wheelType
	{
		get
		{
			return this.m_wheelOption.wheelType;
		}
	}

	public ServerWheelOptionsData.DATA_TYPE dataType
	{
		get
		{
			if (this.m_dataType == ServerWheelOptionsData.DATA_TYPE.NONE && this.m_wheelOption != null)
			{
				if (this.m_wheelOption.GetOrgGeneralData() != null)
				{
					this.m_dataType = ServerWheelOptionsData.DATA_TYPE.GENERAL;
				}
				else if (this.m_wheelOption.GetOrgNormalData() != null)
				{
					this.m_dataType = ServerWheelOptionsData.DATA_TYPE.NORMAL;
				}
				else if (this.m_wheelOption.GetOrgRankupData() != null)
				{
					this.m_dataType = ServerWheelOptionsData.DATA_TYPE.RANKUP;
				}
			}
			return this.m_dataType;
		}
	}

	public RouletteCategory category
	{
		get
		{
			return this.m_wheelOption.category;
		}
	}

	public bool isValid
	{
		get
		{
			return this.m_wheelOption.isValid;
		}
	}

	public bool isRemainingRefresh
	{
		get
		{
			return this.m_wheelOption.isRemainingRefresh;
		}
	}

	public int itemWon
	{
		get
		{
			return this.m_wheelOption.itemWon;
		}
	}

	public ServerItem itemWonData
	{
		get
		{
			return this.m_wheelOption.itemWonData;
		}
	}

	public int rouletteId
	{
		get
		{
			return this.m_wheelOption.rouletteId;
		}
	}

	public int multi
	{
		get
		{
			return this.m_wheelOption.multi;
		}
	}

	public bool isGeneral
	{
		get
		{
			return this.m_wheelOption.GetOrgGeneralData() != null;
		}
	}

	public int numJackpotRing
	{
		get
		{
			return this.m_wheelOption.numJackpotRing;
		}
	}

	public ServerWheelOptionsData(ServerWheelOptionsData data)
	{
		if (data.GetOrgGeneralData() != null)
		{
			this.m_wheelOption = new ServerWheelOptionsOrgGen(data.GetOrgGeneralData());
		}
		else if (data.GetOrgNormalData() != null)
		{
			this.m_wheelOption = new ServerWheelOptionsNormal(data.GetOrgNormalData());
		}
		else if (data.GetOrgRankupData() != null)
		{
			this.m_wheelOption = new ServerWheelOptionsRankup(data.GetOrgRankupData());
		}
	}

	public ServerWheelOptionsData(ServerChaoWheelOptions data)
	{
		this.m_wheelOption = new ServerWheelOptionsNormal(data);
	}

	public ServerWheelOptionsData(ServerWheelOptions data)
	{
		this.m_wheelOption = new ServerWheelOptionsRankup(data);
	}

	public ServerWheelOptionsData(ServerWheelOptionsGeneral data)
	{
		this.m_wheelOption = new ServerWheelOptionsOrgGen(data);
	}

	public void Setup(ServerChaoWheelOptions data)
	{
		this.m_wheelOption.Setup(data);
	}

	public void Setup(ServerWheelOptions data)
	{
		this.m_wheelOption.Setup(data);
	}

	public void Setup(ServerWheelOptionsGeneral data)
	{
		this.m_wheelOption.Setup(data);
	}

	public bool ChangeMulti(int multi)
	{
		return this.m_wheelOption.ChangeMulti(multi);
	}

	public bool IsMulti(int multi)
	{
		return this.m_wheelOption.IsMulti(multi);
	}

	public int GetRouletteBoardPattern()
	{
		if (this.m_wheelOption == null)
		{
			return 0;
		}
		return this.m_wheelOption.GetRouletteBoardPattern();
	}

	public string GetRouletteArrowSprite()
	{
		if (this.m_wheelOption == null)
		{
			return null;
		}
		return this.m_wheelOption.GetRouletteArrowSprite();
	}

	public string GetRouletteBgSprite()
	{
		if (this.m_wheelOption == null)
		{
			return null;
		}
		return this.m_wheelOption.GetRouletteBgSprite();
	}

	public string GetRouletteBoardSprite()
	{
		if (this.m_wheelOption == null)
		{
			return null;
		}
		return this.m_wheelOption.GetRouletteBoardSprite();
	}

	public string GetRouletteTicketSprite()
	{
		if (this.m_wheelOption == null)
		{
			return null;
		}
		return this.m_wheelOption.GetRouletteTicketSprite();
	}

	public RouletteUtility.WheelRank GetRouletteRank()
	{
		if (this.m_wheelOption == null)
		{
			return RouletteUtility.WheelRank.Normal;
		}
		return this.m_wheelOption.GetRouletteRank();
	}

	public int GetCellEgg(int cellIndex)
	{
		if (this.m_wheelOption == null)
		{
			return 0;
		}
		return this.m_wheelOption.GetCellEgg(cellIndex);
	}

	public ServerItem GetCellItem(int cellIndex, out int num)
	{
		if (this.m_wheelOption == null)
		{
			num = 0;
			return default(ServerItem);
		}
		return this.m_wheelOption.GetCellItem(cellIndex, out num);
	}

	public ServerItem GetCellItem(int cellIndex)
	{
		if (this.m_wheelOption == null)
		{
			return default(ServerItem);
		}
		int num = 0;
		return this.m_wheelOption.GetCellItem(cellIndex, out num);
	}

	public float GetCellWeight(int cellIndex)
	{
		if (this.m_wheelOption == null)
		{
			return 0f;
		}
		return this.m_wheelOption.GetCellWeight(cellIndex);
	}

	public void PlayBgm(float delay = 0f)
	{
		if (this.m_wheelOption == null)
		{
			return;
		}
		this.m_wheelOption.PlayBgm(delay);
	}

	public void PlaySe(ServerWheelOptionsData.SE_TYPE seType, float delay = 0f)
	{
		if (this.m_wheelOption == null)
		{
			return;
		}
		this.m_wheelOption.PlaySe(seType, delay);
	}

	public ServerWheelOptionsData.SPIN_BUTTON GetSpinButtonSeting(out int count, out bool btnActive)
	{
		if (this.m_wheelOption == null)
		{
			count = 0;
			btnActive = false;
			return ServerWheelOptionsData.SPIN_BUTTON.NONE;
		}
		return this.m_wheelOption.GetSpinButtonSeting(out count, out btnActive);
	}

	public ServerWheelOptionsData.SPIN_BUTTON GetSpinButtonSeting()
	{
		if (this.m_wheelOption == null)
		{
			return ServerWheelOptionsData.SPIN_BUTTON.NONE;
		}
		return this.m_wheelOption.GetSpinButtonSeting();
	}

	public int GetSpinCostItemId()
	{
		if (this.m_wheelOption == null)
		{
			return 0;
		}
		return this.m_wheelOption.GetSpinCostItemId();
	}

	public List<int> GetSpinCostItemIdList()
	{
		if (this.m_wheelOption == null)
		{
			return null;
		}
		return this.m_wheelOption.GetSpinCostItemIdList();
	}

	public int GetSpinCostItemNum(int costItemId)
	{
		if (this.m_wheelOption == null)
		{
			return 0;
		}
		return this.m_wheelOption.GetSpinCostItemNum(costItemId);
	}

	public int GetSpinCostItemCost(int costItemId)
	{
		if (this.m_wheelOption == null)
		{
			return 0;
		}
		return this.m_wheelOption.GetSpinCostItemCost(costItemId);
	}

	public bool ChangeSpinCost(int selectIndex)
	{
		return this.m_wheelOption != null && this.m_wheelOption.ChangeSpinCost(selectIndex);
	}

	public int GetCurrentSpinCostIndex()
	{
		if (this.m_wheelOption == null)
		{
			return 0;
		}
		return this.m_wheelOption.GetSpinCostCurrentIndex();
	}

	public bool GetEggSeting(out int count)
	{
		if (this.m_wheelOption == null)
		{
			count = 0;
			return false;
		}
		return this.m_wheelOption.GetEggSeting(out count);
	}

	public ServerWheelOptions GetOrgRankupData()
	{
		if (this.m_wheelOption == null)
		{
			return null;
		}
		return this.m_wheelOption.GetOrgRankupData();
	}

	public ServerChaoWheelOptions GetOrgNormalData()
	{
		if (this.m_wheelOption == null)
		{
			return null;
		}
		return this.m_wheelOption.GetOrgNormalData();
	}

	public ServerWheelOptionsGeneral GetOrgGeneralData()
	{
		if (this.m_wheelOption == null)
		{
			return null;
		}
		return this.m_wheelOption.GetOrgGeneralData();
	}

	public List<Constants.Campaign.emType> GetCampaign()
	{
		if (this.m_wheelOption == null)
		{
			return null;
		}
		return this.m_wheelOption.GetCampaign();
	}

	public bool IsCampaign()
	{
		return this.m_wheelOption != null && this.m_wheelOption.IsCampaign();
	}

	public bool IsCampaign(Constants.Campaign.emType campaign)
	{
		return this.m_wheelOption != null && this.m_wheelOption.IsCampaign(campaign);
	}

	public List<string[]> GetItemOdds()
	{
		if (this.m_wheelOption == null)
		{
			return null;
		}
		return this.m_wheelOption.GetItemOdds();
	}

	public string ShowSpinErrorWindow()
	{
		if (this.m_wheelOption == null)
		{
			return null;
		}
		return this.m_wheelOption.ShowSpinErrorWindow();
	}

	public List<ServerItem> GetAttentionItemList()
	{
		if (this.m_wheelOption == null)
		{
			return null;
		}
		return this.m_wheelOption.GetAttentionItemList();
	}

	public bool IsPrizeDataList()
	{
		return this.m_wheelOption != null && this.m_wheelOption.IsPrizeDataList();
	}

	public void CopyTo(ServerWheelOptionsData to)
	{
		to.Setup(this.m_wheelOption.GetOrgGeneralData());
		to.Setup(this.m_wheelOption.GetOrgNormalData());
		to.Setup(this.m_wheelOption.GetOrgRankupData());
	}
}
