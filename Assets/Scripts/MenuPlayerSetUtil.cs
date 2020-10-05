using SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayerSetUtil
{
	public static float FadeInTime = 1f;

	public static float FadeOutTime = 1f;

	private static CharaType charaType = CharaType.UNKNOWN;

	private static readonly int CandidateRange = 5;

	public static readonly AbilityType[] AbilityLevelUpOrder = new AbilityType[]
	{
		AbilityType.INVINCIBLE,
		AbilityType.MAGNET,
		AbilityType.TRAMPOLINE,
		AbilityType.COMBO,
		AbilityType.LASER,
		AbilityType.DRILL,
		AbilityType.ASTEROID,
		AbilityType.DISTANCE_BONUS,
		AbilityType.RING_BONUS,
		AbilityType.ANIMAL
	};

	public static CharaType MarkCharaType
	{
		get
		{
			return MenuPlayerSetUtil.charaType;
		}
	}

	public static GameObject GetUIRoot()
	{
		return GameObject.Find("UI Root (2D)");
	}

	public static GameObject GetPlayerSetRoot()
	{
		GameObject uIRoot = MenuPlayerSetUtil.GetUIRoot();
		return GameObjectUtil.FindChildGameObject(uIRoot, "PlayerSet_2_UI");
	}

	public static int GetOpenedCharaCount()
	{
		int num = 0;
		for (int i = 0; i < 29; i++)
		{
			if (MenuPlayerSetUtil.IsOpenedCharacter((CharaType)i))
			{
				num++;
			}
		}
		return num;
	}

	public static bool IsOpenedCharacter(CharaType charaType)
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			ServerPlayerState playerState = ServerInterface.PlayerState;
			if (playerState != null)
			{
				ServerCharacterState serverCharacterState = playerState.CharacterState(charaType);
				if (serverCharacterState != null && serverCharacterState.Id > 0)
				{
					return true;
				}
			}
		}
		else if (charaType <= CharaType.KNUCKLES)
		{
			return true;
		}
		return false;
	}

	public static bool IsCharacterLevelMax(CharaType charaType)
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			ServerPlayerState playerState = ServerInterface.PlayerState;
			if (playerState != null)
			{
				ServerCharacterState serverCharacterState = playerState.CharacterState(charaType);
				if (serverCharacterState != null && serverCharacterState.Status == ServerCharacterState.CharacterStatus.MaxLevel)
				{
					return true;
				}
			}
		}
		else
		{
			int totalLevel = MenuPlayerSetUtil.GetTotalLevel(charaType);
			if (totalLevel >= 100)
			{
				return true;
			}
		}
		return false;
	}

	public static CharaType GetCharaTypeFromPageIndex(int pageIndex)
	{
		CharaType result = CharaType.UNKNOWN;
		int num = 0;
		for (int i = 0; i < 29; i++)
		{
			CharaType charaType = (CharaType)i;
			if (MenuPlayerSetUtil.IsOpenedCharacter(charaType))
			{
				if (num == pageIndex)
				{
					result = charaType;
					break;
				}
				num++;
			}
		}
		return result;
	}

	public static int GetPageIndexFromCharaType(CharaType charaType)
	{
		int num = 0;
		for (int i = 0; i < (int)charaType; i++)
		{
			if (MenuPlayerSetUtil.IsOpenedCharacter((CharaType)i))
			{
				num++;
			}
		}
		return num;
	}

	public static void ActivateCharaPageObjects(GameObject parentGameObject, bool isActive)
	{
		if (parentGameObject == null)
		{
			return;
		}
		int childCount = parentGameObject.transform.childCount;
		bool flag = false;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = parentGameObject.transform.GetChild(i).gameObject;
			if (!(gameObject == null))
			{
				if (gameObject.name == "_guide")
				{
					flag = true;
				}
				for (int j = 1; j < 3; j++)
				{
					if (gameObject.name == "eff_set" + j)
					{
						gameObject.SetActive(false);
					}
				}
			}
		}
		if (!flag)
		{
			return;
		}
		string[] array = new string[]
		{
			"Btn_lv_up",
			"Btn_player_main",
			"slot",
			"deck_tab"
		};
		for (int k = 0; k < childCount; k++)
		{
			GameObject gameObject2 = parentGameObject.transform.GetChild(k).gameObject;
			if (!(gameObject2 == null))
			{
				if (gameObject2.name == "_guide")
				{
					gameObject2.SetActive(true);
				}
				else
				{
					bool flag2 = false;
					string[] array2 = array;
					for (int l = 0; l < array2.Length; l++)
					{
						string text = array2[l];
						if (!string.IsNullOrEmpty(text))
						{
							if (gameObject2.name == text)
							{
								flag2 = true;
							}
						}
					}
					if (flag2)
					{
						global::Debug.Log("GetChild.Name" + k.ToString() + " = " + gameObject2.name);
						if (isActive)
						{
							if (gameObject2.activeInHierarchy)
							{
								goto IL_1D6;
							}
						}
						else if (!gameObject2.activeInHierarchy)
						{
							goto IL_1D6;
						}
						gameObject2.SetActive(isActive);
					}
				}
			}
			IL_1D6:;
		}
	}

	public static AbilityType GetNextLevelUpAbility(CharaType charaType)
	{
		int num = 10000;
		for (int i = 0; i < 10; i++)
		{
			AbilityType abilityType = MenuPlayerSetUtil.AbilityLevelUpOrder[i];
			int level = MenuPlayerSetUtil.GetLevel(charaType, abilityType);
			if (num > level)
			{
				num = level;
			}
		}
		ImportAbilityTable instance = ImportAbilityTable.GetInstance();
		int num2 = 0;
		List<AbilityType> list = new List<AbilityType>();
		for (int j = 0; j < 10; j++)
		{
			bool flag = true;
			AbilityType abilityType2 = MenuPlayerSetUtil.AbilityLevelUpOrder[j];
			int level2 = MenuPlayerSetUtil.GetLevel(charaType, abilityType2);
			if (level2 - num >= MenuPlayerSetUtil.CandidateRange)
			{
				flag = false;
			}
			int maxLevel = instance.GetMaxLevel(abilityType2);
			if (level2 >= maxLevel)
			{
				flag = false;
			}
			if (flag)
			{
				list.Add(abilityType2);
				num2++;
			}
		}
		if (num2 <= 0)
		{
			return AbilityType.LASER;
		}
		int index = UnityEngine.Random.Range(0, num2);
		return list[index];
	}

	public static int GetLevel(CharaType charaType, AbilityType abilityType)
	{
		CharaData charaData = SaveDataManager.Instance.CharaData;
		CharaAbility charaAbility = charaData.AbilityArray[(int)charaType];
		int value = (int)charaAbility.Ability[(int)abilityType];
		ImportAbilityTable instance = ImportAbilityTable.GetInstance();
		int maxLevel = instance.GetMaxLevel(abilityType);
		return Mathf.Clamp(value, 0, maxLevel);
	}

	public static float GetLevelAbility(CharaType charaType, AbilityType abilityType, int level)
	{
		ImportAbilityTable instance = ImportAbilityTable.GetInstance();
		return instance.GetAbilityPotential(abilityType, level);
	}

	public static int GetTotalLevel(CharaType charaType)
	{
		int num = 0;
		for (int i = 0; i < 10; i++)
		{
			AbilityType abilityType = (AbilityType)i;
			num += MenuPlayerSetUtil.GetLevel(charaType, abilityType);
		}
		return num;
	}

	public static int GetAbilityCost(CharaType charaType)
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			ServerItem serverItem = new ServerItem(charaType);
			int id = (int)serverItem.id;
			ServerCampaignData campaignInSession = ServerInterface.CampaignState.GetCampaignInSession(Constants.Campaign.emType.CharacterUpgradeCost, id);
			if (campaignInSession != null)
			{
				return campaignInSession.iContent;
			}
			ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(charaType);
			if (serverCharacterState != null)
			{
				return serverCharacterState.Cost;
			}
		}
		return 1000;
	}

	public static int GetCurrentExp(CharaType charaType)
	{
		int result = 0;
		ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(charaType);
		if (serverCharacterState != null)
		{
			result = serverCharacterState.Exp;
		}
		return result;
	}

	public static float GetCurrentExpRatio(CharaType charaType)
	{
		int abilityCost = MenuPlayerSetUtil.GetAbilityCost(charaType);
		int currentExp = MenuPlayerSetUtil.GetCurrentExp(charaType);
		if (abilityCost == 0)
		{
			return 1f;
		}
		return (float)currentExp / (float)abilityCost;
	}

	public static int TransformServerAbilityID(AbilityType abilityType)
	{
		ServerItem serverItem = new ServerItem(abilityType);
		return (int)serverItem.id;
	}

	public static int GetPlayableCharaCount()
	{
		int num = 0;
		for (int i = 0; i < 29; i++)
		{
			CharaData charaData = SaveDataManager.Instance.CharaData;
			if (charaData.Status[i] == 1)
			{
				num++;
			}
		}
		return num;
	}

	public static void SetMarkCharaPage(CharaType type)
	{
		MenuPlayerSetUtil.charaType = type;
	}

	public static void ResetMarkCharaPage()
	{
		MenuPlayerSetUtil.charaType = CharaType.UNKNOWN;
	}

	public static bool IsMarkCharaPage()
	{
		return MenuPlayerSetUtil.charaType != CharaType.UNKNOWN;
	}
}
