using System;
using UnityEngine;

namespace Chao
{
	public class RadiconMovePursue : ChaoMoveBase
	{
		private enum PhaseXAxis
		{
			DEFAULT,
			CHARA_SPEED_UP,
			CHARA_SPEED_DOWN
		}

		private const float SPEED_UP_THRESHOLD_VEC = 3f;

		private const float MAX_OFFESET_X = 3f;

		private const float MAX_OFFESET_Y = 5f;

		private const float SPEED_DOWN_THRESHOLD_DISTANCE = 2.9f;

		private const float SPEED_DOWN_THRESHOLD_VEC = 4f;

		private const float MaxDist_V = 7f;

		private const float FirstV_Speed = 0.05f;

		private const float VertPursue_Height = 2.5f;

		private float m_speedOffsetX;

		private float m_vertSpeed;

		private float m_radicon_v_acc = 1f;

		private float m_radicon_v_dec = 1f;

		private float m_radicon_v_vel = 1f;

		private float m_speedUpThreshold = 4f;

		private Vector3 m_targetPos = new Vector3(0f, 0f, 0f);

		private Vector3 m_preTargetPos = new Vector3(0f, 0f, 0f);

		private Vector3 m_prePosition = new Vector3(0f, 0f, 0f);

		private Vector3 m_basePosition = new Vector3(0f, 0f, 0f);

		private Vector3 m_offsetRadicon = new Vector3(0f, 0f, 0f);

		private RadiconMovePursue.PhaseXAxis m_phaseXAxis;

		public override void Enter(ChaoMovement context)
		{
			this.m_offsetRadicon = context.OffsetPosition;
			this.m_preTargetPos = context.TargetPosition + this.m_offsetRadicon;
			if (context.FromComeIn)
			{
				this.m_basePosition = this.m_preTargetPos;
				this.m_speedOffsetX = 0f;
				this.m_phaseXAxis = RadiconMovePursue.PhaseXAxis.DEFAULT;
			}
			else
			{
				this.m_basePosition = context.Position;
				this.m_speedOffsetX = this.m_basePosition.x - this.m_preTargetPos.x;
				this.m_phaseXAxis = RadiconMovePursue.PhaseXAxis.DEFAULT;
				if (this.m_speedOffsetX < 0f)
				{
					this.m_speedOffsetX = 0f;
				}
			}
			this.m_prePosition = this.m_basePosition;
			this.m_vertSpeed = (Vector3.Dot(context.MovedVelocity, ChaoMovement.VertDir) * ChaoMovement.VertDir).magnitude;
			this.m_radicon_v_acc = context.ParameterData.m_radicon_v_acc_speed;
			this.m_radicon_v_dec = context.ParameterData.m_radicon_v_dec_speed;
			this.m_radicon_v_vel = context.ParameterData.m_radicon_v_vel;
			this.m_speedUpThreshold = 3f;
		}

		public override void Leave(ChaoMovement context)
		{
		}

		public override void Step(ChaoMovement context, float deltaTime)
		{
			this.m_targetPos = context.TargetPosition + this.m_offsetRadicon;
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
				this.m_phaseXAxis = RadiconMovePursue.PhaseXAxis.DEFAULT;
			}
			Vector3 vector2 = this.m_targetPos - this.m_basePosition;
			Vector3 b = Vector3.Dot(vector2, ChaoMovement.HorzDir) * ChaoMovement.HorzDir;
			Vector3 subVert = vector2 - b;
			this.CalcXPosition(context, deltaTime);
			this.CalcVertVelocity(context, deltaTime, subVert);
			Vector3 prePosition = this.m_prePosition;
			prePosition.x = this.m_targetPos.x - this.m_speedOffsetX;
			prePosition.y = this.m_prePosition.y + deltaTime * this.m_vertSpeed * subVert.y;
			this.m_basePosition = prePosition;
			if (this.m_basePosition.x < this.m_prePosition.x)
			{
				this.m_basePosition.x = this.m_prePosition.x;
				if (this.m_phaseXAxis == RadiconMovePursue.PhaseXAxis.CHARA_SPEED_UP)
				{
					this.m_phaseXAxis = RadiconMovePursue.PhaseXAxis.CHARA_SPEED_DOWN;
				}
			}
			context.Position = this.m_basePosition + context.Hovering;
			this.m_preTargetPos = this.m_targetPos;
			this.m_prePosition = this.m_basePosition;
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

		private void CalcXPosition(ChaoMovement context, float deltaTime)
		{
			bool flag = this.IsXAxisSpeedUp(context);
			if (flag)
			{
				this.m_phaseXAxis = RadiconMovePursue.PhaseXAxis.CHARA_SPEED_UP;
			}
			if (this.m_phaseXAxis == RadiconMovePursue.PhaseXAxis.CHARA_SPEED_UP)
			{
				this.m_speedOffsetX += this.m_radicon_v_dec * deltaTime;
				if (this.m_speedOffsetX > 3f)
				{
					this.m_speedOffsetX = 3f;
				}
				if (!flag && this.m_speedOffsetX > 2.9f)
				{
					this.m_phaseXAxis = RadiconMovePursue.PhaseXAxis.CHARA_SPEED_DOWN;
				}
				else if (this.IsXAxisSpeedDown(context))
				{
					this.m_phaseXAxis = RadiconMovePursue.PhaseXAxis.CHARA_SPEED_DOWN;
				}
			}
			else if (this.m_phaseXAxis == RadiconMovePursue.PhaseXAxis.CHARA_SPEED_DOWN)
			{
				this.m_speedOffsetX -= this.m_radicon_v_acc * deltaTime;
				if (this.m_speedOffsetX < 0f)
				{
					this.m_speedOffsetX = 0f;
					this.m_phaseXAxis = RadiconMovePursue.PhaseXAxis.DEFAULT;
				}
			}
			else
			{
				this.m_speedOffsetX = 0f;
			}
		}

		private void CalcVertVelocity(ChaoMovement context, float deltaTime, Vector3 subVert)
		{
			float magnitude = subVert.magnitude;
			if (this.m_vertSpeed < 0.05f)
			{
				if (magnitude > 2.5f)
				{
					this.m_vertSpeed = 0.05f;
				}
			}
			else
			{
				float num = 7f;
				if (magnitude < this.m_vertSpeed * deltaTime)
				{
					this.m_vertSpeed = 0f;
				}
				else if (magnitude > 7f)
				{
					this.m_vertSpeed = this.m_radicon_v_vel;
				}
				else
				{
					this.m_vertSpeed = Mathf.Min(this.m_vertSpeed, this.m_radicon_v_vel);
					float num2 = Mathf.Lerp(0f, this.m_radicon_v_vel, magnitude / num);
					if (this.m_vertSpeed < num2)
					{
						this.m_vertSpeed = Mathf.MoveTowards(this.m_vertSpeed, num2, this.m_radicon_v_acc * deltaTime);
					}
					else
					{
						this.m_vertSpeed = Mathf.MoveTowards(this.m_vertSpeed, num2, this.m_radicon_v_dec * deltaTime);
					}
				}
			}
		}
	}
}
