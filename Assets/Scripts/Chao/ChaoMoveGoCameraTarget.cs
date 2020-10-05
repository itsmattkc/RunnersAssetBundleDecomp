using System;
using UnityEngine;

namespace Chao
{
	public class ChaoMoveGoCameraTarget : ChaoMoveBase
	{
		private Vector3 m_screenOffsetRate = Vector3.zero;

		private Vector3 m_targetScreenPos = Vector3.zero;

		private Vector3 m_currentScreenPos = Vector3.zero;

		private GameObject m_cameraObject;

		private float m_speedRate;

		private float m_distance;

		private float m_speed;

		private float m_posZ;

		public override void Enter(ChaoMovement context)
		{
			this.m_posZ = context.Position.z;
			this.m_cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
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
				}
			}
		}

		public override void Leave(ChaoMovement context)
		{
			this.m_cameraObject = null;
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
			this.UpdateTargetPosition(context);
		}

		public void SetParameter(Vector3 screenOffsetRate, float speedRate)
		{
			this.m_screenOffsetRate = screenOffsetRate;
			this.m_speedRate = speedRate;
			if (this.m_cameraObject != null)
			{
				Camera component = this.m_cameraObject.GetComponent<Camera>();
				if (component != null)
				{
					this.m_targetScreenPos.x = component.pixelWidth * this.m_screenOffsetRate.x;
					this.m_targetScreenPos.y = component.pixelHeight * this.m_screenOffsetRate.y;
					this.m_targetScreenPos.z = this.m_currentScreenPos.z;
				}
			}
			this.m_distance = Vector3.Distance(this.m_targetScreenPos, this.m_currentScreenPos);
		}

		private void UpdateTargetPosition(ChaoMovement context)
		{
			this.m_currentScreenPos = Vector3.MoveTowards(this.m_currentScreenPos, this.m_targetScreenPos, this.m_distance * this.m_speedRate * Time.deltaTime);
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
