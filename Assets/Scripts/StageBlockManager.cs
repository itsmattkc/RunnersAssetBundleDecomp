using App;
using Message;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Scripts/Runners/Game/Level/StageBlockManager")]
[Serializable]
public class StageBlockManager : MonoBehaviour
{
	public class StageBlockInfo
	{
		private int _m_activateNo_k__BackingField;

		private int _m_blockNo_k__BackingField;

		private int _m_layerNo_k__BackingField;

		private float _m_totalLength_k__BackingField;

		private Vector3 _m_startPoint_k__BackingField;

		private Vector3 _m_endPoint_k__BackingField;

		private StageBlockManager.OrderType _m_orderType_k__BackingField;

		private int _m_orderNum_k__BackingField;

		private StageBlockManager.Callback _startCurrentCallback_k__BackingField;

		private StageBlockManager.Callback _endCurrentCallback_k__BackingField;

		public int m_activateNo
		{
			get;
			set;
		}

		public int m_blockNo
		{
			get;
			set;
		}

		public int m_layerNo
		{
			get;
			set;
		}

		public float m_totalLength
		{
			get;
			set;
		}

		public Vector3 m_startPoint
		{
			get;
			set;
		}

		public Vector3 m_endPoint
		{
			get;
			set;
		}

		public StageBlockManager.OrderType m_orderType
		{
			get;
			set;
		}

		public int m_orderNum
		{
			get;
			set;
		}

		public StageBlockManager.Callback startCurrentCallback
		{
			get;
			set;
		}

		public StageBlockManager.Callback endCurrentCallback
		{
			get;
			set;
		}

		public float GetBlockRunningLength(float totalLength)
		{
			return totalLength - this.m_startPoint.x;
		}

		public float GetPastDistanceFromEndPoint(float pos)
		{
			return pos - this.m_endPoint.x;
		}

		public bool IsNearToGoal(float nowLength, float remainLength)
		{
			float num = this.m_endPoint.x - nowLength;
			return remainLength > num;
		}
	}

	private class BlockOrderInfo
	{
		public int blockNo;

		public bool fixedLayerNo;

		public int layerNo;

		public int rndLayerNo;

		public bool isBoss;

		public StageBlockManager.Callback startCurrentCallback;

		public StageBlockManager.Callback endCurrentCallback;

		public void SetBlockAndFixedLayer(int block, int layer)
		{
			this.blockNo = block;
			this.fixedLayerNo = true;
			this.layerNo = layer;
		}

		public void SetBlockAndRandomLayer(int block, int rndNum)
		{
			this.blockNo = block;
			this.fixedLayerNo = false;
			this.rndLayerNo = rndNum;
		}

		public int GetNewLayerNo()
		{
			if (this.fixedLayerNo)
			{
				return this.layerNo;
			}
			return UnityEngine.Random.Range(0, this.rndLayerNo);
		}
	}

	[Serializable]
	public class BlockLevelData
	{
		public int numBlock = 5;

		public int numPlacement = 5;

		public int layerNum = 1;

		public int repeatNum = 1;

		public int fixedLayerNo = -1;

		public void CopyTo(StageBlockManager.BlockLevelData dst)
		{
			dst.numBlock = this.numBlock;
			dst.numPlacement = this.numPlacement;
			dst.layerNum = this.layerNum;
			dst.repeatNum = this.repeatNum;
			dst.fixedLayerNo = this.fixedLayerNo;
		}
	}

	[Serializable]
	public class StartingBlockInfo
	{
		public int fixedBlockNo = 2;

		public int startingLayerNum = 1;
	}

	[Serializable]
	public class DebugBlockInfo
	{
		public int block;

		public int layer;
	}

	public enum FixedLayerNumber
	{
		SYSTEM,
		FEVER_BOSS_1,
		FEVER_BOSS_2,
		FEVER_BOSS_3,
		MAP_BOSS_1 = 11,
		MAP_BOSS_2,
		MAP_BOSS_3,
		EVENT_BOSS_1 = 21,
		EVENT_BOSS_2,
		EVENT_BOSS_3
	}

	public struct CreateInfo
	{
		public string stageName;

		public bool isSpawnableManager;

		public bool isTerrainManager;

		public bool isPathBlockManager;

		public PathManager pathManager;

		public bool showInfo;

		public bool randomBlock;

		public bool bossMode;

		public bool quickMode;
	}

	public enum OrderType
	{
		BOSS_SINGLE,
		ASCENDING,
		RANDOM,
		TUTORIAL,
		DEBUG
	}

	public delegate void Callback(StageBlockManager.StageBlockInfo blockInfo);

	private const int BLOCK_LEVEL_DATA_NUM = 5;

	private const int HIGH_SPEED_FILE_MAX_COUNT = 6;

	private const int HIGH_SPEED_FILE_ONCE_READ_LIMIT = 2;

	private const int DefaultLayerNo = 0;

	private const float DistanceOfActivateNext = 150f;

