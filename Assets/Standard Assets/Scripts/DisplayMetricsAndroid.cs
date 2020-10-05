using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DisplayMetricsAndroid
{
	private static float _Density_k__BackingField;

	private static int _DensityDPI_k__BackingField;

	private static int _HeightPixels_k__BackingField;

	private static int _WidthPixels_k__BackingField;

	private static float _ScaledDensity_k__BackingField;

	private static float _XDPI_k__BackingField;

	private static float _YDPI_k__BackingField;

	public static float Density
	{
		get;
		protected set;
	}

	public static int DensityDPI
	{
		get;
		protected set;
	}

	public static int HeightPixels
	{
		get;
		protected set;
	}

	public static int WidthPixels
	{
		get;
		protected set;
	}

	public static float ScaledDensity
	{
		get;
		protected set;
	}

	public static float XDPI
	{
		get;
		protected set;
	}

	public static float YDPI
	{
		get;
		protected set;
	}

	static DisplayMetricsAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (new AndroidJavaClass("android.util.DisplayMetrics"))
			{
				using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.util.DisplayMetrics", new object[0]))
				{
					using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						using (AndroidJavaObject androidJavaObject2 = @static.Call<AndroidJavaObject>("getWindowManager", new object[0]))
						{
							using (AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getDefaultDisplay", new object[0]))
							{
								androidJavaObject3.Call("getMetrics", new object[]
								{
									androidJavaObject
								});
								DisplayMetricsAndroid.Density = androidJavaObject.Get<float>("density");
								DisplayMetricsAndroid.DensityDPI = androidJavaObject.Get<int>("densityDpi");
								DisplayMetricsAndroid.HeightPixels = androidJavaObject.Get<int>("heightPixels");
								DisplayMetricsAndroid.WidthPixels = androidJavaObject.Get<int>("widthPixels");
								DisplayMetricsAndroid.ScaledDensity = androidJavaObject.Get<float>("scaledDensity");
								DisplayMetricsAndroid.XDPI = androidJavaObject.Get<float>("xdpi");
								DisplayMetricsAndroid.YDPI = androidJavaObject.Get<float>("ydpi");
							}
						}
					}
				}
			}
		}
	}
}
