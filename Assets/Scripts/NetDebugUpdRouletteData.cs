using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetDebugUpdRouletteData : NetBase
{
	private int _paramRank_k__BackingField;

	private int _paramNumRemaining_k__BackingField;

	private int _paramItemWon_k__BackingField;

	public int paramRank
	{
		get;
		set;
	}

	public int paramNumRemaining
	{
		get;
		set;
	}

	public int paramItemWon
	{
		get;
		set;
	}

	public NetDebugUpdRouletteData() : this(0, 0, 0)
	{
	}

	public NetDebugUpdRouletteData(int rank, int numRemaining, int itemWon)
	{
		this.paramRank = rank;
		this.paramNumRemaining = numRemaining;
		this.paramItemWon = itemWon;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/updRouletteData");
		this.SetParameter_Roulette();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Roulette()
	{
		base.WriteActionParamValue("rouletteRank", this.paramRank);
		base.WriteActionParamValue("numRemainingRoulette", this.paramNumRemaining);
		base.WriteActionParamValue("itemWon", this.paramItemWon);
	}
}
