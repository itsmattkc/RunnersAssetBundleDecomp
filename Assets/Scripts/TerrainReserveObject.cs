using System;
using UnityEngine;

public class TerrainReserveObject
{
	private GameObject m_gameObject;

	private string m_blockName = string.Empty;

	private int m_reserveIndex = -1;

	private bool m_rented;

	public string blockName
	{
		get
		{
			return this.m_blockName;
		}
	}

	public bool EableReservation
	{
		get
		{
			return !this.m_rented;
		}
	}

	public int ReserveIndex
	{
		get
		{
			return this.m_reserveIndex;
		}
	}

	public TerrainReserveObject(GameObject obj, string name, int reserveIndex)
	{
		this.m_gameObject = obj;
		this.m_blockName = name;
		this.m_reserveIndex = reserveIndex;
		this.m_rented = false;
	}

	public GameObject ReserveObject()
	{
		this.m_rented = true;
		return this.m_gameObject;
	}

	public void ReturnObject()
	{
		this.m_rented = false;
		if (this.m_gameObject != null && this.m_gameObject.activeSelf)
		{
			this.m_gameObject.SetActive(false);
		}
	}

	public GameObject GetGameObject()
	{
		return this.m_gameObject;
	}
}
