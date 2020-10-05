using System;
using UnityEngine;

public class InstantItemButton : MonoBehaviour
{
	public delegate void ClickCallback(BoostItemType itemType, bool isChecked);

	private bool m_isChecked;

	private UIToggle m_uiToggle;

	private BoostItemType m_itemType;

	private ItemSetRingManagement m_ringManagement;

	private InstantItemButton.ClickCallback m_callback;

	private bool m_isEnableCheck = true;

	private bool m_isTutorialEnd;

	private int m_freeItemCount;

	public BoostItemType boostItemType
	{
		get
		{
			return this.m_itemType;
		}
	}

	public bool itemLock
	{
		get
		{
			bool flag = false;
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null)
			{
				flag = (this.m_itemType == BoostItemType.SUB_CHARACTER && instance.PlayerData.SubChara == CharaType.UNKNOWN);
			}
			bool flag2 = false;
			if (StageModeManager.Instance != null)
			{
				flag2 = StageModeManager.Instance.IsQuickMode();
			}
			if (!flag && this.m_itemType != BoostItemType.SUB_CHARACTER)
			{
				switch (HudMenuUtility.itemSelectMode)
				{
				case HudMenuUtility.ITEM_SELECT_MODE.NORMAL:
					if (MileageMapUtility.IsBossStage() && !flag2 && (this.m_itemType == BoostItemType.SCORE_BONUS || this.m_itemType == BoostItemType.ASSIST_TRAMPOLINE))
					{
						flag = true;
					}
					break;
				case HudMenuUtility.ITEM_SELECT_MODE.EVENT_STAGE:
					if (this.m_itemType == BoostItemType.SCORE_BONUS)
					{
						flag = true;
					}
					break;
				case HudMenuUtility.ITEM_SELECT_MODE.EVENT_BOSS:
					if (!flag2 && (this.m_itemType == BoostItemType.SCORE_BONUS || this.m_itemType == BoostItemType.ASSIST_TRAMPOLINE))
					{
						flag = true;
					}
					break;
				}
			}
			return flag;
		}
	}

	public bool IsChecked()
	{
		return this.m_isChecked;
	}

	public void Setup(BoostItemType itemType, InstantItemButton.ClickCallback callback)
	{
		this.m_itemType = itemType;
		this.m_callback = callback;
		string name = "Btn_toggle_" + ((int)(itemType + 1)).ToString();
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, name);
		if (gameObject == null)
		{
			return;
		}
		this.m_uiToggle = gameObject.GetComponent<UIToggle>();
		UIButtonMessage uIButtonMessage = gameObject.GetComponent<UIButtonMessage>();
		if (uIButtonMessage == null)
		{
			uIButtonMessage = gameObject.AddComponent<UIButtonMessage>();
		}
		uIButtonMessage.target = base.gameObject;
		uIButtonMessage.functionName = "OnClickButton";
		this.m_ringManagement = ItemSetUtility.GetItemSetRingManagement();
		this.OnEnable();
	}

	public void ResetCheckMark()
	{
		if (this.m_isChecked)
		{
			if (!this.IsFreeItem() && this.m_ringManagement != null)
			{
				int instantItemCost = ItemSetUtility.GetInstantItemCost(this.m_itemType);
				this.m_ringManagement.AddOffset(instantItemCost);
			}
			if (this.m_uiToggle != null)
			{
				this.m_uiToggle.value = false;
			}
			this.m_isChecked = false;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		this.UpdateCampaignView();
	}

	private void OnEnable()
	{
		this.m_isEnableCheck = !this.itemLock;
		this.m_isTutorialEnd = true;
		string name = "Btn_toggle_" + ((int)(this.m_itemType + 1)).ToString();
		UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(base.gameObject, name);
		if (uIImageButton)
		{
			if (this.m_isEnableCheck)
			{
				uIImageButton.isEnabled = true;
			}
			else
			{
				uIImageButton.isEnabled = false;
			}
		}
		this.UpdateCampaignView();
	}

	private bool IsFreeItem()
	{
		bool result = false;
		if (this.m_isTutorialEnd && this.m_freeItemCount > 0)
		{
			result = true;
		}
		return result;
	}

	public void UpdateFreeItemCount(int count)
	{
		this.m_freeItemCount = count;
		this.UpdateCampaignView();
	}

	private void UpdateCampaignView()
	{
		int num = (int)(this.m_itemType + 1);
		string name = "Lbl_ring_number_" + num.ToString();
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, name);
		if (uILabel != null)
		{
			uILabel.text = ItemSetUtility.GetInstantItemCostString(this.m_itemType);
		}
		bool active = false;
		bool flag = this.IsFreeItem();
		ServerItem[] serverItemTable = ServerItem.GetServerItemTable(ServerItem.IdType.BOOST_ITEM);
		int id = (int)serverItemTable[(int)this.m_itemType].id;
		ServerCampaignData campaignDataInSession = ItemSetUtility.GetCampaignDataInSession(id);
		if (campaignDataInSession != null)
		{
			active = true;
		}
		string name2 = "img_free_icon_" + num.ToString();
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, name2);
		if (gameObject != null)
		{
			gameObject.SetActive(flag);
		}
		if (flag)
		{
			active = false;
		}
		string name3 = "img_sale_icon_" + num.ToString();
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, name3);
		if (gameObject2 != null)
		{
			gameObject2.SetActive(active);
		}
	}

	private void OnClickButton()
	{
		this.m_isChecked = !this.m_isChecked;
		global::Debug.Log("InstantItemButton:OnClickButton   this button >>" + this.m_itemType.ToString());
		bool flag = this.IsFreeItem();
		bool flag2 = true;
		int instantItemCost = ItemSetUtility.GetInstantItemCost(this.m_itemType);
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance == null)
		{
			return;
		}
		if (!this.m_isEnableCheck)
		{
			this.m_isChecked = false;
			if (this.m_uiToggle)
			{
				this.m_uiToggle.value = false;
				flag2 = false;
			}
		}
		else if (!flag)
		{
			if (this.m_isChecked)
			{
				if (this.m_ringManagement != null)
				{
					this.m_ringManagement.AddOffset(-instantItemCost);
				}
			}
			else if (this.m_ringManagement != null)
			{
				this.m_ringManagement.AddOffset(instantItemCost);
			}
		}
		string cueName = string.Empty;
		string spriteName = string.Empty;
		if (this.m_isChecked)
		{
			cueName = "sys_menu_decide";
			spriteName = "ui_itemset_3_bost_1";
		}
		else if (flag2)
		{
			cueName = "sys_cancel";
			spriteName = "ui_itemset_3_bost_0";
		}
		else if (!this.m_isEnableCheck)
		{
			cueName = "sys_error";
			spriteName = "ui_itemset_3_bost_4";
		}
		else
		{
			cueName = "sys_error";
			spriteName = "ui_itemset_3_bost_0";
		}
		SoundManager.SePlay(cueName, "SE");
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_bg");
		if (uISprite != null)
		{
			uISprite.spriteName = spriteName;
		}
		if (this.m_callback != null)
		{
			this.m_callback(this.m_itemType, this.m_isChecked);
		}
	}

	public void SetupBoostedItemButton(bool isChecked)
	{
		if (!this.m_isEnableCheck)
		{
			return;
		}
		this.m_isChecked = isChecked;
		bool flag = this.IsFreeItem();
		string spriteName = string.Empty;
		this.m_uiToggle.value = isChecked;
		if (this.m_isChecked)
		{
			if (this.m_isEnableCheck)
			{
				if (!flag)
				{
					int instantItemCost = ItemSetUtility.GetInstantItemCost(this.m_itemType);
					this.m_ringManagement.AddOffset(-instantItemCost);
				}
				spriteName = "ui_itemset_3_bost_1";
			}
			else
			{
				this.m_isChecked = false;
				this.m_uiToggle.value = false;
				spriteName = "ui_itemset_3_bost_4";
			}
		}
		else if (!this.m_isEnableCheck)
		{
			spriteName = "ui_itemset_3_bost_4";
		}
		else
		{
			spriteName = "ui_itemset_3_bost_0";
		}
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_bg");
		if (uISprite != null)
		{
			uISprite.spriteName = spriteName;
		}
		if (this.m_callback != null)
		{
			this.m_callback(this.m_itemType, this.m_isChecked);
		}
	}
}
