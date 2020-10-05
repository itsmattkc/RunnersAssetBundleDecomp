using System;
using System.Collections.Generic;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public interface IRealTimeMultiplayerClient
	{
		void CreateQuickGame(int minOpponents, int maxOpponents, int variant, RealTimeMultiplayerListener listener);

		void CreateWithInvitationScreen(int minOpponents, int maxOppponents, int variant, RealTimeMultiplayerListener listener);

		void AcceptFromInbox(RealTimeMultiplayerListener listener);

		void AcceptInvitation(string invitationId, RealTimeMultiplayerListener listener);

		void SendMessageToAll(bool reliable, byte[] data);

		void SendMessageToAll(bool reliable, byte[] data, int offset, int length);

		void SendMessage(bool reliable, string participantId, byte[] data);

		void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length);

		List<Participant> GetConnectedParticipants();

		Participant GetSelf();

		Participant GetParticipant(string participantId);

		void LeaveRoom();

		bool IsRoomConnected();

		void DeclineInvitation(string invitationId);
	}
}
