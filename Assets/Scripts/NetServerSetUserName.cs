using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerSetUserName : NetBase
{
	private string _userName_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	public string userName
	{
		private get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public NetServerSetUserName(string userName)
	{
		this.userName = userName;
	}

	protected override void DoRequest()
	{
		base.SetAction("Player/setUserName");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string setUserNameString = instance.GetSetUserNameString(this.userName);
			base.WriteJsonString(setUserNameString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_UserName()
	{
		base.WriteActionParamValue("userName", this.userName);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}
}
