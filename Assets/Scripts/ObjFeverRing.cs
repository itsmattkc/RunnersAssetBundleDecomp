using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjFeverRing : MonoBehaviour
{
	private enum Type
	{
		RING,
		SUPER_RING,
		REDSTAR_RING,
		BRONZE_TIMER,
		SILVER_TIMER,
		GOLD_TIMER,
		NUM
	}

	private class FeverRingData
	{
		public string m_name;

		public ResourceCategory m_category;

		public FeverRingData(string name, ResourceCategory category)
		{
			this.m_name = name;
			this.m_category = category;
		}
	}

	private class FeverRingParam
	{
		public float m_add_x;

		public float m_add_y;

		public FeverRingParam(float add_x, float add_y)
		{
			this.m_add_x = add_x;
			this.m_add_y = add_y;
		}
	}

	private struct FeverRingInfo
	{
		public ObjFeverRing.FeverRingParam m_param;

		public ObjFeverRing.Type m_type;
	}

	private const float END_TIME = 6f;

	private const float RING_SPEED = 6f;

	private const float RING_GRAVITY = -6.1f;

	private const float BOUND_Y = 1f;

	private const float BOUND_DOWN_X = 0.5f;

	private const float BOUND_DOWN_Y = 0.01f;

	private const float BOUND_MIN = 1.8f;

	private const int CREATE_MAX = 5;

	private static readonly ObjFeverRing.FeverRingData[] OBJDATA_PARAMS = new ObjFeverRing.FeverRingData[]
	{
		new ObjFeverRing.FeverRingData("ObjRing", ResourceCategory.OBJECT_PREFAB),
		new ObjFeverRing.FeverRingData("ObjSuperRing", ResourceCategory.OBJECT_PREFAB),
		new ObjFeverRing.FeverRingData("ObjRedStarRing", ResourceCategory.OBJECT_PREFAB),
		new ObjFeverRing.FeverRingData("ObjTimerBronze", ResourceCategory.OBJECT_PREFAB),
		new ObjFeverRing.FeverRingData("ObjTimerSilver", ResourceCategory.OBJECT_PREFAB),
		new ObjFeverRing.FeverRingData("ObjTimerGold", ResourceCategory.OBJECT_PREFAB)
	};

	private static readonly ObjFeverRing.FeverRingParam[] FEVERRING_PARAM = new ObjFeverRing.FeverRingParam[]
	{
		new ObjFeverRing.FeverRingParam(4f, 0.81f),
		new ObjFeverRing.FeverRingParam(3.5f, 1.22f),
		new ObjFeverRing.FeverRingParam(2.5f, 0.23f),
		new ObjFeverRing.FeverRingParam(4.4f, 0.44f),
		new ObjFeverRing.FeverRingParam(3.8f, 1.35f),
		new ObjFeverRing.FeverRingParam(5f, 0.56f),
		new ObjFeverRing.FeverRingParam(2f, 1.17f),
		new ObjFeverRing.FeverRingParam(4f, 1.58f),
		new ObjFeverRing.FeverRingParam(3.5f, 1.39f),
		new ObjFeverRing.FeverRingParam(3f, 0.31f),
		new ObjFeverRing.FeverRingParam(1.3f, 0.72f),
		new ObjFeverRing.FeverRingParam(3.2f, 0.43f),
		new ObjFeverRing.FeverRingParam(2.4f, 1.24f),
		new ObjFeverRing.FeverRingParam(4.3f, 0.55f),
		new ObjFeverRing.FeverRingParam(3.2f, 1.26f),
		new ObjFeverRing.FeverRingParam(3.6f, 1.27f),
		new ObjFeverRing.FeverRingParam(2.4f, 0.88f),
		new ObjFeverRing.FeverRingParam(4.2f, 0.49f),
		new ObjFeverRing.FeverRingParam(3f, 1.21f),
		new ObjFeverRing.FeverRingParam(4.1f, 0.82f)
	};

	private int m_count;

	private int m_createCount;

	private float m_time;

	private float m_add_player_speed;

	private ObjFeverRing.FeverRingInfo[] m_info;

	private int m_bossType;

	private PlayerInformation m_playerInformation;

	private LevelInformation m_levelInformation;

	private List<SpawnableObject> m_rivivalObj = new List<SpawnableObject>();

	private GameObject m_stageBlockManager;

	private void Update()
	{
		if (this.m_count > 0)
		{
			if (this.m_createCount < this.m_count)
			{
				this.CreateRing(this.m_createCount, Mathf.Min(this.m_createCount + 5, this.m_count));
			}
			this.m_time += Time.deltaTime;
			if (this.m_time > 6f)
			{
				foreach (SpawnableObject current in this.m_rivivalObj)
				{
					if (current.Share)
					{
						current.Sleep = true;
						GameObject gameObject = GameObjectUtil.FindChildGameObject(current.gameObject, "MotorThrow(Clone)");
						if (gameObject != null)
						{
							UnityEngine.Object.Destroy(gameObject);
						}
						current.SetSleep();
					}
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public void Setup(int ringCount, int in_superRing, int in_redStarRing, int in_bronzeTimer, int in_silverTimer, int in_goldTimer, BossType type)
	{
		this.m_playerInformation = ObjUtil.GetPlayerInformation();
		this.m_levelInformation = ObjUtil.GetLevelInformation();
		this.m_stageBlockManager = GameObjectUtil.FindGameObjectWithTag("StageManager", "StageBlockManager");
		int num = in_redStarRing;
		if (StageModeManager.Instance != null && StageModeManager.Instance.FirstTutorial)
		{
			num = 0;
		}
		this.m_bossType = (int)type;
		this.m_add_player_speed = ObjUtil.GetPlayerAddSpeed();
		this.m_time = 0f;
		this.m_count = ringCount;
		if (this.m_count > 0)
		{
			this.m_info = new ObjFeverRing.FeverRingInfo[this.m_count];
			int num2 = UnityEngine.Random.Range(0, ObjFeverRing.FEVERRING_PARAM.Length);
			float num3 = 0f;
			int num4 = num2;
			int num5 = num2;
			for (int i = 0; i < this.m_count; i++)
			{
				this.m_info[i].m_type = ObjFeverRing.Type.RING;
				this.m_info[i].m_param = new ObjFeverRing.FeverRingParam(0f, 0f);
				if (num4 >= ObjFeverRing.FEVERRING_PARAM.Length)
				{
					num3 = (float)(num5 / ObjFeverRing.FEVERRING_PARAM.Length);
					num4 = 0;
				}
				this.m_info[i].m_param.m_add_x = ObjFeverRing.FEVERRING_PARAM[num4].m_add_x + num3 * 0.05f;
				this.m_info[i].m_param.m_add_y = ObjFeverRing.FEVERRING_PARAM[num4].m_add_y + num3 * 0.05f;
				switch (this.m_bossType)
				{
				case 1:
				case 3:
					this.m_info[i].m_param.m_add_x *= 0.5f;
					break;
				}
				num4++;
				num5++;
			}
			float num6 = (float)in_superRing * 0.01f;
			int num7 = (int)((float)this.m_count * num6);
			num7 = Mathf.Min(num7, this.m_info.Length);
			for (int j = 0; j < num7; j++)
			{
				this.m_info[j].m_type = ObjFeverRing.Type.SUPER_RING;
			}
			if (this.IsEnableCreateTimer() && this.m_info.Length > 1)
			{
				int randomRange = ObjUtil.GetRandomRange100();
				int num8 = in_bronzeTimer + in_silverTimer;
				int num9 = num8 + in_goldTimer;
				if (randomRange < in_bronzeTimer)
				{
					this.m_info[this.m_info.Length - 2].m_type = ObjFeverRing.Type.BRONZE_TIMER;
				}
				else if (randomRange < num8)
				{
					this.m_info[this.m_info.Length - 2].m_type = ObjFeverRing.Type.SILVER_TIMER;
				}
				else if (randomRange < num9)
				{
					this.m_info[this.m_info.Length - 2].m_type = ObjFeverRing.Type.GOLD_TIMER;
				}
			}
			int randomRange2 = ObjUtil.GetRandomRange100();
			if (randomRange2 < num)
			{
				this.m_info[this.m_info.Length - 1].m_type = ObjFeverRing.Type.REDSTAR_RING;
			}
			this.CreateRing(0, Mathf.Min(5, this.m_count));
		}
	}

	private bool IsEnableCreateTimer()
	{
		return StageModeManager.Instance != null && StageModeManager.Instance.IsQuickMode() && ObjTimerUtil.IsEnableCreateTimer();
	}

	private void CreateRing(int startCount, int endCount)
	{
		if (endCount > this.m_info.Length)
		{
			return;
		}
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.OBJECT_PREFAB, "MotorThrow");
		if (gameObject != null)
		{
			float num = this.m_add_player_speed * 0.2f;
			for (int i = startCount; i < endCount; i++)
			{
				string text = string.Empty;
				ResourceCategory category = ResourceCategory.UNKNOWN;
				uint type = (uint)this.m_info[i].m_type;
				if ((ulong)type < (ulong)((long)ObjFeverRing.OBJDATA_PARAMS.Length))
				{
					text = ObjFeverRing.OBJDATA_PARAMS[(int)((UIntPtr)type)].m_name;
					category = ObjFeverRing.OBJDATA_PARAMS[(int)((UIntPtr)type)].m_category;
				}
				GameObject gameObject2 = ObjUtil.GetChangeObject(ResourceManager.Instance, this.m_playerInformation, this.m_levelInformation, text);
				if (gameObject2 == null)
				{
					gameObject2 = ResourceManager.Instance.GetGameObject(category, text);
				}
				if (gameObject2 != null)
				{
					Vector3 vector = base.transform.position + new Vector3(1f + num, 0f, 0f);
					SpawnableObject spawnableObject = this.GetReviveSpawnableObject(gameObject2);
					bool flag = false;
					GameObject gameObject3;
					if (spawnableObject != null)
					{
						this.SetRevivalSpawnableObject(spawnableObject, vector);
						this.m_rivivalObj.Add(spawnableObject);
						gameObject3 = spawnableObject.gameObject;
						flag = true;
					}
					else
					{
						gameObject3 = (UnityEngine.Object.Instantiate(gameObject2, vector, base.transform.rotation) as GameObject);
					}
					GameObject gameObject4 = UnityEngine.Object.Instantiate(gameObject, vector, base.transform.rotation) as GameObject;
					if (gameObject3 && gameObject4)
					{
						gameObject3.gameObject.SetActive(true);
						gameObject3.transform.parent = base.gameObject.transform;
						if (!flag)
						{
							spawnableObject = gameObject3.GetComponent<SpawnableObject>();
							spawnableObject.AttachModelObject();
							ObjTimerBase component = gameObject3.GetComponent<ObjTimerBase>();
							if (component != null)
							{
								component.SetMoveType(ObjTimerBase.MoveType.Bound);
							}
						}
						gameObject4.gameObject.SetActive(true);
						gameObject4.transform.parent = gameObject3.transform;
						MotorThrow component2 = gameObject4.GetComponent<MotorThrow>();
						if (component2)
						{
							component2.Setup(new MotorThrow.ThrowParam
							{
								m_obj = gameObject3,
								m_speed = 6f,
								m_gravity = -6.1f,
								m_add_x = this.m_info[i].m_param.m_add_x + num,
								m_add_y = this.m_info[i].m_param.m_add_y,
								m_up = base.transform.up,
								m_forward = base.transform.right,
								m_rot_angle = Vector3.zero,
								m_rot_speed = 0f,
								m_rot_downspeed = 0f,
								m_bound = true,
								m_bound_pos_y = 0f,
								m_bound_add_y = 1.8f + this.m_info[i].m_param.m_add_y * 1f,
								m_bound_down_x = 0.5f,
								m_bound_down_y = 0.01f
							});
						}
					}
				}
				this.m_createCount++;
			}
		}
	}

	private void SetRevivalSpawnableObject(SpawnableObject spawnableObject, Vector3 pos)
	{
		if (spawnableObject != null)
		{
			spawnableObject.Sleep = false;
			spawnableObject.gameObject.transform.position = pos;
			spawnableObject.gameObject.transform.rotation = base.transform.rotation;
			spawnableObject.OnRevival();
		}
	}

	private SpawnableObject GetReviveSpawnableObject(GameObject srcObj)
	{
		if (srcObj == null)
		{
			return null;
		}
		SpawnableObject component = srcObj.GetComponent<SpawnableObject>();
		if (component == null)
		{
			return null;
		}
		if (this.m_stageBlockManager != null)
		{
			ObjectSpawnManager component2 = this.m_stageBlockManager.GetComponent<ObjectSpawnManager>();
			if (component2 != null && component.IsStockObject())
			{
				return component2.GetSpawnableObject(component.GetStockObjectType());
			}
		}
		return null;
	}
}
