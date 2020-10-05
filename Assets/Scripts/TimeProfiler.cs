using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeProfiler : MonoBehaviour
{
	private Dictionary<string, float> m_checkList;

	public static bool m_active = true;

	private static TimeProfiler instance;

	public static TimeProfiler Instance
	{
		get
		{
			return TimeProfiler.instance;
		}
	}

	protected void Awake()
	{
		this.CheckInstance();
	}

	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Update()
	{
	}

	public static void StartCountTime(string index)
	{
		if (TimeProfiler.Instance == null)
		{
			return;
		}
		if (TimeProfiler.Instance.m_checkList == null)
		{
			return;
		}
		if (TimeProfiler.Instance.m_checkList.ContainsKey(index))
		{
			global::Debug.Log("TimeProfile:" + index + " is Counting Already.");
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		TimeProfiler.Instance.m_checkList.Add(index, realtimeSinceStartup);
	}

	public static float EndCountTime(string index)
	{
		if (TimeProfiler.Instance == null)
		{
			return 0f;
		}
		if (TimeProfiler.Instance.m_checkList == null)
		{
			return 0f;
		}
		if (TimeProfiler.Instance.m_checkList.ContainsKey(index))
		{
			float num = TimeProfiler.Instance.m_checkList[index];
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float result = realtimeSinceStartup - num;
			global::Debug.Log("LS:TimeProfile:Time Count:" + index + ":" + result.ToString("F3"));
			TimeProfiler.Instance.m_checkList.Remove(index);
			return result;
		}
		global::Debug.Log("TimeProfile:" + index + "is Not Found.");
		return 0f;
	}

	protected bool CheckInstance()
	{
		if (TimeProfiler.instance == null)
		{
			TimeProfiler.instance = this;
			return true;
		}
		if (this == TimeProfiler.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}

	private void OnDestroy()
	{
		if (this == TimeProfiler.instance)
		{
			TimeProfiler.instance = null;
		}
	}
}
