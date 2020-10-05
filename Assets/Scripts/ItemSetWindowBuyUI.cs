using Message;
using System;
using Text;
using UnityEngine;

public class ItemSetWindowBuyUI : MonoBehaviour
{
	private int m_id = -1;

	private int m_count;

	private ShopItemData m_shopItemData;

	private bool m_isUnsetTriggerOfPopupList;

	private int buyItemCount
	{
		get
		{
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_buy_volume");
			if (uILabel != null)
			{
				return int.Parse(uILabel.text);
			}
			return 0;
		}
	}

	private int buyNeedRingCount
	{
		get
		{
			if (this.m_shopItemData != null)
			{
				return this.m_shopItemData.rings * this.buyItemCount;
			}
			return 0;
		}
	}

	private void Start()
	{
		this.m_id = -1;
	}

	private void Update()
	{
		if (this.m_isUnsetTriggerOfPopupList)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Drop-down List");
			if (gameObject != null)
			{
				foreach (BoxCollider current in GameObjectUtil.FindChildGameObjectsComponents<BoxCollider>(gameObject, "Label"))
				{
					current.isTrigger = false;
				}
			}
			this.m_isUnsetTriggerOfPopupList = false;
		}
		if (GeneralWindow.IsCreated("ItemBuyOverError") && GeneralWindow.IsButtonPressed)
		{
			GeneralWindow.Close();
			this.OpenWindow(this.m_id, this.m_count);
		}
		if (GeneralWindow.IsCreated("ItemBuyRingError") && GeneralWindow.IsButtonPressed)
		{
			GeneralWindow.Close();
			if (GeneralWindow.IsYesButtonPressed)
			{
				GameObjectUtil.SendMessageFindGameObject("ItemSetUI", "OnClose", "ShopUI.OnOpenRing", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public void OpenWindow(int id, int count)
	{
		this.m_id = id;
		this.m_count = count;
		this.m_shopItemData = ShopItemTable.GetShopItemData(id);
		SoundManager.SePlay("sys_window_open", "SE");
		UIPopupList uIPopupList = GameObjectUtil.FindChildGameObjectComponent<UIPopupList>(base.gameObject, "Ppl_buy_volume");
		if (uIPopupList != null)
		{
			uIPopupList.value = uIPopupList.items[0];
		}
		this.UpdateView();
	}

	private void UpdateView()
	{
		if (this.m_id == -1)
		{
			return;
		}
		if (this.m_shopItemData != null)
		{
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_item_name");
			if (uILabel != null)
			{
				uILabel.text = this.m_shopItemData.name;
			}
			UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbt_item_effect");
			if (uILabel2 != null)
			{
				uILabel2.text = ItemSetWindowEquipUI.GetItemDetailsText(this.m_shopItemData);
			}
			UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_price");
			if (uILabel3 != null)
			{
				uILabel3.text = HudUtility.GetFormatNumString<int>(this.m_shopItemData.rings);
			}
		}
		UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_item_volume");
		if (uILabel4 != null)
		{
			uILabel4.text = HudUtility.GetFormatNumString<int>(this.m_count);
		}
		UILabel uILabel5 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_total_price");
		if (uILabel5 != null)
		{
			uILabel5.text = HudUtility.GetFormatNumString<int>(this.buyNeedRingCount);
		}
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_item");
		if (uISprite != null)
		{
			uISprite.spriteName = "ui_cmn_icon_item_" + this.m_id.ToString();
		}
	}

	private void OnClickBuy()
	{
		int itemCount = (int)SaveDataManager.Instance.ItemData.GetItemCount((ItemType)this.m_id);
		if ((long)itemCount >= 99L || 99L - (long)itemCount < (long)this.buyItemCount)
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "ItemBuyOverError",
				buttonType = GeneralWindow.ButtonType.Ok,
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemSet", "gw_buy_over_error_caption").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemSet", "gw_buy_over_error_text").text,
				parentGameObject = base.gameObject
			});
		}
		else if ((ulong)SaveDataManager.Instance.ItemData.RingCount < (ulong)((long)this.buyNeedRingCount))
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				name = "ItemBuyRingError",
				buttonType = GeneralWindow.ButtonType.ShopCancel,
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemSet", "gw_buy_ring__error_caption").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemSet", "gw_buy_ring__error_text").text,
				parentGameObject = base.gameObject
			});
		}
		else
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				ServerInterface arg_16B_0 = loggedInServerInterface;
				ServerItem serverItem = new ServerItem((ItemType)this.m_id);
				arg_16B_0.RequestServerRingExchange((int)serverItem.id, this.buyItemCount, base.gameObject);
			}
			else
			{
				SoundManager.SePlay("sys_buy", "SE");
				SaveDataManager.Instance.ItemData.RingCount -= (uint)this.buyNeedRingCount;
				SaveDataManager.Instance.ItemData.SetItemCount((ItemType)this.m_id, SaveDataManager.Instance.ItemData.GetItemCount((ItemType)this.m_id) + (uint)this.buyItemCount);
				ItemSetWindowBuyUI.UpdateItemSetUIView();
			}
		}
	}

	private void ServerRingExchange_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		SoundManager.SePlay("sys_buy", "SE");
		ItemSetWindowBuyUI.UpdateItemSetUIView();
	}

	public static void UpdateItemSetUIView()
	{
		ItemSetUI itemSetUI = GameObjectUtil.FindGameObjectComponent<ItemSetUI>("ItemSetUI");
		if (itemSetUI != null)
		{
			itemSetUI.UpdateView();
		}
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	private void OnClickClose()
	{
		SoundManager.SePlay("sys_window_close", "SE");
	}

	public void OnValueChangePopupList()
	{
		this.UpdateView();
	}

	public void OnClickPopupList()
	{
		this.m_isUnsetTriggerOfPopupList = true;
	}
}
