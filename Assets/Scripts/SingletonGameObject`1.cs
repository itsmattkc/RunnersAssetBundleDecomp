using System;
using UnityEngine;

public class SingletonGameObject<T> : MonoBehaviour where T : MonoBehaviour
{
	[Header("シーン切替時の削除設定(true:削除)"), SerializeField]
	private bool m_isOnLoadDestroy;

	private static bool s_isDestroy;

	private static T s_instance;

	public static T Instance
	{
		get
		{
			if (SingletonGameObject<T>.s_instance == null && !SingletonGameObject<T>.s_isDestroy)
			{
				SingletonGameObject<T>.s_instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
				if (SingletonGameObject<T>.s_instance != null)
				{
					string text = typeof(T).ToString();
					if (text.IndexOf("Debug") != -1 || text.IndexOf("debug") != -1)
					{
						UnityEngine.Object.Destroy(SingletonGameObject<T>.s_instance.gameObject);
						SingletonGameObject<T>.s_instance = (T)((object)null);
						SingletonGameObject<T>.s_isDestroy = true;
						global::Debug.Log("debug SingletonGameObject auto delete !!! :" + text);
					}
				}
			}
			return SingletonGameObject<T>.s_instance;
		}
	}

	private void Awake()
	{
		if (this != SingletonGameObject<T>.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SingletonGameObject<T>.s_instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
		if (!this.m_isOnLoadDestroy)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	public static void Remove()
	{
		if (SingletonGameObject<T>.s_instance != null)
		{
			UnityEngine.Object.Destroy(SingletonGameObject<T>.s_instance.gameObject);
			SingletonGameObject<T>.s_instance = (T)((object)null);
			SingletonGameObject<T>.s_isDestroy = true;
		}
	}
}
