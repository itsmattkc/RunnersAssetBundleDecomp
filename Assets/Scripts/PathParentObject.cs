using System;
using UnityEngine;

public class PathParentObject : MonoBehaviour
{
	private static Vector3 NEW_OFFSET_POS = new Vector3(1f, 0f, 0f);

	private void Update()
	{
		this.UpdatePose();
	}

	public void UpdatePose()
	{
		if (base.transform.childCount > 0)
		{
			GameObject gameObject = base.transform.GetChild(0).gameObject;
			if (gameObject)
			{
				Vector3 start = gameObject.transform.position;
				for (int i = 0; i < base.transform.childCount; i++)
				{
					GameObject gameObject2 = base.transform.GetChild(i).gameObject;
					if (gameObject2)
					{
						int num = i + 1;
						if (num < base.transform.childCount)
						{
							GameObject gameObject3 = base.transform.GetChild(num).gameObject;
							if (gameObject3)
							{
								gameObject2.transform.LookAt(gameObject3.transform);
							}
						}
						else
						{
							GameObject gameObject4 = base.transform.GetChild(i - 1).gameObject;
							if (gameObject4)
							{
								gameObject2.transform.rotation = gameObject4.transform.rotation;
							}
						}
						Vector3 position = gameObject2.transform.position;
						global::Debug.DrawLine(start, position, Color.red);
						start = position;
					}
				}
			}
		}
	}

	public void AddPathObject(string name, float size)
	{
		int childCount = base.transform.childCount;
		if (childCount > 0)
		{
			GameObject pathObject = this.GetPathObject((uint)(childCount - 1));
			if (pathObject != null)
			{
				this.CreatePathObject(name, pathObject.transform.position + PathParentObject.NEW_OFFSET_POS, Quaternion.identity, size);
			}
		}
		else
		{
			this.CreatePathObject(name, Vector3.zero, Quaternion.identity, size);
		}
	}

	public void CreatePathObject(string name, Vector3 pos, Quaternion rot, float size)
	{
		string name2 = name + base.transform.childCount.ToString();
		GameObject gameObject = new GameObject(name2);
		if (gameObject)
		{
			gameObject.transform.parent = base.transform;
			gameObject.transform.position = pos;
			gameObject.transform.rotation = rot;
			gameObject.SetActive(true);
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			if (sphereCollider)
			{
				sphereCollider.radius = size;
			}
		}
	}

	public int GetPathObjectCount()
	{
		return base.transform.childCount;
	}

	public GameObject GetPathObject(uint index)
	{
		if ((ulong)index < (ulong)((long)base.transform.childCount))
		{
			return base.transform.GetChild((int)index).gameObject;
		}
		return null;
	}

	public void SetZeroZ()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			GameObject gameObject = base.transform.GetChild(i).gameObject;
			if (gameObject)
			{
				gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0f);
			}
		}
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			GameObject gameObject = base.transform.GetChild(i).gameObject;
			if (gameObject)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(gameObject.transform.position, 0.2f);
			}
		}
	}
}
