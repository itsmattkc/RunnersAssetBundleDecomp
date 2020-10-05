using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimalResourceManager : MonoBehaviour
{
	private class StockParam
	{
		public int m_stockCount;

		public ChaoAbility m_chaoAbility;

		public StockParam(int stockCount, ChaoAbility chaoAbility)
		{
			this.m_stockCount = stockCount;
			this.m_chaoAbility = chaoAbility;
		}
	}

	public class DepotObjs
	{
		private List<GameObject> m_objList = new List<GameObject>();

		public void Add(GameObject obj)
		{
			if (obj != null)
			{
				this.m_objList.Add(obj);
			}
		}

		public GameObject Get()
		{
			foreach (GameObject current in this.m_objList)
			{
				ObjAnimalBase component = current.GetComponent<ObjAnimalBase>();
				if (component != null && component.IsSleep())
				{
					component.Sleep = false;
					component.OnRevival();
					return current;
				}
			}
			return null;
		}

		public void Sleep(GameObject obj)
		{
			if (obj != null)
			{
				ObjAnimalBase component = obj.GetComponent<ObjAnimalBase>();
				if (component != null)
				{
					component.Sleep = true;
				}
			}
		}
	}

	private static AnimalResourceManager s_instance = null;

	private static readonly AnimalResourceManager.StockParam[] STOCK_PARAM = new AnimalResourceManager.StockParam[]
	{
		new AnimalResourceManager.StockParam(8, ChaoAbility.SPECIAL_ANIMAL),
		new AnimalResourceManager.StockParam(4, ChaoAbility.UNKNOWN),
		new AnimalResourceManager.StockParam(4, ChaoAbility.UNKNOWN),
		new AnimalResourceManager.StockParam(4, ChaoAbility.UNKNOWN),
		new AnimalResourceManager.StockParam(4, ChaoAbility.UNKNOWN),
		new AnimalResourceManager.StockParam(4, ChaoAbility.UNKNOWN),
		new AnimalResourceManager.StockParam(4, ChaoAbility.UNKNOWN),
		new AnimalResourceManager.StockParam(8, ChaoAbility.SPECIAL_ANIMAL_PSO2)
	};

	private Dictionary<AnimalType, AnimalResourceManager.DepotObjs> m_depotObjs = new Dictionary<AnimalType, AnimalResourceManager.DepotObjs>();

	public static AnimalResourceManager Instance
	{
		get
		{
			return AnimalResourceManager.s_instance;
		}
	}

	public GameObject GetStockAnimal(AnimalType type)
	{
		if (this.m_depotObjs.ContainsKey(type))
		{
			return this.m_depotObjs[type].Get();
		}
		return null;
	}

	public void SetSleep(AnimalType type, GameObject obj)
	{
		if (this.m_depotObjs.ContainsKey(type))
		{
			this.m_depotObjs[type].Sleep(obj);
		}
	}

	public void StockAnimalObject(bool bossStage)
	{
		if (bossStage)
		{
			return;
		}
		for (int i = 0; i < 8; i++)
		{
			if (this.CheckAblity(AnimalResourceManager.STOCK_PARAM[i].m_chaoAbility))
			{
				AnimalType animalType = (AnimalType)i;
				GameObject gameObject = new GameObject(animalType.ToString());
				if (gameObject != null)
				{
					gameObject.transform.parent = base.gameObject.transform;
					AnimalResourceManager.DepotObjs depotObjs = new AnimalResourceManager.DepotObjs();
					for (int j = 0; j < AnimalResourceManager.STOCK_PARAM[i].m_stockCount; j++)
					{
						depotObjs.Add(ObjAnimalUtil.CreateStockAnimal(gameObject, animalType));
					}
					this.m_depotObjs.Add(animalType, depotObjs);
				}
			}
		}
	}

	private bool CheckAblity(ChaoAbility ability)
	{
		return ability == ChaoAbility.UNKNOWN || (StageAbilityManager.Instance != null && StageAbilityManager.Instance.HasChaoAbility(ability));
	}

	protected void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
		base.enabled = false;
	}

	private void OnDestroy()
	{
		if (AnimalResourceManager.s_instance == this)
		{
			AnimalResourceManager.s_instance = null;
		}
	}

	private void SetInstance()
	{
		if (AnimalResourceManager.s_instance == null)
		{
			AnimalResourceManager.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
