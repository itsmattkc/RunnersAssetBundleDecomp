using Message;
using System;
using Tutorial;
using UnityEngine;

namespace Boss
{
	public class BossStatePassFever : FSMState<ObjBossEggmanState>
	{
		private enum State
		{
			Idle,
			Bom,
			Shot,
			End
		}

		private const float PASS_SPEED = 2f;

		private const float BOM_DISTANCE1 = 0f;

		private const float BOM_DISTANCE2 = 9f;

		private const float BOM_DISTANCE3 = 7f;

		private const float BOM_SHOT_SPEED = 20f;

		private BossStatePassFever.State m_state;

		private float m_pass_speed;

		private float m_distance_1;

		private float m_distance_2;

		private float m_distance_3;

		private GameObject m_bom_obj;

		public override void Enter(ObjBossEggmanState context)
		{
			context.DebugDrawState("BossStatePassFever");
			context.SetHitCheck(false);
			this.m_state = BossStatePassFever.State.Bom;
			this.m_pass_speed = context.BossParam.PlayerSpeed * 2f;
			this.m_distance_1 = 0f + context.BossParam.AddSpeedDistance;
			this.m_distance_2 = 9f + context.BossParam.AddSpeedDistance;
			this.m_distance_3 = 7f + context.BossParam.AddSpeedDistance;
			context.BossMotion.SetMotion(BossMotion.PASS, true);
			context.BossEffect.PlayBoostEffect(ObjBossEggmanEffect.BoostType.Attack);
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
			case BossStatePassFever.State.Bom:
			{
				float playerBossPositionX = context.GetPlayerBossPositionX();
				if (playerBossPositionX > this.m_distance_1)
				{
					ObjUtil.PauseCombo(MsgPauseComboTimer.State.PAUSE_TIMER, -1f);
					this.m_bom_obj = context.CreateBom(false, 20f, false);
					this.m_state = BossStatePassFever.State.Shot;
				}
				break;
			}
			case BossStatePassFever.State.Shot:
			{
				float playerBossPositionX2 = context.GetPlayerBossPositionX();
				if (playerBossPositionX2 > this.m_distance_2)
				{
					context.ShotBom(this.m_bom_obj);
					this.m_state = BossStatePassFever.State.End;
				}
				break;
			}
			case BossStatePassFever.State.End:
			{
				float num = this.m_bom_obj.transform.position.x - context.GetPlayerPosition().x;
				if (num < this.m_distance_3)
				{
					context.BlastBom(this.m_bom_obj);
					context.BossEnd(false);
					this.m_state = BossStatePassFever.State.Idle;
				}
				break;
			}
			}
		}
	}
}
