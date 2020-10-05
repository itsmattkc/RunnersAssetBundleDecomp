using Message;
using System;
using UnityEngine;

namespace Boss
{
	public class BossStateAppearFever : FSMState<ObjBossEggmanState>
	{
		private enum State
		{
			Idle,
			Start,
			Appear,
			Open,
			Wait
		}

		private const float PLAYER_DISTANCE = 9f;

		private const float WAIT_TIME = 2f;

		private static Vector3 APPEAR_ROT = new Vector3(0f, 90f, 0f);

		private BossStateAppearFever.State m_state;

		private float m_time;

		private float m_distance;

		public override void Enter(ObjBossEggmanState context)
		{
			context.DebugDrawState("BossStateAppearFever");
			context.SetHitCheck(false);
			context.transform.rotation = Quaternion.Euler(BossStateAppearFever.APPEAR_ROT);
			context.OpenHpGauge();
			this.m_state = BossStateAppearFever.State.Start;
			this.m_time = 0f;
			this.m_distance = 9f + context.BossParam.AddSpeedDistance;
		}

		public override void Leave(ObjBossEggmanState context)
		{
		}

		public override void Step(ObjBossEggmanState context, float delta)
		{
			switch (this.m_state)
			{
			case BossStateAppearFever.State.Start:
				context.BossMotion.SetMotion(BossMotion.MOVE_R, true);
				context.BossEffect.PlayBoostEffect(ObjBossEggmanEffect.BoostType.Normal);
				this.m_state = BossStateAppearFever.State.Appear;
				break;
			case BossStateAppearFever.State.Appear:
			{
				float playerDistance = context.GetPlayerDistance();
				if (playerDistance < this.m_distance)
				{
					context.RequestStartChaoAbility();
					context.KeepSpeed();
					context.BossEffect.PlayFoundEffect();
					context.BossMotion.SetMotion(BossMotion.NOTICE, true);
					ObjUtil.PlaySE("boss_find", "SE");
					GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnMsgTutorialFeverBoss", new MsgTutorialFeverBoss(), SendMessageOptions.DontRequireReceiver);
					this.m_time = 0f;
					this.m_state = BossStateAppearFever.State.Open;
				}
				break;
			}
			case BossStateAppearFever.State.Open:
				this.m_time += delta;
				if (this.m_time > 0.6f)
				{
					ObjUtil.PlaySE("boss_bomb_drop", "SE");
					this.m_time = 0f;
					this.m_state = BossStateAppearFever.State.Wait;
				}
				break;
			case BossStateAppearFever.State.Wait:
				this.m_time += delta;
				if (this.m_time > 2f)
				{
					context.BossParam.SetupBossTable();
					context.StartGauge();
					context.ChangeState(STATE_ID.AttackFever);
					this.m_state = BossStateAppearFever.State.Idle;
				}
				break;
			}
		}
	}
}
