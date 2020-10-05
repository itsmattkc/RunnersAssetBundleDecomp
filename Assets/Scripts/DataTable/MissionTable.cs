using System;
using System.IO;
using System.Xml.Serialization;
using Text;
using UnityEngine;

namespace DataTable
{
	public class MissionTable : MonoBehaviour
	{
		[SerializeField]
		private TextAsset m_missionTabel;

		private static MissionData[] m_missionDataTable;

		private static MissionTable s_instance;

		public static MissionTable Instance
		{
			get
			{
				return MissionTable.s_instance;
			}
		}

		private void Awake()
		{
			if (MissionTable.s_instance == null)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				MissionTable.s_instance = this;
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void OnDestroy()
		{
			if (MissionTable.s_instance == this)
			{
				MissionTable.m_missionDataTable = null;
				MissionTable.s_instance = null;
			}
		}

		private void Start()
		{
			if (MissionTable.m_missionDataTable == null)
			{
				string s = AESCrypt.Decrypt(this.m_missionTabel.text);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(MissionData[]));
				StringReader textReader = new StringReader(s);
				MissionTable.m_missionDataTable = (MissionData[])xmlSerializer.Deserialize(textReader);
			}
		}

		public static MissionData[] GetDataTable()
		{
			return MissionTable.m_missionDataTable;
		}

		public static MissionData GetMissionData(int id)
		{
			if (MissionTable.GetDataTable() == null)
			{
				return null;
			}
			MissionData[] dataTable = MissionTable.GetDataTable();
			for (int i = 0; i < dataTable.Length; i++)
			{
				MissionData missionData = dataTable[i];
				if (missionData.id == id)
				{
					return missionData;
				}
			}
			return null;
		}

		public static MissionData GetMissionDataOfIndex(int index)
		{
			return MissionTable.m_missionDataTable[index];
		}

		public static void LoadSetup()
		{
			GameObject gameObject = GameObject.Find("MissionTable");
			if (gameObject != null)
			{
				MissionData[] dataTable = MissionTable.GetDataTable();
				if (dataTable != null)
				{
					MissionData[] array = dataTable;
					for (int i = 0; i < array.Length; i++)
					{
						MissionData missionData = array[i];
						int type = (int)missionData.type;
						TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "DailyMission", "caption" + type);
						if (text != null)
						{
							missionData.SetText(text.text);
						}
					}
				}
				if (gameObject.transform.parent != null && gameObject.transform.parent.name == "ETC")
				{
					gameObject.transform.parent = null;
				}
			}
		}
	}
}
