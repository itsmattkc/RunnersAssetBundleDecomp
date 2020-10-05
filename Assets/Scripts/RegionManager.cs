using SaveData;
using System;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
	private static RegionManager m_instance;

	private RegionInfoTable m_table;

	public static RegionManager Instance
	{
		get
		{
			return RegionManager.m_instance;
		}
		private set
		{
		}
	}

	public RegionInfo GetRegionInfo()
	{
		if (this.m_table != null)
		{
			string countryCode = SystemSaveManager.GetCountryCode();
			return this.m_table.GetInfo(countryCode);
		}
		return null;
	}

	public bool IsJapan()
	{
		RegionInfo regionInfo = this.GetRegionInfo();
		return regionInfo != null && regionInfo.CountryCode == "JP";
	}

	public bool IsNeedIapMessage()
	{
		return false;
	}

	public bool IsNeedESRB()
	{
		RegionInfo regionInfo = this.GetRegionInfo();
		bool result = true;
		if (regionInfo != null && !string.IsNullOrEmpty(regionInfo.Limit))
		{
			string limit = regionInfo.Limit;
			if (limit.IndexOf("ESRB") == -1 && limit.IndexOf("esrb") == -1 && limit.IndexOf("Esrb") == -1)
			{
				result = false;
			}
		}
		return result;
	}

	public bool IsUseSNS()
	{
		bool result = true;
		if (this.IsNeedESRB())
		{
			result = false;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			int num = 1;
			if (loggedInServerInterface != null)
			{
				ServerSettingState settingState = ServerInterface.SettingState;
				if (settingState != null && !string.IsNullOrEmpty(settingState.m_birthday))
				{
					num = HudUtility.GetAge(DateTime.Parse(settingState.m_birthday), NetUtil.GetCurrentTime());
				}
			}
			if (num >= 13)
			{
				result = true;
			}
		}
		return result;
	}

	public bool IsUseHardlightAds()
	{
		RegionInfo regionInfo = this.GetRegionInfo();
		if (regionInfo == null)
		{
			return false;
		}
		if (regionInfo.CountryCode == "US")
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			int num = 1;
			if (loggedInServerInterface != null)
			{
				ServerSettingState settingState = ServerInterface.SettingState;
				if (settingState != null && !string.IsNullOrEmpty(settingState.m_birthday))
				{
					num = HudUtility.GetAge(DateTime.Parse(settingState.m_birthday), NetUtil.GetCurrentTime());
				}
			}
			return num >= 13;
		}
		return true;
	}

	private void Awake()
	{
		if (RegionManager.m_instance == null)
		{
			RegionManager.m_instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			this.Init();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Init()
	{
		this.m_table = new RegionInfoTable();
	}

	private void OnDestroy()
	{
		if (RegionManager.m_instance == this)
		{
			RegionManager.m_instance = null;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
