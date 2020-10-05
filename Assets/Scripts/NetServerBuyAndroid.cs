using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerBuyAndroid : NetBase
{
	private string _receipt_data_k__BackingField;

	private string _signature_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	public string receipt_data
	{
		get;
		set;
	}

	public string signature
	{
		get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public NetServerBuyAndroid() : this(string.Empty, string.Empty)
	{
	}

	public NetServerBuyAndroid(string receiptData, string signature)
	{
		this.receipt_data = receiptData;
		this.signature = signature;
	}

	protected override void DoRequest()
	{
		base.SetAction("Store/buyAndroid");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string buyAndroidString = instance.GetBuyAndroidString(this.receipt_data, this.signature);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(buyAndroidString);
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
		base.WriteActionParamValue("receipt_signature", this.signature);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}
}