	private const float DistanceOfDeactivate = 150f;

	private const float DistanceOfChangeCurrent = 1f;

	public const int BossSingleBlockNo = 0;

	public const int TutorialBlockNo = 91;

	public const int StartActBlockNo = 92;

	[SerializeField]
	public StageBlockManager.DebugBlockInfo[] m_debugBlockInfo;

	private float m_totalDistance;

	private int m_numCreateBlock;

	private PlayerInformation m_playerInformation;

	private string m_stageName;

	private bool m_showStageInfo;

	private Rect m_window;

	private Dictionary<int, float> m_terrainPlacement;

	private Dictionary<int, StageBlockManager.StageBlockInfo> m_activeBlockInfo;

	private StageBlockManager.StageBlockInfo m_currentBlock;

	private StageBlockManager.OrderType m_orderType;

	private StageBlockManager.OrderType m_firstOrderType;

	private StageBlockManager.BlockOrderInfo[] m_blockOrder;

	private int m_currentOrderNum;

	private int m_nowBlockOrderNum;

	private int m_stageRepeatNum;

	private ObjectSpawnManager m_objectSpawnableManager;

	private TerrainPlacementManager m_terrainPlacementManager;

	private StageBlockPathManager m_stagePathManager;

	private LevelInformation m_levelInformation;

	private int[] m_highSpeedTable = new int[]
	{
		21,
		26,
		31,
		36,
		41,
		46
	};

	private int m_highSpeedCount = 1;

	private bool m_highSpeedSetFlag;

	[SerializeField]
	private StageBlockManager.BlockLevelData[] m_blockLevelData = new StageBlockManager.BlockLevelData[5];

	[SerializeField]
	private StageBlockManager.BlockLevelData[] m_blockLevelDataForQuickMode = new StageBlockManager.BlockLevelData[5];

	private StageBlockManager.BlockLevelData[] m_currentBlockLevelData = new StageBlockManager.BlockLevelData[5];

	private int m_nowBlockLevelNum;

	private int m_nextBlockLevelNum;

	[SerializeField]
	private int m_apeearCheckPointNumber = 3;

	[SerializeField]
	private StageBlockManager.StartingBlockInfo m_startingBlockInfo = new StageBlockManager.StartingBlockInfo();

	[SerializeField]
	private bool m_useDebugOrder;

	private int[] m_firstBlockNoOnLevel;

	private bool m_nextBossBlock;

	private bool m_quickMode;

	private bool m_quickFirstReplaceEnd;

	private StageBlockManager.StageBlockInfo CurrentBlock
	{
		get
		{
			return this.m_currentBlock;
		}
		set
		{
			this.m_currentBlock = value;
		}
	}

	private StageBlockManager.StageBlockInfo NextBlock
	{
		get
		{
			StageBlockManager.StageBlockInfo currentBlock = this.CurrentBlock;
			if (currentBlock == null)
			{
				return null;
			}
			StageBlockManager.StageBlockInfo result = null;
			if (this.m_activeBlockInfo.TryGetValue(currentBlock.m_activateNo + 1, out result))
			{
				return result;
			}
			return null;
		}
	}

	public string StageName
	{
		get
		{
			return this.m_stageName;
		}
	}

	private void Start()
	{
		this.m_showStageInfo = false;
		App.Random.ShuffleInt(this.m_highSpeedTable);
	}

	private void Update()
	{
		if (this.m_playerInformation != null)
		{
			if (this.m_playerInformation.Position.x <= base.transform.position.x)
			{
				return;
			}
			base.transform.position.Set(this.m_playerInformation.Position.x, base.transform.position.y, base.transform.position.z);
			this.m_totalDistance = base.transform.position.x;
			if (this.m_levelInformation != null)
			{
				this.m_levelInformation.DistanceOnStage = this.m_totalDistance;
			}
		}
		this.CheckNextBlock();
	}

	private void CheckNextBlock()
	{
		if (this.CurrentBlock != null)
		{
			if (this.NextBlock == null)
			{
				if (this.CurrentBlock.IsNearToGoal(this.m_totalDistance, 150f))
				{
					StageBlockManager.BlockOrderInfo nextActivateBlockInfo = this.GetNextActivateBlockInfo();
					if (nextActivateBlockInfo != null)
					{
						Vector3 endPoint = this.CurrentBlock.m_endPoint;
						MsgActivateBlock.CheckPoint checkPoint = MsgActivateBlock.CheckPoint.None;
						if (this.m_orderType == StageBlockManager.OrderType.DEBUG || this.m_orderType == StageBlockManager.OrderType.RANDOM || this.m_orderType == StageBlockManager.OrderType.ASCENDING)
						{
							if (this.m_currentOrderNum == 1 && this.m_playerInformation != null && this.m_playerInformation.SpeedLevel > PlayerSpeed.LEVEL_1)
							{
								checkPoint = MsgActivateBlock.CheckPoint.First;
							}
							else if (this.m_currentOrderNum > 1 && this.m_currentOrderNum % this.m_apeearCheckPointNumber == 1)
							{
								checkPoint = MsgActivateBlock.CheckPoint.Internal;
							}
						}
						this.ActivateBlock(nextActivateBlockInfo, endPoint, false, this.m_orderType, this.m_currentOrderNum, checkPoint);
					}
				}
			}
			else
			{
				StageBlockManager.StageBlockInfo nextBlock = this.NextBlock;
				if (this.CurrentBlock.IsNearToGoal(this.m_totalDistance, 1f) && nextBlock != null)
				{
					this.ChangeCurrentBlock(nextBlock);
					if (this.m_nextBossBlock)
					{
						MsgBossStart value = new MsgBossStart();
						GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnSendToGameModeStage", value, SendMessageOptions.DontRequireReceiver);
						this.m_nextBossBlock = false;
					}
				}
			}
		}
		this.SearchAndDeleteRangeOutedBlock(this.m_totalDistance);
	}

