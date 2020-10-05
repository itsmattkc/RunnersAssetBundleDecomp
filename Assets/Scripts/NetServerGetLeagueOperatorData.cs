using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetLeagueOperatorData : NetBase
{
	private int _paramMode_k__BackingField;

	private List<ServerLeagueOperatorData> _leagueOperatorData_k__BackingField;

	private int _mode_k__BackingField;

	public int paramMode
	{
		get;
		set;
	}

	public List<ServerLeagueOperatorData> leagueOperatorData
	{
		get;
		set;
	}

	public int mode
	{
		get;
		set;
	}

	public NetServerGetLeagueOperatorData(int mode)
	{
		this.paramMode = mode;
		base.SetSecureFlag(false);
	}

	protected override void DoRequest()
	{
		base.SetAction("Leaderboard/getLeagueOperatorData");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getLeagueOperatorDataString = instance.GetGetLeagueOperatorDataString(this.paramMode, -1);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(getLeagueOperatorDataString);
		}
	}

	protected void SetParameter_NetServerGetLeagueOperatorData()
	{
		this.SetParameter_League();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_LeagueOperatorData(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_League()
	{
		base.WriteActionParamValue("league", -1);
	}

	private void SetParameter_Mode()
	{
		base.WriteActionParamValue("mode", this.paramMode);
	}

	private void GetResponse_LeagueOperatorData(JsonData jdata)
	{
		this.leagueOperatorData = NetUtil.AnalyzeLeagueDatas(jdata, "leagueOperatorList");
		this.mode = NetUtil.GetJsonInt(jdata, "leagueId");
	}
}
