using LitJson;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class NetServerGetFriendUserIdList : NetBase
{
	public List<ServerUserTransformData> resultTransformDataList;

	private List<string> _paramFriendFBIdList_k__BackingField;

	public List<string> paramFriendFBIdList
	{
		get;
		set;
	}

	public NetServerGetFriendUserIdList() : this(null)
	{
	}

	public NetServerGetFriendUserIdList(List<string> friendFBIdList)
	{
		this.paramFriendFBIdList = friendFBIdList;
	}

	protected override void DoRequest()
	{
		base.SetAction("Friend/getFriendUserIdList");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string getFacebookFriendUserIdList = instance.GetGetFacebookFriendUserIdList(this.paramFriendFBIdList);
			UnityEngine.Debug.Log("CPlusPlusLink.actRetry");
			base.WriteJsonString(getFacebookFriendUserIdList);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.GetResponse_TransformDataList(jdata);
	}

	protected override void DoDidSuccessEmulation()
	{
	}

	private void SetParameter_FriendIdList()
	{
		List<object> list = new List<object>();
		foreach (string current in this.paramFriendFBIdList)
		{
			if (!string.IsNullOrEmpty(current))
			{
				list.Add(current);
			}
		}
		base.WriteActionParamArray("facebookIdList", list);
	}

	private void GetResponse_TransformDataList(JsonData jdata)
	{
		this.resultTransformDataList = NetUtil.AnalyzeUserTransformData(jdata);
	}
}
