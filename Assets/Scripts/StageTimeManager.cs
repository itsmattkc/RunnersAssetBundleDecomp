using System;
using System.Collections.Generic;
using UnityEngine;

public class StageTimeManager : MonoBehaviour
{
	public enum ExtendPattern
	{
		UNKNOWN,
		CONTINUE,
		BRONZE_WATCH,
		SILVER_WATCH,
		GOLD_WATCH
	}

	private Dictionary<StageTimeManager.ExtendPattern, float> m_extendParams = new Dictionary<StageTimeManager.ExtendPattern, float>();

	private bool m_playing;

	private bool m_phantomPause;

	private float m_time = 60f;

	private float m_extendedTime;

	private float m_cotinueTime;

	private float m_reservedExtendedTime;

	private float m_charaOverlapBonus = 6f;

	private float m_extendedLimit = 480f;

	private StageScoreManager.MaskedInt m_bronzeCount = default(StageScoreManager.MaskedInt);

	private StageScoreManager.MaskedInt m_silverCount = default(StageScoreManager.MaskedInt);

	private StageScoreManager.MaskedInt m_goldCount = default(StageScoreManager.MaskedInt);

	private StageScoreManager.MaskedInt m_continuCount = default(StageScoreManager.MaskedInt);

	private StageScoreManager.MaskedInt m_mainCharaExtend = default(StageScoreManager.MaskedInt);

	private StageScoreManager.MaskedInt m_subCharaExtend = default(StageScoreManager.MaskedInt);

	private StageScoreManager.MaskedInt m_totalTime = default(StageScoreManager.MaskedInt);

	private StageScoreManager.MaskedLong m_playTime = default(StageScoreManager.MaskedLong);

	private StageTimeTable m_stageTimeTable;

	[Header("DebugFlag にチェックを入れると、下記項目にて設定した値が適応されます"), SerializeField]
	private bool m_debugFlag;

	[Header("開始時の残り時間"), SerializeField]
	private float m_debugStartTime = 60f;

	[Header("アイテムの効果"), SerializeField]
	private float m_debugBronzeWatch = 1f;

	[SerializeField]
	private float m_debugSilverWatch = 5f;

	[SerializeField]
	private float m_debugGoldWatch = 10f;

	[SerializeField]
	private float m_debugContinue = 60f;

	private static StageTimeManager s_instance;

	public float Time
	{
		get
		{
			return this.m_time;
		}
	}

	public static StageTimeManager Instance
	{
		get
		{
			return StageTimeManager.s_instance;
		}
	}

	private void Awake()
	{
		if (StageTimeManager.s_instance == null)
		{
			StageTimeManager.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_playing && !this.m_phantomPause && this.m_time > 0f)
		{
			this.m_time -= UnityEngine.Time.deltaTime;
			if (this.m_time < 0f)
			{
				this.m_time = 0f;
			}
			this.m_playTime.Set(this.m_playTime.Get() + (long)(UnityEngine.Time.deltaTime * 1000f));
		}
	}

	public void SetTable()
	{
		StageTimeTable stageTimeTable = GameObjectUtil.FindGameObjectComponent<StageTimeTable>("StageTimeTable");
		if (stageTimeTable != null)
		{
			this.m_stageTimeTable = stageTimeTable;
		}
		this.SetTime();
	}

