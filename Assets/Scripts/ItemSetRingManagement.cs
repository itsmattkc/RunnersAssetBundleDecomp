using System;
using UnityEngine;

public class ItemSetRingManagement : MonoBehaviour
{
	private int m_offset;

	public void AddOffset(int offset)
	{
		this.m_offset += offset;
		if (this.m_offset > 0)
		{
			this.m_offset = 0;
			return;
		}
		this.UpdateRingCount();
	}

	public void UpdateRingCount()
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			instance.ItemData.RingCountOffset = this.m_offset;
		}
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	public bool IsEnablePurchase(int itemCost)
	{
		int displayRingCount = this.GetDisplayRingCount();
		return itemCost <= displayRingCount;
	}

	public int GetDisplayRingCount()
	{
		int result = 0;
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			result = instance.ItemData.DisplayRingCount;
		}
		return result;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
