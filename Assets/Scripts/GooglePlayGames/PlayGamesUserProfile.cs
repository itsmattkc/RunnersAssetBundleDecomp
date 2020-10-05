using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesUserProfile : IUserProfile
	{
		public string userName
		{
			get
			{
				return string.Empty;
			}
		}

		public string id
		{
			get
			{
				return string.Empty;
			}
		}

		public bool isFriend
		{
			get
			{
				return false;
			}
		}

		public UserState state
		{
			get
			{
				return UserState.Online;
			}
		}

		public Texture2D image
		{
			get
			{
				return null;
			}
		}

		internal PlayGamesUserProfile()
		{
		}
	}
}
