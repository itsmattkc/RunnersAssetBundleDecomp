using SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSet : MonoBehaviour
{
	private List<ItemButton> m_buttons = new List<ItemButton>();

	private ItemWindow m_window;

	private bool[] m_enableColor = new bool[3];

	private ItemType[] m_itemType = new ItemType[3];

	private void Awake()
	{
		for (int i = 0; i < this.m_itemType.Length; i++)
		{
			this.m_itemType[i] = ItemType.UNKNOWN;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		if (this.m_window != null)
		{
			if (StageAbilityManager.Instance != null)
			{
				StageAbilityManager.Instance.RecalcAbilityVaue();
			}
			this.m_window.SetItemType(ItemType.INVINCIBLE);
		}
	}

	private void OnDestroy()
	{
		StageInfo stageInfo = GameObjectUtil.FindGameObjectComponent<StageInfo>("StageInfo");
		if (stageInfo != null)
		{
			for (int i = 0; i < 3; i++)
			{
				stageInfo.EquippedItems[i] = this.m_itemType[i];
			}
		}
	}

	public void Setup()
	{
		this.SetupItem();
	}

	public void ResetCheckMark()
	{
		for (int i = 0; i < 3; i++)
		{
			this.m_itemType[i] = ItemType.UNKNOWN;
			this.m_enableColor[i] = true;
		}
		foreach (ItemButton current in this.m_buttons)
		{
			if (!(current == null))
			{
				ItemButton.CursorColor cursorColor = current.GetCursorColor();
				if (cursorColor != ItemButton.CursorColor.NONE)
				{
					current.RemoveCursor();
				}
				current.SetButtonActive(true);
			}
		}
		if (this.m_window != null)
		{
			this.m_window.SetWindowActive();
			this.m_window.SetEquipMark(false);
		}
	}

	public void SetupEquipItem()
	{
		if (this.m_window != null)
		{
			this.m_window.SetItemType(ItemType.INVINCIBLE);
			this.m_window.SetWindowActive();
		}
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			PlayerData playerData = instance.PlayerData;
			if (playerData != null)
			{
				for (int i = 0; i < 3; i++)
				{
					ItemType item = playerData.EquippedItem[i];
					this.SetupEquipItemOne(i, item);
				}
			}
		}
	}

	private void SetupEquipItemOne(int equipIndex, ItemType item)
	{
		if (item == ItemType.UNKNOWN)
		{
			return;
		}
		if (this.m_window != null && !this.m_buttons[(int)item].itemLock)
		{
			this.m_window.SetWindowActive();
			this.m_window.SetEquipMark(true);
			this.m_window.SetItemType(item);
		}
		this.m_buttons[(int)item].SetCursor((ItemButton.CursorColor)equipIndex);
		if (this.m_buttons[(int)item].IsEquiped())
		{
			if (this.m_window != null)
			{
				this.m_window.SetEquipMarkColor((ItemButton.CursorColor)equipIndex);
			}
			this.m_itemType[equipIndex] = item;
			this.m_enableColor[equipIndex] = false;
		}
		this.SetButtonActive();
	}

	public void UpdateView()
	{
		if (this.m_window != null)
		{
			this.m_window.UpdateView();
		}
	}

	public void UpdateFreeItemList(ServerFreeItemState freeItemState)
	{
		List<ServerItemState> itemList = freeItemState.itemList;
		foreach (ItemButton current in this.m_buttons)
		{
			if (!(current == null))
			{
				for (int i = 0; i < itemList.Count; i++)
				{
					if (current.itemType == itemList[i].GetItem().itemType)
					{
						current.UpdateFreeItemCount(itemList[i].m_num);
					}
				}
			}
		}
	}

	private void SetupItem()
	{
		for (int i = 0; i < 3; i++)
		{
			this.m_itemType[i] = ItemType.UNKNOWN;
			this.m_enableColor[i] = true;
		}
		GameObject itemSetRootObject = ItemSetUtility.GetItemSetRootObject();
		this.m_window = GameObjectUtil.FindChildGameObjectComponent<ItemWindow>(itemSetRootObject, "info_pla");
		this.m_window.SetItemBuyCallback(new ItemWindow.ItemBuyCallback(this.ItemBuyCallback));
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "slot_bg");
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(base.gameObject, "slot_item");
		if (gameObject != null && gameObject2 != null)
		{
			int childCount = gameObject.transform.childCount;
			int childCount2 = gameObject2.transform.childCount;
			if (childCount == childCount2)
			{
				int num = 8;
				this.m_buttons.Clear();
				for (int j = 0; j < num; j++)
				{
					Transform child = gameObject2.transform.GetChild(j);
					if (!(child == null))
					{
						GameObject gameObject3 = child.gameObject;
						if (!(gameObject3 == null))
						{
							ItemButton itemButton = gameObject3.GetComponent<ItemButton>();
							if (itemButton == null)
							{
								itemButton = gameObject3.AddComponent<ItemButton>();
							}
							itemButton.Setup((ItemType)j, gameObject.transform.GetChild(j).gameObject);
							itemButton.SetCallback(new ItemButton.ClickCallback(this.ClickButtonCallback));
							this.m_buttons.Add(itemButton);
						}
					}
				}
				int num2 = childCount;
				if (num < num2)
				{
					for (int k = num; k < num2; k++)
					{
						Transform child2 = gameObject2.transform.GetChild(k);
						if (child2 != null)
						{
							GameObject gameObject4 = child2.gameObject;
							if (gameObject4 != null)
							{
								gameObject4.SetActive(false);
							}
						}
						Transform child3 = gameObject.transform.GetChild(k);
						if (child3 != null)
						{
							GameObject gameObject5 = child3.gameObject;
							if (gameObject5 != null)
							{
								gameObject5.SetActive(false);
							}
						}
					}
				}
			}
		}
	}

	private void ClickButtonCallback(ItemType itemType, bool isEquiped)
	{
		if (this.m_window != null)
		{
			this.m_window.SetWindowActive();
			this.m_window.SetEquipMark(isEquiped);
			this.m_window.SetItemType(itemType);
		}
		if (isEquiped)
		{
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				if (this.m_enableColor[i])
				{
					num = i;
					this.m_enableColor[i] = false;
					break;
				}
			}
			ItemButton.CursorColor cursorColor = (ItemButton.CursorColor)num;
			this.m_buttons[(int)itemType].SetCursor(cursorColor);
			if (this.m_window != null)
			{
				this.m_window.SetEquipMarkColor(cursorColor);
			}
			this.m_itemType[num] = itemType;
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null)
			{
				PlayerData playerData = instance.PlayerData;
				if (playerData != null)
				{
					playerData.EquippedItem[num] = itemType;
				}
			}
		}
		else
		{
			ItemButton.CursorColor cursorColor2 = this.m_buttons[(int)itemType].GetCursorColor();
			if (cursorColor2 != ItemButton.CursorColor.NONE)
			{
				this.m_enableColor[(int)cursorColor2] = true;
				this.m_itemType[(int)cursorColor2] = ItemType.UNKNOWN;
				this.m_buttons[(int)itemType].RemoveCursor();
				SaveDataManager instance2 = SaveDataManager.Instance;
				if (instance2 != null)
				{
					PlayerData playerData2 = instance2.PlayerData;
					if (playerData2 != null)
					{
						playerData2.EquippedItem[(int)cursorColor2] = ItemType.UNKNOWN;
					}
				}
			}
		}
		this.SetButtonActive();
	}

	private void SetButtonActive()
	{
		int num = 0;
		foreach (ItemButton current in this.m_buttons)
		{
			if (!(current == null))
			{
				if (current.IsEquiped())
				{
					num++;
				}
			}
		}
		if (num >= 3)
		{
			foreach (ItemButton current2 in this.m_buttons)
			{
				if (!(current2 == null))
				{
					if (!current2.IsEquiped())
					{
						current2.SetButtonActive(false);
					}
				}
			}
		}
		else
		{
			foreach (ItemButton current3 in this.m_buttons)
			{
				if (!(current3 == null))
				{
					current3.SetButtonActive(true);
				}
			}
		}
	}

	private void ItemBuyCallback(ItemType itemType)
	{
		ItemButton itemButton = this.m_buttons[(int)itemType];
		if (itemButton == null)
		{
			return;
		}
		itemButton.UpdateItemCount();
	}

	public ItemType[] GetItem()
	{
		return this.m_itemType;
	}
}
