using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

namespace DataTable
{
	public class RankingLeagueTable : MonoBehaviour
	{
		private static RankingLeagueTable s_instance;

		public static RankingLeagueTable Instance
		{
			get
			{
				return RankingLeagueTable.s_instance;
			}
		}

		private void Awake()
		{
			if (RankingLeagueTable.s_instance == null)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				RankingLeagueTable.s_instance = this;
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void OnDestroy()
		{
			if (RankingLeagueTable.s_instance == this)
			{
				RankingLeagueTable.s_instance = null;
			}
		}

		private void Setup()
		{
			base.enabled = false;
		}

		public static string GetItemText(List<ServerRemainOperator> leagueDataRemainOperator, string unitText = null, string unitTextMore = null, int head = 0, bool descendingOrder = false)
		{
			int num = 9876543;
			string text = string.Empty;
			List<RankingLeagueItem> list = new List<RankingLeagueItem>();
			List<ServerRemainOperator> list2 = new List<ServerRemainOperator>();
			Dictionary<int, List<ServerRemainOperator>> dictionary = new Dictionary<int, List<ServerRemainOperator>>();
			if (head < 0)
			{
				head = 0;
			}
			int num2 = 0;
			foreach (ServerRemainOperator current in leagueDataRemainOperator)
			{
				int num3 = 0;
				if (current.operatorData != 6)
				{
					int number = current.number;
					if (current.operatorData == 2)
					{
						current.operatorData = 4;
						current.number++;
					}
					else if (current.operatorData == 3)
					{
						current.operatorData = 5;
						current.number--;
					}
					if (current.number >= head)
					{
						list2.Add(current);
					}
					num3++;
					if (num2 < number)
					{
						num2 = number;
					}
				}
				else if (!dictionary.ContainsKey(num3))
				{
					dictionary.Add(num3, new List<ServerRemainOperator>
					{
						current
					});
				}
				else
				{
					dictionary[num3].Add(current);
				}
			}
			if (list2.Count == 0)
			{
				bool flag = true;
				if (dictionary.Count > 0)
				{
					Dictionary<int, List<ServerRemainOperator>>.KeyCollection keys = dictionary.Keys;
					foreach (int current2 in keys)
					{
						List<ServerRemainOperator> list3 = dictionary[current2];
						foreach (ServerRemainOperator current3 in list3)
						{
							if (head % current3.number == 0)
							{
								flag = false;
								ServerRemainOperator serverRemainOperator = new ServerRemainOperator();
								current3.CopyTo(serverRemainOperator);
								serverRemainOperator.number = head;
								serverRemainOperator.operatorData = 0;
								list2.Add(serverRemainOperator);
								serverRemainOperator = leagueDataRemainOperator[leagueDataRemainOperator.Count - 1];
								serverRemainOperator.number = head + 1;
								if (serverRemainOperator.operatorData == 2)
								{
									serverRemainOperator.operatorData = 4;
									serverRemainOperator.number++;
								}
								list2.Add(serverRemainOperator);
								break;
							}
						}
						if (!flag)
						{
							break;
						}
					}
				}
				if (flag)
				{
					ServerRemainOperator serverRemainOperator2 = leagueDataRemainOperator[leagueDataRemainOperator.Count - 1];
					serverRemainOperator2.number = head;
					if (serverRemainOperator2.operatorData == 2)
					{
						serverRemainOperator2.operatorData = 4;
						serverRemainOperator2.number++;
					}
					list2.Add(serverRemainOperator2);
				}
			}
			if (dictionary.Count > 0)
			{
				global::Debug.Log(string.Concat(new object[]
				{
					"+serverRemainOpeLoop:",
					dictionary.Count,
					"   serverRemainOpeLoop:",
					list2.Count
				}));
				Dictionary<int, List<ServerRemainOperator>>.KeyCollection keys2 = dictionary.Keys;
				foreach (int current4 in keys2)
				{
					List<ServerRemainOperator> list4 = dictionary[current4];
					ServerRemainOperator serverRemainOperator3 = null;
					List<bool> list5 = new List<bool>();
					if (leagueDataRemainOperator.Count > 0)
					{
						serverRemainOperator3 = leagueDataRemainOperator[leagueDataRemainOperator.Count - 1];
					}
					int num4 = 0;
					foreach (ServerRemainOperator current5 in list4)
					{
						if (num4 < current5.number)
						{
							num4 = current5.number;
						}
						list5.Add(false);
					}
					int num5 = head - num2;
					if (num5 < 0)
					{
						num5 = 0;
					}
					for (int i = num2 + 1 + num5; i < num4 + (num2 + 1) + head; i++)
					{
						ServerRemainOperator serverRemainOperator4 = null;
						for (int j = 0; j < list4.Count; j++)
						{
							if (i % list4[j].number == 0)
							{
								serverRemainOperator4 = new ServerRemainOperator();
								list4[j].CopyTo(serverRemainOperator4);
								list5[j] = true;
								break;
							}
						}
						if (serverRemainOperator4 != null)
						{
							serverRemainOperator4.number = i;
							serverRemainOperator4.operatorData = 0;
							list2.Add(serverRemainOperator4);
							if (serverRemainOperator3 != null && serverRemainOperator3.operatorData == 4)
							{
								ServerRemainOperator serverRemainOperator5 = new ServerRemainOperator();
								serverRemainOperator3.CopyTo(serverRemainOperator5);
								serverRemainOperator5.number = i + 1;
								list2.Add(serverRemainOperator5);
							}
						}
					}
				}
				global::Debug.Log(string.Concat(new object[]
				{
					"-serverRemainOpeLoop:",
					dictionary.Count,
					"   serverRemainOpeLoop:",
					list2.Count
				}));
			}
			if (list2.Count > 0)
			{
				list2.Sort(new Comparison<ServerRemainOperator>(RankingLeagueTable.RemainDownComparer));
				foreach (ServerRemainOperator current6 in list2)
				{
					RankingLeagueItem rankingLeagueItem = new RankingLeagueItem();
					rankingLeagueItem.ranking1 = current6.number;
					rankingLeagueItem.ranking2 = current6.number;
					rankingLeagueItem.operatorData = current6.operatorData;
					if (current6.ItemState != null && current6.ItemState.Count > 0)
					{
						Dictionary<int, ServerItemState>.KeyCollection keys3 = current6.ItemState.Keys;
						foreach (int current7 in keys3)
						{
							ServerItem serverItem = new ServerItem((ServerItem.Id)current7);
							ServerItemState serverItemState = new ServerItemState();
							serverItemState.m_itemId = (int)serverItem.id;
							serverItemState.m_num = current6.ItemState[current7].m_num;
							rankingLeagueItem.item.Add(serverItemState);
						}
					}
					list.Add(rankingLeagueItem);
				}
				switch (list[0].operatorData)
				{
				case 2:
				case 4:
					list[0].ranking2 = num;
					break;
				}
				int num6 = list[0].ranking2 + 1;
				foreach (RankingLeagueItem current8 in list)
				{
					current8.ranking2 = num6 - 1;
					int operatorData = current8.operatorData;
					if (operatorData == 2)
					{
						current8.ranking1++;
					}
					num6 = current8.ranking1;
				}
				if (!descendingOrder)
				{
					list.Reverse(0, list.Count);
				}
			}
			if (list.Count > 0)
			{
				string text2;
				if (string.IsNullOrEmpty(unitText))
				{
					text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "rank_unit").text;
				}
				else
				{
					text2 = unitText;
				}
				for (int k = 0; k < list.Count; k++)
				{
					string text3 = text2;
					RankingLeagueItem rankingLeagueItem2 = list[k];
					string text4 = " ";
					int ranking = rankingLeagueItem2.ranking1;
					int ranking2 = rankingLeagueItem2.ranking2;
					if (ranking == ranking2)
					{
						string text5 = text3;
						text5 = TextUtility.Replaces(text5, new Dictionary<string, string>
						{
							{
								"{PARAM}",
								ranking.ToString()
							}
						});
						text4 += text5;
					}
					else
					{
						string text5 = ranking.ToString();
						string text6 = text3;
						if (ranking2 != num)
						{
							text6 = TextUtility.Replaces(text6, new Dictionary<string, string>
							{
								{
									"{PARAM}",
									ranking2.ToString()
								}
							});
							text4 = text4 + text5 + " - " + text6;
						}
						else if (!string.IsNullOrEmpty(unitTextMore))
						{
							text5 = TextUtility.Replaces(unitTextMore, new Dictionary<string, string>
							{
								{
									"{PARAM}",
									ranking.ToString()
								}
							});
							text4 += text5;
						}
						else
						{
							text6 = "   ";
							text4 = text4 + text5 + " - " + text6;
						}
					}
					text4 += " ";
					text3 = string.Empty;
					for (int l = 0; l < rankingLeagueItem2.item.Count; l++)
					{
						text3 += RankingLeagueTable.GetRankingHelpItem(new ServerItem((ServerItem.Id)rankingLeagueItem2.item[l].m_itemId), rankingLeagueItem2.item[l].m_num);
						if (l + 1 < rankingLeagueItem2.item.Count)
						{
							text3 += ",";
						}
					}
					if (!string.IsNullOrEmpty(text3))
					{
						if (k > 0)
						{
							text += "\n";
						}
						text = text + text4 + text3;
					}
				}
			}
			return text;
		}

