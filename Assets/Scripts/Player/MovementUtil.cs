using System;
using UnityEngine;

namespace Player
{
	public class MovementUtil
	{
		public class SweepMoveInnerParam
		{
			public readonly CapsuleCollider collider;

			public Vector3 move;

			public int layerMask;

			public SweepMoveInnerParam(CapsuleCollider colli, Vector3 mov, int layer)
			{
				this.collider = colli;
				this.move = mov;
				this.layerMask = layer;
			}
		}

		public class SweepMoveOuterParam
		{
			public Vector3 outMove;

			public RaycastHit hitInfo;

			public SweepMoveOuterParam()
			{
				this.hitInfo = default(RaycastHit);
			}

			public void Reset()
			{
				this.outMove = Vector3.zero;
				this.hitInfo = default(RaycastHit);
			}
		}

		public static bool SweepMove(Transform transform, MovementUtil.SweepMoveInnerParam innerParam, MovementUtil.SweepMoveOuterParam outerParam)
		{
			CapsuleCollider collider = innerParam.collider;
			Vector3 position = transform.position;
			float num = 0.01f;
			float radius = collider.radius;
			float d = collider.height * 0.5f - radius;
			Vector3 a = position + transform.TransformDirection(collider.center);
			Vector3 up = transform.up;
			Vector3 point = a - up * d;
			Vector3 point2 = a + up * d;
			Vector3 vector = Vector3.zero;
			Vector3 move = innerParam.move;
			float magnitude = move.magnitude;
			if (magnitude < 0.0001f)
			{
				return false;
			}
			Vector3 normalized = move.normalized;
			if (Physics.CapsuleCast(point, point2, radius, normalized, out outerParam.hitInfo, magnitude, innerParam.layerMask))
			{
				float distance = outerParam.hitInfo.distance;
				vector = normalized * (distance - num);
				transform.position += vector;
				outerParam.outMove = vector;
				return true;
			}
			transform.position += move;
			outerParam.outMove = move;
			return false;
		}

		public static bool RotateByCollision(Transform transform, CapsuleCollider collider, Vector3 up)
		{
			Vector3 up2 = transform.up;
			float radius = collider.radius;
			int layermask = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Terrain");
			int num = 4;
			float num2 = 1f;
			float d = collider.height * 0.5f - collider.radius;
			for (int i = 0; i < num; i++)
			{
				Vector3 vector = Vector3.Lerp(up2, up, num2);
				Vector3 a = transform.position + vector * collider.height * 0.5f;
				Vector3 start = a - vector * d;
				Vector3 end = a + vector * d;
				if (!Physics.CheckCapsule(start, end, radius, layermask))
				{
					transform.rotation = Quaternion.FromToRotation(up2, vector) * transform.rotation;
					if (Vector3.Dot(transform.forward, CharacterDefs.BaseFrontTangent) < 0f)
					{
						global::Debug.Log("Warning:CharacterRotate is Reversed.");
						Quaternion identity = Quaternion.identity;
						identity.SetLookRotation(-transform.forward, transform.up);
						transform.rotation = identity;
					}
					return true;
				}
				num2 -= 1f / (float)num;
			}
			return false;
		}

		public static bool CheckOverlapTerarinAndMoveOutCollision(CharacterMovement context, Transform transform, CapsuleCollider collider)
		{
			float num = 0.01f;
			float num2 = collider.radius - num;
			float num3 = collider.height * 0.5f - collider.radius;
			Vector3 up = transform.up;
			Vector3 vector = transform.position + up * collider.height * 0.5f;
			Vector3 start = vector - up * num3;
			Vector3 end = vector + up * num3;
			int layermask = 1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Default");
			if (!Physics.CheckCapsule(start, end, num2, layermask))
			{
				return false;
			}
			bool flag = false;
			float num4 = num2 + num3;
			Collider[] array = Physics.OverlapSphere(vector, num4);
			Vector3 vector2 = Vector3.zero;
			Collider[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Collider collider2 = array2[i];
				if (!collider2.isTrigger)
				{
					Vector3 vector3 = collider2.bounds.center - vector;
					Vector3 normalized = vector3.normalized;
					Vector3 origin = vector + -normalized * num4;
					float distance = vector3.magnitude + num4;
					Ray ray = new Ray(origin, normalized);
					RaycastHit raycastHit = default(RaycastHit);
					if (collider2.Raycast(ray, out raycastHit, distance))
					{
						float d = num4 * 2f - (raycastHit.distance - 0.01f);
						vector2 += -normalized * d;
						flag = true;
					}
				}
			}
			if (flag)
			{
				Vector3 vector4 = transform.position + vector2;
				transform.position = vector4;
				context.SetRaycastCheckPosition(vector4);
				return true;
			}
			return false;
		}

