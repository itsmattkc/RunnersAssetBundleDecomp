using System;
using UnityEngine;

public class UIDebugMenuButton : MonoBehaviour
{
	private Rect m_rect;

	private string m_name;

	private GameObject m_callbackObject;

	private bool m_isActive;

	public void Setup(Rect rect, string name, GameObject callbackObject)
	{
		this.m_rect = rect;
		this.m_name = name;
		this.m_callbackObject = callbackObject;
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
		if (this.m_name == null)
		{
			return;
		}
		if (this.m_callbackObject == null)
		{
			return;
		}
		if (GUI.Button(this.m_rect, this.m_name))
		{
			this.m_callbackObject.SendMessage("OnClicked", this.m_name);
		}
	}
}
