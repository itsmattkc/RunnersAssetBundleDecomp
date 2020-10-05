using DataTable;
using System;
using System.Collections.Generic;

public class BonusParam
{
	public enum BonusTarget
	{
		CHARA,
		CHAO_MAIN,
		CHAO_SUB,
		ALL
	}

	public enum BonusType
	{
		SCORE,
		RING,
		ANIMAL,
		DISTANCE,
		ENEMY_OBJBREAK,
		TOTAL_SCORE,
		SPEED,
		NONE
	}

	public enum BonusAffinity
	{
		NONE,
		GOOD
	}

	private Dictionary<BonusParam.BonusTarget, List<float>> m_bonusData;

	private Dictionary<BonusParam.BonusTarget, CharacterAttribute> m_attribute;

	public Dictionary<BonusParam.BonusTarget, List<float>> orgBonusData
	{
		get
		{
			return this.m_bonusData;
		}
	}

	public BonusParam()
	{
		this.Reset();
	}

	public static Dictionary<BonusParam.BonusTarget, List<float>> GetBonusDataTotal(Dictionary<BonusParam.BonusTarget, List<float>> orgDataA, Dictionary<BonusParam.BonusTarget, List<float>> orgDataB)
	{
		Dictionary<BonusParam.BonusTarget, List<float>> dictionary = null;
		if (orgDataA != null && orgDataA.Count > 0 && orgDataB != null && orgDataB.Count > 0)
		{
			dictionary = new Dictionary<BonusParam.BonusTarget, List<float>>();
			Dictionary<BonusParam.BonusTarget, List<float>>.KeyCollection keys = orgDataA.Keys;
			foreach (BonusParam.BonusTarget current in keys)
			{
				List<float> list = new List<float>();
				if (orgDataA[current] != null && orgDataB[current] != null)
				{
					for (int i = 0; i < orgDataA[current].Count; i++)
					{
						if (i == 5)
						{
							float totalScoreBonus = BonusUtil.GetTotalScoreBonus(orgDataA[current][i], orgDataB[current][i]);
							list.Add(totalScoreBonus);
						}
						else
						{
							list.Add(orgDataA[current][i] + orgDataB[current][i]);
						}
					}
				}
				dictionary.Add(current, list);
			}
		}
		else if (orgDataA != null && orgDataA.Count > 0)
		{
			dictionary = new Dictionary<BonusParam.BonusTarget, List<float>>();
			Dictionary<BonusParam.BonusTarget, List<float>>.KeyCollection keys2 = orgDataA.Keys;
			foreach (BonusParam.BonusTarget current2 in keys2)
			{
				List<float> list2 = new List<float>();
				if (orgDataA[current2] != null)
				{
					for (int j = 0; j < orgDataA[current2].Count; j++)
					{
						list2.Add(orgDataA[current2][j]);
					}
				}
				dictionary.Add(current2, list2);
			}
		}
		else if (orgDataB != null && orgDataB.Count > 0)
		{
			dictionary = new Dictionary<BonusParam.BonusTarget, List<float>>();
			Dictionary<BonusParam.BonusTarget, List<float>>.KeyCollection keys3 = orgDataB.Keys;
			foreach (BonusParam.BonusTarget current3 in keys3)
			{
				List<float> list3 = new List<float>();
				if (orgDataB[current3] != null)
				{
					for (int k = 0; k < orgDataB[current3].Count; k++)
					{
						list3.Add(orgDataB[current3][k]);
					}
				}
				dictionary.Add(current3, list3);
			}
		}
		return dictionary;
	}

	public void Reset()
	{
		if (this.m_bonusData != null)
		{
			Dictionary<BonusParam.BonusTarget, List<float>>.KeyCollection keys = this.m_bonusData.Keys;
			foreach (BonusParam.BonusTarget current in keys)
			{
				if (this.m_bonusData[current] != null)
				{
					this.m_bonusData[current].Clear();
				}
			}
			this.m_bonusData.Clear();
		}
		if (this.m_attribute != null)
		{
			this.m_attribute.Clear();
		}
		this.m_bonusData = new Dictionary<BonusParam.BonusTarget, List<float>>();
		this.m_attribute = new Dictionary<BonusParam.BonusTarget, CharacterAttribute>();
	}

