using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ServerPlayCharacterState
{
	public enum CharacterStatus
	{
		Locked,
		Unlocked,
		MaxLevel
	}

	public enum LockCondition
	{
		OPEN,
		MILEAGE_EPISODE,
		RING_OR_RED_STAR_RING
	}

	public List<int> AbilityLevel = new List<int>();

	public List<int> AbilityNumRings = new List<int>();

	public List<int> abilityLevelUp = new List<int>();

	public List<int> abilityLevelUpExp = new List<int>();

	private int _Id_k__BackingField;

	private ServerPlayCharacterState.CharacterStatus _Status_k__BackingField;

	private int _Level_k__BackingField;

	private int _Cost_k__BackingField;

	private int _NumRedRings_k__BackingField;

	private ServerPlayCharacterState.LockCondition _Condition_k__BackingField;

	private int _Exp_k__BackingField;

	private int _star_k__BackingField;

	private int _starMax_k__BackingField;

	private int _priceNumRings_k__BackingField;

	private int _priceNumRedRings_k__BackingField;

	public int Id
	{
		get;
		set;
	}

	public ServerPlayCharacterState.CharacterStatus Status
	{
		get;
		set;
	}

	public int Level
	{
		get;
		set;
	}

	public int Cost
	{
		get;
		set;
	}

	public int NumRedRings
	{
		get;
		set;
	}

	public ServerPlayCharacterState.LockCondition Condition
	{
		get;
		set;
	}

	public int Exp
	{
		get;
		set;
	}

	public int star
	{
		get;
		set;
	}

	public int starMax
	{
		get;
		set;
	}

	public int priceNumRings
	{
		get;
		set;
	}

	public int priceNumRedRings
	{
		get;
		set;
	}

	public int UnlockCost
	{
		get
		{
			if (this.Status == ServerPlayCharacterState.CharacterStatus.Locked)
			{
				return this.Cost;
			}
			return -1;
		}
	}

	public int LevelUpCost
	{
		get
		{
			if (this.Status == ServerPlayCharacterState.CharacterStatus.Unlocked)
			{
				return this.Cost;
			}
			return -1;
		}
	}

	public bool IsUnlocked
	{
		get
		{
			return ServerPlayCharacterState.CharacterStatus.Locked != this.Status;
		}
	}

	public bool IsMaxLevel
	{
		get
		{
			return ServerPlayCharacterState.CharacterStatus.MaxLevel == this.Status;
		}
	}

	public float QuickModeTimeExtension
	{
		get
		{
			float result = 0f;
			if (this.starMax > 0)
			{
				StageTimeTable stageTimeTable = GameObjectUtil.FindGameObjectComponent<StageTimeTable>("StageTimeTable");
				if (stageTimeTable != null)
				{
					float num = (float)stageTimeTable.GetTableItemData(StageTimeTableItem.OverlapBonus);
					result = (float)this.star * num;
				}
			}
			return result;
		}
	}

	public ServerPlayCharacterState()
	{
		this.Id = -1;
		this.Status = ServerPlayCharacterState.CharacterStatus.Locked;
		this.Level = 10;
		this.Cost = 0;
		this.star = 0;
		this.starMax = 0;
		this.priceNumRings = 0;
		this.priceNumRedRings = 0;
	}

	public static ServerCharacterState CreateCharacterState(ServerPlayCharacterState playCharaState)
	{
		if (playCharaState == null)
		{
			return null;
		}
		ServerCharacterState serverCharacterState = new ServerCharacterState();
		serverCharacterState.Id = playCharaState.Id;
		serverCharacterState.Status = (ServerCharacterState.CharacterStatus)playCharaState.Status;
		serverCharacterState.Level = playCharaState.Level;
		serverCharacterState.Cost = playCharaState.Cost;
		serverCharacterState.NumRedRings = playCharaState.NumRedRings;
		serverCharacterState.star = playCharaState.star;
		serverCharacterState.starMax = playCharaState.starMax;
		serverCharacterState.priceNumRings = playCharaState.priceNumRings;
		serverCharacterState.priceNumRedRings = playCharaState.priceNumRedRings;
		foreach (int current in playCharaState.AbilityLevel)
		{
			serverCharacterState.AbilityLevel.Add(current);
		}
		foreach (int current2 in playCharaState.AbilityNumRings)
		{
			serverCharacterState.AbilityNumRings.Add(current2);
		}
		serverCharacterState.Condition = (ServerCharacterState.LockCondition)playCharaState.Condition;
		serverCharacterState.Exp = playCharaState.Exp;
		return serverCharacterState;
	}

	public static ServerPlayCharacterState CreatePlayCharacterState(ServerCharacterState charaState)
	{
		if (charaState == null)
		{
			return null;
		}
		ServerPlayCharacterState serverPlayCharacterState = new ServerPlayCharacterState();
		serverPlayCharacterState.Id = charaState.Id;
		serverPlayCharacterState.Status = serverPlayCharacterState.Status;
		serverPlayCharacterState.Level = charaState.Level;
		serverPlayCharacterState.Cost = charaState.Cost;
		serverPlayCharacterState.NumRedRings = charaState.NumRedRings;
		serverPlayCharacterState.star = charaState.star;
		serverPlayCharacterState.starMax = charaState.starMax;
		serverPlayCharacterState.priceNumRings = charaState.priceNumRings;
		serverPlayCharacterState.priceNumRedRings = charaState.priceNumRedRings;
		foreach (int current in charaState.AbilityLevel)
		{
			serverPlayCharacterState.AbilityLevel.Add(current);
		}
		foreach (int current2 in charaState.AbilityNumRings)
		{
			serverPlayCharacterState.AbilityNumRings.Add(current2);
		}
		serverPlayCharacterState.Condition = (ServerPlayCharacterState.LockCondition)charaState.Condition;
		serverPlayCharacterState.Exp = charaState.Exp;
		return serverPlayCharacterState;
	}

	public void Dump()
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Id=",
			this.Id,
			", Status=",
			this.Status,
			", Level=",
			this.Level,
			", Cost=",
			this.Cost,
			", UnlockCost=",
			this.UnlockCost,
			", LevelUpCost=",
			this.LevelUpCost
		}));
	}
}
