using System;

public class NetNoticeItem
{
	private const int ANNOUNCE_TYPE_EVERYDAY = 0;

	private const int ANNOUNCE_TYPE_ONCE = 1;

	private const int ANNOUNCE_TYPE_FULLTIME = 2;

	private const int ANNOUNCE_TYPE_NOT_POP_UP = 3;

	public static int OPERATORINFO_START_ID = 1000000000;

	public static int OPERATORINFO_RANKINGRESULT_ID = NetNoticeItem.OPERATORINFO_START_ID;

	public static int OPERATORINFO_EVENTRANKINGRESULT_ID = NetNoticeItem.OPERATORINFO_START_ID + 1;

	public static int OPERATORINFO_QUICKRANKINGRESULT_ID = NetNoticeItem.OPERATORINFO_START_ID + 2;

	private long m_id;

	private int m_priority;

	private long m_start;

	private long m_end;

	private int m_announceType;

	private int m_windowType;

	private string m_imageId;

	private string m_message;

	private string m_webAdress;

	private string m_saveKey;

	private string m_designatedArea;

	public long Id
	{
		get
		{
			return this.m_id;
		}
	}

	public int Priority
	{
		get
		{
			return this.m_priority;
		}
	}

	public long Start
	{
		get
		{
			return this.m_start;
		}
	}

	public long End
	{
		get
		{
			return this.m_end;
		}
	}

	public int AnnounceType
	{
		get
		{
			return this.m_announceType;
		}
	}

	public int WindowType
	{
		get
		{
			return this.m_windowType;
		}
	}

	public string ImageId
	{
		get
		{
			return this.m_imageId;
		}
	}

	public string Message
	{
		get
		{
			return this.m_message;
		}
	}

	public string Adress
	{
		get
		{
			return this.m_webAdress;
		}
	}

	public string SaveKey
	{
		get
		{
			return this.m_saveKey;
		}
	}

	public void Init(long id, int priority, long start, long end, string param, string saveKey)
	{
		this.m_id = id;
		this.m_priority = priority;
		this.m_start = start;
		this.m_end = end;
		this.m_saveKey = saveKey;
		string[] array = param.Split(new char[]
		{
			'_'
		});
		this.m_announceType = 0;
		this.m_imageId = "-1";
		this.m_message = string.Empty;
		this.m_windowType = 0;
		this.m_webAdress = string.Empty;
		this.m_designatedArea = string.Empty;
		if (array.Length > 0)
		{
			int.TryParse(array[0], out this.m_announceType);
		}
		if (array.Length > 1)
		{
			this.m_message = array[1];
		}
		if (array.Length > 2)
		{
			this.m_imageId = array[2];
		}
		if (array.Length > 3)
		{
			int.TryParse(array[3], out this.m_windowType);
		}
		if (array.Length == 5)
		{
			if (this.m_windowType == 16 || this.m_windowType == 17)
			{
				this.m_designatedArea = array[4];
			}
			else
			{
				this.m_webAdress = array[4];
			}
		}
		else if (array.Length > 5 && (this.m_windowType != 16 || this.m_windowType == 17))
		{
			for (int i = 4; i < array.Length; i++)
			{
				if (i == 4)
				{
					this.m_webAdress = array[i];
				}
				else
				{
					this.m_webAdress = this.m_webAdress + "_" + array[i];
				}
			}
		}
	}

	public bool IsEveryDay()
	{
		return 0 == this.m_announceType;
	}

	public bool IsOnce()
	{
		return 1 == this.m_announceType;
	}

	public bool IsFullTime()
	{
		return 2 == this.m_announceType;
	}

	public bool IsOnlyInformationPage()
	{
		return 3 == this.m_announceType;
	}

	public bool IsOutsideDesignatedArea()
	{
		bool result = false;
		if (this.m_windowType == 16 || this.m_windowType == 17)
		{
			result = true;
			if (RegionManager.Instance != null)
			{
				RegionInfo regionInfo = RegionManager.Instance.GetRegionInfo();
				if (regionInfo != null && !string.IsNullOrEmpty(this.m_designatedArea) && !string.IsNullOrEmpty(regionInfo.CountryCode) && this.m_designatedArea == regionInfo.CountryCode)
				{
					result = false;
				}
			}
		}
		return result;
	}
}
