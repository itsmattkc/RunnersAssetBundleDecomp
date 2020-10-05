using System;
using System.Runtime.CompilerServices;

public class ServerChaoData
{
	public enum RarityType
	{
		NORMAL,
		RARE,
		SRARE,
		PLAYER = 100,
		CAMPAIGN
	}

	private int _Id_k__BackingField;

	private int _Level_k__BackingField;

	private int _Rarity_k__BackingField;

	public int Id
	{
		get;
		set;
	}

	public int Level
	{
		get;
		set;
	}

	public int Rarity
	{
		get;
		set;
	}

	public ServerChaoData()
	{
		this.Id = 0;
		this.Level = 0;
		this.Rarity = -1;
	}

	public void Dump()
	{
	}
}
