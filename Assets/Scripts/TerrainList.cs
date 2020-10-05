using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal class TerrainList
{
	private Dictionary<string, Terrain> m_terrainList;

	private string _m_name_k__BackingField;

	public string m_name
	{
		get;
		private set;
	}

	public TerrainList(string name)
	{
		this.m_name = name;
		this.m_terrainList = new Dictionary<string, Terrain>();
	}

	public void AddTerrain(Terrain terrain)
	{
		this.m_terrainList.Add(terrain.m_name, terrain);
	}

	public int GetTerrainCount()
	{
		return this.m_terrainList.Count;
	}

	public Terrain GetTerrain(string name)
	{
		if (!this.m_terrainList.ContainsKey(name))
		{
			return null;
		}
		return this.m_terrainList[name];
	}

	public Terrain GetTerrain(int index)
	{
		string name = string.Format("{0:00}", index);
		return this.GetTerrain(name);
	}
}
