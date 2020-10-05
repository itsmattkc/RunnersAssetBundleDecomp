using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class FoxPluginJS : MonoBehaviour
{
	[NonSerialized]
	private static FoxPlugin csScript;

	[NonSerialized]
	public const string PARAM_SKU = "_sku";

	[NonSerialized]
	public const string PARAM_PRICE = "_price";

	[NonSerialized]
	public const string PARAM_OUT = "_out";

	[NonSerialized]
	public const string PARAM_CURRENCY = "_currency";

	public static void sendConversion()
	{
		FoxPlugin.sendConversion();
	}

	public static void sendConversionWithStartPage(object url)
	{
		object arg_1B_0 = url;
		if (!(url is string))
		{
			arg_1B_0 = RuntimeServices.Coerce(url, typeof(string));
		}
		FoxPlugin.sendConversion((string)arg_1B_0);
	}

	public static void sendConversionWithStartPageOnConsent(object url)
	{
		object arg_1B_0 = url;
		if (!(url is string))
		{
			arg_1B_0 = RuntimeServices.Coerce(url, typeof(string));
		}
		FoxPlugin.sendConversionOnconsent((string)arg_1B_0);
	}

	public static void setMode(object mode)
	{
		object arg_1B_0 = mode;
		if (!(mode is string))
		{
			arg_1B_0 = RuntimeServices.Coerce(mode, typeof(string));
		}
		FoxPlugin.setMode((string)arg_1B_0);
	}

	public static void setMessage(object title, object msg)
	{
		object arg_1B_0 = title;
		if (!(title is string))
		{
			arg_1B_0 = RuntimeServices.Coerce(title, typeof(string));
		}
		string arg_40_0 = (string)arg_1B_0;
		object arg_3B_0 = msg;
		if (!(msg is string))
		{
			arg_3B_0 = RuntimeServices.Coerce(msg, typeof(string));
		}
		FoxPlugin.setMessage(arg_40_0, (string)arg_3B_0);
	}

	public static void setOptout(object optout)
	{
		FoxPlugin.setOptout(RuntimeServices.UnboxBoolean(optout));
	}

	public static void updateFrom2_10_4g()
	{
		FoxPlugin.updateFrom2_10_4g();
	}

	public static void sendLtv(object cvId)
	{
		FoxPlugin.sendLtv(RuntimeServices.UnboxInt32(cvId));
	}

	public static void sendLtv(object cvId, object adId)
	{
		int arg_26_0 = RuntimeServices.UnboxInt32(cvId);
		object arg_21_0 = adId;
		if (!(adId is string))
		{
			arg_21_0 = RuntimeServices.Coerce(adId, typeof(string));
		}
		FoxPlugin.sendLtv(arg_26_0, (string)arg_21_0);
	}

	public static void addParameter(object name, object val)
	{
		object arg_1B_0 = name;
		if (!(name is string))
		{
			arg_1B_0 = RuntimeServices.Coerce(name, typeof(string));
		}
		string arg_40_0 = (string)arg_1B_0;
		object arg_3B_0 = val;
		if (!(val is string))
		{
			arg_3B_0 = RuntimeServices.Coerce(val, typeof(string));
		}
		FoxPlugin.addParameter(arg_40_0, (string)arg_3B_0);
	}

	public static void sendStartSession()
	{
		FoxPlugin.sendStartSession();
	}

	public static void sendEndSession()
	{
		FoxPlugin.sendEndSession();
	}

	public static void sendEvent(object eventName, object action, object label, object value)
	{
		object arg_1B_0 = eventName;
		if (!(eventName is string))
		{
			arg_1B_0 = RuntimeServices.Coerce(eventName, typeof(string));
		}
		string arg_66_0 = (string)arg_1B_0;
		object arg_3B_0 = action;
		if (!(action is string))
		{
			arg_3B_0 = RuntimeServices.Coerce(action, typeof(string));
		}
		string arg_66_1 = (string)arg_3B_0;
		object arg_5B_0 = label;
		if (!(label is string))
		{
			arg_5B_0 = RuntimeServices.Coerce(label, typeof(string));
		}
		FoxPlugin.sendEvent(arg_66_0, arg_66_1, (string)arg_5B_0, RuntimeServices.UnboxInt32(value));
	}

	public static void sendEventPurchase(object eventName, object action, object label, object orderId, object sku, object itemName, object price, object quantity, object currency)
	{
		object arg_1B_0 = eventName;
		if (!(eventName is string))
		{
			arg_1B_0 = RuntimeServices.Coerce(eventName, typeof(string));
		}
		string arg_100_0 = (string)arg_1B_0;
		object arg_3B_0 = action;
		if (!(action is string))
		{
			arg_3B_0 = RuntimeServices.Coerce(action, typeof(string));
		}
		string arg_100_1 = (string)arg_3B_0;
		object arg_5B_0 = label;
		if (!(label is string))
		{
			arg_5B_0 = RuntimeServices.Coerce(label, typeof(string));
		}
		string arg_100_2 = (string)arg_5B_0;
		object arg_7B_0 = orderId;
		if (!(orderId is string))
		{
			arg_7B_0 = RuntimeServices.Coerce(orderId, typeof(string));
		}
		string arg_100_3 = (string)arg_7B_0;
		object arg_9F_0 = sku;
		if (!(sku is string))
		{
			arg_9F_0 = RuntimeServices.Coerce(sku, typeof(string));
		}
		string arg_100_4 = (string)arg_9F_0;
		object arg_C3_0 = itemName;
		if (!(itemName is string))
		{
			arg_C3_0 = RuntimeServices.Coerce(itemName, typeof(string));
		}
		string arg_100_5 = (string)arg_C3_0;
		double arg_100_6 = RuntimeServices.UnboxDouble(price);
		int arg_100_7 = RuntimeServices.UnboxInt32(quantity);
		object arg_FB_0 = currency;
		if (!(currency is string))
		{
			arg_FB_0 = RuntimeServices.Coerce(currency, typeof(string));
		}
		FoxPlugin.sendEventPurchase(arg_100_0, arg_100_1, arg_100_2, arg_100_3, arg_100_4, arg_100_5, arg_100_6, arg_100_7, (string)arg_FB_0);
	}

	public static void sendUserInfo(object userId, object userAttr1, object userAttr2, object userAttr3, object userAttr4, object userAttr5)
	{
		object arg_1B_0 = userId;
		if (!(userId is string))
		{
			arg_1B_0 = RuntimeServices.Coerce(userId, typeof(string));
		}
		string arg_C8_0 = (string)arg_1B_0;
		object arg_3B_0 = userAttr1;
		if (!(userAttr1 is string))
		{
			arg_3B_0 = RuntimeServices.Coerce(userAttr1, typeof(string));
		}
		string arg_C8_1 = (string)arg_3B_0;
		object arg_5B_0 = userAttr2;
		if (!(userAttr2 is string))
		{
			arg_5B_0 = RuntimeServices.Coerce(userAttr2, typeof(string));
		}
		string arg_C8_2 = (string)arg_5B_0;
		object arg_7B_0 = userAttr3;
		if (!(userAttr3 is string))
		{
			arg_7B_0 = RuntimeServices.Coerce(userAttr3, typeof(string));
		}
		string arg_C8_3 = (string)arg_7B_0;
		object arg_9F_0 = userAttr4;
		if (!(userAttr4 is string))
		{
			arg_9F_0 = RuntimeServices.Coerce(userAttr4, typeof(string));
		}
		string arg_C8_4 = (string)arg_9F_0;
		object arg_C3_0 = userAttr5;
		if (!(userAttr5 is string))
		{
			arg_C3_0 = RuntimeServices.Coerce(userAttr5, typeof(string));
		}
		FoxPlugin.sendUserInfo(arg_C8_0, arg_C8_1, arg_C8_2, arg_C8_3, arg_C8_4, (string)arg_C3_0);
	}

	public void Main()
	{
		FoxPluginJS.csScript = (FoxPlugin)this.GetComponent("FoxPlugin");
	}
}
