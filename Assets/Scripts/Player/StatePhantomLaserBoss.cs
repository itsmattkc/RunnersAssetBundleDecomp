using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StatePhantomLaserBoss : FSMState<CharacterState>
	{
		private enum Mode
		{
			Up,
			GoTarget
		}

		private const float Speed = 25f;

		private float m_time;

		private bool m_returnFromPhantom;

		private float m_speed;

		private GameObject m_boss;

		private StatePhantomLaserBoss.Mode m_mode;

		public override void Enter(CharacterState context)
		{
			this.m_mode = StatePhantomLaserBoss.Mode.Up;
			StateUtil.SetRotation(context, Vector3.up, CharacterDefs.BaseFrontTangent);
			PhantomLaserUtil.ChangeVisualOnEnter(context);
			StateUtil.SetNotDrawItemEffect(context, true);
			context.OnAttack(AttackPower.PlayerColorPower, DefensePower.PlayerColorPower);
			context.OnAttackAttribute(AttackAttribute.PhantomLaser);
			SoundManager.SePlay("phantom_laser_shoot", "SE");
			this.m_time = 5f;
			this.m_returnFromPhantom = false;
			this.m_speed = 25f;
			StateUtil.DeactiveInvincible(context);
			StateUtil.SendMessageTransformPhantom(context, PhantomType.LASER);
			MsgBossInfo bossInfo = StateUtil.GetBossInfo(null);
			if (bossInfo != null && bossInfo.m_succeed)
			{
				this.m_boss = bossInfo.m_boss;
				this.GoModeUp(context, bossInfo.m_position);
			}
			else
			{
				this.m_returnFromPhantom = true;
			}
			StateUtil.SetPhantomMagnetColliderRange(context, PhantomType.LASER);
		}

		public override void Leave(CharacterState context)
		{
			StateUtil.SetRotation(context, Vector3.up, CharacterDefs.BaseFrontTangent);
			PhantomLaserUtil.ChangeVisualOnLeave(context);
			context.OffAttack();
			SoundManager.SeStop("phantom_laser_shoot", "SE");
			StateUtil.SetNotDrawItemEffect(context, false);
			StateUtil.SendMessageReturnFromPhantom(context, PhantomType.LASER);
			this.m_boss = null;
			context.SetChangePhantomCancel(ItemType.UNKNOWN);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			StatePhantomLaserBoss.Mode mode = this.m_mode;
			if (mode != StatePhantomLaserBoss.Mode.Up)
			{
				if (mode == StatePhantomLaserBoss.Mode.GoTarget)
				{
					this.StepModeGoTarget(context);
				}
			}
			else
			{
				this.StepModeUp(context);
			}
			this.m_time -= deltaTime;
			if (this.m_time < 0f || this.m_returnFromPhantom)
			{
				StateUtil.ResetVelocity(context);
				StateUtil.ReturnFromPhantomAndChangeState(context, PhantomType.LASER, this.m_returnFromPhantom);
				return;
			}
		}

		public override bool DispatchMessage(CharacterState context, int messageId, MessageBase msg)
		{
			if (messageId != 16385)
			{
				return false;
			}
			MsgHitDamageSucceed msgHitDamageSucceed = msg as MsgHitDamageSucceed;
			if (msgHitDamageSucceed != null && msgHitDamageSucceed.m_sender == this.m_boss)
			{
				this.m_returnFromPhantom = true;
			}
			return true;
		}

		private void GoModeUp(CharacterState context, Vector3 targetPos)
		{
			Vector3 lhs = targetPos - context.Position;
			Vector3 upDir = context.Movement.GetUpDir();
			Vector3 b = Vector3.Dot(lhs, upDir) * upDir;
			Vector3 position = context.Position + b;
			context.ChangeMovement(MOVESTATE_ID.GoTarget);
			CharacterMoveTarget movementState = context.GetMovementState<CharacterMoveTarget>();
			if (movementState != null)
			{
				movementState.SetTargetAndSpeed(context.Movement, position, Quaternion.identity, this.m_speed);
				movementState.SetRotateVelocityDir(true);
			}
			this.m_mode = StatePhantomLaserBoss.Mode.Up;
		}

		private void GoModeGoTarget(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.GoTargetBoss);
			CharacterMoveTargetBoss currentState = context.Movement.GetCurrentState<CharacterMoveTargetBoss>();
			if (currentState != null)
			{
				currentState.SetTarget(this.m_boss);
				currentState.SetSpeed(this.m_speed);
				currentState.SetRotateVelocityDir(true);
			}
			this.m_mode = StatePhantomLaserBoss.Mode.GoTarget;
		}

		private void StepModeUp(CharacterState context)
		{
			CharacterMoveTarget movementState = context.GetMovementState<CharacterMoveTarget>();
			if (movementState != null)
			{
				if (movementState.DoesReachTarget())
				{
					this.GoModeGoTarget(context);
				}
			}
			else
			{
				this.m_returnFromPhantom = true;
			}
		}

		private void StepModeGoTarget(CharacterState context)
		{
			CharacterMoveTargetBoss currentState = context.Movement.GetCurrentState<CharacterMoveTargetBoss>();
			if (currentState != null)
			{
				currentState.SetSpeed(this.m_speed);
				bool flag = currentState.IsTargetNotFound();
				this.m_returnFromPhantom |= flag;
			}
			else
			{
				this.m_returnFromPhantom = true;
			}
		}
	}
}
