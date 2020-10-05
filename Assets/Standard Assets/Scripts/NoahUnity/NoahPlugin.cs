using System;
using System.Collections;
using UnityEngine;

namespace NoahUnity
{
	public abstract class NoahPlugin
	{
		public abstract void Initialize();

		public abstract void Suspend();

		public abstract void Resume();

		public abstract void Close();

		public abstract void CloseBanner();

		public abstract void Commit(string action_id);

		public abstract void Connect(string consumer_key, string secret_key);

		public abstract void Connect(string consumer_key, string secret_key, string action_id);

		public abstract void Delete();

		public abstract void GetBannerView(Noah.BannerSize size, float x, float y);

		public abstract void GetBannerView(Noah.BannerSize size, float x, float y, string tag);

		public abstract bool GetBannerFlag();

		public abstract string GetNoahID();

		public abstract string GetCheckToken();

		public abstract bool GetOfferFlag();

		public abstract Texture2D GetOfferBitmap(Noah.OfferButtonImage type);

		public abstract void GetPoint();

		public abstract void GetPurchased();

		public abstract bool GetReviewFlag();

		public abstract bool GetRewardFlag();

		public abstract bool GetBannerWallFlag();

		public abstract int GetRewardNum();

		public abstract void SetRewardEffect(Noah.RewardEffect effect);

		public abstract void GetRewardView(float x, float y);

		public abstract bool GetShopFlag();

		public abstract string GetVersion();

		public abstract void Offer(string guid, Noah.Orientation orientation);

		public abstract void Offer(string guid, Noah.Orientation orientation, string tag);

		public abstract void Review();

		public abstract void Review(string tag);

		public abstract void SetBannerEffect(Noah.BannerEffect effect);

		public abstract void SetDebugMode(bool flag);

		public abstract bool GetDebugMode();

		public abstract void SetGUID(string guid);

		public abstract void Shop(string guid, Noah.Orientation orientation);

		public abstract void Shop(string guid, Noah.Orientation orientation, string tag);

		public abstract void UsePoint(int use_point);

		public abstract bool HasNewReward();

		public abstract bool HasNewOffer();

		public abstract Texture2D GetNewBadge(Noah.BadgeType badge_type);

		public abstract Noah.ResultState ConvertResultState(int result);

		public abstract void BannerWall(Noah.Orientation orientation);

		public abstract void BannerWall(Noah.Orientation orientation, bool isRotatable);

		public abstract void BannerWall(Noah.Orientation orientation, bool isRotatable, string tag);

		public abstract Vector2 GetBannerSize(Noah.BannerSize size);

		public abstract void ShowBannerView(Noah.BannerSize size, float x, float y);

		public abstract void ShowBannerView(Noah.BannerSize size, float x, float y, string tag);

		public abstract Vector2 GetRewardSize();

		public abstract void ShowRewardView(float x, float y);

		public abstract ArrayList GetAlertMessage();

		public abstract bool IsConnecting();

		public abstract int GetOfferDisplayType();

		public abstract string GetLastErrorMessage();
	}
}
