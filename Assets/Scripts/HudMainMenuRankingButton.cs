using System;
using UnityEngine;

public class HudMainMenuRankingButton
{
	private enum State
	{
		INIT,
		UPDATE,
		NUM
	}

	private GameObject m_mainMenuObject;

	private bool m_isQuickMode;

	private GameObject m_buttonParentObject;

	private static readonly float UPDATE_TIME = 60f;

	private float m_nextUpdateTime;

	private HudMainMenuRankingButton.State m_state;

	public void Intialize(GameObject mainMenuObject, bool isQuickMode)
	{
		if (mainMenuObject == null)
		{
			return;
		}
		this.m_mainMenuObject = mainMenuObject;
		this.m_isQuickMode = isQuickMode;
		if (isQuickMode)
		{
			this.m_buttonParentObject = null;
			GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_mainMenuObject, "Anchor_5_MC");
			if (gameObject == null)
			{
				return;
			}
			this.m_buttonParentObject = GameObjectUtil.FindChildGameObject(gameObject, "1_Quick");
			if (this.m_buttonParentObject == null)
			{
				return;
			}
		}
		else
		{
			this.m_buttonParentObject = null;
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_mainMenuObject, "Anchor_5_MC");
			if (gameObject2 == null)
			{
				return;
			}
			this.m_buttonParentObject = GameObjectUtil.FindChildGameObject(gameObject2, "0_Endless");
			if (this.m_buttonParentObject == null)
			{
				return;
			}
		}
		this.m_nextUpdateTime = HudMainMenuRankingButton.UPDATE_TIME;
	}

	public void Update()
	{
		if (this.m_buttonParentObject == null)
		{
			return;
		}
		HudMainMenuRankingButton.State state = this.m_state;
		if (state != HudMainMenuRankingButton.State.INIT)
		{
			if (state == HudMainMenuRankingButton.State.UPDATE)
			{
				this.m_nextUpdateTime -= Time.deltaTime;
				if (this.m_nextUpdateTime <= 0f)
				{
					if (this.m_isQuickMode)
					{
						GeneralUtil.SetQuickRankingTime(this.m_buttonParentObject, "Btn_1_ranking");
					}
					else
					{
						GeneralUtil.SetEndlessRankingTime(this.m_buttonParentObject, "Btn_1_ranking");
					}
					this.m_nextUpdateTime = HudMainMenuRankingButton.UPDATE_TIME;
				}
			}
		}
		else
		{
			bool flag;
			if (this.m_isQuickMode)
			{
				flag = GeneralUtil.SetQuickRankingBtnIcon(this.m_buttonParentObject, "Btn_1_ranking");
			}
			else
			{
				flag = GeneralUtil.SetEndlessRankingBtnIcon(this.m_buttonParentObject, "Btn_1_ranking");
			}
			if (flag)
			{
				this.m_state = HudMainMenuRankingButton.State.UPDATE;
			}
		}
	}
}
