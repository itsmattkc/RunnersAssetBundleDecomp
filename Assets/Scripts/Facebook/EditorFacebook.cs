using Facebook.MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Facebook
{
	internal class EditorFacebook : AbstractFacebook, IFacebook
	{
		private sealed class _OnInit_c__Iterator2 : IDisposable, IEnumerator, IEnumerator<object>
		{
			internal InitDelegate onInitComplete;

			internal string appId;

			internal bool cookie;

			internal bool logging;

			internal bool status;

			internal bool xfbml;

			internal string channelUrl;

			internal string authResponse;

			internal bool frictionlessRequests;

			internal HideUnityDelegate hideUnityDelegate;

			internal int _PC;

			internal object _current;

			internal InitDelegate ___onInitComplete;

			internal string ___appId;

			internal bool ___cookie;

			internal bool ___logging;

			internal bool ___status;

			internal bool ___xfbml;

			internal string ___channelUrl;

			internal string ___authResponse;

			internal bool ___frictionlessRequests;

			internal HideUnityDelegate ___hideUnityDelegate;

			internal EditorFacebook __f__this;

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
					break;
				case 1u:
					break;
				default:
					return false;
				}
				if (this.__f__this.fb == null)
				{
					this._current = null;
					this._PC = 1;
					return true;
				}
				this.__f__this.fb.Init(this.onInitComplete, this.appId, this.cookie, this.logging, this.status, this.xfbml, this.channelUrl, this.authResponse, this.frictionlessRequests, this.hideUnityDelegate);
				this.__f__this.isInitialized = true;
				if (this.onInitComplete != null)
				{
					this.onInitComplete();
				}
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

		private AbstractFacebook fb;

		private FacebookDelegate loginCallback;

		public override int DialogMode
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public override bool LimitEventUsage
		{
			get
			{
				return this.limitEventUsage;
			}
			set
			{
				this.limitEventUsage = value;
			}
		}

		protected override void OnAwake()
		{
			base.StartCoroutine(FB.RemoteFacebookLoader.LoadFacebookClass("CanvasFacebook", new FB.RemoteFacebookLoader.LoadedDllCallback(this.OnDllLoaded)));
		}

		public override void Init(InitDelegate onInitComplete, string appId, bool cookie = false, bool logging = true, bool status = true, bool xfbml = false, string channelUrl = "", string authResponse = null, bool frictionlessRequests = false, HideUnityDelegate hideUnityDelegate = null)
		{
			base.StartCoroutine(this.OnInit(onInitComplete, appId, cookie, logging, status, xfbml, channelUrl, authResponse, frictionlessRequests, hideUnityDelegate));
		}

		private IEnumerator OnInit(InitDelegate onInitComplete, string appId, bool cookie = false, bool logging = true, bool status = true, bool xfbml = false, string channelUrl = "", string authResponse = null, bool frictionlessRequests = false, HideUnityDelegate hideUnityDelegate = null)
		{
			EditorFacebook._OnInit_c__Iterator2 _OnInit_c__Iterator = new EditorFacebook._OnInit_c__Iterator2();
			_OnInit_c__Iterator.onInitComplete = onInitComplete;
			_OnInit_c__Iterator.appId = appId;
			_OnInit_c__Iterator.cookie = cookie;
			_OnInit_c__Iterator.logging = logging;
			_OnInit_c__Iterator.status = status;
			_OnInit_c__Iterator.xfbml = xfbml;
			_OnInit_c__Iterator.channelUrl = channelUrl;
			_OnInit_c__Iterator.authResponse = authResponse;
			_OnInit_c__Iterator.frictionlessRequests = frictionlessRequests;
			_OnInit_c__Iterator.hideUnityDelegate = hideUnityDelegate;
			_OnInit_c__Iterator.___onInitComplete = onInitComplete;
			_OnInit_c__Iterator.___appId = appId;
			_OnInit_c__Iterator.___cookie = cookie;
			_OnInit_c__Iterator.___logging = logging;
			_OnInit_c__Iterator.___status = status;
			_OnInit_c__Iterator.___xfbml = xfbml;
			_OnInit_c__Iterator.___channelUrl = channelUrl;
			_OnInit_c__Iterator.___authResponse = authResponse;
			_OnInit_c__Iterator.___frictionlessRequests = frictionlessRequests;
			_OnInit_c__Iterator.___hideUnityDelegate = hideUnityDelegate;
			_OnInit_c__Iterator.__f__this = this;
			return _OnInit_c__Iterator;
		}

		private void OnDllLoaded(IFacebook fb)
		{
			this.fb = (AbstractFacebook)fb;
		}

		public override void Login(string scope = "", FacebookDelegate callback = null)
		{
			base.AddAuthDelegate(callback);
			FBComponentFactory.GetComponent<EditorFacebookAccessToken>(IfNotExist.AddNew);
		}

		public override void Logout()
		{
			this.isLoggedIn = false;
			this.userId = string.Empty;
			this.accessToken = string.Empty;
			this.fb.UserId = string.Empty;
			this.fb.AccessToken = string.Empty;
		}

		public override void AppRequest(string message, OGActionType actionType, string objectId, string[] to = null, List<object> filters = null, string[] excludeIds = null, int? maxRecipients = null, string data = "", string title = "", FacebookDelegate callback = null)
		{
			this.fb.AppRequest(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
		}

		public override void FeedRequest(string toId = "", string link = "", string linkName = "", string linkCaption = "", string linkDescription = "", string picture = "", string mediaSource = "", string actionName = "", string actionLink = "", string reference = "", Dictionary<string, string[]> properties = null, FacebookDelegate callback = null)
		{
			this.fb.FeedRequest(toId, link, linkName, linkCaption, linkDescription, picture, mediaSource, actionName, actionLink, reference, properties, callback);
		}

		public override void Pay(string product, string action = "purchaseitem", int quantity = 1, int? quantityMin = null, int? quantityMax = null, string requestId = null, string pricepointId = null, string testCurrency = null, FacebookDelegate callback = null)
		{
			FbDebug.Info("Pay method only works with Facebook Canvas.  Does nothing in the Unity Editor, iOS or Android");
		}

		public override void GameGroupCreate(string name, string description, string privacy = "CLOSED", FacebookDelegate callback = null)
		{
			throw new PlatformNotSupportedException("There is no Facebook GameGroupCreate Dialog on Editor");
		}

		public override void GameGroupJoin(string id, FacebookDelegate callback = null)
		{
			throw new PlatformNotSupportedException("There is no Facebook GameGroupJoin Dialog on Editor");
		}

		public override void GetAuthResponse(FacebookDelegate callback = null)
		{
			this.fb.GetAuthResponse(callback);
		}

		public override void PublishInstall(string appId, FacebookDelegate callback = null)
		{
		}

		public override void ActivateApp(string appId = null)
		{
			FbDebug.Info("This only needs to be called for iOS or Android.");
		}

		public override void GetDeepLink(FacebookDelegate callback)
		{
			FbDebug.Info("No Deep Linking in the Editor");
			if (callback != null)
			{
				callback(new FBResult("<platform dependent>", null));
			}
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum = null, Dictionary<string, object> parameters = null)
		{
			FbDebug.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency = "USD", Dictionary<string, object> parameters = null)
		{
			FbDebug.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
		}

		public void MockLoginCallback(FBResult result)
		{
			UnityEngine.Object.Destroy(FBComponentFactory.GetComponent<EditorFacebookAccessToken>(IfNotExist.AddNew));
			if (result.Error != null)
			{
				this.BadAccessToken(result.Error);
				return;
			}
			try
			{
				List<object> list = (List<object>)Json.Deserialize(result.Text);
				List<string> list2 = new List<string>();
				foreach (object current in list)
				{
					if (current is Dictionary<string, object>)
					{
						Dictionary<string, object> dictionary = (Dictionary<string, object>)current;
						if (dictionary.ContainsKey("body"))
						{
							list2.Add((string)dictionary["body"]);
						}
					}
				}
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)Json.Deserialize(list2[0]);
				Dictionary<string, object> dictionary3 = (Dictionary<string, object>)Json.Deserialize(list2[1]);
				if (FB.AppId != (string)dictionary3["id"])
				{
					this.BadAccessToken("Access token is not for current app id: " + FB.AppId);
				}
				else
				{
					this.userId = (string)dictionary2["id"];
					this.fb.UserId = this.userId;
					this.fb.AccessToken = this.accessToken;
					this.isLoggedIn = true;
					base.OnAuthResponse(new FBResult(string.Empty, null));
				}
			}
			catch (Exception ex)
			{
				this.BadAccessToken("Could not get data from access token: " + ex.Message);
			}
		}

		public void MockCancelledLoginCallback()
		{
			base.OnAuthResponse(new FBResult(string.Empty, null));
		}

		private void BadAccessToken(string error)
		{
			FbDebug.Error(error);
			this.userId = string.Empty;
			this.fb.UserId = string.Empty;
			this.accessToken = string.Empty;
			this.fb.AccessToken = string.Empty;
			FBComponentFactory.GetComponent<EditorFacebookAccessToken>(IfNotExist.AddNew);
		}
	}
}