	public void Initialize(StageBlockManager.CreateInfo cinfo)
	{
		this.m_quickMode = cinfo.quickMode;
		this.m_stageName = cinfo.stageName;
		this.m_totalDistance = 0f;
		GameObject gameObject = GameObject.Find("PlayerInformation");
		if (gameObject != null && this.m_playerInformation == null)
		{
			this.m_playerInformation = gameObject.GetComponent<PlayerInformation>();
		}
		if (this.m_levelInformation == null)
		{
			this.m_levelInformation = GameObjectUtil.FindGameObjectComponent<LevelInformation>("LevelInformation");
		}
		for (int i = 0; i < this.m_currentBlockLevelData.Length; i++)
		{
			this.m_currentBlockLevelData[i] = new StageBlockManager.BlockLevelData();
			if (this.m_quickMode)
			{
				if (i < this.m_blockLevelDataForQuickMode.Length)
				{
					this.m_blockLevelDataForQuickMode[i].CopyTo(this.m_currentBlockLevelData[i]);
				}
			}
			else if (i < this.m_blockLevelData.Length)
			{
				this.m_blockLevelData[i].CopyTo(this.m_currentBlockLevelData[i]);
			}
		}
		if (this.m_terrainPlacement == null)
		{
			this.m_terrainPlacement = new Dictionary<int, float>();
		}
		if (this.m_activeBlockInfo == null)
		{
			this.m_activeBlockInfo = new Dictionary<int, StageBlockManager.StageBlockInfo>();
		}
		if (cinfo.isSpawnableManager)
		{
			this.m_objectSpawnableManager = base.gameObject.AddComponent<ObjectSpawnManager>();
			this.m_objectSpawnableManager.enabled = false;
		}
		if (cinfo.isTerrainManager)
		{
			this.m_terrainPlacementManager = base.gameObject.AddComponent<TerrainPlacementManager>();
			this.m_terrainPlacementManager.enabled = false;
		}
		if (cinfo.isPathBlockManager && cinfo.pathManager)
		{
			this.m_stagePathManager = base.gameObject.AddComponent<StageBlockPathManager>();
			this.m_stagePathManager.SetPathManager(cinfo.pathManager);
			this.m_stagePathManager.enabled = false;
		}
		this.m_showStageInfo = cinfo.showInfo;
		if (cinfo.bossMode)
		{
			this.m_orderType = StageBlockManager.OrderType.BOSS_SINGLE;
		}
		else
		{
			this.m_orderType = ((!cinfo.randomBlock) ? StageBlockManager.OrderType.ASCENDING : StageBlockManager.OrderType.RANDOM);
		}
		this.m_firstOrderType = this.m_orderType;
		this.m_currentOrderNum = 0;
		this.m_stageRepeatNum = 0;
		if (this.m_levelInformation != null)
		{
			for (int j = 0; j < this.m_currentBlockLevelData.Length; j++)
			{
				if (this.m_currentBlockLevelData[j].repeatNum > 0)
				{
					this.m_nowBlockLevelNum = j;
					break;
				}
			}
			this.m_nextBlockLevelNum = this.m_nowBlockLevelNum;
			this.m_levelInformation.FeverBossCount = this.m_nowBlockLevelNum;
		}
		if (this.m_orderType != StageBlockManager.OrderType.DEBUG)
		{
			this.m_firstBlockNoOnLevel = new int[this.m_currentBlockLevelData.Length];
			int num = 1;
			for (int k = 0; k < this.m_firstBlockNoOnLevel.Length; k++)
			{
				if (this.m_currentBlockLevelData[k] == null)
				{
					this.m_currentBlockLevelData[k] = new StageBlockManager.BlockLevelData();
				}
				this.m_firstBlockNoOnLevel[k] = num;
				if (k < 2)
				{
					num += this.m_currentBlockLevelData[k].numBlock;
				}
			}
		}
	}

