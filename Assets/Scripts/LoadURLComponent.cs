using System;
using UnityEngine;

public class LoadURLComponent : MonoBehaviour
{
	private static LoadURLComponent m_instance;

	private void Start()
	{
		if (LoadURLComponent.m_instance == null)
		{
			global::Debug.Log("LoadURLComponent:Created");
			LoadURLComponent.m_instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			this.Init();
		}
		else
		{
			global::Debug.Log("LoadURLComponent:Destroyed");
			UnityEngine.Object.Destroy(this);
		}
	}

	private void OnDestroy()
	{
	}

	private void Init()
	{
		DebugSaveServerUrl.LoadURL();
		global::Debug.Log("LoadURLComponent:LoadURL");
	}
}
