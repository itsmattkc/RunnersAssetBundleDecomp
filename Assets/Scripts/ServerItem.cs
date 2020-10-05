using System;
using System.Collections.Generic;
using Text;

public struct ServerItem
{
	public enum Id
	{
		NONE = -1,
		BOOST_SCORE = 110000,
		BOOST_TRAMPOLINE,
		BOOST_SUBCHARA,
		INVINCIBLE = 120000,
		BARRIER,
		MAGNET,
		TRAMPOLINE,
		COMBO,
		LASER,
		DRILL,
		ASTEROID,
		RING_BONUS,
		DISTANCE_BONUS,
		ANIMAL_BONUS,
		PACKED_INVINCIBLE_0 = 120100,
		PACKED_BARRIER_0,
		PACKED_MAGNET_0,
		PACKED_TRAMPOLINE_0,
		PACKED_COMBO_0,
		PACKED_LASER_0,
		PACKED_DRILL_0,
		PACKED_ASTEROID_0,
		PACKED_RING_BONUS_0,
		PACKED_SCORE_BONUS_0,
		PACKED_ANIMAL_BONUS_0,
		PACKED_INVINCIBLE_1 = 121000,
		PACKED_BARRIER_1,
		PACKED_MAGNET_1,
		PACKED_TRAMPOLINE_1,
		PACKED_COMBO_1,
		PACKED_LASER_1,
		PACKED_DRILL_1,
		PACKED_ASTEROID_1,
		PACKED_RING_BONUS_1,
		PACKED_SCORE_BONUS_1,
		PACKED_ANIMAL_BONUS_1,
		BIG = 200000,
		SUPER,
		JACKPOT,
		ROULLETE_TOKEN = 210000,
		SPECIAL_EGG = 220000,
		ROULLETE_TICKET_BEGIN = 229999,
		ROULLETE_TICKET_PREMIAM,
		ROULLETE_TICKET_ITEM = 240000,
		ROULLETE_TICKET_RAID = 250000,
		ROULLETE_TICKET_EVENT = 260000,
		ROULLETE_TICKET_END = 299999,
		CHARA_BEGIN,
		CHAO_BEGIN = 400000,
		CHAO_BEGIN_RARE = 401000,
		CHAO_BEGIN_SRARE = 402000,
		RSRING = 900000,
		RSRING_0 = 900010,
		RSRING_1 = 900030,
		RSRING_2 = 900060,
		RSRING_3 = 900210,
		RSRING_4 = 900380,
		RING = 910000,
		RING_0 = 910021,
		RING_1 = 910045,
		RING_2 = 910094,
		RING_3 = 910147,
		RING_4 = 910204,
		RING_5 = 910265,
		ENERGY = 920000,
		ENERGY_0,
		ENERGY_1 = 920005,
		ENERGY_2 = 920010,
		ENERGY_3 = 920015,
		ENERGY_4 = 920020,
		ENERGY_5 = 920025,
		ENERGY_MAX = 930000,
		SUB_CHARA = 940000,
		CONTINUE = 950000,
		RAIDRING = 960000,
		DAILY_BATTLE_RESET_0 = 980000,
		DAILY_BATTLE_RESET_1,
		DAILY_BATTLE_RESET_2
	}

	public enum IdType
	{
		NONE = -1,
		BOOST_ITEM = 11,
		EQUIP_ITEM,
		ITEM_ROULLETE_WIN = 20,
		ROULLETE_TOKEN,
		EGG_ITEM,
		PREMIUM_ROULLETE_TICKET,
		ITEM_ROULLETE_TICKET,
		CHARA = 30,
		CHAO = 40,
		RSRING = 90,
		RING,
		ENERGY,
		ENERGY_MAX,
		RAIDRING = 96
	}

	private const int SERVER_ID_INDEX_DIVISOR = 10000;

	private const int SERVER_ID_EQUIP_ITEM_INDEX_DIVISOR = 100;

