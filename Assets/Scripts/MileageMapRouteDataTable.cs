using System;
using System.IO;
using System.Xml.Serialization;
using Text;
using UnityEngine;

public class MileageMapRouteDataTable : MonoBehaviour
{
	[SerializeField]
	private TextAsset m_xml_data;

	private MileageMapRouteData[] m_route_data;

	private static MileageMapRouteDataTable instance;

	public static MileageMapRouteDataTable Instance
	{
		get
		{
			return MileageMapRouteDataTable.instance;
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
		if (MileageMapRouteDataTable.instance == this)
		{
			MileageMapRouteDataTable.instance = null;
		}
	}

	private void SetInstance()
	{
		if (MileageMapRouteDataTable.instance == null)
		{
			MileageMapRouteDataTable.instance = this;
		}
		else if (this != MileageMapRouteDataTable.Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void SetData()
	{
		if (this.m_xml_data != null)
		{
			string s = AESCrypt.Decrypt(this.m_xml_data.text);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(MileageMapRouteData[]));
			StringReader textReader = new StringReader(s);
			this.m_route_data = (MileageMapRouteData[])xmlSerializer.Deserialize(textReader);
			if (this.m_route_data != null)
			{
				Array.Sort<MileageMapRouteData>(this.m_route_data);
			}
		}
	}

	public MileageMapRouteData GetMileageMapRouteData(int id)
	{
		if (this.m_route_data != null)
		{
			int num = this.m_route_data.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.m_route_data[i].id == id)
				{
					return this.m_route_data[i];
				}
			}
		}
		return null;
	}

	public MileageBonus GetBonusType(int id)
	{
		MileageBonus result = MileageBonus.UNKNOWN;
		if (this.m_route_data != null)
		{
			int num = this.m_route_data.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.m_route_data[i].id == id)
				{
					result = this.m_route_data[i].ability_type;
					break;
				}
			}
		}
		return result;
	}

	public string GetBonusTypeText(int id)
	{
		string bonusTypeTextWithoutColor = this.GetBonusTypeTextWithoutColor(id);
		if (bonusTypeTextWithoutColor != null)
		{
			string str = "[00d2ff]";
			return str + bonusTypeTextWithoutColor;
		}
		return null;
	}

	public string GetBonusTypeTextWithoutColor(int id)
	{
		if (this.m_route_data != null)
		{
			MileageBonus bonusType = this.GetBonusType(id);
			TextObject textObject = null;
			TextManager.TextType textType = TextManager.TextType.TEXTTYPE_COMMON_TEXT;
			switch (bonusType)
			{
			case MileageBonus.SCORE:
				textObject = TextManager.GetText(textType, "Score", "score");
				break;
			case MileageBonus.ANIMAL:
				textObject = TextManager.GetText(textType, "Score", "animal");
				break;
			case MileageBonus.RING:
				textObject = TextManager.GetText(textType, "Item", "get_ring");
				break;
			case MileageBonus.DISTANCE:
				textObject = TextManager.GetText(textType, "Score", "distance");
				break;
			case MileageBonus.FINAL_SCORE:
				textObject = TextManager.GetText(textType, "Score", "final_score");
				break;
			}
			if (textObject != null)
			{
				return textObject.text;
			}
		}
		return null;
	}
}
