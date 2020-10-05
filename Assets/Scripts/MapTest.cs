using System;
using UnityEngine;

public class MapTest : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("MapTest2");
		}
	}
}
