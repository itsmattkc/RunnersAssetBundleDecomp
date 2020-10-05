using System;
using Tutorial;
using UnityEngine;

namespace Boss
{
	public class BossStateDeadFever : FSMState<ObjBossEggmanState>
	{
		private enum State
		{
			Idle,
			Wait1,
			Wait2,
			Bom,
			End
		}

		private const float PASS_SPEED = 2f;

		private const float BOM_DISTANCE = 7f;

		private const float BOM_TIME = 0.7f;

		private const float WAIT_TIME1 = 1f;

		private const float WAIT_TIME2 = 0.5f;

		private const float BOM_SHOT_SPEED = 25f;

		private BossStateDeadFever.State m_state;

		private float m_pass_speed;

		private float m_time;

		private float m_distance;

		private float m_bom_time;

		private GameObject m_bom_obj;

		public override void Enter(ObjBossEggmanState context)
		{
			context.DebugDrawState("BossStateDeadFever");
			context.SetHitCheck(false);
			this.m_state = BossStateDeadFever.State.Wait1;
			this.m_pass_speed = 0f;
			this.m_time = 0f;
			this.m_bom_time = 0.7f;
			this.m_distance = 7f + context.BossParam.AddSpeedDistance;
			context.BossEffect.PlayBoostEffect(ObjBossEggmanEffect.BoostType.Normal);
			ObjUtil.SendMessageTutorialClear(EventID.FEVER_BOSS);
		}

		public override void Leave(ObjBossEggmanState context)
		{
		}

		public override void Step(ObjBossEggmanState context, float delta)
		{
			context.UpdateSpeedUp(delta, this.m_pass_speed);
			switch (this.m_state)
			{
			case BossStateDeadFever.State.Wait1:
				this.m_time += delta;
				if (this.m_time > 1f)
				{
					context.BossMotion.SetMotion(BossMotion.ESCAPE, true);
					this.m_time = 0f;
					this.m_state = BossStateDeadFever.State.Wait2;
				}
				break;
			case BossStateDeadFever.State.Wait2:
				this.m_time += delta;
				if (this.m_time > 0.5f)
				{
					this.m_pass_speed = context.BossParam.PlayerSpeed * 2f;
					this.m_time = 0f;
					this.m_state = BossStateDeadFever.State.Bom;
				}
				break;
			case BossStateDeadFever.State.Bom:
			{
				float playerBossPositionX = context.GetPlayerBossPositionX();
				if (playerBossPositionX > this.m_distance)
				{
					this.m_bom_obj = context.CreateBom(false, 25f, true);
					this.m_time = 0f;
					this.m_state = BossStateDeadFever.State.End;
				}
				break;
			}
			case BossStateDeadFever.State.End:
				this.m_time += delta;
				if (this.m_time > this.m_bom_time)
				{
					context.BlastBom(this.m_bom_obj);
					context.BossEnd(true);
					this.m_state = BossStateDeadFever.State.Idle;
				}
				break;
			}
		}
	}
}
