using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetFirstLaunchChao : NetBase
{
	private ServerPlayerState _resultPlayerState_k__BackingField;

	private List<ServerChaoState> _resultChaoState_k__BackingField;

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public List<ServerChaoState> resultChaoState
	{
		get;
		private set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Chao/getFirstLaunchChao");
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
		this.GetResponse_ChaoState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}

	private void GetResponse_ChaoState(JsonData jdata)
	{
		this.resultChaoState = NetUtil.AnalyzePlayerState_ChaoStates(jdata);
	}
}
