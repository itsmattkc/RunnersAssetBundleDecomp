using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerLoginBonus : NetBase
{
	private ServerLoginBonusState _loginBonusState_k__BackingField;

	private DateTime _startTime_k__BackingField;

	private DateTime _endTime_k__BackingField;

	private List<ServerLoginBonusReward> _loginBonusRewardList_k__BackingField;

	private List<ServerLoginBonusReward> _firstLoginBonusRewardList_k__BackingField;

	private int _rewardId_k__BackingField;

	private int _rewardDays_k__BackingField;

	private int _firstRewardDays_k__BackingField;

	public ServerLoginBonusState loginBonusState
	{
		get;
		private set;
	}

	public DateTime startTime
	{
		get;
		private set;
	}

	public DateTime endTime
	{
		get;
		private set;
	}

	public List<ServerLoginBonusReward> loginBonusRewardList
	{
		get;
		private set;
	}

	public List<ServerLoginBonusReward> firstLoginBonusRewardList
	{
		get;
		private set;
	}

	public int rewardId
	{
		get;
		private set;
	}

	public int rewardDays
	{
		get;
		private set;
	}

	public int firstRewardDays
	{
		get;
		private set;
	}

	public int resultLoginBonusRewardCount
	{
		get
		{
			if (this.loginBonusRewardList != null)
			{
				return this.loginBonusRewardList.Count;
			}
			return 0;
		}
	}

	protected override void DoRequest()
	{
		base.SetAction("Login/loginBonus");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_LoginBonusStatus(jdata);
		this.GetResponse_StartTime(jdata);
		this.GetResponse_EndTime(jdata);
		this.GetResponse_LoginBonusRewardList(jdata);
		this.GetResponse_FirstLoginBonusRewardList(jdata);
		this.GetResponse_RewardId(jdata);
		this.GetResponse_RewardDays(jdata);
		this.GetResponse_FirstRewardDays(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_LoginBonusStatus(JsonData jdata)
	{
		this.loginBonusState = new ServerLoginBonusState();
		JsonData jsonObject = NetUtil.GetJsonObject(jdata, "loginBonusStatus");
		if (jsonObject != null)
		{
			this.loginBonusState = new ServerLoginBonusState
			{
				m_numLogin = NetUtil.GetJsonInt(jsonObject, "numLogin"),
				m_numBonus = NetUtil.GetJsonInt(jsonObject, "numBonus"),
				m_lastBonusTime = NetUtil.GetLocalDateTime((long)NetUtil.GetJsonInt(jsonObject, "lastBonusTime"))
			};
		}
	}

	private void GetResponse_StartTime(JsonData jdata)
	{
		this.startTime = NetUtil.GetLocalDateTime((long)NetUtil.GetJsonInt(jdata, "startTime"));
	}

	private void GetResponse_EndTime(JsonData jdata)
	{
		this.endTime = NetUtil.GetLocalDateTime((long)NetUtil.GetJsonInt(jdata, "endTime"));
	}

	private void GetResponse_LoginBonusRewardList(JsonData jdata)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "loginBonusRewardList");
		this.loginBonusRewardList = new List<ServerLoginBonusReward>();
		if (jsonArray == null)
		{
			return;
		}
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerLoginBonusReward serverLoginBonusReward = new ServerLoginBonusReward();
			JsonData jsonArray2 = NetUtil.GetJsonArray(jdata2, "selectRewardList");
			int count2 = jsonArray2.Count;
			for (int j = 0; j < count2; j++)
			{
				JsonData jsonArray3 = NetUtil.GetJsonArray(jsonArray2[j], "itemList");
				int count3 = jsonArray3.Count;
				for (int k = 0; k < count3; k++)
				{
					serverLoginBonusReward.m_itemList.Add(NetUtil.AnalyzeItemStateJson(jsonArray3[k], string.Empty));
				}
			}
			this.loginBonusRewardList.Add(serverLoginBonusReward);
		}
	}

	private void GetResponse_FirstLoginBonusRewardList(JsonData jdata)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "firstLoginBonusRewardList");
		this.firstLoginBonusRewardList = new List<ServerLoginBonusReward>();
		if (jsonArray == null)
		{
			return;
		}
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerLoginBonusReward serverLoginBonusReward = new ServerLoginBonusReward();
			JsonData jsonArray2 = NetUtil.GetJsonArray(jdata2, "selectRewardList");
			int count2 = jsonArray2.Count;
			for (int j = 0; j < count2; j++)
			{
				JsonData jsonArray3 = NetUtil.GetJsonArray(jsonArray2[j], "itemList");
				int count3 = jsonArray3.Count;
				for (int k = 0; k < count3; k++)
				{
					serverLoginBonusReward.m_itemList.Add(NetUtil.AnalyzeItemStateJson(jsonArray3[k], string.Empty));
				}
			}
			this.firstLoginBonusRewardList.Add(serverLoginBonusReward);
		}
	}

	private void GetResponse_RewardId(JsonData jdata)
	{
		this.rewardId = NetUtil.GetJsonInt(jdata, "rewardId");
	}

	private void GetResponse_RewardDays(JsonData jdata)
	{
		this.rewardDays = NetUtil.GetJsonInt(jdata, "rewardDays");
	}

	private void GetResponse_FirstRewardDays(JsonData jdata)
	{
		this.firstRewardDays = NetUtil.GetJsonInt(jdata, "firstRewardDays");
	}
}
