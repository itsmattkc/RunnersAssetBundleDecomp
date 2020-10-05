using System;
using System.Xml;
using UnityEngine;

public class EventBossParamTable : MonoBehaviour
{
	public struct DropRingParam
	{
		public int rate;

		public int normal;

		public int rare;

		public int srare;
	}

	public class BossParam
	{
		public EventBossParamTable.DropRingParam[] m_rings = new EventBossParamTable.DropRingParam[10];
	}

	public const int ITEM_COUNT_MAX = 12;

	public const int RING_LEVEL_COUNT_MAX = 10;

	public const int TBL_COUNT_MAX = 1;

	[SerializeField]
	private TextAsset m_dataTabel;

	private static readonly string[] ITEM_NAMES = new string[]
	{
		"Level1",
		"Level2",
		"Level3",
		"Level4",
		"WispRatio",
		"WispRatioDown",
		"BoostAttack1",
		"BoostAttack2",
		"BoostAttack3",
		"BoostSpeed1",
		"BoostSpeed2",
		"BoostSpeed3"
	};

	private static readonly string[] RING_DROP_LEVEL_NAMES = new string[]
	{
		"RingDropLevel1",
		"RingDropLevel2",
		"RingDropLevel3",
		"RingDropLevel4",
		"RingDropLevel5",
		"RingDropLevel6",
		"RingDropLevel7",
		"RingDropLevel8",
		"RingDropLevel9",
		"RingDropLevel10"
	};

	private float[] m_tblInfo;

	private EventBossParamTable.BossParam m_newTblInfo;

	private int m_tblCount;

	private static EventBossParamTable s_instance = null;

	public static EventBossParamTable Instance
	{
		get
		{
			return EventBossParamTable.s_instance;
		}
	}

	private void Awake()
	{
		if (EventBossParamTable.s_instance == null)
		{
			EventBossParamTable.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (EventBossParamTable.s_instance == this)
		{
			EventBossParamTable.s_instance = null;
		}
	}

	private void Start()
	{
		this.Setup();
	}

	public static string GetItemName(uint index)
	{
		if (index < 12u && (ulong)index < (ulong)((long)EventBossParamTable.ITEM_NAMES.Length))
		{
			return EventBossParamTable.ITEM_NAMES[(int)((UIntPtr)index)];
		}
		return string.Empty;
	}

	public static string GetRingDropName(uint index)
	{
		if (index < 10u && (ulong)index < (ulong)((long)EventBossParamTable.RING_DROP_LEVEL_NAMES.Length))
		{
			return EventBossParamTable.RING_DROP_LEVEL_NAMES[(int)((UIntPtr)index)];
		}
		return string.Empty;
	}

	private void Setup()
	{
		if (this.m_tblInfo == null)
		{
			this.m_tblInfo = new float[12];
		}
		if (this.m_newTblInfo == null)
		{
			this.m_newTblInfo = new EventBossParamTable.BossParam();
		}
		TextAsset dataTabel = this.m_dataTabel;
		if (dataTabel)
		{
			string xml = AESCrypt.Decrypt(dataTabel.text);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			EventBossParamTable.CreateTable(xmlDocument, this.m_tblInfo, this.m_newTblInfo, out this.m_tblCount);
			if (this.m_tblCount == 0)
			{
			}
		}
	}

	public static void CreateTable(XmlDocument doc, float[] data, EventBossParamTable.BossParam param, out int tbl_count)
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
		XmlNodeList xmlNodeList = doc.DocumentElement.SelectNodes("EventBossParamTable");
		if (xmlNodeList == null || xmlNodeList.Count == 0)
		{
			return;
		}
		int num = 0;
		foreach (XmlNode xmlNode in xmlNodeList)
		{
			XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("Item");
			foreach (XmlNode xmlNode2 in xmlNodeList2)
			{
				for (int i = 0; i < 12; i++)
				{
					string itemName = EventBossParamTable.GetItemName((uint)i);
					XmlAttribute xmlAttribute = xmlNode2.Attributes[itemName];
					float num2 = 0f;
					if (xmlAttribute != null)
					{
						num2 = float.Parse(xmlNode2.Attributes[itemName].Value);
					}
					int num3 = num * 12 + i;
					data[num3] = num2;
				}
			}
			XmlNodeList xmlNodeList3 = xmlNode.SelectNodes("Ring");
			int num4 = 0;
			foreach (XmlNode xmlNode3 in xmlNodeList3)
			{
				if (num4 < 10)
				{
					int rate = 0;
					if (int.TryParse(xmlNode3.Attributes["SurperRingRate"].Value, out rate))
					{
						param.m_rings[num4].rate = rate;
					}
					int normal = 0;
					if (int.TryParse(xmlNode3.Attributes["Normal"].Value, out normal))
					{
						param.m_rings[num4].normal = normal;
					}
					int rare = 0;
					if (int.TryParse(xmlNode3.Attributes["Rare"].Value, out rare))
					{
						param.m_rings[num4].rare = rare;
					}
					int srare = 0;
					if (int.TryParse(xmlNode3.Attributes["SRare"].Value, out srare))
					{
						param.m_rings[num4].srare = srare;
					}
					num4++;
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

	public float GetData(EventBossParamTableItem item_index)
	{
		int num = 0;
		if (this.m_tblInfo != null && (ulong)num < (ulong)((long)this.m_tblCount))
		{
			int num2 = (int)(num * 12 + item_index);
			if (num2 < this.m_tblInfo.Length)
			{
				return this.m_tblInfo[num2];
			}
		}
		return 0f;
	}

	public int GetData(BossType bossType, int playerAggressivity)
	{
		if (this.m_newTblInfo != null)
		{
			for (int i = 0; i < 10; i++)
			{
				int num = -1;
				switch (bossType)
				{
				case BossType.EVENT1:
					num = this.m_newTblInfo.m_rings[i].normal;
					break;
				case BossType.EVENT2:
					num = this.m_newTblInfo.m_rings[i].rare;
					break;
				case BossType.EVENT3:
					num = this.m_newTblInfo.m_rings[i].srare;
					break;
				}
				if (num > 0 && playerAggressivity <= num)
				{
					return this.m_newTblInfo.m_rings[i].rate;
				}
			}
		}
		return 0;
	}

	private float GetData(int index)
	{
		return this.GetData((EventBossParamTableItem)index);
	}

	public static void LoadSetup()
	{
		EventBossParamTable instance = EventBossParamTable.Instance;
		if (instance == null)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.EVENT_RESOURCE, "EventBossParamTable");
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
		}
	}

	public static float GetItemData(EventBossParamTableItem item_index)
	{
		EventBossParamTable instance = EventBossParamTable.Instance;
		if (instance != null)
		{
			return instance.GetData(item_index);
		}
		return 0f;
	}

	public static int GetSuperRingDropData(BossType bossType, int playerAggressivity)
	{
		if (EventBossParamTable.Instance != null)
		{
			return EventBossParamTable.Instance.GetData(bossType, playerAggressivity);
		}
		return 0;
	}
}
