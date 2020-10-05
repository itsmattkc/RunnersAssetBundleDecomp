using SaveData;
using System;
using UnityEngine;

public class SystemSettings : MonoBehaviour
{
	public enum QualityLevel
	{
		Normal,
		Low
	}

	private class SystemInformation
	{
		public SystemSettings.QualityLevel m_qualityLevel;

		public string m_deviceModel;

		public int m_targetFrameRate = 60;

		public int m_unityQualityLevel;
	}

	private static SystemSettings.SystemInformation m_information = new SystemSettings.SystemInformation();

	private static SystemSettings instance = null;

	public static int TargetFrameRate
	{
		get
		{
			if (SystemSettings.m_information != null)
			{
				return SystemSettings.m_information.m_targetFrameRate;
			}
			return 60;
		}
		set
		{
			if (SystemSettings.m_information != null)
			{
				SystemSettings.m_information.m_targetFrameRate = value;
			}
		}
	}

	public static string DeviceModel
	{
		get
		{
			if (SystemSettings.m_information != null)
			{
				return SystemSettings.m_information.m_deviceModel;
			}
			return null;
		}
	}

	public static SystemSettings Instance
	{
		get
		{
			if (SystemSettings.instance == null)
			{
				SystemSettings.instance = (UnityEngine.Object.FindObjectOfType(typeof(SystemSettings)) as SystemSettings);
			}
			return SystemSettings.instance;
		}
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.orientation = ScreenOrientation.AutoRotation;
		this.InitInformation();
	}

	private void Update()
	{
	}

	private void InitInformation()
	{
		SystemSettings.m_information.m_deviceModel = SystemInfo.deviceModel;
		SystemSettings.m_information.m_targetFrameRate = 60;
		SystemSettings.m_information.m_unityQualityLevel = QualitySettings.GetQualityLevel();
	}

	public static void ChangeQualityLevel(SystemSettings.QualityLevel level)
	{
		if (SystemSettings.m_information == null)
		{
			return;
		}
		SystemSettings.m_information.m_qualityLevel = level;
		if (level != SystemSettings.QualityLevel.Normal)
		{
			if (level == SystemSettings.QualityLevel.Low)
			{
				SystemSettings.m_information.m_targetFrameRate = 30;
				SystemSettings.m_information.m_unityQualityLevel = 0;
				QualitySettings.SetQualityLevel(SystemSettings.m_information.m_unityQualityLevel);
			}
		}
		else
		{
			SystemSettings.m_information.m_targetFrameRate = 60;
			SystemSettings.m_information.m_unityQualityLevel = 1;
			QualitySettings.SetQualityLevel(SystemSettings.m_information.m_unityQualityLevel);
		}
	}

	public static void ChangeQualityLevelBySaveData()
	{
		SystemData systemSaveData = SystemSaveManager.GetSystemSaveData();
		if (systemSaveData != null)
		{
			bool lightMode = systemSaveData.lightMode;
			SystemSettings.ChangeQualityLevel((!lightMode) ? SystemSettings.QualityLevel.Normal : SystemSettings.QualityLevel.Low);
		}
	}

	private bool CheckInstance()
	{
		if (SystemSettings.instance == null)
		{
			SystemSettings.instance = this;
			return true;
		}
		if (this == SystemSettings.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}

	private void OnDestroy()
	{
		if (SystemSettings.instance == this)
		{
			SystemSettings.instance = null;
		}
	}

	protected void Awake()
	{
		this.CheckInstance();
	}
}
