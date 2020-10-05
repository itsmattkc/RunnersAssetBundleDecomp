using Message;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HudAlert : MonoBehaviour
{
	private const float IconDisplayTime = 1f;

	private List<HudAlertIcon> m_iconList;

	private Camera m_camera;

	public void StartAlert(GameObject chaseObject)
	{
		HudAlertIcon hudAlertIcon = base.gameObject.AddComponent<HudAlertIcon>();
		hudAlertIcon.Setup(this.m_camera, chaseObject, 1f);
		this.m_iconList.Add(hudAlertIcon);
	}

	public void EndAlert(GameObject chaseObject)
	{
		HudAlertIcon hudAlertIcon = null;
		foreach (HudAlertIcon current in this.m_iconList)
		{
			if (!(current == null))
			{
				if (current.IsChasingObject(chaseObject))
				{
					hudAlertIcon = current;
				}
			}
		}
		if (hudAlertIcon != null)
		{
			this.m_iconList.Remove(hudAlertIcon);
			UnityEngine.Object.Destroy(hudAlertIcon);
		}
	}

	private void Start()
	{
		this.m_iconList = new List<HudAlertIcon>();
		GameObject gameObject = GameObject.Find("GameMainCamera");
		if (gameObject != null)
		{
			this.m_camera = gameObject.GetComponent<Camera>();
			if (this.m_camera == null)
			{
			}
		}
	}

	private void Update()
	{
		if (this.m_iconList.Count <= 0)
		{
			return;
		}
		List<HudAlertIcon> list = new List<HudAlertIcon>();
		foreach (HudAlertIcon current in this.m_iconList)
		{
			if (!(current == null))
			{
				if (current.IsEnd)
				{
					list.Add(current);
				}
			}
		}
		foreach (HudAlertIcon current2 in list)
		{
			if (!(current2 == null))
			{
				this.m_iconList.Remove(current2);
				UnityEngine.Object.Destroy(current2);
			}
		}
		list.Clear();
	}

	private void OnMsgExitStage(MsgExitStage msg)
	{
		base.enabled = false;
	}
}
