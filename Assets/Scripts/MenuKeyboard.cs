using System;
using UnityEngine;

public class MenuKeyboard : MonoBehaviour
{
	private TouchScreenKeyboard m_keyboard;

	private string m_inputText = string.Empty;

	private bool m_isOpen;

	private bool m_isDone;

	private bool m_isCanceled;

	public bool IsDone
	{
		get
		{
			return this.m_isDone;
		}
		private set
		{
			this.m_isDone = value;
		}
	}

	public bool IsCanceled
	{
		get
		{
			return this.m_isCanceled;
		}
		private set
		{
			this.m_isCanceled = value;
		}
	}

	public string InputText
	{
		get
		{
			return this.m_inputText;
		}
		private set
		{
			this.m_inputText = value;
		}
	}

	public void Open()
	{
		this.m_keyboard = TouchScreenKeyboard.Open(string.Empty, TouchScreenKeyboardType.Default);
		this.m_isOpen = true;
		this.m_isDone = false;
		this.m_isCanceled = false;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		if (this.m_keyboard != null && this.m_isOpen)
		{
			this.m_inputText = this.m_keyboard.text;
			if (this.m_keyboard.done)
			{
				this.m_isDone = true;
				this.m_keyboard = null;
				this.m_isOpen = false;
			}
			else if (this.m_keyboard.wasCanceled)
			{
				this.m_isCanceled = true;
				this.m_keyboard = null;
				this.m_isOpen = false;
			}
		}
	}
}
