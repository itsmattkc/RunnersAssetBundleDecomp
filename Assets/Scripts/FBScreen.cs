using System;
using System.Collections.Generic;
using UnityEngine;

public class FBScreen
{
	public class Layout
	{
		public class OptionLeft : FBScreen.Layout
		{
			public float Amount;
		}

		public class OptionTop : FBScreen.Layout
		{
			public float Amount;
		}

		public class OptionCenterHorizontal : FBScreen.Layout
		{
		}

		public class OptionCenterVertical : FBScreen.Layout
		{
		}
	}

	private static bool resizable;

	public static bool FullScreen
	{
		get
		{
			return Screen.fullScreen;
		}
		set
		{
			Screen.fullScreen = value;
		}
	}

	public static bool Resizable
	{
		get
		{
			return FBScreen.resizable;
		}
	}

	public static int Width
	{
		get
		{
			return Screen.width;
		}
	}

	public static int Height
	{
		get
		{
			return Screen.height;
		}
	}

	public static void SetResolution(int width, int height, bool fullscreen, int preferredRefreshRate = 0, params FBScreen.Layout[] layoutParams)
	{
		Screen.SetResolution(width, height, fullscreen, preferredRefreshRate);
	}

	public static void SetAspectRatio(int width, int height, params FBScreen.Layout[] layoutParams)
	{
		int width2 = Screen.height / height * width;
		Screen.SetResolution(width2, Screen.height, Screen.fullScreen);
	}

	public static void SetUnityPlayerEmbedCSS(string key, string value)
	{
	}

	public static FBScreen.Layout.OptionLeft Left(float amount)
	{
		return new FBScreen.Layout.OptionLeft
		{
			Amount = amount
		};
	}

	public static FBScreen.Layout.OptionTop Top(float amount)
	{
		return new FBScreen.Layout.OptionTop
		{
			Amount = amount
		};
	}

	public static FBScreen.Layout.OptionCenterHorizontal CenterHorizontal()
	{
		return new FBScreen.Layout.OptionCenterHorizontal();
	}

	public static FBScreen.Layout.OptionCenterVertical CenterVertical()
	{
		return new FBScreen.Layout.OptionCenterVertical();
	}

	private static void SetLayout(IEnumerable<FBScreen.Layout> parameters)
	{
	}
}
