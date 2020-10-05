using LitJson;
using System;

public class NetServerGetFreeItemList : NetBase
{
	public ServerFreeItemState resultFreeItemState;

	protected override void DoRequest()
	{
		base.SetAction("Game/getFreeItemList");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_FreeItemList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_FreeItemList(JsonData jdata)
	{
		this.resultFreeItemState = NetUtil.AnalyzeFreeItemList(jdata);
	}
}
