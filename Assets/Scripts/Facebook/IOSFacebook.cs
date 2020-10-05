using Facebook.MiniJSON;
using System;
using System.Collections.Generic;

namespace Facebook
{
	internal class IOSFacebook : AbstractFacebook, IFacebook
	{
		private class NativeDict
		{
			public int numEntries;

			public string[] keys;

			public string[] vals;

			public NativeDict()
			{
				this.numEntries = 0;
				this.keys = null;
				this.vals = null;
			}
		}

		public enum FBInsightsFlushBehavior
		{
			FBInsightsFlushBehaviorAuto,
			FBInsightsFlushBehaviorExplicitOnly
		}

		private const string CancelledResponse = "{\"cancelled\":true}";

		private int dialogMode = 1;

		private InitDelegate externalInitDelegate;

		private FacebookDelegate deepLinkDelegate;

		public override int DialogMode
		{
			get
			{
				return this.dialogMode;
			}
			set
			{
				this.dialogMode = value;
				this.iosSetShareDialogMode(this.dialogMode);
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
				this.iosFBAppEventsSetLimitEventUsage(value);
			}
		}

		private void iosInit(string appId, bool cookie, bool logging, bool status, bool frictionlessRequests, string urlSuffix)
		{
		}

		private void iosLogin(string scope)
		{
		}

		private void iosLogout()
		{
		}

		private void iosSetShareDialogMode(int mode)
		{
		}

		private void iosFeedRequest(int requestId, string toId, string link, string linkName, string linkCaption, string linkDescription, string picture, string mediaSource, string actionName, string actionLink, string reference)
		{
		}

		private void iosAppRequest(int requestId, string message, string actionType, string objectId, string[] to = null, int toLength = 0, string filters = "", string[] excludeIds = null, int excludeIdsLength = 0, bool hasMaxRecipients = false, int maxRecipients = 0, string data = "", string title = "")
		{
		}

		private void iosCreateGameGroup(int requestId, string name, string description, string privacy)
		{
		}

		private void iosJoinGameGroup(int requestId, string id)
		{
		}

		private void iosFBSettingsPublishInstall(int requestId, string appId)
		{
		}

		private void iosFBSettingsActivateApp(string appId)
		{
		}

		private void iosFBAppEventsLogEvent(string logEvent, double valueToSum, int numParams, string[] paramKeys, string[] paramVals)
		{
		}

		private void iosFBAppEventsLogPurchase(double logPurchase, string currency, int numParams, string[] paramKeys, string[] paramVals)
		{
		}

		private void iosFBAppEventsSetLimitEventUsage(bool limitEventUsage)
		{
		}

		private void iosGetDeepLink()
		{
		}

		protected override void OnAwake()
		{
			this.accessToken = "NOT_USED_ON_IOS_FACEBOOK";
		}

		public override void Init(InitDelegate onInitComplete, string appId, bool cookie = false, bool logging = true, bool status = true, bool xfbml = false, string channelUrl = "", string authResponse = null, bool frictionlessRequests = false, HideUnityDelegate hideUnityDelegate = null)
		{
			this.iosInit(appId, cookie, logging, status, frictionlessRequests, FBSettings.IosURLSuffix);
			this.externalInitDelegate = onInitComplete;
		}

		public override void Login(string scope = "", FacebookDelegate callback = null)
		{
			base.AddAuthDelegate(callback);
			this.iosLogin(scope);
		}

		public override void Logout()
		{
			this.iosLogout();
			this.isLoggedIn = false;
		}

		public override void AppRequest(string message, OGActionType actionType, string objectId, string[] to = null, List<object> filters = null, string[] excludeIds = null, int? maxRecipients = null, string data = "", string title = "", FacebookDelegate callback = null)
		{
			if (string.IsNullOrEmpty(message))
			{
				throw new ArgumentNullException("message", "message cannot be null or empty!");
			}
			if (actionType != null && string.IsNullOrEmpty(objectId))
			{
				throw new ArgumentNullException("objectId", "You cannot provide an actionType without an objectId");
			}
			if (actionType == null && !string.IsNullOrEmpty(objectId))
			{
				throw new ArgumentNullException("actionType", "You cannot provide an objectId without an actionType");
			}
			string text = null;
			if (filters != null && filters.Count > 0)
			{
				text = (filters[0] as string);
			}
			this.iosAppRequest(Convert.ToInt32(base.AddFacebookDelegate(callback)), message, (actionType == null) ? null : actionType.ToString(), objectId, to, (to == null) ? 0 : to.Length, (text == null) ? string.Empty : text, excludeIds, (excludeIds == null) ? 0 : excludeIds.Length, maxRecipients.HasValue, (!maxRecipients.HasValue) ? 0 : maxRecipients.Value, data, title);
		}

		public override void FeedRequest(string toId = "", string link = "", string linkName = "", string linkCaption = "", string linkDescription = "", string picture = "", string mediaSource = "", string actionName = "", string actionLink = "", string reference = "", Dictionary<string, string[]> properties = null, FacebookDelegate callback = null)
		{
			this.iosFeedRequest(Convert.ToInt32(base.AddFacebookDelegate(callback)), toId, link, linkName, linkCaption, linkDescription, picture, mediaSource, actionName, actionLink, reference);
		}

		public override void Pay(string product, string action = "purchaseitem", int quantity = 1, int? quantityMin = null, int? quantityMax = null, string requestId = null, string pricepointId = null, string testCurrency = null, FacebookDelegate callback = null)
		{
			throw new PlatformNotSupportedException("There is no Facebook Pay Dialog on iOS");
		}

