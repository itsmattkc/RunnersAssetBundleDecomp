using System;
using UnityEngine;

public class CriAtomServer : MonoBehaviour
{
	private static CriAtomServer _instance;

	public Action<bool> onApplicationPausePreProcess;

	public Action<bool> onApplicationPausePostProcess;

	public static CriAtomServer instance
	{
		get
		{
			CriAtomServer.CreateInstance();
			return CriAtomServer._instance;
		}
	}

	public static void CreateInstance()
	{
		if (CriAtomServer._instance == null)
		{
			CriWare.managerObject.AddComponent<CriAtomServer>();
		}
	}

	public static void DestroyInstance()
	{
		if (CriAtomServer._instance != null)
		{
			UnityEngine.Object.Destroy(CriAtomServer._instance);
		}
	}

	private void Awake()
	{
		if (CriAtomServer._instance == null)
		{
			CriAtomServer._instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	private void OnDisable()
	{
		if (CriAtomServer._instance == this)
		{
			CriAtomServer._instance = null;
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (this.onApplicationPausePreProcess != null)
		{
			this.onApplicationPausePreProcess(pause);
		}
		CriAtomPlugin.Pause(pause);
		if (this.onApplicationPausePostProcess != null)
		{
			this.onApplicationPausePostProcess(pause);
		}
	}
}