	private static Dictionary<ServerItem.IdType, string> IdTypeAtlasName = new Dictionary<ServerItem.IdType, string>
	{
		{
			ServerItem.IdType.NONE,
			string.Empty
		},
		{
			ServerItem.IdType.BOOST_ITEM,
			"ui_item_set_3_Atlas"
		},
		{
			ServerItem.IdType.EQUIP_ITEM,
			"ui_player_set_icon_Atlas"
		},
		{
			ServerItem.IdType.ITEM_ROULLETE_WIN,
			string.Empty
		},
		{
			ServerItem.IdType.ROULLETE_TOKEN,
			"ui_cmn_item_Atlas"
		},
		{
			ServerItem.IdType.EGG_ITEM,
			"ui_mainmenu_Atlas"
		},
		{
			ServerItem.IdType.CHARA,
			"ui_cmn_player_bundle_Atlas"
		},
		{
			ServerItem.IdType.CHAO,
			"ui_cmn_chao_Atlas"
		},
		{
			ServerItem.IdType.RSRING,
			"ui_cmn_item_Atlas"
		},
		{
			ServerItem.IdType.RING,
			"ui_cmn_item_Atlas"
		},
		{
			ServerItem.IdType.ENERGY,
			"ui_cmn_item_Atlas"
		},
		{
			ServerItem.IdType.ENERGY_MAX,
			"ui_cmn_item_Atlas"
		}
	};

	private static Dictionary<AbilityType, ServerItem.Id> AbilityToServerId = new Dictionary<AbilityType, ServerItem.Id>
	{
		{
			AbilityType.LASER,
			ServerItem.Id.LASER
		},
		{
			AbilityType.DRILL,
			ServerItem.Id.DRILL
		},
		{
			AbilityType.ASTEROID,
			ServerItem.Id.ASTEROID
		},
		{
			AbilityType.RING_BONUS,
			ServerItem.Id.RING_BONUS
		},
		{
			AbilityType.DISTANCE_BONUS,
			ServerItem.Id.DISTANCE_BONUS
		},
		{
			AbilityType.TRAMPOLINE,
			ServerItem.Id.TRAMPOLINE
		},
		{
			AbilityType.ANIMAL,
			ServerItem.Id.ANIMAL_BONUS
		},
		{
			AbilityType.COMBO,
			ServerItem.Id.COMBO
		},
		{
			AbilityType.MAGNET,
			ServerItem.Id.MAGNET
		},
		{
			AbilityType.INVINCIBLE,
			ServerItem.Id.INVINCIBLE
		}
	};

	private static Dictionary<ServerItem.Id, AbilityType> ServerIdToAbility = new Dictionary<ServerItem.Id, AbilityType>
	{
		{
			ServerItem.Id.LASER,
			AbilityType.LASER
		},
		{
			ServerItem.Id.DRILL,
			AbilityType.DRILL
		},
		{
			ServerItem.Id.ASTEROID,
			AbilityType.ASTEROID
		},
		{
			ServerItem.Id.RING_BONUS,
			AbilityType.RING_BONUS
		},
		{
			ServerItem.Id.DISTANCE_BONUS,
			AbilityType.DISTANCE_BONUS
		},
		{
			ServerItem.Id.TRAMPOLINE,
			AbilityType.TRAMPOLINE
		},
		{
			ServerItem.Id.ANIMAL_BONUS,
			AbilityType.ANIMAL
		},
		{
			ServerItem.Id.COMBO,
			AbilityType.COMBO
		},
		{
			ServerItem.Id.MAGNET,
			AbilityType.MAGNET
		},
		{
			ServerItem.Id.INVINCIBLE,
			AbilityType.INVINCIBLE
		}
	};

	private static int[] s_chaoIdTable;

	private static Dictionary<ServerItem.IdType, ServerItem[]> s_dicServerItemTable = new Dictionary<ServerItem.IdType, ServerItem[]>();

	private ServerItem.Id m_id;

