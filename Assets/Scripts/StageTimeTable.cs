using System;
using System.Globalization;
using System.Xml;
using UnityEngine;

public class StageTimeTable : MonoBehaviour
{
	public const int ITEM_COUNT_MAX = 7;

	public const int TBL_COUNT_MAX = 1;

	[SerializeField]
	private TextAsset m_stageTimeTable;

	private static readonly string[] ITEM_NAMES = new string[]
	{
		"StartTime",
		"OverlapBonus",
		"ItemExtendedLimit",
		"BronzeWatch",
		"SilverWatch",
		"GoldWatch",
		"Continue"
	};

	private int[] m_tblInfo;

	private int m_tblCount;

	private void Start()
	{
		if (this.m_tblInfo == null)
		{
			this.m_tblInfo = new int[7];
		}
		if (this.m_stageTimeTable != null && this.m_stageTimeTable)
		{
			string xml = AESCrypt.Decrypt(this.m_stageTimeTable.text);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			StageTimeTable.CreateTable(xmlDocument, this.m_tblInfo, out this.m_tblCount);
			if (this.m_tblCount == 0)
			{
			}
		}
	}

	public static string GetItemName(uint index)
	{
		if (index < 7u && (ulong)index < (ulong)((long)StageTimeTable.ITEM_NAMES.Length))
		{
			return StageTimeTable.ITEM_NAMES[(int)((UIntPtr)index)];
		}
		return string.Empty;
	}

	public int GetTableItemData(StageTimeTableItem item)
	{
		return this.GetData((int)item);
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
		XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("StageTimeTable");
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
				for (int i = 0; i < 7; i++)
				{
					string itemName = StageTimeTable.GetItemName((uint)i);
					XmlAttribute xmlAttribute = xmlNode2.Attributes[itemName];
					int num2 = 0;
					if (xmlAttribute != null)
					{
						num2 = int.Parse(xmlNode2.Attributes[itemName].Value, NumberStyles.AllowLeadingSign);
					}
					int num3 = num * 7 + i;
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

	private int GetData(int tbl_index, int item_index)
	{
		if (this.m_tblInfo != null && (ulong)tbl_index < (ulong)((long)this.m_tblCount))
		{
			int num = tbl_index * 7 + item_index;
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
}
