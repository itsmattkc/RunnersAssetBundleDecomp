using System;
using System.Globalization;
using System.Xml;
using UnityEngine;

public class BossMap3Table
{
	public const int ATTACK_COUNT_MAX = 16;

	public const int ITEM_COUNT_MAX = 80;

	public const int TBL_COUNT_MAX = 24;

	private static readonly string[] ITEM_NAMES = new string[]
	{
		"Map3AttackType",
		"BallRand_A",
		"Position_A",
		"BallRand_B",
		"Position_B"
	};

	private int[] m_tblInfo;

	private int m_tblCount;

	private void Start()
	{
	}

	public static string GetItemName(uint index)
	{
		uint itemIndex = (uint)BossMap3Table.GetItemIndex((int)index);
		if ((ulong)itemIndex < (ulong)((long)BossMap3Table.ITEM_NAMES.Length))
		{
			return BossMap3Table.ITEM_NAMES[(int)((UIntPtr)itemIndex)];
		}
		return string.Empty;
	}

	public static int GetItemIndex(int index)
	{
		int attackItemTableCount = BossMap3Table.GetAttackItemTableCount(index);
		if (attackItemTableCount > 0)
		{
			index -= attackItemTableCount * 5;
		}
		return index;
	}

	public static int GetAttackItemTableCount(int index)
	{
		int result = 0;
		if (index >= 5)
		{
			result = index / 5;
		}
		return result;
	}

	public Map3AttackData GetMap3AttackData(int tbl_index, int attack_index)
	{
		int num = tbl_index * 80 + 5 * attack_index;
		BossAttackType data = (BossAttackType)this.GetData(num);
		int data2 = this.GetData(++num);
		BossAttackPos data3 = (BossAttackPos)this.GetData(++num);
		int data4 = this.GetData(++num);
		BossAttackPos data5 = (BossAttackPos)this.GetData(num + 1);
		return new Map3AttackData(data, data2, data3, data4, data5);
	}

	public void Setup(TerrainXmlData terrainData)
	{
		if (this.m_tblInfo == null)
		{
			this.m_tblInfo = new int[1920];
		}
		if (terrainData != null)
		{
			TextAsset bossMap3TableData = terrainData.BossMap3TableData;
			if (bossMap3TableData)
			{
				string xml = AESCrypt.Decrypt(bossMap3TableData.text);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				BossMap3Table.CreateTable(xmlDocument, this.m_tblInfo, out this.m_tblCount);
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
		XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("BossMap3Table");
		if (xmlNodeList == null || xmlNodeList.Count == 0)
		{
			return;
		}
		int num = 0;
		foreach (XmlNode xmlNode in xmlNodeList)
		{
			for (int i = 0; i < 16; i++)
			{
				XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("Attack_" + i.ToString());
				foreach (XmlNode xmlNode2 in xmlNodeList2)
				{
					for (int j = 0; j < 5; j++)
					{
						string itemName = BossMap3Table.GetItemName((uint)j);
						XmlAttribute xmlAttribute = xmlNode2.Attributes[itemName];
						int num2 = 0;
						if (xmlAttribute != null)
						{
							num2 = int.Parse(xmlNode2.Attributes[itemName].Value, NumberStyles.AllowLeadingSign);
						}
						int num3 = num * 80 + j + 5 * i;
						data[num3] = num2;
					}
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
			int num = tbl_index * 80 + item_index;
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

	public static int BossAttackTypeMinMax(int val)
	{
		int num = val;
		if (num < 0)
		{
			num = 0;
		}
		else if (num >= 11)
		{
			num = 10;
		}
		return num;
	}

	public static int GetBossAttackCount(BossAttackType type)
	{
		switch (type)
		{
		case BossAttackType.NONE:
			return 0;
		case BossAttackType.W:
			return 1;
		case BossAttackType.H:
			return 1;
		default:
			return 2;
		}
	}
}
