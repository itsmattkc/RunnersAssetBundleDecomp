using System;

public class ServerCampaignData
{
	public Constants.Campaign.emType campaignType;

	public int id;

	public long beginDate;

	public long endDate;

	public int iContent;

	public int iSubContent;

	public float fContent
	{
		get
		{
			return (float)this.iContent / ServerCampaignData.fContentBasis;
		}
	}

	public static float fContentBasis
	{
		get
		{
			return 1000f;
		}
	}

	public float fSubContent
	{
		get
		{
			return (float)this.iSubContent / 1000f;
		}
	}

	public ServerCampaignData()
	{
		this.campaignType = Constants.Campaign.emType.BankedRingBonus;
		this.id = 0;
		this.beginDate = 0L;
		this.endDate = 0L;
		this.iContent = 0;
	}

	public void CopyTo(ServerCampaignData to)
	{
		to.campaignType = this.campaignType;
		to.id = this.id;
		to.beginDate = this.beginDate;
		to.endDate = this.endDate;
		to.iContent = this.iContent;
	}

	public bool InSession()
	{
		DateTime t = (this.beginDate == 0L) ? DateTime.MinValue : NetUtil.GetLocalDateTime(this.beginDate);
		DateTime t2 = (this.endDate == 0L) ? DateTime.MaxValue : NetUtil.GetLocalDateTime(this.endDate);
		DateTime currentTime = NetUtil.GetCurrentTime();
		return currentTime >= t && currentTime <= t2;
	}
}
