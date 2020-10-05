using System;
using UnityEngine;

[Serializable]
public class MultiSetSectorParameter : SpawnableParameter
{
	public int m_count;

	public float m_radius;

	public float m_angle;

	public GameObject m_object;

	public MultiSetSectorParameter() : base("MultiSetSector")
	{
		this.m_count = 2;
		this.m_radius = 1f;
		this.m_angle = 180f;
	}
}
