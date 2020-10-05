using System;
using UnityEngine;

namespace Chao
{
	public class ChaoMovePursue : ChaoMoveBase
	{
		private enum PhaseXAxis
		{
			DEFAULT,
			CHARA_SPEED_UP,
			CHARA_SPEED_DOWN
		}

		private enum PhaseYAxis
		{
			DEFAULT,
			CHARA_UP,
			CHARA_DWON
		}

		private const float SPEED_DOWN_THRESHOLD_DISTANCE = 2.9f;

		private const float SPEED_DOWN_THRESHOLD_VEC = 4f;

		private const float SPEED_UP_THRESHOLD_VEC = 3.5f;

		private const float MAX_OFFESET_X = 3f;

		private const float MAX_OFFESET_Y = 5f;

		private const float PURSUE_MAX_SPEED_Y = 3f;

		private const float PURSUE_MIN_SPEED_Y = 2f;

		private const float Y_PURSUE_DISTANCE_THRESHOLD = 3f;

		private float m_deltaVecY;

		private float m_chao_v_acc = 1f;

		private float m_chao_v_dec = 1f;

		private float m_pursue_v_acc = 1f;

		private float m_speedOffsetX;

		private float m_speedOffsetY;

		private float m_speedUpThreshold = 4f;

		private Vector3 m_targetPos = new Vector3(0f, 0f, 0f);

		private Vector3 m_preTargetPos = new Vector3(0f, 0f, 0f);

		private Vector3 m_prePosition = new Vector3(0f, 0f, 0f);

		private Vector3 m_basePosition = new Vector3(0f, 0f, 0f);

		private Vector3 m_offsetChao = new Vector3(0f, 0f, 0f);

		private ChaoMovePursue.PhaseXAxis m_phaseXAxis;

		private ChaoMovePursue.PhaseYAxis m_phaseYAxis;

		public override void Enter(ChaoMovement context)
		{
			this.m_offsetChao = context.OffsetPosition;
			this.m_preTargetPos = context.TargetPosition + this.m_offsetChao;
			if (context.FromComeIn)
			{
				this.m_basePosition = this.m_preTargetPos;
				this.m_speedOffsetX = 0f;
				this.m_phaseXAxis = ChaoMovePursue.PhaseXAxis.DEFAULT;
			}
			else
			{
				this.m_basePosition = context.Position;
				this.m_speedOffsetX = this.m_basePosition.x - this.m_preTargetPos.x;
				this.m_phaseXAxis = ChaoMovePursue.PhaseXAxis.DEFAULT;
				if (this.m_speedOffsetX < 0f)
				{
					this.m_speedOffsetX = 0f;
				}
			}
			this.m_prePosition = this.m_basePosition;
			this.m_chao_v_acc = context.ParameterData.m_chao_v_acc_speed;
			this.m_chao_v_dec = context.ParameterData.m_chao_v_dec_speed;
			this.m_pursue_v_acc = context.Parameter.Data.m_chao_v_acc;
			this.m_deltaVecY = 0f;
			this.m_speedUpThreshold = 3.5f;
		}

		public override void Leave(ChaoMovement context)
		{
		}

		public override void Step(ChaoMovement context, float deltaTime)
		{
			this.m_targetPos = context.TargetPosition + this.m_offsetChao;
			if (!context.IsPlyayerMoved)
			{
				context.Position = this.m_basePosition + context.Hovering;
				this.m_preTargetPos = this.m_targetPos;
				return;
			}
			if (deltaTime <= 0f)
			{
				return;
			}
			if ((this.m_targetPos - this.m_preTargetPos).x < -100f)
			{
				this.m_preTargetPos.x = 0f;
				Vector3 vector = context.TargetPosition - this.m_preTargetPos;
				this.m_speedOffsetX = 0f;
				this.m_phaseXAxis = ChaoMovePursue.PhaseXAxis.DEFAULT;
			}
			this.CalcXPosition(context, deltaTime);
			this.CalcYPosition(context, deltaTime);
			Vector3 preTargetPos = this.m_preTargetPos;
			preTargetPos.x = this.m_targetPos.x - this.m_speedOffsetX;
			preTargetPos.y = this.m_basePosition.y + this.m_speedOffsetY;
			this.m_basePosition = preTargetPos;
			if (this.m_basePosition.x < this.m_prePosition.x)
			{
				this.m_basePosition.x = this.m_prePosition.x;
				if (this.m_phaseXAxis == ChaoMovePursue.PhaseXAxis.CHARA_SPEED_UP)
				{
					this.m_phaseXAxis = ChaoMovePursue.PhaseXAxis.CHARA_SPEED_DOWN;
				}
			}
			context.Position = this.m_basePosition + context.Hovering;
			this.m_preTargetPos = this.m_targetPos;
			this.m_prePosition = this.m_basePosition;
		}

		private void SetSpeedUpThreshold()
		{
			StageAbilityManager instance = StageAbilityManager.Instance;
			if (instance != null)
			{
				float num = 1f;
				if (instance.IsEasySpeed(PlayingCharacterType.MAIN))
				{
					this.m_speedUpThreshold += num;
				}
				if (instance.IsEasySpeed(PlayingCharacterType.SUB))
				{
					this.m_speedUpThreshold += num;
				}
			}
		}

