using System;
using UnityEngine;

namespace Player
{
	public class StateStumble : FSMState<CharacterState>
	{
		private const float DamageTime = 2f;

		private const float EnableJump = 1.7f;

		private const float horzVelocityNowall = 5f;

		private float m_timer;

		private bool m_noWall;

		private bool m_onAir;

		public override void Enter(CharacterState context)
		{
			context.ChangeMovement(MOVESTATE_ID.Air);
			context.Movement.OffGround();
			this.m_timer = 2f;
			context.GetAnimator().CrossFade("Damaged", 0.1f);
			context.Movement.Velocity = context.Movement.GetUpDir() * context.Parameter.m_stumbleJumpForce + context.Movement.GetForwardDir() * context.DefaultSpeed;
			this.m_noWall = false;
			this.m_onAir = true;
			context.ClearAirAction();
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
				if (this.m_onAir)
				{
					context.ChangeMovement(MOVESTATE_ID.Run);
					this.m_onAir = false;
				}
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
				if (!this.m_onAir)
				{
					context.ChangeMovement(MOVESTATE_ID.Air);
					this.m_onAir = true;
				}
				if (!this.m_noWall)
				{
					Vector3 position = context.Position;
					Vector3 baseFrontTangent = CharacterDefs.BaseFrontTangent;
					int layerMask = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Terrain");
					Ray ray = new Ray(position, baseFrontTangent);
					RaycastHit raycastHit;
					if (!Physics.Raycast(ray, out raycastHit, 1.5f, layerMask))
					{
						this.m_noWall = true;
						context.Movement.VertVelocity = context.Movement.GetUpDir() * 5f;
					}
				}
				else
				{
					vector += context.Movement.GetGravity() * deltaTime;
					context.Movement.VertVelocity = vector;
				}
			}
			context.Movement.HorzVelocity = context.Movement.GetForwardDir() * context.DefaultSpeed;
			this.m_timer -= deltaTime;
			if (this.m_timer <= 1.7f && context.m_input.IsTouched())
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
			if (StateUtil.CheckDeadByHitWall(context, deltaTime))
			{
				context.ChangeState(STATE_ID.Dead);
				return;
			}
		}
	}
}
