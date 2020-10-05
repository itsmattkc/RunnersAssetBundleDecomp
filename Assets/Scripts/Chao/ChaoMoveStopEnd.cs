using System;
using UnityEngine;

namespace Chao
{
	public class ChaoMoveStopEnd : ChaoMoveBase
	{
		private Vector3 m_chao_pos = new Vector3(0f, 0f, 0f);

		private float m_distance;

		public override void Enter(ChaoMovement context)
		{
			this.m_chao_pos = context.TargetPosition + context.Hovering + context.OffsetPosition;
			Camera main = Camera.main;
			if (main != null)
			{
				Vector3 position = main.WorldToScreenPoint(context.Position);
				position.x = 0f;
				Vector3 position2 = main.ScreenToWorldPoint(position);
				if (position2.x > context.Position.x)
				{
					context.Position = position2;
					this.m_distance = this.m_chao_pos.x - position2.x;
				}
				else
				{
					this.m_distance = this.m_chao_pos.x - context.Position.x;
				}
			}
			else
			{
				this.m_distance = 10f;
				Vector3 chao_pos = this.m_chao_pos;
				chao_pos.x -= this.m_distance;
				context.Position = chao_pos;
			}
		}

		public override void Leave(ChaoMovement context)
		{
		}

		public override void Step(ChaoMovement context, float deltaTime)
		{
			this.m_chao_pos = context.TargetPosition + context.Hovering + context.OffsetPosition;
			this.m_distance -= context.ComeInSpeed * deltaTime;
			if (this.m_distance < 0f)
			{
				this.m_distance = 0f;
				context.NextState = true;
			}
			this.m_chao_pos.x = this.m_chao_pos.x - this.m_distance;
			context.Position = this.m_chao_pos;
		}
	}
}
