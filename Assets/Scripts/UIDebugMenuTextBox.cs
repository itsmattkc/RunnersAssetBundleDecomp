using System;
using UnityEngine;

public class UIDebugMenuTextBox : MonoBehaviour
{
	private Rect m_rect;

	private string m_text;

	private bool m_isActive;

	private Vector2 m_scrollViewVector = Vector2.zero;

	private Vector2 m_scrollScale = new Vector2(1f, 2f);

	public string Text
	{
		get
		{
			return this.m_text;
		}
		set
		{
			this.m_text = value;
		}
	}

	public Vector2 ScrollScale
	{
		get
		{
			return this.m_scrollScale;
		}
		set
		{
			this.m_scrollScale = value;
		}
	}

	public string text
	{
		get
		{
			return this.m_text;
		}
		set
		{
			this.m_text = value;
		}
	}

	public void Setup(Rect rect, string text)
	{
		this.m_rect = rect;
		this.m_text = text;
		this.m_isActive = false;
	}

	public void SetActive(bool flag)
	{
		this.m_isActive = flag;
	}

	private void OnGUI()
	{
		if (!this.m_isActive)
		{
			return;
		}
		Rect rect = new Rect(this.m_rect.left, this.m_rect.top, this.m_rect.width * this.m_scrollScale.x, this.m_rect.height * this.m_scrollScale.y);
		this.m_scrollViewVector = GUI.BeginScrollView(this.m_rect, this.m_scrollViewVector, rect);
		GUI.TextArea(rect, this.m_text);
		GUI.EndScrollView();
	}
}