	public void Setup(bool bossStage)
	{
		if (this.m_objectSpawnableManager != null)
		{
			this.m_objectSpawnableManager.Setup(bossStage);
		}
		if (this.m_terrainPlacementManager != null)
		{
			this.m_terrainPlacementManager.Setup(bossStage);
			this.m_terrainPlacementManager.enabled = true;
		}
		if (this.m_stagePathManager != null)
		{
			this.m_stagePathManager.Setup();
			this.m_stagePathManager.enabled = true;
		}
	}

	public bool IsSetupEnded()
	{
		return !this.m_objectSpawnableManager || this.m_objectSpawnableManager.IsDataLoaded();
	}

	public void PauseTerrainPlacement(bool value)
	{
		if (this.m_terrainPlacementManager)
		{
			this.m_terrainPlacementManager.enabled = !value;
		}
	}

	public void AddTerrainInfo(int terrainIndex, float terrainLength)
	{
		if (this.m_terrainPlacement.ContainsKey(terrainIndex))
		{
			return;
		}
		this.m_terrainPlacement.Add(terrainIndex, terrainLength);
	}

	public void DeactivateAll()
	{
		MsgDeactivateAllBlock value = new MsgDeactivateAllBlock();
		base.gameObject.SendMessage("OnDeactivateAllBlock", value, SendMessageOptions.DontRequireReceiver);
		this.m_activeBlockInfo.Clear();
		this.m_numCreateBlock = 0;
		this.m_currentBlock = null;
	}

	public void ReCreateTerrain()
	{
		if (this.m_terrainPlacementManager != null)
		{
			this.m_terrainPlacementManager.ReCreateTerrain();
		}
	}

	private void ActiveFirstBlock(Vector3 position, Quaternion rotation, bool isGameStartBlock, bool insertStartAct)
	{
		switch (this.m_orderType)
		{
		case StageBlockManager.OrderType.BOSS_SINGLE:
			goto IL_64;
		case StageBlockManager.OrderType.TUTORIAL:
			goto IL_64;
		case StageBlockManager.OrderType.DEBUG:
			this.m_blockOrder = null;
			goto IL_64;
		}
		this.m_blockOrder = null;
		this.MakeOrderTable(this.m_nowBlockLevelNum, this.m_firstBlockNoOnLevel[this.m_nowBlockLevelNum], isGameStartBlock, insertStartAct);
		IL_64:
		if (this.m_blockOrder != null && this.m_blockOrder.Length > 0)
		{
			this.m_currentOrderNum = 0;
			StageBlockManager.StageBlockInfo stageBlockInfo = this.ActivateBlock(this.m_blockOrder[this.m_currentOrderNum], position, true, this.m_orderType, this.m_currentOrderNum, MsgActivateBlock.CheckPoint.None);
			if (stageBlockInfo != null)
			{
				this.ChangeCurrentBlock(stageBlockInfo);
			}
		}
	}

	private void SetOrderMode(StageBlockManager.OrderType type)
	{
		this.m_orderType = type;
	}

	private void SetOrderModeToDefault()
	{
		this.m_orderType = this.m_firstOrderType;
	}

	private void ChangeNextOrderModeToFeverBoss()
	{
		this.SetOrderMode(StageBlockManager.OrderType.BOSS_SINGLE);
		this.CreateBlockOrderInfo(1);
		int layer = Mathf.Min(1 + this.m_nowBlockLevelNum, 3);
		this.m_blockOrder[0].SetBlockAndFixedLayer(0, layer);
		this.m_blockOrder[0].isBoss = true;
		this.m_currentOrderNum = 0;
		this.m_nextBossBlock = true;
	}

	private void ResetLevelInformationOnReplace()
	{
		if (this.m_levelInformation != null && this.m_blockOrder != null)
		{
			float num = 0f;
			StageBlockManager.BlockOrderInfo[] blockOrder = this.m_blockOrder;
			for (int i = 0; i < blockOrder.Length; i++)
			{
				StageBlockManager.BlockOrderInfo blockOrderInfo = blockOrder[i];
				num += this.GetTerrainLength(blockOrderInfo.blockNo);
			}
			this.m_levelInformation.DistanceToBossOnStart = num;
			this.m_levelInformation.DistanceOnStage = 0f;
		}
	}

