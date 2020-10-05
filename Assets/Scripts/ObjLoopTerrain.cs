using Message;
using System;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Object/Common/ObjFloor")]
public class ObjLoopTerrain : SpawnableObject
{
	private const float Path_DefaultZOFfset = 1.5f;

	private PathComponent m_component;

	private string m_pathName;

	private float m_pathZOffset;

	protected override void OnSpawned()
	{
		if (this.m_pathName != null)
		{
			PathManager pathManager = GameObjectUtil.FindGameObjectComponent<PathManager>("StagePathManager");
			if (pathManager != null)
			{
				Vector3 position = base.transform.position;
				position.z += this.m_pathZOffset + 1.5f;
				this.m_component = pathManager.CreatePathComponent(this.m_pathName, position);
			}
		}
	}

	private void OnDestroy()
	{
		PathManager pathManager = GameObjectUtil.FindGameObjectComponent<PathManager>("StagePathManager");
		if (pathManager != null && this.m_component != null)
		{
			pathManager.DestroyComponent(this.m_component);
		}
		this.m_component = null;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.m_component != null)
		{
			MsgRunLoopPath value = new MsgRunLoopPath(this.m_component);
			other.gameObject.SendMessage("OnRunLoopPath", value, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void SetPathName(string pathName)
	{
		this.m_pathName = pathName;
	}

	public void SetZOffset(float zoffset)
	{
		this.m_pathZOffset = zoffset;
	}
}
