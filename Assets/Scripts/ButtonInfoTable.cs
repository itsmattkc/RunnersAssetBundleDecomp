using Message;
using System;
using UnityEngine;

public class ButtonInfoTable
{
	public enum PageType
	{
		MAIN,
		CHAO,
		EVENT,
		INFOMATION,
		ITEM,
		STAGE,
		OPTION,
		PLAYER_MAIN,
		PLAYER_SUB,
		PRESENT_BOX,
		DAILY_CHALLENGE,
		DAILY_BATTLE,
		ROULETTE,
		SHOP_RSR,
		SHOP_RING,
		SHOP_ENERGY,
		SHOP_EVENT,
		EPISODE,
		EPISODE_PLAY,
		EPISODE_RANKING,
		QUICK,
		QUICK_RANKING,
		PLAY_AT_EPISODE_PAGE,
		NUM,
		NON = -1
	}

	public class AnimInfo
	{
		public string animName;

		public string targetName;

		public AnimInfo(string targetName, string animName)
		{
			this.targetName = targetName;
			this.animName = animName;
		}
	}

	public class MessageInfo
	{
		public string targetName;

		public string methodName;

		public string componentName;

		public bool uiFlag;

		public MessageInfo(string target, string method, string component, bool ui = true)
		{
			this.targetName = target;
			this.methodName = method;
			this.componentName = component;
			this.uiFlag = ui;
		}
	}

	public class ButtonInfo
	{
		public MsgMenuSequence.SequeneceType nextMenuId = MsgMenuSequence.SequeneceType.NON;

		public ButtonInfoTable.PageType nextPageType = ButtonInfoTable.PageType.NON;

		public ButtonInfoTable.MessageInfo btnMsgInfo;

		public string clickButtonPath = string.Empty;

		public string seName = string.Empty;

		public ButtonInfo(MsgMenuSequence.SequeneceType menuId, ButtonInfoTable.PageType pageType, string path, string se, ButtonInfoTable.MessageInfo info = null)
		{
			this.nextMenuId = menuId;
			this.nextPageType = pageType;
			this.btnMsgInfo = info;
			this.clickButtonPath = path;
			this.seName = se;
		}

		public ButtonInfo(MsgMenuSequence.SequeneceType menuId, string path, string se)
		{
			this.nextMenuId = menuId;
			this.nextPageType = ButtonInfoTable.PageType.NON;
			this.btnMsgInfo = null;
			this.clickButtonPath = path;
			this.seName = se;
		}

		public ButtonInfo(MsgMenuSequence.SequeneceType menuId, ButtonInfoTable.PageType pageType)
		{
			this.nextMenuId = menuId;
			this.nextPageType = pageType;
			this.btnMsgInfo = null;
			this.clickButtonPath = string.Empty;
			this.seName = string.Empty;
		}
	}

	public enum ButtonType
	{
		PRESENT_BOX,
		DAILY_CHALLENGE,
		DAILY_BATTLE,
		CHARA_MAIN,
		CHARA_SUB,
		CHAO,
		VIRTUAL_NEW_ITEM,
		PLAY_ITEM,
		OPTION,
		INFOMATION,
		ROULETTE,
		CHAO_ROULETTE,
		REWARDLIST_TO_CHAO_ROULETTE,
		ITEM_ROULETTE,
		CHAO_TO_ROULETTE,
		EPISODE,
		EPISODE_PLAY,
		EPISODE_RANKING,
		QUICK,
		QUICK_RANKING,
		PLAY_AT_EPISODE_PAGE,
		PLAY_EVENT,
		PRESENT_BOX_BACK,
		DAILY_CHALLENGE_BACK,
		DAILY_BATTLE_BACK,
		CHARA_BACK,
		ITEM_BACK,
		CHAO_BACK,
		SHOP_BACK,
		EPISODE_BACK,
		EPISODE_PLAY_BACK,
		EPISODE_RANKING_BACK,
		QUICK_BACK,
		QUICK_RANKING_BACK,
		PLAY_AT_EPISODE_PAGE_BACK,
		INFOMATION_BACK,
		ROULETTE_BACK,
		OPTION_BACK,
		REDSTAR_TO_SHOP,
		RING_TO_SHOP,
		CHALLENGE_TO_SHOP,
		RAIDENERGY_TO_SHOP,
		EVENT_RAID,
		EVENT_SPECIAL,
		EVENT_COLLECT,
		EVENT_BACK,
		FORCE_MAIN_BACK,
		TITLE_BACK,
		GO_STAGE,
		NUM,
		UNKNOWN = -1
	}

