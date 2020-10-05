using System;
using UnityEngine;

public class HudInformation : MonoBehaviour
{
	private GameObject m_badgeObj;

	private GameObject m_mileageObj;

	private UILabel m_volumeLabel;

	private bool m_initFlag;

	private void Start()
	{
		HudMenuUtility.SetTagHudSaveItem(base.gameObject);
		base.enabled = false;
	}

	private void Initialize()
	{
		this.m_initFlag = true;
		GameObject mainMenuUIObject = HudMenuUtility.GetMainMenuUIObject();
		if (mainMenuUIObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(mainMenuUIObject, "Anchor_7_BL");
			if (gameObject != null)
			{
				GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "Btn_0_info");
				if (gameObject2 != null)
				{
					this.m_badgeObj = GameObjectUtil.FindChildGameObject(gameObject2, "badge");
					this.m_volumeLabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject2, "Lbl_present_volume");
				}
			}
		}
	}

	private void UpdateInfoIcon()
	{
		int num = 0;
		if (ServerInterface.NoticeInfo != null && ServerInterface.NoticeInfo.m_noticeItems != null)
		{
			foreach (NetNoticeItem current in ServerInterface.NoticeInfo.m_noticeItems)
			{
				if (current != null && !ServerInterface.NoticeInfo.IsCheckedForMenuIcon(current) && ServerInterface.NoticeInfo.IsOnTime(current))
				{
					num++;
				}
			}
		}
		if (num == 0)
		{
			if (this.m_badgeObj != null && this.m_badgeObj.activeSelf)
			{
				this.m_badgeObj.SetActive(false);
			}
		}
		else
		{
			if (this.m_badgeObj != null && !this.m_badgeObj.activeSelf)
			{
				this.m_badgeObj.SetActive(true);
			}
			if (this.m_volumeLabel != null)
			{
				this.m_volumeLabel.text = num.ToString();
			}
		}
	}

	public void OnUpdateInformationDisplay()
	{
		if (!this.m_initFlag)
		{
			this.Initialize();
		}
		this.UpdateInfoIcon();
	}

	public void OnUpdateSaveDataDisplay()
	{
		if (!this.m_initFlag)
		{
			this.Initialize();
		}
		if (EventManager.Instance != null && EventManager.Instance.IsInEvent())
		{
			GeneralUtil.SetEventBanner(this.m_mileageObj, "event_banner");
		}
		this.UpdateInfoIcon();
	}
}
