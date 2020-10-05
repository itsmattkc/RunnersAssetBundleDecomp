using System;
using UnityEngine;

namespace App
{
	public struct Segment3
	{
		private Vector3 p0;

		private Vector3 p1;

		public Segment3(Vector3 pt0, Vector3 pt1)
		{
			this.p0 = pt0;
			this.p1 = pt1;
		}

		public void Set(Vector3 pt0, Vector3 pt1)
		{
			this.p0 = pt0;
			this.p1 = pt1;
		}

		public Vector3 GetP0()
		{
			return this.p0;
		}

		public Vector3 GetP1()
		{
			return this.p1;
		}

		public static float DistanceSq(ref Vector3 pt0, ref Vector3 pt1, ref Vector3 pt, ref float? t)
		{
			Line3 line = Line3.FromPoints(pt0, pt1);
			Vector3 a = pt0;
			Vector3 b = pt1;
			float magnitude = (a - b).magnitude;
			float? num = new float?(0f);
			float result = line.DistanceSq(pt, ref num);
			if (num.HasValue && num.Value < 0f)
			{
				if (t.HasValue)
				{
					t = new float?(0f);
				}
				result = Vector3.SqrMagnitude(pt - pt0);
			}
			else if (num.HasValue && num.Value > magnitude)
			{
				if (t.HasValue)
				{
					t = new float?(1f);
				}
				result = Vector3.SqrMagnitude(pt - pt1);
			}
			else if (t.HasValue)
			{
				t = new float?(((!num.HasValue) ? null : new float?(num.Value / magnitude)).Value);
			}
			return result;
		}

		public float DistanceSq(Vector3 pt, ref float? t)
		{
			Line3 line = Line3.FromPoints(this.p0, this.p1);
			Vector3 a = this.p0;
			Vector3 b = this.p1;
			float magnitude = (a - b).magnitude;
			float? num = new float?(0f);
			float result = line.DistanceSq(pt, ref num);
			if (num.HasValue && num.Value < 0f)
			{
				if (t.HasValue)
				{
					t = new float?(0f);
				}
				result = Vector3.SqrMagnitude(pt - this.p0);
			}
			else if (num.HasValue && num.Value > magnitude)
			{
				if (t.HasValue)
				{
					t = new float?(1f);
				}
				result = Vector3.SqrMagnitude(pt - this.p1);
			}
			else if (t.HasValue)
			{
				t = new float?(((!num.HasValue) ? null : new float?(num.Value / magnitude)).Value);
			}
			return result;
		}

		public float DistanceSq(Segment3 seg, ref float? s, ref float? t)
		{
			Vector3 b = this.p0;
			Vector3 a = this.p1;
			Vector3 vector = seg.p1 - seg.p0;
			Vector3 vector2 = a - b;
			Vector3 vector3 = seg.p0 - b;
			float sqrMagnitude = vector.sqrMagnitude;
			float num = Vector3.Dot(vector, vector2);
			float sqrMagnitude2 = vector2.sqrMagnitude;
			float num2 = Vector3.Dot(vector, vector3);
			float num3 = Vector3.Dot(vector2, vector3);
			float num4 = sqrMagnitude * sqrMagnitude2 - num * num;
			float num5 = num4;
			float num6 = num4;
			float num7;
			float num8;
			if (Math.NearZero(num4, 0.0001f))
			{
				num7 = 0f;
				num5 = 1f;
				num8 = num3;
				num6 = sqrMagnitude2;
			}
			else
			{
				num7 = num * num3 - sqrMagnitude2 * num2;
				num8 = sqrMagnitude * num3 - num * num2;
				if (num7 < 0f)
				{
					num7 = 0f;
					num8 = num3;
					num6 = sqrMagnitude2;
				}
				else if (num7 > num5)
				{
					num7 = num5;
					num8 = num3 + num;
					num6 = sqrMagnitude2;
				}
			}
			if (num8 < 0f)
			{
				num8 = 0f;
				if (-num2 < 0f)
				{
					num7 = 0f;
				}
				else if (-num2 > sqrMagnitude)
				{
					num7 = num5;
				}
				else
				{
					num7 = -num2;
					num5 = sqrMagnitude;
				}
			}
			else if (num8 > num6)
			{
				num8 = num6;
				if (-num2 + num < 0f)
				{
					num7 = 0f;
				}
				else if (-num2 + num > sqrMagnitude)
				{
					num7 = num5;
				}
				else
				{
					num7 = -num2 + num;
					num5 = sqrMagnitude;
				}
			}
			float num9 = (!Math.NearZero(num7, 0.0001f)) ? (num7 / num5) : 0f;
			float num10 = (!Math.NearZero(num8, 0.0001f)) ? (num8 / num6) : 0f;
			if (s.HasValue)
			{
				s = new float?(num9);
			}
			if (t.HasValue)
			{
				t = new float?(num10);
			}
			return Vector3.SqrMagnitude(vector3 + vector * num9 - vector2 * num10);
		}

		public override bool Equals(object o)
		{
			return true;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(Segment3 lhs, Segment3 rhs)
		{
			return lhs.p0 == rhs.p0 && lhs.p1 == rhs.p1;
		}

		public static bool operator !=(Segment3 lhs, Segment3 rhs)
		{
			return lhs.p0 != rhs.p0 || lhs.p1 == rhs.p1;
		}
	}
}
