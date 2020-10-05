using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class StageSuggestedDataTable : MonoBehaviour
{
	[SerializeField]
	private TextAsset m_xml_data;

	private StageSuggestedData[] m_data;

	private static StageSuggestedDataTable instance;

	public static StageSuggestedDataTable Instance
	{
		get
		{
			return StageSuggestedDataTable.instance;
		}
	}

	protected void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
		this.SetData();
		base.enabled = false;
	}

	private void OnDestroy()
	{
		if (StageSuggestedDataTable.instance == this)
		{
			StageSuggestedDataTable.instance = null;
		}
	}

	private void SetInstance()
	{
		if (StageSuggestedDataTable.instance == null)
		{
			StageSuggestedDataTable.instance = this;
		}
		else if (this != StageSuggestedDataTable.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void SetData()
	{
		if (this.m_xml_data != null)
		{
			string s = AESCrypt.Decrypt(this.m_xml_data.text);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(StageSuggestedData[]));
			StringReader textReader = new StringReader(s);
			this.m_data = (StageSuggestedData[])xmlSerializer.Deserialize(textReader);
			if (this.m_data != null)
			{
				Array.Sort<StageSuggestedData>(this.m_data);
			}
		}
	}

	public CharacterAttribute[] GetStageSuggestedData(int stageIndex)
	{
		if (this.m_data != null)
		{
			int num = this.m_data.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.m_data[i].id == stageIndex)
				{
					return this.m_data[i].charaAttribute;
				}
			}
		}
		return null;
	}
}
