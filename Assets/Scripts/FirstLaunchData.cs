using System;
using UnityEngine;

public class FirstLaunchData : MonoBehaviour
{
	public enum Type
	{
		TYPE_NONE = -1,
		TYPE_GET_CHAOEGG,
		TYPE_MILEAGE_START,
		TYPE_MILEAGE_AFTER_TUTORIAL,
		TYPE_MILEAGE_BOSS_LOSE_FIRST,
		TYPE_MILEAGE_BOSS_LOSE,
		TYPE_NUM
	}

	private static FirstLaunchData m_instance;

	private bool[] m_isLaunched = new bool[5];

	public bool IsDone(FirstLaunchData.Type type)
	{
		if (type >= FirstLaunchData.Type.TYPE_NUM)
		{
			global::Debug.Log("FirstLaunchData.IsDone: Invalid parameter");
			return false;
		}
		return this.m_isLaunched[(int)type];
	}

	public void Register(FirstLaunchData.Type type, bool isLaunched)
	{
		if (type >= FirstLaunchData.Type.TYPE_NUM)
		{
			global::Debug.Log("FirstLaunchData.Register: Invalid parameter");
			return;
		}
		this.m_isLaunched[(int)type] = isLaunched;
		this.StoreSaveData();
	}

	private void Awake()
	{
		if (FirstLaunchData.m_instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			FirstLaunchData.m_instance = this;
			this.LoadSaveData();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		FirstLaunchData.m_instance = null;
	}

	private void LoadSaveData()
	{
	}

	private void StoreSaveData()
	{
	}
}