	private static Dictionary<ServerItem.Id, RewardType> s_dic_ServerItemId_to_RewardType = new Dictionary<ServerItem.Id, RewardType>
	{
		{
			ServerItem.Id.INVINCIBLE,
			RewardType.ITEM_INVINCIBLE
		},
		{
			ServerItem.Id.BARRIER,
			RewardType.ITEM_BARRIER
		},
		{
			ServerItem.Id.MAGNET,
			RewardType.ITEM_MAGNET
		},
		{
			ServerItem.Id.TRAMPOLINE,
			RewardType.ITEM_TRAMPOLINE
		},
		{
			ServerItem.Id.COMBO,
			RewardType.ITEM_COMBO
		},
		{
			ServerItem.Id.LASER,
			RewardType.ITEM_LASER
		},
		{
			ServerItem.Id.DRILL,
			RewardType.ITEM_DRILL
		},
		{
			ServerItem.Id.ASTEROID,
			RewardType.ITEM_ASTEROID
		},
		{
			ServerItem.Id.RING,
			RewardType.RING
		},
		{
			ServerItem.Id.RSRING,
			RewardType.RSRING
		},
		{
			ServerItem.Id.ENERGY,
			RewardType.ENERGY
		}
	};

	public ServerItem.Id id
	{
		get
		{
			return this.m_id;
		}
	}

	public ServerItem.IdType idType
	{
		get
		{
			return (ServerItem.IdType)(this.m_id / (ServerItem.Id)10000);
		}
	}

	public int idIndex
	{
		get
		{
			return (int)(this.m_id % ((this.idType != ServerItem.IdType.EQUIP_ITEM) ? ((ServerItem.Id)10000) : ((ServerItem.Id)100)));
		}
	}

	public bool isPacked
	{
		get
		{
			return this.packedNumber != 0;
		}
	}

	public bool isValid
	{
		get
		{
			return this.m_id != ServerItem.Id.NONE;
		}
	}

	private int packedNumber
	{
		get
		{
			return (int)((this.idType != ServerItem.IdType.EQUIP_ITEM) ? ((ServerItem.Id)0) : (this.m_id % (ServerItem.Id)10000 / (ServerItem.Id)100));
		}
	}

	public int serverItemNum
	{
		get
		{
			return this.packedNumber;
		}
	}

	public string serverItemName
	{
		get
		{
			string result = null;
			int num = (int)(this.m_id % (ServerItem.Id)1000);
			ServerItem.IdType idType = this.idType;
			switch (idType)
			{
			case ServerItem.IdType.ROULLETE_TOKEN:
				return result;
			case ServerItem.IdType.EGG_ITEM:
			{
				string cellID = "sp_egg_name";
				result = TextUtility.GetCommonText("ChaoRoulette", cellID);
				return result;
			}
			case ServerItem.IdType.PREMIUM_ROULLETE_TICKET:
			{
				string cellID = "premium_roulette_ticket";
				result = TextUtility.GetCommonText("Item", cellID);
				return result;
			}
			case ServerItem.IdType.ITEM_ROULLETE_TICKET:
			{
				string cellID = "item_roulette_ticket";
				result = TextUtility.GetCommonText("Item", cellID);
				return result;
			}
			case (ServerItem.IdType)25:
			case (ServerItem.IdType)26:
			case (ServerItem.IdType)27:
			case (ServerItem.IdType)28:
			case (ServerItem.IdType)29:
				IL_49:
				switch (idType)
				{
				case ServerItem.IdType.RSRING:
				{
					string cellID = "red_star_ring";
					result = TextUtility.GetCommonText("Item", cellID);
					return result;
				}
				case ServerItem.IdType.RING:
				{
					string cellID = "ring";
					result = TextUtility.GetCommonText("Item", cellID);
					return result;
				}
				case ServerItem.IdType.ENERGY:
				{
					string cellID = "energy";
					result = TextUtility.GetCommonText("Item", cellID);
					return result;
				}
				case ServerItem.IdType.ENERGY_MAX:
				{
					string cellID = "energy";
					result = TextUtility.GetCommonText("Item", cellID);
					return result;
				}
				case (ServerItem.IdType)94:
				case (ServerItem.IdType)95:
				{
					IL_6F:
					if (idType == ServerItem.IdType.BOOST_ITEM)
					{
						return result;
					}
					if (idType == ServerItem.IdType.EQUIP_ITEM)
					{
						string cellID = string.Format("name{0}", num % 100 + 1);
						result = TextUtility.GetCommonText("ShopItem", cellID);
						return result;
					}
					if (idType != ServerItem.IdType.CHAO)
					{
						return result;
					}
					string cellID2 = "name" + this.chaoId.ToString("D4");
					result = TextUtility.GetChaoText("Chao", cellID2);
					return result;
				}
				case ServerItem.IdType.RAIDRING:
				{
					string cellID = "raidboss_ring";
					result = TextUtility.GetCommonText("Item", cellID);
					return result;
				}
				}
				goto IL_6F;
			case ServerItem.IdType.CHARA:
			{
				CharacterDataNameInfo.Info dataByServerID = CharacterDataNameInfo.Instance.GetDataByServerID((int)this.m_id);
				if (dataByServerID != null)
				{
					string cellID = dataByServerID.m_name.ToLower();
					result = TextUtility.GetCommonText("CharaName", cellID);
				}
				return result;
			}
			}
			goto IL_49;
		}
	}

