using System;
using UnityEngine;

namespace Chao
{
	public class ChaoMoveExit : ChaoMoveBase
	{
		private Vector3 m_chao_pos = new Vector3(0f, 0f, 0f);

		private Vector3 m_init_pos = new Vector3(0f, 0f, 0f);

		private float m_move_distance;

		public override void Enter(ChaoMovement context)
		{
			this.m_init_pos = context.TargetPosition;
			this.m_chao_pos = this.m_init_pos + context.Hovering + context.OffsetPosition;
			this.m_move_distance = 0f;
		}

		public override void Leave(ChaoMovement context)
		{
		}

		public override void Step(ChaoMovement context, float deltaTime)
		{
			if (!context.NextState)
			{
				this.m_chao_pos = this.m_init_pos + context.Hovering + context.OffsetPosition;
				if (context.PlayerInfo != null)
				{
					float x = context.PlayerInfo.HorizonVelocity.x;
					float num = x - 2f * context.ComeInSpeed;
					this.m_move_distance += num * deltaTime;
					this.m_chao_pos.x = this.m_chao_pos.x + this.m_move_distance;
					if (this.IsOffscreen())
					{
						context.NextState = true;
					}
				}
				context.Position = this.m_chao_pos;
			}
		}

		private bool IsOffscreen()
		{
			return Camera.main.WorldToScreenPoint(this.m_chao_pos).x < 0f;
		}
	}
}
