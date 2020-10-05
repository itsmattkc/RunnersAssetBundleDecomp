using Facebook;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class FB : ScriptableObject
{
	public sealed class AppEvents
	{
		public static bool LimitEventUsage
		{
			get
			{
				return FB.facebook != null && FB.facebook.LimitEventUsage;
			}
			set
			{
				FB.facebook.LimitEventUsage = value;
			}
		}

		public static void LogEvent(string logEvent, float? valueToSum = null, Dictionary<string, object> parameters = null)
		{
			FB.FacebookImpl.AppEventsLogEvent(logEvent, valueToSum, parameters);
		}

		public static void LogPurchase(float logPurchase, string currency = "USD", Dictionary<string, object> parameters = null)
		{
			FB.FacebookImpl.AppEventsLogPurchase(logPurchase, currency, parameters);
		}
	}

	public sealed class Canvas
	{
		public static void Pay(string product, string action = "purchaseitem", int quantity = 1, int? quantityMin = null, int? quantityMax = null, string requestId = null, string pricepointId = null, string testCurrency = null, FacebookDelegate callback = null)
		{
			FB.FacebookImpl.Pay(product, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, callback);
		}

		public static void SetResolution(int width, int height, bool fullscreen, int preferredRefreshRate = 0, params FBScreen.Layout[] layoutParams)
		{
			FBScreen.SetResolution(width, height, fullscreen, preferredRefreshRate, layoutParams);
		}

		public static void SetAspectRatio(int width, int height, params FBScreen.Layout[] layoutParams)
		{
			FBScreen.SetAspectRatio(width, height, layoutParams);
		}
	}

	public sealed class Android
	{
		public static string KeyHash
		{
			get
			{
				AndroidFacebook androidFacebook = FB.facebook as AndroidFacebook;
				return (!(androidFacebook != null)) ? string.Empty : androidFacebook.KeyHash;
			}
		}
	}

	public abstract class RemoteFacebookLoader : MonoBehaviour
	{
		public delegate void LoadedDllCallback(IFacebook fb);

		private sealed class _LoadFacebookClass_c__Iterator0 : IDisposable, IEnumerator, IEnumerator<object>
		{
			internal string className;

			internal string _url___0;

			internal WWW _www___1;

			internal WWW _authTokenWww___2;

			internal Assembly _assembly___3;

			internal Type _facebookClass___4;

			internal IFacebook _fb___5;

			internal FB.RemoteFacebookLoader.LoadedDllCallback callback;

			internal int _PC;

			internal object _current;

			internal string ___className;

			internal FB.RemoteFacebookLoader.LoadedDllCallback ___callback;

			object IEnumerator<object>.Current
			{
				get
				{
					return this._current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					this._url___0 = string.Format(IntegratedPluginCanvasLocation.DllUrl, this.className);
					this._www___1 = new WWW(this._url___0);
					FbDebug.Log("loading dll: " + this._url___0);
					this._current = this._www___1;
					this._PC = 1;
					return true;
				case 1u:
					if (this._www___1.error == null)
					{
						this._authTokenWww___2 = new WWW(IntegratedPluginCanvasLocation.KeyUrl);
						this._current = this._authTokenWww___2;
						this._PC = 2;
						return true;
					}
					FbDebug.Error(this._www___1.error);
					if (FB.RemoteFacebookLoader.retryLoadCount < 3)
					{
						FB.RemoteFacebookLoader.retryLoadCount++;
					}
					this._www___1.Dispose();
					break;
				case 2u:
					if (this._authTokenWww___2.error != null)
					{
						FbDebug.Error("Cannot load from " + IntegratedPluginCanvasLocation.KeyUrl + ": " + this._authTokenWww___2.error);
						this._authTokenWww___2.Dispose();
					}
					else
					{
						this._assembly___3 = Security.LoadAndVerifyAssembly(this._www___1.bytes, this._authTokenWww___2.text);
						if (this._assembly___3 == null)
						{
							FbDebug.Error("Could not securely load assembly from " + this._url___0);
							this._www___1.Dispose();
						}
						else
						{
							this._facebookClass___4 = this._assembly___3.GetType("Facebook." + this.className);
							if (this._facebookClass___4 == null)
							{
								FbDebug.Error(this.className + " not found in assembly!");
								this._www___1.Dispose();
							}
							else
							{
								this._fb___5 = (typeof(FBComponentFactory).GetMethod("GetComponent").MakeGenericMethod(new Type[]
								{
									this._facebookClass___4
								}).Invoke(null, new object[]
								{
									IfNotExist.AddNew
								}) as IFacebook);
								if (this._fb___5 == null)
								{
									FbDebug.Error(this.className + " couldn't be created.");
									this._www___1.Dispose();
								}
								else
								{
									this.callback(this._fb___5);
									this._www___1.Dispose();
									this._PC = -1;
								}
							}
						}
					}
					break;
				}
				return false;
			}

			public void Dispose()
			{
				this._PC = -1;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		private sealed class _Start_c__Iterator1 : IDisposable, IEnumerator, IEnumerator<object>
		{
			internal IEnumerator _loader___0;

			internal int _PC;

			internal object _current;

			internal FB.RemoteFacebookLoader __f__this;

			object IEnumerator<object>.Current
			{
				get
				{
					return this._current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					this._loader___0 = FB.RemoteFacebookLoader.LoadFacebookClass(this.__f__this.className, new FB.RemoteFacebookLoader.LoadedDllCallback(this.__f__this.OnDllLoaded));
					break;
				case 1u:
					break;
				default:
					return false;
				}
				if (this._loader___0.MoveNext())
				{
					this._current = this._loader___0.Current;
					this._PC = 1;
					return true;
				}
				UnityEngine.Object.Destroy(this.__f__this);
				this._PC = -1;
				return false;
			}

			public void Dispose()
			{
				this._PC = -1;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		private const string facebookNamespace = "Facebook.";

		private const int maxRetryLoadCount = 3;

		private static int retryLoadCount;

		protected abstract string className
		{
			get;
		}

		public static IEnumerator LoadFacebookClass(string className, FB.RemoteFacebookLoader.LoadedDllCallback callback)
		{
			FB.RemoteFacebookLoader._LoadFacebookClass_c__Iterator0 _LoadFacebookClass_c__Iterator = new FB.RemoteFacebookLoader._LoadFacebookClass_c__Iterator0();
			_LoadFacebookClass_c__Iterator.className = className;
			_LoadFacebookClass_c__Iterator.callback = callback;
			_LoadFacebookClass_c__Iterator.___className = className;
			_LoadFacebookClass_c__Iterator.___callback = callback;
			return _LoadFacebookClass_c__Iterator;
		}

		private IEnumerator Start()
		{
			FB.RemoteFacebookLoader._Start_c__Iterator1 _Start_c__Iterator = new FB.RemoteFacebookLoader._Start_c__Iterator1();
			_Start_c__Iterator.__f__this = this;
			return _Start_c__Iterator;
		}

		private void OnDllLoaded(IFacebook fb)
		{
			FB.facebook = fb;
			FB.OnDllLoaded();
		}
	}

	public abstract class CompiledFacebookLoader : MonoBehaviour
	{
		protected abstract IFacebook fb
		{
			get;
		}

		private void Start()
		{
			FB.facebook = this.fb;
			FB.OnDllLoaded();
			UnityEngine.Object.Destroy(this);
		}
	}

	public static InitDelegate OnInitComplete;

	public static HideUnityDelegate OnHideUnity;

	private static IFacebook facebook;

	private static string authResponse;

	private static bool isInitCalled;

	private static string appId;

	private static bool cookie;

	private static bool logging;

	private static bool status;

	private static bool xfbml;

	private static bool frictionlessRequests;

	private static IFacebook FacebookImpl
	{
		get
		{
			if (FB.facebook == null)
			{
				throw new NullReferenceException("Facebook object is not yet loaded.  Did you call FB.Init()?");
			}
			return FB.facebook;
		}
	}

	public static string AppId
	{
		get
		{
			return FB.appId;
		}
	}

	public static string UserId
	{
		get
		{
			return (FB.facebook == null) ? string.Empty : FB.facebook.UserId;
		}
	}

	public static string AccessToken
	{
		get
		{
			return (FB.facebook == null) ? string.Empty : FB.facebook.AccessToken;
		}
	}

	public static DateTime AccessTokenExpiresAt
	{
		get
		{
			return (FB.facebook == null) ? DateTime.MinValue : FB.facebook.AccessTokenExpiresAt;
		}
	}

	public static bool IsLoggedIn
	{
		get
		{
			return FB.facebook != null && FB.facebook.IsLoggedIn;
		}
	}

	public static bool IsInitialized
	{
		get
		{
			return FB.facebook != null && FB.facebook.IsInitialized;
		}
	}

	public static void Init(InitDelegate onInitComplete, HideUnityDelegate onHideUnity = null, string authResponse = null)
	{
		FB.Init(onInitComplete, FBSettings.AppId, FBSettings.Cookie, FBSettings.Logging, FBSettings.Status, FBSettings.Xfbml, FBSettings.FrictionlessRequests, onHideUnity, authResponse);
	}

	public static void Init(InitDelegate onInitComplete, string appId, bool cookie = true, bool logging = true, bool status = true, bool xfbml = false, bool frictionlessRequests = true, HideUnityDelegate onHideUnity = null, string authResponse = null)
	{
		FB.appId = appId;
		FB.cookie = cookie;
		FB.logging = logging;
		FB.status = status;
		FB.xfbml = xfbml;
		FB.frictionlessRequests = frictionlessRequests;
		FB.authResponse = authResponse;
		FB.OnInitComplete = onInitComplete;
		FB.OnHideUnity = onHideUnity;
		if (!FB.isInitCalled)
		{
			FBBuildVersionAttribute versionAttributeOfType = FBBuildVersionAttribute.GetVersionAttributeOfType(typeof(IFacebook));
			if (versionAttributeOfType == null)
			{
				FbDebug.Warn("Cannot find Facebook SDK Version");
			}
			else
			{
				FbDebug.Info(string.Format("Using SDK {0}, Build {1}", versionAttributeOfType.SdkVersion, versionAttributeOfType.BuildVersion));
			}
			FBComponentFactory.GetComponent<AndroidFacebookLoader>(IfNotExist.AddNew);
			FB.isInitCalled = true;
			return;
		}
		FbDebug.Warn("FB.Init() has already been called.  You only need to call this once and only once.");
		if (FB.FacebookImpl != null)
		{
			FB.OnDllLoaded();
		}
	}

	private static void OnDllLoaded()
	{
		FBBuildVersionAttribute versionAttributeOfType = FBBuildVersionAttribute.GetVersionAttributeOfType(FB.FacebookImpl.GetType());
		if (versionAttributeOfType != null)
		{
			FbDebug.Log(string.Format("Finished loading Facebook dll. Version {0} Build {1}", versionAttributeOfType.SdkVersion, versionAttributeOfType.BuildVersion));
		}
		FB.FacebookImpl.Init(FB.OnInitComplete, FB.appId, FB.cookie, FB.logging, FB.status, FB.xfbml, FBSettings.ChannelUrl, FB.authResponse, FB.frictionlessRequests, FB.OnHideUnity);
	}

	public static void Login(string scope = "", FacebookDelegate callback = null)
	{
		FB.FacebookImpl.Login(scope, callback);
	}

	public static void Logout()
	{
		FB.FacebookImpl.Logout();
	}

	public static void AppRequest(string message, OGActionType actionType, string objectId, string[] to, string data = "", string title = "", FacebookDelegate callback = null)
	{
		FB.FacebookImpl.AppRequest(message, actionType, objectId, to, null, null, null, data, title, callback);
	}

	public static void AppRequest(string message, OGActionType actionType, string objectId, List<object> filters = null, string[] excludeIds = null, int? maxRecipients = null, string data = "", string title = "", FacebookDelegate callback = null)
	{
		FB.FacebookImpl.AppRequest(message, actionType, objectId, null, filters, excludeIds, maxRecipients, data, title, callback);
	}

	public static void AppRequest(string message, string[] to = null, List<object> filters = null, string[] excludeIds = null, int? maxRecipients = null, string data = "", string title = "", FacebookDelegate callback = null)
	{
		FB.FacebookImpl.AppRequest(message, null, null, to, filters, excludeIds, maxRecipients, data, title, callback);
	}

	public static void Feed(string toId = "", string link = "", string linkName = "", string linkCaption = "", string linkDescription = "", string picture = "", string mediaSource = "", string actionName = "", string actionLink = "", string reference = "", Dictionary<string, string[]> properties = null, FacebookDelegate callback = null)
	{
		FB.FacebookImpl.FeedRequest(toId, link, linkName, linkCaption, linkDescription, picture, mediaSource, actionName, actionLink, reference, properties, callback);
	}

	public static void API(string query, HttpMethod method, FacebookDelegate callback = null, Dictionary<string, string> formData = null)
	{
		FB.FacebookImpl.API(query, method, formData, callback);
	}

	public static void API(string query, HttpMethod method, FacebookDelegate callback, WWWForm formData)
	{
		FB.FacebookImpl.API(query, method, formData, callback);
	}

	[Obsolete("use FB.ActivateApp()")]
	public static void PublishInstall(FacebookDelegate callback = null)
	{
		FB.FacebookImpl.PublishInstall(FB.AppId, callback);
	}

	public static void ActivateApp()
	{
		FB.FacebookImpl.ActivateApp(FB.AppId);
	}

	public static void GetDeepLink(FacebookDelegate callback)
	{
		FB.FacebookImpl.GetDeepLink(callback);
	}

	public static void GameGroupCreate(string name, string description, string privacy = "CLOSED", FacebookDelegate callback = null)
	{
		FB.FacebookImpl.GameGroupCreate(name, description, privacy, callback);
	}

	public static void GameGroupJoin(string id, FacebookDelegate callback = null)
	{
		FB.FacebookImpl.GameGroupJoin(id, callback);
	}
}
