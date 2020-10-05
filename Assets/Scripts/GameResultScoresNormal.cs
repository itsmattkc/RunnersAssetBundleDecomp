using AnimationOrTween;
using System;
using UnityEngine;

public class GameResultScoresNormal : GameResultScores
{
	public enum MileageBonusType
	{
		NONE = -1,
		SCORE,
		RING,
		REDSTAR_RING,
		ANIMAL,
		DISTANCE,
		TOTAL,
		NUM
	}

	private string[] MileageBonusLabelNameList = new string[]
	{
		"Lbl_score_bonus",
		"Lbl_ring_bonus",
		string.Empty,
		"Lbl_animal_bonus",
		"Lbl_distance_bonus",
		"Lbl_totalscore"
	};

	private GameResultScoreInterporate[] m_mileageBonusScores = new GameResultScoreInterporate[6];

	private Animation[] m_mileageBonusAnims = new Animation[6];

	private bool m_countSE;

	private GameResultScoreInterporate m_bestScore;

	private GameObject m_bestScoreEffect;

	private bool m_bestScoreEffectOn;

	private bool m_isEvent;

	private GameObject m_easyBonusDisplay;

	private bool m_isEasyBonus;

	protected override bool IsBonus(StageScoreManager.ResultData data1, StageScoreManager.ResultData data2, StageScoreManager.ResultData data3)
	{
		long num = 0L;
		if (data1 != null)
		{
			num += data1.Sum();
		}
		if (data2 != null)
		{
			num += data2.Sum();
		}
		if (data3 != null)
		{
			num += data3.Sum();
		}
		return num > 0L;
	}

