using System;
using UnityEngine;

public class FoxPluginDefaults
{
	public static void sendConversion()
	{
		UnityEngine.Debug.Log("FOX-SDK sendConversion");
	}

	public static void sendConversion(string url)
	{
		UnityEngine.Debug.Log("FOX-SDK sendConversion " + url);
	}

	public static void sendConversionWithStartPage(string url)
	{
		UnityEngine.Debug.Log("FOX-SDK sendConversionWithStartPage " + url);
	}

	public static void sendConversionWithStartPageOnConsent(string url)
	{
		UnityEngine.Debug.Log("FOX-SDK sendConversionWithStartPageOnConsent " + url);
	}

	public static void sendConversionOnconsent(string url)
	{
		UnityEngine.Debug.Log("FOX-SDK sendConversionOnconsent " + url);
	}

	public static void sendConversion(string url, string buid)
	{
		UnityEngine.Debug.Log("FOX-SDK sendConversion " + url + " " + buid);
	}

	public static void sendConversionWithStartPage(string url, string buid)
	{
		UnityEngine.Debug.Log("FOX-SDK sendConversionWithStartPage " + url + " " + buid);
	}

	public static void sendConversionForMobage(string mbgaAppId)
	{
		UnityEngine.Debug.Log("FOX-SDK sendConversionForMobage " + mbgaAppId);
	}

	public static void sendConversionForMobage(string url, string mbgaAppId)
	{
		UnityEngine.Debug.Log("FOX-SDK sendConversionForMobage " + url + " " + mbgaAppId);
	}

	public static void sendUserIdForMobage(string mbgaUserId)
	{
		UnityEngine.Debug.Log("FOX-SDK sendUserIdForMobage " + mbgaUserId);
	}

	public static void setServerUrl(string serverUrl)
	{
		UnityEngine.Debug.Log("FOX-SDK setServerUrl " + serverUrl);
	}

	public static void setMode(string mode)
	{
		UnityEngine.Debug.Log("FOX-SDK setMode " + mode);
	}

	public static void setMessage(string title, string msg)
	{
		UnityEngine.Debug.Log("FOX-SDK setMessage " + msg);
	}

	public static void setOptout(bool optout)
	{
		UnityEngine.Debug.Log("FOX-SDK setOptout " + optout.ToString());
	}

	public static void updateFrom2_10_4g()
	{
		UnityEngine.Debug.Log("FOX-SDK updateFrom2_10_4g ");
	}

	public static void sendLtv(int cvId)
	{
		UnityEngine.Debug.Log("FOX-SDK sendLtv " + cvId);
	}

	public static void sendLtv(int cvId, string adId)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"FOX-SDK sendLtv ",
			cvId,
			" ",
			adId
		}));
	}

	public static void addParameter(string name, string val)
	{
		UnityEngine.Debug.Log("FOX-SDK addParameter " + name + " " + val);
	}

	public static void sendStartSession()
	{
		UnityEngine.Debug.Log("FOX-LIB sendStartSession");
	}

	public static void sendEndSession()
	{
		UnityEngine.Debug.Log("FOX-LIB sendEndSession");
	}

	public static void sendEvent(string eventName, string action, string label, int value)
	{
		UnityEngine.Debug.Log("FOX-LIB sendEvent");
	}

	public static void sendEventPurchase(string eventName, string action, string label, string orderId, string sku, string itemName, double price, int quantity, string currency)
	{
		UnityEngine.Debug.Log("FOX-LIB sendEventPurchase");
	}

	public static void sendUserInfo(string userId, string userAttr1, string userAttr2, string userAttr3, string userAttr4, string userAttr5)
	{
		UnityEngine.Debug.Log("FOX-LIB sendUserInfo");
	}

	public static void registerForRemoteNotifications()
	{
		UnityEngine.Debug.Log("this is default");
	}

	public static void registerToGCM(string senderId)
	{
		UnityEngine.Debug.Log("this is default");
	}

	public static void checkVersionWithDelegate()
	{
		UnityEngine.Debug.Log("this is default");
	}

	public static void setListenerGameObject(string gameObject)
	{
		UnityEngine.Debug.Log("this is default");
	}
}
