using System;
using UnityEngine;

public class LoadingInfo : MonoBehaviour
{
	public class LoadingData
	{
		public string m_titleText;

		public string m_mainText;

		public string m_bonusNameText;

		public string m_bonusValueText;

		public Texture m_texture;

		public bool m_optionTutorial;
	}

	private LoadingInfo.LoadingData m_loadingData;

	private void Start()
	{
		base.enabled = false;
	}

	public void SetInfo(LoadingInfo.LoadingData data)
	{
		this.m_loadingData = data;
	}

	public LoadingInfo.LoadingData GetInfo()
	{
		return this.m_loadingData;
	}

	public void ResetData()
	{
		this.m_loadingData = new LoadingInfo.LoadingData();
	}

	public static LoadingInfo CreateLoadingInfo()
	{
		LoadingInfo loadingInfo = null;
		LoadingInfo loadingInfo2 = GameObjectUtil.FindGameObjectComponent<LoadingInfo>("LoadingInfo");
		if (loadingInfo2 != null)
		{
			UnityEngine.Object.Destroy(loadingInfo2.gameObject);
		}
		GameObject gameObject = new GameObject("LoadingInfo");
		if (gameObject != null)
		{
			loadingInfo = gameObject.AddComponent<LoadingInfo>();
			loadingInfo.ResetData();
			UnityEngine.Object.DontDestroyOnLoad(loadingInfo.gameObject);
		}
		return loadingInfo;
	}
}
