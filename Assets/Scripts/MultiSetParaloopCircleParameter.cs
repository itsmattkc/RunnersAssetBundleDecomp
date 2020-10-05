using System;
using UnityEngine;

[Serializable]
public class MultiSetParaloopCircleParameter : SpawnableParameter
{
	public int m_count;

	public float m_radius;

	public GameObject m_object;

	private float m_size_x;

	private float m_size_y;

	private float m_size_z;

	private float m_center_x;

	private float m_center_y;

	private float m_center_z;

	public MultiSetParaloopCircleParameter() : base("MultiSetParaloopCircle")
	{
		this.m_count = 2;
		this.m_radius = 1f;
		this.m_size_x = 0f;
		this.m_size_y = 0f;
		this.m_size_z = 0f;
		this.m_center_x = 0f;
		this.m_center_y = 0f;
		this.m_center_z = 0f;
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

	public void SetCenter(Vector3 center)
	{
		this.m_center_x = center.x;
		this.m_center_y = center.y;
		this.m_center_z = center.z;
	}

	public Vector3 GetCenter()
	{
		return new Vector3(this.m_center_x, this.m_center_y, this.m_center_z);
	}
}
