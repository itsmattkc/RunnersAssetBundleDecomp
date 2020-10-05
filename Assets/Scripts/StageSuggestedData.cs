using System;
using System.Runtime.CompilerServices;

public class StageSuggestedData : IComparable
{
	private int _id_k__BackingField;

	private CharacterAttribute[] _charaAttribute_k__BackingField;

	public int id
	{
		get;
		set;
	}

	public CharacterAttribute[] charaAttribute
	{
		get;
		set;
	}

	public int CompareTo(object obj)
	{
		if (this == (StageSuggestedData)obj)
		{
			return 0;
		}
		return -1;
	}
}
