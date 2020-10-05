using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NetServerCommitWheelSpin : NetBase
{
	public int paramCount;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	private ServerCharacterState[] _resultCharacterState_k__BackingField;

	private List<ServerChaoState> _resultChaoState_k__BackingField;

	private ServerWheelOptions _resultWheelOptions_k__BackingField;

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

	public ServerWheelOptions resultWheelOptions
	{
		get;
		private set;
	}

	public ServerSpinResultGeneral resultSpinResultGeneral
	{
		get;
		private set;
	}

	public NetServerCommitWheelSpin(int count)
	{
		this.paramCount = count;
	}

	protected override void DoRequest()
	{
		base.SetAction("Spin/commitWheelSpin");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string commitWheelSpinString = instance.GetCommitWheelSpinString(this.paramCount);
			global::Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(commitWheelSpinString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_CharacterState(jdata);
		this.GetResponse_ChaoState(jdata);
		this.GetResponse_WheelOptions(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
		this.resultPlayerState = ServerInterface.PlayerState;
		this.resultWheelOptions = ServerInterface.WheelOptions;
		this.resultPlayerState.RefreshFakeState();
		this.resultWheelOptions.RefreshFakeState();
		if (this.resultWheelOptions.m_spinCost > this.resultPlayerState.m_numRedRings)
		{
			base.resultStCd = ServerInterface.StatusCode.NotEnoughRedStarRings;
		}
		else
		{
			this.resultPlayerState.m_numRedRings -= this.resultWheelOptions.m_spinCost;
			this.resultWheelOptions.m_spinCost = this.resultWheelOptions.m_nextSpinCost;
			this.resultWheelOptions.m_nextSpinCost++;
			ServerWheelOptions.WheelItemType wheelItemType = (ServerWheelOptions.WheelItemType)this.resultWheelOptions.m_items[this.resultWheelOptions.m_itemWon];
			if (wheelItemType == ServerWheelOptions.WheelItemType.SpinAgain)
			{
				this.resultWheelOptions.m_nextSpinCost = this.resultWheelOptions.m_spinCost;
				this.resultWheelOptions.m_spinCost = 0;
			}
			DateTime now = DateTime.Now;
			if (this.resultWheelOptions.m_nextFreeSpin <= now)
			{
				TimeSpan t = new TimeSpan(1, 0, 0, 0);
				while (this.resultWheelOptions.m_nextFreeSpin <= now)
				{
					this.resultWheelOptions.m_nextFreeSpin += t;
				}
			}
			Array values = Enum.GetValues(typeof(ServerWheelOptions.WheelItemType));
			this.resultWheelOptions.m_items[this.resultWheelOptions.m_itemWon] = (int)values.GetValue(UnityEngine.Random.Range(0, values.Length - 1));
			this.resultWheelOptions.m_itemWon = UnityEngine.Random.Range(0, this.resultWheelOptions.m_items.Length);
		}
	}

	private void SetParameter_WheelSpin()
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

	private void GetResponse_WheelOptions(JsonData jdata)
	{
		this.resultWheelOptions = NetUtil.AnalyzeWheelOptionsJson(jdata, "wheelOptions");
	}

	private void GetResponse_WheelResult(JsonData jdata)
	{
		this.resultSpinResultGeneral = NetUtil.AnalyzeSpinResultJson(jdata, "spinResultList");
	}
}
