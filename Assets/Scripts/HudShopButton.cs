using System;
using UnityEngine;

public class HudShopButton : MonoBehaviour
{
	private GameObject m_shoBtn;

	private bool m_forceDisable;

	private void Start()
	{
		HudMenuUtility.SetTagHudSaveItem(base.gameObject);
		base.enabled = false;
	}

	private void Initialize()
	{
		if (this.m_shoBtn != null)
		{
			return;
		}
		GameObject mainMenuCmnUIObject = HudMenuUtility.GetMainMenuCmnUIObject();
		if (mainMenuCmnUIObject != null)
		{
			this.m_shoBtn = GameObjectUtil.FindChildGameObject(mainMenuCmnUIObject, "Btn_shop");
		}
		this.SetBtn();
	}

	private void SetShopButton(bool flag)
	{
		if (this.m_shoBtn != null)
		{
			this.m_shoBtn.SetActive(flag && !this.m_forceDisable);
		}
		this.SetBtn();
	}

	public void OnEnableShopButton(bool enableFlag)
	{
		this.Initialize();
		this.SetShopButton(enableFlag);
	}

	public void OnForceDisableShopButton(bool disableFlag)
	{
		this.Initialize();
		this.m_forceDisable = disableFlag;
		this.SetShopButton(!disableFlag);
	}

	private void SetBtn()
	{
		if (this.m_shoBtn != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_shoBtn, "Btn_charge_rsring");
			if (gameObject != null)
			{
				gameObject.SetActive(ServerInterface.IsRSREnable());
			}
		}
	}
}
