using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Text;
using UnityEngine;

namespace DataTable
{
	public class ChaoTable : MonoBehaviour
	{
		private const int MARKER_INTERVAL = 10;

		private const int LEVEL_MAX = 10;

		[SerializeField]
		private TextAsset m_chaoTabel;

		private static ChaoData[] s_chaoDataTable;

		private static Dictionary<int, List<int>> s_chaoDataTableMarker;

		private static bool s_setup;

		private static bool s_eventList;

		private static int s_loadingCount;

		private static List<ChaoData> s_loadingChaoList;

		private static Func<ChaoData, Guid> __f__am_cache7;

		private void Start()
		{
			if (ChaoTable.s_chaoDataTable == null)
			{
				string s = AESCrypt.Decrypt(this.m_chaoTabel.text);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ChaoData[]));
				StringReader textReader = new StringReader(s);
				ChaoTable.s_chaoDataTable = (ChaoData[])xmlSerializer.Deserialize(textReader);
			}
		}

		private void OnDestroy()
		{
			ChaoTable.s_chaoDataTable = null;
			if (ChaoTable.s_chaoDataTableMarker != null)
			{
				ChaoTable.s_chaoDataTableMarker.Clear();
				ChaoTable.s_chaoDataTableMarker = null;
			}
			if (ChaoTable.s_loadingChaoList != null)
			{
				ChaoTable.s_loadingChaoList.Clear();
				ChaoTable.s_loadingChaoList = null;
			}
			ChaoTable.s_setup = false;
			ChaoTable.s_loadingCount = 0;
			ChaoTable.s_eventList = false;
		}

		private static void Setup()
		{
			if (ChaoTable.s_setup)
			{
				return;
			}
			if (ChaoTable.s_chaoDataTable != null)
			{
				Array.Sort<ChaoData>(ChaoTable.s_chaoDataTable, new Comparison<ChaoData>(ChaoData.ChaoCompareById));
				ChaoTable.s_chaoDataTableMarker = new Dictionary<int, List<int>>();
				int item = 0;
				int num = -1;
				ChaoData[] array = ChaoTable.s_chaoDataTable;
				for (int i = 0; i < array.Length; i++)
				{
					ChaoData chaoData = array[i];
					int num2 = chaoData.id / 1000 % 10;
					if (num2 != num)
					{
						if (!ChaoTable.s_chaoDataTableMarker.ContainsKey(num2))
						{
							ChaoTable.s_chaoDataTableMarker[num2] = new List<int>();
							ChaoTable.s_chaoDataTableMarker[num2].Add(item);
						}
					}
					else if (chaoData.id % 10 == 0 && ChaoTable.s_chaoDataTableMarker.ContainsKey(num2))
					{
						ChaoTable.s_chaoDataTableMarker[num2].Add(item);
					}
					chaoData.index = item++;
					chaoData.name = TextUtility.GetChaoText("Chao", "name" + chaoData.id.ToString("D4"));
					chaoData.nameTwolines = TextUtility.GetChaoText("Chao", "name_for_menu_" + chaoData.id.ToString("D4"));
					chaoData.StatusUpdate();
					num = num2;
				}
				ChaoTable.s_setup = true;
			}
		}

		public static int ChaoMaxLevel()
		{
			return 10;
		}

		public static bool ChangeChaoAbility(int chaoId, int abilityEventId)
		{
			bool result = false;
			if (ChaoTable.s_chaoDataTable != null)
			{
				ChaoData[] array = ChaoTable.s_chaoDataTable;
				for (int i = 0; i < array.Length; i++)
				{
					ChaoData chaoData = array[i];
					if (chaoData.id == chaoId)
					{
						result = chaoData.SetChaoAbility(abilityEventId);
						break;
					}
				}
			}
			return result;
		}

		public static bool ChangeChaoAbilityIndex(int chaoId, int abilityIndex)
		{
			bool result = false;
			if (ChaoTable.s_chaoDataTable != null)
			{
				ChaoData[] array = ChaoTable.s_chaoDataTable;
				for (int i = 0; i < array.Length; i++)
				{
					ChaoData chaoData = array[i];
					if (chaoData.id == chaoId)
					{
						result = chaoData.SetChaoAbilityIndex(abilityIndex);
						break;
					}
				}
			}
			return result;
		}

		public static bool ChangeChaoAbilityNext(int chaoId)
		{
			bool result = false;
			if (ChaoTable.s_chaoDataTable != null)
			{
				ChaoData[] array = ChaoTable.s_chaoDataTable;
				for (int i = 0; i < array.Length; i++)
				{
					ChaoData chaoData = array[i];
					if (chaoData.id == chaoId)
					{
						if (chaoData.abilityNum - 1 > chaoData.currentAbility)
						{
							result = chaoData.SetChaoAbilityIndex(chaoData.currentAbility + 1);
						}
						else
						{
							result = chaoData.SetChaoAbilityIndex(0);
						}
						break;
					}
				}
			}
			return result;
		}

		public static ChaoData[] GetDataTable()
		{
			ChaoTable.Setup();
			return ChaoTable.s_chaoDataTable;
		}

		public static List<ChaoData> GetDataTable(ChaoData.Rarity rarity)
		{
			List<ChaoData> result = null;
			if (rarity != ChaoData.Rarity.NONE)
			{
				ChaoTable.Setup();
				ChaoDataVisitorBase chaoDataVisitorBase = new ChaoDataVisitorRarity();
				if (ChaoTable.s_chaoDataTable != null)
				{
					ChaoData[] array = ChaoTable.s_chaoDataTable;
					for (int i = 0; i < array.Length; i++)
					{
						ChaoData chaoData = array[i];
						chaoData.accept(ref chaoDataVisitorBase);
					}
				}
				ChaoDataVisitorRarity chaoDataVisitorRarity = (ChaoDataVisitorRarity)chaoDataVisitorBase;
				result = chaoDataVisitorRarity.GetChaoList(rarity, ChaoData.Rarity.NONE);
			}
			return result;
		}

		public static List<ChaoData> GetPossessionChaoData()
		{
			List<ChaoData> list = null;
			if (ChaoTable.s_chaoDataTable == null)
			{
				ChaoTable.Setup();
			}
			if (ChaoTable.s_chaoDataTable != null && ChaoTable.s_chaoDataTable.Length > 0)
			{
				int num = ChaoTable.s_chaoDataTable.Length;
				for (int i = 0; i < num; i++)
				{
					ChaoData chaoData = ChaoTable.s_chaoDataTable[i];
					if (chaoData != null && chaoData.level >= 0)
					{
						if (list == null)
						{
							list = new List<ChaoData>();
						}
						list.Add(chaoData);
					}
				}
			}
			return list;
		}

		public static List<ChaoData> GetChaoData(List<int> ids)
		{
			List<ChaoData> list = null;
			if (ids != null && ids.Count > 0)
			{
				ChaoTable.Setup();
				int count = ids.Count;
				for (int i = 0; i < count; i++)
				{
					int id = ids[i];
					ChaoData chaoData = ChaoTable.GetChaoData(id);
					if (chaoData != null)
					{
						if (list == null)
						{
							list = new List<ChaoData>();
						}
						list.Add(chaoData);
					}
				}
			}
			return list;
		}

		public static ChaoData GetChaoData(int id)
		{
			ChaoData result = null;
			ChaoTable.Setup();
			if (id >= 0 && ChaoTable.s_chaoDataTable != null && ChaoTable.s_chaoDataTableMarker != null)
			{
				int num = ChaoTable.s_chaoDataTable.Length;
				bool flag = false;
				int num2 = 0;
				int num3 = id / 1000 % 10;
				if (ChaoTable.s_chaoDataTableMarker.ContainsKey(num3))
				{
					int num4 = id % 10;
					if (num4 >= 5)
					{
						flag = true;
					}
					int num5 = id / 10 % 100;
					if (ChaoTable.s_chaoDataTableMarker[num3].Count > num5)
					{
						if (flag && ChaoTable.s_chaoDataTableMarker[num3].Count - 1 == num5)
						{
							if (!ChaoTable.s_chaoDataTableMarker.ContainsKey(num3 + 1))
							{
								num2 = num - 1;
							}
							else
							{
								num2 = ChaoTable.s_chaoDataTableMarker[num3 + 1][0] - 1;
							}
						}
						else if (flag)
						{
							num2 = ChaoTable.s_chaoDataTableMarker[num3][num5 + 1] - 1;
						}
						else
						{
							num2 = ChaoTable.s_chaoDataTableMarker[num3][num5];
						}
					}
					if (num2 < 0)
					{
						num2 = 0;
					}
				}
				for (int i = 0; i < num; i++)
				{
					int num6;
					if (!flag)
					{
						num6 = (num2 + i) % num;
					}
					else
					{
						num6 = (num2 - i + num) % num;
					}
					if (ChaoTable.s_chaoDataTable[num6].id == id)
					{
						result = ChaoTable.s_chaoDataTable[num6];
						break;
					}
				}
			}
			return result;
		}

		private static void ResetLoadingChao()
		{
			ChaoTable.s_loadingChaoList = null;
			ChaoTable.s_loadingCount = 0;
			ChaoTable.s_eventList = false;
		}

		public static void CheckEventTime()
		{
			if (ChaoTable.s_eventList)
			{
				if (EventManager.Instance != null && !EventManager.Instance.IsInEvent())
				{
					ChaoTable.ResetLoadingChao();
				}
			}
			else if (EventManager.Instance != null && EventManager.Instance.IsInEvent())
			{
				ChaoTable.ResetLoadingChao();
			}
		}

		public static List<ChaoData> GetEyeCatcherChaoData(List<ServerChaoState> serverChaoList)
		{
			if (serverChaoList != null)
			{
				List<int> list = new List<int>();
				EyeCatcherChaoData[] eyeCatcherChaoDatas = EventManager.Instance.GetEyeCatcherChaoDatas();
				if (eyeCatcherChaoDatas != null)
				{
					EyeCatcherChaoData[] array = eyeCatcherChaoDatas;
					for (int i = 0; i < array.Length; i++)
					{
						EyeCatcherChaoData eyeCatcherChaoData = array[i];
						foreach (ServerChaoState current in serverChaoList)
						{
							int num = eyeCatcherChaoData.chao_id + 400000;
							if (num == current.Id)
							{
								list.Add(eyeCatcherChaoData.chao_id);
								break;
							}
						}
					}
				}
				RewardChaoData rewardChaoData = EventManager.Instance.GetRewardChaoData();
				if (rewardChaoData != null)
				{
					foreach (ServerChaoState current2 in serverChaoList)
					{
						int num2 = rewardChaoData.chao_id + 400000;
						if (num2 == current2.Id)
						{
							list.Add(rewardChaoData.chao_id);
							break;
						}
					}
				}
				if (list.Count > 0)
				{
					return ChaoTable.GetChaoData(list);
				}
			}
			return null;
		}

		public static ChaoData GetLoadingChao()
		{
			ChaoTable.Setup();
			if (ServerInterface.LoggedInServerInterface == null && ChaoTable.s_chaoDataTable != null)
			{
				ChaoData[] array = ChaoTable.s_chaoDataTable;
				for (int l = 0; l < array.Length; l++)
				{
					ChaoData chaoData = array[l];
					if (chaoData.id == 0)
					{
						return chaoData;
					}
				}
				return null;
			}
			ChaoTable.CheckEventTime();
			if (ChaoTable.s_loadingChaoList == null && ChaoTable.s_chaoDataTable != null)
			{
				List<ChaoData> list = null;
				List<ServerChaoState> list2 = null;
				ServerPlayerState playerState = ServerInterface.PlayerState;
				if (playerState != null && playerState.ChaoStates != null)
				{
					list2 = playerState.ChaoStates;
				}
				if (EventManager.Instance != null && EventManager.Instance.IsInEvent())
				{
					ChaoDataVisitorBase chaoDataVisitorBase = new ChaoDataVisitorEvent();
					ChaoData[] array2 = ChaoTable.s_chaoDataTable;
					for (int j = 0; j < array2.Length; j++)
					{
						ChaoData chaoData2 = array2[j];
						if (list2 != null)
						{
							foreach (ServerChaoState current in list2)
							{
								int num = chaoData2.id + 400000;
								if (current.Id == num)
								{
									chaoData2.accept(ref chaoDataVisitorBase);
									break;
								}
							}
						}
						else
						{
							chaoData2.accept(ref chaoDataVisitorBase);
						}
					}
					ChaoDataVisitorEvent chaoDataVisitorEvent = (ChaoDataVisitorEvent)chaoDataVisitorBase;
					if (chaoDataVisitorEvent != null)
					{
						list = chaoDataVisitorEvent.GetChaoList(EventManager.GetSpecificId());
						if (list == null)
						{
							list = ChaoTable.GetEyeCatcherChaoData(list2);
						}
					}
					ChaoTable.s_eventList = true;
				}
				if (list == null)
				{
					ChaoDataVisitorBase chaoDataVisitorBase2 = new ChaoDataVisitorRarity();
					ChaoData[] array3 = ChaoTable.s_chaoDataTable;
					for (int k = 0; k < array3.Length; k++)
					{
						ChaoData chaoData3 = array3[k];
						if (list2 != null)
						{
							foreach (ServerChaoState current2 in list2)
							{
								int num2 = chaoData3.id + 400000;
								if (current2.Id == num2)
								{
									chaoData3.accept(ref chaoDataVisitorBase2);
									break;
								}
							}
						}
						else
						{
							chaoData3.accept(ref chaoDataVisitorBase2);
						}
					}
					ChaoDataVisitorRarity chaoDataVisitorRarity = (ChaoDataVisitorRarity)chaoDataVisitorBase2;
					if (chaoDataVisitorRarity != null)
					{
						list = chaoDataVisitorRarity.GetChaoList(ChaoData.Rarity.SRARE, ChaoData.Rarity.RARE);
					}
				}
				if (list != null)
				{
					ChaoTable.s_loadingChaoList = (from i in list
					orderby Guid.NewGuid()
					select i).ToList<ChaoData>();
				}
			}
			if (ChaoTable.s_loadingChaoList == null || ChaoTable.s_loadingChaoList.Count <= 0)
			{
				return null;
			}
			int index = ChaoTable.s_loadingCount % ChaoTable.s_loadingChaoList.Count;
			ChaoTable.s_loadingCount++;
			return ChaoTable.s_loadingChaoList[index];
		}

		public static ChaoData GetChaoDataOfIndex(int index)
		{
			ChaoData[] dataTable = ChaoTable.GetDataTable();
			return (dataTable == null || (ulong)index >= (ulong)((long)dataTable.Length)) ? null : dataTable[index];
		}

		public static int GetChaoIdOfIndex(int index)
		{
			ChaoData chaoDataOfIndex = ChaoTable.GetChaoDataOfIndex(index);
			return (chaoDataOfIndex == null) ? (-1) : chaoDataOfIndex.id;
		}

		public static int GetRandomChaoId()
		{
			ChaoData[] dataTable = ChaoTable.GetDataTable();
			return (dataTable == null) ? (-1) : dataTable[UnityEngine.Random.Range(0, dataTable.Length)].id;
		}
	}
}
