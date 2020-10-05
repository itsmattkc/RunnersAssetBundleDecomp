using System;
using System.Globalization;
using System.Xml;
using UnityEngine;

public class RareEnemyTable
{
	public const int TBL_COUNT_MAX = 8;

	private int[] m_tblInfo;

	private int m_tblCount;

	public bool IsRareEnemy(uint tbl_index)
	{
		if (this.m_tblInfo != null && (ulong)tbl_index < (ulong)((long)this.m_tblCount) && (ulong)tbl_index < (ulong)((long)this.m_tblInfo.Length))
		{
			int randomRange = ObjUtil.GetRandomRange100();
			int num = this.m_tblInfo[(int)((UIntPtr)tbl_index)];
			num = ObjUtil.GetChaoAbliltyValue(ChaoAbility.RARE_ENEMY_UP, num);
			if (num > randomRange)
			{
				return true;
			}
		}
		return false;
	}

	public void Setup(TerrainXmlData terrainData)
	{
		if (this.m_tblInfo == null)
		{
			this.m_tblInfo = new int[8];
		}
		if (terrainData != null)
		{
			TextAsset rareEnemyTableData = terrainData.RareEnemyTableData;
			if (rareEnemyTableData)
			{
				string xml = AESCrypt.Decrypt(rareEnemyTableData.text);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				RareEnemyTable.CreateTable(xmlDocument, this.m_tblInfo, out this.m_tblCount);
				if (this.m_tblCount == 0)
				{
				}
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
		XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("RareEnemyTable");
		if (xmlNodeList == null || xmlNodeList.Count == 0)
		{
			return;
		}
		int num = 0;
		foreach (XmlNode xmlNode in xmlNodeList)
		{
			XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("Table");
			foreach (XmlNode xmlNode2 in xmlNodeList2)
			{
				int num2 = int.Parse(xmlNode2.Attributes["Param"].Value, NumberStyles.AllowLeadingSign);
				data[num] = num2;
			}
			num++;
		}
		tbl_count = num;
	}
}
