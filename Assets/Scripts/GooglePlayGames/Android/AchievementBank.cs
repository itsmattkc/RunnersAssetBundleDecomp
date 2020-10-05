using GooglePlayGames.BasicApi;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class AchievementBank
	{
		private Dictionary<string, Achievement> mAchievements = new Dictionary<string, Achievement>();

		internal AchievementBank()
		{
		}

		internal void ProcessBuffer(AndroidJavaObject achBuffer)
		{
			Logger.d("AchievementBank: processing achievement buffer given as Java object.");
			if (achBuffer == null)
			{
				Logger.w("AchievementBank: given buffer was null. Ignoring.");
				return;
			}
			int num = achBuffer.Call<int>("getCount", new object[0]);
			Logger.d("AchievementBank: buffer contains " + num + " achievements.");
			for (int i = 0; i < num; i++)
			{
				Logger.d("AchievementBank: processing achievement #" + i);
				Achievement achievement = new Achievement();
				AndroidJavaObject androidJavaObject = achBuffer.Call<AndroidJavaObject>("get", new object[]
				{
					i
				});
				if (androidJavaObject == null)
				{
					Logger.w("Achievement #" + i + " was null. Ignoring.");
				}
				else
				{
					achievement.Id = androidJavaObject.Call<string>("getAchievementId", new object[0]);
					achievement.IsIncremental = (androidJavaObject.Call<int>("getType", new object[0]) == 1);
					int num2 = androidJavaObject.Call<int>("getState", new object[0]);
					achievement.IsRevealed = (num2 != 2);
					achievement.IsUnlocked = (num2 == 0);
					achievement.Name = androidJavaObject.Call<string>("getName", new object[0]);
					achievement.Description = androidJavaObject.Call<string>("getDescription", new object[0]);
					if (achievement.IsIncremental)
					{
						achievement.CurrentSteps = androidJavaObject.Call<int>("getCurrentSteps", new object[0]);
						achievement.TotalSteps = androidJavaObject.Call<int>("getTotalSteps", new object[0]);
					}
					Logger.d("AchievementBank: processed: " + achievement.ToString());
					if (achievement.Id != null && achievement.Id.Length > 0)
					{
						this.mAchievements[achievement.Id] = achievement;
					}
					else
					{
						Logger.w("Achievement w/ missing ID received. Ignoring.");
					}
				}
			}
			Logger.d("AchievementBank: bank now contains " + this.mAchievements.Count + " entries.");
		}

		internal Achievement GetAchievement(string id)
		{
			if (this.mAchievements.ContainsKey(id))
			{
				return this.mAchievements[id];
			}
			Logger.w("Achievement ID not found in bank: id " + id);
			return null;
		}

		internal List<Achievement> GetAchievements()
		{
			List<Achievement> list = new List<Achievement>();
			foreach (Achievement current in this.mAchievements.Values)
			{
				list.Add(current);
			}
			return list;
		}
	}
}
