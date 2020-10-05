using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerRetrievePlayerState : NetBase
{
	private ServerPlayerState _resultPlayerState_k__BackingField;

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Player/getPlayerState");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}
}
