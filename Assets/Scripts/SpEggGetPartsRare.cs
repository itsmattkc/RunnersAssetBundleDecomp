using System;
using Text;
using UnityEngine;

public class SpEggGetPartsRare : SpEggGetPartsBase
{
	private int m_rarity;

	private int m_acquiredSpEggCount;

	public SpEggGetPartsRare(int chaoId, int rarity, int acquiredSpEggCount)
	{
		this.m_chaoId = chaoId;
		this.m_rarity = rarity;
		this.m_acquiredSpEggCount = acquiredSpEggCount;
	}

	public override void Setup(GameObject spEggGetObjectRoot)
	{
		UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(spEggGetObjectRoot, "img_chao_1");
		if (uITexture != null)
		{
			int idFromServerId = ChaoWindowUtility.GetIdFromServerId(this.m_chaoId);
			ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(uITexture, null, true);
			ChaoTextureManager.Instance.GetTexture(idFromServerId, info);
		}
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(spEggGetObjectRoot, "img_egg_0");
		if (uISprite != null)
		{
			string spriteName = "ui_roulette_egg_" + (2 * this.m_rarity).ToString();
			uISprite.spriteName = spriteName;
		}
		UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(spEggGetObjectRoot, "img_egg_1");
		if (uISprite2 != null)
		{
			string spriteName2 = "ui_roulette_egg_" + (2 * this.m_rarity + 1).ToString();
			uISprite2.spriteName = spriteName2;
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(spEggGetObjectRoot, "Lbl_item_name");
		if (uILabel != null)
		{
			string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "sp_egg_name").text;
			uILabel.text = text;
		}
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(spEggGetObjectRoot, "Lbl_item_number");
		if (uILabel2 != null)
		{
			TextObject text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Score", "number_of_pieces");
			text2.ReplaceTag("{NUM}", this.m_acquiredSpEggCount.ToString());
			uILabel2.text = text2.text;
		}
		UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(spEggGetObjectRoot, "Lbl_info");
		if (uILabel3 != null)
		{
			TextObject text3 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoRoulette", "sp_egg_info");
			string text4 = TextManager.GetText(TextManager.TextType.TEXTTYPE_CHAO_TEXT, "Chao", ChaoWindowUtility.GetChaoLabelName(this.m_chaoId)).text;
			text3.ReplaceTag("{CHAO_NAME}", text4);
			uILabel3.text = text3.text;
		}
	}

	public override void PlaySE(string seType)
	{
		if (seType == ChaoWindowUtility.SeHatch)
		{
			SoundManager.SePlay("sys_chao_hatch", "SE");
		}
		else if (seType == ChaoWindowUtility.SeSpEgg)
		{
			SoundManager.SePlay("sys_specialegg", "SE");
		}
		else if (seType == ChaoWindowUtility.SeBreak)
		{
			SoundManager.SePlay("sys_chao_birth", "SE");
		}
	}
}
