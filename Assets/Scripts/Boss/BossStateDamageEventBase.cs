using System;
using UnityEngine;

namespace Boss
{
	public class BossStateDamageEventBase : FSMState<ObjBossEventBossState>
	{
		private enum State
		{
			Idle,
			SpeedDown,
			Wait
		}

		private const float START_SPEED = 60f;

		private const float START_DOWNSPEED = 120f;

		private const float WAIT_DOWNSPEED = 1f;

		private const float ROT_SPEED = 30f;

		private const float ROT_DOWNSPEED = 0.3f;

		private const float ROT_MIN = 10f;

		private BossStateDamageEventBase.State m_state;

		private Quaternion m_start_rot = Quaternion.identity;

		private float m_speed_down;

		private float m_distance;

		private bool m_rot_end;

		public override void Enter(ObjBossEventBossState context)
		{
			context.DebugDrawState("BossStateDamageEvent");
			context.SetHitCheck(false);
			context.AddDamage();
			context.BossEffect.PlayHitEffect(context.BossParam.BoostLevel);
			if (context.BossParam.BossHP > 0)
			{
				ObjUtil.PlaySE("boss_damage", "SE");
			}
			else
			{
				ObjUtil.PlaySE("boss_explosion", "SE");
				context.BossClear();
			}
			this.SetMotion(context, true);
			this.m_state = BossStateDamageEventBase.State.SpeedDown;
			this.m_start_rot = context.transform.rotation;
			this.m_speed_down = 0f;
			this.m_distance = 0f;
			this.m_rot_end = false;
			float damageSpeedParam = context.GetDamageSpeedParam();
			context.SetSpeed(60f * damageSpeedParam);
			this.SetSpeedDown(120f);
			this.SetDistance(context.BossParam.DefaultPlayerDistance);
			if (context.ColorPowerHit)
			{
				context.DebugDrawState("BossStateDamageEvent ColorPowerHit");
				this.SetupColorPowerDamage(context);
			}
			else
			{
				context.CreateEventFeverRing(context.GetDropRingAggressivity());
			}
		}

		public override void Leave(ObjBossEventBossState context)
		{
			context.ColorPowerHit = false;
			context.ChaoHit = false;
		}

		public override void Step(ObjBossEventBossState context, float delta)
		{
			context.UpdateSpeedDown(delta, this.m_speed_down);
			BossStateDamageEventBase.State state = this.m_state;
			if (state != BossStateDamageEventBase.State.SpeedDown)
			{
				if (state == BossStateDamageEventBase.State.Wait)
				{
					if (context.IsPlayerDead() && context.IsClear())
					{
						this.ChangeStateWait(context);
					}
					else
					{
						float playerDistance = context.GetPlayerDistance();
						if (playerDistance < this.m_distance)
						{
							context.SetSpeed(context.BossParam.PlayerSpeed * 0.7f);
							context.transform.rotation = this.m_start_rot;
							if (!this.m_rot_end)
							{
								this.SetMotion(context, false);
							}
							context.KeepSpeed();
							this.ChangeStateWait(context);
						}
					}
				}
			}
			else if (context.BossParam.Speed < context.BossParam.PlayerSpeed)
			{
				this.SetSpeedDown(1f);
				this.m_state = BossStateDamageEventBase.State.Wait;
			}
		}

		private void SetupColorPowerDamage(ObjBossEventBossState context)
		{
			context.SetSpeed(60f);
			this.SetSpeedDown(120f);
		}

		private void SetMotion(ObjBossEventBossState context, bool flag)
		{
			if (flag)
			{
				context.BossMotion.SetMotion(EventBossMotion.DAMAGE, true);
			}
			else if (context.BossParam.BossHP > 0)
			{
				context.BossMotion.SetMotion(EventBossMotion.ATTACK, true);
			}
			else
			{
				context.BossEffect.PlayEscapeEffect(context);
				context.BossMotion.SetMotion(EventBossMotion.ESCAPE, true);
			}
		}

		protected virtual void ChangeStateWait(ObjBossEventBossState context)
		{
		}

		protected void SetSpeedDown(float val)
		{
			this.m_speed_down = val;
		}

		protected void SetDistance(float val)
		{
			this.m_distance = val;
		}
	}
}
