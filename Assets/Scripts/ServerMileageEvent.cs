using System;
using System.Runtime.CompilerServices;

public class ServerMileageEvent
{
	public enum emEventType
	{
		Incentive,
		BeginBonus,
		EndBonus,
		Goal
	}

	private int _Distance_k__BackingField;

	private ServerMileageEvent.emEventType _EventType_k__BackingField;

	private int _Content_k__BackingField;

	private ServerConstants.NumType _NumType_k__BackingField;

	private int _Num_k__BackingField;

	private int _Level_k__BackingField;

	public int Distance
	{
		get;
		set;
	}

	public ServerMileageEvent.emEventType EventType
	{
		get;
		set;
	}

	public int Content
	{
		get;
		set;
	}

	public ServerConstants.NumType NumType
	{
		get;
		set;
	}

	public int Num
	{
		get;
		set;
	}

	public int Level
	{
		get;
		set;
	}

	public ServerMileageEvent()
	{
		this.Distance = 0;
		this.EventType = ServerMileageEvent.emEventType.Incentive;
		this.Content = 0;
		this.NumType = ServerConstants.NumType.Number;
		this.Num = 0;
		this.Level = 0;
	}
}
