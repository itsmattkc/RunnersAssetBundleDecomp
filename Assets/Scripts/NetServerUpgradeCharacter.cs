using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerUpgradeCharacter : NetBase
{
	private int _paramCharacterId_k__BackingField;

	private int _paramAbilityId_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	private ServerCharacterState[] _resultCharacterState_k__BackingField;

	public int paramCharacterId
	{
		get;
		set;
	}

	public int paramAbilityId
	{
		get;
		set;
	}

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

	public NetServerUpgradeCharacter() : this(0, 0)
	{
	}

	public NetServerUpgradeCharacter(int characterId, int abilityId)
	{
		this.paramCharacterId = characterId;
		this.paramAbilityId = abilityId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Character/upgradeCharacter");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string upgradeCharacterString = instance.GetUpgradeCharacterString(this.paramCharacterId, this.paramAbilityId);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(upgradeCharacterString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_CharacterState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
		this.resultPlayerState = ServerInterface.PlayerState;
		this.resultPlayerState.RefreshFakeState();
	}

	private void SetParameter()
	{
		base.WriteActionParamValue("characterId", this.paramCharacterId);
		base.WriteActionParamValue("abilityId", this.paramAbilityId);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}

	private void GetResponse_CharacterState(JsonData jdata)
	{
		this.resultCharacterState = NetUtil.AnalyzePlayerState_CharactersStates(jdata);
	}
}
