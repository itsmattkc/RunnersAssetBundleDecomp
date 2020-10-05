using AnimationOrTween;
using Message;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class ItemSetWindowEquipUI : MonoBehaviour
{
	private int m_id;

	private int m_count;

	public void OpenWindow(int id, int count)
	{
		this.m_id = id;
		this.m_count = count;
		SoundManager.SePlay("sys_window_open", "SE");
		this.UpdateView();
		Animation animation = GameObjectUtil.FindGameObjectComponent<Animation>("item_set_window_equip");
		if (animation != null)
		{
			ActiveAnimation.Play(animation, "ui_cmn_window_Anim", Direction.Forward, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
		}
	}

	private void OnClickClose()
	{
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private static int GetMainCharaItemAbilityLevel(ItemType itemType)
	{
		AbilityType abilityType = StageItemManager.s_dicItemTypeToCharAbilityType[itemType];
		SaveDataManager instance = SaveDataManager.Instance;
		CharaType mainChara = instance.PlayerData.MainChara;
		CharaAbility charaAbility = instance.CharaData.AbilityArray[(int)mainChara];
		return (int)(((ulong)abilityType >= (ulong)((long)charaAbility.Ability.Length)) ? 0u : charaAbility.Ability[(int)abilityType]);
	}

	private void UpdateView()
	{
		ShopItemData shopItemData = ShopItemTable.GetShopItemData(this.m_id);
		if (shopItemData != null)
		{
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbt_item_name");
			if (uILabel != null)
			{
				uILabel.text = shopItemData.name;
			}
			UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbt_item_effect");
			if (uILabel2 != null)
			{
				uILabel2.text = ItemSetWindowEquipUI.GetItemDetailsText(shopItemData);
			}
		}
		ui_item_set_cell ui_item_set_cell = GameObjectUtil.FindChildGameObjectComponent<ui_item_set_cell>(base.gameObject, "ui_item_set_cell(Clone)");
		if (ui_item_set_cell != null)
		{
			ui_item_set_cell.UpdateView(this.m_id, this.m_count);
		}
		for (EquippedType equippedType = EquippedType.EQUIPPED_01; equippedType < EquippedType.NUM; equippedType++)
		{
			GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_off_" + (int)(equippedType + 1));
			if (gameObject != null)
			{
				gameObject.SetActive(ItemSetUI.SaveDataInterface.GetEquipedItemType(equippedType) == (ItemType)this.m_id);
			}
		}
	}

	private void OnClickEquipSlot0()
	{
		this.EquipItem(EquippedType.EQUIPPED_01);
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickEquipSlot1()
	{
		this.EquipItem(EquippedType.EQUIPPED_02);
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickEquipSlot2()
	{
		this.EquipItem(EquippedType.EQUIPPED_03);
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnClickUnequipSlot0()
	{
		this.UnequipItem(EquippedType.EQUIPPED_01);
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void OnClickUnequipSlot1()
	{
		this.UnequipItem(EquippedType.EQUIPPED_02);
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void OnClickUnequipSlot2()
	{
		this.UnequipItem(EquippedType.EQUIPPED_03);
		SoundManager.SePlay("sys_window_close", "SE");
	}

	private void OnClickToBuy()
	{
		ItemSetWindowBuyUI itemSetWindowBuyUI = GameObjectUtil.FindGameObjectComponent<ItemSetWindowBuyUI>("ItemSetWindowBuyUI");
		if (itemSetWindowBuyUI != null)
		{
			itemSetWindowBuyUI.OpenWindow(this.m_id, this.m_count);
			SoundManager.SePlay("sys_menu_decide", "SE");
		}
	}

	private void EquipItem(EquippedType slot)
	{
		ItemType[] equipItems = this.GetEquipItems();
		EquippedType equipedSlot = ItemSetUI.SaveDataInterface.GetEquipedSlot(this.m_id);
		if (equipedSlot < EquippedType.NUM)
		{
			equipItems[(int)equipedSlot] = ItemSetUI.SaveDataInterface.GetEquipedItemType(slot);
		}
		equipItems[(int)slot] = (ItemType)this.m_id;
		this.UpdateItems(equipItems);
	}

	private void UnequipItem(EquippedType removeSlot)
	{
		ItemType[] equipItems = this.GetEquipItems();
		equipItems[(int)removeSlot] = ItemType.UNKNOWN;
		this.UpdateItems(equipItems);
	}

	private void UpdateItems(ItemType[] equipItems)
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			List<ItemType> list = new List<ItemType>();
			for (EquippedType equippedType = EquippedType.EQUIPPED_01; equippedType < EquippedType.NUM; equippedType++)
			{
				list.Add(equipItems[(int)equippedType]);
			}
			loggedInServerInterface.RequestServerEquipItem(list, base.gameObject);
		}
		else
		{
			this.SetEquipItems(equipItems);
			this.UpdateEquipedItemView();
		}
	}

	private void ServerEquipItem_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		this.UpdateEquipedItemView();
	}

	private void UpdateEquipedItemView()
	{
		ItemSetUI itemSetUI = GameObjectUtil.FindGameObjectComponent<ItemSetUI>("ItemSetUI");
		if (itemSetUI != null)
		{
			itemSetUI.UpdateView();
		}
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	private ItemType[] GetEquipItems()
	{
		ItemType[] array = new ItemType[3];
		for (EquippedType equippedType = EquippedType.EQUIPPED_01; equippedType < EquippedType.NUM; equippedType++)
		{
			array[(int)equippedType] = ItemSetUI.SaveDataInterface.GetEquipedItemType(equippedType);
		}
		return array;
	}

	private void SetEquipItems(ItemType[] equipItems)
	{
		for (EquippedType equippedType = EquippedType.EQUIPPED_01; equippedType < EquippedType.NUM; equippedType++)
		{
			ItemSetUI.SaveDataInterface.SetEquipedItemType(equippedType, equipItems[(int)equippedType]);
		}
	}

	public static string GetItemDetailsText(ShopItemData shopItemData)
	{
		ItemType id = (ItemType)shopItemData.id;
		return TextUtility.Replaces(shopItemData.details, new Dictionary<string, string>
		{
			{
				"{LEVEL}",
				ItemSetWindowEquipUI.GetMainCharaItemAbilityLevel(id).ToString()
			},
			{
				"{TIME}",
				((int)StageItemManager.GetItemTimeFromChara(id)).ToString("0.0")
			}
		});
	}
}
