using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuServerUpPoint : UIDebugMenuTask
{
	private enum TextType
	{
		ENERGY,
		RING,
		RED_STAR_RING,
		NUM
	}

	private UIDebugMenuButton m_backButton;

	private UIDebugMenuButton m_decideButton;

	private UIDebugMenuTextField[] m_TextFields = new UIDebugMenuTextField[3];

	private string[] DefaultTextList = new string[]
	{
		"No. of Additional Challenge",
		"No. of Additional Ring",
		"No. of Additional Red Ring"
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(200f, 200f, 250f, 50f),
		new Rect(200f, 275f, 250f, 50f),
		new Rect(200f, 350f, 250f, 50f)
	};

	private NetDebugUpdPointData m_upPoint;

	protected override void OnStartFromTask()
	{
		this.m_backButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_backButton.Setup(new Rect(200f, 100f, 150f, 50f), "Back", base.gameObject);
		this.m_decideButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_decideButton.Setup(new Rect(200f, 450f, 150f, 50f), "Decide", base.gameObject);
		for (int i = 0; i < 3; i++)
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
		for (int i = 0; i < 3; i++)
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
		for (int i = 0; i < 3; i++)
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
			for (int i = 0; i < 3; i++)
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
			this.m_upPoint = new NetDebugUpdPointData(int.Parse(this.m_TextFields[0].text), 0, int.Parse(this.m_TextFields[1].text), 0, int.Parse(this.m_TextFields[2].text), 0);
			this.m_upPoint.Request();
		}
	}
}
