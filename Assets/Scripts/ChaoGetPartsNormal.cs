using AnimationOrTween;
using DataTable;
using System;
using Text;
using UnityEngine;

public class ChaoGetPartsNormal : ChaoGetPartsBase
{
	private int m_rarity;

	public void Init(int chaoId, int rarity)
	{
		this.m_chaoId = chaoId;
		this.m_rarity = rarity;
	}

	public override void Setup(GameObject chaoGetObjectRoot)
	{
		UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(chaoGetObjectRoot, "img_chao_1");
		if (uITexture != null)
		{
			int idFromServerId = ChaoWindowUtility.GetIdFromServerId(this.m_chaoId);
			ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(uITexture, null, true);
			ChaoTextureManager.Instance.GetTexture(idFromServerId, info);
		}
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(chaoGetObjectRoot, "img_chao_bg_rare");
		if (uISprite != null)
		{
			ChaoWindowUtility.ChangeRaritySpriteFromRarity(uISprite, this.m_rarity);
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(chaoGetObjectRoot, "Lbl_chao_name");
		if (uILabel != null)
		{
			string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_CHAO_TEXT, "Chao", ChaoWindowUtility.GetChaoLabelName(this.m_chaoId)).text;
			uILabel.text = text;
		}
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(chaoGetObjectRoot, "Lbl_chao_lv");
		if (uILabel2 != null)
		{
			uILabel2.text = TextUtility.GetTextLevel("0");
		}
		UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(chaoGetObjectRoot, "img_type_icon");
		if (uISprite2 != null)
		{
			int idFromServerId2 = ChaoWindowUtility.GetIdFromServerId(this.m_chaoId);
			ChaoData chaoData = ChaoTable.GetChaoData(idFromServerId2);
			if (chaoData != null)
			{
				CharacterAttribute charaAtribute = chaoData.charaAtribute;
				string spriteName = "ui_chao_set_type_icon_" + charaAtribute.ToString().ToLower();
				uISprite2.spriteName = spriteName;
			}
		}
		UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(chaoGetObjectRoot, "Lbl_type");
		if (uILabel3 != null)
		{
			int idFromServerId3 = ChaoWindowUtility.GetIdFromServerId(this.m_chaoId);
			ChaoData chaoData2 = ChaoTable.GetChaoData(idFromServerId3);
			if (chaoData2 != null)
			{
				CharacterAttribute charaAtribute2 = chaoData2.charaAtribute;
				string cellName = charaAtribute2.ToString().ToLower();
				string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaAtribute", cellName).text;
				uILabel3.text = text2;
			}
		}
		UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(chaoGetObjectRoot, "img_bonus_icon");
		if (uISprite3 != null)
		{
			uISprite3.enabled = false;
		}
		UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(chaoGetObjectRoot, "Lbl_bonus");
		if (uILabel4 != null)
		{
			int idFromServerId4 = ChaoWindowUtility.GetIdFromServerId(this.m_chaoId);
			uILabel4.text = HudUtility.GetChaoAbilityText(idFromServerId4, -1);
		}
		UISprite uISprite4 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(chaoGetObjectRoot, "img_egg_0");
		if (uISprite4 != null)
		{
			string spriteName2 = "ui_roulette_egg_" + (2 * this.m_rarity).ToString();
			uISprite4.spriteName = spriteName2;
		}
		UISprite uISprite5 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(chaoGetObjectRoot, "img_egg_1");
		if (uISprite5 != null)
		{
			string spriteName3 = "ui_roulette_egg_" + (2 * this.m_rarity + 1).ToString();
			uISprite5.spriteName = spriteName3;
		}
	}

	public override void PlayGetAnimation(Animation anim)
	{
		if (anim == null)
		{
			return;
		}
		ActiveAnimation activeAnimation = ActiveAnimation.Play(anim, "ui_menu_chao_get_Window_Anim", Direction.Forward);
		EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.GetAnimationFinishCallback), true);
	}

	public override ChaoGetPartsBase.BtnType GetButtonType()
	{
		return ChaoGetPartsBase.BtnType.EQUIP_OK;
	}

	public override void PlayEndAnimation(Animation anim)
	{
		if (anim == null)
		{
			return;
		}
		ActiveAnimation activeAnimation = ActiveAnimation.Play(anim, "ui_menu_chao_get_Window_outro_Anim", Direction.Forward);
		EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.EndAnimationFinishCallback), true);
	}

	public override void PlaySE(string seType)
	{
		if (seType == ChaoWindowUtility.SeHatch)
		{
			SoundManager.SePlay("sys_chao_hatch", "SE");
		}
		else if (seType == ChaoWindowUtility.SeBreak)
		{
			ChaoWindowUtility.PlaySEChaoBtrth(this.m_chaoId, this.m_rarity);
		}
	}

	public override EasySnsFeed CreateEasySnsFeed(GameObject rootObject)
	{
		string anchorPath = "Camera/menu_Anim/RouletteUI/Anchor_5_MC";
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "feed_chao_get_caption").text;
		TextObject text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "feed_chao_get_text");
		string text3 = TextManager.GetText(TextManager.TextType.TEXTTYPE_CHAO_TEXT, "Chao", RouletteUtility.GetChaoCellName(this.m_chaoId)).text;
		text2.ReplaceTag("{CHAO}", text3);
		return new EasySnsFeed(rootObject, anchorPath, text, text2.text, null);
	}

	private void GetAnimationFinishCallback()
	{
		global::Debug.Log("ChaoGetPartsNormal.GetAnimationFinishCallback()");
		this.m_callback(ChaoGetPartsBase.AnimType.GET_ANIM_FINISH);
	}

	private void EndAnimationFinishCallback()
	{
		global::Debug.Log("ChaoGetPartsNormal.EndAnimationFinishCallback()");
		this.m_callback(ChaoGetPartsBase.AnimType.OUT_ANIM);
	}
}
