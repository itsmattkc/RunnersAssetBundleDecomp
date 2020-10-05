using System;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
	public enum CursorColor
	{
		NONE = -1,
		BLUE,
		RED,
		GREEN,
		NUM
	}

	public delegate void ClickCallback(ItemType itemType, bool isEquiped);

	private ItemType m_itemType;

	private GameObject m_bgObject;

	private bool m_isEquiped;

	private ItemSetRingManagement m_ringManagement;

	private ItemButton.ClickCallback m_callback;

	private bool m_isActive = true;

	private bool m_isTutorialEnd;

	private int m_freeItemCount;

	private ItemButton.CursorColor m_cursorColor = ItemButton.CursorColor.NONE;

	public ItemType itemType
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
			bool result = false;
			if (this.m_bgObject != null)
			{
				bool flag = false;
				if (StageModeManager.Instance != null)
				{
					flag = StageModeManager.Instance.IsQuickMode();
				}
				switch (HudMenuUtility.itemSelectMode)
				{
				case HudMenuUtility.ITEM_SELECT_MODE.NORMAL:
					if (MileageMapUtility.IsBossStage() && !flag && (this.itemType == ItemType.COMBO || this.itemType == ItemType.TRAMPOLINE))
					{
						result = true;
					}
					break;
				case HudMenuUtility.ITEM_SELECT_MODE.EVENT_STAGE:
					result = false;
					break;
				case HudMenuUtility.ITEM_SELECT_MODE.EVENT_BOSS:
					if (!flag && (this.itemType == ItemType.COMBO || this.itemType == ItemType.TRAMPOLINE))
					{
						result = true;
					}
					break;
				}
			}
			return result;
		}
	}

	public void Setup(ItemType itemType, GameObject bgObject)
	{
		this.m_itemType = itemType;
		this.m_bgObject = bgObject;
		this.CheckItemLock();
		this.m_ringManagement = ItemSetUtility.GetItemSetRingManagement();
		if (this.m_bgObject == null)
		{
			return;
		}
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_item");
		if (uISprite != null)
		{
			UISprite arg_68_0 = uISprite;
			string arg_63_0 = "ui_cmn_icon_item_";
			int itemType2 = (int)this.m_itemType;
			arg_68_0.spriteName = arg_63_0 + itemType2.ToString();
		}
		if (this.m_bgObject != null)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_bgObject, "Btn_toggle");
			if (gameObject != null)
			{
				UIButtonMessage uIButtonMessage = gameObject.GetComponent<UIButtonMessage>();
				if (uIButtonMessage == null)
				{
					uIButtonMessage = gameObject.AddComponent<UIButtonMessage>();
				}
				uIButtonMessage.target = base.gameObject;
				uIButtonMessage.functionName = "OnClickButton";
			}
		}
		this.OnEnable();
		this.RemoveCursor();
	}

	private void OnEnable()
	{
		this.CheckItemLock();
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "badge");
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "Lbl_ring_number");
		if (gameObject != null && gameObject2 != null)
		{
			int itemNum = ItemSetUtility.GetItemNum(this.m_itemType);
			if (itemNum > 0)
			{
				gameObject.SetActive(true);
				gameObject2.SetActive(false);
			}
			else
			{
				gameObject.SetActive(false);
				gameObject2.SetActive(true);
			}
		}
		this.UpdateItemCount();
		this.UpdateCampaignView();
		this.m_isTutorialEnd = true;
	}

	public void SetCallback(ItemButton.ClickCallback callback)
	{
		this.m_callback = callback;
	}

	public bool IsEquiped()
	{
		return this.m_isEquiped;
	}

	public void UpdateItemCount()
	{
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_item_volume");
		if (uILabel != null)
		{
			uILabel.text = ItemSetUtility.GetItemNum(this.m_itemType).ToString();
		}
	}

	public void SetButtonActive(bool isActive)
	{
		if (this.itemLock)
		{
			return;
		}
		this.m_isActive = isActive;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "disabled_filter");
		if (gameObject != null)
		{
			gameObject.SetActive(!this.m_isActive);
		}
		UIButtonScale uIButtonScale = GameObjectUtil.FindChildGameObjectComponent<UIButtonScale>(this.m_bgObject, "Btn_toggle");
		if (uIButtonScale != null)
		{
			uIButtonScale.enabled = isActive;
		}
		UIToggle uIToggle = GameObjectUtil.FindChildGameObjectComponent<UIToggle>(this.m_bgObject, "Btn_toggle");
		if (uIToggle != null)
		{
			uIToggle.enabled = this.m_isActive;
		}
	}

	public void SetCursor(ItemButton.CursorColor cursorColor)
	{
		if (this.itemLock)
		{
			return;
		}
		this.m_isEquiped = true;
		this.m_cursorColor = cursorColor;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_bgObject, "img_cursor");
		if (gameObject != null)
		{
			UISprite component = gameObject.GetComponent<UISprite>();
			if (component != null)
			{
				UISprite arg_5F_0 = component;
				string arg_5A_0 = "ui_itemset_3_cursor_";
				int num = (int)cursorColor;
				arg_5F_0.spriteName = arg_5A_0 + num.ToString();
				component.alpha = 1f;
				component.color = Color.white;
			}
			gameObject.SetActive(true);
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_bgObject, "Btn_toggle");
		if (gameObject2 != null)
		{
			UIToggle component2 = gameObject2.GetComponent<UIToggle>();
			if (component2 != null)
			{
				component2.value = true;
			}
		}
		this.UpdateButtonState();
	}

	public void RemoveCursor()
	{
		this.m_cursorColor = ItemButton.CursorColor.NONE;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_bgObject, "img_cursor");
		if (gameObject != null)
		{
			UISprite component = gameObject.GetComponent<UISprite>();
			if (component != null)
			{
				component.spriteName = string.Empty;
			}
			gameObject.SetActive(false);
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_bgObject, "Btn_toggle");
		if (gameObject2 != null)
		{
			UIToggle component2 = gameObject2.GetComponent<UIToggle>();
			if (component2 != null)
			{
				component2.value = false;
			}
		}
		this.m_isEquiped = false;
		this.UpdateButtonState();
	}

	public ItemButton.CursorColor GetCursorColor()
	{
		return this.m_cursorColor;
	}

	public bool IsButtonActive()
	{
		return this.m_isActive;
	}

	private void Start()
	{
	}

	private void Update()
	{
		this.UpdateCampaignView();
	}

	private void OnClickButton()
	{
		if (this.m_isActive)
		{
			this.m_isEquiped = !this.m_isEquiped;
		}
		string cueName = (!this.m_isEquiped) ? "sys_cancel" : "sys_menu_decide";
		SoundManager.SePlay(cueName, "SE");
		if (this.m_callback != null)
		{
			this.m_callback(this.m_itemType, this.m_isEquiped);
		}
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
		bool active = false;
		bool flag = this.IsFreeItem();
		ServerItem serverItem = new ServerItem(this.m_itemType);
		ServerCampaignData campaignDataInSession = ItemSetUtility.GetCampaignDataInSession((int)serverItem.id);
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "img_free_icon");
		if (gameObject != null)
		{
			gameObject.SetActive(flag);
		}
		if (!flag && campaignDataInSession != null)
		{
			active = true;
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "img_sale_icon");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(active);
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_ring_number");
		if (uILabel != null)
		{
			uILabel.text = ItemSetUtility.GetOneItemCostString(this.m_itemType);
		}
	}

	private void UpdateButtonState()
	{
		bool flag = this.IsFreeItem();
		int itemNum = ItemSetUtility.GetItemNum(this.m_itemType);
		if (!flag)
		{
			if (itemNum > 0)
			{
				int num;
				if (this.m_isEquiped)
				{
					num = itemNum;
				}
				else
				{
					num = itemNum;
				}
				UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_item_volume");
				if (uILabel != null)
				{
					uILabel.text = num.ToString();
				}
			}
			else
			{
				int oneItemCost = ItemSetUtility.GetOneItemCost(this.m_itemType);
				if (this.m_isEquiped)
				{
					if (this.m_ringManagement != null)
					{
						this.m_ringManagement.AddOffset(-oneItemCost);
					}
				}
				else if (this.m_ringManagement != null)
				{
					this.m_ringManagement.AddOffset(oneItemCost);
				}
			}
		}
	}

	private bool CheckItemLock()
	{
		bool itemLock = this.itemLock;
		if (this.m_bgObject != null)
		{
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_bgObject.gameObject, "img_bg");
			BoxCollider boxCollider = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(this.m_bgObject.gameObject, "Btn_toggle");
			if (itemLock)
			{
				if (uISprite != null)
				{
					uISprite.spriteName = "ui_itemset_3_bost_4";
				}
				if (boxCollider != null)
				{
					boxCollider.enabled = false;
				}
			}
			else
			{
				if (uISprite != null)
				{
					uISprite.spriteName = "ui_itemset_3_bost_or_1";
				}
				if (boxCollider != null)
				{
					boxCollider.enabled = true;
				}
			}
		}
		return itemLock;
	}
}
