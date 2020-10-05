using System;
using UnityEngine;

namespace Boss
{
	public class BossStateAttackEventBase : FSMState<ObjBossEventBossState>
	{
		public class TimeParam
		{
			public float m_time;

			public float m_timeMax;

			public TimeParam()
			{
				this.ResetParam(0f, 0f);
			}

			public void ResetParam(float time, float timeMax)
			{
				this.m_time = time;
				this.m_timeMax = timeMax;
			}
		}

		private const float PASS_DISTANCE = 6f;

		private const float PASS_DISTANCE2 = 2f;

		private const float PASS_WARP_DISTANCE = 26f;

		private const float MISSILE_POSX = 17f;

		private const float BUMPER_POSX = 15f;

		private const float SPEEDUP_DISTANCE = 10f;

		private static readonly float[] MISSILE_POSY = new float[]
		{
			1f,
			2f,
			3f
		};

		private static readonly float[] TRAPBLL_POSY = new float[]
		{
			1f,
			1f,
			2f,
			3f
		};

		private float m_time;

		private BossStateAttackEventBase.TimeParam m_bumperTime = new BossStateAttackEventBase.TimeParam();

		private float m_speed_up;

		private float m_speed_down;

		private BossStateAttackEventBase.TimeParam m_missileTime = new BossStateAttackEventBase.TimeParam();

		private BossStateAttackEventBase.TimeParam m_missileWaitTime = new BossStateAttackEventBase.TimeParam();

		private int m_missileCount;

		private BossStateAttackEventBase.TimeParam m_trapBallTime = new BossStateAttackEventBase.TimeParam();

		private BossStateAttackEventBase.TimeParam m_trapBallWaitTime = new BossStateAttackEventBase.TimeParam();

		private int m_trapBallCount;

		private Map3AttackData m_trapBallData;

		public override void Enter(ObjBossEventBossState context)
		{
			context.SetHitCheck(true);
			context.BossMotion.SetMotion(EventBossMotion.ATTACK, true);
			this.m_time = 0f;
			this.m_bumperTime.ResetParam(0f, context.BossParam.BumperInterspace * 0.1f);
			this.m_speed_up = 0f;
			this.m_speed_down = 0f;
			this.m_missileTime.ResetParam(0f, context.BossParam.MissileInterspace * 0.1f);
			this.m_missileWaitTime.ResetParam(0f, context.BossParam.MissileWaitTime * 0.1f);
			this.m_missileCount = 0;
			this.m_trapBallTime.ResetParam(0f, context.BossParam.AttackInterspaceMin * 0.1f);
			this.m_trapBallWaitTime.ResetParam(0f, context.BossParam.AttackInterspaceMax * 0.1f);
			this.m_trapBallCount = 0;
			this.m_trapBallData = null;
		}

		public override void Leave(ObjBossEventBossState context)
		{
			context.SetHitCheck(false);
		}

		protected bool UpdateBoost(ObjBossEventBossState context, float delta)
		{
			float playerBossPositionX = context.GetPlayerBossPositionX();
			if (playerBossPositionX < 2f)
			{
				context.SetSpeed(this.GetBoostSpeed(context, WispBoostLevel.LEVEL3));
			}
			else
			{
				context.SetSpeed(this.GetBoostSpeed(context, context.BossParam.BoostLevel));
			}
			if (playerBossPositionX < 0f && Mathf.Abs(playerBossPositionX) > 6f)
			{
				Vector3 position = new Vector3(context.transform.position.x + 26f, context.BossParam.StartPos.y, context.transform.position.z);
				context.transform.position = position;
				return true;
			}
			return false;
		}

		protected bool UpdateBumper(ObjBossEventBossState context, float delta)
		{
			this.m_bumperTime.m_time += delta;
			if (this.m_bumperTime.m_time > this.m_bumperTime.m_timeMax)
			{
				this.m_bumperTime.m_time = 0f;
				this.m_bumperTime.m_timeMax = context.BossParam.BumperInterspace;
				int randomRange = ObjUtil.GetRandomRange100();
				if (randomRange < context.BossParam.BumperRand)
				{
					context.CreateBumper(false, 15f);
				}
			}
			if (context.IsHitBumper())
			{
				float playerBossPositionX = context.GetPlayerBossPositionX();
				if (playerBossPositionX < 10f)
				{
					this.m_speed_up = context.BossParam.BumperSpeedup;
					context.SetSpeed(this.m_speed_up * 0.1f);
					this.m_speed_down = 0f;
					return true;
				}
			}
			return false;
		}

