using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class MileageMapPointDataTable : MonoBehaviour
{
	[SerializeField]
	private TextAsset m_xml_data;

	private MileageMapPointData[] m_point_data;

	private static MileageMapPointDataTable instance;

	public static MileageMapPointDataTable Instance
	{
		get
		{
			return MileageMapPointDataTable.instance;
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
		if (MileageMapPointDataTable.instance == this)
		{
			MileageMapPointDataTable.instance = null;
		}
	}

	private void SetInstance()
	{
		if (MileageMapPointDataTable.instance == null)
		{
			MileageMapPointDataTable.instance = this;
		}
		else if (this != MileageMapPointDataTable.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void SetData()
	{
		if (this.m_xml_data != null)
		{
			string s = AESCrypt.Decrypt(this.m_xml_data.text);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(MileageMapPointData[]));
			StringReader textReader = new StringReader(s);
			this.m_point_data = (MileageMapPointData[])xmlSerializer.Deserialize(textReader);
			if (this.m_point_data != null)
			{
				Array.Sort<MileageMapPointData>(this.m_point_data);
			}
		}
	}

	public string GetTextureName(int id)
	{
		if (this.m_point_data != null)
		{
			int num = this.m_point_data.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.m_point_data[i].id == id)
				{
					return this.m_point_data[i].texture_name;
				}
			}
		}
		return null;
	}
}
