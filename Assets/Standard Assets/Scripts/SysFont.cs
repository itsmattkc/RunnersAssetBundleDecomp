using System;
using UnityEngine;

public class SysFont : MonoBehaviour
{
	public enum Alignment
	{
		Left,
		Center,
		Right
	}

	private static AndroidJavaObject _unitySysFontInstance;

	private static AndroidJavaObject UnitySysFontInstance
	{
		get
		{
			if (SysFont._unitySysFontInstance == null)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.github.imkira.unitysysfont.UnitySysFont");
				SysFont._unitySysFontInstance = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			}
			return SysFont._unitySysFontInstance;
		}
	}

	private static void _SysFontQueueTexture(string text, string fontName, int fontSize, bool isBold, bool isItalic, SysFont.Alignment alignment, int maxWidthPixels, int maxHeightPixels, int textureID)
	{
		SysFont.UnitySysFontInstance.Call("queueTexture", new object[]
		{
			text,
			fontName,
			fontSize,
			isBold,
			isItalic,
			(int)alignment,
			maxWidthPixels,
			maxHeightPixels,
			textureID
		});
	}

	private static void _SysFontUpdateQueuedTexture(int textureID)
	{
		SysFont.UnitySysFontInstance.Call("updateQueuedTexture", new object[]
		{
			textureID
		});
	}

	private static void _SysFontDequeueTexture(int textureID)
	{
		SysFont.UnitySysFontInstance.Call("dequeueTexture", new object[]
		{
			textureID
		});
	}

	private static int _SysFontGetTextureWidth(int textureID)
	{
		return SysFont.UnitySysFontInstance.Call<int>("getTextureWidth", new object[]
		{
			textureID
		});
	}

	private static int _SysFontGetTextureHeight(int textureID)
	{
		return SysFont.UnitySysFontInstance.Call<int>("getTextureHeight", new object[]
		{
			textureID
		});
	}

	private static int _SysFontGetTextWidth(int textureID)
	{
		return SysFont.UnitySysFontInstance.Call<int>("getTextWidth", new object[]
		{
			textureID
		});
	}

	private static int _SysFontGetTextHeight(int textureID)
	{
		return SysFont.UnitySysFontInstance.Call<int>("getTextHeight", new object[]
		{
			textureID
		});
	}

	private static void _SysFontRender()
	{
		SysFont.UnitySysFontInstance.Call("processQueue", new object[0]);
	}

	public static int GetTextureWidth(int textureID)
	{
		return Mathf.Max(SysFont._SysFontGetTextureWidth(textureID), 1);
	}

	public static int GetTextureHeight(int textureID)
	{
		return Mathf.Max(SysFont._SysFontGetTextureHeight(textureID), 1);
	}

	public static int GetTextWidth(int textureID)
	{
		return Mathf.Max(SysFont._SysFontGetTextWidth(textureID), 1);
	}

	public static int GetTextHeight(int textureID)
	{
		return Mathf.Max(SysFont._SysFontGetTextHeight(textureID), 1);
	}

	public static void QueueTexture(string text, string fontName, int fontSize, bool isBold, bool isItalic, SysFont.Alignment alignment, bool isMultiLine, int maxWidthPixels, int maxHeightPixels, int textureID)
	{
		if (!isMultiLine)
		{
			text = text.Replace("\r\n", string.Empty).Replace("\n", string.Empty);
		}
		SysFont._SysFontQueueTexture(text, fontName, fontSize, isBold, isItalic, alignment, maxWidthPixels, maxHeightPixels, textureID);
	}

	public static void UpdateQueuedTexture(int textureID)
	{
		SysFont._SysFontUpdateQueuedTexture(textureID);
		SysFont._SysFontRender();
	}

	public static void DequeueTexture(int textureID)
	{
		SysFont._SysFontDequeueTexture(textureID);
	}

	public static void SafeDestroy(UnityEngine.Object obj)
	{
		if (obj != null)
		{
			if (Application.isEditor)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			else
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}
}
