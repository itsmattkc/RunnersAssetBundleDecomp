using System;

namespace Boss
{
	public class BossStateDeadMap : FSMState<ObjBossEggmanState>
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

		private BossStateDeadMap.State m_state;

		private float m_pass_speed;

		private float m_time;

		public override void Enter(ObjBossEggmanState context)
		{
			context.DebugDrawState("BossStateDeadMap");
			context.SetHitCheck(false);
			context.BossEffect.PlayBoostEffect(ObjBossEggmanEffect.BoostType.Normal);
			this.m_state = BossStateDeadMap.State.Wait1;
			this.m_pass_speed = 0f;
			this.m_time = 0f;
		}

		public override void Leave(ObjBossEggmanState context)
		{
		}

		public override void Step(ObjBossEggmanState context, float delta)
		{
			context.UpdateSpeedUp(delta, this.m_pass_speed);
			BossStateDeadMap.State state = this.m_state;
			if (state != BossStateDeadMap.State.Wait1)
			{
				if (state == BossStateDeadMap.State.Wait2)
				{
					this.m_time += delta;
					if (this.m_time > 3f)
					{
						context.BossEnd(true);
						this.m_state = BossStateDeadMap.State.Idle;
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
					this.m_state = BossStateDeadMap.State.Wait2;
				}
			}
		}
	}
}
