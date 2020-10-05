using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerCommitChaoWheelSpin : NetBase
{
	public int paramCount;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	private ServerCharacterState[] _resultCharacterState_k__BackingField;

	private List<ServerChaoState> _resultChaoState_k__BackingField;

	private ServerChaoWheelOptions _resultChaoWheelOptions_k__BackingField;

	private ServerSpinResultGeneral _resultSpinResultGeneral_k__BackingField;

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public ServerCharacterState[] resultCharacterState
	{
		get;
		private set;
	}

	public List<ServerChaoState> resultChaoState
	{
		get;
		private set;
	}

	public ServerChaoWheelOptions resultChaoWheelOptions
	{
		get;
		private set;
	}

	public ServerSpinResultGeneral resultSpinResultGeneral
	{
		get;
		private set;
	}

	public NetServerCommitChaoWheelSpin(int count)
	{
		this.paramCount = count;
	}

	protected override void DoRequest()
	{
		base.SetAction("Chao/commitChaoWheelSpin");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string commitChaoWheelSpinString = instance.GetCommitChaoWheelSpinString(this.paramCount);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(commitChaoWheelSpinString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_CharacterState(jdata);
		this.GetResponse_ChaoState(jdata);
		this.GetResponse_ChaoWheelOptions(jdata);
		this.GetResponse_ChaoWheelResult(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_ChaoWheelSpin()
	{
		base.WriteActionParamValue("count", this.paramCount);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}

	private void GetResponse_CharacterState(JsonData jdata)
	{
		this.resultCharacterState = NetUtil.AnalyzePlayerState_CharactersStates(jdata);
	}

	private void GetResponse_ChaoState(JsonData jdata)
	{
		this.resultChaoState = NetUtil.AnalyzePlayerState_ChaoStates(jdata);
	}

	private void GetResponse_ChaoWheelOptions(JsonData jdata)
	{
		this.resultChaoWheelOptions = NetUtil.AnalyzeChaoWheelOptionsJson(jdata, "chaoWheelOptions");
	}

	private void GetResponse_ChaoWheelResult(JsonData jdata)
	{
		this.resultSpinResultGeneral = NetUtil.AnalyzeSpinResultJson(jdata, "chaoSpinResultList");
	}
}
