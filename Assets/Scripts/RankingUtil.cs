using App;
using Message;
using SaveData;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class RankingUtil
{
	public enum UserDataType
	{
		RANKING,
		RAID_BOSS,
		DAILY_BATTLE,
		RANK_UP
	}

	public enum RankingMode
	{
		ENDLESS,
		QUICK,
		COUNT
	}

	public enum RankingScoreType
	{
		HIGH_SCORE,
		TOTAL_SCORE,
		NONE
	}

	public enum RankChange
	{
		NONE,
		STAY,
		UP,
		DOWN
	}

	public enum RankingRankerType
	{
		FRIEND,
		ALL,
		RIVAL,
		HISTORY,
		SP_ALL,
		SP_FRIEND,
		COUNT
	}

	public class Ranker
	{
		public static SocialInterface s_socialInterface;

		private bool m_isBoxCollider = true;

		private int _rankIndex_k__BackingField;

		private int _rankIndexChanged_k__BackingField;

		private long _score_k__BackingField;

		private long _hiScore_k__BackingField;

		private int _mapRank_k__BackingField;

		private string _userName_k__BackingField;

		private bool _isFriend_k__BackingField;

		private bool _isSentEnergy_k__BackingField;

		private CharaType _charaType_k__BackingField;

		private CharaType _charaSubType_k__BackingField;

		private int _charaLevel_k__BackingField;

		private int _mainChaoId_k__BackingField;

		private int _subChaoId_k__BackingField;

		private int _mainChaoLevel_k__BackingField;

		private int _subChaoLevel_k__BackingField;

		private Env.Language _language_k__BackingField;

		private int _leagueIndex_k__BackingField;

		private string _id_k__BackingField;

		private DateTime _loginTime_k__BackingField;

		private RankingUtil.UserDataType _userDataType_k__BackingField;

		public int rankIndex
		{
			get;
			set;
		}

		public int rankIndexChanged
		{
			get;
			set;
		}

		public long score
		{
			get;
			set;
		}

		public long hiScore
		{
			get;
			set;
		}

		public int mapRank
		{
			private get;
			set;
		}

		public string dispMapRank
		{
			get
			{
				return (this.mapRank + 1).ToString("D3");
			}
		}

		public string userName
		{
			get;
			set;
		}

		public bool isFriend
		{
			get;
			set;
		}

		public bool isSentEnergy
		{
			get;
			set;
		}

		public CharaType charaType
		{
			get;
			set;
		}

		public CharaType charaSubType
		{
			get;
			set;
		}

		public int charaLevel
		{
			get;
			set;
		}

		public int mainChaoId
		{
			get;
			set;
		}

		public int subChaoId
		{
			get;
			set;
		}

		public int mainChaoLevel
		{
			get;
			set;
		}

		public int subChaoLevel
		{
			get;
			set;
		}

		public Env.Language language
		{
			get;
			set;
		}

		public int leagueIndex
		{
			get;
			set;
		}

		public string id
		{
			get;
			set;
		}

		public DateTime loginTime
		{
			get;
			set;
		}

		public RankingUtil.UserDataType userDataType
		{
			get;
			set;
		}

		public bool isBoxCollider
		{
			get
			{
				return this.m_isBoxCollider;
			}
			set
			{
				this.m_isBoxCollider = value;
			}
		}

		public int rankIconIndex
		{
			get
			{
				return (this.rankIndex >= 1) ? ((this.rankIndex >= 10) ? ((this.rankIndex >= 50) ? ((this.rankIndex >= 150) ? (-1) : 3) : 2) : 1) : 0;
			}
		}

		public int mainChaoRarity
		{
			get
			{
				return this.mainChaoId / 1000;
			}
		}

		public int subChaoRarity
		{
			get
			{
				return this.subChaoId / 1000;
			}
		}

		public bool isMy
		{
			get
			{
				bool result = false;
				if (!string.IsNullOrEmpty(this.id))
				{
					string gameID = SystemSaveManager.GetGameID();
					if (!string.IsNullOrEmpty(gameID) && this.id == gameID)
					{
						result = true;
					}
				}
				return result;
			}
		}

		public Ranker(ServerDailyBattleData user)
		{
			this.id = user.userId;
			this.score = user.maxScore;
			this.hiScore = user.maxScore;
			this.userName = user.name;
			this.isSentEnergy = user.isSentEnergy;
			this.rankIndex = user.goOnWin;
			this.rankIndexChanged = user.goOnWin;
			this.mapRank = user.numRank;
			this.loginTime = NetUtil.GetLocalDateTime(user.loginTime);
			ServerItem serverItem = new ServerItem((ServerItem.Id)user.charaId);
			this.charaType = serverItem.charaType;
			ServerItem serverItem2 = new ServerItem((ServerItem.Id)user.subCharaId);
			this.charaSubType = serverItem2.charaType;
			this.charaLevel = user.charaLevel;
			ServerItem serverItem3 = new ServerItem((ServerItem.Id)user.mainChaoId);
			this.mainChaoId = serverItem3.chaoId;
			ServerItem serverItem4 = new ServerItem((ServerItem.Id)user.subChaoId);
			this.subChaoId = serverItem4.chaoId;
			this.mainChaoLevel = user.mainChaoLevel;
			this.subChaoLevel = user.subChaoLevel;
			this.leagueIndex = user.league;
			this.language = user.language;
			this.userDataType = RankingUtil.UserDataType.DAILY_BATTLE;
			if (RankingUtil.Ranker.s_socialInterface == null)
			{
				RankingUtil.Ranker.s_socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
			}
			if (RankingUtil.Ranker.s_socialInterface != null)
			{
				this.isFriend = (SocialInterface.GetSocialUserDataFromGameId(RankingUtil.Ranker.s_socialInterface.FriendList, this.id) != null);
			}
			else
			{
				this.isFriend = (this.isSentEnergy || UnityEngine.Random.Range(0, 3) != 0);
			}
		}

		public Ranker(RaidBossUser user)
		{
			this.id = user.id;
			this.score = 0L;
			this.hiScore = 0L;
			this.userName = user.userName;
			this.isSentEnergy = user.isSentEnergy;
			this.rankIndex = user.rankIndex;
			this.rankIndexChanged = user.rankIndexChanged;
			this.mapRank = user.mapRank;
			this.loginTime = user.loginTime;
			this.charaType = user.charaType;
			this.charaSubType = user.charaSubType;
			this.charaLevel = user.charaLevel;
			this.mainChaoId = user.mainChaoId;
			this.subChaoId = user.subChaoId;
			this.mainChaoLevel = user.mainChaoLevel;
			this.subChaoLevel = user.subChaoLevel;
			this.leagueIndex = user.leagueIndex;
			this.language = user.language;
			this.userDataType = RankingUtil.UserDataType.RAID_BOSS;
			if (RankingUtil.Ranker.s_socialInterface == null)
			{
				RankingUtil.Ranker.s_socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
			}
			if (RankingUtil.Ranker.s_socialInterface != null)
			{
				this.isFriend = (SocialInterface.GetSocialUserDataFromGameId(RankingUtil.Ranker.s_socialInterface.FriendList, this.id) != null);
			}
			else
			{
				this.isFriend = (this.isSentEnergy || UnityEngine.Random.Range(0, 3) != 0);
			}
		}

		public Ranker(ServerLeaderboardEntry serverLeaderboardEntry)
		{
			this.id = serverLeaderboardEntry.m_hspId;
			this.score = serverLeaderboardEntry.m_score;
			this.hiScore = serverLeaderboardEntry.m_hiScore;
			this.userName = serverLeaderboardEntry.m_name;
			serverLeaderboardEntry.m_url = string.Empty;
			serverLeaderboardEntry.m_userData = 0;
			this.isSentEnergy = serverLeaderboardEntry.m_energyFlg;
			this.rankIndex = serverLeaderboardEntry.m_grade - 1;
			this.rankIndexChanged = serverLeaderboardEntry.m_gradeChanged;
			this.mapRank = serverLeaderboardEntry.m_numRank;
			this.loginTime = NetUtil.GetLocalDateTime(serverLeaderboardEntry.m_loginTime);
			ServerItem serverItem = new ServerItem((ServerItem.Id)serverLeaderboardEntry.m_charaId);
			this.charaType = serverItem.charaType;
			ServerItem serverItem2 = new ServerItem((ServerItem.Id)serverLeaderboardEntry.m_subCharaId);
			this.charaSubType = serverItem2.charaType;
			this.charaLevel = serverLeaderboardEntry.m_charaLevel;
			ServerItem serverItem3 = new ServerItem((ServerItem.Id)serverLeaderboardEntry.m_mainChaoId);
			this.mainChaoId = serverItem3.chaoId;
			ServerItem serverItem4 = new ServerItem((ServerItem.Id)serverLeaderboardEntry.m_subChaoId);
			this.subChaoId = serverItem4.chaoId;
			this.mainChaoLevel = serverLeaderboardEntry.m_mainChaoLevel;
			this.subChaoLevel = serverLeaderboardEntry.m_subChaoLevel;
			this.leagueIndex = serverLeaderboardEntry.m_leagueIndex;
			this.language = serverLeaderboardEntry.m_language;
			this.userDataType = RankingUtil.UserDataType.RANKING;
			if (RankingUtil.Ranker.s_socialInterface == null)
			{
				RankingUtil.Ranker.s_socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
			}
			if (RankingUtil.Ranker.s_socialInterface != null)
			{
				this.isFriend = (SocialInterface.GetSocialUserDataFromGameId(RankingUtil.Ranker.s_socialInterface.FriendList, this.id) != null);
			}
			else
			{
				this.isFriend = (this.isSentEnergy || UnityEngine.Random.Range(0, 3) != 0);
			}
		}

		public bool CheckRankerIdentity(RankingUtil.Ranker target)
		{
			bool result = false;
			if (this.score == target.score && this.userName == target.userName && this.id == target.id && this.rankIndex == target.rankIndex)
			{
				result = true;
			}
			return result;
		}
	}

	public class RankingDataSet
	{
		private RankingUtil.RankingMode m_rankingMode;

		private RankingUtil.RankingScoreType m_rankingTargetRivalScoreType;

		private RankingUtil.RankingScoreType m_rankingTargetSpScoreType = RankingUtil.RankingScoreType.TOTAL_SCORE;

		private ServerWeeklyLeaderboardOptions m_rankingWeeklyLeaderboardOptions;

		private RankingDataContainer m_rankingDataContainer;

		private ServerLeagueData m_leagueData;

		public RankingUtil.RankingMode rankingMode
		{
			get
			{
				return this.m_rankingMode;
			}
		}

		public RankingUtil.RankingScoreType targetRivalScoreType
		{
			get
			{
				return this.m_rankingTargetRivalScoreType;
			}
		}

		public RankingUtil.RankingScoreType targetSpScoreType
		{
			get
			{
				return this.m_rankingTargetSpScoreType;
			}
		}

		public ServerWeeklyLeaderboardOptions weeklyLeaderboardOptions
		{
			get
			{
				return this.m_rankingWeeklyLeaderboardOptions;
			}
		}

		public RankingDataContainer dataContainer
		{
			get
			{
				return this.m_rankingDataContainer;
			}
		}

		public ServerLeagueData leagueData
		{
			get
			{
				return this.m_leagueData;
			}
		}

		public RankingDataSet(ServerWeeklyLeaderboardOptions leaderboardOptions)
		{
			this.Setup(leaderboardOptions);
		}

		public void Setup(ServerWeeklyLeaderboardOptions leaderboardOptions)
		{
			int num = leaderboardOptions.mode;
			if (num < 0 || num >= 2)
			{
				num = 0;
			}
			this.m_rankingMode = (RankingUtil.RankingMode)num;
			this.m_rankingTargetRivalScoreType = leaderboardOptions.rankingScoreType;
			this.m_rankingTargetSpScoreType = RankingUtil.RankingScoreType.TOTAL_SCORE;
			this.m_rankingWeeklyLeaderboardOptions = new ServerWeeklyLeaderboardOptions();
			leaderboardOptions.CopyTo(this.m_rankingWeeklyLeaderboardOptions);
			this.m_rankingDataContainer = new RankingDataContainer();
			this.m_leagueData = null;
		}

		public void SetLeagueData(ServerLeagueData data)
		{
			this.m_leagueData = new ServerLeagueData();
			data.CopyTo(this.m_leagueData);
			global::Debug.Log(string.Concat(new object[]
			{
				"RankingDataSet SetLeagueData mode:",
				this.m_leagueData.rankinMode,
				" leagueId:",
				this.m_leagueData.leagueId,
				"  groupId:",
				this.m_leagueData.groupId,
				" !!!"
			}));
		}

		public void Reset(RankingUtil.RankingRankerType type)
		{
			if (this.m_rankingDataContainer != null)
			{
				this.m_rankingDataContainer.Reset(type);
			}
		}

		public void Reset()
		{
			if (this.m_rankingDataContainer != null)
			{
				this.m_rankingDataContainer.Reset();
			}
		}

		public void SaveRanking()
		{
			if (this.m_rankingDataContainer != null)
			{
				this.m_rankingDataContainer.SavePlayerRanking();
			}
		}

		public bool UpdateSendChallengeList(RankingUtil.RankingRankerType type, string id)
		{
			bool result = false;
			if (this.m_rankingDataContainer != null)
			{
				result = this.m_rankingDataContainer.UpdateSendChallengeList(type, id);
			}
			return result;
		}

		public RankingUtil.RankChange GetRankChange(RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType)
		{
			RankingUtil.RankChange result = RankingUtil.RankChange.NONE;
			if (this.m_rankingDataContainer != null)
			{
				result = this.m_rankingDataContainer.GetRankChange(scoreType, rankerType);
			}
			return result;
		}

		public RankingUtil.RankChange GetRankChange(RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType, out int currentRank, out int oldRank)
		{
			RankingUtil.RankChange result = RankingUtil.RankChange.NONE;
			currentRank = -1;
			oldRank = -1;
			if (this.m_rankingDataContainer != null)
			{
				result = this.m_rankingDataContainer.GetRankChange(scoreType, rankerType, out currentRank, out oldRank);
			}
			return result;
		}

		public void ResetRankChange(RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType)
		{
			if (this.m_rankingDataContainer != null)
			{
				this.m_rankingDataContainer.ResetRankChange(scoreType, rankerType);
			}
		}

		public void AddRankerList(MsgGetLeaderboardEntriesSucceed msg)
		{
			if (this.m_rankingDataContainer != null)
			{
				this.m_rankingDataContainer.AddRankerList(msg);
			}
		}

		public List<RankingUtil.Ranker> GetRankerList(RankingUtil.RankingRankerType rankerType, RankingUtil.RankingScoreType scoreType, int page)
		{
			List<RankingUtil.Ranker> result = null;
			if (this.m_rankingDataContainer != null)
			{
				result = this.m_rankingDataContainer.GetRankerList(rankerType, scoreType, page);
			}
			return result;
		}
	}

	public const int RANKING_GET_LIMIT = 30000;

	private static readonly LeagueTypeParam[] LEAGUE_PARAMS = new LeagueTypeParam[]
	{
		new LeagueTypeParam(LeagueCategory.F, "F"),
		new LeagueTypeParam(LeagueCategory.F, "F"),
		new LeagueTypeParam(LeagueCategory.F, "F"),
		new LeagueTypeParam(LeagueCategory.E, "E"),
		new LeagueTypeParam(LeagueCategory.E, "E"),
		new LeagueTypeParam(LeagueCategory.E, "E"),
		new LeagueTypeParam(LeagueCategory.D, "D"),
		new LeagueTypeParam(LeagueCategory.D, "D"),
		new LeagueTypeParam(LeagueCategory.D, "D"),
		new LeagueTypeParam(LeagueCategory.C, "C"),
		new LeagueTypeParam(LeagueCategory.C, "C"),
		new LeagueTypeParam(LeagueCategory.C, "C"),
		new LeagueTypeParam(LeagueCategory.B, "B"),
		new LeagueTypeParam(LeagueCategory.B, "B"),
		new LeagueTypeParam(LeagueCategory.B, "B"),
		new LeagueTypeParam(LeagueCategory.A, "A"),
		new LeagueTypeParam(LeagueCategory.A, "A"),
		new LeagueTypeParam(LeagueCategory.A, "A"),
		new LeagueTypeParam(LeagueCategory.S, "S"),
		new LeagueTypeParam(LeagueCategory.S, "S"),
		new LeagueTypeParam(LeagueCategory.S, "S")
	};

	public static SocialInterface s_socialInterface = null;

	private static RankingUtil.RankingMode m_currentRankingMode = RankingUtil.RankingMode.COUNT;

	public static RankingUtil.RankingMode currentRankingMode
	{
		get
		{
			RankingUtil.RankingMode result = RankingUtil.RankingMode.ENDLESS;
			if (RankingUtil.m_currentRankingMode != RankingUtil.RankingMode.COUNT)
			{
				result = RankingUtil.m_currentRankingMode;
			}
			else
			{
				global::Debug.Log("RankingUtil currentMode error !!!!!");
			}
			return result;
		}
	}

	public static void SetCurrentRankingMode(RankingUtil.RankingMode mode)
	{
		RankingUtil.m_currentRankingMode = mode;
		global::Debug.Log("RankingUtil SetCurrentRankingMode  currentRankingMode:" + RankingUtil.m_currentRankingMode);
	}

	public static bool IsRankingUserFrontAndBack(RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType, int page)
	{
		bool result = false;
		if (rankerType != RankingUtil.RankingRankerType.RIVAL && page == 0 && rankerType == RankingUtil.RankingRankerType.SP_ALL)
		{
			result = true;
		}
		return result;
	}

	public static int GetRankingCode(RankingUtil.RankingMode rankingMode, RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType)
	{
		int num = -1;
		if (rankingMode != RankingUtil.RankingMode.COUNT && scoreType != RankingUtil.RankingScoreType.NONE && rankerType != RankingUtil.RankingRankerType.COUNT)
		{
			num = (int)((RankingUtil.RankingMode)1000 * rankingMode);
			num += RankingUtil.GetRankingType(scoreType, rankerType);
		}
		return num;
	}

	public static int GetRankingType(RankingUtil.RankingScoreType scoreType, RankingUtil.RankingRankerType rankerType)
	{
		if (scoreType == RankingUtil.RankingScoreType.NONE || rankerType == RankingUtil.RankingRankerType.COUNT)
		{
			return -1;
		}
		return (int)(rankerType * RankingUtil.RankingRankerType.RIVAL + (int)scoreType);
	}

	public static RankingUtil.RankingMode GetRankerMode(int rankingType)
	{
		RankingUtil.RankingMode result = RankingUtil.RankingMode.ENDLESS;
		if (rankingType >= 1000)
		{
			result = (RankingUtil.RankingMode)(rankingType / 1000);
		}
		return result;
	}

	public static RankingUtil.RankingScoreType GetRankerScoreType(int rankingType)
	{
		rankingType %= 1000;
		return (RankingUtil.RankingScoreType)(rankingType % 2);
	}

	public static RankingUtil.RankingRankerType GetRankerType(int rankingType)
	{
		rankingType %= 1000;
		return (RankingUtil.RankingRankerType)(rankingType / 2);
	}

	public static string GetLeagueName(LeagueType type)
	{
		if (type < LeagueType.NUM)
		{
			TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "league_" + (uint)type);
			if (text != null)
			{
				return text.text;
			}
		}
		return string.Empty;
	}

	public static LeagueCategory GetLeagueCategory(LeagueType type)
	{
		if (type < LeagueType.NUM)
		{
			return RankingUtil.LEAGUE_PARAMS[(int)type].m_category;
		}
		return LeagueCategory.NONE;
	}

	public static string GetLeagueCategoryName(LeagueType type)
	{
		if (type < LeagueType.NUM)
		{
			return RankingUtil.LEAGUE_PARAMS[(int)type].m_categoryName;
		}
		return string.Empty;
	}

	public static uint GetLeagueCategoryClass(LeagueType type)
	{
		if (type < LeagueType.NUM)
		{
			return (uint)(type % LeagueType.E_M);
		}
		return 0u;
	}

	public static void GetMyLeagueData(RankingUtil.RankingMode rankingMode, ref int leagueIndex, ref int upCount, ref int downCount)
	{
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			RankingUtil.RankingDataSet rankingDataSet = SingletonGameObject<RankingManager>.Instance.GetRankingDataSet(rankingMode);
			if (rankingDataSet != null && rankingDataSet.leagueData != null)
			{
				leagueIndex = rankingDataSet.leagueData.leagueId;
				upCount = rankingDataSet.leagueData.numUp;
				downCount = rankingDataSet.leagueData.numDown;
			}
		}
	}

	public static void SetLeagueObject(RankingUtil.RankingMode rankingMode, ref UISprite icon0, ref UISprite icon1, ref UILabel rankText0, ref UILabel rankText1)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		RankingUtil.GetMyLeagueData(rankingMode, ref num, ref num2, ref num3);
		icon0.spriteName = RankingUtil.GetLeagueIconNameL((LeagueType)num);
		icon1.spriteName = RankingUtil.GetLeagueIconNameL2((LeagueType)num);
		rankText0.text = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_league_tab_1").text, new Dictionary<string, string>
		{
			{
				"{PARAM_1}",
				RankingUtil.GetLeagueName((LeagueType)num)
			}
		});
		string value = string.Empty;
		if (RankingManager.EndlessRivalRankingScoreType == RankingUtil.RankingScoreType.HIGH_SCORE)
		{
			value = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_Lbl_high_score").text;
		}
		else
		{
			value = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ui_Lbl_total_score").text;
		}
		if (num2 == 0)
		{
			rankText1.text = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_league_tab_4").text, new Dictionary<string, string>
			{
				{
					"{SCORE}",
					value
				},
				{
					"{PARAM_1}",
					num3.ToString()
				}
			});
		}
		else if (num3 == 0)
		{
			rankText1.text = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_league_tab_3").text, new Dictionary<string, string>
			{
				{
					"{SCORE}",
					value
				},
				{
					"{PARAM_1}",
					num2.ToString()
				}
			});
		}
		else
		{
			rankText1.text = TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_league_tab_2").text, new Dictionary<string, string>
			{
				{
					"{SCORE}",
					value
				},
				{
					"{PARAM_1}",
					num2.ToString()
				},
				{
					"{PARAM_2}",
					num3.ToString()
				}
			});
		}
	}

	public static void SetLeagueObjectForMainMenu(RankingUtil.RankingMode rankingMode, UISprite icon0, UISprite icon1, UILabel rankingText)
	{
		int leagueType = 0;
		int num = 0;
		int num2 = 0;
		RankingUtil.GetMyLeagueData(rankingMode, ref leagueType, ref num, ref num2);
		icon0.spriteName = RankingUtil.GetLeagueIconName((LeagueType)leagueType);
		icon1.spriteName = RankingUtil.GetLeagueIconName2((LeagueType)leagueType);
		int num3 = 0;
		RankingUtil.RankingRankerType rankType = RankingUtil.RankingRankerType.RIVAL;
		RankingUtil.RankingScoreType scoreType = RankingUtil.RankingScoreType.HIGH_SCORE;
		if (rankingMode == RankingUtil.RankingMode.ENDLESS)
		{
			scoreType = RankingManager.EndlessRivalRankingScoreType;
		}
		else if (rankingMode == RankingUtil.RankingMode.QUICK)
		{
			scoreType = RankingManager.QuickRivalRankingScoreType;
		}
		RankingUtil.Ranker myRank = RankingManager.GetMyRank(rankingMode, rankType, scoreType);
		if (myRank != null)
		{
			num3 = myRank.rankIndex + 1;
		}
		rankingText.text = num3.ToString("00");
	}

	public static string GetLeagueIconName(LeagueType leagueType)
	{
		return "ui_ranking_league_icon_s_" + RankingUtil.GetLeagueCategoryName(leagueType).ToLower();
	}

	public static string GetLeagueIconName2(LeagueType leagueType)
	{
		return "ui_ranking_league_icon_s_" + RankingUtil.GetLeagueCategoryClass(leagueType).ToString();
	}

	public static string GetLeagueIconNameL(LeagueType leagueType)
	{
		return "ui_ranking_league_icon_" + RankingUtil.GetLeagueCategoryName(leagueType).ToLower();
	}

	public static string GetLeagueIconNameL2(LeagueType leagueType)
	{
		return "ui_ranking_league_icon_" + RankingUtil.GetLeagueCategoryClass(leagueType).ToString();
	}

	public static Texture2D GetProfilePictureTexture(RankingUtil.Ranker ranker, Action<Texture2D> callback)
	{
		return RankingUtil.GetProfilePictureTexture(ranker.id, callback);
	}

	public static Texture2D GetProfilePictureTexture(string userId, Action<Texture2D> callback)
	{
		if (RankingUtil.s_socialInterface == null)
		{
			RankingUtil.s_socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		}
		global::Debug.Log("GetProfilePictureTexture.gameId:" + userId);
		string text = null;
		if (RankingUtil.s_socialInterface != null)
		{
			SocialUserData socialUserDataFromGameId = SocialInterface.GetSocialUserDataFromGameId(RankingUtil.s_socialInterface.FriendWithMeList, userId);
			if (socialUserDataFromGameId != null)
			{
				text = socialUserDataFromGameId.Id;
			}
		}
		global::Debug.Log("GetProfilePictureTexture.socialId:" + text);
		PlayerImageManager playerImageManager = GameObjectUtil.FindGameObjectComponent<PlayerImageManager>("PlayerImageManager");
		if (playerImageManager != null)
		{
			return playerImageManager.GetPlayerImage(text, string.Empty, callback);
		}
		return null;
	}

	public static List<RankingUtil.Ranker> GetRankerList(MsgGetLeaderboardEntriesSucceed msg)
	{
		List<RankingUtil.Ranker> list = new List<RankingUtil.Ranker>();
		if (msg != null && msg.m_leaderboardEntries != null)
		{
			ServerLeaderboardEntries leaderboardEntries = msg.m_leaderboardEntries;
			if (leaderboardEntries.m_myLeaderboardEntry != null)
			{
				list.Add(new RankingUtil.Ranker(leaderboardEntries.m_myLeaderboardEntry));
			}
			else
			{
				list.Add(null);
			}
			if (leaderboardEntries.m_leaderboardEntries != null && leaderboardEntries.m_leaderboardEntries.Count > 0)
			{
				List<ServerLeaderboardEntry> leaderboardEntries2 = leaderboardEntries.m_leaderboardEntries;
				if (leaderboardEntries2 != null)
				{
					int num = leaderboardEntries2.Count;
					if (!leaderboardEntries.IsRivalRanking() && leaderboardEntries.IsNext())
					{
						num = leaderboardEntries2.Count - 1;
					}
					for (int i = 0; i < leaderboardEntries2.Count; i++)
					{
						if (i >= num)
						{
							break;
						}
						list.Add(new RankingUtil.Ranker(leaderboardEntries2[i]));
					}
				}
			}
		}
		else
		{
			list.Add(null);
		}
		return list;
	}

	public static MsgGetLeaderboardEntriesSucceed InitRankingMsg(MsgGetLeaderboardEntriesSucceed msg)
	{
		int num = 4;
		MsgGetLeaderboardEntriesSucceed msgGetLeaderboardEntriesSucceed = new MsgGetLeaderboardEntriesSucceed();
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries = new ServerLeaderboardEntries();
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_resetTime = msg.m_leaderboardEntries.m_resetTime;
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_startTime = msg.m_leaderboardEntries.m_startTime;
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_startIndex = msg.m_leaderboardEntries.m_startIndex;
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_first = msg.m_leaderboardEntries.m_first;
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_count = msg.m_leaderboardEntries.m_count;
		if (msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_count > num)
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_count = num;
		}
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_rankingType = msg.m_leaderboardEntries.m_rankingType;
		if (msg.m_leaderboardEntries.m_friendIdList == null)
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_friendIdList = null;
		}
		else
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_friendIdList = new string[msg.m_leaderboardEntries.m_friendIdList.Length];
		}
		if (msg.m_leaderboardEntries.m_myLeaderboardEntry == null)
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_myLeaderboardEntry = null;
		}
		else
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_myLeaderboardEntry = msg.m_leaderboardEntries.m_myLeaderboardEntry.Clone();
		}
		if (msg.m_leaderboardEntries.m_leaderboardEntries == null)
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries = null;
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_resultTotalEntries = 0;
		}
		else
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries = new List<ServerLeaderboardEntry>();
			int num2 = 0;
			foreach (ServerLeaderboardEntry current in msg.m_leaderboardEntries.m_leaderboardEntries)
			{
				msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries.Add(current.Clone());
				num2++;
				if (num <= num2)
				{
					break;
				}
			}
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_resultTotalEntries = msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries.Count;
		}
		return msgGetLeaderboardEntriesSucceed;
	}

	public static MsgGetLeaderboardEntriesSucceed CopyRankingMsg(MsgGetLeaderboardEntriesSucceed msg, MsgGetLeaderboardEntriesSucceed org = null)
	{
		MsgGetLeaderboardEntriesSucceed msgGetLeaderboardEntriesSucceed = new MsgGetLeaderboardEntriesSucceed();
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries = new ServerLeaderboardEntries();
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_resetTime = msg.m_leaderboardEntries.m_resetTime;
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_startTime = msg.m_leaderboardEntries.m_startTime;
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_startIndex = msg.m_leaderboardEntries.m_startIndex;
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_first = msg.m_leaderboardEntries.m_first;
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_count = msg.m_leaderboardEntries.m_count;
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_rankingType = msg.m_leaderboardEntries.m_rankingType;
		if (msg.m_leaderboardEntries.m_friendIdList == null)
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_friendIdList = null;
		}
		else
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_friendIdList = new string[msg.m_leaderboardEntries.m_friendIdList.Length];
		}
		if (msg.m_leaderboardEntries.m_myLeaderboardEntry == null)
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_myLeaderboardEntry = null;
		}
		else
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_myLeaderboardEntry = msg.m_leaderboardEntries.m_myLeaderboardEntry.Clone();
		}
		if (msg.m_leaderboardEntries.m_leaderboardEntries == null)
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries = null;
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_resultTotalEntries = 0;
		}
		else
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries = new List<ServerLeaderboardEntry>();
			foreach (ServerLeaderboardEntry current in msg.m_leaderboardEntries.m_leaderboardEntries)
			{
				msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries.Add(current.Clone());
			}
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_resultTotalEntries = msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries.Count;
		}
		if (org == null || (msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_first == org.m_leaderboardEntries.m_first && msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_count == org.m_leaderboardEntries.m_count))
		{
			return msgGetLeaderboardEntriesSucceed;
		}
		bool flag = false;
		bool flag2 = false;
		int num = msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_first + msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_resultTotalEntries;
		int num2 = org.m_leaderboardEntries.m_first + org.m_leaderboardEntries.m_resultTotalEntries;
		if (msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_first <= org.m_leaderboardEntries.m_first && num >= num2)
		{
			return msgGetLeaderboardEntriesSucceed;
		}
		if (num == num2)
		{
			if (msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_count <= msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_resultTotalEntries)
			{
				flag = true;
			}
		}
		else if (num > num2)
		{
			if (msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_count <= msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_resultTotalEntries)
			{
				flag = true;
			}
		}
		else if (org.m_leaderboardEntries.m_count <= org.m_leaderboardEntries.m_resultTotalEntries)
		{
			flag = true;
		}
		List<ServerLeaderboardEntry> list = new List<ServerLeaderboardEntry>();
		if (msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_first > org.m_leaderboardEntries.m_first)
		{
			flag2 = true;
			foreach (ServerLeaderboardEntry current2 in org.m_leaderboardEntries.m_leaderboardEntries)
			{
				list.Add(current2);
			}
		}
		else
		{
			flag2 = false;
			foreach (ServerLeaderboardEntry current3 in msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries)
			{
				list.Add(current3);
			}
		}
		if (flag2)
		{
			int num3 = msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_first - 1;
			int num4 = num2;
			if (num > num2)
			{
				num4 = num;
			}
			int count = list.Count;
			for (int i = num3; i < num4; i++)
			{
				if (i < count)
				{
					if (i - num3 < msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries.Count)
					{
						list[i] = msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries[i - num3];
					}
				}
				else if (i - num3 < msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries.Count)
				{
					list.Add(msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries[i - num3]);
				}
			}
		}
		else
		{
			int num3 = msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_first - org.m_leaderboardEntries.m_first;
			int num4 = num - msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_first;
			for (int j = num3; j < num4; j++)
			{
				if (j >= list.Count && j - num3 < org.m_leaderboardEntries.m_leaderboardEntries.Count)
				{
					list.Add(org.m_leaderboardEntries.m_leaderboardEntries[j - num3]);
				}
			}
		}
		if (msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_first > org.m_leaderboardEntries.m_first)
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_first = org.m_leaderboardEntries.m_first;
		}
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_leaderboardEntries = list;
		msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_resultTotalEntries = list.Count;
		if (flag)
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_count = list.Count;
		}
		else
		{
			msgGetLeaderboardEntriesSucceed.m_leaderboardEntries.m_count = list.Count - 1;
		}
		return msgGetLeaderboardEntriesSucceed;
	}

	public static string[] GetFriendIdList()
	{
		string[] result = null;
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface != null && socialInterface.IsLoggedIn)
		{
			result = SocialInterface.GetGameIdList(socialInterface.FriendList).ToArray();
		}
		return result;
	}

	public static string GetResetTime(TimeSpan span, bool isHeadText = true)
	{
		string text = string.Empty;
		bool flag = span.Ticks <= 0L;
		if (flag)
		{
			text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_sumup").text;
		}
		else
		{
			if (isHeadText)
			{
				text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_reset").text + "\n";
			}
			text += TextUtility.Replaces(TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", (span.Days <= 0) ? ((span.Hours <= 0) ? ((span.Minutes <= 0) ? "ranking_reset_seconds" : "ranking_reset_minutes") : "ranking_reset_hours") : "ranking_reset_days").text, new Dictionary<string, string>
			{
				{
					"{DAYS}",
					span.Days.ToString()
				},
				{
					"{HOURS}",
					span.Hours.ToString()
				},
				{
					"{MINUTES}",
					span.Minutes.ToString()
				}
			});
		}
		return text;
	}

	public static bool ShowRankingChangeWindow(RankingUtil.RankingMode rankingMode)
	{
		global::Debug.Log("ShowRankingChangeWindow");
		if (RankingResultBitWindow.Instance != null)
		{
			RankingResultBitWindow.Instance.Open(rankingMode);
			return true;
		}
		global::Debug.Log("ShowRankingChangeWindow error?");
		return false;
	}

	public static bool IsEndRankingChangeWindow()
	{
		if (RankingResultBitWindow.Instance != null)
		{
			return RankingResultBitWindow.Instance.IsEnd;
		}
		global::Debug.Log("IsEndRankingChangeWindow error?");
		return false;
	}

	public static string GetFriendIconSpriteName(RankingUtil.Ranker ranker)
	{
		if (ranker == null || !ranker.isFriend)
		{
			return string.Empty;
		}
		if (ranker.isSentEnergy)
		{
			return "ui_ranking_scroll_icon_friend_1";
		}
		return "ui_ranking_scroll_icon_friend_0";
	}

	public static string GetFriendIconSpriteName(RaidBossUser user)
	{
		if (user == null || !user.isFriend)
		{
			return string.Empty;
		}
		if (user.isSentEnergy)
		{
			return "ui_ranking_scroll_icon_friend_1";
		}
		return "ui_ranking_scroll_icon_friend_0";
	}

	public static void DebugRankingChange()
	{
		global::Debug.Log("DebugRankingChange !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		RankingUtil.RankingRankerType rankingRankerType = RankingUtil.RankingRankerType.RIVAL;
		RankingUtil.RankingScoreType endlessRivalRankingScoreType = RankingManager.EndlessRivalRankingScoreType;
		int num = -1;
		int num2 = -1;
		RankingUtil.RankChange rankingRankChange = SingletonGameObject<RankingManager>.Instance.GetRankingRankChange(RankingUtil.RankingMode.ENDLESS, endlessRivalRankingScoreType, rankingRankerType, out num, out num2);
		string text = string.Concat(new object[]
		{
			"set rankerType:",
			rankingRankerType,
			"  scoreType:",
			endlessRivalRankingScoreType
		});
		text = text + "\n  change:" + rankingRankChange;
		if (rankingRankChange != RankingUtil.RankChange.NONE)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"   ",
				num2,
				" → ",
				num
			});
		}
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "debug_ranking_change",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = "Debug",
			message = text
		});
	}

	public static void DebugCurrentRanking(bool isEvent, long score)
	{
		long num;
		long num2;
		int type;
		bool currentRankingStatus = RankingManager.GetCurrentRankingStatus(RankingUtil.RankingMode.ENDLESS, isEvent, out num, out num2, out type);
		long num3 = score;
		if (isEvent)
		{
			num3 = score + num;
		}
		int num4 = 0;
		bool flag;
		long num5;
		long num6;
		int currentHighScoreRank = RankingManager.GetCurrentHighScoreRank(RankingUtil.RankingMode.ENDLESS, isEvent, ref num3, out flag, out num5, out num6, out num4);
		string text = string.Concat(new object[]
		{
			"set  isEvent:",
			isEvent,
			"  score:",
			score
		});
		text += "\n◆GetCurrentRankingStatus";
		text = text + "\n\u3000\u3000isStatus:" + currentRankingStatus;
		text = text + "\n\u3000\u3000myScore:" + num;
		text = text + "\n\u3000\u3000myLeague:" + RankingUtil.GetLeagueName((LeagueType)type);
		text = text + "\n◆GetCurrentHighScoreRank    currentScore:" + num3;
		text = text + "\n\u3000\u3000rank:" + currentHighScoreRank;
		text = text + "\n\u3000\u3000isHighScore:" + flag;
		text = text + "\n\u3000\u3000nextRankScore:" + num5;
		text = text + "\n\u3000\u3000prveRankScore:" + num6;
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "debug_info",
			buttonType = GeneralWindow.ButtonType.Ok,
			caption = "Debug",
			message = text
		});
	}
}
