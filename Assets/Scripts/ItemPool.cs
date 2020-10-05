using SaveData;
using System;

public static class ItemPool
{
	private class ItemData
	{
		public uint[] item_count = new uint[8];

		public ItemData()
		{
			uint num = 8u;
			for (uint num2 = 0u; num2 < num; num2 += 1u)
			{
				this.item_count[(int)((UIntPtr)num2)] = 0u;
			}
		}
	}

	public const uint MAX_ITEM_COUNT = 99u;

	public const uint MAX_RING_COUNT = 9999999u;

	public const uint MAX_REDRING_COUNT = 9999999u;

	private static ItemPool.ItemData m_item_data = new ItemPool.ItemData();

	private static uint m_ring_count = 0u;

	private static uint m_red_ring_count = 0u;

	public static uint RingCount
	{
		get
		{
			return ItemPool.m_ring_count;
		}
		set
		{
			ItemPool.m_ring_count = value;
			if (ItemPool.m_ring_count > 9999999u)
			{
				ItemPool.m_ring_count = 9999999u;
			}
		}
	}

	public static uint RedRingCount
	{
		get
		{
			return ItemPool.m_red_ring_count;
		}
		set
		{
			ItemPool.m_red_ring_count = value;
			if (ItemPool.m_red_ring_count > 9999999u)
			{
				ItemPool.m_red_ring_count = 9999999u;
			}
		}
	}

	public static void SetItemCount(ItemType i_item_type, uint i_count)
	{
		if (i_item_type < ItemType.NUM)
		{
			ItemPool.m_item_data.item_count[(int)((UIntPtr)i_item_type)] = i_count;
			if (ItemPool.m_item_data.item_count[(int)((UIntPtr)i_item_type)] > 99u)
			{
				ItemPool.m_item_data.item_count[(int)((UIntPtr)i_item_type)] = 99u;
			}
		}
	}

	public static uint GetItemCount(ItemType i_item_type)
	{
		if (i_item_type < ItemType.NUM)
		{
			return ItemPool.m_item_data.item_count[(int)((UIntPtr)i_item_type)];
		}
		return 0u;
	}

	public static void Initialize()
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			SaveData.ItemData itemData = instance.ItemData;
			ItemPool.RingCount = itemData.RingCount;
			ItemPool.RedRingCount = itemData.RedRingCount;
			for (uint num = 0u; num < 8u; num += 1u)
			{
				ItemType itemType = (ItemType)num;
				ItemPool.SetItemCount(itemType, itemData.GetItemCount(itemType));
			}
		}
	}

	public static void ReflctSaveData()
	{
		SaveDataManager instance = SaveDataManager.Instance;
		if (instance != null)
		{
			SaveData.ItemData itemData = instance.ItemData;
			itemData.RingCount = ItemPool.m_ring_count;
			itemData.RedRingCount = ItemPool.m_red_ring_count;
			for (uint num = 0u; num < 8u; num += 1u)
			{
				itemData.SetItemCount((ItemType)num, ItemPool.m_item_data.item_count[(int)((UIntPtr)num)]);
			}
			instance.ItemData = itemData;
			instance.SaveItemData();
		}
	}
}
