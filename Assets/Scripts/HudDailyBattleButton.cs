using System;
using UnityEngine;

public class HudDailyBattleButton
{
	private GameObject m_mainMenuObject;

	private GameObject m_quickModeObject;

	private static readonly float UPDATE_TIME = 1f;

	private float m_nextUpdateTime;

	public void Initialize(GameObject mainMenuObject)
	{
		if (mainMenuObject == null)
		{
			return;
		}
		this.m_mainMenuObject = mainMenuObject;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_mainMenuObject, "Anchor_5_MC");
		if (gameObject == null)
		{
			return;
		}
		this.m_quickModeObject = GameObjectUtil.FindChildGameObject(gameObject, "1_Quick");
		if (this.m_quickModeObject == null)
		{
			return;
		}
		GeneralUtil.SetDailyBattleBtnIcon(this.m_quickModeObject, "Btn_2_battle");
	}

	public void Update()
	{
		this.m_nextUpdateTime -= Time.deltaTime;
		if (this.m_nextUpdateTime <= 0f)
		{
			GeneralUtil.SetDailyBattleTime(this.m_quickModeObject, "Btn_2_battle");
			this.m_nextUpdateTime = HudDailyBattleButton.UPDATE_TIME;
		}
	}

	public void UpdateView()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_mainMenuObject, "Anchor_5_MC");
		if (gameObject == null)
		{
			return;
		}
		this.m_quickModeObject = GameObjectUtil.FindChildGameObject(gameObject, "1_Quick");
		if (this.m_quickModeObject == null)
		{
			return;
		}
		GeneralUtil.SetDailyBattleBtnIcon(this.m_quickModeObject, "Btn_2_battle");
	}
}
