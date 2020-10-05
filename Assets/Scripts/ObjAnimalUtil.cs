using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjAnimalUtil
{
	private class AnimalParam
	{
		public ResourceCategory m_category;

		public string m_model;

		public int m_count;

		public AnimalMoveType m_moveType;

		public ChaoAbility m_chaoAbility;

		public AnimalParam(ResourceCategory category, string model, int count, AnimalMoveType moveType, ChaoAbility chaoAbility)
		{
			this.m_category = category;
			this.m_model = model;
			this.m_count = count;
			this.m_moveType = moveType;
			this.m_chaoAbility = chaoAbility;
		}
	}

	private static readonly ObjAnimalUtil.AnimalParam[] ANIMAL_PARAM = new ObjAnimalUtil.AnimalParam[]
	{
		new ObjAnimalUtil.AnimalParam(ResourceCategory.ENEMY_RESOURCE, "ani_flicky", 6, AnimalMoveType.FLY, ChaoAbility.SPECIAL_ANIMAL),
		new ObjAnimalUtil.AnimalParam(ResourceCategory.ENEMY_RESOURCE, "ani_picky", 1, AnimalMoveType.JUMP, ChaoAbility.UNKNOWN),
		new ObjAnimalUtil.AnimalParam(ResourceCategory.ENEMY_RESOURCE, "ani_pecky", 1, AnimalMoveType.JUMP, ChaoAbility.UNKNOWN),
		new ObjAnimalUtil.AnimalParam(ResourceCategory.ENEMY_RESOURCE, "ani_rocky", 1, AnimalMoveType.JUMP, ChaoAbility.UNKNOWN),
		new ObjAnimalUtil.AnimalParam(ResourceCategory.ENEMY_RESOURCE, "ani_ricky", 1, AnimalMoveType.JUMP, ChaoAbility.UNKNOWN),
		new ObjAnimalUtil.AnimalParam(ResourceCategory.ENEMY_RESOURCE, "ani_cookie", 1, AnimalMoveType.JUMP, ChaoAbility.UNKNOWN),
		new ObjAnimalUtil.AnimalParam(ResourceCategory.ENEMY_RESOURCE, "ani_pocky", 1, AnimalMoveType.JUMP, ChaoAbility.UNKNOWN),
		new ObjAnimalUtil.AnimalParam(ResourceCategory.CHAO_MODEL, "ani_rappy", 6, AnimalMoveType.FLY, ChaoAbility.SPECIAL_ANIMAL_PSO2)
	};

	private static readonly AnimalType[] NORMALTYPE_TABLE = new AnimalType[]
	{
		AnimalType.PICKY,
		AnimalType.PECKY,
		AnimalType.ROCKY,
		AnimalType.RICKY,
		AnimalType.COOKIE,
		AnimalType.POCKY
	};

	private static readonly string[] MOVECOMP_NAMES = new string[]
	{
		"ObjAnimalFly",
		"ObjAnimalJump"
	};

	private static Vector3 ModelLocalRotation = new Vector3(0f, 180f, 0f);

	private static string GetMoveCompName(AnimalMoveType moveType)
	{
		if ((ulong)moveType < (ulong)((long)ObjAnimalUtil.MOVECOMP_NAMES.Length))
		{
			return ObjAnimalUtil.MOVECOMP_NAMES[(int)moveType];
		}
		return string.Empty;
	}

	private static int GetChaoAbilityAnimalCount(ChaoAbility ability, int defaultCount)
	{
		int num = 0;
		if (StageAbilityManager.Instance != null)
		{
			num = (int)StageAbilityManager.Instance.GetChaoAbilityExtraValue(ability, ChaoType.MAIN);
			if (num == 0)
			{
				num = (int)StageAbilityManager.Instance.GetChaoAbilityExtraValue(ability, ChaoType.SUB);
			}
		}
		if (num == 0)
		{
			num = defaultCount;
		}
		return num;
	}

	public static void CreateAnimal(GameObject enm_obj, AnimalType type)
	{
		if ((ulong)type >= (ulong)((long)ObjAnimalUtil.ANIMAL_PARAM.Length))
		{
			return;
		}
		if (enm_obj)
		{
			string moveCompName = ObjAnimalUtil.GetMoveCompName(ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_moveType);
			string model = ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_model;
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.ENEMY_PREFAB, moveCompName);
			GameObject gameObject2 = ResourceManager.Instance.GetGameObject(ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_category, model);
			if (gameObject != null && gameObject2 != null)
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject, enm_obj.transform.position, Quaternion.identity) as GameObject;
				GameObject gameObject4 = UnityEngine.Object.Instantiate(gameObject2, gameObject3.transform.position, gameObject3.transform.rotation) as GameObject;
				if (gameObject3 != null && gameObject4 != null)
				{
					gameObject3.gameObject.SetActive(true);
					SphereCollider component = gameObject3.GetComponent<SphereCollider>();
					if (component != null)
					{
						component.enabled = false;
					}
					gameObject4.gameObject.SetActive(true);
					gameObject4.transform.parent = gameObject3.transform;
					gameObject4.transform.localRotation = Quaternion.Euler(ObjAnimalUtil.ModelLocalRotation);
					int animalAddCount = (ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_chaoAbility != ChaoAbility.UNKNOWN) ? ObjAnimalUtil.GetChaoAbilityAnimalCount(ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_chaoAbility, ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_count) : ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_count;
					if (ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_moveType == AnimalMoveType.FLY)
					{
						ObjAnimalFly component2 = gameObject3.GetComponent<ObjAnimalFly>();
						if (component2 != null)
						{
							component2.SetAnimalAddCount(animalAddCount);
						}
					}
					else
					{
						ObjAnimalJump component3 = gameObject3.GetComponent<ObjAnimalJump>();
						if (component3 != null)
						{
							component3.SetAnimalAddCount(animalAddCount);
						}
					}
					if (type != AnimalType.FLICKY)
					{
						if (type == AnimalType.PSO2_1)
						{
							ObjUtil.RequestStartAbilityToChao(ChaoAbility.SPECIAL_ANIMAL_PSO2, true);
						}
					}
					else
					{
						ObjUtil.RequestStartAbilityToChao(ChaoAbility.SPECIAL_ANIMAL, true);
					}
				}
			}
		}
	}

	public static void CreateAnimal(GameObject enm_obj)
	{
		AnimalType animalType = AnimalType.ERROR;
		if (StageAbilityManager.Instance != null)
		{
			List<AnimalType> list = new List<AnimalType>();
			if (StageAbilityManager.Instance.HasChaoAbility(ChaoAbility.SPECIAL_ANIMAL))
			{
				int num = (int)StageAbilityManager.Instance.GetChaoAbilityValue(ChaoAbility.SPECIAL_ANIMAL);
				if (num >= ObjUtil.GetRandomRange100())
				{
					list.Add(AnimalType.FLICKY);
				}
			}
			if (StageAbilityManager.Instance.HasChaoAbility(ChaoAbility.SPECIAL_ANIMAL_PSO2))
			{
				int num2 = (int)StageAbilityManager.Instance.GetChaoAbilityValue(ChaoAbility.SPECIAL_ANIMAL_PSO2);
				if (num2 >= ObjUtil.GetRandomRange100())
				{
					list.Add(AnimalType.PSO2_1);
				}
			}
			if (list.Count == 1)
			{
				animalType = list[0];
			}
			else if (list.Count >= 2)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				animalType = list[index];
			}
		}
		if (animalType == AnimalType.ERROR)
		{
			int num3 = UnityEngine.Random.Range(0, ObjAnimalUtil.NORMALTYPE_TABLE.Length);
			animalType = ObjAnimalUtil.NORMALTYPE_TABLE[num3];
		}
		if (AnimalResourceManager.Instance != null)
		{
			GameObject stockAnimal = AnimalResourceManager.Instance.GetStockAnimal(animalType);
			if (stockAnimal != null)
			{
				stockAnimal.transform.position = enm_obj.transform.position;
				stockAnimal.transform.rotation = Quaternion.identity;
				stockAnimal.SetActive(true);
				AnimalType animalType2 = animalType;
				if (animalType2 != AnimalType.FLICKY)
				{
					if (animalType2 == AnimalType.PSO2_1)
					{
						ObjUtil.RequestStartAbilityToChao(ChaoAbility.SPECIAL_ANIMAL_PSO2, true);
					}
				}
				else
				{
					ObjUtil.RequestStartAbilityToChao(ChaoAbility.SPECIAL_ANIMAL, true);
				}
				return;
			}
		}
		ObjAnimalUtil.CreateAnimal(enm_obj, animalType);
	}

	public static GameObject CreateStockAnimal(GameObject parentObj, AnimalType type)
	{
		if ((ulong)type >= (ulong)((long)ObjAnimalUtil.ANIMAL_PARAM.Length))
		{
			return null;
		}
		if (parentObj != null)
		{
			string moveCompName = ObjAnimalUtil.GetMoveCompName(ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_moveType);
			string model = ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_model;
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.ENEMY_PREFAB, moveCompName);
			GameObject gameObject2 = ResourceManager.Instance.GetGameObject(ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_category, model);
			if (gameObject != null && gameObject2 != null)
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity) as GameObject;
				GameObject gameObject4 = UnityEngine.Object.Instantiate(gameObject2, Vector3.zero, Quaternion.identity) as GameObject;
				if (gameObject3 != null && gameObject4 != null)
				{
					gameObject3.name = moveCompName;
					gameObject3.gameObject.SetActive(false);
					gameObject3.gameObject.transform.parent = parentObj.transform;
					SphereCollider component = gameObject3.GetComponent<SphereCollider>();
					if (component != null)
					{
						component.enabled = false;
					}
					gameObject4.name = model;
					gameObject4.gameObject.SetActive(true);
					gameObject4.transform.parent = gameObject3.transform;
					gameObject4.transform.localRotation = Quaternion.Euler(ObjAnimalUtil.ModelLocalRotation);
					int animalAddCount = (ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_chaoAbility != ChaoAbility.UNKNOWN) ? ObjAnimalUtil.GetChaoAbilityAnimalCount(ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_chaoAbility, ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_count) : ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_count;
					if (ObjAnimalUtil.ANIMAL_PARAM[(int)type].m_moveType == AnimalMoveType.FLY)
					{
						ObjAnimalFly component2 = gameObject3.GetComponent<ObjAnimalFly>();
						if (component2 != null)
						{
							component2.SetAnimalAddCount(animalAddCount);
							component2.SetShareState(type);
						}
					}
					else
					{
						ObjAnimalJump component3 = gameObject3.GetComponent<ObjAnimalJump>();
						if (component3 != null)
						{
							component3.SetAnimalAddCount(animalAddCount);
							component3.SetShareState(type);
						}
					}
					if (type != AnimalType.FLICKY)
					{
						if (type == AnimalType.PSO2_1)
						{
							ObjUtil.RequestStartAbilityToChao(ChaoAbility.SPECIAL_ANIMAL_PSO2, false);
						}
					}
					else
					{
						ObjUtil.RequestStartAbilityToChao(ChaoAbility.SPECIAL_ANIMAL, false);
					}
					return gameObject3;
				}
			}
		}
		return null;
	}
}
