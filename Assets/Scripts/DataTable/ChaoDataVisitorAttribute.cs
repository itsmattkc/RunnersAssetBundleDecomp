using System;
using System.Collections.Generic;

namespace DataTable
{
	public class ChaoDataVisitorAttribute : ChaoDataVisitorBase, IChaoDataSorting
	{
		private Dictionary<CharacterAttribute, List<ChaoData>> m_chaoList;

		public ChaoDataVisitorAttribute()
		{
			this.m_chaoList = new Dictionary<CharacterAttribute, List<ChaoData>>();
			List<ChaoData> value = new List<ChaoData>();
			List<ChaoData> value2 = new List<ChaoData>();
			List<ChaoData> value3 = new List<ChaoData>();
			this.m_chaoList.Add(CharacterAttribute.SPEED, value);
			this.m_chaoList.Add(CharacterAttribute.FLY, value2);
			this.m_chaoList.Add(CharacterAttribute.POWER, value3);
		}

		public override void visit(ChaoData chaoData)
		{
			if (this.m_chaoList != null)
			{
				switch (chaoData.charaAtribute)
				{
				case CharacterAttribute.SPEED:
					this.m_chaoList[CharacterAttribute.SPEED].Add(chaoData);
					break;
				case CharacterAttribute.FLY:
					this.m_chaoList[CharacterAttribute.FLY].Add(chaoData);
					break;
				case CharacterAttribute.POWER:
					this.m_chaoList[CharacterAttribute.POWER].Add(chaoData);
					break;
				}
			}
		}

		public List<ChaoData> GetChaoList(CharacterAttribute attribute)
		{
			List<ChaoData> result = null;
			switch (attribute)
			{
			case CharacterAttribute.SPEED:
				result = this.m_chaoList[CharacterAttribute.SPEED];
				break;
			case CharacterAttribute.FLY:
				result = this.m_chaoList[CharacterAttribute.FLY];
				break;
			case CharacterAttribute.POWER:
				result = this.m_chaoList[CharacterAttribute.POWER];
				break;
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
			Dictionary<int, List<ChaoData>> dictionary = new Dictionary<int, List<ChaoData>>();
			dictionary.Add(0, this.GetChaoList(CharacterAttribute.SPEED));
			dictionary.Add(1, this.GetChaoList(CharacterAttribute.FLY));
			dictionary.Add(2, this.GetChaoList(CharacterAttribute.POWER));
			int count = dictionary.Count;
			for (int i = 0; i < count; i++)
			{
				int key = (i + offset) % count;
				if (dictionary[key] != null)
				{
					dictionary[key].Reverse();
					if (list == null)
					{
						list = dictionary[key];
					}
					else
					{
						list.AddRange(dictionary[key]);
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
