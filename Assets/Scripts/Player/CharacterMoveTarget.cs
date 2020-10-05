using System;
using UnityEngine;

namespace Player
{
	public class CharacterMoveTarget : CharacterMoveBase
	{
		private Vector3 m_targetPosition;

		private float m_speed;

		private bool m_rotateVelocityDir;

		private bool m_reachTarget;

		public override void Enter(CharacterMovement context)
		{
			this.m_rotateVelocityDir = false;
			this.m_reachTarget = false;
		}

		public override void Leave(CharacterMovement context)
		{
		}

		public override void Step(CharacterMovement context, float deltaTime)
		{
			float sqrMagnitude = (context.transform.position - this.m_targetPosition).sqrMagnitude;
			float num = this.m_speed * deltaTime;
			if (sqrMagnitude < num * num)
			{
				context.transform.position = this.m_targetPosition;
				this.m_reachTarget = true;
			}
			else
			{
				Vector3 normalized = (this.m_targetPosition - context.transform.position).normalized;
				context.transform.position += normalized * this.m_speed * deltaTime;
				if (this.m_rotateVelocityDir)
				{
					Vector3 front = normalized;
					Vector3 up = Vector3.Cross(normalized, CharacterDefs.BaseRightTangent);
					context.SetLookRotation(front, up);
				}
			}
		}

		public void SetTarget(CharacterMovement context, Vector3 position, Quaternion rotation, float time)
		{
			this.m_targetPosition = position;
			this.m_speed = Vector3.Distance(this.m_targetPosition, context.transform.position) / time;
		}

		public void SetTargetAndSpeed(CharacterMovement context, Vector3 position, Quaternion rotation, float speed)
		{
			this.m_targetPosition = position;
			this.m_speed = speed;
		}

		public void SetRotateVelocityDir(bool value)
		{
			this.m_rotateVelocityDir = value;
		}

		public bool DoesReachTarget()
		{
			return this.m_reachTarget;
		}
	}
}
