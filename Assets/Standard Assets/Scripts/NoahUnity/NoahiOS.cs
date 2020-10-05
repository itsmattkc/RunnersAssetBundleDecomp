using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NoahUnity
{
	public class NoahiOS : NoahPlugin
	{
		private enum NoahBannerSize
		{
			NoahBannerSize224x336,
			NoahBannerSize336x224,
			NoahBannerSize320x50,
			NoahBannerSize480x32,
			NoahBannerSize448x672,
			NoahBannerSize672x448,
			NoahBannerSize640x100,
			NoahBannerSize960x64,
			SizeStandard,
			SizeStandard2x,
			SizeWide,
			SizeWide2x
		}

		private enum NoahConnectStatusCode
		{
			NoahConnectStatusCodeUnConnected,
			NoahConnectStatusCodeFailed,
			NoahConnectStatusCodeSuccessful,
			NoahConnectStatusCodeCommitOver,
			NoahConnectStatusCodeLackOfPoint,
			NoahConnectStatusCodeMismatched,
			NoahConnectStatusCodeChange,
			NoahConnectStatusCodeClose,
			NoahConnectStatusCodeOK
		}

		private enum NoahDrawOrientation
		{
			NoahDrawOrientationPortrait,
			NoahDrawOrientationLandscapse
		}

		private enum NoahOfferButton
		{
			NoahOfferButtonWhite,
			NoahOfferButtonBlack
		}

		private enum NoahBannerEffect
		{
			NoahBannerEffectDown,
			NoahBannerEffectUp
		}

		private enum NoahNewRewardEffect
		{
			NoahNewRewardEffectDown,
			NoahNewRewardEffectUp
		}

		private enum NoahBadgeType
		{
			NoahBadgeDefaultRed,
			NoahBadgeDefaultBlue,
			NoahBadgeDefaultGray
		}

		public enum DeviceOrientation
		{
			Portrait,
			PortraitUpsideDown,
			LandscapeLeft,
			LandscapeRight
		}

		public const string NOHA_UNITY_IOS_VERSION = "1.7.0";

		private const string tmpFileName = "offer_btn.png";

		private const string badgeFileName = "badge_btn.png";

		private int ConvertBannerSize(Noah.BannerSize size)
		{
			switch (size)
			{
			case Noah.BannerSize.Size224x336:
				return 0;
			case Noah.BannerSize.Size320x50:
				return 2;
			case Noah.BannerSize.Size336x224:
				return 1;
			case Noah.BannerSize.Size480x32:
				return 3;
			case Noah.BannerSize.SizeNormal:
				return 2;
			case Noah.BannerSize.Size448x672:
				return 4;
			case Noah.BannerSize.Size640x100:
				return 6;
			case Noah.BannerSize.Size672x448:
				return 5;
			case Noah.BannerSize.Size960x64:
				return 7;
			case Noah.BannerSize.SizeStandard:
				return 8;
			case Noah.BannerSize.SizeStandard2x:
				return 9;
			case Noah.BannerSize.SizeWide:
				return 10;
			case Noah.BannerSize.SizeWide2x:
				return 11;
			}
			return 2;
		}

		private int ConvertBannerEffect(Noah.BannerEffect effect)
		{
			if (effect == Noah.BannerEffect.EffectUp)
			{
				return 1;
			}
			if (effect != Noah.BannerEffect.EffectDown)
			{
				return 0;
			}
			return 0;
		}

		private int ConvertRewardEffect(Noah.RewardEffect effect)
		{
			if (effect == Noah.RewardEffect.EffectUp)
			{
				return 1;
			}
			if (effect != Noah.RewardEffect.EffectDown)
			{
				return 0;
			}
			return 0;
		}

		private int ConvertBadgeType(Noah.BadgeType type)
		{
			switch (type)
			{
			case Noah.BadgeType.Red:
				return 0;
			case Noah.BadgeType.Blue:
				return 1;
			case Noah.BadgeType.Gray:
				return 2;
			default:
				return 0;
			}
		}

		private int ConvertResultState(Noah.ResultState state)
		{
			switch (state)
			{
			case Noah.ResultState.CommitOver:
				return 3;
			case Noah.ResultState.LackOfPoint:
				return 4;
			case Noah.ResultState.Mismatching:
				return 5;
			case Noah.ResultState.Unconnected:
				return 0;
			case Noah.ResultState.Success:
				return 2;
			case Noah.ResultState.Failure:
				return 1;
			case Noah.ResultState.Ok:
				return 8;
			case Noah.ResultState.Close:
				return 7;
			case Noah.ResultState.Change:
				return 6;
			default:
				return 1;
			}
		}

		public override Noah.ResultState ConvertResultState(int result)
		{
			switch (result)
			{
			case 0:
				return Noah.ResultState.Unconnected;
			case 1:
				return Noah.ResultState.Failure;
			case 2:
				return Noah.ResultState.Success;
			case 3:
				return Noah.ResultState.CommitOver;
			case 4:
				return Noah.ResultState.LackOfPoint;
			case 5:
				return Noah.ResultState.Mismatching;
			case 6:
				return Noah.ResultState.Change;
			case 7:
				return Noah.ResultState.Close;
			case 8:
				return Noah.ResultState.Ok;
			default:
				return Noah.ResultState.Failure;
			}
		}

		private int ConvertOfferButtonImage(Noah.OfferButtonImage type)
		{
			if (type == Noah.OfferButtonImage.White)
			{
				return 0;
			}
			if (type != Noah.OfferButtonImage.Black)
			{
				return 0;
			}
			return 1;
		}

		private int ConvertOrientation(Noah.Orientation orientation)
		{
			switch (orientation)
			{
			case Noah.Orientation.Landscape:
			case Noah.Orientation.ReverseLandscape:
			case Noah.Orientation.SensorLandscape:
				return 1;
			case Noah.Orientation.Portrait:
			case Noah.Orientation.ReversePortrait:
			case Noah.Orientation.SensorPortrait:
				return 0;
			default:
				return 1;
			}
		}

		private int ConvertBannerWallOrientation(Noah.Orientation orientation)
		{
			switch (orientation)
			{
			case Noah.Orientation.Landscape:
				return 1;
			case Noah.Orientation.ReverseLandscape:
				return 3;
			case Noah.Orientation.ReversePortrait:
				return 2;
			case Noah.Orientation.SensorLandscape:
				return 5;
			case Noah.Orientation.SensorPortrait:
				return 4;
			}
			return 0;
		}

		[DllImport("__Internal")]
		private static extern string NoahGetNSTemporaryDirectoryPath_();

		[DllImport("__Internal")]
		private static extern void NoahInitialize_();

		[DllImport("__Internal")]
		private static extern void NoahConnect_(string consumer_key, string secret_key);

		[DllImport("__Internal")]
		private static extern void NoahConnect2_(string consumer_key, string secret_key, string action_id);

		[DllImport("__Internal")]
		private static extern void NoahAdAlert_();

		[DllImport("__Internal")]
		private static extern bool NoahGetAdIdFlag_();

		[DllImport("__Internal")]
		private static extern void NoahSetGUID_(string guid);

		[DllImport("__Internal")]
		private static extern void NoahShowBanner_(int type, float x, float y);

		[DllImport("__Internal")]
		private static extern void NoahShowBannerWithTag_(int type, float x, float y, string tag);

		[DllImport("__Internal")]
		private static extern void NoahSetBannerEffect_(int effect_type);

		[DllImport("__Internal")]
		private static extern void NoahCloseBanner_();

		[DllImport("__Internal")]
		private static extern void NoahOffer_(string guid, int orientation);

		[DllImport("__Internal")]
		private static extern void NoahOfferWithTag_(string guid, int orientation, string tag);

		[DllImport("__Internal")]
		private static extern void NoahCloseOffer_();

		[DllImport("__Internal")]
		private static extern void NoahShop_(string guid, int orientation);

		[DllImport("__Internal")]
		private static extern void NoahShopWithTag_(string guid, int orientation, string tag);

		[DllImport("__Internal")]
		private static extern void NoahCloseShop_();

		[DllImport("__Internal")]
		private static extern void NoahGetPurchased_();

		[DllImport("__Internal")]
		private static extern void NoahCommit_(string action_id);

		[DllImport("__Internal")]
		private static extern bool NoahGetOfferImage_(int button_num, string tmpFileName);

		[DllImport("__Internal")]
		private static extern void NoahReview_();

		[DllImport("__Internal")]
		private static extern void NoahReviewWithTag_(string tag);

		[DllImport("__Internal")]
		private static extern void NoahGetPoint_();

		[DllImport("__Internal")]
		private static extern void NoahUsePoint_(int use_point);

		[DllImport("__Internal")]
		private static extern string NoahGetVersion_();

		[DllImport("__Internal")]
		private static extern int NoahGetNewRewardNum_();

		[DllImport("__Internal")]
		private static extern void NoahShowNewRewardLabel_(float x, float y);

		[DllImport("__Internal")]
		private static extern void NoahSetNewRewardLabelEffect_(int effect_type);

		[DllImport("__Internal")]
		private static extern bool NoahGetBannerFlag_();

		[DllImport("__Internal")]
		private static extern bool NoahGetOfferFlag_();

		[DllImport("__Internal")]
		private static extern bool NoahGetShopFlag_();

		[DllImport("__Internal")]
		private static extern bool NoahGetReviewFlag_();

		[DllImport("__Internal")]
		private static extern bool NoahGetRewardFlag_();

		[DllImport("__Internal")]
		private static extern bool NoahGetBannerWallFlag_();

		[DllImport("__Internal")]
		private static extern string NoahGetNoahId_();

		[DllImport("__Internal")]
		private static extern string NoahGetCheckToken_();

		[DllImport("__Internal")]
		private static extern string NoahGetAlertMessage_();

		[DllImport("__Internal")]
		private static extern void NoahBackground_();

		[DllImport("__Internal")]
		private static extern void NoahResume_();

		[DllImport("__Internal")]
		private static extern void NoahSetDebugMode_(bool flg);

		[DllImport("__Internal")]
		private static extern bool NoahGetDebugMode_();

		[DllImport("__Internal")]
		private static extern void NoahDeleteUserData_();

		[DllImport("__Internal")]
		private static extern bool NoahHasNewReward_();

		[DllImport("__Internal")]
		private static extern bool NoahHasNewOffer_();

		[DllImport("__Internal")]
		private static extern void NoahBannerWall_(int orientation);

		[DllImport("__Internal")]
		private static extern void NoahBannerWall2_(int type, bool flg);

		[DllImport("__Internal")]
		private static extern void NoahBannerWallWithTag_(int type, bool flg, string tag);

		[DllImport("__Internal")]
		private static extern bool NoahGetNewBadge_(int button_num, string tmpFileName);

		[DllImport("__Internal")]
		private static extern void NoahAlert_(string title, string message);

		[DllImport("__Internal")]
		private static extern bool NoahIsConnecting_();

		[DllImport("__Internal")]
		private static extern int NoahGetOfferDisplayType_();

		[DllImport("__Internal")]
		private static extern string NoahGetLastErrorMessage_();

		[DllImport("__Internal")]
		private static extern int GetRenderScreenWidth_();

		[DllImport("__Internal")]
		private static extern int GetRenderScreenHeight_();

		[DllImport("__Internal")]
		private static extern float NoahGetDisplayScale_();

		public override void Initialize()
		{
		}

		public override void Suspend()
		{
		}

		public override void Resume()
		{
		}

		public override void Close()
		{
		}

		public override void CloseBanner()
		{
		}

		public override void Commit(string action_id)
		{
		}

		public override void Connect(string consumer_key, string secret_key)
		{
		}

		public override void Connect(string consumer_key, string secret_key, string action_id)
		{
		}

		public override void Delete()
		{
		}

		public void AdAlert()
		{
		}

		public bool GetAdIdFlag()
		{
			return false;
		}

		public override void GetBannerView(Noah.BannerSize size, float x, float y)
		{
		}

		public override void GetBannerView(Noah.BannerSize size, float x, float y, string tag)
		{
		}

		public override bool GetBannerFlag()
		{
			return false;
		}

		public override string GetNoahID()
		{
			return null;
		}

		public override string GetCheckToken()
		{
			return null;
		}

		public override ArrayList GetAlertMessage()
		{
			return null;
		}

		public override bool GetOfferFlag()
		{
			return false;
		}

		public override Texture2D GetOfferBitmap(Noah.OfferButtonImage type)
		{
			return null;
		}

		public override void GetPoint()
		{
		}

		public override void GetPurchased()
		{
		}

		public override bool GetReviewFlag()
		{
			return false;
		}

		public override bool GetRewardFlag()
		{
			return false;
		}

		public override int GetRewardNum()
		{
			return -1;
		}

		public override void SetRewardEffect(Noah.RewardEffect effect)
		{
		}

		public override void GetRewardView(float x, float y)
		{
		}

		public override bool GetShopFlag()
		{
			return false;
		}

		public override string GetVersion()
		{
			return null;
		}

		public override void Offer(string guid, Noah.Orientation orientation)
		{
		}

		public override void Offer(string guid, Noah.Orientation orientation, string tag)
		{
		}

		public override void Review()
		{
		}

		public override void Review(string tag)
		{
		}

		public override void SetBannerEffect(Noah.BannerEffect effect)
		{
		}

		public override void SetDebugMode(bool flag)
		{
		}

		public override bool GetDebugMode()
		{
			return false;
		}

		public override void SetGUID(string guid)
		{
		}

		public override void Shop(string guid, Noah.Orientation orientation)
		{
		}

		public override void Shop(string guid, Noah.Orientation orientation, string tag)
		{
		}

		public override void UsePoint(int use_point)
		{
		}

		public void CloseOffer()
		{
		}

		public void CloseShop()
		{
		}

		public override bool HasNewReward()
		{
			return false;
		}

		public override bool HasNewOffer()
		{
			return false;
		}

		public override Texture2D GetNewBadge(Noah.BadgeType type)
		{
			return null;
		}

		public static void Alert(string title, string message)
		{
		}

		public override void BannerWall(Noah.Orientation orientation)
		{
		}

		public override void BannerWall(Noah.Orientation orientation, bool isRotatable)
		{
		}

		public override void BannerWall(Noah.Orientation orientation, bool isRotatable, string tag)
		{
		}

		public override bool GetBannerWallFlag()
		{
			return false;
		}

		public static bool IsRetina()
		{
			return false;
		}

		public static float DisplayScale()
		{
			return 1f;
		}

		public override Vector2 GetBannerSize(Noah.BannerSize size)
		{
			return Vector2.zero;
		}

		public override void ShowBannerView(Noah.BannerSize size, float x, float y)
		{
		}

		public override void ShowBannerView(Noah.BannerSize size, float x, float y, string tag)
		{
		}

		public override Vector2 GetRewardSize()
		{
			return Vector2.zero;
		}

		public override void ShowRewardView(float x, float y)
		{
		}

		public override bool IsConnecting()
		{
			return false;
		}

		public override int GetOfferDisplayType()
		{
			return -1;
		}

		public override string GetLastErrorMessage()
		{
			return null;
		}
	}
}
