using System;
using UnityEngine;

public class ui_roulette_window_odds_scroll : MonoBehaviour
{
	[SerializeField]
	private UILabel m_prizeName;

	[SerializeField]
	private UILabel m_oddsValue;

	public void UpdateView(string prizeNmae, string oddsValue)
	{
		this.m_prizeName.text = prizeNmae;
		this.m_oddsValue.text = oddsValue;
	}
}
