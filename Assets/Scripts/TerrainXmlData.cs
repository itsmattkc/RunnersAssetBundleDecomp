using System;
using UnityEngine;

public class TerrainXmlData : MonoBehaviour
{
	public const string DefaultSetDataAssetName = "TerrainBlockData";

	[SerializeField]
	private TextAsset m_terrainBlock;

	[SerializeField]
	private TextAsset m_sideViewPath;

	[SerializeField]
	private TextAsset m_loopPath;

	[SerializeField]
	private TextAsset[] m_setData = new TextAsset[22];

	[SerializeField]
	private TextAsset m_itemTableData;

	[SerializeField]
	private TextAsset m_rareEnemyTableData;

	[SerializeField]
	private TextAsset m_bossTableData;

	[SerializeField]
	private TextAsset m_bossMap3TableData;

	[SerializeField]
	private TextAsset m_objectPartTableData;

	[SerializeField]
	private TextAsset m_EnemyExtendItemTableData;

	[SerializeField]
	private int m_moveTrapBooRand;

	public static string m_setDataAssetName = "TerrainBlockData";

	public static string DataAssetName
	{
		get
		{
			return TerrainXmlData.m_setDataAssetName;
		}
	}

	public TextAsset TerrainBlock
	{
		get
		{
			return this.m_terrainBlock;
		}
	}

	public TextAsset SideViewPath
	{
		get
		{
			return this.m_sideViewPath;
		}
	}

	public TextAsset LoopPath
	{
		get
		{
			return this.m_loopPath;
		}
	}

	public TextAsset[] SetData
	{
		get
		{
			return this.m_setData;
		}
	}

	public TextAsset ItemTableData
	{
		get
		{
			return this.m_itemTableData;
		}
	}

	public TextAsset RareEnemyTableData
	{
		get
		{
			return this.m_rareEnemyTableData;
		}
	}

	public TextAsset BossTableData
	{
		get
		{
			return this.m_bossTableData;
		}
	}

	public TextAsset BossMap3TableData
	{
		get
		{
			return this.m_bossMap3TableData;
		}
	}

	public TextAsset ObjectPartTableData
	{
		get
		{
			return this.m_objectPartTableData;
		}
	}

	public TextAsset EnemyExtendItemTableData
	{
		get
		{
			return this.m_EnemyExtendItemTableData;
		}
	}

	public int MoveTrapBooRand
	{
		get
		{
			return this.m_moveTrapBooRand;
		}
	}

	public static void SetAssetName(string stageName)
	{
		TerrainXmlData.m_setDataAssetName = stageName + "_TerrainBlockData";
	}
}
