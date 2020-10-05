using System;
using System.Collections;
using UnityEngine;

namespace NoahUnity
{
	public class NoahAndroid : NoahPlugin
	{
		public const string NOHA_UNITY_ANDROID_VERSION = "1.7.0";

		public const int BANNER_SIZE_NOMAL = 100;

		public const int BANNER_SIZE_224x336 = 200;

		public const int BANNER_SIZE_320x50 = 201;

		public const int BANNER_SIZE_336x224 = 300;

		public const int BANNER_SIZE_480x32 = 301;

		public const int BANNER_SIZE_NOMAL2X = 101;

		public const int BANNER_SIZE_448x672 = 202;

		public const int BANNER_SIZE_640x100 = 203;

		public const int BANNER_SIZE_672x448 = 302;

		public const int BANNER_SIZE_960x64 = 303;

		public const int BANNER_EFFECT_DOWN = 400;

		public const int BANNER_EFFECT_UP = 401;

		public const int REWARD_EFFECT_DOWN = 400;

		public const int REWARD_EFFECT_UP = 401;

		public const int OFFER_BUTTON_WHITE = 500;

		public const int OFFER_BUTTON_BLACK = 501;

		public const int COMMIT_OVER = 600;

		public const int LACK_OF_POINT = 700;

		public const int MISMATCHING = 701;

		public const int UNCONNECTED = 800;

		public const int SUCCESS = 900;

		public const int FAILURE = 901;

		public const int OK = 902;

		public const int CLOSE = 903;

		public const int CHANGE = 904;

		public const int NOAH_BANNER_SIZE_STANDARD = 100;

		public const int NOAH_BANNER_SIZE_STANDARD_2X = 101;

		public const int NOAH_BANNER_SIZE_WIDE = 102;

		public const int NOAH_BANNER_SIZE_WIDE_2X = 103;

		public const int NOAH_BANNER_SIZE_STANDARD_FILL_PARENT_WIDTH = 201;

		public const int NOAH_BANNER_SIZE_STANDARD_2X_FILL_PARENT_WIDTH = 203;

		public const int NOAH_BANNER_SIZE_WIDE_FILL_PARENT_WIDTH = 301;

		public const int NOAH_BANNER_SIZE_WIDE_2X_FILL_PARENT_WIDTH = 303;

		public const int SCREEN_ORIENTATION_PORTRAIT = 0;

		public const int SCREEN_ORIENTATION_LANDSCAPE = 1;

		public const int SCREEN_ORIENTATION_REVERSE_PORTRAIT = 2;

		public const int SCREEN_ORIENTATION_REVERSE_LANDSCAPE = 3;

		public const int SCREEN_ORIENTATION_SENSOR_PORTRAIT = 4;

		public const int SCREEN_ORIENTATION_SENSOR_LANDSCAPE = 5;

		public const int SCREEN_ORIENTATION_SENSOR = 6;

		public const int BADGE_DEFAULT_RED = 0;

		public const int BADGE_DEFAULT_BLUE = 1;

		public const int BADGE_DEFAULT_GRAY = 2;

		private AndroidJavaClass noahJavaClass;

		private AndroidJavaObject jo;

