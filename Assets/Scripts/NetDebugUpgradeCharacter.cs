using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetDebugUpgradeCharacter : NetBase
{
	private int _paramCharacterId_k__BackingField;

	private int _paramLevel_k__BackingField;

	public int paramCharacterId
	{
		get;
		set;
	}

	public int paramLevel
	{
		get;
		set;
	}

	public NetDebugUpgradeCharacter() : this(0, 0)
	{
	}

	public NetDebugUpgradeCharacter(int characterId, int level)
	{
		this.paramCharacterId = characterId;
		this.paramLevel = level;
	}

	protected override void DoRequest()
	{
		base.SetAction("Debug/upgradeCharacter");
		this.SetParameter_Character();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Character()
	{
		base.WriteActionParamValue("characterId", this.paramCharacterId);
		base.WriteActionParamValue("lv", this.paramLevel);
	}
}
