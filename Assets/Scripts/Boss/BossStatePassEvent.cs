using System;

namespace Boss
{
	public class BossStatePassEvent : FSMState<ObjBossEventBossState>
	{
		private enum State
		{
			Idle,
			Wait1,
			Wait2,
			End
		}

		private const float PASS_SPEED = 2f;

		private const float END_DISTANCE = 8f;

		private const float END_TIME = 1f;

		private const float WAIT_TIME = 1f;

		private BossStatePassEvent.State m_state;

		private float m_time;

		private float m_pass_speed;

		public override void Enter(ObjBossEventBossState context)
		{
			context.DebugDrawState("BossStatePassMap");
			context.SetHitCheck(false);
			this.m_state = BossStatePassEvent.State.Wait1;
			this.m_time = 0f;
			context.BossMotion.SetMotion(EventBossMotion.PASS, true);
		}

		public override void Leave(ObjBossEventBossState context)
		{
		}

		public override void Step(ObjBossEventBossState context, float delta)
		{
			switch (this.m_state)
			{
			case BossStatePassEvent.State.Wait1:
				this.m_time += delta;
				if (this.m_time > 1f)
				{
					this.m_pass_speed = context.BossParam.PlayerSpeed * 2f;
					this.m_state = BossStatePassEvent.State.Wait2;
				}
				break;
			case BossStatePassEvent.State.Wait2:
			{
				context.UpdateSpeedUp(delta, this.m_pass_speed);
				float playerBossPositionX = context.GetPlayerBossPositionX();
				if (playerBossPositionX > 8f)
				{
					context.SetFailed();
					this.m_time = 0f;
					this.m_state = BossStatePassEvent.State.End;
				}
				break;
			}
			case BossStatePassEvent.State.End:
				context.UpdateSpeedUp(delta, this.m_pass_speed);
				this.m_time += delta;
				if (this.m_time > 1f)
				{
					context.BossEnd(false);
					this.m_state = BossStatePassEvent.State.Idle;
				}
				break;
			}
		}
	}
}
