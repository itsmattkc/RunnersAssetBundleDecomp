using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TerrainPlacementManager : MonoBehaviour
{
	public class TerrainReserveInfo
	{
		public string m_baseName;

		public int m_count;

		public TerrainReserveInfo(string name, int count)
		{
			this.m_baseName = name;
			this.m_count = count;
		}

		public string GetBlockName(string stageName)
		{
			return stageName + this.m_baseName;
		}
	}

	private sealed class _CreateTerrain_c__Iterator1A : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal string _stageName___0;

		internal GameModeStage _gameModeStage___1;

		internal TerrainPlacementManager.TerrainReserveInfo[] _tbl___2;

		internal TerrainPlacementManager.TerrainReserveInfo[] __s_289___3;

		internal int __s_290___4;

		internal TerrainPlacementManager.TerrainReserveInfo _dataInfo___5;

		internal string _blockName___6;

		internal GameObject _gameObject___7;

		internal int _index___8;

		internal GameObject _gameObjectCopy___9;

		internal TerrainReserveObject _data___10;

		internal int _waiteframe___11;

		internal List<TerrainReserveObject>.Enumerator __s_291___12;

		internal TerrainReserveObject _obj___13;

		internal int _PC;

		internal object _current;

		internal TerrainPlacementManager __f__this;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				if (this.__f__this.m_isCreateTerrain)
				{
					goto IL_2B3;
				}
				this._stageName___0 = "w01";
				this._gameModeStage___1 = GameObjectUtil.FindGameObjectComponent<GameModeStage>("GameModeStage");
				if (this._gameModeStage___1 != null)
				{
					this._stageName___0 = this._gameModeStage___1.GetStageName();
				}
				this._tbl___2 = ((!this.__f__this.m_isBossStage) ? this.__f__this.TERRAIN_MODEL_TBL : this.__f__this.BOSS_STAGE_TERRAIN_MODEL_TBL);
				this.__s_289___3 = this._tbl___2;
				this.__s_290___4 = 0;
				while (this.__s_290___4 < this.__s_289___3.Length)
				{
					this._dataInfo___5 = this.__s_289___3[this.__s_290___4];
					this._blockName___6 = this._dataInfo___5.GetBlockName(this._stageName___0);
					this._gameObject___7 = ResourceManager.Instance.GetGameObject(ResourceCategory.TERRAIN_MODEL, this._blockName___6);
					if (this._gameObject___7 != null)
					{
						this._index___8 = 0;
						while (this._index___8 < this._dataInfo___5.m_count)
						{
							this._gameObjectCopy___9 = (UnityEngine.Object.Instantiate(this._gameObject___7, Vector3.zero, Quaternion.identity) as GameObject);
							if (this._gameObjectCopy___9 != null)
							{
								this._gameObjectCopy___9.isStatic = true;
								this._gameObjectCopy___9.SetActive(true);
								this._data___10 = new TerrainReserveObject(this._gameObjectCopy___9, this._blockName___6, this.__f__this.m_reserveObjectList.Count);
								if (this._data___10 != null)
								{
									this.__f__this.m_reserveObjectList.Add(this._data___10);
								}
							}
							this._index___8++;
						}
					}
					this.__s_290___4++;
				}
				if (this.__f__this.m_reserveObjectList.Count <= 0)
				{
					goto IL_2B3;
				}
				this._waiteframe___11 = 1;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._waiteframe___11 > 0)
			{
				this._waiteframe___11--;
				this._current = null;
				this._PC = 1;
				return true;
			}
			this.__s_291___12 = this.__f__this.m_reserveObjectList.GetEnumerator();
			try
			{
				while (this.__s_291___12.MoveNext())
				{
					this._obj___13 = this.__s_291___12.Current;
					if (this._obj___13 != null)
					{
						this._obj___13.ReturnObject();
					}
				}
			}
			finally
			{
				((IDisposable)this.__s_291___12).Dispose();
			}
			this.__f__this.m_isCreateTerrain = true;
			GC.Collect();
			IL_2B3:
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private const float RangeInDistance = 40f;

	private const float RangeOutDistance = 30f;

	private const float MaxOfRange = 200f;

	private const int SystemBlockIndexStart = 91;

	private const int SystemBlockIndexEnd = 99;

	private const float ZOffset = 1.5f;

	public TextAsset m_setData;

	private TerrainList m_terrainList;

	private List<TerrainPlacementInfo> m_placementList;

	private List<TerrainReserveObject> m_reserveObjectList = new List<TerrainReserveObject>();

	private int m_defaultLayer;

	private bool m_isBossStage;

	private bool m_isCreateTerrain;

	private TerrainPlacementManager.TerrainReserveInfo[] TERRAIN_MODEL_TBL = new TerrainPlacementManager.TerrainReserveInfo[]
	{
		new TerrainPlacementManager.TerrainReserveInfo("_pf_002m", 8),
		new TerrainPlacementManager.TerrainReserveInfo("_pf_006m", 4),
		new TerrainPlacementManager.TerrainReserveInfo("_pf_012m", 3),
		new TerrainPlacementManager.TerrainReserveInfo("_pf_024m", 3),
		new TerrainPlacementManager.TerrainReserveInfo("_pfloop_014m", 3),
		new TerrainPlacementManager.TerrainReserveInfo("_pfslopedown_012m", 3),
		new TerrainPlacementManager.TerrainReserveInfo("_pfslopedown_014m", 3),
		new TerrainPlacementManager.TerrainReserveInfo("_pfslopedown_018m", 2),
		new TerrainPlacementManager.TerrainReserveInfo("_pfslopeup_012m", 3),
		new TerrainPlacementManager.TerrainReserveInfo("_pfslopeup_014m", 3),
		new TerrainPlacementManager.TerrainReserveInfo("_pfslopeup_018m", 2),
		new TerrainPlacementManager.TerrainReserveInfo("_pfslope_down_012m", 3),
		new TerrainPlacementManager.TerrainReserveInfo("_pfslope_down_014m", 3),
		new TerrainPlacementManager.TerrainReserveInfo("_pfslope_down_018m", 2),
		new TerrainPlacementManager.TerrainReserveInfo("_pfslope_up_012m", 3),
		new TerrainPlacementManager.TerrainReserveInfo("_pfslope_up_014m", 3),
		new TerrainPlacementManager.TerrainReserveInfo("_pfslope_up_018m", 2)
	};

	private TerrainPlacementManager.TerrainReserveInfo[] BOSS_STAGE_TERRAIN_MODEL_TBL = new TerrainPlacementManager.TerrainReserveInfo[]
	{
		new TerrainPlacementManager.TerrainReserveInfo("_pf_002m", 4),
		new TerrainPlacementManager.TerrainReserveInfo("_pf_006m", 4),
		new TerrainPlacementManager.TerrainReserveInfo("_pf_012m", 3),
		new TerrainPlacementManager.TerrainReserveInfo("_pf_024m", 3)
	};

	private void Start()
	{
		base.tag = "StageManager";
		this.m_defaultLayer = LayerMask.NameToLayer("Terrain");
	}

	private void Update()
	{
		float x = base.transform.position.x;
		this.CheckRangeIn(x);
		this.CheckRangeOut(x);
	}

	public void Setup(bool isBossStage)
	{
		this.m_isBossStage = isBossStage;
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.TERRAIN_MODEL, TerrainXmlData.DataAssetName);
		if (gameObject == null)
		{
			return;
		}
		TerrainXmlData component = gameObject.GetComponent<TerrainXmlData>();
		if (component == null)
		{
			return;
		}
		this.m_terrainList = ImportTerrains.Import(component.TerrainBlock);
		this.m_placementList = new List<TerrainPlacementInfo>();
		StageBlockManager component2 = base.gameObject.GetComponent<StageBlockManager>();
		if (component2 == null)
		{
			return;
		}
		if (this.m_terrainList == null)
		{
			return;
		}
		int terrainCount = this.m_terrainList.GetTerrainCount();
		for (int i = 0; i < terrainCount; i++)
		{
			this.AddTerrainInfo(component2, i);
		}
		for (int j = 91; j <= 99; j++)
		{
			this.AddTerrainInfo(component2, j);
		}
		base.StartCoroutine(this.CreateTerrain());
	}

	public void ReCreateTerrain()
	{
		this.DeleteTerrain();
		base.StartCoroutine(this.CreateTerrain());
	}

	private void AddTerrainInfo(StageBlockManager blockManager, int index)
	{
		Terrain terrain = this.m_terrainList.GetTerrain(index);
		if (terrain == null)
		{
			return;
		}
		int terrainIndex = int.Parse(terrain.m_name);
		float meter = terrain.m_meter;
		blockManager.AddTerrainInfo(terrainIndex, meter);
	}

	public void ActivateTerrain(int terrainIndex, Vector3 originPosition)
	{
		if (this.m_terrainList == null)
		{
			return;
		}
		Terrain terrain = this.m_terrainList.GetTerrain(terrainIndex);
		if (terrain == null)
		{
			return;
		}
		int blockCount = terrain.GetBlockCount();
		for (int i = 0; i < blockCount; i++)
		{
			TerrainBlock block = terrain.GetBlock(i);
			if (block != null)
			{
				string name = block.m_name;
				GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.TERRAIN_MODEL, name);
				if (!(gameObject == null))
				{
					TerrainPlacementInfo terrainPlacementInfo = new TerrainPlacementInfo();
					terrainPlacementInfo.m_terrainIndex = terrainIndex;
					Vector3 pos = originPosition + block.m_transform.m_pos;
					Vector3 rot = block.m_transform.m_rot;
					TransformParam transform = new TransformParam(pos, rot);
					terrainPlacementInfo.m_block = new TerrainBlock(block.m_name, transform);
					gameObject.layer = this.m_defaultLayer;
					this.m_placementList.Add(terrainPlacementInfo);
				}
			}
		}
	}

	public void DeactivateTerrain(int terrainIndex, float basePosition)
	{
		for (int i = this.m_placementList.Count - 1; i >= 0; i--)
		{
			TerrainPlacementInfo terrainPlacementInfo = this.m_placementList[i];
			if (terrainPlacementInfo != null)
			{
				if (terrainPlacementInfo.m_terrainIndex == terrainIndex && StageBlockUtil.IsPastPosition(terrainPlacementInfo.m_block.m_transform.m_pos.x, basePosition, 0f))
				{
					if (terrainPlacementInfo.IsReserveTerrain())
					{
						this.ReturnTerrainReserveObject(terrainPlacementInfo);
					}
					else
					{
						this.DestroyTerrain(terrainPlacementInfo);
					}
					this.m_placementList.Remove(terrainPlacementInfo);
				}
			}
		}
	}

	private void DeleteTerrain()
	{
		foreach (TerrainReserveObject current in this.m_reserveObjectList)
		{
			GameObject gameObject = current.GetGameObject();
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		this.m_reserveObjectList.Clear();
		this.m_isCreateTerrain = false;
	}

	private IEnumerator CreateTerrain()
	{
		TerrainPlacementManager._CreateTerrain_c__Iterator1A _CreateTerrain_c__Iterator1A = new TerrainPlacementManager._CreateTerrain_c__Iterator1A();
		_CreateTerrain_c__Iterator1A.__f__this = this;
		return _CreateTerrain_c__Iterator1A;
	}

	private TerrainReserveObject GetTerrainReserveObject(string blockName)
	{
		foreach (TerrainReserveObject current in this.m_reserveObjectList)
		{
			if (current != null && current.EableReservation && current.blockName == blockName)
			{
				return current;
			}
		}
		return null;
	}

	private void ReturnTerrainReserveObject(TerrainPlacementInfo info)
	{
		if (info != null)
		{
			foreach (TerrainReserveObject current in this.m_reserveObjectList)
			{
				if (current != null && current.ReserveIndex == info.ReserveIndex)
				{
					current.ReturnObject();
					info.DestroyObject();
					break;
				}
			}
		}
	}

	private void CheckRangeIn(float basePosition)
	{
		if (this.m_placementList == null)
		{
			return;
		}
		List<TerrainPlacementInfo> list = null;
		foreach (TerrainPlacementInfo current in this.m_placementList)
		{
			if (current != null)
			{
				if (!current.Created)
				{
					float x = current.m_block.m_transform.m_pos.x;
					float num = x - basePosition;
					if (num > 200f)
					{
						break;
					}
					if (num < 40f)
					{
						TerrainReserveObject terrainReserveObject = this.GetTerrainReserveObject(current.m_block.m_name);
						if (terrainReserveObject != null)
						{
							current.m_gameObject = terrainReserveObject.ReserveObject();
							current.ReserveIndex = terrainReserveObject.ReserveIndex;
							Vector3 pos = current.m_block.m_transform.m_pos;
							pos.z += 1.5f;
							current.m_gameObject.transform.position = pos;
							current.m_gameObject.transform.rotation = Quaternion.Euler(current.m_block.m_transform.m_rot);
							current.m_gameObject.SetActive(true);
						}
						else
						{
							global::Debug.Log("Terrain Instantiate!!  m_block = " + current.m_block.m_name);
							if (!this.CreateTerrainObject(current))
							{
								if (list == null)
								{
									list = new List<TerrainPlacementInfo>();
								}
								list.Add(current);
							}
						}
					}
				}
			}
		}
		if (list == null)
		{
			return;
		}
		foreach (TerrainPlacementInfo current2 in list)
		{
			if (current2 != null)
			{
				this.m_placementList.Remove(current2);
			}
		}
	}

	private void CheckRangeOut(float basePosition)
	{
		if (this.m_placementList == null)
		{
			return;
		}
		List<TerrainPlacementInfo> list = null;
		foreach (TerrainPlacementInfo current in this.m_placementList)
		{
			if (current != null)
			{
				if (current.Created)
				{
					if (!current.Destroyed)
					{
						float num = basePosition - current.m_block.m_transform.m_pos.x;
						if (num < 0f)
						{
							break;
						}
						if (num > 30f)
						{
							if (current.IsReserveTerrain())
							{
								this.ReturnTerrainReserveObject(current);
							}
							else
							{
								this.DestroyTerrain(current);
							}
							if (list == null)
							{
								list = new List<TerrainPlacementInfo>();
							}
							list.Add(current);
						}
					}
				}
			}
		}
		if (list == null)
		{
			return;
		}
		foreach (TerrainPlacementInfo current2 in list)
		{
			if (current2 != null)
			{
				this.m_placementList.Remove(current2);
			}
		}
	}

	private bool CreateTerrainObject(TerrainPlacementInfo info)
	{
		if (info == null)
		{
			return false;
		}
		string name = info.m_block.m_name;
		GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.TERRAIN_MODEL, name);
		if (gameObject == null)
		{
			return false;
		}
		Vector3 pos = info.m_block.m_transform.m_pos;
		pos.z += 1.5f;
		Vector3 rot = info.m_block.m_transform.m_rot;
		Quaternion rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, pos, rotation) as GameObject;
		if (gameObject2 == null)
		{
			return false;
		}
		gameObject2.isStatic = true;
		gameObject2.SetActive(true);
		info.m_gameObject = gameObject2;
		return true;
	}

	private void DestroyTerrain(TerrainPlacementInfo info)
	{
		UnityEngine.Object.Destroy(info.m_gameObject);
		info.DestroyObject();
	}

	public void OnActivateBlock(MsgActivateBlock msg)
	{
		if (msg != null)
		{
			this.ActivateTerrain(msg.m_blockNo, msg.m_originPosition);
		}
	}

	private void OnDeactivateBlock(MsgDeactivateBlock msg)
	{
		if (msg != null)
		{
			this.DeactivateTerrain(msg.m_blockNo, msg.m_distance);
		}
	}

	private void OnDeactivateAllBlock(MsgDeactivateAllBlock msg)
	{
		foreach (TerrainPlacementInfo current in this.m_placementList)
		{
			if (current.IsReserveTerrain())
			{
				this.ReturnTerrainReserveObject(current);
			}
			else
			{
				this.DestroyTerrain(current);
			}
		}
		this.m_placementList.Clear();
	}

	private void OnMsgExitStage(MsgExitStage msg)
	{
		base.enabled = false;
	}
}