		public static bool CheckAndPushOutByRaycast(Transform transform, Vector3 prevRayPosition, ref Vector3 newRayPosition)
		{
			Vector3 vector = transform.position + transform.up * 0.1f;
			Vector3 vector2 = vector - prevRayPosition;
			int layerMask = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Terrain");
			Ray ray = new Ray(prevRayPosition, vector2.normalized);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, vector2.magnitude, layerMask))
			{
				transform.position = prevRayPosition;
				newRayPosition = prevRayPosition;
				return true;
			}
			newRayPosition = vector;
			return false;
		}

		public static void UpdateRotateOnGravityUp(CharacterMovement context, float degSpeed, float deltaTime)
		{
			Vector3 upDir = context.GetUpDir();
			Vector3 target = -context.GetGravityDir();
			Vector3 up = Vector3.RotateTowards(upDir, target, degSpeed * deltaTime * 0.0174532924f, 0f);
			MovementUtil.RotateByCollision(context.transform, context.GetComponent<CapsuleCollider>(), up);
		}

		public static void SweepMoveForRunAndAir(CharacterMovement context, float deltaTime, ref HitInfo sweepInfo)
		{
			sweepInfo.Reset();
			Transform transform = context.transform;
			int layer = -1 - (1 << LayerMask.NameToLayer("Player"));
			if (context.ThroughBreakable)
			{
				layer = -1 - (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Broken"));
			}
			Vector3 vector = context.VertVelocity;
			Vector3 a = context.Velocity - vector;
			Vector3 vector2 = a * deltaTime;
			Vector3 b = new Vector3(0f, 0f, -transform.position.z);
			vector2 += b;
			CapsuleCollider component = context.GetComponent<CapsuleCollider>();
			MovementUtil.SweepMoveInnerParam sweepMoveInnerParam = new MovementUtil.SweepMoveInnerParam(component, vector2, layer);
			MovementUtil.SweepMoveOuterParam sweepMoveOuterParam = new MovementUtil.SweepMoveOuterParam();
			if (MovementUtil.SweepMove(transform, sweepMoveInnerParam, sweepMoveOuterParam))
			{
				Vector3 vector3 = sweepMoveOuterParam.outMove;
				a = vector3 / deltaTime;
				if (context.IsOnGround() && a.sqrMagnitude > 0.0001f)
				{
					float num = Vector3.Dot(sweepMoveOuterParam.hitInfo.normal, context.transform.up);
					if (num > context.EnableLandCos)
					{
						MovementUtil.RotateByCollision(transform, component, sweepMoveOuterParam.hitInfo.normal);
						vector2 -= vector3;
						vector2 -= Vector3.Project(vector2, sweepMoveOuterParam.hitInfo.normal);
						sweepMoveInnerParam.move = vector2;
						sweepMoveOuterParam.Reset();
						MovementUtil.SweepMove(transform, sweepMoveInnerParam, sweepMoveOuterParam);
						Vector3 outMove = sweepMoveOuterParam.outMove;
						a = outMove / deltaTime;
						vector3 += outMove;
					}
				}
			}
			else
			{
				Vector3 vector3 = sweepMoveOuterParam.outMove;
			}
			sweepMoveInnerParam.move = vector * deltaTime;
			sweepMoveOuterParam.Reset();
			if (MovementUtil.SweepMove(transform, sweepMoveInnerParam, sweepMoveOuterParam))
			{
				vector = sweepMoveOuterParam.outMove / deltaTime;
				sweepInfo.Set(sweepMoveOuterParam.hitInfo);
			}
			Vector3 vector4 = transform.forward;
			Vector3 up = transform.up;
			Quaternion identity = Quaternion.identity;
			if (Mathf.Abs(Vector3.Dot(vector4, CharacterDefs.BaseRightTangent)) > 0.001f)
			{
				Vector3 vector5 = Vector3.Cross(up, CharacterDefs.BaseRightTangent);
				if (Vector3.Dot(vector5, up) < 0f)
				{
					vector4 = -vector5;
				}
				else
				{
					vector4 = vector5;
				}
			}
			identity.SetLookRotation(vector4, up);
			context.transform.rotation = identity;
			context.Velocity = a + vector;
		}
	}
}
