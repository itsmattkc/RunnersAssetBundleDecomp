using System;
using System.Collections.Generic;

public class ServerCampaignState
{
	private Dictionary<Constants.Campaign.emType, List<ServerCampaignData>> m_dic;

	public ServerCampaignState()
	{
		this.m_dic = new Dictionary<Constants.Campaign.emType, List<ServerCampaignData>>();
	}

	public bool InSession(int id)
	{
		return this.InSession(id, 0);
	}

	public bool InSession(int id, int index)
	{
		ServerCampaignData campaign = this.GetCampaign(id, index);
		return campaign != null && campaign.InSession();
	}

	public bool InAnyIdSession(Constants.Campaign.emType campaignType)
	{
		ServerCampaignData anyIdCampaign = this.GetAnyIdCampaign(campaignType);
		return anyIdCampaign != null && anyIdCampaign.InSession();
	}

	public bool InSession(Constants.Campaign.emType campaignType)
	{
		return this.InSession(campaignType, -1);
	}

	public bool InSession(Constants.Campaign.emType campaignType, int id)
	{
		ServerCampaignData campaign = this.GetCampaign(campaignType, id);
		return campaign != null && campaign.InSession();
	}

	public ServerCampaignData GetCampaign(int id)
	{
		return this.GetCampaign(id, 0);
	}

	public ServerCampaignData GetCampaign(int id, int index)
	{
		int num = this.CampaignCount(id);
		if (0 > index || num <= index)
		{
			return null;
		}
		int num2 = 0;
		foreach (KeyValuePair<Constants.Campaign.emType, List<ServerCampaignData>> current in this.m_dic)
		{
			int count = current.Value.Count;
			int i = 0;
			while (i < count)
			{
				ServerCampaignData serverCampaignData = current.Value[i];
				if (serverCampaignData.id == id)
				{
					if (num2 == index)
					{
						return serverCampaignData;
					}
					num2++;
					break;
				}
				else
				{
					i++;
				}
			}
		}
		return null;
	}

	public ServerCampaignData GetAnyIdCampaign(Constants.Campaign.emType campaignType)
	{
		return this.GetCampaign(campaignType, -1);
	}

	public ServerCampaignData GetCampaign(Constants.Campaign.emType campaignType)
	{
		return this.GetCampaign(campaignType, -1);
	}

	public ServerCampaignData GetCampaign(Constants.Campaign.emType campaignType, int id)
	{
		List<ServerCampaignData> list = null;
		if (this.m_dic.TryGetValue(campaignType, out list))
		{
			int count = list.Count;
			if (0 < count)
			{
				for (int i = 0; i < count; i++)
				{
					ServerCampaignData serverCampaignData = list[i];
					if (serverCampaignData.id == id || id == -1)
					{
						return serverCampaignData;
					}
				}
			}
		}
		return null;
	}

	public ServerCampaignData GetCampaignInSession(int id)
	{
		int num = this.CampaignCount(id);
		for (int i = 0; i < num; i++)
		{
			ServerCampaignData campaignInSession = this.GetCampaignInSession(id, i);
			if (campaignInSession != null)
			{
				return campaignInSession;
			}
		}
		return null;
	}

	public ServerCampaignData GetCampaignInSession(int id, int index)
	{
		if (this.InSession(id, index))
		{
			return this.GetCampaign(id, index);
		}
		return null;
	}

	public ServerCampaignData GetCampaignInSession(Constants.Campaign.emType campaignType)
	{
		return this.GetCampaignInSession(campaignType, -1);
	}

	public ServerCampaignData GetCampaignInSession(Constants.Campaign.emType campaignType, int id)
	{
		if (this.InSession(campaignType, id))
		{
			return this.GetCampaign(campaignType, id);
		}
		return null;
	}

	public int CampaignCount(int id)
	{
		int num = 0;
		foreach (KeyValuePair<Constants.Campaign.emType, List<ServerCampaignData>> current in this.m_dic)
		{
			int count = current.Value.Count;
			for (int i = 0; i < count; i++)
			{
				ServerCampaignData serverCampaignData = current.Value[i];
				if (serverCampaignData.id == id)
				{
					num++;
					break;
				}
			}
		}
		return num;
	}

	public bool RegistCampaign(ServerCampaignData registData)
	{
		List<ServerCampaignData> list = null;
		if (!this.m_dic.TryGetValue(registData.campaignType, out list))
		{
			list = new List<ServerCampaignData>();
			list.Add(registData);
			this.m_dic.Add(registData.campaignType, list);
		}
		else
		{
			int count = list.Count;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				ServerCampaignData serverCampaignData = list[i];
				if (serverCampaignData.id == registData.id && serverCampaignData.beginDate == registData.beginDate && serverCampaignData.endDate == registData.endDate)
				{
					registData.CopyTo(serverCampaignData);
					flag = true;
				}
			}
			if (!flag)
			{
				list.Add(registData);
			}
		}
		return true;
	}

	public void RemoveCampaign(ServerCampaignData registData)
	{
		List<ServerCampaignData> list = null;
		if (!this.m_dic.TryGetValue(registData.campaignType, out list))
		{
			return;
		}
		List<ServerCampaignData> list2 = new List<ServerCampaignData>();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			ServerCampaignData serverCampaignData = list[i];
			if (serverCampaignData.id == registData.id)
			{
				list2.Add(serverCampaignData);
			}
		}
		foreach (ServerCampaignData current in list2)
		{
			if (current != null)
			{
				list.Remove(current);
			}
		}
	}
}
