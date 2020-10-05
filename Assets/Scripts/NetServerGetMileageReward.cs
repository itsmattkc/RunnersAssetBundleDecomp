using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetMileageReward : NetBase
{
	private int m_episode;

	private int m_chapter;

	private List<ServerMileageReward> _m_rewardList_k__BackingField;

	public List<ServerMileageReward> m_rewardList
	{
		get;
		private set;
	}

	public NetServerGetMileageReward(int episode, int chapter)
	{
		this.m_episode = episode;
		this.m_chapter = chapter;
		this.m_rewardList = new List<ServerMileageReward>();
	}

	protected override void DoRequest()
	{
		base.SetAction("Game/getMileageReward");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getMileageRewardString = instance.GetGetMileageRewardString(this.m_episode, this.m_chapter);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(getMileageRewardString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_MileageReward(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_EpisodeChapter()
	{
		base.WriteActionParamValue("episode", this.m_episode);
		base.WriteActionParamValue("chapter", this.m_chapter);
	}

	private void GetResponse_MileageReward(JsonData jdata)
	{
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "mileageMapRewardList");
		if (jsonArray == null)
		{
			return;
		}
		for (int i = 0; i < jsonArray.Count; i++)
		{
			ServerMileageReward serverMileageReward = this.AnalyzeMileageRewardJson(jsonArray[i]);
			serverMileageReward.m_startTime = ServerInterface.MileageMapState.m_chapterStartTime;
			this.m_rewardList.Add(serverMileageReward);
		}
	}

	private ServerMileageReward AnalyzeMileageRewardJson(JsonData jdata)
	{
		if (jdata == null)
		{
			return null;
		}
		ServerMileageReward serverMileageReward = new ServerMileageReward();
		if (serverMileageReward != null)
		{
			serverMileageReward.m_episode = this.m_episode;
			serverMileageReward.m_chapter = this.m_chapter;
			serverMileageReward.m_type = NetUtil.GetJsonInt(jdata, "type");
			serverMileageReward.m_point = NetUtil.GetJsonInt(jdata, "point");
			serverMileageReward.m_itemId = NetUtil.GetJsonInt(jdata, "itemId");
			serverMileageReward.m_numItem = NetUtil.GetJsonInt(jdata, "numItem");
			serverMileageReward.m_limitTime = NetUtil.GetJsonInt(jdata, "limitTime");
		}
		return serverMileageReward;
	}
}
