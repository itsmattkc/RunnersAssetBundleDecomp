using System;
using UnityEngine;

public class ConnectAlertSimpleUI : MonoBehaviour
{
	private int m_refCount;

	private GameObject m_alertObj;

	private void Start()
	{
		this.m_alertObj = GameObjectUtil.FindChildGameObject(base.gameObject, "Alert");
		if (this.m_alertObj != null)
		{
			this.m_alertObj.SetActive(false);
		}
		base.enabled = false;
	}

	public void StartCollider()
	{
		this.m_refCount++;
		if (this.m_alertObj != null)
		{
			this.m_alertObj.SetActive(true);
		}
	}

	public void EndCollider()
	{
		this.m_refCount--;
		if (this.m_refCount > 0)
		{
			return;
		}
		this.m_refCount = 0;
		if (this.m_alertObj != null)
		{
			this.m_alertObj.SetActive(false);
		}
	}
}
