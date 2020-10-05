using System;
using UnityEngine;

public class StageModeManager : MonoBehaviour
{
	public enum Mode
	{
		ENDLESS,
		QUICK,
		UNKNOWN
	}

	private static StageModeManager m_instance;

	[Header("debugFlag にチェックを入れると、Console にテキストが表示されます")]
	public bool m_debugFlag;

	public bool m_firstTutorial;

	[Header("モード設定パラメータ")]
	public StageModeManager.Mode m_mode = StageModeManager.Mode.UNKNOWN;

	private CharacterAttribute m_stageCharaAttribute;

	private int m_stageIndex = 1;

	public static StageModeManager Instance
	{
		get
		{
			return StageModeManager.m_instance;
		}
	}

	public StageModeManager.Mode StageMode
	{
		get
		{
			return this.m_mode;
		}
		set
		{
			this.m_mode = value;
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

	public CharacterAttribute QuickStageCharaAttribute
	{
		get
		{
			return this.m_stageCharaAttribute;
		}
		private set
		{
		}
	}

	public int QuickStageIndex
	{
		get
		{
			return this.m_stageIndex;
		}
		private set
		{
		}
	}

	public bool IsQuickMode()
	{
		return this.m_mode == StageModeManager.Mode.QUICK;
	}

	public void DrawQuickStageIndex()
	{
		this.m_stageIndex = 1;
		if (EventManager.Instance != null && EventManager.Instance.IsQuickEvent())
		{
			EventStageData stageData = EventManager.Instance.GetStageData();
			if (stageData != null)
			{
				this.m_stageIndex = MileageMapUtility.GetStageIndex(stageData.stage_key);
			}
		}
		else
		{
			UnityEngine.Random.seed = NetUtil.GetCurrentUnixTime();
			int num = 1;
			int num2 = 4;
			int num3 = UnityEngine.Random.Range(num, num2);
			if (num3 >= num2)
			{
				num3 = num;
			}
			this.m_stageIndex = num3;
		}
		switch (this.m_stageIndex)
		{
		case 1:
			this.m_stageCharaAttribute = CharacterAttribute.SPEED;
			break;
		case 2:
			this.m_stageCharaAttribute = CharacterAttribute.FLY;
			break;
		case 3:
			this.m_stageCharaAttribute = CharacterAttribute.POWER;
			break;
		default:
			this.m_stageCharaAttribute = CharacterAttribute.UNKNOWN;
			break;
		}
	}

	private void Awake()
	{
		if (StageModeManager.m_instance == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			StageModeManager.m_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		base.enabled = false;
	}

	private void OnDestroy()
	{
		if (StageModeManager.m_instance == this)
		{
			StageModeManager.m_instance = null;
		}
	}

	private void SetDebugDraw(string msg)
	{
	}
}