	private StageBlockManager.StageBlockInfo ActivateBlock(StageBlockManager.BlockOrderInfo orderInfo, Vector3 originPoint, bool replaceStage, StageBlockManager.OrderType orderType, int orderNum, MsgActivateBlock.CheckPoint checkPoint)
	{
		if (orderInfo == null)
		{
			return null;
		}
		float terrainLength = this.GetTerrainLength(orderInfo.blockNo);
		if (App.Math.NearZero(terrainLength, 1E-06f))
		{
			return null;
		}
		StageBlockManager.StageBlockInfo stageBlockInfo = new StageBlockManager.StageBlockInfo();
		this.m_numCreateBlock++;
		stageBlockInfo.m_activateNo = this.m_numCreateBlock;
		stageBlockInfo.m_blockNo = orderInfo.blockNo;
		stageBlockInfo.m_layerNo = orderInfo.GetNewLayerNo();
		stageBlockInfo.m_totalLength = terrainLength;
		stageBlockInfo.m_startPoint = originPoint;
		stageBlockInfo.m_endPoint = originPoint + new Vector3(terrainLength, 0f, 0f);
		stageBlockInfo.startCurrentCallback = orderInfo.startCurrentCallback;
		stageBlockInfo.endCurrentCallback = orderInfo.endCurrentCallback;
		stageBlockInfo.m_orderNum = orderNum;
		stageBlockInfo.m_orderType = orderType;
		this.m_activeBlockInfo.Add(stageBlockInfo.m_activateNo, stageBlockInfo);
		MsgActivateBlock msgActivateBlock = new MsgActivateBlock(this.m_stageName, stageBlockInfo.m_blockNo, stageBlockInfo.m_layerNo, stageBlockInfo.m_activateNo, originPoint, Quaternion.identity);
		msgActivateBlock.m_replaceStage = replaceStage;
		msgActivateBlock.m_checkPoint = checkPoint;
		ObjectSpawnManager component = base.gameObject.GetComponent<ObjectSpawnManager>();
		if (component != null)
		{
			component.OnActivateBlock(msgActivateBlock);
		}
		TerrainPlacementManager component2 = base.gameObject.GetComponent<TerrainPlacementManager>();
		if (component2 != null)
		{
			component2.OnActivateBlock(msgActivateBlock);
		}
		StageBlockPathManager component3 = base.gameObject.GetComponent<StageBlockPathManager>();
		if (component3 != null)
		{
			component3.OnActivateBlock(msgActivateBlock);
		}
		return stageBlockInfo;
	}

	private void DeactivateBlock(StageBlockManager.StageBlockInfo info, float pos)
	{
		MsgDeactivateBlock value = new MsgDeactivateBlock(info.m_blockNo, info.m_activateNo, pos);
		base.gameObject.SendMessage("OnDeactivateBlock", value, SendMessageOptions.DontRequireReceiver);
		this.m_activeBlockInfo.Remove(info.m_activateNo);
	}

	private void ChangeCurrentBlock(StageBlockManager.StageBlockInfo nextBlock)
	{
		if (this.CurrentBlock != null && this.CurrentBlock.endCurrentCallback != null)
		{
			this.CurrentBlock.endCurrentCallback(this.CurrentBlock);
		}
		this.CurrentBlock = nextBlock;
		if (this.CurrentBlock != null && this.CurrentBlock.startCurrentCallback != null)
		{
			this.CurrentBlock.startCurrentCallback(this.CurrentBlock);
		}
		MsgChangeCurrentBlock value = new MsgChangeCurrentBlock(this.CurrentBlock.m_blockNo, this.CurrentBlock.m_layerNo, this.CurrentBlock.m_activateNo);
		base.gameObject.SendMessage("OnChangeCurerntBlock", value, SendMessageOptions.DontRequireReceiver);
	}

	private void SearchAndDeleteRangeOutedBlock(float pos)
	{
		if (this.m_activeBlockInfo == null)
		{
			return;
		}
		List<StageBlockManager.StageBlockInfo> list = null;
		foreach (StageBlockManager.StageBlockInfo current in this.m_activeBlockInfo.Values)
		{
			if (current.GetPastDistanceFromEndPoint(pos) > 150f)
			{
				if (list == null)
				{
					list = new List<StageBlockManager.StageBlockInfo>();
				}
				list.Add(current);
			}
		}
		if (list == null)
		{
			return;
		}
		foreach (StageBlockManager.StageBlockInfo current2 in list)
		{
			this.DeactivateBlock(current2, pos);
		}
	}

	private float GetTerrainLength(int block)
	{
		float result = 0f;
		if (this.m_terrainPlacement.TryGetValue(block, out result))
		{
			return result;
		}
		return 0f;
	}

	private StageBlockManager.BlockOrderInfo GetNextActivateBlockInfo()
	{
		switch (this.m_orderType)
		{
		case StageBlockManager.OrderType.BOSS_SINGLE:
			global::Debug.Log("CheckNextBlock:BOSS_SINGLE  " + this.m_currentBlock.m_blockNo.ToString());
			this.m_currentOrderNum = 0;
			goto IL_C0;
		case StageBlockManager.OrderType.TUTORIAL:
			this.ChangeNextOrderModeToFeverBoss();
			goto IL_C0;
		}
		this.m_currentOrderNum++;
		if (this.m_currentOrderNum >= this.m_nowBlockOrderNum)
		{
			this.ChangeNextOrderModeToFeverBoss();
		}
		else
		{
			global::Debug.Log("CheckNextBlock " + this.m_currentOrderNum.ToString() + " " + this.m_blockOrder[this.m_currentOrderNum].blockNo.ToString());
		}
		IL_C0:
		if (this.m_currentOrderNum < this.m_blockOrder.Length)
		{
			return this.m_blockOrder[this.m_currentOrderNum];
		}
		return null;
	}

