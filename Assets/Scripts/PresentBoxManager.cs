using System;
using System.Collections.Generic;
using UnityEngine;

public class PresentBoxManager : MonoBehaviour
{
	private static PresentBoxManager instance;

	private List<PresentItem> m_present_datas;

	public static PresentBoxManager Instance
	{
		get
		{
			return PresentBoxManager.instance;
		}
	}

	protected void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
		if (this.m_present_datas == null)
		{
			this.m_present_datas = new List<PresentItem>();
		}
		base.enabled = false;
	}

	private void OnDestroy()
	{
		if (PresentBoxManager.instance == this)
		{
			PresentBoxManager.instance = null;
		}
	}

	private void SetInstance()
	{
		if (PresentBoxManager.instance == null)
		{
			PresentBoxManager.instance = this;
		}
		else if (this != PresentBoxManager.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public PresentItem GetData(int index)
	{
		if (index < this.m_present_datas.Count)
		{
			return this.m_present_datas[index];
		}
		return null;
	}

	public int GetDataCount()
	{
		return this.m_present_datas.Count;
	}
}
