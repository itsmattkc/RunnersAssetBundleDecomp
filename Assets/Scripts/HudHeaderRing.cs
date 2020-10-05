using System;
using UnityEngine;

public class HudHeaderRing : MonoBehaviour
{
	private const string m_ring_path = "Anchor_3_TR/mainmenu_info_quantum/img_bg_stock_ring";

	private const string m_sale_path = "Anchor_3_TR/mainmenu_info_quantum/Btn_shop/img_sale_icon_ring";

	private UILabel m_ui_ring_label;

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
				GameObject gameObject = mainMenuCmnUIObject.transform.FindChild("Anchor_3_TR/mainmenu_info_quantum/img_bg_stock_ring/Lbl_stock").gameObject;
				if (gameObject != null)
				{
					this.m_ui_ring_label = gameObject.GetComponent<UILabel>();
				}
				this.m_sale_obj = mainMenuCmnUIObject.transform.FindChild("Anchor_3_TR/mainmenu_info_quantum/Btn_shop/img_sale_icon_ring").gameObject;
			}
			this.m_initEnd = true;
		}
	}

	public void OnUpdateSaveDataDisplay()
	{
		this.Initialize();
		if (this.m_ui_ring_label != null)
		{
			int num = 0;
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null)
			{
				num = instance.ItemData.DisplayRingCount;
			}
			this.m_ui_ring_label.text = HudUtility.GetFormatNumString<int>(num);
		}
		if (this.m_sale_obj != null)
		{
			bool active = HudMenuUtility.IsSale(Constants.Campaign.emType.PurchaseAddRings);
			this.m_sale_obj.SetActive(active);
		}
	}
}
