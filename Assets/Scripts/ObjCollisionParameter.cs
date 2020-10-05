using System;
using UnityEngine;

[Serializable]
public class ObjCollisionParameter : SpawnableParameter
{
	private float m_size_x;

	private float m_size_y;

	private float m_size_z;

	public ObjCollisionParameter() : base("ObjCollision")
	{
		this.m_size_x = 1f;
		this.m_size_y = 1f;
		this.m_size_z = 1f;
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
