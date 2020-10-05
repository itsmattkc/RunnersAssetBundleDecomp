using System;

namespace SaveData
{
	public class PlayerData
	{
		public const uint MAX_CHALLENGE_COUNT = 99999u;

		private uint m_progress_status;

		private long m_total_distance;

		private uint m_challenge_count;

		private int m_challenge_count_offset;

		private long m_best_score;

		private long m_best_score_quick;

		private uint m_rank;

		private int m_rank_offset;

		private CharaType m_main_chara_type;

		private CharaType m_sub_chara_type;

		private int m_main_chao_id;

		private int m_sub_chao_id;

		private int m_friend_chao_id;

		private int m_friend_chao_level;

		private string m_rental_friend_id;

		private DailyMissionData m_daily_mission_data = new DailyMissionData();

		private DailyMissionData m_beforeDailyMissionData = new DailyMissionData();

		private ItemType[] m_equipped_item = new ItemType[3];

		private ItemStatus[] m_equipped_item_use_status = new ItemStatus[3];

		private bool[] m_boosted_item = new bool[3];

		public uint Rank
		{
			get
			{
				return this.m_rank;
			}
			set
			{
				this.m_rank = value;
			}
		}

		public int RankOffset
		{
			get
			{
				return this.m_rank_offset;
			}
			set
			{
				this.m_rank_offset = value;
			}
		}

		public uint DisplayRank
		{
			get
			{
				return ((this.RankOffset < 0) ? (this.Rank - (uint)(-(uint)this.RankOffset)) : (this.Rank + (uint)this.RankOffset)) + 1u;
			}
		}

		public uint ProgressStatus
		{
			get
			{
				return this.m_progress_status;
			}
			set
			{
				this.m_progress_status = value;
			}
		}

		public long TotalDistance
		{
			get
			{
				return this.m_total_distance;
			}
			set
			{
				this.m_total_distance = value;
			}
		}

		public uint ChallengeCount
		{
			get
			{
				return this.m_challenge_count;
			}
			set
			{
				this.m_challenge_count = value;
			}
		}

		public int ChallengeCountOffset
		{
			get
			{
				return this.m_challenge_count_offset;
			}
			set
			{
				this.m_challenge_count_offset = value;
			}
		}

		public uint DisplayChallengeCount
		{
			get
			{
				return (this.ChallengeCountOffset < 0) ? (this.ChallengeCount - (uint)(-(uint)this.ChallengeCountOffset)) : (this.ChallengeCount + (uint)this.ChallengeCountOffset);
			}
		}

		public long BestScore
		{
			get
			{
				return this.m_best_score;
			}
			set
			{
				this.m_best_score = value;
			}
		}

		public long BestScoreQuick
		{
			get
			{
				return this.m_best_score_quick;
			}
			set
			{
				this.m_best_score_quick = value;
			}
		}

		public CharaType MainChara
		{
			get
			{
				return this.m_main_chara_type;
			}
			set
			{
				this.m_main_chara_type = value;
			}
		}

		public CharaType SubChara
		{
			get
			{
				return this.m_sub_chara_type;
			}
			set
			{
				this.m_sub_chara_type = value;
			}
		}

		public int MainChaoID
		{
			get
			{
				return this.m_main_chao_id;
			}
			set
			{
				this.m_main_chao_id = value;
			}
		}

		public int SubChaoID
		{
			get
			{
				return this.m_sub_chao_id;
			}
			set
			{
				this.m_sub_chao_id = value;
			}
		}

		public int FriendChaoID
		{
			get
			{
				return this.m_friend_chao_id;
			}
			set
			{
				this.m_friend_chao_id = value;
			}
		}

		public int FriendChaoLevel
		{
			get
			{
				return this.m_friend_chao_level;
			}
			set
			{
				this.m_friend_chao_level = value;
			}
		}

		public string RentalFriendId
		{
			get
			{
				return this.m_rental_friend_id;
			}
			set
			{
				this.m_rental_friend_id = value;
			}
		}

		public ItemType[] EquippedItem
		{
			get
			{
				return this.m_equipped_item;
			}
			set
			{
				this.m_equipped_item = value;
			}
		}

		public ItemStatus[] EquippedItemUseStatue
		{
			get
			{
				return this.m_equipped_item_use_status;
			}
			set
			{
				this.m_equipped_item_use_status = value;
			}
		}

		public bool[] BoostedItem
		{
			get
			{
				return this.m_boosted_item;
			}
			set
			{
				this.m_boosted_item = value;
			}
		}

		public DailyMissionData DailyMission
		{
			get
			{
				return this.m_daily_mission_data;
			}
			set
			{
				this.m_daily_mission_data = value;
			}
		}

		public DailyMissionData BeforeDailyMissionData
		{
			get
			{
				return this.m_beforeDailyMissionData;
			}
			set
			{
				this.m_beforeDailyMissionData = value;
			}
		}

		public PlayerData()
		{
			this.m_progress_status = 0u;
			this.m_total_distance = 0L;
			this.m_challenge_count = 3u;
			this.m_best_score = 0L;
			this.m_best_score_quick = 0L;
			this.m_main_chao_id = -1;
			this.m_sub_chao_id = -1;
			this.m_friend_chao_id = -1;
			this.m_friend_chao_level = -1;
			this.m_rental_friend_id = string.Empty;
			this.m_main_chara_type = CharaType.SONIC;
			this.m_sub_chara_type = CharaType.UNKNOWN;
			this.m_rank = 1u;
			for (int i = 0; i < 3; i++)
			{
				this.m_equipped_item[i] = ItemType.UNKNOWN;
				this.m_equipped_item_use_status[i] = ItemStatus.NO_USE;
			}
			for (int j = 0; j < 3; j++)
			{
				this.m_boosted_item[j] = false;
			}
		}
	}
}
