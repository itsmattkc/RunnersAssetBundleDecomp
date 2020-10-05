using System;
using System.Runtime.CompilerServices;

public class ServerChaoWheelOptions
{
	public enum ChaoSpinType
	{
		Normal,
		Special,
		NUM
	}

	private int m_multi;

	private int[] _Rarities_k__BackingField;

	private int[] _ItemWeight_k__BackingField;

	private int _Cost_k__BackingField;

	private ServerChaoWheelOptions.ChaoSpinType _SpinType_k__BackingField;

	private int _NumSpecialEggs_k__BackingField;

	private bool _IsValid_k__BackingField;

	private int _NumRouletteToken_k__BackingField;

	private bool _IsTutorial_k__BackingField;

	private bool _IsConnected_k__BackingField;

	public int[] Rarities
	{
		get;
		set;
	}

	public int[] ItemWeight
	{
		get;
		set;
	}

	public int Cost
	{
		get;
		set;
	}

	public ServerChaoWheelOptions.ChaoSpinType SpinType
	{
		get;
		set;
	}

	public int NumSpecialEggs
	{
		get;
		set;
	}

	public bool IsValid
	{
		get;
		set;
	}

	public int NumRouletteToken
	{
		get;
		set;
	}

	public bool IsTutorial
	{
		get;
		set;
	}

	public bool IsConnected
	{
		get;
		set;
	}

	public int multi
	{
		get
		{
			return this.m_multi;
		}
	}

	public ServerChaoWheelOptions()
	{
		this.Rarities = new int[8];
		this.ItemWeight = new int[8];
		for (int i = 0; i < 8; i++)
		{
			this.Rarities[i] = 0;
			this.ItemWeight[i] = 0;
		}
		this.m_multi = 1;
		this.Cost = 0;
		this.SpinType = ServerChaoWheelOptions.ChaoSpinType.Normal;
		this.NumSpecialEggs = 0;
		this.IsValid = true;
		this.NumRouletteToken = 0;
		this.IsTutorial = false;
		this.IsConnected = false;
	}

	public bool ChangeMulti(int multi)
	{
		bool flag = this.IsMulti(multi);
		if (flag)
		{
			this.m_multi = multi;
			if (this.m_multi < 1)
			{
				this.m_multi = 1;
			}
		}
		else
		{
			this.m_multi = 1;
		}
		return flag;
	}

	public bool IsMulti(int multi)
	{
		bool result = false;
		if (multi <= 1)
		{
			result = true;
		}
		else
		{
			int cost = this.Cost;
			int num;
			if (this.NumRouletteToken > 0 && this.NumRouletteToken >= this.Cost)
			{
				num = this.NumRouletteToken;
				cost = this.Cost;
			}
			else
			{
				num = (int)SaveDataManager.Instance.ItemData.RedRingCount;
				cost = this.Cost;
			}
			if (num >= cost * multi)
			{
				result = true;
			}
		}
		return result;
	}

	public void Dump()
	{
	}

	public void CopyTo(ServerChaoWheelOptions to)
	{
		to.Cost = this.Cost;
		to.SpinType = this.SpinType;
		to.Rarities = (this.Rarities.Clone() as int[]);
		to.ItemWeight = (this.ItemWeight.Clone() as int[]);
		to.NumSpecialEggs = this.NumSpecialEggs;
		to.IsValid = this.IsValid;
		to.NumRouletteToken = this.NumRouletteToken;
		to.IsTutorial = this.IsTutorial;
		to.IsConnected = this.IsConnected;
		to.m_multi = this.m_multi;
	}
}
