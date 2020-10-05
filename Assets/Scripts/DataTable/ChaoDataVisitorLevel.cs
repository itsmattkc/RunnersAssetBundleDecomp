using System;
using System.Collections.Generic;

namespace DataTable
{
	public class ChaoDataVisitorLevel : ChaoDataVisitorBase, IChaoDataSorting
	{
		private const int LV_MAX = 10;

		private Dictionary<int, List<ChaoData>> m_chaoList;

		public ChaoDataVisitorLevel()
		{
			this.m_chaoList = new Dictionary<int, List<ChaoData>>();
			for (int i = 0; i <= 11; i++)
			{
				this.m_chaoList.Add(i, new List<ChaoData>());
			}
		}

		public override void visit(ChaoData chaoData)
		{
			if (this.m_chaoList != null)
			{
				if (chaoData.level >= 0 && chaoData.level <= 10)
				{
					this.m_chaoList[chaoData.level].Add(chaoData);
				}
				else
				{
					this.m_chaoList[this.m_chaoList.Count - 1].Add(chaoData);
				}
			}
		}

		public List<ChaoData> GetChaoList(int level)
		{
			List<ChaoData> result = null;
			if (level >= 0 && level <= 10)
			{
				result = this.m_chaoList[level];
			}
			else if (level == -1)
			{
				result = this.m_chaoList[this.m_chaoList.Count - 1];
			}
			return result;
		}

		public List<ChaoData> GetChaoListAll(bool descending = false, ChaoData.Rarity exclusion = ChaoData.Rarity.NONE)
		{
			return this.GetChaoListAllOffset(0, descending, exclusion);
		}

		public List<ChaoData> GetChaoListAllOffset(int offset, bool descending = false, ChaoData.Rarity exclusion = ChaoData.Rarity.NONE)
		{
			List<ChaoData> list = null;
			for (int i = 0; i <= 10; i++)
			{
				if (this.m_chaoList[i].Count > 0)
				{
					if (list == null)
					{
						list = this.m_chaoList[i];
					}
					else
					{
						list.AddRange(this.m_chaoList[i]);
					}
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
			if (list == null)
			{
				list = this.m_chaoList[this.m_chaoList.Count - 1];
			}
			else
			{
				list.AddRange(this.m_chaoList[this.m_chaoList.Count - 1]);
			}
			return list;
		}
	}
}
