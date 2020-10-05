using System;
using UnityEngine;

public class FriendSignPointData
{
	public GameObject m_obj;

	public float m_distance;

	public float m_nextDistance;

	public bool m_myPoint;

	public FriendSignPointData(GameObject obj, float distance, float nextDistance, bool myPoint)
	{
		this.m_obj = obj;
		this.m_distance = distance;
		this.m_nextDistance = nextDistance;
		this.m_myPoint = myPoint;
	}
}
