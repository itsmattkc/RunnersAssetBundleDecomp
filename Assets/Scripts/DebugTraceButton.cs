using System;
using UnityEngine;

public class DebugTraceButton : MonoBehaviour
{
	public delegate void ButtonClickedCallback(string name);

	private DebugTraceButton.ButtonClickedCallback m_callback;

	private string m_name;

	private static Vector2 s_DefaultSize = new Vector2(200f, 50f);

	private Vector2 m_position;

	private Vector2 m_size;

	private Rect m_rect;

	private bool m_isActive;

	public void Setup(string name, DebugTraceButton.ButtonClickedCallback callback, Vector2 position)
	{
		this.Setup(name, callback, position, DebugTraceButton.s_DefaultSize);
	}

	public void Setup(string name, DebugTraceButton.ButtonClickedCallback callback, Vector2 position, Vector2 size)
	{
		this.m_name = name;
		this.m_callback = callback;
		this.m_position = position;
		this.m_size = size;
		this.m_rect = new Rect(this.m_position.x, this.m_position.y, this.m_size.x, this.m_size.y);
	}

	public void SetActive(bool isActive)
	{
		this.m_isActive = isActive;
	}

	private void OnGUI()
	{
		if (!this.m_isActive)
		{
			return;
		}
		if (this.m_name == null)
		{
			return;
		}
		if (GUI.Button(this.m_rect, this.m_name))
		{
			if (this.m_callback == null)
			{
				return;
			}
			this.m_callback(this.m_name);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
