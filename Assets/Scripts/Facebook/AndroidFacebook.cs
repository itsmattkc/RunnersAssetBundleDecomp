using Facebook.MiniJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Facebook
{
	internal sealed class AndroidFacebook : AbstractFacebook, IFacebook
	{
		public const int BrowserDialogMode = 0;

		private const string AndroidJavaFacebookClass = "com.facebook.unity.FB";

		private const string CallbackIdKey = "callback_id";

		private string keyHash;

		private FacebookDelegate deepLinkDelegate;

		private AndroidJavaClass fbJava;

		private InitDelegate onInitComplete;

		public string KeyHash
		{
			get
			{
				return this.keyHash;
			}
		}

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
				this.CallFB("SetLimitEventUsage", value.ToString());
			}
		}

		private AndroidJavaClass FB
		{
			get
			{
				if (this.fbJava == null)
				{
					this.fbJava = new AndroidJavaClass("com.facebook.unity.FB");
					if (this.fbJava == null)
					{
						throw new MissingReferenceException(string.Format("AndroidFacebook failed to load {0} class", "com.facebook.unity.FB"));
					}
				}
				return this.fbJava;
			}
		}

		private void CallFB(string method, string args)
		{
			this.FB.CallStatic(method, new object[]
			{
				args
			});
		}

		protected override void OnAwake()
		{
			this.keyHash = string.Empty;
		}

		private bool IsErrorResponse(string response)
		{
			return false;
		}

		public override void Init(InitDelegate onInitComplete, string appId, bool cookie = false, bool logging = true, bool status = true, bool xfbml = false, string channelUrl = "", string authResponse = null, bool frictionlessRequests = false, HideUnityDelegate hideUnityDelegate = null)
		{
			if (string.IsNullOrEmpty(appId))
			{
				throw new ArgumentException("appId cannot be null or empty!");
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("appId", appId);
			if (cookie)
			{
				dictionary.Add("cookie", true);
			}
			if (!logging)
			{
				dictionary.Add("logging", false);
			}
			if (!status)
			{
				dictionary.Add("status", false);
			}
			if (xfbml)
			{
				dictionary.Add("xfbml", true);
			}
			if (!string.IsNullOrEmpty(channelUrl))
			{
				dictionary.Add("channelUrl", channelUrl);
			}
			if (!string.IsNullOrEmpty(authResponse))
			{
				dictionary.Add("authResponse", authResponse);
			}
			if (frictionlessRequests)
			{
				dictionary.Add("frictionlessRequests", true);
			}
			string text = Json.Serialize(dictionary);
			this.onInitComplete = onInitComplete;
			this.CallFB("Init", text.ToString());
		}

		public void OnInitComplete(string message)
		{
			this.isInitialized = true;
			this.OnLoginComplete(message);
			if (this.onInitComplete != null)
			{
				this.onInitComplete();
			}
		}

		public override void Login(string scope = "", FacebookDelegate callback = null)
		{
			string args = Json.Serialize(new Dictionary<string, object>
			{
				{
					"scope",
					scope
				}
			});
			base.AddAuthDelegate(callback);
			this.CallFB("Login", args);
		}

		public void OnLoginComplete(string message)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(message);
			if (dictionary.ContainsKey("user_id"))
			{
				this.isLoggedIn = true;
				this.userId = (string)dictionary["user_id"];
				this.accessToken = (string)dictionary["access_token"];
				this.accessTokenExpiresAt = this.FromTimestamp(int.Parse((string)dictionary["expiration_timestamp"]));
			}
			if (dictionary.ContainsKey("key_hash"))
			{
				this.keyHash = (string)dictionary["key_hash"];
			}
			base.OnAuthResponse(new FBResult(message, null));
		}

		public void OnGroupCreateComplete(string message)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(message);
			string uniqueId = (string)dictionary["callback_id"];
			dictionary.Remove("callback_id");
			base.OnFacebookResponse(uniqueId, new FBResult(Json.Serialize(dictionary), null));
		}

		public void OnAccessTokenRefresh(string message)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(message);
			if (dictionary.ContainsKey("access_token"))
			{
				this.accessToken = (string)dictionary["access_token"];
				this.accessTokenExpiresAt = this.FromTimestamp(int.Parse((string)dictionary["expiration_timestamp"]));
			}
		}

		public override void Logout()
		{
			this.CallFB("Logout", string.Empty);
		}

		public void OnLogoutComplete(string message)
		{
			this.isLoggedIn = false;
			this.userId = string.Empty;
			this.accessToken = string.Empty;
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
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["message"] = message;
			if (callback != null)
			{
				dictionary["callback_id"] = base.AddFacebookDelegate(callback);
			}
			if (actionType != null && !string.IsNullOrEmpty(objectId))
			{
				dictionary["action_type"] = actionType.ToString();
				dictionary["object_id"] = objectId;
			}
			if (to != null)
			{
				dictionary["to"] = string.Join(",", to);
			}
			if (filters != null && filters.Count > 0)
			{
				string text = filters[0] as string;
				if (text != null)
				{
					dictionary["filters"] = text;
				}
			}
			if (maxRecipients.HasValue)
			{
				dictionary["max_recipients"] = maxRecipients.Value;
			}
			if (!string.IsNullOrEmpty(data))
			{
				dictionary["data"] = data;
			}
			if (!string.IsNullOrEmpty(title))
			{
				dictionary["title"] = title;
			}
			this.CallFB("AppRequest", Json.Serialize(dictionary));
		}

		public void OnAppRequestsComplete(string message)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(message);
			if (dictionary.ContainsKey("callback_id"))
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				string uniqueId = (string)dictionary["callback_id"];
				dictionary.Remove("callback_id");
				if (dictionary.Count > 0)
				{
					List<string> list = new List<string>(dictionary.Count - 1);
					foreach (string current in dictionary.Keys)
					{
						if (!current.StartsWith("to"))
						{
							dictionary2[current] = dictionary[current];
						}
						else
						{
							list.Add((string)dictionary[current]);
						}
					}
					dictionary2.Add("to", list);
					dictionary.Clear();
					base.OnFacebookResponse(uniqueId, new FBResult(Json.Serialize(dictionary2), null));
				}
				else
				{
					base.OnFacebookResponse(uniqueId, new FBResult(Json.Serialize(dictionary2), "Malformed request response.  Please file a bug with facebook here: https://developers.facebook.com/bugs/create"));
				}
			}
		}

		public override void FeedRequest(string toId = "", string link = "", string linkName = "", string linkCaption = "", string linkDescription = "", string picture = "", string mediaSource = "", string actionName = "", string actionLink = "", string reference = "", Dictionary<string, string[]> properties = null, FacebookDelegate callback = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (callback != null)
			{
				dictionary["callback_id"] = base.AddFacebookDelegate(callback);
			}
			if (!string.IsNullOrEmpty(toId))
			{
				dictionary.Add("to", toId);
			}
			if (!string.IsNullOrEmpty(link))
			{
				dictionary.Add("link", link);
			}
			if (!string.IsNullOrEmpty(linkName))
			{
				dictionary.Add("name", linkName);
			}
			if (!string.IsNullOrEmpty(linkCaption))
			{
				dictionary.Add("caption", linkCaption);
			}
			if (!string.IsNullOrEmpty(linkDescription))
			{
				dictionary.Add("description", linkDescription);
			}
			if (!string.IsNullOrEmpty(picture))
			{
				dictionary.Add("picture", picture);
			}
			if (!string.IsNullOrEmpty(mediaSource))
			{
				dictionary.Add("source", mediaSource);
			}
			if (!string.IsNullOrEmpty(actionName) && !string.IsNullOrEmpty(actionLink))
			{
				dictionary.Add("actions", new Dictionary<string, object>[]
				{
					new Dictionary<string, object>
					{
						{
							"name",
							actionName
						},
						{
							"link",
							actionLink
						}
					}
				});
			}
			if (!string.IsNullOrEmpty(reference))
			{
				dictionary.Add("ref", reference);
			}
			if (properties != null)
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				foreach (KeyValuePair<string, string[]> current in properties)
				{
					if (current.Value.Length >= 1)
					{
						if (current.Value.Length == 1)
						{
							dictionary2.Add(current.Key, current.Value[0]);
						}
						else
						{
							Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
							dictionary3.Add("text", current.Value[0]);
							dictionary3.Add("href", current.Value[1]);
							dictionary2.Add(current.Key, dictionary3);
						}
					}
				}
				dictionary.Add("properties", dictionary2);
			}
			this.CallFB("FeedRequest", Json.Serialize(dictionary));
		}

		public void OnFeedRequestComplete(string message)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(message);
			if (dictionary.ContainsKey("callback_id"))
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				string uniqueId = (string)dictionary["callback_id"];
				dictionary.Remove("callback_id");
				if (dictionary.Count > 0)
				{
					foreach (string current in dictionary.Keys)
					{
						dictionary2[current] = dictionary[current];
					}
					dictionary.Clear();
					base.OnFacebookResponse(uniqueId, new FBResult(Json.Serialize(dictionary2), null));
				}
				else
				{
					base.OnFacebookResponse(uniqueId, new FBResult(Json.Serialize(dictionary2), "Malformed request response.  Please file a bug with facebook here: https://developers.facebook.com/bugs/create"));
				}
			}
		}

		public override void Pay(string product, string action = "purchaseitem", int quantity = 1, int? quantityMin = null, int? quantityMax = null, string requestId = null, string pricepointId = null, string testCurrency = null, FacebookDelegate callback = null)
		{
			throw new PlatformNotSupportedException("There is no Facebook Pay Dialog on Android");
		}

		public override void GameGroupCreate(string name, string description, string privacy = "CLOSED", FacebookDelegate callback = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["name"] = name;
			dictionary["description"] = description;
			dictionary["privacy"] = privacy;
			if (callback != null)
			{
				dictionary["callback_id"] = base.AddFacebookDelegate(callback);
			}
			this.CallFB("GameGroupCreate", Json.Serialize(dictionary));
		}

		public override void GameGroupJoin(string id, FacebookDelegate callback = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["id"] = id;
			if (callback != null)
			{
				dictionary["callback_id"] = base.AddFacebookDelegate(callback);
			}
			this.CallFB("GameGroupJoin", Json.Serialize(dictionary));
		}

		public override void GetDeepLink(FacebookDelegate callback)
		{
			if (callback != null)
			{
				this.deepLinkDelegate = callback;
				this.CallFB("GetDeepLink", string.Empty);
			}
		}

		public void OnGetDeepLinkComplete(string message)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(message);
			if (this.deepLinkDelegate != null)
			{
				object empty = string.Empty;
				dictionary.TryGetValue("deep_link", out empty);
				this.deepLinkDelegate(new FBResult(empty.ToString(), null));
			}
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum = null, Dictionary<string, object> parameters = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["logEvent"] = logEvent;
			if (valueToSum.HasValue)
			{
				dictionary["valueToSum"] = valueToSum.Value;
			}
			if (parameters != null)
			{
				dictionary["parameters"] = this.ToStringDict(parameters);
			}
			this.CallFB("AppEvents", Json.Serialize(dictionary));
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency = "USD", Dictionary<string, object> parameters = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["logPurchase"] = logPurchase;
			dictionary["currency"] = (string.IsNullOrEmpty(currency) ? "USD" : currency);
			if (parameters != null)
			{
				dictionary["parameters"] = this.ToStringDict(parameters);
			}
			this.CallFB("AppEvents", Json.Serialize(dictionary));
		}

		public override void PublishInstall(string appId, FacebookDelegate callback = null)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(2);
			dictionary["app_id"] = appId;
			if (callback != null)
			{
				dictionary["callback_id"] = base.AddFacebookDelegate(callback);
			}
			this.CallFB("PublishInstall", Json.Serialize(dictionary));
		}

		public void OnPublishInstallComplete(string message)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(message);
			if (dictionary.ContainsKey("callback_id"))
			{
				base.OnFacebookResponse((string)dictionary["callback_id"], new FBResult(string.Empty, null));
			}
		}

		public override void ActivateApp(string appId = null)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(1);
			if (!string.IsNullOrEmpty(appId))
			{
				dictionary["app_id"] = appId;
			}
			this.CallFB("ActivateApp", Json.Serialize(dictionary));
		}

		private Dictionary<string, string> ToStringDict(Dictionary<string, object> dict)
		{
			if (dict == null)
			{
				return null;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, object> current in dict)
			{
				dictionary[current.Key] = current.Value.ToString();
			}
			return dictionary;
		}

		private DateTime FromTimestamp(int timestamp)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return dateTime.AddSeconds((double)timestamp);
		}
	}
}
