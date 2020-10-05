using System;
using System.Xml;
using UnityEngine;

public class ImportAbilityTable : MonoBehaviour
{
	[SerializeField]
	private TextAsset m_textAsset;

	private static ImportAbilityTable m_instance;

	private AbilityUpParamTable m_table;

	private void Awake()
	{
		if (ImportAbilityTable.m_instance == null)
		{
			this.Initialize();
			ImportAbilityTable.m_instance = this;
		}
	}

	private void OnDestroy()
	{
		if (ImportAbilityTable.m_instance == this)
		{
			ImportAbilityTable.m_instance = null;
		}
	}

	private void Initialize()
	{
		if (this.m_textAsset == null)
		{
			return;
		}
		string xml = AESCrypt.Decrypt(this.m_textAsset.text);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		XmlNode documentElement = xmlDocument.DocumentElement;
		if (documentElement == null)
		{
			return;
		}
		XmlNodeList childNodes = documentElement.ChildNodes;
		if (childNodes == null)
		{
			return;
		}
		this.m_table = new AbilityUpParamTable();
		int num = 0;
		foreach (XmlNode xmlNode in childNodes)
		{
			if (xmlNode != null)
			{
				AbilityUpParamList abilityUpParamList = new AbilityUpParamList();
				XmlNodeList childNodes2 = xmlNode.ChildNodes;
				foreach (XmlNode xmlNode2 in childNodes2)
				{
					if (xmlNode2 != null)
					{
						string value = xmlNode2.Attributes.GetNamedItem("ring_cost").Value;
						string value2 = xmlNode2.Attributes.GetNamedItem("potential").Value;
						abilityUpParamList.AddAbilityUpParam(new AbilityUpParam
						{
							Cost = float.Parse(value),
							Potential = float.Parse(value2)
						});
					}
				}
				if (abilityUpParamList.Count > 0)
				{
					AbilityType type = (AbilityType)num;
					this.m_table.AddList(type, abilityUpParamList);
					num++;
				}
			}
		}
	}

	public float GetAbilityPotential(AbilityType type, int level)
	{
		if (this.m_table == null)
		{
			return 0f;
		}
		AbilityUpParamList list = this.m_table.GetList(type);
		if (list == null)
		{
			return 0f;
		}
		AbilityUpParam abilityUpParam = list.GetAbilityUpParam(level);
		if (abilityUpParam == null)
		{
			return 0f;
		}
		return abilityUpParam.Potential;
	}

	public float GetAbilityCost(AbilityType type, int level)
	{
		if (this.m_table == null)
		{
			return 0f;
		}
		AbilityUpParamList list = this.m_table.GetList(type);
		if (list == null)
		{
			return 0f;
		}
		AbilityUpParam abilityUpParam = list.GetAbilityUpParam(level);
		if (abilityUpParam == null)
		{
			return 0f;
		}
		return abilityUpParam.Cost;
	}

	public int GetMaxLevel(AbilityType type)
	{
		if (this.m_table == null)
		{
			return 0;
		}
		AbilityUpParamList list = this.m_table.GetList(type);
		if (list == null)
		{
			return 0;
		}
		return list.GetMaxLevel();
	}

	public static ImportAbilityTable GetInstance()
	{
		return ImportAbilityTable.m_instance;
	}
}
