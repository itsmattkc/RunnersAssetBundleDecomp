using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetDebugUpgradeChao : NetBase
{
	private int _paramChaoId_k__BackingField;

	private int _paramLevel_k__BackingField;

	public int paramChaoId
	{
		get;
		set;
	}

	public int paramLevel
	{
		get;
		set;
	}

	public NetDebugUpgradeChao() : this(0, 0)
	{
	}

	public NetDebugUpgradeChao(int chaoId, int level)
	{
		this.paramChaoId = chaoId;
		this.paramLevel = level;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/upgradeChao");
		this.SetParameter_Chao();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Chao()
	{
		base.WriteActionParamValue("chaoId", this.paramChaoId);
		base.WriteActionParamValue("lv", this.paramLevel);
	}
}
