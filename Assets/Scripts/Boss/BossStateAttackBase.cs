using System;

namespace Boss
{
	public class BossStateAttackBase : FSMState<ObjBossEggmanState>
	{
		private float m_speed;

		private float m_time;

		private float m_boss_y;

		public override void Enter(ObjBossEggmanState context)
		{
			context.SetHitCheck(true);
			context.SetupMoveY(0f);
			this.m_speed = 0f;
			this.m_time = 0f;
			this.m_boss_y = 0f;
		}

		public override void Leave(ObjBossEggmanState context)
		{
			context.SetHitCheck(false);
		}

		protected bool UpdateTime(float delta, float time_max)
		{
			this.m_time += delta;
			return this.m_time > time_max;
		}

		protected void ResetTime()
		{
			this.m_time = 0f;
		}

		protected void UpdateMove(ObjBossEggmanState context, float delta)
		{
			context.UpdateMoveY(delta, this.m_boss_y, this.m_speed);
		}

		protected void SetMove(ObjBossEggmanState context, float step, float speed, float boss_y)
		{
			context.SetupMoveY(step);
			this.m_speed = speed;
			this.m_boss_y = boss_y;
		}

		protected bool IsMoveStepEquals(ObjBossEggmanState context, float val)
		{
			return context.BossParam.StepMoveY.Equals(val);
		}
	}
}
