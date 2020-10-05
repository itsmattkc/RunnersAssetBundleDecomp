using System;
using UnityEngine;

namespace Chao
{
	public class ChaoMoveAttackBoss : ChaoMoveBase
	{
		public enum Mode
		{
			Up,
			Homing,
			AfterAttack
		}

		private const float UpVelocity = 4.5f;

		private const float AfterAttackVelocity = 7f;

		private const float SpeedRate = 4f;

		private ChaoMoveAttackBoss.Mode m_mode;

		private GameObject m_boss;

		private Vector3 m_prevPlrPos;

		public override void Enter(ChaoMovement context)
		{
			this.m_mode = ChaoMoveAttackBoss.Mode.Up;
			this.m_boss = null;
			context.Velocity = Vector3.zero;
			if (context.PlayerInfo != null)
			{
				this.m_prevPlrPos = context.PlayerInfo.Position;
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
			if (this.m_boss == null)
			{
				context.Position += context.Velocity * deltaTime;
				this.m_prevPlrPos = context.PlayerInfo.Position;
				return;
			}
			Vector3 position = this.m_boss.transform.position;
			switch (this.m_mode)
			{
			case ChaoMoveAttackBoss.Mode.Up:
				this.MoveUp(context, deltaTime);
				break;
			case ChaoMoveAttackBoss.Mode.Homing:
			{
				float speed = context.PlayerInfo.DefaultSpeed * 4f;
				this.MoveHoming(context, position, speed, deltaTime);
				break;
			}
			case ChaoMoveAttackBoss.Mode.AfterAttack:
				this.MoveAfterAttack(context, deltaTime);
				break;
			}
			this.m_prevPlrPos = context.PlayerInfo.Position;
		}

		public void SetTarget(GameObject boss)
		{
			this.m_boss = boss;
		}

		public void ChangeMode(ChaoMoveAttackBoss.Mode mode)
		{
			this.m_mode = mode;
		}

		private void MoveHoming(ChaoMovement context, Vector3 targetPosition, float speed, float deltaTime)
		{
			Vector3 vector = targetPosition - context.Position;
			float magnitude = vector.magnitude;
			Vector3 normalized = vector.normalized;
			if (magnitude < speed * deltaTime)
			{
				context.Position = targetPosition;
			}
			else
			{
				context.Velocity = normalized * speed;
				context.Position += context.Velocity * deltaTime;
			}
		}

		private void MoveUp(ChaoMovement context, float deltaTime)
		{
			if (context.PlayerInfo == null)
			{
				return;
			}
			if (Vector3.Distance(this.m_prevPlrPos, context.PlayerInfo.Position) < 1.401298E-45f)
			{
				return;
			}
			context.Velocity = ChaoMovement.HorzDir * context.PlayerInfo.DefaultSpeed + ChaoMovement.VertDir * 4.5f;
			context.Position += context.Velocity * deltaTime;
		}

		private void MoveAfterAttack(ChaoMovement context, float deltaTime)
		{
			if (context.PlayerInfo == null)
			{
				return;
			}
			Vector3 position = context.PlayerInfo.Position;
			if (Vector3.Distance(this.m_prevPlrPos, position) < 1.401298E-45f)
			{
				return;
			}
			Vector3 lhs = position - context.Position;
			float num = Vector3.Dot(lhs, ChaoMovement.HorzDir);
			Vector3 a;
			if (num > 0f)
			{
				a = ChaoMovement.HorzDir * context.PlayerInfo.DefaultSpeed;
			}
			else
			{
				a = ChaoMovement.HorzDir * context.PlayerInfo.DefaultSpeed * 0.25f;
			}
			context.Velocity = a + ChaoMovement.VertDir * 7f;
			context.Position += context.Velocity * deltaTime;
		}
	}
}
