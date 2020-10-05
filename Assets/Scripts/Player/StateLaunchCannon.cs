using App.Utility;
using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StateLaunchCannon : FSMState<CharacterState>
	{
		private enum Flags
		{
			MODEL_ON
		}

		private enum SubState
		{
			LAUNCH,
			FALL
		}

		private const float lerpDelta = 0.5f;

		private StateLaunchCannon.SubState m_substate;

		private float m_outOfControlTime;

		private float m_lerpRotate;

		private float m_speed;

		private float m_drawLength;

		private Vector3 m_startPosition;

		private Bitset32 m_flags;

		private GameObject m_effect;

		public override void Enter(CharacterState context)
		{
			StateUtil.ResetVelocity(context);
			context.ChangeMovement(MOVESTATE_ID.Air);
			StateUtil.SetAirMovementToRotateGround(context, false);
			this.m_outOfControlTime = 0f;
			this.m_lerpRotate = 0f;
			Vector3 velocity = Vector3.zero;
			this.m_startPosition = context.Position;
			this.m_drawLength = 0f;
			CannonLaunchParameter enteringParameter = context.GetEnteringParameter<CannonLaunchParameter>();
			if (enteringParameter != null)
			{
				Vector3 up = enteringParameter.m_rotation * Vector3.up;
				context.Movement.ResetPosition(enteringParameter.m_position);
				StateUtil.SetRotation(context, up);
				this.m_outOfControlTime = enteringParameter.m_outOfControlTime;
				this.m_speed = enteringParameter.m_firstSpeed;
				velocity = enteringParameter.m_rotation * Vector3.up * this.m_speed;
				this.m_drawLength = enteringParameter.m_height;
			}
			context.Movement.Velocity = velocity;
			context.GetAnimator().Play("Cannon");
			this.m_flags.Reset();
			context.OnAttack(AttackPower.PlayerPower, DefensePower.PlayerPower);
			context.OnAttackAttribute(AttackAttribute.Power);
			context.SetModelNotDraw(true);
			StateUtil.SetNotDrawItemEffect(context, true);
			StateUtil.ThroughBreakable(context, true);
			this.m_substate = StateLaunchCannon.SubState.LAUNCH;
			context.SetNotCharaChange(true);
			context.SetNotUseItem(true);
			StateUtil.SetCannonMagnet(context, true);
		}

		public override void Leave(CharacterState context)
		{
			context.OffAttack();
			context.SetModelNotDraw(false);
			StateUtil.SetNotDrawItemEffect(context, false);
			context.SetNotCharaChange(false);
			context.SetNotUseItem(false);
			StateUtil.ThroughBreakable(context, false);
			StateUtil.SetCannonMagnet(context, false);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			this.m_outOfControlTime -= deltaTime;
			Vector3 position = context.Position;
			StateLaunchCannon.SubState substate = this.m_substate;
			if (substate != StateLaunchCannon.SubState.LAUNCH)
			{
				if (substate == StateLaunchCannon.SubState.FALL)
				{
					Vector3 gravityDir = context.Movement.GetGravityDir();
					Vector3 a = context.Movement.GetForwardDir() * StateUtil.GetForwardSpeedAir(context, context.DefaultSpeed, deltaTime);
					Vector3 vertVelocity = context.Movement.VertVelocity;
					context.Movement.Velocity = a + vertVelocity + context.Movement.GetGravity() * deltaTime;
					if (this.m_lerpRotate < 1f)
					{
						this.m_lerpRotate = Mathf.Min(this.m_lerpRotate + 0.5f * deltaTime, 1f);
						Vector3 up;
						if (this.m_lerpRotate < 1f)
						{
							up = Vector3.Lerp(context.Movement.GetUpDir(), -gravityDir, this.m_lerpRotate);
						}
						else
						{
							up = -gravityDir;
						}
						MovementUtil.RotateByCollision(context.transform, context.GetComponent<CapsuleCollider>(), up);
					}
					if (context.m_input.IsTouched() && StateUtil.CheckAndChangeStateToAirAttack(context, true, false))
					{
						return;
					}
				}
			}
			else
			{
				if ((position - this.m_startPosition).sqrMagnitude > this.m_drawLength * this.m_drawLength)
				{
					context.SetModelNotDraw(false);
					StateUtil.SetNotDrawItemEffect(context, false);
					this.m_flags.Set(0, true);
				}
				if (this.m_outOfControlTime < 0f)
				{
					StateUtil.ThroughBreakable(context, false);
					context.GetAnimator().CrossFade("Fall", 0.5f);
					context.OffAttack();
					context.SetNotCharaChange(false);
					context.SetNotUseItem(false);
					this.m_substate = StateLaunchCannon.SubState.FALL;
				}
			}
			if (context.Movement.IsOnGround())
			{
				StateUtil.NowLanding(context, true);
				context.Movement.Velocity = context.transform.forward * this.m_speed;
				context.ChangeState(STATE_ID.Run);
				return;
			}
		}

		public override bool DispatchMessage(CharacterState context, int messageId, MessageBase msg)
		{
			return StateUtil.ChangeAfterSpinattack(context, messageId, msg);
		}
	}
}
