using System;
using UnityEngine;

public class HudHeaderRedRing : MonoBehaviour
{
	private const string m_redring_path = "Anchor_3_TR/mainmenu_info_quantum/img_bg_rsring";

	private const string m_sale_path = "Anchor_3_TR/mainmenu_info_quantum/Btn_shop/img_sale_icon_rsring";

	private UILabel m_ui_red_ring_label;

	private GameObject m_sale_obj;

	private bool m_initEnd;

	private void Start()
	{
		HudMenuUtility.SetTagHudSaveItem(base.gameObject);
		base.enabled = false;
	}

	private void Initialize()
	{
		if (!this.m_initEnd)
		{
			GameObject mainMenuCmnUIObject = HudMenuUtility.GetMainMenuCmnUIObject();
			if (mainMenuCmnUIObject != null)
			{
				GameObject gameObject = mainMenuCmnUIObject.transform.FindChild("Anchor_3_TR/mainmenu_info_quantum/img_bg_rsring/Lbl_rsring").gameObject;
				if (gameObject != null)
				{
					this.m_ui_red_ring_label = gameObject.GetComponent<UILabel>();
				}
				this.m_sale_obj = mainMenuCmnUIObject.transform.FindChild("Anchor_3_TR/mainmenu_info_quantum/Btn_shop/img_sale_icon_rsring").gameObject;
			}
			this.m_initEnd = true;
		}
	}

	public void OnUpdateSaveDataDisplay()
	{
		this.Initialize();
		if (this.m_ui_red_ring_label != null)
		{
			int num = 0;
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null)
			{
				num = instance.ItemData.DisplayRedRingCount;
			}
			this.m_ui_red_ring_label.text = HudUtility.GetFormatNumString<int>(num);
		}
		if (this.m_sale_obj != null)
		{
			bool flag = HudMenuUtility.IsSale(Constants.Campaign.emType.PurchaseAddRsrings);
			bool flag2 = HudMenuUtility.IsSale(Constants.Campaign.emType.PurchaseAddRsringsNoChargeUser);
			if (flag || flag2)
			{
				this.m_sale_obj.SetActive(true);
			}
			else
			{
				this.m_sale_obj.SetActive(false);
			}
		}
	}
}
