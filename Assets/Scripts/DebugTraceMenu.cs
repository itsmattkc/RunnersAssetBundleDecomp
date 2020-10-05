using System;
using UnityEngine;

public class DebugTraceMenu : MonoBehaviour
{
	public enum State
	{
		OFF,
		ON
	}

	private DebugTraceMenu.State m_state;

	private DebugTraceButton m_offButton;

	private DebugTraceButton m_onButton;

	private DebugTraceDisplayMenu m_displayMenu;

	public DebugTraceMenu.State currentState
	{
		get
		{
			return this.m_state;
		}
		private set
		{
		}
	}

	private void Start()
	{
		bool flag = this.m_state == DebugTraceMenu.State.ON;
		this.m_offButton = base.gameObject.AddComponent<DebugTraceButton>();
		this.m_offButton.Setup("TraceOff", new DebugTraceButton.ButtonClickedCallback(this.DebugButtonClickedCallback), new Vector2(10f, 10f));
		this.m_offButton.SetActive(flag);
		this.m_onButton = base.gameObject.AddComponent<DebugTraceButton>();
		this.m_onButton.Setup("TraceOn", new DebugTraceButton.ButtonClickedCallback(this.DebugButtonClickedCallback), new Vector2(10f, 10f));
		this.m_onButton.SetActive(!flag);
		this.m_displayMenu = base.gameObject.AddComponent<DebugTraceDisplayMenu>();
		this.m_displayMenu.Setup();
		this.m_displayMenu.SetActive(flag);
	}

	private void DebugButtonClickedCallback(string buttonName)
	{
		if (buttonName == "TraceOff")
		{
			this.m_onButton.SetActive(true);
			this.m_offButton.SetActive(false);
			this.m_displayMenu.SetActive(false);
			GeneralWindow.Close();
			this.m_state = DebugTraceMenu.State.OFF;
		}
		else if (buttonName == "TraceOn")
		{
			this.m_onButton.SetActive(false);
			this.m_offButton.SetActive(true);
			this.m_displayMenu.SetActive(true);
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				buttonType = GeneralWindow.ButtonType.YesNo,
				caption = string.Empty,
				message = "トレース表示中"
			});
			this.m_state = DebugTraceMenu.State.ON;
		}
	}
}
