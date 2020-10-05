using SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InstantItemSet : MonoBehaviour
{
	private InstantItemButton[] m_instantButtons = new InstantItemButton[3];

	private InstantItemWindow m_window;

	private BoostItemType m_itemType;

	public List<BoostItemType> GetCheckedItemType()
	{
		List<BoostItemType> list = new List<BoostItemType>();
		for (int i = 0; i < 3; i++)
		{
			InstantItemButton instantItemButton = this.m_instantButtons[i];
			if (!(instantItemButton == null))
			{
				if (instantItemButton.IsChecked())
				{
					list.Add((BoostItemType)i);
				}
			}
		}
		return list;
	}

	public void Setup()
	{
		GameObject itemSetRootObject = ItemSetUtility.GetItemSetRootObject();
		this.m_window = GameObjectUtil.FindChildGameObjectComponent<InstantItemWindow>(itemSetRootObject, "info_pla");
		for (int i = 0; i < 3; i++)
		{
			InstantItemButton instantItemButton = GameObjectUtil.FindChildGameObjectComponent<InstantItemButton>(base.gameObject, ItemSetUtility.ButtonObjectName[i]);
			if (!(instantItemButton == null))
			{
				instantItemButton.Setup((BoostItemType)i, new InstantItemButton.ClickCallback(this.OnClickInstantButton));
				this.m_instantButtons[i] = instantItemButton;
			}
		}
		this.m_itemType = BoostItemType.SCORE_BONUS;
		this.m_window.SetWindowActive();
		this.m_window.SetInstantItemType(this.m_itemType);
		this.m_window.SetCheckMark(false);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		if (this.m_window)
		{
			this.m_window.SetInstantItemType(this.m_itemType);
		}
	}

	public void ResetCheckMark()
	{
		StageInfo stageInfo = GameObjectUtil.FindGameObjectComponent<StageInfo>("StageInfo");
		if (stageInfo != null)
		{
			int num = stageInfo.BoostItemValid.Length;
			for (int i = 0; i < num; i++)
			{
				stageInfo.BoostItemValid[i] = false;
			}
		}
		if (this.m_window != null)
		{
			this.m_window.SetCheckMark(false);
		}
		for (int j = 0; j < 3; j++)
		{
			if (this.m_instantButtons[j] != null)
			{
				this.m_instantButtons[j].ResetCheckMark();
			}
		}
	}

	public void SetupBoostedItem()
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			PlayerData playerData = instance.PlayerData;
			if (playerData != null)
			{
				for (int i = 0; i < 3; i++)
				{
					bool isChecked = playerData.BoostedItem[i];
					if (!(this.m_instantButtons[i] == null))
					{
						this.m_instantButtons[i].SetupBoostedItemButton(isChecked);
					}
				}
			}
		}
	}

	public void UpdateFreeItemList(ServerFreeItemState freeItemState)
	{
		List<ServerItemState> itemList = freeItemState.itemList;
		InstantItemButton[] instantButtons = this.m_instantButtons;
		for (int i = 0; i < instantButtons.Length; i++)
		{
			InstantItemButton instantItemButton = instantButtons[i];
			if (!(instantItemButton == null))
			{
				for (int j = 0; j < itemList.Count; j++)
				{
					if (instantItemButton.boostItemType == itemList[j].GetItem().boostItemType)
					{
						instantItemButton.UpdateFreeItemCount(itemList[j].m_num);
					}
				}
			}
		}
	}

	private void OnClickInstantButton(BoostItemType itemType, bool isChecked)
	{
		if (this.m_window == null)
		{
			return;
		}
		this.m_window.SetWindowActive();
		this.m_window.SetInstantItemType(itemType);
		this.m_window.SetCheckMark(isChecked);
		this.m_itemType = itemType;
		StageInfo stageInfo = GameObjectUtil.FindGameObjectComponent<StageInfo>("StageInfo");
		if (stageInfo != null)
		{
			stageInfo.BoostItemValid[(int)itemType] = isChecked;
		}
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			PlayerData playerData = instance.PlayerData;
			if (playerData != null)
			{
				playerData.BoostedItem[(int)itemType] = isChecked;
			}
		}
	}
}
