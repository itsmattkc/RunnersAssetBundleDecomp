using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerWheelOptionsGeneral
{
	private const long COST_ITEM_REQ_OFFSET = 10000000L;

	private int m_currentCostSelect;

	private List<int> m_itemId;

	private List<int> m_itemNum;

	private List<int> m_itemWeight;

	private int m_rouletteId;

	private int m_rank;

	private int m_jackpotRing;

	private int m_remaining;

	private int m_spEgg;

	private int m_multi;

	private Dictionary<int, long> m_costItem;

	private DateTime m_nextFreeSpin;

	private int m_patternType = -1;

	public int currentCostSelect
	{
		get
		{
			return this.m_currentCostSelect;
		}
		private set
		{
			this.m_currentCostSelect = value;
		}
	}

	public int multi
	{
		get
		{
			return this.m_multi;
		}
		private set
		{
			this.m_multi = value;
		}
	}

	public int itemLenght
	{
		get
		{
			if (this.m_itemId == null)
			{
				return 0;
			}
			return this.m_itemId.Count;
		}
	}

	public int rouletteId
	{
		get
		{
			return this.m_rouletteId;
		}
	}

	public int remainingTicketTotal
	{
		get
		{
			return this.GetRemainingTicket();
		}
	}

	public int remainingFree
	{
		get
		{
			return this.m_remaining - this.GetRemainingTicket();
		}
	}

	public int jackpotRing
	{
		get
		{
			return this.m_jackpotRing;
		}
	}

	public int spEgg
	{
		get
		{
			return this.m_spEgg;
		}
		set
		{
			this.m_spEgg = value;
		}
	}

	public DateTime nextFreeSpin
	{
		get
		{
			return this.m_nextFreeSpin;
		}
	}

	public RouletteUtility.WheelType type
	{
		get
		{
			return (!this.isRankup) ? RouletteUtility.WheelType.Normal : RouletteUtility.WheelType.Rankup;
		}
	}

	public RouletteUtility.WheelRank rank
	{
		get
		{
			return RouletteUtility.GetRouletteRank(this.m_rank);
		}
	}

	public int patternType
	{
		get
		{
			if (this.m_patternType < 0)
			{
				return this.GetRoulettePatternType();
			}
			return this.m_patternType;
		}
	}

	public string spriteNameBg
	{
		get
		{
			return RouletteUtility.GetRouletteBgSpriteName(this);
		}
	}

	public string spriteNameBoard
	{
		get
		{
			return RouletteUtility.GetRouletteBoardSpriteName(this);
		}
	}

	public string spriteNameArrow
	{
		get
		{
			return RouletteUtility.GetRouletteArrowSpriteName(this);
		}
	}

	public string spriteNameCostItem
	{
		get
		{
			return RouletteUtility.GetRouletteCostItemName(this.GetCurrentCostItemId());
		}
	}

	public bool isRankup
	{
		get
		{
			bool result = false;
			if (this.m_rank > 0)
			{
				result = true;
			}
			else
			{
				foreach (int current in this.m_itemId)
				{
					ServerItem serverItem = new ServerItem((ServerItem.Id)current);
					if (serverItem.idType == ServerItem.IdType.ITEM_ROULLETE_WIN)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}

	public ServerWheelOptionsGeneral()
	{
		this.m_itemId = new List<int>();
		this.m_itemNum = new List<int>();
		this.m_itemWeight = new List<int>();
		this.m_costItem = new Dictionary<int, long>();
		for (int i = 0; i < 8; i++)
		{
			if (i == 0)
			{
				this.m_itemId.Add(200000);
			}
			else
			{
				this.m_itemId.Add(120000 + i - 1);
			}
			this.m_itemNum.Add(1);
			this.m_itemWeight.Add(1);
		}
		this.m_remaining = 0;
		this.m_jackpotRing = 0;
		this.m_rank = 0;
		this.m_multi = 1;
		this.m_nextFreeSpin = DateTime.Now;
		this.m_nextFreeSpin = this.m_nextFreeSpin.AddDays(999.0);
	}

	private int GetRoulettePatternType()
	{
		int num = 0;
		int num2 = 9999999;
		foreach (int current in this.m_itemWeight)
		{
			if (num < current)
			{
				num = current;
			}
			if (num2 > current)
			{
				num2 = current;
			}
		}
		int num3;
		if ((float)num2 / (float)num < 0.35f)
		{
			num3 = 0;
		}
		else
		{
			num3 = 1;
		}
		this.m_patternType = num3;
		return num3;
	}

	public ServerWheelOptionsData.SPIN_BUTTON GetSpinButton()
	{
		ServerWheelOptionsData.SPIN_BUTTON sPIN_BUTTON = ServerWheelOptionsData.SPIN_BUTTON.NONE;
		if (this.remainingFree > 0)
		{
			sPIN_BUTTON = ServerWheelOptionsData.SPIN_BUTTON.FREE;
		}
		else if (sPIN_BUTTON == ServerWheelOptionsData.SPIN_BUTTON.NONE)
		{
			int currentCostItemId = this.GetCurrentCostItemId();
			if (currentCostItemId != 900000)
			{
				if (currentCostItemId != 910000)
				{
					if (currentCostItemId != 960000)
					{
						sPIN_BUTTON = ServerWheelOptionsData.SPIN_BUTTON.TICKET;
					}
					else
					{
						sPIN_BUTTON = ServerWheelOptionsData.SPIN_BUTTON.RAID;
					}
				}
				else
				{
					sPIN_BUTTON = ServerWheelOptionsData.SPIN_BUTTON.RING;
				}
			}
			else
			{
				sPIN_BUTTON = ServerWheelOptionsData.SPIN_BUTTON.RSRING;
			}
		}
		return sPIN_BUTTON;
	}

	public RouletteUtility.CellType GetCell(int index)
	{
		RouletteUtility.CellType result = RouletteUtility.CellType.Item;
		if (index >= 0 && index < this.itemLenght)
		{
			int num = this.m_itemId[index];
			if (num >= 0)
			{
				ServerItem serverItem = new ServerItem((ServerItem.Id)num);
				ServerItem.IdType idType = serverItem.idType;
				if (idType != ServerItem.IdType.ITEM_ROULLETE_WIN)
				{
					if (idType != ServerItem.IdType.CHARA && idType != ServerItem.IdType.CHAO)
					{
						result = RouletteUtility.CellType.Item;
					}
					else
					{
						result = RouletteUtility.CellType.Egg;
					}
				}
				else
				{
					result = RouletteUtility.CellType.Rankup;
				}
			}
		}
		return result;
	}

	public float GetCellWeight(int index)
	{
		float result = 0f;
		if (index >= 0 && index < this.itemLenght)
		{
			result = (float)this.m_itemWeight[index];
		}
		return result;
	}

	public RouletteUtility.CellType GetCell(int index, out int itemId, out int itemNum, out float itemRate)
	{
		RouletteUtility.CellType result = RouletteUtility.CellType.Item;
		itemId = 0;
		itemNum = 0;
		itemRate = 0f;
		if (index >= 0 && index < this.itemLenght)
		{
			int num = this.m_itemId[index];
			int num2 = this.m_itemNum[index];
			if (num >= 0)
			{
				itemId = num;
				itemNum = num2;
				itemRate = this.GetItemRate(index);
				ServerItem serverItem = new ServerItem((ServerItem.Id)num);
				ServerItem.IdType idType = serverItem.idType;
				if (idType != ServerItem.IdType.ITEM_ROULLETE_WIN)
				{
					if (idType != ServerItem.IdType.CHARA && idType != ServerItem.IdType.CHAO)
					{
						result = RouletteUtility.CellType.Item;
					}
					else
					{
						result = RouletteUtility.CellType.Egg;
						itemNum = 0;
					}
				}
				else
				{
					result = RouletteUtility.CellType.Rankup;
					itemNum = 0;
				}
			}
		}
		return result;
	}

	private float GetItemRate(int index)
	{
		float result = 0f;
		if (index >= 0 && index < this.itemLenght)
		{
			int itemMaxWeightIndex = this.GetItemMaxWeightIndex();
			int itemTotalWeight = this.GetItemTotalWeight();
			int num = this.m_itemWeight[index];
			if (itemTotalWeight > 0 && num > 0)
			{
				if (itemMaxWeightIndex < 0 || index != itemMaxWeightIndex)
				{
					result = Mathf.Round((float)num / (float)itemTotalWeight * 10000f) / 100f;
				}
				else
				{
					float num2 = 0f;
					for (int i = 0; i < this.m_itemWeight.Count; i++)
					{
						if (i != itemMaxWeightIndex)
						{
							num2 += Mathf.Round((float)num / (float)itemTotalWeight * 10000f) / 100f;
						}
					}
					result = 100f - num2;
				}
			}
		}
		return result;
	}

	private int GetItemTotalWeight()
	{
		int num = 0;
		for (int i = 0; i < this.m_itemWeight.Count; i++)
		{
			num += this.m_itemWeight[i];
		}
		return num;
	}

	private int GetItemMaxWeightIndex()
	{
		int result = -1;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.m_itemWeight.Count; i++)
		{
			int num3 = num;
			if (this.m_itemWeight[i] > num)
			{
				num = this.m_itemWeight[i];
				result = i;
			}
			if (num3 != num)
			{
				num2++;
			}
		}
		if (num2 <= 1)
		{
			result = -1;
		}
		return result;
	}

	public void SetupItem(int index, int itemId, int weight, int num = 0)
	{
		if (this.m_itemId != null && this.m_itemNum != null && this.m_itemWeight != null && index >= 0)
		{
			if (index < this.m_itemId.Count)
			{
				this.m_itemId[index] = itemId;
				this.m_itemNum[index] = num;
				this.m_itemWeight[index] = weight;
			}
			else
			{
				int num2 = index - this.m_itemId.Count + 1;
				for (int i = 0; i < num2; i++)
				{
					this.m_itemId.Add(itemId);
					this.m_itemNum.Add(num);
					this.m_itemWeight.Add(weight);
				}
			}
		}
	}

	public void ResetupCostItem()
	{
		if (this.m_costItem != null)
		{
			this.m_costItem.Clear();
		}
		this.m_costItem = new Dictionary<int, long>();
	}

	public void AddCostItem(int itemId, int itemNum, int oneCost = 1)
	{
		if (this.m_costItem == null)
		{
			this.m_costItem = new Dictionary<int, long>();
		}
		global::Debug.Log(string.Concat(new object[]
		{
			"ServerWheelOptionsGeneral AddCostItem  itemId:",
			itemId,
			"  itemNum:",
			itemNum,
			"  oneCost:",
			oneCost
		}));
		if (this.m_costItem.ContainsKey(itemId))
		{
			Dictionary<int, long> costItem;
			Dictionary<int, long> expr_70 = costItem = this.m_costItem;
			long num = costItem[itemId];
			expr_70[itemId] = num + (long)itemNum;
		}
		else
		{
			this.m_costItem.Add(itemId, (long)itemNum + 10000000L * (long)oneCost);
		}
		GeneralUtil.SetItemCount((ServerItem.Id)itemId, this.m_costItem[itemId] % 10000000L);
	}

	public void CopyToCostItem(Dictionary<int, long> items)
	{
		if (this.m_costItem == null)
		{
			this.m_costItem = new Dictionary<int, long>();
		}
		else
		{
			this.m_costItem.Clear();
			this.m_costItem = new Dictionary<int, long>();
		}
		if (items != null && items.Count > 0)
		{
			Dictionary<int, long>.KeyCollection keys = items.Keys;
			foreach (int current in keys)
			{
				this.m_costItem.Add(current, items[current]);
			}
		}
	}

	public List<int> GetCostItemList()
	{
		List<int> list = null;
		if (this.m_costItem != null && this.m_costItem.Count > 0)
		{
			list = new List<int>();
			Dictionary<int, long>.KeyCollection keys = this.m_costItem.Keys;
			foreach (int current in keys)
			{
				list.Add(current);
			}
		}
		return list;
	}

	public int GetCostItemNum(int costItemId)
	{
		int result = -1;
		if (this.m_costItem != null && this.m_costItem.Count > 0 && this.m_costItem.ContainsKey(costItemId))
		{
			result = (int)GeneralUtil.GetItemCount((ServerItem.Id)costItemId);
		}
		return result;
	}

	public int GetCostItemCost(int costItemId)
	{
		int result = -1;
		if (this.m_costItem != null && this.m_costItem.Count > 0 && this.m_costItem.ContainsKey(costItemId))
		{
			result = (int)(this.m_costItem[costItemId] / 10000000L);
		}
		return result;
	}

	public int GetDefultCostItemId()
	{
		int result = -1;
		List<int> costItemList = this.GetCostItemList();
		if (costItemList != null && costItemList.Count > 0)
		{
			result = costItemList[0];
		}
		return result;
	}

	public int GetCurrentCostItemId()
	{
		int num = -1;
		List<int> costItemList = this.GetCostItemList();
		if (costItemList != null && costItemList.Count > 0)
		{
			if (this.m_currentCostSelect <= 0)
			{
				for (int i = 0; i < costItemList.Count; i++)
				{
					int costItemNum = this.GetCostItemNum(costItemList[i]);
					int costItemCost = this.GetCostItemCost(costItemList[i]);
					if (costItemNum >= costItemCost)
					{
						num = costItemList[i];
						break;
					}
				}
			}
			else if (this.m_currentCostSelect <= costItemList.Count)
			{
				for (int j = 0; j < costItemList.Count; j++)
				{
					int num2 = (this.m_currentCostSelect + j - 1) % costItemList.Count;
					if (num2 < costItemList.Count)
					{
						int costItemNum = this.GetCostItemNum(costItemList[num2]);
						int costItemCost = this.GetCostItemCost(costItemList[num2]);
						if (costItemNum >= costItemCost)
						{
							num = costItemList[num2];
							this.m_currentCostSelect = num2 + 1;
							break;
						}
					}
				}
			}
			if (num == -1)
			{
				num = costItemList[0];
			}
		}
		return num;
	}

	public int GetCurrentCostItemNum()
	{
		int result = 0;
		int currentCostItemId = this.GetCurrentCostItemId();
		if (currentCostItemId > 0)
		{
			int costItemNum = this.GetCostItemNum(currentCostItemId);
			int costItemCost = this.GetCostItemCost(currentCostItemId);
			if (costItemNum >= costItemCost)
			{
				result = costItemNum / costItemCost;
			}
		}
		return result;
	}

	public bool ChangeCostItem(int selectIndex)
	{
		bool result = false;
		if (this.currentCostSelect == selectIndex)
		{
			return false;
		}
		if (selectIndex <= 0)
		{
			this.currentCostSelect = 0;
			return true;
		}
		List<int> costItemList = this.GetCostItemList();
		if (costItemList != null && costItemList.Count > 1)
		{
			if (costItemList.Count > selectIndex - 1)
			{
				int costItemId = costItemList[selectIndex - 1];
				int costItemCost = this.GetCostItemCost(costItemId);
				int costItemNum = this.GetCostItemNum(costItemId);
				if (costItemNum >= costItemCost)
				{
					this.currentCostSelect = selectIndex;
				}
				else
				{
					this.currentCostSelect = 0;
				}
			}
			else
			{
				this.currentCostSelect = 99;
			}
			result = true;
		}
		return result;
	}

	public bool ChangeMulti(int multi)
	{
		bool flag = this.IsMulti(multi);
		if (flag)
		{
			this.m_multi = multi;
			if (this.m_multi < 1)
			{
				this.m_multi = 1;
			}
		}
		else
		{
			this.m_multi = 1;
		}
		return flag;
	}

	public bool IsMulti(int multi)
	{
		bool result = false;
		if (multi <= 1)
		{
			result = true;
		}
		else
		{
			int currentCostItemId = this.GetCurrentCostItemId();
			ServerWheelOptionsData.SPIN_BUTTON spinButton = this.GetSpinButton();
			int costItemCost = this.GetCostItemCost(currentCostItemId);
			int num = 0;
			bool flag = true;
			switch (spinButton)
			{
			case ServerWheelOptionsData.SPIN_BUTTON.RING:
				num = (int)SaveDataManager.Instance.ItemData.RingCount;
				break;
			case ServerWheelOptionsData.SPIN_BUTTON.RSRING:
				num = (int)SaveDataManager.Instance.ItemData.RedRingCount;
				break;
			case ServerWheelOptionsData.SPIN_BUTTON.TICKET:
			case ServerWheelOptionsData.SPIN_BUTTON.RAID:
				num = this.GetCostItemNum(currentCostItemId);
				break;
			default:
				flag = false;
				break;
			}
			if (flag && num >= costItemCost * multi)
			{
				result = true;
			}
		}
		return result;
	}

	public void SetupParam(int rouletteId, int remaining, int jackpotRing, int rank, int spEggNum, DateTime nextFree)
	{
		this.m_rouletteId = rouletteId;
		this.m_remaining = remaining;
		this.m_jackpotRing = jackpotRing;
		this.m_rank = rank;
		this.m_spEgg = spEggNum;
		this.m_nextFreeSpin = nextFree;
	}

	public void SetupParam(int rouletteId, int remaining, int jackpotRing, int rank, int spEggNum)
	{
		this.m_rouletteId = rouletteId;
		this.m_remaining = remaining;
		this.m_jackpotRing = jackpotRing;
		this.m_rank = rank;
		this.m_spEgg = spEggNum;
	}

	public void CopyTo(ServerWheelOptionsGeneral to)
	{
		for (int i = 0; i < this.itemLenght; i++)
		{
			to.SetupItem(i, this.m_itemId[i], this.m_itemWeight[i], this.m_itemNum[i]);
		}
		to.SetupParam(this.m_rouletteId, this.m_remaining, this.m_jackpotRing, this.m_rank, this.m_spEgg, this.m_nextFreeSpin);
		to.CopyToCostItem(this.m_costItem);
	}

	private int GetRemainingTicket()
	{
		int num = 0;
		if (this.m_costItem != null && this.m_costItem.Count > 0)
		{
			Dictionary<int, long>.KeyCollection keys = this.m_costItem.Keys;
			foreach (int current in keys)
			{
				if (current >= 240000 && current < 250000)
				{
					int num2 = (int)(this.m_costItem[current] / 10000000L);
					int num3 = (int)(this.m_costItem[current] % 10000000L);
					if (num3 >= num2)
					{
						num += num3 / num2;
					}
				}
			}
		}
		return num;
	}
}
