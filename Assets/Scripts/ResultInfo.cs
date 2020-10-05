using System;
using UnityEngine;

public class ResultInfo : MonoBehaviour
{
	private ResultData m_resultData;

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
	}

	public void SetInfo(ResultData data)
	{
		this.m_resultData = data;
	}

	public ResultData GetInfo()
	{
		return this.m_resultData;
	}

	public void ResetData()
	{
		this.m_resultData = new ResultData();
	}

	public static ResultInfo CreateResultInfo()
	{
		ResultInfo resultInfo = GameObjectUtil.FindGameObjectComponent<ResultInfo>("ResultInfo");
		if (resultInfo)
		{
			UnityEngine.Object.Destroy(resultInfo.gameObject);
		}
		GameObject gameObject = new GameObject("ResultInfo");
		ResultInfo resultInfo2 = gameObject.AddComponent<ResultInfo>();
		resultInfo2.ResetData();
		UnityEngine.Object.DontDestroyOnLoad(resultInfo2.gameObject);
		return resultInfo2;
	}

	public static void CreateOptionTutorialResultInfo()
	{
		ResultInfo resultInfo = GameObjectUtil.FindGameObjectComponent<ResultInfo>("ResultInfo");
		if (resultInfo)
		{
			UnityEngine.Object.Destroy(resultInfo.gameObject);
		}
		GameObject gameObject = new GameObject("ResultInfo");
		ResultInfo resultInfo2 = gameObject.AddComponent<ResultInfo>();
		resultInfo2.ResetData();
		resultInfo2.m_resultData.m_fromOptionTutorial = true;
		resultInfo2.m_resultData.m_validResult = false;
		UnityEngine.Object.DontDestroyOnLoad(resultInfo2.gameObject);
	}
}
