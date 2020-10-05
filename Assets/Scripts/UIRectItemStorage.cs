using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Item/UI Rect Item Storage")]
public class UIRectItemStorage : MonoBehaviour
{
	public enum ActiveType
	{
		ACTIVE,
		NOT_ACTTIVE,
		DEFAULT
	}

	public bool isPlaceVertical;

	public int maxItemCount = 8;

	public int maxRows = 4;

	public int maxColumns = 4;

	public GameObject template;

	public UIWidget background;

	public int spacing_x = 128;

	public int spacing_y = 128;

	public int padding = 10;

	private List<UIInvGameItem> mItems = new List<UIInvGameItem>();

	public UIRectItemStorage.ActiveType m_activeType = UIRectItemStorage.ActiveType.DEFAULT;

	private bool m_initCountainer;

	public List<UIInvGameItem> items
	{
		get
		{
			while (this.mItems.Count < this.maxItemCount)
			{
				this.mItems.Add(null);
			}
			return this.mItems;
		}
	}

	public UIInvGameItem GetItem(int slot)
	{
		return (slot >= this.items.Count) ? null : this.mItems[slot];
	}

	public UIInvGameItem Replace(int slot, UIInvGameItem item)
	{
		if (slot < this.maxItemCount)
		{
			UIInvGameItem result = this.items[slot];
			this.mItems[slot] = item;
			return result;
		}
		return item;
	}

	private void Start()
	{
		if (!this.m_initCountainer)
		{
			this.InitContainer();
		}
	}

	private void Place(int x, int y, int count, Bounds b)
	{
		GameObject gameObject = NGUITools.AddChild(base.gameObject, this.template);
		if (gameObject != null)
		{
			Transform transform = gameObject.transform;
			transform.localPosition = new Vector3((float)this.padding + ((float)x + 0.5f) * (float)this.spacing_x, (float)(-(float)this.padding) - ((float)y + 0.5f) * (float)this.spacing_y, 0f);
			UIRectItemStorageSlot component = gameObject.GetComponent<UIRectItemStorageSlot>();
			if (component != null)
			{
				component.storage = this;
				component.slot = count;
			}
			UIRectItemStorage.ActiveType activeType = this.m_activeType;
			if (activeType != UIRectItemStorage.ActiveType.ACTIVE)
			{
				if (activeType == UIRectItemStorage.ActiveType.NOT_ACTTIVE)
				{
					gameObject.SetActive(false);
				}
			}
			else
			{
				gameObject.SetActive(true);
			}
		}
		b.Encapsulate(new Vector3((float)this.padding * 2f + (float)((x + 1) * this.spacing_x), (float)(-(float)this.padding) * 2f - (float)((y + 1) * this.spacing_y), 0f));
		if (++count >= this.maxItemCount && this.background != null)
		{
			this.background.transform.localScale = b.size;
		}
	}

	public void Restart()
	{
		GameObject gameObject = base.gameObject;
		GameObject[] array = new GameObject[gameObject.transform.childCount];
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			array[i] = gameObject.transform.GetChild(i).gameObject;
		}
		GameObject[] array2 = array;
		for (int j = 0; j < array2.Length; j++)
		{
			GameObject gameObject2 = array2[j];
			gameObject2.transform.parent = null;
			gameObject2.SetActive(false);
			UnityEngine.Object.Destroy(gameObject2);
		}
		this.InitContainer();
	}

	private void InitContainer()
	{
		if (this.template != null)
		{
			int count = 0;
			Bounds b = default(Bounds);
			if (this.isPlaceVertical)
			{
				for (int i = 0; i < this.maxColumns; i++)
				{
					for (int j = 0; j < this.maxRows; j++)
					{
						this.Place(i, j, count, b);
					}
				}
			}
			else
			{
				for (int k = 0; k < this.maxRows; k++)
				{
					for (int l = 0; l < this.maxColumns; l++)
					{
						this.Place(l, k, count, b);
					}
				}
			}
			if (this.background != null)
			{
				this.background.transform.localScale = b.size;
			}
			this.m_initCountainer = true;
		}
	}

	public void Strip()
	{
		while (base.transform.childCount > this.maxItemCount)
		{
			GameObject gameObject = base.transform.GetChild(base.transform.childCount - 1).gameObject;
			gameObject.transform.parent = null;
			UnityEngine.Object.Destroy(gameObject);
		}
	}
}