		public NoahAndroid()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				this.jo = new AndroidJavaObject("jp.co.sega.noah.unity.NoahUnityPlugin", new object[]
				{
					@static
				});
			}
			this.noahJavaClass = new AndroidJavaClass("jp.noahapps.sdk.Noah");
		}

		private int ConvertBannerSize(Noah.BannerSize size)
		{
			switch (size)
			{
			case Noah.BannerSize.Size224x336:
				return 200;
			case Noah.BannerSize.Size320x50:
				return 201;
			case Noah.BannerSize.Size336x224:
				return 300;
			case Noah.BannerSize.Size480x32:
				return 301;
			case Noah.BannerSize.SizeNormal:
				return 100;
			case Noah.BannerSize.SizeNormal2x:
				return 101;
			case Noah.BannerSize.Size448x672:
				return 202;
			case Noah.BannerSize.Size640x100:
				return 203;
			case Noah.BannerSize.Size672x448:
				return 302;
			case Noah.BannerSize.Size960x64:
				return 303;
			case Noah.BannerSize.SizeStandard:
				return 100;
			case Noah.BannerSize.SizeStandard2x:
				return 101;
			case Noah.BannerSize.SizeWide:
				return 102;
			case Noah.BannerSize.SizeWide2x:
				return 103;
			case Noah.BannerSize.SizeStandardFillParentWidth:
				return 201;
			case Noah.BannerSize.SizeStandard2xFillParentWidth:
				return 203;
			case Noah.BannerSize.SizeWideFillParentWidth:
				return 301;
			case Noah.BannerSize.SizeWide2xFillParentWidth:
				return 303;
			default:
				return 201;
			}
		}

		private int ConvertBannerEffect(Noah.BannerEffect effect)
		{
			if (effect == Noah.BannerEffect.EffectUp)
			{
				return 401;
			}
			if (effect != Noah.BannerEffect.EffectDown)
			{
				return 400;
			}
			return 400;
		}

		private int ConvertRewardEffect(Noah.RewardEffect effect)
		{
			if (effect == Noah.RewardEffect.EffectUp)
			{
				return 401;
			}
			if (effect != Noah.RewardEffect.EffectDown)
			{
				return 400;
			}
			return 400;
		}

		private int ConvertOfferButtonImage(Noah.OfferButtonImage type)
		{
			if (type == Noah.OfferButtonImage.White)
			{
				return 500;
			}
			if (type != Noah.OfferButtonImage.Black)
			{
				return 500;
			}
			return 501;
		}

		private int ConvertOrientation(Noah.Orientation orientation)
		{
			switch (orientation)
			{
			case Noah.Orientation.Landscape:
				return 1;
			case Noah.Orientation.Portrait:
				return 0;
			case Noah.Orientation.ReverseLandscape:
				return 3;
			case Noah.Orientation.ReversePortrait:
				return 2;
			case Noah.Orientation.SensorLandscape:
				return 5;
			case Noah.Orientation.SensorPortrait:
				return 4;
			case Noah.Orientation.Sensor:
				return 6;
			default:
				return 1;
			}
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
				return 600;
			case Noah.ResultState.LackOfPoint:
				return 700;
			case Noah.ResultState.Mismatching:
				return 701;
			case Noah.ResultState.Unconnected:
				return 800;
			case Noah.ResultState.Success:
				return 900;
			case Noah.ResultState.Failure:
				return 901;
			case Noah.ResultState.Ok:
				return 902;
			case Noah.ResultState.Close:
				return 903;
			case Noah.ResultState.Change:
				return 904;
			default:
				return 901;
			}
		}

		public override Noah.ResultState ConvertResultState(int result)
		{
			switch (result)
			{
			case 900:
				return Noah.ResultState.Success;
			case 901:
				return Noah.ResultState.Failure;
			case 902:
				return Noah.ResultState.Ok;
			case 903:
				return Noah.ResultState.Close;
			case 904:
				return Noah.ResultState.Change;
			default:
				if (result == 700)
				{
					return Noah.ResultState.LackOfPoint;
				}
				if (result == 701)
				{
					return Noah.ResultState.Mismatching;
				}
				if (result == 600)
				{
					return Noah.ResultState.CommitOver;
				}
				if (result != 800)
				{
					return Noah.ResultState.Failure;
				}
				return Noah.ResultState.Unconnected;
			}
		}

		public override void Initialize()
		{
			this.jo.Call("initialize", new object[0]);
		}

		public override void Close()
		{
			this.jo.Call("close", new object[0]);
		}

		public override void Suspend()
		{
			this.jo.Call("suspend", new object[0]);
		}

		public override void Resume()
		{
			this.jo.Call("resume", new object[0]);
		}

		public override void CloseBanner()
		{
			this.jo.Call("closeBanner", new object[0]);
		}

		public override void Commit(string action_id)
		{
			this.jo.Call("commit", new object[]
			{
				action_id
			});
		}

		public override void Connect(string consumer_key, string secret_key)
		{
			this.jo.Call("connect", new object[]
			{
				consumer_key,
				secret_key
			});
		}

		public override void Connect(string consumer_key, string secret_key, string action_id)
		{
			this.jo.Call("connect", new object[]
			{
				consumer_key,
				secret_key,
				action_id
			});
		}

		public override void Delete()
		{
			this.jo.Call("delete", new object[0]);
		}

		public override void GetBannerView(Noah.BannerSize size, float x, float y)
		{
			this.jo.Call("getBannerView", new object[]
			{
				this.ConvertBannerSize(size),
				(int)x,
				(int)y
			});
		}

		public override void GetBannerView(Noah.BannerSize size, float x, float y, string tag)
		{
			this.jo.Call("getBannerView", new object[]
			{
				this.ConvertBannerSize(size),
				(int)x,
				(int)y,
				tag
			});
		}

		public override bool GetBannerFlag()
		{
			return this.jo.Call<bool>("getBannerFlag", new object[0]);
		}

		public override string GetNoahID()
		{
			if (this.jo.Call<string>("getNoahID", new object[0]) != "null")
			{
				return this.jo.Call<string>("getNoahID", new object[0]);
			}
			return null;
		}

		public override string GetCheckToken()
		{
			return this.jo.Call<string>("getCheckToken", new object[0]);
		}

		public override bool GetOfferFlag()
		{
			return this.jo.Call<bool>("getOfferFlag", new object[0]);
		}

		public override Texture2D GetOfferBitmap(Noah.OfferButtonImage type)
		{
			byte[] array = this.jo.Call<byte[]>("getOfferBitmap", new object[]
			{
				this.ConvertOfferButtonImage(type)
			});
			if (array != null)
			{
				Texture2D texture2D = new Texture2D(64, 32);
				texture2D.LoadImage(array);
				return texture2D;
			}
			return null;
		}

		public override void GetPoint()
		{
			this.jo.Call("getPoint", new object[0]);
		}

		public override void GetPurchased()
		{
			this.jo.Call("getPurchased", new object[0]);
		}

		public override bool GetReviewFlag()
		{
			return this.jo.Call<bool>("getReviewFlag", new object[0]);
		}

		public override bool GetRewardFlag()
		{
			return this.jo.Call<bool>("getRewardFlag", new object[0]);
		}

		public override bool GetBannerWallFlag()
		{
			return this.jo.Call<bool>("getBannerWallFlag", new object[0]);
		}

		public override int GetRewardNum()
		{
			return this.jo.Call<int>("getRewardNum", new object[0]);
		}

		public override void SetRewardEffect(Noah.RewardEffect effect)
		{
			this.jo.Call("setRewardEffect", new object[]
			{
				this.ConvertRewardEffect(effect)
			});
		}

		public override void GetRewardView(float x, float y)
		{
			this.jo.Call("getRewardView", new object[]
			{
				(int)x,
				(int)y
			});
		}

		public override bool GetShopFlag()
		{
			return this.jo.Call<bool>("getShopFlag", new object[0]);
		}

		public override string GetVersion()
		{
			return this.noahJavaClass.GetStatic<string>("VERSION");
		}

		public override void Offer(string guid, Noah.Orientation orientation)
		{
			this.jo.Call("offer", new object[]
			{
				guid,
				this.ConvertOrientation(orientation)
			});
		}

		public override void Offer(string guid, Noah.Orientation orientation, string tag)
		{
			this.jo.Call("offer", new object[]
			{
				guid,
				this.ConvertOrientation(orientation),
				tag
			});
		}

		public override void Review()
		{
			this.jo.Call("review", new object[0]);
		}

		public override void Review(string tag)
		{
			this.jo.Call("review", new object[]
			{
				tag
			});
		}

		public override void SetBannerEffect(Noah.BannerEffect effect)
		{
			this.jo.Call("setBannerEffect", new object[]
			{
				this.ConvertBannerEffect(effect)
			});
		}

		public override void SetDebugMode(bool flag)
		{
			this.jo.Call("setDebugMode", new object[]
			{
				flag
			});
		}

		public override bool GetDebugMode()
		{
			return this.jo.Call<bool>("getDebugMode", new object[0]);
		}

		public override void SetGUID(string guid)
		{
			this.jo.Call("setGUID", new object[]
			{
				guid
			});
		}

		public override void Shop(string guid, Noah.Orientation orientation)
		{
			this.jo.Call("shop", new object[]
			{
				guid,
				this.ConvertOrientation(orientation)
			});
		}

		public override void Shop(string guid, Noah.Orientation orientation, string tag)
		{
			this.jo.Call("shop", new object[]
			{
				guid,
				this.ConvertOrientation(orientation),
				tag
			});
		}

		public override void UsePoint(int use_point)
		{
			this.jo.Call("usePoint", new object[]
			{
				use_point
			});
		}

		public override bool HasNewReward()
		{
			return this.jo.Call<bool>("hasNewOffer", new object[0]);
		}

		public override bool HasNewOffer()
		{
			return this.jo.Call<bool>("hasNewOffer", new object[0]);
		}

		public override Texture2D GetNewBadge(Noah.BadgeType type)
		{
			byte[] array = this.jo.Call<byte[]>("getNewBadge", new object[]
			{
				this.ConvertBadgeType(type)
			});
			if (array != null)
			{
				Texture2D texture2D = new Texture2D(64, 32);
				texture2D.LoadImage(array);
				return texture2D;
			}
			return null;
		}

		public static void Toast(string message)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject androidJavaObject = new AndroidJavaObject("jp.co.sega.noah.unity.NoahUnityPlugin", new object[]
				{
					@static
				});
				androidJavaObject.Call("toast", new object[]
				{
					message
				});
			}
		}

		public override void BannerWall(Noah.Orientation orientation)
		{
			this.jo.Call("bannerWall", new object[]
			{
				this.ConvertOrientation(orientation)
			});
		}

		public override void BannerWall(Noah.Orientation orientation, bool isRotatable)
		{
			this.jo.Call("bannerWall", new object[]
			{
				this.ConvertOrientation(orientation),
				isRotatable
			});
		}

		public override void BannerWall(Noah.Orientation orientation, bool isRotatable, string tag)
		{
			this.jo.Call("bannerWall", new object[]
			{
				this.ConvertOrientation(orientation),
				isRotatable,
				tag
			});
		}

		public override Vector2 GetBannerSize(Noah.BannerSize size)
		{
			switch (size)
			{
			case Noah.BannerSize.Size224x336:
				return new Vector2((float)Screen.width, (float)Screen.height);
			case Noah.BannerSize.Size320x50:
			case Noah.BannerSize.SizeNormal:
			case Noah.BannerSize.SizeStandard:
			case Noah.BannerSize.SizeStandardFillParentWidth:
				return new Vector2(320f, 50f) * DisplayMetricsAndroid.Density;
			case Noah.BannerSize.Size336x224:
				return new Vector2((float)Screen.width, (float)Screen.height);
			case Noah.BannerSize.Size480x32:
			case Noah.BannerSize.SizeWide:
			case Noah.BannerSize.SizeWideFillParentWidth:
				return new Vector2(480f, 32f) * DisplayMetricsAndroid.Density;
			case Noah.BannerSize.SizeNormal2x:
			case Noah.BannerSize.Size640x100:
			case Noah.BannerSize.SizeStandard2x:
			case Noah.BannerSize.SizeStandard2xFillParentWidth:
				return new Vector2(640f, 100f) * DisplayMetricsAndroid.Density;
			case Noah.BannerSize.Size448x672:
				return new Vector2((float)Screen.width, (float)Screen.height);
			case Noah.BannerSize.Size672x448:
				return new Vector2((float)Screen.width, (float)Screen.height);
			case Noah.BannerSize.Size960x64:
			case Noah.BannerSize.SizeWide2x:
			case Noah.BannerSize.SizeWide2xFillParentWidth:
				return new Vector2(960f, 64f) * DisplayMetricsAndroid.Density;
			default:
				return Vector2.zero;
			}
		}

		public override void ShowBannerView(Noah.BannerSize size, float x, float y)
		{
			this.jo.Call("getBannerView", new object[]
			{
				this.ConvertBannerSize(size),
				(int)x,
				(int)y
			});
		}

		public override void ShowBannerView(Noah.BannerSize size, float x, float y, string tag)
		{
			this.jo.Call("getBannerView", new object[]
			{
				this.ConvertBannerSize(size),
				(int)x,
				(int)y,
				tag
			});
		}

		public override Vector2 GetRewardSize()
		{
			return new Vector2(240f, 48f) * DisplayMetricsAndroid.Density;
		}

		public override void ShowRewardView(float x, float y)
		{
			this.jo.Call("getRewardView", new object[]
			{
				(int)x,
				(int)y
			});
		}

		public override ArrayList GetAlertMessage()
		{
			string jsonString = this.jo.Call<string>("getAlertMessage", new object[0]);
			return (ArrayList)NewJSON.JsonDecode(jsonString);
		}

		public override bool IsConnecting()
		{
			return this.jo.Call<bool>("isConnect", new object[0]);
		}

		public override int GetOfferDisplayType()
		{
			return this.jo.Call<int>("getOfferDisplayType", new object[0]);
		}

		public override string GetLastErrorMessage()
		{
			return this.jo.Call<string>("getLastErrorMessage", new object[0]);
		}
	}
}
