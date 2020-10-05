using App;
using SaveData;
using System;
using UnityEngine;

public class FoxManager : MonoBehaviour
{
	private static int[] m_ltvIDTable;

	private static FoxManager instance;

	public static FoxManager Instance
	{
		get
		{
			return FoxManager.instance;
		}
	}

	private void Awake()
	{
		if (this.CheckInstance())
		{
			if (!Env.isReleaseApplication)
			{
			}
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	private void Start()
	{
		FoxManager.m_ltvIDTable = FoxDataTable.LtvIDAndroid;
	}

	private void Update()
	{
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
		}
	}

	private void OnApplicationQuit()
	{
	}

	public static void SendLtvPoint(FoxLtvType ltvType)
	{
		string text;
		if (!FoxManager.IsEnableSendLtvPoint(out text))
		{
			return;
		}
		int num = FoxManager.m_ltvIDTable[(int)ltvType];
	}

	public static void SendLtvPointMap(int rank)
	{
		string text;
		if (!FoxManager.IsEnableSendLtvPoint(out text))
		{
			return;
		}
		int num = FoxManager.m_ltvIDTable[2];
		FoxPlugin.addParameter("rank", rank.ToString());
	}

	public static void SendLtvPointBuyRSR(int rsrType)
	{
		string text;
		if (!FoxManager.IsEnableSendLtvPoint(out text))
		{
			return;
		}
		int num = FoxManager.m_ltvIDTable[4];
		FoxPlugin.addParameter("rsr", rsrType.ToString());
	}

	public static void SendLtvPointPremiumRoulette(bool free)
	{
		string text;
		if (!FoxManager.IsEnableSendLtvPoint(out text))
		{
			return;
		}
		int num = (!free) ? 5 : 6;
		int num2 = FoxManager.m_ltvIDTable[num];
	}

	private static bool IsEnableSendLtvPoint(out string userID)
	{
		userID = SystemSaveManager.GetGameID();
		return !string.IsNullOrEmpty(userID) && SystemSaveManager.IsUserIDValid() && (FoxManager.m_ltvIDTable == null || FoxManager.Instance == null) && false;
	}

	private bool CheckInstance()
	{
		if (FoxManager.instance == null)
		{
			FoxManager.instance = this;
			return true;
		}
		if (this == FoxManager.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}

	private void OnDestroy()
	{
		if (FoxManager.instance == this)
		{
			FoxManager.instance = null;
		}
	}
}
