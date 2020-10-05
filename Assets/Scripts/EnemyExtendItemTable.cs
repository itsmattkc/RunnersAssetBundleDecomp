using System;
using System.Globalization;
using System.Xml;
using UnityEngine;

public class EnemyExtendItemTable
{
	public const int ITEM_COUNT_MAX = 3;

	public const int TBL_COUNT_MAX = 1;

	private static readonly string[] ITEM_NAMES = new string[]
	{
		"BronzeTimer",
		"SilverTimer",
		"GoldTimer"
	};

	private int[] m_tblInfo;

	private int m_tblCount;

	public void Setup(TerrainXmlData terrainData)
	{
		if (this.m_tblInfo == null)
		{
			this.m_tblInfo = new int[3];
		}
		if (terrainData != null)
		{
			TextAsset enemyExtendItemTableData = terrainData.EnemyExtendItemTableData;
			if (enemyExtendItemTableData)
			{
				string xml = AESCrypt.Decrypt(enemyExtendItemTableData.text);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				EnemyExtendItemTable.CreateTable(xmlDocument, this.m_tblInfo, out this.m_tblCount);
				if (this.m_tblCount == 0)
				{
				}
			}
		}
	}

	public static string GetItemName(uint index)
	{
		if (index < 3u && (ulong)index < (ulong)((long)EnemyExtendItemTable.ITEM_NAMES.Length))
		{
			return EnemyExtendItemTable.ITEM_NAMES[(int)((UIntPtr)index)];
		}
		return string.Empty;
	}

	public int GetTableItemData(EnemyExtendItemTableItem item)
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
		XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("EnemyExtendItemTable");
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
				for (int i = 0; i < 3; i++)
				{
					string itemName = EnemyExtendItemTable.GetItemName((uint)i);
					XmlAttribute xmlAttribute = xmlNode2.Attributes[itemName];
					int num2 = 0;
					if (xmlAttribute != null)
					{
						num2 = int.Parse(xmlNode2.Attributes[itemName].Value, NumberStyles.AllowLeadingSign);
					}
					int num3 = num * 3 + i;
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
			int num = tbl_index * 3 + item_index;
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
