using System;
using UnityEngine;

namespace Player
{
	public class CharacterMoveRun : CharacterMoveBase
	{
		private float m_errorTime;

		private float m_errorTimeMax;

		private Vector3 m_prevPos = Vector3.zero;

		private HitInfo m_sweepHitInfo;

		public override void Enter(CharacterMovement context)
		{
			this.m_sweepHitInfo = default(HitInfo);
			this.m_errorTime = 0f;
			this.m_prevPos = Vector3.zero;
			this.m_errorTimeMax = context.Parameter.m_hitWallDeadTime * 2f;
		}

		public override void Leave(CharacterMovement context)
		{
			this.m_sweepHitInfo.Reset();
		}

		public override void Step(CharacterMovement context, float deltaTime)
		{
			float num = context.Velocity.magnitude * deltaTime;
			if (num <= 0.0001f)
			{
				context.SetRaycastCheckPosition(context.RaycastCheckPosition);
				return;
			}
			MovementUtil.SweepMoveForRunAndAir(context, deltaTime, ref this.m_sweepHitInfo);
			Vector3 raycastCheckPosition = context.RaycastCheckPosition;
			MovementUtil.CheckAndPushOutByRaycast(context.transform, context.RaycastCheckPosition, ref raycastCheckPosition);
			context.SetRaycastCheckPosition(raycastCheckPosition);
			if (this.m_prevPos == raycastCheckPosition)
			{
				this.m_errorTime += deltaTime;
				if (this.m_errorTime > this.m_errorTimeMax)
				{
					CapsuleCollider component = context.GetComponent<CapsuleCollider>();
					if (component != null)
					{
						int layer = -1 - (1 << LayerMask.NameToLayer("Player"));
						MovementUtil.SweepMoveInnerParam innerParam = new MovementUtil.SweepMoveInnerParam(component, new Vector3(-0.2f, 0.1f, 0f), layer);
						MovementUtil.SweepMoveOuterParam outerParam = new MovementUtil.SweepMoveOuterParam();
						MovementUtil.SweepMove(context.transform, innerParam, outerParam);
					}
				}
			}
			else
			{
				this.m_errorTime = 0f;
			}
			this.m_prevPos = raycastCheckPosition;
			context.SetSweepHitInfo(this.m_sweepHitInfo);
			if (Vector3.Dot(context.transform.forward, CharacterDefs.BaseFrontTangent) < -0.866f)
			{
				global::Debug.Log("Warning:CharacterRotate is Reversed.");
				context.SetLookRotation(context.transform.forward, context.transform.up);
			}
		}
	}
}
