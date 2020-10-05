using System;
using System.Collections.Generic;

public class StageSpawnableParameterContainer
{
	private Dictionary<uint, BlockSpawnableParameterContainer> m_setData;

	public StageSpawnableParameterContainer()
	{
		this.m_setData = new Dictionary<uint, BlockSpawnableParameterContainer>();
	}

	public void AddData(int block, int layer, BlockSpawnableParameterContainer data)
	{
		uint uniqueID = StageSpawnableParameterContainer.GetUniqueID(block, layer);
		this.m_setData.Add(uniqueID, data);
	}

	public BlockSpawnableParameterContainer GetBlockData(int blockID, int layerID)
	{
		uint uniqueID = StageSpawnableParameterContainer.GetUniqueID(blockID, layerID);
		BlockSpawnableParameterContainer result;
		this.m_setData.TryGetValue(uniqueID, out result);
		return result;
	}

	public SpawnableParameter GetParameter(int blockID, int layerID, int id)
	{
		uint uniqueID = StageSpawnableParameterContainer.GetUniqueID(blockID, layerID);
		if (!this.m_setData.ContainsKey(uniqueID))
		{
			return null;
		}
		BlockSpawnableParameterContainer blockSpawnableParameterContainer = this.m_setData[uniqueID];
		if (blockSpawnableParameterContainer != null)
		{
			return blockSpawnableParameterContainer.GetParameter(id);
		}
		return null;
	}

	private static uint GetUniqueID(int block, int layer)
	{
		return (uint)((block << 4) + layer);
	}
}
