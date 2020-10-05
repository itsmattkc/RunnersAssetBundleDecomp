using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerReLogin : NetBase
{
	private string _resultSessionId_k__BackingField;

	private int _sessionTimeLimit_k__BackingField;

	public string resultSessionId
	{
		get;
		private set;
	}

	public int sessionTimeLimit
	{
		get;
		private set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Login/reLogin");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_SessionId(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_SessionId(JsonData jdata)
	{
		this.resultSessionId = NetUtil.GetJsonString(jdata, "sessionId");
		this.sessionTimeLimit = NetUtil.GetJsonInt(jdata, "sessionTimeLimit");
	}
}
