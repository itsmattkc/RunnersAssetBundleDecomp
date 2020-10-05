using System;
using System.Runtime.CompilerServices;

public class ServerMileageBonus
{
	public enum emBonusType
	{
		Score,
		Ring
	}

	private ServerMileageBonus.emBonusType _BonusType_k__BackingField;

	private ServerConstants.NumType _NumType_k__BackingField;

	private int _NumBonus_k__BackingField;

	public ServerMileageBonus.emBonusType BonusType
	{
		get;
		set;
	}

	public ServerConstants.NumType NumType
	{
		get;
		set;
	}

	public int NumBonus
	{
		get;
		set;
	}

	public ServerMileageBonus()
	{
		this.BonusType = ServerMileageBonus.emBonusType.Score;
		this.NumType = ServerConstants.NumType.Number;
		this.NumBonus = 0;
	}
}