		private static string GetRankingHelpItem(ServerItem serverItem, int num)
		{
			ServerItem.IdType idType = serverItem.idType;
			if (idType == ServerItem.IdType.RSRING)
			{
				TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_help_rs_ring");
				text.ReplaceTag("{PARAM}", HudUtility.GetFormatNumString<int>(num));
				return text.text;
			}
			if (idType != ServerItem.IdType.RING)
			{
				if (idType != ServerItem.IdType.CHARA)
				{
					if (idType != ServerItem.IdType.CHAO)
					{
						string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "tutorial_sp_egg1_text").text;
						text2 = text2.Replace("{COUNT}", HudUtility.GetFormatNumString<int>(num));
						return serverItem.serverItemName + " " + text2;
					}
					int idIndex = serverItem.idIndex;
					ChaoData chaoData = ChaoTable.GetChaoData(idIndex);
					if (chaoData != null)
					{
						return chaoData.name;
					}
				}
				else
				{
					int idIndex2 = serverItem.idIndex;
					if ((ulong)idIndex2 < (ulong)((long)CharaName.Name.Length))
					{
						return TextUtility.GetCommonText("CharaName", CharaName.Name[idIndex2]);
					}
				}
				return string.Empty;
			}
			TextObject text3 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_help_ring");
			text3.ReplaceTag("{PARAM}", HudUtility.GetFormatNumString<int>(num));
			return text3.text;
		}

