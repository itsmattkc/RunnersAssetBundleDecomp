using System;
using System.Runtime.CompilerServices;

public class ServerLoginState
{
	public string m_lineId;

	public string m_altLineId;

	public string m_lineAuthToken;

	public string m_segaAuthToken;

	private string m_sessionId;

	private ulong m_seqNum;

	public int sessionTimeLimit;

	private bool m_isChangeDataVersion;

	private int m_dataVersion;

	private bool m_isChangeAssetsVersion;

	private int m_assetsVersion;

	private string m_assetsVersionString;

	private string m_infoVersionString;

	private string _serverVersion_k__BackingField;

	private int _serverVersionValue_k__BackingField;

	private long _serverLastTime_k__BackingField;

	public bool IsLoggedIn
	{
		get
		{
			return this.sessionId != null && string.Empty != this.sessionId;
		}
	}

	public string sessionId
	{
		get
		{
			return this.m_sessionId;
		}
		set
		{
			this.m_sessionId = value;
		}
	}

	public ulong seqNum
	{
		get
		{
			return this.m_seqNum;
		}
		set
		{
			this.m_seqNum = value;
		}
	}

	public string serverVersion
	{
		get;
		set;
	}

	public int serverVersionValue
	{
		get;
		set;
	}

	public long serverLastTime
	{
		get;
		set;
	}

	public bool IsChangeDataVersion
	{
		get
		{
			return this.m_isChangeDataVersion;
		}
		set
		{
			this.m_isChangeDataVersion = value;
		}
	}

	public int DataVersion
	{
		get
		{
			return this.m_dataVersion;
		}
		set
		{
			if (value != this.m_dataVersion)
			{
				this.m_dataVersion = value;
				this.m_isChangeDataVersion = true;
			}
		}
	}

	public bool IsChangeAssetsVersion
	{
		get
		{
			return this.m_isChangeAssetsVersion;
		}
		set
		{
			this.m_isChangeAssetsVersion = value;
		}
	}

	public int AssetsVersion
	{
		get
		{
			return this.m_assetsVersion;
		}
		set
		{
			if (value != this.m_assetsVersion)
			{
				this.m_assetsVersion = value;
				this.m_isChangeAssetsVersion = true;
			}
		}
	}

	public string AssetsVersionString
	{
		get
		{
			return this.m_assetsVersionString;
		}
		set
		{
			if (value != this.m_assetsVersionString)
			{
				this.m_assetsVersionString = value;
				this.m_isChangeAssetsVersion = true;
			}
		}
	}

	public string InfoVersionString
	{
		get
		{
			return this.m_infoVersionString;
		}
		set
		{
			this.m_infoVersionString = value;
		}
	}

	public ServerLoginState()
	{
		this.m_lineId = null;
		this.m_altLineId = null;
		this.m_lineAuthToken = null;
		this.m_segaAuthToken = null;
		this.sessionId = null;
		this.seqNum = 0uL;
		this.m_isChangeDataVersion = false;
		this.m_dataVersion = -1;
		this.m_isChangeAssetsVersion = false;
		this.m_assetsVersion = -1;
	}
}
