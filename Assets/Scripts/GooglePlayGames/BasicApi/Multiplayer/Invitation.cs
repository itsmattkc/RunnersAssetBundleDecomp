using System;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class Invitation
	{
		public enum InvType
		{
			RealTime,
			TurnBased,
			Unknown
		}

		private Invitation.InvType mInvitationType;

		private string mInvitationId;

		private Participant mInviter;

		private int mVariant;

		public Invitation.InvType InvitationType
		{
			get
			{
				return this.mInvitationType;
			}
		}

		public string InvitationId
		{
			get
			{
				return this.mInvitationId;
			}
		}

		public Participant Inviter
		{
			get
			{
				return this.mInviter;
			}
		}

		public int Variant
		{
			get
			{
				return this.mVariant;
			}
		}

		internal Invitation(Invitation.InvType invType, string invId, Participant inviter, int variant)
		{
			this.mInvitationType = invType;
			this.mInvitationId = invId;
			this.mInviter = inviter;
			this.mVariant = variant;
		}

		public override string ToString()
		{
			return string.Format("[Invitation: InvitationType={0}, InvitationId={1}, Inviter={2}, Variant={3}]", new object[]
			{
				this.InvitationType,
				this.InvitationId,
				this.Inviter,
				this.Variant
			});
		}
	}
}
