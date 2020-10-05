using LitJson;
using SaveData;
using System;
using System.Collections.Generic;

public class NetServerGetNoticeInfo : NetBase
{
	private List<NetNoticeItem> m_noticeItems;

	public NetServerGetNoticeInfo()
	{
		this.m_noticeItems = new List<NetNoticeItem>();
		base.SetSecureFlag(false);
	}

	protected override void DoRequest()
	{
		base.SetAction("login/getInformation");
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			string onlySendBaseParamString = instance.GetOnlySendBaseParamString();
			base.WriteJsonString(onlySendBaseParamString);
		}
	}

	protected override void DoDidSuccess(JsonData jdata)
	{
		this.m_noticeItems.Clear();
		JsonData jsonArray = NetUtil.GetJsonArray(jdata, "informations");
		int count = jsonArray.Count;
		for (int i = 0; i < count; i++)
		{
			JsonData jdata2 = jsonArray[i];
			long jsonLong = NetUtil.GetJsonLong(jdata2, "id");
			int jsonInt = NetUtil.GetJsonInt(jdata2, "priority");
			long jsonLong2 = NetUtil.GetJsonLong(jdata2, "start");
			long jsonLong3 = NetUtil.GetJsonLong(jdata2, "end");
			string jsonString = NetUtil.GetJsonString(jdata2, "param");
			this.AddInfo(jsonLong, jsonInt, jsonLong2, jsonLong3, jsonString, string.Empty);
		}
		JsonData jsonArray2 = NetUtil.GetJsonArray(jdata, "operatorEachInfos");
		int count2 = jsonArray2.Count;
		for (int j = 0; j < count2; j++)
		{
			JsonData jdata3 = jsonArray2[j];
			long num = (long)NetNoticeItem.OPERATORINFO_START_ID + NetUtil.GetJsonLong(jdata3, "id");
			int priority = -10000;
			long start = 0L;
			long end = 0L;
			string jsonString2 = NetUtil.GetJsonString(jdata3, "content");
			string saveKey = string.Empty;
			string param = "1_" + jsonString2 + "_0_0_url";
			if (num == (long)NetNoticeItem.OPERATORINFO_RANKINGRESULT_ID)
			{
				priority = -10001;
				string[] array = jsonString2.Split(new char[]
				{
					','
				});
				if (array.Length >= 10)
				{
					saveKey = array[5] + array[6];
				}
			}
			if (num == (long)NetNoticeItem.OPERATORINFO_QUICKRANKINGRESULT_ID)
			{
				priority = -10002;
				string[] array2 = jsonString2.Split(new char[]
				{
					','
				});
				if (array2.Length >= 10)
				{
					saveKey = array2[5] + array2[6] + "QUICK";
				}
			}
			else if (num == (long)NetNoticeItem.OPERATORINFO_EVENTRANKINGRESULT_ID)
			{
				priority = -10003;
				string text = "0";
				EventRankingServerInfoConverter eventRankingServerInfoConverter = new EventRankingServerInfoConverter(jsonString2);
				if (eventRankingServerInfoConverter != null)
				{
					if (eventRankingServerInfoConverter.eventId > 0)
					{
						text = "EventResult" + eventRankingServerInfoConverter.eventId.ToString();
					}
					saveKey = eventRankingServerInfoConverter.eventId.ToString();
				}
				param = string.Concat(new string[]
				{
					"1_",
					jsonString2,
					"_",
					text,
					"_0_url"
				});
			}
			this.AddInfo(num, priority, start, end, param, saveKey);
		}
		ServerInterface.PlayerState.m_numUnreadMessages += NetUtil.GetJsonInt(jdata, "numOperatorInfo");
		this.UpdateInformationData();
	}

	protected override void DoDidSuccessEmulation()
	{
		long unixTime = NetUtil.GetUnixTime(new DateTime(2014, 9, 1, 14, 0, 0, 0));
		long unixTime2 = NetUtil.GetUnixTime(new DateTime(2016, 11, 11, 14, 0, 0, 0));
		this.AddInfo((long)NetNoticeItem.OPERATORINFO_RANKINGRESULT_ID, -10001, 0L, 0L, "0_1,2,3,4,1,100000,900000,1,1,9876,1,1_0_0", unixTime.ToString() + unixTime2.ToString());
	}

	public NetNoticeItem GetInfo(int index)
	{
		if (index < this.m_noticeItems.Count)
		{
			return this.m_noticeItems[index];
		}
		return null;
	}

	public int GetInfoCount()
	{
		return this.m_noticeItems.Count;
	}

	private void UpdateInformationData()
	{
		InformationSaveManager instance = InformationSaveManager.Instance;
		if (instance != null)
		{
			InformationData informationData = instance.GetInformationData();
			int num = informationData.DataCount();
			for (int i = 0; i < num; i++)
			{
				string data = informationData.GetData(i, InformationData.DataType.ID);
				if (data != InformationData.INVALID_PARAM)
				{
					bool flag = false;
					if (long.Parse(data) == (long)NetNoticeItem.OPERATORINFO_EVENTRANKINGRESULT_ID)
					{
						foreach (NetNoticeItem current in this.m_noticeItems)
						{
							if (current.SaveKey == informationData.GetData(i, InformationData.DataType.ADD_INFO))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							InformationImageManager instance2 = InformationImageManager.Instance;
							if (instance2 != null)
							{
								string data2 = informationData.GetData(i, InformationData.DataType.IMAGE_ID);
								instance2.DeleteImageData(data2);
							}
							informationData.Reset(i);
						}
					}
					else
					{
						foreach (NetNoticeItem current2 in this.m_noticeItems)
						{
							if (current2.Id.ToString() == data)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							informationData.Reset(i);
						}
					}
				}
			}
			instance.SaveInformationData();
		}
	}

	private void AddInfo(long id, int priority, long start, long end, string param, string saveKey)
	{
		NetNoticeItem netNoticeItem = new NetNoticeItem();
		netNoticeItem.Init(id, priority, start, end, param, saveKey);
		this.m_noticeItems.Add(netNoticeItem);
	}
}
