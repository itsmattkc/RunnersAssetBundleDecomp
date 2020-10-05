using SaveData;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Text;

namespace DataTable
{
	public class ChaoData : IComparable
	{
		public enum Rarity
		{
			NORMAL,
			RARE,
			SRARE,
			NONE
		}

		private const string SP_TEXT_COLOR = "[ffff00]";

		private const string SP_LOADING_TEXT_COLOR = "[e0b000]";

		private const string TEXT_WHITE_COLOR = "[ffffff]";

		public const int ID_NONE = -1;

		public const int ID_MIN = 0;

		public const int LEVEL_NONE = -1;

		public const int LEVEL_MIN = 0;

		public const int LEVEL_MAX = 10;

		private int m_currentAbility;

		private string[] m_abilityStatus;

		private ChaoDataAbilityStatus[] m_abilityStatusData;

		private int _id_k__BackingField;

		private ChaoData.Rarity _rarity_k__BackingField;

		private CharacterAttribute _charaAtribute_k__BackingField;

		private ChaoAbility[] _chaoAbilitys_k__BackingField;

		private string _name_k__BackingField;

		private string _nameTwolines_k__BackingField;

		private int _index_k__BackingField;

		public int id
		{
			get;
			private set;
		}

		public ChaoData.Rarity rarity
		{
			get;
			private set;
		}

		public CharacterAttribute charaAtribute
		{
			get;
			private set;
		}

		public int currentAbility
		{
			get
			{
				return this.m_currentAbility;
			}
			set
			{
				this.m_currentAbility = value;
				if (this.m_currentAbility >= this.abilityNum)
				{
					this.m_currentAbility = this.abilityNum - 1;
				}
				if (this.m_currentAbility < 0)
				{
					this.m_currentAbility = 0;
				}
			}
		}

		public int abilityNum
		{
			get
			{
				return this.m_abilityStatus.Length;
			}
		}

		public ChaoAbility[] chaoAbilitys
		{
			get;
			set;
		}

		public string[] abilityStatus
		{
			get
			{
				return this.m_abilityStatus;
			}
			set
			{
				this.m_abilityStatus = value;
				this.m_abilityStatusData = new ChaoDataAbilityStatus[this.m_abilityStatus.Length];
				for (int i = 0; i < this.m_abilityStatus.Length; i++)
				{
					this.m_abilityStatusData[i] = new ChaoDataAbilityStatus(this.m_abilityStatus[i], this.id, i);
				}
			}
		}

		public ChaoAbility chaoAbility
		{
			get
			{
				return this.chaoAbilitys[this.currentAbility];
			}
		}

		public float[] abilityValue
		{
			get
			{
				return this.GetAbilityValues();
			}
		}

		public float[] bonusAbilityValue
		{
			get
			{
				return this.GetBonusAbilityValues();
			}
		}

		public int eventId
		{
			get
			{
				return this.GetAbilityEventId();
			}
		}

		public float extraValue
		{
			get
			{
				return this.GetAbilitiyExtraValue();
			}
		}

		public string name
		{
			get;
			set;
		}

		public string nameTwolines
		{
			get;
			set;
		}

		public string details
		{
			get
			{
				return this.m_abilityStatusData[this.currentAbility].details;
			}
		}

		public string loadingDetails
		{
			get
			{
				return this.m_abilityStatusData[this.currentAbility].loadingDetails;
			}
		}

		public string loadingLongDetails
		{
			get
			{
				return this.m_abilityStatusData[this.currentAbility].loadingLongDetails;
			}
		}

		public string growDetails
		{
			get
			{
				return this.m_abilityStatusData[this.currentAbility].growDetails;
			}
		}

		public string menuDetails
		{
			get
			{
				return this.m_abilityStatusData[this.currentAbility].menuDetails;
			}
		}

		public string bgmName
		{
			get
			{
				return this.m_abilityStatusData[this.currentAbility].bgmName;
			}
		}

		public string cueSheetName
		{
			get
			{
				return this.m_abilityStatusData[this.currentAbility].cueSheetName;
			}
		}

		public int index
		{
			get;
			set;
		}

		public List<int> eventIdList
		{
			get
			{
				List<int> list = null;
				if (this.m_abilityStatusData != null && this.m_abilityStatusData.Length > 0)
				{
					list = new List<int>();
					int num = this.m_abilityStatusData.Length;
					for (int i = 0; i < num; i++)
					{
						list.Add(this.m_abilityStatusData[i].eventId);
					}
				}
				return list;
			}
		}

