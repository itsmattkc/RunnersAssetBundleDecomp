using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetDebugUpdPointData : NetBase
{
	private int _paramAddEnergyFree_k__BackingField;

	private int _paramAddEnergyBuy_k__BackingField;

	private int _paramAddRingFree_k__BackingField;

	private int _paramAddRingBuy_k__BackingField;

	private int _paramAddRedRingFree_k__BackingField;

	private int _paramAddRedRingBuy_k__BackingField;

	public int paramAddEnergyFree
	{
		get;
		set;
	}

	public int paramAddEnergyBuy
	{
		get;
		set;
	}

	public int paramAddRingFree
	{
		get;
		set;
	}

	public int paramAddRingBuy
	{
		get;
		set;
	}

	public int paramAddRedRingFree
	{
		get;
		set;
	}

	public int paramAddRedRingBuy
	{
		get;
		set;
	}

	public NetDebugUpdPointData() : this(0, 0, 0, 0, 0, 0)
	{
	}

	public NetDebugUpdPointData(int addEnergyFree, int addEnergyBuy, int addRingFree, int addRingBuy, int addRedStarRingFree, int addRedStarRingBuy)
	{
		this.paramAddEnergyFree = addEnergyFree;
		this.paramAddEnergyBuy = addEnergyBuy;
		this.paramAddRingFree = addRingFree;
		this.paramAddRingBuy = addRingBuy;
		this.paramAddRedRingFree = addRedStarRingFree;
		this.paramAddRedRingBuy = addRedStarRingBuy;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/updPointData");
		this.SetParameter_AddEnergy();
		this.SetParameter_AddRing();
		this.SetParameter_AddRedStarRing();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_AddEnergy()
	{
		base.WriteActionParamValue("addEnergyFree", this.paramAddEnergyFree);
		base.WriteActionParamValue("addEnergyBuy", this.paramAddEnergyBuy);
	}

	private void SetParameter_AddRing()
	{
		base.WriteActionParamValue("addRingFree", this.paramAddRingFree);
		base.WriteActionParamValue("addRingBuy", this.paramAddRingBuy);
	}

	private void SetParameter_AddRedStarRing()
	{
		base.WriteActionParamValue("addRedstarFree", this.paramAddRedRingFree);
		base.WriteActionParamValue("addRedstarBuy", this.paramAddRedRingBuy);
	}
}
