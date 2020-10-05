using System;

namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class Player
	{
		private string mDisplayName = string.Empty;

		private string mPlayerId = string.Empty;

		public string DisplayName
		{
			get
			{
				return this.mDisplayName;
			}
		}

		public string PlayerId
		{
			get
			{
				return this.mPlayerId;
			}
		}

		internal Player(string displayName, string playerId)
		{
			this.mDisplayName = displayName;
			this.mPlayerId = playerId;
		}

		public override string ToString()
		{
			return string.Format("[Player: '{0}' (id {1})]", this.mDisplayName, this.mPlayerId);
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
			if (obj.GetType() != typeof(Player))
			{
				return false;
			}
			Player player = (Player)obj;
			return this.mPlayerId == player.mPlayerId;
		}

		public override int GetHashCode()
		{
			return (this.mPlayerId == null) ? 0 : this.mPlayerId.GetHashCode();
		}
	}
}