		protected bool UpdateBumperSpeedup(ObjBossEventBossState context, float delta)
		{
			context.UpdateSpeedDown(delta, this.m_speed_down);
			float num = this.GetBoostSpeed(context, context.BossParam.BoostLevel) * 0.7f;
			if (context.BossParam.Speed < num)
			{
				context.SetSpeed(num);
				return true;
			}
			this.m_speed_down += delta * this.m_speed_up * 1.2f;
			return false;
		}

		protected void UpdateMissile(ObjBossEventBossState context, float delta)
		{
			if (context.BossParam.MissileCount <= 0)
			{
				return;
			}
			this.m_missileWaitTime.m_time += delta;
			if (this.m_missileWaitTime.m_time > this.m_missileWaitTime.m_timeMax)
			{
				this.m_missileTime.m_time += delta;
				if (this.m_missileTime.m_time > this.m_missileTime.m_timeMax)
				{
					this.m_missileTime.m_time = 0f;
					this.m_missileTime.m_timeMax = context.BossParam.MissileInterspace;
					int num = 2;
					int randomRange = ObjUtil.GetRandomRange100();
					int missilePos = context.BossParam.MissilePos1;
					int missilePos2 = context.BossParam.MissilePos2;
					if (randomRange < missilePos)
					{
						num = 0;
					}
					else if (randomRange < missilePos + missilePos2)
					{
						num = 1;
					}
					float y = BossStateAttackEventBase.MISSILE_POSY[num];
					Vector3 pos = new Vector3(context.GetPlayerPosition().x + 17f, y, context.transform.position.z);
					context.CreateMissile(pos);
					this.m_missileCount++;
					if (this.m_missileCount >= context.BossParam.MissileCount)
					{
						this.m_missileCount = 0;
						this.m_missileTime.m_time = 0f;
						this.m_missileTime.m_timeMax = context.BossParam.MissileWaitTime;
					}
				}
			}
		}

		protected void UpdateTrapBall(ObjBossEventBossState context, float delta)
		{
			this.m_trapBallWaitTime.m_time += delta;
			if (this.m_trapBallWaitTime.m_time > this.m_trapBallWaitTime.m_timeMax)
			{
				this.m_trapBallTime.m_time += delta;
				if (this.m_trapBallTime.m_time > this.m_trapBallTime.m_timeMax)
				{
					this.m_trapBallTime.m_time = 0f;
					this.m_trapBallTime.m_timeMax = context.BossParam.AttackInterspaceMin;
					if (this.m_trapBallData == null)
					{
						this.m_trapBallData = context.BossParam.GetMap3AttackData();
					}
					Map3AttackData trapBallData = this.m_trapBallData;
					if (trapBallData != null)
					{
						context.BossParam.AttackBallFlag = true;
						if (this.m_trapBallCount == 0)
						{
							context.CreateTrapBall(context.BossParam.GetMap3BomTblA(trapBallData.m_type), BossStateAttackEventBase.TRAPBLL_POSY[(int)trapBallData.m_posA], 0, false);
						}
						else
						{
							context.CreateTrapBall(context.BossParam.GetMap3BomTblB(trapBallData.m_type), BossStateAttackEventBase.TRAPBLL_POSY[(int)trapBallData.m_posB], 0, false);
						}
						this.m_trapBallCount++;
						if (this.m_trapBallCount >= trapBallData.GetAttackCount())
						{
							this.m_trapBallData = null;
							this.m_trapBallCount = 0;
							this.m_trapBallWaitTime.m_time = 0f;
							this.m_trapBallWaitTime.m_timeMax = context.BossParam.AttackInterspaceMax;
						}
					}
				}
			}
		}

		private float GetBoostSpeed(ObjBossEventBossState context, WispBoostLevel level)
		{
			if (level == WispBoostLevel.NONE)
			{
				return context.BossParam.PlayerSpeed;
			}
			float boostSpeedParam = context.BossParam.GetBoostSpeedParam(level);
			return context.BossParam.PlayerSpeed - boostSpeedParam;
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
	}
}
