using LitJson;
using System;

public class NetServerGetCampaignList : NetBase
{
	protected override void DoRequest()
	{
		base.SetAction("Game/getCampaignList");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_CampaignList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_CampaignList(JsonData jdata)
	{
		NetUtil.GetResponse_CampaignList(jdata);
	}
}
