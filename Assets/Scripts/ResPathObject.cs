using App;
using System;
using UnityEngine;

public class ResPathObject
{
	public enum PlaybackType
	{
		PLAYBACK_LOOP,
		PLAYBACK_ONCE,
		PLAYBACK_UNKNOWN = -1
	}

	private ResPathObjectData m_pathData;

	public string Name
	{
		get
		{
			this.AssertNoData();
			return this.m_pathData.name;
		}
	}

	public ResPathObject.PlaybackType PlayBack
	{
		get
		{
			this.AssertNoData();
			return (ResPathObject.PlaybackType)this.m_pathData.playbackType;
		}
	}

	public float Length
	{
		get
		{
			this.AssertNoData();
			return this.m_pathData.length;
		}
	}

	public int NumKeys
	{
		get
		{
			this.AssertNoData();
			return (int)this.m_pathData.numKeys;
		}
	}

	public uint NumVertices
	{
		get
		{
			this.AssertNoData();
			return this.m_pathData.numVertices;
		}
	}

	public Bounds GetBownds
	{
		get
		{
			Bounds result = default(Bounds);
			result.SetMinMax(this.m_pathData.min, this.m_pathData.max);
			return result;
		}
	}

	public ResPathObject(ResPathObjectData data)
	{
		this.m_pathData = data;
	}

	public ResPathObject(ResPathObjectData data, Vector3 offsetPosition)
	{
		this.m_pathData = this.CreateMovedPathData(data, offsetPosition);
	}

	public float GetDistance(int key)
	{
		this.AssertNoData();
		return this.m_pathData.distance[key];
	}

	public Vector3 GetPosition(int key)
	{
		this.AssertNoData();
		return this.m_pathData.position[key];
	}

	public Vector3 GetNormal(int key)
	{
		this.AssertNoData();
		return this.m_pathData.normal[key];
	}

	public Vector3 GetTangent(int key)
	{
		this.AssertNoData();
		return this.m_pathData.tangent[key];
	}

	private Vector3 GetVertex(int i)
	{
		this.AssertNoData();
		return this.m_pathData.vertices[i];
	}

	public ResPathObjectData GetObjectData()
	{
		this.AssertNoData();
		return this.m_pathData;
	}

	public float NormalizeDistance(float dist)
	{
		this.AssertNoData();
		ResPathObjectData pathData = this.m_pathData;
		float length = pathData.length;
		ResPathObject.PlaybackType playbackType = (ResPathObject.PlaybackType)pathData.playbackType;
		if (playbackType != ResPathObject.PlaybackType.PLAYBACK_LOOP)
		{
			if (playbackType != ResPathObject.PlaybackType.PLAYBACK_ONCE)
			{
				return 0f;
			}
			if (dist > length)
			{
				return length;
			}
			if (dist < 0f)
			{
				return 0f;
			}
			return dist;
		}
		else
		{
			if (dist >= 0f)
			{
				return dist % length;
			}
			return dist % length + length;
		}
	}

	public int GetKeyAtDistance(float dist, ref int? cache)
	{
		float dist2 = this.NormalizeDistance(dist);
		return this.GetKeyAtDistanceCore(dist2, ref cache);
	}

	public void EvaluateResult(float dist, ref Vector3? wpos, ref Vector3? normal, ref Vector3? tangent, ref int? cache)
	{
		this.AssertNoData();
		ResPathObjectData pathData = this.m_pathData;
		int num = (int)(pathData.numKeys - 1);
		int keyAtDistanceCore = this.GetKeyAtDistanceCore(dist, ref cache);
		if (keyAtDistanceCore >= num)
		{
			if (wpos.HasValue)
			{
				wpos = new Vector3?(pathData.position[num]);
			}
		}
		else if (wpos.HasValue)
		{
			wpos = new Vector3?(ResPathObject.InterpolatePosition(dist, keyAtDistanceCore, pathData.distance, pathData.position));
		}
		if (normal.HasValue)
		{
			normal = new Vector3?(pathData.normal[keyAtDistanceCore]);
		}
		if (tangent.HasValue)
		{
			tangent = new Vector3?(pathData.tangent[keyAtDistanceCore]);
		}
	}

