using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerUseSubCharacter : NetBase
{
	private bool _useFlag_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	public bool useFlag
	{
		get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public NetServerUseSubCharacter() : this(false)
	{
	}

	public NetServerUseSubCharacter(bool useFlag)
	{
		this.useFlag = useFlag;
	}

	protected override void DoRequest()
	{
		base.SetAction("Character/useSubCharacter");
		this.SetParameter_UseFlag();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_UseFlag()
	{
		base.WriteActionParamValue("use_flag", (!this.useFlag) ? 0 : 1);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}
}