	public readonly MsgMenuSequence.SequeneceType[] m_sequences = new MsgMenuSequence.SequeneceType[]
	{
		MsgMenuSequence.SequeneceType.MAIN,
		MsgMenuSequence.SequeneceType.CHAO,
		MsgMenuSequence.SequeneceType.EVENT_TOP,
		MsgMenuSequence.SequeneceType.INFOMATION,
		MsgMenuSequence.SequeneceType.PLAY_ITEM,
		MsgMenuSequence.SequeneceType.NON,
		MsgMenuSequence.SequeneceType.OPTION,
		MsgMenuSequence.SequeneceType.CHARA_MAIN,
		MsgMenuSequence.SequeneceType.CHARA_MAIN,
		MsgMenuSequence.SequeneceType.PRESENT_BOX,
		MsgMenuSequence.SequeneceType.DAILY_CHALLENGE,
		MsgMenuSequence.SequeneceType.DAILY_BATTLE,
		MsgMenuSequence.SequeneceType.ROULETTE,
		MsgMenuSequence.SequeneceType.SHOP,
		MsgMenuSequence.SequeneceType.SHOP,
		MsgMenuSequence.SequeneceType.SHOP,
		MsgMenuSequence.SequeneceType.SHOP,
		MsgMenuSequence.SequeneceType.EPISODE,
		MsgMenuSequence.SequeneceType.EPISODE_PLAY,
		MsgMenuSequence.SequeneceType.EPISODE_RANKING,
		MsgMenuSequence.SequeneceType.QUICK,
		MsgMenuSequence.SequeneceType.QUICK_RANKING,
		MsgMenuSequence.SequeneceType.PLAY_AT_EPISODE_PAGE
	};

	public readonly ButtonInfoTable.ButtonType[] m_platformBackButtonType = new ButtonInfoTable.ButtonType[]
	{
		ButtonInfoTable.ButtonType.TITLE_BACK,
		ButtonInfoTable.ButtonType.CHAO_BACK,
		ButtonInfoTable.ButtonType.EVENT_BACK,
		ButtonInfoTable.ButtonType.INFOMATION_BACK,
		ButtonInfoTable.ButtonType.ITEM_BACK,
		ButtonInfoTable.ButtonType.ITEM_BACK,
		ButtonInfoTable.ButtonType.OPTION_BACK,
		ButtonInfoTable.ButtonType.CHARA_BACK,
		ButtonInfoTable.ButtonType.CHARA_BACK,
		ButtonInfoTable.ButtonType.PRESENT_BOX_BACK,
		ButtonInfoTable.ButtonType.DAILY_CHALLENGE_BACK,
		ButtonInfoTable.ButtonType.DAILY_BATTLE_BACK,
		ButtonInfoTable.ButtonType.ROULETTE_BACK,
		ButtonInfoTable.ButtonType.SHOP_BACK,
		ButtonInfoTable.ButtonType.SHOP_BACK,
		ButtonInfoTable.ButtonType.SHOP_BACK,
		ButtonInfoTable.ButtonType.SHOP_BACK,
		ButtonInfoTable.ButtonType.EPISODE_BACK,
		ButtonInfoTable.ButtonType.EPISODE_PLAY_BACK,
		ButtonInfoTable.ButtonType.EPISODE_RANKING_BACK,
		ButtonInfoTable.ButtonType.QUICK_BACK,
		ButtonInfoTable.ButtonType.QUICK_RANKING_BACK,
		ButtonInfoTable.ButtonType.PLAY_AT_EPISODE_PAGE_BACK
	};