		public bool IsValidate
		{
			get
			{
				return this.id != -1 && this.level != -1;
			}
		}

		public int level
		{
			get
			{
				return (this.id == -1) ? (-1) : this.GetChaoLevel();
			}
		}

		public string spriteNameSuffix
		{
			get
			{
				return this.id.ToString("D4");
			}
		}

		public ChaoData()
		{
		}

		public ChaoData(ChaoData src)
		{
			this.id = src.id;
			this.rarity = src.rarity;
			this.charaAtribute = src.charaAtribute;
			this.currentAbility = src.currentAbility;
			this.chaoAbilitys = new ChaoAbility[src.chaoAbilitys.Length];
			for (int i = 0; i < src.chaoAbilitys.Length; i++)
			{
				this.chaoAbilitys[i] = src.chaoAbilitys[i];
			}
			this.m_abilityStatus = new string[src.m_abilityStatus.Length];
			for (int j = 0; j < src.m_abilityStatus.Length; j++)
			{
				this.m_abilityStatus[j] = src.m_abilityStatus[j];
			}
			this.m_abilityStatusData = new ChaoDataAbilityStatus[src.m_abilityStatusData.Length];
			for (int k = 0; k < src.m_abilityStatusData.Length; k++)
			{
				this.m_abilityStatusData[k] = src.m_abilityStatusData[k];
			}
			this.name = src.name;
			this.nameTwolines = src.nameTwolines;
			this.index = src.index;
		}

		public int CompareTo(object obj)
		{
			return this.id - ((ChaoData)obj).id;
		}

		public static string GetSpriteNameSuffix(int id)
		{
			return id.ToString("D4");
		}

		private int GetChaoLevel()
		{
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null && instance.ChaoData != null && instance.ChaoData.Info != null)
			{
				SaveData.ChaoData.ChaoDataInfo[] info = instance.ChaoData.Info;
				for (int i = 0; i < info.Length; i++)
				{
					SaveData.ChaoData.ChaoDataInfo chaoDataInfo = info[i];
					if (chaoDataInfo.chao_id == this.id)
					{
						return chaoDataInfo.level;
					}
				}
			}
			return -1;
		}

		private int GetAbilityEventId()
		{
			ChaoDataAbilityStatus chaoDataAbilityStatus = this.m_abilityStatusData[this.currentAbility];
			return chaoDataAbilityStatus.eventId;
		}

		private float GetAbilitiyExtraValue()
		{
			ChaoDataAbilityStatus chaoDataAbilityStatus = this.m_abilityStatusData[this.currentAbility];
			return chaoDataAbilityStatus.extraValue;
		}

		private float[] GetAbilityValues()
		{
			ChaoDataAbilityStatus chaoDataAbilityStatus = this.m_abilityStatusData[this.currentAbility];
			return chaoDataAbilityStatus.abilityValues;
		}

		private float[] GetBonusAbilityValues()
		{
			ChaoDataAbilityStatus chaoDataAbilityStatus = this.m_abilityStatusData[this.currentAbility];
			return chaoDataAbilityStatus.bonusAbilityValues;
		}

		public string GetFeaturedDetail()
		{
			string cellID = "featured_footnote" + this.id.ToString("D4");
			return TextUtility.GetChaoText("Chao", cellID);
		}

		public string GetDetailLevelPlusSP(int chaoLevel)
		{
			return this.GetDetailLevelPlusSP(chaoLevel, "[ffff00]");
		}

		public string GetLoadingPageDetailLevelPlusSP(int chaoLevel)
		{
			return this.GetLoadingPageDetailLevelPlusSP(chaoLevel, "[e0b000]");
		}

		private string GetDetailLevelPlusSP(int chaoLevel, string color)
		{
			string text = this.GetSPDetailsLevel(chaoLevel);
			if (string.IsNullOrEmpty(text))
			{
				text = this.GetDetailsLevel(chaoLevel);
			}
			else
			{
				text = color + text + "[-]";
				text += "\n";
				text += this.GetDetailsLevel(chaoLevel);
			}
			return text;
		}

		private string GetLoadingPageDetailLevelPlusSP(int chaoLevel, string color)
		{
			string text = this.GetSPLoadingLongDetailsLevel(chaoLevel);
			if (string.IsNullOrEmpty(text))
			{
				text = this.GetLoadingLongDetailsLevel(chaoLevel);
			}
			else
			{
				text = color + text + "[-]";
				text += "\n";
				text += this.GetLoadingLongDetailsLevel(chaoLevel);
			}
			return text;
		}

