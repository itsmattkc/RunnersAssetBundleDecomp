using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjLostRing : MonoBehaviour
{
	private class LostRingParam
	{
		public float m_add_x;

		public float m_add_y;

		public LostRingParam(float add_x, float add_y)
		{
			this.m_add_x = add_x;
			this.m_add_y = add_y;
		}
	}

	private const float END_TIME = 1.2f;

	private const float RING_SPEED = 6f;

	private const float RING_GRAVITY = -6.1f;

	private const int RING_MAX = 6;

	private const float MAGNET_SPEED = 0.1f;

	private const float MAGNET_TIME = 0.2f;

	private const float MAGNET_ADDX = 0f;

	private const float MAGNET_ADDY = 0f;

	private int m_count;

	private int m_createCount;

	private float m_time;

	private static readonly ObjLostRing.LostRingParam[] LOSTRING_PARAM = new ObjLostRing.LostRingParam[]
	{
		new ObjLostRing.LostRingParam(0.8f, 0.8f),
		new ObjLostRing.LostRingParam(0.5f, 1.2f),
		new ObjLostRing.LostRingParam(0.2f, 0.2f),
		new ObjLostRing.LostRingParam(0.4f, 0.4f),
		new ObjLostRing.LostRingParam(0.8f, 1.2f),
		new ObjLostRing.LostRingParam(0.5f, 0.5f),
		new ObjLostRing.LostRingParam(1.2f, 1.1f),
		new ObjLostRing.LostRingParam(0.6f, 0.5f),
		new ObjLostRing.LostRingParam(0.6f, 0.9f),
		new ObjLostRing.LostRingParam(1f, 1.3f)
	};

	private GameObject m_chaoObj;

	private List<GameObject> m_objList = new List<GameObject>();

	private float m_magnetSpeed;

	private bool m_magnetStart;

	private int m_ringCount;

	private void Start()
	{
		this.m_count = this.m_ringCount;
		if (this.m_count > 6)
		{
			this.m_count = 6;
		}
		if (this.m_count > 0 && this.m_count <= ObjLostRing.LOSTRING_PARAM.Length)
		{
			this.CreateRing(0, Mathf.Min(this.m_count, 3));
		}
		if (this.m_chaoObj != null)
		{
			ObjUtil.RequestStartAbilityToChao(ChaoAbility.RECOVERY_RING, false);
		}
	}

	private void Update()
	{
		if (this.m_createCount < this.m_count)
		{
			this.CreateRing(this.m_createCount, this.m_count);
		}
		else
		{
			this.m_time += Time.deltaTime;
			if (this.IsChaoMagnet())
			{
				if (!this.m_magnetStart)
				{
					if (this.m_time > 0.2f)
					{
						this.StartChaoMagnet();
						this.m_magnetStart = true;
					}
				}
				else if (this.UpdateChaoMagnet())
				{
					this.m_time = 2.4f;
				}
			}
			if (this.m_time > 1.2f)
			{
				if (this.m_magnetStart)
				{
					this.TakeChaoRing();
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	private void CreateRing(int startCount, int endCount)
	{
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ObjRing.GetRingModelCategory(), ObjRing.GetRingModelName());
		GameObject gameObject2 = ResourceManager.Instance.GetGameObject(ResourceCategory.OBJECT_PREFAB, "MotorThrow");
		if (gameObject != null && gameObject2 != null)
		{
			for (int i = startCount; i < endCount; i++)
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject, base.transform.position, base.transform.rotation) as GameObject;
				GameObject gameObject4 = UnityEngine.Object.Instantiate(gameObject2, base.transform.position, base.transform.rotation) as GameObject;
				if (gameObject3 && gameObject4)
				{
					gameObject3.gameObject.SetActive(true);
					gameObject3.transform.parent = base.gameObject.transform;
					gameObject3.transform.localRotation = Quaternion.Euler(Vector3.zero);
					gameObject4.gameObject.SetActive(true);
					gameObject4.transform.parent = gameObject3.transform;
					MotorThrow component = gameObject4.GetComponent<MotorThrow>();
					if (component)
					{
						MotorThrow.ThrowParam throwParam = new MotorThrow.ThrowParam();
						throwParam.m_obj = gameObject3;
						throwParam.m_speed = 6f;
						throwParam.m_gravity = -6.1f;
						throwParam.m_add_x = ObjLostRing.LOSTRING_PARAM[i].m_add_x;
						throwParam.m_add_y = ObjLostRing.LOSTRING_PARAM[i].m_add_y;
						throwParam.m_up = base.transform.up;
						throwParam.m_rot_speed = 0f;
						throwParam.m_rot_angle = Vector3.zero;
						throwParam.m_forward = -base.transform.right;
						if (this.IsChaoMagnet())
						{
							MotorThrow.ThrowParam expr_1A7 = throwParam;
							expr_1A7.m_add_x = expr_1A7.m_add_x;
							MotorThrow.ThrowParam expr_1B4 = throwParam;
							expr_1B4.m_add_y = expr_1B4.m_add_y;
						}
						component.Setup(throwParam);
					}
					this.m_objList.Add(gameObject3);
				}
				this.m_createCount++;
			}
		}
	}

	public void SetChaoMagnet(GameObject chaoObj)
	{
		if (this.m_chaoObj == null)
		{
			this.m_chaoObj = chaoObj;
		}
	}

	public void SetRingCount(int ringCount)
	{
		this.m_ringCount = ringCount;
	}

	private bool IsChaoMagnet()
	{
		return this.m_chaoObj != null;
	}

	private void StartChaoMagnet()
	{
		if (this.m_chaoObj != null)
		{
			float num = ObjUtil.GetPlayerAddSpeed();
			if (num < 0f)
			{
				num = 0f;
			}
			this.m_magnetSpeed = 0.1f + 0.01f * num;
			foreach (GameObject current in this.m_objList)
			{
				if (current)
				{
					for (int i = 0; i < current.transform.childCount; i++)
					{
						GameObject gameObject = current.transform.GetChild(i).gameObject;
						MotorThrow component = gameObject.GetComponent<MotorThrow>();
						if (component)
						{
							component.SetEnd();
							break;
						}
					}
				}
			}
		}
	}

	private bool UpdateChaoMagnet()
	{
		if (this.m_chaoObj == null)
		{
			return true;
		}
		bool result = true;
		foreach (GameObject current in this.m_objList)
		{
			if (current)
			{
				float num = 0.1f - this.m_time * this.m_magnetSpeed;
				if (num < 0.02f)
				{
					num = 0f;
				}
				else
				{
					result = false;
				}
				Vector3 zero = Vector3.zero;
				Vector3 position = this.m_chaoObj.transform.position;
				current.transform.position = Vector3.SmoothDamp(current.transform.position, position, ref zero, num);
			}
		}
		return result;
	}

	private void TakeChaoRing()
	{
		StageAbilityManager instance = StageAbilityManager.Instance;
		if (instance)
		{
			instance.SetLostRingCount(this.m_ringCount);
		}
	}
}
