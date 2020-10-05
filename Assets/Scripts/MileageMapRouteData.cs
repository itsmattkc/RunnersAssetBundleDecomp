using System;
using System.Runtime.CompilerServices;

public class MileageMapRouteData : IComparable
{
	private int _id_k__BackingField;

	private MileageBonus _ability_type_k__BackingField;

	private float _ability_value_k__BackingField;

	private int _effect_flag_k__BackingField;

	private string _texture_name_k__BackingField;

	public int id
	{
		get;
		set;
	}

	public MileageBonus ability_type
	{
		get;
		set;
	}

	public float ability_value
	{
		get;
		set;
	}

	public int effect_flag
	{
		get;
		set;
	}

	public string texture_name
	{
		get;
		set;
	}

	public int CompareTo(object obj)
	{
		if (this == (MileageMapRouteData)obj)
		{
			return 0;
		}
		return -1;
	}
}
