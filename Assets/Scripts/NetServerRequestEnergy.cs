using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerRequestEnergy : NetBase
{
	private string _paramFriendId_k__BackingField;

	private ServerPlayerState _resultPlayerState_k__BackingField;

	private long _resultExpireTime_k__BackingField;

	public string paramFriendId
	{
		get;
		set;
	}

	public ServerPlayerState resultPlayerState
	{
		get;
		private set;
	}

	public long resultExpireTime
	{
		get;
		private set;
	}

	public NetServerRequestEnergy() : this(string.Empty)
	{
	}

	public NetServerRequestEnergy(string friendId)
	{
		this.paramFriendId = friendId;
	}

	protected override void DoRequest()
	{
		base.SetAction("Friend/requestEnergy");
		this.SetParameter_FriendId();
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_PlayerState(jdata);
		this.GetResponse_ExpireTime(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
		this.resultPlayerState = ServerInterface.PlayerState;
		this.resultPlayerState.RefreshFakeState();
	}

	private void SetParameter_FriendId()
	{
		base.WriteActionParamValue("friendId", this.paramFriendId);
	}

	private void GetResponse_PlayerState(JsonData jdata)
	{
		this.resultPlayerState = NetUtil.AnalyzePlayerStateJson(jdata, "playerState");
	}

	private void GetResponse_ExpireTime(JsonData jdata)
	{
		this.resultExpireTime = NetUtil.GetJsonLong(jdata, "expireTime");
	}
}
