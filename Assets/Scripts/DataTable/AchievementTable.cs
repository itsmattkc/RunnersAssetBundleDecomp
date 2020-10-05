using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace DataTable
{
	public class AchievementTable : MonoBehaviour
	{
		[SerializeField]
		private TextAsset m_achievementTabel;

		private static AchievementData[] m_achievementDataTable;

		private void Start()
		{
			if (AchievementTable.m_achievementDataTable == null)
			{
				string s = AESCrypt.Decrypt(this.m_achievementTabel.text);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(AchievementData[]));
				StringReader textReader = new StringReader(s);
				AchievementTable.m_achievementDataTable = (AchievementData[])xmlSerializer.Deserialize(textReader);
			}
		}

		private void OnDestroy()
		{
			AchievementTable.m_achievementDataTable = null;
		}

		public static AchievementData[] GetDataTable()
		{
			return AchievementTable.m_achievementDataTable;
		}

		public static int GetDataTableCount()
		{
			if (AchievementTable.m_achievementDataTable != null)
			{
				return AchievementTable.m_achievementDataTable.Length;
			}
			return 0;
		}

		public static AchievementData GetAchievementData(string id)
		{
			if (AchievementTable.GetDataTable() == null)
			{
				return null;
			}
			AchievementData[] dataTable = AchievementTable.GetDataTable();
			for (int i = 0; i < dataTable.Length; i++)
			{
				AchievementData achievementData = dataTable[i];
				if (achievementData.GetID() == id)
				{
					return achievementData;
				}
			}
			return null;
		}

		public static AchievementData GetAchievementDataOfIndex(int index)
		{
			if (AchievementTable.GetDataTable() == null)
			{
				return null;
			}
			if (index < AchievementTable.m_achievementDataTable.Length)
			{
				return AchievementTable.m_achievementDataTable[index];
			}
			return null;
		}
	}
}
