using System;
using UnityEngine;

public class ButtonEventTimer : MonoBehaviour
{
	private float m_waitButtonTime;

	public void SetWaitTime(float waitTime)
	{
		this.m_waitButtonTime = waitTime;
	}

	public void SetWaitTimeDefault()
	{
		this.SetWaitTime(0.4f);
	}

	public bool IsWaiting()
	{
		return this.m_waitButtonTime > 0f;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_waitButtonTime > 0f)
		{
			this.m_waitButtonTime -= RealTime.deltaTime;
			if (this.m_waitButtonTime <= 0f)
			{
				this.m_waitButtonTime = 0f;
			}
		}
	}
}
