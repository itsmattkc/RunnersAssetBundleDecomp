using System;
using System.Globalization;
using System.Xml;
using UnityEngine;

public class EventCommonDataTable : MonoBehaviour
{
	public const int ITEM_COUNT_MAX = 8;

	public const int TBL_COUNT_MAX = 1;

	[SerializeField]
	private TextAsset m_xml_data;

	private static readonly string[] ITEM_NAMES = new string[]
	{
		"SeFileName",
		"MenuBgmFileName",
		"Roulette_BgmName",
		"RouletteS_BgmName",
		"EventTop_BgmName",
		"RouletteDecide_SeCueName",
		"RouletteChange_SeCueName",
		"RouletteChao_Number"
	};

	private string[] m_tblInfo;

	private int m_tblCount;

	private static EventCommonDataTable s_instance = null;

	public static EventCommonDataTable Instance
	{
		get
		{
			return EventCommonDataTable.s_instance;
		}
	}

	private void Awake()
	{
		if (EventCommonDataTable.s_instance == null)
		{
			EventCommonDataTable.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (EventCommonDataTable.s_instance == this)
		{
			EventCommonDataTable.s_instance = null;
		}
	}

	private void Start()
	{
		this.Setup();
	}

	public void Setup()
	{
		if (this.m_tblInfo == null)
		{
			this.m_tblInfo = new string[8];
		}
		if (this.m_xml_data)
		{
			string xml = AESCrypt.Decrypt(this.m_xml_data.text);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			EventCommonDataTable.CreateTable(xmlDocument, this.m_tblInfo, out this.m_tblCount);
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
		XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("EventCommonDataTable");
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
					string itemName = EventCommonDataTable.GetItemName((uint)i);
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

	public string GetData(EventCommonDataItem item_index)
	{
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

	public bool IsRouletteEventChao(int chaoId)
	{
		string data = this.GetData(EventCommonDataItem.RouletteChao_Number);
		if (data != null && data != string.Empty)
		{
			string[] array = data.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string s = array2[i];
				int num = int.Parse(s, NumberStyles.AllowLeadingSign);
				if (num == chaoId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static string GetItemName(uint index)
	{
		if (index < 8u && (ulong)index < (ulong)((long)EventCommonDataTable.ITEM_NAMES.Length))
		{
			return EventCommonDataTable.ITEM_NAMES[(int)((UIntPtr)index)];
		}
		return string.Empty;
	}

	public static void LoadSetup()
	{
		GameObject gameObject = GameObject.Find("EventResourceCommon");
		if (gameObject != null)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
				if (gameObject2.name == "EventCommonDataTable" && !gameObject2.activeSelf)
				{
					gameObject2.SetActive(true);
				}
			}
		}
	}
}
