using System;
using System.Runtime.CompilerServices;

public class MileageMapBGData : IComparable
{
	private int _id_k__BackingField;

	private string _texture_name_k__BackingField;

	private string _window_texture_name_k__BackingField;

	public int id
	{
		get;
		set;
	}

	public string texture_name
	{
		get;
		set;
	}

	public string window_texture_name
	{
		get;
		set;
	}

	public int CompareTo(object obj)
	{
		if (this == (MileageMapBGData)obj)
		{
			return 0;
		}
		return -1;
	}
}
