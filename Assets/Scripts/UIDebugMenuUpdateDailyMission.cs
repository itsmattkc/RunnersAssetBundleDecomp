using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuUpdateDailyMission : UIDebugMenuTask
{
	private enum TextType
	{
		MISSION_ID,
		NUM
	}

	private UIDebugMenuButton m_backButton;

	private UIDebugMenuButton m_decideButton;

	private UIDebugMenuTextField[] m_TextFields = new UIDebugMenuTextField[1];

	private string[] DefaultTextList = new string[]
	{
		"Mission ID"
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(200f, 200f, 250f, 50f)
	};

	private NetDebugUpdDailyMission m_updateDailyMission;

	protected override void OnStartFromTask()
	{
		this.m_backButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_backButton.Setup(new Rect(200f, 100f, 150f, 50f), "Back", base.gameObject);
		this.m_decideButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_decideButton.Setup(new Rect(200f, 450f, 150f, 50f), "Decide", base.gameObject);
		for (int i = 0; i < 1; i++)
		{
			this.m_TextFields[i] = base.gameObject.AddComponent<UIDebugMenuTextField>();
			this.m_TextFields[i].Setup(this.RectList[i], this.DefaultTextList[i]);
		}
	}

	protected override void OnTransitionTo()
	{
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(false);
		}
		if (this.m_decideButton != null)
		{
			this.m_decideButton.SetActive(false);
		}
		for (int i = 0; i < 1; i++)
		{
			if (!(this.m_TextFields[i] == null))
			{
				this.m_TextFields[i].SetActive(false);
			}
		}
	}

	protected override void OnTransitionFrom()
	{
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(true);
		}
		if (this.m_decideButton != null)
		{
			this.m_decideButton.SetActive(true);
		}
		for (int i = 0; i < 1; i++)
		{
			if (!(this.m_TextFields[i] == null))
			{
				this.m_TextFields[i].SetActive(true);
			}
		}
	}

	private void OnClicked(string name)
	{
		if (name == "Back")
		{
			base.TransitionToParent();
		}
		else if (name == "Decide")
		{
			for (int i = 0; i < 1; i++)
			{
				UIDebugMenuTextField uIDebugMenuTextField = this.m_TextFields[i];
				if (!(uIDebugMenuTextField == null))
				{
					int num;
					if (!int.TryParse(uIDebugMenuTextField.text, out num))
					{
						return;
					}
				}
			}
			this.m_updateDailyMission = new NetDebugUpdDailyMission(int.Parse(this.m_TextFields[0].text));
			this.m_updateDailyMission.Request();
		}
	}
}
