using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerGetLeagueData : NetBase
{
	private int _paramMode_k__BackingField;

	private ServerLeagueData _leagueData_k__BackingField;

	public int paramMode
	{
		get;
		set;
	}

	public ServerLeagueData leagueData
	{
		get;
		set;
	}

	public NetServerGetLeagueData(int mode)
	{
		this.paramMode = mode;
		base.SetSecureFlag(false);
	}

	protected override void DoRequest()
	{
		base.SetAction("Leaderboard/getLeagueData");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getLeagueDataString = instance.GetGetLeagueDataString(this.paramMode);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(getLeagueDataString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_LeagueData(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Mode()
	{
		base.WriteActionParamValue("mode", this.paramMode);
	}

	private void GetResponse_LeagueData(JsonData jdata)
	{
		this.leagueData = NetUtil.AnalyzeLeagueData(jdata, "leagueData");
	}
}
