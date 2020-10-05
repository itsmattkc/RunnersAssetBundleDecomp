using GooglePlayGames.BasicApi.Multiplayer;
using System;

namespace GooglePlayGames.BasicApi
{
	public interface IPlayGamesClient
	{
		void Authenticate(Action<bool> callback, bool silent);

		bool IsAuthenticated();

		void SignOut();

		string GetUserId();

		string GetUserDisplayName();

		Achievement GetAchievement(string achId);

		void UnlockAchievement(string achId, Action<bool> callback);

		void RevealAchievement(string achId, Action<bool> callback);

		void IncrementAchievement(string achId, int steps, Action<bool> callback);

		void ShowAchievementsUI();

		void ShowLeaderboardUI(string lbId);

		void SubmitScore(string lbId, long score, Action<bool> callback);

		void SetCloudCacheEncrypter(BufferEncrypter encrypter);

		void LoadState(int slot, OnStateLoadedListener listener);

		void UpdateState(int slot, byte[] data, OnStateLoadedListener listener);

		IRealTimeMultiplayerClient GetRtmpClient();

		ITurnBasedMultiplayerClient GetTbmpClient();

		void RegisterInvitationDelegate(InvitationReceivedDelegate deleg);
	}
}
