using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuEvent : UIDebugMenuTask
{
	private enum MenuType
	{
		SPECIAL_STAGE_01,
		SPECIAL_STAGE_02,
		ANIMAL_COLLECT,
		NUM
	}

	private List<string> MenuObjName = new List<string>
	{
		"special_stage_1 (w7 spring)",
		"special_stage_2 (w5 desert)",
		"animal_collect"
	};

	private List<int> MenuObjId = new List<int>
	{
		100010000,
		100020000,
		300010000
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(200f, 200f, 250f, 40f),
		new Rect(200f, 250f, 250f, 40f),
		new Rect(200f, 300f, 250f, 40f)
	};

	private UIDebugMenuButtonList m_buttonList;

	private UIDebugMenuButton m_backButton;

	private UIDebugMenuTextField m_textField = new UIDebugMenuTextField();

	protected override void OnStartFromTask()
	{
		this.m_backButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_backButton.Setup(new Rect(200f, 100f, 150f, 50f), "Back", base.gameObject);
		this.m_buttonList = base.gameObject.AddComponent<UIDebugMenuButtonList>();
		for (int i = 0; i < 3; i++)
		{
			string name = this.MenuObjName[i];
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, name);
			if (gameObject == null)
			{
				gameObject = new GameObject();
				gameObject.name = name;
			}
			this.m_buttonList.Add(this.RectList, this.MenuObjName, base.gameObject);
		}
		this.m_textField = base.gameObject.AddComponent<UIDebugMenuTextField>();
		this.m_textField.Setup(new Rect(200f, 400f, 350f, 40f), "log");
		this.m_textField.text = string.Empty;
	}

	protected override void OnTransitionTo()
	{
		if (this.m_buttonList != null)
		{
			this.m_buttonList.SetActive(false);
		}
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(false);
		}
		if (this.m_textField != null)
		{
			this.m_textField.SetActive(false);
		}
	}

	protected override void OnTransitionFrom()
	{
		if (this.m_buttonList != null)
		{
			this.m_buttonList.SetActive(true);
		}
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(true);
		}
		if (this.m_textField != null)
		{
			this.m_textField.SetActive(true);
		}
	}

	private void OnClicked(string name)
	{
		if (name == "Back")
		{
			base.TransitionToParent();
		}
	}

	private bool CheckEvent()
	{
		return EventManager.Instance != null && EventManager.Instance.Type != EventManager.EventType.UNKNOWN;
	}

	private string GetEventType()
	{
		if (EventManager.Instance != null)
		{
			return EventManager.Instance.Type.ToString();
		}
		return string.Empty;
	}
}
