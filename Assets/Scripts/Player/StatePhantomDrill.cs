using Message;
using System;
using UnityEngine;

namespace Player
{
	public class StatePhantomDrill : FSMState<CharacterState>
	{
		private enum SubState
		{
			GODOWN,
			RUN,
			RETURN
		}

		private const float jump_force = 14f;

		private const float lerpDelta = 3f;

		private const float godown_offset = 1.5f;

		private const float ray_offset = 3f;

		private const float ray_offset2 = 5f;

		private const float offset_noground = 2f;

		private float m_time;

		private StatePhantomDrill.SubState m_substate;

		private GameObject m_truck;

		private Vector3 m_targetPos = Vector3.zero;

		private PathEvaluator m_targetPath;

		private GameObject m_effect;

		private Vector3 m_prevPosition;

		private bool m_nowInDirt;

		private bool m_changePhantomCancel;

		public override void Enter(CharacterState context)
		{
			StateUtil.DeactiveInvincible(context);
			StateUtil.SetNotDrawItemEffect(context, true);
			StateUtil.SetRotation(context, Vector3.up, CharacterDefs.BaseFrontTangent);
			this.m_effect = PhantomDrillUtil.ChangeVisualOnEnter(context);
			this.m_truck = PhantomDrillUtil.CreateTruck(context);
			context.OnAttack(AttackPower.PlayerColorPower, DefensePower.PlayerColorPower);
			context.OnAttackAttribute(AttackAttribute.PhantomDrill);
			this.m_time = -1f;
			ChangePhantomParameter enteringParameter = context.GetEnteringParameter<ChangePhantomParameter>();
			if (enteringParameter != null)
			{
				this.m_time = enteringParameter.Timer;
			}
			this.m_targetPath = StateUtil.GetStagePathEvaluator(context, BlockPathController.PathType.DRILL);
			if (this.m_targetPath != null)
			{
				this.m_targetPos = this.m_targetPath.GetWorldPosition();
			}
			this.GotoDown(context);
			this.m_prevPosition = context.Position;
			this.m_nowInDirt = false;
			StateUtil.SendMessageTransformPhantom(context, PhantomType.DRILL);
			if (context.GetChangePhantomCancel() == ItemType.DRILL)
			{
				this.m_changePhantomCancel = true;
			}
			else
			{
				this.m_changePhantomCancel = false;
			}
		}

		public override void Leave(CharacterState context)
		{
			context.OffAttack();
			StateUtil.SetNotDrawItemEffect(context, false);
			PhantomDrillUtil.ChangeVisualOnLeave(context, this.m_effect);
			PhantomDrillUtil.DestroyTruck(this.m_truck);
			this.m_effect = null;
			this.m_truck = null;
			this.m_targetPath = null;
			StateUtil.SendMessageReturnFromPhantom(context, PhantomType.DRILL);
			context.SetChangePhantomCancel(ItemType.UNKNOWN);
		}

		public override void Step(CharacterState context, float deltaTime)
		{
			if (this.m_changePhantomCancel)
			{
				this.m_changePhantomCancel = false;
				this.DispatchMessage(context, 12289, new MsgInvalidateItem(ItemType.DRILL));
				return;
			}
			switch (this.m_substate)
			{
			case StatePhantomDrill.SubState.GODOWN:
				if (this.StepGoDown(context, deltaTime))
				{
					return;
				}
				break;
			case StatePhantomDrill.SubState.RUN:
				if (this.StepRunning(context, deltaTime))
				{
					return;
				}
				break;
			case StatePhantomDrill.SubState.RETURN:
				if (this.StepReturn(context, deltaTime))
				{
					return;
				}
				break;
			}
			bool nowInDirt = this.m_nowInDirt;
			this.m_nowInDirt = PhantomDrillUtil.CheckTruckDraw(context, this.m_truck);
			if ((nowInDirt && !this.m_nowInDirt) || (!nowInDirt && this.m_nowInDirt))
			{
				PhantomDrillUtil.CheckAndCreateFogEffect(context, !nowInDirt && this.m_nowInDirt, this.m_prevPosition);
			}
			this.m_prevPosition = context.Position;
		}

		private bool StepGoDown(CharacterState context, float deltaTime)
		{
			float magnitude = context.Movement.Velocity.magnitude;
			float num = Vector3.Distance(this.m_targetPos, context.Position);
			Vector3 vector = Vector3.Normalize(this.m_targetPos - context.Position);
			if (num > magnitude * deltaTime)
			{
				Vector3 up = Vector3.Cross(vector, context.transform.right);
				StateUtil.SetRotation(context, up, vector);
				context.Movement.Velocity = vector * context.Parameter.m_drillSpeed;
			}
			else
			{
				context.Movement.ResetPosition(this.m_targetPos);
				this.GotoRun(context);
			}
			return false;
		}

