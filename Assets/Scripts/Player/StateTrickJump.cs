using App.Utility;
using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StateTrickJump : FSMState<CharacterState>
	{
		private enum Flags
		{
			SUCCEED,
			ENABLE_TRICK,
			TRICK_END,
			ISFALL,
			AUTO_SUCCEED
		}

		private const float LerpDelta = 0.5f;

		private const float cos5 = 0.9962f;

		private bool m_jumpCamera;

		private float m_animTime = 0.2f;

		private Bitset32 m_flag;

		private float m_outOfControlTime;

		private int m_numTrick;

		private bool Succeed
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

		private bool EnableTrick
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

		private bool TrickEnd
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

		private bool Falling
		{
			get
			{
				return this.m_flag.Test(3);
			}
			set
			{
				this.m_flag.Set(3, value);
			}
		}

		private bool AutoSucceed
		{
			get
			{
				return this.m_flag.Test(4);
			}
			set
			{
				this.m_flag.Set(4, value);
			}
		}

		public override void Enter(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.Air);
			StateUtil.SetAirMovementToRotateGround(context, false);
			this.m_flag.Reset();
			this.m_outOfControlTime = 0f;
			Vector3 velocity = Vector3.zero;
			TrickJumpParameter enteringParameter = context.GetEnteringParameter<TrickJumpParameter>();
			if (enteringParameter != null)
			{
				context.Movement.ResetPosition(enteringParameter.m_position);
				StateUtil.SetRotation(context, -context.Movement.GetGravityDir());
				this.m_outOfControlTime = enteringParameter.m_outOfControlTime;
				float d = enteringParameter.m_firstSpeed;
				velocity = enteringParameter.m_rotation * Vector3.up * d;
				this.Succeed = enteringParameter.m_succeed;
			}
			StageAbilityManager instance = StageAbilityManager.Instance;
			if (instance != null)
			{
				ChaoAbility ability = ChaoAbility.JUMP_RAMP;
				if (instance.HasChaoAbility(ability))
				{
					float chaoAbilityValue = instance.GetChaoAbilityValue(ability);
					float num = UnityEngine.Random.Range(0f, 99.9f);
					if (chaoAbilityValue >= num)
					{
						if (!this.Succeed)
						{
							this.m_outOfControlTime = enteringParameter.m_succeedOutOfcontrol;
							float d = enteringParameter.m_succeedFirstSpeed;
							velocity = enteringParameter.m_succeedRotation * Vector3.up * d;
						}
						this.Succeed = true;
						instance.RequestPlayChaoEffect(ability);
					}
				}
			}
			context.Movement.Velocity = velocity;
			if (this.Succeed)
			{
				context.GetAnimator().CrossFade("TrickJumpIdle", 0.1f);
				this.EnableTrick = true;
				this.m_jumpCamera = true;
				SoundManager.SePlay("obj_jumpboard_ok", "SE");
				StateUtil.SetJumpRampMagnet(context, true);
				ChaoAbility ability2 = ChaoAbility.JUMP_RAMP_TRICK_SUCCESS;
				if (instance.HasChaoAbility(ability2))
				{
					float chaoAbilityValue2 = instance.GetChaoAbilityValue(ability2);
					float num2 = UnityEngine.Random.Range(0f, 99.9f);
					if (chaoAbilityValue2 >= num2)
					{
						this.AutoSucceed = true;
						ObjUtil.RequestStartAbilityToChao(ability2, false);
					}
				}
			}
			else
			{
				context.GetAnimator().CrossFade("Damaged", 0.1f);
				this.m_jumpCamera = false;
				SoundManager.SePlay("obj_jumpboard_ng", "SE");
			}
			context.OnAttack(AttackPower.PlayerSpin, DefensePower.PlayerSpin);
			StateUtil.SetAttackAttributePowerIfPowerType(context, true);
			context.SetNotCharaChange(true);
			this.m_numTrick = 0;
			context.ClearAirAction();
			if (this.m_jumpCamera && context.GetCamera() != null)
			{
				MsgPushCamera value = new MsgPushCamera(CameraType.JUMPBOARD, 0.5f, null);
				context.GetCamera().SendMessage("OnPushCamera", value);
			}
			StateUtil.ThroughBreakable(context, true);
		}

		public override void Leave(CharacterState context)
		{
			context.OffAttack();
			context.SetNotCharaChange(false);
			if (this.m_jumpCamera && context.GetCamera() != null)
			{
				MsgPopCamera value = new MsgPopCamera(CameraType.JUMPBOARD, 0.5f);
				context.GetCamera().SendMessage("OnPopCamera", value);
			}
			StateUtil.ThroughBreakable(context, false);
			StateUtil.SetJumpRampMagnet(context, false);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			this.m_outOfControlTime -= deltaTime;
			this.CheckTrick(context, deltaTime);
			if (this.Succeed && !this.TrickEnd && !this.EnableTrick)
			{
				this.CheckAnimation(context, deltaTime);
			}
			if (this.m_outOfControlTime < 0f)
			{
				StateUtil.ThroughBreakable(context, false);
				if (!this.Falling && !this.Succeed)
				{
					context.GetAnimator().CrossFade("Fall", 0.3f);
				}
				this.Falling = true;
				Vector3 vector = context.Movement.Velocity;
				vector += context.Movement.GetGravity() * deltaTime;
				context.Movement.Velocity = vector;
			}
			if (context.Movement.GetVertVelocityScalar() <= 0f && context.Movement.IsOnGround())
			{
				StateUtil.NowLanding(context, true);
				context.ChangeState(STATE_ID.Run);
				return;
			}
		}

		private void CheckTrick(CharacterState context, float deltaTime)
		{
			CharacterInput component = context.GetComponent<CharacterInput>();
			if (component != null && this.EnableTrick && !StateUtil.IsAnimationInTransition(context) && (this.AutoSucceed || component.IsTouched()))
			{
				int num = CharacterDefs.TrickScore[this.m_numTrick];
				MsgCaution caution = new MsgCaution(HudCaution.Type.TRICK0 + this.m_numTrick);
				HudCaution.Instance.SetCaution(caution);
				MsgCaution caution2 = new MsgCaution(HudCaution.Type.TRICK_BONUS_N, num);
				HudCaution.Instance.SetCaution(caution2);
				ObjUtil.SendMessageAddScore(num);
				ObjUtil.SendMessageScoreCheck(new StageScoreData(1, num));
				string text = "TrickJump";
				text += (this.m_numTrick % 3 + 1).ToString("D1");
				context.GetAnimator().CrossFade(text, 0.05f);
				GameObject gameobj = StateUtil.CreateEffect(context, "ef_pl_trick01", true);
				StateUtil.SetObjectLocalPositionToCenter(context, gameobj);
				this.EnableTrick = false;
				this.m_numTrick++;
				this.m_animTime = 0.25f;
				if (this.m_numTrick < 5)
				{
					SoundManager.SePlay("obj_jumpboard_trick", "SE");
				}
				else
				{
					SoundManager.SePlay("obj_jumpboard_trick_last", "SE");
				}
			}
		}

		private void CheckAnimation(CharacterState context, float deltaTime)
		{
			if (!this.EnableTrick && this.m_numTrick > 0)
			{
				if (this.m_animTime > 0f)
				{
					this.m_animTime -= deltaTime;
					if (this.m_animTime <= 0f)
					{
						context.GetAnimator().CrossFade("TrickJumpIdle", 0.05f);
						this.m_animTime = -1f;
					}
				}
				else if (this.m_numTrick >= 5)
				{
					context.GetAnimator().CrossFade("Fall", 0.5f);
					this.TrickEnd = true;
					this.EnableTrick = false;
				}
				else
				{
					this.EnableTrick = true;
				}
			}
		}

		public override bool DispatchMessage(CharacterState context, int messageId, MessageBase msg)
		{
			return this.Falling && StateUtil.ChangeAfterSpinattack(context, messageId, msg);
		}
	}
}
