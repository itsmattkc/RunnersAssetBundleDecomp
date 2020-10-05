using System;
using System.Runtime.CompilerServices;

public class ShopItemData
{
	public const int ID_NONE = -1;

	public const int ID_ORIGIN = 0;

	private int _number_k__BackingField;

	private string _name_k__BackingField;

	private int _rings_k__BackingField;

	private string _details_k__BackingField;

	public int number
	{
		get;
		set;
	}

	public string name
	{
		get;
		private set;
	}

	public int rings
	{
		get;
		set;
	}

	public string details
	{
		get;
		private set;
	}

	public int id
	{
		get
		{
			return this.number - 1;
		}
	}

	public int index
	{
		get
		{
			return this.id;
		}
	}

	public bool IsValidate
	{
		get
		{
			return this.id != -1;
		}
	}

	public void SetName(string name)
	{
		this.name = name;
	}

	public void SetDetails(string details)
	{
		this.details = details;
	}
}
