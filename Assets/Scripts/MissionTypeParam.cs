using Mission;
using System;

public class MissionTypeParam
{
	public EventID m_eventID;

	public MissionCategory m_category;

	public MissionTypeParam(EventID eventID, MissionCategory category)
	{
		this.m_eventID = eventID;
		this.m_category = category;
	}
}
