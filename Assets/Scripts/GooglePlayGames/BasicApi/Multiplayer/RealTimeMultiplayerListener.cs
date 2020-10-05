using System;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public interface RealTimeMultiplayerListener
	{
		void OnRoomSetupProgress(float percent);

		void OnRoomConnected(bool success);

		void OnLeftRoom();

		void OnPeersConnected(string[] participantIds);

		void OnPeersDisconnected(string[] participantIds);

		void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data);
	}
}
