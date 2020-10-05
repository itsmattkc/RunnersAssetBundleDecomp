using DataTable;
using System;
using System.Collections.Generic;
using Text;

public class BonusUtil
{
	private const int BOUNES_OFFSET_POINT = 0;

	public const float TEAM_ATTRIBUTE_DEMERIT_VALUE_EASY = -0.2f;

	public const float TEAM_ATTRIBUTE_DEMERIT_DOUBLE_VALUE_EASY = -0.36f;

	public const float TEAM_ATTRIBUTE_DEMERIT_VALUE_EASY_RATIO = 0.8f;

	public const float TEAM_ATTRIBUTE_DEMERIT_VALUE_EASY_DOUBLE_RATIO = 0.64f;

	private const string ABILITY_ICON_SPRITE_NAME_BASE = "ui_chao_set_ability_icon_{PARAM}";

	private static List<CharaType> s_charaList;

	private static List<ChaoData> s_chaoList;

	public static float GetTotalScoreBonus(float currentBonusRate, float addBonusRate)
	{
		float result;
		if (currentBonusRate == 0f)
		{
			result = addBonusRate;
		}
		else if (currentBonusRate < 0f)
		{
			if (addBonusRate > 0f)
			{
				result = currentBonusRate + addBonusRate;
			}
			else
			{
				float num = 1f + addBonusRate;
				float num2 = 1f + currentBonusRate;
				result = -1f + num2 * num;
			}
		}
		else if (addBonusRate > 0f)
		{
			float num3 = 1f - addBonusRate;
			float num4 = 1f - currentBonusRate;
			result = 1f - num4 * num3;
		}
		else
		{
			result = currentBonusRate + addBonusRate;
		}
		return result;
	}

