using SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuServerAddMessage : UIDebugMenuTask
{
	private enum TextType
	{
		FROM_ID,
		TO_ID,
		NUM
	}

	private UIDebugMenuButton m_backButton;

	private UIDebugMenuButton m_decideButton;

	private UIDebugMenuTextField[] m_TextFields = new UIDebugMenuTextField[2];

	private string[] DefaultTextList = new string[]
	{
		"送信元のGameID",
		"送信先のGameID(エナジーをもらう側)"
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(200f, 200f, 250f, 50f),
		new Rect(200f, 275f, 250f, 50f)
	};

	private NetDebugAddMessage m_addMsg;

	private string GetGameId()
	{
		return SystemSaveManager.GetGameID();
	}

	protected override void OnStartFromTask()
	{
		this.m_backButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_backButton.Setup(new Rect(200f, 100f, 150f, 50f), "Back", base.gameObject);
		this.m_decideButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_decideButton.Setup(new Rect(200f, 450f, 150f, 50f), "Decide", base.gameObject);
		for (int i = 0; i < 2; i++)
		{
			this.m_TextFields[i] = base.gameObject.AddComponent<UIDebugMenuTextField>();
			if (i == 0)
			{
				this.m_TextFields[i].Setup(this.RectList[i], this.DefaultTextList[i], "0");
			}
			else
			{
				this.m_TextFields[i].Setup(this.RectList[i], this.DefaultTextList[i], this.GetGameId());
			}
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
		for (int i = 0; i < 2; i++)
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
		for (int i = 0; i < 2; i++)
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
			for (int i = 0; i < 2; i++)
			{
				UIDebugMenuTextField x = this.m_TextFields[i];
				if (x == null)
				{
				}
			}
			this.m_addMsg = new NetDebugAddMessage(this.m_TextFields[0].text, this.m_TextFields[1].text, 2);
			this.m_addMsg.Request();
		}
	}
}
