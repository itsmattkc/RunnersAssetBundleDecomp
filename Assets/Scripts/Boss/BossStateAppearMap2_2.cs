using System;
using UnityEngine;

namespace Boss
{
	public class BossStateAppearMap2_2 : FSMState<ObjBossEggmanState>
	{
		private enum State
		{
			Idle,
			Move,
			Wait
		}

		private static Vector3 APPEAR_ROT = new Vector3(0f, 90f, 0f);

		private static float APPEAR_OFFSET_POS_Y = 7f;

		private static float MOVE_SPEED = 2.5f;

		private static float WAIT_TIME = 0.5f;

		private BossStateAppearMap2_2.State m_state;

		private float m_time;

		public override void Enter(ObjBossEggmanState context)
		{
			context.DebugDrawState("BossStateAppearMap2_2");
			context.SetHitCheck(false);
			context.transform.rotation = Quaternion.Euler(BossStateAppearMap2_2.APPEAR_ROT);
			context.transform.position = new Vector3(context.GetPlayerPosition().x + context.BossParam.DefaultPlayerDistance, context.BossParam.StartPos.y + BossStateAppearMap2_2.APPEAR_OFFSET_POS_Y, 0f);
			context.SetupMoveY(1f);
			context.KeepSpeed();
			context.BossEffect.PlayBoostEffect(ObjBossEggmanEffect.BoostType.Normal);
			this.m_time = 0f;
			this.m_state = BossStateAppearMap2_2.State.Move;
		}

		public override void Leave(ObjBossEggmanState context)
		{
		}

		public override void Step(ObjBossEggmanState context, float delta)
		{
			BossStateAppearMap2_2.State state = this.m_state;
			if (state != BossStateAppearMap2_2.State.Move)
			{
				if (state == BossStateAppearMap2_2.State.Wait)
				{
					this.m_time += delta;
					if (this.m_time > BossStateAppearMap2_2.WAIT_TIME)
					{
						context.ChangeState(STATE_ID.AttackMap2);
						this.m_state = BossStateAppearMap2_2.State.Idle;
					}
				}
			}
			else
			{
				context.UpdateMoveY(delta, context.BossParam.StartPos.y, BossStateAppearMap2_2.MOVE_SPEED);
				if (Mathf.Abs(context.BossParam.StartPos.y - context.transform.position.y) < 0.1f)
				{
					context.transform.position = new Vector3(context.transform.position.x, context.BossParam.StartPos.y, context.transform.position.z);
					context.BossMotion.SetMotion(BossMotion.MISSILE_START, true);
					this.m_state = BossStateAppearMap2_2.State.Wait;
				}
			}
		}
	}
}
