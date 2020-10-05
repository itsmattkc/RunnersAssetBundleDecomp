using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Game/Level")]
public class ObjectSpawnManager : MonoBehaviour
{
	public class StockData
	{
		public string m_name;

		public int m_stockCount;

		public bool m_bossStage;

		public StockData(string name, int stockCount, bool bossStage)
		{
			this.m_name = name;
			this.m_stockCount = stockCount;
			this.m_bossStage = bossStage;
		}
	}

	private class CheckPointInfo
	{
		public int m_activateID;

		public bool m_onReplace;
	}

	public class DepotObjs
	{
		private GameObject m_depot;

		private List<SpawnableObject> m_objList = new List<SpawnableObject>();

		public DepotObjs(GameObject parentObj, StockObjectType type)
		{
			this.m_depot = new GameObject();
			this.m_depot.name = type.ToString();
			this.m_depot.transform.parent = parentObj.transform;
		}

		public void Add(SpawnableObject obj)
		{
			obj.Share = true;
			this.m_objList.Add(obj);
		}

		public SpawnableObject Get()
		{
			foreach (SpawnableObject current in this.m_objList)
			{
				if (current.Sleep)
				{
					current.Sleep = false;
					return current;
				}
			}
			return null;
		}

		public void Sleep(SpawnableObject obj)
		{
			obj.Sleep = true;
			if (obj.gameObject != null && this.m_depot != null)
			{
				obj.gameObject.transform.parent = this.m_depot.transform;
				if (obj.gameObject.activeSelf)
				{
					obj.gameObject.SetActive(false);
				}
			}
		}
	}

	private sealed class _LoadSetTableFirst_c__Iterator14 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _PC;

		internal object _current;

