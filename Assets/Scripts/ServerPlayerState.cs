using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ServerPlayerState
{
	public enum CHARA_SORT
	{
		NONE,
		CHARA_ATTR,
		TEAM_ATTR
	}

	public long m_highScore;

	public int m_multiplier;

	public int m_numContinuesUsed;

	public int m_currentMissionSet;

	public bool[] m_missionsComplete;

	public long m_totalHighScore;

	public long m_totalHighScoreQuick;

	public long m_totalDistance;

	public long m_maxDistance;

	public int m_leagueIndex;

	public int m_leagueIndexQuick;

	public int m_numRings;

	public int m_numFreeRings;

	public int m_numBuyRings;

	public int m_numRedRings;

	public int m_numFreeRedRings;

	public int m_numBuyRedRings;

	public int m_numEnergy;

	public int m_numFreeEnergy;

	public int m_numBuyEnergy;

	public DateTime m_energyRenewsAt;

	public DateTime m_nextWeeklyLeaderboard;

	public DateTime m_endDailyMissionDate;

	public int m_numUnreadMessages;

	public int m_dailyMissionId;

	public int m_dailyMissionValue;

	public bool m_dailyChallengeComplete;

	public int m_numDailyChalCont;

	public int m_mainCharaId;

	public int m_subCharaId;

	public bool m_useSubCharacter;

	public int m_mainChaoId;

	public int m_subChaoId;

	public int[] m_equipItemList = new int[3];

	public int m_numPlaying;

	public int m_numAnimals;

	public int m_numRank;

	private ServerCharacterState[] m_characterState;

	private List<ServerChaoState> m_chaoState;

	private Dictionary<int, ServerItemState> m_itemStates;

	private ServerPlayCharacterState[] m_playCharacterState;

	private static Converter<bool, string> __f__am_cache29;

	public int unlockedCharacterNum
	{
		get
		{
			int num = 0;
			if (this.m_characterState != null && this.m_characterState.Length > 0)
			{
				ServerCharacterState[] characterState = this.m_characterState;
				for (int i = 0; i < characterState.Length; i++)
				{
					ServerCharacterState serverCharacterState = characterState[i];
					if (serverCharacterState != null && serverCharacterState.IsUnlocked)
					{
						num++;
					}
				}
			}
			return num;
		}
	}

	public List<ServerChaoState> ChaoStates
	{
		get
		{
			return this.m_chaoState;
		}
	}

	public ServerPlayerState()
	{
		this.m_highScore = 1234L;
		this.m_multiplier = 1;
		this.m_numContinuesUsed = 0;
		this.m_currentMissionSet = 0;
		this.m_missionsComplete = new bool[3];
		for (int i = 0; i < 3; i++)
		{
			this.m_missionsComplete[i] = false;
		}
		this.m_totalHighScoreQuick = 0L;
		this.m_totalHighScore = 0L;
		this.m_totalDistance = 0L;
		this.m_maxDistance = 0L;
		this.m_leagueIndex = 0;
		this.m_leagueIndexQuick = 0;
		this.m_characterState = new ServerCharacterState[29];
		for (int j = 0; j < 29; j++)
		{
			this.m_characterState[j] = new ServerCharacterState();
		}
		this.m_characterState[0].m_tokenCost = 0;
		this.m_characterState[0].m_numTokens = 0;
		this.m_characterState[0].Status = ServerCharacterState.CharacterStatus.Unlocked;
		this.m_characterState[1].m_tokenCost = 20;
		this.m_characterState[2].m_tokenCost = 30;
		this.m_chaoState = new List<ServerChaoState>();
		this.m_playCharacterState = new ServerPlayCharacterState[29];
		for (int k = 0; k < this.m_equipItemList.Length; k++)
		{
			this.m_equipItemList[k] = -1;
		}
		this.m_itemStates = new Dictionary<int, ServerItemState>();
		this.m_numRings = 0;
		this.m_numFreeRings = 0;
		this.m_numBuyRings = 0;
		this.m_numRedRings = 0;
		this.m_numFreeRedRings = 0;
		this.m_numBuyRedRings = 0;
		this.m_numEnergy = 0;
		this.m_numFreeEnergy = 0;
		this.m_numBuyEnergy = 0;
		this.m_numUnreadMessages = 0;
		this.m_dailyMissionId = 0;
		this.m_dailyMissionValue = 0;
		this.m_dailyChallengeComplete = false;
		this.m_numDailyChalCont = 0;
		this.m_mainChaoId = -1;
		this.m_subChaoId = -1;
		this.m_energyRenewsAt = DateTime.Now;
		this.m_nextWeeklyLeaderboard = DateTime.Now + new TimeSpan(7, 0, 0, 0);
		this.m_endDailyMissionDate = DateTime.Now;
	}

	public ServerCharacterState CharacterState(CharaType type)
	{
		if (type == CharaType.UNKNOWN)
		{
			return null;
		}
		if (type >= CharaType.NUM)
		{
			return null;
		}
		return this.m_characterState[(int)type];
	}

	public void ClearCharacterState()
	{
		for (int i = 0; i < 29; i++)
		{
			this.m_characterState[i] = new ServerCharacterState();
		}
	}

	public ServerCharacterState CharacterStateByItemID(int itemID)
	{
		ServerItem serverItem = new ServerItem((ServerItem.Id)itemID);
		return this.CharacterState(serverItem.charaType);
	}

	public ServerPlayCharacterState PlayCharacterState(CharaType type)
	{
		if (type == CharaType.UNKNOWN)
		{
			return null;
		}
		if (type >= CharaType.NUM)
		{
			return null;
		}
		return this.m_playCharacterState[(int)type];
	}

	public void SetCharacterState(ServerCharacterState characterState)
	{
		if (characterState == null)
		{
			return;
		}
		ServerItem serverItem = new ServerItem((ServerItem.Id)characterState.Id);
		CharaType charaType = serverItem.charaType;
		if (charaType == CharaType.UNKNOWN)
		{
			return;
		}
		int num = (int)charaType;
		if (num >= 29)
		{
			return;
		}
		ServerCharacterState serverCharacterState = this.m_characterState[num];
		ServerItem serverItem2 = new ServerItem((ServerItem.Id)serverCharacterState.Id);
		CharaType charaType2 = serverItem2.charaType;
		if (charaType2 != CharaType.UNKNOWN)
		{
			characterState.OldStatus = serverCharacterState.Status;
			characterState.OldCost = serverCharacterState.Cost;
			characterState.OldExp = serverCharacterState.Exp;
			characterState.OldAbiltyLevel.Clear();
			foreach (int current in serverCharacterState.AbilityLevel)
			{
				characterState.OldAbiltyLevel.Add(current);
			}
		}
		this.m_characterState[num] = characterState;
		NetUtil.SyncSaveDataAndDataBase(this.m_characterState);
	}

	public void SetCharacterState(ServerCharacterState[] characterStates)
	{
		if (characterStates == null)
		{
			return;
		}
		for (int i = 0; i < characterStates.Length; i++)
		{
			ServerCharacterState serverCharacterState = characterStates[i];
			if (serverCharacterState != null)
			{
				this.SetCharacterState(serverCharacterState);
			}
		}
	}

	public void SetPlayCharacterState(ServerPlayCharacterState playCharacterState)
	{
		if (playCharacterState == null)
		{
			return;
		}
		ServerItem serverItem = new ServerItem((ServerItem.Id)playCharacterState.Id);
		CharaType charaType = serverItem.charaType;
		if (charaType == CharaType.UNKNOWN)
		{
			return;
		}
		int num = (int)charaType;
		if (num >= 29)
		{
			return;
		}
		ServerCharacterState serverCharacterState = this.m_characterState[num];
		if (serverCharacterState != null)
		{
			serverCharacterState.OldAbiltyLevel.Clear();
			foreach (int current in serverCharacterState.AbilityLevel)
			{
				serverCharacterState.OldAbiltyLevel.Add(current);
			}
			serverCharacterState.OldStatus = serverCharacterState.Status;
			serverCharacterState.OldCost = serverCharacterState.Cost;
			serverCharacterState.OldExp = serverCharacterState.Exp;
			serverCharacterState.AbilityLevel.Clear();
			foreach (int current2 in playCharacterState.AbilityLevel)
			{
				serverCharacterState.AbilityLevel.Add(current2);
			}
			serverCharacterState.Level = playCharacterState.Level;
			serverCharacterState.Cost = playCharacterState.Cost;
			serverCharacterState.Exp = playCharacterState.Exp;
			serverCharacterState.NumRedRings = playCharacterState.NumRedRings;
			this.m_characterState[num] = serverCharacterState;
			NetUtil.SyncSaveDataAndDataBase(this.m_characterState);
		}
		this.m_playCharacterState[num] = playCharacterState;
	}

	public void ClearPlayCharacterState()
	{
		for (int i = 0; i < 29; i++)
		{
			this.m_playCharacterState[i] = null;
		}
	}

	public ServerChaoState ChaoStateByItemID(int itemID)
	{
		if (this.m_chaoState != null)
		{
			int count = this.m_chaoState.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.m_chaoState[i].Id == itemID)
				{
					return this.m_chaoState[i];
				}
			}
		}
		return null;
	}

	public ServerChaoState ChaoStateByArrayIndex(int index)
	{
		if (this.m_chaoState != null && index < this.m_chaoState.Count)
		{
			return this.m_chaoState[index];
		}
		return null;
	}

	public void SetChaoState(List<ServerChaoState> newChaoStateList)
	{
		if (this.m_chaoState == null)
		{
			return;
		}
		foreach (ServerChaoState current in newChaoStateList)
		{
			if (current != null)
			{
				bool flag = false;
				for (int i = 0; i < this.m_chaoState.Count; i++)
				{
					if (this.m_chaoState[i].Id == current.Id)
					{
						this.m_chaoState[i] = current;
						flag = true;
					}
				}
				if (!flag)
				{
					this.m_chaoState.Add(current);
				}
			}
		}
		NetUtil.SyncSaveDataAndDataBase(this.m_chaoState);
	}

	public ServerItemState GetItemStateByType(ServerConstants.RunModifierType type)
	{
		int id = 0;
		switch (type)
		{
		case ServerConstants.RunModifierType.SpringBonus:
			id = 3;
			break;
		case ServerConstants.RunModifierType.RingStreak:
			id = 2;
			break;
		case ServerConstants.RunModifierType.EnemyCombo:
			id = 4;
			break;
		}
		return this.GetItemStateById(id);
	}

	public ServerItemState GetItemStateById(int id)
	{
		if (this.m_itemStates.ContainsKey(id))
		{
			return this.m_itemStates[id];
		}
		return null;
	}

	public int GetNumItemByType(ServerConstants.RunModifierType type)
	{
		ServerItemState itemStateByType = this.GetItemStateByType(type);
		if (itemStateByType != null)
		{
			return itemStateByType.m_num;
		}
		return 0;
	}

	public int GetNumItemById(int id)
	{
		ServerItemState itemStateById = this.GetItemStateById(id);
		if (itemStateById != null)
		{
			return itemStateById.m_num;
		}
		return 0;
	}

	public void AddItemState(ServerItemState itemState)
	{
		if (this.m_itemStates.ContainsKey(itemState.m_itemId))
		{
			this.m_itemStates[itemState.m_itemId].m_num += itemState.m_num;
		}
		else
		{
			this.m_itemStates.Add(itemState.m_itemId, itemState);
		}
	}

	public void Dump()
	{
		string arg = string.Join(":", Array.ConvertAll<bool, string>(this.m_missionsComplete, (bool item) => item.ToString()));
		UnityEngine.Debug.Log(string.Format("highScore={0}, multiplier={1}, numRings={2}, numRedRings={3}, energy={4}, energyRenewsAt={5}", new object[]
		{
			this.m_highScore,
			this.m_multiplier,
			this.m_numRings,
			this.m_numRedRings,
			this.m_numEnergy,
			this.m_energyRenewsAt
		}));
		UnityEngine.Debug.Log(string.Format("currentMissionSet={0}, missions={1}, numContinuesUsed={2}", this.m_currentMissionSet, arg, this.m_numContinuesUsed));
		for (int i = 0; i < 29; i++)
		{
			this.m_characterState[i].Dump();
		}
	}

	public void RefreshFakeState()
	{
	}

	public void CopyTo(ServerPlayerState to)
	{
		to.m_highScore = this.m_highScore;
		to.m_numContinuesUsed = this.m_numContinuesUsed;
		to.m_multiplier = this.m_multiplier;
		to.m_currentMissionSet = this.m_currentMissionSet;
		to.m_missionsComplete = (this.m_missionsComplete.Clone() as bool[]);
		to.m_totalHighScoreQuick = this.m_totalHighScoreQuick;
		to.m_totalHighScore = this.m_totalHighScore;
		to.m_totalDistance = this.m_totalDistance;
		to.m_maxDistance = this.m_maxDistance;
		to.m_leagueIndex = this.m_leagueIndex;
		to.m_leagueIndexQuick = this.m_leagueIndexQuick;
		to.m_numRings = this.m_numRings;
		to.m_numFreeRings = this.m_numFreeRings;
		to.m_numBuyRings = this.m_numBuyRings;
		to.m_numRedRings = this.m_numRedRings;
		to.m_numFreeRedRings = this.m_numFreeRedRings;
		to.m_numBuyRedRings = this.m_numBuyRedRings;
		to.m_numEnergy = this.m_numEnergy;
		to.m_numFreeEnergy = this.m_numFreeEnergy;
		to.m_numBuyEnergy = this.m_numBuyEnergy;
		to.m_energyRenewsAt = this.m_energyRenewsAt;
		to.m_numUnreadMessages = this.m_numUnreadMessages;
		to.m_dailyMissionId = this.m_dailyMissionId;
		to.m_dailyMissionValue = this.m_dailyMissionValue;
		to.m_dailyChallengeComplete = this.m_dailyChallengeComplete;
		to.m_numDailyChalCont = this.m_numDailyChalCont;
		to.m_nextWeeklyLeaderboard = this.m_nextWeeklyLeaderboard;
		to.m_endDailyMissionDate = this.m_endDailyMissionDate;
		to.m_mainChaoId = this.m_mainChaoId;
		to.m_mainCharaId = this.m_mainCharaId;
		to.m_subCharaId = this.m_subCharaId;
		to.m_useSubCharacter = this.m_useSubCharacter;
		to.m_subChaoId = this.m_subChaoId;
		to.m_numPlaying = this.m_numPlaying;
		to.m_numAnimals = this.m_numAnimals;
		to.m_numRank = this.m_numRank;
		to.m_itemStates.Clear();
		foreach (ServerItemState current in this.m_itemStates.Values)
		{
			to.m_itemStates.Add(current.m_itemId, current);
		}
		for (int i = 0; i < this.m_equipItemList.Length; i++)
		{
			to.m_equipItemList[i] = this.m_equipItemList[i];
		}
		ServerCharacterState[] characterState = this.m_characterState;
		for (int j = 0; j < characterState.Length; j++)
		{
			ServerCharacterState serverCharacterState = characterState[j];
			if (serverCharacterState != null)
			{
				if (serverCharacterState.Id >= 0)
				{
					to.SetCharacterState(serverCharacterState);
				}
			}
		}
		to.SetChaoState(this.m_chaoState);
		NetUtil.SyncSaveDataAndDataBase(this);
	}

	private void SetChaoState(ServerChaoState srcState, ref ServerChaoState dstState)
	{
		dstState.IsLvUp = (0 != dstState.Id & (dstState.IsLvUp | dstState.Level < srcState.Level));
		dstState.IsNew = (0 != dstState.Id & (dstState.IsNew | (dstState.Status == ServerChaoState.ChaoStatus.NotOwned && srcState.Status != ServerChaoState.ChaoStatus.NotOwned)));
		dstState.Id = srcState.Id;
		dstState.Level = srcState.Level;
		dstState.Rarity = srcState.Rarity;
		dstState.Status = srcState.Status;
		dstState.Dealing = srcState.Dealing;
		dstState.NumInvite = srcState.NumInvite;
		dstState.NumAcquired = srcState.NumAcquired;
		dstState.Hidden = srcState.Hidden;
		if (dstState.IsNew)
		{
			UnityEngine.Debug.Log("requrired new chao. id : " + dstState.Id);
		}
		if (dstState.IsLvUp)
		{
			UnityEngine.Debug.Log("chao level up. id : " + dstState.Id);
		}
	}

	public List<CharaType> GetCharacterTypeList(ServerPlayerState.CHARA_SORT sort = ServerPlayerState.CHARA_SORT.NONE, bool descending = false, int offset = 0)
	{
		List<CharaType> list = null;
		Dictionary<CharaType, ServerCharacterState> characterStateList = this.GetCharacterStateList(sort, descending, offset);
		if (characterStateList != null && characterStateList.Count > 0)
		{
			list = new List<CharaType>();
			Dictionary<CharaType, ServerCharacterState>.KeyCollection keys = characterStateList.Keys;
			foreach (CharaType current in keys)
			{
				list.Add(current);
			}
		}
		return list;
	}

	public Dictionary<CharaType, ServerCharacterState> GetCharacterStateList(ServerPlayerState.CHARA_SORT sort = ServerPlayerState.CHARA_SORT.NONE, bool descending = false, int offset = 0)
	{
		Dictionary<CharaType, ServerCharacterState> dictionary = new Dictionary<CharaType, ServerCharacterState>();
		Dictionary<CharaType, ServerCharacterState> dictionary2 = null;
		if (this.m_characterState != null && this.m_characterState.Length > 0)
		{
			ServerCharacterState[] characterState = this.m_characterState;
			for (int i = 0; i < characterState.Length; i++)
			{
				ServerCharacterState serverCharacterState = characterState[i];
				CharaType charaType = serverCharacterState.charaType;
				if (charaType != CharaType.UNKNOWN)
				{
					if (sort == ServerPlayerState.CHARA_SORT.NONE)
					{
						dictionary.Add(charaType, serverCharacterState);
					}
					else
					{
						if (dictionary2 == null)
						{
							dictionary2 = new Dictionary<CharaType, ServerCharacterState>();
						}
						dictionary2.Add(charaType, serverCharacterState);
					}
				}
			}
		}
		if (sort != ServerPlayerState.CHARA_SORT.NONE && dictionary2 != null)
		{
			if (sort != ServerPlayerState.CHARA_SORT.CHARA_ATTR)
			{
				if (sort != ServerPlayerState.CHARA_SORT.TEAM_ATTR)
				{
					dictionary = dictionary2;
				}
				else
				{
					this.GetCharacterStateListTeamAttr(ref dictionary, dictionary2, descending, offset);
				}
			}
			else
			{
				this.GetCharacterStateListCharaAttr(ref dictionary, dictionary2, descending, offset);
			}
		}
		return dictionary;
	}

	private void GetCharacterStateListCharaAttr(ref Dictionary<CharaType, ServerCharacterState> outList, Dictionary<CharaType, ServerCharacterState> orgList, bool descending, int offset)
	{
		Dictionary<CharacterAttribute, List<ServerCharacterState>> dictionary = new Dictionary<CharacterAttribute, List<ServerCharacterState>>();
		CharacterDataNameInfo instance = CharacterDataNameInfo.Instance;
		if (instance != null)
		{
			Dictionary<CharaType, ServerCharacterState>.KeyCollection keys = orgList.Keys;
			foreach (CharaType current in keys)
			{
				CharacterDataNameInfo.Info dataByID = instance.GetDataByID(current);
				if (dictionary.ContainsKey(dataByID.m_attribute))
				{
					dictionary[dataByID.m_attribute].Add(orgList[current]);
				}
				else
				{
					List<ServerCharacterState> list = new List<ServerCharacterState>();
					list.Add(orgList[current]);
					dictionary.Add(dataByID.m_attribute, list);
				}
			}
		}
		if (dictionary.Count > 0 && outList != null)
		{
			int num = 3;
			for (int i = 0; i < num; i++)
			{
				CharacterAttribute key;
				if (descending)
				{
					key = (CharacterAttribute)((offset - i + num) % num);
				}
				else
				{
					key = (CharacterAttribute)((offset + i) % num);
				}
				if (dictionary.ContainsKey(key))
				{
					foreach (ServerCharacterState current2 in dictionary[key])
					{
						outList.Add(current2.charaType, current2);
					}
				}
			}
		}
	}

	private void GetCharacterStateListTeamAttr(ref Dictionary<CharaType, ServerCharacterState> outList, Dictionary<CharaType, ServerCharacterState> orgList, bool descending, int offset)
	{
		Dictionary<TeamAttribute, List<ServerCharacterState>> dictionary = new Dictionary<TeamAttribute, List<ServerCharacterState>>();
		CharacterDataNameInfo instance = CharacterDataNameInfo.Instance;
		if (instance != null)
		{
			Dictionary<CharaType, ServerCharacterState>.KeyCollection keys = orgList.Keys;
			foreach (CharaType current in keys)
			{
				CharacterDataNameInfo.Info dataByID = instance.GetDataByID(current);
				if (dictionary.ContainsKey(dataByID.m_teamAttribute))
				{
					dictionary[dataByID.m_teamAttribute].Add(orgList[current]);
				}
				else
				{
					List<ServerCharacterState> list = new List<ServerCharacterState>();
					list.Add(orgList[current]);
					dictionary.Add(dataByID.m_teamAttribute, list);
				}
			}
		}
		if (dictionary.Count > 0 && outList != null)
		{
			int num = 8;
			for (int i = 0; i < num; i++)
			{
				TeamAttribute key;
				if (descending)
				{
					key = (TeamAttribute)((offset - i + num) % num);
				}
				else
				{
					key = (TeamAttribute)((offset + i) % num);
				}
				if (dictionary.ContainsKey(key))
				{
					foreach (ServerCharacterState current2 in dictionary[key])
					{
						outList.Add(current2.charaType, current2);
					}
				}
			}
		}
	}
}
