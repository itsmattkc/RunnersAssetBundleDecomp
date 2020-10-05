using App;
using System;
using UnityEngine;

public class PathEvaluator
{
	private PathComponent m_path;

	private float m_dist;

	private int m_cache;

	public float Distance
	{
		get
		{
			this.AssertPathNotValid();
			return this.m_dist;
		}
		set
		{
			this.AssertPathNotValid();
			this.m_dist = value;
		}
	}

	public PathEvaluator()
	{
	}

	public PathEvaluator(PathComponent component)
	{
		this.SetPathObject(component);
	}

	public PathEvaluator(PathEvaluator src)
	{
		this.SetPathObject(src.GetPathObject());
		this.Distance = src.Distance;
	}

	public void SetPathObject(PathComponent path)
	{
		this.m_path = path;
		this.m_dist = 0f;
		this.m_cache = 0;
	}

	public PathComponent GetPathObject()
	{
		return this.m_path;
	}

	public bool IsValid()
	{
		return this.m_path.IsValid();
	}

	private void Reset()
	{
		this.m_path = null;
	}

	public float GetLength()
	{
		this.AssertPathNotValid();
		ResPathObject resPath = PathEvaluator.GetResPath(this);
		if (resPath != null)
		{
			return resPath.Length;
		}
		return 0f;
	}

	public Vector3 GetWorldPosition(float dist)
	{
		Vector3? vector = new Vector3?(default(Vector3));
		Vector3? vector2 = null;
		this.GetPNT(dist, ref vector, ref vector2, ref vector2);
		return vector.Value;
	}

	public Vector3 GetWorldPosition()
	{
		return this.GetWorldPosition(this.Distance);
	}

	public Vector3 GetNormal(float dist)
	{
		Vector3? vector = new Vector3?(default(Vector3));
		Vector3? vector2 = null;
		this.GetPNT(dist, ref vector2, ref vector, ref vector2);
		return vector.Value;
	}

	public Vector3 GetNormal()
	{
		return this.GetNormal(this.Distance);
	}

	public Vector3 GetTangent(float dist)
	{
		Vector3? vector = new Vector3?(default(Vector3));
		Vector3? vector2 = null;
		this.GetPNT(dist, ref vector2, ref vector2, ref vector);
		return vector.Value;
	}

	public Vector3 GetTangent()
	{
		return this.GetTangent(this.Distance);
	}

	public void GetPNT(float dist, ref Vector3? wpos, ref Vector3? normal, ref Vector3? tangent)
	{
		this.AssertPathNotValid();
		ResPathObject resPath = PathEvaluator.GetResPath(this);
		if (resPath == null)
		{
			return;
		}
		int? num = new int?(this.m_cache);
		resPath.EvaluateResult(dist, ref wpos, ref normal, ref tangent, ref num);
		this.m_cache = num.Value;
	}

	public void GetPNT(ref Vector3? wpos, ref Vector3? normal, ref Vector3? tangent)
	{
		this.GetPNT(this.Distance, ref wpos, ref normal, ref tangent);
	}

	public void Advance(float delta)
	{
		this.AssertPathNotValid();
		ResPathObject resPath = PathEvaluator.GetResPath(this);
		if (resPath == null)
		{
			return;
		}
		this.m_dist = resPath.NormalizeDistance(this.m_dist + delta);
	}

	public void GetClosestPositionAlongSpline(Vector3 point, float from, float to, out float dist)
	{
		this.AssertPathNotValid();
		ResPathObject resPath = PathEvaluator.GetResPath(this);
		if (resPath == null)
		{
			dist = 0f;
			return;
		}
		resPath.GetClosestPosition(ref point, from, to, out dist);
	}

	public float GetClosestPositionAlongSpline(Vector3 point, float from, float to)
	{
		float result;
		this.GetClosestPositionAlongSpline(point, from, to, out result);
		return result;
	}

