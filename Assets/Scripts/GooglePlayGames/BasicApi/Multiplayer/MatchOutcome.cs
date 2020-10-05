using System;
using System.Collections.Generic;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class MatchOutcome
	{
		public enum ParticipantResult
		{
			Unset = -1,
			None,
			Win,
			Loss,
			Tie
		}

		public const int PlacementUnset = -1;

		private List<string> mParticipantIds = new List<string>();

		private Dictionary<string, int> mPlacements = new Dictionary<string, int>();

		private Dictionary<string, MatchOutcome.ParticipantResult> mResults = new Dictionary<string, MatchOutcome.ParticipantResult>();

		public List<string> ParticipantIds
		{
			get
			{
				return this.mParticipantIds;
			}
		}

		public void SetParticipantResult(string participantId, MatchOutcome.ParticipantResult result, int placement)
		{
			if (!this.mParticipantIds.Contains(participantId))
			{
				this.mParticipantIds.Add(participantId);
			}
			this.mPlacements[participantId] = placement;
			this.mResults[participantId] = result;
		}

		public void SetParticipantResult(string participantId, MatchOutcome.ParticipantResult result)
		{
			this.SetParticipantResult(participantId, result, -1);
		}

		public void SetParticipantResult(string participantId, int placement)
		{
			this.SetParticipantResult(participantId, MatchOutcome.ParticipantResult.Unset, placement);
		}

		public MatchOutcome.ParticipantResult GetResultFor(string participantId)
		{
			return (!this.mResults.ContainsKey(participantId)) ? MatchOutcome.ParticipantResult.Unset : this.mResults[participantId];
		}

		public int GetPlacementFor(string participantId)
		{
			return (!this.mPlacements.ContainsKey(participantId)) ? (-1) : this.mPlacements[participantId];
		}

		public override string ToString()
		{
			string str = "[MatchOutcome";
			foreach (string current in this.mParticipantIds)
			{
				str += string.Format(" {0}->({1},{2})", current, this.GetResultFor(current), this.GetPlacementFor(current));
			}
			return str + "]";
		}
	}
}