		internal ObjectSpawnManager __f__this;

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
				this.__f__this.enabled = false;
				this.__f__this.m_setDataLoaded = false;
				this._current = this.__f__this.StartCoroutine(this.__f__this.LoadSetTable(0));
				this._PC = 1;
				return true;
			case 1u:
				this._current = this.__f__this.StartCoroutine(this.__f__this.LoadSetTable(91));
				this._PC = 2;
				return true;
			case 2u:
				this.__f__this.m_setDataLoaded = true;
				this.__f__this.enabled = true;
				GC.Collect();
				this._PC = -1;
				break;
			}
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

	private sealed class _LoadSetTable_c__Iterator15 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int firstBlock;

		internal int _i___0;

		internal int numBlock;

		internal int _PC;

		internal object _current;

		internal int ___firstBlock;

		internal int ___numBlock;

		internal ObjectSpawnManager __f__this;

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
				this.__f__this.m_setDataLoaded = false;
				this._i___0 = this.firstBlock;
				break;
			case 1u:
				this._i___0 += 5;
				break;
			default:
				return false;
			}
			if (this._i___0 < this.firstBlock + this.numBlock)
			{
				this._current = this.__f__this.StartCoroutine(this.__f__this.LoadSetTable(this._i___0));
				this._PC = 1;
				return true;
			}
			this.__f__this.m_setDataLoaded = true;
			this.__f__this.enabled = true;
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

	private sealed class _LoadSetTable_c__Iterator16 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int[] blockTable;

		internal int readFileCount;

		internal int _count___0;

		internal int _index___1;

		internal int _PC;

		internal object _current;

		internal int[] ___blockTable;

		internal int ___readFileCount;

		internal ObjectSpawnManager __f__this;

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
				this.__f__this.m_setDataLoaded = false;
				this._count___0 = Mathf.Min(this.blockTable.Length, this.readFileCount);
				this._index___1 = 0;
				break;
			case 1u:
				this._index___1++;
				break;
			default:
				return false;
			}
			if (this._index___1 < this._count___0)
			{
				this._current = this.__f__this.StartCoroutine(this.__f__this.LoadSetTable(this.blockTable[this._index___1]));
				this._PC = 1;
				return true;
			}
			this.__f__this.m_setDataLoaded = true;
			this.__f__this.enabled = true;
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

	private sealed class _LoadSetTable_c__Iterator17 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _arrayIndex___0;

		internal int blockIndex;

		internal GameObject _assetObject___1;

		internal TerrainXmlData _terrainData___2;

		internal TextAsset[] _asset___3;

		internal TextAsset _xmlText___4;

		internal GameObject _parserObj___5;

		internal SpawnableParser _parser___6;

		internal int _PC;

		internal object _current;

		internal int ___blockIndex;

		internal ObjectSpawnManager __f__this;

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
				this._arrayIndex___0 = 0;
				if (this.blockIndex == 0)
				{
					this._arrayIndex___0 = 0;
				}
				else
				{
					this._arrayIndex___0 = (this.blockIndex - 1) / 5 + 1;
					this.blockIndex = (this._arrayIndex___0 - 1) * 5 + 1;
				}
				if (this.__f__this.m_stageSetData.GetBlockData(this.blockIndex, 0) == null && this.__f__this.m_stageSetData.GetBlockData(this.blockIndex, 1) == null)
				{
					this._assetObject___1 = this.__f__this.m_resourceManager.GetGameObject(ResourceCategory.TERRAIN_MODEL, TerrainXmlData.DataAssetName);
					if (this._assetObject___1)
					{
						this._terrainData___2 = this._assetObject___1.GetComponent<TerrainXmlData>();
						if (this._terrainData___2)
						{
							this._asset___3 = this._terrainData___2.SetData;
							if (this._asset___3 != null)
							{
								this._xmlText___4 = this._asset___3[this._arrayIndex___0];
								if (this._xmlText___4 != null)
								{
									this._parserObj___5 = new GameObject();
									this._parser___6 = this._parserObj___5.AddComponent<SpawnableParser>();
									this._current = this.__f__this.StartCoroutine(this._parser___6.CreateSetData(this.__f__this.m_resourceManager, this._xmlText___4, this.__f__this.m_stageSetData));
									this._PC = 1;
									return true;
								}
							}
						}
					}
				}
				break;
			case 1u:
				UnityEngine.Object.Destroy(this._parserObj___5);
				break;
			default:
				return false;
			}
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

	private const float MaxOfRange = 200f;

	private readonly ObjectSpawnManager.StockData[] STOCK_DATA_TABLE = new ObjectSpawnManager.StockData[]
	{
		new ObjectSpawnManager.StockData("ObjRing", 50, true),
		new ObjectSpawnManager.StockData("ObjSuperRing", 20, true),
		new ObjectSpawnManager.StockData("ObjCrystal_A", 40, false),
		new ObjectSpawnManager.StockData("ObjCrystal_B", 40, false),
		new ObjectSpawnManager.StockData("ObjCrystal_C", 40, false),
		new ObjectSpawnManager.StockData("ObjCrystal10_A", 40, false),
		new ObjectSpawnManager.StockData("ObjCrystal10_B", 40, false),
		new ObjectSpawnManager.StockData("ObjCrystal10_C", 40, false),
		new ObjectSpawnManager.StockData("ObjEventCrystal", 50, false),
		new ObjectSpawnManager.StockData("ObjEventCrystal10", 40, false),
		new ObjectSpawnManager.StockData("ObjAirTrap", 40, false),
		new ObjectSpawnManager.StockData("ObjTrap", 10, false)
	};

	private PlayerInformation m_playerInformation;

	private LevelInformation m_levelInformation;

	private StageSpawnableParameterContainer m_stageSetData;

	private List<SpawnableInfo> m_spawnableInfoList;

	private List<string> m_onlyOneObjectName;

	private ResourceManager m_resourceManager;

	private List<ObjectSpawnManager.CheckPointInfo> m_checkPointInfos = new List<ObjectSpawnManager.CheckPointInfo>();

	private GameObject m_stock;

	private Dictionary<StockObjectType, ObjectSpawnManager.DepotObjs> m_dicSpawnableObjs = new Dictionary<StockObjectType, ObjectSpawnManager.DepotObjs>();

	private bool m_setDataLoaded;

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_playerInformation != null && this.m_playerInformation.Position.x > base.transform.position.x)
		{
			base.transform.position = this.m_playerInformation.Position;
		}
		float x = base.transform.position.x;
		this.CheckRangeIn(x);
		this.CheckRangeOut(x);
	}

	private void OnDestroy()
	{
		if (this.m_spawnableInfoList == null)
		{
			return;
		}
		foreach (SpawnableInfo current in this.m_spawnableInfoList)
		{
			this.DestroyObject(current);
		}
		this.m_spawnableInfoList.Clear();
	}

	public void Setup(bool bossStage)
	{
		this.m_resourceManager = ResourceManager.Instance;
		this.m_playerInformation = ObjUtil.GetPlayerInformation();
		this.m_levelInformation = ObjUtil.GetLevelInformation();
		this.m_spawnableInfoList = new List<SpawnableInfo>();
		this.m_onlyOneObjectName = new List<string>();
		this.m_stageSetData = new StageSpawnableParameterContainer();
		this.StockStageObject(bossStage);
		base.StartCoroutine(this.LoadSetTableFirst());
	}

	private void StockStageObject(bool bossStage)
	{
		if (this.m_stock == null)
		{
			this.m_stock = new GameObject("StockGameObject");
			if (this.m_stock)
			{
				this.m_stock.transform.position = Vector3.zero;
				this.m_stock.transform.rotation = Quaternion.identity;
			}
		}
		bool flag = false;
		if (StageModeManager.Instance != null)
		{
			flag = StageModeManager.Instance.IsQuickMode();
		}
		if (this.m_dicSpawnableObjs.Count == 0)
		{
			for (int i = 0; i < 12; i++)
			{
				StockObjectType stockObjectType = (StockObjectType)i;
				this.m_dicSpawnableObjs.Add(stockObjectType, new ObjectSpawnManager.DepotObjs(this.m_stock, stockObjectType));
			}
		}
		if (ResourceManager.Instance != null)
		{
			for (int j = 0; j < 12; j++)
			{
				if (!bossStage || this.STOCK_DATA_TABLE[j].m_bossStage)
				{
					if (j == 8 || j == 9)
					{
						if (!flag)
						{
							goto IL_1CB;
						}
						if (EventManager.Instance != null && EventManager.Instance.Type != EventManager.EventType.QUICK)
						{
							goto IL_1CB;
						}
					}
					if (j != 10 || !flag)
					{
						GameObject spawnableGameObject = ResourceManager.Instance.GetSpawnableGameObject(this.STOCK_DATA_TABLE[j].m_name);
						int stockCount = this.STOCK_DATA_TABLE[j].m_stockCount;
						for (int k = 0; k < stockCount; k++)
						{
							GameObject gameObject = UnityEngine.Object.Instantiate(spawnableGameObject, Vector3.zero, Quaternion.identity) as GameObject;
							if (gameObject != null)
							{
								gameObject.name = spawnableGameObject.name;
								SpawnableObject component = gameObject.GetComponent<SpawnableObject>();
								if (component != null)
								{
									this.AddSpawnableObject(component);
								}
							}
						}
					}
				}
				IL_1CB:;
			}
		}
	}

	public IEnumerator LoadSetTableFirst()
	{
		ObjectSpawnManager._LoadSetTableFirst_c__Iterator14 _LoadSetTableFirst_c__Iterator = new ObjectSpawnManager._LoadSetTableFirst_c__Iterator14();
		_LoadSetTableFirst_c__Iterator.__f__this = this;
		return _LoadSetTableFirst_c__Iterator;
	}

	public IEnumerator LoadSetTable(int firstBlock, int numBlock)
	{
		ObjectSpawnManager._LoadSetTable_c__Iterator15 _LoadSetTable_c__Iterator = new ObjectSpawnManager._LoadSetTable_c__Iterator15();
		_LoadSetTable_c__Iterator.firstBlock = firstBlock;
		_LoadSetTable_c__Iterator.numBlock = numBlock;
		_LoadSetTable_c__Iterator.___firstBlock = firstBlock;
		_LoadSetTable_c__Iterator.___numBlock = numBlock;
		_LoadSetTable_c__Iterator.__f__this = this;
		return _LoadSetTable_c__Iterator;
	}

	public IEnumerator LoadSetTable(int[] blockTable, int readFileCount)
	{
		ObjectSpawnManager._LoadSetTable_c__Iterator16 _LoadSetTable_c__Iterator = new ObjectSpawnManager._LoadSetTable_c__Iterator16();
		_LoadSetTable_c__Iterator.blockTable = blockTable;
		_LoadSetTable_c__Iterator.readFileCount = readFileCount;
		_LoadSetTable_c__Iterator.___blockTable = blockTable;
		_LoadSetTable_c__Iterator.___readFileCount = readFileCount;
		_LoadSetTable_c__Iterator.__f__this = this;
		return _LoadSetTable_c__Iterator;
	}

	public bool IsDataLoaded()
	{
		return this.m_setDataLoaded;
	}

	public void RegisterOnlyOneObject(SpawnableInfo info)
	{
		if (info != null)
		{
			this.m_onlyOneObjectName.Add(info.m_parameters.ObjectName);
		}
	}

	private IEnumerator LoadSetTable(int blockIndex)
	{
		ObjectSpawnManager._LoadSetTable_c__Iterator17 _LoadSetTable_c__Iterator = new ObjectSpawnManager._LoadSetTable_c__Iterator17();
		_LoadSetTable_c__Iterator.blockIndex = blockIndex;
		_LoadSetTable_c__Iterator.___blockIndex = blockIndex;
		_LoadSetTable_c__Iterator.__f__this = this;
		return _LoadSetTable_c__Iterator;
	}

	private void LoadSetTable()
	{
		GameObject gameObject = this.m_resourceManager.GetGameObject(ResourceCategory.TERRAIN_MODEL, TerrainXmlData.DataAssetName);
		if (gameObject)
		{
			TerrainXmlData component = gameObject.GetComponent<TerrainXmlData>();
			if (component)
			{
				TextAsset[] setData = component.SetData;
				if (setData != null)
				{
					GameObject gameObject2 = new GameObject();
					SpawnableParser spawnableParser = gameObject2.AddComponent<SpawnableParser>();
					spawnableParser.CreateSetData(this.m_resourceManager, setData[0], this.m_stageSetData);
					UnityEngine.Object.Destroy(gameObject2);
				}
			}
		}
	}

	public void OnActivateBlock(MsgActivateBlock msg)
	{
		if (msg != null)
		{
			this.ActivateBlock(msg.m_blockNo, msg.m_layerNo, msg.m_activateID, msg.m_originPosition, msg.m_originRotation);
			if (msg.m_checkPoint != MsgActivateBlock.CheckPoint.None)
			{
				ObjectSpawnManager.CheckPointInfo checkPointInfo = new ObjectSpawnManager.CheckPointInfo();
				checkPointInfo.m_activateID = msg.m_activateID;
				checkPointInfo.m_onReplace = (msg.m_checkPoint == MsgActivateBlock.CheckPoint.First);
				this.m_checkPointInfos.Add(checkPointInfo);
			}
			if (msg.m_replaceStage)
			{
				this.CheckRangeIn(msg.m_originPosition.x);
			}
		}
	}

	private void OnDeactivateBlock(MsgDeactivateBlock msg)
	{
		if (msg != null)
		{
			this.DeactivateBlock(msg.m_activateID, true);
		}
	}

	private void OnDeactivateAllBlock(MsgDeactivateAllBlock msg)
	{
		foreach (SpawnableInfo current in this.m_spawnableInfoList)
		{
			this.DestroyObject(current);
		}
		this.m_spawnableInfoList.Clear();
		this.m_onlyOneObjectName.Clear();
	}

	private void OnChangeCurerntBlock(MsgChangeCurrentBlock msg)
	{
	}

	private bool CheckAndActivePointMarker(SpawnableObject createdObject, ObjectSpawnManager.CheckPointInfo info)
	{
		if (createdObject && createdObject.name.Contains("ObjPointMarker"))
		{
			MsgActivePointMarker msgActivePointMarker = new MsgActivePointMarker((!info.m_onReplace) ? PointMarkerType.OrderBlock : PointMarkerType.BossEnd);
			createdObject.SendMessage("OnActivePointMarker", msgActivePointMarker, SendMessageOptions.DontRequireReceiver);
			if (msgActivePointMarker.m_activated)
			{
				this.m_checkPointInfos.Remove(info);
			}
			return true;
		}
		return false;
	}

	private void ActivateBlock(int block, int layer, int activateID, Vector3 originPoint, Quaternion originRotation)
	{
		Vector3 position = base.transform.position;
		Quaternion rotation = base.transform.rotation;
		base.transform.position = originPoint;
		base.transform.rotation = originRotation;
		BlockSpawnableParameterContainer blockData = this.m_stageSetData.GetBlockData(block, layer);
		if (blockData != null)
		{
			foreach (SpawnableParameter current in blockData.GetParameters())
			{
				SpawnableInfo spawnableInfo = new SpawnableInfo();
				spawnableInfo.m_block = block;
				spawnableInfo.m_blockActivateID = activateID;
				spawnableInfo.m_parameters = current;
				spawnableInfo.m_position = base.transform.TransformPoint(current.Position);
				spawnableInfo.m_rotation = rotation * current.Rotation;
				spawnableInfo.m_manager = this;
				this.m_spawnableInfoList.Add(spawnableInfo);
			}
		}
		base.transform.position = position;
		base.transform.rotation = rotation;
		GC.Collect();
	}

	private void DeactivateBlock(int blockActivateID, bool ignoreNotRangeOut)
	{
		int i = this.m_spawnableInfoList.Count - 1;
		while (i >= 0)
		{
			SpawnableInfo spawnableInfo = this.m_spawnableInfoList[i];
			if (spawnableInfo.m_blockActivateID != blockActivateID || !spawnableInfo.m_object)
			{
				goto IL_71;
			}
			if (!spawnableInfo.NotRangeOut || !ignoreNotRangeOut)
			{
				this.DestroyObject(spawnableInfo);
				this.m_spawnableInfoList.Remove(this.m_spawnableInfoList[i]);
				goto IL_71;
			}
			IL_C5:
			i--;
			continue;
			IL_71:
			foreach (ObjectSpawnManager.CheckPointInfo current in this.m_checkPointInfos)
			{
				if (current.m_activateID == blockActivateID)
				{
					this.m_checkPointInfos.Remove(current);
					break;
				}
			}
			goto IL_C5;
		}
	}

	public SpawnableObject GetSpawnableObject(StockObjectType type)
	{
		if (this.m_dicSpawnableObjs.ContainsKey(type))
		{
			return this.m_dicSpawnableObjs[type].Get();
		}
		return null;
	}

	private void AddSpawnableObject(SpawnableObject spawnableObject)
	{
		if (spawnableObject != null && spawnableObject.IsStockObject())
		{
			spawnableObject.AttachModelObject();
			StockObjectType stockObjectType = spawnableObject.GetStockObjectType();
			if (this.m_dicSpawnableObjs.ContainsKey(stockObjectType))
			{
				this.m_dicSpawnableObjs[stockObjectType].Add(spawnableObject);
				this.m_dicSpawnableObjs[stockObjectType].Sleep(spawnableObject);
			}
		}
	}

	public void SleepSpawnableObject(SpawnableObject spawnableObject)
	{
		if (spawnableObject != null && spawnableObject.Share)
		{
			StockObjectType stockObjectType = spawnableObject.GetStockObjectType();
			if (this.m_dicSpawnableObjs.ContainsKey(stockObjectType))
			{
				this.m_dicSpawnableObjs[stockObjectType].Sleep(spawnableObject);
			}
		}
	}

	private bool CreateObject(SpawnableInfo info)
	{
		if (this.m_onlyOneObjectName.IndexOf(info.m_parameters.ObjectName) != -1)
		{
			return false;
		}
		GameObject gameObject = ObjUtil.GetChangeObject(this.m_resourceManager, this.m_playerInformation, this.m_levelInformation, info.m_parameters.ObjectName);
		if (gameObject == null)
		{
			gameObject = this.m_resourceManager.GetSpawnableGameObject(info.m_parameters.ObjectName);
		}
		if (gameObject != null)
		{
			SpawnableObject component = gameObject.GetComponent<SpawnableObject>();
			if (component != null && !component.IsValid())
			{
				return false;
			}
		}
		SpawnableObject spawnableObject = this.GetReviveSpawnableObject(gameObject);
		if (spawnableObject != null)
		{
			spawnableObject.Sleep = false;
			spawnableObject.gameObject.transform.position = info.m_position;
			spawnableObject.gameObject.transform.rotation = info.m_rotation;
			spawnableObject.gameObject.SetActive(true);
			spawnableObject.gameObject.transform.parent = this.m_stock.transform;
			info.SpawnedObject(spawnableObject);
			spawnableObject.AttachSpawnableInfo(info);
			SpawnableBehavior component2 = spawnableObject.gameObject.GetComponent<SpawnableBehavior>();
			if (component2)
			{
				component2.SetParameters(info.m_parameters);
			}
			spawnableObject.OnRevival();
			return true;
		}
		if (gameObject != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, info.m_position, info.m_rotation) as GameObject;
			if (gameObject2)
			{
				gameObject2.gameObject.SetActive(true);
				spawnableObject = gameObject2.GetComponent<SpawnableObject>();
				if (spawnableObject != null)
				{
					info.SpawnedObject(spawnableObject);
					spawnableObject.AttachSpawnableInfo(info);
				}
				SpawnableBehavior component3 = gameObject2.GetComponent<SpawnableBehavior>();
				if (component3)
				{
					component3.SetParameters(info.m_parameters);
				}
				if (spawnableObject != null)
				{
					spawnableObject.AttachModelObject();
					spawnableObject.OnCreate();
				}
				ObjectSpawnManager.CheckPointInfo checkPointInfo = null;
				if (this.IsCreatePointmarkerBlock(info.m_blockActivateID, ref checkPointInfo) && checkPointInfo != null)
				{
					this.CheckAndActivePointMarker(spawnableObject, checkPointInfo);
				}
				return true;
			}
		}
		return false;
	}

	private SpawnableObject GetReviveSpawnableObject(GameObject srcObj)
	{
		if (srcObj == null)
		{
			return null;
		}
		SpawnableObject component = srcObj.GetComponent<SpawnableObject>();
		if (component == null)
		{
			return null;
		}
		if (component.IsStockObject())
		{
			return this.GetSpawnableObject(component.GetStockObjectType());
		}
		return null;
	}

	private void DestroyObject(SpawnableInfo info)
	{
		if (info.Spawned)
		{
			if (info.m_object.Share)
			{
				info.m_object.SetSleep();
			}
			else
			{
				UnityEngine.Object.Destroy(info.m_object.gameObject);
			}
			info.DestroyedObject();
		}
	}

	public void DetachObject(SpawnableInfo info)
	{
		info.RequestDestroy = true;
		info.DestroyedObject();
		this.m_spawnableInfoList.Remove(info);
	}

	public void DetachInfoList(SpawnableInfo info)
	{
		this.m_spawnableInfoList.Remove(info);
	}

	private void CheckRangeIn(float basePosition)
	{
		List<SpawnableInfo> list = null;
		foreach (SpawnableInfo current in this.m_spawnableInfoList)
		{
			if (!current.Spawned && !current.Destroyed)
			{
				float x = current.m_position.x;
				float num = x - basePosition;
				if (x - basePosition < current.m_parameters.RangeIn && !this.CreateObject(current))
				{
					if (list == null)
					{
						new List<SpawnableInfo>();
					}
					if (list != null)
					{
						list.Add(current);
					}
				}
				if (num > 200f)
				{
					break;
				}
			}
		}
		if (list != null)
		{
			foreach (SpawnableInfo current2 in list)
			{
				this.m_spawnableInfoList.Remove(current2);
			}
		}
	}

	private void CheckRangeOut(float basePosition)
	{
		List<SpawnableInfo> list = null;
		foreach (SpawnableInfo current in this.m_spawnableInfoList)
		{
			if (current.Spawned)
			{
				if (!current.NotRangeOut)
				{
					float num = basePosition - current.m_position.x;
					if (num < 0f)
					{
						break;
					}
					if (num > current.m_parameters.RangeOut)
					{
						if (list == null)
						{
							new List<SpawnableInfo>();
						}
						if (list != null)
						{
							list.Add(current);
						}
						this.DestroyObject(current);
					}
				}
			}
		}
		if (list != null)
		{
			foreach (SpawnableInfo current2 in list)
			{
				this.m_spawnableInfoList.Remove(current2);
			}
		}
	}

	private void DrawObjectInfo(string infoname)
	{
		global::Debug.Log(infoname + "Start");
		foreach (SpawnableInfo current in this.m_spawnableInfoList)
		{
			string text = current.m_parameters.ObjectName + ":";
			text += ((!current.Spawned) ? "NotSpawned" : "Spawned ");
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"pos ",
				current.m_position.x.ToString("F2"),
				" ",
				current.m_position.y.ToString("F2"),
				" ",
				current.m_position.z.ToString("F2")
			});
			global::Debug.Log(text + "\n");
		}
		global::Debug.Log(infoname + "End");
	}

	private bool IsCreatePointmarkerBlock(int activateID, ref ObjectSpawnManager.CheckPointInfo retInfo)
	{
		foreach (ObjectSpawnManager.CheckPointInfo current in this.m_checkPointInfos)
		{
			if (current.m_activateID == activateID)
			{
				retInfo = current;
				return true;
			}
		}
		return false;
	}
}
