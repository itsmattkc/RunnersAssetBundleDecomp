using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerRedStarExchange : NetBase
{
	private int _paramStoreItemId_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	public int paramStoreItemId
	{
		get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public NetServerRedStarExchange() : this(0)
	{
	}

	public NetServerRedStarExchange(int storeItemId)
	{
		this.paramStoreItemId = storeItemId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Store/redstarExchange");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string redStarExchangeString = instance.GetRedStarExchangeString(this.paramStoreItemId);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(redStarExchangeString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_StoreItemId()
	{
		base.WriteActionParamValue("itemId", this.paramStoreItemId);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}
}