	protected override void OnSetup(GameObject resultRoot)
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(resultRoot, "window_result");
		if (gameObject == null)
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			string name = string.Format("row_{0}", i + 1) + "_2";
			GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, name);
			if (gameObject2 != null)
			{
				UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject2, this.MileageBonusLabelNameList[i]);
				if (uILabel != null)
				{
					this.m_mileageBonusScores[i] = new GameResultScoreInterporate();
					this.m_mileageBonusScores[i].Setup(uILabel);
					this.m_mileageBonusScores[i].SetLabelStartValue(0L);
					this.m_mileageBonusAnims[i] = GameObjectUtil.FindChildGameObjectComponent<Animation>(gameObject2, "mileage_bunus_Anim");
					if (this.m_mileageBonusAnims[i] != null)
					{
						this.m_mileageBonusAnims[i].gameObject.SetActive(false);
					}
				}
			}
		}
		this.m_isEvent = false;
		EventManager instance = EventManager.Instance;
		if (instance != null)
		{
			this.m_isEvent = instance.IsSpecialStage();
		}
		if (this.m_isEvent)
		{
			GameObject gameObject3 = GameObjectUtil.FindChildGameObject(resultRoot, "window_record");
			if (gameObject3 != null)
			{
				gameObject3.SetActive(false);
			}
		}
		UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(resultRoot, "Lbl_best_score");
		if (uILabel2 != null)
		{
			this.m_bestScore = new GameResultScoreInterporate();
			this.m_bestScore.Setup(uILabel2);
			long oldBestScore = GameResultUtility.GetOldBestScore();
			this.m_bestScore.SetLabelStartValue(oldBestScore);
		}
		this.m_bestScoreEffect = GameObjectUtil.FindChildGameObject(resultRoot, "pattern_best");
		if (this.m_bestScoreEffect != null)
		{
			this.m_bestScoreEffect.SetActive(false);
		}
		this.m_easyBonusDisplay = GameObjectUtil.FindChildGameObject(resultRoot, "player_negative_bonus");
		if (this.m_easyBonusDisplay != null)
		{
			SaveDataManager instance2 = SaveDataManager.Instance;
			StageAbilityManager instance3 = StageAbilityManager.Instance;
			if (instance3 != null)
			{
				bool flag = false;
				float num = 1f;
				GameObject gameObject4 = GameObjectUtil.FindChildGameObject(this.m_easyBonusDisplay, "img_player_main");
				if (instance3.IsEasySpeed(PlayingCharacterType.MAIN))
				{
					CharaType mainChara = instance2.PlayerData.MainChara;
					if (mainChara != CharaType.UNKNOWN)
					{
						UISprite component = gameObject4.GetComponent<UISprite>();
						GameResultUtility.SetCharaTexture(component, mainChara);
						this.m_isEasyBonus = true;
						flag = true;
						num = 0.8f;
					}
				}
				else
				{
					gameObject4.SetActive(false);
				}
				GameObject gameObject5 = GameObjectUtil.FindChildGameObject(this.m_easyBonusDisplay, "img_player_sub");
				if (instance3.IsEasySpeed(PlayingCharacterType.SUB))
				{
					CharaType subChara = instance2.PlayerData.SubChara;
					if (subChara != CharaType.UNKNOWN)
					{
						UISprite component2 = gameObject5.GetComponent<UISprite>();
						GameResultUtility.SetCharaTexture(component2, subChara);
						if (flag)
						{
							num = 0.64f;
						}
						else
						{
							num = 0.8f;
						}
						this.m_isEasyBonus = true;
					}
				}
				else
				{
					gameObject5.SetActive(false);
				}
				if (this.m_isEasyBonus)
				{
					num = 100f * (1f - num);
					UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_easyBonusDisplay, "Lbl_player_negative_bonus");
					uILabel3.text = "-" + num.ToString() + "%";
				}
				this.m_easyBonusDisplay.SetActive(false);
			}
		}
		base.SetEnableNextButton(true);
	}

	protected override void OnFinish()
	{
		if (this.m_bestScoreEffect != null)
		{
			this.m_bestScoreEffect.SetActive(false);
		}
		if (this.m_isEasyBonus)
		{
			this.m_easyBonusDisplay.SetActive(false);
		}
		for (int i = 0; i < 6; i++)
		{
			if (this.m_mileageBonusAnims[i] != null)
			{
				this.m_mileageBonusAnims[i].gameObject.SetActive(false);
			}
		}
	}

	protected override void OnStartMileageBonusScore()
	{
		Animation component = this.m_resultRoot.GetComponent<Animation>();
		if (component != null)
		{
			ActiveAnimation.Play(component, "ui_result_intro2_Anim", Direction.Forward);
		}
		base.SetBonusEventScoreActive(GameResultScores.Category.NONE);
		StageScoreManager.ResultData scoreData = this.m_scoreManager.ScoreData;
		StageScoreManager.ResultData mileageBonusScoreData = this.m_scoreManager.MileageBonusScoreData;
		if (this.m_mileageBonusScores[0] != null)
		{
			this.m_mileageBonusScores[0].PlayStart(scoreData.score, scoreData.score + mileageBonusScoreData.score, GameResultUtility.ScoreInterpolateTime);
		}
		if (this.m_mileageBonusScores[1] != null)
		{
			this.m_mileageBonusScores[1].PlayStart(scoreData.ring, scoreData.ring + mileageBonusScoreData.ring, GameResultUtility.ScoreInterpolateTime);
		}
		if (this.m_mileageBonusScores[3] != null)
		{
			this.m_mileageBonusScores[3].PlayStart(scoreData.animal, scoreData.animal + mileageBonusScoreData.animal, GameResultUtility.ScoreInterpolateTime);
		}
		if (this.m_mileageBonusScores[4] != null)
		{
			this.m_mileageBonusScores[4].PlayStart(scoreData.distance, scoreData.distance + mileageBonusScoreData.distance, GameResultUtility.ScoreInterpolateTime);
		}
		long num = scoreData.Sum();
		long finalScore = this.m_scoreManager.FinalScore;
		if (this.m_mileageBonusScores[5] != null)
		{
			this.m_mileageBonusScores[5].PlayStart(scoreData.Sum(), this.m_scoreManager.FinalScore, GameResultUtility.ScoreInterpolateTime);
			if (this.m_isEasyBonus)
			{
				this.m_easyBonusDisplay.SetActive(true);
			}
		}
		long num2 = mileageBonusScoreData.Sum() + (finalScore - num);
		if (num2 > 0L)
		{
			SoundManager.SePlay("sys_result_count", "SE");
			this.m_countSE = true;
		}
		else
		{
			this.m_countSE = false;
		}
		StageAbilityManager stageAbilityManager = GameObjectUtil.FindGameObjectComponent<StageAbilityManager>("StageAbilityManager");
		if (stageAbilityManager != null)
		{
			StageAbilityManager.BonusRate mileageBonusScoreRate = stageAbilityManager.MileageBonusScoreRate;
			float[] array = new float[]
			{
				mileageBonusScoreRate.score * 100f,
				mileageBonusScoreRate.ring * 100f,
				0f,
				mileageBonusScoreRate.animal * 100f,
				mileageBonusScoreRate.distance * 100f,
				mileageBonusScoreRate.final_score * 100f
			};
			for (int i = 0; i < 6; i++)
			{
				if (this.m_mileageBonusAnims[i] != null)
				{
					GameResultUtility.SetActiveBonus(this.m_mileageBonusAnims[i].gameObject, "Lbl_mileage_bonus_score", array[i]);
				}
			}
		}
		if (this.m_bestScore != null)
		{
			long oldBestScore = GameResultUtility.GetOldBestScore();
			long num3 = GameResultUtility.GetNewBestScore();
			if (oldBestScore > num3)
			{
				num3 = oldBestScore;
			}
			this.m_bestScore.PlayStart(oldBestScore, num3, GameResultUtility.ScoreInterpolateTime);
			this.m_bestScore.IsPause = true;
		}
	}

	protected override void OnUpdateMileageBonusScore()
	{
		float num = 0f;
		long num2 = 0L;
		float deltaTime = Time.deltaTime;
		for (int i = 0; i < 6; i++)
		{
			GameResultScoreInterporate gameResultScoreInterporate = this.m_mileageBonusScores[i];
			if (gameResultScoreInterporate != null)
			{
				long num3 = gameResultScoreInterporate.Update(deltaTime);
				if (i == 5)
				{
					num2 = num3;
					num = gameResultScoreInterporate.CurrentTime;
				}
			}
		}
		if (this.m_bestScore == null)
		{
			return;
		}
		long oldBestScore = GameResultUtility.GetOldBestScore();
		if (num2 >= oldBestScore && this.m_bestScore.IsPause)
		{
			float interpolateTime = GameResultUtility.ScoreInterpolateTime - num;
			long num4 = GameResultUtility.GetOldBestScore();
			long num5 = this.m_scoreManager.ScoreData.Sum();
			if (num4 <= num5)
			{
				num4 = num5;
			}
			long newBestScore = GameResultUtility.GetNewBestScore();
			if (newBestScore >= num4)
			{
				this.m_bestScore.PlayStart(num4, newBestScore, interpolateTime);
				this.m_bestScore.IsPause = false;
			}
		}
		this.m_bestScore.Update(deltaTime);
	}

	protected override void OnSkipMileageBonusScore()
	{
		for (int i = 0; i < 6; i++)
		{
			GameResultScoreInterporate gameResultScoreInterporate = this.m_mileageBonusScores[i];
			if (gameResultScoreInterporate != null)
			{
				gameResultScoreInterporate.PlaySkip();
			}
		}
		if (this.m_bestScore != null)
		{
			this.m_bestScore.PlaySkip();
		}
		if (this.m_isEasyBonus)
		{
			this.m_easyBonusDisplay.SetActive(true);
		}
	}

	protected override void OnEndMileageBonusScore()
	{
		if (this.m_countSE)
		{
			SoundManager.SeStop("sys_result_count", "SE");
		}
	}

	protected override bool IsEndMileageBonusScore()
	{
		for (int i = 0; i < 6; i++)
		{
			GameResultScoreInterporate gameResultScoreInterporate = this.m_mileageBonusScores[i];
			if (gameResultScoreInterporate != null)
			{
				if (!gameResultScoreInterporate.IsEnd)
				{
					return false;
				}
			}
		}
		return true;
	}

	protected override void OnStartFinished()
	{
		long oldBestScore = GameResultUtility.GetOldBestScore();
		long newBestScore = GameResultUtility.GetNewBestScore();
		if (newBestScore > oldBestScore && !this.m_isEvent && this.m_bestScoreEffect != null)
		{
			this.m_bestScoreEffect.SetActive(true);
			SoundManager.SePlay("sys_result_best", "SE");
			this.m_bestScoreEffectOn = true;
			RegionManager instance = RegionManager.Instance;
			if (instance != null && instance.IsJapan())
			{
				SoundManager.SePlay("sys_voice_record_jp", "SE");
			}
			else
			{
				SoundManager.SePlay("sys_voice_record_e", "SE");
			}
		}
	}

	protected override void OnSetActiveDetailsButton(bool on)
	{
		if (this.m_bestScoreEffectOn && this.m_bestScoreEffect != null)
		{
			UIObjectContainer component = this.m_bestScoreEffect.GetComponent<UIObjectContainer>();
			if (component != null)
			{
				component.SetActive(!on);
			}
		}
	}

	protected override void OnScoreInAnimation(EventDelegate.Callback callback)
	{
		Animation animation = GameResultUtility.SearchAnimation(this.m_resultRoot);
		if (animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, "ui_result_intro_score_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, callback, true);
		}
	}

	protected override void OnScoreOutAnimation(EventDelegate.Callback callback)
	{
		Animation animation = GameResultUtility.SearchAnimation(this.m_resultRoot);
		if (animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, "ui_result_intro_score_Anim", Direction.Reverse);
			EventDelegate.Add(activeAnimation.onFinished, callback, true);
		}
	}
}
