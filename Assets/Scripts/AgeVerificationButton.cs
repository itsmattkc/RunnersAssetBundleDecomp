using System;
using System.Collections.Generic;
using UnityEngine;

public class AgeVerificationButton : MonoBehaviour
{
	public enum LabelType
	{
		TYPE_NONE = -1,
		TYPE_ONE,
		TYPE_TEN,
		TYPE_COUNT
	}

	public delegate void ButtonClickedCallback();

	private AgeVerificationButton.ButtonClickedCallback m_callback;

	private AgeVerificationButton.LabelType m_labelType;

	private UILabel m_label;

	private GameObject m_upObject;

	private GameObject m_downObject;

	private int m_currentIndex;

	private List<int> m_valuePreset = new List<int>();

	private bool m_noInput = true;

	public int CurrentValue
	{
		get
		{
			if (this.m_valuePreset != null)
			{
				return this.m_valuePreset[this.m_currentIndex];
			}
			return -1;
		}
		private set
		{
		}
	}

	public bool NoInput
	{
		get
		{
			return this.m_noInput;
		}
		private set
		{
		}
	}

	public void SetLabel(AgeVerificationButton.LabelType labelType, UILabel label)
	{
		this.m_labelType = labelType;
		this.m_label = label;
	}

	public void SetButton(GameObject upObject, GameObject downObject)
	{
		if (upObject == null)
		{
			return;
		}
		if (downObject == null)
		{
			return;
		}
		this.m_upObject = upObject;
		this.m_downObject = downObject;
		UIButtonMessage uIButtonMessage = this.m_upObject.GetComponent<UIButtonMessage>();
		if (uIButtonMessage == null)
		{
			uIButtonMessage = this.m_upObject.AddComponent<UIButtonMessage>();
		}
		uIButtonMessage.target = base.gameObject;
		uIButtonMessage.functionName = "ButtonMessageUpClicked";
		UIButtonMessage uIButtonMessage2 = this.m_downObject.GetComponent<UIButtonMessage>();
		if (uIButtonMessage2 == null)
		{
			uIButtonMessage2 = this.m_downObject.AddComponent<UIButtonMessage>();
		}
		uIButtonMessage2.target = base.gameObject;
		uIButtonMessage2.functionName = "ButtonMessageDownClicked";
	}

	public void Setup(AgeVerificationButton.ButtonClickedCallback callback)
	{
		this.m_callback = callback;
	}

	public void AddValuePreset(int value)
	{
		this.m_valuePreset.Add(value);
	}

	public void SetDefaultValue(int value)
	{
		for (int i = 0; i < this.m_valuePreset.Count; i++)
		{
			if (value == this.m_valuePreset[i])
			{
				this.m_currentIndex = i;
				this.SetValue(this.m_valuePreset[i]);
			}
		}
	}

	private void SetValue(int value)
	{
		string str = string.Empty;
		string str2 = string.Empty;
		if (this.m_noInput)
		{
			str = string.Empty;
			str2 = "-";
		}
		else
		{
			if (this.m_labelType == AgeVerificationButton.LabelType.TYPE_TEN)
			{
				str = (value / 10).ToString();
			}
			str2 = (value % 10).ToString();
		}
		if (this.m_label != null)
		{
			string text = str + str2;
			this.m_label.text = text;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void ButtonMessageUpClicked()
	{
		if (this.m_noInput)
		{
			this.m_noInput = false;
		}
		else
		{
			this.m_currentIndex++;
			if (this.m_currentIndex >= this.m_valuePreset.Count)
			{
				this.m_currentIndex = 0;
			}
		}
		int value = this.m_valuePreset[this.m_currentIndex];
		this.SetValue(value);
		this.m_callback();
	}

	private void ButtonMessageDownClicked()
	{
		if (this.m_noInput)
		{
			this.m_noInput = false;
		}
		else
		{
			this.m_currentIndex--;
			if (this.m_currentIndex < 0)
			{
				this.m_currentIndex = this.m_valuePreset.Count - 1;
			}
		}
		int value = this.m_valuePreset[this.m_currentIndex];
		this.SetValue(value);
		this.m_callback();
	}
}
