using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Localize"), RequireComponent(typeof(UIWidget))]
public class UILocalize : MonoBehaviour
{
	public string key;

	private string mLanguage;

	private bool mStarted;

	private void OnLocalize(Localization loc)
	{
		if (this.mLanguage != loc.currentLanguage)
		{
			this.Localize();
		}
	}

	private void OnEnable()
	{
		if (this.mStarted && Localization.instance != null)
		{
			this.Localize();
		}
	}

	private void Start()
	{
		this.mStarted = true;
		if (Localization.instance != null)
		{
			this.Localize();
		}
	}

	public void Localize()
	{
		Localization instance = Localization.instance;
		UIWidget component = base.GetComponent<UIWidget>();
		UILabel uILabel = component as UILabel;
		UISprite uISprite = component as UISprite;
		if (string.IsNullOrEmpty(this.mLanguage) && string.IsNullOrEmpty(this.key) && uILabel != null)
		{
			this.key = uILabel.text;
		}
		string text = (!string.IsNullOrEmpty(this.key)) ? instance.Get(this.key) : string.Empty;
		if (uILabel != null)
		{
			UIInput uIInput = NGUITools.FindInParents<UIInput>(uILabel.gameObject);
			if (uIInput != null && uIInput.label == uILabel)
			{
				uIInput.defaultText = text;
			}
			else
			{
				uILabel.text = text;
			}
		}
		else if (uISprite != null)
		{
			uISprite.spriteName = text;
			uISprite.MakePixelPerfect();
		}
		this.mLanguage = instance.currentLanguage;
	}
}
