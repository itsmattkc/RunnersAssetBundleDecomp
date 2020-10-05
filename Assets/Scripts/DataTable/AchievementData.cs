using System;
using System.Runtime.CompilerServices;

namespace DataTable
{
	public class AchievementData
	{
		public enum Type
		{
			ANIMAL,
			DISTANCE,
			PLAYER_OPEN,
			PLAYER_LEVEL,
			CHAO_OPEN,
			CHAO_LEVEL,
			COUNT,
			NONE = -1
		}

		public const int ID_MIN_VALUE = 1;

		private int _number_k__BackingField;

		private string _explanation_k__BackingField;

		private AchievementData.Type _type_k__BackingField;

		private int _itemID_k__BackingField;

		private int _value_k__BackingField;

		private string _iosId_k__BackingField;

		private string _androidId_k__BackingField;

		public int number
		{
			get;
			set;
		}

		public string explanation
		{
			get;
			set;
		}

		public AchievementData.Type type
		{
			get;
			set;
		}

		public int itemID
		{
			get;
			set;
		}

		public int value
		{
			get;
			set;
		}

		public string iosId
		{
			get;
			set;
		}

		public string androidId
		{
			get;
			set;
		}

		public AchievementData()
		{
		}

		public AchievementData(int number, string explanation, AchievementData.Type type, int itemID, int value, string iosId, string androidId)
		{
			this.number = number;
			this.explanation = explanation;
			this.type = type;
			this.itemID = itemID;
			this.value = value;
			this.iosId = iosId;
			this.androidId = androidId;
		}

		public string GetID()
		{
			return this.androidId;
		}
	}
}
