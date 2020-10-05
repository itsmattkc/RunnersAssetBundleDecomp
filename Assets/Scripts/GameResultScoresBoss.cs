using AnimationOrTween;
using System;
using UnityEngine;

public class GameResultScoresBoss : GameResultScores
{
	private enum AnimState
	{
		NONE = -1,
		IDLE,
		PLAYING1,
		PLAYING2,
		FINISHED,
		END
	}

	private const string noMissBonusClip = "ui_result_nomiss_bonus_Anim";

	private GameResultScoresBoss.AnimState m_animState;

	private Animation m_noMissBonusAnim;

	private float m_noMissBonusViewTime;

	private bool m_isNomiss;

	public void SetNoMissFlag(bool flag)
	{
		this.m_isNomiss = flag;
	}

	protected override bool IsBonus(StageScoreManager.ResultData data1, StageScoreManager.ResultData data2, StageScoreManager.ResultData data3)
	{
		long num = 0L;
		if (data1 != null)
		{
			num += data1.ring;
		}
		if (data2 != null)
		{
			num += data2.ring;
		}
		if (data3 != null)
		{
			num += data3.ring;
		}
		return num > 0L;
	}

	protected override void OnSetup(GameObject resultRoot)
	{
		this.m_noMissBonusViewTime = 0f;
		this.m_noMissBonusAnim = GameObjectUtil.FindChildGameObjectComponent<Animation>(resultRoot, "nomiss_bonus_Anim");
		if (this.m_noMissBonusAnim != null)
		{
			this.m_noMissBonusAnim.gameObject.SetActive(false);
			this.m_noMissBonusViewTime = this.m_noMissBonusAnim["ui_result_nomiss_bonus_Anim"].length * 0.3f;
		}
		if (base.IsBonusEvent())
		{
			base.SetEnableNextButton(true);
		}
		this.m_animState = GameResultScoresBoss.AnimState.IDLE;
		this.m_isBossResult = true;
	}

	protected override void OnFinish()
	{
	}

	protected override void OnStartFinished()
	{
		base.SetBonusEventScoreActive(GameResultScores.Category.NONE);
		if (this.m_isReplay)
		{
			this.m_animState = GameResultScoresBoss.AnimState.FINISHED;
		}
		else if (!this.m_isNomiss)
		{
			this.m_animState = GameResultScoresBoss.AnimState.FINISHED;
		}
		else
		{
			if (this.m_noMissBonusAnim != null)
			{
				this.m_noMissBonusAnim.gameObject.SetActive(true);
			}
			SoundManager.SePlay("sys_specialegg", "SE");
			this.m_animState = GameResultScoresBoss.AnimState.PLAYING1;
			base.SetEnableNextButton(false);
		}
	}

	protected override void OnUpdateFinished()
	{
		if (!this.m_isNomiss)
		{
			return;
		}
		if (this.m_animState == GameResultScoresBoss.AnimState.PLAYING1)
		{
			float time = this.m_noMissBonusAnim["ui_result_nomiss_bonus_Anim"].time;
			if (time > this.m_noMissBonusViewTime)
			{
				base.SetEnableNextButton(true);
				this.m_animState = GameResultScoresBoss.AnimState.PLAYING2;
			}
		}
		if (this.m_animState == GameResultScoresBoss.AnimState.PLAYING2 && !this.m_noMissBonusAnim.isPlaying)
		{
			this.m_animState = GameResultScoresBoss.AnimState.FINISHED;
		}
	}

	protected override void OnSkipFinished()
	{
		if (!this.m_isNomiss)
		{
			return;
		}
		if (this.m_animState == GameResultScoresBoss.AnimState.PLAYING2 && this.m_noMissBonusAnim != null)
		{
			this.m_noMissBonusAnim.Stop();
			this.m_noMissBonusAnim.gameObject.SetActive(false);
			this.m_animState = GameResultScoresBoss.AnimState.FINISHED;
		}
	}

	protected override bool IsEndFinished()
	{
		return this.m_animState == GameResultScoresBoss.AnimState.FINISHED;
	}

	protected override void OnEndFinished()
	{
		base.SetEnableNextButton(true);
	}

	protected override void OnScoreInAnimation(EventDelegate.Callback callback)
	{
		Animation animation = GameResultUtility.SearchAnimation(this.m_resultRoot);
		if (animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, "ui_result_boss_intro_score_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, callback, true);
		}
	}

	protected override void OnScoreOutAnimation(EventDelegate.Callback callback)
	{
		Animation animation = GameResultUtility.SearchAnimation(this.m_resultRoot);
		if (animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, "ui_result_boss_intro_score_Anim", Direction.Reverse);
			EventDelegate.Add(activeAnimation.onFinished, callback, true);
		}
	}
}