		private static int RankUpComparer(RankingLeagueItem itemA, RankingLeagueItem itemB)
		{
			if (itemA != null && itemB != null)
			{
				if (itemA.ranking1 > itemB.ranking1)
				{
					return 1;
				}
				if (itemA.ranking1 < itemB.ranking1)
				{
					return -1;
				}
			}
			return 0;
		}

		private static int RemainDownComparer(ServerRemainOperator itemA, ServerRemainOperator itemB)
		{
			if (itemA != null && itemB != null)
			{
				if (itemA.number > itemB.number)
				{
					return -1;
				}
				if (itemA.number < itemB.number)
				{
					return 1;
				}
			}
			return 0;
		}

		private static int RemainUpComparer(ServerRemainOperator itemA, ServerRemainOperator itemB)
		{
			if (itemA != null && itemB != null)
			{
				if (itemA.number > itemB.number)
				{
					return 1;
				}
				if (itemA.number < itemB.number)
				{
					return -1;
				}
			}
			return 0;
		}

		public static void SetupRankingLeagueTable()
		{
			RankingLeagueTable instance = RankingLeagueTable.Instance;
			if (instance == null)
			{
				GameObject gameObject = new GameObject("RankingLeagueTable");
				gameObject.AddComponent<RankingLeagueTable>();
				instance = RankingLeagueTable.Instance;
				if (instance != null)
				{
					instance.Setup();
				}
			}
			else
			{
				instance.Setup();
			}
		}

		public static RankingLeagueTable GetRankingLeagueTable()
		{
			RankingLeagueTable instance = RankingLeagueTable.Instance;
			if (instance == null)
			{
				RankingLeagueTable.SetupRankingLeagueTable();
				instance = RankingLeagueTable.Instance;
			}
			return instance;
		}
	}
}
