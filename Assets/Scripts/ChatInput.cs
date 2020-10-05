using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Chat Input"), RequireComponent(typeof(UIInput))]
public class ChatInput : MonoBehaviour
{
	public UITextList textList;

	public bool fillWithDummyData;

	private UIInput mInput;

	private bool mIgnoreNextEnter;

	private void Start()
	{
		this.mInput = base.GetComponent<UIInput>();
		if (this.fillWithDummyData && this.textList != null)
		{
			for (int i = 0; i < 30; i++)
			{
				this.textList.Add(string.Concat(new object[]
				{
					(i % 2 != 0) ? "[AAAAAA]" : "[FFFFFF]",
					"This is an example paragraph for the text list, testing line ",
					i,
					"[-]"
				}));
			}
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyUp(KeyCode.Return))
		{
			if (!this.mIgnoreNextEnter && !this.mInput.selected)
			{
				this.mInput.label.maxLineCount = 1;
				this.mInput.selected = true;
			}
			this.mIgnoreNextEnter = false;
		}
	}

	public void OnSubmit()
	{
		if (this.textList != null)
		{
			string text = NGUITools.StripSymbols(this.mInput.value);
			if (!string.IsNullOrEmpty(text))
			{
				this.textList.Add(text);
				this.mInput.value = string.Empty;
				this.mInput.selected = false;
			}
		}
		this.mIgnoreNextEnter = true;
	}
}
