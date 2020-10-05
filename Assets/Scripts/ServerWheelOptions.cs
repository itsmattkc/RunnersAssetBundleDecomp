using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ServerWheelOptions
{
	public enum WheelItemType
	{
		CharacterTokenAmy,
		CharacterTokenTails,
		CharacterTokenKnuckles,
		CharacterTokenShadow,
		CharacterTokenBlaze,
		FeverTime,
		GoldenEnemy,
		RedStarRingsSmall,
		RedStarRingsMedium,
		RedStarRingsLarge,
		RedStarRingsJackpot,
		RingsSmall,
		RingsMedium,
		RingsLarge,
		RingsJackpot,
		SpinAgain,
		Energy,
		Max
	}

	public int m_nextSpinCost;

	public int[] m_items;

	public int[] m_itemQuantities;

	public int[] m_itemWeight;

	public int m_itemWon;

	public int m_spinCost;

	public RouletteUtility.WheelRank m_rouletteRank;

	public int m_numRouletteToken;

	public int m_numJackpotRing;

	public int m_numRemaining;

	public DateTime m_nextFreeSpin;

	public Dictionary<int, ServerItemState> m_itemList;

	private static Converter<int, string> __f__am_cacheC;

	public int m_numRemainingFree
	{
		get
		{
			int num = this.m_numRemaining - this.m_numRouletteToken;
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}
	}

	public int NumRequiredSpEggs
	{
		get
		{
			int num = 0;
			if (this.m_itemList != null)
			{
				Dictionary<int, ServerItemState>.KeyCollection keys = this.m_itemList.Keys;
				foreach (int current in keys)
				{
					if (current == 220000)
					{
						num += this.m_itemList[current].m_num;
					}
				}
			}
			return num;
		}
	}

	public ServerWheelOptions(ServerWheelOptions options = null)
	{
		this.m_nextSpinCost = 1;
		this.m_items = new int[8];
		this.m_itemQuantities = new int[8];
		this.m_itemWeight = new int[8];
		for (int i = 0; i < 8; i++)
		{
			if (i == 0)
			{
				this.m_items[i] = 200000;
			}
			else
			{
				this.m_items[i] = 120000 + i - 1;
			}
			this.m_itemQuantities[i] = 1;
			this.m_itemWeight[i] = 1;
		}
		this.m_itemWon = 0;
		this.m_spinCost = 0;
		this.m_rouletteRank = RouletteUtility.WheelRank.Normal;
		this.m_numRouletteToken = 0;
		this.m_numJackpotRing = 0;
		this.m_numRemaining = 1;
		this.m_nextFreeSpin = DateTime.Now;
		if (options != null)
		{
			options.CopyTo(this);
		}
	}

	public void Dump()
	{
		string text = string.Join(",", Array.ConvertAll<int, string>(this.m_items, (int item) => item.ToString()));
		UnityEngine.Debug.Log(string.Format("items={0}, itemWon={1}, spinCost={2}, nextSpinCost={3}, nextFreeSpin={4}", new object[]
		{
			text,
			this.m_itemWon,
			this.m_spinCost,
			this.m_nextSpinCost,
			this.m_nextFreeSpin
		}));
	}

	public void RefreshFakeState()
	{
	}

	public ServerItemState GetItem()
	{
		ServerItemState serverItemState = null;
		if (this.m_itemWon >= 0 && this.m_itemWon < this.m_items.Length)
		{
			ServerItem serverItem = new ServerItem((ServerItem.Id)this.m_items[this.m_itemWon]);
			if (serverItem.idType != ServerItem.IdType.CHAO && serverItem.idType != ServerItem.IdType.CHARA)
			{
				serverItemState = new ServerItemState();
				serverItemState.m_itemId = (int)serverItem.id;
				serverItemState.m_num = this.m_itemQuantities[this.m_itemWon];
			}
		}
		return serverItemState;
	}

	public ServerChaoState GetChao()
	{
		ServerChaoState serverChaoState = null;
		if (this.m_itemWon >= 0 && this.m_itemWon < this.m_items.Length)
		{
			ServerItem serverItem = new ServerItem((ServerItem.Id)this.m_items[this.m_itemWon]);
			if (serverItem.idType == ServerItem.IdType.CHAO || serverItem.idType == ServerItem.IdType.CHARA)
			{
				ServerPlayerState playerState = ServerInterface.PlayerState;
				serverChaoState = new ServerChaoState();
				if (serverItem.idType == ServerItem.IdType.CHAO)
				{
					ServerChaoState serverChaoState2 = playerState.ChaoStateByItemID((int)serverItem.id);
					serverChaoState.Id = (int)serverItem.id;
					if (serverChaoState2 != null)
					{
						serverChaoState.Status = serverChaoState2.Status;
						serverChaoState.Level = serverChaoState2.Level;
						serverChaoState.Rarity = serverChaoState2.Rarity;
					}
					else
					{
						serverChaoState.Status = ServerChaoState.ChaoStatus.NotOwned;
						serverChaoState.Level = 0;
						serverChaoState.Rarity = 0;
					}
				}
				else if (serverItem.idType == ServerItem.IdType.CHARA)
				{
					ServerCharacterState serverCharacterState = playerState.CharacterStateByItemID((int)serverItem.id);
					serverChaoState.Id = (int)serverItem.id;
					serverChaoState.Status = ServerChaoState.ChaoStatus.MaxLevel;
					serverChaoState.Level = 0;
					serverChaoState.Rarity = 100;
					if (serverCharacterState == null)
					{
						serverChaoState.Status = ServerChaoState.ChaoStatus.NotOwned;
					}
					else if (serverCharacterState.Id < 0 || !serverCharacterState.IsUnlocked)
					{
						serverChaoState.Status = ServerChaoState.ChaoStatus.NotOwned;
					}
				}
			}
		}
		return serverChaoState;
	}

	public bool IsItemList()
	{
		return this.m_itemList != null;
	}

	public void ResetItemList()
	{
		if (this.m_itemList != null)
		{
			this.m_itemList.Clear();
		}
		this.m_itemList = null;
	}

	public void AddItemList(ServerItemState item)
	{
		if (this.m_itemList == null)
		{
			this.m_itemList = new Dictionary<int, ServerItemState>();
		}
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"ServerWheelOptions AddItemList id:",
			item.m_itemId,
			"  num:",
			item.m_num,
			"  !!!!!!!!!!!!!!!!!!!!!!!!!"
		}));
		if (this.m_itemList.ContainsKey(item.m_itemId))
		{
			this.m_itemList[item.m_itemId].m_num += item.m_num;
		}
		else
		{
			this.m_itemList.Add(item.m_itemId, item);
		}
	}

	public void CopyTo(ServerWheelOptions to)
	{
		to.m_nextSpinCost = this.m_nextSpinCost;
		to.m_itemWon = this.m_itemWon;
		to.m_items = (this.m_items.Clone() as int[]);
		to.m_itemQuantities = (this.m_itemQuantities.Clone() as int[]);
		to.m_itemWeight = (this.m_itemWeight.Clone() as int[]);
		to.m_spinCost = this.m_spinCost;
		to.m_rouletteRank = this.m_rouletteRank;
		to.m_numRouletteToken = this.m_numRouletteToken;
		to.m_numJackpotRing = this.m_numJackpotRing;
		to.m_numRemaining = this.m_numRemaining;
		to.m_nextFreeSpin = this.m_nextFreeSpin;
		to.ResetItemList();
		if (this.m_itemList != null && this.m_itemList.Count > 0)
		{
			Dictionary<int, ServerItemState>.KeyCollection keys = this.m_itemList.Keys;
			foreach (int current in keys)
			{
				to.AddItemList(this.m_itemList[current]);
			}
		}
	}
}
