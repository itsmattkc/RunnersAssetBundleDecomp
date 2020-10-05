using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class MileageMapBGDataTable : MonoBehaviour
{
	[SerializeField]
	private TextAsset m_xml_data;

	private MileageMapBGData[] m_bg_data;

	private static MileageMapBGDataTable instance;

	public static MileageMapBGDataTable Instance
	{
		get
		{
			return MileageMapBGDataTable.instance;
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
		if (MileageMapBGDataTable.instance == this)
		{
			MileageMapBGDataTable.instance = null;
		}
	}

	private void SetInstance()
	{
		if (MileageMapBGDataTable.instance == null)
		{
			MileageMapBGDataTable.instance = this;
		}
		else if (this != MileageMapBGDataTable.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void SetData()
	{
		if (this.m_xml_data != null)
		{
			string s = AESCrypt.Decrypt(this.m_xml_data.text);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(MileageMapBGData[]));
			StringReader textReader = new StringReader(s);
			this.m_bg_data = (MileageMapBGData[])xmlSerializer.Deserialize(textReader);
			if (this.m_bg_data != null)
			{
				Array.Sort<MileageMapBGData>(this.m_bg_data);
			}
		}
	}

	public string GetTextureName(int id)
	{
		if (this.m_bg_data != null)
		{
			int num = this.m_bg_data.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.m_bg_data[i].id == id)
				{
					return this.m_bg_data[i].texture_name;
				}
			}
		}
		return null;
	}

	public string GetWindowTextureName(int id)
	{
		if (this.m_bg_data != null)
		{
			int num = this.m_bg_data.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.m_bg_data[i].id == id)
				{
					return this.m_bg_data[i].window_texture_name;
				}
			}
		}
		return null;
	}
}
