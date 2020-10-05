using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetDebugGetSpecialItem : NetBase
{
	private int _paramItemId_k__BackingField;

	private int _paramAddQuantity_k__BackingField;

	public int paramItemId
	{
		get;
		set;
	}

	public int paramAddQuantity
	{
		get;
		set;
	}

	public NetDebugGetSpecialItem() : this(0, 0)
	{
	}

	public NetDebugGetSpecialItem(int itemId, int addQuantity)
	{
		this.paramItemId = itemId;
		this.paramAddQuantity = addQuantity;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/getSpecialItem");
		this.SetParameter_Item();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Item()
	{
		base.WriteActionParamValue("addItemId", this.paramItemId);
		base.WriteActionParamValue("addNumItem", this.paramAddQuantity);
	}
}
