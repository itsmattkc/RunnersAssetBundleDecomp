using System;
using UnityEngine;

namespace Boss
{
	public class BossStateAttackMap3 : BossStateAttackBase
	{
		private enum State
		{
			Idle,
			Start,
			Move,
			CreateBom,
			CreateBomNext,
			Bom
		}

		private const float MOVE_SPEED1 = 7f;

		private const float MOVE_SPEED2 = 30f;

		private const float MOVE_SPEED3 = 15f;

		private const float MOVE_TIME = 0.2f;

		private static Map3AttackData ATTACK_DATA_NONE = new Map3AttackData();

		private static readonly float[] BOSS_POSY = new float[]
		{
			2f,
			2f,
			3f,
			4f
		};

		private static readonly float[] BOM_SPEED = new float[]
		{
			7f,
			7f,
			7f,
			30f,
			30f,
			30f,
			30f,
			15f,
			15f,
			15f,
			15f
		};

		private BossStateAttackMap3.State m_state;

		private int m_attackCount;

		private Map3AttackData m_attackData = BossStateAttackMap3.ATTACK_DATA_NONE;

		private float m_attackInterspace;

		public override void Enter(ObjBossEggmanState context)
		{
			base.Enter(context);
			context.DebugDrawState("BossStateAttackMap3");
			this.m_state = BossStateAttackMap3.State.Start;
			this.m_attackCount = 0;
			this.m_attackData = BossStateAttackMap3.ATTACK_DATA_NONE;
			this.m_attackInterspace = 0f;
		}

		public override void Leave(ObjBossEggmanState context)
		{
			base.Leave(context);
		}

		public override void Step(ObjBossEggmanState context, float delta)
		{
			switch (this.m_state)
			{
			case BossStateAttackMap3.State.Start:
				if (!context.IsPlayerDead())
				{
					if (context.IsBossDistanceEnd())
					{
						context.ChangeState(STATE_ID.PassMapDistanceEnd);
					}
					else if (this.m_attackData.m_type == BossAttackType.NONE)
					{
						Map3AttackData map3AttackData = context.BossParam.GetMap3AttackData();
						if (map3AttackData != null)
						{
							this.m_attackData = map3AttackData;
							this.m_attackCount = 0;
							int num = Mathf.Min((int)this.m_attackData.m_posA, BossStateAttackMap3.BOSS_POSY.Length);
							int num2 = Mathf.Min((int)this.m_attackData.m_type, BossStateAttackMap3.BOM_SPEED.Length);
							float speed = BossStateAttackMap3.BOM_SPEED[num2];
							base.SetMove(context, 1f, speed, BossStateAttackMap3.BOSS_POSY[num]);
							base.ResetTime();
							this.m_state = BossStateAttackMap3.State.Move;
						}
					}
					else
					{
						int num3 = Mathf.Min((int)this.m_attackData.m_posB, BossStateAttackMap3.BOSS_POSY.Length);
						int num4 = Mathf.Min((int)this.m_attackData.m_type, BossStateAttackMap3.BOM_SPEED.Length);
						float speed2 = BossStateAttackMap3.BOM_SPEED[num4];
						base.SetMove(context, 1f, speed2, BossStateAttackMap3.BOSS_POSY[num3]);
						base.ResetTime();
						this.m_state = BossStateAttackMap3.State.Move;
					}
				}
				break;
			case BossStateAttackMap3.State.Move:
			{
				base.UpdateMove(context, delta);
				bool flag = base.UpdateTime(delta, 0.2f);
				if (base.IsMoveStepEquals(context, 0f) && flag)
				{
					this.m_state = BossStateAttackMap3.State.CreateBom;
				}
				break;
			}
			case BossStateAttackMap3.State.CreateBom:
				if (!context.IsPlayerDead())
				{
					if (context.IsBossDistanceEnd())
					{
						context.ChangeState(STATE_ID.PassMapDistanceEnd);
					}
					else
					{
						if (this.m_attackCount == 0)
						{
							context.CreateTrapBall(context.BossParam.GetMap3BomTblA(this.m_attackData.m_type), 0f, this.m_attackData.m_randA, true);
						}
						else
						{
							context.CreateTrapBall(context.BossParam.GetMap3BomTblB(this.m_attackData.m_type), 0f, this.m_attackData.m_randB, true);
						}
						this.m_attackCount++;
						if (this.m_attackCount >= this.m_attackData.GetAttackCount())
						{
							this.m_attackInterspace = context.GetAttackInterspace();
							this.m_attackData = BossStateAttackMap3.ATTACK_DATA_NONE;
							this.m_state = BossStateAttackMap3.State.Bom;
						}
						else
						{
							this.m_state = BossStateAttackMap3.State.CreateBomNext;
						}
						base.ResetTime();
					}
				}
				break;
			case BossStateAttackMap3.State.CreateBomNext:
				if (base.UpdateTime(delta, 0.2f))
				{
					base.ResetTime();
					this.m_state = BossStateAttackMap3.State.Start;
				}
				break;
			case BossStateAttackMap3.State.Bom:
				if (base.UpdateTime(delta, this.m_attackInterspace))
				{
					base.ResetTime();
					this.m_state = BossStateAttackMap3.State.Start;
				}
				break;
			}
		}
	}
}
