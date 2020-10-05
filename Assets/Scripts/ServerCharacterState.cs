using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class ServerCharacterState
{
	public enum CharacterStatus
	{
		Locked,
		Unlocked,
		MaxLevel
	}

	public enum LockCondition
	{
		OPEN,
		MILEAGE_EPISODE,
		RING_OR_RED_STAR_RING,
		ROULETTE
	}

	public int m_currentUnlocks;

	public int m_numTokens;

	public int m_tokenCost;

	public List<int> AbilityLevel = new List<int>();

	public List<int> OldAbiltyLevel = new List<int>();

	public List<int> AbilityNumRings = new List<int>();

	private int _Id_k__BackingField;

	private ServerCharacterState.CharacterStatus _Status_k__BackingField;

	private ServerCharacterState.CharacterStatus _OldStatus_k__BackingField;

	private int _Level_k__BackingField;

	private int _Cost_k__BackingField;

	private int _OldCost_k__BackingField;

	private int _NumRedRings_k__BackingField;

	private ServerCharacterState.LockCondition _Condition_k__BackingField;

	private int _Exp_k__BackingField;

	private int _OldExp_k__BackingField;

	private int _star_k__BackingField;

	private int _starMax_k__BackingField;

	private int _priceNumRings_k__BackingField;

	private int _priceNumRedRings_k__BackingField;

	public int Id
	{
		get;
		set;
	}

	public ServerCharacterState.CharacterStatus Status
	{
		get;
		set;
	}

	public ServerCharacterState.CharacterStatus OldStatus
	{
		get;
		set;
	}

	public int Level
	{
		get;
		set;
	}

	public int Cost
	{
		get;
		set;
	}

	public int OldCost
	{
		get;
		set;
	}

	public int NumRedRings
	{
		get;
		set;
	}

	public ServerCharacterState.LockCondition Condition
	{
		get;
		set;
	}

	public int Exp
	{
		get;
		set;
	}

	public int OldExp
	{
		get;
		set;
	}

	public int star
	{
		get;
		set;
	}

	public int starMax
	{
		get;
		set;
	}

	public int priceNumRings
	{
		get;
		set;
	}

	public int priceNumRedRings
	{
		get;
		set;
	}

	public CharaType charaType
	{
		get
		{
			CharaType result = CharaType.UNKNOWN;
			if (this.Id >= 0)
			{
				ServerItem serverItem = new ServerItem((ServerItem.Id)this.Id);
				result = serverItem.charaType;
			}
			return result;
		}
	}

	public CharacterDataNameInfo.Info charaInfo
	{
		get
		{
			CharacterDataNameInfo.Info result = null;
			CharacterDataNameInfo instance = CharacterDataNameInfo.Instance;
			if (instance != null)
			{
				result = instance.GetDataByID(this.charaType);
			}
			return result;
		}
	}

	public bool IsBuy
	{
		get
		{
			bool result = false;
			if (this.starMax > 0 && this.star < this.starMax && (this.priceNumRings > 0 || this.priceNumRedRings > 0))
			{
				result = true;
			}
			return result;
		}
	}

	public bool IsRoulette
	{
		get
		{
			bool result = false;
			if (RouletteManager.Instance != null)
			{
				if (this.Condition == ServerCharacterState.LockCondition.ROULETTE)
				{
					result = true;
				}
				else if (RouletteManager.Instance.IsPicupChara(this.charaType))
				{
					result = true;
				}
			}
			return result;
		}
	}

	public int UnlockCost
	{
		get
		{
			if (this.Status == ServerCharacterState.CharacterStatus.Locked)
			{
				return this.Cost;
			}
			return -1;
		}
	}

	public int LevelUpCost
	{
		get
		{
			if (this.Status == ServerCharacterState.CharacterStatus.Unlocked)
			{
				return this.Cost;
			}
			return -1;
		}
	}

	public bool IsUnlocked
	{
		get
		{
			return ServerCharacterState.CharacterStatus.Locked != this.Status;
		}
	}

	public bool IsMaxLevel
	{
		get
		{
			return ServerCharacterState.CharacterStatus.MaxLevel == this.Status;
		}
	}

	public float QuickModeTimeExtension
	{
		get
		{
			float result = 0f;
			if (this.starMax > 0)
			{
				StageTimeTable stageTimeTable = GameObjectUtil.FindGameObjectComponent<StageTimeTable>("StageTimeTable");
				if (stageTimeTable != null)
				{
					float num = (float)stageTimeTable.GetTableItemData(StageTimeTableItem.OverlapBonus);
					result = (float)this.star * num;
				}
			}
			return result;
		}
	}

	public ServerCharacterState()
	{
		this.Id = -1;
		this.Status = ServerCharacterState.CharacterStatus.Locked;
		this.Level = 10;
		this.Cost = 0;
		this.star = 0;
		this.starMax = 0;
		this.priceNumRings = 0;
		this.priceNumRedRings = 0;
	}

	public Dictionary<ServerItem.Id, int> GetBuyCostItemList()
	{
		Dictionary<ServerItem.Id, int> dictionary = null;
		if (this.IsBuy)
		{
			if (this.priceNumRings > 0)
			{
				if (dictionary == null)
				{
					dictionary = new Dictionary<ServerItem.Id, int>();
				}
				dictionary.Add(ServerItem.Id.RING, this.priceNumRings);
			}
			if (this.priceNumRedRings > 0)
			{
				if (dictionary == null)
				{
					dictionary = new Dictionary<ServerItem.Id, int>();
				}
				dictionary.Add(ServerItem.Id.RSRING, this.priceNumRedRings);
			}
		}
		return dictionary;
	}

	public Dictionary<BonusParam.BonusType, float> GetStarBonusList()
	{
		Dictionary<BonusParam.BonusType, float> result = null;
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.ETC, "OverlapBonusTable");
		if (gameObject != null)
		{
			OverlapBonusTable component = gameObject.GetComponent<OverlapBonusTable>();
			if (component != null)
			{
				result = component.GetStarBonusList(this.charaType);
			}
		}
		return result;
	}

	public Dictionary<BonusParam.BonusType, float> GetTeamBonusList()
	{
		Dictionary<BonusParam.BonusType, float> result = null;
		BonusUtil.GetTeamBonus(this.charaType, out result);
		return result;
	}

	public string GetCharaAttributeText()
	{
		string text = string.Empty;
		string cellName = "chara_attribute_" + CharaName.Name[(int)this.charaType];
		if (!this.IsUnlocked)
		{
			cellName = "recommend_chara_unlock_text_" + CharaName.Name[(int)this.charaType];
			string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", cellName).text;
			if (string.IsNullOrEmpty(text2))
			{
				List<string> list = new List<string>();
				Dictionary<ServerItem.Id, int> buyCostItemList = this.GetBuyCostItemList();
				if (this.IsRoulette)
				{
					list.Add(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "GeneralWindow", "ui_Lbl_roulette").text);
				}
				if (buyCostItemList != null && buyCostItemList.Count > 0)
				{
					Dictionary<ServerItem.Id, int>.KeyCollection keys = buyCostItemList.Keys;
					foreach (ServerItem.Id current in keys)
					{
						ServerItem serverItem = new ServerItem(current);
						list.Add(serverItem.serverItemName);
					}
				}
				global::Debug.Log("nameList.Count:" + list.Count);
				if (list.Count > 0)
				{
					int num = list.Count;
					if (num > 3)
					{
						num = 3;
					}
					text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "recommend_chara_unlock_text_type_" + num).text;
					text = text2;
					for (int i = 0; i < num; i++)
					{
						text = text.Replace("{PARAM" + (i + 1) + "}", list[i]);
					}
					text = text.Replace("{BONUS}", this.GetTeamBonusText());
				}
				else
				{
					text = string.Empty;
				}
			}
			else
			{
				text = text2;
				text = text.Replace("{BONUS}", this.GetTeamBonusText());
			}
		}
		else
		{
			text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", cellName).text;
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Replace("{BONUS}", this.GetTeamBonusText());
			}
			else
			{
				text = "Unknown";
			}
		}
		if (this.starMax > 0 && this.star > 0)
		{
			string text3 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "chara_overlap_text_2").text;
			float quickModeTimeExtension = this.QuickModeTimeExtension;
			float num2 = 0f;
			Dictionary<BonusParam.BonusType, float> starBonusList = this.GetStarBonusList();
			if (starBonusList != null && starBonusList.Count > 0)
			{
				Dictionary<BonusParam.BonusType, float>.KeyCollection keys2 = starBonusList.Keys;
				using (Dictionary<BonusParam.BonusType, float>.KeyCollection.Enumerator enumerator2 = keys2.GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						BonusParam.BonusType current2 = enumerator2.Current;
						float num3 = starBonusList[current2];
						num2 = num3;
					}
				}
			}
			text3 = text3.Replace("{TIME}", quickModeTimeExtension.ToString());
			text3 = text3.Replace("{PARAM}", num2.ToString());
			text = text + "\n\n" + text3;
		}
		return text;
	}

	private string GetTeamBonusText()
	{
		string text = string.Empty;
		Dictionary<BonusParam.BonusType, float> teamBonusList = this.GetTeamBonusList();
		Dictionary<BonusParam.BonusType, float>.KeyCollection keys = teamBonusList.Keys;
		string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoSet", "bonus_percent").text;
		foreach (BonusParam.BonusType current in keys)
		{
			global::Debug.Log("GetTeamBonusText key:" + current);
			float num = teamBonusList[current];
			string text3 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ChaoSet", "ui_Lbl_bonusname_" + (int)current).text;
			if (current == BonusParam.BonusType.SPEED)
			{
				text3 = text3 + " " + text2.Replace("{BONUS}", (100f - num).ToString());
			}
			else if (current == BonusParam.BonusType.TOTAL_SCORE && Mathf.Abs(num) <= 1f)
			{
				num *= 100f;
				if (num >= 0f)
				{
					text3 = text3 + " +" + text2.Replace("{BONUS}", num.ToString());
				}
				else
				{
					text3 = text3 + " " + text2.Replace("{BONUS}", num.ToString());
				}
			}
			else if (num >= 0f)
			{
				text3 = text3 + " +" + text2.Replace("{BONUS}", num.ToString());
			}
			else
			{
				text3 = text3 + " " + text2.Replace("{BONUS}", num.ToString());
			}
			if (string.IsNullOrEmpty(text))
			{
				text = text3;
			}
			else
			{
				text = text + "\n" + text3;
			}
		}
		return text;
	}

	public bool IsExpGot()
	{
		if (this.OldExp < this.Exp)
		{
			return true;
		}
		if (this.OldAbiltyLevel.Count == this.AbilityLevel.Count)
		{
			int count = this.AbilityLevel.Count;
			for (int i = 0; i < count; i++)
			{
				int num = this.OldAbiltyLevel[i];
				int num2 = this.AbilityLevel[i];
				if (num < num2)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void Dump()
	{
		global::Debug.Log(string.Concat(new object[]
		{
			"Id=",
			this.Id,
			", Status=",
			this.Status,
			", Level=",
			this.Level,
			", Cost=",
			this.Cost,
			", UnlockCost=",
			this.UnlockCost,
			", LevelUpCost=",
			this.LevelUpCost
		}));
	}
}