	private void SetTime()
	{
		float num = 0f;
		ServerPlayerState playerState = ServerInterface.PlayerState;
		if (playerState != null)
		{
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null && instance.PlayerData != null)
			{
				ServerCharacterState serverCharacterState = playerState.CharacterState(instance.PlayerData.MainChara);
				if (serverCharacterState != null)
				{
					float quickModeTimeExtension = serverCharacterState.QuickModeTimeExtension;
					num += quickModeTimeExtension;
					this.m_mainCharaExtend.Set((int)quickModeTimeExtension);
				}
				if (StageAbilityManager.Instance != null && StageAbilityManager.Instance.BoostItemValidFlag[2])
				{
					ServerCharacterState serverCharacterState2 = playerState.CharacterState(instance.PlayerData.SubChara);
					if (serverCharacterState2 != null)
					{
						float quickModeTimeExtension2 = serverCharacterState2.QuickModeTimeExtension;
						num += quickModeTimeExtension2;
						this.m_subCharaExtend.Set((int)quickModeTimeExtension2);
					}
				}
			}
		}
		this.m_extendParams.Clear();
		if (!this.m_debugFlag && this.m_stageTimeTable != null && this.m_stageTimeTable.IsSetupEnd())
		{
			this.m_extendedLimit = (float)this.m_stageTimeTable.GetTableItemData(StageTimeTableItem.ItemExtendedLimit);
			this.m_charaOverlapBonus = (float)this.m_stageTimeTable.GetTableItemData(StageTimeTableItem.OverlapBonus);
			this.m_extendParams.Add(StageTimeManager.ExtendPattern.CONTINUE, (float)this.m_stageTimeTable.GetTableItemData(StageTimeTableItem.Continue));
			this.m_extendParams.Add(StageTimeManager.ExtendPattern.BRONZE_WATCH, (float)this.m_stageTimeTable.GetTableItemData(StageTimeTableItem.BronzeWatch));
			this.m_extendParams.Add(StageTimeManager.ExtendPattern.SILVER_WATCH, (float)this.m_stageTimeTable.GetTableItemData(StageTimeTableItem.SilverWatch));
			this.m_extendParams.Add(StageTimeManager.ExtendPattern.GOLD_WATCH, (float)this.m_stageTimeTable.GetTableItemData(StageTimeTableItem.GoldWatch));
			this.m_time = (float)this.m_stageTimeTable.GetTableItemData(StageTimeTableItem.StartTime) + num;
		}
		else if (this.m_debugFlag)
		{
			this.m_extendParams.Add(StageTimeManager.ExtendPattern.CONTINUE, this.m_debugContinue);
			this.m_extendParams.Add(StageTimeManager.ExtendPattern.BRONZE_WATCH, this.m_debugBronzeWatch);
			this.m_extendParams.Add(StageTimeManager.ExtendPattern.SILVER_WATCH, this.m_debugSilverWatch);
			this.m_extendParams.Add(StageTimeManager.ExtendPattern.GOLD_WATCH, this.m_debugGoldWatch);
			this.m_time = this.m_debugStartTime + num;
		}
		else
		{
			this.m_extendParams.Add(StageTimeManager.ExtendPattern.CONTINUE, 60f);
			this.m_extendParams.Add(StageTimeManager.ExtendPattern.BRONZE_WATCH, 1f);
			this.m_extendParams.Add(StageTimeManager.ExtendPattern.SILVER_WATCH, 5f);
			this.m_extendParams.Add(StageTimeManager.ExtendPattern.GOLD_WATCH, 10f);
			this.m_time = this.m_debugStartTime + num;
		}
		this.m_totalTime.Set((int)this.m_time);
		this.m_mainCharaExtend = default(StageScoreManager.MaskedInt);
		this.m_subCharaExtend = default(StageScoreManager.MaskedInt);
	}

	public void PlayStart()
	{
		this.m_playing = true;
		this.m_phantomPause = false;
	}

	public void PlayEnd()
	{
		this.m_playing = false;
	}

	public void Pause()
	{
		this.m_playing = false;
	}

	public void Resume()
	{
		this.m_playing = true;
	}

	public void PhantomPause(bool pause)
	{
		this.m_phantomPause = pause;
	}

	public int GetTakeTimerCount(StageTimeManager.ExtendPattern pattern)
	{
		switch (pattern)
		{
		case StageTimeManager.ExtendPattern.BRONZE_WATCH:
			return this.m_bronzeCount.Get();
		case StageTimeManager.ExtendPattern.SILVER_WATCH:
			return this.m_silverCount.Get();
		case StageTimeManager.ExtendPattern.GOLD_WATCH:
			return this.m_goldCount.Get();
		default:
			return 0;
		}
	}

	public void ExtendTime(StageTimeManager.ExtendPattern pattern)
	{
		if (this.m_extendParams.ContainsKey(pattern))
		{
			this.m_time += this.m_extendParams[pattern];
			switch (pattern)
			{
			case StageTimeManager.ExtendPattern.CONTINUE:
				this.m_continuCount.Set(this.m_continuCount.Get() + 1);
				break;
			case StageTimeManager.ExtendPattern.BRONZE_WATCH:
				this.m_bronzeCount.Set(this.m_bronzeCount.Get() + 1);
				this.m_extendedTime += this.m_extendParams[pattern];
				break;
			case StageTimeManager.ExtendPattern.SILVER_WATCH:
				this.m_silverCount.Set(this.m_silverCount.Get() + 1);
				this.m_extendedTime += this.m_extendParams[pattern];
				break;
			case StageTimeManager.ExtendPattern.GOLD_WATCH:
				this.m_goldCount.Set(this.m_goldCount.Get() + 1);
				this.m_extendedTime += this.m_extendParams[pattern];
				break;
			}
			this.m_totalTime.Set(this.m_totalTime.Get() + (int)this.m_extendParams[pattern]);
		}
	}

	public void ReserveExtendTime(StageTimeManager.ExtendPattern pattern)
	{
		if (this.m_extendParams.ContainsKey(pattern))
		{
			switch (pattern)
			{
			case StageTimeManager.ExtendPattern.BRONZE_WATCH:
			case StageTimeManager.ExtendPattern.SILVER_WATCH:
			case StageTimeManager.ExtendPattern.GOLD_WATCH:
				this.m_reservedExtendedTime += this.m_extendParams[pattern];
				break;
			}
		}
	}

	public void CancelReservedExtendTime(StageTimeManager.ExtendPattern pattern)
	{
		if (this.m_extendParams.ContainsKey(pattern))
		{
			switch (pattern)
			{
			case StageTimeManager.ExtendPattern.BRONZE_WATCH:
			case StageTimeManager.ExtendPattern.SILVER_WATCH:
			case StageTimeManager.ExtendPattern.GOLD_WATCH:
				this.m_reservedExtendedTime -= this.m_extendParams[pattern];
				break;
			}
		}
	}

	public float GetExtendTime(StageTimeManager.ExtendPattern pattern)
	{
		if (this.m_extendParams.ContainsKey(pattern))
		{
			return this.m_extendParams[pattern];
		}
		return 0f;
	}

	public void CheckResultTimer()
	{
		CPlusPlusLink instance = CPlusPlusLink.Instance;
		if (instance != null)
		{
			instance.CheckNativeQuickModeResultTimer(this.m_goldCount.Get(), this.m_silverCount.Get(), this.m_bronzeCount.Get(), this.m_continuCount.Get(), this.m_mainCharaExtend.Get(), this.m_subCharaExtend.Get(), this.m_totalTime.Get(), this.m_playTime.Get());
		}
	}

	public bool IsTimeUp()
	{
		return this.m_time <= 0f;
	}

	public bool IsReachedExtendedLimit()
	{
		return this.m_extendedTime + this.m_reservedExtendedTime >= this.m_extendedLimit;
	}
}
