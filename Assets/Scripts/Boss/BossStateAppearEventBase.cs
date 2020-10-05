using Message;
using System;
using UnityEngine;

namespace Boss
{
	public class BossStateAppearEventBase : FSMState<ObjBossEventBossState>
	{
		private enum State
		{
			Idle,
			Start,
			Move,
			Wait1,
			Wait2,
			Wait3
		}

		private static Vector3 APPEAR_ROT = new Vector3(0f, 90f, 0f);

		private static float APPEAR_OFFSET_POS_Y = 7f;

		private static float MOVE_SPEED = 2.5f;

		private static float START_TIME = 3.5f;

		private static float WAIT_TIME1 = 2f;

		private static float WAIT_TIME2 = 1f;

		private static float WAIT_TIME3 = 1f;

		private BossStateAppearEventBase.State m_state;

		private float m_time;

		public override void Enter(ObjBossEventBossState context)
		{
			context.DebugDrawState("BossStateAppearEvent");
			context.SetHitCheck(false);
			context.transform.rotation = Quaternion.Euler(BossStateAppearEventBase.APPEAR_ROT);
			context.transform.position = new Vector3(context.GetPlayerPosition().x + context.BossParam.DefaultPlayerDistance, context.BossParam.StartPos.y + BossStateAppearEventBase.APPEAR_OFFSET_POS_Y, 0f);
			context.SetupMoveY(1f);
			context.KeepSpeed();
			if (this.IsFirst())
			{
				context.OpenHpGauge();
			}
			else
			{
				context.BossMotion.SetMotion(EventBossMotion.ATTACK, false);
			}
			this.m_time = 0f;
			this.m_state = BossStateAppearEventBase.State.Start;
		}

		public override void Leave(ObjBossEventBossState context)
		{
		}

		public override void Step(ObjBossEventBossState context, float delta)
		{
			switch (this.m_state)
			{
			case BossStateAppearEventBase.State.Start:
				if (this.IsFirst())
				{
					this.m_time += delta;
					if (this.m_time > BossStateAppearEventBase.START_TIME)
					{
						this.m_time = 0f;
						this.m_state = BossStateAppearEventBase.State.Move;
					}
				}
				else
				{
					this.m_time = 0f;
					this.m_state = BossStateAppearEventBase.State.Move;
				}
				break;
			case BossStateAppearEventBase.State.Move:
				context.UpdateMoveY(delta, context.BossParam.StartPos.y, BossStateAppearEventBase.MOVE_SPEED);
				if (Mathf.Abs(context.BossParam.StartPos.y - context.transform.position.y) < 0.1f)
				{
					context.transform.position = new Vector3(context.transform.position.x, context.BossParam.StartPos.y, context.transform.position.z);
					if (this.IsFirst())
					{
						context.BossMotion.SetMotion(EventBossMotion.APPEAR, true);
					}
					this.m_time = 0f;
					this.m_state = BossStateAppearEventBase.State.Wait1;
				}
				break;
			case BossStateAppearEventBase.State.Wait1:
				if (this.IsFirst())
				{
					this.m_time += delta;
					if (this.m_time > BossStateAppearEventBase.WAIT_TIME1)
					{
						MsgTutorialMapBoss value = new MsgTutorialMapBoss();
						GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnMsgTutorialMapBoss", value, SendMessageOptions.DontRequireReceiver);
						context.BossParam.SetupBossTable();
						context.StartGauge();
						this.m_time = 0f;
						this.m_state = BossStateAppearEventBase.State.Wait2;
					}
				}
				else
				{
					this.m_time = 0f;
					this.m_state = BossStateAppearEventBase.State.Wait2;
				}
				break;
			case BossStateAppearEventBase.State.Wait2:
				if (this.IsFirst())
				{
					this.m_time += delta;
					if (this.m_time > BossStateAppearEventBase.WAIT_TIME2)
					{
						context.RequestStartChaoAbility();
						this.m_time = 0f;
						this.m_state = BossStateAppearEventBase.State.Wait3;
					}
				}
				else
				{
					this.m_time = 0f;
					this.m_state = BossStateAppearEventBase.State.Wait3;
				}
				break;
			case BossStateAppearEventBase.State.Wait3:
				this.m_time += delta;
				if (this.m_time > BossStateAppearEventBase.WAIT_TIME3)
				{
					context.ChangeState(this.GetNextChangeState());
					this.m_state = BossStateAppearEventBase.State.Idle;
				}
				break;
			}
		}

		protected virtual EVENTBOSS_STATE_ID GetNextChangeState()
		{
			return EVENTBOSS_STATE_ID.AttackEvent1;
		}

		protected virtual bool IsFirst()
		{
			return true;
		}
	}
}
