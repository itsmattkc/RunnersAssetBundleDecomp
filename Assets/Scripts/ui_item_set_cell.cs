using AnimationOrTween;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ui_item_set_cell : MonoBehaviour
{
	private int m_id;

	private int m_count;

	private static Dictionary<string, int> __f__switch_map6;

	private void Start()
	{
		base.gameObject.SetActive(false);
	}

	public void UpdateView(ItemType itemType, int count)
	{
		this.UpdateView((int)itemType, count);
	}

	public void UpdateView(int id, int count)
	{
		this.m_id = id;
		this.m_count = count;
		if (id == -1)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.gameObject.SetActive(true);
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_item_volume");
			if (uILabel != null)
			{
				uILabel.text = count.ToString();
			}
			bool flag = base.gameObject.transform.parent.name == "slot" || base.gameObject.transform.parent.name == "slot_equip";
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(base.gameObject, "img_item");
			if (uISprite != null)
			{
				uISprite.spriteName = "ui_cmn_icon_item_" + ((count != 0 || !flag) ? string.Empty : "g_") + id.ToString();
			}
		}
	}

	private void OnClick()
	{
		string name = base.gameObject.transform.parent.name;
		if (name != null)
		{
			if (ui_item_set_cell.__f__switch_map6 == null)
			{
				ui_item_set_cell.__f__switch_map6 = new Dictionary<string, int>(3)
				{
					{
						"slot",
						0
					},
					{
						"slot_equip",
						1
					},
					{
						"slot_item",
						1
					}
				};
			}
			int num;
			if (ui_item_set_cell.__f__switch_map6.TryGetValue(name, out num))
			{
				if (num != 0)
				{
					if (num == 1)
					{
						ItemSetWindowEquipUI itemSetWindowEquipUI = GameObjectUtil.FindChildGameObjectComponent<ItemSetWindowEquipUI>(base.gameObject.transform.root.gameObject, "ItemSetWindowEquipUI");
						if (itemSetWindowEquipUI != null)
						{
							itemSetWindowEquipUI.gameObject.SetActive(true);
							itemSetWindowEquipUI.OpenWindow(this.m_id, this.m_count);
						}
					}
				}
				else
				{
					SoundManager.SePlay("sys_menu_decide", "SE");
					Animation animation = GameObjectUtil.FindGameObjectComponent<Animation>("menu_Anim");
					if (animation != null)
					{
						ActiveAnimation.Play(animation, "ui_menu_item_Anim", Direction.Forward, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
					}
				}
			}
		}
	}
}
