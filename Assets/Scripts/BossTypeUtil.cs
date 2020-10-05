using SaveData;
using System;
using Text;

public class BossTypeUtil
{
	private const string HUD_SPRITENAME1 = "ui_gp_word_boss_";

	private const string HUD_SPRITENAME2 = "ui_gp_gauge_boss_icon_";

	private static readonly BossParam[] BOSS_PARAMS = new BossParam[]
	{
		new BossParam("eggman", SystemData.FlagStatus.TUTORIAL_FEVER_BOSS, HudTutorial.Id.FEVERBOSS, BossCharaType.EGGMAN, BossCategory.FEVER, 0, 0),
		new BossParam("eggman1", SystemData.FlagStatus.TUTORIAL_BOSS_MAP_1, HudTutorial.Id.MAPBOSS_1, BossCharaType.EGGMAN, BossCategory.MAP, 0, 0),
		new BossParam("eggman2", SystemData.FlagStatus.TUTORIAL_BOSS_MAP_2, HudTutorial.Id.MAPBOSS_2, BossCharaType.EGGMAN, BossCategory.MAP, 1, 1),
		new BossParam("eggman3", SystemData.FlagStatus.TUTORIAL_BOSS_MAP_3, HudTutorial.Id.MAPBOSS_3, BossCharaType.EGGMAN, BossCategory.MAP, 2, 2),
		new BossParam("n", SystemData.FlagStatus.NONE, HudTutorial.Id.EVENTBOSS_1, BossCharaType.EVENT, BossCategory.EVENT, 0, 0),
		new BossParam("r", SystemData.FlagStatus.NONE, HudTutorial.Id.EVENTBOSS_1, BossCharaType.EVENT, BossCategory.EVENT, 1, 0),
		new BossParam("p", SystemData.FlagStatus.NONE, HudTutorial.Id.EVENTBOSS_2, BossCharaType.EVENT, BossCategory.EVENT, 2, 1)
	};

	public static SystemData.FlagStatus GetBossSaveDataFlagStatus(BossType type)
	{
		if (type < BossType.NUM)
		{
			return BossTypeUtil.BOSS_PARAMS[(int)type].m_flagStatus;
		}
		return SystemData.FlagStatus.NONE;
	}

	public static HudTutorial.Id GetBossTutorialID(BossType type)
	{
		if (type < BossType.NUM)
		{
			return BossTypeUtil.BOSS_PARAMS[(int)type].m_tutorialID;
		}
		return HudTutorial.Id.NONE;
	}

	public static BossCharaType GetBossCharaType(BossType type)
	{
		if (type < BossType.NUM)
		{
			return BossTypeUtil.BOSS_PARAMS[(int)type].m_bossCharaType;
		}
		return BossCharaType.NONE;
	}

	public static BossCategory GetBossCategory(BossType type)
	{
		if (type < BossType.NUM)
		{
			return BossTypeUtil.BOSS_PARAMS[(int)type].m_bossCategory;
		}
		return BossCategory.FEVER;
	}

	public static int GetLayerNumber(BossType type)
	{
		if (type < BossType.NUM)
		{
			return BossTypeUtil.BOSS_PARAMS[(int)type].m_layerNumber;
		}
		return 0;
	}

	public static int GetIndexNumber(BossType type)
	{
		if (type < BossType.NUM)
		{
			return BossTypeUtil.BOSS_PARAMS[(int)type].m_indexNumber;
		}
		return 0;
	}

	public static string GetBossBgmName(BossType type)
	{
		if (BossTypeUtil.GetBossCharaType(type) == BossCharaType.EGGMAN)
		{
			return "BGM_boss01";
		}
		return EventBossObjectTable.GetItemData(EventBossObjectTableItem.BgmFile);
	}

	public static string GetBossBgmCueSheetName(BossType type)
	{
		if (BossTypeUtil.GetBossCharaType(type) == BossCharaType.EGGMAN)
		{
			return "bgm_z_boss01";
		}
		int num = BossTypeUtil.GetLayerNumber(type) + 1;
		return EventBossObjectTable.GetItemData(EventBossObjectTableItem.BgmCueName) + "_" + num.ToString("D2");
	}

	public static string GetBossHudSpriteName(BossType type)
	{
		return "ui_gp_word_boss_" + ((int)BossTypeUtil.GetBossCharaType(type)).ToString(string.Empty);
	}

	public static string GetBossHudSpriteIconName(BossType type)
	{
		return "ui_gp_gauge_boss_icon_" + ((int)BossTypeUtil.GetBossCharaType(type)).ToString(string.Empty);
	}

	public static string GetTextCommonBossName(BossType type)
	{
		if (type >= BossType.NUM)
		{
			return string.Empty;
		}
		if (BossTypeUtil.BOSS_PARAMS[(int)type].m_bossCharaType == BossCharaType.EGGMAN)
		{
			return TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "BossName", BossTypeUtil.BOSS_PARAMS[(int)type].m_name).text;
		}
		int specificId = EventManager.GetSpecificId();
		return TextManager.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "EventBossName", "bossname_" + BossTypeUtil.BOSS_PARAMS[(int)type].m_name + "_" + specificId.ToString()).text;
	}

	public static string GetTextCommonBossCharaName(BossType type)
	{
		if (type >= BossType.NUM)
		{
			return string.Empty;
		}
		if (BossTypeUtil.BOSS_PARAMS[(int)type].m_bossCharaType == BossCharaType.EGGMAN)
		{
			return TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "BossName", "eggman").text;
		}
		return TextManager.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "EventBossName", "bossname_" + EventManager.GetSpecificId().ToString()).text;
	}

	public static BossType GetBossTypeRarity(int rarity)
	{
		switch (rarity)
		{
		case 0:
			return BossType.EVENT1;
		case 1:
			return BossType.EVENT2;
		case 2:
			return BossType.EVENT3;
		default:
			return BossType.EVENT1;
		}
	}
}
