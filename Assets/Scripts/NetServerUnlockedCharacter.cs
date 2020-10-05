using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerUnlockedCharacter : NetBase
{
	private CharaType _charaType_k__BackingField;

	private ServerItem _serverItem_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	private ServerCharacterState[] _resultCharacterState_k__BackingField;

	public CharaType charaType
	{
		get;
		set;
	}

	public ServerItem serverItem
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

	public NetServerUnlockedCharacter(CharaType charaType, ServerItem serverItem)
	{
		this.charaType = charaType;
		this.serverItem = serverItem;
	}

	protected override void DoRequest()
	{
		base.SetAction("Character/unlockedCharacter");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			int characterId = 0;
			ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(this.charaType);
			if (serverCharacterState != null)
			{
				characterId = serverCharacterState.Id;
			}
			string unlockedCharacterString = instance.GetUnlockedCharacterString(characterId, (int)this.serverItem.id);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(unlockedCharacterString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_CharacterState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_CharaType()
	{
		ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(this.charaType);
		if (serverCharacterState != null)
		{
			int id = serverCharacterState.Id;
			base.WriteActionParamValue("characterId", id);
		}
	}

	private void SetParameter_Item()
	{
		int id = (int)this.serverItem.id;
		base.WriteActionParamValue("itemId", id);
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
