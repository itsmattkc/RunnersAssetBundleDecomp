using System;
using UnityEngine;

public class HudHeaderChallenge : MonoBehaviour
{
	private enum LabelType
	{
		CHALLENGE_COUNT,
		TIME_COUNT,
		OVER_CHALLENGE,
		NUM
	}

	private string[] m_labelName = new string[]
	{
		"Lbl_challenge",
		"Lbl_time",
		"Lbl_over_challenge"
	};

	private GameObject[] m_ui_obj = new GameObject[3];

	private UILabel[] m_ui_label = new UILabel[3];

	private EnergyStorage m_energy_storage;

	private float m_time;

	private bool m_fill_up_flag;

	private bool m_initEnd;

	private bool m_calledInit;

	private GameObject m_sale_obj;

	private void Start()
	{
		HudMenuUtility.SetTagHudSaveItem(base.gameObject);
	}

	private void Update()
	{
		if (this.m_initEnd)
		{
			this.UpdateTimeCountDisplay();
		}
		else if (this.m_calledInit)
		{
			this.m_time -= Time.deltaTime;
			if (this.m_time < 0f)
			{
				this.Initialize();
			}
		}
	}

	private void Initialize()
	{
		if (this.m_initEnd)
		{
			return;
		}
		if (this.m_energy_storage == null)
		{
			GameObject gameObject = GameObject.Find("EnergyStorage");
			if (gameObject != null)
			{
				this.m_energy_storage = gameObject.GetComponent<EnergyStorage>();
				if (this.m_energy_storage != null)
				{
					this.m_fill_up_flag = this.m_energy_storage.IsFillUpCount();
				}
			}
		}
		GameObject mainMenuCmnUIObject = HudMenuUtility.GetMainMenuCmnUIObject();
		if (mainMenuCmnUIObject != null)
		{
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(mainMenuCmnUIObject, "Anchor_3_TR");
			if (gameObject2 != null)
			{
				for (int i = 0; i < 3; i++)
				{
					if (this.m_ui_obj[i] == null)
					{
						this.m_ui_obj[i] = GameObjectUtil.FindChildGameObject(gameObject2, this.m_labelName[i]);
						if (this.m_ui_obj[i] != null)
						{
							this.m_ui_label[i] = this.m_ui_obj[i].GetComponent<UILabel>();
						}
					}
				}
				if (this.m_sale_obj == null)
				{
					this.m_sale_obj = GameObjectUtil.FindChildGameObject(gameObject2, "img_sale_icon_challenge");
				}
			}
		}
		this.m_initEnd = true;
		for (int j = 0; j < 3; j++)
		{
			if (this.m_ui_obj[j] == null)
			{
				this.m_initEnd = false;
				break;
			}
		}
		if (this.m_sale_obj == null)
		{
			this.m_initEnd = false;
		}
		if (this.m_energy_storage == null)
		{
			this.m_initEnd = false;
		}
		this.m_calledInit = true;
		this.m_time = 1f;
	}

	private void SetChallengeCount()
	{
		uint num = 0u;
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			num = instance.PlayerData.DisplayChallengeCount;
		}
		if (this.m_ui_label[0] != null)
		{
			this.m_ui_label[0].text = num.ToString();
		}
		if (this.m_ui_label[2])
		{
			this.m_ui_label[2].text = HudUtility.GetFormatNumString<uint>(num);
		}
	}

	private void SetLabelActive()
	{
		if (this.m_ui_obj[2] != null)
		{
			this.m_ui_obj[2].SetActive(this.m_fill_up_flag);
		}
		if (this.m_ui_obj[0] != null)
		{
			this.m_ui_obj[0].SetActive(!this.m_fill_up_flag);
		}
		if (this.m_ui_obj[1] != null)
		{
			this.m_ui_obj[1].SetActive(!this.m_fill_up_flag);
		}
	}

	private void UpdateTimeCountDisplay()
	{
		if (this.m_energy_storage != null)
		{
			this.m_fill_up_flag = this.m_energy_storage.IsFillUpCount();
			if (!this.m_fill_up_flag)
			{
				TimeSpan restTimeForRenew = this.m_energy_storage.GetRestTimeForRenew();
				if (this.m_ui_label[1] != null)
				{
					int num = restTimeForRenew.Seconds;
					int num2 = restTimeForRenew.Minutes;
					if (num < 0 && num2 <= 0)
					{
						num2 = 15;
						num = 0;
					}
					this.m_ui_label[1].text = string.Format("{0}:{1}", num2, num.ToString("D2"));
				}
			}
		}
	}

	public void OnUpdateSaveDataDisplay()
	{
		this.Initialize();
		if (this.m_energy_storage != null)
		{
			this.m_fill_up_flag = this.m_energy_storage.IsFillUpCount();
		}
		this.SetChallengeCount();
		this.SetLabelActive();
		this.UpdateTimeCountDisplay();
		if (this.m_sale_obj != null)
		{
			bool active = HudMenuUtility.IsSale(Constants.Campaign.emType.PurchaseAddEnergys);
			this.m_sale_obj.SetActive(active);
		}
	}

	public void OnUpdateChallengeCountDisply()
	{
		this.OnUpdateSaveDataDisplay();
	}
}
