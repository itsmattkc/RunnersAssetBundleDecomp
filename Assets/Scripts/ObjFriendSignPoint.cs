using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjFriendSignPoint : SpawnableObject
{
	private const float m_offsetPosY = 2.3f;

	private const float m_offsetArea = 10f;

	private const float m_checkArea = 20f;

	private static int CREATE_COUNT = 5;

	private FriendSignPointData m_myData;

	private List<FriendSignCreateData> m_createDataList;

	private int m_createCount;

	private int m_createCountMax;

	private bool m_setupFind;

	private bool m_setup;

	private PlayerInformation m_playerInfo;

	private float m_setupDistance;

	private bool m_debugDraw;

	private static Comparison<FriendSignPointData> __f__am_cacheA;

	protected override void OnSpawned()
	{
	}

	private void Update()
	{
		if (!this.m_setupFind)
		{
			if (this.IsFindFriendSignPoint())
			{
				this.SetupFriendSign();
				this.m_setup = true;
			}
			else
			{
				this.m_setupDistance = this.GetTotalDistance();
				this.DebugDraw(string.Concat(new object[]
				{
					"IsFindFriendSignPoint NG 1 m_setupDistance=",
					this.m_setupDistance,
					" pos.x=",
					base.transform.position.x
				}));
			}
			this.m_setupFind = true;
		}
		if (!this.m_setup)
		{
			float num = this.GetTotalDistance() - this.m_setupDistance;
			if (num > 20f)
			{
				this.DebugDraw("IsFindFriendSignPoint OK pos.x=" + base.transform.position.x);
				this.SetupFriendSign();
				this.m_setup = true;
			}
		}
		if (this.m_setup && this.m_createCountMax > 0)
		{
			int num2 = 0;
			foreach (FriendSignCreateData current in this.m_createDataList)
			{
				if (!current.m_create)
				{
					this.CreateObject(current.m_texture);
					current.m_create = true;
					num2++;
					if (num2 >= this.m_createCountMax)
					{
						break;
					}
				}
			}
			if (num2 == 0)
			{
				this.m_createCountMax = 0;
			}
		}
	}

	private void SetupFriendSign()
	{
		this.m_createDataList = new List<FriendSignCreateData>();
		float totalDistance = this.GetTotalDistance();
		float x = this.GetPlayerPos().x;
		List<GameObject> list = new List<GameObject>();
		this.FindFriendSignPoint(ref list);
		List<FriendSignPointData> list2 = new List<FriendSignPointData>();
		foreach (GameObject current in list)
		{
			float num = current.transform.position.x - x + totalDistance - 10f;
			if (num < 0f)
			{
				num = 0f;
			}
			FriendSignPointData friendSignPointData = new FriendSignPointData(current, num, 0f, base.transform.position.x == current.transform.position.x);
			list2.Add(friendSignPointData);
			this.DebugDraw(string.Concat(new object[]
			{
				"ObjFriendSignPoint Data : my=",
				friendSignPointData.m_myPoint.ToString(),
				" distance=",
				friendSignPointData.m_distance,
				" pos.x=",
				current.transform.position.x
			}));
		}
		list2.Sort((FriendSignPointData d1, FriendSignPointData d2) => d2.m_distance.CompareTo(d1.m_distance));
		float num2 = 0f;
		foreach (FriendSignPointData current2 in list2)
		{
			if (num2 == 0f)
			{
				num2 = current2.m_distance + 50f + 10f;
			}
			if (current2.m_myPoint)
			{
				this.m_myData = new FriendSignPointData(current2.m_obj, current2.m_distance, num2, current2.m_myPoint);
				this.DebugDraw(string.Concat(new object[]
				{
					"ObjFriendSignPoint myPoint :  distance=",
					this.m_myData.m_distance,
					" next=",
					this.m_myData.m_nextDistance,
					" pos.x=",
					this.m_myData.m_obj.transform.position.x
				}));
				break;
			}
			num2 = current2.m_distance + 10f;
		}
		FriendSignManager instance = FriendSignManager.Instance;
		if (instance)
		{
			List<FriendSignData> friendSignDataList = instance.GetFriendSignDataList();
			foreach (FriendSignData current3 in friendSignDataList)
			{
				if (!current3.m_appear && this.AddFriendSignData(current3.m_distance, current3.m_texture))
				{
					instance.SetAppear(current3.m_index);
				}
			}
		}
		if (this.m_createDataList.Count > 0)
		{
			if (this.m_createDataList.Count >= ObjFriendSignPoint.CREATE_COUNT)
			{
				this.m_createCountMax = this.m_createDataList.Count / ObjFriendSignPoint.CREATE_COUNT + 1;
			}
			else
			{
				this.m_createCountMax = 1;
			}
		}
	}

	private bool AddFriendSignData(float friendDistance, Texture2D texture)
	{
		if (this.m_myData.m_distance <= friendDistance && friendDistance < this.m_myData.m_nextDistance)
		{
			this.m_createDataList.Add(new FriendSignCreateData(texture));
			return true;
		}
		return false;
	}

	private void CreateObject(Texture2D texture)
	{
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.OBJECT_PREFAB, "ObjFriendSign");
		if (gameObject)
		{
			float num = 2.3f * (float)this.m_createCount;
			Vector3 position = new Vector3(base.transform.position.x, base.transform.position.y + num, base.transform.position.z);
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, position, base.transform.rotation) as GameObject;
			if (gameObject2)
			{
				gameObject2.SetActive(true);
				gameObject2.transform.parent = base.transform;
				SpawnableObject component = gameObject2.GetComponent<SpawnableObject>();
				if (component)
				{
					component.AttachModelObject();
				}
				BoxCollider component2 = gameObject2.GetComponent<BoxCollider>();
				if (component2)
				{
					component2.center = new Vector3(component2.center.x, component2.center.y - num, component2.center.z);
				}
				ObjFriendSign component3 = gameObject2.GetComponent<ObjFriendSign>();
				if (component3)
				{
					component3.ChangeTexture(texture);
				}
				this.m_createCount++;
			}
		}
	}

	private void FindFriendSignPoint(ref List<GameObject> objList)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("FriendSign");
		GameObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			GameObject gameObject = array2[i];
			if (base.transform.position.x <= gameObject.transform.position.x)
			{
				objList.Add(gameObject);
			}
		}
	}

	private bool IsFindFriendSignPoint()
	{
		List<GameObject> list = new List<GameObject>();
		this.FindFriendSignPoint(ref list);
		return list.Count > 1;
	}

	private float GetTotalDistance()
	{
		PlayerInformation playerInfo = this.GetPlayerInfo();
		if (playerInfo != null)
		{
			return playerInfo.TotalDistance;
		}
		return 0f;
	}

	private Vector3 GetPlayerPos()
	{
		PlayerInformation playerInfo = this.GetPlayerInfo();
		if (playerInfo != null)
		{
			return playerInfo.Position;
		}
		return Vector3.zero;
	}

	private PlayerInformation GetPlayerInfo()
	{
		if (this.m_playerInfo == null)
		{
			this.m_playerInfo = ObjUtil.GetPlayerInformation();
		}
		return this.m_playerInfo;
	}

	private void DebugDraw(string msg)
	{
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.transform.position, 0.5f);
	}
}