	public static string GetAbilityIconSpriteName(BonusParam.BonusType type, float value)
	{
		string result = string.Empty;
		switch (type)
		{
		case BonusParam.BonusType.SCORE:
			if (BonusUtil.IsBonusMerit(type, value))
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Uscore");
			}
			else
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Dscore");
			}
			break;
		case BonusParam.BonusType.RING:
			if (BonusUtil.IsBonusMerit(type, value))
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Uring");
			}
			else
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Dring");
			}
			break;
		case BonusParam.BonusType.ANIMAL:
			if (BonusUtil.IsBonusMerit(type, value))
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Uanimal");
			}
			else
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Danimal");
			}
			break;
		case BonusParam.BonusType.DISTANCE:
			if (BonusUtil.IsBonusMerit(type, value))
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Urange");
			}
			else
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Drange");
			}
			break;
		case BonusParam.BonusType.ENEMY_OBJBREAK:
			if (BonusUtil.IsBonusMerit(type, value))
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Uenemy");
			}
			else
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Denemy");
			}
			break;
		case BonusParam.BonusType.TOTAL_SCORE:
			if (BonusUtil.IsBonusMerit(type, value))
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Ufscore");
			}
			else
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Dfscore");
			}
			break;
		case BonusParam.BonusType.SPEED:
			if (value > 100f)
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Uspeed");
			}
			else
			{
				result = "ui_chao_set_ability_icon_{PARAM}".Replace("{PARAM}", "Dspeed");
			}
			break;
		}
		return result;
	}

	private static float GetTeamDemritBonus(TeamAttribute type)
	{
		float result = 0f;
		if (type == TeamAttribute.EASY)
		{
			result = -0.2f;
		}
		return result;
	}

	public static Dictionary<BonusParam.BonusType, bool> IsTeamBonus(CharaType charaType, List<BonusParam.BonusType> types)
	{
		Dictionary<BonusParam.BonusType, bool> dictionary = null;
		if (charaType != CharaType.UNKNOWN && charaType != CharaType.NUM && types != null)
		{
			dictionary = new Dictionary<BonusParam.BonusType, bool>();
			Dictionary<BonusParam.BonusType, float> dictionary2;
			if (BonusUtil.GetTeamBonus(charaType, out dictionary2))
			{
				for (int i = 0; i < types.Count; i++)
				{
					dictionary.Add(types[i], dictionary2.ContainsKey(types[i]));
				}
			}
		}
		return dictionary;
	}

	public static bool IsTeamBonus(CharaType charaType, BonusParam.BonusType type)
	{
		bool result = false;
		Dictionary<BonusParam.BonusType, float> dictionary;
		if (charaType != CharaType.UNKNOWN && charaType != CharaType.NUM && type != BonusParam.BonusType.NONE && BonusUtil.GetTeamBonus(charaType, out dictionary) && dictionary.ContainsKey(type))
		{
			result = true;
		}
		return result;
	}

	public static bool IsBonusMerit(BonusParam.BonusType type, float value)
	{
		bool result = false;
		switch (type)
		{
		case BonusParam.BonusType.SCORE:
		case BonusParam.BonusType.RING:
		case BonusParam.BonusType.ANIMAL:
		case BonusParam.BonusType.DISTANCE:
		case BonusParam.BonusType.ENEMY_OBJBREAK:
		case BonusParam.BonusType.TOTAL_SCORE:
			if (value >= 0f)
			{
				result = true;
			}
			break;
		case BonusParam.BonusType.SPEED:
			if (value < 100f)
			{
				result = true;
			}
			break;
		}
		return result;
	}

	public static bool IsBonusMeritByOrgValue(BonusParam.BonusType type, float orgValue)
	{
		bool result = false;
		switch (type)
		{
		case BonusParam.BonusType.SCORE:
		case BonusParam.BonusType.RING:
		case BonusParam.BonusType.ANIMAL:
		case BonusParam.BonusType.DISTANCE:
		case BonusParam.BonusType.ENEMY_OBJBREAK:
		case BonusParam.BonusType.TOTAL_SCORE:
			if (orgValue >= 0f)
			{
				result = true;
			}
			break;
		case BonusParam.BonusType.SPEED:
			if (orgValue > 0f)
			{
				result = true;
			}
			break;
		}
		return result;
	}

	public static float GetBonusParamValue(BonusParam.BonusType type, float orgValue, ref bool merit)
	{
		float result = 0f;
		switch (type)
		{
		case BonusParam.BonusType.SCORE:
		case BonusParam.BonusType.RING:
		case BonusParam.BonusType.ANIMAL:
		case BonusParam.BonusType.DISTANCE:
		case BonusParam.BonusType.ENEMY_OBJBREAK:
			result = orgValue;
			merit = (orgValue >= 0f);
			break;
		case BonusParam.BonusType.TOTAL_SCORE:
			result = orgValue * 100f;
			merit = (orgValue >= 0f);
			break;
		case BonusParam.BonusType.SPEED:
			result = 100f - orgValue;
			merit = (orgValue > 0f);
			break;
		}
		return result;
	}

	public static float GetBonusParamValue(BonusParam.BonusType type, float orgValue)
	{
		float result = 0f;
		switch (type)
		{
		case BonusParam.BonusType.SCORE:
		case BonusParam.BonusType.RING:
		case BonusParam.BonusType.ANIMAL:
		case BonusParam.BonusType.DISTANCE:
		case BonusParam.BonusType.ENEMY_OBJBREAK:
			result = orgValue;
			break;
		case BonusParam.BonusType.TOTAL_SCORE:
			result = orgValue * 100f;
			break;
		case BonusParam.BonusType.SPEED:
			result = 100f - orgValue;
			break;
		}
		return result;
	}

	public static string GetBonusParamText(BonusParam.BonusType type, float orgValue)
	{
		string result = string.Empty;
		bool flag = false;
		float bonusParamValue = BonusUtil.GetBonusParamValue(type, orgValue, ref flag);
		string text = string.Empty;
		switch (type)
		{
		case BonusParam.BonusType.SCORE:
		case BonusParam.BonusType.RING:
		case BonusParam.BonusType.ANIMAL:
		case BonusParam.BonusType.DISTANCE:
		case BonusParam.BonusType.ENEMY_OBJBREAK:
		case BonusParam.BonusType.TOTAL_SCORE:
			if (bonusParamValue != 0f)
			{
				if (flag)
				{
					text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "bonus", "info_type_up_" + type.ToString()).text;
				}
				else
				{
					text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "bonus", "info_type_down_" + type.ToString()).text;
				}
			}
			break;
		case BonusParam.BonusType.SPEED:
			if (bonusParamValue != 100f)
			{
				if (flag)
				{
					text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "bonus", "info_type_down_" + type.ToString()).text;
				}
				else
				{
					text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "bonus", "info_type_up_" + type.ToString()).text;
				}
			}
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			result = text.Replace("{PARAM}", bonusParamValue.ToString());
		}
		return result;
	}

	public static bool GetTeamBonus(CharaType type, out Dictionary<BonusParam.BonusType, float> bonusParam)
	{
		bonusParam = new Dictionary<BonusParam.BonusType, float>();
		if (type != CharaType.UNKNOWN && type != CharaType.NUM && CharacterDataNameInfo.Instance != null)
		{
			CharacterDataNameInfo.Info dataByID = CharacterDataNameInfo.Instance.GetDataByID(type);
			if (dataByID != null)
			{
				TeamAttribute teamAttribute = CharaTypeUtil.GetTeamAttribute(type);
				switch (dataByID.m_teamAttributeCategory)
				{
				case TeamAttributeCategory.DISTANCE:
				{
					float value = dataByID.GetTeamAttributeValue(TeamAttributeBonusType.DISTANCE);
					bonusParam.Add(BonusParam.BonusType.DISTANCE, value);
					break;
				}
				case TeamAttributeCategory.SCORE:
				{
					float value = dataByID.GetTeamAttributeValue(TeamAttributeBonusType.SCORE);
					bonusParam.Add(BonusParam.BonusType.SCORE, value);
					break;
				}
				case TeamAttributeCategory.RING:
				{
					float value = dataByID.GetTeamAttributeValue(TeamAttributeBonusType.RING);
					bonusParam.Add(BonusParam.BonusType.RING, value);
					break;
				}
				case TeamAttributeCategory.ANIMAL:
				{
					float value = dataByID.GetTeamAttributeValue(TeamAttributeBonusType.ANIMAL);
					bonusParam.Add(BonusParam.BonusType.ANIMAL, value);
					break;
				}
				case TeamAttributeCategory.ENEMY_OBJBREAK:
				{
					float value = dataByID.GetTeamAttributeValue(TeamAttributeBonusType.ENEMY);
					bonusParam.Add(BonusParam.BonusType.ENEMY_OBJBREAK, value);
					break;
				}
				case TeamAttributeCategory.EASY_SPEED:
				{
					float value = dataByID.GetTeamAttributeValue(TeamAttributeBonusType.SPEED);
					bonusParam.Add(BonusParam.BonusType.SPEED, value);
					if (BonusUtil.GetTeamDemritBonus(teamAttribute) != 0f)
					{
						value = BonusUtil.GetTeamDemritBonus(teamAttribute);
						bonusParam.Add(BonusParam.BonusType.TOTAL_SCORE, value);
					}
					break;
				}
				case TeamAttributeCategory.DISTANCE_ANIMAL:
				{
					float value = dataByID.GetTeamAttributeValue(TeamAttributeBonusType.DISTANCE);
					bonusParam.Add(BonusParam.BonusType.DISTANCE, value);
					value = dataByID.GetTeamAttributeValue(TeamAttributeBonusType.ANIMAL);
					bonusParam.Add(BonusParam.BonusType.ANIMAL, value);
					break;
				}
				case TeamAttributeCategory.LOW_SPEED_SCORE:
				{
					float value = dataByID.GetTeamAttributeValue(TeamAttributeBonusType.SCORE);
					bonusParam.Add(BonusParam.BonusType.SCORE, value);
					value = dataByID.GetTeamAttributeValue(TeamAttributeBonusType.SPEED);
					bonusParam.Add(BonusParam.BonusType.SPEED, value);
					break;
				}
				}
			}
		}
		return bonusParam.Count > 0;
	}

	private static void ResetUserBelongings()
	{
		if (BonusUtil.s_charaList != null)
		{
			BonusUtil.s_charaList.Clear();
			BonusUtil.s_charaList = null;
		}
		if (BonusUtil.s_chaoList != null)
		{
			BonusUtil.s_chaoList.Clear();
			BonusUtil.s_chaoList = null;
		}
	}

	private static void SetupUserBelongings()
	{
		ServerPlayerState playerState = ServerInterface.PlayerState;
		if (playerState != null)
		{
			int num = 29;
			if (BonusUtil.s_charaList != null)
			{
				BonusUtil.s_charaList.Clear();
				BonusUtil.s_charaList = null;
			}
			for (int i = 0; i < num; i++)
			{
				ServerCharacterState serverCharacterState = playerState.CharacterState((CharaType)i);
				if (serverCharacterState != null && serverCharacterState.IsUnlocked)
				{
					if (BonusUtil.s_charaList == null)
					{
						BonusUtil.s_charaList = new List<CharaType>();
						BonusUtil.s_charaList.Add((CharaType)i);
					}
					else
					{
						BonusUtil.s_charaList.Add((CharaType)i);
					}
				}
			}
		}
		if (BonusUtil.s_chaoList != null)
		{
			BonusUtil.s_chaoList.Clear();
			BonusUtil.s_chaoList = null;
		}
		BonusUtil.s_chaoList = ChaoTable.GetPossessionChaoData();
	}

	public static BonusParamContainer GetCurrentBonusData(CharaType charaMainType, CharaType charaSubType, int chaoMainId, int chaoSubId)
	{
		BonusParamContainer bonusParamContainer = null;
		if (charaMainType != CharaType.UNKNOWN && charaMainType != CharaType.NUM)
		{
			BonusUtil.SetupUserBelongings();
			bonusParamContainer = new BonusParamContainer();
			BonusParam currentBonusParam = BonusUtil.GetCurrentBonusParam(charaMainType, charaSubType, chaoMainId, chaoSubId);
			if (currentBonusParam != null)
			{
				bonusParamContainer.addBonus(currentBonusParam);
			}
			if (charaSubType != CharaType.UNKNOWN && charaSubType != CharaType.NUM)
			{
				currentBonusParam = BonusUtil.GetCurrentBonusParam(charaSubType, charaMainType, chaoMainId, chaoSubId);
				if (currentBonusParam != null)
				{
					bonusParamContainer.addBonus(currentBonusParam);
				}
			}
		}
		return bonusParamContainer;
	}

	private static BonusParam GetCurrentBonusParam(CharaType charaMainType, CharaType charaSubType, int chaoMainId, int chaoSubId)
	{
		BonusParam bonusParam = null;
		if (charaMainType != CharaType.UNKNOWN && charaMainType != CharaType.NUM)
		{
			if (BonusUtil.s_chaoList == null || BonusUtil.s_charaList == null)
			{
				BonusUtil.SetupUserBelongings();
			}
			if (BonusUtil.s_charaList != null && BonusUtil.s_charaList.Count > 0)
			{
				int num = -1;
				int num2 = -1;
				for (int i = 0; i < BonusUtil.s_charaList.Count; i++)
				{
					if (BonusUtil.s_charaList[i] == charaMainType)
					{
						num = i;
						if (charaSubType == CharaType.UNKNOWN || charaSubType == CharaType.NUM || num2 != -1)
						{
							break;
						}
					}
					if (BonusUtil.s_charaList[i] == charaSubType)
					{
						num2 = i;
						if (num != -1)
						{
							break;
						}
					}
				}
				if (num >= 0)
				{
					ServerPlayerState playerState = ServerInterface.PlayerState;
					if (playerState != null)
					{
						ServerCharacterState charaSubState = null;
						ServerCharacterState serverCharacterState = playerState.CharacterState(BonusUtil.s_charaList[num]);
						if (num2 >= 0)
						{
							charaSubState = playerState.CharacterState(BonusUtil.s_charaList[num2]);
						}
						if (serverCharacterState != null)
						{
							bonusParam = new BonusParam();
							bonusParam.AddBonusChara(serverCharacterState, charaMainType, charaSubState, charaSubType);
						}
					}
				}
				if (bonusParam != null)
				{
					ChaoData chaoData = null;
					ChaoData chaoData2 = null;
					if (BonusUtil.s_chaoList != null && BonusUtil.s_chaoList.Count > 0 && (chaoMainId >= 0 || chaoSubId >= 0))
					{
						foreach (ChaoData current in BonusUtil.s_chaoList)
						{
							if (current.id == chaoMainId)
							{
								chaoData = current;
								if (chaoData2 != null || chaoSubId < 0)
								{
									break;
								}
							}
							else if (current.id == chaoSubId)
							{
								chaoData2 = current;
								if (chaoData != null || chaoMainId < 0)
								{
									break;
								}
							}
						}
					}
					if (chaoData != null || chaoData2 != null)
					{
						if (chaoData2 == null)
						{
							bonusParam.AddBonusChao(chaoData, null);
						}
						else if (chaoData == null)
						{
							bonusParam.AddBonusChao(chaoData2, null);
						}
						else
						{
							bonusParam.AddBonusChao(chaoData, chaoData2);
						}
					}
				}
			}
		}
		return bonusParam;
	}
}
