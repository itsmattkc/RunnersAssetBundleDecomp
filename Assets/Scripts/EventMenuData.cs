using System;
using System.Runtime.CompilerServices;

public class EventMenuData : IComparable
{
	private WindowEventData[] _window_data_k__BackingField;

	private EventStageData _stage_data_k__BackingField;

	private EventChaoData _chao_data_k__BackingField;

	private EventProductionData _puduction_data_k__BackingField;

	private EventRaidProductionData _raid_data_k__BackingField;

	private EventAvertData _advert_data_k__BackingField;

	public WindowEventData[] window_data
	{
		get;
		set;
	}

	public EventStageData stage_data
	{
		get;
		set;
	}

	public EventChaoData chao_data
	{
		get;
		set;
	}

	public EventProductionData puduction_data
	{
		get;
		set;
	}

	public EventRaidProductionData raid_data
	{
		get;
		set;
	}

	public EventAvertData advert_data
	{
		get;
		set;
	}

	public EventMenuData()
	{
	}

	public EventMenuData(WindowEventData[] window_data, EventStageData stage_data, EventChaoData chao_data, EventProductionData puduction_data, EventRaidProductionData raid_data, EventAvertData advert_data)
	{
		this.stage_data = stage_data;
		this.window_data = window_data;
		this.chao_data = chao_data;
		this.puduction_data = puduction_data;
		this.raid_data = raid_data;
		this.advert_data = advert_data;
	}

	public int CompareTo(object obj)
	{
		if (this == (EventMenuData)obj)
		{
			return 0;
		}
		return -1;
	}
}
