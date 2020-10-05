using System;
using System.Collections.Generic;

public class BlockSpawnableParameterContainer
{
	public readonly int m_block;

	public readonly int m_layer;

	private List<SpawnableParameter> m_parameters;

	public int Block
	{
		get
		{
			return this.m_block;
		}
	}

	public int Layer
	{
		get
		{
			return this.m_layer;
		}
	}

	public BlockSpawnableParameterContainer(int blk, int ptn)
	{
		this.m_parameters = new List<SpawnableParameter>();
		this.m_block = blk;
		this.m_layer = ptn;
	}

	private BlockSpawnableParameterContainer()
	{
		this.m_parameters = new List<SpawnableParameter>();
	}

	public void AddParameter(SpawnableParameter param)
	{
		this.m_parameters.Add(param);
	}

	public SpawnableParameter GetParameter(int id)
	{
		if (id >= this.m_parameters.Count)
		{
			return null;
		}
		return this.m_parameters[id];
	}

	public List<SpawnableParameter> GetParameters()
	{
		return this.m_parameters;
	}
}
