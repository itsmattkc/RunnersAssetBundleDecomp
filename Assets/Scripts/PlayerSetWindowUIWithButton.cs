using System;
using UnityEngine;

public class PlayerSetWindowUIWithButton : PlayerSetWindowUI
{
	private GameObject m_btnMain;

	private GameObject m_btnSub;

	private bool m_isClickCancel;

	private bool m_isClickMain;

	private bool m_isClickSub;

	public bool isClickCancel
	{
		get
		{
			return this.m_isClickCancel;
		}
	}

	public bool isClickMain
	{
		get
		{
			return this.m_isClickMain;
		}
	}

	public bool isClickSub
	{
		get
		{
			return this.m_isClickSub;
		}
	}

	public void SetupWithButton(CharaType charaType)
	{
		base.Setup(charaType, null, PlayerSetWindowUI.WINDOW_MODE.DEFAULT);
		this.m_btnMain = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_main");
		this.m_btnSub = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_sub");
		PlayerSetWindowUIWithButton.SetupButton(this.m_btnMain, base.gameObject, "OnClickMainButton");
		PlayerSetWindowUIWithButton.SetupButton(this.m_btnSub, base.gameObject, "OnClickSubButton");
		this.m_isClickCancel = false;
		this.m_isClickMain = false;
		this.m_isClickSub = false;
	}

	private void OnClickBtn()
	{
		this.m_isClickCancel = true;
	}

	private void OnClickMainButton()
	{
		this.m_isClickMain = true;
	}

	private void OnClickSubButton()
	{
		this.m_isClickSub = true;
	}

	public static PlayerSetWindowUIWithButton Create(CharaType charaType)
	{
		GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
		if (cameraUIObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(cameraUIObject, "PlayerSetWindowUI");
			PlayerSetWindowUIWithButton playerSetWindowUIWithButton = null;
			if (gameObject != null)
			{
				playerSetWindowUIWithButton = gameObject.GetComponent<PlayerSetWindowUIWithButton>();
				if (playerSetWindowUIWithButton == null)
				{
					playerSetWindowUIWithButton = gameObject.AddComponent<PlayerSetWindowUIWithButton>();
				}
				if (playerSetWindowUIWithButton != null)
				{
					playerSetWindowUIWithButton.SetupWithButton(charaType);
				}
			}
			return playerSetWindowUIWithButton;
		}
		return null;
	}

	private static void SetupButton(GameObject buttonObject, GameObject targetObject, string functionName)
	{
		if (buttonObject == null)
		{
			return;
		}
		buttonObject.SetActive(true);
		UIButtonMessage uIButtonMessage = buttonObject.GetComponent<UIButtonMessage>();
		if (uIButtonMessage == null)
		{
			uIButtonMessage = buttonObject.AddComponent<UIButtonMessage>();
			uIButtonMessage.target = targetObject;
			uIButtonMessage.functionName = functionName;
		}
	}
}
