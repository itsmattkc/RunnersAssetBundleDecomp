using App.Utility;
using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StateFly : FSMState<CharacterState>
	{
		private enum Flag
		{
			CANNOTFLY,
			NOWUP,
			HOLD
		}

		private const float ChaoAbilityExtendTimeRate = 2f;

		private float m_canFlyTime;

		private Bitset32 m_flag;

		private GameObject m_effect;

		private string m_effectName;

		private bool CannotFly
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

		private bool NowUp
		{
			get
			{
				return this.m_flag.Test(1);
			}
			set
			{
				this.m_flag.Set(1, value);
			}
		}

		private bool Hold
		{
			get
			{
				return this.m_flag.Test(2);
			}
			set
			{
				this.m_flag.Set(2, value);
			}
		}

		public override void Enter(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.Air);
			context.GetAnimator().CrossFade("SecondJump", 0.1f);
			CharaSEUtil.PlayFlySE(context.charaType);
			this.m_effectName = "ef_pl_" + context.CharacterName.ToLower() + "_fly01";
			this.m_flag.Reset();
			this.NowUp = true;
			this.Hold = true;
			this.m_canFlyTime = context.Parameter.m_canFlyTime;
			context.Movement.VertVelocity = -context.Movement.GetGravityDir() * context.Parameter.m_flyUpFirstSpeed;
			StateUtil.SetAirMovementToRotateGround(context, true);
			context.OnAttack(AttackPower.PlayerSpin, DefensePower.PlayerSpin);
			this.CreateEffect(context);
			context.AddAirAction();
			StateUtil.SetSpecialtyJumpMagnet(context, CharacterAttribute.FLY, ChaoAbility.MAGNET_FLY_TYPE_JUMP, true);
			StateUtil.SetSpecialtyJumpDestroyEnemy(ChaoAbility.JUMP_DESTROY_ENEMY_AND_TRAP);
			StateUtil.SetSpecialtyJumpDestroyEnemy(ChaoAbility.JUMP_DESTROY_ENEMY);
		}

		public override void Leave(CharacterState context)
		{
			context.OffAttack();
			this.DeleteEffect();
			context.SetStatus(Status.InvincibleByChao, false);
			StateUtil.SetSpecialtyJumpMagnet(context, CharacterAttribute.FLY, ChaoAbility.MAGNET_FLY_TYPE_JUMP, false);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			Vector3 a = context.Movement.GetForwardDir() * StateUtil.GetForwardSpeedAir(context, context.DefaultSpeed * context.Parameter.m_flySpeedRate, deltaTime);
			Vector3 b = Vector3.zero;
			float limitHeitht = context.Parameter.m_limitHeitht;
			Vector3 position = context.Position;
			StateUtil.GetBaseGroundPosition(context, ref position);
			if (context.m_input.IsHold() && !this.CannotFly)
			{
				float num = Vector3.Magnitude(context.Position - position);
				if (num < limitHeitht)
				{
					float d;
					if (!this.NowUp)
					{
						d = context.Parameter.m_flyUpFirstSpeed;
						this.m_canFlyTime -= context.Parameter.m_flyDecSec2ndPress;
						if (!this.Hold)
						{
							CharaSEUtil.PlayFlySE(context.charaType);
						}
					}
					else
					{
						float vertVelocityScalar = context.Movement.GetVertVelocityScalar();
						d = Mathf.Min(vertVelocityScalar + context.Parameter.m_flyUpForce * deltaTime, context.Parameter.m_flyUpSpeedMax);
					}
					b = -context.Movement.GetGravityDir() * d;
				}
				this.NowUp = true;
				this.Hold = true;
				if (this.m_effect == null)
				{
					this.CreateEffect(context);
				}
				this.m_canFlyTime -= deltaTime;
				if (this.m_canFlyTime < 0f)
				{
					this.CannotFly = true;
				}
			}
			else
			{
				this.NowUp = false;
				this.Hold = false;
				float vertVelocityScalar2 = context.Movement.GetVertVelocityScalar();
				if (vertVelocityScalar2 < -context.Parameter.m_flydownSpeedMax)
				{
					b = context.Parameter.m_flydownSpeedMax * context.Movement.GetGravityDir();
				}
				else
				{
					b = context.Movement.VertVelocity + context.Movement.GetGravity() * context.Parameter.m_flyGravityRate * deltaTime;
				}
				this.DeleteEffect();
			}
			context.Movement.Velocity = a + b;
			if (!context.Movement.IsOnGround())
			{
				return;
			}
			if (StateUtil.ChangeToJumpStateIfPrecedeInputTouch(context, 0.1f, false))
			{
				return;
			}
			StateUtil.NowLanding(context, false);
			context.ChangeState(STATE_ID.Run);
		}

		public override bool DispatchMessage(CharacterState context, int messageId, MessageBase msg)
		{
			return StateUtil.ChangeAfterSpinattack(context, messageId, msg);
		}

		private void CreateEffect(CharacterState context)
		{
			this.m_effect = StateUtil.CreateEffect(context, this.m_effectName, true, ResourceCategory.CHARA_EFFECT);
		}

		private void DeleteEffect()
		{
			StateUtil.DestroyParticle(this.m_effect, 1f);
			this.m_effect = null;
		}
	}
}