	private void MakeOrderTable(int numBlockLevelNum, int startBlock, bool isGameStartBlock, bool insertStartAct)
	{
		int num = this.m_currentBlockLevelData[numBlockLevelNum].numPlacement;
		int numBlock = this.m_currentBlockLevelData[numBlockLevelNum].numBlock;
		int num2 = startBlock + numBlock;
		int num3 = 0;
		if (insertStartAct)
		{
			num++;
		}
		this.CreateBlockOrderInfo(num);
		if (insertStartAct)
		{
			this.m_blockOrder[0].SetBlockAndFixedLayer(92, 0);
			StageBlockManager.BlockOrderInfo expr_54 = this.m_blockOrder[0];
			expr_54.startCurrentCallback = (StageBlockManager.Callback)Delegate.Combine(expr_54.startCurrentCallback, new StageBlockManager.Callback(this.CallbackOnStageStartAct));
			if (isGameStartBlock)
			{
				StageBlockManager.BlockOrderInfo expr_83 = this.m_blockOrder[0];
				expr_83.endCurrentCallback = (StageBlockManager.Callback)Delegate.Combine(expr_83.endCurrentCallback, new StageBlockManager.Callback(this.CallbackOnStageStartActEnd));
			}
			else
			{
				StageBlockManager.BlockOrderInfo expr_B1 = this.m_blockOrder[0];
				expr_B1.endCurrentCallback = (StageBlockManager.Callback)Delegate.Combine(expr_B1.endCurrentCallback, new StageBlockManager.Callback(this.CallbackOnStageReplaceActEnd));
			}
			num3 = 1;
		}
		StageBlockManager.OrderType orderType = this.m_orderType;
		if (orderType != StageBlockManager.OrderType.ASCENDING)
		{
			if (orderType == StageBlockManager.OrderType.RANDOM)
			{
				if (this.m_highSpeedSetFlag)
				{
					this.MakeOrderHighSpeedTable(numBlockLevelNum, startBlock, num3, isGameStartBlock, insertStartAct);
				}
				else
				{
					this.MakeOrderLowSpeedTable(numBlockLevelNum, startBlock, num3, isGameStartBlock, insertStartAct);
				}
			}
		}
		else
		{
			int num4 = startBlock;
			for (int i = num3; i < this.m_blockOrder.Length; i++)
			{
				this.m_blockOrder[i].SetBlockAndFixedLayer(num4, this.m_currentBlockLevelData[this.m_nowBlockLevelNum].layerNum);
				if (++num4 >= num2)
				{
					num4 = startBlock;
				}
			}
		}
		this.m_currentOrderNum = 0;
		this.m_nowBlockOrderNum = num;
	}