	public void GetClosestPosition(ref Vector3 point, float from, float to, out float dist)
	{
		this.AssertNoData();
		ResPathObjectData pathData = this.m_pathData;
		float num = this.NormalizeDistance(from);
		float num2 = this.NormalizeDistance(to);
		if (pathData.playbackType == 1)
		{
			int? num3 = null;
			int keyAtDistanceCore = this.GetKeyAtDistanceCore(num, ref num3);
			int keyAtDistanceCore2 = this.GetKeyAtDistanceCore(num2, ref num3);
			if (keyAtDistanceCore >= (int)(pathData.numKeys - 1))
			{
				dist = pathData.length;
				return;
			}
			Vector3 vector = ResPathObject.InterpolatePosition(num, keyAtDistanceCore, pathData.distance, pathData.position);
			Vector3 vector2 = ResPathObject.InterpolatePosition(num2, keyAtDistanceCore2, pathData.distance, pathData.position);
			if (keyAtDistanceCore == keyAtDistanceCore2)
			{
				float? num4 = new float?(0f);
				ResPathObject.DistanceSqPointPath(ref point, ref vector, ref vector2, ref num4);
				dist = Mathf.Lerp(num, num2, num4.Value);
			}
			else
			{
				float? num5 = new float?(0f);
				float num6 = ResPathObject.DistanceSqPointPath(ref point, ref vector, ref pathData.position[keyAtDistanceCore + 1], ref num5);
				float num7 = Mathf.Lerp(num, pathData.distance[keyAtDistanceCore + 1], num5.Value);
				float num8 = ResPathObject.DistanceSqPointPath(ref point, ref pathData.position[keyAtDistanceCore2], ref vector2, ref num5);
				if (num6 > num8)
				{
					num6 = num8;
					num7 = Mathf.Lerp(pathData.distance[keyAtDistanceCore2], num2, num5.Value);
				}
				for (int i = keyAtDistanceCore + 1; i < keyAtDistanceCore2; i++)
				{
					num8 = ResPathObject.DistanceSqPointPath(ref point, ref pathData.position[i], ref pathData.position[i + 1], ref num5);
					if (num6 > num8)
					{
						num6 = num8;
						num7 = Mathf.Lerp(pathData.distance[i], pathData.distance[i + 1], num5.Value);
					}
				}
				dist = num7;
			}
		}
		else
		{
			int num9 = (int)(pathData.numKeys - 1);
			int num10;
			int num11;
			if (Mathf.Abs(from - to) >= pathData.length)
			{
				num = 0f;
				num2 = pathData.length;
				num10 = 0;
				num11 = num9 - 1;
			}
			else
			{
				int? num12 = null;
				num10 = this.GetKeyAtDistance(num, ref num12);
				num11 = this.GetKeyAtDistance(num2, ref num12);
				if (num11 < num10)
				{
					num11 += num9;
				}
			}
			Vector3 vector3 = ResPathObject.InterpolatePosition(num, num10 % num9, pathData.distance, pathData.position);
			Vector3 vector4 = ResPathObject.InterpolatePosition(num2, num11 % num9, pathData.distance, pathData.position);
			if (num10 == num11)
			{
				float? num13 = new float?(0f);
				ResPathObject.DistanceSqPointPath(ref point, ref vector3, ref vector4, ref num13);
				dist = Mathf.Lerp(num, num2, num13.Value);
			}
			else
			{
				float? num14 = new float?(0f);
				float num15 = ResPathObject.DistanceSqPointPath(ref point, ref vector3, ref pathData.position[num10 % num9 + 1], ref num14);
				float num16 = Mathf.Lerp(num, pathData.distance[num10 % num9 + 1], num14.Value);
				float num17 = ResPathObject.DistanceSqPointPath(ref point, ref pathData.position[num11 % num9], ref vector4, ref num14);
				if (num15 > num17)
				{
					num15 = num17;
					num16 = Mathf.Lerp(pathData.distance[num11 % num9], num2, num14.Value);
				}
				for (int j = num10 + 1; j < num11; j++)
				{
					int num18 = j % num9;
					num17 = ResPathObject.DistanceSqPointPath(ref point, ref pathData.position[num18], ref pathData.position[num18 + 1], ref num14);
					if (num15 > num17)
					{
						num15 = num17;
						num16 = Mathf.Lerp(pathData.distance[num18], pathData.distance[num18 + 1], num14.Value);
					}
				}
				dist = num16;
			}
		}
	}

