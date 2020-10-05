using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Terrain
{
	private List<TerrainBlock> m_blocks;

	private string _m_name_k__BackingField;

	private float _m_meter_k__BackingField;

	public string m_name
	{
		get;
		private set;
	}

	public float m_meter
	{
		get;
		private set;
	}

	public Terrain(string name, float meter)
	{
		this.m_name = name;
		this.m_meter = meter;
		this.m_blocks = new List<TerrainBlock>();
	}

	public void AddTerrainBlock(TerrainBlock terrainBlock)
	{
		this.m_blocks.Add(terrainBlock);
	}

	public int GetBlockCount()
	{
		return this.m_blocks.Count;
	}

	public TerrainBlock GetBlock(int index)
	{
		if (index >= this.GetBlockCount())
		{
			return null;
		}
		return this.m_blocks[index];
	}
}
