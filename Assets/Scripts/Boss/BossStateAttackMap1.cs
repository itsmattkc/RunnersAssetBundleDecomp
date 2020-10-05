using System;
using UnityEngine;

namespace Boss
{
	public class BossStateAttackMap1 : BossStateAttackBase
	{
		private enum State
		{
			Idle,
			Start,
			Bom
		}

		private BossStateAttackMap1.State m_state;

		private float m_attackInterspace;

		public override void Enter(ObjBossEggmanState context)
		{
			base.Enter(context);
			context.DebugDrawState("BossStateAttackMap1");
			this.m_state = BossStateAttackMap1.State.Start;
			this.m_attackInterspace = 0f;
		}

		public override void Leave(ObjBossEggmanState context)
		{
			base.Leave(context);
		}

		public override void Step(ObjBossEggmanState context, float delta)
		{
			BossStateAttackMap1.State state = this.m_state;
			if (state != BossStateAttackMap1.State.Start)
			{
				if (state == BossStateAttackMap1.State.Bom)
				{
					if (base.UpdateTime(delta, this.m_attackInterspace))
					{
						this.m_state = BossStateAttackMap1.State.Start;
					}
				}
			}
			else if (!context.IsPlayerDead())
			{
				if (context.IsBossDistanceEnd())
				{
					context.ChangeState(STATE_ID.PassMapDistanceEnd);
				}
				else
				{
					bool flag = true;
					if (context.BossParam.AttackBallFlag)
					{
						flag = false;
						context.BossParam.AttackBallFlag = false;
					}
					else
					{
						int randomRange = ObjUtil.GetRandomRange100();
						if (randomRange < context.BossParam.TrapRand && context.BossParam.AttackTrapCount < context.BossParam.AttackTrapCountMax)
						{
							flag = false;
						}
					}
					if (!flag)
					{
						this.CreateBall(context, BossBallType.TRAP);
						context.BossParam.AttackTrapCount++;
					}
					else
					{
						this.CreateBall(context, BossBallType.ATTACK);
						context.BossParam.AttackBallFlag = true;
						context.BossParam.AttackTrapCount = 0;
					}
					base.ResetTime();
					this.m_attackInterspace = context.GetAttackInterspace();
					this.m_state = BossStateAttackMap1.State.Bom;
				}
			}
		}

		private void CreateBall(ObjBossEggmanState context, BossBallType type)
		{
			GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.ENEMY_PREFAB, "ObjBossBall");
			if (gameObject != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, ObjBossUtil.GetBossHatchPos(context.gameObject), Quaternion.identity) as GameObject;
				if (gameObject2)
				{
					gameObject2.gameObject.SetActive(true);
					ObjBossBall component = gameObject2.GetComponent<ObjBossBall>();
					if (component)
					{
						component.Setup(new ObjBossBall.SetData
						{
							obj = context.gameObject,
							bound_param = context.GetBoundParam(),
							type = type,
							shot_rot = context.GetShotRotation(context.BossParam.ShotRotBase),
							shot_speed = context.BossParam.ShotSpeed,
							attack_speed = context.BossParam.AttackSpeed,
							firstSpeed = 0f,
							outOfcontrol = 0f,
							ballSpeed = context.BossParam.BallSpeed,
							bossAppear = true
						});
					}
				}
			}
		}
	}
}
