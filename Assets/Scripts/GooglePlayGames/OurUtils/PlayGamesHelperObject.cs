using System;
using System.Collections.Generic;
using UnityEngine;

namespace GooglePlayGames.OurUtils
{
	public class PlayGamesHelperObject : MonoBehaviour
	{
		private static PlayGamesHelperObject instance = null;

		private static bool sIsDummy = false;

		private static List<Action> sQueue = new List<Action>();

		private static volatile bool sQueueEmpty = true;

		private static Action<bool> sPauseCallback = null;

		private static Action<bool> sFocusCallback = null;

		public static void CreateObject()
		{
			if (PlayGamesHelperObject.instance != null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				GameObject gameObject = new GameObject("PlayGames_QueueRunner");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				PlayGamesHelperObject.instance = gameObject.AddComponent<PlayGamesHelperObject>();
			}
			else
			{
				PlayGamesHelperObject.instance = new PlayGamesHelperObject();
				PlayGamesHelperObject.sIsDummy = true;
			}
		}

		private void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		private void OnDisable()
		{
			if (PlayGamesHelperObject.instance == this)
			{
				PlayGamesHelperObject.instance = null;
			}
		}

		public static void RunOnGameThread(Action action)
		{
			if (PlayGamesHelperObject.sIsDummy)
			{
				return;
			}
			List<Action> obj = PlayGamesHelperObject.sQueue;
			lock (obj)
			{
				PlayGamesHelperObject.sQueue.Add(action);
				PlayGamesHelperObject.sQueueEmpty = false;
			}
		}

		private void Update()
		{
			if (PlayGamesHelperObject.sIsDummy)
			{
				return;
			}
			if (PlayGamesHelperObject.sQueueEmpty)
			{
				return;
			}
			List<Action> list = new List<Action>();
			List<Action> obj = PlayGamesHelperObject.sQueue;
			lock (obj)
			{
				foreach (Action current in PlayGamesHelperObject.sQueue)
				{
					list.Add(current);
				}
				PlayGamesHelperObject.sQueue.Clear();
				PlayGamesHelperObject.sQueueEmpty = true;
			}
			foreach (Action current2 in list)
			{
				current2();
			}
		}

		private void OnApplicationFocus(bool focused)
		{
			Logger.d("PlayGamesHelperObject.OnApplicationFocus " + focused);
			if (PlayGamesHelperObject.sFocusCallback != null)
			{
				PlayGamesHelperObject.sFocusCallback(focused);
			}
		}

		private void OnApplicationPause(bool paused)
		{
			Logger.d("PlayGamesHelperObject.OnApplicationPause " + paused);
			if (PlayGamesHelperObject.sPauseCallback != null)
			{
				PlayGamesHelperObject.sPauseCallback(paused);
			}
		}

		public static void SetFocusCallback(Action<bool> callback)
		{
			PlayGamesHelperObject.sFocusCallback = callback;
		}

		public static void SetPauseCallback(Action<bool> callback)
		{
			PlayGamesHelperObject.sPauseCallback = callback;
		}
	}
}
