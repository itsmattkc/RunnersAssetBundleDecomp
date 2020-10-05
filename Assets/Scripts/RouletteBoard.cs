using System;
using System.Collections.Generic;
using UnityEngine;

public class RouletteBoard : RoulettePartsBase
{
	public const int ROULETTE_SPIN_MIN_ROT = 2;

	public const float ROULETTE_SPIN_SPEED = 10f;

	public const float ROULETTE_SPIN_SLOW_SPEED_RATE = 0.4f;

	public const float ROULETTE_SPIN_SLOW_SPEED_LAST = 0.025f;

	public const float ROULETTE_SPIN_SLOW_DEG = 180f;

	public const float ROULETTE_SPIN_SKIP_POINT_RATE = 0.5f;

	public const float ROULETTE_SPIN_MAX = 9999999f;

	private const float ARROW_ROTATION_X = 42f;

	private const float ARROW_ROTATION_Y = 9.139092E-05f;

	[SerializeField]
	private GameObject m_arrow;

	[SerializeField]
	private List<RouletteBoardPattern> m_pattern;

	[SerializeField]
	private RouletteItem m_orgRouletteItem;

	private float m_currentDegree;

	private float m_degreeSlow;

	private float m_degreeSlowLast;

	private float m_currentDegreeMax;

	private float m_timeRate = 1f;

	private float m_arrowSpeed;

	private int m_currentArrowPos = -1;

	private RouletteBoardPattern m_currentBoardPattern;

	private float ONE_FPS_TIME = 0.0166666675f;

	public ServerWheelOptionsData wheelData
	{
		get
		{
			if (this.m_parent == null)
			{
				return null;
			}
			return this.m_parent.wheelData;
		}
	}

	public override void Setup(RouletteTop parent)
	{
		base.Setup(parent);
		this.m_isEffectLock = false;
		if (this.m_parent != null && this.m_parent.wheelData != null && this.m_parent.wheelData.category == RouletteCategory.ITEM)
		{
			this.m_isEffectLock = true;
		}
		this.ONE_FPS_TIME = 1f / (float)Application.targetFrameRate;
		this.SetupBoard(this.m_parent.wheelData);
		this.SetupArrow(this.m_parent.wheelData);
		this.UpdateEffectSetting();
	}

	public override void OnUpdateWheelData(ServerWheelOptionsData data)
	{
		this.m_isEffectLock = false;
		if (data != null && data.category == RouletteCategory.ITEM)
		{
			this.m_isEffectLock = true;
		}
		this.SetupBoard(data);
		this.SetupArrow(data);
		this.UpdateEffectSetting();
	}

	private void SetupBoard(ServerWheelOptionsData data)
	{
		if (data != null)
		{
			int rouletteBoardPattern = data.GetRouletteBoardPattern();
			if (this.m_pattern != null && this.m_pattern.Count > 0)
			{
				for (int i = 0; i < this.m_pattern.Count; i++)
				{
					RouletteBoardPattern rouletteBoardPattern2 = this.m_pattern[i];
					if (rouletteBoardPattern2 != null)
					{
						if (rouletteBoardPattern == i)
						{
							rouletteBoardPattern2.Setup(this, this.m_orgRouletteItem, 0);
							this.m_currentBoardPattern = rouletteBoardPattern2;
						}
						else
						{
							rouletteBoardPattern2.Reset();
						}
					}
				}
			}
		}
	}

