using System;
using UnityEngine;

[Serializable]
public class SpawnableParameter
{
	public const float DefaultRangeIn = 20f;

	public const float DefaultRangeOut = 30f;

	private string objectname;

	private Vector3 position;

	private Quaternion rotation;

	private uint m_id;

	public float rangein = 20f;

	public float rangeout = 30f;

	public uint ID
	{
		get
		{
			return this.m_id;
		}
		set
		{
			this.m_id = value;
		}
	}

	public string ObjectName
	{
		get
		{
			return this.objectname;
		}
		set
		{
			this.objectname = value;
		}
	}

	public Vector3 Position
	{
		get
		{
			return this.position;
		}
		set
		{
			this.position = value;
		}
	}

	public Quaternion Rotation
	{
		get
		{
			return this.rotation;
		}
		set
		{
			this.rotation = value;
		}
	}

	public float RangeIn
	{
		get
		{
			return this.rangein;
		}
		set
		{
			this.rangein = value;
		}
	}

	public float RangeOut
	{
		get
		{
			return this.rangeout;
		}
		set
		{
			this.rangeout = value;
		}
	}

	public SpawnableParameter()
	{
	}

	public SpawnableParameter(string name)
	{
		this.objectname = name;
	}
}
