using System;
using UnityEngine;

namespace Boss
{
	public class BossStateAttackMap2 : BossStateAttackBase
	{
		private enum State
		{
			Idle,
			Start,
			Bumper,
			Missile,
			BossAttackReady,
			BossAttack
		}

		private const float MOVE_SPEED = 14f;

		private const float ATTACK_READY = 1f;

		private const float ATTACK_POSY = 1.5f;

		private const float PASS_DISTANCE = 6f;

		private const float PASS_WARP_DISTANCE = 26f;

		private const float MISSILE_POSX = 10f;

		private static readonly float[] MISSILE_POSY = new float[]
		{
			1f,
			1f,
			2f,
			2f,
			3f
		};

		private static readonly float[] BOSS_POSY = new float[]
		{
			2.5f,
			2.5f,
			3.5f,
			3.5f,
			1.5f
		};

		private BossStateAttackMap2.State m_state;

		private float m_missile_time;

		private float m_boss_time;

		private float m_attackInterspace;

		public override void Enter(ObjBossEggmanState context)
		{
			base.Enter(context);
			context.DebugDrawState("BossStateAttackMap2");
			context.BossMotion.SetMotion(BossMotion.MISSILE_START, true);
			this.m_state = BossStateAttackMap2.State.Start;
			this.m_missile_time = 0f;
			this.m_boss_time = 0f;
			this.m_attackInterspace = context.GetAttackInterspace();
		}

		public override void Leave(ObjBossEggmanState context)
		{
			base.Leave(context);
		}

		public override void Step(ObjBossEggmanState context, float delta)
		{
			switch (this.m_state)
			{
			case BossStateAttackMap2.State.Start:
				if (!context.IsPlayerDead())
				{
					if (context.IsBossDistanceEnd())
					{
						context.ChangeState(STATE_ID.PassMapDistanceEnd);
					}
					else
					{
						int randomRange = ObjUtil.GetRandomRange100();
						if (randomRange < context.BossParam.BumperRand)
						{
							context.CreateBumper(true, 0f);
						}
						this.m_missile_time = 0f;
						this.m_state = BossStateAttackMap2.State.Bumper;
					}
				}
				break;
			case BossStateAttackMap2.State.Bumper:
				this.m_missile_time += delta;
				if (this.m_missile_time > context.BossParam.MissileInterspace * 0.5f)
				{
					if (!context.IsPlayerDead() && !context.IsBossDistanceEnd())
					{
						int num = UnityEngine.Random.Range(0, BossStateAttackMap2.MISSILE_POSY.Length);
						float num2 = BossStateAttackMap2.MISSILE_POSY[num];
						Vector3 pos = new Vector3(context.transform.position.x + 10f, num2, context.transform.position.z);
						context.CreateMissile(pos);
						if (num < BossStateAttackMap2.BOSS_POSY.Length)
						{
							if (Mathf.Abs(num2 - context.transform.position.y) < 2f)
							{
								base.SetMove(context, 1f, 14f, BossStateAttackMap2.BOSS_POSY[num]);
							}
							else
							{
								base.SetMove(context, 0f, 0f, context.transform.position.y);
							}
						}
					}
					this.m_state = BossStateAttackMap2.State.Missile;
				}
				break;
			case BossStateAttackMap2.State.Missile:
				base.UpdateMove(context, delta);
				this.m_missile_time += delta;
				if (this.m_missile_time > context.BossParam.MissileInterspace)
				{
					this.m_state = BossStateAttackMap2.State.Start;
				}
				break;
			case BossStateAttackMap2.State.BossAttackReady:
				base.UpdateMove(context, delta);
				this.m_boss_time += delta;
				if (this.m_boss_time > 1f)
				{
					context.BossEffect.PlayBoostEffect(ObjBossEggmanEffect.BoostType.Attack);
					ObjUtil.PlaySE("boss_eggmobile_dash", "SE");
					this.m_boss_time = 0f;
					this.m_state = BossStateAttackMap2.State.BossAttack;
				}
				break;
			case BossStateAttackMap2.State.BossAttack:
			{
				if (UnityEngine.Random.Range(0, 2) == 0)
				{
					context.SetSpeed(-context.BossParam.AttackSpeedMin);
				}
				else
				{
					context.SetSpeed(-context.BossParam.AttackSpeedMax);
				}
				context.BossParam.MinSpeed = context.BossParam.Speed;
				float playerBossPositionX = context.GetPlayerBossPositionX();
				if (playerBossPositionX < 0f && Mathf.Abs(playerBossPositionX) > 6f)
				{
					context.SetSpeed(0f);
					context.BossParam.MinSpeed = context.BossParam.Speed;
					Vector3 position = new Vector3(context.transform.position.x + 26f, context.BossParam.StartPos.y, context.transform.position.z);
					context.transform.position = position;
					context.ChangeState(STATE_ID.AppearMap2_2);
				}
				break;
			}
			}
			if (!context.IsPlayerDead() && this.m_state != BossStateAttackMap2.State.BossAttack && this.m_state != BossStateAttackMap2.State.BossAttackReady)
			{
				this.m_boss_time += delta;
				if (this.m_boss_time > this.m_attackInterspace)
				{
					if (context.IsBossDistanceEnd())
					{
						context.ChangeState(STATE_ID.PassMapDistanceEnd);
					}
					else
					{
						this.m_boss_time = 0f;
						base.SetMove(context, 1f, 14f, 1.5f);
						context.BossMotion.SetMotion(BossMotion.ATTACK, true);
						this.m_state = BossStateAttackMap2.State.BossAttackReady;
					}
				}
			}
		}
	}
}
