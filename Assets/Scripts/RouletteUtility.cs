using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class RouletteUtility
{
	public enum AchievementType
	{
		NONE,
		PlayerGet,
		ChaoGet,
		LevelUp,
		LevelMax,
		Multi
	}

	public enum NextType
	{
		NONE,
		EQUIP,
		CHARA_EQUIP
	}

	public enum CellType
	{
		Item,
		Egg,
		Rankup
	}

	public enum WheelRank
	{
		Normal,
		Big,
		Super,
		MAX
	}

	public enum WheelType
	{
		NONE,
		Normal,
		Rankup
	}

	public enum RouletteColor
	{
		NONE,
		Blue,
		Purple,
		Green,
		Silver,
		Gold
	}

	public const bool ROULETTE_PARTS_DSTROY = false;

	public const bool ITEM_ROULETTE_USE_RING = false;

	public const bool ROULETTE_CHANGE_EFFECT = false;

	public const float ROULETTE_BASIC_RELOAD_SPAN = 5f;

	public const float ROULETTE_MULTI_GET_EFFECT_TIME = 5f;

	public const float ROULETTE_SPIN_WAIT_LIMIT_TIME = 10f;

	public const int ROULETTE_MULTI_NUM_0 = 1;

	public const int ROULETTE_MULTI_NUM_1 = 3;

	public const int ROULETTE_MULTI_NUM_2 = 5;

	public const int ROULETTE_TUTORIAL_ADD_SP_EGG = 10;

	private const string ROULETTE_CHANGE_ICON_SPRITE_NAME = "ui_roulette_pager_icon_{CATEGORY}";

	private const string ROULETTE_BG_SPRITE_NAME = "ui_roulette_tablebg_{COLOR}";

	private const string ROULETTE_BOARD_SPRITE_NAME = "ui_roulette_table_{COLOR}_{TYPE}";

	private const string ROULETTE_ARROW_SPRITE_NAME = "ui_roulette_arrow_{COLOR}";

	private const string ROULETTE_COST_ITEM_SPRITE_NAME = "ui_cmn_icon_item_{ID}";

	private const string ROULETTE_HEADER_NAME = "ui_Header_{TYPE}_Roulette";

	public static readonly int OddsDisplayDecimal = 2;

	private static bool s_itemRouletteUse;

	private static bool s_loginRoulette;

	private static RouletteCategory s_rouletteDefault = RouletteCategory.ITEM;

	public static ServerSpinResultGeneral s_spinResult;

	public static int s_spinResultCount;

	private static bool s_rouletteTurtorialEnd;

	private static bool s_rouletteTurtorial;

	private static bool s_rouletteTurtorialLock;

	private static string s_jackpotFeedText = string.Empty;

	public static RouletteCategory rouletteDefault
	{
		get
		{
			return RouletteUtility.s_rouletteDefault;
		}
		set
		{
			RouletteUtility.s_rouletteDefault = value;
		}
	}

	public static bool loginRoulette
	{
		get
		{
			return RouletteUtility.s_loginRoulette;
		}
		set
		{
			RouletteUtility.s_loginRoulette = value;
		}
	}

	public static string jackpotFeedText
	{
		get
		{
			return RouletteUtility.s_jackpotFeedText;
		}
	}

	public static bool rouletteTurtorialEnd
	{
		get
		{
			return RouletteUtility.s_rouletteTurtorialEnd;
		}
		set
		{
			RouletteUtility.s_rouletteTurtorial = false;
			RouletteUtility.s_rouletteTurtorialEnd = value;
			if (!RouletteUtility.s_rouletteTurtorialEnd)
			{
				RouletteUtility.s_rouletteTurtorialLock = true;
			}
		}
	}

	public static bool isTutorial
	{
		get
		{
			bool flag = false;
			if (RouletteUtility.s_rouletteTurtorialLock)
			{
				return false;
			}
			if (RouletteUtility.s_rouletteTurtorial)
			{
				flag = true;
			}
			else if (ServerInterface.ChaoWheelOptions != null && !RouletteUtility.s_rouletteTurtorialEnd)
			{
				flag = ServerInterface.ChaoWheelOptions.IsTutorial;
				if (flag)
				{
					RouletteUtility.s_rouletteTurtorial = true;
				}
			}
			return flag;
		}
	}

	public static RouletteTicketCategory GetRouletteTicketCategory(int itemId)
	{
		RouletteTicketCategory result = RouletteTicketCategory.NONE;
		if (itemId > 229999 && itemId <= 299999)
		{
			if (itemId >= 230000 && itemId < 240000)
			{
				result = RouletteTicketCategory.PREMIUM;
			}
			else if (itemId >= 240000 && itemId < 250000)
			{
				result = RouletteTicketCategory.ITEM;
			}
			else if (itemId >= 250000 && itemId < 260000)
			{
				result = RouletteTicketCategory.RAID;
			}
			else if (itemId >= 260000 && itemId < 270000)
			{
				result = RouletteTicketCategory.EVENT;
			}
		}
		return result;
	}

	public static bool SetItemRouletteUseRing(bool use)
	{
		return false;
	}

	public static string GetRouletteCostItemName(int costItemId)
	{
		string text = "ui_cmn_icon_item_{ID}";
		return text.Replace("{ID}", costItemId.ToString());
	}

	public static string GetRouletteColorName(RouletteUtility.RouletteColor rcolor)
	{
		string result = null;
		switch (rcolor)
		{
		case RouletteUtility.RouletteColor.Blue:
			result = "blu";
			break;
		case RouletteUtility.RouletteColor.Purple:
			result = "pur";
			break;
		case RouletteUtility.RouletteColor.Green:
			result = "gre";
			break;
		case RouletteUtility.RouletteColor.Silver:
			result = "sil";
			break;
		case RouletteUtility.RouletteColor.Gold:
			result = "gol";
			break;
		}
		return result;
	}

	public static RouletteUtility.WheelRank GetRouletteRank(int rank)
	{
		RouletteUtility.WheelRank result = RouletteUtility.WheelRank.Normal;
		switch (rank % 100)
		{
		case 0:
			result = RouletteUtility.WheelRank.Normal;
			break;
		case 1:
			result = RouletteUtility.WheelRank.Big;
			break;
		case 2:
			result = RouletteUtility.WheelRank.Super;
			break;
		}
		return result;
	}

	public static string GetRouletteChangeIconSpriteName(RouletteCategory category)
	{
		string text = "ui_roulette_pager_icon_{CATEGORY}";
		int num = (int)category;
		return text.Replace("{CATEGORY}", num.ToString());
	}

	public static string GetRouletteBgSpriteName(ServerWheelOptionsGeneral wheel)
	{
		string text = "ui_roulette_tablebg_{COLOR}";
		string rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Green);
		RouletteUtility.WheelType type = wheel.type;
		if (type != RouletteUtility.WheelType.Normal)
		{
			if (type == RouletteUtility.WheelType.Rankup)
			{
				rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Green);
			}
		}
		else
		{
			rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Blue);
		}
		return text.Replace("{COLOR}", rouletteColorName);
	}

	public static string GetRouletteBoardSpriteName(ServerWheelOptionsGeneral wheel)
	{
		RouletteCategory rouletteCategory = RouletteUtility.GetRouletteCategory(wheel);
		string text = "ui_roulette_table_{COLOR}_{TYPE}";
		string rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Green);
		int patternType = wheel.patternType;
		string str = string.Empty;
		RouletteUtility.WheelRank rank = wheel.rank;
		RouletteUtility.WheelType type = wheel.type;
		if (type != RouletteUtility.WheelType.Normal)
		{
			if (type == RouletteUtility.WheelType.Rankup)
			{
				switch (rank)
				{
				case RouletteUtility.WheelRank.Normal:
					rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Green);
					break;
				case RouletteUtility.WheelRank.Big:
					rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Silver);
					break;
				case RouletteUtility.WheelRank.Super:
					rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Gold);
					str = "r";
					break;
				}
			}
		}
		else if (rouletteCategory != RouletteCategory.SPECIAL)
		{
			rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Silver);
		}
		else
		{
			rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Gold);
		}
		text = text.Replace("{COLOR}", rouletteColorName);
		text = text.Replace("{TYPE}", patternType.ToString());
		return text + str;
	}

	public static string GetRouletteArrowSpriteName(ServerWheelOptionsGeneral wheel)
	{
		string text = "ui_roulette_arrow_{COLOR}";
		string rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Silver);
		RouletteUtility.WheelType type = wheel.type;
		if (type != RouletteUtility.WheelType.Normal)
		{
			if (type == RouletteUtility.WheelType.Rankup)
			{
				if (wheel.rank == RouletteUtility.WheelRank.Super)
				{
					rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Gold);
				}
				else
				{
					rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Silver);
				}
			}
		}
		else if (wheel.rank == RouletteUtility.WheelRank.Normal)
		{
			rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Silver);
		}
		else
		{
			rouletteColorName = RouletteUtility.GetRouletteColorName(RouletteUtility.RouletteColor.Gold);
		}
		return text.Replace("{COLOR}", rouletteColorName);
	}

	public static RouletteCategory GetRouletteCategory(ServerWheelOptionsGeneral wheel)
	{
		RouletteCategory result = RouletteCategory.NONE;
		if (wheel != null && wheel.rouletteId >= 0)
		{
			result = RouletteCategory.RAID;
		}
		return result;
	}

	public static bool ChangeRouletteHeader(RouletteCategory category)
	{
		bool result = false;
		if (category != RouletteCategory.ALL)
		{
			string rouletteCategoryName = RouletteUtility.GetRouletteCategoryName(category);
			if (!string.IsNullOrEmpty(rouletteCategoryName))
			{
				string text = "ui_Header_{TYPE}_Roulette";
				text = text.Replace("{TYPE}", rouletteCategoryName);
				HudMenuUtility.SendChangeHeaderText(text);
				result = true;
			}
		}
		else
		{
			string cellName = "ui_Header_Roulette_top";
			HudMenuUtility.SendChangeHeaderText(cellName);
			result = true;
		}
		return result;
	}

	public static string GetRouletteCategoryHeaderText(RouletteCategory category)
	{
		string result = string.Empty;
		string text = string.Empty;
		if (category != RouletteCategory.ALL)
		{
			string rouletteCategoryName = RouletteUtility.GetRouletteCategoryName(category);
			if (!string.IsNullOrEmpty(rouletteCategoryName))
			{
				text = "ui_Header_{TYPE}_Roulette";
				text = text.Replace("{TYPE}", rouletteCategoryName);
			}
		}
		else
		{
			text = "ui_Header_Roulette_top";
		}
		TextObject text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", text);
		if (text2 != null)
		{
			result = text2.text;
		}
		return result;
	}

	public static string GetRouletteCategoryName(RouletteCategory category)
	{
		string result = null;
		if (category != RouletteCategory.NONE && category != RouletteCategory.ALL)
		{
			switch (category)
			{
			case RouletteCategory.PREMIUM:
				result = "Premium";
				break;
			case RouletteCategory.ITEM:
				result = "Item";
				break;
			case RouletteCategory.RAID:
				result = "Raidboss";
				break;
			case RouletteCategory.EVENT:
				result = "Event";
				break;
			case RouletteCategory.SPECIAL:
				result = "Special";
				break;
			}
		}
		return result;
	}

	private static void ShowItem(int id, int num)
	{
		ServerItem serverItem = new ServerItem((ServerItem.Id)id);
		if (serverItem.idType != ServerItem.IdType.ITEM_ROULLETE_WIN)
		{
			ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
			if (itemGetWindow != null)
			{
				itemGetWindow.Create(new ItemGetWindow.CInfo
				{
					name = "ItemGet",
					caption = RouletteUtility.GetText("gw_item_caption", null),
					serverItemId = id,
					imageCount = RouletteUtility.GetText("gw_item_text", "{COUNT}", HudUtility.GetFormatNumString<int>(num))
				});
			}
		}
		else
		{
			RouletteUtility.ShowJackpot(num);
		}
	}

	private static void ShowJackpot(int jackpotRing)
	{
		RouletteManager.isShowGetWindow = true;
		int serverItemId = 910000;
		ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
		if (itemGetWindow != null)
		{
			itemGetWindow.Create(new ItemGetWindow.CInfo
			{
				name = "Jackpot",
				buttonType = ItemGetWindow.ButtonType.TweetCancel,
				caption = RouletteUtility.GetText("gw_jackpot_caption", null),
				serverItemId = serverItemId,
				imageCount = RouletteUtility.GetText("gw_jackpot_text", "{COUNT}", HudUtility.GetFormatNumString<int>(RouletteManager.numJackpotRing))
			});
			RouletteUtility.s_jackpotFeedText = RouletteUtility.GetText("feed_jackpot_text", "{COUNT}", HudUtility.GetFormatNumString<int>(RouletteManager.numJackpotRing));
			RouletteManager.numJackpotRing = jackpotRing;
		}
	}

	public static void ShowGetWindow(ServerWheelOptions data)
	{
		GameObject x = GameObject.Find("UI Root (2D)");
		if (x != null)
		{
			RouletteManager.isShowGetWindow = true;
			BackKeyManager.InvalidFlag = false;
			int id = data.m_items[data.m_itemWon];
			int num = data.m_itemQuantities[data.m_itemWon];
			ServerItem serverItem = new ServerItem((ServerItem.Id)id);
			if (serverItem.idType == ServerItem.IdType.ITEM_ROULLETE_WIN && data.m_rouletteRank == RouletteUtility.WheelRank.Super)
			{
				RouletteUtility.ShowJackpot(data.m_numJackpotRing);
			}
			else if (serverItem.idType == ServerItem.IdType.CHAO || serverItem.idType == ServerItem.IdType.CHARA)
			{
				ServerChaoState chao = data.GetChao();
				if (chao != null)
				{
					RouletteUtility.ShowOtomo(chao, !data.IsItemList(), data.m_itemList, data.NumRequiredSpEggs, false);
				}
			}
			else
			{
				RouletteUtility.ShowItem(id, num);
			}
		}
	}

	public static void ShowGetWindow(ServerSpinResultGeneral data)
	{
		RouletteManager.isShowGetWindow = false;
		RouletteUtility.s_spinResult = null;
		RouletteUtility.s_spinResultCount = -1;
		GameObject x = GameObject.Find("UI Root (2D)");
		if (x != null)
		{
			global::Debug.Log("ShowGetWindow ItemWon:" + data.ItemWon + " !!!!!!!!");
			if (data.ItemWon >= 0)
			{
				if (data.AcquiredChaoData.Count > 0)
				{
					Dictionary<int, ServerChaoData>.KeyCollection keys = data.AcquiredChaoData.Keys;
					using (Dictionary<int, ServerChaoData>.KeyCollection.Enumerator enumerator = keys.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							int current = enumerator.Current;
							RouletteUtility.ShowOtomo(data.AcquiredChaoData[current], data.IsRequiredChao[current], data.ItemState, data.NumRequiredSpEggs, false);
						}
					}
				}
				else if (data.ItemState.Count > 0)
				{
					Dictionary<int, ServerItemState>.KeyCollection keys2 = data.ItemState.Keys;
					using (Dictionary<int, ServerItemState>.KeyCollection.Enumerator enumerator2 = keys2.GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							int current2 = enumerator2.Current;
							RouletteUtility.ShowItem(data.ItemState[current2].m_itemId, data.ItemState[current2].m_num);
						}
					}
				}
				else
				{
					global::Debug.Log("RouletteUtility ShowGetWindow G  single error?");
				}
			}
			else
			{
				RouletteUtility.s_spinResult = data;
				RouletteUtility.s_spinResultCount = data.GetOtomoAndCharaMax() - 1;
				RouletteManager.isShowGetWindow = false;
				global::Debug.Log("ShowGetWindow ResultCount:" + RouletteUtility.s_spinResultCount + " !!!!!!!!");
				if (RouletteUtility.s_spinResultCount >= 0)
				{
					RouletteManager.isMultiGetWindow = true;
					RouletteUtility.ShowGetAllOtomoAndChara();
				}
				else
				{
					RouletteManager.isMultiGetWindow = false;
					string acquiredListText = data.AcquiredListText;
					if (!string.IsNullOrEmpty(acquiredListText))
					{
						GeneralWindow.Create(new GeneralWindow.CInfo
						{
							name = "RouletteGetAllList",
							buttonType = GeneralWindow.ButtonType.Ok,
							caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "GeneralWindow", "ui_Lbl_get_list").text,
							message = acquiredListText
						});
					}
				}
			}
		}
	}

	public static bool IsGetOtomoOrCharaWindow()
	{
		bool flag = false;
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject != null)
		{
			ChaoGetWindow chaoGetWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoGetWindow>(gameObject, "ro_PlayerGetWindowUI");
			if (chaoGetWindow != null && chaoGetWindow.gameObject.activeSelf)
			{
				flag = true;
			}
			if (!flag)
			{
				chaoGetWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoGetWindow>(gameObject, "chao_get_Window");
				if (chaoGetWindow != null && chaoGetWindow.gameObject.activeSelf)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				chaoGetWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoGetWindow>(gameObject, "chao_rare_get_Window");
				if (chaoGetWindow != null && chaoGetWindow.gameObject.activeSelf)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				ChaoMergeWindow chaoMergeWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoMergeWindow>(gameObject, "chao_merge_Window");
				if (chaoMergeWindow != null && chaoMergeWindow.gameObject.activeSelf)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				PlayerMergeWindow playerMergeWindow = GameObjectUtil.FindChildGameObjectComponent<PlayerMergeWindow>(gameObject, "player_merge_Window");
				if (playerMergeWindow != null && playerMergeWindow.gameObject.activeSelf)
				{
					flag = true;
				}
			}
		}
		return flag;
	}

	public static bool ShowGetAllOtomoAndChara()
	{
		bool result = false;
		if (RouletteUtility.s_spinResult != null && RouletteUtility.s_spinResultCount >= 0)
		{
			ServerChaoData showData = RouletteUtility.s_spinResult.GetShowData(RouletteUtility.s_spinResultCount);
			if (showData != null)
			{
				RouletteUtility.ShowOtomo(showData, true, null, 0, true);
				result = true;
			}
			RouletteUtility.s_spinResultCount--;
		}
		return result;
	}

	public static void ShowGetAllListEnd()
	{
		string name = "RouletteGetAllListEnd";
		string text = string.Empty;
		string text2 = string.Empty;
		GeneralWindow.ButtonType buttonType = GeneralWindow.ButtonType.YesNo;
		if (RouletteUtility.s_spinResult != null)
		{
			text2 = RouletteUtility.s_spinResult.AcquiredListText;
			bool flag = RouletteUtility.s_spinResult.CheckGetChara();
			if (!string.IsNullOrEmpty(text2))
			{
				string text3 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Roulette", "get_item_list_text").text;
				if (!string.IsNullOrEmpty(text3))
				{
					text = text3.Replace("{PARAN}", text2);
					if (flag)
					{
						name = "RouletteGetAllListEndChara";
						string text4 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_Header_PlayerSet").text;
						text = text.Replace("{PAGE}", text4);
					}
					else
					{
						name = "RouletteGetAllListEndChao";
						string text5 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "ui_Header_ChaoSet").text;
						text = text.Replace("{PAGE}", text5);
					}
				}
				else
				{
					text = text2;
					buttonType = GeneralWindow.ButtonType.Ok;
				}
			}
		}
		RouletteManager.isShowGetWindow = true;
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = name,
			buttonType = buttonType,
			caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "GeneralWindow", "ui_Lbl_get_list").text,
			message = text
		});
	}

	private static void ShowOtomo(ServerChaoData data, bool required, Dictionary<int, ServerItemState> itemState, int numRequiredSpEggs, bool multi)
	{
		ServerItem serverItem = new ServerItem((ServerItem.Id)data.Id);
		GameObject uiRoot = GameObject.Find("UI Root (2D)");
		if (data.Rarity == 100 || serverItem.idType == ServerItem.IdType.CHARA)
		{
			RouletteUtility.ShowGetWindowChara(data, uiRoot, itemState, multi);
		}
		else if (data.Level == 0)
		{
			RouletteUtility.ShowGetWindowOtomo(data, uiRoot, multi);
		}
		else if (RouletteUtility.IsLevelMaxChao(data.Id) && !required)
		{
			if (numRequiredSpEggs > 0)
			{
				RouletteUtility.ShowGetWindowOtomoMax(data, uiRoot, numRequiredSpEggs);
			}
			else
			{
				RouletteUtility.ShowGetWindowOtomoLvup(data, uiRoot, multi);
			}
		}
		else if (!multi && !required)
		{
			RouletteUtility.ShowGetWindowOtomoMax(data, uiRoot, numRequiredSpEggs);
		}
		else
		{
			RouletteUtility.ShowGetWindowOtomoLvup(data, uiRoot, multi);
		}
	}

	public static void ShowGetWindow(ServerChaoSpinResult data)
	{
		RouletteManager.isShowGetWindow = false;
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject != null)
		{
			ServerItem serverItem = new ServerItem((ServerItem.Id)data.AcquiredChaoData.Id);
			if (data.AcquiredChaoData.Rarity == 100 || serverItem.idType == ServerItem.IdType.CHARA)
			{
				RouletteUtility.ShowGetWindowChara(data.AcquiredChaoData, gameObject, data.ItemState, false);
			}
			else if (data.AcquiredChaoData.Level == 0)
			{
				RouletteUtility.ShowGetWindowOtomo(data.AcquiredChaoData, gameObject, false);
			}
			else if (RouletteUtility.IsLevelMaxChao(data.AcquiredChaoData.Id) && !data.IsRequiredChao)
			{
				RouletteUtility.ShowGetWindowOtomoMax(data.AcquiredChaoData, gameObject, data.NumRequiredSpEggs);
			}
			else
			{
				RouletteUtility.ShowGetWindowOtomoLvup(data.AcquiredChaoData, gameObject, false);
			}
		}
	}

	private static void ShowGetWindowChara(ServerChaoData data, GameObject uiRoot, Dictionary<int, ServerItemState> itemState, bool multi)
	{
		int id = data.Id;
		int rarity = data.Rarity;
		int level = data.Level;
		RouletteUtility.AchievementType achievement = RouletteUtility.AchievementType.PlayerGet;
		ServerPlayerState playerState = ServerInterface.PlayerState;
		CharacterDataNameInfo.Info dataByServerID = CharacterDataNameInfo.Instance.GetDataByServerID(id);
		ServerCharacterState serverCharacterState = playerState.CharacterState(dataByServerID.m_ID);
		if ((itemState == null || (itemState != null && itemState.Count == 0)) && serverCharacterState.star > 0)
		{
			PlayerMergeWindow playerMergeWindow = GameObjectUtil.FindChildGameObjectComponent<PlayerMergeWindow>(uiRoot, "player_merge_Window");
			if (playerMergeWindow != null)
			{
				playerMergeWindow.PlayStart(id, achievement);
			}
		}
		else
		{
			ChaoGetWindow chaoGetWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoGetWindow>(uiRoot, "ro_PlayerGetWindowUI");
			if (chaoGetWindow != null)
			{
				if (multi)
				{
					achievement = RouletteUtility.AchievementType.Multi;
				}
				ChaoGetPartsBase chaoGetParts;
				if (itemState != null && itemState.Count > 0)
				{
					PlayerGetPartsOverlap playerGetPartsOverlap = chaoGetWindow.gameObject.GetComponent<PlayerGetPartsOverlap>();
					if (playerGetPartsOverlap == null)
					{
						playerGetPartsOverlap = chaoGetWindow.gameObject.AddComponent<PlayerGetPartsOverlap>();
					}
					playerGetPartsOverlap.Init(id, rarity, level, itemState, PlayerGetPartsOverlap.IntroType.NORMAL);
					chaoGetParts = playerGetPartsOverlap;
				}
				else
				{
					if (RouletteUtility.isTutorial && RouletteTop.Instance != null && RouletteTop.Instance.category == RouletteCategory.PREMIUM)
					{
						TutorialCursor.StartTutorialCursor(TutorialCursor.Type.ROULETTE_OK);
					}
					PlayerGetPartsOverlap playerGetPartsOverlap2 = chaoGetWindow.gameObject.GetComponent<PlayerGetPartsOverlap>();
					if (playerGetPartsOverlap2 == null)
					{
						playerGetPartsOverlap2 = chaoGetWindow.gameObject.AddComponent<PlayerGetPartsOverlap>();
					}
					playerGetPartsOverlap2.Init(id, rarity, level, null, PlayerGetPartsOverlap.IntroType.NORMAL);
					chaoGetParts = playerGetPartsOverlap2;
				}
				if (multi)
				{
					chaoGetWindow.isSetuped = false;
				}
				chaoGetWindow.PlayStart(chaoGetParts, RouletteUtility.isTutorial, false, achievement);
			}
		}
	}

	private static void ShowGetWindowOtomo(ServerChaoData data, GameObject uiRoot, bool multi)
	{
		ChaoGetPartsBase chaoGetParts = null;
		int rarity = data.Rarity;
		ChaoGetWindow chaoGetWindow;
		if (rarity == 0 || rarity == 1)
		{
			chaoGetWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoGetWindow>(uiRoot, "chao_get_Window");
			if (chaoGetWindow != null)
			{
				ChaoGetPartsNormal chaoGetPartsNormal = chaoGetWindow.GetComponent<ChaoGetPartsNormal>();
				if (chaoGetPartsNormal == null)
				{
					chaoGetPartsNormal = chaoGetWindow.gameObject.AddComponent<ChaoGetPartsNormal>();
				}
				chaoGetPartsNormal.Init(data.Id, rarity);
				chaoGetParts = chaoGetPartsNormal;
			}
		}
		else
		{
			chaoGetWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoGetWindow>(uiRoot, "chao_rare_get_Window");
			if (chaoGetWindow != null)
			{
				ChaoGetPartsRare chaoGetPartsRare = chaoGetWindow.gameObject.GetComponent<ChaoGetPartsRare>();
				if (chaoGetPartsRare == null)
				{
					chaoGetPartsRare = chaoGetWindow.gameObject.AddComponent<ChaoGetPartsRare>();
				}
				chaoGetPartsRare.Init(data.Id, rarity);
				chaoGetParts = chaoGetPartsRare;
			}
		}
		if (chaoGetWindow != null)
		{
			if (multi)
			{
				chaoGetWindow.isSetuped = false;
				chaoGetWindow.PlayStart(chaoGetParts, RouletteUtility.isTutorial, false, RouletteUtility.AchievementType.Multi);
			}
			else
			{
				chaoGetWindow.PlayStart(chaoGetParts, RouletteUtility.isTutorial, false, RouletteUtility.AchievementType.ChaoGet);
			}
		}
	}

	private static void ShowGetWindowOtomoLvup(ServerChaoData data, GameObject uiRoot, bool multi)
	{
		ChaoMergeWindow chaoMergeWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoMergeWindow>(uiRoot, "chao_merge_Window");
		if (chaoMergeWindow != null)
		{
			if (multi)
			{
				chaoMergeWindow.isSetuped = false;
				chaoMergeWindow.PlayStart(data.Id, data.Level, data.Rarity, RouletteUtility.AchievementType.Multi);
			}
			else
			{
				chaoMergeWindow.PlayStart(data.Id, data.Level, data.Rarity, RouletteUtility.AchievementType.LevelUp);
			}
		}
	}

	private static void ShowGetWindowOtomoMax(ServerChaoData data, GameObject uiRoot, int numRequiredSpEggs)
	{
		int rarity = data.Rarity;
		SpEggGetWindow spEggGetWindow;
		SpEggGetPartsBase spEggGetParts;
		if (rarity == 0)
		{
			spEggGetWindow = GameObjectUtil.FindChildGameObjectComponent<SpEggGetWindow>(uiRoot, "chao_egg_transform_Window");
			spEggGetParts = new SpEggGetPartsNormal(data.Id, numRequiredSpEggs);
		}
		else
		{
			spEggGetWindow = GameObjectUtil.FindChildGameObjectComponent<SpEggGetWindow>(uiRoot, "chao_rare_egg_transform_Window");
			spEggGetParts = new SpEggGetPartsRare(data.Id, rarity, numRequiredSpEggs);
		}
		if (spEggGetWindow != null)
		{
			spEggGetWindow.PlayStart(spEggGetParts, RouletteUtility.AchievementType.LevelMax);
		}
	}

	public static void ShowLoginBounsInfoWindow(string param = "")
	{
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "today_roulette_caption").text;
		string message;
		if (string.IsNullOrEmpty(param))
		{
			message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", "today_roulette_text").text;
		}
		else
		{
			message = param;
		}
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "LoginBouns",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = text,
			message = message
		});
	}

	private static string GetText(string cellName, Dictionary<string, string> dicReplaces = null)
	{
		string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemRoulette", cellName).text;
		if (dicReplaces != null)
		{
			text = TextUtility.Replaces(text, dicReplaces);
		}
		return text;
	}

	private static string GetText(string cellName, string srcText, string dstText)
	{
		return RouletteUtility.GetText(cellName, new Dictionary<string, string>
		{
			{
				srcText,
				dstText
			}
		});
	}

	public static string GetPrizeList(ServerPrizeState prizeState)
	{
		string text = string.Empty;
		int num = -1;
		Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();
		List<int> list = new List<int>();
		foreach (ServerPrizeData current in prizeState.prizeList)
		{
			if (current.priority >= 0)
			{
				if (!list.Contains(current.priority))
				{
					list.Add(current.priority);
				}
				if (dictionary.ContainsKey(current.priority))
				{
					dictionary[current.priority].Add(current.GetItemName());
				}
				else
				{
					List<string> list2 = new List<string>();
					list2.Add(current.GetItemName());
					dictionary.Add(current.priority, list2);
				}
			}
		}
		list.Sort();
		for (int i = 0; i < list.Count; i++)
		{
			int num2 = list[i];
			List<string> list3 = new List<string>();
			int num3 = 0;
			foreach (string current2 in dictionary[num2])
			{
				if (!list3.Contains(current2))
				{
					if (num != num2)
					{
						num = num2;
						if (!string.IsNullOrEmpty(text))
						{
							if (num3 != 0)
							{
								text += Environment.NewLine;
							}
							text += Environment.NewLine;
						}
						num3 = 0;
						string cellName = "ui_Lbl_rarity_" + num2.ToString();
						text += "[00ff00]";
						text += TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Roulette", cellName).text;
						text += "[-]";
						text += Environment.NewLine;
					}
					else if (num3 > 0)
					{
						text += ", ";
					}
					text += current2;
					list3.Add(current2);
					num3++;
					if (num3 >= 3)
					{
						text += Environment.NewLine;
						num3 = 0;
					}
				}
			}
		}
		return text;
	}

	public static List<Constants.Campaign.emType> GetCampaign(RouletteCategory category)
	{
		List<Constants.Campaign.emType> list = null;
		if (RouletteUtility.isTutorial && category == RouletteCategory.PREMIUM)
		{
			return null;
		}
		ServerCampaignState campaignState = ServerInterface.CampaignState;
		if (campaignState != null)
		{
			if (category != RouletteCategory.PREMIUM)
			{
				if (category == RouletteCategory.ITEM)
				{
					if (campaignState.InSession(Constants.Campaign.emType.FreeWheelSpinCount))
					{
						if (list == null)
						{
							list = new List<Constants.Campaign.emType>();
						}
						list.Add(Constants.Campaign.emType.FreeWheelSpinCount);
					}
					if (campaignState.InSession(Constants.Campaign.emType.JackPotValueBonus))
					{
						if (list == null)
						{
							list = new List<Constants.Campaign.emType>();
						}
						list.Add(Constants.Campaign.emType.JackPotValueBonus);
					}
				}
			}
			else
			{
				if (campaignState.InSession(Constants.Campaign.emType.PremiumRouletteOdds))
				{
					if (list == null)
					{
						list = new List<Constants.Campaign.emType>();
					}
					list.Add(Constants.Campaign.emType.PremiumRouletteOdds);
				}
				if (campaignState.InSession(Constants.Campaign.emType.ChaoRouletteCost))
				{
					if (list == null)
					{
						list = new List<Constants.Campaign.emType>();
					}
					list.Add(Constants.Campaign.emType.ChaoRouletteCost);
				}
			}
		}
		return list;
	}

	public static string GetChaoGroupName(int chaoId)
	{
		ServerItem serverItem = new ServerItem((ServerItem.Id)chaoId);
		if (serverItem.idType == ServerItem.IdType.CHARA)
		{
			return "CharaName";
		}
		return "Chao";
	}

	public static string GetChaoCellName(int chaoId)
	{
		string result = string.Empty;
		ServerItem serverItem = new ServerItem((ServerItem.Id)chaoId);
		if (serverItem.idType == ServerItem.IdType.CHARA)
		{
			ServerItem serverItem2 = new ServerItem((ServerItem.Id)chaoId);
			int charaType = (int)serverItem2.charaType;
			result = CharaName.Name[charaType];
		}
		else
		{
			int num = chaoId - 400000;
			result = string.Format("name{0:D4}", num);
		}
		return result;
	}

	public static ServerChaoState.ChaoStatus GetChaoStatus(int chaoId)
	{
		ServerChaoState.ChaoStatus result = ServerChaoState.ChaoStatus.NotOwned;
		ServerPlayerState playerState = ServerInterface.PlayerState;
		if (playerState == null)
		{
			return result;
		}
		ServerChaoState serverChaoState = playerState.ChaoStateByItemID(chaoId);
		if (serverChaoState == null)
		{
			return result;
		}
		return serverChaoState.Status;
	}

	public static bool IsLevelMaxChao(int chaoId)
	{
		ServerChaoState.ChaoStatus chaoStatus = RouletteUtility.GetChaoStatus(chaoId);
		return chaoStatus == ServerChaoState.ChaoStatus.MaxLevel;
	}
}
