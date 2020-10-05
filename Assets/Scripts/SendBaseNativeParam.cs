using System;
using System.Runtime.CompilerServices;

public struct SendBaseNativeParam
{
	private string _sessionId_k__BackingField;

	private string _version_k__BackingField;

	private string _seq_k__BackingField;

	public string sessionId
	{
		get;
		private set;
	}

	public string version
	{
		get;
		private set;
	}

	public string seq
	{
		get;
		private set;
	}

	public void Init()
	{
		ServerLoginState loginState = ServerInterface.LoginState;
		if (loginState.IsLoggedIn)
		{
			this.sessionId = loginState.sessionId;
		}
		else
		{
			this.sessionId = string.Empty;
		}
		this.version = CurrentBundleVersion.version;
		this.seq = (loginState.seqNum + 1uL).ToString();
	}
}
