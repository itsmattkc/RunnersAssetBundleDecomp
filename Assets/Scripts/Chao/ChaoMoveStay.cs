using System;
using UnityEngine;

namespace Chao
{
	public class ChaoMoveStay : ChaoMoveBase
	{
		private Vector3 m_stayPosition;

		public override void Enter(ChaoMovement context)
		{
			this.m_stayPosition = context.Position - context.Hovering;
		}

		public override void Leave(ChaoMovement context)
		{
		}

		public override void Step(ChaoMovement context, float deltaTime)
		{
			context.Position = this.m_stayPosition + context.Hovering;
		}
	}
}
