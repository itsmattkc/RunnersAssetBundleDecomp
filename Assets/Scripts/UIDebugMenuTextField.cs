using System;
using UnityEngine;

public class UIDebugMenuTextField : MonoBehaviour
{
	private Rect m_rect;

	private string m_title;

	private string m_text;

	private bool m_isActive;

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

	public void Setup(Rect rect, string titleText)
	{
		this.Setup(rect, titleText, "0");
	}

	public void Setup(Rect rect, string titleText, string fieldText)
	{
		this.m_rect = rect;
		this.m_title = titleText;
		this.m_text = fieldText;
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
		GUI.TextArea(new Rect(this.m_rect.xMin, this.m_rect.yMin - 20f, this.m_rect.width, 20f), this.m_title);
		this.m_text = GUI.TextField(this.m_rect, this.m_text);
	}
}
