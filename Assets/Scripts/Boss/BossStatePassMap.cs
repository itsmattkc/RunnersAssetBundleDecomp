using System;

namespace Boss
{
	public class BossStatePassMap : FSMState<ObjBossEggmanState>
	{
		private enum State
		{
			Idle,
			Wait,
			End
		}

		private const float PASS_SPEED = 2f;

		private const float END_DISTANCE = 8f;

		private const float END_TIME = 1f;

		private BossStatePassMap.State m_state;

		private float m_time;

		private float m_pass_speed;

		public override void Enter(ObjBossEggmanState context)
		{
			context.DebugDrawState("BossStatePassMap");
			context.SetHitCheck(false);
			context.BossEffect.PlayBoostEffect(ObjBossEggmanEffect.BoostType.Normal);
			this.m_state = BossStatePassMap.State.Wait;
			this.m_time = 0f;
			this.m_pass_speed = context.BossParam.PlayerSpeed * 2f;
		}

		public override void Leave(ObjBossEggmanState context)
		{
		}

		public override void Step(ObjBossEggmanState context, float delta)
		{
			context.UpdateSpeedUp(delta, this.m_pass_speed);
			BossStatePassMap.State state = this.m_state;
			if (state != BossStatePassMap.State.Wait)
			{
				if (state == BossStatePassMap.State.End)
				{
					this.m_time += delta;
					if (this.m_time > 1f)
					{
						context.BossEnd(false);
						this.m_state = BossStatePassMap.State.Idle;
					}
				}
			}
			else
			{
				float playerBossPositionX = context.GetPlayerBossPositionX();
				if (playerBossPositionX > 8f)
				{
					context.SetFailed();
					this.m_time = 0f;
					this.m_state = BossStatePassMap.State.End;
				}
			}
		}
	}
}
