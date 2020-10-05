using LitJson;
using System;
using System.Collections.Generic;

public class NetServerGetEventRaidBossDesiredList : NetBase
{
	private int m_eventId;

	private long m_raidBossId;

	private List<string> m_friendIdList = new List<string>();

	private List<ServerEventRaidBossDesiredState> m_desiredList;

	public List<ServerEventRaidBossDesiredState> DesiredList
	{
		get
		{
			return this.m_desiredList;
		}
	}

	public NetServerGetEventRaidBossDesiredList(int eventId, long raidBossId, List<string> friendIdList)
	{
		this.m_eventId = eventId;
		this.m_raidBossId = raidBossId;
		if (friendIdList != null)
		{
			foreach (string current in friendIdList)
			{
				this.m_friendIdList.Add(current);
			}
		}
	}

	protected override void DoRequest()
	{
		base.SetAction("Event/getEventRaidbossDesiredList");
		base.WriteActionParamValue("raidbossId", this.m_raidBossId);
		base.WriteActionParamValue("eventId", this.m_eventId);
		List<object> list = new List<object>();
		foreach (string current in this.m_friendIdList)
		{
			if (!string.IsNullOrEmpty(current))
			{
				list.Add(current);
			}
		}
		base.WriteActionParamArray("friendIdList", list);
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.m_desiredList = NetUtil.AnalyzeEventRaidbossDesiredList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}
}
