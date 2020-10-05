using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesPlatform : ISocialPlatform
	{
		private static PlayGamesPlatform sInstance;

		private PlayGamesLocalUser mLocalUser;

		private IPlayGamesClient mClient;

		private string mDefaultLbUi;

		private Dictionary<string, string> mIdMap = new Dictionary<string, string>();

		public static PlayGamesPlatform Instance
		{
			get
			{
				if (PlayGamesPlatform.sInstance == null)
				{
					PlayGamesPlatform.sInstance = new PlayGamesPlatform();
				}
				return PlayGamesPlatform.sInstance;
			}
		}

		public static bool DebugLogEnabled
		{
			get
			{
				return Logger.DebugLogEnabled;
			}
			set
			{
				Logger.DebugLogEnabled = value;
			}
		}

		public IRealTimeMultiplayerClient RealTime
		{
			get
			{
				return this.mClient.GetRtmpClient();
			}
		}

		public ITurnBasedMultiplayerClient TurnBased
		{
			get
			{
				return this.mClient.GetTbmpClient();
			}
		}

		public ILocalUser localUser
		{
			get
			{
				return this.mLocalUser;
			}
		}

		private PlayGamesPlatform()
		{
			this.mLocalUser = new PlayGamesLocalUser(this);
		}

		public static PlayGamesPlatform Activate()
		{
			Logger.d("Activating PlayGamesPlatform.");
			Social.Active = PlayGamesPlatform.Instance;
			Logger.d("PlayGamesPlatform activated: " + Social.Active);
			return PlayGamesPlatform.Instance;
		}

		public void AddIdMapping(string fromId, string toId)
		{
			this.mIdMap[fromId] = toId;
		}

		public void Authenticate(Action<bool> callback)
		{
			this.Authenticate(callback, false);
		}

		public void Authenticate(Action<bool> callback, bool silent)
		{
			if (this.mClient == null)
			{
				Logger.d("Creating platform-specific Play Games client.");
				this.mClient = PlayGamesClientFactory.GetPlatformPlayGamesClient();
			}
			this.mClient.Authenticate(callback, silent);
		}

		public void Authenticate(ILocalUser unused, Action<bool> callback)
		{
			this.Authenticate(callback, false);
		}

		public bool IsAuthenticated()
		{
			return this.mClient != null && this.mClient.IsAuthenticated();
		}

		public void SignOut()
		{
			if (this.mClient != null)
			{
				this.mClient.SignOut();
			}
		}

		public void LoadUsers(string[] userIDs, Action<IUserProfile[]> callback)
		{
			Logger.w("PlayGamesPlatform.LoadUsers is not implemented.");
			if (callback != null)
			{
				callback(new IUserProfile[0]);
			}
		}

		public string GetUserId()
		{
			if (!this.IsAuthenticated())
			{
				Logger.e("GetUserId() can only be called after authentication.");
				return "0";
			}
			return this.mClient.GetUserId();
		}

		public string GetUserDisplayName()
		{
			if (!this.IsAuthenticated())
			{
				Logger.e("GetUserDisplayName can only be called after authentication.");
				return string.Empty;
			}
			return this.mClient.GetUserDisplayName();
		}

		public void ReportProgress(string achievementID, double progress, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				Logger.e("ReportProgress can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			Logger.d(string.Concat(new object[]
			{
				"ReportProgress, ",
				achievementID,
				", ",
				progress
			}));
			achievementID = this.MapId(achievementID);
			if (progress < 1E-06)
			{
				Logger.d("Progress 0.00 interpreted as request to reveal.");
				this.mClient.RevealAchievement(achievementID, callback);
				return;
			}
			int num = 0;
			int num2 = 0;
			Achievement achievement = this.mClient.GetAchievement(achievementID);
			bool flag;
			if (achievement == null)
			{
				Logger.w("Unable to locate achievement " + achievementID);
				Logger.w("As a quick fix, assuming it's standard.");
				flag = false;
			}
			else
			{
				flag = achievement.IsIncremental;
				num = achievement.CurrentSteps;
				num2 = achievement.TotalSteps;
				Logger.d("Achievement is " + ((!flag) ? "STANDARD" : "INCREMENTAL"));
				if (flag)
				{
					Logger.d(string.Concat(new object[]
					{
						"Current steps: ",
						num,
						"/",
						num2
					}));
				}
			}
			if (flag)
			{
				Logger.d("Progress " + progress + " interpreted as incremental target (approximate).");
				int num3 = (int)(progress * (double)num2);
				int num4 = num3 - num;
				Logger.d(string.Concat(new object[]
				{
					"Target steps: ",
					num3,
					", cur steps:",
					num
				}));
				Logger.d("Steps to increment: " + num4);
				if (num4 > 0)
				{
					this.mClient.IncrementAchievement(achievementID, num4, callback);
				}
			}
			else
			{
				Logger.d("Progress " + progress + " interpreted as UNLOCK.");
				this.mClient.UnlockAchievement(achievementID, callback);
			}
		}

		public void IncrementAchievement(string achievementID, int steps, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				Logger.e("IncrementAchievement can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			Logger.d(string.Concat(new object[]
			{
				"IncrementAchievement: ",
				achievementID,
				", steps ",
				steps
			}));
			achievementID = this.MapId(achievementID);
			this.mClient.IncrementAchievement(achievementID, steps, callback);
		}

		public void LoadAchievementDescriptions(Action<IAchievementDescription[]> callback)
		{
			Logger.w("PlayGamesPlatform.LoadAchievementDescriptions is not implemented.");
			if (callback != null)
			{
				callback(new IAchievementDescription[0]);
			}
		}

		public void LoadAchievements(Action<IAchievement[]> callback)
		{
			Logger.w("PlayGamesPlatform.LoadAchievements is not implemented.");
			if (callback != null)
			{
				callback(new IAchievement[0]);
			}
		}

		public void LoadAchievementDescriptions(string[] idList, Action<IAchievement[]> callback)
		{
			if (!this.IsAuthenticated())
			{
				Logger.e("IncrementAchievement can only be called after authentication.");
				if (callback != null)
				{
					callback(new IAchievement[0]);
				}
				return;
			}
			List<PlayGamesAchievement> list = new List<PlayGamesAchievement>();
			for (int i = 0; i < idList.Length; i++)
			{
				string text = idList[i];
				Achievement achievement = this.mClient.GetAchievement(text);
				if (achievement != null)
				{
					PlayGamesAchievement playGamesAchievement = new PlayGamesAchievement();
					playGamesAchievement.id = text;
					if (achievement.IsUnlocked)
					{
						playGamesAchievement.percentCompleted = 100.0;
					}
					else
					{
						playGamesAchievement.percentCompleted = 0.0;
					}
					global::Debug.Log("Add Load AchievementDescriptions" + playGamesAchievement.id);
					list.Add(playGamesAchievement);
				}
			}
			PlayGamesAchievement[] obj = new PlayGamesAchievement[list.Count];
			obj = list.ToArray();
			if (callback != null)
			{
				callback(obj);
			}
		}

		public void LoadAchievements(string[] idList, Action<IAchievement[]> callback)
		{
			if (!this.IsAuthenticated())
			{
				Logger.e("IncrementAchievement can only be called after authentication.");
				if (callback != null)
				{
					callback(new IAchievement[0]);
				}
				return;
			}
			List<PlayGamesAchievement> list = new List<PlayGamesAchievement>();
			for (int i = 0; i < idList.Length; i++)
			{
				string text = idList[i];
				Achievement achievement = this.mClient.GetAchievement(text);
				if (achievement != null && achievement.IsUnlocked)
				{
					PlayGamesAchievement playGamesAchievement = new PlayGamesAchievement();
					playGamesAchievement.id = text;
					global::Debug.Log("Add Load Achievements" + playGamesAchievement.id);
					playGamesAchievement.percentCompleted = 100.0;
					list.Add(playGamesAchievement);
				}
			}
			PlayGamesAchievement[] obj = new PlayGamesAchievement[list.Count];
			obj = list.ToArray();
			if (callback != null)
			{
				callback(obj);
			}
		}

		public IAchievement CreateAchievement()
		{
			return new PlayGamesAchievement();
		}

		public void ReportScore(long score, string board, Action<bool> callback)
		{
			if (!this.IsAuthenticated())
			{
				Logger.e("ReportScore can only be called after authentication.");
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			Logger.d(string.Concat(new object[]
			{
				"ReportScore: score=",
				score,
				", board=",
				board
			}));
			string lbId = this.MapId(board);
			this.mClient.SubmitScore(lbId, score, callback);
		}

		public void LoadScores(string leaderboardID, Action<IScore[]> callback)
		{
			Logger.w("PlayGamesPlatform.LoadScores not implemented.");
			if (callback != null)
			{
				callback(new IScore[0]);
			}
		}

		public ILeaderboard CreateLeaderboard()
		{
			Logger.w("PlayGamesPlatform.CreateLeaderboard not implemented. Returning null.");
			return null;
		}

		public void ShowAchievementsUI()
		{
			if (!this.IsAuthenticated())
			{
				Logger.e("ShowAchievementsUI can only be called after authentication.");
				return;
			}
			Logger.d("ShowAchievementsUI");
			this.mClient.ShowAchievementsUI();
		}

		public void ShowLeaderboardUI()
		{
			if (!this.IsAuthenticated())
			{
				Logger.e("ShowLeaderboardUI can only be called after authentication.");
				return;
			}
			Logger.d("ShowLeaderboardUI");
			this.mClient.ShowLeaderboardUI(this.MapId(this.mDefaultLbUi));
		}

		public void ShowLeaderboardUI(string lbId)
		{
			if (!this.IsAuthenticated())
			{
				Logger.e("ShowLeaderboardUI can only be called after authentication.");
				return;
			}
			Logger.d("ShowLeaderboardUI, lbId=" + lbId);
			if (lbId != null)
			{
				lbId = this.MapId(lbId);
			}
			this.mClient.ShowLeaderboardUI(lbId);
		}

		public void SetDefaultLeaderboardForUI(string lbid)
		{
			Logger.d("SetDefaultLeaderboardForUI: " + lbid);
			if (lbid != null)
			{
				lbid = this.MapId(lbid);
			}
			this.mDefaultLbUi = lbid;
		}

		public void LoadFriends(ILocalUser user, Action<bool> callback)
		{
			Logger.w("PlayGamesPlatform.LoadFriends not implemented.");
			if (callback != null)
			{
				callback(false);
			}
		}

		public void LoadScores(ILeaderboard board, Action<bool> callback)
		{
			Logger.w("PlayGamesPlatform.LoadScores not implemented.");
			if (callback != null)
			{
				callback(false);
			}
		}

		public bool GetLoading(ILeaderboard board)
		{
			return false;
		}

		public void SetCloudCacheEncrypter(BufferEncrypter encrypter)
		{
			this.mClient.SetCloudCacheEncrypter(encrypter);
		}

		public void LoadState(int slot, OnStateLoadedListener listener)
		{
			if (!this.IsAuthenticated())
			{
				Logger.e("LoadState can only be called after authentication.");
				if (listener != null)
				{
					listener.OnStateLoaded(false, slot, null);
				}
				return;
			}
			this.mClient.LoadState(slot, listener);
		}

		public void UpdateState(int slot, byte[] data, OnStateLoadedListener listener)
		{
			if (!this.IsAuthenticated())
			{
				Logger.e("UpdateState can only be called after authentication.");
				if (listener != null)
				{
					listener.OnStateSaved(false, slot);
				}
				return;
			}
			this.mClient.UpdateState(slot, data, listener);
		}

		public void RegisterInvitationDelegate(InvitationReceivedDelegate deleg)
		{
			this.mClient.RegisterInvitationDelegate(deleg);
		}

		private string MapId(string id)
		{
			if (id == null)
			{
				return null;
			}
			if (this.mIdMap.ContainsKey(id))
			{
				string text = this.mIdMap[id];
				Logger.d("Mapping alias " + id + " to ID " + text);
				return text;
			}
			return id;
		}
	}
}
