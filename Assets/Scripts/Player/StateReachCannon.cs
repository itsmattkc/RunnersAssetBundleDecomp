using App.Utility;
using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StateReachCannon : FSMState<CharacterState>
	{
		private enum Flags
		{
			LAUNCH_CANNON,
			MODEL_OFF
		}

		private enum SubState
		{
			JUMP,
			GOTARGET,
			HOLD
		}

		private const float ReachedTime = 0.5f;

		private const float HeightOffset = 1.5f;

		private const float GoTargetTime = 0.2f;

		private Vector3 m_reachPosition;

		private float m_height;

		private GameObject m_catchedObject;

		private Vector3 m_horzVelocity;

		private Vector3 m_vertVelocity;

		private float m_timer;

		private Bitset32 m_flag;

		private StateReachCannon.SubState m_substate;

		public override void Enter(CharacterState context)
		{
			CannonReachParameter enteringParameter = context.GetEnteringParameter<CannonReachParameter>();
			if (enteringParameter != null)
			{
				this.m_reachPosition = enteringParameter.m_position;
				this.m_height = enteringParameter.m_height;
				this.m_catchedObject = enteringParameter.m_catchedObject;
			}
			else
			{
				this.m_reachPosition = context.Position;
				this.m_height = 0f;
				this.m_catchedObject = null;
			}
			this.m_flag.Reset();
			context.ChangeMovement(MOVESTATE_ID.Air);
			this.m_substate = StateReachCannon.SubState.JUMP;
			StateUtil.SetAirMovementToRotateGround(context, false);
			context.GetAnimator().CrossFade("SpinBall", 0.1f);
			this.CalcReachedVelocity(context);
			this.m_timer = 0.25f;
			context.SetNotCharaChange(true);
			context.SetNotUseItem(true);
			context.ClearAirAction();
			ObjUtil.PauseCombo(MsgPauseComboTimer.State.PAUSE, -1f);
			ObjUtil.SetDisableEquipItem(true);
		}

		public override void Leave(CharacterState context)
		{
			if (!this.m_flag.Test(0))
			{
				if (this.m_flag.Test(1))
				{
					context.SetModelNotDraw(false);
					StateUtil.SetNotDrawItemEffect(context, false);
				}
				if (this.m_catchedObject != null)
				{
					MsgOnExitAbideObject value = new MsgOnExitAbideObject();
					this.m_catchedObject.SendMessage("OnExitAbideObject", value);
				}
			}
			context.SetNotCharaChange(false);
			context.SetNotUseItem(false);
			ObjUtil.PauseCombo(MsgPauseComboTimer.State.PLAY, -1f);
			ObjUtil.SetDisableEquipItem(false);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			switch (this.m_substate)
			{
			case StateReachCannon.SubState.JUMP:
				context.Movement.Velocity += context.Movement.GetGravity() * deltaTime;
				this.m_timer -= deltaTime;
				if (this.m_timer <= 0f)
				{
					context.ChangeMovement(MOVESTATE_ID.GoTarget);
					StateUtil.SetTargetMovement(context, this.m_reachPosition, context.transform.rotation, 0.2f);
					this.m_timer = 0.2f;
					this.m_substate = StateReachCannon.SubState.GOTARGET;
				}
				break;
			case StateReachCannon.SubState.GOTARGET:
				if (!this.m_flag.Test(1) && (this.m_reachPosition - context.Position).sqrMagnitude < this.m_height * this.m_height)
				{
					context.SetModelNotDraw(true);
					StateUtil.SetNotDrawItemEffect(context, true);
					this.m_flag.Set(1, true);
				}
				this.m_timer -= deltaTime;
				if (this.m_timer <= 0f)
				{
					if (!this.m_flag.Test(1))
					{
						context.SetModelNotDraw(true);
						StateUtil.SetNotDrawItemEffect(context, true);
						this.m_flag.Set(1, true);
					}
					if (this.m_catchedObject != null)
					{
						MsgOnAbidePlayerLocked value = new MsgOnAbidePlayerLocked();
						this.m_catchedObject.SendMessage("OnAbidePlayerLocked", value);
					}
					StateUtil.ResetVelocity(context);
					context.ChangeMovement(MOVESTATE_ID.Air);
					this.m_substate = StateReachCannon.SubState.HOLD;
				}
				break;
			case StateReachCannon.SubState.HOLD:
				StateUtil.ResetVelocity(context);
				break;
			}
		}

		public override bool DispatchMessage(CharacterState context, int messageId, MessageBase msg)
		{
			if (messageId != 24578)
			{
				return false;
			}
			if (this.m_substate != StateReachCannon.SubState.HOLD)
			{
				return true;
			}
			MsgOnCannonImpulse msgOnCannonImpulse = msg as MsgOnCannonImpulse;
			if (msgOnCannonImpulse != null)
			{
				CannonLaunchParameter cannonLaunchParameter = context.CreateEnteringParameter<CannonLaunchParameter>();
				if (cannonLaunchParameter != null)
				{
					cannonLaunchParameter.Set(msgOnCannonImpulse.m_position, msgOnCannonImpulse.m_rotation, msgOnCannonImpulse.m_firstSpeed, this.m_height, msgOnCannonImpulse.m_outOfControl);
					this.m_flag.Set(0, true);
					context.ChangeState(STATE_ID.LaunchCannon);
					msgOnCannonImpulse.m_succeed = true;
				}
				return true;
			}
			return true;
		}

		private void CalcReachedVelocity(CharacterState context)
		{
			Vector3 vector = this.m_reachPosition - context.Position;
			Vector3 b = Vector3.Project(vector, -context.Movement.GetGravityDir());
			Vector3 a = vector - b;
			Vector3 gravity = context.Movement.GetGravity();
			float num = 0.5f;
			float num2 = b.magnitude + 1.5f;
			float d = (num2 + 0.5f * gravity.magnitude * num * num) / num;
			this.m_vertVelocity = Vector3.up * d;
			this.m_horzVelocity = a / 0.5f;
			context.Movement.Velocity = this.m_vertVelocity + this.m_horzVelocity;
		}
	}
}