		private bool StepRunning(CharacterState context, float deltaTime)
		{
			Vector3 velocity = context.Parameter.m_drillSpeed * CharacterDefs.BaseFrontTangent;
			context.Movement.Velocity = velocity;
			if (this.m_time > 0f)
			{
				this.m_time -= deltaTime;
				if (this.m_time < 0f)
				{
					this.GotoReturn(context);
					return false;
				}
			}
			CharacterMoveOnPathPhantomDrill currentState = context.Movement.GetCurrentState<CharacterMoveOnPathPhantomDrill>();
			if (currentState != null && currentState.IsPathEnd(0f))
			{
				this.GotoReturn(context);
				return false;
			}
			if (currentState != null && context.m_input.IsTouched())
			{
				currentState.Jump(context.Movement);
			}
			return false;
		}

		private bool StepReturn(CharacterState context, float deltaTime)
		{
			float magnitude = context.Movement.Velocity.magnitude;
			float num = Vector3.Distance(this.m_targetPos, context.Position);
			Vector3 vector = Vector3.Normalize(this.m_targetPos - context.Position);
			if (num > magnitude * deltaTime)
			{
				Vector3 up = Vector3.Cross(vector, context.transform.right);
				StateUtil.SetRotation(context, up, vector);
				context.Movement.Velocity = vector * context.Parameter.m_drillSpeed;
				return false;
			}
			StateUtil.SetRotation(context, Vector3.up, CharacterDefs.BaseFrontTangent);
			context.Movement.ResetPosition(this.m_targetPos);
			context.Movement.Velocity = context.Movement.GetForwardDir() * context.DefaultSpeed + context.Movement.GetUpDir() * 14f;
			StateUtil.ReturnFromPhantomAndChangeState(context, PhantomType.DRILL, false);
			return true;
		}

		private void GotoDown(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.IgnoreCollision);
			this.m_substate = StatePhantomDrill.SubState.GODOWN;
		}

		private void GotoRun(CharacterState context)
		{
			this.StartPathMove(context);
			this.m_substate = StatePhantomDrill.SubState.RUN;
		}

		private void GotoReturn(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.IgnoreCollision);
			PathEvaluator stagePathEvaluator = StateUtil.GetStagePathEvaluator(context, BlockPathController.PathType.SV);
			if (stagePathEvaluator != null)
			{
				this.m_targetPos = stagePathEvaluator.GetWorldPosition();
				Vector3 a = -context.Movement.GetGravityDir();
				Vector3 origin = this.m_targetPos + a * 1.5f;
				Ray ray = new Ray(origin, -a);
				int layerMask = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Terrain");
				layerMask = -1 - (1 << LayerMask.NameToLayer("Player"));
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, 3f, layerMask))
				{
					this.m_targetPos = raycastHit.point + raycastHit.normal * 0.1f;
				}
				else
				{
					origin = this.m_targetPos + a * 5f;
					if (Physics.Raycast(ray, out raycastHit, 5f, layerMask))
					{
						this.m_targetPos = raycastHit.point + raycastHit.normal * 0.1f;
					}
					else
					{
						this.m_targetPos = stagePathEvaluator.GetWorldPosition() + a * 2f;
					}
				}
			}
			else
			{
				this.m_targetPos = context.Position;
			}
			CapsuleCollider component = context.GetComponent<CapsuleCollider>();
			if (component != null)
			{
				int layerMask2 = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Terrain");
				Vector3 vector = -context.Movement.GetGravityDir();
				if (StateUtil.CheckOverlapSphere(this.m_targetPos, vector, component.height * 0.5f + 0.2f, layerMask2))
				{
					float num = 2f;
					StateUtil.CapsuleCast(component, this.m_targetPos + vector * num, vector, layerMask2, -vector, num - 0.1f, ref this.m_targetPos, true);
				}
			}
			this.m_substate = StatePhantomDrill.SubState.RETURN;
		}

		private void StartPathMove(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.RunOnPathPhantomDrill);
			CharacterMoveOnPathPhantomDrill currentState = context.Movement.GetCurrentState<CharacterMoveOnPathPhantomDrill>();
			if (currentState != null)
			{
				currentState.SetupPath(context.Movement, BlockPathController.PathType.DRILL, false, 0f);
				currentState.SetSpeed(context.Movement, context.Parameter.m_drillSpeed);
			}
		}

		public override bool DispatchMessage(CharacterState context, int messageId, MessageBase msg)
		{
			if (messageId != 12289)
			{
				return false;
			}
			MsgInvalidateItem msgInvalidateItem = msg as MsgInvalidateItem;
			if (msgInvalidateItem != null && msgInvalidateItem.m_itemType == ItemType.DRILL)
			{
				if (this.m_substate == StatePhantomDrill.SubState.RUN)
				{
					this.GotoReturn(context);
				}
				else if (this.m_substate == StatePhantomDrill.SubState.GODOWN)
				{
					StateUtil.ReturnFromPhantomAndChangeState(context, PhantomType.DRILL, false);
				}
				return true;
			}
			return true;
		}
	}
}
