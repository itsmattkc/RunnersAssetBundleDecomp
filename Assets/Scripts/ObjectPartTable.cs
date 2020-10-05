using System;
using System.Globalization;
using System.Xml;
using UnityEngine;

public class ObjectPartTable
{
	private static readonly string[] ITEM_NAMES = new string[]
	{
		"BrokenBonusRatio",
		"BrokenBonusSuperRing",
		"BrokenBonusRedStarRing",
		"BrokenBonusCrystal10",
		"ComboBonusCombo1",
		"ComboBonusCombo2",
		"ComboBonusCombo3",
		"ComboBonusCombo4",
		"ComboBonusCombo5",
		"ComboBonusCombo6",
		"ComboBonusCombo7",
		"ComboBonusBonus1",
		"ComboBonusBonus2",
		"ComboBonusBonus3",
		"ComboBonusBonus4",
		"ComboBonusBonus5",
		"ComboBonusBonus6",
		"ComboBonusBonus7",
		"ComboBonusBonus8"
	};

	private int[] m_tblInfo;

	private void Start()
	{
	}

	public static string GetItemName(uint index)
	{
		if ((ulong)index < (ulong)((long)ObjectPartTable.ITEM_NAMES.Length))
		{
			return ObjectPartTable.ITEM_NAMES[(int)((UIntPtr)index)];
		}
		return string.Empty;
	}

	public BrokenBonusType GetBrokenBonusType()
	{
		int data = this.GetData(ObjectPartType.BROKENBONUS_RATIO);
		int randomRange = ObjUtil.GetRandomRange100();
		if (randomRange < data)
		{
			bool flag = false;
			if (StageModeManager.Instance != null)
			{
				flag = StageModeManager.Instance.FirstTutorial;
			}
			int randomRange2 = ObjUtil.GetRandomRange100();
			int num = 0;
			for (int i = 1; i < 4; i++)
			{
				int num2 = this.GetData((ObjectPartType)i);
				if (flag && i == 2)
				{
					num2 = 0;
				}
				if (num <= randomRange2 && randomRange2 < num + num2)
				{
					switch (i)
					{
					case 1:
						return BrokenBonusType.SUPER_RING;
					case 2:
						return BrokenBonusType.REDSTAR_RING;
					case 3:
						return BrokenBonusType.CRYSTAL10;
					}
				}
				num += num2;
			}
		}
		return BrokenBonusType.NONE;
	}

	public BrokenBonusType GetBrokenBonusTypeForChaoAbility()
	{
		int data = this.GetData(ObjectPartType.BROKENBONUS_RATIO);
		int randomRange = ObjUtil.GetRandomRange100();
		if (randomRange >= data)
		{
			return BrokenBonusType.NONE;
		}
		int randomRange2 = ObjUtil.GetRandomRange100();
		int num = 0;
		if (StageAbilityManager.Instance != null)
		{
			num = (int)StageAbilityManager.Instance.GetChaoAbilityExtraValue(ChaoAbility.COMBO_STEP_DESTROY_GET_10_RING);
		}
		if (randomRange2 <= 1)
		{
			return BrokenBonusType.REDSTAR_RING;
		}
		if (randomRange2 <= num)
		{
			return BrokenBonusType.SUPER_RING;
		}
		return BrokenBonusType.CRYSTAL10;
	}

	public int GetComboBonusComboNum(int index)
	{
		return this.GetData(ObjectPartType.COMBOBONUS_COMBO1 + index);
	}

	public int GetComboBonusBonusNum(int index)
	{
		return this.GetData(ObjectPartType.COMBOBONUS_BONUS1 + index);
	}

	public void Setup(TerrainXmlData terrainData)
	{
		if (this.m_tblInfo == null)
		{
			this.m_tblInfo = new int[19];
		}
		if (terrainData != null)
		{
			TextAsset objectPartTableData = terrainData.ObjectPartTableData;
			if (objectPartTableData)
			{
				string xml = AESCrypt.Decrypt(objectPartTableData.text);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				ObjectPartTable.CreateTable(xmlDocument, this.m_tblInfo);
			}
		}
	}

	public static void CreateTable(XmlDocument doc, int[] data)
	{
		if (doc == null)
		{
			return;
		}
		if (doc.DocumentElement == null)
		{
			return;
		}
		XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("ObjectPartTable");
		if (xmlNodeList == null || xmlNodeList.Count == 0)
		{
			return;
		}
		for (int i = 0; i < 19; i++)
		{
			int num = 0;
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				string itemName = ObjectPartTable.GetItemName((uint)i);
				XmlNodeList xmlNodeList2 = xmlNode.SelectNodes(itemName);
				foreach (XmlNode xmlNode2 in xmlNodeList2)
				{
					if (xmlNode2.InnerText != null)
					{
						num = int.Parse(xmlNode2.InnerText, NumberStyles.AllowLeadingSign);
					}
				}
			}
			data[i] = num;
		}
	}

	private int GetData(ObjectPartType item_index)
	{
		if (this.m_tblInfo != null && item_index < (ObjectPartType)this.m_tblInfo.Length)
		{
			return this.m_tblInfo[(int)item_index];
		}
		return 0;
	}
}
