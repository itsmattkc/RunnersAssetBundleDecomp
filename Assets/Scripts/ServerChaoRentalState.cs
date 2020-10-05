using System;
using System.Runtime.CompilerServices;

public class ServerChaoRentalState
{
	private string _FriendId_k__BackingField;

	private string _Name_k__BackingField;

	private string _Url_k__BackingField;

	private ServerChaoData _ChaoData_k__BackingField;

	private int _RentalState_k__BackingField;

	private long _NextRentalAt_k__BackingField;

	private float _TimeSinceStartup_k__BackingField;

	public string FriendId
	{
		get;
		set;
	}

	public string Name
	{
		get;
		set;
	}

	public string Url
	{
		get;
		set;
	}

	public ServerChaoData ChaoData
	{
		get;
		set;
	}

	public int RentalState
	{
		get;
		set;
	}

	public long NextRentalAt
	{
		get;
		set;
	}

	public bool IsRented
	{
		get
		{
			return 1 == this.RentalState;
		}
	}

	public bool IsRentalable
	{
		get
		{
			return 0L == this.NextRentalAt;
		}
	}

	public float TimeSinceStartup
	{
		get;
		set;
	}

	public ServerChaoRentalState()
	{
		this.FriendId = string.Empty;
		this.Name = string.Empty;
		this.Url = string.Empty;
		this.ChaoData = null;
		this.RentalState = 0;
		this.NextRentalAt = 0L;
		this.TimeSinceStartup = 0f;
	}

	public void Dump()
	{
	}
}
