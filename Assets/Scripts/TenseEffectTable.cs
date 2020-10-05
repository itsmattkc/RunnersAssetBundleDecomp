using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TenseEffectTable : MonoBehaviour
{
	[SerializeField]
	private TextAsset m_dataTabel;

	private List<TenseParameter> m_editParameters = new List<TenseParameter>();

	private static TenseEffectTable s_instance;

	public static TenseEffectTable Instance
	{
		get
		{
			return TenseEffectTable.s_instance;
		}
	}

	private void Awake()
	{
		if (TenseEffectTable.s_instance == null)
		{
			TenseEffectTable.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (TenseEffectTable.s_instance == this)
		{
			TenseEffectTable.s_instance = null;
		}
	}

	private void Start()
	{
		this.Setup();
	}

	private void Setup()
	{
		TextAsset dataTabel = this.m_dataTabel;
		if (dataTabel)
		{
			string xml = AESCrypt.Decrypt(dataTabel.text);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			TenseEffectTable.CreateTable(xmlDocument, out this.m_editParameters);
			if (this.m_editParameters.Count == 0)
			{
			}
		}
	}

	public static void CreateTable(XmlDocument doc, out List<TenseParameter> outdata)
	{
		outdata = new List<TenseParameter>();
		outdata.Clear();
		if (doc == null)
		{
			return;
		}
		if (doc.DocumentElement == null)
		{
			return;
		}
		XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("TenseEffectTable");
		if (xmlNodeList == null || xmlNodeList.Count == 0)
		{
			return;
		}
		foreach (XmlNode xmlNode in xmlNodeList)
		{
			string name_ = string.Empty;
			XmlAttribute xmlAttribute = xmlNode.Attributes["item"];
			if (xmlAttribute != null)
			{
				name_ = xmlAttribute.Value;
			}
			float r = 0f;
			XmlAttribute xmlAttribute2 = xmlNode.Attributes["r"];
			if (xmlAttribute2 != null)
			{
				r = float.Parse(xmlAttribute2.Value);
			}
			float g = 0f;
			XmlAttribute xmlAttribute3 = xmlNode.Attributes["g"];
			if (xmlAttribute3 != null)
			{
				g = float.Parse(xmlAttribute3.Value);
			}
			float b = 0f;
			XmlAttribute xmlAttribute4 = xmlNode.Attributes["b"];
			if (xmlAttribute4 != null)
			{
				b = float.Parse(xmlAttribute4.Value);
			}
			float a = 0f;
			XmlAttribute xmlAttribute5 = xmlNode.Attributes["a"];
			if (xmlAttribute5 != null)
			{
				a = float.Parse(xmlAttribute5.Value);
			}
			TenseParameter item = new TenseParameter(name_, new Color(r, g, b, a));
			outdata.Add(item);
		}
	}

	public bool IsSetupEnd()
	{
		return this.m_editParameters.Count != 0;
	}

	public Color GetData(string itemName)
	{
		if (this.IsSetupEnd())
		{
			foreach (TenseParameter current in this.m_editParameters)
			{
				if (current.name == itemName)
				{
					return current.color;
				}
			}
		}
		return Color.white;
	}

	public static Color GetItemData(string itemName)
	{
		TenseEffectTable instance = TenseEffectTable.Instance;
		if (instance != null)
		{
			return instance.GetData(itemName);
		}
		return Color.white;
	}
}
