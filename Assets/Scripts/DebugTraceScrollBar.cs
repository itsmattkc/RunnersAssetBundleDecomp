using App;
using System;
using UnityEngine;

public class DebugTraceScrollBar : MonoBehaviour
{
	public delegate void ValueChangedCallback(string name, float value);

	private DebugTraceScrollBar.ValueChangedCallback m_callback;

	public static readonly float MaxValue = 100f;

	public static readonly float MinValue = 1f;

	private string m_name;

	private bool m_isActive;

	private float m_scrollValue;

	private float m_scrollValuePrev;

	private static readonly Vector2 s_Size = new Vector2(200f, 50f);

	private Rect m_rect;

	public void SetActive(bool isActive)
	{
		this.m_isActive = isActive;
	}

	public void SetUp(string name, DebugTraceScrollBar.ValueChangedCallback callback, Vector2 position)
	{
		this.m_name = name;
		this.m_callback = callback;
		this.m_rect = new Rect(position.x, position.y, DebugTraceScrollBar.s_Size.x, DebugTraceScrollBar.s_Size.y);
	}

	private void OnGUI()
	{
		if (!this.m_isActive)
		{
			return;
		}
		GUI.Label(new Rect(this.m_rect.left, this.m_rect.top - 20f, this.m_rect.width, this.m_rect.height), this.m_name);
		this.m_scrollValue = GUI.HorizontalScrollbar(this.m_rect, this.m_scrollValue, 1f, DebugTraceScrollBar.MinValue, DebugTraceScrollBar.MaxValue);
		if (!App.Math.NearEqual(this.m_scrollValue, this.m_scrollValuePrev, 1E-06f))
		{
			this.m_scrollValuePrev = this.m_scrollValue;
			if (this.m_callback != null)
			{
				this.m_callback(this.m_name, this.m_scrollValue);
			}
		}
	}
}