	public float GetClosestPositionAlongSplineInterpolate(Vector3 point, float from, float to, ref Vector3? center, ref float? radius)
	{
		float closestPositionAlongSpline = this.GetClosestPositionAlongSpline(point, from, to);
		if (center.HasValue)
		{
			center = new Vector3?(Vector3.zero);
		}
		if (radius.HasValue)
		{
			radius = new float?(0f);
		}
		Vector3 vector = default(Vector3);
		Vector3 vector2 = default(Vector3);
		Vector3 vector3 = default(Vector3);
		ResPathObject resPathObject = this.GetPathObject().GetResPathObject();
		if (resPathObject == null)
		{
			return 0f;
		}
		ResPathObjectData objectData = resPathObject.GetObjectData();
		int num = (int)(objectData.numKeys - 1);
		if (num <= 1)
		{
			return closestPositionAlongSpline;
		}
		float dist = resPathObject.NormalizeDistance(closestPositionAlongSpline);
		int? num2 = new int?(this.m_cache);
		int keyAtDistance = resPathObject.GetKeyAtDistance(dist, ref num2);
		this.m_cache = num2.Value;
		ResPathObject.PlaybackType playbackType = (ResPathObject.PlaybackType)objectData.playbackType;
		if (playbackType == ResPathObject.PlaybackType.PLAYBACK_ONCE)
		{
			int num3 = keyAtDistance;
			if (num3 == 0)
			{
				num3++;
			}
			else if (num3 >= num)
			{
				num3--;
			}
			vector2 = objectData.position[num3 - 1];
			vector = objectData.position[num3];
			vector3 = objectData.position[num3 + 1];
		}
		else if (playbackType == ResPathObject.PlaybackType.PLAYBACK_LOOP)
		{
			vector2 = objectData.position[(keyAtDistance != 0) ? (keyAtDistance - 1) : (num - 1)];
			vector = objectData.position[keyAtDistance];
			vector3 = objectData.position[(keyAtDistance != num) ? (keyAtDistance + 1) : 1];
		}
		Vector3 lhs = vector2 - vector;
		Vector3 rhs = vector3 - vector;
		float magnitude = lhs.magnitude;
		float magnitude2 = rhs.magnitude;
		if (magnitude > 1E-06f && magnitude2 > 1E-06f)
		{
			float num4 = Vector3.Dot(lhs, rhs) / magnitude / magnitude2;
			num4 = Mathf.Clamp(num4, -1f, 1f);
			float num5 = 3.14159274f - Mathf.Acos(num4);
			if (num5 > 1E-06f && (magnitude + magnitude2) * (6.28318548f / num5) < 100000f)
			{
				Vector3 vector4 = Vector3.Cross(lhs, rhs);
				if (!App.Math.Vector3NormalizeIfNotZero(vector4, out vector4))
				{
					return closestPositionAlongSpline;
				}
				Matrix4x4 m = App.Math.Matrix44OrthonormalDirection2(vector4, lhs.normalized);
				Matrix4x4 matrix4x = App.Math.Matrix34InverseNonSingular(m);
				Vector3 vector5 = matrix4x.MultiplyVector(vector);
				Vector3 vector6 = matrix4x.MultiplyVector(vector2);
				Vector3 vector7 = matrix4x.MultiplyVector(vector3);
				float num6 = 0f;
				float num7 = 0f;
				float num8 = 0f;
				float num9 = 0f;
				bool midperpendicular2D = PathEvaluator.GetMidperpendicular2D(vector6.x, vector6.y, vector5.x, vector5.y, ref num6, ref num8);
				bool midperpendicular2D2 = PathEvaluator.GetMidperpendicular2D(vector5.x, vector5.y, vector7.x, vector7.y, ref num7, ref num9);
				if (!midperpendicular2D && !midperpendicular2D2)
				{
					return closestPositionAlongSpline;
				}
				float num10;
				float y;
				if (!midperpendicular2D)
				{
					num10 = (vector5.x + vector6.x) * 0.5f;
					y = num7 * num10 + num9;
				}
				else if (!midperpendicular2D2)
				{
					num10 = (vector5.x + vector7.x) * 0.5f;
					y = num6 * num10 + num8;
				}
				else
				{
					num10 = (num9 - num8) / (num6 - num7);
					y = num6 * num10 + num8;
				}
				Vector3 vector8;
				vector8.x = num10;
				vector8.y = y;
				vector8.z = vector5.z;
				float num11 = vector5.x - vector8.x;
				float num12 = vector5.y - vector8.y;
				float num13 = Mathf.Sqrt(num11 * num11 + num12 * num12);
				Vector3 a = matrix4x.MultiplyVector(point);
				Vector3 a2 = a - vector8;
				a2.z = 0f;
				float magnitude3 = a2.magnitude;
				Vector3 v;
				if (magnitude3 > 1E-06f)
				{
					v = vector8 + a2 * (num13 / magnitude3);
				}
				else
				{
					v = vector8;
				}
				Vector3 value = m.MultiplyVector(vector8);
				Vector3 point2 = m.MultiplyVector(v);
				closestPositionAlongSpline = this.GetClosestPositionAlongSpline(point2, from, to);
				if (center.HasValue)
				{
					center = new Vector3?(value);
				}
				if (radius.HasValue)
				{
					radius = new float?(num13);
				}
			}
		}
		return closestPositionAlongSpline;
	}

	private static bool GetMidperpendicular2D(float sx, float sy, float ex, float ey, ref float slant, ref float intercept)
	{
		if (Mathf.Abs(sx - ex) <= 1E-06f)
		{
			slant = 0f;
			intercept = (sy + ey) * 0.5f;
			return true;
		}
		float num = (sy - ey) / (sx - ex);
		if (Mathf.Abs(num) <= 1E-06f)
		{
			return false;
		}
		float num2 = sx + (ex - sx) * 0.5f;
		float num3 = sy + (ey - sy) * 0.5f;
		slant = -1f / num;
		intercept = num3 - slant * num2;
		return true;
	}

	private void AssertPathNotValid()
	{
	}

	private static ResPathObject GetResPath(PathEvaluator self)
	{
		return self.GetPathObject().GetResPathObject();
	}
}
