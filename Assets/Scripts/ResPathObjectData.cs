using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResPathObjectData
{
	private string _name_k__BackingField;

	private byte _playbackType_k__BackingField;

	private byte _flags_k__BackingField;

	private ushort _numKeys_k__BackingField;

	private float _length_k__BackingField;

	private ushort[] _knotType_k__BackingField;

	private float[] _distance_k__BackingField;

	private Vector3[] _position_k__BackingField;

	private Vector3[] _normal_k__BackingField;

	private Vector3[] _tangent_k__BackingField;

	private uint _numVertices_k__BackingField;

	private Vector3[] _vertices_k__BackingField;

	private Vector3 _min_k__BackingField;

	private Vector3 _max_k__BackingField;

	private ulong _uid_k__BackingField;

	public string name
	{
		get;
		set;
	}

	public byte playbackType
	{
		get;
		set;
	}

	public byte flags
	{
		get;
		set;
	}

	public ushort numKeys
	{
		get;
		set;
	}

	public float length
	{
		get;
		set;
	}

	public ushort[] knotType
	{
		get;
		set;
	}

	public float[] distance
	{
		get;
		set;
	}

	public Vector3[] position
	{
		get;
		set;
	}

	public Vector3[] normal
	{
		get;
		set;
	}

	public Vector3[] tangent
	{
		get;
		set;
	}

	public uint numVertices
	{
		get;
		set;
	}

	public Vector3[] vertices
	{
		get;
		set;
	}

	public Vector3 min
	{
		get;
		set;
	}

	public Vector3 max
	{
		get;
		set;
	}

	public ulong uid
	{
		get;
		set;
	}
}
