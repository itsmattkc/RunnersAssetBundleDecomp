using System;
using System.Xml;
using UnityEngine;

public class EventBossObjectTable : MonoBehaviour
{
	public const int ITEM_COUNT_MAX = 15;

	public const int TBL_COUNT_MAX = 1;

	[SerializeField]
	private TextAsset m_dataTabel;

	private static readonly string[] ITEM_NAMES = new string[]
	{
		"BgmFile",
		"BgmCueName",
		"RingModel",
		"Ring10Model",
		"RingEffect",
		"Ring10Effect",
		"RingSE",
		"Ring10SE",
		"EscapeEffect",
		"Obj1_ModelName",
		"Obj1_EffectName",
		"Obj1_LoopEffectName",
		"Obj1_SetSeName",
		"Obj2_ModelName",
		"Obj2_EffectName"
	};

	private string[] m_tblInfo;

	private int m_tblCount;

	private static EventBossObjectTable s_instance = null;

	public static EventBossObjectTable Instance
	{
		get
		{
			return EventBossObjectTable.s_instance;
		}
	}

	private void Awake()
	{
		if (EventBossObjectTable.s_instance == null)
		{
			EventBossObjectTable.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (EventBossObjectTable.s_instance == this)
		{
			EventBossObjectTable.s_instance = null;
		}
	}

	private void Start()
	{
	}

	public static string GetItemName(uint index)
	{
		if (index < 15u && (ulong)index < (ulong)((long)EventBossObjectTable.ITEM_NAMES.Length))
		{
			return EventBossObjectTable.ITEM_NAMES[(int)((UIntPtr)index)];
		}
		return string.Empty;
	}

	public void Setup()
	{
		if (this.m_tblInfo == null)
		{
			this.m_tblInfo = new string[15];
		}
		TextAsset dataTabel = this.m_dataTabel;
		if (dataTabel)
		{
			string xml = AESCrypt.Decrypt(dataTabel.text);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			EventBossObjectTable.CreateTable(xmlDocument, this.m_tblInfo, out this.m_tblCount);
			if (this.m_tblCount == 0)
			{
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
		XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("EventBossObjectTable");
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
				for (int i = 0; i < 15; i++)
				{
					string itemName = EventBossObjectTable.GetItemName((uint)i);
					XmlAttribute xmlAttribute = xmlNode2.Attributes[itemName];
					string text = string.Empty;
					if (xmlAttribute != null)
					{
						text = xmlNode2.Attributes[itemName].Value;
					}
					int num2 = num * 15 + i;
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

	public string GetData(EventBossObjectTableItem item_index)
	{
		int num = 0;
		if (this.m_tblInfo != null && (ulong)num < (ulong)((long)this.m_tblCount))
		{
			int num2 = (int)(num * 15 + item_index);
			if (num2 < this.m_tblInfo.Length)
			{
				return this.m_tblInfo[num2];
			}
		}
		return string.Empty;
	}

	private string GetData(int index)
	{
		return this.GetData((EventBossObjectTableItem)index);
	}

	public static void LoadSetup()
	{
		EventBossObjectTable instance = EventBossObjectTable.Instance;
		if (instance == null)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.EVENT_RESOURCE, "EventBossObjectTable");
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
			EventBossObjectTable instance2 = EventBossObjectTable.Instance;
			if (instance2 != null)
			{
				instance2.Setup();
			}
		}
	}

	public static string GetItemData(EventBossObjectTableItem item_index)
	{
		EventBossObjectTable instance = EventBossObjectTable.Instance;
		if (instance != null)
		{
			return instance.GetData(item_index);
		}
		return string.Empty;
	}
}
