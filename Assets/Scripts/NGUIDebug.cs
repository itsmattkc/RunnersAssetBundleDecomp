using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Debug")]
public class NGUIDebug : MonoBehaviour
{
	private static List<string> mLines = new List<string>();

	private static NGUIDebug mInstance = null;

	public static void Log(string text)
	{
		if (Application.isPlaying)
		{
			if (NGUIDebug.mLines.Count > 20)
			{
				NGUIDebug.mLines.RemoveAt(0);
			}
			NGUIDebug.mLines.Add(text);
			if (NGUIDebug.mInstance == null)
			{
				GameObject gameObject = new GameObject("_NGUI Debug");
				NGUIDebug.mInstance = gameObject.AddComponent<NGUIDebug>();
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
		}
		else
		{
			global::Debug.Log(text);
		}
	}

	public static void DrawBounds(Bounds b)
	{
		Vector3 center = b.center;
		Vector3 vector = b.center - b.extents;
		Vector3 vector2 = b.center + b.extents;
		global::Debug.DrawLine(new Vector3(vector.x, vector.y, center.z), new Vector3(vector2.x, vector.y, center.z), Color.red);
		global::Debug.DrawLine(new Vector3(vector.x, vector.y, center.z), new Vector3(vector.x, vector2.y, center.z), Color.red);
		global::Debug.DrawLine(new Vector3(vector2.x, vector.y, center.z), new Vector3(vector2.x, vector2.y, center.z), Color.red);
		global::Debug.DrawLine(new Vector3(vector.x, vector2.y, center.z), new Vector3(vector2.x, vector2.y, center.z), Color.red);
	}

	private void OnGUI()
	{
		int i = 0;
		int count = NGUIDebug.mLines.Count;
		while (i < count)
		{
			GUILayout.Label(NGUIDebug.mLines[i], new GUILayoutOption[0]);
			i++;
		}
	}
}
