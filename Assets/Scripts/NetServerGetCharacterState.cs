using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerGetCharacterState : NetBase
{
	private ServerCharacterState[] _resultCharacterState_k__BackingField;

	public ServerCharacterState[] resultCharacterState
	{
		get;
		private set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Player/getCharacterState");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_CharacterState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_CharacterState(JsonData jdata)
	{
		this.resultCharacterState = NetUtil.AnalyzePlayerState_CharactersStates(jdata);
	}
}
