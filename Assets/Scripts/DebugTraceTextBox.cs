using System;
using UnityEngine;

public class DebugTraceTextBox : MonoBehaviour
{
	private string m_text;

	private Vector2 m_scrollViewVector = Vector2.zero;

	private Vector2 m_scrollScale = new Vector2(1f, 1f);

	private bool m_isActive;

	private Vector2 m_position;

	private static Vector2 s_DefaultSize = new Vector2(300f, 300f);

	private Vector2 m_size;

	private float m_sizeScale = 1f;

	private Rect m_rect;

	public void Setup(Vector2 position)
	{
		this.Setup(position, DebugTraceTextBox.s_DefaultSize);
	}

	public void Setup(Vector2 position, Vector2 size)
	{
		this.m_position = position;
		this.SetSize(size);
	}

	public void SetActive(bool isActive)
	{
		this.m_isActive = isActive;
	}

	public void SetText(string text)
	{
		this.m_text = text;
	}

	public void SetSize(Vector2 size)
	{
		this.m_size = size;
		this.m_rect = new Rect(this.m_position.x, this.m_position.y, this.m_size.x * this.m_sizeScale, this.m_size.y * this.m_sizeScale);
	}

	public void SetSizeScale(float sizeScale)
	{
		this.m_sizeScale = sizeScale;
		this.m_rect = new Rect(this.m_position.x, this.m_position.y, this.m_size.x * this.m_sizeScale, this.m_size.y * this.m_sizeScale);
	}

	public void SetScrollScale(Vector2 scale)
	{
		this.m_scrollViewVector = Vector2.zero;
		this.m_scrollScale = scale;
	}

	private void OnGUI()
	{
		if (!this.m_isActive)
		{
			return;
		}
		Rect rect = new Rect(this.m_rect.left, this.m_rect.top, this.m_rect.width, this.m_rect.height * this.m_scrollScale.y);
		this.m_scrollViewVector = GUI.BeginScrollView(this.m_rect, this.m_scrollViewVector, rect);
		int startIndex = Mathf.Max(this.m_text.Length - 10000, 0);
		GUI.TextArea(rect, this.m_text.Substring(startIndex));
		GUI.EndScrollView();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
