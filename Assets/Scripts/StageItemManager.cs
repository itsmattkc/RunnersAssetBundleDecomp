using App;
using Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/GameMode/Stage")]
public class StageItemManager : MonoBehaviour
{
	private class EquippedItem
	{
		public ItemType item;

		public int index;

		public bool revivedFlag;
	}

	private class ChaoAbilityOption
	{
		public float m_specifiedTime;
	}

	private enum PhantomUseType
	{
		ATTACK_PAUSE,
		DISABLE,
		ENABLE
	}

	private const float DEBUG_TIME = -1f;

	private const float VALIDATE_TIME = 3f;

	private const float PHANTOM_VALIDATE_TIME = 6f;

	private const float ALTITUDE_TRAMPOLINE_OFFSET = 6f;

	private const float INVALID_TIME_ON_PAUSED = 2f;

	private const int ITEM_COMBO_SCORE_RATE = 2;

	private const int ITEM_COMBO_SCORE_MAX_RATE = 10;

	private const int EQUIP_ITEM_MAX_COUNT = 3;

	[SerializeField]
	public bool m_debugEquipItem;

	[SerializeField]
	public ItemType[] m_debugEquipItemTypes = new ItemType[3];

	private float[] m_items_fullTime = new float[8];

	private float[] m_items_time = new float[8];

	private bool[] m_items_paused;

	private float m_chaoAblityComboUpRate = 1f;

	private float m_niths_combo_time;

	private float m_item_combo_time;

	private ItemTable m_item_table;

	private bool m_disable_equipItem;

	private bool m_nowPhantom;

	private bool m_characterChange;

	private bool m_bossStage;

	private bool m_forcePhantomInvalidate;

	private bool m_equipItemTutorial;

	public static Dictionary<ItemType, AbilityType> s_dicItemTypeToCharAbilityType = new Dictionary<ItemType, AbilityType>
	{
		{
			ItemType.INVINCIBLE,
			AbilityType.INVINCIBLE
		},
		{
			ItemType.BARRIER,
			AbilityType.NUM
		},
		{
			ItemType.MAGNET,
			AbilityType.MAGNET
		},
		{
			ItemType.TRAMPOLINE,
			AbilityType.TRAMPOLINE
		},
		{
			ItemType.COMBO,
			AbilityType.COMBO
		},
		{
			ItemType.LASER,
			AbilityType.LASER
		},
		{
			ItemType.DRILL,
			AbilityType.DRILL
		},
		{
			ItemType.ASTEROID,
			AbilityType.ASTEROID
		}
	};

	private static StageItemManager instance = null;

	private bool m_availableItem;

	private List<ItemType> m_displayEquipItems = new List<ItemType>();

	private List<StageItemManager.EquippedItem> m_equipItems = new List<StageItemManager.EquippedItem>();

	private ItemType m_busyItem = ItemType.UNKNOWN;

	private int[] m_itemChangeTable = new int[]
	{
		0,
		1,
		2,
		3,
		4,
		5,
		6,
		7
	};

	private static ItemType[] m_phantomItemTypes = new ItemType[]
	{
		ItemType.LASER,
		ItemType.DRILL,
		ItemType.ASTEROID
	};

	private static ItemType[] m_feverBossNoPauseItemTypes = new ItemType[]
	{
		ItemType.BARRIER,
		ItemType.MAGNET,
		ItemType.COMBO
	};

	private StageItemManager.PhantomUseType m_phantomUseType;

	private LevelInformation m_levelInformation;

	private PlayerInformation m_playerInformation;

	private List<ItemType> m_stockColorItems = new List<ItemType>();

	private List<ItemType> m_stockItems = new List<ItemType>();

	private bool m_activeTrampoline;

	private bool m_activeAltitudeTrampoline;

	private bool m_Altitude;

	private bool m_bossItemTutorial;

	private static ItemType[] m_cautionItemTypePriority = new ItemType[]
	{
		ItemType.LASER,
		ItemType.DRILL,
		ItemType.ASTEROID,
		ItemType.INVINCIBLE,
		ItemType.TRAMPOLINE,
		ItemType.MAGNET,
		ItemType.COMBO
	};

	public static StageItemManager Instance
	{
		get
		{
			return StageItemManager.instance;
		}
	}

