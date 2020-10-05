using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GooglePlayGames.Android
{
	public class AndroidClient : IPlayGamesClient
	{
		private enum AuthState
		{
			NoAuth,
			AuthPending,
			InProgress,
			LoadingAchs,
			Done
		}

		private class OnAchievementsLoadedResultProxy : AndroidJavaProxy
		{
			private AndroidClient mOwner;

			internal OnAchievementsLoadedResultProxy(AndroidClient c) : base("com.google.android.gms.common.api.ResultCallback")
			{
				this.mOwner = c;
			}

			public void onResult(AndroidJavaObject result)
			{
				Logger.d("OnAchievementsLoadedResultProxy invoked");
				Logger.d("    result=" + result);
				int statusCode = JavaUtil.GetStatusCode(result);
				AndroidJavaObject androidJavaObject = JavaUtil.CallNullSafeObjectMethod(result, "getAchievements", new object[0]);
				this.mOwner.OnAchievementsLoaded(statusCode, androidJavaObject);
				if (androidJavaObject != null)
				{
					androidJavaObject.Dispose();
				}
			}
		}

		private class OnInvitationReceivedProxy : AndroidJavaProxy
		{
			private AndroidClient mOwner;

			internal OnInvitationReceivedProxy(AndroidClient owner) : base("com.google.android.gms.games.multiplayer.OnInvitationReceivedListener")
			{
				this.mOwner = owner;
			}

			public void onInvitationReceived(AndroidJavaObject invitationObj)
			{
				Logger.d("OnInvitationReceivedProxy.onInvitationReceived");
				this.mOwner.OnInvitationReceived(invitationObj);
			}

			public void onInvitationRemoved(string invitationId)
			{
				Logger.d("OnInvitationReceivedProxy.onInvitationRemoved");
				this.mOwner.OnInvitationRemoved(invitationId);
			}
		}

		private sealed class _Authenticate_c__AnonStoreyCF
		{
			internal Action<bool> callback;

			internal AndroidClient __f__this;

			internal void __m__1()
			{
				GameHelperManager.ConnectionState state = this.__f__this.mGHManager.State;
				if (state != GameHelperManager.ConnectionState.Connecting)
				{
					if (state != GameHelperManager.ConnectionState.Connected)
					{
						this.__f__this.mAuthCallback = this.callback;
						if (this.__f__this.mSilentAuth)
						{
							Logger.d("AUTH: not connected and silent=true, so failing.");
							this.__f__this.mAuthState = AndroidClient.AuthState.NoAuth;
							this.__f__this.InvokeAuthCallback(false);
						}
						else
						{
							Logger.d("AUTH: not connected and silent=false, so starting flow.");
							this.__f__this.mAuthState = AndroidClient.AuthState.InProgress;
							this.__f__this.mGHManager.BeginUserInitiatedSignIn();
						}
					}
					else
					{
						Logger.d("AUTH: already connected! Proceeding to achievement load phase.");
						this.__f__this.mAuthCallback = this.callback;
						this.__f__this.DoInitialAchievementLoad();
					}
				}
				else
				{
					Logger.d("AUTH: connection in progress; auth now pending.");
					this.__f__this.mAuthCallback = this.callback;
					this.__f__this.mAuthState = AndroidClient.AuthState.AuthPending;
				}
			}
		}

		private sealed class _InvokeAuthCallback_c__AnonStoreyD0
		{
			internal Action<bool> cb;

			internal bool success;

			internal void __m__2()
			{
				this.cb(this.success);
			}
		}

		private sealed class _RunWhenConnectionStable_c__AnonStoreyD1
		{
			internal Action a;

			internal AndroidClient __f__this;

			internal void __m__4()
			{
				if (this.__f__this.mGHManager.Paused || this.__f__this.mGHManager.Connecting)
				{
					Logger.d("Action scheduled for later (connection currently in progress).");
					this.__f__this.mActionsPendingSignIn.Add(this.a);
				}
				else
				{
					this.a();
				}
			}
		}

		private sealed class _CallClientApi_c__AnonStoreyD2
		{
			internal string desc;

			internal Action call;

			internal Action<bool> callback;

			internal AndroidClient __f__this;

			internal void __m__5()
			{
				if (this.__f__this.mGHManager.IsConnected())
				{
					Logger.d("Connected! Calling API: " + this.desc);
					this.call();
					if (this.callback != null)
					{
						PlayGamesHelperObject.RunOnGameThread(delegate
						{
							this.callback(true);
						});
					}
				}
				else
				{
					Logger.w("Not connected! Failed to call API :" + this.desc);
					if (this.callback != null)
					{
						PlayGamesHelperObject.RunOnGameThread(delegate
						{
							this.callback(false);
						});
					}
				}
			}

			internal void __m__13()
			{
				this.callback(true);
			}

			internal void __m__14()
			{
				this.callback(false);
			}
		}

		private sealed class _UnlockAchievement_c__AnonStoreyD3
		{
			internal string achId;

			internal AndroidClient __f__this;

			internal void __m__6()
			{
				this.__f__this.mGHManager.CallGmsApi("games.Games", "Achievements", "unlock", new object[]
				{
					this.achId
				});
			}
		}

		private sealed class _RevealAchievement_c__AnonStoreyD4
		{
			internal string achId;

			internal AndroidClient __f__this;

			internal void __m__7()
			{
				this.__f__this.mGHManager.CallGmsApi("games.Games", "Achievements", "reveal", new object[]
				{
					this.achId
				});
			}
		}

		private sealed class _IncrementAchievement_c__AnonStoreyD5
		{
			internal string achId;

			internal int steps;

			internal AndroidClient __f__this;

			internal void __m__8()
			{
				this.__f__this.mGHManager.CallGmsApi("games.Games", "Achievements", "increment", new object[]
				{
					this.achId,
					this.steps
				});
			}
		}

		private sealed class _ShowLeaderboardUI_c__AnonStoreyD6
		{
			internal string lbId;

			internal AndroidClient __f__this;

			internal void __m__A()
			{
				using (AndroidJavaObject leaderboardIntent = this.__f__this.GetLeaderboardIntent(this.lbId))
				{
					using (AndroidJavaObject activity = this.__f__this.GetActivity())
					{
						Logger.d(string.Concat(new object[]
						{
							"About to show LB UI with intent ",
							leaderboardIntent,
							", activity ",
							activity
						}));
						if (leaderboardIntent != null && activity != null)
						{
							activity.Call("startActivityForResult", new object[]
							{
								leaderboardIntent,
								9999
							});
						}
					}
				}
			}
		}

		private sealed class _SubmitScore_c__AnonStoreyD7
		{
			internal string lbId;

			internal long score;

			internal AndroidClient __f__this;

			internal void __m__B()
			{
				this.__f__this.mGHManager.CallGmsApi("games.Games", "Leaderboards", "submitScore", new object[]
				{
					this.lbId,
					this.score
				});
			}
		}

		private sealed class _LoadState_c__AnonStoreyD8
		{
			internal OnStateLoadedListener listener;

			internal int slot;

			internal AndroidClient __f__this;

			internal void __m__C()
			{
				OnStateResultProxy callbackProxy = new OnStateResultProxy(this.__f__this, this.listener);
				this.__f__this.mGHManager.CallGmsApiWithResult("appstate.AppStateManager", null, "load", callbackProxy, new object[]
				{
					this.slot
				});
			}
		}

		private sealed class _ResolveState_c__AnonStoreyD9
		{
			internal OnStateLoadedListener listener;

			internal int slot;

			internal string resolvedVersion;

			internal byte[] resolvedData;

			internal AndroidClient __f__this;

			internal void __m__D()
			{
				this.__f__this.mGHManager.CallGmsApiWithResult("appstate.AppStateManager", null, "resolve", new OnStateResultProxy(this.__f__this, this.listener), new object[]
				{
					this.slot,
					this.resolvedVersion,
					this.resolvedData
				});
			}
		}

		private sealed class _UpdateState_c__AnonStoreyDA
		{
			internal int slot;

			internal byte[] data;

			internal AndroidClient __f__this;

			internal void __m__E()
			{
				this.__f__this.mGHManager.CallGmsApi("appstate.AppStateManager", null, "update", new object[]
				{
					this.slot,
					this.data
				});
			}
		}

		private sealed class _RegisterInvitationDelegate_c__AnonStoreyDB
		{
			internal Invitation inv;

			internal AndroidClient __f__this;

			internal void __m__F()
			{
				if (this.__f__this.mInvitationDelegate != null)
				{
					this.__f__this.mInvitationDelegate(this.inv, true);
				}
			}
		}

		private sealed class _CheckForConnectionExtras_c__AnonStoreyDC
		{
			internal Invitation invFromNotif;

			internal AndroidClient __f__this;

			internal void __m__11()
			{
				if (this.__f__this.mInvitationDelegate != null)
				{
					this.__f__this.mInvitationDelegate(this.invFromNotif, true);
				}
			}
		}

		private sealed class _OnInvitationReceived_c__AnonStoreyDD
		{
			internal Invitation inv;

			internal AndroidClient __f__this;

			internal void __m__12()
			{
				if (this.__f__this.mInvitationDelegate != null)
				{
					this.__f__this.mInvitationDelegate(this.inv, false);
				}
			}
		}

		private const int RC_UNUSED = 9999;

		private GameHelperManager mGHManager;

		private AndroidClient.AuthState mAuthState;

		private bool mSilentAuth;

		private string mUserId;

		private string mUserDisplayName;

		private Action<bool> mAuthCallback;

		private AchievementBank mAchievementBank = new AchievementBank();

		private List<Action> mActionsPendingSignIn = new List<Action>();

		private bool mSignOutInProgress;

		private AndroidRtmpClient mRtmpClient;

		private AndroidTbmpClient mTbmpClient;

		private InvitationReceivedDelegate mInvitationDelegate;

		private bool mRegisteredInvitationListener;

		private Invitation mInvitationFromNotification;

		internal GameHelperManager GHManager
		{
			get
			{
				return this.mGHManager;
			}
		}

		public string PlayerId
		{
			get
			{
				return this.mUserId;
			}
		}

		public AndroidClient()
		{
			this.mRtmpClient = new AndroidRtmpClient(this);
			this.mTbmpClient = new AndroidTbmpClient(this);
			this.RunOnUiThread(delegate
			{
				Logger.d("Initializing Android Client.");
				Logger.d("Creating GameHelperManager to manage GameHelper.");
				this.mGHManager = new GameHelperManager(this);
				this.mGHManager.AddOnStopDelegate(new GameHelperManager.OnStopDelegate(this.mRtmpClient.OnStop));
			});
		}

		public void Authenticate(Action<bool> callback, bool silent)
		{
			if (this.mAuthState != AndroidClient.AuthState.NoAuth)
			{
				Logger.w("Authenticate() called while an authentication process was active. " + this.mAuthState);
				this.mAuthCallback = callback;
				return;
			}
			Logger.d("Making sure PlayGamesHelperObject is ready.");
			PlayGamesHelperObject.CreateObject();
			Logger.d("PlayGamesHelperObject created.");
			this.mSilentAuth = silent;
			Logger.d("AUTH: starting auth process, silent=" + this.mSilentAuth);
			this.RunOnUiThread(delegate
			{
				GameHelperManager.ConnectionState state = this.mGHManager.State;
				if (state != GameHelperManager.ConnectionState.Connecting)
				{
					if (state != GameHelperManager.ConnectionState.Connected)
					{
						this.mAuthCallback = callback;
						if (this.mSilentAuth)
						{
							Logger.d("AUTH: not connected and silent=true, so failing.");
							this.mAuthState = AndroidClient.AuthState.NoAuth;
							this.InvokeAuthCallback(false);
						}
						else
						{
							Logger.d("AUTH: not connected and silent=false, so starting flow.");
							this.mAuthState = AndroidClient.AuthState.InProgress;
							this.mGHManager.BeginUserInitiatedSignIn();
						}
					}
					else
					{
						Logger.d("AUTH: already connected! Proceeding to achievement load phase.");
						this.mAuthCallback = callback;
						this.DoInitialAchievementLoad();
					}
				}
				else
				{
					Logger.d("AUTH: connection in progress; auth now pending.");
					this.mAuthCallback = callback;
					this.mAuthState = AndroidClient.AuthState.AuthPending;
				}
			});
		}

		private void DoInitialAchievementLoad()
		{
			Logger.d("AUTH: Now performing initial achievement load...");
			this.mAuthState = AndroidClient.AuthState.LoadingAchs;
			this.mGHManager.CallGmsApiWithResult("games.Games", "Achievements", "load", new AndroidClient.OnAchievementsLoadedResultProxy(this), new object[]
			{
				false
			});
			Logger.d("AUTH: Initial achievement load call made.");
		}

		private void OnAchievementsLoaded(int statusCode, AndroidJavaObject buffer)
		{
			if (this.mAuthState == AndroidClient.AuthState.LoadingAchs)
			{
				Logger.d("AUTH: Initial achievement load finished.");
				if (statusCode == 0 || statusCode == 3 || statusCode == 5)
				{
					Logger.d("Processing achievement buffer.");
					this.mAchievementBank.ProcessBuffer(buffer);
					Logger.d("Closing achievement buffer.");
					buffer.Call("close", new object[0]);
					Logger.d("AUTH: Auth process complete!");
					this.mAuthState = AndroidClient.AuthState.Done;
					this.InvokeAuthCallback(true);
					this.CheckForConnectionExtras();
					this.mRtmpClient.OnSignInSucceeded();
					this.mTbmpClient.OnSignInSucceeded();
				}
				else
				{
					Logger.w("AUTH: Failed to load achievements, status code " + statusCode);
					this.mAuthState = AndroidClient.AuthState.NoAuth;
					this.InvokeAuthCallback(false);
				}
			}
			else
			{
				Logger.w("OnAchievementsLoaded called unexpectedly in auth state " + this.mAuthState);
			}
		}

		private void InvokeAuthCallback(bool success)
		{
			if (this.mAuthCallback == null)
			{
				return;
			}
			Logger.d("AUTH: Calling auth callback: success=" + success);
			Action<bool> cb = this.mAuthCallback;
			this.mAuthCallback = null;
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				cb(success);
			});
		}

		private void RetrieveUserInfo()
		{
			Logger.d("Attempting to retrieve player info.");
			using (AndroidJavaObject androidJavaObject = this.mGHManager.CallGmsApi<AndroidJavaObject>("games.Games", "Players", "getCurrentPlayer", new object[0]))
			{
				if (this.mUserId == null)
				{
					this.mUserId = androidJavaObject.Call<string>("getPlayerId", new object[0]);
					Logger.d("Player ID: " + this.mUserId);
				}
				if (this.mUserDisplayName == null)
				{
					this.mUserDisplayName = androidJavaObject.Call<string>("getDisplayName", new object[0]);
					Logger.d("Player display name: " + this.mUserDisplayName);
				}
			}
		}

		internal void OnSignInSucceeded()
		{
			Logger.d("AndroidClient got OnSignInSucceeded.");
			this.RetrieveUserInfo();
			if (this.mAuthState == AndroidClient.AuthState.AuthPending || this.mAuthState == AndroidClient.AuthState.InProgress)
			{
				Logger.d("AUTH: Auth succeeded. Proceeding to achievement loading.");
				this.DoInitialAchievementLoad();
			}
			else if (this.mAuthState == AndroidClient.AuthState.LoadingAchs)
			{
				Logger.w("AUTH: Got OnSignInSucceeded() while in achievement loading phase (unexpected).");
				Logger.w("AUTH: Trying to fix by issuing a new achievement load call.");
				this.DoInitialAchievementLoad();
			}
			else
			{
				Logger.d("Normal lifecycle OnSignInSucceeded received.");
				this.RunPendingActions();
				this.CheckForConnectionExtras();
				this.mRtmpClient.OnSignInSucceeded();
				this.mTbmpClient.OnSignInSucceeded();
			}
		}

		internal void OnSignInFailed()
		{
			Logger.d("AndroidClient got OnSignInFailed.");
			if (this.mAuthState == AndroidClient.AuthState.AuthPending)
			{
				if (this.mSilentAuth)
				{
					Logger.d("AUTH: Auth flow was pending, but silent=true, so failing.");
					this.mAuthState = AndroidClient.AuthState.NoAuth;
					this.InvokeAuthCallback(false);
				}
				else
				{
					Logger.d("AUTH: Auth flow was pending and silent=false, so doing noisy auth.");
					this.mAuthState = AndroidClient.AuthState.InProgress;
					this.mGHManager.BeginUserInitiatedSignIn();
				}
			}
			else if (this.mAuthState == AndroidClient.AuthState.InProgress)
			{
				Logger.d("AUTH: FAILED!");
				this.mAuthState = AndroidClient.AuthState.NoAuth;
				this.InvokeAuthCallback(false);
			}
			else if (this.mAuthState == AndroidClient.AuthState.LoadingAchs)
			{
				Logger.d("AUTH: FAILED (while loading achievements).");
				this.mAuthState = AndroidClient.AuthState.NoAuth;
				this.InvokeAuthCallback(false);
			}
			else if (this.mAuthState == AndroidClient.AuthState.NoAuth)
			{
				Logger.d("Normal OnSignInFailed received.");
			}
			else if (this.mAuthState == AndroidClient.AuthState.Done)
			{
				Logger.e("Authentication has been lost!");
				this.mAuthState = AndroidClient.AuthState.NoAuth;
			}
		}

		private void RunPendingActions()
		{
			if (this.mActionsPendingSignIn.Count > 0)
			{
				Logger.d("Running pending actions on the UI thread.");
				while (this.mActionsPendingSignIn.Count > 0)
				{
					Action action = this.mActionsPendingSignIn[0];
					this.mActionsPendingSignIn.RemoveAt(0);
					action();
				}
				Logger.d("Done running pending actions on the UI thread.");
			}
			else
			{
				Logger.d("No pending actions to run on UI thread.");
			}
		}

		public bool IsAuthenticated()
		{
			return this.mAuthState == AndroidClient.AuthState.Done && !this.mSignOutInProgress;
		}

		public void SignOut()
		{
			Logger.d("AndroidClient.SignOut");
			this.mSignOutInProgress = true;
			this.RunWhenConnectionStable(delegate
			{
				Logger.d("Calling GHM.SignOut");
				this.mGHManager.SignOut();
				this.mAuthState = AndroidClient.AuthState.NoAuth;
				this.mSignOutInProgress = false;
				Logger.d("Now signed out.");
			});
		}

		internal AndroidJavaObject GetActivity()
		{
			AndroidJavaObject @static;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				@static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			return @static;
		}

		internal void RunOnUiThread(Action action)
		{
			using (AndroidJavaObject activity = this.GetActivity())
			{
				activity.Call("runOnUiThread", new object[]
				{
					new AndroidJavaRunnable(action.Invoke)
				});
			}
		}

		private void RunWhenConnectionStable(Action a)
		{
			this.RunOnUiThread(delegate
			{
				if (this.mGHManager.Paused || this.mGHManager.Connecting)
				{
					Logger.d("Action scheduled for later (connection currently in progress).");
					this.mActionsPendingSignIn.Add(a);
				}
				else
				{
					a();
				}
			});
		}

		internal void CallClientApi(string desc, Action call, Action<bool> callback)
		{
			Logger.d("Requesting API call: " + desc);
			this.RunWhenConnectionStable(delegate
			{
				if (this.mGHManager.IsConnected())
				{
					Logger.d("Connected! Calling API: " + desc);
					call();
					if (callback != null)
					{
						PlayGamesHelperObject.RunOnGameThread(delegate
						{
							callback(true);
						});
					}
				}
				else
				{
					Logger.w("Not connected! Failed to call API :" + desc);
					if (callback != null)
					{
						PlayGamesHelperObject.RunOnGameThread(delegate
						{
							callback(false);
						});
					}
				}
			});
		}

		public string GetUserId()
		{
			return this.mUserId;
		}

		public string GetUserDisplayName()
		{
			return this.mUserDisplayName;
		}

		public void UnlockAchievement(string achId, Action<bool> callback)
		{
			Logger.d("AndroidClient.UnlockAchievement: " + achId);
			Achievement achievement = this.GetAchievement(achId);
			if (achievement != null && achievement.IsUnlocked)
			{
				Logger.d("...was already unlocked, so no-op.");
				if (callback != null)
				{
					callback(true);
				}
				return;
			}
			this.CallClientApi("unlock ach " + achId, delegate
			{
				this.mGHManager.CallGmsApi("games.Games", "Achievements", "unlock", new object[]
				{
					achId
				});
			}, callback);
			achievement = this.GetAchievement(achId);
			if (achievement != null)
			{
				achievement.IsUnlocked = (achievement.IsRevealed = true);
			}
		}

		public void RevealAchievement(string achId, Action<bool> callback)
		{
			Logger.d("AndroidClient.RevealAchievement: " + achId);
			Achievement achievement = this.GetAchievement(achId);
			if (achievement != null && achievement.IsRevealed)
			{
				Logger.d("...was already revealed, so no-op.");
				if (callback != null)
				{
					callback(true);
				}
				return;
			}
			this.CallClientApi("reveal ach " + achId, delegate
			{
				this.mGHManager.CallGmsApi("games.Games", "Achievements", "reveal", new object[]
				{
					achId
				});
			}, callback);
			achievement = this.GetAchievement(achId);
			if (achievement != null)
			{
				achievement.IsRevealed = true;
			}
		}

		public void IncrementAchievement(string achId, int steps, Action<bool> callback)
		{
			Logger.d(string.Concat(new object[]
			{
				"AndroidClient.IncrementAchievement: ",
				achId,
				", steps ",
				steps
			}));
			this.CallClientApi("increment ach " + achId, delegate
			{
				this.mGHManager.CallGmsApi("games.Games", "Achievements", "increment", new object[]
				{
					achId,
					steps
				});
			}, callback);
			Achievement achievement = this.GetAchievement(achId);
			if (achievement != null)
			{
				achievement.CurrentSteps += steps;
				if (achievement.CurrentSteps >= achievement.TotalSteps)
				{
					achievement.CurrentSteps = achievement.TotalSteps;
				}
			}
		}

		public List<Achievement> GetAchievements()
		{
			return this.mAchievementBank.GetAchievements();
		}

		public Achievement GetAchievement(string achId)
		{
			return this.mAchievementBank.GetAchievement(achId);
		}

		public void ShowAchievementsUI()
		{
			Logger.d("AndroidClient.ShowAchievementsUI.");
			this.CallClientApi("show achievements ui", delegate
			{
				using (AndroidJavaObject androidJavaObject = this.mGHManager.CallGmsApi<AndroidJavaObject>("games.Games", "Achievements", "getAchievementsIntent", new object[0]))
				{
					using (AndroidJavaObject activity = this.GetActivity())
					{
						Logger.d(string.Concat(new object[]
						{
							"About to show achievements UI with intent ",
							androidJavaObject,
							", activity ",
							activity
						}));
						if (androidJavaObject != null && activity != null)
						{
							activity.Call("startActivityForResult", new object[]
							{
								androidJavaObject,
								9999
							});
						}
					}
				}
			}, null);
		}

		private AndroidJavaObject GetLeaderboardIntent(string lbId)
		{
			return (lbId != null) ? this.mGHManager.CallGmsApi<AndroidJavaObject>("games.Games", "Leaderboards", "getLeaderboardIntent", new object[]
			{
				lbId
			}) : this.mGHManager.CallGmsApi<AndroidJavaObject>("games.Games", "Leaderboards", "getAllLeaderboardsIntent", new object[0]);
		}

		public void ShowLeaderboardUI(string lbId)
		{
			Logger.d("AndroidClient.ShowLeaderboardUI, lb=" + ((lbId != null) ? lbId : "(all)"));
			this.CallClientApi("show LB ui", delegate
			{
				using (AndroidJavaObject leaderboardIntent = this.GetLeaderboardIntent(lbId))
				{
					using (AndroidJavaObject activity = this.GetActivity())
					{
						Logger.d(string.Concat(new object[]
						{
							"About to show LB UI with intent ",
							leaderboardIntent,
							", activity ",
							activity
						}));
						if (leaderboardIntent != null && activity != null)
						{
							activity.Call("startActivityForResult", new object[]
							{
								leaderboardIntent,
								9999
							});
						}
					}
				}
			}, null);
		}

		public void SubmitScore(string lbId, long score, Action<bool> callback)
		{
			Logger.d(string.Concat(new object[]
			{
				"AndroidClient.SubmitScore, lb=",
				lbId,
				", score=",
				score
			}));
			this.CallClientApi(string.Concat(new object[]
			{
				"submit score ",
				score,
				", lb ",
				lbId
			}), delegate
			{
				this.mGHManager.CallGmsApi("games.Games", "Leaderboards", "submitScore", new object[]
				{
					lbId,
					score
				});
			}, callback);
		}

		public void LoadState(int slot, OnStateLoadedListener listener)
		{
			Logger.d("AndroidClient.LoadState, slot=" + slot);
			this.CallClientApi("load state slot=" + slot, delegate
			{
				OnStateResultProxy callbackProxy = new OnStateResultProxy(this, listener);
				this.mGHManager.CallGmsApiWithResult("appstate.AppStateManager", null, "load", callbackProxy, new object[]
				{
					slot
				});
			}, null);
		}

		internal void ResolveState(int slot, string resolvedVersion, byte[] resolvedData, OnStateLoadedListener listener)
		{
			Logger.d(string.Format("AndroidClient.ResolveState, slot={0}, ver={1}, data={2}", slot, resolvedVersion, resolvedData));
			this.CallClientApi("resolve state slot=" + slot, delegate
			{
				this.mGHManager.CallGmsApiWithResult("appstate.AppStateManager", null, "resolve", new OnStateResultProxy(this, listener), new object[]
				{
					slot,
					resolvedVersion,
					resolvedData
				});
			}, null);
		}

		public void UpdateState(int slot, byte[] data, OnStateLoadedListener listener)
		{
			Logger.d(string.Format("AndroidClient.UpdateState, slot={0}, data={1}", slot, Logger.describe(data)));
			this.CallClientApi("update state, slot=" + slot, delegate
			{
				this.mGHManager.CallGmsApi("appstate.AppStateManager", null, "update", new object[]
				{
					slot,
					data
				});
			}, null);
			listener.OnStateSaved(true, slot);
		}

		public void SetCloudCacheEncrypter(BufferEncrypter encrypter)
		{
			Logger.d("Ignoring cloud cache encrypter (not used in Android)");
		}

		public void RegisterInvitationDelegate(InvitationReceivedDelegate deleg)
		{
			Logger.d("AndroidClient.RegisterInvitationDelegate");
			if (deleg == null)
			{
				Logger.w("AndroidClient.RegisterInvitationDelegate called w/ null argument.");
				return;
			}
			this.mInvitationDelegate = deleg;
			if (!this.mRegisteredInvitationListener)
			{
				Logger.d("Registering an invitation listener.");
				this.RegisterInvitationListener();
			}
			if (this.mInvitationFromNotification != null)
			{
				Logger.d("Delivering pending invitation from notification now.");
				Invitation inv = this.mInvitationFromNotification;
				this.mInvitationFromNotification = null;
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					if (this.mInvitationDelegate != null)
					{
						this.mInvitationDelegate(inv, true);
					}
				});
			}
		}

		public Invitation GetInvitationFromNotification()
		{
			Logger.d("AndroidClient.GetInvitationFromNotification");
			Logger.d("Returning invitation: " + ((this.mInvitationFromNotification != null) ? this.mInvitationFromNotification.ToString() : "(null)"));
			return this.mInvitationFromNotification;
		}

		public bool HasInvitationFromNotification()
		{
			bool flag = this.mInvitationFromNotification != null;
			Logger.d("AndroidClient.HasInvitationFromNotification, returning " + flag);
			return flag;
		}

		private void RegisterInvitationListener()
		{
			Logger.d("AndroidClient.RegisterInvitationListener");
			this.CallClientApi("register invitation listener", delegate
			{
				this.mGHManager.CallGmsApi("games.Games", "Invitations", "registerInvitationListener", new object[]
				{
					new AndroidClient.OnInvitationReceivedProxy(this)
				});
			}, null);
			this.mRegisteredInvitationListener = true;
		}

		public IRealTimeMultiplayerClient GetRtmpClient()
		{
			return this.mRtmpClient;
		}

		public ITurnBasedMultiplayerClient GetTbmpClient()
		{
			return this.mTbmpClient;
		}

		internal void ClearInvitationIfFromNotification(string invitationId)
		{
			Logger.d("AndroidClient.ClearInvitationIfFromNotification: " + invitationId);
			if (this.mInvitationFromNotification != null && this.mInvitationFromNotification.InvitationId.Equals(invitationId))
			{
				Logger.d("Clearing invitation from notification: " + invitationId);
				this.mInvitationFromNotification = null;
			}
		}

		private void CheckForConnectionExtras()
		{
			Logger.d("AndroidClient: CheckInvitationFromNotification.");
			Logger.d("AndroidClient: looking for invitation in our GameHelper.");
			Invitation invFromNotif = null;
			AndroidJavaObject invitation = this.mGHManager.GetInvitation();
			AndroidJavaObject turnBasedMatch = this.mGHManager.GetTurnBasedMatch();
			this.mGHManager.ClearInvitationAndTurnBasedMatch();
			if (invitation != null)
			{
				Logger.d("Found invitation in GameHelper. Converting.");
				invFromNotif = this.ConvertInvitation(invitation);
				Logger.d("Found invitation in our GameHelper: " + invFromNotif);
			}
			else
			{
				Logger.d("No invitation in our GameHelper. Trying SignInHelperManager.");
				AndroidJavaClass @class = JavaUtil.GetClass("com.google.example.games.pluginsupport.SignInHelperManager");
				using (AndroidJavaObject androidJavaObject = @class.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					if (androidJavaObject.Call<bool>("hasInvitation", new object[0]))
					{
						invFromNotif = this.ConvertInvitation(androidJavaObject.Call<AndroidJavaObject>("getInvitation", new object[0]));
						Logger.d("Found invitation in SignInHelperManager: " + invFromNotif);
						androidJavaObject.Call("forgetInvitation", new object[0]);
					}
					else
					{
						Logger.d("No invitation in SignInHelperManager either.");
					}
				}
			}
			TurnBasedMatch turnBasedMatch2 = null;
			if (turnBasedMatch != null)
			{
				Logger.d("Found match in GameHelper. Converting.");
				turnBasedMatch2 = JavaUtil.ConvertMatch(this.mUserId, turnBasedMatch);
				Logger.d("Match from GameHelper: " + turnBasedMatch2);
			}
			else
			{
				Logger.d("No match in our GameHelper. Trying SignInHelperManager.");
				AndroidJavaClass class2 = JavaUtil.GetClass("com.google.example.games.pluginsupport.SignInHelperManager");
				using (AndroidJavaObject androidJavaObject2 = class2.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					if (androidJavaObject2.Call<bool>("hasTurnBasedMatch", new object[0]))
					{
						turnBasedMatch2 = JavaUtil.ConvertMatch(this.mUserId, androidJavaObject2.Call<AndroidJavaObject>("getTurnBasedMatch", new object[0]));
						Logger.d("Found match in SignInHelperManager: " + turnBasedMatch2);
						androidJavaObject2.Call("forgetTurnBasedMatch", new object[0]);
					}
					else
					{
						Logger.d("No match in SignInHelperManager either.");
					}
				}
			}
			if (invFromNotif != null)
			{
				if (this.mInvitationDelegate != null)
				{
					Logger.d("Invoking invitation received delegate to deal with invitation  from notification.");
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						if (this.mInvitationDelegate != null)
						{
							this.mInvitationDelegate(invFromNotif, true);
						}
					});
				}
				else
				{
					Logger.d("No delegate to handle invitation from notification; queueing.");
					this.mInvitationFromNotification = invFromNotif;
				}
			}
			if (turnBasedMatch2 != null)
			{
				this.mTbmpClient.HandleMatchFromNotification(turnBasedMatch2);
			}
		}

		private Invitation ConvertInvitation(AndroidJavaObject invObj)
		{
			Logger.d("Converting Android invitation to our Invitation object.");
			string invId = invObj.Call<string>("getInvitationId", new object[0]);
			int num = invObj.Call<int>("getInvitationType", new object[0]);
			Participant inviter;
			using (AndroidJavaObject androidJavaObject = invObj.Call<AndroidJavaObject>("getInviter", new object[0]))
			{
				inviter = JavaUtil.ConvertParticipant(androidJavaObject);
			}
			int variant = invObj.Call<int>("getVariant", new object[0]);
			int num2 = num;
			Invitation.InvType invType;
			if (num2 != 0)
			{
				if (num2 != 1)
				{
					Logger.e("Unknown invitation type " + num);
					invType = Invitation.InvType.Unknown;
				}
				else
				{
					invType = Invitation.InvType.TurnBased;
				}
			}
			else
			{
				invType = Invitation.InvType.RealTime;
			}
			Invitation invitation = new Invitation(invType, invId, inviter, variant);
			Logger.d("Converted invitation: " + invitation.ToString());
			return invitation;
		}

		private void OnInvitationReceived(AndroidJavaObject invitationObj)
		{
			Logger.d("AndroidClient.OnInvitationReceived. Converting invitation...");
			Invitation inv = this.ConvertInvitation(invitationObj);
			Logger.d("Invitation: " + inv.ToString());
			if (this.mInvitationDelegate != null)
			{
				Logger.d("Delivering invitation to invitation received delegate.");
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					if (this.mInvitationDelegate != null)
					{
						this.mInvitationDelegate(inv, false);
					}
				});
			}
			else
			{
				Logger.w("AndroidClient.OnInvitationReceived discarding invitation because  delegate is null.");
			}
		}

		private void OnInvitationRemoved(string invitationId)
		{
			Logger.d("AndroidClient.OnInvitationRemoved: " + invitationId);
			this.ClearInvitationIfFromNotification(invitationId);
		}
	}
}
