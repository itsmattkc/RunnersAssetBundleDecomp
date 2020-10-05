using System;
using System.Runtime.CompilerServices;

public class MileageMapData : IComparable
{
	private ScenarioData _scenario_k__BackingField;

	private LoadingData _loading_k__BackingField;

	private MapData _map_data_k__BackingField;

	private EventData _event_data_k__BackingField;

	private WindowEventData[] _window_data_k__BackingField;

	public ScenarioData scenario
	{
		get;
		set;
	}

	public LoadingData loading
	{
		get;
		set;
	}

	public MapData map_data
	{
		get;
		set;
	}

	public EventData event_data
	{
		get;
		set;
	}

	public WindowEventData[] window_data
	{
		get;
		set;
	}

	public MileageMapData()
	{
	}

	public MileageMapData(ScenarioData scenario, LoadingData loading, MapData map_data, EventData event_data, WindowEventData[] window_data)
	{
		this.scenario = scenario;
		this.loading = loading;
		this.map_data = map_data;
		this.event_data = event_data;
		this.window_data = window_data;
	}

	public int CompareTo(object obj)
	{
		if (this == (MileageMapData)obj)
		{
			return 0;
		}
		return -1;
	}
}
