using System;
using System.Collections.Generic;

namespace DataTable
{
	public class ChaoDataVisitorRarity : ChaoDataVisitorBase, IChaoDataSorting
	{
		private Dictionary<ChaoData.Rarity, List<ChaoData>> m_chaoRarityList;

		public ChaoDataVisitorRarity()
		{
			this.m_chaoRarityList = new Dictionary<ChaoData.Rarity, List<ChaoData>>();
			List<ChaoData> value = new List<ChaoData>();
			List<ChaoData> value2 = new List<ChaoData>();
			List<ChaoData> value3 = new List<ChaoData>();
			this.m_chaoRarityList.Add(ChaoData.Rarity.NORMAL, value);
			this.m_chaoRarityList.Add(ChaoData.Rarity.RARE, value2);
			this.m_chaoRarityList.Add(ChaoData.Rarity.SRARE, value3);
		}

		public override void visit(ChaoData chaoData)
		{
			if (this.m_chaoRarityList != null)
			{
				switch (chaoData.rarity)
				{
				case ChaoData.Rarity.NORMAL:
					this.m_chaoRarityList[ChaoData.Rarity.NORMAL].Add(chaoData);
					break;
				case ChaoData.Rarity.RARE:
					this.m_chaoRarityList[ChaoData.Rarity.RARE].Add(chaoData);
					break;
				case ChaoData.Rarity.SRARE:
					this.m_chaoRarityList[ChaoData.Rarity.SRARE].Add(chaoData);
					break;
				}
			}
		}

		public List<ChaoData> GetChaoList(ChaoData.Rarity rarity, ChaoData.Rarity raritySub = ChaoData.Rarity.NONE)
		{
			List<ChaoData> list = null;
			switch (rarity)
			{
			case ChaoData.Rarity.NORMAL:
				list = this.m_chaoRarityList[ChaoData.Rarity.NORMAL];
				break;
			case ChaoData.Rarity.RARE:
				list = this.m_chaoRarityList[ChaoData.Rarity.RARE];
				break;
			case ChaoData.Rarity.SRARE:
				list = this.m_chaoRarityList[ChaoData.Rarity.SRARE];
				break;
			}
			if (raritySub != ChaoData.Rarity.NONE && rarity != raritySub && list != null)
			{
				switch (raritySub)
				{
				case ChaoData.Rarity.NORMAL:
					list.AddRange(this.m_chaoRarityList[ChaoData.Rarity.NORMAL]);
					break;
				case ChaoData.Rarity.RARE:
					list.AddRange(this.m_chaoRarityList[ChaoData.Rarity.RARE]);
					break;
				case ChaoData.Rarity.SRARE:
					list.AddRange(this.m_chaoRarityList[ChaoData.Rarity.SRARE]);
					break;
				}
			}
			return list;
		}

		public List<ChaoData> GetChaoListAll(bool descending = false, ChaoData.Rarity exclusion = ChaoData.Rarity.NONE)
		{
			return this.GetChaoListAllOffset(0, descending, exclusion);
		}

		public List<ChaoData> GetChaoListAllOffset(int offset, bool descending = false, ChaoData.Rarity exclusion = ChaoData.Rarity.NONE)
		{
			List<ChaoData> list = null;
			List<ChaoData> chaoList = this.GetChaoList(ChaoData.Rarity.NORMAL, ChaoData.Rarity.NONE);
			List<ChaoData> chaoList2 = this.GetChaoList(ChaoData.Rarity.RARE, ChaoData.Rarity.NONE);
			List<ChaoData> chaoList3 = this.GetChaoList(ChaoData.Rarity.SRARE, ChaoData.Rarity.NONE);
			if (chaoList != null && list == null)
			{
				list = chaoList;
			}
			if (chaoList2 != null)
			{
				if (list == null)
				{
					list = chaoList2;
				}
				else
				{
					list.AddRange(chaoList2);
				}
			}
			if (chaoList3 != null)
			{
				if (list == null)
				{
					list = chaoList3;
				}
				else
				{
					list.AddRange(chaoList3);
				}
			}
			if (descending && list != null)
			{
				list.Reverse();
			}
			if (exclusion != ChaoData.Rarity.NONE && list != null)
			{
				List<ChaoData> list2 = new List<ChaoData>();
				List<ChaoData> list3 = new List<ChaoData>();
				foreach (ChaoData current in list)
				{
					if (current.rarity != exclusion)
					{
						list2.Add(current);
					}
					else
					{
						list3.Add(current);
					}
				}
				list = list2;
				list.AddRange(list3);
			}
			return list;
		}
	}
}
