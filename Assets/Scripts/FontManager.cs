using System;
using UnityEngine;

public class FontManager : MonoBehaviour
{
	private UIFont m_uiFont;

	private bool m_loadedFont;

	private static FontManager instance;

	public static FontManager Instance
	{
		get
		{
			return FontManager.instance;
		}
	}

	public bool IsNecessaryLoadFont()
	{
		return !this.m_loadedFont;
	}

	public void LoadResourceData()
	{
		if (this.IsNecessaryLoadFont())
		{
			GameObject gameObject = Resources.Load(this.GetResourceName()) as GameObject;
			if (gameObject != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, gameObject.transform.localPosition, Quaternion.identity) as GameObject;
				if (gameObject2 != null)
				{
					gameObject2.transform.parent = base.gameObject.transform;
					this.m_uiFont = gameObject2.GetComponent<UIFont>();
					this.m_loadedFont = true;
				}
			}
		}
	}

	protected void Awake()
	{
		this.SetInstance();
	}

	private void Start()
	{
		base.enabled = false;
	}

	private void OnDestroy()
	{
		if (FontManager.instance == this)
		{
			FontManager.instance = null;
		}
	}

	private void SetInstance()
	{
		if (FontManager.instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			FontManager.instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void ReplaceFont()
	{
		if (this.m_uiFont != null)
		{
			UIFont[] array = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
			UIFont[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				UIFont uIFont = array2[i];
				if (uIFont != null && uIFont.name == "UCGothic_26_mix_reference")
				{
					uIFont.replacement = this.m_uiFont;
				}
			}
		}
	}

	private string GetFontName()
	{
		return "UCGothic_26_mix";
	}

	private string GetResourceName()
	{
		return "Prefabs/Font/UCGothic_26_mix";
	}
}
