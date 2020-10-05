using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerChangeCharacter : NetBase
{
	private int _mainCharaId_k__BackingField;

	private int _subCharaId_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	public int mainCharaId
	{
		get;
		set;
	}

	public int subCharaId
	{
		get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public NetServerChangeCharacter() : this(0, 0)
	{
	}

	public NetServerChangeCharacter(int mainCharaId, int subCharaId)
	{
		this.mainCharaId = mainCharaId;
		this.subCharaId = subCharaId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Character/changeCharacter");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string changeCharacterString = instance.GetChangeCharacterString(this.mainCharaId, this.subCharaId);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(changeCharacterString);
		}
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
		base.WriteActionParamValue("mainCharacterId", this.mainCharaId);
		base.WriteActionParamValue("subCharacterId", this.subCharaId);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}
}
