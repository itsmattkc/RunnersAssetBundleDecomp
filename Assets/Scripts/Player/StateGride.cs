using App.Utility;
using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StateGride : FSMState<CharacterState>
	{
		private enum Flag
		{
			DISABLE_GRAVITY
		}

		private float m_timer;

		private Bitset32 m_flag;

		private string m_effectname;

		private string m_attackEffectname;

		private bool DisableGravity
		{
			get
			{
				return this.m_flag.Test(0);
			}
			set
			{
				this.m_flag.Set(0, value);
			}
		}

		public override void Enter(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.Air);
			context.GetAnimator().CrossFade("SecondJump", 0.1f);
			CharaSEUtil.PlayPowerAttackSE(context.charaType);
			context.Movement.Velocity = context.Movement.GetForwardDir() * context.DefaultSpeed * context.Parameter.m_powerGrideSpeedRate + -context.Movement.GetGravityDir() * context.Parameter.m_grideFirstUpForce;
			StateUtil.SetAirMovementToRotateGround(context, true);
			context.OnAttack(AttackPower.PlayerPower, DefensePower.PlayerPower);
			context.OnAttackAttribute(AttackAttribute.Power);
			this.m_effectname = "ef_pl_" + context.CharacterName.ToLower() + "_attack_aura01";
			this.m_attackEffectname = "ef_pl_" + context.CharacterName.ToLower() + "_attack01";
			GameObject gameobj = StateUtil.CreateEffect(context, this.m_effectname, false, ResourceCategory.CHARA_EFFECT);
			StateUtil.SetObjectLocalPositionToCenter(context, gameobj);
			this.m_timer = 0f;
			this.m_flag.Reset();
			this.DisableGravity = true;
			context.AddAirAction();
			StateUtil.ThroughBreakable(context, true);
			StateUtil.SetSpecialtyJumpMagnet(context, CharacterAttribute.POWER, ChaoAbility.MAGNET_POWER_TYPE_JUMP, true);
			StateUtil.SetSpecialtyJumpDestroyEnemy(ChaoAbility.JUMP_DESTROY_ENEMY_AND_TRAP);
			StateUtil.SetSpecialtyJumpDestroyEnemy(ChaoAbility.JUMP_DESTROY_ENEMY);
		}

		public override void Leave(CharacterState context)
		{
			context.OffAttack();
			StateUtil.StopEffect(context, this.m_effectname, 0.5f);
			StateUtil.ThroughBreakable(context, false);
			StateUtil.SetSpecialtyJumpMagnet(context, CharacterAttribute.POWER, ChaoAbility.MAGNET_POWER_TYPE_JUMP, false);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			this.m_timer += deltaTime;
			if (this.DisableGravity && this.m_timer > context.Parameter.m_disableGravityTime)
			{
				this.DisableGravity = false;
			}
			Vector3 a = context.Movement.VertVelocity;
			if (!this.DisableGravity)
			{
				a += context.Movement.GetGravity() * context.Parameter.m_grideGravityRate * deltaTime;
			}
			Vector3 b = context.Movement.GetForwardDir() * context.DefaultSpeed * context.Parameter.m_powerGrideSpeedRate;
			context.Movement.Velocity = a + b;
			if (context.m_input.IsTouched() && StateUtil.CheckAndChangeStateToAirAttack(context, true, false))
			{
				return;
			}
			if (context.Movement.IsOnGround())
			{
				if (StateUtil.ChangeToJumpStateIfPrecedeInputTouch(context, 0.1f, false))
				{
					return;
				}
				StateUtil.SetVelocityForwardRun(context, false);
				StateUtil.NowLanding(context, false);
				context.ChangeState(STATE_ID.Run);
				return;
			}
			else
			{
				if (this.m_timer > context.Parameter.m_grideTime)
				{
					StateUtil.SetVelocityForwardRun(context, true);
					context.ChangeState(STATE_ID.Fall);
					return;
				}
				return;
			}
		}

		public override bool DispatchMessage(CharacterState context, int messageId, MessageBase msg)
		{
			if (messageId != 16385)
			{
				return false;
			}
			StateUtil.CreateEffect(context, context.Position, context.transform.rotation, this.m_attackEffectname, true, ResourceCategory.CHARA_EFFECT);
			context.Movement.Velocity = context.Movement.GetForwardDir() * context.DefaultSpeed * context.Parameter.m_powerGrideSpeedRate;
			this.m_timer = 0f;
			return true;
		}
	}
}
