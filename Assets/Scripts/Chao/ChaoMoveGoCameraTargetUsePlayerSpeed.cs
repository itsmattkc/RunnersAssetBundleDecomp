using App;
using System;
using UnityEngine;

namespace Chao
{
	public class ChaoMoveGoCameraTargetUsePlayerSpeed : ChaoMoveBase
	{
		private Vector3 m_screenOffsetRate;

		private Vector3 m_targetPos;

		private GameObject m_cameraObject;

		private float m_speedRate;

		public override void Enter(ChaoMovement context)
		{
			this.m_cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
			if (this.m_cameraObject != null)
			{
				Camera component = this.m_cameraObject.GetComponent<Camera>();
				if (component != null)
				{
					Vector3 position = component.WorldToScreenPoint(context.Position);
					if (position.x < 0f)
					{
						position.x = -0.5f;
						context.Position = component.ScreenToWorldPoint(position);
					}
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
			Vector3 vector = this.m_targetPos - context.Position;
			Vector3 normalized = vector.normalized;
			float num = this.m_speedRate * context.PlayerInfo.FrontSpeed;
			context.Velocity = normalized * num;
			Vector3 vector2 = context.Position;
			if (vector.sqrMagnitude < App.Math.Sqr(num * deltaTime))
			{
				vector2 = this.m_targetPos;
				context.NextState = true;
			}
			else
			{
				vector2 += context.Velocity * deltaTime;
			}
			context.Position = vector2;
		}

		public void SetParameter(Vector3 screenOffsetRate, float speedRate)
		{
			this.m_screenOffsetRate = screenOffsetRate;
			this.m_speedRate = speedRate;
		}

		private bool UpdateTargetPosition(ChaoMovement context)
		{
			if (this.m_cameraObject == null)
			{
				return false;
			}
			Camera component = this.m_cameraObject.GetComponent<Camera>();
			if (component == null)
			{
				return false;
			}
			Vector3 position = component.WorldToScreenPoint(context.Position);
			position.x = component.pixelWidth * this.m_screenOffsetRate.x;
			position.y = component.pixelHeight * this.m_screenOffsetRate.y;
			this.m_targetPos = component.ScreenToWorldPoint(position);
			return true;
		}
	}
}
