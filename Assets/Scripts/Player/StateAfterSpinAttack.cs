using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StateAfterSpinAttack : FSMState<CharacterState>
	{
		private float m_jumpForce;

		private float m_speed;

		private CharacterLoopEffect m_effect;

		public override void Enter(CharacterState context)
		{
			StateUtil.SetRotationOnGravityUp(context);
			context.ChangeMovement(MOVESTATE_ID.Air);
			StateUtil.SetAirMovementToRotateGround(context, true);
			this.m_jumpForce = context.Parameter.m_spinAttackForce;
			this.m_speed = Mathf.Max(context.Movement.GetForwardVelocityScalar(), 0f);
			Vector3 a = -context.Movement.GetGravityDir();
			Vector3 forward = context.transform.forward;
			context.Movement.Velocity = forward * this.m_speed + this.m_jumpForce * a;
			context.GetAnimator().CrossFade("Jump", 0.05f);
			context.OnAttack(AttackPower.PlayerSpin, DefensePower.PlayerSpin);
			StateUtil.SetAttackAttributePowerIfPowerType(context, true);
			context.SetAirAction(1);
			this.m_effect = context.GetSpinAttackEffect();
			if (this.m_effect != null)
			{
				this.m_effect.SetValid(true);
			}
		}

		public override void Leave(CharacterState context)
		{
			context.OffAttack();
			if (this.m_effect != null)
			{
				this.m_effect.SetValid(false);
			}
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			Vector3 vector = context.Movement.VertVelocity;
			Vector3 a = context.Movement.Velocity - vector;
			vector += context.Movement.GetGravity() * deltaTime;
			a = context.Movement.GetForwardDir() * StateUtil.GetForwardSpeedAir(context, context.DefaultSpeed, deltaTime);
			context.Movement.Velocity = a + vector;
			STATE_ID state = STATE_ID.Non;
			if (StateUtil.CheckHitWallAndGoDeadOrStumble(context, deltaTime, ref state))
			{
				context.ChangeState(state);
				return;
			}
			if (context.m_input.IsTouched() && StateUtil.CheckAndChangeStateToAirAttack(context, false, false))
			{
				return;
			}
			if (context.Movement.GetVertVelocityScalar() > 0f || !context.Movement.IsOnGround())
			{
				return;
			}
			if (StateUtil.ChangeToJumpStateIfPrecedeInputTouch(context, 0.1f, true))
			{
				return;
			}
			StateUtil.NowLanding(context, true);
			context.ChangeState(STATE_ID.Run);
		}

		public override bool DispatchMessage(CharacterState context, int messageId, MessageBase msg)
		{
			return StateUtil.ChangeAfterSpinattack(context, messageId, msg);
		}
	}
}
