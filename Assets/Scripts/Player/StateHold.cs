using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StateHold : FSMState<CharacterState>
	{
		public override void Enter(CharacterState context)
		{
			StateUtil.ResetVelocity(context);
			context.ChangeMovement(MOVESTATE_ID.IgnoreCollision);
			Collider[] componentsInChildren = context.GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Collider collider = array[i];
				collider.enabled = false;
			}
			Collider component = context.GetComponent<Collider>();
			if (component)
			{
				component.enabled = false;
			}
			MsgChaoStateUtil.SendMsgChaoState(MsgChaoState.State.STOP);
			context.SetNotCharaChange(true);
			context.SetNotUseItem(true);
			context.SetStatus(Status.Hold, true);
		}

		public override void Leave(CharacterState context)
		{
			Collider[] componentsInChildren = context.GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Collider collider = array[i];
				collider.enabled = true;
			}
			Collider component = context.GetComponent<Collider>();
			if (component)
			{
				component.enabled = true;
			}
			MsgChaoStateUtil.SendMsgChaoState(MsgChaoState.State.STOP_END);
			context.SetNotCharaChange(false);
			context.SetNotUseItem(false);
			context.SetStatus(Status.Hold, false);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			StateUtil.ResetVelocity(context);
		}

		public override bool DispatchMessage(CharacterState context, int messageId, MessageBase msg)
		{
			int iD = msg.ID;
			if (iD == 12311)
			{
				StateUtil.SetVelocityForwardRun(context, false);
				context.ChangeState(STATE_ID.Run);
				return true;
			}
			if (iD != 20483)
			{
				return false;
			}
			StateUtil.SetVelocityForwardRun(context, false);
			context.ChangeState(STATE_ID.Run);
			return true;
		}
	}
}
