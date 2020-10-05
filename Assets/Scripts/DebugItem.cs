using System;
using UnityEngine;

public class DebugItem : MonoBehaviour
{
	private GameObject[] m_item_object = new GameObject[8];

	private GameObject m_ring_object;

	private GameObject m_red_ring_object;

	private void Start()
	{
		this.m_item_object[(int)((UIntPtr)0)] = GameObject.Find("InvincibleCountLabel");
		this.m_item_object[(int)((UIntPtr)1)] = GameObject.Find("BarrierCountLabel");
		this.m_item_object[(int)((UIntPtr)2)] = GameObject.Find("MagnetCountLabel");
		this.m_item_object[(int)((UIntPtr)3)] = GameObject.Find("TrampolineCountLabel");
		this.m_item_object[(int)((UIntPtr)4)] = GameObject.Find("ComboCountLabel");
		this.m_item_object[(int)((UIntPtr)5)] = GameObject.Find("LaserCountLabel");
		this.m_item_object[(int)((UIntPtr)6)] = GameObject.Find("DrillCountLabel");
		this.m_item_object[(int)((UIntPtr)7)] = GameObject.Find("AsteroidCountLabel");
		this.m_ring_object = GameObject.Find("RingCountLabel");
		this.m_red_ring_object = GameObject.Find("RedRingCountLabel");
		ItemPool.Initialize();
	}

	private void Update()
	{
		for (uint num = 0u; num < 8u; num += 1u)
		{
			if (this.m_item_object[(int)((UIntPtr)num)])
			{
				uint itemCount = ItemPool.GetItemCount((ItemType)num);
				UILabel component = this.m_item_object[(int)((UIntPtr)num)].GetComponent<UILabel>();
				if (component)
				{
					component.text = itemCount.ToString();
				}
			}
		}
		if (this.m_ring_object)
		{
			uint ringCount = ItemPool.RingCount;
			UILabel component2 = this.m_ring_object.GetComponent<UILabel>();
			if (component2)
			{
				component2.text = ringCount.ToString();
			}
		}
		if (this.m_red_ring_object)
		{
			uint redRingCount = ItemPool.RedRingCount;
			UILabel component3 = this.m_red_ring_object.GetComponent<UILabel>();
			if (component3)
			{
				component3.text = redRingCount.ToString();
			}
		}
	}

	private void OnAddInvincibleCount(GameObject obj)
	{
		if (obj.name == "InvincibleAddButton")
		{
			uint itemCount = ItemPool.GetItemCount(ItemType.INVINCIBLE);
			ItemPool.SetItemCount(ItemType.INVINCIBLE, itemCount + 1u);
		}
	}

	private void OnSubInvincibleCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.INVINCIBLE);
		if (itemCount > 0u)
		{
			ItemPool.SetItemCount(ItemType.INVINCIBLE, itemCount - 1u);
		}
	}

	private void OnAddBarrierCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.BARRIER);
		ItemPool.SetItemCount(ItemType.BARRIER, itemCount + 1u);
	}

	private void OnSubBarrierCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.BARRIER);
		if (itemCount > 0u)
		{
			ItemPool.SetItemCount(ItemType.BARRIER, itemCount - 1u);
		}
	}

	private void OnAddMagnetCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.MAGNET);
		ItemPool.SetItemCount(ItemType.MAGNET, itemCount + 1u);
	}

	private void OnSubMagnetCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.MAGNET);
		if (itemCount > 0u)
		{
			ItemPool.SetItemCount(ItemType.MAGNET, itemCount - 1u);
		}
	}

	private void OnAddTrampolineCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.TRAMPOLINE);
		ItemPool.SetItemCount(ItemType.TRAMPOLINE, itemCount + 1u);
	}

	private void OnSubTrampolineCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.TRAMPOLINE);
		if (itemCount > 0u)
		{
			ItemPool.SetItemCount(ItemType.TRAMPOLINE, itemCount - 1u);
		}
	}

	private void OnAddComboCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.COMBO);
		ItemPool.SetItemCount(ItemType.COMBO, itemCount + 1u);
	}

	private void OnSubComboCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.COMBO);
		if (itemCount > 0u)
		{
			ItemPool.SetItemCount(ItemType.COMBO, itemCount - 1u);
		}
	}

	private void OnAddLaserCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.LASER);
		ItemPool.SetItemCount(ItemType.LASER, itemCount + 1u);
	}

	private void OnSubLaserCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.LASER);
		if (itemCount > 0u)
		{
			ItemPool.SetItemCount(ItemType.LASER, itemCount - 1u);
		}
	}

	private void OnAddDrillCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.DRILL);
		ItemPool.SetItemCount(ItemType.DRILL, itemCount + 1u);
	}

	private void OnSubDrillCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.DRILL);
		if (itemCount > 0u)
		{
			ItemPool.SetItemCount(ItemType.DRILL, itemCount - 1u);
		}
	}

	private void OnAddAsteroidCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.ASTEROID);
		ItemPool.SetItemCount(ItemType.ASTEROID, itemCount + 1u);
	}

	private void OnSubAsteroidCount()
	{
		uint itemCount = ItemPool.GetItemCount(ItemType.ASTEROID);
		if (itemCount > 0u)
		{
			ItemPool.SetItemCount(ItemType.ASTEROID, itemCount - 1u);
		}
	}

	private void OnAddRingCount()
	{
		uint ringCount = ItemPool.RingCount;
		ItemPool.RingCount = ringCount + 1u;
	}

	private void OnSubRingCount()
	{
		uint ringCount = ItemPool.RingCount;
		if (ringCount > 0u)
		{
			ItemPool.RingCount = ringCount - 1u;
		}
	}

	private void OnAddRedRingCount()
	{
		uint redRingCount = ItemPool.RedRingCount;
		ItemPool.RedRingCount = redRingCount + 1u;
	}

	private void OnSubRedRingCount()
	{
		uint redRingCount = ItemPool.RedRingCount;
		if (redRingCount > 0u)
		{
			ItemPool.RedRingCount = redRingCount - 1u;
		}
	}
}
