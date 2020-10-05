using System;
using UnityEngine;

public class HudEpisodeButton
{
	private GameObject m_mainMenuObject;

	private bool m_isBossStage;

	public void Initialize(GameObject mainMenuObject, bool isBossStage, CharacterAttribute charaAttribute)
	{
		if (mainMenuObject == null)
		{
			return;
		}
		this.m_mainMenuObject = mainMenuObject;
		this.m_isBossStage = isBossStage;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_mainMenuObject, "Anchor_5_MC");
		if (gameObject == null)
		{
			return;
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "0_Endless");
		if (gameObject2 == null)
		{
			return;
		}
		GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject2, "Btn_3_play");
		if (gameObject3 == null)
		{
			return;
		}
		GameObject gameObject4 = GameObjectUtil.FindChildGameObject(gameObject3, "next_map");
		if (gameObject4 != null)
		{
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject4, "img_icon_type_1");
			if (uISprite != null)
			{
				uISprite.spriteName = HudEpisodeButton.CalcCharaTypeSpriteName(charaAttribute);
			}
			UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject4, "img_next_map");
			if (uISprite2 != null)
			{
				string spriteName = "ui_mm_map_thumb_w" + string.Format("{0:D2}", (int)(charaAttribute + 1)) + "a";
				uISprite2.spriteName = spriteName;
			}
		}
		GameObject gameObject5;
		GameObject gameObject6;
		if (this.m_isBossStage)
		{
			gameObject5 = GameObjectUtil.FindChildGameObject(gameObject3, "img_word_play");
			gameObject6 = GameObjectUtil.FindChildGameObject(gameObject3, "img_word_bossplay");
		}
		else
		{
			gameObject6 = GameObjectUtil.FindChildGameObject(gameObject3, "img_word_play");
			gameObject5 = GameObjectUtil.FindChildGameObject(gameObject3, "img_word_bossplay");
		}
		if (gameObject6 != null)
		{
			gameObject6.SetActive(true);
		}
		if (gameObject5 != null)
		{
			gameObject5.SetActive(false);
		}
	}

	private static string CalcCharaTypeSpriteName(CharacterAttribute charaAttribute)
	{
		string text = "ui_chao_set_type_icon_";
		switch (charaAttribute)
		{
		case CharacterAttribute.SPEED:
			text += "speed";
			break;
		case CharacterAttribute.FLY:
			text += "fly";
			break;
		case CharacterAttribute.POWER:
			text += "power";
			break;
		}
		return text;
	}
}
