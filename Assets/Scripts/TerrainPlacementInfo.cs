using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TerrainPlacementInfo
{
	private int m_reserveIndex = -1;

	private bool m_destroyed;

	private int _m_terrainIndex_k__BackingField;

	private TerrainBlock _m_block_k__BackingField;

	private GameObject _m_gameObject_k__BackingField;

	public int m_terrainIndex
	{
		get;
		set;
	}

	public TerrainBlock m_block
	{
		get;
		set;
	}

	public GameObject m_gameObject
	{
		get;
		set;
	}

	public int ReserveIndex
	{
		get
		{
			return this.m_reserveIndex;
		}
		set
		{
			this.m_reserveIndex = value;
		}
	}

	public bool Created
	{
		get
		{
			return this.m_gameObject != null;
		}
	}

	public bool Destroyed
	{
		get
		{
			return this.m_destroyed;
		}
	}

	public TerrainPlacementInfo()
	{
		this.m_destroyed = false;
	}

	public bool IsReserveTerrain()
	{
		return this.m_reserveIndex != -1;
	}

	public void DestroyObject()
	{
		this.m_gameObject = null;
		this.m_destroyed = true;
	}
}
