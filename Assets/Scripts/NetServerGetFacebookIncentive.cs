using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetFacebookIncentive : NetBase
{
	private int _incentiveType_k__BackingField;

	private int _achievementCount_k__BackingField;

	private ServerPlayerState _playerState_k__BackingField;

	private List<ServerPresentState> _incentive_k__BackingField;

	public int incentiveType
	{
		get;
		set;
	}

	public int achievementCount
	{
		get;
		set;
	}

	public ServerPlayerState playerState
	{
		get;
		set;
	}

	public List<ServerPresentState> incentive
	{
		get;
		set;
	}

	public NetServerGetFacebookIncentive() : this(0, 0)
	{
	}

	public NetServerGetFacebookIncentive(int incentiveType, int achievementCount)
	{
		this.incentiveType = incentiveType;
		this.achievementCount = achievementCount;
	}

	protected override void DoRequest()
	{
		base.SetAction("Friend/getFacebookIncentive");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getFacebookIncentiveString = instance.GetGetFacebookIncentiveString(this.incentiveType, this.achievementCount);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(getFacebookIncentiveString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_Incentive(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_Data()
	{
		base.WriteActionParamValue("type", this.incentiveType);
		base.WriteActionParamValue("achievementCount", this.achievementCount);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.playerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}

	private void GetResponse_Incentive(JsonData jdata)
	{
		this.incentive = new List<ServerPresentState>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "incentive");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			ServerPresentState item = NetUtil.AnalyzePresentStateJson(jsonArray[i], string.Empty);
			this.incentive.Add(item);
		}
	}
}
