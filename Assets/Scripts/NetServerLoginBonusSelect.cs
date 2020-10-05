using LitJson;
using System;
using System.Runtime.CompilerServices;

public class NetServerLoginBonusSelect : NetBase
{
	public int paramRewardId;

	public int paramRewardDays;

	public int paramRewardSelect;

	public int paramFirstRewardDays;

	public int paramFirstRewardSelect;

	private ServerLoginBonusReward _loginBonusReward_k__BackingField;

	private ServerLoginBonusReward _firstLoginBonusReward_k__BackingField;

	public ServerLoginBonusReward loginBonusReward
	{
		get;
		private set;
	}

	public ServerLoginBonusReward firstLoginBonusReward
	{
		get;
		private set;
	}

	public NetServerLoginBonusSelect() : this(0, 0, 0, 0, 0)
	{
	}

	public NetServerLoginBonusSelect(int rewardId, int rewardDays, int rewardSelect, int firstRewardDays, int firstRewardSelect)
	{
		this.paramRewardId = rewardId;
		this.paramRewardDays = rewardDays;
		this.paramRewardSelect = rewardSelect;
		this.paramFirstRewardDays = firstRewardDays;
		this.paramFirstRewardSelect = firstRewardSelect;
	}

	protected override void DoRequest()
	{
		base.SetAction("Login/loginBonusSelect");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string jsonString = instance.LoginBonusSelectString(this.paramRewardId, this.paramRewardDays, this.paramRewardSelect, this.paramFirstRewardDays, this.paramFirstRewardSelect);
			base.WriteJsonString(jsonString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_LoginBonusRewardList(jdata);
		this.GetResponse_FirstLoginBonusRewardList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void GetResponse_LoginBonusRewardList(JsonData jdata)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "rewardList");
		if (jsonArray == null)
		{
			this.loginBonusReward = null;
			return;
		}
		this.loginBonusReward = new ServerLoginBonusReward();
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			this.loginBonusReward.m_itemList.Add(NetUtil.AnalyzeItemStateJson(jsonArray[i], string.Empty));
		}
	}

	private void GetResponse_FirstLoginBonusRewardList(JsonData jdata)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "firstRewardList");
		this.firstLoginBonusReward = new ServerLoginBonusReward();
		if (jsonArray == null)
		{
			this.firstLoginBonusReward = null;
			return;
		}
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			this.firstLoginBonusReward.m_itemList.Add(NetUtil.AnalyzeItemStateJson(jsonArray[i], string.Empty));
		}
	}
}
