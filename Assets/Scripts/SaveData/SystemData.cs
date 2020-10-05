using App.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace SaveData
{
	public class SystemData
	{
		public enum DeckType
		{
			CHARA_MAIN,
			CHARA_SUB,
			CHAO_MAIN,
			CHAO_SUB,
			YOBI_A,
			YOBI_B,
			NUM
		}

		public enum FlagStatus
		{
			NONE = -1,
			ROULETTE_RULE_EXPLAINED,
			TUTORIAL_BOSS_PRESENT,
			TUTORIAL_END,
			TUTORIAL_BOSS_MAP_1,
			TUTORIAL_BOSS_MAP_2,
			TUTORIAL_BOSS_MAP_3,
			TUTORIAL_ANOTHER_CHARA,
			INVITE_FRIEND_SEQUENCE_END,
			RECOMMEND_REVIEW_END,
			SUB_CHARA_ITEM_EXPLAINED,
			ANOTHER_CHARA_EXPLAINED,
			CHARA_LEVEL_UP_EXPLAINED,
			FRIEDN_ACCEPT_INVITE,
			TUTORIAL_RANKING,
			FACEBOOK_FRIEND_INIT,
			TUTORIAL_FEVER_BOSS,
			FIRST_LAUNCH_TUTORIAL_END,
			TUTORIAL_EQIP_ITEM_END
		}

		public enum ItemTutorialFlagStatus
		{
			NONE = -1,
			INVINCIBLE,
			BARRIER,
			MAGNET,
			TRAMPOLINE,
			COMBO,
			LASER,
			DRILL,
			ASTEROID
		}

		public enum CharaTutorialFlagStatus
		{
			NONE = -1,
			CHARA_1,
			CHARA_2,
			CHARA_3,
			CHARA_4,
			CHARA_5,
			CHARA_6,
			CHARA_7,
			CHARA_8,
			CHARA_9,
			CHARA_10,
			CHARA_11,
			CHARA_12,
			CHARA_13,
			CHARA_14,
			CHARA_15,
			CHARA_16,
			CHARA_17,
			CHARA_0,
			CHARA_18,
			CHARA_19,
			CHARA_20,
			CHARA_21,
			CHARA_22,
			CHARA_23,
			CHARA_24,
			CHARA_25,
			CHARA_26,
			CHARA_27,
			CHARA_28
		}

		public enum ActionTutorialFlagStatus
		{
			NONE = -1,
			ACTION_1
		}

		public enum QuickModeTutorialFlagStatus
		{
			NONE = -1,
			QUICK_1
		}

		public enum PushNoticeFlagStatus
		{
			NONE = -1,
			EVENT_INFO,
			CHALLENGE_INFO,
			FRIEND_INFO,
			NUM
		}

		public const int MAX_STOCK_DECK = 6;

		public int chaoSortType01;

		public int chaoSortType02;

		public List<string> fbFriends = new List<string>();

		private int _version_k__BackingField;

		private int _bgmVolume_k__BackingField;

		private int _seVolume_k__BackingField;

		private int _achievementIncentiveCount_k__BackingField;

		private int _iap_k__BackingField;

		private bool _pushNotice_k__BackingField;

		private bool _lightMode_k__BackingField;

		private bool _highTexture_k__BackingField;

		private string _noahId_k__BackingField;

		private string _purchasedRecipt_k__BackingField;

		private string _country_k__BackingField;

		private string _deckData_k__BackingField;

		private string _facebookTime_k__BackingField;

		private string _gameStartTime_k__BackingField;

		private int _playCount_k__BackingField;

		private string _loginRankigTime_k__BackingField;

		private int _achievementCancelCount_k__BackingField;

		private Bitset32 _flags_k__BackingField;

		private Bitset32 _itemTutorialFlags_k__BackingField;

		private Bitset32 _charaTutorialFlags_k__BackingField;

		private Bitset32 _actionTutorialFlags_k__BackingField;

		private Bitset32 _quickModeTutorialFlags_k__BackingField;

		private Bitset32 _pushNoticeFlags_k__BackingField;

		private int _pictureShowEventId_k__BackingField;

		private int _pictureShowProgress_k__BackingField;

		private int _pictureShowEmergeRaidBossProgress_k__BackingField;

		private int _pictureShowRaidBossFirstBattle_k__BackingField;

		private long _currentRaidDrawIndex_k__BackingField;

		private bool _raidEntryFlag_k__BackingField;

		public int version
		{
			get;
			set;
		}

		public int bgmVolume
		{
			get;
			set;
		}

		public int seVolume
		{
			get;
			set;
		}

		public int achievementIncentiveCount
		{
			get;
			set;
		}

		public int iap
		{
			get;
			set;
		}

		public bool pushNotice
		{
			get;
			set;
		}

		public bool lightMode
		{
			get;
			set;
		}

		public bool highTexture
		{
			get;
			set;
		}

		public string noahId
		{
			get;
			set;
		}

		public string purchasedRecipt
		{
			get;
			set;
		}

		public string country
		{
			get;
			set;
		}

		public string deckData
		{
			get;
			set;
		}

		public string facebookTime
		{
			get;
			set;
		}

		public string gameStartTime
		{
			get;
			set;
		}

		public int playCount
		{
			get;
			set;
		}

		public string loginRankigTime
		{
			get;
			set;
		}

		public int achievementCancelCount
		{
			get;
			set;
		}

		public Bitset32 flags
		{
			get;
			set;
		}

		public Bitset32 itemTutorialFlags
		{
			get;
			set;
		}

		public Bitset32 charaTutorialFlags
		{
			get;
			set;
		}

		public Bitset32 actionTutorialFlags
		{
			get;
			set;
		}

		public Bitset32 quickModeTutorialFlags
		{
			get;
			set;
		}

		public Bitset32 pushNoticeFlags
		{
			get;
			set;
		}

		public int pictureShowEventId
		{
			get;
			set;
		}

		public int pictureShowProgress
		{
			get;
			set;
		}

		public int pictureShowEmergeRaidBossProgress
		{
			get;
			set;
		}

		public int pictureShowRaidBossFirstBattle
		{
			get;
			set;
		}

		public long currentRaidDrawIndex
		{
			get;
			set;
		}

		public bool raidEntryFlag
		{
			get;
			set;
		}

		private static string deckDefalut
		{
			get
			{
				int num = 6;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Empty + 300000 + ",");
				for (int i = 0; i < num - 1; i++)
				{
					stringBuilder.Append("-1,");
				}
				return stringBuilder.ToString();
			}
		}

		public SystemData()
		{
			this.Init();
		}

		public void SetFlagStatus(SystemData.FlagStatus status, bool flag)
		{
			this.flags = this.flags.Set((int)status, flag);
		}

		public bool IsFlagStatus(SystemData.FlagStatus status)
		{
			return this.flags.Test((int)status);
		}

		public void SetFlagStatus(SystemData.ItemTutorialFlagStatus status, bool flag)
		{
			this.itemTutorialFlags = this.itemTutorialFlags.Set((int)status, flag);
		}

		public bool IsFlagStatus(SystemData.ItemTutorialFlagStatus status)
		{
			return this.itemTutorialFlags.Test((int)status);
		}

		public void SetFlagStatus(SystemData.CharaTutorialFlagStatus status, bool flag)
		{
			this.charaTutorialFlags = this.charaTutorialFlags.Set((int)status, flag);
		}

		public bool IsFlagStatus(SystemData.CharaTutorialFlagStatus status)
		{
			return this.charaTutorialFlags.Test((int)status);
		}

		public void SetFlagStatus(SystemData.ActionTutorialFlagStatus status, bool flag)
		{
			this.actionTutorialFlags = this.actionTutorialFlags.Set((int)status, flag);
		}

		public bool IsFlagStatus(SystemData.ActionTutorialFlagStatus status)
		{
			return this.actionTutorialFlags.Test((int)status);
		}

		public void SetFlagStatus(SystemData.QuickModeTutorialFlagStatus status, bool flag)
		{
			this.quickModeTutorialFlags = this.quickModeTutorialFlags.Set((int)status, flag);
		}

		public bool IsFlagStatus(SystemData.QuickModeTutorialFlagStatus status)
		{
			return this.quickModeTutorialFlags.Test((int)status);
		}

		public void SetFlagStatus(SystemData.PushNoticeFlagStatus status, bool flag)
		{
			this.pushNoticeFlags = this.pushNoticeFlags.Set((int)status, flag);
		}

		public bool IsFlagStatus(SystemData.PushNoticeFlagStatus status)
		{
			return this.pushNoticeFlags.Test((int)status);
		}

		public void Init(int ver)
		{
			this.Init();
			this.version = ver;
		}

		public void Init()
		{
			this.iap = 0;
			this.version = 0;
			this.bgmVolume = 100;
			this.seVolume = 100;
			this.achievementIncentiveCount = 0;
			this.pushNotice = false;
			this.lightMode = false;
			this.highTexture = false;
			this.noahId = string.Empty;
			this.purchasedRecipt = string.Empty;
			this.country = string.Empty;
			this.deckData = SystemData.DeckAllDefalut();
			this.facebookTime = DateTime.Now.ToString();
			this.gameStartTime = DateTime.Now.ToString();
			this.playCount = 0;
			this.loginRankigTime = string.Empty;
			this.achievementCancelCount = 0;
			this.flags = new Bitset32(0u);
			this.itemTutorialFlags = new Bitset32(0u);
			this.charaTutorialFlags = new Bitset32(0u);
			this.actionTutorialFlags = new Bitset32(0u);
			this.quickModeTutorialFlags = new Bitset32(0u);
			this.pushNoticeFlags = new Bitset32(0u);
			this.SetFlagStatus(SystemData.PushNoticeFlagStatus.EVENT_INFO, false);
			this.SetFlagStatus(SystemData.PushNoticeFlagStatus.CHALLENGE_INFO, false);
			this.SetFlagStatus(SystemData.PushNoticeFlagStatus.FRIEND_INFO, false);
			this.pictureShowEventId = -1;
			this.pictureShowProgress = -1;
			this.pictureShowEmergeRaidBossProgress = -1;
			this.pictureShowRaidBossFirstBattle = -1;
			this.currentRaidDrawIndex = -1L;
			this.raidEntryFlag = false;
			this.chaoSortType01 = 0;
			this.chaoSortType02 = 0;
		}

		public static string DeckAllDefalut()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 6; i++)
			{
				stringBuilder.Append(SystemData.deckDefalut);
			}
			return stringBuilder.ToString();
		}

		public bool CheckDeck()
		{
			if (string.IsNullOrEmpty(this.deckData))
			{
				return false;
			}
			bool result = false;
			string[] array = this.deckData.Split(new char[]
			{
				','
			});
			if (array != null && array.Length > 0 && array.Length >= 36)
			{
				result = true;
			}
			return result;
		}

		public bool CheckExsitDeck()
		{
			SaveDataManager instance = SaveDataManager.Instance;
			CharaType mainChara = instance.PlayerData.MainChara;
			CharaType subChara = instance.PlayerData.SubChara;
			int mainChaoID = instance.PlayerData.MainChaoID;
			int subChaoID = instance.PlayerData.SubChaoID;
			for (int i = 0; i < 6; i++)
			{
				CharaType charaType;
				CharaType charaType2;
				int num;
				int num2;
				this.GetDeckData(i, out charaType, out charaType2, out num, out num2);
				if (mainChara == charaType && subChara == charaType2 && mainChaoID == num && subChaoID == num2)
				{
					return true;
				}
			}
			return false;
		}

		public int GetDeckCurrentStockIndex()
		{
			int result = 0;
			SaveDataManager instance = SaveDataManager.Instance;
			CharaType mainChara = instance.PlayerData.MainChara;
			CharaType subChara = instance.PlayerData.SubChara;
			int mainChaoID = instance.PlayerData.MainChaoID;
			int subChaoID = instance.PlayerData.SubChaoID;
			for (int i = 0; i < 6; i++)
			{
				CharaType charaType;
				CharaType charaType2;
				int num;
				int num2;
				this.GetDeckData(i, out charaType, out charaType2, out num, out num2);
				if (mainChara == charaType && subChara == charaType2 && mainChaoID == num && subChaoID == num2)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public void RestDeckData(int stock)
		{
			this.SaveDeckData(stock, CharaType.SONIC, CharaType.UNKNOWN, -1, -1);
		}

		public void SaveDeckData(int stock, CharaType currentMainCharaType, CharaType currentSubCharaType, int currentMainId, int currentSubId)
		{
			if (stock < 0 || stock >= 6)
			{
				return;
			}
			int num = -1;
			int id = -1;
			ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(currentMainCharaType);
			ServerCharacterState serverCharacterState2 = ServerInterface.PlayerState.CharacterState(currentSubCharaType);
			if (serverCharacterState != null)
			{
				num = serverCharacterState.Id;
			}
			if (serverCharacterState2 != null)
			{
				id = serverCharacterState2.Id;
			}
			if (num < 0)
			{
				num = 300000;
			}
			this.SetDeckId(stock, SystemData.DeckType.CHARA_MAIN, num);
			this.SetDeckId(stock, SystemData.DeckType.CHARA_SUB, id);
			this.SetDeckId(stock, SystemData.DeckType.CHAO_MAIN, currentMainId);
			this.SetDeckId(stock, SystemData.DeckType.CHAO_SUB, currentSubId);
			this.SetDeckId(stock, SystemData.DeckType.YOBI_A, -1);
			this.SetDeckId(stock, SystemData.DeckType.YOBI_B, -1);
			SystemSaveManager.Save();
		}

		public void SaveDeckDataChara(int stock)
		{
			if (stock < 0 || stock >= 6)
			{
				return;
			}
			SaveDataManager instance = SaveDataManager.Instance;
			int id = -1;
			int id2 = -1;
			ServerCharacterState serverCharacterState = ServerInterface.PlayerState.CharacterState(instance.PlayerData.MainChara);
			ServerCharacterState serverCharacterState2 = ServerInterface.PlayerState.CharacterState(instance.PlayerData.SubChara);
			if (serverCharacterState != null)
			{
				id = serverCharacterState.Id;
			}
			if (serverCharacterState2 != null)
			{
				id2 = serverCharacterState2.Id;
			}
			this.SetDeckId(stock, SystemData.DeckType.CHARA_MAIN, id);
			this.SetDeckId(stock, SystemData.DeckType.CHARA_SUB, id2);
			this.SetDeckId(stock, SystemData.DeckType.YOBI_A, -1);
			this.SetDeckId(stock, SystemData.DeckType.YOBI_B, -1);
			SystemSaveManager.Save();
		}

		public void SaveDeckDataChao(int stock)
		{
			if (stock < 0 || stock >= 6)
			{
				return;
			}
			SaveDataManager instance = SaveDataManager.Instance;
			int mainChaoID = instance.PlayerData.MainChaoID;
			int subChaoID = instance.PlayerData.SubChaoID;
			this.SetDeckId(stock, SystemData.DeckType.CHAO_MAIN, mainChaoID);
			this.SetDeckId(stock, SystemData.DeckType.CHAO_SUB, subChaoID);
			this.SetDeckId(stock, SystemData.DeckType.YOBI_A, -1);
			this.SetDeckId(stock, SystemData.DeckType.YOBI_B, -1);
			SystemSaveManager.Save();
		}

		public bool IsSaveDeckData(int stock)
		{
			if (stock < 0 || stock >= 6)
			{
				return false;
			}
			bool result = true;
			CharaType charaType;
			CharaType charaType2;
			int num;
			int num2;
			this.GetDeckData(stock, out charaType, out charaType2, out num, out num2);
			if (charaType == CharaType.SONIC && charaType2 == CharaType.UNKNOWN && num == -1 && num2 == -1)
			{
				result = false;
			}
			return result;
		}

		public int GetCurrentDeckData(out CharaType mainCharaType, out CharaType subCharaType, out int mainChaoId, out int subChaoId)
		{
			int deckCurrentStockIndex = this.GetDeckCurrentStockIndex();
			this.GetDeckData(deckCurrentStockIndex, out mainCharaType, out subCharaType, out mainChaoId, out subChaoId);
			return deckCurrentStockIndex;
		}

		public int GetCurrentDeckData(out CharaType mainCharaType, out CharaType subCharaType)
		{
			int deckCurrentStockIndex = this.GetDeckCurrentStockIndex();
			this.GetDeckData(deckCurrentStockIndex, out mainCharaType, out subCharaType);
			return deckCurrentStockIndex;
		}

		public void GetDeckData(int stock, out CharaType mainCharaType, out CharaType subCharaType, out int mainChaoId, out int subChaoId)
		{
			int deckId = this.GetDeckId(stock, SystemData.DeckType.CHARA_MAIN);
			int deckId2 = this.GetDeckId(stock, SystemData.DeckType.CHARA_SUB);
			ServerItem serverItem = new ServerItem((ServerItem.Id)deckId);
			mainCharaType = serverItem.charaType;
			ServerItem serverItem2 = new ServerItem((ServerItem.Id)deckId2);
			subCharaType = serverItem2.charaType;
			mainChaoId = this.GetDeckId(stock, SystemData.DeckType.CHAO_MAIN);
			subChaoId = this.GetDeckId(stock, SystemData.DeckType.CHAO_SUB);
		}

		public void GetDeckData(int stock, out CharaType mainCharaType, out CharaType subCharaType)
		{
			int deckId = this.GetDeckId(stock, SystemData.DeckType.CHARA_MAIN);
			int deckId2 = this.GetDeckId(stock, SystemData.DeckType.CHARA_SUB);
			ServerItem serverItem = new ServerItem((ServerItem.Id)deckId);
			mainCharaType = serverItem.charaType;
			ServerItem serverItem2 = new ServerItem((ServerItem.Id)deckId2);
			subCharaType = serverItem2.charaType;
		}

		public void GetDeckData(int stock, out int mainChaoId, out int subChaoId)
		{
			mainChaoId = this.GetDeckId(stock, SystemData.DeckType.CHAO_MAIN);
			subChaoId = this.GetDeckId(stock, SystemData.DeckType.CHAO_SUB);
		}

		private int GetDeckId(int index, SystemData.DeckType type)
		{
			if (string.IsNullOrEmpty(this.deckData))
			{
				this.deckData = SystemData.DeckAllDefalut();
			}
			int num = -1;
			string[] array = this.deckData.Split(new char[]
			{
				','
			});
			int num2 = 6;
			int num3 = index * num2;
			if (array.Length >= 6 * num2 && array.Length > num3 && type != SystemData.DeckType.NUM)
			{
				string s = array[(int)(num3 + type)];
				num = int.Parse(s);
			}
			if (num < 0)
			{
				num = -1;
			}
			return num;
		}

		private bool SetDeckId(int index, SystemData.DeckType type, int id)
		{
			if (string.IsNullOrEmpty(this.deckData))
			{
				this.deckData = SystemData.DeckAllDefalut();
			}
			bool result = false;
			string[] array = this.deckData.Split(new char[]
			{
				','
			});
			int num = 6;
			int num2 = index * num;
			if (array.Length >= 6 * num && array.Length > num2 && type != SystemData.DeckType.NUM)
			{
				array[(int)(num2 + type)] = id.ToString();
				this.deckData = string.Empty;
				for (int i = 0; i < array.Length; i++)
				{
					this.deckData = this.deckData + array[i] + ",";
				}
				result = true;
			}
			return result;
		}

		private bool IsFacebookWindowOrg()
		{
			bool result = false;
			if (string.IsNullOrEmpty(this.facebookTime))
			{
				result = true;
			}
			else
			{
				DateTime now = DateTime.Now;
				DateTime t = Convert.ToDateTime(this.facebookTime, DateTimeFormatInfo.InvariantInfo);
				if (now > t)
				{
					result = true;
				}
			}
			return result;
		}

		public bool IsFacebookWindow()
		{
			return this.IsFacebookWindowOrg();
		}

		public void SetFacebookWindow(bool isActive, float hideTime)
		{
			if (isActive)
			{
				this.facebookTime = null;
			}
			else
			{
				this.facebookTime = DateTime.Now.AddHours((double)hideTime).ToString();
			}
		}

		public void SetFacebookWindow(bool isActive)
		{
			if (isActive)
			{
				this.facebookTime = null;
			}
			else
			{
				this.facebookTime = DateTime.Now.AddHours(48.0).ToString();
			}
		}

		public bool CheckLoginTime()
		{
			bool result = false;
			if (string.IsNullOrEmpty(this.loginRankigTime))
			{
				result = true;
			}
			else
			{
				DateTime currentTime = NetBase.GetCurrentTime();
				DateTime d = Convert.ToDateTime(this.loginRankigTime, DateTimeFormatInfo.InvariantInfo);
				TimeSpan timeSpan = currentTime - d;
				UnityEngine.Debug.Log("LoginRanking Span TotalHours =" + timeSpan.TotalHours.ToString());
				if (timeSpan.TotalHours >= 24.0)
				{
					result = true;
				}
			}
			return result;
		}

		public void SetLoginTime()
		{
			this.loginRankigTime = NetBase.GetCurrentTime().ToString();
			UnityEngine.Debug.Log("LoginRankingTime=" + this.loginRankigTime);
		}

		public bool IsNewUser()
		{
			if (this.gameStartTime == null)
			{
				return true;
			}
			DateTime now = DateTime.Now;
			DateTime t = Convert.ToDateTime(this.gameStartTime, DateTimeFormatInfo.InvariantInfo).AddHours(24.0);
			return !(now > t);
		}

		public void CopyTo(SystemData dst, bool temp)
		{
			dst.version = this.version;
			dst.bgmVolume = this.bgmVolume;
			dst.seVolume = this.seVolume;
			dst.achievementIncentiveCount = this.achievementIncentiveCount;
			dst.pushNotice = this.pushNotice;
			dst.lightMode = this.lightMode;
			dst.highTexture = this.highTexture;
			dst.noahId = this.noahId;
			dst.purchasedRecipt = this.purchasedRecipt;
			dst.country = this.country;
			dst.flags = new Bitset32(this.flags);
			dst.itemTutorialFlags = new Bitset32(this.itemTutorialFlags);
			dst.charaTutorialFlags = new Bitset32(this.charaTutorialFlags);
			dst.actionTutorialFlags = new Bitset32(this.actionTutorialFlags);
			dst.quickModeTutorialFlags = new Bitset32(this.quickModeTutorialFlags);
			dst.pushNoticeFlags = new Bitset32(this.pushNoticeFlags);
			dst.deckData = this.deckData;
			dst.pictureShowEventId = this.pictureShowEventId;
			dst.pictureShowProgress = this.pictureShowProgress;
			dst.pictureShowEmergeRaidBossProgress = this.pictureShowEmergeRaidBossProgress;
			dst.pictureShowRaidBossFirstBattle = this.pictureShowRaidBossFirstBattle;
			dst.currentRaidDrawIndex = this.currentRaidDrawIndex;
			dst.raidEntryFlag = this.raidEntryFlag;
			dst.chaoSortType01 = this.chaoSortType01;
			dst.chaoSortType02 = this.chaoSortType02;
			dst.facebookTime = this.facebookTime;
			dst.gameStartTime = this.gameStartTime;
			dst.playCount = this.playCount;
			dst.loginRankigTime = this.loginRankigTime;
			dst.achievementCancelCount = this.achievementCancelCount;
		}
	}
}
