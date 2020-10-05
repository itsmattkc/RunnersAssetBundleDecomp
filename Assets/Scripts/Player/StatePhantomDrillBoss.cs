using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StatePhantomDrillBoss : FSMState<CharacterState>
	{
		private enum Mode
		{
			Down,
			Run,
			Up
		}

		private const float Speed = 25f;

		private const float SeachGroundLength = 30f;

		private const float DigLength = 2f;

		private float m_time;

		private bool m_returnFromPhantom;

		private float m_speed;

		private GameObject m_boss;

		private StatePhantomDrillBoss.Mode m_mode;

		private GameObject m_effect;

		private GameObject m_truck;

		private Vector3 m_prevPosition;

		private bool m_nowInDirt;

		public override void Enter(CharacterState context)
		{
			StateUtil.SetRotation(context, Vector3.up, CharacterDefs.BaseFrontTangent);
			this.m_effect = PhantomDrillUtil.ChangeVisualOnEnter(context);
			this.m_truck = PhantomDrillUtil.CreateTruck(context);
			StateUtil.SetNotDrawItemEffect(context, true);
			context.OnAttack(AttackPower.PlayerColorPower, DefensePower.PlayerColorPower);
			context.OnAttackAttribute(AttackAttribute.PhantomDrill);
			this.m_time = 5f;
			this.m_returnFromPhantom = false;
			this.m_speed = 25f;
			this.m_prevPosition = context.Position;
			this.m_nowInDirt = false;
			StateUtil.DeactiveInvincible(context);
			StateUtil.SendMessageTransformPhantom(context, PhantomType.DRILL);
			MsgBossInfo bossInfo = StateUtil.GetBossInfo(null);
			if (bossInfo != null && bossInfo.m_succeed)
			{
				this.m_boss = bossInfo.m_boss;
				this.GoModeDown(context);
			}
			else
			{
				this.m_returnFromPhantom = true;
			}
		}

		public override void Leave(CharacterState context)
		{
			StateUtil.SetRotation(context, Vector3.up, CharacterDefs.BaseFrontTangent);
			context.OffAttack();
			StateUtil.SetNotDrawItemEffect(context, false);
			PhantomDrillUtil.ChangeVisualOnLeave(context, this.m_effect);
			PhantomDrillUtil.DestroyTruck(this.m_truck);
			this.m_effect = null;
			this.m_truck = null;
			StateUtil.SendMessageReturnFromPhantom(context, PhantomType.DRILL);
			this.m_boss = null;
			context.SetChangePhantomCancel(ItemType.UNKNOWN);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			switch (this.m_mode)
			{
			case StatePhantomDrillBoss.Mode.Down:
				this.StepModeDown(context);
				break;
			case StatePhantomDrillBoss.Mode.Run:
				this.StepModeRun(context);
				break;
			case StatePhantomDrillBoss.Mode.Up:
				this.StepModeUp(context);
				break;
			}
			this.m_time -= deltaTime;
			if (this.m_time < 0f || this.m_returnFromPhantom)
			{
				StateUtil.ReturnFromPhantomAndChangeState(context, PhantomType.DRILL, this.m_returnFromPhantom);
				return;
			}
			bool nowInDirt = this.m_nowInDirt;
			this.m_nowInDirt = PhantomDrillUtil.CheckTruckDraw(context, this.m_truck);
			if ((nowInDirt && !this.m_nowInDirt) || (!nowInDirt && this.m_nowInDirt))
			{
				PhantomDrillUtil.CheckAndCreateFogEffect(context, !nowInDirt && this.m_nowInDirt, this.m_prevPosition);
			}
			this.m_prevPosition = context.Position;
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

		private void GoModeDown(CharacterState context)
		{
			Vector3 gravityDir = context.Movement.GetGravityDir();
			Vector3 position = context.Position;
			RaycastHit raycastHit;
			if (Physics.Raycast(context.Position, gravityDir, out raycastHit, 30f))
			{
				position = raycastHit.point + gravityDir * 2f;
			}
			context.ChangeMovement(MOVESTATE_ID.GoTarget);
			CharacterMoveTarget movementState = context.GetMovementState<CharacterMoveTarget>();
			if (movementState != null)
			{
				movementState.SetTargetAndSpeed(context.Movement, position, Quaternion.identity, this.m_speed);
				movementState.SetRotateVelocityDir(true);
			}
			this.m_mode = StatePhantomDrillBoss.Mode.Down;
		}

		private void GoModeRun(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.GoTargetBoss);
			CharacterMoveTargetBoss currentState = context.Movement.GetCurrentState<CharacterMoveTargetBoss>();
			if (currentState != null)
			{
				currentState.SetTarget(this.m_boss);
				currentState.SetSpeed(this.m_speed);
				currentState.SetRotateVelocityDir(true);
				currentState.SetOnlyHorizon(true);
			}
			this.m_mode = StatePhantomDrillBoss.Mode.Run;
		}

		private void GoModeUp(CharacterState context)
		{
			CharacterMoveTargetBoss currentState = context.Movement.GetCurrentState<CharacterMoveTargetBoss>();
			if (currentState != null)
			{
				currentState.SetTarget(this.m_boss);
				currentState.SetSpeed(this.m_speed);
				currentState.SetRotateVelocityDir(true);
				currentState.SetOnlyHorizon(false);
			}
			this.m_mode = StatePhantomDrillBoss.Mode.Up;
		}

		private void StepModeDown(CharacterState context)
		{
			CharacterMoveTarget movementState = context.GetMovementState<CharacterMoveTarget>();
			if (movementState != null)
			{
				if (movementState.DoesReachTarget())
				{
					this.GoModeRun(context);
				}
			}
			else
			{
				this.m_returnFromPhantom = true;
			}
		}

		private void StepModeRun(CharacterState context)
		{
			CharacterMoveTargetBoss movementState = context.GetMovementState<CharacterMoveTargetBoss>();
			if (movementState != null)
			{
				if (movementState.DoesReachTarget())
				{
					this.GoModeUp(context);
				}
				else
				{
					this.m_returnFromPhantom |= movementState.IsTargetNotFound();
				}
			}
			else
			{
				this.m_returnFromPhantom = true;
			}
		}

		private void StepModeUp(CharacterState context)
		{
			CharacterMoveTargetBoss currentState = context.Movement.GetCurrentState<CharacterMoveTargetBoss>();
			if (currentState != null)
			{
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
