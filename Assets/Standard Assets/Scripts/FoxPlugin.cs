using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FoxPlugin : MonoBehaviour
{
	public const string PARAM_SKU = "_sku";

	public const string PARAM_PRICE = "_price";

	public const string PARAM_OUT = "_out";

	public const string PARAM_CURRENCY = "_currency";

	[SerializeField]
	private ConversionData conversionData = new ConversionData("default");

	private ArrayList parms;





	private static Action<string> __f__am_cache3;

	public static event Action<string> CheckVersion;

	private void Start()
	{
		this._sendConversion();
	}

	public static void sendConversion()
	{
		FoxPluginDefaults.sendConversion();
		UnityEngine.Debug.Log("FOX-LIB sendConversion");
	}

	public static void sendConversion(string url)
	{
		FoxPluginDefaults.sendConversion(url);
		UnityEngine.Debug.Log("FOX-LIB sendConversion android");
	}

	public static void sendConversionOnconsent(string url)
	{
		FoxPluginDefaults.sendConversion(url);
	}

	public static void sendConversion(string url, string buid)
	{
		FoxPluginDefaults.sendConversion(url, buid);
		UnityEngine.Debug.Log("FOX-LIB sendConversion android");
	}

	public static void sendConversionForMobage(string mbgaAppId)
	{
		FoxPluginDefaults.sendConversionForMobage(mbgaAppId);
	}

	public static void sendConversionForMobage(string url, string mbgaAppId)
	{
		FoxPluginDefaults.sendConversionForMobage(url, mbgaAppId);
	}

	public static void sendUserIdForMobage(string mbgaUserId)
	{
		FoxPluginDefaults.sendUserIdForMobage(mbgaUserId);
	}

	public static void setServerUrl(string serverUrl)
	{
		FoxPluginDefaults.setServerUrl(serverUrl);
	}

	public static void setMode(string mode)
	{
		FoxPluginDefaults.setMode(mode);
	}

	public static void setMessage(string title, string msg)
	{
		FoxPluginDefaults.setMessage(title, msg);
	}

	public static void setOptout(bool optout)
	{
		FoxPluginDefaults.setOptout(optout);
	}

	public static void updateFrom2_10_4g()
	{
		FoxPluginDefaults.updateFrom2_10_4g();
	}

	public static void sendLtv(int cvId)
	{
		FoxPluginDefaults.sendLtv(cvId);
	}

	public static void sendLtv(int cvId, string adId)
	{
		FoxPluginDefaults.sendLtv(cvId, adId);
	}

	public static void addParameter(string name, string val)
	{
		FoxPluginDefaults.addParameter(name, val);
	}

	private void _sendConversion()
	{
		if (this.conversionData.Url == null || this.conversionData.Url.Length == 0)
		{
			FoxPlugin.sendConversion("default");
		}
		else
		{
			FoxPlugin.sendConversion(this.conversionData.Url);
		}
	}

	public static void sendStartSession()
	{
		FoxPluginDefaults.sendStartSession();
	}

	public static void sendEndSession()
	{
		FoxPluginDefaults.sendEndSession();
	}

	public static void sendEvent(string eventName, string action, string label, int value)
	{
		action = FoxPlugin.nvl(action);
		label = FoxPlugin.nvl(label);
		FoxPluginDefaults.sendEvent(eventName, action, label, value);
	}

	public static void sendEventPurchase(string eventName, string action, string label, string orderId, string sku, string itemName, double price, int quantity, string currency)
	{
		eventName = FoxPlugin.nvl(eventName);
		action = FoxPlugin.nvl(action);
		label = FoxPlugin.nvl(label);
		orderId = FoxPlugin.nvl(orderId);
		sku = FoxPlugin.nvl(sku);
		itemName = FoxPlugin.nvl(itemName);
		currency = FoxPlugin.nvl(currency);
		FoxPluginDefaults.sendEventPurchase(eventName, action, label, orderId, sku, itemName, price, quantity, currency);
	}

	public static void sendUserInfo(string userId, string userAttr1, string userAttr2, string userAttr3, string userAttr4, string userAttr5)
	{
		userId = FoxPlugin.nvl(userId);
		userAttr1 = FoxPlugin.nvl(userAttr1);
		userAttr2 = FoxPlugin.nvl(userAttr2);
		userAttr3 = FoxPlugin.nvl(userAttr3);
		userAttr4 = FoxPlugin.nvl(userAttr4);
		userAttr5 = FoxPlugin.nvl(userAttr5);
		FoxPluginDefaults.sendUserInfo(userId, userAttr1, userAttr2, userAttr3, userAttr4, userAttr5);
	}

	public static void registerForRemoteNotifications()
	{
	}

	public static void registerForRemoteNotifications(string senderId)
	{
		FoxPluginDefaults.registerToGCM(senderId);
	}

	public static void checkVersionWithDelegate()
	{
	}

	public static void setListenerGameObject(string listenerGameObject)
	{
	}

	private static string nvl(string str)
	{
		return (str != null) ? str : string.Empty;
	}
}
