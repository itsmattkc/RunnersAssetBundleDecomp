using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace NoahUnity
{
	public class Noah
	{
		public enum BannerSize
		{
			Size224x336,
			Size320x50,
			Size336x224,
			Size480x32,
			SizeNormal,
			SizeNormal2x,
			Size448x672,
			Size640x100,
			Size672x448,
			Size960x64,
			SizeStandard,
			SizeStandard2x,
			SizeWide,
			SizeWide2x,
			SizeStandardFillParentWidth,
			SizeStandard2xFillParentWidth,
			SizeWideFillParentWidth,
			SizeWide2xFillParentWidth
		}

		public enum BannerEffect
		{
			EffectUp,
			EffectDown
		}

		public enum RewardEffect
		{
			EffectUp,
			EffectDown
		}

		public enum OfferButtonImage
		{
			White,
			Black
		}

		public enum ResultState
		{
			CommitOver,
			LackOfPoint,
			Mismatching,
			Unconnected,
			Success,
			Failure,
			Ok,
			Close,
			Change
		}

		public enum BadgeType
		{
			Red,
			Blue,
			Gray
		}

		public enum Orientation
		{
			Landscape,
			Portrait,
			ReverseLandscape,
			ReversePortrait,
			SensorLandscape,
			SensorPortrait,
			Sensor
		}

		private bool m_isShowBanner;

		public static readonly Noah Instance;

		private static NoahPlugin _NoahInstance_k__BackingField;

		private static NoahPlugin NoahInstance
		{
			get;
			set;
		}

		public bool IsShowBanner
		{
			get
			{
				return this.m_isShowBanner;
			}
			private set
			{
			}
		}

		private Noah()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				Noah.NoahInstance = new NoahAndroid();
			}
			if (Noah.NoahInstance != null)
			{
				Noah.NoahInstance.Initialize();
			}
		}

		static Noah()
		{
			Noah.Instance = new Noah();
		}

		public void Suspend()
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Suspend();
		}

		public void Resume()
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Resume();
		}

		public void Close()
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Close();
		}

		public void CloseBanner()
		{
			this.m_isShowBanner = false;
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.CloseBanner();
		}

		public void Commit(string action_id)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Commit(action_id);
		}

		public void Connect(string consumer_key, string secret_key)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Connect(consumer_key, secret_key);
		}

		public void Connect(string consumer_key, string secret_key, string action_id)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Connect(consumer_key, secret_key, action_id);
		}

		public void Delete()
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Delete();
		}

		public void AdAlert()
		{
		}

		public bool GetAdIdFlag()
		{
			return false;
		}

		public void GetBannerView(Noah.BannerSize size, int x, int y)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.GetBannerView(size, (float)x, (float)y);
		}

		public bool GetBannerFlag()
		{
			return Noah.NoahInstance != null && Noah.NoahInstance.GetBannerFlag();
		}

		public bool GetBannerWallFlag()
		{
			return Noah.NoahInstance != null && Noah.NoahInstance.GetBannerWallFlag();
		}

		public string GetNoahID()
		{
			if (Noah.NoahInstance == null)
			{
				return null;
			}
			return Noah.NoahInstance.GetNoahID();
		}

		public string GetCheckToken()
		{
			if (Noah.NoahInstance == null)
			{
				return null;
			}
			return Noah.NoahInstance.GetCheckToken();
		}

		public ArrayList GetAlertMessage()
		{
			if (Noah.NoahInstance == null)
			{
				return null;
			}
			return Noah.NoahInstance.GetAlertMessage();
		}

		public bool GetOfferFlag()
		{
			return Noah.NoahInstance != null && Noah.NoahInstance.GetOfferFlag();
		}

		public Texture2D GetOfferBitmap(Noah.OfferButtonImage type)
		{
			if (Noah.NoahInstance == null)
			{
				return null;
			}
			return Noah.NoahInstance.GetOfferBitmap(type);
		}

		public void GetPoint()
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.GetPoint();
		}

		public void GetPurchased()
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.GetPurchased();
		}

		public bool GetReviewFlag()
		{
			return Noah.NoahInstance != null && Noah.NoahInstance.GetReviewFlag();
		}

		public bool GetRewardFlag()
		{
			return Noah.NoahInstance != null && Noah.NoahInstance.GetRewardFlag();
		}

		public int GetRewardNum()
		{
			if (Noah.NoahInstance == null)
			{
				return -1;
			}
			return Noah.NoahInstance.GetRewardNum();
		}

		public void SetRewardEffect(Noah.RewardEffect effect)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.SetRewardEffect(effect);
		}

		public void GetRewardView(int x, int y)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.GetRewardView((float)x, (float)y);
		}

		public bool GetShopFlag()
		{
			return Noah.NoahInstance != null && Noah.NoahInstance.GetShopFlag();
		}

		public string GetVersion()
		{
			if (Noah.NoahInstance == null)
			{
				return null;
			}
			return Noah.NoahInstance.GetVersion();
		}

		public void Offer(string guid, Noah.Orientation orientation)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Offer(guid, orientation);
		}

		public void Offer(string guid, Noah.Orientation orientation, string tag)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Offer(guid, orientation, tag);
		}

		public void CloseOffer()
		{
		}

		public void Review()
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Review();
		}

		public void Review(string tag)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Review(tag);
		}

		public void SetBannerEffect(Noah.BannerEffect effect)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.SetBannerEffect(effect);
		}

		public void SetDebugMode(bool flag)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.SetDebugMode(flag);
		}

		public void SetGUID(string guid)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.SetGUID(guid);
		}

		public void Shop(string guid, Noah.Orientation orientation)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Shop(guid, orientation);
		}

		public void Shop(string guid, Noah.Orientation orientation, string tag)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.Shop(guid, orientation, tag);
		}

		public void CloseShop()
		{
		}

		public void UsePoint(int use_point)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.UsePoint(use_point);
		}

		[Obsolete("This method is obsolete; use HasNewOffer")]
		public bool HasNewReward()
		{
			return Noah.NoahInstance != null && Noah.NoahInstance.HasNewReward();
		}

		public bool HasNewOffer()
		{
			return Noah.NoahInstance != null && Noah.NoahInstance.HasNewOffer();
		}

		public Texture2D GetNewBadge(Noah.BadgeType type)
		{
			if (Noah.NoahInstance == null)
			{
				return null;
			}
			return Noah.NoahInstance.GetNewBadge(type);
		}

		public static Noah.ResultState ConvertResultState(int result)
		{
			if (Noah.NoahInstance == null)
			{
				return Noah.ResultState.Failure;
			}
			return Noah.NoahInstance.ConvertResultState(result);
		}

		public void BannerWall(Noah.Orientation orientation)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.BannerWall(orientation);
		}

		public void BannerWall(Noah.Orientation orientation, bool isRotate)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.BannerWall(orientation, isRotate);
		}

		public void BannerWall(Noah.Orientation orientation, bool isRotate, string tag)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.BannerWall(orientation, isRotate, tag);
		}

		public Vector2 GetBannerSize(Noah.BannerSize size)
		{
			if (Noah.NoahInstance == null)
			{
				return Vector2.zero;
			}
			return Noah.NoahInstance.GetBannerSize(size);
		}

		public void ShowBannerView(Noah.BannerSize size, float x, float y)
		{
			this.m_isShowBanner = true;
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.ShowBannerView(size, x, y);
		}

		public void ShowBannerView(Noah.BannerSize size, float x, float y, string tag)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.ShowBannerView(size, x, y, tag);
		}

		public Vector2 GetRewardSize()
		{
			if (Noah.NoahInstance == null)
			{
				return Vector2.zero;
			}
			return Noah.NoahInstance.GetRewardSize();
		}

		public void ShowRewardView(float x, float y)
		{
			if (Noah.NoahInstance == null)
			{
				return;
			}
			Noah.NoahInstance.ShowRewardView(x, y);
		}
	}
}
