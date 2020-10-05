using System;

namespace Boss
{
	public class BossStateDamageFever : BossStateDamageBase
	{
		private const float START_SPEED = 60f;

		private const float START_DOWNSPEED = 120f;

		private const float WAIT_DOWNSPEED = 0.5f;

		private const float ROT_SPEED = 45f;

		private const float ROT_DOWNSPEED = 0.9f;

		private const float ROT_MIN = 10f;

		private const float END_DISTANCE = 9f;

		protected override string GetStateName()
		{
			return "BossStateDamageFever";
		}

		protected override void Setup(ObjBossEggmanState context)
		{
			float damageSpeedParam = context.GetDamageSpeedParam();
			context.SetSpeed((60f + context.BossParam.AddSpeed) * damageSpeedParam);
			base.SetSpeedDown(120f + context.BossParam.AddSpeed);
			base.SetRotSpeed(45f);
			base.SetRotSpeedDown(0.9f);
			base.SetRotMin(10f);
			base.SetDistance(9f);
		}

		protected override void SetStateSpeedDown(ObjBossEggmanState context)
		{
			base.SetSpeedDown(0.5f);
		}

		protected override void ChangeStateWait(ObjBossEggmanState context)
		{
			if (context.BossParam.BossHP > 0)
			{
				context.UpdateBossStateAfterAttack();
				context.ChangeState(STATE_ID.AttackFever);
			}
			else
			{
				context.ChangeState(STATE_ID.DeadFever);
			}
		}
	}
}