		public string GetSPLoadingLongDetailsLevel(int chaoLevel)
		{
			string result = string.Empty;
			for (int i = 0; i < this.abilityNum; i++)
			{
				this.currentAbility = i;
				if (EventManager.IsVaildEvent(this.eventId))
				{
					result = this.GetLoadingLongDetailsLevel(chaoLevel);
					break;
				}
			}
			this.currentAbility = 0;
			return result;
		}

		private bool IsRateText(string text)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(text) && text.IndexOf("{RATE") != -1)
			{
				result = true;
			}
			return result;
		}

		private Dictionary<string, string> CreateReplacesDic(string targetText, float param1, float param2)
		{
			return this.CreateReplacesDic(targetText, new List<float>
			{
				param1,
				param2
			});
		}

		private Dictionary<string, string> CreateReplacesDic(string targetText, float param1, float param2, float param3, float param4)
		{
			return this.CreateReplacesDic(targetText, new List<float>
			{
				param1,
				param2,
				param3,
				param4
			});
		}

		private Dictionary<string, string> CreateReplacesDic(string targetText, List<float> paramList)
		{
			if (paramList == null || paramList.Count <= 0)
			{
				return null;
			}
			int count = paramList.Count;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			bool flag = this.IsRateText(targetText);
			for (int i = 0; i < count; i++)
			{
				int num = i + 1;
				int num2 = (int)paramList[i];
				dictionary.Add("{PARAM" + num + "}", num2.ToString());
				if (flag)
				{
					float num3 = (paramList[i] + 100f) / 100f;
					dictionary.Add("{RATE" + num + "}", num3.ToString());
				}
			}
			return dictionary;
		}

		public string GetLoadingLongDetailsLevel(int chaoLevel)
		{
			if (ChaoTableUtility.IsKingArthur(this.id))
			{
				return this.GetKingArtherDetailsLevel(chaoLevel, true);
			}
			string loadingLongDetails = this.loadingLongDetails;
			float param = 0f;
			float param2 = 0f;
			if (chaoLevel >= 0)
			{
				if (chaoLevel < this.abilityValue.Length)
				{
					param = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					param2 = this.bonusAbilityValue[chaoLevel];
				}
			}
			Dictionary<string, string> replaceDic = this.CreateReplacesDic(loadingLongDetails, param, param2);
			return TextUtility.Replaces(loadingLongDetails, replaceDic);
		}

		public string GetDetailsLevel(int chaoLevel)
		{
			if (ChaoTableUtility.IsKingArthur(this.id))
			{
				return this.GetKingArtherDetailsLevel(chaoLevel, false);
			}
			string details = this.details;
			float param = 0f;
			float param2 = 0f;
			if (chaoLevel >= 0)
			{
				if (chaoLevel < this.abilityValue.Length)
				{
					param = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					param2 = this.bonusAbilityValue[chaoLevel];
				}
			}
			Dictionary<string, string> replaceDic = this.CreateReplacesDic(details, param, param2);
			return TextUtility.Replaces(details, replaceDic);
		}

		public string GetKingArtherDetailsLevel(int chaoLevel, bool loadingLoingDetalFlag)
		{
			string text = (!loadingLoingDetalFlag) ? this.details : this.loadingLongDetails;
			float param = 0f;
			float param2 = 0f;
			float param3 = 0f;
			float param4 = 0f;
			if (chaoLevel >= 0)
			{
				if (chaoLevel < this.abilityValue.Length)
				{
					param = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					param2 = this.bonusAbilityValue[chaoLevel];
				}
				this.currentAbility = 1;
				if (chaoLevel < this.abilityValue.Length)
				{
					param3 = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					param4 = this.bonusAbilityValue[chaoLevel];
				}
				this.currentAbility = 0;
			}
			Dictionary<string, string> replaceDic = this.CreateReplacesDic(text, param, param2, param3, param4);
			return TextUtility.Replaces(text, replaceDic);
		}

		public string GetSPDetailsLevel(int chaoLevel)
		{
			string result = string.Empty;
			for (int i = 0; i < this.abilityNum; i++)
			{
				this.currentAbility = i;
				if (EventManager.IsVaildEvent(this.eventId))
				{
					result = this.GetDetailsLevel(chaoLevel);
					break;
				}
			}
			this.currentAbility = 0;
			return result;
		}

		public string GetGrowDetailLevelPlusSP(int chaoLevel)
		{
			string text = this.GetSPGrowDetailsLevel(chaoLevel);
			if (string.IsNullOrEmpty(text))
			{
				text = this.GetGrowDetailsLevel(chaoLevel);
			}
			else
			{
				text += "\n\n";
				text += this.GetGrowDetailsLevel(chaoLevel);
			}
			return text;
		}

		public string GetGrowDetailsLevel(int chaoLevel)
		{
			if (ChaoTableUtility.IsKingArthur(this.id))
			{
				return this.GetKingArtherGrowDetailsLevel(chaoLevel);
			}
			float param = 0f;
			float param2 = 0f;
			float param3 = 0f;
			float param4 = 0f;
			int num = chaoLevel - 1;
			if (num >= 0)
			{
				if (chaoLevel < this.abilityValue.Length)
				{
					param = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					param2 = this.bonusAbilityValue[chaoLevel];
				}
				if (num < this.abilityValue.Length)
				{
					param3 = this.abilityValue[num];
				}
				if (num < this.bonusAbilityValue.Length)
				{
					param4 = this.bonusAbilityValue[num];
				}
			}
			Dictionary<string, string> replaceDic = this.CreateReplacesDic(this.growDetails, param, param2, param3, param4);
			return TextUtility.Replaces(this.growDetails, replaceDic);
		}

		private string GetKingArtherGrowDetailsLevel(int chaoLevel)
		{
			float[] array = new float[8];
			int num = chaoLevel - 1;
			if (num >= 0)
			{
				if (chaoLevel < this.abilityValue.Length)
				{
					array[0] = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					array[1] = this.bonusAbilityValue[chaoLevel];
				}
				if (num < this.abilityValue.Length)
				{
					array[2] = this.abilityValue[num];
				}
				if (num < this.bonusAbilityValue.Length)
				{
					array[3] = this.bonusAbilityValue[num];
				}
				this.currentAbility = 1;
				if (chaoLevel < this.abilityValue.Length)
				{
					array[4] = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					array[5] = this.bonusAbilityValue[chaoLevel];
				}
				if (num < this.abilityValue.Length)
				{
					array[6] = this.abilityValue[num];
				}
				if (num < this.bonusAbilityValue.Length)
				{
					array[7] = this.bonusAbilityValue[num];
				}
				this.currentAbility = 0;
			}
			List<float> paramList = new List<float>(array);
			Dictionary<string, string> replaceDic = this.CreateReplacesDic(this.growDetails, paramList);
			return TextUtility.Replaces(this.growDetails, replaceDic);
		}

		public string GetSPGrowDetailsLevel(int chaoLevel)
		{
			string text = string.Empty;
			for (int i = 0; i < this.abilityNum; i++)
			{
				this.currentAbility = i;
				if (EventManager.IsVaildEvent(this.eventId))
				{
					text = this.GetGrowDetailsLevel(chaoLevel);
					if (!string.IsNullOrEmpty(text))
					{
						text = "[ffff00]" + text + "[ffffff]";
					}
					break;
				}
			}
			this.currentAbility = 0;
			return text;
		}

		public string GetLoadingDetailsLevel(int chaoLevel)
		{
			if (ChaoTableUtility.IsKingArthur(this.id))
			{
				return this.GetKingArtherLoadingDetailsLevel(chaoLevel);
			}
			float param = 0f;
			float param2 = 0f;
			if (chaoLevel >= 0)
			{
				if (chaoLevel < this.abilityValue.Length)
				{
					param = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					param2 = this.bonusAbilityValue[chaoLevel];
				}
			}
			Dictionary<string, string> replaceDic = this.CreateReplacesDic(this.loadingDetails, param, param2);
			return TextUtility.Replaces(this.loadingDetails, replaceDic);
		}

		public string GetKingArtherLoadingDetailsLevel(int chaoLevel)
		{
			float param = 0f;
			float param2 = 0f;
			float param3 = 0f;
			float param4 = 0f;
			if (chaoLevel >= 0)
			{
				if (chaoLevel < this.abilityValue.Length)
				{
					param = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					param2 = this.bonusAbilityValue[chaoLevel];
				}
				this.currentAbility = 1;
				if (chaoLevel < this.abilityValue.Length)
				{
					param3 = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					param4 = this.bonusAbilityValue[chaoLevel];
				}
				this.currentAbility = 0;
			}
			Dictionary<string, string> replaceDic = this.CreateReplacesDic(this.loadingDetails, param, param2, param3, param4);
			return TextUtility.Replaces(this.loadingDetails, replaceDic);
		}

		public string GetSPLoadingDetailsLevel(int chaoLevel)
		{
			string result = string.Empty;
			for (int i = 0; i < this.abilityNum; i++)
			{
				this.currentAbility = i;
				if (EventManager.IsVaildEvent(this.eventId))
				{
					result = this.GetLoadingDetailsLevel(chaoLevel);
					break;
				}
			}
			this.currentAbility = 0;
			return result;
		}

		public string GetMainMenuDetailsLevel(int chaoLevel)
		{
			if (ChaoTableUtility.IsKingArthur(this.id))
			{
				return this.GetKingArtherMainMenuDetailsLevel(chaoLevel);
			}
			float param = 0f;
			float param2 = 0f;
			if (chaoLevel >= 0)
			{
				if (chaoLevel < this.abilityValue.Length)
				{
					param = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					param2 = this.bonusAbilityValue[chaoLevel];
				}
			}
			Dictionary<string, string> replaceDic = this.CreateReplacesDic(this.menuDetails, param, param2);
			return TextUtility.Replaces(this.menuDetails, replaceDic);
		}

		public string GetKingArtherMainMenuDetailsLevel(int chaoLevel)
		{
			float param = 0f;
			float param2 = 0f;
			float param3 = 0f;
			float param4 = 0f;
			if (chaoLevel >= 0)
			{
				if (chaoLevel < this.abilityValue.Length)
				{
					param = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					param2 = this.bonusAbilityValue[chaoLevel];
				}
				this.currentAbility = 1;
				if (chaoLevel < this.abilityValue.Length)
				{
					param3 = this.abilityValue[chaoLevel];
				}
				if (chaoLevel < this.bonusAbilityValue.Length)
				{
					param4 = this.bonusAbilityValue[chaoLevel];
				}
				this.currentAbility = 0;
			}
			Dictionary<string, string> replaceDic = this.CreateReplacesDic(this.menuDetails, param, param2, param3, param4);
			return TextUtility.Replaces(this.menuDetails, replaceDic);
		}

		public string GetSPMainMenuDetailsLevel(int chaoLevel)
		{
			string text = string.Empty;
			for (int i = 0; i < this.abilityNum; i++)
			{
				this.currentAbility = i;
				if (EventManager.IsVaildEvent(this.eventId))
				{
					text = this.GetMainMenuDetailsLevel(chaoLevel);
					if (!string.IsNullOrEmpty(text))
					{
						text = "[ffff00]" + text + "[-]";
					}
					break;
				}
			}
			this.currentAbility = 0;
			if (string.IsNullOrEmpty(text))
			{
				text = this.GetMainMenuDetailsLevel(chaoLevel);
			}
			return text;
		}

		public void StatusUpdate()
		{
			for (int i = 0; i < this.m_abilityStatusData.Length; i++)
			{
				this.m_abilityStatusData[i].update(this.id);
			}
		}

		public bool SetChaoAbility(int abilityEventId)
		{
			bool result = false;
			if (this.abilityNum > 1)
			{
				for (int i = 0; i < this.abilityNum; i++)
				{
					ChaoDataAbilityStatus chaoDataAbilityStatus = this.m_abilityStatusData[i];
					if (abilityEventId == chaoDataAbilityStatus.eventId)
					{
						this.currentAbility = i;
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public bool SetChaoAbilityIndex(int abilityIndex)
		{
			bool result = false;
			if (this.abilityNum > 1 && abilityIndex >= 0 && this.abilityNum > abilityIndex)
			{
				this.currentAbility = abilityIndex;
				result = true;
			}
			return result;
		}

		public void accept(ref ChaoDataVisitorBase visitor)
		{
			visitor.visit(this);
		}

		public static int ChaoCompareById(ChaoData x, ChaoData y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			return x.id - y.id;
		}

		public void SetDummyData()
		{
			this.id = 0;
			this.rarity = ChaoData.Rarity.NORMAL;
			this.charaAtribute = CharacterAttribute.SPEED;
			this.chaoAbilitys = new ChaoAbility[1];
			this.chaoAbilitys[0] = ChaoAbility.ALL_BONUS_COUNT;
			this.m_abilityStatus = new string[1];
			this.m_abilityStatus[0] = "dummy";
			this.m_abilityStatusData = new ChaoDataAbilityStatus[1];
			this.m_abilityStatusData[0] = new ChaoDataAbilityStatus("0,0", 0, 0);
			this.currentAbility = 0;
		}
	}
}
