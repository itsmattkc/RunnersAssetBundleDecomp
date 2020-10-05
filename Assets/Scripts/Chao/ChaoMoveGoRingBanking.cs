using System;
using UnityEngine;

namespace Chao
{
	public class ChaoMoveGoRingBanking : ChaoMoveBase
	{
		private const float ScreenOffsetWidth = 0.85f;

		private const float UpperOffsetFromHud = 1.5f;

		private const float SpeedRate = 0.6f;

		private GameObject m_cameraObject;

		private float m_posZ;

		private Vector3 m_targetScreenPos = Vector3.zero;

		private Vector3 m_currentScreenPos = Vector3.zero;

		private float m_distance;

		public override void Enter(ChaoMovement context)
		{
			this.m_cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
			context.Velocity = context.HorzVelocity;
			this.m_posZ = context.Position.z;
			if (this.m_cameraObject != null)
			{
				Camera component = this.m_cameraObject.GetComponent<Camera>();
				if (component != null)
				{
					Vector3 vector = component.WorldToScreenPoint(context.Position);
					if (vector.x < 0f)
					{
						vector.x = -0.5f;
						context.Position = component.ScreenToWorldPoint(vector);
					}
					this.m_currentScreenPos = vector;
					vector.y = component.pixelHeight;
					vector.x = component.pixelWidth * 0.85f;
					Vector3 vector2 = component.ScreenToWorldPoint(vector);
					vector2 += ChaoMovement.VertDir * 1.5f;
					this.m_targetScreenPos = component.WorldToScreenPoint(vector2);
					this.m_targetScreenPos.z = this.m_currentScreenPos.z;
					this.m_distance = Vector3.Distance(this.m_targetScreenPos, this.m_currentScreenPos);
				}
			}
		}

		public override void Leave(ChaoMovement context)
		{
		}

		public override void Step(ChaoMovement context, float deltaTime)
		{
			if (context.PlayerInfo == null)
			{
				return;
			}
			if (!context.IsPlyayerMoved)
			{
				return;
			}
			this.m_currentScreenPos = Vector3.MoveTowards(this.m_currentScreenPos, this.m_targetScreenPos, this.m_distance * 0.6f * Time.deltaTime);
			if (this.m_cameraObject != null)
			{
				Camera component = this.m_cameraObject.GetComponent<Camera>();
				if (component != null)
				{
					Vector3 position = component.ScreenToWorldPoint(this.m_currentScreenPos);
					position.z = this.m_posZ;
					context.Position = position;
				}
			}
		}
	}
}
