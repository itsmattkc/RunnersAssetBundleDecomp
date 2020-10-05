using System;
using UnityEngine;

public class GameModeTitleReset : MonoBehaviour
{
	private bool m_isLoadedLevel;

	private void Start()
	{
	}

	private void Update()
	{
		if (!this.m_isLoadedLevel)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(TitleDefine.TitleSceneName);
			this.m_isLoadedLevel = true;
		}
	}
}