	public static readonly ButtonInfoTable.AnimInfo[] m_animInfos = new ButtonInfoTable.AnimInfo[]
	{
		new ButtonInfoTable.AnimInfo("MainMenuUI4", "ui_mm_Anim"),
		new ButtonInfoTable.AnimInfo("ChaoSetUI", "ui_mm_chao_Anim"),
		null,
		new ButtonInfoTable.AnimInfo("InformationUI", "ui_daily_challenge_infomation_intro_Anim"),
		new ButtonInfoTable.AnimInfo("ItemSet_3_UI", "ui_itemset_3_intro_Anim"),
		null,
		new ButtonInfoTable.AnimInfo("OptionUI", "ui_menu_option_intro_Anim"),
		new ButtonInfoTable.AnimInfo("PlayerSet_3_UI", "ui_mm_player_set_2_intro_Anim"),
		new ButtonInfoTable.AnimInfo("PlayerSet_2_UI", "ui_mm_player_set_2_intro_Anim"),
		new ButtonInfoTable.AnimInfo("PresentBoxUI", "ui_menu_presentbox_intro_Anim"),
		null,
		new ButtonInfoTable.AnimInfo("DailyInfoUI", "ui_daily_challenge_infomation_intro_Anim"),
		new ButtonInfoTable.AnimInfo("RouletteTopUI", null),
		new ButtonInfoTable.AnimInfo("ShopUI2", "ui_mm_shop_intro_Anim"),
		new ButtonInfoTable.AnimInfo("ShopUI2", "ui_mm_shop_intro_Anim"),
		new ButtonInfoTable.AnimInfo("ShopUI2", "ui_mm_shop_intro_Anim"),
		new ButtonInfoTable.AnimInfo("ShopUI2", "ui_mm_shop_intro_Anim"),
		null,
		new ButtonInfoTable.AnimInfo("ItemSet_3_UI", "ui_itemset_3_intro_Anim"),
		null,
		new ButtonInfoTable.AnimInfo("ItemSet_3_UI", "ui_itemset_3_intro_Anim"),
		null,
		new ButtonInfoTable.AnimInfo("ItemSet_3_UI", "ui_itemset_3_intro_Anim")
	};

	public readonly ButtonInfoTable.MessageInfo[] m_msgInfosForPages;

	public readonly ButtonInfoTable.MessageInfo[] m_msgInfosForEndPages;

	public readonly ButtonInfoTable.ButtonInfo[] m_button_info;

