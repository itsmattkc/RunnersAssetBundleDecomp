using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetMileageData : NetBase
{
	private string[] m_distanceFriendList;

	private ServerMileageMapState _resultMileageMapState_k__BackingField;

	private List<ServerMileageFriendEntry> _m_resultMileageFriendList_k__BackingField;

	public ServerMileageMapState resultMileageMapState
	{
		get;
		private set;
	}

	public List<ServerMileageFriendEntry> m_resultMileageFriendList
	{
		get;
		private set;
	}

	public NetServerGetMileageData(string[] distanceFriendList)
	{
		this.m_distanceFriendList = distanceFriendList;
	}

	protected override void DoRequest()
	{
		base.SetAction("Game/getMileageData");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_MileageState(jdata);
		this.GetResponse_MileageFriendList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_DistanceFriendList()
	{
		if (this.m_distanceFriendList != null && this.m_distanceFriendList.Length != 0)
		{
			base.WriteActionParamArray("distanceFriendList", new List<object>(this.m_distanceFriendList));
		}
	}

	private void GetResponse_MileageState(JsonData jdata)
	{
		this.resultMileageMapState = NetUtil.AnalyzeMileageMapStateJson(jdata, "mileageMapState");
	}

	private void GetResponse_MileageFriendList(JsonData jdata)
	{
		this.m_resultMileageFriendList = NetUtil.AnalyzeMileageFriendListJson(jdata, "mileageFriendList");
	}
}
