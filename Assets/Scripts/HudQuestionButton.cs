using System;
using Text;
using UnityEngine;

public class HudQuestionButton : MonoBehaviour
{
	private bool m_isQuickMode;

	public void Initialize(bool isQuickMode)
	{
		this.m_isQuickMode = isQuickMode;
		if (isQuickMode)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Anchor_5_MC");
			if (gameObject == null)
			{
				return;
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "1_Quick");
			if (gameObject2 == null)
			{
				return;
			}
			UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(gameObject2, "Btn_0_help");
			if (uIButtonMessage == null)
			{
				return;
			}
			uIButtonMessage.target = base.gameObject;
			uIButtonMessage.functionName = "QuickModeQuestionButtonClicked";
		}
		else
		{
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(base.gameObject, "Anchor_5_MC");
			if (gameObject3 == null)
			{
				return;
			}
			GameObject gameObject4 = GameObjectUtil.FindChildGameObject(gameObject3, "0_Endless");
			if (gameObject4 == null)
			{
				return;
			}
			UIButtonMessage uIButtonMessage2 = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(gameObject4, "Btn_0_help");
			if (uIButtonMessage2 == null)
			{
				return;
			}
			uIButtonMessage2.target = base.gameObject;
			uIButtonMessage2.functionName = "EndlessModeQuestionButtonClicked";
		}
	}

	private void QuickModeQuestionButtonClicked()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "QuickModeQuestion",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "help_quick_mode_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "help_quick_mode_text").text
		});
	}

	private void EndlessModeQuestionButtonClicked()
	{
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "EndlessModeQuestion",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "help_episode_mode_caption").text,
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "help_episode_mode_text").text
		});
	}

	private void Start()
	{
	}

	private void Update()
	{
		bool flag = false;
		if (GeneralWindow.IsCreated("QuickModeQuestion"))
		{
			flag = true;
		}
		else if (GeneralWindow.IsCreated("QuickModeQuestion"))
		{
			flag = true;
		}
		if (flag && GeneralWindow.IsOkButtonPressed)
		{
			GeneralWindow.Close();
		}
	}
}
