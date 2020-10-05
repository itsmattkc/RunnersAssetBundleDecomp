using System;
using UnityEngine;

public class DebugTraceDisplayMenu : MonoBehaviour
{
	private bool m_isActive;

	private DebugTraceTextBox m_textBox;

	private DebugTraceScrollBar m_textBoxSizeBar;

	private DebugTraceScrollBar m_textScrollRatioBar;

	private DebugTraceButton m_nextTypeButton;

	private DebugTraceButton m_prevTypeButton;

	private DebugTraceButton m_clearButton;

	private DebugTraceManager.TraceType m_traceType;

	private static readonly float TextMaxScale = 4f;

	private static readonly float ScrollMaxScale = 200f;

	public void Setup()
	{
		this.m_textBox = base.gameObject.AddComponent<DebugTraceTextBox>();
		this.m_textBox.Setup(new Vector2(210f, 10f));
		this.m_textBoxSizeBar = base.gameObject.AddComponent<DebugTraceScrollBar>();
		this.m_textBoxSizeBar.SetUp("BoxSize", new DebugTraceScrollBar.ValueChangedCallback(this.DebugScrollBarChangedCallback), new Vector2(10f, 200f));
		this.m_textScrollRatioBar = base.gameObject.AddComponent<DebugTraceScrollBar>();
		this.m_textScrollRatioBar.SetUp("ScrollRatio", new DebugTraceScrollBar.ValueChangedCallback(this.DebugScrollBarChangedCallback), new Vector2(10f, 300f));
		this.m_nextTypeButton = base.gameObject.AddComponent<DebugTraceButton>();
		this.m_nextTypeButton.Setup("NextType", new DebugTraceButton.ButtonClickedCallback(this.DebugButtonClickedCallback), new Vector2(110f, 100f), new Vector2(100f, 50f));
		this.m_prevTypeButton = base.gameObject.AddComponent<DebugTraceButton>();
		this.m_prevTypeButton.Setup("PrevType", new DebugTraceButton.ButtonClickedCallback(this.DebugButtonClickedCallback), new Vector2(10f, 100f), new Vector2(100f, 50f));
		this.m_clearButton = base.gameObject.AddComponent<DebugTraceButton>();
		this.m_clearButton.Setup("Clear", new DebugTraceButton.ButtonClickedCallback(this.DebugButtonClickedCallback), new Vector2(10f, 400f));
	}

	public void SetActive(bool isActive)
	{
		this.m_isActive = isActive;
		if (this.m_textBox != null)
		{
			this.m_textBox.SetActive(isActive);
		}
		if (this.m_textBoxSizeBar != null)
		{
			this.m_textBoxSizeBar.SetActive(isActive);
		}
		if (this.m_textScrollRatioBar != null)
		{
			this.m_textScrollRatioBar.SetActive(isActive);
		}
		if (this.m_nextTypeButton != null)
		{
			this.m_nextTypeButton.SetActive(isActive);
		}
		if (this.m_prevTypeButton != null)
		{
			this.m_prevTypeButton.SetActive(isActive);
		}
		if (this.m_clearButton != null)
		{
			this.m_clearButton.SetActive(isActive);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		DebugTraceManager instance = DebugTraceManager.Instance;
		if (instance == null)
		{
			return;
		}
		if (this.m_textBox != null)
		{
			string text = instance.GetTraceText(this.m_traceType);
			if (string.IsNullOrEmpty(text))
			{
				text = "+Empty";
			}
			this.m_textBox.SetText(text);
		}
	}

	private void OnGUI()
	{
		if (!this.m_isActive)
		{
			return;
		}
		GUI.Label(new Rect(10f, 150f, 200f, 50f), DebugTraceManager.TypeName[(int)this.m_traceType]);
	}

	private void DebugScrollBarChangedCallback(string name, float value)
	{
		if (this.m_textBox == null)
		{
			return;
		}
		if (name == "BoxSize")
		{
			float sizeScale = value * DebugTraceDisplayMenu.TextMaxScale / DebugTraceScrollBar.MaxValue + 1f;
			this.m_textBox.SetSizeScale(sizeScale);
		}
		else if (name == "ScrollRatio")
		{
			float num = value * DebugTraceDisplayMenu.ScrollMaxScale / DebugTraceScrollBar.MaxValue + 1f;
			this.m_textBox.SetScrollScale(new Vector2(num, num));
		}
	}

	private void DebugButtonClickedCallback(string buttonName)
	{
		if (buttonName == "NextType")
		{
			if (this.m_traceType == DebugTraceManager.TraceType.GAME)
			{
				this.m_traceType = DebugTraceManager.TraceType.ALL;
			}
			else
			{
				this.m_traceType++;
			}
		}
		else if (buttonName == "PrevType")
		{
			if (this.m_traceType == DebugTraceManager.TraceType.ALL)
			{
				this.m_traceType = DebugTraceManager.TraceType.GAME;
			}
			else
			{
				this.m_traceType--;
			}
		}
		else if (buttonName == "Clear")
		{
			DebugTraceManager instance = DebugTraceManager.Instance;
			if (instance != null)
			{
				instance.ClearTrace(this.m_traceType);
			}
		}
	}
}
