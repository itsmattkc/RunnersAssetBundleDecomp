using System;
using System.Runtime.CompilerServices;

public class ServerWheelSpinInfo
{
	private int _id_k__BackingField;

	private DateTime _start_k__BackingField;

	private DateTime _end_k__BackingField;

	private string _param_k__BackingField;

	public int id
	{
		get;
		set;
	}

	public DateTime start
	{
		get;
		set;
	}

	public DateTime end
	{
		get;
		set;
	}

	public string param
	{
		get;
		set;
	}

	public bool isEnabled
	{
		get
		{
			bool result = false;
			if (NetBase.GetCurrentTime() >= this.start && NetBase.GetCurrentTime() < this.end)
			{
				result = true;
			}
			return result;
		}
	}

	public ServerWheelSpinInfo()
	{
		this.id = 1;
		this.start = NetBase.GetCurrentTime();
		this.end = NetBase.GetCurrentTime();
		this.param = string.Empty;
	}

	public void Dump()
	{
	}

	public void CopyTo(ServerWheelSpinInfo to)
	{
		to.id = this.id;
		to.start = this.start;
		to.end = this.end;
		to.param = this.param;
	}
}
