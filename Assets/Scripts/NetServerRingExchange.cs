using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerRingExchange : NetBase
{
	public int itemId;

	public int itemNum;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public NetServerRingExchange() : this(0, 0)
	{
	}

	public NetServerRingExchange(int itemId, int itemNum)
	{
		this.itemId = itemId;
		this.itemNum = itemNum;
	}

	protected override void DoRequest()
	{
		base.SetAction("Store/ringExchange");
		this.SetParameter_ItemData();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_ItemData()
	{
		base.WriteActionParamValue("itemId", this.itemId);
		base.WriteActionParamValue("itemNum", this.itemNum);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}
}
