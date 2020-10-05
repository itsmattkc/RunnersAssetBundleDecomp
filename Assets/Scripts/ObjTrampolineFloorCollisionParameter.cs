using System;
using UnityEngine;

[Serializable]
public class ObjTrampolineFloorCollisionParameter : SpawnableParameter
{
	public float m_firstSpeed;

	public float m_outOfcontrol;

	private float m_size_x;

	private float m_size_y;

	private float m_size_z;

	public ObjTrampolineFloorCollisionParameter() : base("ObjTrampolineFloorCollisionParameter")
	{
		this.m_size_x = 1f;
		this.m_size_y = 1f;
		this.m_size_z = 1f;
		this.m_firstSpeed = 8f;
		this.m_outOfcontrol = 0.1f;
	}

	public void SetSize(Vector3 size)
	{
		this.m_size_x = size.x;
		this.m_size_y = size.y;
		this.m_size_z = size.z;
	}

	public Vector3 GetSize()
	{
		return new Vector3(this.m_size_x, this.m_size_y, this.m_size_z);
	}
}