	public string serverItemComment
	{
		get
		{
			string result = null;
			int num = (int)(this.m_id % (ServerItem.Id)1000);
			ServerItem.IdType idType = this.idType;
			switch (idType)
			{
			case ServerItem.IdType.RSRING:
			{
				string cellID = "red_star_ring_details";
				result = TextUtility.GetCommonText("Item", cellID);
				break;
			}
			case ServerItem.IdType.RING:
			{
				string cellID = "ring_details";
				result = TextUtility.GetCommonText("Item", cellID);
				break;
			}
			case ServerItem.IdType.ENERGY:
			{
				string cellID = "energy_details";
				result = TextUtility.GetCommonText("Item", cellID);
				break;
			}
			case ServerItem.IdType.ENERGY_MAX:
			{
				string cellID = "energy_details";
				result = TextUtility.GetCommonText("Item", cellID);
				break;
			}
			default:
				if (idType != ServerItem.IdType.BOOST_ITEM)
				{
					if (idType != ServerItem.IdType.EQUIP_ITEM)
					{
						if (idType != ServerItem.IdType.ROULLETE_TOKEN)
						{
							if (idType != ServerItem.IdType.EGG_ITEM)
							{
								if (idType != ServerItem.IdType.CHARA)
								{
									if (idType != ServerItem.IdType.CHAO)
									{
									}
								}
								else
								{
									CharacterDataNameInfo.Info dataByServerID = CharacterDataNameInfo.Instance.GetDataByServerID((int)this.m_id);
									if (dataByServerID != null)
									{
										string cellID = string.Format("chara_attribute_{0}", dataByServerID.m_name.ToLower());
										result = TextUtility.GetCommonText("WindowText", cellID);
									}
								}
							}
							else
							{
								string cellID = "sp_egg_details";
								result = TextUtility.GetCommonText("ChaoRoulette", cellID);
							}
						}
						else if (num == 0)
						{
							result = "BIG";
						}
						else if (num == 1)
						{
							result = "SUPER";
						}
						else
						{
							result = "ジャックポット";
						}
					}
					else
					{
						string cellID = string.Format("details{0}", num % 100 + 1);
						result = TextUtility.GetCommonText("ShopItem", cellID);
					}
				}
				else
				{
					result = "[BOOST_ITEM]";
					if (num % 3 == 0)
					{
						result = "スコアボーナス100%";
					}
					else if (num % 3 == 1)
					{
						result = "アシストトランポリン";
					}
					else if (num % 3 == 2)
					{
						result = "サブキャラクター";
					}
				}
				break;
			}
			return result;
		}
	}

