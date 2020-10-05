using GooglePlayGames.BasicApi;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GooglePlayGames.Android
{
	internal class OnStateResultProxy : AndroidJavaProxy
	{
		private sealed class _OnStateConflict_c__AnonStoreyF7
		{
			internal int stateKey;

			internal byte[] localData;

			internal byte[] serverData;

			internal string resolvedVersion;

			internal OnStateResultProxy __f__this;

			internal void __m__3C()
			{
				byte[] resolvedData = this.__f__this.mListener.OnStateConflict(this.stateKey, this.localData, this.serverData);
				this.__f__this.mAndroidClient.ResolveState(this.stateKey, this.resolvedVersion, resolvedData, this.__f__this.mListener);
			}
		}

		private sealed class _OnStateLoaded_c__AnonStoreyF8
		{
			internal bool success;

			internal int stateKey;

			internal byte[] localData;

			internal OnStateResultProxy __f__this;

			internal void __m__3D()
			{
				this.__f__this.mListener.OnStateLoaded(this.success, this.stateKey, this.localData);
			}
		}

		private OnStateLoadedListener mListener;

		private AndroidClient mAndroidClient;

		internal OnStateResultProxy(AndroidClient androidClient, OnStateLoadedListener listener) : base("com.google.android.gms.common.api.ResultCallback")
		{
			this.mListener = listener;
			this.mAndroidClient = androidClient;
		}

		private void OnStateConflict(int stateKey, string resolvedVersion, byte[] localData, byte[] serverData)
		{
			Logger.d(string.Concat(new object[]
			{
				"OnStateResultProxy.onStateConflict called, stateKey=",
				stateKey,
				", resolvedVersion=",
				resolvedVersion
			}));
			this.debugLogData("localData", localData);
			this.debugLogData("serverData", serverData);
			if (this.mListener != null)
			{
				Logger.d("OnStateResultProxy.onStateConflict invoking conflict callback.");
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					byte[] resolvedData = this.mListener.OnStateConflict(stateKey, localData, serverData);
					this.mAndroidClient.ResolveState(stateKey, resolvedVersion, resolvedData, this.mListener);
				});
			}
			else
			{
				Logger.w("No conflict callback specified! Cannot resolve cloud save conflict.");
			}
		}

		private void OnStateLoaded(int statusCode, int stateKey, byte[] localData)
		{
			Logger.d(string.Concat(new object[]
			{
				"OnStateResultProxy.onStateLoaded called, status ",
				statusCode,
				", stateKey=",
				stateKey
			}));
			this.debugLogData("localData", localData);
			bool success = false;
			switch (statusCode)
			{
			case 0:
				Logger.d("Status is OK, so success.");
				success = true;
				goto IL_124;
			case 1:
			case 2:
				IL_86:
				if (statusCode != 2002)
				{
					Logger.e("Cloud load failed with status code " + statusCode);
					success = false;
					localData = null;
					goto IL_124;
				}
				Logger.d("Status is KEY NOT FOUND, which is a success, but with no data.");
				success = true;
				localData = null;
				goto IL_124;
			case 3:
				Logger.d("Status is STALE DATA, so considering as success.");
				success = true;
				goto IL_124;
			case 4:
				Logger.d("Status is NO DATA (no network?), so it's a failure.");
				success = false;
				localData = null;
				goto IL_124;
			}
			goto IL_86;
			IL_124:
			if (this.mListener != null)
			{
				Logger.d("OnStateResultProxy.onStateLoaded invoking load callback.");
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mListener.OnStateLoaded(success, stateKey, localData);
				});
			}
			else
			{
				Logger.w("No load callback specified!");
			}
		}

		private void debugLogData(string tag, byte[] data)
		{
			Logger.d("   " + tag + ": " + Logger.describe(data));
		}

		public void onResult(AndroidJavaObject result)
		{
			Logger.d("OnStateResultProxy.onResult, result=" + result);
			int statusCode = JavaUtil.GetStatusCode(result);
			Logger.d("OnStateResultProxy: status code is " + statusCode);
			if (result == null)
			{
				Logger.e("OnStateResultProxy: result is null.");
				return;
			}
			Logger.d("OnstateResultProxy: retrieving result objects...");
			AndroidJavaObject androidJavaObject = JavaUtil.CallNullSafeObjectMethod(result, "getLoadedResult", new object[0]);
			AndroidJavaObject androidJavaObject2 = JavaUtil.CallNullSafeObjectMethod(result, "getConflictResult", new object[0]);
			Logger.d("Got result objects.");
			Logger.d("loadedResult = " + androidJavaObject);
			Logger.d("conflictResult = " + androidJavaObject2);
			if (androidJavaObject2 != null)
			{
				Logger.d("OnStateResultProxy: processing conflict.");
				int stateKey = androidJavaObject2.Call<int>("getStateKey", new object[0]);
				string resolvedVersion = androidJavaObject2.Call<string>("getResolvedVersion", new object[0]);
				byte[] localData = JavaUtil.ConvertByteArray(JavaUtil.CallNullSafeObjectMethod(androidJavaObject2, "getLocalData", new object[0]));
				byte[] serverData = JavaUtil.ConvertByteArray(JavaUtil.CallNullSafeObjectMethod(androidJavaObject2, "getServerData", new object[0]));
				Logger.d("OnStateResultProxy: conflict args parsed, calling.");
				this.OnStateConflict(stateKey, resolvedVersion, localData, serverData);
			}
			else if (androidJavaObject != null)
			{
				Logger.d("OnStateResultProxy: processing normal load.");
				int stateKey2 = androidJavaObject.Call<int>("getStateKey", new object[0]);
				byte[] localData2 = JavaUtil.ConvertByteArray(JavaUtil.CallNullSafeObjectMethod(androidJavaObject, "getLocalData", new object[0]));
				Logger.d("OnStateResultProxy: loaded args parsed, calling.");
				this.OnStateLoaded(statusCode, stateKey2, localData2);
			}
			else
			{
				Logger.e("OnStateResultProxy: both loadedResult and conflictResult are null!");
			}
		}
	}
}
