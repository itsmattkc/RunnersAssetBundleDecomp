using System;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesLocalUser : PlayGamesUserProfile, IUserProfile, ILocalUser
	{
		private PlayGamesPlatform mPlatform;

		public IUserProfile[] friends
		{
			get
			{
				return new IUserProfile[0];
			}
		}

		public bool authenticated
		{
			get
			{
				return this.mPlatform.IsAuthenticated();
			}
		}

		public bool underage
		{
			get
			{
				return true;
			}
		}

		public new string userName
		{
			get
			{
				return (!this.authenticated) ? string.Empty : this.mPlatform.GetUserDisplayName();
			}
		}

		public new string id
		{
			get
			{
				return (!this.authenticated) ? string.Empty : this.mPlatform.GetUserId();
			}
		}

		public new bool isFriend
		{
			get
			{
				return true;
			}
		}

		public new UserState state
		{
			get
			{
				return UserState.Online;
			}
		}

		internal PlayGamesLocalUser(PlayGamesPlatform plaf)
		{
			this.mPlatform = plaf;
		}

		public void Authenticate(Action<bool> callback)
		{
			this.mPlatform.Authenticate(callback);
		}

		public void Authenticate(Action<bool> callback, bool silent)
		{
			this.mPlatform.Authenticate(callback, silent);
		}

		public void LoadFriends(Action<bool> callback)
		{
			if (callback != null)
			{
				callback(false);
			}
		}
	}
}
