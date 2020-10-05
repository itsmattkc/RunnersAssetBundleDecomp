using Message;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemSetUI : MonoBehaviour
{
	public class SaveDataInterface
	{
		public static int GetItemCount(ItemType itemType)
		{
			SaveDataManager instance = SaveDataManager.Instance;
			return (int)((!(instance != null)) ? 0u : instance.ItemData.GetItemCount(itemType));
		}

		public static ItemType GetEquipedItemType(EquippedType slot)
		{
			SaveDataManager instance = SaveDataManager.Instance;
			return (!(instance != null) || slot >= EquippedType.NUM) ? ItemType.INVINCIBLE : instance.PlayerData.EquippedItem[(int)slot];
		}

		public static void SetEquipedItemType(EquippedType slot, ItemType itemType)
		{
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null && slot < EquippedType.NUM)
			{
				instance.PlayerData.EquippedItem[(int)slot] = itemType;
				HudMenuUtility.SendMsgUpdateSaveDataDisplay();
			}
		}

		public static EquippedType GetEquipedSlot(ItemType itemType)
		{
			for (EquippedType equippedType = EquippedType.EQUIPPED_01; equippedType < EquippedType.NUM; equippedType++)
			{
				if (ItemSetUI.SaveDataInterface.GetEquipedItemType(equippedType) == itemType)
				{
					return equippedType;
				}
			}
			return EquippedType.NUM;
		}

		public static void SetEquipedItemId(EquippedType slot, int id)
		{
			ItemSetUI.SaveDataInterface.SetEquipedItemType(slot, (ItemType)id);
		}

		public static EquippedType GetEquipedSlot(int id)
		{
			return ItemSetUI.SaveDataInterface.GetEquipedSlot((ItemType)id);
		}
	}

	public class ShopItemInfo : ShopItemData
	{
		private int _count_k__BackingField;

		public int count
		{
			get;
			private set;
		}

		public void SetCount(int count)
		{
			this.count = count;
		}
	}

	private GameObject m_slot_equip;

	private GameObject m_slot_item;

	private bool m_isInitialized;

	private string m_next;

	private void Start()
	{
		this.m_slot_equip = GameObjectUtil.FindChildGameObject(base.gameObject, "slot_equip");
		this.m_slot_item = GameObjectUtil.FindChildGameObject(base.gameObject, "slot_item");
		UIRectItemStorage[] array = new UIRectItemStorage[]
		{
			this.m_slot_item.GetComponent<UIRectItemStorage>(),
			GameObjectUtil.FindChildGameObjectComponent<UIRectItemStorage>(base.gameObject, "slot_bg")
		};
		for (int i = 0; i < array.Length; i++)
		{
			UIRectItemStorage uIRectItemStorage = array[i];
			uIRectItemStorage.maxRows = (ShopItemTable.GetDataTable().Length + uIRectItemStorage.maxColumns - 1) / uIRectItemStorage.maxColumns;
			uIRectItemStorage.maxItemCount = ShopItemTable.GetDataTable().Length;
		}
		ItemSetWindowEquipUI itemSetWindowEquipUI = GameObjectUtil.FindChildGameObjectComponent<ItemSetWindowEquipUI>(base.gameObject.transform.root.gameObject, "ItemSetWindowEquipUI");
		if (itemSetWindowEquipUI != null)
		{
			itemSetWindowEquipUI.gameObject.SetActive(true);
		}
	}

	private void Update()
	{
		if (!this.m_isInitialized)
		{
			this.UpdateView();
			this.m_isInitialized = true;
		}
	}

	public void UpdateView()
	{
		for (EquippedType equippedType = EquippedType.EQUIPPED_01; equippedType < EquippedType.NUM; equippedType++)
		{
			ItemType equipedItemType = ItemSetUI.SaveDataInterface.GetEquipedItemType(equippedType);
			this.UpdateEquipedItemView(equippedType, equipedItemType);
		}
		for (int i = 0; i < ShopItemTable.GetDataTable().Length; i++)
		{
			ItemType id = (ItemType)ShopItemTable.GetShopItemDataOfIndex(i).id;
			this.UpdateExistItemView(i, id);
		}
	}

	public void UpdateEquipedItemView(EquippedType slot, ItemType itemType)
	{
		this.UpdateItemView(this.m_slot_equip, (int)slot, itemType);
	}

	public void UpdateExistItemView(int index, ItemType itemType)
	{
		this.UpdateItemView(this.m_slot_item, index, itemType);
	}

	private void UpdateItemView(GameObject parent, int index, ItemType itemType)
	{
		List<ui_item_set_cell> list = GameObjectUtil.FindChildGameObjectsComponents<ui_item_set_cell>(parent, "ui_item_set_cell(Clone)");
		if (list != null)
		{
			list[index].UpdateView(itemType, ItemSetUI.SaveDataInterface.GetItemCount(itemType));
		}
	}

	private void OnStartItemSet()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetRingExchangeList(base.gameObject);
		}
	}

	private void ServerGetRingExchangeList_Succeeded(MsgGetRingExchangeListSucceed msg)
	{
		foreach (ServerRingExchangeList current in msg.m_exchangeList)
		{
			ServerItem serverItem = new ServerItem((ServerItem.Id)current.m_itemId);
			ItemType itemType = serverItem.itemType;
			ShopItemData shopItemData = ShopItemTable.GetShopItemData((int)itemType);
			if (shopItemData != null)
			{
				shopItemData.rings = current.m_price;
			}
		}
	}

	private void OnClose(string next)
	{
		HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.REDSTAR_TO_SHOP, false);
	}
}
