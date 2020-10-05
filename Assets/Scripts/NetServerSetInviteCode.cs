using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerSetInviteCode : NetBase
{
	private string _friendId_k__BackingField;

	private ServerPlayerState _playerState_k__BackingField;

	private List<ServerPresentState> _incentive_k__BackingField;

	public string friendId
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

	public NetServerSetInviteCode() : this(string.Empty)
	{
	}

	public NetServerSetInviteCode(string friendId)
	{
		this.friendId = friendId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Friend/setInviteCode");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string setInviteCodeString = instance.GetSetInviteCodeString(this.friendId);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(setInviteCodeString);
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

	private void SetParameter_FriendId()
	{
		base.WriteActionParamValue("friendId", this.friendId);
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
			ServerPresentState serverPresentState = NetUtil.AnalyzePresentStateJson(jsonArray[i], string.Empty);
			if (serverPresentState != null)
			{
				this.incentive.Add(serverPresentState);
			}
		}
	}
}
