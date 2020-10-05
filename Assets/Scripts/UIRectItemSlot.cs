using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

public abstract class UIRectItemSlot : MonoBehaviour
{
	public UISprite icon;

	public UIWidget background;

	public UILabel label;

	public AudioClip grabSound;

	public AudioClip placeSound;

	public AudioClip errorSound;

	private UIInvGameItem mItem;

	private string mText = string.Empty;

	private static UIInvGameItem mDraggedItem;

	protected abstract UIInvGameItem observedItem
	{
		get;
	}

	protected abstract UIInvGameItem Replace(UIInvGameItem item);

	private void OnTooltip(bool show)
	{
		UIInvGameItem uIInvGameItem = (!show) ? null : this.mItem;
		if (uIInvGameItem != null)
		{
			UIInvBaseItem baseItem = uIInvGameItem.baseItem;
			if (baseItem != null)
			{
				string text = string.Concat(new string[]
				{
					"[",
					NGUITools.EncodeColor(uIInvGameItem.color),
					"]",
					uIInvGameItem.name,
					"[-]\n"
				});
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"[AFAFAF]Level ",
					uIInvGameItem.itemLevel,
					" ",
					baseItem.slot
				});
				List<UIInvStat> list = uIInvGameItem.CalculateStats();
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					UIInvStat uIInvStat = list[i];
					if (uIInvStat.amount != 0)
					{
						if (uIInvStat.amount < 0)
						{
							text = text + "\n[FF0000]" + uIInvStat.amount;
						}
						else
						{
							text = text + "\n[00FF00]+" + uIInvStat.amount;
						}
						if (uIInvStat.modifier == UIInvStat.Modifier.Percent)
						{
							text += "%";
						}
						text = text + " " + uIInvStat.id;
						text += "[-]";
					}
					i++;
				}
				if (!string.IsNullOrEmpty(baseItem.description))
				{
					text = text + "\n[FF9900]" + baseItem.description;
				}
				UITooltip.ShowText(text);
				return;
			}
		}
		UITooltip.ShowText(null);
	}

	private void OnClick()
	{
		if (UIRectItemSlot.mDraggedItem != null)
		{
			this.OnDrop(null);
		}
		else if (this.mItem != null)
		{
			UIRectItemSlot.mDraggedItem = this.Replace(null);
			if (UIRectItemSlot.mDraggedItem != null)
			{
				NGUITools.PlaySound(this.grabSound);
			}
			this.UpdateCursor();
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (UIRectItemSlot.mDraggedItem == null && this.mItem != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UIRectItemSlot.mDraggedItem = this.Replace(null);
			NGUITools.PlaySound(this.grabSound);
			this.UpdateCursor();
		}
	}

	private void OnDrop(GameObject go)
	{
		UIInvGameItem uIInvGameItem = this.Replace(UIRectItemSlot.mDraggedItem);
		if (UIRectItemSlot.mDraggedItem == uIInvGameItem)
		{
			NGUITools.PlaySound(this.errorSound);
		}
		else if (uIInvGameItem != null)
		{
			NGUITools.PlaySound(this.grabSound);
		}
		else
		{
			NGUITools.PlaySound(this.placeSound);
		}
		UIRectItemSlot.mDraggedItem = uIInvGameItem;
		this.UpdateCursor();
	}

	private void UpdateCursor()
	{
		if (UIRectItemSlot.mDraggedItem != null && UIRectItemSlot.mDraggedItem.baseItem != null)
		{
			UI.UICursor.Set(UIRectItemSlot.mDraggedItem.baseItem.iconAtlas, UIRectItemSlot.mDraggedItem.baseItem.iconName);
		}
		else
		{
			UI.UICursor.Clear();
		}
	}

	private void Update()
	{
		UIInvGameItem observedItem = this.observedItem;
		if (this.mItem != observedItem)
		{
			this.mItem = observedItem;
			UIInvBaseItem uIInvBaseItem = (observedItem == null) ? null : observedItem.baseItem;
			if (this.label != null)
			{
				string text = (observedItem == null) ? null : observedItem.name;
				if (string.IsNullOrEmpty(this.mText))
				{
					this.mText = this.label.text;
				}
				this.label.text = ((text == null) ? this.mText : text);
			}
			if (this.icon != null)
			{
				if (uIInvBaseItem == null || uIInvBaseItem.iconAtlas == null)
				{
					this.icon.enabled = false;
				}
				else
				{
					this.icon.atlas = uIInvBaseItem.iconAtlas;
					this.icon.spriteName = uIInvBaseItem.iconName;
					this.icon.enabled = true;
					this.icon.MakePixelPerfect();
				}
			}
			if (this.background != null)
			{
				this.background.color = ((observedItem == null) ? Color.white : observedItem.color);
			}
		}
	}
}
