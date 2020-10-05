using System;
using UnityEngine;

namespace App
{
	internal struct Line3
	{
		private Vector3 p;

		private Vector3 d;

		public Line3(Vector3 p_, Vector3 d_)
		{
			this.p = p_;
			this.d = d_;
		}

		public void Set(Vector3 p_, Vector3 d_)
		{
			this.p = p_;
			this.d = d_;
		}

		public Vector3 GetP()
		{
			return this.p;
		}

		public Vector3 GetD()
		{
			return this.d;
		}

		public void Normalize()
		{
			this.d.Normalize();
		}

		public float DistanceSq(Vector3 pt, ref float? t)
		{
			Vector3 vector = this.p;
			float num = Vector3.Dot(this.d, pt - vector);
			Vector3 b = vector + this.d * num;
			if (t.HasValue)
			{
				t = new float?(num);
			}
			return (pt - b).sqrMagnitude;
		}

		public float DistanceSq(Line3 l, ref float? s, ref float? t)
		{
			Vector3 b = this.p;
			Vector3 vector = this.d;
			Vector3 vector2 = l.p - b;
			float num = Vector3.Dot(l.d, vector);
			float num2 = Vector3.Dot(l.d, vector2);
			float num3 = Vector3.Dot(vector, vector2);
			float num4 = 1f - num * num;
			if (Math.NearZero(num4, 0.0001f))
			{
				if (s.HasValue)
				{
					s = new float?(0f);
				}
				if (t.HasValue)
				{
					t = new float?(num3);
				}
				return vector2.sqrMagnitude;
			}
			float num5 = 1f / num4;
			float num6 = (num * num3 - num2) * num5;
			float num7 = (num3 - num * num2) * num5;
			if (s.HasValue)
			{
				s = new float?(num6);
			}
			if (t.HasValue)
			{
				t = new float?(num7);
			}
			return (l.d * num6 - vector * num7 + vector2).sqrMagnitude;
		}

		public static Line3 FromPoints(Vector3 p1, Vector3 p2)
		{
			return new Line3(p1, (p2 - p1).normalized);
		}

		public static Line3 FromSegment(ref Segment3 s)
		{
			return new Line3(s.GetP0(), (s.GetP1() - s.GetP0()).normalized);
		}

		public override bool Equals(object o)
		{
			return true;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(Line3 lhs, Line3 rhs)
		{
			return lhs.p == rhs.p && lhs.d == rhs.d;
		}

		public static bool operator !=(Line3 lhs, Line3 rhs)
		{
			return lhs.p != rhs.p || lhs.d != rhs.d;
		}
	}
}
