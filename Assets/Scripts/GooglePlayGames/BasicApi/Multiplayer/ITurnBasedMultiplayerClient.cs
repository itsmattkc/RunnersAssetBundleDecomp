using System;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public interface ITurnBasedMultiplayerClient
	{
		void CreateQuickMatch(int minOpponents, int maxOpponents, int variant, Action<bool, TurnBasedMatch> callback);

		void CreateWithInvitationScreen(int minOpponents, int maxOpponents, int variant, Action<bool, TurnBasedMatch> callback);

		void AcceptFromInbox(Action<bool, TurnBasedMatch> callback);

		void AcceptInvitation(string invitationId, Action<bool, TurnBasedMatch> callback);

		void RegisterMatchDelegate(MatchDelegate del);

		void TakeTurn(string matchId, byte[] data, string pendingParticipantId, Action<bool> callback);

		int GetMaxMatchDataSize();

		void Finish(string matchId, byte[] data, MatchOutcome outcome, Action<bool> callback);

		void AcknowledgeFinished(string matchId, Action<bool> callback);

		void Leave(string matchId, Action<bool> callback);

		void LeaveDuringTurn(string matchId, string pendingParticipantId, Action<bool> callback);

		void Cancel(string matchId, Action<bool> callback);

		void Rematch(string matchId, Action<bool, TurnBasedMatch> callback);

		void DeclineInvitation(string invitationId);
	}
}
