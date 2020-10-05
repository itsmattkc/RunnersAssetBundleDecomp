using System;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesScore : IScore
	{
		private string mLbId;

		private long mValue;

		public string leaderboardID
		{
			get
			{
				return this.mLbId;
			}
			set
			{
				this.mLbId = value;
			}
		}

		public long value
		{
			get
			{
				return this.mValue;
			}
			set
			{
				this.mValue = value;
			}
		}

		public DateTime date
		{
			get
			{
				return new DateTime(1970, 1, 1, 0, 0, 0);
			}
		}

		public string formattedValue
		{
			get
			{
				return this.mValue.ToString();
			}
		}

		public string userID
		{
			get
			{
				return string.Empty;
			}
		}

		public int rank
		{
			get
			{
				return 1;
			}
		}

		public void ReportScore(Action<bool> callback)
		{
			PlayGamesPlatform.Instance.ReportScore(this.mValue, this.mLbId, callback);
		}
	}
}
