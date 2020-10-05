using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StateDamage : FSMState<CharacterState>
	{
		private float m_timer;

		public override void Enter(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.Run);
			context.Movement.OffGround();
			this.m_timer = context.Parameter.m_damageStumbleTime;
			context.GetAnimator().CrossFade("Damaged", 0.05f);
			context.Movement.HorzVelocity = context.Movement.GetForwardDir() * context.DefaultSpeed * context.Parameter.m_damageSpeedRate;
			context.StartDamageBlink();
			if (!context.m_notDropRing)
			{
				SoundManager.SePlay("act_ringspread", "SE");
			}
			context.ClearAirAction();
			if (StageTutorialManager.Instance)
			{
				MsgTutorialDamage value = new MsgTutorialDamage();
				StageTutorialManager.Instance.SendMessage("OnMsgTutorialDamage", value, SendMessageOptions.DontRequireReceiver);
			}
			GameObjectUtil.SendDelayedMessageToTagObjects("Boss", "OnPlayerDamage", new MsgBossPlayerDamage(false));
			ObjUtil.StopCombo();
		}

		public override void Leave(CharacterState context)
		{
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			bool flag = context.Movement.IsOnGround();
			Vector3 vector = context.Movement.VertVelocity;
			if (flag)
			{
				HitInfo hitInfo;
				if (context.Movement.GetGroundInfo(out hitInfo))
				{
					Vector3 normal = hitInfo.info.normal;
					vector -= Vector3.Project(vector, normal);
					context.Movement.VertVelocity = vector;
				}
			}
			else
			{
				vector += context.Movement.GetGravity() * deltaTime;
				context.Movement.VertVelocity = vector;
			}
			context.Movement.HorzVelocity = context.Movement.GetForwardDir() * context.DefaultSpeed * context.Parameter.m_damageSpeedRate;
			this.m_timer -= deltaTime;
			if (this.m_timer <= context.Parameter.m_damageEnableJumpTime && context.m_input.IsTouched())
			{
				context.ChangeState(STATE_ID.Jump);
				return;
			}
			if (this.m_timer <= 0f)
			{
				if (flag)
				{
					context.ChangeState(STATE_ID.Run);
				}
				else
				{
					context.ChangeState(STATE_ID.Fall);
				}
				return;
			}
			STATE_ID state = STATE_ID.Non;
			if (StateUtil.CheckHitWallAndGoDeadOrStumble(context, deltaTime, ref state))
			{
				context.ChangeState(state);
				return;
			}
		}
	}
}
