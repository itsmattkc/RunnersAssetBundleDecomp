using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiSetBase : SpawnableObject
{
	private static int CREATE_COUNT = 5;

	protected List<ObjCreateData> m_dataList;

	private int m_createCountMax;

	private PlayerInformation m_playerInformation;

	private LevelInformation m_levelInformation;

	protected override void OnSpawned()
	{
	}

	protected virtual void OnCreateSetup()
	{
	}

	protected virtual void UpdateLocal()
	{
	}

	protected override void OnDestroyed()
	{
		for (int i = 0; i < this.m_dataList.Count; i++)
		{
			if (this.m_dataList[i].m_obj != null)
			{
				SpawnableObject component = this.m_dataList[i].m_obj.GetComponent<SpawnableObject>();
				if (component != null && component.Share)
				{
					base.SetSleep(this.m_dataList[i].m_obj);
				}
			}
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
					this.m_dataList[i].m_create = true;
					objCreateData.m_obj = this.CreateObject(base.gameObject, objCreateData.m_src, objCreateData.m_pos, objCreateData.m_rot);
					if (objCreateData.m_obj != null)
					{
						this.OnCreateSetup();
					}
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
		this.UpdateLocal();
	}

	public void Setup()
	{
		if (this.m_dataList == null)
		{
			this.m_dataList = new List<ObjCreateData>();
		}
		this.m_playerInformation = ObjUtil.GetPlayerInformation();
		this.m_levelInformation = ObjUtil.GetLevelInformation();
	}

	public void AddObject(GameObject srcObject, Vector3 pos, Quaternion rot)
	{
		if (this.m_dataList == null)
		{
			return;
		}
		if (srcObject != null)
		{
			SpawnableObject component = srcObject.GetComponent<SpawnableObject>();
			if (component != null && component.IsValid())
			{
				this.m_dataList.Add(new ObjCreateData(srcObject, pos, rot));
			}
		}
		if (this.m_dataList.Count > 0)
		{
			if (this.m_dataList.Count >= MultiSetBase.CREATE_COUNT)
			{
				this.m_createCountMax = this.m_dataList.Count / MultiSetBase.CREATE_COUNT + 1;
			}
			else
			{
				this.m_createCountMax = 1;
			}
		}
	}

	private GameObject CreateObject(GameObject parent, GameObject srcObject, Vector3 pos, Quaternion rot)
	{
		GameObject gameObject = ObjUtil.GetChangeObject(ResourceManager.Instance, this.m_playerInformation, this.m_levelInformation, srcObject.name);
		SpawnableObject spawnableObject;
		if (gameObject != null)
		{
			spawnableObject = this.GetReviveSpawnableObject(gameObject);
		}
		else
		{
			gameObject = ObjUtil.GetCrystalChangeObject(ResourceManager.Instance, srcObject);
			if (gameObject != null)
			{
				spawnableObject = this.GetReviveSpawnableObject(gameObject);
			}
			else
			{
				spawnableObject = this.GetReviveSpawnableObject(srcObject);
			}
		}
		GameObject gameObject2;
		if (spawnableObject != null)
		{
			this.SetRevivalSpawnableObject(spawnableObject, pos, rot);
			gameObject2 = spawnableObject.gameObject;
		}
		else
		{
			if (gameObject != null)
			{
				gameObject2 = (UnityEngine.Object.Instantiate(gameObject, pos, rot) as GameObject);
			}
			else
			{
				gameObject2 = (UnityEngine.Object.Instantiate(srcObject, pos, rot) as GameObject);
			}
			spawnableObject = gameObject2.GetComponent<SpawnableObject>();
			if (spawnableObject != null)
			{
				spawnableObject.AttachModelObject();
			}
		}
		if (gameObject2 && parent)
		{
			gameObject2.SetActive(true);
			gameObject2.transform.parent = parent.transform;
		}
		return gameObject2;
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
		ObjectSpawnManager manager = base.GetManager();
		if (manager != null && component.IsStockObject())
		{
			return manager.GetSpawnableObject(component.GetStockObjectType());
		}
		return null;
	}

	private void SetRevivalSpawnableObject(SpawnableObject spawnableObject, Vector3 pos, Quaternion rot)
	{
		if (spawnableObject != null)
		{
			spawnableObject.Sleep = false;
			spawnableObject.gameObject.transform.position = pos;
			spawnableObject.gameObject.transform.rotation = rot;
			spawnableObject.OnRevival();
		}
	}
}