	private int GetKeyAtDistanceCore(float dist, ref int? cache)
	{
		this.AssertNoData();
		ResPathObjectData pathData = this.m_pathData;
		float[] distance = pathData.distance;
		if (cache.HasValue)
		{
			int value = cache.Value;
			if (value == (int)(pathData.numKeys - 1))
			{
				if (dist >= distance[value])
				{
					return value;
				}
			}
			else
			{
				float num = distance[value];
				float num2 = distance[value + 1];
				if (dist >= num && dist < num2)
				{
					return value;
				}
			}
		}
		int num3 = 0;
		int num4 = (int)(pathData.numKeys - 1);
		while (num4 - num3 > 1)
		{
			int num5 = (num3 + num4) / 2;
			if (dist >= distance[num5])
			{
				num3 = num5;
			}
			else
			{
				num4 = num5;
			}
		}
		int num6 = num3;
		if (cache.HasValue)
		{
			cache = new int?(num6);
		}
		return num6;
	}

	private static Vector3 InterpolatePosition(float dist, int key, float[] distanceArray, Vector3[] positionArray)
	{
		if (dist == distanceArray[key])
		{
			return positionArray[key];
		}
		float x = distanceArray[key + 1] - distanceArray[key];
		float t = (dist - distanceArray[key]) * App.Math.Reciprocal(x);
		return Vector3.Lerp(positionArray[key], positionArray[key + 1], t);
	}

	private static float DistanceSqPointPath(ref Vector3 point, ref Vector3 lp0, ref Vector3 lp1, ref float? t)
	{
		if (Vector3.SqrMagnitude(lp0 - lp1) < 0.01f)
		{
			t = new float?(0f);
			return Vector3.SqrMagnitude(point - lp0);
		}
		return Segment3.DistanceSq(ref lp0, ref lp1, ref point, ref t);
	}

	private void AssertNoData()
	{
	}

	private ResPathObjectData CreateMovedPathData(ResPathObjectData src, Vector3 offset)
	{
		ResPathObjectData resPathObjectData = new ResPathObjectData();
		resPathObjectData.name = src.name;
		resPathObjectData.playbackType = src.playbackType;
		resPathObjectData.flags = src.flags;
		resPathObjectData.numKeys = src.numKeys;
		resPathObjectData.length = src.length;
		resPathObjectData.distance = new float[(int)resPathObjectData.numKeys];
		src.distance.CopyTo(resPathObjectData.distance, 0);
		resPathObjectData.position = new Vector3[(int)resPathObjectData.numKeys];
		for (int i = 0; i < resPathObjectData.position.Length; i++)
		{
			resPathObjectData.position[i] = src.position[i] + offset;
		}
		resPathObjectData.normal = new Vector3[(int)resPathObjectData.numKeys];
		src.normal.CopyTo(resPathObjectData.normal, 0);
		resPathObjectData.tangent = new Vector3[(int)resPathObjectData.numKeys];
		src.tangent.CopyTo(resPathObjectData.tangent, 0);
		resPathObjectData.numVertices = src.numVertices;
		resPathObjectData.vertices = new Vector3[resPathObjectData.numVertices];
		for (int j = 0; j < resPathObjectData.vertices.Length; j++)
		{
			resPathObjectData.vertices[j] += src.vertices[j] + offset;
		}
		resPathObjectData.min = src.min + offset;
		resPathObjectData.max = src.max + offset;
		resPathObjectData.uid = src.uid;
		return resPathObjectData;
	}
}
