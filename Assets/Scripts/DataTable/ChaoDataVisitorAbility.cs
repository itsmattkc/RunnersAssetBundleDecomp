using System;
using System.Collections.Generic;

namespace DataTable
{
	public class ChaoDataVisitorAbility : ChaoDataVisitorBase, IChaoDataSorting
	{
		private Dictionary<ChaoAbility, List<ChaoData>> m_chaoList;

		public ChaoDataVisitorAbility()
		{
			this.m_chaoList = new Dictionary<ChaoAbility, List<ChaoData>>();
			int num = 94;
			for (int i = 0; i < num; i++)
			{
				this.m_chaoList.Add((ChaoAbility)i, new List<ChaoData>());
			}
		}

		public override void visit(ChaoData chaoData)
		{
			if (this.m_chaoList != null && chaoData.chaoAbility >= ChaoAbility.ALL_BONUS_COUNT && chaoData.chaoAbility < ChaoAbility.NUM)
			{
				this.m_chaoList[chaoData.chaoAbility].Add(chaoData);
			}
		}

		public List<ChaoData> GetChaoList(ChaoAbility ability)
		{
			List<ChaoData> result = null;
			if (ability >= ChaoAbility.ALL_BONUS_COUNT && ability < ChaoAbility.NUM)
			{
				result = this.m_chaoList[ability];
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
			int num = 94;
			for (int i = 0; i <= num; i++)
			{
				ChaoAbility key = (ChaoAbility)((i + offset) % num);
				if (this.m_chaoList[key].Count > 0)
				{
					this.m_chaoList[key].Reverse();
					if (list == null)
					{
						list = this.m_chaoList[key];
					}
					else
					{
						list.AddRange(this.m_chaoList[key]);
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
			return list;
		}
	}
}
