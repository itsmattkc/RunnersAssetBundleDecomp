using DataTable;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Text;

public class ServerSpinResultGeneral
{
	private Dictionary<int, List<ServerItemState>> m_itemState;

	private Dictionary<int, List<ServerChaoData>> m_acquiredChaoData;

	public int m_requiredSpEggs;

	private int m_getOtomoAndCharaMax = -1;

	private Dictionary<int, bool> _IsRequiredChao_k__BackingField;

	private int _ItemWon_k__BackingField;

	private Dictionary<int, int> _m_getAlreadyOverlap_k__BackingField;

	private Dictionary<int, int> _m_acquiredChaoCount_k__BackingField;

	public int NumRequiredSpEggs
	{
		get
		{
			return this.m_requiredSpEggs;
		}
	}

	public bool IsRequiredSpEgg
	{
		get
		{
			return 0 < this.m_requiredSpEggs;
		}
	}

	public Dictionary<int, ServerItemState> ItemState
	{
		get
		{
			Dictionary<int, ServerItemState> dictionary = new Dictionary<int, ServerItemState>();
			if (this.m_itemState != null && this.m_itemState.Count > 0)
			{
				Dictionary<int, List<ServerItemState>>.KeyCollection keys = this.m_itemState.Keys;
				foreach (int current in keys)
				{
					List<ServerItemState> list = this.m_itemState[current];
					if (list != null && list.Count > 0)
					{
						ServerItemState serverItemState = new ServerItemState();
						list[0].CopyTo(serverItemState);
						int num = 0;
						foreach (ServerItemState current2 in list)
						{
							if (current2 != null)
							{
								num += current2.m_num;
							}
						}
						serverItemState.m_num = num;
						dictionary.Add(current, serverItemState);
					}
				}
			}
			return dictionary;
		}
	}

	public Dictionary<int, ServerChaoData> AcquiredChaoData
	{
		get
		{
			Dictionary<int, ServerChaoData> dictionary = new Dictionary<int, ServerChaoData>();
			if (this.m_acquiredChaoData != null && this.m_acquiredChaoData.Count > 0)
			{
				Dictionary<int, List<ServerChaoData>>.KeyCollection keys = this.m_acquiredChaoData.Keys;
				foreach (int current in keys)
				{
					List<ServerChaoData> list = this.m_acquiredChaoData[current];
					if (list != null && list.Count > 0)
					{
						dictionary.Add(current, list[list.Count - 1]);
					}
				}
			}
			return dictionary;
		}
	}

	public Dictionary<int, bool> IsRequiredChao
	{
		get;
		private set;
	}

	public int ItemWon
	{
		get;
		set;
	}

	private Dictionary<int, int> m_getAlreadyOverlap
	{
		get;
		set;
	}

	public bool IsMulti
	{
		get
		{
			return this.ItemWon == -1;
		}
	}

	public string AcquiredListText
	{
		get
		{
			return this.GetAcquiredListText();
		}
	}

	private Dictionary<int, int> m_acquiredChaoCount
	{
		get;
		set;
	}