		public override void GameGroupCreate(string name, string description, string privacy = "CLOSED", FacebookDelegate callback = null)
		{
			this.iosCreateGameGroup(Convert.ToInt32(base.AddFacebookDelegate(callback)), name, description, privacy);
		}

		public override void GameGroupJoin(string id, FacebookDelegate callback = null)
		{
			this.iosJoinGameGroup(Convert.ToInt32(base.AddFacebookDelegate(callback)), id);
		}

		public override void GetDeepLink(FacebookDelegate callback)
		{
			if (callback == null)
			{
				return;
			}
			this.deepLinkDelegate = callback;
			this.iosGetDeepLink();
		}

		public void OnGetDeepLinkComplete(string message)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(message);
			if (this.deepLinkDelegate == null)
			{
				return;
			}
			object empty = string.Empty;
			dictionary.TryGetValue("deep_link", out empty);
			this.deepLinkDelegate(new FBResult(empty.ToString(), null));
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum = null, Dictionary<string, object> parameters = null)
		{
			IOSFacebook.NativeDict nativeDict = this.MarshallDict(parameters);
			if (valueToSum.HasValue)
			{
				this.iosFBAppEventsLogEvent(logEvent, (double)valueToSum.Value, nativeDict.numEntries, nativeDict.keys, nativeDict.vals);
			}
			else
			{
				this.iosFBAppEventsLogEvent(logEvent, 0.0, nativeDict.numEntries, nativeDict.keys, nativeDict.vals);
			}
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency = "USD", Dictionary<string, object> parameters = null)
		{
			IOSFacebook.NativeDict nativeDict = this.MarshallDict(parameters);
			if (string.IsNullOrEmpty(currency))
			{
				currency = "USD";
			}
			this.iosFBAppEventsLogPurchase((double)logPurchase, currency, nativeDict.numEntries, nativeDict.keys, nativeDict.vals);
		}

		public override void PublishInstall(string appId, FacebookDelegate callback = null)
		{
			this.iosFBSettingsPublishInstall(Convert.ToInt32(base.AddFacebookDelegate(callback)), appId);
		}

		public override void ActivateApp(string appId = null)
		{
			this.iosFBSettingsActivateApp(appId);
		}

		private IOSFacebook.NativeDict MarshallDict(Dictionary<string, object> dict)
		{
			IOSFacebook.NativeDict nativeDict = new IOSFacebook.NativeDict();
			if (dict != null && dict.Count > 0)
			{
				nativeDict.keys = new string[dict.Count];
				nativeDict.vals = new string[dict.Count];
				nativeDict.numEntries = 0;
				foreach (KeyValuePair<string, object> current in dict)
				{
					nativeDict.keys[nativeDict.numEntries] = current.Key;
					nativeDict.vals[nativeDict.numEntries] = current.Value.ToString();
					nativeDict.numEntries++;
				}
			}
			return nativeDict;
		}

		private IOSFacebook.NativeDict MarshallDict(Dictionary<string, string> dict)
		{
			IOSFacebook.NativeDict nativeDict = new IOSFacebook.NativeDict();
			if (dict != null && dict.Count > 0)
			{
				nativeDict.keys = new string[dict.Count];
				nativeDict.vals = new string[dict.Count];
				nativeDict.numEntries = 0;
				foreach (KeyValuePair<string, string> current in dict)
				{
					nativeDict.keys[nativeDict.numEntries] = current.Key;
					nativeDict.vals[nativeDict.numEntries] = current.Value;
					nativeDict.numEntries++;
				}
			}
			return nativeDict;
		}

		private void OnInitComplete(string msg)
		{
			this.isInitialized = true;
			if (!string.IsNullOrEmpty(msg))
			{
				this.OnLogin(msg);
			}
			this.externalInitDelegate();
		}

		public void OnLogin(string msg)
		{
			if (string.IsNullOrEmpty(msg))
			{
				base.OnAuthResponse(new FBResult("{\"cancelled\":true}", null));
				return;
			}
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(msg);
			if (dictionary.ContainsKey("user_id"))
			{
				this.isLoggedIn = true;
			}
			this.ParseLoginDict(dictionary);
			base.OnAuthResponse(new FBResult(msg, null));
		}

		public void ParseLoginDict(Dictionary<string, object> parameters)
		{
			if (parameters.ContainsKey("user_id"))
			{
				this.userId = (string)parameters["user_id"];
			}
			if (parameters.ContainsKey("access_token"))
			{
				this.accessToken = (string)parameters["access_token"];
			}
			if (parameters.ContainsKey("expiration_timestamp"))
			{
				this.accessTokenExpiresAt = this.FromTimestamp(int.Parse((string)parameters["expiration_timestamp"]));
			}
		}

		public void OnAccessTokenRefresh(string message)
		{
			Dictionary<string, object> parameters = (Dictionary<string, object>)Json.Deserialize(message);
			this.ParseLoginDict(parameters);
		}

		private DateTime FromTimestamp(int timestamp)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return dateTime.AddSeconds((double)timestamp);
		}

		public void OnLogout(string msg)
		{
			this.isLoggedIn = false;
		}

		public void OnRequestComplete(string msg)
		{
			int num = msg.IndexOf(":");
			if (num <= 0)
			{
				FbDebug.Error("Malformed callback from ios.  I expected the form id:message but couldn't find either the ':' character or the id.");
				FbDebug.Error("Here's the message that errored: " + msg);
				return;
			}
			string text = msg.Substring(0, num);
			string text2 = msg.Substring(num + 1);
			FbDebug.Info("id:" + text + " msg:" + text2);
			base.OnFacebookResponse(text, new FBResult(text2, null));
		}
	}
}
