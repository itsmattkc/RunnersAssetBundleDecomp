using App;
using System;
using UnityEngine;

namespace Player
{
	public class CharacterMoveOnPathPhantomDrill : CharacterMoveBase
	{
		private const float TargetSearchRate = 3f;

		private const float MaxRotate = 180f;

		private PathEvaluator m_path;

		private BlockPathController.PathType m_pathType;

		private bool m_noJump;

		private Vector3 m_offset;

		private float m_speed;

		private bool m_nowJump;

		private Vector3 m_basePosition;

		private Vector3 m_baseVelocity;

		private Vector3 m_jumpVelocity;

		public override void Enter(CharacterMovement context)
		{
			this.m_path = null;
			this.m_basePosition = context.transform.position;
			this.m_baseVelocity = context.Velocity;
			this.m_nowJump = false;
			this.m_speed = 0f;
			this.m_offset = Vector3.zero;
		}

		public override void Leave(CharacterMovement context)
		{
			this.m_path = null;
			this.EndJump();
		}

		public override void Step(CharacterMovement context, float deltaTime)
		{
			this.StepRunOnPath(context, deltaTime);
			this.StepJump(context, deltaTime);
			context.Velocity = this.m_baseVelocity + this.m_jumpVelocity;
			context.transform.position += context.Velocity * deltaTime;
			this.Rotate(context, deltaTime);
		}

		public void StepRunOnPath(CharacterMovement context, float deltaTime)
		{
			if (this.m_path == null)
			{
				return;
			}
			if (!this.m_path.IsValid())
			{
				return;
			}
			Vector3 basePosition = this.m_basePosition;
			float num = this.m_speed * deltaTime;
			float num2 = num * 3f;
			Vector3? vector = new Vector3?(Vector3.zero);
			if (!this.CheckAndChangeToNextPath(context, num))
			{
				this.m_basePosition += this.m_baseVelocity * deltaTime;
				return;
			}
			Vector3? vector2 = null;
			float distance = this.m_path.Distance;
			this.m_path.GetPNT(distance + num2, ref vector, ref vector2, ref vector2);
			Vector3 baseVelocity = this.m_baseVelocity;
			Vector3 vector3 = Vector3.SmoothDamp(basePosition - this.m_offset, vector.Value, ref baseVelocity, deltaTime * 3f, float.PositiveInfinity, deltaTime);
			float distance2;
			this.m_path.GetClosestPositionAlongSpline(vector3, distance - num, distance + num, out distance2);
			this.m_path.Distance = distance2;
			this.m_basePosition = vector3 + this.m_offset;
			this.m_baseVelocity = (this.m_basePosition - basePosition) / deltaTime;
		}

		public void StepJump(CharacterMovement context, float deltaTime)
		{
			if (!this.m_nowJump)
			{
				return;
			}
			Vector3 rhs = -context.GetGravityDir();
			if (Vector3.Dot(this.m_jumpVelocity, rhs) < 0f && Vector3.Dot(context.transform.position - this.m_basePosition, rhs) < 0f)
			{
				this.EndJump();
				return;
			}
			this.m_jumpVelocity += context.GetGravityDir() * context.Parameter.m_drillJumpGravity * deltaTime;
		}

		private bool CheckAndChangeToNextPath(CharacterMovement context, float runLength)
		{
			if (this.m_path.Distance > this.m_path.GetLength() - runLength)
			{
				PathEvaluator stagePathEvaluator = CharacterMoveOnPathPhantomDrill.GetStagePathEvaluator(context, this.m_pathType);
				if (stagePathEvaluator == null)
				{
					this.m_path = null;
					return false;
				}
				if (stagePathEvaluator.GetPathObject().GetID() == this.m_path.GetPathObject().GetID())
				{
					return false;
				}
				this.SetupPath(context, stagePathEvaluator);
			}
			return true;
		}

		public void Jump(CharacterMovement context)
		{
			if (this.m_nowJump || this.m_noJump)
			{
				return;
			}
			this.m_nowJump = true;
			this.m_jumpVelocity = -context.GetGravityDir() * context.Parameter.m_drillJumpForce;
		}

		private void EndJump()
		{
			this.m_nowJump = false;
			this.m_jumpVelocity = Vector3.zero;
		}

		private void Rotate(CharacterMovement context, float deltaTime)
		{
			Vector3 vector = context.GetForwardDir();
			Vector3 current = vector;
			Vector3 vector2 = context.GetUpDir();
			if (App.Math.Vector3NormalizeIfNotZero(context.Velocity, out vector))
			{
				vector = Vector3.RotateTowards(current, vector, 180f * deltaTime, 0f);
				vector2 = Vector3.Cross(vector, CharacterDefs.BaseRightTangent);
				if (Mathf.Abs(Vector3.Dot(vector, CharacterDefs.BaseRightTangent)) > 0.001f)
				{
					Vector3 vector3 = Vector3.Cross(vector2, CharacterDefs.BaseRightTangent);
					if (Vector3.Dot(vector3, vector) < 0f)
					{
						vector = -vector3;
					}
					else
					{
						vector = vector3;
					}
					vector2 = Vector3.Cross(vector, CharacterDefs.BaseRightTangent);
				}
				context.SetLookRotation(vector, vector2);
			}
		}

		public void SetupPath(CharacterMovement context, BlockPathController.PathType pathtype, bool noJump, float offset)
		{
			this.m_pathType = pathtype;
			this.m_noJump = noJump;
			this.m_offset = offset * Vector3.up;
			PathEvaluator stagePathEvaluator = CharacterMoveOnPathPhantomDrill.GetStagePathEvaluator(context, this.m_pathType);
			if (stagePathEvaluator != null)
			{
				this.SetupPath(context, stagePathEvaluator);
			}
			else
			{
				this.m_path = null;
			}
		}

		private void SetupPath(CharacterMovement context, PathEvaluator pathEvaluator)
		{
			this.m_path = new PathEvaluator(pathEvaluator);
			float closestPositionAlongSpline = pathEvaluator.GetClosestPositionAlongSpline(context.transform.position, 0f, this.m_path.GetLength());
			this.m_path.Distance = closestPositionAlongSpline;
		}

		public void SetSpeed(CharacterMovement context, float speed)
		{
			this.m_speed = speed;
			this.m_baseVelocity = context.GetForwardDir() * speed;
		}

		public bool IsPathEnd(float remainDist)
		{
			return this.m_path == null || !this.m_path.IsValid() || this.m_path.GetLength() - this.m_path.Distance < remainDist;
		}

		public bool IsPathValid()
		{
			return this.m_path.IsValid();
		}

		public static PathEvaluator GetStagePathEvaluator(CharacterMovement context, BlockPathController.PathType patytype)
		{
			StageBlockPathManager blockPathManager = context.GetBlockPathManager();
			if (blockPathManager != null)
			{
				PathEvaluator curentPathEvaluator = blockPathManager.GetCurentPathEvaluator(patytype);
				if (curentPathEvaluator != null)
				{
					return curentPathEvaluator;
				}
			}
			return null;
		}
	}
}
