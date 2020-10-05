using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TransformParam
{
	private Vector3 _m_pos_k__BackingField;

	private Vector3 _m_rot_k__BackingField;

	public Vector3 m_pos
	{
		get;
		private set;
	}

	public Vector3 m_rot
	{
		get;
		private set;
	}

	public TransformParam(Vector3 pos, Vector3 rot)
	{
		this.m_pos = pos;
		this.m_rot = rot;
	}
}
