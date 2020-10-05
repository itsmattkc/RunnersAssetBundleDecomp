using System;
using UnityEngine;

namespace App
{
	public class Math
	{
		public const float EPSILON = 1E-06f;

		public const float PI = 3.14159274f;

		public const float F_E = 2.71828175f;

		public const float F_LOG2E = 1.442695f;

		public const float F_LOG10E = 0.4342945f;

		public const float F_LN2 = 0.6931472f;

		public const float F_LN10 = 2.30258512f;

		public const float F_PI = 3.14159274f;

		public const float F_SQRTPI = 1.7724539f;

		public const float F_SQRT2 = 1.41421354f;

		public const float F_SQRT3 = 1.73205078f;

		public const float F_INVLN2 = 1.442695f;

		public const float F_MAX = 3.40282347E+38f;

		public const float F_MIN = 1.17549435E-38f;

		public const float F_EPSILON = 1E-06f;

		private const float VEC3_NORMALIZE_EPS = 0.0001f;

		public static float Reciprocal(float x)
		{
			return 1f / x;
		}

		public static bool NearZero(float a, float epsilon = 1E-06f)
		{
			return Mathf.Abs(a) <= epsilon;
		}

		public static bool NearEqual(float a, float b, float epsilon = 1E-06f)
		{
			return Math.NearZero(a - b, epsilon);
		}

		public static float Sqr(float a)
		{
			return a * a;
		}

		public static Matrix4x4 Matrix44OrthonormalDirection2(Vector3 zAxis, Vector3 yAxis)
		{
			Vector3 vector = Vector3.Cross(yAxis, zAxis);
			if (!Math.Vector3NormalizeIfNotZero(vector, out vector))
			{
				vector = Vector3.right;
			}
			Vector3 c = Vector3.Cross(zAxis, vector);
			return Math.Matrix44SetColumn3(vector, c, zAxis);
		}

		public static Matrix4x4 Matrix44SetColumn3(Vector3 c0, Vector3 c1, Vector3 c2)
		{
			Matrix4x4 result = default(Matrix4x4);
			result.SetColumn(0, c0);
			result.SetColumn(1, c1);
			result.SetColumn(2, c2);
			result.SetColumn(3, Vector3.zero);
			return result;
		}

		public static Matrix4x4 Matrix34InverseNonSingular(Matrix4x4 m)
		{
			Matrix4x4 matrix4x = default(Matrix4x4);
			return m.inverse;
		}

		public static bool Vector3NormalizeIfNotZero(Vector3 src, out Vector3 dst)
		{
			if (src.sqrMagnitude < 0.0001f)
			{
				dst = Vector3.zero;
				return false;
			}
			dst = src.normalized;
			return true;
		}

		public static void Vector3NormalizeZero(Vector3 src, out Vector3 dst)
		{
			if (src.sqrMagnitude < 0.0001f)
			{
				dst = Vector3.zero;
				return;
			}
			dst = src.normalized;
		}
	}
}
