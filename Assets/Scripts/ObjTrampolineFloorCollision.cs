using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjTrampolineFloorCollision : SpawnableObject
{
	private const int CREATE_COUNT = 5;

	private ObjTrampolineFloorCollisionParameter m_param;

	private List<ObjCreateData> m_dataList = new List<ObjCreateData>();

	private int m_createCountMax;

	private bool m_end;

	protected override void OnSpawned()
	{
		if (this.m_end)
		{
			return;
		}
		if (this.m_createCountMax == 0 && StageItemManager.Instance != null && StageItemManager.Instance.IsActiveTrampoline())
		{
			this.CreateTrampolineFloor();
		}
	}

	private void Update()
	{
		if (this.m_createCountMax > 0 && this.m_dataList != null)
		{
			int num = 0;
			for (int i = 0; i < this.m_dataList.Count; i++)
			{
				ObjCreateData objCreateData = this.m_dataList[i];
				if (objCreateData.m_obj == null && !objCreateData.m_create)
				{
					objCreateData.m_create = true;
					objCreateData.m_obj = this.CreateObject(objCreateData.m_src, objCreateData.m_pos, objCreateData.m_rot);
					num++;
					if (num >= this.m_createCountMax)
					{
						break;
					}
				}
			}
			if (num == 0)
			{
				this.m_createCountMax = 0;
			}
		}
	}

	public void SetObjCollisionParameter(ObjTrampolineFloorCollisionParameter param)
	{
		this.m_param = param;
	}

	public void OnTransformPhantom(MsgTransformPhantom msg)
	{
		if (this.m_dataList != null)
		{
			foreach (ObjCreateData current in this.m_dataList)
			{
				if (current.m_obj)
				{
					UnityEngine.Object.Destroy(current.m_obj);
				}
			}
			this.m_dataList.Clear();
		}
		this.m_end = false;
	}

	private void OnUseItem(MsgUseItem item)
	{
		if (this.m_end)
		{
			return;
		}
		if (item.m_itemType == ItemType.TRAMPOLINE && this.m_createCountMax == 0)
		{
			this.CreateTrampolineFloor();
		}
	}

	private void CreateTrampolineFloor()
	{
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.OBJECT_PREFAB, "ObjTrampolineFloor");
		if (gameObject)
		{
			BoxCollider component = base.GetComponent<BoxCollider>();
			if (component)
			{
				int num = Mathf.FloorToInt(component.size.x);
				float num2 = 1f;
				float num3 = 0f;
				float num4 = num2 / 2f;
				if (num % 2 != 0)
				{
					this.AddObject(gameObject, base.transform.position, base.transform.rotation);
					num3 = num4;
				}
				int num5 = num / 2;
				for (int i = 0; i < num5; i++)
				{
					float d = num4 + (float)i * num2 + num3;
					Vector3 pos = base.transform.position + base.transform.right * d;
					this.AddObject(gameObject, pos, base.transform.rotation);
					Vector3 pos2 = base.transform.position + -base.transform.right * d;
					this.AddObject(gameObject, pos2, base.transform.rotation);
				}
				this.m_createCountMax = Mathf.Min(this.m_dataList.Count, 5);
			}
		}
		this.m_end = true;
	}

	private void AddObject(GameObject src, Vector3 pos, Quaternion rot)
	{
		this.m_dataList.Add(new ObjCreateData(src, pos, rot));
	}

	private GameObject CreateObject(GameObject src, Vector3 pos, Quaternion rot)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(src, pos, rot) as GameObject;
		if (gameObject)
		{
			gameObject.SetActive(true);
			gameObject.transform.parent = base.transform;
			SpawnableObject component = gameObject.GetComponent<SpawnableObject>();
			if (component)
			{
				component.AttachModelObject();
			}
			if (this.m_param != null)
			{
				ObjTrampolineFloor component2 = gameObject.GetComponent<ObjTrampolineFloor>();
				if (component2)
				{
					component2.SetParam(this.m_param.m_firstSpeed, this.m_param.m_outOfcontrol);
				}
			}
		}
		return gameObject;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}
}
