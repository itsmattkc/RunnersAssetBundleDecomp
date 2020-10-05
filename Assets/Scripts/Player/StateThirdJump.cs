using Message;
using System;
using Tutorial;
using UnityEngine;

namespace Player
{
	public class StateThirdJump : FSMState<CharacterState>
	{
		private float m_jumpForce;

		private float m_speed;

		private float m_addForceTmer;

		private float m_addAcc;

		private CharacterLoopEffect m_effect;

		private bool m_specialJumpMagnet;

		public override void Enter(CharacterState context)
		{
			Vector3 horzVelocity = context.Movement.HorzVelocity;
			bool flag = false;
			JumpParameter enteringParameter = context.GetEnteringParameter<JumpParameter>();
			if (enteringParameter != null)
			{
				flag = enteringParameter.m_onAir;
			}
			context.ChangeMovement(MOVESTATE_ID.Air);
			StateUtil.SetAirMovementToRotateGround(context, true);
			this.m_jumpForce = context.Parameter.m_jumpForce;
			this.m_addForceTmer = context.Parameter.m_jumpAddSec;
			this.m_addAcc = context.Parameter.m_jumpAddAcc;
			this.m_speed = horzVelocity.magnitude;
			context.Movement.Velocity = horzVelocity + this.m_jumpForce * Vector3.up;
			context.GetAnimator().CrossFade("ThirdJump", 0.05f);
			context.AddAirAction();
			context.OnAttack(AttackPower.PlayerSpin, DefensePower.PlayerSpin);
			StateUtil.SetAttackAttributePowerIfPowerType(context, true);
			if (this.m_effect != null)
			{
				this.m_effect.SetValid(true);
			}
			if (flag)
			{
				StateUtil.Create2ndJumpEffect(context);
			}
			else
			{
				StateUtil.CreateJumpEffect(context);
				CharaSEUtil.PlayJumpSE(context.charaType);
			}
			this.m_specialJumpMagnet = false;
			if (context.NumAirAction > 2)
			{
				StateUtil.SetSpecialtyJumpMagnet(context, CharacterAttribute.POWER, ChaoAbility.MAGNET_POWER_TYPE_JUMP, true);
				StateUtil.SetSpecialtyJumpDestroyEnemy(ChaoAbility.JUMP_DESTROY_ENEMY_AND_TRAP);
				StateUtil.SetSpecialtyJumpDestroyEnemy(ChaoAbility.JUMP_DESTROY_ENEMY);
				this.m_specialJumpMagnet = true;
			}
		}

		public override void Leave(CharacterState context)
		{
			context.OffAttack();
			if (this.m_effect != null)
			{
				this.m_effect.SetValid(false);
			}
			ObjUtil.SendMessageTutorialClear(EventID.JUMP);
			if (this.m_specialJumpMagnet)
			{
				StateUtil.SetSpecialtyJumpMagnet(context, CharacterAttribute.POWER, ChaoAbility.MAGNET_POWER_TYPE_JUMP, false);
			}
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			Vector3 vector = context.Movement.VertVelocity;
			Vector3 a = context.Movement.Velocity - vector;
			vector += context.Movement.GetGravity() * deltaTime;
			if (this.m_addForceTmer >= 0f)
			{
				this.m_addForceTmer -= deltaTime;
				vector += context.Movement.GetUpDir() * this.m_addAcc * deltaTime;
			}
			float magnitude = a.magnitude;
			if (magnitude < this.m_speed && magnitude < context.DefaultSpeed)
			{
				this.m_speed = context.DefaultSpeed;
			}
			float targetSpeed = Mathf.Max(context.DefaultSpeed, this.m_speed);
			a = context.Movement.GetForwardDir() * StateUtil.GetForwardSpeedAir(context, targetSpeed, deltaTime);
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