	private void SetBonusChao(ChaoData chaoData, BonusParam.BonusTarget target, CharacterAttribute charaAtribute)
	{
		if (chaoData != null && chaoData.chaoAbilitys != null && chaoData.chaoAbilitys.Length > 0)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float item = 0f;
			float item2 = 0f;
			for (int i = 0; i < chaoData.chaoAbilitys.Length; i++)
			{
				ChaoAbility chaoAbility = chaoData.chaoAbilitys[i];
				float num6;
				if (charaAtribute == chaoData.charaAtribute)
				{
					num6 = chaoData.bonusAbilityValue[chaoData.level];
				}
				else
				{
					num6 = chaoData.abilityValue[chaoData.level];
				}
				ChaoAbility chaoAbility2 = chaoAbility;
				switch (chaoAbility2)
				{
				case ChaoAbility.ALL_BONUS_COUNT:
					num += num6;
					num2 += num6;
					num3 += num6;
					num4 += num6;
					goto IL_113;
				case ChaoAbility.SCORE_COUNT:
					num += num6;
					goto IL_113;
				case ChaoAbility.RING_COUNT:
					num2 += num6;
					goto IL_113;
				case ChaoAbility.RED_RING_COUNT:
					IL_B8:
					if (chaoAbility2 != ChaoAbility.ENEMY_SCORE)
					{
						goto IL_113;
					}
					num5 += num6;
					goto IL_113;
				case ChaoAbility.ANIMAL_COUNT:
					num3 += num6;
					goto IL_113;
				case ChaoAbility.RUNNIGN_DISTANCE:
					num4 += num6;
					goto IL_113;
				}
				goto IL_B8;
				IL_113:;
			}
			List<float> list = new List<float>();
			list.Add(num);
			list.Add(num2);
			list.Add(num3);
			list.Add(num4);
			list.Add(num5);
			list.Add(item);
			list.Add(item2);
			if (!this.m_bonusData.ContainsKey(target))
			{
				this.m_bonusData.Add(target, list);
				this.m_attribute.Add(target, chaoData.charaAtribute);
			}
			else
			{
				this.m_bonusData[target] = list;
				if (this.m_attribute.ContainsKey(target))
				{
					this.m_attribute[target] = chaoData.charaAtribute;
				}
			}
		}
	}

	public void AddBonusChao(ChaoData chaoDataMain, ChaoData chaoDataSub = null)
	{
		if (this.m_attribute != null && this.m_attribute.ContainsKey(BonusParam.BonusTarget.CHARA))
		{
			CharacterAttribute charaAtribute = this.m_attribute[BonusParam.BonusTarget.CHARA];
			this.SetBonusChao(chaoDataMain, BonusParam.BonusTarget.CHAO_MAIN, charaAtribute);
			this.SetBonusChao(chaoDataSub, BonusParam.BonusTarget.CHAO_SUB, charaAtribute);
		}
	}

	public void AddBonusChara(ServerCharacterState charaMainState, CharaType mainType, ServerCharacterState charaSubState, CharaType subType)
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			Dictionary<BonusParam.BonusType, float> dictionary;
			if (BonusUtil.GetTeamBonus(mainType, out dictionary) && dictionary != null)
			{
				Dictionary<BonusParam.BonusType, float>.KeyCollection keys = dictionary.Keys;
				foreach (BonusParam.BonusType current in keys)
				{
					switch (current)
					{
					case BonusParam.BonusType.SCORE:
						num += dictionary[current];
						break;
					case BonusParam.BonusType.RING:
						num2 += dictionary[current];
						break;
					case BonusParam.BonusType.ANIMAL:
						num3 += dictionary[current];
						break;
					case BonusParam.BonusType.DISTANCE:
						num4 += dictionary[current];
						break;
					case BonusParam.BonusType.ENEMY_OBJBREAK:
						num5 += dictionary[current];
						break;
					case BonusParam.BonusType.TOTAL_SCORE:
						num6 += dictionary[current];
						break;
					case BonusParam.BonusType.SPEED:
						num7 += dictionary[current];
						break;
					default:
						UnityEngine.Debug.Log(" not bonus team !");
						break;
					}
				}
			}
			if (subType != CharaType.UNKNOWN && subType != CharaType.NUM && BonusUtil.GetTeamBonus(subType, out dictionary) && dictionary != null)
			{
				Dictionary<BonusParam.BonusType, float>.KeyCollection keys2 = dictionary.Keys;
				foreach (BonusParam.BonusType current2 in keys2)
				{
					switch (current2)
					{
					case BonusParam.BonusType.SCORE:
						num += dictionary[current2];
						break;
					case BonusParam.BonusType.RING:
						num2 += dictionary[current2];
						break;
					case BonusParam.BonusType.ANIMAL:
						num3 += dictionary[current2];
						break;
					case BonusParam.BonusType.DISTANCE:
						num4 += dictionary[current2];
						break;
					case BonusParam.BonusType.ENEMY_OBJBREAK:
						num5 += dictionary[current2];
						break;
					case BonusParam.BonusType.TOTAL_SCORE:
						num6 = BonusUtil.GetTotalScoreBonus(num6, dictionary[current2]);
						break;
					case BonusParam.BonusType.SPEED:
						num7 += dictionary[current2];
						break;
					default:
						UnityEngine.Debug.Log(" not bonus team !");
						break;
					}
				}
			}
			ImportAbilityTable instance2 = ImportAbilityTable.GetInstance();
			if (instance2 != null)
			{
				num2 += instance2.GetAbilityPotential(AbilityType.RING_BONUS, charaMainState.AbilityLevel[8]);
				num3 += instance2.GetAbilityPotential(AbilityType.ANIMAL, charaMainState.AbilityLevel[10]);
				num4 += instance2.GetAbilityPotential(AbilityType.DISTANCE_BONUS, charaMainState.AbilityLevel[9]);
				Dictionary<BonusParam.BonusType, float> starBonusList = charaMainState.GetStarBonusList();
				if (starBonusList != null)
				{
					Dictionary<BonusParam.BonusType, float>.KeyCollection keys3 = starBonusList.Keys;
					foreach (BonusParam.BonusType current3 in keys3)
					{
						switch (current3)
						{
						case BonusParam.BonusType.SCORE:
							num += starBonusList[current3];
							break;
						case BonusParam.BonusType.RING:
							num2 += starBonusList[current3];
							break;
						case BonusParam.BonusType.ANIMAL:
							num3 += starBonusList[current3];
							break;
						case BonusParam.BonusType.DISTANCE:
							num4 += starBonusList[current3];
							break;
						case BonusParam.BonusType.ENEMY_OBJBREAK:
							num5 += starBonusList[current3];
							break;
						case BonusParam.BonusType.TOTAL_SCORE:
							num6 += starBonusList[current3];
							break;
						case BonusParam.BonusType.SPEED:
							num7 += starBonusList[current3];
							break;
						}
					}
				}
				if (charaSubState != null)
				{
					num2 += instance2.GetAbilityPotential(AbilityType.RING_BONUS, charaSubState.AbilityLevel[8]);
					num3 += instance2.GetAbilityPotential(AbilityType.ANIMAL, charaSubState.AbilityLevel[10]);
					num4 += instance2.GetAbilityPotential(AbilityType.DISTANCE_BONUS, charaSubState.AbilityLevel[9]);
					Dictionary<BonusParam.BonusType, float> starBonusList2 = charaSubState.GetStarBonusList();
					if (starBonusList2 != null)
					{
						Dictionary<BonusParam.BonusType, float>.KeyCollection keys4 = starBonusList2.Keys;
						foreach (BonusParam.BonusType current4 in keys4)
						{
							switch (current4)
							{
							case BonusParam.BonusType.SCORE:
								num += starBonusList2[current4];
								break;
							case BonusParam.BonusType.RING:
								num2 += starBonusList2[current4];
								break;
							case BonusParam.BonusType.ANIMAL:
								num3 += starBonusList2[current4];
								break;
							case BonusParam.BonusType.DISTANCE:
								num4 += starBonusList2[current4];
								break;
							case BonusParam.BonusType.ENEMY_OBJBREAK:
								num5 += starBonusList2[current4];
								break;
							case BonusParam.BonusType.TOTAL_SCORE:
								num6 += starBonusList2[current4];
								break;
							case BonusParam.BonusType.SPEED:
								num7 += starBonusList2[current4];
								break;
							}
						}
					}
				}
			}
			List<float> list = new List<float>();
			list.Add(num);
			list.Add(num2);
			list.Add(num3);
			list.Add(num4);
			list.Add(num5);
			list.Add(num6);
			list.Add(num7);
			if (!this.m_bonusData.ContainsKey(BonusParam.BonusTarget.CHARA))
			{
				this.m_bonusData.Add(BonusParam.BonusTarget.CHARA, list);
				this.m_attribute.Add(BonusParam.BonusTarget.CHARA, CharaTypeUtil.GetCharacterAttribute(mainType));
			}
			else
			{
				this.m_bonusData[BonusParam.BonusTarget.CHARA] = list;
				if (this.m_attribute.ContainsKey(BonusParam.BonusTarget.CHARA))
				{
					this.m_attribute[BonusParam.BonusTarget.CHARA] = CharaTypeUtil.GetCharacterAttribute(mainType);
				}
			}
		}
	}

	public BonusParam.BonusAffinity GetBonusAffinity(BonusParam.BonusTarget target)
	{
		BonusParam.BonusAffinity result = BonusParam.BonusAffinity.NONE;
		if (this.m_attribute.ContainsKey(BonusParam.BonusTarget.CHARA) && target != BonusParam.BonusTarget.ALL && target != BonusParam.BonusTarget.CHARA)
		{
			CharacterAttribute characterAttribute = this.m_attribute[BonusParam.BonusTarget.CHARA];
			if (this.m_attribute.ContainsKey(target) && this.m_attribute[target] == characterAttribute)
			{
				result = BonusParam.BonusAffinity.GOOD;
			}
		}
		return result;
	}

	public bool IsDetailInfo(out string detailText)
	{
		detailText = BonusParam.GetDetailInfoText(this.m_bonusData);
		return !string.IsNullOrEmpty(detailText);
	}

	public static string GetDetailInfoText(Dictionary<BonusParam.BonusTarget, List<float>> orgBonusData)
	{
		if (orgBonusData == null || orgBonusData.Count == 0)
		{
			return string.Empty;
		}
		string text = string.Empty;
		Dictionary<BonusParam.BonusTarget, List<float>>.KeyCollection keys = orgBonusData.Keys;
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (BonusParam.BonusTarget current in keys)
		{
			if (current == BonusParam.BonusTarget.CHARA)
			{
				List<float> list3 = orgBonusData[current];
				for (int i = 0; i < list3.Count; i++)
				{
					BonusParam.BonusType bonusType = (BonusParam.BonusType)i;
					float orgValue = list3[i];
					BonusParam.BonusType bonusType2 = bonusType;
					if (bonusType2 != BonusParam.BonusType.TOTAL_SCORE)
					{
						if (bonusType2 == BonusParam.BonusType.SPEED)
						{
							string bonusParamText = BonusUtil.GetBonusParamText(bonusType, orgValue);
							if (!string.IsNullOrEmpty(bonusParamText))
							{
								if (BonusUtil.IsBonusMeritByOrgValue(bonusType, orgValue))
								{
									list.Add(BonusUtil.GetBonusParamText(bonusType, orgValue));
								}
								else
								{
									list2.Add(BonusUtil.GetBonusParamText(bonusType, orgValue));
								}
							}
						}
					}
					else if (!BonusUtil.IsBonusMeritByOrgValue(bonusType, orgValue))
					{
						string bonusParamText = BonusUtil.GetBonusParamText(bonusType, orgValue);
						if (!string.IsNullOrEmpty(bonusParamText))
						{
							list2.Add(BonusUtil.GetBonusParamText(bonusType, orgValue));
						}
					}
				}
			}
		}
		if (list.Count > 0)
		{
			foreach (string current2 in list)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				text += current2;
			}
		}
		if (list2.Count > 0)
		{
			foreach (string current3 in list2)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				text += current3;
			}
		}
		return text;
	}

	public Dictionary<BonusParam.BonusType, float> GetBonusInfo(BonusParam.BonusTarget target = BonusParam.BonusTarget.ALL, bool offsetUse = true)
	{
		Dictionary<BonusParam.BonusType, float> dictionary = new Dictionary<BonusParam.BonusType, float>();
		if (target == BonusParam.BonusTarget.ALL)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			Dictionary<BonusParam.BonusTarget, List<float>>.KeyCollection keys = this.m_bonusData.Keys;
			foreach (BonusParam.BonusTarget current in keys)
			{
				if (this.m_bonusData[current] != null && this.m_bonusData[current].Count > 0)
				{
					num += this.m_bonusData[current][0];
					num2 += this.m_bonusData[current][1];
					num3 += this.m_bonusData[current][2];
					num4 += this.m_bonusData[current][3];
					num5 += this.m_bonusData[current][4];
					num7 += this.m_bonusData[current][6];
					if (num6 == 0f)
					{
						num6 = this.m_bonusData[current][5];
					}
					else if (this.m_bonusData[current][5] != 0f)
					{
						num6 = BonusUtil.GetTotalScoreBonus(num6, this.m_bonusData[current][5]);
					}
				}
			}
			if (offsetUse)
			{
				dictionary.Add(BonusParam.BonusType.SCORE, BonusUtil.GetBonusParamValue(BonusParam.BonusType.SCORE, num));
				dictionary.Add(BonusParam.BonusType.RING, BonusUtil.GetBonusParamValue(BonusParam.BonusType.RING, num2));
				dictionary.Add(BonusParam.BonusType.ANIMAL, BonusUtil.GetBonusParamValue(BonusParam.BonusType.ANIMAL, num3));
				dictionary.Add(BonusParam.BonusType.DISTANCE, BonusUtil.GetBonusParamValue(BonusParam.BonusType.DISTANCE, num4));
				dictionary.Add(BonusParam.BonusType.ENEMY_OBJBREAK, BonusUtil.GetBonusParamValue(BonusParam.BonusType.ENEMY_OBJBREAK, num5));
				dictionary.Add(BonusParam.BonusType.SPEED, BonusUtil.GetBonusParamValue(BonusParam.BonusType.SPEED, num7));
				dictionary.Add(BonusParam.BonusType.TOTAL_SCORE, BonusUtil.GetBonusParamValue(BonusParam.BonusType.TOTAL_SCORE, num6));
			}
			else
			{
				dictionary.Add(BonusParam.BonusType.SCORE, num);
				dictionary.Add(BonusParam.BonusType.RING, num2);
				dictionary.Add(BonusParam.BonusType.ANIMAL, num3);
				dictionary.Add(BonusParam.BonusType.DISTANCE, num4);
				dictionary.Add(BonusParam.BonusType.ENEMY_OBJBREAK, num5);
				dictionary.Add(BonusParam.BonusType.SPEED, num7);
				dictionary.Add(BonusParam.BonusType.TOTAL_SCORE, num6);
			}
		}
		else if (this.m_bonusData != null && this.m_bonusData.ContainsKey(target))
		{
			if (offsetUse)
			{
				dictionary.Add(BonusParam.BonusType.SCORE, BonusUtil.GetBonusParamValue(BonusParam.BonusType.SCORE, this.m_bonusData[target][0]));
				dictionary.Add(BonusParam.BonusType.RING, BonusUtil.GetBonusParamValue(BonusParam.BonusType.RING, this.m_bonusData[target][1]));
				dictionary.Add(BonusParam.BonusType.ANIMAL, BonusUtil.GetBonusParamValue(BonusParam.BonusType.SCORE, this.m_bonusData[target][2]));
				dictionary.Add(BonusParam.BonusType.DISTANCE, BonusUtil.GetBonusParamValue(BonusParam.BonusType.SCORE, this.m_bonusData[target][3]));
				dictionary.Add(BonusParam.BonusType.ENEMY_OBJBREAK, BonusUtil.GetBonusParamValue(BonusParam.BonusType.SCORE, this.m_bonusData[target][4]));
				dictionary.Add(BonusParam.BonusType.SPEED, BonusUtil.GetBonusParamValue(BonusParam.BonusType.SCORE, this.m_bonusData[target][6]));
				dictionary.Add(BonusParam.BonusType.TOTAL_SCORE, BonusUtil.GetBonusParamValue(BonusParam.BonusType.SCORE, this.m_bonusData[target][5]));
			}
			else
			{
				dictionary.Add(BonusParam.BonusType.SCORE, this.m_bonusData[target][0]);
				dictionary.Add(BonusParam.BonusType.RING, this.m_bonusData[target][1]);
				dictionary.Add(BonusParam.BonusType.ANIMAL, this.m_bonusData[target][2]);
				dictionary.Add(BonusParam.BonusType.DISTANCE, this.m_bonusData[target][3]);
				dictionary.Add(BonusParam.BonusType.ENEMY_OBJBREAK, this.m_bonusData[target][4]);
				dictionary.Add(BonusParam.BonusType.SPEED, this.m_bonusData[target][6]);
				dictionary.Add(BonusParam.BonusType.TOTAL_SCORE, this.m_bonusData[target][5]);
			}
		}
		else if (offsetUse)
		{
			dictionary.Add(BonusParam.BonusType.SCORE, BonusUtil.GetBonusParamValue(BonusParam.BonusType.SCORE, 0f));
			dictionary.Add(BonusParam.BonusType.RING, BonusUtil.GetBonusParamValue(BonusParam.BonusType.RING, 0f));
			dictionary.Add(BonusParam.BonusType.ANIMAL, BonusUtil.GetBonusParamValue(BonusParam.BonusType.ANIMAL, 0f));
			dictionary.Add(BonusParam.BonusType.DISTANCE, BonusUtil.GetBonusParamValue(BonusParam.BonusType.DISTANCE, 0f));
			dictionary.Add(BonusParam.BonusType.ENEMY_OBJBREAK, BonusUtil.GetBonusParamValue(BonusParam.BonusType.ENEMY_OBJBREAK, 0f));
			dictionary.Add(BonusParam.BonusType.SPEED, BonusUtil.GetBonusParamValue(BonusParam.BonusType.SPEED, 0f));
			dictionary.Add(BonusParam.BonusType.TOTAL_SCORE, BonusUtil.GetBonusParamValue(BonusParam.BonusType.TOTAL_SCORE, 0f));
		}
		else
		{
			dictionary.Add(BonusParam.BonusType.SCORE, 0f);
			dictionary.Add(BonusParam.BonusType.RING, 0f);
			dictionary.Add(BonusParam.BonusType.ANIMAL, 0f);
			dictionary.Add(BonusParam.BonusType.DISTANCE, 0f);
			dictionary.Add(BonusParam.BonusType.ENEMY_OBJBREAK, 0f);
			dictionary.Add(BonusParam.BonusType.SPEED, 0f);
			dictionary.Add(BonusParam.BonusType.TOTAL_SCORE, 0f);
		}
		return dictionary;
	}

	public Dictionary<BonusParam.BonusType, float> GetBonusInfo(BonusParam.BonusTarget targetA, BonusParam.BonusTarget targetB, bool offsetUse = true)
	{
		Dictionary<BonusParam.BonusType, float> dictionary = new Dictionary<BonusParam.BonusType, float>();
		if (targetA != BonusParam.BonusTarget.ALL && targetB != BonusParam.BonusTarget.ALL && targetA != targetB)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			Dictionary<BonusParam.BonusTarget, List<float>>.KeyCollection keys = this.m_bonusData.Keys;
			foreach (BonusParam.BonusTarget current in keys)
			{
				if (this.m_bonusData[current] != null && this.m_bonusData[current].Count > 0 && (current == targetA || current == targetB))
				{
					num += this.m_bonusData[current][0];
					num2 += this.m_bonusData[current][1];
					num3 += this.m_bonusData[current][2];
					num4 += this.m_bonusData[current][3];
					num5 += this.m_bonusData[current][4];
					num7 += this.m_bonusData[current][6];
					if (num6 == 0f)
					{
						num6 = this.m_bonusData[current][5];
					}
					else if (this.m_bonusData[current][5] != 0f)
					{
						num6 = BonusUtil.GetTotalScoreBonus(num6, this.m_bonusData[current][5]);
					}
				}
			}
			if (offsetUse)
			{
				dictionary.Add(BonusParam.BonusType.SCORE, BonusUtil.GetBonusParamValue(BonusParam.BonusType.SCORE, num));
				dictionary.Add(BonusParam.BonusType.RING, BonusUtil.GetBonusParamValue(BonusParam.BonusType.RING, num2));
				dictionary.Add(BonusParam.BonusType.ANIMAL, BonusUtil.GetBonusParamValue(BonusParam.BonusType.ANIMAL, num3));
				dictionary.Add(BonusParam.BonusType.DISTANCE, BonusUtil.GetBonusParamValue(BonusParam.BonusType.DISTANCE, num4));
				dictionary.Add(BonusParam.BonusType.ENEMY_OBJBREAK, BonusUtil.GetBonusParamValue(BonusParam.BonusType.ENEMY_OBJBREAK, num5));
				dictionary.Add(BonusParam.BonusType.SPEED, BonusUtil.GetBonusParamValue(BonusParam.BonusType.SPEED, num7));
				dictionary.Add(BonusParam.BonusType.TOTAL_SCORE, BonusUtil.GetBonusParamValue(BonusParam.BonusType.TOTAL_SCORE, num6));
			}
			else
			{
				dictionary.Add(BonusParam.BonusType.SCORE, num);
				dictionary.Add(BonusParam.BonusType.RING, num2);
				dictionary.Add(BonusParam.BonusType.ANIMAL, num3);
				dictionary.Add(BonusParam.BonusType.DISTANCE, num4);
				dictionary.Add(BonusParam.BonusType.ENEMY_OBJBREAK, num5);
				dictionary.Add(BonusParam.BonusType.SPEED, num7);
				dictionary.Add(BonusParam.BonusType.TOTAL_SCORE, num6);
			}
		}
		return dictionary;
	}
}
