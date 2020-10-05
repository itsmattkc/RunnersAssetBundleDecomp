using System;
using System.Runtime.CompilerServices;

public class ServerChaoState : ServerChaoData
{
	public enum ChaoStatus
	{
		NotOwned,
		Owned,
		MaxLevel
	}

	public enum ChaoDealing
	{
		None,
		Leader,
		Sub
	}

	private ServerChaoState.ChaoStatus _Status_k__BackingField;

	private ServerChaoState.ChaoDealing _Dealing_k__BackingField;

	private int _NumInvite_k__BackingField;

	private int _NumAcquired_k__BackingField;

	private bool _Hidden_k__BackingField;

	private bool _IsLvUp_k__BackingField;

	private bool _IsNew_k__BackingField;

	public ServerChaoState.ChaoStatus Status
	{
		get;
		set;
	}

	public ServerChaoState.ChaoDealing Dealing
	{
		get;
		set;
	}

	public int NumInvite
	{
		get;
		set;
	}

	public int NumAcquired
	{
		get;
		set;
	}

	public bool Hidden
	{
		get;
		set;
	}

	public bool IsLvUp
	{
		get;
		set;
	}

	public bool IsNew
	{
		get;
		set;
	}

	public bool IsInvite
	{
		get
		{
			return 0 < this.NumInvite;
		}
	}

	public bool IsOwned
	{
		get
		{
			return ServerChaoState.ChaoStatus.NotOwned != this.Status;
		}
	}

	public ServerChaoState()
	{
		this.Status = ServerChaoState.ChaoStatus.NotOwned;
		this.Dealing = ServerChaoState.ChaoDealing.None;
		this.NumInvite = 0;
		this.NumAcquired = 0;
		this.Hidden = false;
		this.IsLvUp = false;
		this.IsNew = false;
	}
}
