using System;

namespace Boss
{
	public class BossStateAttackEvent2 : BossStateAttackEventBase
	{
		private enum State
		{
			Idle,
			Speedup
		}

		private BossStateAttackEvent2.State m_state;

		public override void Enter(ObjBossEventBossState context)
		{
			base.Enter(context);
			context.DebugDrawState("BossStateAttackEvent2");
			this.m_state = BossStateAttackEvent2.State.Idle;
		}

		public override void Leave(ObjBossEventBossState context)
		{
			base.Leave(context);
		}

		public override void Step(ObjBossEventBossState context, float delta)
		{
			if (context.IsPlayerDead())
			{
				return;
			}
			if (context.IsBossDistanceEnd())
			{
				context.ChangeState(EVENTBOSS_STATE_ID.PassDistanceEnd);
				return;
			}
			base.UpdateTrapBall(context, delta);
			if (base.UpdateBumper(context, delta))
			{
				this.m_state = BossStateAttackEvent2.State.Speedup;
			}
			BossStateAttackEvent2.State state = this.m_state;
			if (state != BossStateAttackEvent2.State.Idle)
			{
				if (state == BossStateAttackEvent2.State.Speedup)
				{
					if (base.UpdateBumperSpeedup(context, delta))
					{
						this.m_state = BossStateAttackEvent2.State.Idle;
					}
				}
			}
			else if (base.UpdateBoost(context, delta))
			{
				context.ChangeState(EVENTBOSS_STATE_ID.AppearEvent2_2);
			}
		}
	}
}
