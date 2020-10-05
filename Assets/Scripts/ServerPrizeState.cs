using System;
using System.Collections.Generic;
using Text;

public class ServerPrizeState
{
	private List<ServerPrizeData> m_prizeList;

	private string m_prizeText;

	public List<ServerPrizeData> prizeList
	{
		get
		{
			return this.m_prizeList;
		}
	}

	public ServerPrizeState()
	{
		this.m_prizeList = new List<ServerPrizeData>();
		this.m_prizeText = null;
	}

	public ServerPrizeState(ServerWheelOptionsData data)
	{
		this.m_prizeList = null;
		this.m_prizeText = null;
		if (data.dataType == ServerWheelOptionsData.DATA_TYPE.RANKUP)
		{
			ServerWheelOptions orgRankupData = data.GetOrgRankupData();
			if (orgRankupData != null)
			{
				int num = orgRankupData.m_items.Length;
				for (int i = 0; i < num; i++)
				{
					this.AddPrize(new ServerPrizeData
					{
						itemId = orgRankupData.m_items[i],
						num = orgRankupData.m_itemQuantities[i],
						weight = orgRankupData.m_itemWeight[i]
					});
				}
			}
		}
	}

	public bool AddPrize(ServerPrizeData data)
	{
		if (this.m_prizeList == null)
		{
			this.m_prizeList = new List<ServerPrizeData>();
		}
		this.m_prizeList.Add(data);
		return true;
	}

	public void ResetPrizeList()
	{
		if (this.m_prizeList != null)
		{
			this.m_prizeList.Clear();
		}
		this.m_prizeList = new List<ServerPrizeData>();
	}

	public bool IsExpired()
	{
		return false;
	}

	public bool IsData()
	{
		bool result = false;
		if (this.m_prizeList != null && this.m_prizeList.Count > 0)
		{
			result = true;
		}
		return result;
	}

	public List<string[]> GetItemOdds(ServerWheelOptionsData data)
	{
		return data.GetItemOdds();
	}

	public string GetPrizeText(ServerWheelOptionsData data)
	{
		string result = null;
		RouletteCategory category = data.category;
		if (category != RouletteCategory.NONE && category != RouletteCategory.ALL && category != RouletteCategory.GENERAL)
		{
			this.m_prizeText = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Roulette", "note_" + RouletteUtility.GetRouletteCategoryName(category)).text;
			string prizeList = RouletteUtility.GetPrizeList(this);
			result = this.m_prizeText.Replace("{PARAM}", prizeList);
		}
		return result;
	}

	public List<ServerItem> GetAttentionList()
	{
		List<ServerItem> list = null;
		if (this.m_prizeList != null && this.m_prizeList.Count > 0)
		{
			List<ServerItem> list2 = new List<ServerItem>();
			List<ServerItem> list3 = new List<ServerItem>();
			foreach (ServerPrizeData current in this.m_prizeList)
			{
				ServerItem item = new ServerItem((ServerItem.Id)current.itemId);
				if (item.idType == ServerItem.IdType.CHARA)
				{
					bool flag = true;
					if (list2.Count > 0)
					{
						foreach (ServerItem current2 in list2)
						{
							if (current2.id == item.id)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						list2.Add(item);
					}
				}
				else if (item.idType == ServerItem.IdType.CHAO && current.itemId >= 402000)
				{
					bool flag2 = true;
					if (list3.Count > 0)
					{
						foreach (ServerItem current3 in list3)
						{
							if (current3.id == item.id)
							{
								flag2 = false;
								break;
							}
						}
					}
					if (flag2)
					{
						list3.Add(item);
					}
				}
			}
			if (list2.Count > 0 || list3.Count > 0)
			{
				list = new List<ServerItem>();
				int num = 4;
				int num2 = list2.Count;
				int num3 = list3.Count;
				if (num2 > 2)
				{
					num2 = 2;
				}
				if (num3 > num - num2)
				{
					num3 = num - num2;
				}
				GeneralUtil.RandomList<ServerItem>(ref list2);
				GeneralUtil.RandomList<ServerItem>(ref list3);
				if (num2 > 0)
				{
					for (int i = 0; i < num2; i++)
					{
						list.Add(list2[i]);
					}
				}
				if (num3 > 0)
				{
					for (int j = 0; j < num3; j++)
					{
						list.Add(list3[j]);
					}
				}
			}
		}
		return list;
	}

	public void CopyTo(ServerPrizeState to)
	{
		if (to == null || this.prizeList == null)
		{
			return;
		}
		if (this.prizeList.Count <= 0)
		{
			return;
		}
		to.ResetPrizeList();
		foreach (ServerPrizeData current in this.prizeList)
		{
			if (current != null)
			{
				to.AddPrize(current);
			}
		}
	}
}
