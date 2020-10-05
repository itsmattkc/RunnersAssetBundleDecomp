using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StatePhantomAsteroidBoss : FSMState<CharacterState>
	{
		private const float FirstSpeed = 10f;

		private const float MaxSpeed = 15f;

		private const float SpeedAcc = 15f;

		private float m_time;

		private GameObject m_effect;

		private bool m_returnFromPhantom;

		private float m_speed;

		private GameObject m_boss;

		public override void Enter(CharacterState context)
		{
			StateUtil.SetRotation(context, Vector3.up, CharacterDefs.BaseFrontTangent);
			this.m_effect = PhantomAsteroidUtil.ChangeVisualOnEnter(context);
			StateUtil.SetNotDrawItemEffect(context, true);
			context.OnAttack(AttackPower.PlayerColorPower, DefensePower.PlayerColorPower);
			context.OnAttackAttribute(AttackAttribute.PhantomAsteroid);
			this.m_time = 5f;
			StateUtil.DeactiveInvincible(context);
			StateUtil.SendMessageTransformPhantom(context, PhantomType.ASTEROID);
			this.m_returnFromPhantom = false;
			context.ChangeMovement(MOVESTATE_ID.GoTargetBoss);
			MsgBossInfo bossInfo = StateUtil.GetBossInfo(null);
			if (bossInfo != null && bossInfo.m_succeed)
			{
				this.m_boss = bossInfo.m_boss;
			}
			this.m_speed = 10f;
			CharacterMoveTargetBoss currentState = context.Movement.GetCurrentState<CharacterMoveTargetBoss>();
			if (currentState != null)
			{
				currentState.SetTarget(this.m_boss);
				currentState.SetSpeed(this.m_speed);
			}
			StateUtil.SetPhantomMagnetColliderRange(context, PhantomType.ASTEROID);
		}

		public override void Leave(CharacterState context)
		{
			context.OffAttack();
			PhantomAsteroidUtil.ChangeVisualOnLeave(context, this.m_effect);
			this.m_effect = null;
			StateUtil.SetNotDrawItemEffect(context, false);
			StateUtil.SendMessageReturnFromPhantom(context, PhantomType.ASTEROID);
			this.m_boss = null;
			context.SetChangePhantomCancel(ItemType.UNKNOWN);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			bool flag = false;
			this.m_speed = Mathf.Min(this.m_speed + 15f * deltaTime, 15f);
			CharacterMoveTargetBoss currentState = context.Movement.GetCurrentState<CharacterMoveTargetBoss>();
			if (currentState != null)
			{
				currentState.SetSpeed(this.m_speed);
				flag = currentState.IsTargetNotFound();
			}
			this.m_time -= deltaTime;
			if (this.m_time < 0f || this.m_returnFromPhantom || flag)
			{
				StateUtil.ReturnFromPhantomAndChangeState(context, PhantomType.ASTEROID, this.m_returnFromPhantom);
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
			if (msgHitDamageSucceed != null)
			{
				if (msgHitDamageSucceed.m_sender == this.m_boss)
				{
					this.m_returnFromPhantom = true;
				}
				StateUtil.CreateEffect(context, msgHitDamageSucceed.m_position, msgHitDamageSucceed.m_rotation, "ef_ph_aste_bom01", true);
			}
			return true;
		}
	}
}
