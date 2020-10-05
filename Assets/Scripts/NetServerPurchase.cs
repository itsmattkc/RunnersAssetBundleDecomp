using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerPurchase : NetBase
{
	private bool _paramPurchaseResult_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	public bool paramPurchaseResult
	{
		get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public NetServerPurchase() : this(false)
	{
	}

	public NetServerPurchase(bool isSuccess)
	{
		this.paramPurchaseResult = isSuccess;
	}

	protected override void DoRequest()
	{
		base.SetAction("Store/purchase");
		this.SetParameter_PurchaseResult();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		if (!this.paramPurchaseResult)
		{
			base.state = NetBase.emState.UnavailableFailed;
			base.resultStCd = ServerInterface.StatusCode.HspPurchaseError;
		}
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_PurchaseResult()
	{
		base.WriteActionParamValue("isSuccess", (!this.paramPurchaseResult) ? 0 : 1);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}
}
