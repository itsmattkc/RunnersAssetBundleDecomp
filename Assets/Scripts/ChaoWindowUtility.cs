using DataTable;
using System;

public class ChaoWindowUtility
{
	public static readonly string SeHatch = "SeHatch";

	public static readonly string SeBreak = "SeBreak";

	public static readonly string SeSpEgg = "SeSpEgg";

	public static int GetIdFromServerId(int serverId)
	{
		int num = 400000;
		return serverId - num;
	}

	public static string GetChaoSpriteName(int serverChaoId)
	{
		int idFromServerId = ChaoWindowUtility.GetIdFromServerId(serverChaoId);
		return string.Format("ui_tex_chao_{0:D4}", idFromServerId);
	}

	public static string GetChaoLabelName(int serverChaoId)
	{
		int idFromServerId = ChaoWindowUtility.GetIdFromServerId(serverChaoId);
		return string.Format("name{0:D4}", idFromServerId);
	}

	public static void ChangeRaritySpriteFromServerChaoId(UISprite sprite, int serverChaoId)
	{
		int clientChaoId = serverChaoId - 400000;
		ChaoWindowUtility.ChangeRaritySrptieFromClientChaoId(sprite, clientChaoId);
	}

	public static void ChangeRaritySrptieFromClientChaoId(UISprite sprite, int clientChaoId)
	{
		ChaoData chaoData = ChaoTable.GetChaoData(clientChaoId);
		if (chaoData != null)
		{
			int rarity = (int)chaoData.rarity;
			ChaoWindowUtility.ChangeRaritySpriteFromRarity(sprite, rarity);
		}
	}

	public static void ChangeRaritySpriteFromRarity(UISprite sprite, int rarity)
	{
		if (sprite == null)
		{
			return;
		}
		sprite.spriteName = "ui_chao_set_bg_ll_" + rarity.ToString();
	}

	public static void PlaySEChaoBtrth(int chaoId, int rarity)
	{
		if (EventManager.Instance != null && EventCommonDataTable.Instance != null && EventManager.Instance.Type != EventManager.EventType.UNKNOWN && EventCommonDataTable.Instance.IsRouletteEventChao(chaoId))
		{
			string cueSheetName = "SE_" + EventManager.GetEventTypeName(EventManager.Instance.Type);
			if (rarity == 2)
			{
				SoundManager.SePlay("sys_chao_birthS", cueSheetName);
			}
			else
			{
				SoundManager.SePlay("sys_chao_birth", cueSheetName);
			}
			return;
		}
		if (rarity == 2)
		{
			SoundManager.SePlay("sys_chao_birthS", "SE");
		}
		else
		{
			SoundManager.SePlay("sys_chao_birth", "SE");
		}
	}

	public static void PlaySEChaoUnite(int chaoId)
	{
		if (EventManager.Instance != null && EventCommonDataTable.Instance != null && EventManager.Instance.Type != EventManager.EventType.UNKNOWN && EventCommonDataTable.Instance.IsRouletteEventChao(chaoId))
		{
			string cueSheetName = "SE_" + EventManager.GetEventTypeName(EventManager.Instance.Type);
			SoundManager.SePlay("sys_chao_unite", cueSheetName);
			return;
		}
		SoundManager.SePlay("sys_chao_unite", "SE");
	}
}
