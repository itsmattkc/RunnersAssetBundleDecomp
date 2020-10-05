using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StateFallingDead : FSMState<CharacterState>
	{
		private float m_timer;

		private bool m_sendMessage;

		public override void Enter(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.Air);
			StateUtil.SetAirMovementToRotateGround(context, true);
			context.Movement.HorzVelocity = Vector3.zero;
			context.GetAnimator().CrossFade("Fall", 0.2f);
			StateUtil.Dead(context);
			SoundManager.SePlay("act_fall", "SE");
			this.m_timer = 0f;
			this.m_sendMessage = false;
			MsgChaoStateUtil.SendMsgChaoState(MsgChaoState.State.STOP);
			if (StageTutorialManager.Instance)
			{
				MsgTutorialMiss value = new MsgTutorialMiss();
				StageTutorialManager.Instance.SendMessage("OnMsgTutorialMiss", value, SendMessageOptions.DontRequireReceiver);
			}
			ObjUtil.SetPlayerDeadRecoveryRing(context.GetPlayerInformation());
			if (context.NowPhantomType != PhantomType.NONE)
			{
				ItemType item = ItemType.UNKNOWN;
				switch (context.NowPhantomType)
				{
				case PhantomType.LASER:
					item = ItemType.LASER;
					break;
				case PhantomType.DRILL:
					item = ItemType.DRILL;
					break;
				case PhantomType.ASTEROID:
					item = ItemType.ASTEROID;
					break;
				}
				StateUtil.SendMessageToTerminateItem(item);
				context.NowPhantomType = PhantomType.NONE;
			}
		}

		public override void Leave(CharacterState context)
		{
			context.SetStatus(Status.Dead, false);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			if (this.m_timer >= 1f)
			{
				context.Movement.Velocity = Vector3.zero;
				if (!this.m_sendMessage)
				{
					StateUtil.CheckCharaChangeOnDieAndSendMessage(context);
					this.m_sendMessage = true;
				}
			}
			else
			{
				this.m_timer += deltaTime;
				context.Movement.Velocity = context.Movement.Velocity + context.Movement.GetGravity() * deltaTime;
			}
		}
	}
}
