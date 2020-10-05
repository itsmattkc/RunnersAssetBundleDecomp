using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetDailyMissionData : NetBase
{
	private int _resultNumContinue_k__BackingField;

	private int _resultNumDailyChalDay_k__BackingField;

	private int _resultMaxDailyChalDay_k__BackingField;

	private int _resultMaxIncentive_k__BackingField;

	private DateTime _resultChalEndTime_k__BackingField;

	private List<ServerDailyChallengeIncentive> _resultDailyMissionIncentiveList_k__BackingField;

	public int resultNumContinue
	{
		get;
		private set;
	}

	public int resultIncentives
	{
		get
		{
			if (this.resultDailyMissionIncentiveList != null)
			{
				return this.resultDailyMissionIncentiveList.Count;
			}
			return 0;
		}
	}

	public int resultNumDailyChalDay
	{
		get;
		private set;
	}

	public int resultMaxDailyChalDay
	{
		get;
		private set;
	}

	public int resultMaxIncentive
	{
		get;
		private set;
	}

	public DateTime resultChalEndTime
	{
		get;
		private set;
	}

	protected List<ServerDailyChallengeIncentive> resultDailyMissionIncentiveList
	{
		get;
		set;
	}

	protected override void DoRequest()
	{
		base.SetAction("Game/getDailyChalData");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_IncentiveList(jdata);
		this.GetResponse_NumContinue(jdata);
		this.GetResponse_NumDailyChalDay(jdata);
		this.GetResponse_MaxDailyChalDay(jdata);
		this.GetResponse_MaxIncentive(jdata);
		this.GetResponse_ChalEndTime(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	public ServerDailyChallengeIncentive GetResultDailyMissionIncentive(int index)
	{
		if (0 <= index && this.resultIncentives > index)
		{
			return this.resultDailyMissionIncentiveList[index];
		}
		return null;
	}

	private void GetResponse_IncentiveList(JsonData jdata)
	{
		this.resultDailyMissionIncentiveList = new List<ServerDailyChallengeIncentive>();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "incentiveList");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			ServerDailyChallengeIncentive item = NetUtil.AnalyzeDailyMissionIncentiveJson(jdata2, string.Empty);
			this.resultDailyMissionIncentiveList.Add(item);
		}
	}

	private void GetResponse_NumContinue(JsonData jdata)
	{
		this.resultNumContinue = NetUtil.GetJsonInt(jdata, "numDailyChalCont");
	}

	private void GetResponse_NumDailyChalDay(JsonData jdata)
	{
		this.resultNumDailyChalDay = NetUtil.GetJsonInt(jdata, "numDailyChalDay");
	}

	private void GetResponse_MaxDailyChalDay(JsonData jdata)
	{
		this.resultMaxDailyChalDay = NetUtil.GetJsonInt(jdata, "maxDailyChalDay");
	}

	private void GetResponse_MaxIncentive(JsonData jdata)
	{
		this.resultMaxIncentive = NetUtil.GetJsonInt(jdata, "incentiveListCont");
	}

	private void GetResponse_ChalEndTime(JsonData jdata)
	{
		this.resultChalEndTime = NetUtil.GetLocalDateTime(NetUtil.GetJsonLong(jdata, "chalEndTime"));
		UnityEngine.Debug.Log("resultChalEndTime:" + this.resultChalEndTime.ToString());
	}
}
