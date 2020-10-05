using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerPreparePurchase : NetBase
{
	private int _paramItemId_k__BackingField;

	public int paramItemId
	{
		get;
		set;
	}

	public NetServerPreparePurchase() : this(0)
	{
	}

	public NetServerPreparePurchase(int itemId)
	{
		this.paramItemId = itemId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Store/preparePurchase");
		this.SetParameter_ItemId();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_ItemId()
	{
		base.WriteActionParamValue("itemId", this.paramItemId);
	}
}
