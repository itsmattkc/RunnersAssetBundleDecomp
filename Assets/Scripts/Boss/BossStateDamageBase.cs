using System;
using UnityEngine;

namespace Boss
{
	public class BossStateDamageBase : FSMState<ObjBossEggmanState>
	{
		private enum State
		{
			Idle,
			SpeedDown,
			Wait
		}

		private const float START_SPEED = 60f;

		private const float START_DOWNSPEED = 120f;

		private const float ROT_SPEED = 30f;

		private const float ROT_DOWNSPEED = 0.3f;

		private const float ROT_MIN = 10f;

		private BossStateDamageBase.State m_state;

		private Quaternion m_start_rot = Quaternion.identity;

		private float m_speed_down;

		private float m_rot_speed;

		private float m_rot_speed_down;

		private float m_rot_time;

		private float m_rot_min;

		private Vector3 m_rot_agl = Vector3.zero;

		private float m_distance;

		private bool m_rot_end;

		public override void Enter(ObjBossEggmanState context)
		{
			context.DebugDrawState(this.GetStateName());
			context.SetHitCheck(false);
			context.AddDamage();
			context.BossEffect.PlayHitEffect();
			context.BossEffect.PlayBoostEffect(ObjBossEggmanEffect.BoostType.Normal);
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
			this.m_state = BossStateDamageBase.State.SpeedDown;
			this.m_start_rot = context.transform.rotation;
			this.m_speed_down = 0f;
			this.m_rot_speed = 0f;
			this.m_rot_speed_down = 0f;
			this.m_rot_time = 0f;
			this.m_rot_min = 0f;
			this.m_rot_agl = context.transform.right;
			this.m_distance = 0f;
			this.m_rot_end = false;
			this.Setup(context);
			if (context.ColorPowerHit)
			{
				context.DebugDrawState(this.GetStateName() + "ColorPowerHit");
				this.SetupColorPowerDamage(context);
			}
			else
			{
				context.CreateFeverRing();
			}
		}

		public override void Leave(ObjBossEggmanState context)
		{
			context.ColorPowerHit = false;
			context.ChaoHit = false;
		}

		public override void Step(ObjBossEggmanState context, float delta)
		{
			context.UpdateSpeedDown(delta, this.m_speed_down);
			if (!this.m_rot_end)
			{
				float d = delta * 60f * this.m_rot_speed;
				this.m_rot_speed -= delta * 60f * this.m_rot_speed_down;
				context.transform.rotation = Quaternion.Euler(this.m_rot_agl * d) * context.transform.rotation;
				float x = context.transform.rotation.eulerAngles.x;
				if (this.m_rot_speed < this.m_rot_min && x > 270f && x < 359f)
				{
					this.SetMotion(context, false);
					this.m_rot_time = 0f;
					this.m_rot_end = true;
				}
			}
			if (this.m_rot_end)
			{
				this.m_rot_time += delta * 5f;
				context.transform.rotation = Quaternion.Slerp(context.transform.rotation, this.m_start_rot, this.m_rot_time);
			}
			BossStateDamageBase.State state = this.m_state;
			if (state != BossStateDamageBase.State.SpeedDown)
			{
				if (state == BossStateDamageBase.State.Wait)
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
							this.ChangeStateWait(context);
						}
					}
				}
			}
			else if (context.BossParam.Speed < context.BossParam.PlayerSpeed)
			{
				this.SetStateSpeedDown(context);
				this.m_state = BossStateDamageBase.State.Wait;
			}
		}

		protected virtual string GetStateName()
		{
			return string.Empty;
		}

		protected virtual void Setup(ObjBossEggmanState context)
		{
		}

		private void SetupColorPowerDamage(ObjBossEggmanState context)
		{
			float damageSpeedParam = context.GetDamageSpeedParam();
			context.SetSpeed(60f * damageSpeedParam);
			this.SetSpeedDown(120f);
			this.SetRotSpeed(30f);
			this.SetRotSpeedDown(0.3f);
			this.SetRotMin(10f);
			this.SetRotAngle(-context.transform.right);
		}

		private void SetMotion(ObjBossEggmanState context, bool flag)
		{
			if (flag)
			{
				context.BossMotion.SetMotion(BossMotion.DAMAGE_R_HC, flag);
			}
			else if (context.BossParam.BossHP > 0)
			{
				context.BossMotion.SetMotion(BossMotion.DAMAGE_R_HC, false);
			}
			else
			{
				context.BossEffect.PlayEscapeEffect(context);
				context.BossMotion.SetMotion(BossMotion.ESCAPE, true);
			}
		}

		protected virtual void SetStateSpeedDown(ObjBossEggmanState context)
		{
		}

		protected virtual void ChangeStateWait(ObjBossEggmanState context)
		{
		}

		protected void SetRotAngle(Vector3 angle)
		{
			this.m_rot_agl = angle;
		}

		protected void SetSpeedDown(float val)
		{
			this.m_speed_down = val;
		}

		protected void SetRotSpeed(float val)
		{
			this.m_rot_speed = val;
		}

		protected void SetRotSpeedDown(float val)
		{
			this.m_rot_speed_down = val;
		}

		protected void SetRotMin(float val)
		{
			this.m_rot_min = val;
		}

		protected void SetDistance(float val)
		{
			this.m_distance = val;
		}
	}
}
