using System;
using System.Collections.Generic;

namespace DataTable
{
	public class ChaoDataSorting
	{
		private ChaoDataVisitorBase m_visitor;

		private IChaoDataSorting m_chaoSorting;

		public ChaoDataVisitorBase visitor
		{
			get
			{
				return this.m_visitor;
			}
		}

		public ChaoDataSorting(ChaoSort sort)
		{
			switch (sort)
			{
			case ChaoSort.RARE:
				this.m_visitor = new ChaoDataVisitorRarity();
				break;
			case ChaoSort.LEVEL:
				this.m_visitor = new ChaoDataVisitorLevel();
				break;
			case ChaoSort.ATTRIBUTE:
				this.m_visitor = new ChaoDataVisitorAttribute();
				break;
			case ChaoSort.ABILITY:
				this.m_visitor = new ChaoDataVisitorAbility();
				break;
			case ChaoSort.EVENT:
				this.m_visitor = new ChaoDataVisitorEvent();
				break;
			}
			if (this.m_visitor != null)
			{
				this.m_chaoSorting = (IChaoDataSorting)this.m_visitor;
			}
		}

		public List<ChaoData> GetChaoList(bool descending = false, ChaoData.Rarity exclusion = ChaoData.Rarity.NONE)
		{
			List<ChaoData> list = null;
			if (this.m_chaoSorting != null)
			{
				List<ChaoData> chaoListAll = this.m_chaoSorting.GetChaoListAll(descending, exclusion);
				if (chaoListAll != null)
				{
					list = new List<ChaoData>();
					foreach (ChaoData current in chaoListAll)
					{
						if (current.level >= 0)
						{
							list.Add(current);
						}
					}
				}
			}
			return list;
		}

		public List<ChaoData> GetChaoListAll(bool descending = false, ChaoData.Rarity exclusion = ChaoData.Rarity.NONE)
		{
			List<ChaoData> result = null;
			if (this.m_chaoSorting != null)
			{
				result = this.m_chaoSorting.GetChaoListAll(descending, exclusion);
			}
			return result;
		}

		public List<ChaoData> GetChaoListAllOffset(int offset, bool descending = false, ChaoData.Rarity exclusion = ChaoData.Rarity.NONE)
		{
			List<ChaoData> result = null;
			if (this.m_chaoSorting != null)
			{
				result = this.m_chaoSorting.GetChaoListAllOffset(offset, descending, exclusion);
			}
			return result;
		}
	}
}
