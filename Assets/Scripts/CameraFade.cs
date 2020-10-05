using System;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
	private static CameraFade mInstance;

	public GUIStyle m_BackgroundStyle = new GUIStyle();

	public Texture2D m_FadeTexture;

	public Color m_CurrentScreenOverlayColor = new Color(0f, 0f, 0f, 0f);

	public Color m_TargetScreenOverlayColor = new Color(0f, 0f, 0f, 0f);

	public Color m_DeltaColor = new Color(0f, 0f, 0f, 0f);

	public int m_FadeGUIDepth = -1000;

	public float m_FadeDelay;

	public Action m_OnFadeFinish;

	private static CameraFade instance
	{
		get
		{
			if (CameraFade.mInstance == null)
			{
				GameObject gameObject = new GameObject("CameraFade");
				if (gameObject != null)
				{
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
					CameraFade.mInstance = gameObject.AddComponent<CameraFade>();
				}
			}
			return CameraFade.mInstance;
		}
	}

	private void Awake()
	{
		if (CameraFade.mInstance == null)
		{
			CameraFade.mInstance = this;
			CameraFade.instance.init();
		}
	}

	public void init()
	{
		CameraFade.instance.m_FadeTexture = new Texture2D(1, 1);
		CameraFade.instance.m_BackgroundStyle.normal.background = CameraFade.instance.m_FadeTexture;
	}

	private void OnGUI()
	{
		if (Time.time > CameraFade.instance.m_FadeDelay && CameraFade.instance.m_CurrentScreenOverlayColor != CameraFade.instance.m_TargetScreenOverlayColor)
		{
			if (Mathf.Abs(CameraFade.instance.m_CurrentScreenOverlayColor.a - CameraFade.instance.m_TargetScreenOverlayColor.a) < Mathf.Abs(CameraFade.instance.m_DeltaColor.a) * Time.deltaTime)
			{
				CameraFade.instance.m_CurrentScreenOverlayColor = CameraFade.instance.m_TargetScreenOverlayColor;
				CameraFade.SetScreenOverlayColor(CameraFade.instance.m_CurrentScreenOverlayColor);
				CameraFade.instance.m_DeltaColor = new Color(0f, 0f, 0f, 0f);
				if (CameraFade.instance.m_OnFadeFinish != null)
				{
					CameraFade.instance.m_OnFadeFinish();
				}
			}
			else
			{
				CameraFade.SetScreenOverlayColor(CameraFade.instance.m_CurrentScreenOverlayColor + CameraFade.instance.m_DeltaColor * Time.deltaTime);
			}
		}
		if (this.m_CurrentScreenOverlayColor.a > 0f)
		{
			GUI.depth = CameraFade.instance.m_FadeGUIDepth;
			GUI.Label(new Rect(-20f, -20f, (float)(Screen.width + 20), (float)(Screen.height + 20)), CameraFade.instance.m_FadeTexture, CameraFade.instance.m_BackgroundStyle);
		}
	}

	private static void SetScreenOverlayColor(Color newScreenOverlayColor)
	{
		CameraFade.instance.m_CurrentScreenOverlayColor = newScreenOverlayColor;
		CameraFade.instance.m_FadeTexture.SetPixel(0, 0, CameraFade.instance.m_CurrentScreenOverlayColor);
		CameraFade.instance.m_FadeTexture.Apply();
	}

	public static void StartAlphaFade(Color newScreenOverlayColor, bool isFadeIn, float fadeDuration)
	{
		if (fadeDuration <= 0f)
		{
			CameraFade.SetScreenOverlayColor(newScreenOverlayColor);
		}
		else
		{
			if (isFadeIn)
			{
				CameraFade.instance.m_TargetScreenOverlayColor = new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0f);
				CameraFade.SetScreenOverlayColor(newScreenOverlayColor);
			}
			else
			{
				CameraFade.instance.m_TargetScreenOverlayColor = newScreenOverlayColor;
				CameraFade.SetScreenOverlayColor(new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0f));
			}
			CameraFade.instance.m_DeltaColor = (CameraFade.instance.m_TargetScreenOverlayColor - CameraFade.instance.m_CurrentScreenOverlayColor) / fadeDuration;
		}
	}

	public static void StartAlphaFade(Color nowScreenOverlayColor, Color newScreenOverlayColor, bool isFadeIn, float fadeDuration)
	{
		if (fadeDuration <= 0f)
		{
			CameraFade.SetScreenOverlayColor(newScreenOverlayColor);
		}
		else
		{
			if (isFadeIn)
			{
				CameraFade.instance.m_TargetScreenOverlayColor = new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0f);
				CameraFade.SetScreenOverlayColor(nowScreenOverlayColor);
			}
			else
			{
				CameraFade.instance.m_TargetScreenOverlayColor = newScreenOverlayColor;
				CameraFade.SetScreenOverlayColor(new Color(nowScreenOverlayColor.r, nowScreenOverlayColor.g, nowScreenOverlayColor.b, 0f));
			}
			CameraFade.instance.m_DeltaColor = (CameraFade.instance.m_TargetScreenOverlayColor - CameraFade.instance.m_CurrentScreenOverlayColor) / fadeDuration;
		}
	}

	public static void StartAlphaFade(Color newScreenOverlayColor, bool isFadeIn, float fadeDuration, float fadeDelay)
	{
		if (fadeDuration <= 0f)
		{
			CameraFade.SetScreenOverlayColor(newScreenOverlayColor);
		}
		else
		{
			CameraFade.instance.m_FadeDelay = Time.time + fadeDelay;
			if (isFadeIn)
			{
				CameraFade.instance.m_TargetScreenOverlayColor = new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0f);
				CameraFade.SetScreenOverlayColor(newScreenOverlayColor);
			}
			else
			{
				CameraFade.instance.m_TargetScreenOverlayColor = newScreenOverlayColor;
				CameraFade.SetScreenOverlayColor(new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0f));
			}
			CameraFade.instance.m_DeltaColor = (CameraFade.instance.m_TargetScreenOverlayColor - CameraFade.instance.m_CurrentScreenOverlayColor) / fadeDuration;
		}
	}

	public static void StartAlphaFade(Color newScreenOverlayColor, bool isFadeIn, float fadeDuration, float fadeDelay, Action OnFadeFinish)
	{
		if (fadeDuration <= 0f)
		{
			CameraFade.SetScreenOverlayColor(newScreenOverlayColor);
		}
		else
		{
			CameraFade.instance.m_OnFadeFinish = OnFadeFinish;
			CameraFade.instance.m_FadeDelay = Time.time + fadeDelay;
			if (isFadeIn)
			{
				CameraFade.instance.m_TargetScreenOverlayColor = new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0f);
				CameraFade.SetScreenOverlayColor(newScreenOverlayColor);
			}
			else
			{
				CameraFade.instance.m_TargetScreenOverlayColor = newScreenOverlayColor;
				CameraFade.SetScreenOverlayColor(new Color(newScreenOverlayColor.r, newScreenOverlayColor.g, newScreenOverlayColor.b, 0f));
			}
			CameraFade.instance.m_DeltaColor = (CameraFade.instance.m_TargetScreenOverlayColor - CameraFade.instance.m_CurrentScreenOverlayColor) / fadeDuration;
		}
	}

	private void OnApplicationQuit()
	{
		CameraFade.mInstance = null;
	}
}
