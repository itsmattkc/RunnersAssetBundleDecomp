using System;
using UnityEngine;

public class HudFlagWatcher
{
	public delegate void ValueChangeCallback(float newValue, float prevValue);

	private GameObject m_watchObject;

	private float m_value;

	private HudFlagWatcher.ValueChangeCallback m_callback;

	public void Setup(GameObject watchObject, HudFlagWatcher.ValueChangeCallback callback)
	{
		this.m_watchObject = watchObject;
		this.m_callback = callback;
		if (this.m_watchObject != null)
		{
			this.m_watchObject.SetActive(true);
			this.m_value = this.m_watchObject.transform.localPosition.x;
		}
	}

	public void Update()
	{
		if (this.m_watchObject != null)
		{
			float x = this.m_watchObject.transform.localPosition.x;
			if (x != this.m_value)
			{
				if (this.m_callback != null)
				{
					this.m_callback(x, this.m_value);
				}
				this.m_value = x;
			}
		}
	}
}
