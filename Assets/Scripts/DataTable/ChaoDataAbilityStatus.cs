using System;
using System.Runtime.CompilerServices;
using Text;

namespace DataTable
{
	public class ChaoDataAbilityStatus
	{
		private int m_chaoId;

		private int m_abilityIndex;

		private string m_orgAbilityStatus;

		private int _maxLevel_k__BackingField;

		private int _eventId_k__BackingField;

		private float _extraValue_k__BackingField;

		private string _bgmName_k__BackingField;

		private string _cueSheetName_k__BackingField;

		private float[] _abilityValues_k__BackingField;

		private float[] _bonusAbilityValues_k__BackingField;

		private string _details_k__BackingField;

		private string _loadingDetails_k__BackingField;

		private string _loadingLongDetails_k__BackingField;

		private string _growDetails_k__BackingField;

		private string _menuDetails_k__BackingField;

		public int maxLevel
		{
			get;
			private set;
		}

		public int eventId
		{
			get;
			private set;
		}

		public float extraValue
		{
			get;
			private set;
		}

		public string bgmName
		{
			get;
			private set;
		}

		public string cueSheetName
		{
			get;
			private set;
		}

		public float[] abilityValues
		{
			get;
			private set;
		}

		public float[] bonusAbilityValues
		{
			get;
			private set;
		}

		public string details
		{
			get;
			private set;
		}

		public string loadingDetails
		{
			get;
			private set;
		}

		public string loadingLongDetails
		{
			get;
			private set;
		}

		public string growDetails
		{
			get;
			private set;
		}

		public string menuDetails
		{
			get;
			private set;
		}

		public ChaoDataAbilityStatus(string status, int id, int abilityIndex)
		{
			this.m_chaoId = id;
			this.m_abilityIndex = abilityIndex;
			this.m_orgAbilityStatus = status;
			this.update(id);
		}

		public ChaoDataAbilityStatus(ChaoDataAbilityStatus src)
		{
			this.m_chaoId = src.m_chaoId;
			this.m_orgAbilityStatus = src.m_orgAbilityStatus;
			this.maxLevel = src.maxLevel;
			this.eventId = src.eventId;
			this.extraValue = src.extraValue;
			this.bgmName = src.bgmName;
			this.cueSheetName = src.cueSheetName;
			this.abilityValues = new float[src.abilityValues.Length];
			for (int i = 0; i < src.abilityValues.Length; i++)
			{
				this.abilityValues[i] = src.abilityValues[i];
			}
			this.bonusAbilityValues = new float[src.bonusAbilityValues.Length];
			for (int j = 0; j < src.bonusAbilityValues.Length; j++)
			{
				this.bonusAbilityValues[j] = src.bonusAbilityValues[j];
			}
			this.details = src.details;
			this.loadingDetails = src.loadingDetails;
			this.growDetails = src.growDetails;
			this.menuDetails = src.menuDetails;
		}

		public void update(int id)
		{
			this.m_chaoId = id;
			int num = 4;
			string[] array = this.m_orgAbilityStatus.Split(new char[]
			{
				','
			});
			this.maxLevel = (array.Length - num) / 2;
			if (array.Length > 0)
			{
				this.eventId = int.Parse(array[0]);
			}
			if (array.Length > 1)
			{
				this.extraValue = float.Parse(array[1]);
			}
			if (array.Length > 2)
			{
				if (array[2] == "non")
				{
					this.cueSheetName = string.Empty;
				}
				else
				{
					this.cueSheetName = array[2];
				}
			}
			if (array.Length > 3)
			{
				if (array[3] == "non")
				{
					this.bgmName = string.Empty;
				}
				else
				{
					this.bgmName = array[3];
				}
			}
			if (this.maxLevel > 0 && array.Length >= this.maxLevel * 2 + num)
			{
				this.abilityValues = new float[this.maxLevel];
				this.bonusAbilityValues = new float[this.maxLevel];
				for (int i = 0; i < this.maxLevel; i++)
				{
					this.abilityValues[i] = float.Parse(array[i + num]);
					this.bonusAbilityValues[i] = float.Parse(array[i + num + this.maxLevel]);
				}
			}
			if (ChaoTableUtility.IsKingArthur(id) && this.m_abilityIndex == 1)
			{
				return;
			}
			this.details = this.GetDetailsText("details");
			this.growDetails = this.GetDetailsText("grow_details");
			this.loadingDetails = this.GetDetailsText("loading_details");
			this.menuDetails = this.GetDetailsText("main_menu_details");
			this.loadingLongDetails = this.GetDetailsText("loading_long_details");
		}

		private string GetDetailsText(string callId)
		{
			string chaoText;
			if (this.m_abilityIndex == 0)
			{
				chaoText = TextUtility.GetChaoText("Chao", callId + this.m_chaoId.ToString("D4"));
			}
			else
			{
				chaoText = TextUtility.GetChaoText("Chao", string.Concat(new object[]
				{
					callId,
					this.m_chaoId.ToString("D4"),
					"_",
					this.m_abilityIndex
				}));
			}
			return chaoText;
		}
	}
}