		private bool IsXAxisSpeedUp(ChaoMovement context)
		{
			if (context.PlayerInfo != null)
			{
				float defaultSpeed = context.PlayerInfo.DefaultSpeed;
				return context.PlayerInfo.HorizonVelocity.x > defaultSpeed + this.m_speedUpThreshold;
			}
			return false;
		}

		private bool IsXAxisSpeedDown(ChaoMovement context)
		{
			return context.PlayerInfo != null && context.PlayerInfo.HorizonVelocity.x < context.PlayerInfo.DefaultSpeed - 4f;
		}

		private void CalcXPosition(ChaoMovement context, float deltaTime)
		{
			bool flag = this.IsXAxisSpeedUp(context);
			if (flag)
			{
				this.m_phaseXAxis = ChaoMovePursue.PhaseXAxis.CHARA_SPEED_UP;
			}
			if (this.m_phaseXAxis == ChaoMovePursue.PhaseXAxis.CHARA_SPEED_UP)
			{
				this.m_speedOffsetX += this.m_chao_v_dec * deltaTime;
				if (this.m_speedOffsetX > 3f)
				{
					this.m_speedOffsetX = 3f;
				}
				if (!flag && this.m_speedOffsetX > 2.9f)
				{
					this.m_phaseXAxis = ChaoMovePursue.PhaseXAxis.CHARA_SPEED_DOWN;
				}
				else if (this.IsXAxisSpeedDown(context))
				{
					this.m_phaseXAxis = ChaoMovePursue.PhaseXAxis.CHARA_SPEED_DOWN;
				}
			}
			else if (this.m_phaseXAxis == ChaoMovePursue.PhaseXAxis.CHARA_SPEED_DOWN)
			{
				this.m_speedOffsetX -= this.m_chao_v_acc * deltaTime;
				if (this.m_speedOffsetX < 0f)
				{
					this.m_speedOffsetX = 0f;
					this.m_phaseXAxis = ChaoMovePursue.PhaseXAxis.DEFAULT;
				}
			}
			else
			{
				this.m_speedOffsetX = 0f;
			}
		}

		private bool ChangePhaseY(ChaoMovePursue.PhaseYAxis phase)
		{
			if (this.m_phaseYAxis != phase)
			{
				this.m_phaseYAxis = phase;
				return true;
			}
			return false;
		}

		private void CalcYPosition(ChaoMovement context, float deltaTime)
		{
			float num = this.m_targetPos.y - this.m_basePosition.y;
			if (this.m_phaseYAxis == ChaoMovePursue.PhaseYAxis.DEFAULT)
			{
				if (num > this.m_offsetChao.y)
				{
					if (this.ChangePhaseY(ChaoMovePursue.PhaseYAxis.CHARA_UP))
					{
						this.m_deltaVecY = 0f;
					}
				}
				else
				{
					if (num >= -0.01f)
					{
						this.m_speedOffsetY = 0f;
						this.ChangePhaseY(ChaoMovePursue.PhaseYAxis.DEFAULT);
						return;
					}
					if (this.ChangePhaseY(ChaoMovePursue.PhaseYAxis.CHARA_DWON))
					{
						this.m_deltaVecY = 0f;
					}
				}
			}
			if (num < 3f)
			{
				this.m_deltaVecY -= this.m_pursue_v_acc * deltaTime;
				if (this.m_deltaVecY < 2f)
				{
					this.m_deltaVecY = 2f;
				}
			}
			else
			{
				this.m_deltaVecY += this.m_pursue_v_acc * deltaTime;
				if (this.m_deltaVecY > 3f)
				{
					this.m_deltaVecY = 3f;
				}
			}
			switch (this.m_phaseYAxis)
			{
			case ChaoMovePursue.PhaseYAxis.CHARA_UP:
			{
				float num2 = this.m_deltaVecY * deltaTime;
				float num3 = num - num2;
				if (num3 < 0f)
				{
					this.m_speedOffsetY = num;
					this.m_phaseYAxis = ChaoMovePursue.PhaseYAxis.DEFAULT;
				}
				else if (num3 > 5f)
				{
					this.m_speedOffsetY = num - 5f;
				}
				else
				{
					this.m_speedOffsetY = num2;
				}
				break;
			}
			case ChaoMovePursue.PhaseYAxis.CHARA_DWON:
			{
				float num4 = this.m_deltaVecY * deltaTime;
				float num5 = num + num4;
				if (num5 > 0f)
				{
					if (num5 > this.m_offsetChao.y)
					{
						this.m_speedOffsetY = -num4;
						this.m_phaseYAxis = ChaoMovePursue.PhaseYAxis.CHARA_UP;
					}
					else
					{
						this.m_speedOffsetY = num;
						this.m_phaseYAxis = ChaoMovePursue.PhaseYAxis.DEFAULT;
					}
				}
				else if (num5 < -5f)
				{
					this.m_speedOffsetY = num + 5f;
				}
				else
				{
					this.m_speedOffsetY = -num4;
				}
				break;
			}
			}
			if (-0.0001 < (double)this.m_speedOffsetY && this.m_speedOffsetY < 0.0001f)
			{
				this.m_speedOffsetY = 0f;
			}
		}
	}
}
