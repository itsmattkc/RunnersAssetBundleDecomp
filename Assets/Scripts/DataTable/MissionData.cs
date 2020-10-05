using System;
using System.Runtime.CompilerServices;

namespace DataTable
{
	public class MissionData
	{
		public enum Type
		{
			ENEMY,
			G_ENEMY,
			DISTANCE,
			ANIMAL,
			SCORE,
			RING,
			COUNT,
			NONE = -1
		}

		public const int ID_MIN_VALUE = 1;

		private int _id_k__BackingField;

		private MissionData.Type _type_k__BackingField;

		private string _text_k__BackingField;

		private int _quota_k__BackingField;

		private bool _save_k__BackingField;

		public int id
		{
			get;
			set;
		}

		public MissionData.Type type
		{
			get;
			set;
		}

		public string text
		{
			get;
			set;
		}

		public int quota
		{
			get;
			set;
		}

		public bool save
		{
			get;
			set;
		}

		public MissionData()
		{
		}

		public MissionData(int id, MissionData.Type type, string text, int quota, bool save)
		{
			this.id = id;
			this.type = type;
			this.text = text;
			this.quota = quota;
			this.save = save;
		}

		public void SetText(string text)
		{
			this.text = text;
		}
	}
}
