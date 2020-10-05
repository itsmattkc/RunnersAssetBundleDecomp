using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Language Selection"), RequireComponent(typeof(UIPopupList))]
public class LanguageSelection : MonoBehaviour
{
	private UIPopupList mList;

	private void Start()
	{
		this.mList = base.GetComponent<UIPopupList>();
		if (Localization.instance != null && Localization.instance.languages != null && Localization.instance.languages.Length > 0)
		{
			this.mList.items.Clear();
			int i = 0;
			int num = Localization.instance.languages.Length;
			while (i < num)
			{
				TextAsset textAsset = Localization.instance.languages[i];
				if (textAsset != null)
				{
					this.mList.items.Add(textAsset.name);
				}
				i++;
			}
			this.mList.value = Localization.instance.currentLanguage;
		}
		EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.OnChange));
	}

	private void OnChange()
	{
		if (Localization.instance != null)
		{
			Localization.instance.currentLanguage = UIPopupList.current.value;
		}
	}
}
