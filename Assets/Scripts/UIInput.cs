using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Input Field"), ExecuteInEditMode]
public class UIInput : UIWidgetContainer
{
	public enum KeyboardType
	{
		Default,
		ASCIICapable,
		NumbersAndPunctuation,
		URL,
		NumberPad,
		PhonePad,
		NamePhonePad,
		EmailAddress
	}

	public delegate char Validator(string currentText, char nextChar);

	public static UIInput current;

	public UILabel label;

	public int maxChars;

	public string caratChar = "|";

	public string playerPrefsField;

	public UIInput.Validator validator;

	public UIInput.KeyboardType type;

	public bool isPassword;

	public bool autoCorrect;

	public bool useLabelTextAtStart;

	public Color activeColor = Color.white;

	public GameObject selectOnTab;

	public List<EventDelegate> onSubmit = new List<EventDelegate>();

	[HideInInspector, SerializeField]
	private GameObject eventReceiver;

	[HideInInspector, SerializeField]
	private string functionName = "OnSubmit";

	private string mText = string.Empty;

	private string mDefaultText = string.Empty;

	private Color mDefaultColor = Color.white;

	private UIWidget.Pivot mPivot = UIWidget.Pivot.Left;

	private float mPosition;

	private TouchScreenKeyboard mKeyboard;

	private bool mDoInit = true;

