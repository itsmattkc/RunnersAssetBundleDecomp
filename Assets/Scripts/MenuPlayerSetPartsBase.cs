using System;
using UnityEngine;

public abstract class MenuPlayerSetPartsBase : MonoBehaviour
{
	private UIPanel m_panel;

	private bool m_isReady;

	public MenuPlayerSetPartsBase(string panelName)
	{
		this.m_isReady = false;
	}

	public void PlayStart()
	{
		this.OnPlayStart();
	}

	public void PlayEnd()
	{
		this.OnPlayEnd();
	}

	public void Reset()
	{
		this.m_isReady = false;
	}

	public void LateUpdate()
	{
		float deltaTime = Time.deltaTime;
		if (!this.m_isReady)
		{
			this.OnSetup();
			this.PlayStart();
			this.m_isReady = true;
		}
		this.OnUpdate(deltaTime);
	}

	protected abstract void OnSetup();

	protected abstract void OnPlayStart();

	protected abstract void OnPlayEnd();

	protected abstract void OnUpdate(float deltaTime);
}
