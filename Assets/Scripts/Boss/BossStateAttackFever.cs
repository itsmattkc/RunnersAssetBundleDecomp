using System;
using UnityEngine;

namespace Boss
{
	public class BossStateAttackFever : FSMState<ObjBossEggmanState>
	{
		private enum State
		{
			Idle,
			Start,
			Bom,
			Speedup,
			SpeedupEnd
		}

		private const float PASS_DISTANCE = 8f;

		private const float SWEAT_DISTANCE = 5f;

		private const float SPEEDUP_DISTANCE = 10f;

		private BossStateAttackFever.State m_state;

		private float m_speed_down;

		private float m_speed_down2;

		private float m_speed_up;

		private float m_distance_pass;

		private float m_distance_sweat;

		private bool m_sweat_effect;

		private float m_sweat_effect_time;

		private float m_time;

		private float m_attackInterspace;

		private bool m_bumper = true;

		public override void Enter(ObjBossEggmanState context)
		{
			context.DebugDrawState("BossStateSpeedDown");
			context.SetHitCheck(true);
			context.BossEffect.PlayBoostEffect(ObjBossEggmanEffect.BoostType.Normal);
			this.m_state = BossStateAttackFever.State.Start;
			this.m_time = 0f;
			this.m_speed_down = context.BossParam.DownSpeed * context.BossParam.AddSpeedRatio;
			this.m_speed_down2 = this.m_speed_down;
			this.m_speed_up = 0f;
			this.m_distance_pass = 8f + context.BossParam.AddSpeedDistance;
			this.m_distance_sweat = 5f + context.BossParam.AddSpeedDistance;
			this.m_sweat_effect = false;
			this.m_sweat_effect_time = 0f;
			this.m_bumper = true;
		}

		public override void Leave(ObjBossEggmanState context)
		{
			context.SetHitCheck(false);
			context.BossEffect.PlaySweatEffectEnd();
		}

		public override void Step(ObjBossEggmanState context, float delta)
		{
			context.UpdateSpeedDown(delta, this.m_speed_down);
			float playerBossPositionX = context.GetPlayerBossPositionX();
			if (playerBossPositionX < 0f)
			{
				this.SetSweatEffect(context);
				this.m_bumper = false;
				if (Mathf.Abs(playerBossPositionX) > this.m_distance_pass)
				{
					context.ChangeState(STATE_ID.PassFever);
				}
			}
			else
			{
				if (playerBossPositionX < this.m_distance_sweat)
				{
					this.SetSweatEffect(context);
					this.m_bumper = false;
				}
				else
				{
					this.ResetSweatEffect(context, delta);
					this.m_bumper = true;
				}
				switch (this.m_state)
				{
				case BossStateAttackFever.State.Start:
					if (context.IsHitBumper())
					{
						if (playerBossPositionX < 10f)
						{
							this.m_speed_up = context.BossParam.BumperSpeedup;
							this.m_state = BossStateAttackFever.State.Speedup;
						}
					}
					else
					{
						context.CreateBumper(true, 0f);
						this.m_attackInterspace = context.GetAttackInterspace();
						this.ResetTime();
						this.m_state = BossStateAttackFever.State.Bom;
					}
					break;
				case BossStateAttackFever.State.Bom:
					if (context.IsBossDistanceEnd())
					{
						context.ChangeState(STATE_ID.PassFeverDistanceEnd);
					}
					else if (context.IsHitBumper())
					{
						if (playerBossPositionX < 10f)
						{
							this.m_speed_up = context.BossParam.BumperSpeedup;
							this.m_state = BossStateAttackFever.State.Speedup;
						}
					}
					else if (this.UpdateTime(delta, this.m_attackInterspace) && this.m_bumper)
					{
						this.m_state = BossStateAttackFever.State.Start;
					}
					break;
				case BossStateAttackFever.State.Speedup:
					context.SetSpeed(this.m_speed_up * 0.1f);
					this.m_state = BossStateAttackFever.State.SpeedupEnd;
					break;
				case BossStateAttackFever.State.SpeedupEnd:
				{
					float num = context.BossParam.PlayerSpeed * 0.7f;
					if (context.BossParam.Speed < num)
					{
						this.m_speed_down = this.m_speed_down2;
						context.SetSpeed(num);
						this.m_state = BossStateAttackFever.State.Bom;
					}
					else
					{
						this.m_speed_down += delta * this.m_speed_up * 1.2f;
					}
					break;
				}
				}
			}
		}

		private bool UpdateTime(float delta, float time_max)
		{
			this.m_time += delta;
			return this.m_time > time_max;
		}

		private void ResetTime()
		{
			this.m_time = 0f;
		}

		private void SetSweatEffect(ObjBossEggmanState context)
		{
			if (!this.m_sweat_effect)
			{
				context.BossEffect.PlaySweatEffectStart();
				this.m_sweat_effect_time = 1f;
				this.m_sweat_effect = true;
			}
		}

		private void ResetSweatEffect(ObjBossEggmanState context, float delta)
		{
			this.m_sweat_effect_time -= delta;
			if (this.m_sweat_effect_time < 0f && this.m_sweat_effect)
			{
				context.BossEffect.PlaySweatEffectEnd();
				this.m_sweat_effect = false;
			}
		}
	}
}
