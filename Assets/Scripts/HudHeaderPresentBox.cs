using System;
using System.Collections.Generic;
using UnityEngine;

public class HudHeaderPresentBox : MonoBehaviour
{
	private enum DataType
	{
		PRESEN_BOX,
		VOLUME_LABEL
	}

	private const string m_present_box_path = "Anchor_7_BL/Btn_2_presentbox";

	private GameObject m_present_box_badge;

	private UILabel m_volume_label;

	private BoxCollider m_collider;

	private UIImageButton m_image_button;

	private bool m_initEnd;

	private void Start()
	{
		HudMenuUtility.SetTagHudSaveItem(base.gameObject);
		base.enabled = false;
	}

	private void Initialize()
	{
		if (this.m_initEnd)
		{
			return;
		}
		GameObject mainMenuUIObject = HudMenuUtility.GetMainMenuUIObject();
		if (mainMenuUIObject != null)
		{
			GameObject gameObject = mainMenuUIObject.transform.FindChild("Anchor_7_BL/Btn_2_presentbox").gameObject;
			if (gameObject != null)
			{
				this.m_present_box_badge = gameObject.transform.FindChild("badge").gameObject;
				if (this.m_present_box_badge != null)
				{
					GameObject gameObject2 = this.m_present_box_badge.transform.FindChild("Lbl_present_volume").gameObject;
					if (gameObject2 != null)
					{
						this.m_volume_label = gameObject2.GetComponent<UILabel>();
					}
				}
				this.m_collider = gameObject.GetComponent<BoxCollider>();
				this.m_image_button = gameObject.GetComponent<UIImageButton>();
			}
		}
		this.m_initEnd = true;
	}

	public void OnUpdateSaveDataDisplay()
	{
		this.Initialize();
		int num = 0;
		if (ServerInterface.MessageList != null)
		{
			List<ServerMessageEntry> messageList = ServerInterface.MessageList;
			foreach (ServerMessageEntry current in messageList)
			{
				if (PresentBoxUtility.IsWithinTimeLimit(current.m_expireTiem))
				{
					num++;
				}
			}
		}
		if (ServerInterface.OperatorMessageList != null)
		{
			List<ServerOperatorMessageEntry> operatorMessageList = ServerInterface.OperatorMessageList;
			foreach (ServerOperatorMessageEntry current2 in operatorMessageList)
			{
				if (PresentBoxUtility.IsWithinTimeLimit(current2.m_expireTiem))
				{
					num++;
				}
			}
		}
		if (this.m_volume_label != null)
		{
			this.m_volume_label.text = num.ToString();
		}
		if (num == 0)
		{
			if (this.m_present_box_badge != null)
			{
				this.m_present_box_badge.SetActive(false);
			}
			if (this.m_collider != null)
			{
				this.m_collider.isTrigger = true;
			}
			if (this.m_image_button != null)
			{
				this.m_image_button.isEnabled = false;
			}
		}
		else
		{
			if (this.m_present_box_badge != null)
			{
				this.m_present_box_badge.SetActive(true);
			}
			if (this.m_collider != null)
			{
				this.m_collider.isTrigger = false;
			}
			if (this.m_image_button != null)
			{
				this.m_image_button.isEnabled = true;
			}
		}
	}
}
