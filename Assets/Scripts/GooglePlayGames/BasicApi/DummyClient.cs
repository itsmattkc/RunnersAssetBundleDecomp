using GooglePlayGames.BasicApi.Multiplayer;
using System;
using System.Collections.Generic;

namespace GooglePlayGames.BasicApi
{
	public class DummyClient : IPlayGamesClient
	{
		public void Authenticate(Action<bool> callback, bool silent)
		{
			if (callback != null)
			{
				callback(false);
			}
		}

		public bool IsAuthenticated()
		{
			return false;
		}

		public void SignOut()
		{
		}

		public string GetUserId()
		{
			return "DummyID";
		}

		public string GetUserDisplayName()
		{
			return "Player";
		}

		public List<Achievement> GetAchievements()
		{
			return new List<Achievement>();
		}

		public Achievement GetAchievement(string achId)
		{
			return null;
		}

		public void UnlockAchievement(string achId, Action<bool> callback)
		{
			if (callback != null)
			{
				callback(false);
			}
		}

		public void RevealAchievement(string achId, Action<bool> callback)
		{
			if (callback != null)
			{
				callback(false);
			}
		}

		public void IncrementAchievement(string achId, int steps, Action<bool> callback)
		{
			if (callback != null)
			{
				callback(false);
			}
		}

		public void ShowAchievementsUI()
		{
		}

		public void ShowLeaderboardUI(string lbId)
		{
		}

		public void SubmitScore(string lbId, long score, Action<bool> callback)
		{
			if (callback != null)
			{
				callback(false);
			}
		}

		public void LoadState(int slot, OnStateLoadedListener listener)
		{
			if (listener != null)
			{
				listener.OnStateLoaded(false, slot, null);
			}
		}

		public void UpdateState(int slot, byte[] data, OnStateLoadedListener listener)
		{
		}

		public void SetCloudCacheEncrypter(BufferEncrypter encrypter)
		{
		}

		public IRealTimeMultiplayerClient GetRtmpClient()
		{
			return null;
		}

		public ITurnBasedMultiplayerClient GetTbmpClient()
		{
			return null;
		}

		public void RegisterInvitationDelegate(InvitationReceivedDelegate deleg)
		{
		}

		public Invitation GetInvitationFromNotification()
		{
			return null;
		}

		public bool HasInvitationFromNotification()
		{
			return false;
		}
	}
}