	public float CautionItemTimeRate
	{
		get
		{
			ItemType[] cautionItemTypePriority = StageItemManager.m_cautionItemTypePriority;
			for (int i = 0; i < cautionItemTypePriority.Length; i++)
			{
				ItemType itemType = cautionItemTypePriority[i];
				float num = this.m_items_time[(int)itemType];
				float num2 = this.m_items_fullTime[(int)itemType];
				if (num > 0f && num2 > 0f)
				{
					return num / num2;
				}
			}
			return 0f;
		}
	}

	public float[] ItemsTime
	{
		get
		{
			return this.m_items_time;
		}
	}

	private void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
		this.m_items_fullTime = new float[8];
		this.m_items_time = new float[8];
		this.m_items_paused = new bool[8];
		this.m_item_table = new ItemTable();
		this.m_availableItem = false;
		this.m_equipItems = new List<StageItemManager.EquippedItem>();
		this.m_busyItem = ItemType.UNKNOWN;
		this.m_nowPhantom = false;
		this.m_forcePhantomInvalidate = false;
		this.m_equipItemTutorial = false;
	}

	private void OnDestroy()
	{
		if (StageItemManager.instance == this)
		{
			StageItemManager.instance = null;
		}
	}

	private void SetInstance()
	{
		if (StageItemManager.instance == null)
		{
			StageItemManager.instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void UpdateStockItem(List<ItemType> itemList)
	{
		if (itemList.Count > 0)
		{
			if (itemList[0] != ItemType.UNKNOWN)
			{
				if (!this.m_nowPhantom && (this.m_phantomUseType != StageItemManager.PhantomUseType.DISABLE || Array.IndexOf<ItemType>(StageItemManager.m_phantomItemTypes, itemList[0]) < 0) && !this.m_playerInformation.IsDead() && this.IsAskEquipItemUsed(itemList[0]))
				{
					this.AddItem(itemList[0], false, false, null);
					itemList.RemoveAt(0);
				}
			}
			else
			{
				itemList.RemoveAt(0);
			}
		}
	}

	private void Update()
	{
		if (this.m_levelInformation == null)
		{
			this.m_levelInformation = GameObjectUtil.FindGameObjectComponent<LevelInformation>("LevelInformation");
		}
		if (this.m_playerInformation == null)
		{
			this.m_playerInformation = GameObjectUtil.FindGameObjectComponent<PlayerInformation>("PlayerInformation");
		}
		if (this.m_phantomUseType == StageItemManager.PhantomUseType.ATTACK_PAUSE && this.m_levelInformation != null && this.m_levelInformation.NowBoss)
		{
			this.m_phantomUseType = StageItemManager.PhantomUseType.DISABLE;
		}
		this.CheckDisablePhantom();
		this.UpdateStockItem(this.m_stockColorItems);
		this.UpdateStockItem(this.m_stockItems);
		for (int i = 0; i < 8; i++)
		{
			if (this.m_items_time[i] > 0f && !this.m_items_paused[i])
			{
				if (i == 4)
				{
					if (this.m_niths_combo_time > Time.deltaTime)
					{
						this.m_niths_combo_time -= Time.deltaTime;
					}
					else
					{
						this.m_niths_combo_time = 0f;
					}
					if (this.m_item_combo_time > Time.deltaTime)
					{
						this.m_item_combo_time -= Time.deltaTime;
					}
					else
					{
						this.m_item_combo_time = 0f;
					}
				}
				if (this.m_items_time[i] > Time.deltaTime)
				{
					this.m_items_time[i] -= Time.deltaTime;
				}
				else
				{
					this.m_items_time[i] = 0f;
					this.TimeOutItem((ItemType)i);
				}
			}
		}
		if (this.IsActiveAltitudeTrampoline())
		{
			this.UpdateAltitudeTrampoline();
		}
	}

	private void CheckDisablePhantom()
	{
		if (this.m_disable_equipItem && this.m_playerInformation != null && this.m_playerInformation.PhantomType != PhantomType.NONE && !this.m_forcePhantomInvalidate)
		{
			ItemType phantomItemType = this.GetPhantomItemType();
			if (phantomItemType != ItemType.UNKNOWN)
			{
				this.m_items_paused[(int)phantomItemType] = true;
				MsgInvalidateItem value = new MsgInvalidateItem(this.GetPhantomItemType());
				GameObjectUtil.SendMessageToTagObjects("Player", "OnInvalidateItem", value, SendMessageOptions.DontRequireReceiver);
				GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnPauseItemOnBoss", null, SendMessageOptions.DontRequireReceiver);
				this.m_forcePhantomInvalidate = true;
			}
		}
	}

	private GameObject GetPlayerObject()
	{
		return GameObject.FindWithTag("Player");
	}

	public int GetComboScoreRate()
	{
		int num = 1;
		if (this.m_item_combo_time > 0f)
		{
			num = 2;
		}
		if (this.m_niths_combo_time > 0f)
		{
			num *= (int)this.m_chaoAblityComboUpRate;
		}
		return Mathf.Min(num, 10);
	}

	public ItemTable GetItemTable()
	{
		return this.m_item_table;
	}

	public void OnRequestItemUse(MsgAskEquipItemUsed msg)
	{
		bool flag = false;
		int num = -1;
		for (int i = 0; i < this.m_equipItems.Count; i++)
		{
			if (this.m_equipItems[i].item == msg.m_itemType)
			{
				num = i;
				if (!this.m_equipItems[i].revivedFlag && StageAbilityManager.Instance != null && StageAbilityManager.Instance.HasChaoAbility(ChaoAbility.ITEM_REVIVE))
				{
					int num2 = (int)StageAbilityManager.Instance.GetChaoAbilityValue(ChaoAbility.ITEM_REVIVE);
					if (num2 >= ObjUtil.GetRandomRange100())
					{
						flag = true;
						this.m_equipItems[i].revivedFlag = true;
					}
				}
				break;
			}
		}
		if (num >= 0)
		{
			if (this.m_equipItems[num].item == ItemType.UNKNOWN)
			{
				this.m_equipItems.RemoveRange(num, 1);
			}
			else
			{
				msg.m_ok = this.IsAskEquipItemUsed(this.m_equipItems[num].item);
				if (msg.m_ok)
				{
					this.AddItem(this.m_equipItems[num].item, true, flag, null);
					if (flag)
					{
						ObjUtil.RequestStartAbilityToChao(ChaoAbility.ITEM_REVIVE, false);
					}
					else
					{
						this.m_equipItems.RemoveRange(num, 1);
					}
				}
			}
		}
	}

	public void OnAddItem(MsgAddItemToManager msg)
	{
		if (!this.IsAskEquipItemUsed(msg.m_itemType))
		{
			return;
		}
		this.AddItem(msg.m_itemType, false, false, null);
	}

	public void OnAddEquipItem()
	{
		if (this.m_playerInformation != null)
		{
			if (this.m_playerInformation.IsDead())
			{
				return;
			}
			if (this.m_playerInformation.IsNowLastChance())
			{
				return;
			}
		}
		if (this.m_equipItems.Count < 3)
		{
			List<ItemType> list = new List<ItemType>();
			for (int i = 0; i < 8; i++)
			{
				bool flag = false;
				for (int j = 0; j < this.m_equipItems.Count; j++)
				{
					if (i == (int)this.m_equipItems[j].item)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add((ItemType)i);
				}
			}
			if (list.Count > 1)
			{
				ItemType item = list[UnityEngine.Random.Range(0, list.Count)];
				StageItemManager.EquippedItem equippedItem = new StageItemManager.EquippedItem();
				equippedItem.item = item;
				equippedItem.index = this.m_equipItems.Count;
				this.m_equipItems.Add(equippedItem);
				ItemType[] array = new ItemType[this.m_equipItems.Count];
				for (int k = 0; k < this.m_equipItems.Count; k++)
				{
					array[k] = this.m_equipItems[k].item;
				}
				MsgSetEquippedItem msgSetEquippedItem = new MsgSetEquippedItem(array);
				bool flag2 = false;
				if (Array.IndexOf<ItemType>(StageItemManager.m_phantomItemTypes, this.m_busyItem) >= 0)
				{
					flag2 = true;
				}
				msgSetEquippedItem.m_enabled = (!this.m_disable_equipItem && !flag2);
				GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnSetPresentEquippedItem", msgSetEquippedItem, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public void OnChangeItem()
	{
		int count = this.m_equipItems.Count;
		if (count > 0)
		{
			this.m_displayEquipItems.Clear();
			App.Random.ShuffleInt(this.m_itemChangeTable);
			for (int i = 0; i < count; i++)
			{
				this.m_equipItems[i].item = (ItemType)this.m_itemChangeTable[i];
				this.m_displayEquipItems.Add(this.m_equipItems[i].item);
			}
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnChangeItem", new MsgSetEquippedItem(this.m_displayEquipItems.ToArray()), SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnAddColorItem(MsgAddItemToManager msg)
	{
		if (this.IsAskEquipItemUsed(msg.m_itemType))
		{
			this.AddItem(msg.m_itemType, false, false, null);
		}
		else
		{
			ItemType phantomItemType = this.GetPhantomItemType();
			if (phantomItemType == msg.m_itemType)
			{
				float itemTimeFromChara = StageItemManager.GetItemTimeFromChara(phantomItemType);
				this.m_items_time[(int)phantomItemType] = Mathf.Max(this.m_items_time[(int)phantomItemType], itemTimeFromChara);
				this.m_items_fullTime[(int)phantomItemType] = this.m_items_time[(int)phantomItemType];
				this.CountdownMeter();
			}
			else if (this.IsAskItemStock(msg.m_itemType))
			{
				this.m_stockColorItems.Add(msg.m_itemType);
			}
		}
	}

	public void OnAddDamageTrampoline()
	{
		if (this.IsAskEquipItemUsed(ItemType.TRAMPOLINE))
		{
			this.AddItem(ItemType.TRAMPOLINE, false, false, null);
		}
		else if (this.m_items_time[3] > 0f)
		{
			float itemTimeFromChara = StageItemManager.GetItemTimeFromChara(ItemType.TRAMPOLINE);
			this.m_items_time[3] = Mathf.Max(this.m_items_time[3], itemTimeFromChara);
			this.m_items_fullTime[3] = this.m_items_time[3];
			this.CountdownMeter();
		}
		else if (this.IsAskItemStock(ItemType.TRAMPOLINE))
		{
			this.m_stockItems.Add(ItemType.TRAMPOLINE);
		}
	}

	public bool IsEquipItem()
	{
		return this.m_equipItems.Count > 0;
	}

	public bool IsActiveTrampoline()
	{
		return this.m_activeTrampoline;
	}

	private bool IsActiveAltitudeTrampoline()
	{
		return this.m_activeAltitudeTrampoline;
	}

	public void SetActiveAltitudeTrampoline(bool on)
	{
		this.m_activeAltitudeTrampoline = on;
	}

	public static float GetItemTimeFromChara(ItemType itemType)
	{
		StageAbilityManager stageAbilityManager = StageAbilityManager.Instance;
		switch (itemType)
		{
		case ItemType.INVINCIBLE:
		case ItemType.MAGNET:
		case ItemType.TRAMPOLINE:
		case ItemType.COMBO:
			return (!(stageAbilityManager != null)) ? 3f : stageAbilityManager.GetItemTimePlusAblityBonus(itemType);
		case ItemType.BARRIER:
			return 0f;
		case ItemType.LASER:
		case ItemType.DRILL:
		case ItemType.ASTEROID:
			return (!(stageAbilityManager != null)) ? 6f : stageAbilityManager.GetItemTimePlusAblityBonus(itemType);
		default:
			return 0f;
		}
	}

	public void SetEquipItemTutorial(bool equipItemTutorial)
	{
		this.m_equipItemTutorial = equipItemTutorial;
	}

	public void SetEquippedItem(ItemType[] items)
	{
		if (items == null)
		{
			return;
		}
		if (items.Length == 0)
		{
			return;
		}
		for (int i = 0; i < items.Length; i++)
		{
			this.m_displayEquipItems.Add(items[i]);
		}
		GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnSetEquippedItem", new MsgSetEquippedItem(this.m_displayEquipItems.ToArray()), SendMessageOptions.DontRequireReceiver);
	}

	private void AddItem(ItemType itemType, bool equipped, bool revive = false, StageItemManager.ChaoAbilityOption option = null)
	{
		if (option != null)
		{
			if (itemType == ItemType.MAGNET)
			{
				float num = Mathf.Max(this.m_items_time[(int)itemType], 0f);
				this.m_items_time[(int)itemType] = num + option.m_specifiedTime;
			}
			else if (itemType == ItemType.COMBO)
			{
				this.m_niths_combo_time = option.m_specifiedTime;
				this.m_items_time[(int)itemType] = Mathf.Max(this.m_item_combo_time, this.m_niths_combo_time);
			}
		}
		else if (itemType == ItemType.COMBO)
		{
			this.m_item_combo_time = StageItemManager.GetItemTimeFromChara(itemType);
			this.m_items_time[(int)itemType] = Mathf.Max(this.m_item_combo_time, this.m_niths_combo_time);
		}
		else
		{
			float itemTimeFromChara = StageItemManager.GetItemTimeFromChara(itemType);
			this.m_items_time[(int)itemType] = Mathf.Max(this.m_items_time[(int)itemType], itemTimeFromChara);
		}
		this.m_items_paused[(int)itemType] = false;
		switch (itemType)
		{
		case ItemType.INVINCIBLE:
		case ItemType.MAGNET:
		case ItemType.LASER:
		case ItemType.DRILL:
		case ItemType.ASTEROID:
		{
			GameObject playerObject = this.GetPlayerObject();
			if (playerObject != null)
			{
				MsgUseItem value = new MsgUseItem(itemType, -1f);
				playerObject.SendMessage("OnUseItem", value, SendMessageOptions.DontRequireReceiver);
			}
			break;
		}
		case ItemType.BARRIER:
		{
			GameObject playerObject2 = this.GetPlayerObject();
			if (playerObject2 != null)
			{
				MsgUseItem value2 = new MsgUseItem(itemType, -1f);
				playerObject2.SendMessage("OnUseItem", value2, SendMessageOptions.DontRequireReceiver);
			}
			break;
		}
		case ItemType.TRAMPOLINE:
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Gimmick");
			MsgUseItem value3 = new MsgUseItem(itemType);
			GameObject[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				GameObject gameObject = array2[i];
				gameObject.SendMessage("OnUseItem", value3, SendMessageOptions.DontRequireReceiver);
			}
			GameObject playerObject3 = this.GetPlayerObject();
			if (playerObject3 != null)
			{
				MsgUseItem value4 = new MsgUseItem(itemType, -1f);
				playerObject3.SendMessage("OnUseItem", value4, SendMessageOptions.DontRequireReceiver);
			}
			ObjUtil.RequestStartAbilityToChao(ChaoAbility.TRAMPOLINE_TIME, true);
			ObjUtil.RequestStartAbilityToChao(ChaoAbility.ITEM_TIME, true);
			this.m_activeTrampoline = true;
			break;
		}
		case ItemType.COMBO:
		{
			GameObject playerObject4 = this.GetPlayerObject();
			if (playerObject4 != null)
			{
				MsgUseItem value5 = new MsgUseItem(itemType, -1f);
				playerObject4.SendMessage("OnUseItem", value5, SendMessageOptions.DontRequireReceiver);
			}
			break;
		}
		}
		if (equipped && !revive)
		{
			StageItemManager.SendUsedItemMessageToCockpit(itemType);
		}
		if (itemType == ItemType.BARRIER)
		{
			itemType = ItemType.UNKNOWN;
		}
		if (itemType < ItemType.NUM)
		{
			this.m_items_fullTime[(int)itemType] = this.m_items_time[(int)itemType];
		}
		this.SetupBusyItem();
		HudTutorial.SendItemTutorial(itemType);
	}

	private void SetupBusyItem()
	{
		ItemType busyItem = ItemType.UNKNOWN;
		ItemType[] cautionItemTypePriority = StageItemManager.m_cautionItemTypePriority;
		for (int i = 0; i < cautionItemTypePriority.Length; i++)
		{
			ItemType itemType = cautionItemTypePriority[i];
			float num = this.m_items_time[(int)itemType];
			float num2 = this.m_items_fullTime[(int)itemType];
			if (num > 0f && num2 > 0f)
			{
				busyItem = itemType;
				break;
			}
		}
		this.m_busyItem = busyItem;
		this.CountdownMeter();
	}

	private void CountdownMeter()
	{
		MsgCaution caution = new MsgCaution(HudCaution.Type.COUNTDOWN, this.CautionItemTimeRate);
		HudCaution.Instance.SetCaution(caution);
	}

	private void TimeOutItem(ItemType itemType)
	{
		switch (itemType)
		{
		case ItemType.INVINCIBLE:
		case ItemType.MAGNET:
		case ItemType.COMBO:
		case ItemType.LASER:
		case ItemType.DRILL:
		case ItemType.ASTEROID:
		{
			GameObject playerObject = this.GetPlayerObject();
			if (playerObject != null)
			{
				MsgInvalidateItem value = new MsgInvalidateItem(itemType);
				playerObject.SendMessage("OnInvalidateItem", value, SendMessageOptions.DontRequireReceiver);
			}
			break;
		}
		case ItemType.TRAMPOLINE:
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Gimmick");
			MsgInvalidateItem value2 = new MsgInvalidateItem(itemType);
			GameObject[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				GameObject gameObject = array2[i];
				gameObject.SendMessage("OnInvalidateItem", value2, SendMessageOptions.DontRequireReceiver);
			}
			GameObject playerObject2 = this.GetPlayerObject();
			if (playerObject2 != null)
			{
				playerObject2.SendMessage("OnInvalidateItem", value2, SendMessageOptions.DontRequireReceiver);
			}
			this.m_activeTrampoline = false;
			break;
		}
		}
		this.InvalidateItem(itemType);
	}

	private void ResetItemTime(ItemType itemType)
	{
		this.m_items_time[(int)itemType] = 0f;
		if (itemType == ItemType.TRAMPOLINE)
		{
			this.m_activeTrampoline = false;
		}
	}

	private void OnInvalidateItem(MsgInvalidateItem msg)
	{
		if (this.m_items_time[(int)msg.m_itemType] > 0f)
		{
			this.ResetItemTime(msg.m_itemType);
			this.m_items_paused[(int)msg.m_itemType] = false;
			this.InvalidateItem(msg.m_itemType);
		}
	}

	public void OnTerminateItem(MsgTerminateItem msg)
	{
		if (this.m_items_paused[(int)msg.m_itemType])
		{
			return;
		}
		if (this.m_items_time[(int)msg.m_itemType] > 0f)
		{
			this.ResetItemTime(msg.m_itemType);
			this.InvalidateItem(msg.m_itemType);
		}
	}

	public void OnPauseItemOnBoss(MsgPauseItemOnBoss msg)
	{
		this.m_disable_equipItem = true;
		this.m_forcePhantomInvalidate = false;
		for (int i = 0; i < 8; i++)
		{
			ItemType itemType = (ItemType)i;
			if (!this.IsBossNoPauseItem(itemType))
			{
				float num = this.m_items_time[i];
				if (num > 0f)
				{
					if (this.m_items_time[i] < 2f)
					{
						this.ResetItemTime(itemType);
						this.InvalidateItem(itemType);
						MsgInvalidateItem value = new MsgInvalidateItem(itemType);
						GameObjectUtil.SendMessageToTagObjects("Player", "OnInvalidateItem", value, SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						this.m_items_paused[i] = true;
						MsgInvalidateItem value2 = new MsgInvalidateItem(itemType);
						GameObjectUtil.SendMessageToTagObjects("Player", "OnInvalidateItem", value2, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}
		GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnPauseItemOnBoss", null, SendMessageOptions.DontRequireReceiver);
	}

	public void OnPauseItemOnChangeLevel(MsgPauseItemOnChageLevel msg)
	{
		for (int i = 0; i < 8; i++)
		{
			ItemType itemType = (ItemType)i;
			if (this.IsBossNoPauseItem(itemType))
			{
				float num = this.m_items_time[i];
				if (num > 0f)
				{
					if (this.m_items_time[i] < 2f)
					{
						this.ResetItemTime(itemType);
						this.InvalidateItem(itemType);
						MsgInvalidateItem value = new MsgInvalidateItem(itemType);
						GameObjectUtil.SendMessageToTagObjects("Player", "OnInvalidateItem", value, SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						this.m_items_paused[i] = true;
						MsgInvalidateItem value2 = new MsgInvalidateItem(itemType);
						GameObjectUtil.SendMessageToTagObjects("Player", "OnInvalidateItem", value2, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
		}
	}

	public void OnResumeItemOnBoss(MsgPhatomItemOnBoss msg)
	{
		bool flag = false;
		for (int i = 0; i < 8; i++)
		{
			ItemType itemType = (ItemType)i;
			float num = this.m_items_time[i];
			if (num > 0f && this.m_items_paused[i])
			{
				if (itemType == ItemType.ASTEROID || itemType == ItemType.LASER || itemType == ItemType.DRILL)
				{
					flag = true;
				}
				this.m_items_paused[i] = false;
				MsgUseItem value = new MsgUseItem(itemType, -1f);
				GameObjectUtil.SendMessageToTagObjects("Player", "OnUseItem", value, SendMessageOptions.DontRequireReceiver);
			}
		}
		this.m_disable_equipItem = false;
		this.m_phantomUseType = StageItemManager.PhantomUseType.ATTACK_PAUSE;
		GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnResumeItemOnBoss", flag, SendMessageOptions.DontRequireReceiver);
	}

	public void OnDisableEquipItem(MsgDisableEquipItem msg)
	{
		this.m_disable_equipItem = msg.m_disable;
	}

	private bool IsBossNoPauseItem(ItemType itemType)
	{
		return Array.IndexOf<ItemType>(StageItemManager.m_feverBossNoPauseItemTypes, itemType) >= 0;
	}

	private void InvalidateItem(ItemType itemType)
	{
		bool flag = false;
		foreach (StageItemManager.EquippedItem current in this.m_equipItems)
		{
			if (current.item == itemType)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			StageItemManager.SendUsedItemMessageToCockpit(itemType);
		}
		this.SetupBusyItem();
	}

	public void OnUseEquipItem(MsgUseEquipItem msg)
	{
		if (this.m_availableItem)
		{
			return;
		}
		this.m_availableItem = true;
		this.m_equipItems.Clear();
		if (this.m_displayEquipItems.Count == 0)
		{
			return;
		}
		foreach (ItemType current in this.m_displayEquipItems)
		{
			if (current != ItemType.UNKNOWN)
			{
				StageItemManager.EquippedItem equippedItem = new StageItemManager.EquippedItem();
				equippedItem.item = current;
				this.m_equipItems.Add(equippedItem);
			}
		}
		if (this.m_equipItems.Count == 0)
		{
			return;
		}
		int num = 0;
		foreach (StageItemManager.EquippedItem current2 in this.m_equipItems)
		{
			current2.index = num;
			num++;
		}
		if (this.m_equipItemTutorial)
		{
			this.SendItemBtnTutorial();
		}
		if (this.m_levelInformation != null && !this.m_levelInformation.NowBoss)
		{
			MsgItemButtonEnable value = new MsgItemButtonEnable(true);
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnItemEnable", value, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void SendItemBtnTutorial()
	{
		GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnMsgTutorialItemButton", new MsgTutorialItemButton(), SendMessageOptions.DontRequireReceiver);
		GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnStartTutorial", null, SendMessageOptions.DontRequireReceiver);
	}

	public void OnMsgBossCheckState(MsgBossCheckState msg)
	{
		bool flag = msg.IsAttackOK();
		this.m_phantomUseType = ((!flag) ? StageItemManager.PhantomUseType.DISABLE : StageItemManager.PhantomUseType.ENABLE);
		if (this.m_levelInformation != null && this.m_levelInformation.NowBoss)
		{
			MsgItemButtonEnable value = new MsgItemButtonEnable(flag);
			GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnItemEnable", value, SendMessageOptions.DontRequireReceiver);
		}
		if (this.m_bossItemTutorial && flag)
		{
			this.m_bossItemTutorial = false;
			this.SendItemBtnTutorial();
		}
	}

	private static void SendUsedItemMessageToCockpit(ItemType itemType)
	{
		GameObjectUtil.SendMessageFindGameObject("HudCockpit", "OnUsedItem", new MsgUsedItem(itemType), SendMessageOptions.DontRequireReceiver);
	}

	public void OnAskEquipItemUsed(MsgAskEquipItemUsed msg)
	{
		msg.m_ok = this.IsAskEquipItemUsed(msg.m_itemType);
	}

	public ItemType GetPhantomItemType()
	{
		if (this.m_items_time[5] > 0f)
		{
			return ItemType.LASER;
		}
		if (this.m_items_time[6] > 0f)
		{
			return ItemType.DRILL;
		}
		if (this.m_items_time[7] > 0f)
		{
			return ItemType.ASTEROID;
		}
		return ItemType.UNKNOWN;
	}

	private bool IsAskEquipItemUsed(ItemType itemType)
	{
		if (this.m_playerInformation != null)
		{
			if (this.m_playerInformation.IsDead())
			{
				return false;
			}
			if (this.m_playerInformation.IsNowLastChance())
			{
				return false;
			}
		}
		if (this.m_characterChange)
		{
			return false;
		}
		if (!this.m_availableItem)
		{
			return false;
		}
		if (itemType >= ItemType.NUM)
		{
			return false;
		}
		if (this.m_items_paused[(int)itemType])
		{
			return false;
		}
		if (this.m_disable_equipItem && !this.IsBossNoPauseItem(itemType))
		{
			return false;
		}
		bool flag = false;
		if (Array.IndexOf<ItemType>(StageItemManager.m_phantomItemTypes, this.m_busyItem) >= 0)
		{
			flag = true;
		}
		switch (itemType)
		{
		case ItemType.INVINCIBLE:
			if (flag)
			{
				return false;
			}
			break;
		case ItemType.LASER:
		case ItemType.DRILL:
		case ItemType.ASTEROID:
			if (flag && this.m_busyItem != itemType)
			{
				return false;
			}
			if (this.m_phantomUseType == StageItemManager.PhantomUseType.DISABLE)
			{
				return false;
			}
			break;
		}
		return true;
	}

	private bool IsAskItemStock(ItemType itemType)
	{
		if (this.m_playerInformation != null)
		{
			if (this.m_playerInformation.IsDead())
			{
				return false;
			}
			if (this.m_playerInformation.IsNowLastChance())
			{
				return false;
			}
		}
		return !this.m_characterChange && this.m_availableItem && itemType < ItemType.NUM;
	}

	private void UpdateAltitudeTrampoline()
	{
		if (this.m_playerInformation != null)
		{
			float y = this.m_playerInformation.SideViewPathPos.y;
			float y2 = this.m_playerInformation.Position.y;
			if (y + 6f < y2)
			{
				this.m_Altitude = true;
			}
			else
			{
				if (this.m_Altitude)
				{
					GameObject[] array = GameObject.FindGameObjectsWithTag("Gimmick");
					GameObject[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						GameObject gameObject = array2[i];
						MsgUseItem value = new MsgUseItem(ItemType.TRAMPOLINE);
						gameObject.SendMessage("OnUseItem", value, SendMessageOptions.DontRequireReceiver);
					}
				}
				this.m_Altitude = false;
			}
		}
	}

	public void OnTransformPhantom(MsgTransformPhantom msg)
	{
		this.m_nowPhantom = true;
	}

	public void OnReturnFromPhantom(MsgReturnFromPhantom msg)
	{
		this.m_nowPhantom = false;
	}

	public void OnChangeCharaStart(MsgChangeCharaSucceed msg)
	{
		this.m_characterChange = true;
	}

	private void OnChangeCharaSucceed(MsgChangeCharaSucceed msg)
	{
		this.m_characterChange = false;
	}

	private void OnChaoAbilityLoopComboUp()
	{
		StageAbilityManager stageAbilityManager = StageAbilityManager.Instance;
		if (stageAbilityManager != null && this.IsAskEquipItemUsed(ItemType.COMBO))
		{
			float num = stageAbilityManager.GetChaoAbilityValue(ChaoAbility.LOOP_COMBO_UP);
			num = Mathf.Max(num, 1f);
			StageItemManager.ChaoAbilityOption chaoAbilityOption = new StageItemManager.ChaoAbilityOption();
			chaoAbilityOption.m_specifiedTime = num;
			this.m_chaoAblityComboUpRate = stageAbilityManager.GetChaoAbilityExtraValue(ChaoAbility.LOOP_COMBO_UP, ChaoType.MAIN);
			this.m_chaoAblityComboUpRate += stageAbilityManager.GetChaoAbilityExtraValue(ChaoAbility.LOOP_COMBO_UP, ChaoType.SUB);
			this.AddItem(ItemType.COMBO, false, false, chaoAbilityOption);
		}
	}

	private void OnChaoAbilityLoopMagnet()
	{
		StageAbilityManager stageAbilityManager = StageAbilityManager.Instance;
		if (stageAbilityManager != null && this.IsAskEquipItemUsed(ItemType.MAGNET))
		{
			float num = stageAbilityManager.GetChaoAbilityValue(ChaoAbility.LOOP_MAGNET);
			num = Mathf.Max(num, 1f);
			this.AddItem(ItemType.MAGNET, false, false, new StageItemManager.ChaoAbilityOption
			{
				m_specifiedTime = num
			});
		}
	}

	private void OnMsgExitStage(MsgExitStage msg)
	{
		base.enabled = false;
	}

	public static ItemType GetRandomPhantomItem()
	{
		int num = UnityEngine.Random.Range(0, StageItemManager.m_phantomItemTypes.Length);
		return StageItemManager.m_phantomItemTypes[num];
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLog(string s)
	{
		global::Debug.Log("@ms " + s);
	}

	[Conditional("DEBUG_INFO")]
	private static void DebugLogWarning(string s)
	{
		global::Debug.LogWarning("@ms " + s);
	}
}
