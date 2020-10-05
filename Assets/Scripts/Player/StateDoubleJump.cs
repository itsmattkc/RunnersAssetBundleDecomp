using Message;
using System;
using Tutorial;
using UnityEngine;

namespace Player
{
	public class StateDoubleJump : FSMState<CharacterState>
	{
		private float m_jumpForce;

		private float m_speed;

		private float m_addForceTmer;

		private float m_addAcc;

		private CharacterLoopEffect m_effect;

		public override void Enter(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.Air);
			StateUtil.SetAirMovementToRotateGround(context, true);
			this.m_jumpForce = context.Parameter.m_doubleJumpForce;
			this.m_addForceTmer = context.Parameter.m_doubleJumpAddSec;
			this.m_addAcc = context.Parameter.m_doubleJumpAddAcc;
			this.m_speed = Mathf.Max(context.Movement.GetForwardVelocityScalar(), 0f);
			context.GetAnimator().CrossFade("Jump", 0.05f);
			context.OnAttack(AttackPower.PlayerSpin, DefensePower.PlayerSpin);
			StateUtil.SetAttackAttributePowerIfPowerType(context, true);
			this.m_effect = context.GetSpinAttackEffect();
			if (this.m_effect != null)
			{
				this.m_effect.SetValid(true);
			}
			this.StartJump(context);
			StateUtil.SetSpecialtyJumpMagnet(context, CharacterAttribute.SPEED, ChaoAbility.MAGNET_SPEED_TYPE_JUMP, true);
			StateUtil.SetSpecialtyJumpDestroyEnemy(ChaoAbility.JUMP_DESTROY_ENEMY_AND_TRAP);
			StateUtil.SetSpecialtyJumpDestroyEnemy(ChaoAbility.JUMP_DESTROY_ENEMY);
		}

		public override void Leave(CharacterState context)
		{
			context.OffAttack();
			if (this.m_effect != null)
			{
				this.m_effect.SetValid(false);
			}
			StateUtil.SetSpecialtyJumpMagnet(context, CharacterAttribute.SPEED, ChaoAbility.MAGNET_SPEED_TYPE_JUMP, false);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			Vector3 vector = context.Movement.VertVelocity;
			Vector3 a = context.Movement.Velocity - vector;
			a = context.Movement.GetForwardDir() * StateUtil.GetForwardSpeedAir(context, context.DefaultSpeed, deltaTime);
			vector += context.Movement.GetGravity() * deltaTime;
			if (this.m_addForceTmer >= 0f)
			{
				this.m_addForceTmer -= deltaTime;
				vector += context.Movement.GetUpDir() * this.m_addAcc * deltaTime;
			}
			context.Movement.Velocity = a + vector;
			STATE_ID state = STATE_ID.Non;
			if (StateUtil.CheckHitWallAndGoDeadOrStumble(context, deltaTime, ref state))
			{
				context.ChangeState(state);
				return;
			}
			if (context.m_input.IsTouched())
			{
				STATE_ID sTATE_ID = STATE_ID.Non;
				if (StateUtil.GetNextStateToAirAttack(context, ref sTATE_ID, false))
				{
					if (sTATE_ID == STATE_ID.DoubleJump)
					{
						this.StartJump(context);
					}
					else if (sTATE_ID != STATE_ID.Non)
					{
						ObjUtil.SendMessageTutorialClear(EventID.DOUBLE_JUMP);
						context.ChangeState(sTATE_ID);
					}
					return;
				}
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

		private void StartJump(CharacterState context)
		{
			context.Movement.Velocity = context.transform.forward * this.m_speed + this.m_jumpForce * Vector3.up;
			StateUtil.Create2ndJumpEffect(context);
			context.AddAirAction();
		}
	}
}
