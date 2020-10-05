using App.Utility;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CharacterDataNameInfo : MonoBehaviour
{
	public class Info
	{
		public enum Option
		{
			Big,
			HighSpeed,
			ThirdJump
		}

		public string m_name;

		public CharaType m_ID = CharaType.UNKNOWN;

		public string m_hud_suffix;

		public CharacterAttribute m_attribute = CharacterAttribute.UNKNOWN;

		public TeamAttribute m_teamAttribute = TeamAttribute.UNKNOWN;

		public TeamAttributeCategory m_teamAttributeCategory = TeamAttributeCategory.NONE;

		public TeamAttributeBonusType m_mainAttributeBonus = TeamAttributeBonusType.NONE;

		public TeamAttributeBonusType m_subAttributeBonus = TeamAttributeBonusType.NONE;

		private float m_teamAttributeValue;

		private float m_teamAttributeSubValue;

		public int m_serverID;

		public Bitset32 m_flag;

		public float TeamAttributeValue
		{
			get
			{
				return this.m_teamAttributeValue;
			}
			set
			{
				this.m_teamAttributeValue = value;
			}
		}

		public float TeamAttributeSubValue
		{
			get
			{
				return this.m_teamAttributeSubValue;
			}
			set
			{
				this.m_teamAttributeSubValue = value;
			}
		}

		public bool BigSize
		{
			get
			{
				return this.m_flag.Test(0);
			}
			set
			{
				this.m_flag.Set(0, value);
			}
		}

		public bool HighSpeedEffect
		{
			get
			{
				return this.m_flag.Test(1);
			}
			set
			{
				this.m_flag.Set(1, value);
			}
		}

		public bool ThirdJump
		{
			get
			{
				return this.m_flag.Test(2);
			}
			set
			{
				this.m_flag.Set(2, value);
			}
		}

		public string characterSpriteName
		{
			get
			{
				string result = null;
				int iD = (int)this.m_ID;
				if (iD >= 0)
				{
					result = string.Format("ui_tex_player_{0:00}_{1}", iD, CharacterDataNameInfo.m_prefixNameList[iD]);
				}
				return result;
			}
		}

		public float GetTeamAttributeValue(TeamAttributeBonusType type)
		{
			if (this.m_mainAttributeBonus == type)
			{
				return this.m_teamAttributeValue;
			}
			if (this.m_subAttributeBonus == type)
			{
				return this.m_teamAttributeSubValue;
			}
			return 0f;
		}
	}

	private List<CharacterDataNameInfo.Info> m_list;

	private static string[] m_prefixNameList;

	private static string[] m_charaNameList;

	private static string[] m_charaNameLowerList;

	[SerializeField]
	private TextAsset m_text;

	private static CharacterDataNameInfo instance;

	public static string[] PrefixNameList
	{
		get
		{
			return CharacterDataNameInfo.m_prefixNameList;
		}
	}

	public static string[] CharaNameList
	{
		get
		{
			return CharacterDataNameInfo.m_charaNameList;
		}
	}

	public static string[] CharaNameLowerList
	{
		get
		{
			return CharacterDataNameInfo.m_charaNameLowerList;
		}
	}

	public static CharacterDataNameInfo Instance
	{
		get
		{
			if (CharacterDataNameInfo.instance == null)
			{
				CharacterDataNameInfo.instance = GameObjectUtil.FindGameObjectComponent<CharacterDataNameInfo>("CharacterDataNameInfo");
			}
			return CharacterDataNameInfo.instance;
		}
	}

	private void Awake()
	{
		if (CharacterDataNameInfo.instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			CharacterDataNameInfo.instance = this;
			this.Setup();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (CharacterDataNameInfo.instance == this)
		{
			CharacterDataNameInfo.instance = null;
		}
	}

	private void Start()
	{
	}

	private void Setup()
	{
		if (this.m_list == null)
		{
			this.m_list = new List<CharacterDataNameInfo.Info>();
			string xml = AESCrypt.Decrypt(this.m_text.text);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			this.m_list = new List<CharacterDataNameInfo.Info>();
			CharacterDataNameInfo.CreateTable(xmlDocument, this.m_list);
			this.SetNameList();
		}
	}

	public CharacterDataNameInfo.Info GetDataByIndex(int index)
	{
		if (index < 0 || index >= this.m_list.Count)
		{
			return null;
		}
		return this.m_list[index];
	}

	public CharacterDataNameInfo.Info GetDataByName(string name)
	{
		foreach (CharacterDataNameInfo.Info current in this.m_list)
		{
			if (current.m_name == name)
			{
				return current;
			}
		}
		return null;
	}

	public CharacterDataNameInfo.Info GetDataByID(CharaType id)
	{
		foreach (CharacterDataNameInfo.Info current in this.m_list)
		{
			if (current.m_ID == id)
			{
				return current;
			}
		}
		return null;
	}

	public string GetNameByID(CharaType id)
	{
		foreach (CharacterDataNameInfo.Info current in this.m_list)
		{
			if (current.m_ID == id)
			{
				return current.m_name;
			}
		}
		return null;
	}

	public CharacterDataNameInfo.Info GetDataByServerID(int serverID)
	{
		foreach (CharacterDataNameInfo.Info current in this.m_list)
		{
			if (current.m_serverID == serverID)
			{
				return current;
			}
		}
		return null;
	}

	public string GetNameByServerID(int serverID)
	{
		foreach (CharacterDataNameInfo.Info current in this.m_list)
		{
			if (current.m_serverID == serverID)
			{
				return current.m_name;
			}
		}
		return null;
	}

	public static void CreateTable(XmlDocument doc, List<CharacterDataNameInfo.Info> list)
	{
		if (doc == null)
		{
			return;
		}
		if (doc.DocumentElement == null)
		{
			return;
		}
		XmlNodeList xmlNodeList = doc.SelectNodes("SonicRunnersCharacterInfo");
		if (xmlNodeList == null)
		{
			return;
		}
		if (xmlNodeList.Count == 0)
		{
			return;
		}
		if (xmlNodeList[0] == null)
		{
			return;
		}
		XmlNodeList xmlNodeList2 = xmlNodeList[0].SelectNodes("Character");
		foreach (XmlNode xmlNode in xmlNodeList2)
		{
			CharacterDataNameInfo.Info info = new CharacterDataNameInfo.Info();
			info.m_name = xmlNode.Attributes["Name"].Value;
			string value = xmlNode.Attributes["ID"].Value;
			if (Enum.IsDefined(typeof(CharaType), value))
			{
				info.m_ID = (CharaType)((int)Enum.Parse(typeof(CharaType), value, true));
			}
			info.m_hud_suffix = xmlNode.Attributes["Suffix"].Value;
			string value2 = xmlNode.Attributes["Attribute"].Value;
			if (Enum.IsDefined(typeof(CharacterAttribute), value2))
			{
				info.m_attribute = (CharacterAttribute)((int)Enum.Parse(typeof(CharacterAttribute), value2, true));
			}
			string value3 = xmlNode.Attributes["Team"].Value;
			if (Enum.IsDefined(typeof(TeamAttribute), value3))
			{
				info.m_teamAttribute = (TeamAttribute)((int)Enum.Parse(typeof(TeamAttribute), value3, true));
			}
			string value4 = xmlNode.Attributes["MainBonus"].Value;
			if (Enum.IsDefined(typeof(TeamAttributeBonusType), value4))
			{
				info.m_mainAttributeBonus = (TeamAttributeBonusType)((int)Enum.Parse(typeof(TeamAttributeBonusType), value4, true));
			}
			string value5 = xmlNode.Attributes["SubBonus"].Value;
			if (Enum.IsDefined(typeof(TeamAttributeBonusType), value5))
			{
				info.m_subAttributeBonus = (TeamAttributeBonusType)((int)Enum.Parse(typeof(TeamAttributeBonusType), value5, true));
			}
			float teamAttributeValue = 0f;
			if (float.TryParse(xmlNode.Attributes["TeamAttributeValue"].Value, out teamAttributeValue))
			{
				info.TeamAttributeValue = teamAttributeValue;
			}
			float teamAttributeSubValue = 0f;
			if (float.TryParse(xmlNode.Attributes["TeamAttributeSubValue"].Value, out teamAttributeSubValue))
			{
				info.TeamAttributeSubValue = teamAttributeSubValue;
			}
			string value6 = xmlNode.Attributes["Category"].Value;
			if (Enum.IsDefined(typeof(TeamAttributeCategory), value6))
			{
				info.m_teamAttributeCategory = (TeamAttributeCategory)((int)Enum.Parse(typeof(TeamAttributeCategory), value6, true));
			}
			else
			{
				info.m_teamAttributeCategory = TeamAttributeCategory.NUM;
			}
			int serverID;
			if (int.TryParse(xmlNode.Attributes["ServerID"].Value, out serverID))
			{
				info.m_serverID = serverID;
			}
			if (xmlNode.Attributes["OptBig"] != null)
			{
				string value7 = xmlNode.Attributes["OptBig"].Value;
				if (value7.Equals("true"))
				{
					info.m_flag.Set(0, true);
				}
			}
			if (xmlNode.Attributes["OptHighSpeed"] != null)
			{
				string value8 = xmlNode.Attributes["OptHighSpeed"].Value;
				if (value8.Equals("true"))
				{
					info.m_flag.Set(1, true);
				}
			}
			if (xmlNode.Attributes["OpThirdJump"] != null)
			{
				string value9 = xmlNode.Attributes["OpThirdJump"].Value;
				if (value9.Equals("true"))
				{
					info.m_flag.Set(2, true);
				}
			}
			if (info.m_ID != CharaType.UNKNOWN && info.m_attribute != CharacterAttribute.UNKNOWN && info.m_teamAttribute != TeamAttribute.UNKNOWN)
			{
				list.Add(info);
			}
		}
	}

	private void SetNameList()
	{
		if (this.m_list == null)
		{
			return;
		}
		CharacterDataNameInfo.m_prefixNameList = new string[this.m_list.Count];
		CharacterDataNameInfo.m_charaNameList = new string[this.m_list.Count];
		CharacterDataNameInfo.m_charaNameLowerList = new string[this.m_list.Count];
		foreach (CharacterDataNameInfo.Info current in this.m_list)
		{
			if (current.m_ID < (CharaType)this.m_list.Count)
			{
				CharacterDataNameInfo.m_prefixNameList[(int)current.m_ID] = current.m_hud_suffix;
				CharacterDataNameInfo.m_charaNameList[(int)current.m_ID] = current.m_name;
				CharacterDataNameInfo.m_charaNameLowerList[(int)current.m_ID] = current.m_name.ToLower();
			}
		}
	}

	public static void LoadSetup()
	{
		GameObject gameObject = GameObject.Find("CharacterDataNameInfo");
		if (gameObject != null && gameObject.transform.parent != null && gameObject.transform.parent.name == "ETC")
		{
			gameObject.transform.parent = null;
		}
	}
}
