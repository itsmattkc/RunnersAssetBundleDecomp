using System;
using UnityEngine;

public class HudButtonRoulette : MonoBehaviour
{
	private const string OBJ_PATH = "Anchor_8_BC/Btn_roulette";

	private GameObject m_free_badge;

	private UILabel m_free_label;

	private GameObject m_chao_free_badge;

	private UILabel m_chao_free_label;

	private GameObject m_egg_obj;

	private GameObject m_sale_obj;

	private GameObject m_event_obj;

	private UITexture m_banner;

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
			GameObject gameObject = mainMenuUIObject.transform.FindChild("Anchor_8_BC/Btn_roulette").gameObject;
			if (gameObject != null)
			{
				this.m_free_badge = gameObject.transform.FindChild("badge_spin").gameObject;
				if (this.m_free_badge != null)
				{
					GameObject gameObject2 = this.m_free_badge.transform.FindChild("Lbl_roulette_volume").gameObject;
					if (gameObject2 != null)
					{
						this.m_free_label = gameObject2.GetComponent<UILabel>();
					}
				}
				this.m_chao_free_badge = gameObject.transform.FindChild("badge_p_spin").gameObject;
				if (this.m_chao_free_badge != null)
				{
					GameObject gameObject3 = this.m_chao_free_badge.transform.FindChild("Lbl_roulette_p_volume").gameObject;
					if (gameObject3 != null)
					{
						this.m_chao_free_label = gameObject3.GetComponent<UILabel>();
					}
				}
				this.m_egg_obj = gameObject.transform.FindChild("badge_egg").gameObject;
				this.m_sale_obj = gameObject.transform.FindChild("badge_alert").gameObject;
				this.m_event_obj = gameObject.transform.FindChild("event_icon").gameObject;
				GameObject gameObject4 = gameObject.transform.FindChild("img_ad_tex").gameObject;
				if (gameObject4 != null)
				{
					this.m_banner = gameObject4.GetComponent<UITexture>();
				}
			}
		}
		this.m_initEnd = true;
	}

	public void OnUpdateSaveDataDisplay()
	{
		this.Initialize();
		HudRouletteButtonUtil.SetSpecialEggIcon(this.m_egg_obj);
		bool counterStop = true;
		HudRouletteButtonUtil.SetChaoFreeSpin(this.m_chao_free_badge, this.m_chao_free_label, counterStop);
		HudRouletteButtonUtil.SetFreeSpin(this.m_free_badge, this.m_free_label, counterStop);
		HudRouletteButtonUtil.SetSaleIcon(this.m_sale_obj);
		HudRouletteButtonUtil.SetEventIcon(this.m_event_obj);
		if (this.m_banner != null)
		{
			RouletteInformationManager instance = RouletteInformationManager.Instance;
			if (instance != null)
			{
				instance.LoadInfoBaner(new RouletteInformationManager.InfoBannerRequest(this.m_banner), RouletteCategory.PREMIUM);
			}
		}
	}
}
