using System;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesAchievement : IAchievement
	{
		private string mId = string.Empty;

		private double mPercentComplete;

		private DateTime mLastReportedDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		public string id
		{
			get
			{
				return this.mId;
			}
			set
			{
				this.mId = value;
			}
		}

		public double percentCompleted
		{
			get
			{
				return this.mPercentComplete;
			}
			set
			{
				this.mPercentComplete = value;
			}
		}

		public bool completed
		{
			get
			{
				return false;
			}
		}

		public bool hidden
		{
			get
			{
				return false;
			}
		}

		public DateTime lastReportedDate
		{
			get
			{
				return this.mLastReportedDate;
			}
		}

		internal PlayGamesAchievement()
		{
		}

		public void ReportProgress(Action<bool> callback)
		{
			PlayGamesPlatform.Instance.ReportProgress(this.mId, this.mPercentComplete, callback);
		}
	}
}
