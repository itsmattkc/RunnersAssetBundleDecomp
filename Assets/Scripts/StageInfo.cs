using System;
using UnityEngine;

public class StageInfo : MonoBehaviour
{
	public class MileageMapInfo
	{
		public MileageMapState m_mapState = new MileageMapState();

		public long[] m_pointScore = new long[6];
	}

	private string m_stageName;

	private BossType m_bossType;

	private int m_numBossAttack;

	private TenseType m_tenseType;

	private StageInfo.MileageMapInfo m_mapInfo;

	private bool m_notChangeTense;

	private bool m_existBoss;

	private bool m_bossStage;

	private bool m_fromTitle;

	private bool m_tutorialStage;

	private bool m_eventStage;

	private bool m_quickMode;

	private bool m_firstTutorial;

	private bool[] m_boostItemValid;

	private ItemType[] m_equippedItem;

	public string SelectedStageName
	{
		get
		{
			return this.m_stageName;
		}
		set
		{
			this.m_stageName = value;
		}
	}

	public BossType BossType
	{
		get
		{
			return this.m_bossType;
		}
		set
		{
			this.m_bossType = value;
		}
	}

	public int NumBossAttack
	{
		get
		{
			return this.m_numBossAttack;
		}
		set
		{
			this.m_numBossAttack = value;
		}
	}

	public TenseType TenseType
	{
		get
		{
			return this.m_tenseType;
		}
		set
		{
			this.m_tenseType = value;
		}
	}

	public StageInfo.MileageMapInfo MileageInfo
	{
		get
		{
			return this.m_mapInfo;
		}
		set
		{
			this.m_mapInfo = value;
		}
	}

	public bool NotChangeTense
	{
		get
		{
			return this.m_notChangeTense;
		}
		set
		{
			this.m_notChangeTense = value;
		}
	}

	public bool ExistBoss
	{
		get
		{
			return this.m_existBoss;
		}
		set
		{
			this.m_existBoss = value;
		}
	}

	public bool BossStage
	{
		get
		{
			return this.m_bossStage;
		}
		set
		{
			this.m_bossStage = value;
		}
	}

	public bool FromTitle
	{
		get
		{
			return this.m_fromTitle;
		}
		set
		{
			this.m_fromTitle = value;
		}
	}

	public bool FirstTutorial
	{
		get
		{
			return this.m_firstTutorial;
		}
		set
		{
			this.m_firstTutorial = value;
		}
	}

	public bool TutorialStage
	{
		get
		{
			return this.m_tutorialStage;
		}
		set
		{
			this.m_tutorialStage = value;
		}
	}

	public bool EventStage
	{
		get
		{
			return this.m_eventStage;
		}
		set
		{
			this.m_eventStage = value;
		}
	}

	public bool QuickMode
	{
		get
		{
			return this.m_quickMode;
		}
		set
		{
			this.m_quickMode = value;
		}
	}

	public bool[] BoostItemValid
	{
		get
		{
			return this.m_boostItemValid;
		}
		set
		{
			this.m_boostItemValid = value;
		}
	}

	public ItemType[] EquippedItems
	{
		get
		{
			return this.m_equippedItem;
		}
		set
		{
			this.m_equippedItem = new ItemType[value.Length];
			value.CopyTo(this.m_equippedItem, 0);
		}
	}

	public static string GetStageNameByIndex(int index)
	{
		return "w" + index.ToString("D2");
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.m_stageName = string.Empty;
		this.m_bossType = BossType.FEVER;
		this.m_tenseType = TenseType.AFTERNOON;
		this.m_numBossAttack = 0;
		this.m_mapInfo = new StageInfo.MileageMapInfo();
		this.m_boostItemValid = new bool[3];
		this.m_equippedItem = new ItemType[3];
		for (int i = 0; i < 3; i++)
		{
			this.m_equippedItem[i] = ItemType.UNKNOWN;
		}
		base.enabled = false;
	}
}
