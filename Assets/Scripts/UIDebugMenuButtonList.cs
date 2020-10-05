using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuButtonList : MonoBehaviour
{
	private List<UIDebugMenuButton> m_buttons;

	public UIDebugMenuButtonList()
	{
		this.m_buttons = new List<UIDebugMenuButton>();
	}

	public void Add(List<Rect> rect, List<string> name, GameObject callbackObject)
	{
		if (rect.Count != name.Count)
		{
			return;
		}
		int count = rect.Count;
		for (int i = 0; i < count; i++)
		{
			UIDebugMenuButton uIDebugMenuButton = base.gameObject.AddComponent<UIDebugMenuButton>();
			uIDebugMenuButton.Setup(rect[i], name[i], callbackObject);
			this.m_buttons.Add(uIDebugMenuButton);
		}
	}

	public void SetActive(bool flag)
	{
		foreach (UIDebugMenuButton current in this.m_buttons)
		{
			if (!(current == null))
			{
				current.SetActive(flag);
			}
		}
	}
}