	private void SetupArrow(ServerWheelOptionsData data)
	{
		if (this.m_arrow != null)
		{
			this.m_arrow.SetActive(true);
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_arrow, "img_roulette_arrow_0");
			if (uISprite != null && data != null)
			{
				uISprite.spriteName = data.GetRouletteArrowSprite();
			}
			this.m_currentDegree = 0f;
			this.m_currentArrowPos = -1;
			this.m_arrow.transform.rotation = Quaternion.Euler(42f, 9.139092E-05f, 0f);
			if (this.m_currentBoardPattern != null)
			{
				int cellIndex = this.m_currentBoardPattern.GetCellIndex(this.m_currentDegree);
				if (cellIndex >= 0)
				{
					this.m_currentArrowPos = cellIndex;
					this.m_currentBoardPattern.SetCurrentCell(this.m_currentArrowPos);
				}
			}
		}
	}

	protected override void UpdateParts()
	{
		if (base.isSpin && this.m_currentDegreeMax > 0f && this.m_arrow != null)
		{
			bool flag = false;
			if (this.m_currentDegree < this.m_currentDegreeMax)
			{
				float arrowMove = this.GetArrowMove();
				if (this.m_arrowSpeed > arrowMove)
				{
					flag = true;
				}
				this.m_currentDegree += arrowMove;
				if (this.m_currentDegreeMax <= this.m_currentDegree)
				{
					if (this.m_parent != null)
					{
						this.m_parent.OnRouletteSpinEnd();
					}
					this.m_currentDegree = this.m_currentDegreeMax;
					int currentDegreeRot = this.GetCurrentDegreeRot();
					if (currentDegreeRot > 0)
					{
						this.m_currentDegree -= (float)currentDegreeRot * 360f;
					}
					this.m_currentDegreeMax = 0f;
				}
			}
			if (this.m_partsUpdateCount % 2L == 0L || flag)
			{
				int cellIndex = this.m_currentBoardPattern.GetCellIndex(this.m_currentDegree);
				if (cellIndex >= 0)
				{
					if (this.m_currentArrowPos != cellIndex)
					{
						base.wheel.PlaySe(ServerWheelOptionsData.SE_TYPE.Arrow, 0f);
					}
					this.m_currentArrowPos = cellIndex;
					this.m_currentBoardPattern.SetCurrentCell(this.m_currentArrowPos);
				}
			}
			this.m_arrow.transform.rotation = Quaternion.Euler(42f, 9.139092E-05f, -this.m_currentDegree);
		}
	}

	public override void UpdateEffectSetting()
	{
		if (this.m_pattern != null && this.m_pattern.Count > 0)
		{
			for (int i = 0; i < this.m_pattern.Count; i++)
			{
				RouletteBoardPattern rouletteBoardPattern = this.m_pattern[i];
				if (rouletteBoardPattern != null)
				{
					rouletteBoardPattern.UpdateEffectSetting();
				}
			}
		}
		if (!base.parent.IsEffect(RouletteTop.ROULETTE_EFFECT_TYPE.BOARD))
		{
			this.m_isEffectLock = true;
		}
	}

	public override void DestroyParts()
	{
		if (this.m_currentBoardPattern != null)
		{
			this.m_currentBoardPattern.Reset();
		}
		this.m_currentBoardPattern = null;
		base.DestroyParts();
	}

	public override void OnSpinStart()
	{
		this.ONE_FPS_TIME = 1f / (float)Application.targetFrameRate;
		this.m_degreeSlow = 180f;
		this.m_degreeSlowLast = 1f;
		this.m_currentDegreeMax = 9999999f;
	}

	public override void OnSpinSkip()
	{
		if (base.spinDecisionIndex == -1)
		{
			return;
		}
		float num = this.m_currentDegreeMax - this.m_currentDegree;
		if (num >= this.m_degreeSlow * 0.5f)
		{
			float num2 = (float)((int)((num - this.m_degreeSlow * 0.5f) / 360f));
			if (num2 > 0f)
			{
				this.m_currentDegree += 360f * num2;
			}
		}
	}

	public override void OnSpinDecision()
	{
		if (base.spinDecisionIndex == -1)
		{
			return;
		}
		int currentDegreeRot = this.GetCurrentDegreeRot();
		int num = 3;
		if (currentDegreeRot >= 2)
		{
			num = currentDegreeRot + 1;
		}
		if (this.m_currentBoardPattern != null)
		{
			float num2;
			float num3;
			float num4;
			this.m_currentBoardPattern.GetCellData(base.spinDecisionIndex, out num2, out num3, out num4);
			float num5 = num3 - num2;
			float num6 = UnityEngine.Random.Range(0f, 1f);
			float num7;
			if (num4 > 0.8f)
			{
				num7 = num2 + num5 * num6;
			}
			else
			{
				num6 = 1f - num6 * num6;
				if ((int)(this.m_currentDegree * 100f) % 2 == 0)
				{
					num7 = num2 + num5 * num6;
				}
				else
				{
					num7 = num3 - num5 * num6;
				}
				if (num6 < 0.8f && num4 < 0.5f)
				{
					num4 = 0.5f;
				}
			}
			float currentDegreeMax = (float)num * 360f + num7;
			this.m_degreeSlow = 180f + UnityEngine.Random.Range(0f, 30f);
			if (num4 < 1f)
			{
				this.m_degreeSlow += UnityEngine.Random.Range(10f, 30f);
				if (num4 < 0.5f)
				{
					this.m_degreeSlow += UnityEngine.Random.Range(30f, 50f);
				}
			}
			this.m_currentDegreeMax = currentDegreeMax;
			this.m_degreeSlowLast = num4;
		}
	}

	public override void OnSpinEnd()
	{
	}

	public override void OnSpinError()
	{
	}

	private float GetLastSlowPoint()
	{
		float result = this.m_degreeSlow * 0.4f;
		if (this.m_degreeSlowLast < 1f)
		{
			result = this.m_degreeSlow * (0.4f + (1f - this.m_degreeSlowLast) * 0.02f);
		}
		return result;
	}

	private int GetCurrentDegreeRot()
	{
		return (int)(this.m_currentDegree / 360f);
	}

	private int GetEndDegreeRot()
	{
		float num = this.m_currentDegreeMax - this.m_currentDegree;
		int num2 = (int)(num / 360f);
		if (num2 < 0)
		{
			num2 = 0;
		}
		return num2;
	}

	private float GetEndDegreeRotFloat()
	{
		float num = this.m_currentDegreeMax - this.m_currentDegree;
		float num2 = num / 360f;
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		return num2;
	}

	private float GetArrowMove()
	{
		if (this.m_partsUpdateCount % 7L == 0L || this.m_arrowSpeed <= 0.5f)
		{
			this.m_timeRate = Time.deltaTime / this.ONE_FPS_TIME;
			if (this.m_timeRate > 1.2f)
			{
				this.m_timeRate = 1.2f;
			}
			else if (this.m_timeRate < 0.9f)
			{
				this.m_timeRate = 0.9f;
			}
			this.m_arrowSpeed = 10f * this.m_timeRate;
		}
		float num = this.m_arrowSpeed;
		float num2 = this.GetEndDegreeRotFloat();
		if (num2 > 3f)
		{
			num2 = 3f;
		}
		if (num2 > 0f)
		{
			num *= 1f + num2 * 0.25f;
		}
		if (num2 < 1.5f && this.m_currentDegreeMax - this.m_currentDegree <= this.m_degreeSlow)
		{
			float num3 = this.m_currentDegreeMax - this.m_currentDegree;
			float num4 = num3 / this.m_degreeSlow;
			if (num4 < 0f)
			{
				num4 = 0f;
			}
			if (num4 > 1f)
			{
				num4 = 1f;
			}
			float num5 = num4;
			if (num5 > 1f)
			{
				num5 = 1f;
			}
			if (num5 < 0.025f * this.m_degreeSlowLast)
			{
				num5 = 0.025f * this.m_degreeSlowLast;
			}
			num *= num5;
		}
		return num;
	}
}
