using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DataTable
{
	public class ChaoDataVisitorUtility
	{
		private static Func<KeyValuePair<int, List<ChaoData>>, int> __f__am_cache0;

		private static Func<KeyValuePair<int, List<ChaoData>>, int> __f__am_cache1;

		private static Comparison<ChaoData> __f__am_cache2;

		private static Comparison<ChaoData> __f__am_cache3;

		public static void SortKeyDictionaryInt(ref Dictionary<int, List<ChaoData>> dictionary, bool descending = false)
		{
			Dictionary<int, List<ChaoData>> dictionary2 = new Dictionary<int, List<ChaoData>>();
			IOrderedEnumerable<KeyValuePair<int, List<ChaoData>>> orderedEnumerable;
			if (descending)
			{
				orderedEnumerable = from x in dictionary
				orderby x.Key descending
				select x;
			}
			else
			{
				orderedEnumerable = from x in dictionary
				orderby x.Key
				select x;
			}
			foreach (KeyValuePair<int, List<ChaoData>> current in orderedEnumerable)
			{
				dictionary2.Add(current.Key, current.Value);
			}
			dictionary = dictionary2;
		}

		public static void AddListInt(ref List<ChaoData> list, Dictionary<int, List<ChaoData>> dictionary, bool raritySort = false, bool descending = false)
		{
			if (dictionary == null)
			{
				return;
			}
			if (dictionary.Count <= 0)
			{
				return;
			}
			Dictionary<int, List<ChaoData>>.KeyCollection keys = dictionary.Keys;
			if (raritySort)
			{
				foreach (int current in keys)
				{
					if (dictionary[current] != null && dictionary[current].Count > 0)
					{
						if (!descending)
						{
							dictionary[current].Sort((ChaoData chaoA, ChaoData chaoB) => chaoB.rarity - chaoA.rarity);
						}
						else
						{
							dictionary[current].Sort((ChaoData chaoA, ChaoData chaoB) => chaoA.rarity - chaoB.rarity);
						}
					}
				}
			}
			foreach (int current2 in keys)
			{
				if (list == null)
				{
					list = dictionary[current2];
				}
				else
				{
					list.AddRange(dictionary[current2]);
				}
			}
		}
	}
}
