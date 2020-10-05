using System;
using UnityEngine;

public class HudProgressBar : MonoBehaviour
{
	[SerializeField]
	private UISlider m_slider;

	[SerializeField]
	private UILabel m_parcentLabel;

	private float m_stateNum;

	private float m_state = -1f;

	public void SetUp(int stateNum)
	{
		this.m_stateNum = (float)stateNum;
		this.m_state = -1f;
	}

	public void SetState(int state)
	{
		this.m_state = (float)state;
		if (this.m_state >= 0f)
		{
			base.gameObject.SetActive(true);
			if (this.m_slider != null)
			{
				this.m_slider.value = (this.m_state + 1f) / this.m_stateNum;
				if (this.m_parcentLabel != null)
				{
					int num = (int)(this.m_slider.value * 100f);
					this.m_parcentLabel.text = num.ToString() + "%";
				}
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}
}
