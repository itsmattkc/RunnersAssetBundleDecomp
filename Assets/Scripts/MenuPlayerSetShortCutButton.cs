using System;
using UnityEngine;

public class MenuPlayerSetShortCutButton : MonoBehaviour
{
	public delegate void ButtonClickedCallback(CharaType charaType);

	private CharaType m_charaType;

	private MenuPlayerSetShortCutButton.ButtonClickedCallback m_callback;

	private bool m_isLocked;

	private GameObject m_spriteSdwObject;

	public CharaType Chara
	{
		get
		{
			return this.m_charaType;
		}
		private set
		{
		}
	}

	public void Setup(CharaType charaType, bool isLocked)
	{
		this.m_charaType = charaType;
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		if (component != null)
		{
			component.isTrigger = false;
		}
		this.SetIconLock(isLocked);
		UIButtonMessage uIButtonMessage = base.gameObject.GetComponent<UIButtonMessage>();
		if (uIButtonMessage == null)
		{
			uIButtonMessage = base.gameObject.AddComponent<UIButtonMessage>();
		}
		uIButtonMessage.target = base.gameObject;
		uIButtonMessage.functionName = "ClickedCallback";
	}

	public void SetCallback(MenuPlayerSetShortCutButton.ButtonClickedCallback callback)
	{
		this.m_callback = callback;
	}

	public void SetIconLock(bool isLock)
	{
		this.m_isLocked = isLock;
		string text = string.Empty;
		string text2 = string.Empty;
		if (this.m_isLocked)
		{
			text = "ui_tex_player_set_unlock";
			text2 = "ui_tex_player_set_act_unlock";
		}
		else
		{
			text = "ui_tex_player_set_";
			text += string.Format("{0:00}", (int)this.m_charaType);
			text += "_";
			text += CharaName.PrefixName[(int)this.m_charaType];
			text2 = "ui_tex_player_set_act_";
			text2 += string.Format("{0:00}", (int)this.m_charaType);
			text2 += "_";
			text2 += CharaName.PrefixName[(int)this.m_charaType];
		}
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_pager_bg_temp");
		if (uISprite != null)
		{
			uISprite.spriteName = text;
		}
		UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_pager_act_temp");
		if (uISprite2 != null)
		{
			uISprite2.spriteName = text2;
			this.m_spriteSdwObject = uISprite2.gameObject;
			this.m_spriteSdwObject.SetActive(false);
		}
	}

	public void SetButtonActive(bool isActive)
	{
		if (this.m_spriteSdwObject != null)
		{
			this.m_spriteSdwObject.SetActive(isActive);
		}
	}

	public bool IsVisible(UIPanel panel)
	{
		if (panel != null)
		{
			Transform transform = base.gameObject.transform;
			if (panel.IsVisible(transform.position))
			{
				return true;
			}
		}
		return false;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void ClickedCallback()
	{
		global::Debug.Log("MenuPlayerSetShortCutButton.ButtonClickedCallback");
		if (this.m_callback != null)
		{
			this.m_callback(this.m_charaType);
		}
	}
}
