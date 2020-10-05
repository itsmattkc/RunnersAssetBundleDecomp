using System;
using UnityEngine;

public class PathComponent : MonoBehaviour
{
	private ResPathObject m_resPathObject;

	private PathManager m_pathManager;

	private uint m_id;

	public void SetObject(ResPathObject pathObject)
	{
		this.m_resPathObject = pathObject;
	}

	public ResPathObject GetResPathObject()
	{
		return this.m_resPathObject;
	}

	private PathManager GetManager()
	{
		return this.m_pathManager;
	}

	public string GetName()
	{
		if (this.m_resPathObject != null)
		{
			return this.m_resPathObject.Name;
		}
		return null;
	}

	public uint GetID()
	{
		return this.m_id;
	}

	public bool IsValid()
	{
		return this.m_resPathObject != null;
	}

	private void Cleanup()
	{
		if (this.m_pathManager != null)
		{
			this.m_pathManager = null;
		}
		this.m_resPathObject = null;
	}

	private void OnDestroy()
	{
		this.Cleanup();
	}

	public void SetManager(PathManager manager)
	{
		this.m_pathManager = manager;
	}

	public void SetID(uint id)
	{
		this.m_id = id;
	}

	public void DrawGizmos()
	{
		if (this.m_resPathObject == null)
		{
			return;
		}
		ResPathObjectData objectData = this.m_resPathObject.GetObjectData();
		Vector3 to = objectData.position[0];
		for (int i = 0; i < (int)objectData.numKeys; i++)
		{
			Vector3 vector = objectData.position[i];
			Vector3 a = objectData.normal[i];
			float d = 0.2f;
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(vector, vector + a * d);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(vector, to);
			to = vector;
		}
	}
}
