using System;
using UnityEngine;

public class AndroidObserver : MonoBehaviour
{
	private static AndroidObserver instance;

	private void Awake()
	{
		if (AndroidObserver.instance == null)
		{
			AndroidObserver.instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void OnBeforePurchaseFinishedSuccess(string message)
	{
		global::Debug.Log("AndroidObserver : OnBeforePurchaseFinishedSuccess :" + message);
		NativeObserver.Instance.OnBeforePurchaseFinishedSuccess(message);
	}

	public void OnPurchaseFinishedSuccess(string message)
	{
		global::Debug.Log("AndroidObserver : OnPurchaseFinishedSuccess :" + message);
		NativeObserver.Instance.OnPurchaseFinishedSuccess(message);
	}

	public void OnPurchaseFinishedFailed(string message)
	{
		global::Debug.Log("AndroidObserver : OnPurchaseFinishedFailed :" + message);
		NativeObserver.Instance.OnPurchaseFinishedFailed(message);
	}

	public void OnPurchaseFinishedCancel(string message)
	{
		global::Debug.Log("AndroidObserver : OnPurchaseFinishedCancel :" + message);
		NativeObserver.Instance.OnPurchaseFinishedCancel(message);
	}
}