	public ButtonInfoTable()
	{
		ButtonInfoTable.MessageInfo[] expr_119 = new ButtonInfoTable.MessageInfo[23];
		expr_119[1] = new ButtonInfoTable.MessageInfo("ChaoSetUI", "OnStartChaoSet", "ChaoSetUI", true);
		expr_119[3] = new ButtonInfoTable.MessageInfo("InformationUI", "OnStartInformation", "InformationUI", true);
		expr_119[6] = new ButtonInfoTable.MessageInfo("OptionUI", "OnStartOptionUI", "OptionUI", true);
		expr_119[7] = new ButtonInfoTable.MessageInfo("PlayerSet_3_UI", "Setup", "PlayerCharaList", true);
		expr_119[8] = new ButtonInfoTable.MessageInfo("PlayerSet_2_UI", "StartSubCharacter", "MenuPlayerSet", true);
		expr_119[9] = new ButtonInfoTable.MessageInfo("PresentBoxUI", "OnStartPresentBox", "PresentBoxUI", true);
		expr_119[11] = new ButtonInfoTable.MessageInfo("DailyInfoUI", "Setup", "DailyInfo", true);
		expr_119[12] = new ButtonInfoTable.MessageInfo("RouletteTopUI", "OnRouletteOpenDefault", "RouletteTop", true);
		expr_119[13] = new ButtonInfoTable.MessageInfo("ShopUI2", "OnStartShopRedStarRing", "ShopUI", true);
		expr_119[14] = new ButtonInfoTable.MessageInfo("ShopUI2", "OnStartShopRing", "ShopUI", true);
		expr_119[15] = new ButtonInfoTable.MessageInfo("ShopUI2", "OnStartShopChallenge", "ShopUI", true);
		expr_119[16] = new ButtonInfoTable.MessageInfo("ShopUI2", "OnStartShopEvent", "ShopUI", true);
		expr_119[17] = new ButtonInfoTable.MessageInfo("ui_mm_mileage2_page", "OnStartMileage", "ui_mm_mileage_page", true);
		expr_119[19] = new ButtonInfoTable.MessageInfo("ui_mm_ranking_page", "SetDisplayEndlessModeOn", "RankingUI", true);
		expr_119[21] = new ButtonInfoTable.MessageInfo("ui_mm_ranking_page", "SetDisplayQuickModeOn", "RankingUI", true);
		this.m_msgInfosForPages = expr_119;
		ButtonInfoTable.MessageInfo[] expr_298 = new ButtonInfoTable.MessageInfo[23];
		expr_298[1] = new ButtonInfoTable.MessageInfo("ChaoSetUI", "OnMsgMenuBack", "ChaoSetUI", true);
		expr_298[3] = new ButtonInfoTable.MessageInfo("InformationUI", "OnEndInformation", "InformationUI", true);
		expr_298[4] = new ButtonInfoTable.MessageInfo("ItemSet_3_UI", "OnMsgMenuBack", "ItemSetMenu", true);
		expr_298[6] = new ButtonInfoTable.MessageInfo("MainMenuButtonEvent", "OnOptionBackButtonClicked", string.Empty, false);
		expr_298[9] = new ButtonInfoTable.MessageInfo("PresentBoxUI", "OnEndPresentBox", "PresentBoxUI", true);
		expr_298[11] = new ButtonInfoTable.MessageInfo("DailyInfoUI", "OnClickBackButton", "DailyInfo", true);
		expr_298[12] = new ButtonInfoTable.MessageInfo("RouletteTopUI", "OnRouletteEnd", "RouletteTop", true);
		expr_298[13] = new ButtonInfoTable.MessageInfo("ShopUI2", "OnShopBackButtonClicked", "ShopUI", true);
		expr_298[14] = new ButtonInfoTable.MessageInfo("ShopUI2", "OnShopBackButtonClicked", "ShopUI", true);
		expr_298[15] = new ButtonInfoTable.MessageInfo("ShopUI2", "OnShopBackButtonClicked", "ShopUI", true);
		expr_298[16] = new ButtonInfoTable.MessageInfo("ShopUI2", "OnShopBackButtonClicked", "ShopUI", true);
		expr_298[17] = new ButtonInfoTable.MessageInfo("ui_mm_mileage2_page", "OnEndMileage", "ui_mm_mileage_page", true);
		expr_298[19] = new ButtonInfoTable.MessageInfo("ui_mm_ranking_page", "SetDisplayEndlessModeOff", "RankingUI", true);
		expr_298[21] = new ButtonInfoTable.MessageInfo("ui_mm_ranking_page", "SetDisplayQuickModeOff", "RankingUI", true);
		this.m_msgInfosForEndPages = expr_298;
		this.m_button_info = new ButtonInfoTable.ButtonInfo[]
		{
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.PRESENT_BOX, ButtonInfoTable.PageType.PRESENT_BOX, "MainMenuUI4/Anchor_7_BL/Btn_2_presentbox", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.DAILY_CHALLENGE, ButtonInfoTable.PageType.DAILY_CHALLENGE, "MainMenuUI4/Anchor_9_BR/Btn_1_challenge", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.DAILY_BATTLE, ButtonInfoTable.PageType.DAILY_BATTLE, "MainMenuUI4/Anchor_5_MC/1_Quick/Btn_2_battle", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.CHARA_MAIN, ButtonInfoTable.PageType.PLAYER_MAIN, "MainMenuUI4/Anchor_5_MC/2_Character/Btn_2_player", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.CHARA_MAIN, ButtonInfoTable.PageType.PLAYER_SUB, "MainMenuUI4/Anchor_5_MC/mainmenu_contents/grid/page_3/slot/ui_mm_main2_page(Clone)/player_set/Btn_player_sub", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.CHAO, ButtonInfoTable.PageType.CHAO, "MainMenuUI4/Anchor_5_MC/2_Character/Btn_1_chao", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.PLAY_ITEM, ButtonInfoTable.PageType.ITEM),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.STAGE_CHECK, ButtonInfoTable.PageType.NON),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.OPTION, ButtonInfoTable.PageType.OPTION, "MainMenuUI4/Anchor_7_BL/Btn_1_Option", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.INFOMATION, ButtonInfoTable.PageType.INFOMATION, "MainMenuUI4/Anchor_7_BL/Btn_0_info", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.ROULETTE, ButtonInfoTable.PageType.ROULETTE, "MainMenuUI4/Anchor_8_BC/Btn_roulette", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.CHAO_ROULETTE, ButtonInfoTable.PageType.ROULETTE),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.CHAO_ROULETTE, ButtonInfoTable.PageType.ROULETTE),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.ITEM_ROULETTE, ButtonInfoTable.PageType.ROULETTE),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.CHAO_ROULETTE, ButtonInfoTable.PageType.ROULETTE, "ChaoSetUIPage/ChaoSetUI/Anchor_7_BL/mainmenu_btn_1c/Btn_roulette", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.EPISODE, ButtonInfoTable.PageType.EPISODE, "MainMenuUI4/Anchor_5_MC/0_Endless/Btn_2_mileage", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.MAIN_PLAY_BUTTON, ButtonInfoTable.PageType.EPISODE_PLAY, "MainMenuUI4/Anchor_5_MC/0_Endless/Btn_3_play", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.EPISODE_RANKING, ButtonInfoTable.PageType.EPISODE_RANKING, "MainMenuUI4/Anchor_5_MC/0_Endless/Btn_1_ranking", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.QUICK, ButtonInfoTable.PageType.QUICK, "MainMenuUI4/Anchor_5_MC/1_Quick/Btn_3_play", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.QUICK_RANKING, ButtonInfoTable.PageType.QUICK_RANKING, "MainMenuUI4/Anchor_5_MC/1_Quick/Btn_1_ranking", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.PLAY_AT_EPISODE_PAGE, ButtonInfoTable.PageType.PLAY_AT_EPISODE_PAGE, "ui_mm_mileage2_page/Anchor_9_BR/Btn_play", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.EVENT_TOP, ButtonInfoTable.PageType.EVENT, string.Empty, "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "PresentBoxUI/Anchor_7_BL/mainmenu_btn_1b/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, string.Empty, "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "DailyInfoUI/Anchor_7_BL/mainmenu_btn_1b/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "PlayerSet_3_UI/Anchor_7_BL/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "ItemSet_3_UI/Anchor_7_BL/mainmenu_btn_1b/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "ChaoSetUIPage/ChaoSetUI/Anchor_7_BL/mainmenu_btn_1c/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "ShopPage/ShopUI2/Anchor_7_BL/mainmenu_btn_1b/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "ShopPage/ShopUI2/Anchor_7_BL/mainmenu_btn_1b/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "ui_mm_mileage2_page/Anchor_7_BL/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "ui_mm_ranking_page/Anchor_7_BL/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "ShopPage/ShopUI2/Anchor_7_BL/mainmenu_btn_1b/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "ui_mm_ranking_page/Anchor_7_BL/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "ShopPage/ShopUI2/Anchor_7_BL/mainmenu_btn_1b/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "InformationUI/Anchor_7_BL/mainmenu_btn_1b/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "RouletteUI/Anchor_7_BL/roulette_btn_2/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, "OptionUI/Anchor_7_BL/option_btn/Btn_cmn_back", "sys_menu_decide"),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.SHOP, ButtonInfoTable.PageType.SHOP_RSR, "MainMenuCmnUI/Anchor_3_TR/mainmenu_info_quantum/Btn_shop/Btn_charge_rsring", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.SHOP, ButtonInfoTable.PageType.SHOP_RING, "MainMenuCmnUI/Anchor_3_TR/mainmenu_info_quantum/Btn_shop/Btn_charge_stock", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.SHOP, ButtonInfoTable.PageType.SHOP_ENERGY, "MainMenuCmnUI/Anchor_3_TR/mainmenu_info_quantum/Btn_shop/Btn_charge_challenge", "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.SHOP, ButtonInfoTable.PageType.SHOP_EVENT, string.Empty, "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.EVENT_RAID, ButtonInfoTable.PageType.EVENT, string.Empty, "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.EVENT_SPECIAL, ButtonInfoTable.PageType.EVENT, string.Empty, "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.EVENT_COLLECT, ButtonInfoTable.PageType.EVENT, string.Empty, "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, ButtonInfoTable.PageType.NON, string.Empty, "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.MAIN, ButtonInfoTable.PageType.MAIN, string.Empty, "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.BACK, ButtonInfoTable.PageType.NON, string.Empty, "sys_menu_decide", null),
			new ButtonInfoTable.ButtonInfo(MsgMenuSequence.SequeneceType.STAGE, ButtonInfoTable.PageType.STAGE, string.Empty, "sys_menu_decide", null)
		};
		
	}

	public void PlaySE(ButtonInfoTable.ButtonType button_type)
	{
		if (button_type < ButtonInfoTable.ButtonType.NUM && !string.IsNullOrEmpty(this.m_button_info[(int)button_type].seName))
		{
			SoundManager.SePlay(this.m_button_info[(int)button_type].seName, "SE");
		}
	}

	public ButtonInfoTable.PageType GetPageType(ButtonInfoTable.ButtonType button_type)
	{
		if (button_type < ButtonInfoTable.ButtonType.NUM)
		{
			return this.m_button_info[(int)button_type].nextPageType;
		}
		return ButtonInfoTable.PageType.NON;
	}

	public MsgMenuSequence.SequeneceType GetSequeneceType(ButtonInfoTable.ButtonType button_type)
	{
		if (button_type < ButtonInfoTable.ButtonType.NUM)
		{
			return this.m_button_info[(int)button_type].nextMenuId;
		}
		return MsgMenuSequence.SequeneceType.NON;
	}

	public MsgMenuSequence.SequeneceType GetSequeneceType(ButtonInfoTable.PageType pageType)
	{
		if (pageType != ButtonInfoTable.PageType.NON && pageType < ButtonInfoTable.PageType.NUM)
		{
			return this.m_sequences[(int)pageType];
		}
		return MsgMenuSequence.SequeneceType.NON;
	}

	public ButtonInfoTable.AnimInfo GetPageAnimInfo(ButtonInfoTable.PageType pageType)
	{
		if (pageType != ButtonInfoTable.PageType.NON && pageType < ButtonInfoTable.PageType.NUM)
		{
			return ButtonInfoTable.m_animInfos[(int)pageType];
		}
		return null;
	}

	public ButtonInfoTable.MessageInfo GetPageMessageInfo(ButtonInfoTable.PageType page, bool start)
	{
		if (page == ButtonInfoTable.PageType.NON || page >= ButtonInfoTable.PageType.NUM)
		{
			return null;
		}
		if (start)
		{
			return this.m_msgInfosForPages[(int)page];
		}
		return this.m_msgInfosForEndPages[(int)page];
	}

	public GameObject GetDisplayObj(ButtonInfoTable.PageType nextPageType)
	{
		if (nextPageType != ButtonInfoTable.PageType.NON && nextPageType < ButtonInfoTable.PageType.NUM && ButtonInfoTable.m_animInfos[(int)nextPageType] != null)
		{
			GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
			if (cameraUIObject != null)
			{
				return GameObjectUtil.FindChildGameObject(cameraUIObject, ButtonInfoTable.m_animInfos[(int)nextPageType].targetName);
			}
		}
		return null;
	}
}
