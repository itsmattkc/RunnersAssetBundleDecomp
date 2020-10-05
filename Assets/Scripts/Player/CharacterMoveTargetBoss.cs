using Message;
using System;
using UnityEngine;

namespace Player
{
	public class CharacterMoveTargetBoss : CharacterMoveBase
	{
		private GameObject m_bossObject;

		private float m_speed;

		private bool m_rotateVelocityDir;

		private bool m_targetNotFound;

		private bool m_onlyHorizon;

		private bool m_reachTarget;

		public override void Enter(CharacterMovement context)
		{
			this.m_bossObject = null;
			this.m_rotateVelocityDir = false;
			this.m_targetNotFound = false;
			this.m_onlyHorizon = false;
			this.m_reachTarget = false;
		}

		public override void Leave(CharacterMovement context)
		{
			this.m_bossObject = null;
		}

		public override void Step(CharacterMovement context, float deltaTime)
		{
			bool flag = false;
			Vector3 vector = context.transform.position;
			if (this.m_bossObject != null)
			{
				MsgBossInfo msgBossInfo = new MsgBossInfo();
				this.m_bossObject.SendMessage("OnMsgBossInfo", msgBossInfo);
				if (msgBossInfo.m_succeed)
				{
					vector = this.m_bossObject.transform.position;
					flag = true;
				}
			}
			this.m_targetNotFound = !flag;
			if (this.m_onlyHorizon)
			{
				Vector3 lhs = vector - context.transform.position;
				Vector3 vector2 = -context.GetGravityDir();
				Vector3 b = Vector3.Dot(lhs, vector2) * vector2;
				vector -= b;
			}
			float sqrMagnitude = (context.transform.position - vector).sqrMagnitude;
			float num = this.m_speed * deltaTime;
			if (sqrMagnitude < num * num)
			{
				context.transform.position = vector;
				this.m_reachTarget = true;
			}
			else
			{
				Vector3 normalized = (vector - context.transform.position).normalized;
				context.transform.position += normalized * this.m_speed * deltaTime;
				if (this.m_rotateVelocityDir)
				{
					Vector3 front = normalized;
					Vector3 up = Vector3.Cross(normalized, CharacterDefs.BaseRightTangent);
					context.SetLookRotation(front, up);
				}
			}
		}

		public void SetTarget(GameObject targetObject)
		{
			this.m_bossObject = targetObject;
		}

		public void SetSpeed(float speed)
		{
			this.m_speed = speed;
		}

		public bool IsTargetNotFound()
		{
			return this.m_targetNotFound;
		}

		public void SetRotateVelocityDir(bool value)
		{
			this.m_rotateVelocityDir = value;
		}

		public void SetOnlyHorizon(bool value)
		{
			this.m_onlyHorizon = value;
		}

		public bool DoesReachTarget()
		{
			return this.m_reachTarget;
		}
	}
}
