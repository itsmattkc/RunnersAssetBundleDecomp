using System;
using UnityEngine;

public class MainMenuReset : MonoBehaviour
{
	private bool m_isLoadedScene;

	private void Start()
	{
	}

	private void Update()
	{
		if (!this.m_isLoadedScene)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
		}
	}
}
