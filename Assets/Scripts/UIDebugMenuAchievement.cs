using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugMenuAchievement : UIDebugMenuTask
{
	private enum MenuType
	{
		ALL_RESET,
		ALL_OPEN,
		OUTPUT_INFO,
		SHOW,
		NUM
	}

	private List<string> MenuObjName = new List<string>
	{
		"AllReset",
		"FlagOn:AllOpen",
		"FlagOn:DebugInfo",
		"Show"
	};

	private List<Rect> RectList = new List<Rect>
	{
		new Rect(200f, 200f, 150f, 50f),
		new Rect(200f, 300f, 150f, 50f),
		new Rect(400f, 300f, 150f, 50f),
		new Rect(200f, 400f, 150f, 50f)
	};

	private static string STR_BACK = "Back";

	private UIDebugMenuButtonList m_buttonList;

	private UIDebugMenuButton m_backButton;

	protected override void OnStartFromTask()
	{
		this.m_backButton = base.gameObject.AddComponent<UIDebugMenuButton>();
		this.m_backButton.Setup(new Rect(200f, 100f, 100f, 50f), UIDebugMenuAchievement.STR_BACK, base.gameObject);
		this.m_buttonList = base.gameObject.AddComponent<UIDebugMenuButtonList>();
		for (int i = 0; i < 4; i++)
		{
			string name = this.MenuObjName[i];
			GameObject x = GameObjectUtil.FindChildGameObject(base.gameObject, name);
			if (!(x == null))
			{
				this.m_buttonList.Add(this.RectList, this.MenuObjName, base.gameObject);
			}
		}
	}

	protected override void OnGuiFromTask()
	{
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
	}

	protected override void OnTransitionFrom()
	{
		if (this.m_backButton != null)
		{
			this.m_backButton.SetActive(true);
		}
		if (this.m_buttonList != null)
		{
			this.m_buttonList.SetActive(true);
		}
	}

	private void OnClicked(string name)
	{
		if (name == UIDebugMenuAchievement.STR_BACK)
		{
			base.TransitionToParent();
		}
		else if (name == this.MenuObjName[1])
		{
			AchievementManager.RequestDebugAllOpen(true);
		}
		else if (name == this.MenuObjName[0])
		{
			AchievementManager.RequestReset();
		}
		else if (name == this.MenuObjName[2])
		{
			AchievementManager.RequestDebugInfo(true);
		}
		else if (name == this.MenuObjName[3])
		{
			AchievementManager.RequestShowAchievementsUI();
		}
	}
}