	private void MakeOrderHighSpeedTable(int numBlockLevelNum, int startBlock, int startOrderNum, bool isGameStartBlock, bool insertStartAct)
	{
		int numPlacement = this.m_currentBlockLevelData[numBlockLevelNum].numPlacement;
		int numBlock = this.m_currentBlockLevelData[numBlockLevelNum].numBlock;
		int num = startBlock + numBlock;
		int num2 = Mathf.Min(this.m_highSpeedCount * 2, 6);
		int num3 = Mathf.Min(num2 * 5, numBlock);
		int[] array = new int[num3];
		int num4 = 0;
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				array[num4] = this.m_highSpeedTable[i] + j;
				if (array[num4] >= num)
				{
					array[num4] = startBlock;
				}
				num4++;
				if (num4 >= num3)
				{
					num4 = num3 - 1;
				}
			}
		}
		App.Random.ShuffleInt(array);
		int num5 = 0;
		if (insertStartAct)
		{
			num5++;
		}
		int num6 = 0;
		while (num6 < array.Length && num5 < this.m_blockOrder.Length)
		{
			int fixedLayerNo = this.m_currentBlockLevelData[this.m_nowBlockLevelNum].fixedLayerNo;
			if (fixedLayerNo == -1)
			{
				int layerNum = this.m_currentBlockLevelData[this.m_nowBlockLevelNum].layerNum;
				this.m_blockOrder[num5].SetBlockAndRandomLayer(array[num6], layerNum);
			}
			else
			{
				this.m_blockOrder[num5].SetBlockAndFixedLayer(array[num6], fixedLayerNo);
			}
			num5++;
			num6++;
		}
		this.m_highSpeedCount++;
	}

	private void MakeOrderLowSpeedTable(int numBlockLevelNum, int startBlock, int startOrderNum, bool isGameStartBlock, bool insertStartAct)
	{
		int numPlacement = this.m_currentBlockLevelData[numBlockLevelNum].numPlacement;
		int numBlock = this.m_currentBlockLevelData[numBlockLevelNum].numBlock;
		int num = startBlock + numBlock;
		int num2 = (numBlock <= numPlacement) ? numPlacement : numBlock;
		int num3 = startBlock;
		int fixedBlockNo = this.m_startingBlockInfo.fixedBlockNo;
		if (isGameStartBlock && !this.m_quickMode)
		{
			int num4 = startOrderNum;
			for (int i = 0; i < fixedBlockNo; i++)
			{
				int startingLayerNum = this.m_startingBlockInfo.startingLayerNum;
				this.m_blockOrder[num4].SetBlockAndRandomLayer(startBlock + i, startingLayerNum);
				num4++;
			}
			num3 += fixedBlockNo;
			num2 -= fixedBlockNo;
		}
		int[] array = new int[num2];
		for (int j = 0; j < num2; j++)
		{
			array[j] = num3;
			if (++num3 >= num)
			{
				num3 = startBlock;
			}
		}
		App.Random.ShuffleInt(array);
		int num5 = (!isGameStartBlock || this.m_quickMode) ? 0 : fixedBlockNo;
		if (insertStartAct)
		{
			num5++;
		}
		int num6 = 0;
		while (num6 < array.Length && num5 < this.m_blockOrder.Length)
		{
			int fixedLayerNo = this.m_currentBlockLevelData[this.m_nowBlockLevelNum].fixedLayerNo;
			if (fixedLayerNo == -1)
			{
				int layerNum = this.m_currentBlockLevelData[this.m_nowBlockLevelNum].layerNum;
				this.m_blockOrder[num5].SetBlockAndRandomLayer(array[num6], layerNum);
			}
			else
			{
				this.m_blockOrder[num5].SetBlockAndFixedLayer(array[num6], fixedLayerNo);
			}
			num5++;
			num6++;
		}
	}

	private StageBlockManager.BlockOrderInfo GetCurerntBlockOrderInfo()
	{
		return this.m_blockOrder[this.m_currentOrderNum];
	}

	public int GetBlockLevel()
	{
		return this.m_nowBlockLevelNum;
	}

	public bool IsBlockLevelUp()
	{
		return this.m_nowBlockLevelNum != this.m_nextBlockLevelNum;
	}

	private void OnMsgPrepareStageReplace(MsgPrepareStageReplace msg)
	{
		if (this.m_objectSpawnableManager == null)
		{
			return;
		}
		switch (this.m_firstOrderType)
		{
		case StageBlockManager.OrderType.BOSS_SINGLE:
			return;
		case StageBlockManager.OrderType.TUTORIAL:
			return;
		case StageBlockManager.OrderType.DEBUG:
			base.StartCoroutine(this.m_objectSpawnableManager.LoadSetTable(1, 91));
			return;
		}
		this.m_highSpeedSetFlag = (this.m_nextBlockLevelNum > 1);
		if (this.m_highSpeedSetFlag)
		{
			int readFileCount = Mathf.Min(this.m_highSpeedCount * 2, 6);
			base.StartCoroutine(this.m_objectSpawnableManager.LoadSetTable(this.m_highSpeedTable, readFileCount));
		}
		else
		{
			int num = Mathf.Min(this.m_nextBlockLevelNum, this.m_firstBlockNoOnLevel.Length - 1);
			base.StartCoroutine(this.m_objectSpawnableManager.LoadSetTable(this.m_firstBlockNoOnLevel[num], this.m_currentBlockLevelData[num].numBlock));
		}
	}

	public void OnMsgStageReplace(MsgStageReplace msg)
	{
		this.m_totalDistance = 0f;
		base.transform.position = msg.m_position;
		this.DeactivateAll();
		this.SetOrderModeToDefault();
		this.m_nowBlockLevelNum = Mathf.Min(this.m_nextBlockLevelNum, this.m_currentBlockLevelData.Length - 1);
		this.m_stageRepeatNum++;
		if (this.m_nowBlockLevelNum < this.m_currentBlockLevelData.Length && this.m_stageRepeatNum >= this.m_currentBlockLevelData[this.m_nowBlockLevelNum].repeatNum)
		{
			this.m_stageRepeatNum = 0;
			this.m_nextBlockLevelNum++;
		}
		bool isGameStartBlock;
		if (this.m_quickMode)
		{
			isGameStartBlock = !this.m_quickFirstReplaceEnd;
			this.m_quickFirstReplaceEnd = true;
		}
		else
		{
			isGameStartBlock = (msg.m_speedLevel == PlayerSpeed.LEVEL_1);
		}
		this.ActiveFirstBlock(msg.m_position, msg.m_rotation, isGameStartBlock, true);
		this.ResetLevelInformationOnReplace();
	}

	private void OnMsgSetStageOnMapBoss(MsgSetStageOnMapBoss msg)
	{
		this.m_totalDistance = 0f;
		base.transform.position = msg.m_position;
		int layer = 0;
		BossCategory bossCategory = BossTypeUtil.GetBossCategory(msg.m_bossType);
		if (bossCategory != BossCategory.MAP)
		{
			if (bossCategory == BossCategory.EVENT)
			{
				layer = 21 + BossTypeUtil.GetLayerNumber(msg.m_bossType);
			}
		}
		else
		{
			layer = 11 + BossTypeUtil.GetLayerNumber(msg.m_bossType);
		}
		this.SetOrderMode(StageBlockManager.OrderType.BOSS_SINGLE);
		this.CreateBlockOrderInfo(1);
		this.m_blockOrder[0].SetBlockAndFixedLayer(0, layer);
		if (this.m_levelInformation != null)
		{
			this.m_levelInformation.DistanceToBossOnStart = 0f;
		}
		this.ActiveFirstBlock(msg.m_position, msg.m_rotation, false, false);
	}

	public void SetStageOnTutorial(Vector3 position)
	{
		this.m_totalDistance = 0f;
		base.transform.position = position;
		this.m_nowBlockLevelNum = 0;
		this.SetOrderMode(StageBlockManager.OrderType.TUTORIAL);
		this.CreateBlockOrderInfo(1);
		this.m_blockOrder[0].SetBlockAndFixedLayer(91, 0);
		this.ActiveFirstBlock(position, Quaternion.identity, false, false);
		this.ResetLevelInformationOnReplace();
	}

	private void OnMsgTutorialResetForRetry(MsgTutorialResetForRetry msg)
	{
		base.transform.position = msg.m_position;
		StageBlockManager.OrderType orderType = this.m_currentBlock.m_orderType;
		Vector3 startPoint = this.m_currentBlock.m_startPoint;
		this.DeactivateAll();
		if (orderType == StageBlockManager.OrderType.TUTORIAL)
		{
			this.SetOrderMode(StageBlockManager.OrderType.TUTORIAL);
			this.CreateBlockOrderInfo(1);
			this.m_blockOrder[0].SetBlockAndFixedLayer(91, 0);
		}
		StageBlockManager.StageBlockInfo stageBlockInfo = this.ActivateBlock(this.m_blockOrder[this.m_currentOrderNum], startPoint, true, this.m_orderType, this.m_currentOrderNum, MsgActivateBlock.CheckPoint.None);
		if (stageBlockInfo != null)
		{
			this.ChangeCurrentBlock(stageBlockInfo);
		}
	}

	private void CallbackOnStageStartAct(StageBlockManager.StageBlockInfo info)
	{
		ObjUtil.PushCamera(CameraType.START_ACT, 0.1f);
		ObjUtil.PauseCombo(MsgPauseComboTimer.State.PAUSE, -1f);
	}

	private void CallbackOnStageStartActEnd(StageBlockManager.StageBlockInfo info)
	{
		ObjUtil.PopCamera(CameraType.START_ACT, 0f);
		this.SetQuickModeStart();
		ObjUtil.SendStartItemAndChao();
		ObjUtil.PauseCombo(MsgPauseComboTimer.State.PLAY, -1f);
	}

	private void CallbackOnStageReplaceActEnd(StageBlockManager.StageBlockInfo info)
	{
		ObjUtil.PopCamera(CameraType.START_ACT, 0f);
		this.SetQuickModeStart();
		if (StageItemManager.Instance != null)
		{
			MsgPhatomItemOnBoss msg = new MsgPhatomItemOnBoss();
			StageItemManager.Instance.OnResumeItemOnBoss(msg);
		}
		GameObjectUtil.SendMessageToTagObjects("Chao", "OnResumeChangeLevel", null, SendMessageOptions.DontRequireReceiver);
		ObjUtil.PauseCombo(MsgPauseComboTimer.State.PLAY_RESET, -1f);
	}

	private StageBlockManager.BlockOrderInfo[] CreateBlockOrderInfo(int num)
	{
		this.m_blockOrder = new StageBlockManager.BlockOrderInfo[num];
		for (int i = 0; i < this.m_blockOrder.Length; i++)
		{
			this.m_blockOrder[i] = new StageBlockManager.BlockOrderInfo();
		}
		return this.m_blockOrder;
	}

	private bool IsHighSpeed()
	{
		return this.m_nextBlockLevelNum > 1 && this.m_nowBlockLevelNum > 1;
	}

	private void SetQuickModeStart()
	{
		if (this.m_quickMode && StageTimeManager.Instance != null)
		{
			StageTimeManager.Instance.PlayStart();
		}
	}

	public StageBlockManager.StageBlockInfo GetCurrenBlockInfo()
	{
		return this.m_currentBlock;
	}

	public float GetBlockLocalDistance()
	{
		if (this.CurrentBlock != null)
		{
			return this.CurrentBlock.GetBlockRunningLength(this.m_totalDistance);
		}
		return 0f;
	}

	public Vector3 GetBlockLocalPosition(Vector3 pos)
	{
		if (this.CurrentBlock != null)
		{
			return pos - this.CurrentBlock.m_startPoint;
		}
		return Vector3.zero;
	}
}
