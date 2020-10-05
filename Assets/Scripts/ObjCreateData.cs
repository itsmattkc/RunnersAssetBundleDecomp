using System;
using UnityEngine;

public class ObjCreateData
{
	public GameObject m_obj;

	public GameObject m_src;

	public Vector3 m_pos;

	public Quaternion m_rot;

	public bool m_create;

	public ObjCreateData(GameObject src, Vector3 pos, Quaternion rot)
	{
		this.m_obj = null;
		this.m_src = src;
		this.m_pos = pos;
		this.m_rot = rot;
		this.m_create = false;
	}
}
