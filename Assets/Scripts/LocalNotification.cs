using SaveData;
using System;
using UnityEngine;

public class LocalNotification
{
	private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	public static void Initialize()
	{
		LocalNotification.CancelAllNotifications();
	}

	public static void OnActive()
	{
		LocalNotification.ClearRecieveNotifications();
	}

	private static void ClearRecieveNotifications()
	{
	}

	public static void EnableNotification(bool value)
	{
		if (value)
		{
			PnoteNotification.RequestRegister();
		}
		else
		{
			PnoteNotification.RequestUnregister();
		}
	}

	public static void RegisterNotification(float second, string message)
	{
		if (SystemSaveManager.Instance != null && !SystemSaveManager.Instance.GetSystemdata().pushNotice)
		{
			return;
		}
		DateTime dateTime = DateTime.Now.AddSeconds((double)second);
		long num = LocalNotification.ToUnixTime(dateTime);
		AndroidJavaObject androidJavaObject = new AndroidJavaObject(BindingAndroid.GetPackageName() + ".gcm.GCMManager", new object[0]);
		androidJavaObject.Call("registLocalNotification", new object[]
		{
			num,
			message
		});
	}

	public static void CancelAllNotifications()
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject(BindingAndroid.GetPackageName() + ".gcm.GCMManager", new object[0]);
		androidJavaObject.Call("clearAllNotification", new object[0]);
	}

	private static long ToUnixTime(DateTime dateTime)
	{
		dateTime = dateTime.ToUniversalTime();
		return (long)dateTime.Subtract(LocalNotification.UnixEpoch).TotalMilliseconds;
	}
}
