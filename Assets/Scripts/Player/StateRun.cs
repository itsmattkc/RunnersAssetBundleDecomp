using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StateRun : FSMState<CharacterState>
	{
		private float m_speed;

		private float m_effectTime;

		private bool m_onBoost;

		private bool m_onBoostEx;

		private CharacterLoopEffect m_loopEffect;

		private CharacterLoopEffect m_exLoopEffect;

		private WispBoostLevel m_bossBoostLevel;

		public override void Enter(CharacterState context)
		{
			if (context.TestStatus(Status.NowLanding))
			{
				context.GetAnimator().CrossFade("Landing", 0.1f);
			}
			else
			{
				context.GetAnimator().CrossFade("Run", 0.1f);
			}
			context.ChangeMovement(MOVESTATE_ID.Run);
			this.m_speed = context.Movement.GetForwardVelocityScalar();
			this.m_loopEffect = GameObjectUtil.FindChildGameObjectComponent<CharacterLoopEffect>(context.gameObject, "CharacterBoost");
			this.m_exLoopEffect = GameObjectUtil.FindChildGameObjectComponent<CharacterLoopEffect>(context.gameObject, "CharacterBoostEx");
			context.ClearAirAction();
			this.m_effectTime = 0f;
			this.m_bossBoostLevel = context.BossBoostLevel;
		}

		public override void Leave(CharacterState context)
		{
			this.m_onBoost = false;
			this.m_onBoostEx = false;
			StateUtil.SetOnBoost(context, this.m_loopEffect, false);
			this.m_loopEffect = null;
			if (this.m_exLoopEffect != null)
			{
				StateUtil.SetOnBoostEx(context, this.m_exLoopEffect, false);
				this.m_exLoopEffect = null;
			}
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			bool flag = context.Movement.IsOnGround();
			Vector3 vector = context.Movement.VertVelocity;
			this.m_speed = this.GetRunningSpeed(context, deltaTime);
			context.Movement.HorzVelocity = context.Movement.GetForwardDir() * this.GetRunningSpeed(context, deltaTime);
			HitInfo hitInfo;
			if (flag && context.Movement.GetGroundInfo(out hitInfo))
			{
				StateUtil.SetRotateOnGround(context);
				vector += context.Movement.GetGravity() * deltaTime;
				context.Movement.VertVelocity = vector;
			}
			if (context.m_input.IsTouched())
			{
				context.ChangeState(STATE_ID.Jump);
				return;
			}
			if (!flag)
			{
				context.ChangeState(STATE_ID.Fall);
				return;
			}
			STATE_ID state = STATE_ID.Non;
			if (StateUtil.CheckHitWallAndGoDeadOrStumble(context, deltaTime, ref state))
			{
				context.ChangeState(state);
				return;
			}
			bool onBoost = this.m_onBoost;
			float num = 0f;
			this.m_onBoost = StateUtil.SetRunningAnimationSpeed(context, ref num);
			this.CheckOnBoost(context, onBoost);
			this.CheckOnBoostEx(context, num);
			if (context.BossBoostLevel != this.m_bossBoostLevel)
			{
				this.m_effectTime = 0f;
				this.m_bossBoostLevel = context.BossBoostLevel;
			}
			if (!this.m_onBoost || context.BossBoostLevel != WispBoostLevel.NONE)
			{
				StateUtil.CheckAndCreateRunEffect(context, ref this.m_effectTime, this.m_speed, num, deltaTime);
			}
		}

		public override bool DispatchMessage(CharacterState context, int messageId, MessageBase msg)
		{
			MsgRunLoopPath msgRunLoopPath = msg as MsgRunLoopPath;
			if (msgRunLoopPath != null)
			{
				RunLoopPathParameter runLoopPathParameter = context.CreateEnteringParameter<RunLoopPathParameter>();
				runLoopPathParameter.Set(msgRunLoopPath.m_component);
				context.ChangeState(STATE_ID.RunLoop);
			}
			return true;
		}

		private float GetRunningSpeed(CharacterState context, float deltaTime)
		{
			float speed = this.m_speed;
			float maxSpeed = context.Parameter.m_maxSpeed;
			float num = context.DefaultSpeed;
			float defaultSpeed = context.DefaultSpeed;
			Vector3 forwardDir = context.Movement.GetForwardDir();
			bool flag = Vector3.Dot(forwardDir, context.Movement.GetGravityDir()) > 0.1736f;
			float runAccel = context.Parameter.m_runAccel;
			if (flag)
			{
				num = maxSpeed;
			}
			if (speed > num)
			{
				this.m_speed = Mathf.Max(speed - context.Parameter.m_runDec * deltaTime, num);
			}
			else if (speed < defaultSpeed)
			{
				this.m_speed = defaultSpeed;
			}
			else
			{
				this.m_speed = Mathf.Min(speed + runAccel * deltaTime, num);
			}
			return this.m_speed;
		}

		private void CheckOnBoost(CharacterState context, bool oldBoost)
		{
			if (!oldBoost && this.m_onBoost)
			{
				StateUtil.SetOnBoost(context, this.m_loopEffect, true);
			}
			else if (oldBoost && !this.m_onBoost)
			{
				StateUtil.SetOnBoost(context, this.m_loopEffect, false);
			}
		}

		private void CheckOnBoostEx(CharacterState context, float speed)
		{
			if (this.m_exLoopEffect != null)
			{
				bool flag = 0.6f < speed && speed < 0.9f;
				if (flag && !this.m_onBoostEx)
				{
					StateUtil.SetOnBoostEx(context, this.m_exLoopEffect, true);
					this.m_onBoostEx = true;
				}
				else if (!flag && this.m_onBoostEx)
				{
					StateUtil.SetOnBoostEx(context, this.m_exLoopEffect, false);
					this.m_onBoostEx = false;
				}
			}
		}
	}
}
