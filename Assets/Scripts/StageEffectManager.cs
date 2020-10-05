using SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StageEffectManager : MonoBehaviour
{
	public enum EffectCategory
	{
		CHARACTER,
		ENEMY,
		OBJECT,
		SP_EVENT
	}

	public class EffectData
	{
		public StageEffectManager.EffectCategory m_category;

		public float m_endTime;

		public int m_stockCount;

		public string m_name;

		public bool m_bossStage;

		public EffectData(StageEffectManager.EffectCategory category, string name, float endTime, int stockCount, bool bossStage)
		{
			this.m_category = category;
			this.m_name = name;
			this.m_endTime = endTime;
			this.m_stockCount = stockCount;
			this.m_bossStage = bossStage;
		}
	}

	public class DepotObjs
	{
		private List<GameObject> m_objList = new List<GameObject>();

		private GameObject m_folderObj;

		public DepotObjs(GameObject parentObj, string folderName)
		{
			this.m_folderObj = new GameObject();
			if (this.m_folderObj != null)
			{
				this.m_folderObj.name = folderName;
				if (parentObj != null)
				{
					this.m_folderObj.transform.parent = parentObj.transform;
				}
			}
		}

		public void Add(GameObject obj)
		{
			if (obj != null)
			{
				this.m_objList.Add(obj);
				if (this.m_folderObj != null)
				{
					obj.transform.parent = this.m_folderObj.transform;
				}
			}
		}

		public GameObject Get()
		{
			foreach (GameObject current in this.m_objList)
			{
				EffectShareState component = current.GetComponent<EffectShareState>();
				if (component != null && component.IsSleep())
				{
					component.SetState(EffectShareState.State.Active);
					return current;
				}
			}
			return null;
		}

		public void Sleep(GameObject obj)
		{
			if (obj != null)
			{
				EffectShareState component = obj.GetComponent<EffectShareState>();
				if (component != null)
				{
					component.SetState(EffectShareState.State.Sleep);
					obj.SetActive(false);
					if (this.m_folderObj != null)
					{
						obj.transform.parent = this.m_folderObj.transform;
					}
				}
			}
		}
	}

	private static StageEffectManager instance;

	private StageEffectManager.EffectData[] EFFECT_DATA_TBL = new StageEffectManager.EffectData[]
	{
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.CHARACTER, "ef_pl_fog_jump_st01", 1f, 3, true),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.CHARACTER, "ef_pl_fog_jump_ed01", 1f, 3, true),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.CHARACTER, "ef_pl_fog_run01", 1.5f, 8, true),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.CHARACTER, "ef_pl_fog_speedrun01", 1.5f, 8, true),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.ENEMY, "ef_en_dead_s01", 1f, 10, false),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.ENEMY, "ef_en_dead_m01", 1f, 10, false),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.ENEMY, "ef_en_dead_l01", 1f, 10, false),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.ENEMY, "ef_en_guard01", 1f, 3, false),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.ENEMY, "ef_com_explosion_m01", 1f, 10, false),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.ENEMY, "ef_com_explosion_s01", 1f, 10, false),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.OBJECT, "ef_ob_get_ring01", 1f, 10, true),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.OBJECT, "ef_ob_get_superring01", 1f, 10, true),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.OBJECT, "ef_ob_get_animal01", 1f, 6, false),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.OBJECT, "ef_ob_get_crystal_s01", 1f, 10, false),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.OBJECT, "ef_ob_get_crystal_gr_s01", 1f, 10, false),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.OBJECT, "ef_ob_get_crystal_rd_s01", 1f, 10, false),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.OBJECT, "ef_ob_get_crystal_l01", 1f, 10, false),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.OBJECT, "ef_ob_get_crystal_gr_l01", 1f, 10, false),
		new StageEffectManager.EffectData(StageEffectManager.EffectCategory.OBJECT, "ef_ob_get_crystal_rd_l01", 1f, 10, false)
	};

	[SerializeField]
	private GameObject m_charaParentObj;

	[SerializeField]
	private GameObject m_enemyParentObj;

	[SerializeField]
	private GameObject m_objectParentObj;

	[SerializeField]
	private GameObject m_specialEventParentObj;

	private Dictionary<EffectPlayType, StageEffectManager.DepotObjs> m_depotObjs = new Dictionary<EffectPlayType, StageEffectManager.DepotObjs>();

	private bool m_lightMode;

	public static StageEffectManager Instance
	{
		get
		{
			return StageEffectManager.instance;
		}
	}

	public void StockStageEffect(bool bossStage)
	{
		if (SystemSaveManager.GetSystemSaveData() != null)
		{
			this.m_lightMode = SystemSaveManager.GetSystemSaveData().lightMode;
		}
		if (this.m_lightMode)
		{
			return;
		}
		if (ResourceManager.Instance != null)
		{
			for (int i = 0; i < this.EFFECT_DATA_TBL.Length; i++)
			{
				bool flag = false;
				StageEffectManager.EffectData effectData = this.EFFECT_DATA_TBL[i];
				if (!bossStage || effectData.m_bossStage)
				{
					EffectPlayType effectPlayType = (EffectPlayType)i;
					GameObject parentObj = this.m_charaParentObj;
					switch (effectData.m_category)
					{
					case StageEffectManager.EffectCategory.ENEMY:
						parentObj = this.m_enemyParentObj;
						break;
					case StageEffectManager.EffectCategory.OBJECT:
						parentObj = this.m_objectParentObj;
						break;
					case StageEffectManager.EffectCategory.SP_EVENT:
						parentObj = this.m_specialEventParentObj;
						flag = true;
						break;
					}
					StageEffectManager.DepotObjs depotObjs = new StageEffectManager.DepotObjs(parentObj, effectPlayType.ToString());
					for (int j = 0; j < effectData.m_stockCount; j++)
					{
						ResourceCategory category = (!flag) ? ResourceCategory.COMMON_EFFECT : ResourceCategory.EVENT_RESOURCE;
						GameObject gameObject = ResourceManager.Instance.GetGameObject(category, effectData.m_name);
						if (gameObject)
						{
							GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity) as GameObject;
							if (gameObject2 != null)
							{
								gameObject2.name = effectData.m_name;
								gameObject2.SetActive(false);
								EffectShareState effectShareState = gameObject2.AddComponent<EffectShareState>();
								if (effectShareState != null)
								{
									effectShareState.m_effectType = effectPlayType;
								}
								ykKillTime component = gameObject2.GetComponent<ykKillTime>();
								if (component != null)
								{
									component.enabled = false;
								}
								EffectPlayTime effectPlayTime = gameObject2.GetComponent<EffectPlayTime>();
								if (effectPlayTime == null)
								{
									effectPlayTime = gameObject2.AddComponent<EffectPlayTime>();
									if (effectPlayTime != null)
									{
										effectPlayTime.m_endTime = effectData.m_endTime;
									}
								}
								depotObjs.Add(gameObject2);
							}
						}
					}
					this.m_depotObjs.Add(effectPlayType, depotObjs);
				}
			}
		}
	}

	public void PlayEffect(EffectPlayType type, Vector3 pos, Quaternion rot)
	{
		if (this.m_lightMode)
		{
			return;
		}
		if (this.m_depotObjs.ContainsKey(type))
		{
			GameObject gameObject = this.m_depotObjs[type].Get();
			if (gameObject != null)
			{
				gameObject.transform.position = pos;
				gameObject.transform.rotation = rot;
				EffectPlayTime component = gameObject.GetComponent<EffectPlayTime>();
				if (component != null)
				{
					component.PlayEffect();
					return;
				}
			}
		}
		if (this.IsCreateEffect(type))
		{
			ObjUtil.PlayEffect(this.EFFECT_DATA_TBL[(int)type].m_name, pos, rot, this.EFFECT_DATA_TBL[(int)type].m_endTime, false);
		}
	}

	private bool IsCreateEffect(EffectPlayType type)
	{
		return EffectPlayType.ENEMY_S <= type && type <= EffectPlayType.AIR_TRAP;
	}

	public void SleepEffect(GameObject obj)
	{
		if (this.m_lightMode)
		{
			return;
		}
		if (obj != null)
		{
			EffectShareState component = obj.GetComponent<EffectShareState>();
			if (component != null && this.m_depotObjs.ContainsKey(component.m_effectType))
			{
				this.m_depotObjs[component.m_effectType].Sleep(obj);
			}
		}
	}

	private void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		if (StageEffectManager.instance == this)
		{
			StageEffectManager.instance = null;
		}
	}

	private void SetInstance()
	{
		if (StageEffectManager.instance == null)
		{
			StageEffectManager.instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
