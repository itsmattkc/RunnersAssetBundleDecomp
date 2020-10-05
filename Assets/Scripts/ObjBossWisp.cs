using Message;
using System;
using UnityEngine;

public class ObjBossWisp : MonoBehaviour
{
	private const string MODEL_NAME = "obj_raid_wisp";

	private const ResourceCategory MODEL_CATEGORY = ResourceCategory.EVENT_RESOURCE;

	private const float WAIT_END_TIME = 7f;

	private const float ADD_SPEED = 0.12f;

	public static string EFFECT_NAME = "ef_raid_wisp_get01";

	public static string SE_NAME = "rb_wisp_get";

	private float m_speed = 0.5f;

	private float m_distance = 1f;

	private float m_addX = 1f;

	private float m_time;

	private float m_move_speed;

	private GameObject m_objBoss;

	private static Vector3 ModelLocalRotation = new Vector3(0f, 180f, 0f);

	public void Setup(GameObject objBoss, float speed, float distance, float addX)
	{
		this.m_objBoss = objBoss;
		this.m_speed = speed;
		this.m_distance = distance;
		this.m_addX = addX;
		this.m_move_speed = 0.12f * ObjUtil.GetPlayerAddSpeed();
		this.CreateModel();
		MotorAnimalFly component = base.GetComponent<MotorAnimalFly>();
		if (component)
		{
			component.SetupParam(this.m_speed, this.m_distance, this.m_addX + this.m_move_speed, base.transform.right, 0f, false);
		}
	}

	private void Update()
	{
		this.m_time += Time.deltaTime;
		if (this.m_time > 7f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void EndMotorComponent()
	{
		MotorAnimalFly component = base.GetComponent<MotorAnimalFly>();
		if (component)
		{
			component.enabled = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other)
		{
			GameObject gameObject = other.gameObject;
			if (gameObject)
			{
				string a = LayerMask.LayerToName(gameObject.layer);
				if (a == "Player" || a == "Chao")
				{
					this.TakeWisp();
				}
				else if (a == "Magnet")
				{
				}
			}
		}
	}

	private void OnDrawingRings(MsgOnDrawingRings msg)
	{
		ObjUtil.StartMagnetControl(base.gameObject);
		this.EndMotorComponent();
		this.m_time = 0f;
	}

	private void TakeWisp()
	{
		if (this.m_objBoss != null)
		{
			this.m_objBoss.SendMessage("OnGetWisp", SendMessageOptions.DontRequireReceiver);
		}
		ObjUtil.PlayEffect(ObjBossWisp.EFFECT_NAME, ObjUtil.GetCollisionCenterPosition(base.gameObject), base.transform.rotation, 1f, false);
		ObjUtil.PlayEventSE(ObjBossWisp.SE_NAME, EventManager.EventType.RAID_BOSS);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void CreateModel()
	{
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.EVENT_RESOURCE, "obj_raid_wisp");
		if (gameObject)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, base.transform.position, base.transform.rotation) as GameObject;
			if (gameObject2)
			{
				gameObject2.gameObject.SetActive(true);
				gameObject2.transform.parent = base.gameObject.transform;
				gameObject2.transform.localRotation = Quaternion.Euler(ObjBossWisp.ModelLocalRotation);
			}
		}
	}
}