	public string serverItemSpriteName
	{
		get
		{
			string result = null;
			int num = (int)(this.m_id % (ServerItem.Id)1000);
			ServerItem.IdType idType = this.idType;
			switch (idType)
			{
			case ServerItem.IdType.RSRING:
				result = string.Format("ui_cmn_icon_item_{0}", 9);
				break;
			case ServerItem.IdType.RING:
				result = string.Format("ui_cmn_icon_item_{0}", 8);
				break;
			case ServerItem.IdType.ENERGY:
				result = string.Format("ui_cmn_icon_item_{0}", 92000);
				break;
			case ServerItem.IdType.ENERGY_MAX:
				result = string.Format("ui_cmn_icon_item_{0}", 92000);
				break;
			default:
				if (idType != ServerItem.IdType.BOOST_ITEM)
				{
					if (idType != ServerItem.IdType.EQUIP_ITEM)
					{
						if (idType != ServerItem.IdType.ROULLETE_TOKEN)
						{
							if (idType != ServerItem.IdType.EGG_ITEM)
							{
								if (idType != ServerItem.IdType.CHARA)
								{
									if (idType != ServerItem.IdType.CHAO)
									{
									}
								}
								else if (num % 100 < CharacterDataNameInfo.PrefixNameList.Length)
								{
									string arg = CharacterDataNameInfo.PrefixNameList[num % 100];
									result = string.Format("ui_tex_player_{0:00}_{1}", num % 100, arg);
								}
							}
							else
							{
								result = "ui_cmn_icon_item_220000";
							}
						}
						else
						{
							result = string.Format("ui_cmn_icon_item_{0}", num % 100);
						}
					}
					else
					{
						result = string.Format("ui_mm_player_icon_{0}", num % 100);
					}
				}
				else
				{
					result = string.Format("ui_itemset_2_boost_icon_{0}", num);
				}
				break;
			}
			return result;
		}
	}

	public string serverItemSpriteNameRoulette
	{
		get
		{
			string result = null;
			int num = (int)(this.m_id % (ServerItem.Id)1000);
			ServerItem.IdType idType = this.idType;
			switch (idType)
			{
			case ServerItem.IdType.RSRING:
				result = string.Format("ui_cmn_icon_item_{0}", 9);
				break;
			case ServerItem.IdType.RING:
				result = string.Format("ui_cmn_icon_item_{0}", 8);
				break;
			case ServerItem.IdType.ENERGY:
				result = string.Format("ui_cmn_icon_item_{0}", 92000);
				break;
			case ServerItem.IdType.ENERGY_MAX:
				result = string.Format("ui_cmn_icon_item_{0}", 92000);
				break;
			default:
				if (idType != ServerItem.IdType.EQUIP_ITEM)
				{
					if (idType != ServerItem.IdType.EGG_ITEM)
					{
						if (idType != ServerItem.IdType.CHARA)
						{
							if (idType != ServerItem.IdType.CHAO)
							{
							}
						}
						else if (num % 100 < CharacterDataNameInfo.PrefixNameList.Length)
						{
							string arg = CharacterDataNameInfo.PrefixNameList[num % 100];
							result = string.Format("ui_tex_player_{0:00}_{1}", num % 100, arg);
						}
					}
					else
					{
						result = "ui_cmn_icon_item_220000";
					}
				}
				else
				{
					result = string.Format("ui_cmn_icon_item_{0}", num % 100);
				}
				break;
			}
			return result;
		}
	}

	public CharaType charaType
	{
		get
		{
			if (this.idType == ServerItem.IdType.CHARA && CharacterDataNameInfo.Instance != null)
			{
				CharacterDataNameInfo.Info dataByServerID = CharacterDataNameInfo.Instance.GetDataByServerID((int)this.m_id);
				if (dataByServerID != null)
				{
					return dataByServerID.m_ID;
				}
			}
			return CharaType.UNKNOWN;
		}
	}

	public ItemType itemType
	{
		get
		{
			return (ItemType)((this.idType != ServerItem.IdType.EQUIP_ITEM || this.idIndex >= 8) ? (-1) : this.idIndex);
		}
	}

	public BoostItemType boostItemType
	{
		get
		{
			return (BoostItemType)((this.idType != ServerItem.IdType.BOOST_ITEM || this.idIndex >= 3) ? (-1) : this.idIndex);
		}
	}

