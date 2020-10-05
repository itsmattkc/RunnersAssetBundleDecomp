using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class TurnBasedMatch
	{
		public enum MatchStatus
		{
			Active,
			AutoMatching,
			Cancelled,
			Complete,
			Expired,
			Unknown,
			Deleted
		}

		public enum MatchTurnStatus
		{
			Complete,
			Invited,
			MyTurn,
			TheirTurn,
			Unknown
		}

		private string mMatchId;

		private byte[] mData;

		private bool mCanRematch;

		private int mAvailableAutomatchSlots;

		private string mSelfParticipantId;

		private List<Participant> mParticipants;

		private string mPendingParticipantId;

		private TurnBasedMatch.MatchTurnStatus mTurnStatus;

		private TurnBasedMatch.MatchStatus mMatchStatus;

		private int mVariant;

		public string MatchId
		{
			get
			{
				return this.mMatchId;
			}
		}

		public byte[] Data
		{
			get
			{
				return this.mData;
			}
		}

		public bool CanRematch
		{
			get
			{
				return this.mCanRematch;
			}
		}

		public string SelfParticipantId
		{
			get
			{
				return this.mSelfParticipantId;
			}
		}

		public Participant Self
		{
			get
			{
				return this.GetParticipant(this.mSelfParticipantId);
			}
		}

		public List<Participant> Participants
		{
			get
			{
				return this.mParticipants;
			}
		}

		public string PendingParticipantId
		{
			get
			{
				return this.mPendingParticipantId;
			}
		}

		public Participant PendingParticipant
		{
			get
			{
				return (this.mPendingParticipantId != null) ? this.GetParticipant(this.mPendingParticipantId) : null;
			}
		}

		public TurnBasedMatch.MatchTurnStatus TurnStatus
		{
			get
			{
				return this.mTurnStatus;
			}
		}

		public TurnBasedMatch.MatchStatus Status
		{
			get
			{
				return this.mMatchStatus;
			}
		}

		public int Variant
		{
			get
			{
				return this.mVariant;
			}
		}

		public int AvailableAutomatchSlots
		{
			get
			{
				return this.mAvailableAutomatchSlots;
			}
		}

		internal TurnBasedMatch(string matchId, byte[] data, bool canRematch, string selfParticipantId, List<Participant> participants, int availableAutomatchSlots, string pendingParticipantId, TurnBasedMatch.MatchTurnStatus turnStatus, TurnBasedMatch.MatchStatus matchStatus, int variant)
		{
			this.mMatchId = matchId;
			this.mData = data;
			this.mCanRematch = canRematch;
			this.mSelfParticipantId = selfParticipantId;
			this.mParticipants = participants;
			this.mParticipants.Sort();
			this.mAvailableAutomatchSlots = availableAutomatchSlots;
			this.mPendingParticipantId = pendingParticipantId;
			this.mTurnStatus = turnStatus;
			this.mMatchStatus = matchStatus;
			this.mVariant = variant;
		}

		public Participant GetParticipant(string participantId)
		{
			foreach (Participant current in this.mParticipants)
			{
				if (current.ParticipantId.Equals(participantId))
				{
					return current;
				}
			}
			Logger.w("Participant not found in turn-based match: " + participantId);
			return null;
		}

		public override string ToString()
		{
			return string.Format("[TurnBasedMatch: mMatchId={0}, mData={1}, mCanRematch={2}, mSelfParticipantId={3}, mParticipants={4}, mPendingParticipantId={5}, mTurnStatus={6}, mMatchStatus={7}, mVariant={8}]", new object[]
			{
				this.mMatchId,
				this.mData,
				this.mCanRematch,
				this.mSelfParticipantId,
				this.mParticipants,
				this.mPendingParticipantId,
				this.mTurnStatus,
				this.mMatchStatus,
				this.mVariant
			});
		}
	}
}
