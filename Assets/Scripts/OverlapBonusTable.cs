using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UnityEngine;

public class OverlapBonusTable : MonoBehaviour
{
	[SerializeField]
	private TextAsset m_overlapBonusTabel;

	private static Dictionary<CharaType, List<int[]>> m_OverlapBonus = new Dictionary<CharaType, List<int[]>>();

	private static readonly string[] ITEM_NAMES = new string[]
	{
		"Score",
		"Ring",
		"Animal",
		"Distance",
		"Enemy"
	};

	private int m_tblCount;

	private void Start()
	{
	}

	public void Setup()
	{
		if (this.m_overlapBonusTabel)
		{
			string xml = AESCrypt.Decrypt(this.m_overlapBonusTabel.text);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			OverlapBonusTable.CreateTable(xmlDocument, out this.m_tblCount);
			if (this.m_tblCount == 0)
			{
			}
		}
	}

	public static string GetItemName(uint index)
	{
		if (index < 5u && (ulong)index < (ulong)((long)OverlapBonusTable.ITEM_NAMES.Length))
		{
			return OverlapBonusTable.ITEM_NAMES[(int)((UIntPtr)index)];
		}
		return string.Empty;
	}

	public static void CreateTable(XmlDocument doc, out int tbl_count)
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
		XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("BonusData");
		if (xmlNodeList == null || xmlNodeList.Count == 0)
		{
			return;
		}
		int num = 0;
		foreach (XmlNode xmlNode in xmlNodeList)
		{
			XmlAttribute xmlAttribute = xmlNode.Attributes["charaID"];
			int serverID = 0;
			if (xmlAttribute != null)
			{
				serverID = int.Parse(xmlNode.Attributes["charaID"].Value, NumberStyles.AllowLeadingSign);
			}
			CharacterDataNameInfo instance = CharacterDataNameInfo.Instance;
			if (instance != null)
			{
				CharacterDataNameInfo.Info dataByServerID = instance.GetDataByServerID(serverID);
				if (dataByServerID != null)
				{
					CharaType iD = dataByServerID.m_ID;
					XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("Param");
					int num2 = 0;
					foreach (XmlNode xmlNode2 in xmlNodeList2)
					{
						int[] array = new int[5];
						for (int i = 0; i < 5; i++)
						{
							string itemName = OverlapBonusTable.GetItemName((uint)i);
							XmlAttribute xmlAttribute2 = xmlNode2.Attributes[itemName];
							int num3 = 0;
							if (xmlAttribute2 != null)
							{
								num3 = int.Parse(xmlNode2.Attributes[itemName].Value, NumberStyles.AllowLeadingSign);
							}
							array[i] = num3;
						}
						if (!OverlapBonusTable.m_OverlapBonus.ContainsKey(iD))
						{
							OverlapBonusTable.m_OverlapBonus.Add(iD, new List<int[]>());
						}
						OverlapBonusTable.m_OverlapBonus[iD].Add(array);
						num2++;
					}
					num++;
				}
			}
		}
		tbl_count = num;
	}

	public bool IsSetupEnd()
	{
		return this.m_tblCount > 0;
	}

	public float GetStarBonusList(CharaType charaType, int star, OverlapBonusType bonusType)
	{
		if (this.IsSetupEnd())
		{
			CharaType key = charaType;
			if (!OverlapBonusTable.m_OverlapBonus.ContainsKey(key))
			{
				key = CharaType.SONIC;
			}
			if (star < OverlapBonusTable.m_OverlapBonus[key].Count && OverlapBonusType.SCORE <= bonusType && bonusType < OverlapBonusType.NUM)
			{
				return (float)OverlapBonusTable.m_OverlapBonus[key][star][(int)bonusType];
			}
		}
		return 0f;
	}

	public Dictionary<BonusParam.BonusType, float> GetStarBonusList(CharaType charaType, int star)
	{
		Dictionary<BonusParam.BonusType, float> dictionary = new Dictionary<BonusParam.BonusType, float>();
		if (this.IsSetupEnd())
		{
			CharaType key = charaType;
			if (!OverlapBonusTable.m_OverlapBonus.ContainsKey(key))
			{
				key = CharaType.SONIC;
			}
			if (star < OverlapBonusTable.m_OverlapBonus[key].Count)
			{
				dictionary.Add(BonusParam.BonusType.SCORE, (float)OverlapBonusTable.m_OverlapBonus[key][star][0]);
				dictionary.Add(BonusParam.BonusType.RING, (float)OverlapBonusTable.m_OverlapBonus[key][star][1]);
				dictionary.Add(BonusParam.BonusType.ANIMAL, (float)OverlapBonusTable.m_OverlapBonus[key][star][2]);
				dictionary.Add(BonusParam.BonusType.DISTANCE, (float)OverlapBonusTable.m_OverlapBonus[key][star][3]);
				dictionary.Add(BonusParam.BonusType.ENEMY_OBJBREAK, (float)OverlapBonusTable.m_OverlapBonus[key][star][4]);
			}
		}
		return dictionary;
	}

	public Dictionary<BonusParam.BonusType, float> GetStarBonusList(CharaType charaType)
	{
		Dictionary<BonusParam.BonusType, float> result = null;
		ServerPlayerState playerState = ServerInterface.PlayerState;
		if (playerState != null)
		{
			ServerCharacterState serverCharacterState = playerState.CharacterState(charaType);
			if (serverCharacterState != null)
			{
				return this.GetStarBonusList(charaType, serverCharacterState.star);
			}
		}
		return result;
	}
}
