using Message;
using System;
using Tutorial;
using UnityEngine;

namespace Player
{
	public class StateRunLoop : FSMState<CharacterState>
	{
		private const float MaxSpeed = 20f;

		private float m_speed;

		private bool m_onBoost;

		private bool m_onBoostEx;

		private CharacterLoopEffect m_loopEffect;

		private CharacterLoopEffect m_exLoopEffect;

		private GameObject m_effectParaloop;

		private float m_effectTime;

		public override void Enter(CharacterState context)
		{
			if (context.TestStatus(Status.NowLanding))
			{
				context.GetAnimator().CrossFade("Landing", 0.05f);
			}
			else
			{
				context.GetAnimator().CrossFade("Run", 0.1f);
			}
			RunLoopPathParameter enteringParameter = context.GetEnteringParameter<RunLoopPathParameter>();
			if (enteringParameter != null && enteringParameter.m_pathComponent != null)
			{
				float? distance = null;
				context.ChangeMovement(MOVESTATE_ID.RunOnPath);
				CharacterMoveOnPath currentState = context.Movement.GetCurrentState<CharacterMoveOnPath>();
				if (currentState != null)
				{
					currentState.SetupPath(context.Position, enteringParameter.m_pathComponent, distance);
				}
			}
			this.m_speed = context.Movement.HorzVelocity.magnitude;
			this.m_loopEffect = GameObjectUtil.FindChildGameObjectComponent<CharacterLoopEffect>(context.gameObject, "CharacterBoost");
			this.m_exLoopEffect = GameObjectUtil.FindChildGameObjectComponent<CharacterLoopEffect>(context.gameObject, "CharacterBoostEx");
			this.m_effectTime = 0f;
			float num = 0f;
			this.m_onBoost = StateUtil.SetRunningAnimationSpeed(context, ref num);
			if (this.m_onBoost)
			{
				this.CreateParaLoop(context);
			}
			context.ClearAirAction();
			context.SetNotCharaChange(true);
			context.SetNotUseItem(true);
			if (context.GetCamera() != null)
			{
				MsgPushCamera value = new MsgPushCamera(CameraType.LOOP_TERRAIN, 0.5f, null);
				context.GetCamera().SendMessage("OnPushCamera", value);
			}
			ObjUtil.PauseCombo(MsgPauseComboTimer.State.PAUSE, -1f);
			ObjUtil.SetQuickModeTimePause(true);
			ObjUtil.SetDisableEquipItem(true);
		}

		public override void Leave(CharacterState context)
		{
			this.m_onBoost = false;
			StateUtil.SetOnBoost(context, this.m_loopEffect, false);
			this.DestroyParaloop(context);
			context.SetNotCharaChange(false);
			context.SetNotUseItem(false);
			if (context.GetCamera() != null)
			{
				MsgPopCamera value = new MsgPopCamera(CameraType.LOOP_TERRAIN, 2.5f);
				context.GetCamera().SendMessage("OnPopCamera", value);
			}
			ObjUtil.PauseCombo(MsgPauseComboTimer.State.PLAY, -1f);
			this.m_loopEffect = null;
			if (this.m_exLoopEffect != null)
			{
				StateUtil.SetOnBoostEx(context, this.m_loopEffect, false);
				this.m_exLoopEffect = null;
			}
			ObjUtil.SetQuickModeTimePause(false);
			ObjUtil.SetDisableEquipItem(false);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			CharacterMoveOnPath currentState = context.Movement.GetCurrentState<CharacterMoveOnPath>();
			if (currentState != null && currentState.IsPathEnd(0.1f))
			{
				context.ChangeState(STATE_ID.Run);
				return;
			}
			this.m_speed = this.GetRunningSpeed(context, deltaTime);
			context.Movement.Velocity = context.Movement.GetForwardDir() * this.m_speed;
			float num = 0f;
			bool onBoost = this.m_onBoost;
			this.m_onBoost = StateUtil.SetRunningAnimationSpeed(context, ref num);
			this.CheckOnBoost(context, onBoost);
			this.CheckOnBoostEx(context, num);
			if (!this.m_onBoost)
			{
				StateUtil.CheckAndCreateRunEffect(context, ref this.m_effectTime, this.m_speed, num, deltaTime);
			}
		}

		private float GetRunningSpeed(CharacterState context, float deltaTime)
		{
			float speed = this.m_speed;
			float num = 20f;
			float minLoopRunSpeed = context.Parameter.m_minLoopRunSpeed;
			float num2 = Mathf.Max(minLoopRunSpeed, context.DefaultSpeed);
			Vector3 forwardDir = context.Movement.GetForwardDir();
			bool flag = Vector3.Dot(forwardDir, context.Movement.GetGravityDir()) > 0.1736f;
			float runLoopAccel = context.Parameter.m_runLoopAccel;
			if (flag)
			{
				num2 = num;
			}
			if (speed > num2)
			{
				this.m_speed = Mathf.Max(speed - context.Parameter.m_runDec * deltaTime, num2);
			}
			else if (speed < minLoopRunSpeed)
			{
				this.m_speed = minLoopRunSpeed;
			}
			else
			{
				this.m_speed = Mathf.Min(speed + runLoopAccel * deltaTime, num2);
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

		private void CreateParaLoop(CharacterState context)
		{
			this.m_effectParaloop = StateUtil.CreateEffect(context, "ef_pl_paraloop01", false);
			if (this.m_effectParaloop)
			{
				StateUtil.SetObjectLocalPositionToCenter(context, this.m_effectParaloop);
			}
			SoundManager.SePlay("act_paraloop", "SE");
			context.SetStatus(Status.Paraloop, true);
			ObjUtil.RequestStartAbilityToChao(ChaoAbility.LOOP_COMBO_UP, false);
			ObjUtil.RequestStartAbilityToChao(ChaoAbility.LOOP_MAGNET, false);
			ObjUtil.SendMessageTutorialClear(EventID.PARA_LOOP);
		}

		private void DestroyParaloop(CharacterState context)
		{
			if (this.m_effectParaloop != null)
			{
				StateUtil.DestroyParticle(this.m_effectParaloop, 1f);
				this.m_effectParaloop = null;
			}
			context.SetStatus(Status.Paraloop, false);
		}
	}
}
