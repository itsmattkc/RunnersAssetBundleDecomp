using SaveData;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ServerNoticeInfo
{
	private DateTime m_lastUpdateInfoTime = DateTime.MinValue;

	private bool _m_isGetNoticeInfo_k__BackingField;

	private bool _m_isShowedNoticeInfo_k__BackingField;

	private List<NetNoticeItem> _m_noticeItems_k__BackingField;

	private List<NetNoticeItem> _m_rouletteItems_k__BackingField;

	private List<NetNoticeItem> _m_eventItems_k__BackingField;

	public bool m_isGetNoticeInfo
	{
		get;
		set;
	}

	public bool m_isShowedNoticeInfo
	{
		get;
		set;
	}

	public List<NetNoticeItem> m_noticeItems
	{
		get;
		set;
	}

	public List<NetNoticeItem> m_rouletteItems
	{
		get;
		set;
	}

	public List<NetNoticeItem> m_eventItems
	{
		get;
		set;
	}

	public DateTime LastUpdateInfoTime
	{
		get
		{
			return this.m_lastUpdateInfoTime;
		}
		set
		{
			this.m_lastUpdateInfoTime = value;
		}
	}

	public ServerNoticeInfo()
	{
		this.m_noticeItems = new List<NetNoticeItem>();
		this.m_rouletteItems = new List<NetNoticeItem>();
		this.m_eventItems = new List<NetNoticeItem>();
		this.m_isGetNoticeInfo = false;
		this.m_isShowedNoticeInfo = false;
	}

	public bool IsNeedUpdateInfo()
	{
		DateTime currentTime = NetUtil.GetCurrentTime();
		TimeSpan t = currentTime - this.m_lastUpdateInfoTime;
		TimeSpan t2 = new TimeSpan(1, 0, 0);
		return t >= t2;
	}

	public bool IsAllChecked()
	{
		if (!this.m_isGetNoticeInfo)
		{
			return true;
		}
		if (this.m_isShowedNoticeInfo)
		{
			return true;
		}
		bool flag = true;
		int count = this.m_noticeItems.Count;
		for (int i = 0; i < count; i++)
		{
			flag &= this.IsChecked(this.m_noticeItems[i]);
		}
		return flag;
	}

	public NetNoticeItem GetInfo(int index)
	{
		NetNoticeItem result = null;
		if (this.m_noticeItems.Count > index)
		{
			result = this.m_noticeItems[index];
		}
		return result;
	}

	public void Clear()
	{
		this.m_noticeItems.Clear();
		this.m_rouletteItems.Clear();
		this.m_eventItems.Clear();
		this.m_isGetNoticeInfo = false;
	}

	public bool IsChecked(NetNoticeItem item)
	{
		bool result = false;
		InformationSaveManager instance = InformationSaveManager.Instance;
		if (instance != null)
		{
			InformationData informationData = instance.GetInformationData();
			if (informationData != null && item != null)
			{
				string data = informationData.GetData(item.Id.ToString(), InformationData.DataType.SHOWED_TIME);
				if (data != InformationData.INVALID_PARAM)
				{
					if (item.IsEveryDay())
					{
						result = true;
						DateTime localDateTime = NetUtil.GetLocalDateTime(long.Parse(data));
						DateTime localDateTime2 = NetUtil.GetLocalDateTime((long)NetUtil.GetCurrentUnixTime());
						if (localDateTime.Day != localDateTime2.Day)
						{
							result = false;
						}
						if (localDateTime.Month != localDateTime2.Month)
						{
							result = false;
						}
						if (localDateTime.Year != localDateTime2.Year)
						{
							result = false;
						}
					}
					else if (item.IsOnce())
					{
						if (item.Id == (long)NetNoticeItem.OPERATORINFO_RANKINGRESULT_ID)
						{
							string data2 = informationData.GetData(item.Id.ToString(), InformationData.DataType.ADD_INFO);
							result = (item.SaveKey == data2);
						}
						else if (item.Id == (long)NetNoticeItem.OPERATORINFO_EVENTRANKINGRESULT_ID)
						{
							string eventRankingData = informationData.GetEventRankingData(item.Id.ToString(), item.SaveKey, InformationData.DataType.ADD_INFO);
							result = (item.SaveKey == eventRankingData);
						}
						else if (item.Id == (long)NetNoticeItem.OPERATORINFO_QUICKRANKINGRESULT_ID)
						{
							string data3 = informationData.GetData(item.Id.ToString(), InformationData.DataType.ADD_INFO);
							result = (item.SaveKey == data3);
						}
						else
						{
							result = true;
						}
					}
					else if (item.IsOnlyInformationPage())
					{
						result = true;
					}
				}
			}
		}
		return result;
	}

	public bool IsCheckedForMenuIcon(NetNoticeItem item)
	{
		InformationSaveManager instance = InformationSaveManager.Instance;
		if (instance != null)
		{
			InformationData informationData = instance.GetInformationData();
			if (informationData != null && item != null)
			{
				string data = informationData.GetData(item.Id.ToString(), InformationData.DataType.SHOWED_TIME);
				if (data != InformationData.INVALID_PARAM)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsOnTime(NetNoticeItem item)
	{
		if (item != null)
		{
			long num = (long)NetUtil.GetCurrentUnixTime();
			if (num >= item.Start && item.End > num)
			{
				return true;
			}
		}
		return false;
	}

	public void UpdateChecked(NetNoticeItem item)
	{
		if (item != null)
		{
			InformationSaveManager instance = InformationSaveManager.Instance;
			if (instance != null)
			{
				InformationData informationData = instance.GetInformationData();
				if (informationData != null)
				{
					long num = (long)NetUtil.GetCurrentUnixTime();
					if (item.Id == (long)NetNoticeItem.OPERATORINFO_RANKINGRESULT_ID)
					{
						informationData.UpdateShowedTime(item.Id.ToString(), num.ToString(), item.SaveKey, "-1");
					}
					else if (item.Id == (long)NetNoticeItem.OPERATORINFO_EVENTRANKINGRESULT_ID)
					{
						informationData.UpdateEventRankingShowedTime(item.Id.ToString(), num.ToString(), item.SaveKey, item.ImageId);
					}
					else if (item.Id == (long)NetNoticeItem.OPERATORINFO_QUICKRANKINGRESULT_ID)
					{
						informationData.UpdateShowedTime(item.Id.ToString(), num.ToString(), item.SaveKey, "-1");
					}
					else
					{
						informationData.UpdateShowedTime(item.Id.ToString(), num.ToString(), item.End.ToString(), item.ImageId);
					}
				}
			}
		}
	}

	public void SaveInformation()
	{
		InformationSaveManager instance = InformationSaveManager.Instance;
		if (instance != null)
		{
			instance.SaveInformationData();
		}
	}
}