	public AbilityType abilityType
	{
		get
		{
			if (ServerItem.ServerIdToAbility.ContainsKey(this.m_id))
			{
				return ServerItem.ServerIdToAbility[this.m_id];
			}
			return AbilityType.NONE;
		}
	}

	public int chaoId
	{
		get
		{
			return (this.idType != ServerItem.IdType.CHAO) ? (-1) : this.idIndex;
		}
	}

	public RewardType rewardType
	{
		get
		{
			RewardType id;
			if (!ServerItem.s_dic_ServerItemId_to_RewardType.TryGetValue(this.id, out id))
			{
				id = (RewardType)this.id;
			}
			return id;
		}
	}

	public ServerItem(ServerItem.Id id)
	{
		this.m_id = id;
	}

	public ServerItem(CharaType characterType)
	{
		this.m_id = ServerItem.Id.NONE;
		if (characterType != CharaType.UNKNOWN && CharacterDataNameInfo.Instance != null)
		{
			CharacterDataNameInfo.Info dataByID = CharacterDataNameInfo.Instance.GetDataByID(characterType);
			if (dataByID != null)
			{
				this.m_id = (ServerItem.Id)dataByID.m_serverID;
			}
		}
	}

	public ServerItem(ItemType itemType)
	{
		this.m_id = (ServerItem.Id)((itemType == ItemType.UNKNOWN) ? ItemType.UNKNOWN : (itemType + 120000));
	}

	public ServerItem(BoostItemType boostItemType)
	{
		this.m_id = (ServerItem.Id)((boostItemType >= BoostItemType.NUM) ? BoostItemType.UNKNOWN : (boostItemType + 110000));
	}

	public ServerItem(AbilityType abilityType)
	{
		if (ServerItem.AbilityToServerId.ContainsKey(abilityType))
		{
			this.m_id = ServerItem.AbilityToServerId[abilityType];
		}
		else
		{
			this.m_id = ServerItem.Id.NONE;
		}
	}

	public ServerItem(RewardType rewardType)
	{
		this.m_id = ServerItem.Id.NONE;
		foreach (ServerItem.Id current in ServerItem.s_dic_ServerItemId_to_RewardType.Keys)
		{
			if (ServerItem.s_dic_ServerItemId_to_RewardType[current] == rewardType)
			{
				this.m_id = current;
				break;
			}
		}
		if (this.m_id == ServerItem.Id.NONE)
		{
			this.m_id = (ServerItem.Id)rewardType;
		}
	}

	public static string GetIdTypeAtlasName(ServerItem.IdType idType)
	{
		string result = null;
		if (idType != ServerItem.IdType.NONE)
		{
			result = ServerItem.IdTypeAtlasName[idType];
		}
		return result;
	}

	public static ServerItem.Id ConvertAbilityId(AbilityType abilityType)
	{
		ServerItem.Id result = ServerItem.Id.NONE;
		if (ServerItem.AbilityToServerId.ContainsKey(abilityType))
		{
			result = ServerItem.AbilityToServerId[abilityType];
		}
		return result;
	}

	public static ServerItem CreateFromChaoId(int chaoId)
	{
		return new ServerItem((chaoId == -1) ? ServerItem.Id.NONE : (chaoId + ServerItem.Id.CHAO_BEGIN));
	}

	public static ServerItem[] GetServerItemTable(ServerItem.IdType idType)
	{
		if (!ServerItem.s_dicServerItemTable.ContainsKey(idType))
		{
			List<ServerItem> list = new List<ServerItem>();
			foreach (object current in Enum.GetValues(typeof(ServerItem.Id)))
			{
				ServerItem.Id id = (ServerItem.Id)((int)current);
				ServerItem serverItem = new ServerItem(id);
				if (serverItem.idType == idType)
				{
					list.Add(new ServerItem(id));
				}
			}
			ServerItem.s_dicServerItemTable[idType] = list.ToArray();
		}
		return ServerItem.s_dicServerItemTable[idType];
	}

	public static int GetServerItemCount(ServerItem.IdType idType)
	{
		return ServerItem.GetServerItemTable(idType).Length;
	}
}
