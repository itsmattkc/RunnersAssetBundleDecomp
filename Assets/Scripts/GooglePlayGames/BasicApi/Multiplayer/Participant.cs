using System;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class Participant : IComparable<Participant>
	{
		public enum ParticipantStatus
		{
			NotInvitedYet,
			Invited,
			Joined,
			Declined,
			Left,
			Finished,
			Unresponsive,
			Unknown
		}

		private string mDisplayName = string.Empty;

		private string mParticipantId = string.Empty;

		private Participant.ParticipantStatus mStatus = Participant.ParticipantStatus.Unknown;

		private Player mPlayer;

		private bool mIsConnectedToRoom;

		public string DisplayName
		{
			get
			{
				return this.mDisplayName;
			}
		}

		public string ParticipantId
		{
			get
			{
				return this.mParticipantId;
			}
		}

		public Participant.ParticipantStatus Status
		{
			get
			{
				return this.mStatus;
			}
		}

		public Player Player
		{
			get
			{
				return this.mPlayer;
			}
		}

		public bool IsConnectedToRoom
		{
			get
			{
				return this.mIsConnectedToRoom;
			}
		}

		public bool IsAutomatch
		{
			get
			{
				return this.mPlayer == null;
			}
		}

		internal Participant(string displayName, string participantId, Participant.ParticipantStatus status, Player player, bool connectedToRoom)
		{
			this.mDisplayName = displayName;
			this.mParticipantId = participantId;
			this.mStatus = status;
			this.mPlayer = player;
			this.mIsConnectedToRoom = connectedToRoom;
		}

		public override string ToString()
		{
			return string.Format("[Participant: '{0}' (id {1}), status={2}, player={3}, connected={4}]", new object[]
			{
				this.mDisplayName,
				this.mParticipantId,
				this.mStatus.ToString(),
				(this.mPlayer != null) ? this.mPlayer.ToString() : "NULL",
				this.mIsConnectedToRoom
			});
		}

		public int CompareTo(Participant other)
		{
			return this.mParticipantId.CompareTo(other.mParticipantId);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != typeof(Participant))
			{
				return false;
			}
			Participant participant = (Participant)obj;
			return this.mParticipantId.Equals(participant.mParticipantId);
		}

		public override int GetHashCode()
		{
			return (this.mParticipantId == null) ? 0 : this.mParticipantId.GetHashCode();
		}
	}
}
