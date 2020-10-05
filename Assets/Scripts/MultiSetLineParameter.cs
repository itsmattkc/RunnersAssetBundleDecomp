using System;
using UnityEngine;

[Serializable]
public class MultiSetLineParameter : SpawnableParameter
{
	public int m_count;

	public float m_distance;

	public int m_type;

	public GameObject m_object;

	public MultiSetLineParameter() : base("MultiSetLine")
	{
		this.m_count = 2;
		this.m_distance = 1f;
		this.m_type = 0;
	}
}
