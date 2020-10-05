using System;
using UnityEngine;

namespace Boss
{
	public class BossStateAppearMap2 : BossStateAppearMapBase
	{
		private static float WAIT_TIME1 = 3f;

		private static float WAIT_TIME2 = 2f;

		private static float WAIT_TIME3 = 2f;

		public override void Enter(ObjBossEggmanState context)
		{
			base.Enter(context);
			context.transform.position = new Vector3(context.transform.position.x, context.BossParam.StartPos.y, context.transform.position.z);
		}

		protected override float GetTime1()
		{
			return BossStateAppearMap2.WAIT_TIME1;
		}

		protected override float GetTime2()
		{
			return BossStateAppearMap2.WAIT_TIME2;
		}

		protected override float GetTime3()
		{
			return BossStateAppearMap2.WAIT_TIME3;
		}

		protected override void SetMotion3(ObjBossEggmanState context)
		{
			context.BossMotion.SetMotion(BossMotion.MISSILE_START, true);
		}

		protected override STATE_ID GetNextChangeState()
		{
			return STATE_ID.AttackMap2;
		}
	}
}
