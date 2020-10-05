using System;
using UnityEngine;

[Serializable]
public class ObjLoopTerrainParameter : SpawnableParameter
{
	public string m_pathName;

	public float m_pathZOffset;

	private float m_scalex;

	private float m_scaley;

	private float m_scalez;

	private float m_centerx;

	private float m_centery;

	private float m_centerz;

	public Vector3 Center
	{
		get
		{
			return new Vector3(this.m_centerx, this.m_centery, this.m_centerz);
		}
		set
		{
			this.m_centerx = value.x;
			this.m_centery = value.y;
			this.m_centerz = value.z;
		}
	}

	public Vector3 Size
	{
		get
		{
			return new Vector3(this.m_scalex, this.m_scaley, this.m_scalez);
		}
		set
		{
			this.m_scalex = value.x;
			this.m_scaley = value.y;
			this.m_scalez = value.z;
		}
	}

	public ObjLoopTerrainParameter() : base("ObjLoopTerrain")
	{
	}
}
