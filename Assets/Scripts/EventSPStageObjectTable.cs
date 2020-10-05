using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EventSPStageObjectTable : MonoBehaviour
{
	public const int ITEM_COUNT_MAX = 8;

	public const int TBL_COUNT_MAX = 1;

	[SerializeField]
	private TextAsset m_dataTabel;

	private static readonly string[] ITEM_NAMES = new string[]
	{
		"SPCrystalModelA",
		"SPCrystalModelB",
		"SPCrystalModelC",
		"SPCrystal10Model",
		"SPCrystalEffect",
		"SPCrystal10Effect",
		"SPCrystalSE",
		"SPCrystal10SE"
	};

	private static readonly EventSPStageObjectTableItem[] SPCRYSTAL_MODEL_TBL = new EventSPStageObjectTableItem[]
	{
		EventSPStageObjectTableItem.SPCrystalModelA,
		EventSPStageObjectTableItem.SPCrystalModelB,
		EventSPStageObjectTableItem.SPCrystalModelC
	};

	private List<string> m_spCrystalModelList = new List<string>();

	private string[] m_tblInfo;

	private int m_tblCount;

	private static EventSPStageObjectTable s_instance = null;

	public static EventSPStageObjectTable Instance
	{
		get
		{
			return EventSPStageObjectTable.s_instance;
		}
	}

	private void Awake()
	{
		if (EventSPStageObjectTable.s_instance == null)
		{
			EventSPStageObjectTable.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (EventSPStageObjectTable.s_instance == this)
		{
			EventSPStageObjectTable.s_instance = null;
		}
	}

	private void Start()
	{
	}

	public static string GetItemName(uint index)
	{
		if (index < 8u && (ulong)index < (ulong)((long)EventSPStageObjectTable.ITEM_NAMES.Length))
		{
			return EventSPStageObjectTable.ITEM_NAMES[(int)((UIntPtr)index)];
		}
		return string.Empty;
	}

	private void Setup()
	{
		if (this.IsSetupEnd())
		{
			return;
		}
		if (this.m_tblInfo == null)
		{
			this.m_tblInfo = new string[8];
		}
		TextAsset dataTabel = this.m_dataTabel;
		if (dataTabel)
		{
			string xml = AESCrypt.Decrypt(dataTabel.text);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			EventSPStageObjectTable.CreateTable(xmlDocument, this.m_tblInfo, out this.m_tblCount);
			if (this.m_tblCount != 0)
			{
				for (int i = 0; i < EventSPStageObjectTable.SPCRYSTAL_MODEL_TBL.Length; i++)
				{
					string itemData = EventSPStageObjectTable.GetItemData(EventSPStageObjectTable.SPCRYSTAL_MODEL_TBL[i]);
					if (itemData != null && itemData != string.Empty)
					{
						this.m_spCrystalModelList.Add(itemData);
					}
				}
			}
		}
	}

	public static void CreateTable(XmlDocument doc, string[] data, out int tbl_count)
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
		XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("EventSPStageObjectTable");
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
					string itemName = EventSPStageObjectTable.GetItemName((uint)i);
					XmlAttribute xmlAttribute = xmlNode2.Attributes[itemName];
					string text = string.Empty;
					if (xmlAttribute != null)
					{
						text = xmlNode2.Attributes[itemName].Value;
					}
					int num2 = num * 8 + i;
					data[num2] = text;
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

	public string GetData(EventSPStageObjectTableItem item_index)
	{
		if (!this.IsSetupEnd())
		{
			this.Setup();
		}
		int num = 0;
		if (this.m_tblInfo != null && (ulong)num < (ulong)((long)this.m_tblCount))
		{
			int num2 = (int)(num * 8 + item_index);
			if (num2 < this.m_tblInfo.Length)
			{
				return this.m_tblInfo[num2];
			}
		}
		return string.Empty;
	}

	private string GetData(int index)
	{
		return this.GetData((EventSPStageObjectTableItem)index);
	}

	public List<string> GetSPCrystalModelList()
	{
		if (!this.IsSetupEnd())
		{
			this.Setup();
		}
		return this.m_spCrystalModelList;
	}

	public static void LoadSetup()
	{
		EventSPStageObjectTable instance = EventSPStageObjectTable.Instance;
		if (instance == null)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.EVENT_RESOURCE, "EventSPStageObjectTable");
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
		}
	}

	public static string GetItemData(EventSPStageObjectTableItem item_index)
	{
		EventSPStageObjectTable instance = EventSPStageObjectTable.Instance;
		if (instance != null)
		{
			return instance.GetData(item_index);
		}
		return string.Empty;
	}

	public static string GetSPCrystalModel()
	{
		EventSPStageObjectTable instance = EventSPStageObjectTable.Instance;
		if (instance != null)
		{
			List<string> sPCrystalModelList = instance.GetSPCrystalModelList();
			if (sPCrystalModelList != null && sPCrystalModelList.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, sPCrystalModelList.Count);
				return sPCrystalModelList[index];
			}
		}
		return string.Empty;
	}

	public static EventSPStageObjectTableItem GetEventSPStageObjectTableItem(string objName)
	{
		int num = Array.IndexOf<string>(EventSPStageObjectTable.ITEM_NAMES, objName);
		if (num >= 0)
		{
			return (EventSPStageObjectTableItem)num;
		}
		return EventSPStageObjectTableItem.NONE;
	}
}
