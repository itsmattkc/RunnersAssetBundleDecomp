using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerGetWeeklyLeaderboardOptions : NetBase
{
	private int _paramMode_k__BackingField;

	private ServerWeeklyLeaderboardOptions _weeklyLeaderboardOptions_k__BackingField;

	public int paramMode
	{
		get;
		set;
	}

	public ServerWeeklyLeaderboardOptions weeklyLeaderboardOptions
	{
		get;
		set;
	}

	public NetServerGetWeeklyLeaderboardOptions(int mode)
	{
		this.paramMode = mode;
		base.SetSecureFlag(false);
	}

	protected override void DoRequest()
	{
		base.SetAction("Leaderboard/getWeeklyLeaderboardOptions");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getWeeklyLeaderboardOptionsString = instance.GetGetWeeklyLeaderboardOptionsString(this.paramMode);
			base.WriteJsonString(getWeeklyLeaderboardOptionsString);
		}
	}

	protected void SetParameter_NetServerGetWeeklyLeaderboardOptions()
	{
		this.SetParameter_Option();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_WeeklyLeaderboardOptions(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Option()
	{
		base.WriteActionParamValue("mode", this.paramMode);
	}

	private void GetResponse_WeeklyLeaderboardOptions(JsonData jdata)
	{
		this.weeklyLeaderboardOptions = NetUtil.AnalyzeWeeklyLeaderboardOptions(jdata);
	}
}