	[Obsolete("Use UIInput.value instead")]
	public string text
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	public string value
	{
		get
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			return this.mText;
		}
		set
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			if (this.mText != value)
			{
				this.mText = value;
				this.SaveToPlayerPrefs(this.mText);
			}
			if (this.mKeyboard != null)
			{
				this.mKeyboard.text = value;
			}
			if (this.label != null)
			{
				if (string.IsNullOrEmpty(value))
				{
					value = this.mDefaultText;
				}
				this.label.supportEncoding = false;
				this.label.text = ((!this.selected) ? value : (value + this.caratChar));
				this.label.showLastPasswordChar = this.selected;
				this.label.color = ((!this.selected && !(value != this.mDefaultText)) ? this.mDefaultColor : this.activeColor);
			}
		}
	}

	public bool selected
	{
		get
		{
			return UICamera.selectedObject == base.gameObject;
		}
		set
		{
			if (!value && UICamera.selectedObject == base.gameObject)
			{
				UICamera.selectedObject = null;
			}
			else if (value)
			{
				UICamera.selectedObject = base.gameObject;
			}
		}
	}

	public string defaultText
	{
		get
		{
			return this.mDefaultText;
		}
		set
		{
			if (this.label.text == this.mDefaultText)
			{
				this.label.text = value;
			}
			this.mDefaultText = value;
		}
	}

	protected void Init()
	{
		if (this.mDoInit)
		{
			this.mDoInit = false;
			if (this.label == null)
			{
				this.label = base.GetComponentInChildren<UILabel>();
			}
			if (this.label != null)
			{
				if (this.useLabelTextAtStart)
				{
					this.mText = this.label.text;
				}
				this.mDefaultText = this.label.text;
				this.mDefaultColor = this.label.color;
				this.label.supportEncoding = false;
				this.label.password = this.isPassword;
				this.label.maxLineCount = 1;
				this.mPivot = this.label.pivot;
				this.mPosition = this.label.cachedTransform.localPosition.x;
			}
			else
			{
				base.enabled = false;
			}
		}
	}

	private void SaveToPlayerPrefs(string val)
	{
		if (!string.IsNullOrEmpty(this.playerPrefsField))
		{
			if (string.IsNullOrEmpty(val))
			{
				PlayerPrefs.DeleteKey(this.playerPrefsField);
			}
			else
			{
				PlayerPrefs.SetString(this.playerPrefsField, val);
			}
		}
	}

	private void Awake()
	{
		if (this.label == null)
		{
			this.label = base.GetComponentInChildren<UILabel>();
		}
		if (!string.IsNullOrEmpty(this.playerPrefsField) && PlayerPrefs.HasKey(this.playerPrefsField))
		{
			this.value = PlayerPrefs.GetString(this.playerPrefsField);
		}
	}

	private void Start()
	{
		if (EventDelegate.IsValid(this.onSubmit))
		{
			if (this.eventReceiver != null || !string.IsNullOrEmpty(this.functionName))
			{
				this.eventReceiver = null;
				this.functionName = null;
			}
		}
		else if (this.eventReceiver == null && !EventDelegate.IsValid(this.onSubmit))
		{
			this.eventReceiver = base.gameObject;
		}
	}

	private void OnEnable()
	{
		if (UICamera.IsHighlighted(base.gameObject))
		{
			this.OnSelect(true);
		}
	}

	private void OnDisable()
	{
		if (UICamera.IsHighlighted(base.gameObject))
		{
			this.OnSelect(false);
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.label != null && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (isSelected)
			{
				this.mText = ((this.useLabelTextAtStart || !(this.label.text == this.mDefaultText)) ? this.label.text : string.Empty);
				this.label.color = this.activeColor;
				if (this.isPassword)
				{
					this.label.password = true;
				}
				if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
				{
					if (this.isPassword)
					{
						this.mKeyboard = TouchScreenKeyboard.Open(this.mText, TouchScreenKeyboardType.Default, false, false, true);
					}
					else
					{
						this.mKeyboard = TouchScreenKeyboard.Open(this.mText, (TouchScreenKeyboardType)this.type, this.autoCorrect);
					}
				}
				else
				{
					Input.imeCompositionMode = IMECompositionMode.On;
					Input.compositionCursorPos = UICamera.currentCamera.WorldToScreenPoint(this.label.worldCorners[0]);
				}
				this.UpdateLabel();
			}
			else
			{
				if (this.mKeyboard != null)
				{
					this.mKeyboard.active = false;
				}
				if (string.IsNullOrEmpty(this.mText))
				{
					this.label.text = this.mDefaultText;
					this.label.color = this.mDefaultColor;
					if (this.isPassword)
					{
						this.label.password = false;
					}
				}
				else
				{
					this.label.text = this.mText;
				}
				this.label.showLastPasswordChar = false;
				Input.imeCompositionMode = IMECompositionMode.Off;
				this.RestoreLabel();
			}
		}
	}

	private void Update()
	{
		if (this.mKeyboard != null)
		{
			string text = this.mKeyboard.text;
			if (this.mText != text)
			{
				this.mText = string.Empty;
				for (int i = 0; i < text.Length; i++)
				{
					char c = text[i];
					if (this.validator != null)
					{
						c = this.validator(this.mText, c);
					}
					if (c != '\0')
					{
						this.mText += c;
					}
				}
				if (this.maxChars > 0 && this.mText.Length > this.maxChars)
				{
					this.mText = this.mText.Substring(0, this.maxChars);
				}
				this.UpdateLabel();
				if (this.mText != text)
				{
					this.mKeyboard.text = this.mText;
				}
				base.SendMessage("OnInputChanged", this, SendMessageOptions.DontRequireReceiver);
			}
			if (this.mKeyboard.done)
			{
				this.mKeyboard = null;
				this.Submit();
				this.selected = false;
			}
		}
	}

	private void OnInput(string input)
	{
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.selected && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				return;
			}
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return;
			}
			this.Append(input);
		}
	}

	private void Submit()
	{
		UIInput.current = this;
		if (EventDelegate.IsValid(this.onSubmit))
		{
			EventDelegate.Execute(this.onSubmit);
		}
		else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
		{
			this.eventReceiver.SendMessage(this.functionName, this.mText, SendMessageOptions.DontRequireReceiver);
		}
		this.SaveToPlayerPrefs(this.mText);
		UIInput.current = null;
	}

	private void Append(string input)
	{
		int i = 0;
		int length = input.Length;
		while (i < length)
		{
			char c = input[i];
			if (c == '\b')
			{
				if (this.mText.Length > 0)
				{
					this.mText = this.mText.Substring(0, this.mText.Length - 1);
					base.SendMessage("OnInputChanged", this, SendMessageOptions.DontRequireReceiver);
				}
			}
			else if (c == '\r' || c == '\n')
			{
				if ((UICamera.current.submitKey0 == KeyCode.Return || UICamera.current.submitKey1 == KeyCode.Return) && (!this.label.multiLine || (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
				{
					this.Submit();
					this.selected = false;
					return;
				}
				if (this.validator != null)
				{
					c = this.validator(this.mText, c);
				}
				if (c != '\0')
				{
					if (c == '\n' || c == '\r')
					{
						if (this.label.multiLine)
						{
							this.mText += "\n";
						}
					}
					else
					{
						this.mText += c;
					}
					base.SendMessage("OnInputChanged", this, SendMessageOptions.DontRequireReceiver);
				}
			}
			else if (c >= ' ')
			{
				if (this.validator != null)
				{
					c = this.validator(this.mText, c);
				}
				if (c != '\0')
				{
					this.mText += c;
					base.SendMessage("OnInputChanged", this, SendMessageOptions.DontRequireReceiver);
				}
			}
			i++;
		}
		this.UpdateLabel();
	}

	private void UpdateLabel()
	{
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.maxChars > 0 && this.mText.Length > this.maxChars)
		{
			this.mText = this.mText.Substring(0, this.maxChars);
		}
		if (this.label.font != null)
		{
			string text;
			if (this.isPassword && this.selected)
			{
				text = string.Empty;
				int i = 0;
				int length = this.mText.Length;
				while (i < length)
				{
					text += "*";
					i++;
				}
				text = text + Input.compositionString + this.caratChar;
			}
			else
			{
				text = ((!this.selected) ? this.mText : (this.mText + Input.compositionString + this.caratChar));
			}
			this.label.supportEncoding = false;
			if (this.label.overflowMethod == UILabel.Overflow.ClampContent)
			{
				if (this.label.multiLine)
				{
					this.label.font.WrapText(text, out text, this.label.width, this.label.height, 0, false, UIFont.SymbolStyle.None);
				}
				else
				{
					string endOfLineThatFits = this.label.font.GetEndOfLineThatFits(text, (float)this.label.width, false, UIFont.SymbolStyle.None);
					if (endOfLineThatFits != text)
					{
						text = endOfLineThatFits;
						Vector3 localPosition = this.label.cachedTransform.localPosition;
						localPosition.x = this.mPosition + (float)this.label.width;
						if (this.mPivot == UIWidget.Pivot.Left)
						{
							this.label.pivot = UIWidget.Pivot.Right;
						}
						else if (this.mPivot == UIWidget.Pivot.TopLeft)
						{
							this.label.pivot = UIWidget.Pivot.TopRight;
						}
						else if (this.mPivot == UIWidget.Pivot.BottomLeft)
						{
							this.label.pivot = UIWidget.Pivot.BottomRight;
						}
						this.label.cachedTransform.localPosition = localPosition;
					}
					else
					{
						this.RestoreLabel();
					}
				}
			}
			this.label.text = text;
			this.label.showLastPasswordChar = this.selected;
		}
	}

	private void RestoreLabel()
	{
		if (this.label != null)
		{
			this.label.pivot = this.mPivot;
			Vector3 localPosition = this.label.cachedTransform.localPosition;
			localPosition.x = this.mPosition;
			this.label.cachedTransform.localPosition = localPosition;
		}
	}
}
