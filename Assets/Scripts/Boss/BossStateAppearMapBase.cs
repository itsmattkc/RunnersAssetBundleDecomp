using Message;
using System;
using UnityEngine;

namespace Boss
{
	public class BossStateAppearMapBase : FSMState<ObjBossEggmanState>
	{
		private enum State
		{
			Idle,
			Start,
			Wait1,
			Wait2,
			Event1,
			Event2,
			Event3
		}

		private static Vector3 APPEAR_ROT = new Vector3(0f, 90f, 0f);

		private BossStateAppearMapBase.State m_state;

		private float m_time;

		public override void Enter(ObjBossEggmanState context)
		{
			context.DebugDrawState("BossStateAppearMap");
			context.SetHitCheck(false);
			context.transform.rotation = Quaternion.Euler(BossStateAppearMapBase.APPEAR_ROT);
			context.OpenHpGauge();
			this.m_time = 0f;
			this.m_state = BossStateAppearMapBase.State.Start;
		}

		public override void Leave(ObjBossEggmanState context)
		{
		}

		public override void Step(ObjBossEggmanState context, float delta)
		{
			switch (this.m_state)
			{
			case BossStateAppearMapBase.State.Start:
				context.BossEffect.PlayBoostEffect(ObjBossEggmanEffect.BoostType.Normal);
				this.m_state = BossStateAppearMapBase.State.Wait1;
				break;
			case BossStateAppearMapBase.State.Wait1:
			{
				float playerDistance = context.GetPlayerDistance();
				if (playerDistance < context.BossParam.DefaultPlayerDistance)
				{
					context.KeepSpeed();
					MsgTutorialMapBoss value = new MsgTutorialMapBoss();
					GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnMsgTutorialMapBoss", value, SendMessageOptions.DontRequireReceiver);
					this.m_state = BossStateAppearMapBase.State.Wait2;
				}
				break;
			}
			case BossStateAppearMapBase.State.Wait2:
				this.SetMotion1(context);
				this.m_state = BossStateAppearMapBase.State.Event1;
				break;
			case BossStateAppearMapBase.State.Event1:
				this.m_time += delta;
				if (this.m_time > this.GetTime1())
				{
					context.RequestStartChaoAbility();
					this.SetMotion2(context);
					this.m_time = 0f;
					this.m_state = BossStateAppearMapBase.State.Event2;
				}
				break;
			case BossStateAppearMapBase.State.Event2:
				this.m_time += delta;
				if (this.m_time > this.GetTime2())
				{
					this.SetMotion3(context);
					this.m_state = BossStateAppearMapBase.State.Event3;
				}
				break;
			case BossStateAppearMapBase.State.Event3:
				this.m_time += delta;
				if (this.m_time > this.GetTime3())
				{
					context.BossParam.SetupBossTable();
					context.StartGauge();
					context.ChangeState(this.GetNextChangeState());
					this.m_state = BossStateAppearMapBase.State.Idle;
				}
				break;
			}
		}

		protected virtual float GetTime1()
		{
			return 0f;
		}

		protected virtual float GetTime2()
		{
			return 0f;
		}

		protected virtual float GetTime3()
		{
			return 0f;
		}

		protected virtual void SetMotion1(ObjBossEggmanState context)
		{
			context.BossMotion.SetMotion(BossMotion.APPEAR, true);
		}

		protected virtual void SetMotion2(ObjBossEggmanState context)
		{
			context.BossMotion.SetMotion(BossMotion.BOM_START, true);
			ObjUtil.PlaySE("boss_bomb_drop", "SE");
		}

		protected virtual void SetMotion3(ObjBossEggmanState context)
		{
		}

		protected virtual STATE_ID GetNextChangeState()
		{
			return STATE_ID.AttackMap1;
		}
	}
}
