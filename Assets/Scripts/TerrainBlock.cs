using System;
using System.Runtime.CompilerServices;

public class TerrainBlock
{
	private string _m_name_k__BackingField;

	private TransformParam _m_transform_k__BackingField;

	public string m_name
	{
		get;
		private set;
	}

	public TransformParam m_transform
	{
		get;
		private set;
	}

	public TerrainBlock(string name, TransformParam transform)
	{
		this.m_name = name;
		this.m_transform = transform;
	}
}
