using System;
using System.Globalization;
using System.Xml;
using UnityEngine;

public class EventObjectTable : MonoBehaviour
{
	public const int ITEM_COUNT_MAX = 8;

	public const int TBL_COUNT_MAX = 3;

	[SerializeField]
	private TextAsset m_dataTabel;

	private static readonly EventObjectTableItem[] CRYSTAL_ITEMS = new EventObjectTableItem[]
	{
		EventObjectTableItem.CrystalA,
		EventObjectTableItem.CrystalB,
		EventObjectTableItem.CrystalC,
		EventObjectTableItem.Crystal10A,
		EventObjectTableItem.Crystal10B,
		EventObjectTableItem.Crystal10C
	};

	private static readonly string[] ITEM_NAMES = new string[]
	{
		"ObjRing",
		"ObjSuperRing",
		"ObjCrystal_A",
		"ObjCrystal_B",
		"ObjCrystal_C",
		"ObjCrystal10_A",
		"ObjCrystal10_B",
		"ObjCrystal10_C"
	};

	private int[] m_tblInfo;

	private int m_tblCount;

	private static EventObjectTable s_instance = null;

	public static EventObjectTable Instance
	{
		get
		{
			return EventObjectTable.s_instance;
		}
	}

	private void Awake()
	{
		if (EventObjectTable.s_instance == null)
		{
			EventObjectTable.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (EventObjectTable.s_instance == this)
		{
			EventObjectTable.s_instance = null;
		}
	}

	private void Start()
	{
		this.Setup();
	}

	public static string GetItemName(uint index)
	{
		if (index < 8u && (ulong)index < (ulong)((long)EventObjectTable.ITEM_NAMES.Length))
		{
			return EventObjectTable.ITEM_NAMES[(int)((UIntPtr)index)];
		}
		return string.Empty;
	}

	public static bool IsEventCrystalBig(EventObjectTableItem index)
	{
		switch (index)
		{
		case EventObjectTableItem.CrystalA:
		case EventObjectTableItem.CrystalB:
		case EventObjectTableItem.CrystalC:
			return false;
		default:
			return true;
		}
	}

	private void Setup()
	{
		if (this.m_tblInfo == null)
		{
			this.m_tblInfo = new int[24];
		}
		TextAsset dataTabel = this.m_dataTabel;
		if (dataTabel)
		{
			string xml = AESCrypt.Decrypt(dataTabel.text);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			EventObjectTable.CreateTable(xmlDocument, this.m_tblInfo, out this.m_tblCount);
			if (this.m_tblCount == 0)
			{
			}
		}
	}

	public static void CreateTable(XmlDocument doc, int[] data, out int tbl_count)
	{
		tbl_count = 0;
		if (doc == null)
		{
			return;
		}
		if (doc.DocumentElement == null)
		{
			return;
		}
		XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("EventObjectTable");
		if (xmlNodeList == null || xmlNodeList.Count == 0)
		{
			return;
		}
		int num = 0;
		foreach (XmlNode xmlNode in xmlNodeList)
		{
			XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("Item");
			foreach (XmlNode xmlNode2 in xmlNodeList2)
			{
				for (int i = 0; i < 8; i++)
				{
					string itemName = EventObjectTable.GetItemName((uint)i);
					XmlAttribute xmlAttribute = xmlNode2.Attributes[itemName];
					int num2 = 0;
					if (xmlAttribute != null)
					{
						num2 = int.Parse(xmlNode2.Attributes[itemName].Value, NumberStyles.AllowLeadingSign);
					}
					int num3 = num * 8 + i;
					data[num3] = num2;
				}
			}
			num++;
		}
		tbl_count = num;
	}

	public bool IsSetupEnd()
	{
		return this.m_tblInfo != null;
	}

	public int GetData(int tbl_index, EventObjectTableItem item_index)
	{
		if (this.m_tblInfo != null && (ulong)tbl_index < (ulong)((long)this.m_tblCount))
		{
			int num = (int)(tbl_index * 8 + item_index);
			if (num < this.m_tblInfo.Length)
			{
				return this.m_tblInfo[num];
			}
		}
		return 0;
	}

	private int GetData(int index)
	{
		if (this.m_tblInfo != null && index < this.m_tblInfo.Length)
		{
			return this.m_tblInfo[index];
		}
		return 0;
	}

	public static void LoadSetup()
	{
		EventObjectTable instance = EventObjectTable.Instance;
		if (instance == null)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.EVENT_RESOURCE, "EventObjectTable");
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
		}
	}

	public static int GetItemData(int tbl_index, EventObjectTableItem item_index)
	{
		EventObjectTable instance = EventObjectTable.Instance;
		if (instance != null)
		{
			return instance.GetData(tbl_index, item_index);
		}
		return 0;
	}

	public static EventObjectTableItem GetEventObjectTableItem(string objName)
	{
		int num = Array.IndexOf<string>(EventObjectTable.ITEM_NAMES, objName);
		if (num >= 0)
		{
			return (EventObjectTableItem)num;
		}
		return EventObjectTableItem.NONE;
	}

	public static bool IsCyrstal(EventObjectTableItem item)
	{
		return Array.IndexOf<EventObjectTableItem>(EventObjectTable.CRYSTAL_ITEMS, item) >= 0;
	}
}
