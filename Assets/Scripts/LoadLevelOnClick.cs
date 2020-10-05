using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Load Level On Click")]
public class LoadLevelOnClick : MonoBehaviour
{
	public string levelName;

	private void OnClick()
	{
		if (!string.IsNullOrEmpty(this.levelName))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(this.levelName);
		}
	}
}
