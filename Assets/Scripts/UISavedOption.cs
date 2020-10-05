using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
	public string keyName;

	private UIPopupList mList;

	private UIToggle mCheck;

	private string key
	{
		get
		{
			return (!string.IsNullOrEmpty(this.keyName)) ? this.keyName : ("NGUI State: " + base.name);
		}
	}

	private void Awake()
	{
		this.mList = base.GetComponent<UIPopupList>();
		this.mCheck = base.GetComponent<UIToggle>();
	}

	private void OnEnable()
	{
		if (this.mList != null)
		{
			EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
		}
		if (this.mCheck != null)
		{
			EventDelegate.Add(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
		}
		if (this.mList != null)
		{
			string @string = PlayerPrefs.GetString(this.key);
			if (!string.IsNullOrEmpty(@string))
			{
				this.mList.value = @string;
			}
			return;
		}
		if (this.mCheck != null)
		{
			this.mCheck.value = (PlayerPrefs.GetInt(this.key, 1) != 0);
		}
		else
		{
			string string2 = PlayerPrefs.GetString(this.key);
			UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				UIToggle uIToggle = componentsInChildren[i];
				uIToggle.value = (uIToggle.name == string2);
				i++;
			}
		}
	}

	private void OnDisable()
	{
		if (this.mCheck != null)
		{
			EventDelegate.Remove(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
		}
		if (this.mList != null)
		{
			EventDelegate.Remove(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
		}
		if (this.mCheck == null && this.mList == null)
		{
			UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				UIToggle uIToggle = componentsInChildren[i];
				if (uIToggle.value)
				{
					PlayerPrefs.SetString(this.key, uIToggle.name);
					break;
				}
				i++;
			}
		}
	}

	private void SaveSelection()
	{
		PlayerPrefs.SetString(this.key, UIPopupList.current.value);
	}

	private void SaveState()
	{
		PlayerPrefs.SetInt(this.key, (!UIToggle.current.value) ? 0 : 1);
	}
}
