using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class ItemWindow : MonoBehaviour
{
	public delegate void ItemBuyCallback(ItemType itemType);

	private sealed class _SetupUIRectItemStorage_c__Iterator33 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _PC;

		internal object _current;

		internal ItemWindow __f__this;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				if (this.__f__this.m_itemObject != null)
				{
					this.__f__this.m_itemObject.SetActive(true);
					this._current = null;
					this._PC = 1;
					return true;
				}
				break;
			case 1u:
				this.__f__this.m_itemObject.SetActive(false);
				break;
			case 2u:
				this._PC = -1;
				return false;
			default:
				return false;
			}
			this._current = null;
			this._PC = 2;
			return true;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private GameObject m_instantItemObject;

	private GameObject m_itemObject;

	private ItemType m_itemType;

	private ItemWindow.ItemBuyCallback m_callback;

	private int m_FreeCount;

	public void SetItemBuyCallback(ItemWindow.ItemBuyCallback callback)
	{
		this.m_callback = callback;
	}

	public void SetWindowActive()
	{
		if (this.m_instantItemObject != null && this.m_instantItemObject.activeSelf)
		{
			this.m_instantItemObject.SetActive(false);
		}
		if (this.m_itemObject != null && !this.m_itemObject.activeSelf)
		{
			this.m_itemObject.SetActive(true);
		}
	}

	public void SetItemType(ItemType itemType)
	{
		this.m_itemType = itemType;
		this.m_FreeCount = ItemSetUtility.GetFreeItemNum(this.m_itemType);
		this.UpdateView();
	}

	public void UpdateView()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_itemObject, "row_0");
		if (gameObject != null)
		{
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_item_icon");
			if (uISprite != null)
			{
				UISprite arg_4F_0 = uISprite;
				string arg_4A_0 = "ui_cmn_icon_item_";
				int itemType = (int)this.m_itemType;
				arg_4F_0.spriteName = arg_4A_0 + itemType.ToString();
			}
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject.gameObject, "item_stock");
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(gameObject.gameObject, "img_use_ring_bg");
			GameObject gameObject4 = GameObjectUtil.FindChildGameObject(gameObject.gameObject, "img_free_bg");
			int itemNum = ItemSetUtility.GetItemNum(this.m_itemType);
			if (gameObject2 != null && gameObject3 != null && gameObject4 != null)
			{
				if (this.m_FreeCount > 0)
				{
					gameObject4.SetActive(true);
					gameObject2.SetActive(false);
					gameObject3.SetActive(false);
					this.UpdateFreeCount(this.m_FreeCount);
				}
				else if (itemNum > 0)
				{
					gameObject4.SetActive(false);
					gameObject2.SetActive(true);
					gameObject3.SetActive(false);
					this.UpdateItemCount();
				}
				else
				{
					gameObject4.SetActive(false);
					gameObject2.SetActive(false);
					gameObject3.SetActive(true);
					this.UpdateCampaignView();
				}
			}
			UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_item_name");
			if (uILabel != null)
			{
				string cellName = "name" + ((int)(this.m_itemType + 1)).ToString();
				string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ShopItem", cellName).text;
				uILabel.text = text;
				UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(uILabel.gameObject, "Lbl_item_name_sdw");
				if (uILabel2 != null)
				{
					uILabel2.text = text;
				}
			}
		}
		UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_itemObject, "Lbl_item_info");
		if (uILabel3 != null)
		{
			AbilityType abilityType = StageItemManager.s_dicItemTypeToCharAbilityType[this.m_itemType];
			SaveDataManager instance = SaveDataManager.Instance;
			CharaType mainChara = instance.PlayerData.MainChara;
			CharaAbility charaAbility = instance.CharaData.AbilityArray[(int)mainChara];
			int num = (int)(((ulong)abilityType >= (ulong)((long)charaAbility.Ability.Length)) ? 0u : charaAbility.Ability[(int)abilityType]);
			float itemTimeFromChara = StageItemManager.GetItemTimeFromChara(this.m_itemType);
			string cellName2 = "details" + ((int)(this.m_itemType + 1)).ToString();
			TextObject text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ShopItem", cellName2);
			text2.ReplaceTag("{LEVEL}", num.ToString());
			text2.ReplaceTag("{TIME}", itemTimeFromChara.ToString("0.0"));
			uILabel3.text = text2.text;
		}
	}

	private void UpdateCampaignView()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_itemObject, "row_0");
		if (gameObject == null)
		{
			return;
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject, "Lbl_ring_number");
		if (uILabel != null)
		{
			string oneItemCostString = ItemSetUtility.GetOneItemCostString(this.m_itemType);
			uILabel.text = oneItemCostString;
		}
		GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "img_sale_icon");
		if (gameObject2 != null)
		{
			ServerItem serverItem = new ServerItem(this.m_itemType);
			ServerCampaignData campaignDataInSession = ItemSetUtility.GetCampaignDataInSession((int)serverItem.id);
			bool active = false;
			if (this.m_FreeCount == 0 && campaignDataInSession != null)
			{
				active = true;
			}
			gameObject2.SetActive(active);
		}
	}

	private void UpdateItemCount()
	{
		int itemNum = ItemSetUtility.GetItemNum(this.m_itemType);
		GameObject parent = GameObjectUtil.FindChildGameObject(this.m_itemObject, "row_0");
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_number");
		if (uILabel != null)
		{
			uILabel.text = itemNum.ToString();
			UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_number_sdw");
			if (uILabel2 != null)
			{
				uILabel2.text = itemNum.ToString();
			}
		}
	}

	private void UpdateFreeCount(int value)
	{
		GameObject parent = GameObjectUtil.FindChildGameObject(this.m_itemObject, "row_0");
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(parent, "Lbl_free_number");
		if (uILabel != null)
		{
			uILabel.text = value.ToString();
		}
	}

	public void SetEquipMark(bool isEquip)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_itemObject, "img_cursor");
		if (gameObject != null)
		{
			gameObject.SetActive(isEquip);
		}
	}

	public void SetEquipMarkColor(ItemButton.CursorColor cursorColor)
	{
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_itemObject, "img_cursor");
		UISprite arg_25_0 = uISprite;
		string arg_20_0 = "ui_itemset_2_cursor_";
		int num = (int)cursorColor;
		arg_25_0.spriteName = arg_20_0 + num.ToString();
	}

	private void Start()
	{
		this.m_instantItemObject = GameObjectUtil.FindChildGameObject(base.gameObject, "boost_info_pla");
		this.m_itemObject = GameObjectUtil.FindChildGameObject(base.gameObject, "item_info_pla");
		base.StartCoroutine(this.SetupUIRectItemStorage());
	}

	private void Update()
	{
		this.UpdateCampaignView();
	}

	private IEnumerator SetupUIRectItemStorage()
	{
		ItemWindow._SetupUIRectItemStorage_c__Iterator33 _SetupUIRectItemStorage_c__Iterator = new ItemWindow._SetupUIRectItemStorage_c__Iterator33();
		_SetupUIRectItemStorage_c__Iterator.__f__this = this;
		return _SetupUIRectItemStorage_c__Iterator;
	}

	private void BuyCompleteCallback(ItemType itemType)
	{
		this.UpdateItemCount();
		this.m_callback(itemType);
	}

	private void BuyCancelledCallback(ItemType itemType)
	{
	}
}
