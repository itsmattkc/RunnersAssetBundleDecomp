using System;

namespace Boss
{
	public class BossStateDeadEvent : FSMState<ObjBossEventBossState>
	{
		private enum State
		{
			Idle,
			Wait1,
			Wait2
		}

		private const float PASS_SPEED = 2f;

		private const float WAIT_TIME1 = 1.5f;

		private const float WAIT_TIME2 = 3f;

		private BossStateDeadEvent.State m_state;

		private float m_pass_speed;

		private float m_time;

		public override void Enter(ObjBossEventBossState context)
		{
			context.DebugDrawState("BossStateDeadMap");
			context.SetHitCheck(false);
			this.m_state = BossStateDeadEvent.State.Wait1;
			this.m_pass_speed = 0f;
			this.m_time = 0f;
		}

		public override void Leave(ObjBossEventBossState context)
		{
		}

		public override void Step(ObjBossEventBossState context, float delta)
		{
			context.UpdateSpeedUp(delta, this.m_pass_speed);
			BossStateDeadEvent.State state = this.m_state;
			if (state != BossStateDeadEvent.State.Wait1)
			{
				if (state == BossStateDeadEvent.State.Wait2)
				{
					this.m_time += delta;
					if (this.m_time > 3f)
					{
						context.BossEnd(true);
						this.m_state = BossStateDeadEvent.State.Idle;
					}
				}
			}
			else
			{
				this.m_time += delta;
				if (this.m_time > 1.5f)
				{
					context.SetClear();
					this.m_pass_speed = context.BossParam.PlayerSpeed * 2f;
					this.m_time = 0f;
					this.m_state = BossStateDeadEvent.State.Wait2;
				}
			}
		}
	}
}