	public bool IsRankup
	{
		get
		{
			bool result = false;
			Dictionary<int, List<ServerItemState>>.KeyCollection keys = this.m_itemState.Keys;
			foreach (int current in keys)
			{
				ServerItem serverItem = new ServerItem((ServerItem.Id)current);
				if (serverItem.idType == ServerItem.IdType.ITEM_ROULLETE_WIN)
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}

	public ServerSpinResultGeneral()
	{
		this.m_acquiredChaoData = new Dictionary<int, List<ServerChaoData>>();
		this.m_acquiredChaoCount = new Dictionary<int, int>();
		this.IsRequiredChao = new Dictionary<int, bool>();
		this.m_requiredSpEggs = 0;
		this.m_itemState = new Dictionary<int, List<ServerItemState>>();
		this.ItemWon = 0;
		this.m_getAlreadyOverlap = new Dictionary<int, int>();
		this.m_getOtomoAndCharaMax = -1;
	}

	public ServerSpinResultGeneral(ServerChaoSpinResult result)
	{
		this.m_acquiredChaoData = new Dictionary<int, List<ServerChaoData>>();
		this.m_acquiredChaoCount = new Dictionary<int, int>();
		this.IsRequiredChao = new Dictionary<int, bool>();
		this.m_requiredSpEggs = 0;
		this.m_itemState = new Dictionary<int, List<ServerItemState>>();
		this.ItemWon = 0;
		this.m_getAlreadyOverlap = new Dictionary<int, int>();
		this.m_getOtomoAndCharaMax = 0;
		if (result.AcquiredChaoData != null)
		{
			this.AddChaoState(result.AcquiredChaoData);
			if (result.ItemState != null && result.ItemState.Count > 0)
			{
				Dictionary<int, ServerItemState>.KeyCollection keys = result.ItemState.Keys;
				foreach (int current in keys)
				{
					this.AddItemState(new ServerItemState
					{
						m_itemId = result.ItemState[current].m_itemId,
						m_num = result.ItemState[current].m_num
					});
				}
			}
			this.m_getOtomoAndCharaMax = 1;
		}
		this.ItemWon = result.ItemWon;
	}

	public ServerSpinResultGeneral(ServerWheelOptions newOptions, ServerWheelOptions oldOptions)
	{
		this.m_acquiredChaoData = new Dictionary<int, List<ServerChaoData>>();
		this.m_acquiredChaoCount = new Dictionary<int, int>();
		this.IsRequiredChao = new Dictionary<int, bool>();
		this.m_requiredSpEggs = 0;
		this.m_itemState = new Dictionary<int, List<ServerItemState>>();
		this.ItemWon = oldOptions.m_itemWon;
		this.m_getAlreadyOverlap = new Dictionary<int, int>();
		this.m_getOtomoAndCharaMax = -1;
		ServerChaoData chao = oldOptions.GetChao();
		if (chao != null)
		{
			this.AddChaoState(chao);
			if (newOptions.IsItemList())
			{
				Dictionary<int, ServerItemState>.KeyCollection keys = newOptions.m_itemList.Keys;
				foreach (int current in keys)
				{
					this.AddItemState(newOptions.m_itemList[current]);
				}
			}
			this.m_getOtomoAndCharaMax = 1;
		}
		else
		{
			ServerItemState item = oldOptions.GetItem();
			if (item != null)
			{
				this.AddItemState(oldOptions.GetItem());
			}
			this.m_getOtomoAndCharaMax = 0;
		}
	}

	private string GetAcquiredListText()
	{
		string text = null;
		if (this.IsMulti)
		{
			List<int> list = null;
			if (this.m_acquiredChaoData != null && this.m_acquiredChaoData.Count > 0)
			{
				list = new List<int>();
				Dictionary<int, List<ServerChaoData>>.KeyCollection keys = this.m_acquiredChaoData.Keys;
				foreach (int current in keys)
				{
					foreach (ServerChaoData current2 in this.m_acquiredChaoData[current])
					{
						if (current2.Rarity >= 100)
						{
							list.Add(current);
						}
					}
				}
				foreach (int current3 in keys)
				{
					foreach (ServerChaoData current4 in this.m_acquiredChaoData[current3])
					{
						if (current4.Rarity < 100)
						{
							list.Add(current3);
						}
					}
				}
			}
			if (list != null)
			{
				Dictionary<int, int> dictionary = new Dictionary<int, int>();
				foreach (int current5 in list)
				{
					ServerItem serverItem = new ServerItem((ServerItem.Id)current5);
					string serverItemName = serverItem.serverItemName;
					if (string.IsNullOrEmpty(text))
					{
						text = serverItemName;
					}
					else
					{
						text = text + "\n" + serverItemName;
					}
					if (this.m_getAlreadyOverlap != null && this.m_getAlreadyOverlap.ContainsKey(current5) && this.m_getAlreadyOverlap[current5] > 0)
					{
						if (dictionary.ContainsKey(current5))
						{
							Dictionary<int, int> dictionary2;
							Dictionary<int, int> expr_1E4 = dictionary2 = dictionary;
							int num;
							int expr_1E9 = num = current5;
							num = dictionary2[num];
							expr_1E4[expr_1E9] = num + 1;
						}
						else
						{
							dictionary.Add(current5, 1);
						}
						int num2 = this.m_acquiredChaoCount[current5] - this.m_getAlreadyOverlap[current5];
						if (num2 < dictionary[current5])
						{
							text = text + " " + TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "GeneralWindow", "get_item_overlap").text;
						}
					}
				}
			}
			if (this.m_itemState != null && this.m_itemState.Count > 0)
			{
				Dictionary<int, List<ServerItemState>>.KeyCollection keys2 = this.m_itemState.Keys;
				foreach (int current6 in keys2)
				{
					ServerItem serverItem2 = new ServerItem((ServerItem.Id)current6);
					string serverItemName2 = serverItem2.serverItemName;
					foreach (ServerItemState current7 in this.m_itemState[current6])
					{
						if (string.IsNullOrEmpty(text))
						{
							text = serverItemName2;
						}
						else
						{
							text = text + "\n" + serverItemName2;
						}
						text = text + " × " + current7.m_num;
					}
				}
			}
		}
		return text;
	}

	public void AddItemState(ServerItemState itemState)
	{
		if (this.m_itemState.ContainsKey(itemState.m_itemId))
		{
			this.m_itemState[itemState.m_itemId].Add(itemState);
		}
		else
		{
			List<ServerItemState> list = new List<ServerItemState>();
			list.Add(itemState);
			this.m_itemState.Add(itemState.m_itemId, list);
		}
		if (itemState.m_itemId == 220000)
		{
			this.m_requiredSpEggs += itemState.m_num;
		}
	}

	public void AddChaoState(ServerChaoData chaoState)
	{
		if (this.m_acquiredChaoData.ContainsKey(chaoState.Id))
		{
			this.m_acquiredChaoData[chaoState.Id].Add(chaoState);
		}
		else
		{
			List<ServerChaoData> list = new List<ServerChaoData>();
			list.Add(chaoState);
			this.m_acquiredChaoData.Add(chaoState.Id, list);
		}
		bool flag = false;
		int num = -1;
		ServerPlayerState playerState = ServerInterface.PlayerState;
		if (playerState != null)
		{
			if (chaoState.Rarity < 100)
			{
				ServerChaoState serverChaoState = playerState.ChaoStateByItemID(chaoState.Id);
				if (serverChaoState != null)
				{
					flag = (serverChaoState.Status != ServerChaoState.ChaoStatus.MaxLevel);
					if (serverChaoState.IsOwned)
					{
						num = serverChaoState.Level;
					}
					else
					{
						num = -1;
					}
				}
			}
			else
			{
				ServerCharacterState serverCharacterState = playerState.CharacterStateByItemID(chaoState.Id);
				if (serverCharacterState == null)
				{
					flag = true;
				}
				else if (serverCharacterState.Id < 0 || !serverCharacterState.IsUnlocked || serverCharacterState.star < serverCharacterState.starMax)
				{
					flag = true;
					num = serverCharacterState.star;
				}
			}
		}
		if (this.IsRequiredChao.ContainsKey(chaoState.Id))
		{
			this.IsRequiredChao[chaoState.Id] = flag;
		}
		else
		{
			this.IsRequiredChao.Add(chaoState.Id, flag);
		}
		if (this.m_acquiredChaoCount.ContainsKey(chaoState.Id))
		{
			Dictionary<int, int> acquiredChaoCount;
			Dictionary<int, int> expr_15E = acquiredChaoCount = this.m_acquiredChaoCount;
			int num2;
			int expr_167 = num2 = chaoState.Id;
			num2 = acquiredChaoCount[num2];
			expr_15E[expr_167] = num2 + 1;
		}
		else
		{
			this.m_acquiredChaoCount.Add(chaoState.Id, 1);
		}
		if (this.m_getAlreadyOverlap.ContainsKey(chaoState.Id))
		{
			if (this.m_acquiredChaoCount.ContainsKey(chaoState.Id))
			{
				if (chaoState.Rarity < 100)
				{
					if (num + this.m_acquiredChaoCount[chaoState.Id] > ChaoTable.ChaoMaxLevel())
					{
						Dictionary<int, int> getAlreadyOverlap;
						Dictionary<int, int> expr_1F1 = getAlreadyOverlap = this.m_getAlreadyOverlap;
						int num2;
						int expr_1FA = num2 = chaoState.Id;
						num2 = getAlreadyOverlap[num2];
						expr_1F1[expr_1FA] = num2 + 1;
					}
				}
				else
				{
					ServerCharacterState serverCharacterState2 = playerState.CharacterStateByItemID(chaoState.Id);
					if (serverCharacterState2 != null)
					{
						if (num + this.m_acquiredChaoCount[chaoState.Id] > serverCharacterState2.starMax)
						{
							Dictionary<int, int> getAlreadyOverlap2;
							Dictionary<int, int> expr_250 = getAlreadyOverlap2 = this.m_getAlreadyOverlap;
							int num2;
							int expr_259 = num2 = chaoState.Id;
							num2 = getAlreadyOverlap2[num2];
							expr_250[expr_259] = num2 + 1;
						}
					}
					else
					{
						this.m_getAlreadyOverlap[chaoState.Id] = 1;
					}
				}
			}
		}
		else if (!flag)
		{
			this.m_getAlreadyOverlap.Add(chaoState.Id, 1);
		}
		else
		{
			this.m_getAlreadyOverlap.Add(chaoState.Id, 0);
		}
	}

	public void CopyTo(ServerSpinResultGeneral to)
	{
		to.IsRequiredChao = this.IsRequiredChao;
		to.m_requiredSpEggs = this.m_requiredSpEggs;
		to.m_itemState.Clear();
		to.m_acquiredChaoData.Clear();
		foreach (List<ServerChaoData> current in this.m_acquiredChaoData.Values)
		{
			List<ServerChaoData> list = new List<ServerChaoData>();
			int key = 0;
			foreach (ServerChaoData current2 in current)
			{
				key = current2.Id;
				list.Add(current2);
			}
			to.m_acquiredChaoData.Add(key, list);
		}
		foreach (List<ServerItemState> current3 in this.m_itemState.Values)
		{
			List<ServerItemState> list2 = new List<ServerItemState>();
			int key2 = 0;
			foreach (ServerItemState current4 in current3)
			{
				key2 = current4.m_itemId;
				list2.Add(current4);
			}
			to.m_itemState.Add(key2, list2);
		}
		to.ItemWon = this.ItemWon;
		to.m_getAlreadyOverlap = this.m_getAlreadyOverlap;
		to.m_acquiredChaoCount = this.m_acquiredChaoCount;
		to.GetOtomoAndCharaMax();
	}

	public int GetOtomoAndCharaMax()
	{
		int num = 0;
		Dictionary<int, bool>.KeyCollection keys = this.IsRequiredChao.Keys;
		foreach (int current in keys)
		{
			if (this.IsRequiredChao[current])
			{
				num++;
			}
		}
		this.m_getOtomoAndCharaMax = num;
		return num;
	}

	public bool CheckGetChara()
	{
		bool result = false;
		Dictionary<int, bool>.KeyCollection keys = this.IsRequiredChao.Keys;
		foreach (int current in keys)
		{
			if (current >= 300000 && current < 400000)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public ServerChaoData GetShowData(int index)
	{
		ServerChaoData result = null;
		if (this.m_getOtomoAndCharaMax > 0 && index >= 0 && index < this.m_getOtomoAndCharaMax)
		{
			Dictionary<int, bool>.KeyCollection keys = this.IsRequiredChao.Keys;
			List<int> list = new List<int>();
			foreach (int current in keys)
			{
				if (this.IsRequiredChao[current])
				{
					list.Add(current);
				}
			}
			list.Sort();
			if (list.Count > index && this.m_acquiredChaoData.ContainsKey(list[index]))
			{
				List<ServerChaoData> list2 = this.m_acquiredChaoData[list[index]];
				if (list2 != null && list2.Count > 0)
				{
					result = list2[list2.Count - 1];
				}
			}
		}
		return result;
	}

	public void Dump()
	{
	}
}
