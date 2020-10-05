using System;
using UnityEngine;

[AddComponentMenu("NGUI/Item/UI Rect Item Storage Slot")]
public class UIRectItemStorageSlot : UIRectItemSlot
{
	public UIRectItemStorage storage;

	public UIRectItemStorageRanking storageRanking;

	public int slot;

	protected override UIInvGameItem observedItem
	{
		get
		{
			UIInvGameItem uIInvGameItem = (!(this.storage != null)) ? null : this.storage.GetItem(this.slot);
			if (uIInvGameItem == null)
			{
				uIInvGameItem = ((!(this.storageRanking != null)) ? null : this.storageRanking.GetItem(this.slot));
			}
			return uIInvGameItem;
		}
	}

	protected override UIInvGameItem Replace(UIInvGameItem item)
	{
		UIInvGameItem result;
		if (this.storage != null)
		{
			result = this.storage.Replace(this.slot, item);
		}
		else if (this.storageRanking != null)
		{
			result = this.storageRanking.Replace(this.slot, item);
		}
		else
		{
			result = item;
		}
		return result;
	}
}
