using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class GameHelperManager
	{
		internal enum ConnectionState
		{
			Disconnected,
			Connecting,
			Connected
		}

		private class GameHelperListener : AndroidJavaProxy
		{
			private GameHelperManager mContainer;

			private int mOrigin;

			internal GameHelperListener(GameHelperManager mgr, int origin) : base("com.google.example.games.basegameutils.GameHelper$GameHelperListener")
			{
				this.mContainer = mgr;
				this.mOrigin = origin;
			}

			private void onSignInFailed()
			{
				Logger.d("GHM/GameHelperListener got onSignInFailed, origin " + this.mOrigin + ", notifying GHM.");
				this.mContainer.OnSignInFailed(this.mOrigin);
			}

			private void onSignInSucceeded()
			{
				Logger.d("GHM/GameHelperListener got onSignInSucceeded, origin " + this.mOrigin + ", notifying GHM.");
				this.mContainer.OnSignInSucceeded(this.mOrigin);
			}
		}

		public delegate void OnStopDelegate();

		private const string SignInHelperManagerClass = "com.google.example.games.pluginsupport.SignInHelperManager";

		private const string BaseGameUtilsPkg = "com.google.example.games.basegameutils";

		private const string GameHelperClass = "com.google.example.games.basegameutils.GameHelper";

		private const string GameHelperListenerClass = "com.google.example.games.basegameutils.GameHelper$GameHelperListener";

		private const int ORIGIN_MAIN_ACTIVITY = 1000;

		private const int ORIGIN_SIGN_IN_HELPER_ACTIVITY = 1001;

		private AndroidJavaObject mGameHelper;

		private AndroidClient mAndroidClient;

		private bool mPaused;

		private List<GameHelperManager.OnStopDelegate> mOnStopDelegates = new List<GameHelperManager.OnStopDelegate>();

		internal GameHelperManager.ConnectionState State
		{
			get
			{
				if (this.mGameHelper.Call<bool>("isSignedIn", new object[0]))
				{
					return GameHelperManager.ConnectionState.Connected;
				}
				if (this.mGameHelper.Call<bool>("isConnecting", new object[0]))
				{
					return GameHelperManager.ConnectionState.Connecting;
				}
				return GameHelperManager.ConnectionState.Disconnected;
			}
		}

		internal bool Connecting
		{
			get
			{
				return this.mGameHelper.Call<bool>("isConnecting", new object[0]);
			}
		}

		public bool Paused
		{
			get
			{
				return this.mPaused;
			}
		}

		internal GameHelperManager(AndroidClient client)
		{
			this.mAndroidClient = client;
			Logger.d("Setting up GameHelperManager.");
			Logger.d("GHM creating GameHelper.");
			int num = 7;
			Logger.d("GHM calling GameHelper constructor with flags=" + num);
			this.mGameHelper = new AndroidJavaObject("com.google.example.games.basegameutils.GameHelper", new object[]
			{
				this.mAndroidClient.GetActivity(),
				num
			});
			if (this.mGameHelper == null)
			{
				throw new Exception("Failed to create GameHelper.");
			}
			Logger.d("GHM setting up GameHelper.");
			this.mGameHelper.Call("enableDebugLog", new object[]
			{
				Logger.DebugLogEnabled
			});
			GameHelperManager.GameHelperListener gameHelperListener = new GameHelperManager.GameHelperListener(this, 1000);
			Logger.d("GHM Setting GameHelper options.");
			this.mGameHelper.Call("setMaxAutoSignInAttempts", new object[]
			{
				0
			});
			AndroidJavaClass gmsClass = JavaUtil.GetGmsClass("games.Games$GamesOptions");
			AndroidJavaObject androidJavaObject = gmsClass.CallStatic<AndroidJavaObject>("builder", new object[0]);
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("setSdkVariant", new object[]
			{
				37143
			});
			AndroidJavaObject androidJavaObject3 = androidJavaObject.Call<AndroidJavaObject>("build", new object[0]);
			this.mGameHelper.Call("setGamesApiOptions", new object[]
			{
				androidJavaObject3
			});
			androidJavaObject3.Dispose();
			androidJavaObject2.Dispose();
			androidJavaObject.Dispose();
			Logger.d("GHM calling GameHelper.setup");
			this.mGameHelper.Call("setup", new object[]
			{
				gameHelperListener
			});
			Logger.d("GHM: GameHelper setup done.");
			Logger.d("GHM Setting up lifecycle.");
			PlayGamesHelperObject.SetPauseCallback(delegate(bool paused)
			{
				if (paused)
				{
					this.OnPause();
				}
				else
				{
					this.OnResume();
				}
			});
			Logger.d("GHM calling GameHelper.onStart to try initial auth.");
			this.mGameHelper.Call("onStart", new object[]
			{
				this.mAndroidClient.GetActivity()
			});
		}

		private void OnResume()
		{
			this.mPaused = false;
			Logger.d("GHM got OnResume, relaying to GameHelper");
			this.mGameHelper.Call("onStart", new object[]
			{
				this.mAndroidClient.GetActivity()
			});
		}

		private void OnPause()
		{
			Logger.d("GHM got OnPause, relaying to GameHelper");
			this.mPaused = true;
			foreach (GameHelperManager.OnStopDelegate current in this.mOnStopDelegates)
			{
				current();
			}
			this.mGameHelper.Call("onStop", new object[0]);
		}

		private void OnSignInFailed(int origin)
		{
			Logger.d("GHM got onSignInFailed, origin " + origin + ", notifying AndroidClient.");
			if (origin == 1001)
			{
				Logger.d("GHM got onSignInFailed from Sign In Helper. Showing error message.");
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.example.games.pluginsupport.SignInHelperManager"))
				{
					androidJavaClass.CallStatic("showErrorDialog", new object[]
					{
						this.mAndroidClient.GetActivity()
					});
				}
				Logger.d("Error message shown.");
			}
			this.mAndroidClient.OnSignInFailed();
		}

		private void OnSignInSucceeded(int origin)
		{
			Logger.d("GHM got onSignInSucceeded, origin " + origin + ", notifying AndroidClient.");
			if (origin == 1000)
			{
				this.mAndroidClient.OnSignInSucceeded();
			}
			else if (origin == 1001)
			{
				Logger.d("GHM got helper's OnSignInSucceeded.");
			}
		}

		internal void BeginUserInitiatedSignIn()
		{
			Logger.d("GHM Starting user-initiated sign in.");
			Logger.d("Forcing GameHelper's connect-on-start flag to true.");
			this.mGameHelper.Call("setConnectOnStart", new object[]
			{
				true
			});
			Logger.d("GHM launching sign-in Activity via SignInHelperManager.launchSignIn");
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.example.games.pluginsupport.SignInHelperManager");
			androidJavaClass.CallStatic("launchSignIn", new object[]
			{
				this.mAndroidClient.GetActivity(),
				new GameHelperManager.GameHelperListener(this, 1001),
				Logger.DebugLogEnabled
			});
		}

		public AndroidJavaObject GetApiClient()
		{
			return this.mGameHelper.Call<AndroidJavaObject>("getApiClient", new object[0]);
		}

		public AndroidJavaObject GetInvitation()
		{
			bool flag = this.mGameHelper.Call<bool>("hasInvitation", new object[0]);
			if (flag)
			{
				return this.mGameHelper.Call<AndroidJavaObject>("getInvitation", new object[0]);
			}
			return null;
		}

		public AndroidJavaObject GetTurnBasedMatch()
		{
			bool flag = this.mGameHelper.Call<bool>("hasTurnBasedMatch", new object[0]);
			if (flag)
			{
				return this.mGameHelper.Call<AndroidJavaObject>("getTurnBasedMatch", new object[0]);
			}
			return null;
		}

		public void ClearInvitationAndTurnBasedMatch()
		{
			Logger.d("GHM clearing invitation and turn-based match on GameHelper.");
			this.mGameHelper.Call("clearInvitation", new object[0]);
			this.mGameHelper.Call("clearTurnBasedMatch", new object[0]);
		}

		public bool IsConnected()
		{
			return this.mGameHelper.Call<bool>("isSignedIn", new object[0]);
		}

		public void SignOut()
		{
			Logger.d("GHM SignOut");
			this.mGameHelper.Call("signOut", new object[0]);
		}

		public void AddOnStopDelegate(GameHelperManager.OnStopDelegate del)
		{
			this.mOnStopDelegates.Add(del);
		}

		private object[] makeGmsCallArgs(object[] args)
		{
			object[] array = new object[args.Length + 1];
			array[0] = this.GetApiClient();
			for (int i = 1; i < array.Length; i++)
			{
				array[i] = args[i - 1];
			}
			return array;
		}

		public ReturnType CallGmsApi<ReturnType>(string className, string fieldName, string methodName, params object[] args)
		{
			object[] args2 = this.makeGmsCallArgs(args);
			if (fieldName != null)
			{
				return JavaUtil.GetGmsField(className, fieldName).Call<ReturnType>(methodName, args2);
			}
			return JavaUtil.GetGmsClass(className).CallStatic<ReturnType>(methodName, args2);
		}

		public void CallGmsApi(string className, string fieldName, string methodName, params object[] args)
		{
			object[] args2 = this.makeGmsCallArgs(args);
			if (fieldName != null)
			{
				JavaUtil.GetGmsField(className, fieldName).Call(methodName, args2);
			}
			else
			{
				JavaUtil.GetGmsClass(className).CallStatic(methodName, args2);
			}
		}

		public void CallGmsApiWithResult(string className, string fieldName, string methodName, AndroidJavaProxy callbackProxy, params object[] args)
		{
			using (AndroidJavaObject androidJavaObject = this.CallGmsApi<AndroidJavaObject>(className, fieldName, methodName, args))
			{
				androidJavaObject.Call("setResultCallback", new object[]
				{
					callbackProxy
				});
			}
		}
	}
}
