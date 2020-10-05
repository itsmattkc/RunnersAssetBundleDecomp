using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerBuyIos : NetBase
{
	private string _receipt_data_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	public string receipt_data
	{
		get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public NetServerBuyIos() : this(string.Empty)
	{
	}

	public NetServerBuyIos(string receiptData)
	{
		this.receipt_data = receiptData;
	}

	protected override void DoRequest()
	{
		base.SetAction("Store/buyIos");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string buyIosString = instance.GetBuyIosString(this.receipt_data);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(buyIosString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_ReceiptData()
	{
		base.WriteActionParamValue("receipt_data", this.receipt_data);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}
}
