using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerCommitWheelSpinGeneral : NetBase
{
	public int paramEventId;

	public int paramSpinId;

	public int paramSpinCostItemId;

	public int paramSpinNum;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	private ServerCharacterState[] _resultCharacterState_k__BackingField;

	private List<ServerChaoState> _resultChaoState_k__BackingField;

	private ServerWheelOptionsGeneral _resultWheelOptionsGen_k__BackingField;

	private ServerSpinResultGeneral _resultWheelResultGen_k__BackingField;

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

	public ServerWheelOptionsGeneral resultWheelOptionsGen
	{
		get;
		private set;
	}

	public ServerSpinResultGeneral resultWheelResultGen
	{
		get;
		private set;
	}

	public NetServerCommitWheelSpinGeneral(int eventId, int spinId, int spinCostItemId, int spinNum)
	{
		this.paramEventId = eventId;
		this.paramSpinId = spinId;
		this.paramSpinCostItemId = spinCostItemId;
		this.paramSpinNum = spinNum;
	}

	protected override void DoRequest()
	{
		base.SetAction("RaidbossSpin/commitRaidbossWheelSpin");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string commitWheelSpinGeneralString = instance.GetCommitWheelSpinGeneralString(this.paramEventId, this.paramSpinCostItemId, this.paramSpinId, this.paramSpinNum);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(commitWheelSpinGeneralString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_CharacterState(jdata);
		this.GetResponse_ChaoState(jdata);
		this.GetResponse_WheelOptionsGen(jdata);
		this.GetResponse_WheelResultGen(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter()
	{
		base.WriteActionParamValue("eventId", this.paramEventId);
		base.WriteActionParamValue("id", this.paramSpinId);
		base.WriteActionParamValue("costItemId", this.paramSpinCostItemId);
		base.WriteActionParamValue("num", this.paramSpinNum);
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

	private void GetResponse_WheelOptionsGen(JsonData jdata)
	{
		this.resultWheelOptionsGen = NetUtil.AnalyzeWheelOptionsGeneralJson(jdata, "raidbossWheelOptions");
	}

	private void GetResponse_WheelResultGen(JsonData jdata)
	{
		this.resultWheelResultGen = NetUtil.AnalyzeSpinResultGeneralJson(jdata, "raidbossSpinResult");
	}
}
