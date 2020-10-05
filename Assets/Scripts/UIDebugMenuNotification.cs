using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuNotification : UIDebugMenuTask
{
	private enum MenuType
	{
		REGIST,
		UNREGIST,
		SENDMESSAGE,
		NUM
	}

	private List<string> MenuObjName = new List<string>
	{
		"Regist",
		"UnRegist",
		"SendMessage"
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(200f, 200f, 150f, 50f),
		new Rect(400f, 200f, 150f, 50f),
		new Rect(600f, 200f, 150f, 50f)
	};

	private UIDebugMenuButtonList m_buttonList;

	private UIDebugMenuButton m_backButton;

	private UIDebugMenuTextField m_RecieverGuid;

	protected override void OnStartFromTask()
	{
		this.m_backButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_backButton.Setup(new Rect(200f, 100f, 150f, 50f), "Back", base.gameObject);
		this.m_buttonList = base.gameObject.AddComponent<UIDebugMenuButtonList>();
		for (int i = 0; i < 3; i++)
		{
			string name = this.MenuObjName[i];
			GameObject x = GameObjectUtil.FindChildGameObject(base.gameObject, name);
			if (!(x == null))
			{
				this.m_buttonList.Add(this.RectList, this.MenuObjName, base.gameObject);
			}
		}
		this.m_RecieverGuid = base.gameObject.AddComponent<UIDebugMenuTextField>();
		this.m_RecieverGuid.Setup(new Rect(200f, 350f, 350f, 50f), "RecieverID", string.Empty);
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
		if (this.m_RecieverGuid != null)
		{
			this.m_RecieverGuid.SetActive(false);
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
		if (this.m_RecieverGuid != null)
		{
			this.m_RecieverGuid.SetActive(true);
		}
	}

	private void OnClicked(string name)
	{
		if (name == "Back")
		{
			base.TransitionToParent();
		}
		else if (name.Contains("UnRegist"))
		{
			PnoteNotification.RequestUnregister();
		}
		else if (name.Contains("Regist"))
		{
			PnoteNotification.RequestRegister();
		}
		else if (name.Contains("SendMessage"))
		{
			PnoteNotification.SendMessage("Debug Send By UIDebug", this.m_RecieverGuid.text, PnoteNotification.LaunchOption.None);
		}
	}
}
