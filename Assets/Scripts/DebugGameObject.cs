using DataTable;
using Message;
using SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Text;
using UnityEngine;

public class DebugGameObject : SingletonGameObject<DebugGameObject>
{
	public enum GUI_RECT_ANCHOR
	{
		CENTER,
		CENTER_LEFT,
		CENTER_RIGHT,
		CENTER_TOP,
		CENTER_BOTTOM,
		LEFT_TOP,
		LEFT_BOTTOM,
		RIGHT_TOP,
		RIGHT_BOTTOM
	}

	public enum LOADING_SUFFIXE
	{
		DEBUG_JA,
		DEBUG_DE,
		DEBUG_EN,
		DEBUG_ES,
		DEBUG_FR,
		DEBUG_IT,
		DEBUG_KO,
		DEBUG_PT,
		DEBUG_RU,
		DEBUG_ZH,
		DEBUG_ZHJ,
		NONE
	}

	public enum MOUSE_R_CLICK
	{
		PAUSED,
		ATLAS,
		HI_SPEED,
		LOW_SPEED,
		NONE
	}

	public enum DEBUG_CHECK_TYPE
	{
		DRAW_CALL,
		LOAD_ATLAS,
		NONE
	}

	private enum DEBUG_PLAY_TYPE
	{
		ITEM,
		COLOR,
		BOSS_DESTORY,
		NUM,
		NONE
	}

	private enum DEBUG_MENU_TYPE
	{
		ITEM,
		MILEAGE,
		RANKING,
		CHAO_TEX_RELEASE,
		DAILY_BATTLE,
		DEBUG_GUI_OFF,
		NUM,
		NONE
	}

	private enum DEBUG_MENU_RANKING_CATEGORY
	{
		CACHE,
		CHANGE_TEST,
		NUM
	}

	private enum DEBUG_MENU_ITEM_CATEGORY
	{
		ITEM,
		OTOMO,
		CHARACTER,
		NUM
	}

	private delegate void NetworkRequestSuccessCallback();

	private delegate void NetworkRequestFailedCallback(ServerInterface.StatusCode statusCode);

	private sealed class _NetworkRequest_c__IteratorCB : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetBase request;

		internal DebugGameObject.NetworkRequestSuccessCallback successCallback;

		internal DebugGameObject.NetworkRequestFailedCallback failedCallback;

		internal int _PC;

		internal object _current;

		internal NetBase ___request;

		internal DebugGameObject.NetworkRequestSuccessCallback ___successCallback;

		internal DebugGameObject.NetworkRequestFailedCallback ___failedCallback;

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
				this.request.Request();
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this.request.IsExecuting())
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this.request.IsSucceeded())
			{
				if (this.successCallback != null)
				{
					this.successCallback();
				}
			}
			else if (this.failedCallback != null)
			{
				this.failedCallback(this.request.resultStCd);
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

	private const float DEBUG_DISP_SIZE_W = 800f;

	private const float DEBUG_DISP_SIZE_H = 450f;

	private const float DEBUG_POP_TIME = 10f;

	private const float DEBUG_POP_MOVE_RATE = 0.05f;

	private const int DEBUG_MILEAGE_MAX = 50;

	public const float UPDATE_PERIOD_TIME = 2f;

	private const RankingUtil.RankingScoreType DEFAULT_RAIVAL_SCORE_TYPE = RankingUtil.RankingScoreType.HIGH_SCORE;

	private const RankingUtil.RankingScoreType DEFAULT_SP_SCORE_TYPE = RankingUtil.RankingScoreType.TOTAL_SCORE;

	[Header("デバック用のオブジェクトです。不要な場合は要削除"), SerializeField]
	public bool m_debugActive = true;

	[SerializeField]
	public bool m_debugNetworkActive = true;

	[SerializeField]
	public bool m_debugTestBtn = true;

	[SerializeField]
	public DebugGameObject.DEBUG_CHECK_TYPE m_debugCheckType = DebugGameObject.DEBUG_CHECK_TYPE.NONE;

	[SerializeField]
	public DebugGameObject.MOUSE_R_CLICK m_mouseRightClick = DebugGameObject.MOUSE_R_CLICK.NONE;

	[SerializeField]
	public bool m_mouseWheelUseSpeed;

	[SerializeField]
	public ItemType m_mouseWheelUseItem = ItemType.UNKNOWN;

	[SerializeField]
	public float m_currentTimeScale = 1f;

	[SerializeField]
	private bool m_titleFirstLogin;

	[SerializeField]
	private DebugGameObject.LOADING_SUFFIXE m_titleLoadingSuffixe = DebugGameObject.LOADING_SUFFIXE.NONE;

	[SerializeField]
	private string m_suffixeBaseText = "title_load_index_{LANG}.html";

	[Header("ランキング関連"), SerializeField]
	private bool m_rankingDebug;

	[SerializeField]
	private RankingUtil.RankingRankerType m_targetRankingRankerType = RankingUtil.RankingRankerType.RIVAL;

	[SerializeField]
	private RankingUtil.RankingScoreType m_rivalRankingScoreType;

	[SerializeField]
	private RankingUtil.RankingScoreType m_spRankingScoreType = RankingUtil.RankingScoreType.TOTAL_SCORE;

	[SerializeField]
	private bool m_rankingLog;

	[Header("現在の順位情報取得"), SerializeField]
	private int m_currentScore = -1;

	[SerializeField]
	private int m_currentScoreEvent = -1;

	[Header("通信関連"), SerializeField]
	private int m_msgMax;

	[Header("暗号化フラグ"), SerializeField]
	private bool m_crypt = true;

	[Header("更新頻度(数値表示確認用)"), SerializeField]
	private int m_updateCost;

	[SerializeField]
	private List<string> m_updateCostList;

	[Header("ルーレット関連"), SerializeField]
	private RouletteCategory m_rouletteDummyCategory;

	[SerializeField]
	private bool m_rouletteTutorial;

	[SerializeField]
	private float m_rouletteConectTime = 1f;

	[SerializeField]
	private List<ServerItem.Id> m_rouletteDataPremium;

	[SerializeField]
	private List<ServerItem.Id> m_rouletteDataSpecial;

	[SerializeField]
	private List<ServerItem.Id> m_rouletteDataItem;

	[SerializeField]
	private List<ServerItem.Id> m_rouletteDataRaid;

	[SerializeField]
	private List<ServerItem.Id> m_rouletteDataDefault;

	[Header("ダミー通信障害発生率(%)"), SerializeField]
	private int m_rouletteDummyError;

	[Header("デバック用キャンペーン設定"), SerializeField]
	private List<Constants.Campaign.emType> m_debugCampaign;

	private List<string> m_debugDummy;

	private float m_mouseRightClickDelayTime;

	private Dictionary<string, int> m_updCost;

	private GameObject m_rouletteCallback;

	private MsgGetWheelOptionsGeneralSucceed m_rouletteGetMsg;

	private MsgCommitWheelSpinGeneralSucceed m_rouletteCommitMsg;

	private float m_debugRouletteTime;

	private float m_debugGetRouletteTime;

	private bool m_debugRouletteConectError = true;

	private List<string> m_keys;

	private Dictionary<string, int> m_currentUpdCost;

	private float m_time;

	private float m_wheelInputDelay;

	private Camera m_camera;

	private float m_cameraSizeRate = 1f;

	private bool m_debugPlay;

	private DebugGameObject.DEBUG_PLAY_TYPE m_debugPlayType = DebugGameObject.DEBUG_PLAY_TYPE.NONE;

	private bool m_debugScore;

	private float m_debugScoreDelay;

	private string m_debugScoreText = string.Empty;

	private bool m_debugMenu;

	private DebugGameObject.DEBUG_MENU_TYPE m_debugMenuType = DebugGameObject.DEBUG_MENU_TYPE.NONE;

	private DebugGameObject.DEBUG_MENU_RANKING_CATEGORY m_debugMenuRankingCateg;

	private RankingUtil.RankingRankerType m_debugMenuRankingType = RankingUtil.RankingRankerType.COUNT;

	private DebugGameObject.DEBUG_MENU_ITEM_CATEGORY m_debugMenuItemSelect;

	private Dictionary<DebugGameObject.DEBUG_MENU_ITEM_CATEGORY, List<int>> m_debugMenuItemList;

	private int m_debugMenuItemNum = 1;

	private int m_debugMenuItemPage;

	private List<DataTable.ChaoData> m_debugMenuOtomoList;

	private List<CharacterDataNameInfo.Info> m_debugMenuCharaList = new List<CharacterDataNameInfo.Info>();

	private int m_debugMenuMileageEpi = 2;

	private int m_debugMenuMileageCha = 1;

	private int m_debugMenuRankingCurrentRank;

	private int m_debugMenuRankingCurrentDummyRank;

	private int m_debugMenuRankingCurrentLegMax;

	private bool m_debugCheckFlag;

	private Dictionary<string, Dictionary<string, List<UIDrawCall>>> m_debugDrawCallList;

	private string m_debugDrawCallPanelCurrent = string.Empty;

	private string m_debugDrawCallMatCurrent = string.Empty;

	private List<UIAtlas> m_debugAtlasList;

	private List<UIAtlas> m_debugAtlasLangList;

	private string m_debugAtlasLangCode = "---";

	private Dictionary<long, string> m_debugPop;

	private Dictionary<long, Rect> m_debugPopRect;

	private Dictionary<long, float> m_debugPopTime;

	private long m_debugPopCount;

	private bool m_debugDeck;

	private int m_debugDeckCount;

	private int m_debugDeckCurrentIndex;

	private List<string> m_debugDeckList;

	private bool m_debugCharaData;

	private int m_debugCharaDataCount;

	private ServerPlayerState.CHARA_SORT m_debugCharaDataSort;

	private List<string> m_debugCharaDataList;

	private Dictionary<CharaType, ServerCharacterState> m_debugCharaList;

	private string m_debugCharaDataInfo = string.Empty;

	private ServerCharacterState m_debugCharaDataState;

	private Dictionary<ServerItem.Id, int> m_debugCharaDataBuyCost;

	private bool m_debugCharaDataBuy;

	private ResourceSceneLoader m_debugSceneLoader;

	private int m_debugGiftItemId;

	private GameObject m_debugGiftCallback;

	public bool firstLogin
	{
		get
		{
			return this.m_titleFirstLogin;
		}
	}

	public DebugGameObject.LOADING_SUFFIXE loadingSuffixe
	{
		get
		{
			return this.m_titleLoadingSuffixe;
		}
	}

	public string suffixeBaseText
	{
		get
		{
			return this.m_suffixeBaseText;
		}
	}

	public bool debugActive
	{
		get
		{
			return this.m_debugActive;
		}
	}

	public bool rankingDebug
	{
		get
		{
			return this.m_rankingDebug;
		}
	}

	public RankingUtil.RankingRankerType targetRankingRankerType
	{
		get
		{
			if (!this.rankingDebug)
			{
				return RankingUtil.RankingRankerType.RIVAL;
			}
			return this.m_targetRankingRankerType;
		}
	}

	public RankingUtil.RankingScoreType rivalRankingScoreType
	{
		get
		{
			if (!this.rankingDebug)
			{
				return RankingUtil.RankingScoreType.HIGH_SCORE;
			}
			return this.m_rivalRankingScoreType;
		}
	}

	public RankingUtil.RankingScoreType spRankingScoreType
	{
		get
		{
			if (!this.rankingDebug)
			{
				return RankingUtil.RankingScoreType.TOTAL_SCORE;
			}
			return this.m_spRankingScoreType;
		}
	}

	public RouletteCategory rouletteDummyCategory
	{
		get
		{
			return this.m_rouletteDummyCategory;
		}
	}

	public bool rouletteTutorial
	{
		get
		{
			return this.m_rouletteTutorial;
		}
	}

	public bool crypt
	{
		get
		{
			return this.m_crypt;
		}
	}

	public bool rankingLog
	{
		get
		{
			return this.m_rankingLog;
		}
	}

	public List<Constants.Campaign.emType> debugCampaign
	{
		get
		{
			return this.m_debugCampaign;
		}
	}

	private ResourceSceneLoader debugSceneLoader
	{
		get
		{
			if (this.m_debugSceneLoader == null)
			{
				GameObject gameObject = new GameObject("DebugTextLoader");
				if (gameObject != null)
				{
					this.m_debugSceneLoader = gameObject.AddComponent<ResourceSceneLoader>();
				}
			}
			return this.m_debugSceneLoader;
		}
	}

	public void PopLog(string log, float xRate, float yRate, DebugGameObject.GUI_RECT_ANCHOR anchor = DebugGameObject.GUI_RECT_ANCHOR.CENTER)
	{
	}

	public bool CheckMsgText(string msg)
	{
		if (!string.IsNullOrEmpty(msg) && this.m_msgMax < msg.Length)
		{
			this.m_msgMax = msg.Length;
			return true;
		}
		return false;
	}

	public void CheckUpdate(string name = "")
	{
		if (this.m_currentUpdCost == null)
		{
			this.m_currentUpdCost = new Dictionary<string, int>();
			this.m_currentUpdCost.Add("TOTAL_COST", 1);
			this.m_keys = new List<string>();
			this.m_keys.Add("TOTAL_COST");
			if (!string.IsNullOrEmpty(name) && !this.m_currentUpdCost.ContainsKey(name))
			{
				this.m_currentUpdCost.Add(name, 1);
				this.m_keys.Add(name);
			}
		}
		else
		{
			if (this.m_currentUpdCost.ContainsKey("TOTAL_COST"))
			{
				Dictionary<string, int> currentUpdCost;
				Dictionary<string, int> expr_97 = currentUpdCost = this.m_currentUpdCost;
				string key;
				string expr_9E = key = "TOTAL_COST";
				int num = currentUpdCost[key];
				expr_97[expr_9E] = num + 1;
			}
			if (!string.IsNullOrEmpty(name))
			{
				if (!this.m_currentUpdCost.ContainsKey(name))
				{
					this.m_currentUpdCost.Add(name, 1);
					this.m_keys.Add(name);
				}
				else
				{
					Dictionary<string, int> currentUpdCost2;
					Dictionary<string, int> expr_F0 = currentUpdCost2 = this.m_currentUpdCost;
					int num = currentUpdCost2[name];
					expr_F0[name] = num + 1;
				}
			}
		}
		if (this.m_updCost == null)
		{
			this.m_updCost = new Dictionary<string, int>();
			this.m_updCost.Add("TOTAL_COST", 0);
			if (!string.IsNullOrEmpty(name) && !this.m_updCost.ContainsKey(name))
			{
				this.m_updCost.Add(name, 0);
			}
		}
		else if (!string.IsNullOrEmpty(name) && !this.m_updCost.ContainsKey(name))
		{
			this.m_updCost.Add(name, 0);
		}
	}

	private void OnGUI()
	{
		base.enabled = false;
	}

	private void GuiDummyObject()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("title") != -1 || !this.m_debugTestBtn)
		{
			return;
		}
		int num = 0;
		if (this.m_debugDummy != null && this.m_debugDummy.Count > 0)
		{
			num = this.m_debugDummy.Count;
		}
		Rect position = this.CreateGuiRect(new Rect(0f, 90f, 70f, 25f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP);
		if (GUI.Button(position, "Stress Test\n" + num))
		{
			if (this.m_debugDummy == null)
			{
				this.m_debugDummy = new List<string>();
			}
			for (int i = 0; i < 500; i++)
			{
				this.m_debugDummy.Add(string.Empty + this.m_debugDummy.Count);
			}
		}
		if (this.m_debugDummy != null && this.m_debugDummy.Count > 0)
		{
			int num2 = 0;
			foreach (string current in this.m_debugDummy)
			{
				Rect position2 = this.CreateGuiRect(new Rect(3f * (float)(num2 % 250), 4f * (float)(num2 / 250), 2f, 3f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP);
				GUI.Box(position2, current);
				num2++;
			}
		}
	}

	private void GuiPop()
	{
		if (this.m_debugPop != null && this.m_debugPop.Count > 0 && this.m_camera != null)
		{
			float deltaTime = Time.deltaTime;
			long[] array = new long[this.m_debugPop.Count];
			long num = -1L;
			float num2 = this.m_camera.pixelRect.yMax * 0.05f;
			this.m_debugPop.Keys.CopyTo(array, 0);
			if (array.Length > 0)
			{
				long[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					long num3 = array2[i];
					string text = this.m_debugPop[num3] + "\n count:" + num3;
					Rect position = this.m_debugPopRect[num3];
					float num4 = (10f - this.m_debugPopTime[num3]) / 10f;
					num4 = 1f - (num4 - 1f) * (num4 - 1f);
					position.y -= num2 * num4;
					GUI.Box(position, text);
					Dictionary<long, float> debugPopTime;
					Dictionary<long, float> expr_114 = debugPopTime = this.m_debugPopTime;
					long key;
					long expr_119 = key = num3;
					float num5 = debugPopTime[key];
					expr_114[expr_119] = num5 - deltaTime;
					if (this.m_debugPopTime[num3] <= 0f)
					{
						num = num3;
					}
				}
			}
			if (num >= 0L)
			{
				this.m_debugPop.Remove(num);
				this.m_debugPopRect.Remove(num);
				this.m_debugPopTime.Remove(num);
			}
		}
	}

	private void GuiCheck()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("title") != -1 || this.m_debugCheckType == DebugGameObject.DEBUG_CHECK_TYPE.NONE)
		{
			return;
		}
		if (this.m_debugCheckFlag)
		{
			float num = 350f;
			float num2 = 700f;
			Rect rect = this.CreateGuiRect(new Rect(0f, 0f, num2 + 10f, num + 10f), DebugGameObject.GUI_RECT_ANCHOR.CENTER);
			GUI.Box(rect, string.Empty);
			DebugGameObject.DEBUG_CHECK_TYPE debugCheckType;
			if (GUI.Button(this.CreateGuiRect(new Rect(0f, 0f, 200f, 20f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), "close"))
			{
				debugCheckType = this.m_debugCheckType;
				if (debugCheckType != DebugGameObject.DEBUG_CHECK_TYPE.DRAW_CALL)
				{
					if (debugCheckType == DebugGameObject.DEBUG_CHECK_TYPE.LOAD_ATLAS)
					{
						this.m_debugCheckFlag = !this.m_debugCheckFlag;
						this.m_debugDrawCallPanelCurrent = string.Empty;
						this.m_debugDrawCallMatCurrent = string.Empty;
						if (this.m_debugAtlasList != null)
						{
							this.m_debugAtlasList.Clear();
						}
						HudMenuUtility.SetConnectAlertSimpleUI(false);
					}
				}
				else if (string.IsNullOrEmpty(this.m_debugDrawCallPanelCurrent) && string.IsNullOrEmpty(this.m_debugDrawCallMatCurrent))
				{
					this.m_debugCheckFlag = !this.m_debugCheckFlag;
					this.m_debugDrawCallPanelCurrent = string.Empty;
					this.m_debugDrawCallMatCurrent = string.Empty;
					if (this.m_debugAtlasList != null)
					{
						this.m_debugAtlasList.Clear();
					}
					if (this.m_debugAtlasLangList != null)
					{
						this.m_debugAtlasLangList.Clear();
					}
					HudMenuUtility.SetConnectAlertSimpleUI(false);
				}
				else if (!string.IsNullOrEmpty(this.m_debugDrawCallMatCurrent))
				{
					this.m_debugDrawCallMatCurrent = string.Empty;
				}
				else if (!string.IsNullOrEmpty(this.m_debugDrawCallPanelCurrent))
				{
					this.m_debugDrawCallPanelCurrent = string.Empty;
					this.m_debugDrawCallMatCurrent = string.Empty;
				}
				else
				{
					this.m_debugCheckFlag = !this.m_debugCheckFlag;
					this.m_debugDrawCallPanelCurrent = string.Empty;
					this.m_debugDrawCallMatCurrent = string.Empty;
					HudMenuUtility.SetConnectAlertSimpleUI(false);
				}
			}
			debugCheckType = this.m_debugCheckType;
			if (debugCheckType != DebugGameObject.DEBUG_CHECK_TYPE.DRAW_CALL)
			{
				if (debugCheckType == DebugGameObject.DEBUG_CHECK_TYPE.LOAD_ATLAS)
				{
					this.GuiLoadAtlas(rect, num2, num);
				}
			}
			else
			{
				this.GuiDrawCall2D(rect, num2, num);
			}
		}
		else if (GUI.Button(this.CreateGuiRect(new Rect(0f, 0f, 200f, 20f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), string.Empty + this.m_debugCheckType))
		{
			this.m_debugCheckFlag = !this.m_debugCheckFlag;
			this.m_debugDrawCallPanelCurrent = string.Empty;
			this.m_debugDrawCallMatCurrent = string.Empty;
			DebugGameObject.DEBUG_CHECK_TYPE debugCheckType = this.m_debugCheckType;
			if (debugCheckType != DebugGameObject.DEBUG_CHECK_TYPE.DRAW_CALL)
			{
				if (debugCheckType == DebugGameObject.DEBUG_CHECK_TYPE.LOAD_ATLAS)
				{
					this.CheckLoadAtlas();
				}
			}
			else
			{
				this.CheckDrawCall2D();
			}
			HudMenuUtility.SetConnectAlertSimpleUI(true);
		}
	}

	private void GuiDeckData()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("title") != -1 || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("playingstage") != -1)
		{
			return;
		}
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("MainMenu") == -1)
		{
			return;
		}
		if (!DeckViewWindow.isActive)
		{
			if (this.m_debugDeck)
			{
				float height = 100f;
				float width = 550f;
				Rect rect = this.CreateGuiRect(new Rect(0f, 40f, width, height), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
				GUI.Box(rect, string.Empty);
				if (this.m_debugDeckList != null && this.m_debugDeckList.Count > 0)
				{
					float height2 = 0.7f;
					float num = 0.135714293f;
					float num2 = 0.142857149f;
					float height3 = 0.2f;
					float width2 = num;
					float num3 = (num2 - num) * 0.5f;
					for (int i = 0; i < this.m_debugDeckList.Count; i++)
					{
						GUI.Box(this.CreateGuiRectInRate(rect, new Rect(num3 + num2 * (float)i, 0.03f, num, height2), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP), this.m_debugDeckList[i]);
						if (GUI.Button(this.CreateGuiRectInRate(rect, new Rect(num3 + num2 * (float)i, -0.03f, width2, height3), DebugGameObject.GUI_RECT_ANCHOR.LEFT_BOTTOM), "reset"))
						{
							DeckUtil.DeckReset(i);
							this.DebugCreateDeckList();
						}
					}
					if (GUI.Button(this.CreateGuiRectInRate(rect, new Rect(-num3, 0.03f, num, 0.94f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP), "reload\n\n current:" + this.m_debugDeckCurrentIndex))
					{
						this.DebugCreateDeckList();
					}
				}
				if (GUI.Button(this.CreateGuiRect(new Rect(0f, 10f, 60f, 25f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), "close"))
				{
					this.m_debugDeck = !this.m_debugDeck;
				}
				if (this.m_debugDeckCount > 300)
				{
					this.DebugCreateDeckList();
				}
				this.m_debugDeckCount++;
			}
			else if (this.m_debugCharaData)
			{
				float height4 = 350f;
				float width3 = 600f;
				int num4 = 6;
				Rect rect2 = this.CreateGuiRect(new Rect(0f, 40f, width3, height4), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
				GUI.Box(rect2, string.Empty);
				if (this.m_debugCharaDataList != null && this.m_debugCharaDataList.Count > 0)
				{
					float height5 = 0.19f;
					float num5 = 1f / (float)num4 * 0.95f;
					float num6 = 1f / (float)num4;
					float num7 = (num6 - num5) * 0.5f;
					for (int j = 0; j < this.m_debugCharaDataList.Count; j++)
					{
						int num8 = j % num4;
						int num9 = j / num4;
						if (GUI.Button(this.CreateGuiRectInRate(rect2, new Rect(num7 + num6 * (float)num8, 0.02f + 0.2f * (float)num9, num5, height5), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP), this.m_debugCharaDataList[j]) && !this.m_debugCharaDataBuy)
						{
							this.DebugCreateCharaInfo(j);
						}
					}
					if (GUI.Button(this.CreateGuiRectInRate(rect2, new Rect(0.01f, -0.12f, 0.175f, 0.1f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_BOTTOM), "sort\n" + this.m_debugCharaDataSort))
					{
						int num10 = 3;
						int num11 = (int)this.m_debugCharaDataSort;
						num11 = (num11 + 1) % num10;
						this.DebugCreateCharaList((ServerPlayerState.CHARA_SORT)num11);
					}
					if (GUI.Button(this.CreateGuiRectInRate(rect2, new Rect(0.01f, -0.01f, 0.175f, 0.1f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_BOTTOM), "offset\n" + this.m_debugCharaDataCount))
					{
						this.DebugCreateCharaList(this.m_debugCharaDataSort);
					}
					GUI.Box(this.CreateGuiRectInRate(rect2, new Rect(-0.16f, -0.01f, 0.65f, 0.21f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_BOTTOM), this.m_debugCharaDataInfo);
					if (this.m_debugCharaDataBuyCost != null && this.m_debugCharaDataBuyCost.Count > 0 && !this.m_debugCharaDataBuy)
					{
						int count = this.m_debugCharaDataBuyCost.Count;
						Dictionary<ServerItem.Id, int>.KeyCollection keys = this.m_debugCharaDataBuyCost.Keys;
						float num12 = 0.2f / (float)count;
						float num13 = 0.02f / (float)count;
						float num14 = (num12 + num13) * -1f;
						int num15 = 0;
						foreach (ServerItem.Id current in keys)
						{
							if (GUI.Button(this.CreateGuiRectInRate(rect2, new Rect(-0.005f, -num13 + num14 * (float)num15, 0.145f, num12), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_BOTTOM), string.Concat(new object[]
							{
								string.Empty,
								current,
								"\n",
								this.m_debugCharaDataBuyCost[current]
							})))
							{
								this.DebugBuyChara(current, this.m_debugCharaDataBuyCost[current]);
							}
							num15++;
						}
					}
				}
				if (GUI.Button(this.CreateGuiRect(new Rect(0f, 10f, 60f, 25f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), "close"))
				{
					this.m_debugCharaData = !this.m_debugCharaData;
				}
				if (GUI.Button(this.CreateGuiRect(new Rect(200f, 10f, 100f, 25f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), "roulette\n chara picup") && RouletteManager.Instance != null)
				{
					RouletteManager.Instance.RequestPicupCharaList(false);
				}
			}
			else if (GUI.Button(this.CreateGuiRect(new Rect(-100f, 5f, 60f, 25f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), "deck data"))
			{
				this.m_debugDeck = !this.m_debugDeck;
				this.DebugCreateDeckList();
			}
		}
	}

	private void DebugCreateDeckList()
	{
		this.m_debugDeckCount = 0;
		if (this.m_debugDeckList != null)
		{
			this.m_debugDeckList.Clear();
		}
		this.m_debugDeckList = new List<string>();
		this.m_debugDeckCurrentIndex = DeckUtil.GetDeckCurrentStockIndex();
		for (int i = 0; i < 6; i++)
		{
			CharaType charaType = CharaType.UNKNOWN;
			CharaType charaType2 = CharaType.UNKNOWN;
			int num = -1;
			int num2 = -1;
			DeckUtil.DeckSetLoad(i, ref charaType, ref charaType2, ref num, ref num2, null);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Empty + charaType);
			stringBuilder.Append("\n" + charaType2);
			stringBuilder.Append("\n" + num);
			stringBuilder.Append("\n" + num2);
			this.m_debugDeckList.Add(stringBuilder.ToString());
		}
	}

	private void DebugBuyChara(ServerItem.Id itemId, int cost)
	{
		if (this.m_debugCharaDataState != null)
		{
			long itemCount = GeneralUtil.GetItemCount(itemId);
			if (itemCount >= (long)cost)
			{
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					ServerItem item = new ServerItem(itemId);
					loggedInServerInterface.RequestServerUnlockedCharacter(this.m_debugCharaDataState.charaType, item, base.gameObject);
					this.m_debugCharaDataBuy = true;
				}
			}
			else
			{
				global::Debug.Log(string.Concat(new object[]
				{
					"DebugBuyChara error  ",
					itemId,
					":",
					itemCount
				}));
			}
		}
	}

	private void ServerUnlockedCharacter_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		if (this.m_debugCharaDataList != null)
		{
			this.m_debugCharaDataList.Clear();
		}
		this.m_debugCharaDataList = new List<string>();
		CharaType charaType = CharaType.UNKNOWN;
		if (this.m_debugCharaDataState != null)
		{
			charaType = this.m_debugCharaDataState.charaType;
		}
		ServerPlayerState playerState = ServerInterface.PlayerState;
		this.m_debugCharaList = playerState.GetCharacterStateList(this.m_debugCharaDataSort, false, this.m_debugCharaDataCount);
		int idx = 0;
		int num = 0;
		Dictionary<CharaType, ServerCharacterState>.KeyCollection keys = this.m_debugCharaList.Keys;
		foreach (CharaType current in keys)
		{
			if (charaType == current)
			{
				idx = num;
			}
			ServerCharacterState serverCharacterState = this.m_debugCharaList[current];
			CharacterDataNameInfo.Info charaInfo = this.m_debugCharaList[current].charaInfo;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Empty + charaInfo.m_name);
			stringBuilder.Append("\n Lv:" + serverCharacterState.Level);
			stringBuilder.Append(" ☆" + serverCharacterState.star);
			stringBuilder.Append("\n" + charaInfo.m_attribute);
			stringBuilder.Append("\n" + charaInfo.m_teamAttribute);
			stringBuilder.Append("\n IsUnlock:" + serverCharacterState.IsUnlocked);
			this.m_debugCharaDataList.Add(stringBuilder.ToString());
			num++;
		}
		this.DebugCreateCharaInfo(idx);
		this.m_debugCharaDataBuy = false;
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	private void DebugCreateCharaInfo(int idx)
	{
		ServerPlayerState playerState = ServerInterface.PlayerState;
		Dictionary<CharaType, ServerCharacterState>.KeyCollection keys = this.m_debugCharaList.Keys;
		int num = 0;
		foreach (CharaType current in keys)
		{
			if (idx == num)
			{
				ServerCharacterState serverCharacterState = this.m_debugCharaList[current];
				CharacterDataNameInfo.Info charaInfo = this.m_debugCharaList[current].charaInfo;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Empty + charaInfo.m_name);
				stringBuilder.Append(" Lv:" + serverCharacterState.Level);
				stringBuilder.Append(" ☆" + serverCharacterState.star);
				stringBuilder.Append("  " + charaInfo.m_attribute);
				stringBuilder.Append("  " + charaInfo.m_teamAttribute);
				stringBuilder.Append("  IsUnlock:" + serverCharacterState.IsUnlocked);
				stringBuilder.Append("\n Condition:" + serverCharacterState.Condition);
				stringBuilder.Append(" Status:" + serverCharacterState.Status);
				stringBuilder.Append(" OldStatus:" + serverCharacterState.OldStatus);
				stringBuilder.Append("\n Exp:" + serverCharacterState.Exp);
				stringBuilder.Append(" OldExp:" + serverCharacterState.OldExp);
				stringBuilder.Append(" priceRing:" + serverCharacterState.priceNumRings);
				stringBuilder.Append(" priceRSR:" + serverCharacterState.priceNumRedRings);
				stringBuilder.Append("\n teamAttribute:" + charaInfo.m_teamAttribute);
				stringBuilder.Append(" teamAttributeCategory:" + charaInfo.m_teamAttributeCategory);
				stringBuilder.Append(string.Concat(new object[]
				{
					"\n mainBonus:",
					charaInfo.m_mainAttributeBonus,
					" [",
					charaInfo.GetTeamAttributeValue(charaInfo.m_mainAttributeBonus),
					"]"
				}));
				stringBuilder.Append(string.Concat(new object[]
				{
					" subBonus:",
					charaInfo.m_subAttributeBonus,
					" [",
					charaInfo.GetTeamAttributeValue(charaInfo.m_subAttributeBonus),
					"]"
				}));
				stringBuilder.Append("\n IsRoulette:" + serverCharacterState.IsRoulette);
				this.m_debugCharaDataInfo = stringBuilder.ToString();
				this.m_debugCharaDataState = serverCharacterState;
				this.m_debugCharaDataBuyCost = this.m_debugCharaDataState.GetBuyCostItemList();
				break;
			}
			num++;
		}
	}

	private void DebugCreateCharaList(ServerPlayerState.CHARA_SORT sort)
	{
		if (this.m_debugCharaDataList != null)
		{
			this.m_debugCharaDataList.Clear();
		}
		this.m_debugCharaDataList = new List<string>();
		if (sort != this.m_debugCharaDataSort)
		{
			this.m_debugCharaDataCount = 0;
		}
		else
		{
			this.m_debugCharaDataCount++;
		}
		this.m_debugCharaDataInfo = string.Empty;
		this.m_debugCharaDataSort = sort;
		ServerPlayerState playerState = ServerInterface.PlayerState;
		this.m_debugCharaList = playerState.GetCharacterStateList(this.m_debugCharaDataSort, false, this.m_debugCharaDataCount);
		Dictionary<CharaType, ServerCharacterState>.KeyCollection keys = this.m_debugCharaList.Keys;
		foreach (CharaType current in keys)
		{
			ServerCharacterState serverCharacterState = this.m_debugCharaList[current];
			CharacterDataNameInfo.Info charaInfo = this.m_debugCharaList[current].charaInfo;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Empty + charaInfo.m_name);
			stringBuilder.Append("\n Lv:" + serverCharacterState.Level);
			stringBuilder.Append(" ☆" + serverCharacterState.star);
			stringBuilder.Append("\n" + charaInfo.m_attribute);
			stringBuilder.Append("\n" + charaInfo.m_teamAttribute);
			stringBuilder.Append("\n IsUnlock:" + serverCharacterState.IsUnlocked);
			this.m_debugCharaDataList.Add(stringBuilder.ToString());
		}
	}

	private void GuiEtc()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("title") != -1)
		{
			return;
		}
		if (this.m_camera != null)
		{
			if (Time.timeScale < 5f)
			{
				if (GUI.Button(this.CreateGuiRect(new Rect(0f, 0f, 65f, 20f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_BOTTOM), "hi speed"))
				{
					Time.timeScale = 5f;
				}
			}
			else if (GUI.Button(this.CreateGuiRect(new Rect(0f, 0f, 65f, 20f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_BOTTOM), "reset"))
			{
				Time.timeScale = 1f;
			}
		}
	}

	private void GuiMainMenu()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("title") != -1)
		{
			return;
		}
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("playingstage") != -1)
		{
			return;
		}
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("MainMenu") == -1)
		{
			return;
		}
		if (RouletteManager.IsRouletteEnabled())
		{
			this.m_debugMenu = false;
			this.m_debugMenuType = DebugGameObject.DEBUG_MENU_TYPE.NONE;
			this.m_debugMenuRankingCateg = DebugGameObject.DEBUG_MENU_RANKING_CATEGORY.CACHE;
			this.m_debugMenuRankingType = RankingUtil.RankingRankerType.COUNT;
			this.m_debugMenuItemNum = 1;
			this.m_debugMenuItemPage = 0;
			this.m_debugMenuMileageEpi = 2;
			this.m_debugMenuMileageCha = 1;
			return;
		}
	}

	private void DebugPlayingCurrentScoreCheck()
	{
		this.m_debugScoreText = string.Empty;
		bool flag = false;
		if (EventManager.Instance != null && EventManager.Instance.IsSpecialStage())
		{
			flag = true;
		}
		StageScoreManager instance = StageScoreManager.Instance;
		if (instance != null)
		{
			long num = 0L;
			if (flag)
			{
				num = instance.SpecialCrystal;
			}
			else
			{
				num = instance.GetRealtimeScore();
			}
			if (num > 0L)
			{
				RankingManager instance2 = SingletonGameObject<RankingManager>.Instance;
				if (instance2 != null)
				{
					bool flag2 = false;
					long num2 = 0L;
					long num3 = 0L;
					int num4 = 0;
					int currentHighScoreRank = RankingManager.GetCurrentHighScoreRank(RankingUtil.RankingMode.ENDLESS, flag, ref num, out flag2, out num2, out num3, out num4);
					this.m_debugScoreText = string.Concat(new object[]
					{
						"isSpStage:",
						flag,
						" score:",
						num
					});
					string debugScoreText = this.m_debugScoreText;
					this.m_debugScoreText = string.Concat(new object[]
					{
						debugScoreText,
						"\n rank:",
						currentHighScoreRank,
						" nextRank:",
						num4,
						" isHighScore:",
						flag2
					});
					debugScoreText = this.m_debugScoreText;
					this.m_debugScoreText = string.Concat(new object[]
					{
						debugScoreText,
						"\n nextScore:",
						num2,
						" prveScore:",
						num3
					});
				}
			}
		}
	}

	private void GuiPlayingStageBtn()
	{
		if (this.m_debugScore)
		{
			float num = 60f;
			float num2 = 220f;
			Rect rect = this.CreateGuiRect(new Rect(0f, -20f, num2 + 10f, num + 10f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_BOTTOM);
			if (this.m_debugScoreDelay <= 0f)
			{
				this.DebugPlayingCurrentScoreCheck();
				this.m_debugScoreDelay = 0.2f;
			}
			else
			{
				this.m_debugScoreDelay -= Time.deltaTime;
			}
			GUI.Box(rect, this.m_debugScoreText);
			if (GUI.Button(this.CreateGuiRectInRate(rect, new Rect(0f, -0.01f, 0.95f, 0.32f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_BOTTOM), "close"))
			{
				this.m_debugScore = !this.m_debugScore;
				this.m_debugScoreDelay = 0f;
			}
		}
		else if (GUI.Button(this.CreateGuiRect(new Rect(0f, -80f, 50f, 40f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_BOTTOM), "debug gui\n   off"))
		{
			AllocationStatus.hide = true;
			SingletonGameObject<DebugGameObject>.Remove();
		}
		if (this.m_debugPlay)
		{
			float num3 = 300f;
			float width = 120f;
			Rect rect2 = this.CreateGuiRect(new Rect(0f, 0f, width, num3 + 10f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_BOTTOM);
			GUI.Box(rect2, string.Empty);
			if (this.m_debugPlayType == DebugGameObject.DEBUG_PLAY_TYPE.NONE)
			{
				int num4 = 3;
				float num5 = (num3 - 40f) / num3 / (float)num4;
				float width2 = 0.95f;
				for (int i = 0; i < num4; i++)
				{
					if (GUI.Button(this.CreateGuiRectInRate(rect2, new Rect(0f, 0.02f + num5 * (float)i, width2, num5 * 0.9f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), string.Empty + (DebugGameObject.DEBUG_PLAY_TYPE)i))
					{
						this.m_debugPlayType = (DebugGameObject.DEBUG_PLAY_TYPE)i;
						if (this.m_debugPlayType == DebugGameObject.DEBUG_PLAY_TYPE.BOSS_DESTORY)
						{
							this.m_debugPlayType = DebugGameObject.DEBUG_PLAY_TYPE.NONE;
							MsgBossEnd value = new MsgBossEnd(true);
							GameObjectUtil.SendMessageFindGameObject("GameModeStage", "OnBossEnd", value, SendMessageOptions.DontRequireReceiver);
							MsgUseEquipItem value2 = new MsgUseEquipItem();
							GameObjectUtil.SendMessageFindGameObject("StageItemManager", "OnUseEquipItem", value2, SendMessageOptions.DontRequireReceiver);
						}
					}
				}
				if (GUI.Button(this.CreateGuiRectInRate(rect2, new Rect(0f, -0.02f, 0.833f, 0.1f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_BOTTOM), "close"))
				{
					this.m_debugPlay = false;
					this.m_debugPlayType = DebugGameObject.DEBUG_PLAY_TYPE.NONE;
				}
			}
			else
			{
				DebugGameObject.DEBUG_PLAY_TYPE debugPlayType = this.m_debugPlayType;
				if (debugPlayType != DebugGameObject.DEBUG_PLAY_TYPE.ITEM)
				{
					if (debugPlayType == DebugGameObject.DEBUG_PLAY_TYPE.COLOR)
					{
						this.GuiPlayingStageBtnColor(rect2);
					}
				}
				else
				{
					this.GuiPlayingStageBtnItem(rect2);
				}
			}
		}
		else if (GUI.Button(this.CreateGuiRect(new Rect(0f, 0f, 50f, 40f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_BOTTOM), "debug\n menu"))
		{
			this.m_debugPlay = !this.m_debugPlay;
			this.m_debugPlayType = DebugGameObject.DEBUG_PLAY_TYPE.NONE;
		}
	}

	private void DebugUseItem(ItemType useItem)
	{
		switch (useItem)
		{
		case ItemType.INVINCIBLE:
		case ItemType.BARRIER:
		case ItemType.MAGNET:
		case ItemType.TRAMPOLINE:
		case ItemType.COMBO:
		case ItemType.LASER:
		case ItemType.DRILL:
		case ItemType.ASTEROID:
			global::Debug.Log("debug use:" + useItem);
			break;
		default:
		{
			int min = 0;
			int num = 7;
			int num2 = UnityEngine.Random.Range(min, num + 1);
			useItem = (ItemType)num2;
			global::Debug.Log("debug use:" + useItem);
			break;
		}
		}
		if (useItem != ItemType.UNKNOWN)
		{
			StageItemManager x = UnityEngine.Object.FindObjectOfType<StageItemManager>();
			if (x != null)
			{
				GameObjectUtil.SendMessageFindGameObject("StageItemManager", "OnAddItem", new MsgAddItemToManager(useItem), SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private void GuiPlayingStageBtnItem(Rect target)
	{
		int num = 0;
		int num2 = 4;
		int num3 = num2 - num + 1;
		float height = 0.85f / (float)num3 * 0.95f;
		float num4 = 0.85f / (float)num3;
		int num5 = 0;
		for (int i = num; i <= num2; i++)
		{
			ItemType itemType = (ItemType)i;
			if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0f, 0.02f + (float)num5 * num4, 0.95f, height), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), string.Empty + itemType))
			{
				this.DebugUseItem(itemType);
			}
			num5++;
		}
		if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0f, -0.02f, 0.9f, 0.1f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_BOTTOM), "close"))
		{
			this.m_debugPlayType = DebugGameObject.DEBUG_PLAY_TYPE.NONE;
		}
	}

	private void GuiPlayingStageBtnColor(Rect target)
	{
		int num = 5;
		int num2 = 7;
		int num3 = num2 - num + 1;
		float height = 0.85f / (float)num3 * 0.95f;
		float num4 = 0.85f / (float)num3;
		int num5 = 0;
		for (int i = num; i <= num2; i++)
		{
			ItemType itemType = (ItemType)i;
			if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0f, 0.02f + (float)num5 * num4, 0.95f, height), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), string.Empty + itemType))
			{
				this.DebugUseItem(itemType);
			}
			num5++;
		}
		if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0f, -0.02f, 0.9f, 0.1f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_BOTTOM), "close"))
		{
			this.m_debugPlayType = DebugGameObject.DEBUG_PLAY_TYPE.NONE;
		}
	}

	private void GuiMainMenuBtn()
	{
		if (this.m_debugMenu)
		{
			float num = 250f;
			float num2 = 250f;
			Rect rect = this.CreateGuiRect(new Rect(-100f, 0f, num2, num), DebugGameObject.GUI_RECT_ANCHOR.CENTER_RIGHT);
			GUI.Box(rect, string.Empty);
			if (this.m_debugMenuType == DebugGameObject.DEBUG_MENU_TYPE.NONE)
			{
				int num3 = 6;
				float num4 = num / (float)num3;
				float num5 = num2 - 10f;
				float num6 = num4 - 10f;
				float num7 = num5;
				float num8 = num * -0.5f - num4 * 0.5f;
				this.m_debugMenuRankingCurrentRank = 0;
				for (int i = 0; i < num3; i++)
				{
					DebugGameObject.DEBUG_MENU_TYPE dEBUG_MENU_TYPE = (DebugGameObject.DEBUG_MENU_TYPE)i;
					if (dEBUG_MENU_TYPE == DebugGameObject.DEBUG_MENU_TYPE.DEBUG_GUI_OFF)
					{
						num8 += num4 * 0.125f;
						num6 *= 0.75f;
						num7 *= 0.75f;
					}
					if (GUI.Button(this.CreateGuiRectInRate(rect, new Rect(0f, (num8 + num4 * (float)(i + 1)) / num, num7 / num2, num6 / num), DebugGameObject.GUI_RECT_ANCHOR.CENTER), string.Empty + dEBUG_MENU_TYPE))
					{
						this.m_debugMenuType = dEBUG_MENU_TYPE;
						this.m_debugMenuRankingCateg = DebugGameObject.DEBUG_MENU_RANKING_CATEGORY.CACHE;
						this.m_debugMenuRankingType = RankingUtil.RankingRankerType.COUNT;
						this.m_debugMenuItemNum = 1;
						this.m_debugMenuItemPage = 0;
						this.m_debugMenuMileageEpi = 2;
						this.m_debugMenuMileageCha = 1;
						if (this.m_debugMenuType == DebugGameObject.DEBUG_MENU_TYPE.ITEM)
						{
							if (this.m_debugMenuItemList != null)
							{
								this.m_debugMenuItemList.Clear();
							}
							this.m_debugMenuCharaList = new List<CharacterDataNameInfo.Info>();
							this.m_debugMenuItemList = new Dictionary<DebugGameObject.DEBUG_MENU_ITEM_CATEGORY, List<int>>();
							List<int> list = new List<int>();
							List<int> list2 = new List<int>();
							List<int> list3 = new List<int>();
							list.Add(120000);
							list.Add(120001);
							list.Add(120002);
							list.Add(120003);
							list.Add(120004);
							list.Add(120005);
							list.Add(120006);
							list.Add(120007);
							list.Add(220000);
							list.Add(900000);
							list.Add(910000);
							list.Add(920000);
							list.Add(240000);
							list.Add(230000);
							ServerPlayerState playerState = ServerInterface.PlayerState;
							DataTable.ChaoData[] dataTable = ChaoTable.GetDataTable();
							this.m_debugMenuOtomoList = new List<DataTable.ChaoData>();
							List<int> list4 = new List<int>();
							List<ServerChaoState> list5 = null;
							if (playerState != null && playerState.ChaoStates != null)
							{
								list5 = playerState.ChaoStates;
							}
							if (dataTable != null && dataTable.Length > 0)
							{
								DataTable.ChaoData[] array = dataTable;
								for (int j = 0; j < array.Length; j++)
								{
									DataTable.ChaoData chaoData = array[j];
									if (chaoData.rarity != DataTable.ChaoData.Rarity.NONE && list5 != null && list5.Count > 0)
									{
										this.m_debugMenuOtomoList.Add(chaoData);
									}
								}
							}
							if (this.m_debugMenuOtomoList != null && this.m_debugMenuOtomoList.Count > 0)
							{
								int num9 = this.m_debugMenuOtomoList.Count;
								for (int k = 0; k < num9; k++)
								{
									int item = this.m_debugMenuOtomoList[k].id + 400000;
									if (!list4.Contains(item))
									{
										list2.Add(item);
									}
								}
							}
							if (list4.Count > 0)
							{
								foreach (int current in list4)
								{
									list2.Add(current + 100000);
								}
							}
							if (playerState != null)
							{
								int num9 = 29;
								for (int l = 0; l < num9; l++)
								{
									CharaType charaType = (CharaType)l;
									ServerCharacterState serverCharacterState = playerState.CharacterState(charaType);
									if (serverCharacterState != null)
									{
										CharacterDataNameInfo.Info dataByID = CharacterDataNameInfo.Instance.GetDataByID(charaType);
										if (dataByID != null)
										{
											list3.Add(dataByID.m_serverID);
											this.m_debugMenuCharaList.Add(dataByID);
										}
									}
								}
							}
							this.m_debugMenuItemList.Add(DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.ITEM, list);
							this.m_debugMenuItemList.Add(DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.OTOMO, list2);
							this.m_debugMenuItemList.Add(DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.CHARACTER, list3);
						}
						else if (this.m_debugMenuType == DebugGameObject.DEBUG_MENU_TYPE.RANKING)
						{
							if (SingletonGameObject<RankingManager>.Instance != null)
							{
								RankingUtil.Ranker myRank = RankingManager.GetMyRank(RankingUtil.RankingMode.ENDLESS, RankingUtil.RankingRankerType.RIVAL, RankingManager.EndlessRivalRankingScoreType);
								if (myRank != null)
								{
									this.m_debugMenuRankingCurrentRank = myRank.rankIndex + 1;
									this.m_debugMenuRankingCurrentDummyRank = this.m_debugMenuRankingCurrentRank;
									this.m_debugMenuRankingCurrentLegMax = RankingManager.GetCurrentMyLeagueMax(RankingUtil.RankingMode.ENDLESS);
								}
							}
						}
						else if (this.m_debugMenuType == DebugGameObject.DEBUG_MENU_TYPE.DEBUG_GUI_OFF)
						{
							this.m_debugMenu = !this.m_debugMenu;
							this.m_debugMenuType = DebugGameObject.DEBUG_MENU_TYPE.NONE;
							this.m_debugMenuRankingCateg = DebugGameObject.DEBUG_MENU_RANKING_CATEGORY.CACHE;
							this.m_debugMenuRankingType = RankingUtil.RankingRankerType.COUNT;
							AllocationStatus.hide = true;
							SingletonGameObject<DebugGameObject>.Remove();
						}
						else if (this.m_debugMenuType == DebugGameObject.DEBUG_MENU_TYPE.CHAO_TEX_RELEASE)
						{
							this.m_debugMenu = !this.m_debugMenu;
							this.m_debugMenuType = DebugGameObject.DEBUG_MENU_TYPE.NONE;
							this.m_debugMenuRankingCateg = DebugGameObject.DEBUG_MENU_RANKING_CATEGORY.CACHE;
							this.m_debugMenuRankingType = RankingUtil.RankingRankerType.COUNT;
							SingletonGameObject<RankingManager>.Instance.ResetChaoTexture();
							ChaoTextureManager.Instance.RemoveChaoTextureForMainMenuEnd();
						}
						break;
					}
				}
			}
			else
			{
				switch (this.m_debugMenuType)
				{
				case DebugGameObject.DEBUG_MENU_TYPE.ITEM:
					this.GuiMainMenuBtnItem(rect);
					break;
				case DebugGameObject.DEBUG_MENU_TYPE.MILEAGE:
					this.GuiMainMenuBtnMile(rect);
					break;
				case DebugGameObject.DEBUG_MENU_TYPE.RANKING:
					this.GuiMainMenuBtnRanking(rect);
					break;
				case DebugGameObject.DEBUG_MENU_TYPE.DAILY_BATTLE:
					if (SingletonGameObject<DailyBattleManager>.Instance != null)
					{
					}
					break;
				}
			}
			if (GUI.Button(this.CreateGuiRect(new Rect(0f, 0f, 50f, 40f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_RIGHT), "close"))
			{
				if (this.m_debugMenuType == DebugGameObject.DEBUG_MENU_TYPE.NONE)
				{
					this.m_debugMenu = !this.m_debugMenu;
				}
				else if (this.m_debugMenuRankingCateg == DebugGameObject.DEBUG_MENU_RANKING_CATEGORY.CACHE && this.m_debugMenuRankingType != RankingUtil.RankingRankerType.COUNT)
				{
					this.m_debugMenuRankingCateg = DebugGameObject.DEBUG_MENU_RANKING_CATEGORY.CACHE;
					this.m_debugMenuRankingType = RankingUtil.RankingRankerType.COUNT;
					this.m_debugMenuMileageEpi = 2;
					this.m_debugMenuMileageCha = 1;
				}
				else
				{
					this.m_debugMenuRankingCateg = DebugGameObject.DEBUG_MENU_RANKING_CATEGORY.CACHE;
					this.m_debugMenuRankingType = RankingUtil.RankingRankerType.COUNT;
					this.m_debugMenuType = DebugGameObject.DEBUG_MENU_TYPE.NONE;
					this.m_debugMenuMileageEpi = 2;
					this.m_debugMenuMileageCha = 1;
				}
			}
		}
		else if (GUI.Button(this.CreateGuiRect(new Rect(0f, 0f, 50f, 40f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_RIGHT), "debug\n menu"))
		{
			this.m_debugMenu = !this.m_debugMenu;
			this.m_debugMenuType = DebugGameObject.DEBUG_MENU_TYPE.NONE;
			this.m_debugMenuRankingCateg = DebugGameObject.DEBUG_MENU_RANKING_CATEGORY.CACHE;
			this.m_debugMenuRankingType = RankingUtil.RankingRankerType.COUNT;
		}
	}

	private void ChangeLoadLangeAtlas()
	{
		if (this.m_debugAtlasLangList != null && this.m_debugAtlasLangList.Count > 0)
		{
			foreach (UIAtlas current in this.m_debugAtlasLangList)
			{
				string text = this.ChangeAtlasName(current);
				global::Debug.Log("! " + text);
				GameObject gameObject = ResourceManager.Instance.GetGameObject(ResourceCategory.UI, text);
				if (gameObject != null)
				{
					UIAtlas component = gameObject.GetComponent<UIAtlas>();
					if (current != null && component != null)
					{
						current.replacement = component;
						current.name = text;
					}
					global::Debug.Log("!!! " + current.name);
				}
			}
			Resources.UnloadUnusedAssets();
		}
		this.CheckLoadAtlas();
	}

	private void CheckLoadAtlas()
	{
		if (this.m_debugAtlasList != null)
		{
			this.m_debugAtlasList.Clear();
		}
		if (this.m_debugAtlasLangList != null)
		{
			this.m_debugAtlasLangList.Clear();
		}
		this.m_debugAtlasList = new List<UIAtlas>();
		this.m_debugAtlasLangList = new List<UIAtlas>();
		UIAtlas[] array = Resources.FindObjectsOfTypeAll(typeof(UIAtlas)) as UIAtlas[];
		if (array != null && array.Length > 0)
		{
			UIAtlas[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				UIAtlas uIAtlas = array2[i];
				if (this.IsLangAtlas(uIAtlas))
				{
					this.m_debugAtlasLangList.Add(uIAtlas);
				}
				else
				{
					this.m_debugAtlasList.Add(uIAtlas);
				}
			}
		}
	}

	private bool IsLangAtlas(UIAtlas atlas)
	{
		bool result = false;
		if (atlas != null)
		{
			string[] array = atlas.name.Split(new char[]
			{
				'_'
			});
			if (array != null && array.Length > 1)
			{
				string text = array[array.Length - 1];
				if (!string.IsNullOrEmpty(text) && TextUtility.IsSuffix(text))
				{
					result = true;
				}
			}
		}
		return result;
	}

	private string ChangeAtlasName(UIAtlas atlas)
	{
		string text = null;
		if (atlas != null)
		{
			string[] array = atlas.name.Split(new char[]
			{
				'_'
			});
			if (array != null && array.Length > 1)
			{
				string text2 = array[array.Length - 1];
				if (!string.IsNullOrEmpty(text2) && TextUtility.IsSuffix(text2))
				{
					text = string.Empty;
					for (int i = 0; i < array.Length - 1; i++)
					{
						text = text + array[i] + "_";
					}
					text += this.m_debugAtlasLangCode;
				}
			}
		}
		return text;
	}

	private void CheckDrawCall2D()
	{
		GameObject gameObject = GameObject.Find("UI Root (2D)");
		if (gameObject != null)
		{
			List<UIPanel> list = new List<UIPanel>(gameObject.GetComponentsInChildren<UIPanel>());
			if (list.Count > 0)
			{
				this.m_debugDrawCallList = new Dictionary<string, Dictionary<string, List<UIDrawCall>>>();
				for (int i = 0; i < list.Count; i++)
				{
					UIPanel uIPanel = list[i];
					if (uIPanel != null)
					{
						int num = 0;
						Dictionary<string, List<UIDrawCall>> dictionary = null;
						Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
						for (int j = 0; j < UIDrawCall.list.size; j++)
						{
							UIDrawCall uIDrawCall = UIDrawCall.list[j];
							if (!(uIDrawCall.panel != uIPanel))
							{
								if (dictionary == null)
								{
									dictionary = new Dictionary<string, List<UIDrawCall>>();
								}
								if (!dictionary2.ContainsKey(uIDrawCall.material.name))
								{
									dictionary2.Add(uIDrawCall.material.name, 1);
								}
								else
								{
									Dictionary<string, int> dictionary3;
									Dictionary<string, int> expr_D0 = dictionary3 = dictionary2;
									string name;
									string expr_DF = name = uIDrawCall.material.name;
									int num2 = dictionary3[name];
									expr_D0[expr_DF] = num2 + 1;
								}
								if (dictionary2.ContainsKey(uIDrawCall.material.name))
								{
									string key = uIDrawCall.material.name + " " + dictionary2[uIDrawCall.material.name];
									if (!dictionary.ContainsKey(key))
									{
										dictionary.Add(key, new List<UIDrawCall>
										{
											uIDrawCall
										});
									}
									else
									{
										dictionary[key].Add(uIDrawCall);
									}
								}
								num++;
							}
						}
						if (dictionary != null)
						{
							this.m_debugDrawCallList.Add(uIPanel.name + "  atlas:" + num, dictionary);
						}
					}
				}
			}
			else
			{
				this.m_debugDrawCallList = null;
			}
		}
		else
		{
			this.m_debugDrawCallList = null;
		}
	}

	private void GuiLoadAtlas(Rect target, float sizeW, float sizeH)
	{
		Rect position = this.CreateGuiRectInRate(target, new Rect(0f, 0.007f, 0.5f, 0.056f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
		if (GUI.Button(position, string.Empty + this.m_debugCheckType))
		{
			this.CheckLoadAtlas();
		}
		if (this.m_debugAtlasList != null || this.m_debugAtlasLangList != null)
		{
			int num = 0;
			float num2 = -0.375f;
			float width = 0.24f;
			float num3 = 0.25f;
			float height = 0.06f;
			float num4 = 0.064f;
			foreach (UIAtlas current in this.m_debugAtlasList)
			{
				Rect position2 = this.CreateGuiRectInRate(target, new Rect(num2 + num3 * (float)(num % 4), 0.065f + num4 * (float)(num / 4), width, height), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
				string text = string.Concat(new object[]
				{
					current.name,
					" [",
					current.texture.width,
					"]"
				});
				if (GUI.Button(position2, string.Empty + text))
				{
					int num5 = current.texture.width * current.texture.height;
					int num6 = 0;
					int num7 = 0;
					if (current.spriteList != null)
					{
						num7 = current.spriteList.Count;
					}
					global::Debug.Log(string.Concat(new object[]
					{
						"===================== ",
						text,
						" spriteNum:",
						num7,
						" ========================"
					}));
					if (num7 > 0)
					{
						int num8 = 0;
						string text2 = string.Empty;
						foreach (UISpriteData current2 in current.spriteList)
						{
							if (string.IsNullOrEmpty(text2))
							{
								text2 = string.Concat(new object[]
								{
									string.Empty,
									num8,
									"  ",
									current2.name,
									" [",
									current2.width,
									"×",
									current2.height,
									"]"
								});
							}
							else
							{
								string text3 = text2;
								text2 = string.Concat(new object[]
								{
									text3,
									"\n",
									num8,
									"  ",
									current2.name,
									" [",
									current2.width,
									"×",
									current2.height,
									"]"
								});
							}
							num6 += current2.width * current2.height;
							num8++;
						}
						global::Debug.Log(text2);
					}
					global::Debug.Log("===================================== useArea: " + (float)((int)((float)num6 / (float)num5 * 1000f)) / 10f + "% =============================================");
				}
				num++;
			}
			foreach (UIAtlas current3 in this.m_debugAtlasLangList)
			{
				Rect position3 = this.CreateGuiRectInRate(target, new Rect(num2 + num3 * (float)(num % 4), 0.065f + num4 * (float)(num / 4), width, height), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
				string text4 = string.Concat(new object[]
				{
					current3.name,
					" [",
					current3.texture.width,
					"]"
				});
				if (GUI.Button(position3, string.Empty + text4))
				{
					int num9 = current3.texture.width * current3.texture.height;
					int num10 = 0;
					int num11 = 0;
					if (current3.spriteList != null)
					{
						num11 = current3.spriteList.Count;
					}
					global::Debug.Log(string.Concat(new object[]
					{
						"===================== ",
						text4,
						" spriteNum:",
						num11,
						" ========================"
					}));
					if (num11 > 0)
					{
						string text5 = string.Empty;
						int num12 = 0;
						foreach (UISpriteData current4 in current3.spriteList)
						{
							if (string.IsNullOrEmpty(text5))
							{
								text5 = string.Concat(new object[]
								{
									string.Empty,
									num12,
									"  ",
									current4.name,
									" [",
									current4.width,
									"×",
									current4.height,
									"]"
								});
							}
							else
							{
								string text3 = text5;
								text5 = string.Concat(new object[]
								{
									text3,
									"\n",
									num12,
									"  ",
									current4.name,
									" [",
									current4.width,
									"×",
									current4.height,
									"]"
								});
							}
							num10 += current4.width * current4.height;
							num12++;
						}
						global::Debug.Log(text5);
					}
					global::Debug.Log("===================================== useArea: " + (float)((int)((float)num10 / (float)num9 * 1000f)) / 10f + "% =============================================");
				}
				num++;
			}
		}
	}

	private void GuiDrawCall2D(Rect target, float sizeW, float sizeH)
	{
		Rect position = this.CreateGuiRectInRate(target, new Rect(0f, 0.007f, 0.5f, 0.056f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
		if (this.m_debugDrawCallList != null && this.m_debugDrawCallList.Count > 0)
		{
			if (string.IsNullOrEmpty(this.m_debugDrawCallPanelCurrent))
			{
				if (GUI.Button(position, string.Empty + this.m_debugCheckType))
				{
					this.CheckDrawCall2D();
					this.m_debugDrawCallPanelCurrent = string.Empty;
					this.m_debugDrawCallMatCurrent = string.Empty;
				}
				Dictionary<string, Dictionary<string, List<UIDrawCall>>>.KeyCollection keys = this.m_debugDrawCallList.Keys;
				int num = 0;
				foreach (string current in keys)
				{
					Rect position2 = this.CreateGuiRectInRate(target, new Rect(0f, 0.065f + 0.07f * (float)num, 0.96f, 0.06f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
					if (GUI.Button(position2, string.Empty + current))
					{
						this.m_debugDrawCallPanelCurrent = current;
					}
					num++;
				}
			}
			else if (this.m_debugDrawCallList.ContainsKey(this.m_debugDrawCallPanelCurrent))
			{
				float num2 = -0.25f;
				float num3 = 0.065f;
				if (string.IsNullOrEmpty(this.m_debugDrawCallMatCurrent))
				{
					GUI.Box(position, string.Empty + this.m_debugDrawCallPanelCurrent);
					Dictionary<string, List<UIDrawCall>> dictionary = this.m_debugDrawCallList[this.m_debugDrawCallPanelCurrent];
					Dictionary<string, List<UIDrawCall>>.KeyCollection keys2 = dictionary.Keys;
					int num4 = 0;
					foreach (string current2 in keys2)
					{
						int num5 = num4 % 2;
						int num6 = 0;
						if (num4 >= 2)
						{
							num6 = num4 / 2;
						}
						Rect position3 = this.CreateGuiRectInRate(target, new Rect(num2 + (float)num5 * 0.5f, num3 + 0.06f * (float)num6, 0.48f, 0.057f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
						GUI.Box(position3, string.Empty + current2);
						num4++;
					}
				}
				else
				{
					GUI.Box(position, string.Empty + this.m_debugDrawCallMatCurrent);
					List<UIDrawCall> list = this.m_debugDrawCallList[this.m_debugDrawCallPanelCurrent][this.m_debugDrawCallMatCurrent];
					int num7 = 0;
					foreach (UIDrawCall current3 in list)
					{
						int num8 = num7 % 2;
						int num9 = 0;
						if (num7 >= 2)
						{
							num9 = num7 / 2;
						}
						Rect position4 = this.CreateGuiRectInRate(target, new Rect(num2 + (float)num8 * 0.5f, num3 + 0.06f * (float)num9, 0.48f, 0.057f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
						if (GUI.Button(position4, string.Concat(new object[]
						{
							string.Empty,
							current3.name,
							" ",
							current3.gameObject.activeSelf
						})))
						{
							current3.gameObject.SetActive(!current3.gameObject.activeSelf);
						}
						num7++;
					}
				}
			}
		}
	}

	private void GuiMainMenuBtnMile(Rect target)
	{
		Rect position = this.CreateGuiRectInRate(target, new Rect(0f, 0.04f, 0.44f, 0.08f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
		GUI.Box(position, "Mileage");
		Rect position2 = this.CreateGuiRectInRate(target, new Rect(0f, 0.18f, 0.32f, 0.16f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
		if (GUI.Button(position2, string.Empty + this.m_debugMenuMileageEpi))
		{
			this.m_debugMenuMileageEpi = 2;
		}
		if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(-0.02f, 0.18f, 0.28f, 0.16f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP), ">"))
		{
			this.m_debugMenuMileageEpi++;
		}
		if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0.02f, 0.18f, 0.28f, 0.16f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP), "<"))
		{
			this.m_debugMenuMileageEpi--;
			if (this.m_debugMenuMileageEpi < 2)
			{
				this.m_debugMenuMileageEpi = 2;
			}
		}
		Rect position3 = this.CreateGuiRectInRate(target, new Rect(0f, 0.4f, 0.32f, 0.16f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
		if (GUI.Button(position3, string.Empty + this.m_debugMenuMileageCha))
		{
			this.m_debugMenuMileageCha = 1;
		}
		if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(-0.02f, 0.4f, 0.28f, 0.16f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP), ">"))
		{
			this.m_debugMenuMileageCha++;
		}
		if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0.02f, 0.4f, 0.28f, 0.16f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP), "<"))
		{
			this.m_debugMenuMileageCha--;
			if (this.m_debugMenuMileageCha < 1)
			{
				this.m_debugMenuMileageCha = 1;
			}
		}
		Rect position4 = this.CreateGuiRectInRate(target, new Rect(0f, 0.6f, 0.96f, 0.16f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
		if (GUI.Button(position4, "ok"))
		{
			this.GuiMainMenuBtnMileSetting();
		}
		Rect position5 = this.CreateGuiRectInRate(target, new Rect(0f, 0.78f, 0.96f, 0.08f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
		GUI.Box(position5, "※ back to title!");
	}

	private void GuiMainMenuBtnMileSetting()
	{
		NetDebugUpdMileageData request = new NetDebugUpdMileageData(new ServerMileageMapState
		{
			m_episode = this.m_debugMenuMileageEpi,
			m_chapter = this.m_debugMenuMileageCha,
			m_point = 0,
			m_stageTotalScore = 0L,
			m_numBossAttack = 0,
			m_stageMaxScore = 0L
		});
		base.StartCoroutine(this.NetworkRequest(request, new DebugGameObject.NetworkRequestSuccessCallback(this.AddOpeMessageMileCallback), new DebugGameObject.NetworkRequestFailedCallback(this.NetworkFailedMileCallback)));
	}

	private void AddOpeMessageMileCallback()
	{
		this.m_debugMenuType = DebugGameObject.DEBUG_MENU_TYPE.NONE;
		HudMenuUtility.SendMsgMenuSequenceToMainMenu(MsgMenuSequence.SequeneceType.TITLE);
	}

	private void GuiMainMenuBtnRankingChange(Rect target)
	{
		Rect position = this.CreateGuiRectInRate(target, new Rect(0f, 0.16f, 0.6f, 0.08f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
		if (this.m_debugMenuRankingCurrentRank > 0 && this.m_debugMenuRankingCurrentLegMax > 1)
		{
			GUI.Box(position, "current rank:" + this.m_debugMenuRankingCurrentRank);
			Rect position2 = this.CreateGuiRectInRate(target, new Rect(0f, 0.28f, 0.28f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
			if (GUI.Button(position2, string.Empty + this.m_debugMenuRankingCurrentDummyRank))
			{
				this.m_debugMenuRankingCurrentDummyRank = this.m_debugMenuRankingCurrentRank;
			}
			if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(-0.02f, 0.28f, 0.24f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP), ">"))
			{
				this.m_debugMenuRankingCurrentDummyRank++;
				if (this.m_debugMenuRankingCurrentDummyRank > this.m_debugMenuRankingCurrentLegMax)
				{
					this.m_debugMenuRankingCurrentDummyRank = this.m_debugMenuRankingCurrentLegMax;
				}
			}
			if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0.02f, 0.28f, 0.24f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP), "<"))
			{
				this.m_debugMenuRankingCurrentDummyRank--;
				if (this.m_debugMenuRankingCurrentDummyRank < 1)
				{
					this.m_debugMenuRankingCurrentDummyRank = 1;
				}
			}
			if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0f, 0.48f, 0.76f, 0.16f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), "dummy old rank save"))
			{
				SingletonGameObject<RankingManager>.Instance.SavePlayerRankingDataDummy(RankingUtil.RankingMode.ENDLESS, RankingUtil.RankingRankerType.RIVAL, RankingManager.EndlessRivalRankingScoreType, this.m_debugMenuRankingCurrentDummyRank);
				RankingUI.DebugInitRankingChange();
			}
		}
		else
		{
			GUI.Box(position, "no ranking data");
		}
	}

	private void GuiMainMenuBtnRankingCache(Rect target)
	{
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			RankingUtil.RankingMode rankingMode = RankingUtil.RankingMode.ENDLESS;
			if (this.m_debugMenuRankingType == RankingUtil.RankingRankerType.COUNT)
			{
				int num = 6;
				int num2 = 0;
				float num3 = 0.8f / (float)num;
				float width = 0.92f;
				for (int i = 0; i < num; i++)
				{
					RankingUtil.RankingRankerType rankingRankerType = (RankingUtil.RankingRankerType)i;
					if (SingletonGameObject<RankingManager>.Instance.IsRankingTop(rankingMode, RankingUtil.RankingScoreType.HIGH_SCORE, rankingRankerType) || SingletonGameObject<RankingManager>.Instance.IsRankingTop(rankingMode, RankingUtil.RankingScoreType.TOTAL_SCORE, rankingRankerType))
					{
						if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0f, 0.16f + (float)num2 * num3, width, num3 - 0.01f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), string.Empty + rankingRankerType))
						{
							this.m_debugMenuRankingType = rankingRankerType;
						}
						num2++;
					}
				}
			}
			else
			{
				Rect position = this.CreateGuiRectInRate(target, new Rect(0f, 0.16f, 0.92f, 0.08f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
				Rect rect = this.CreateGuiRectInRate(target, new Rect(0f, 0.26f, 0.92f, 0.32f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
				Rect rect2 = this.CreateGuiRectInRate(target, new Rect(0f, 0.588f, 0.92f, 0.32f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
				Rect position2 = this.CreateGuiRectInRate(target, new Rect(0f, 0.92f, 0.92f, 0.1f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
				GUI.Box(position, string.Empty + this.m_debugMenuRankingType);
				if (GUI.Button(position2, "back"))
				{
					this.m_debugMenuRankingType = RankingUtil.RankingRankerType.COUNT;
				}
				if (SingletonGameObject<RankingManager>.Instance.IsRankingTop(rankingMode, RankingUtil.RankingScoreType.HIGH_SCORE, this.m_debugMenuRankingType))
				{
					this.GuiMainMenuBtnRankingCacheInfo(rect, RankingUtil.RankingScoreType.HIGH_SCORE);
				}
				else
				{
					GUI.Box(rect, "not found");
				}
				if (SingletonGameObject<RankingManager>.Instance.IsRankingTop(rankingMode, RankingUtil.RankingScoreType.TOTAL_SCORE, this.m_debugMenuRankingType))
				{
					this.GuiMainMenuBtnRankingCacheInfo(rect2, RankingUtil.RankingScoreType.TOTAL_SCORE);
				}
				else
				{
					GUI.Box(rect2, "not found");
				}
			}
		}
	}

	private void GuiMainMenuBtnRankingCacheInfo(Rect rect, RankingUtil.RankingScoreType scoreType)
	{
		string text = string.Empty + scoreType;
		List<RankingUtil.Ranker> cacheRankingList = SingletonGameObject<RankingManager>.Instance.GetCacheRankingList(RankingUtil.RankingMode.ENDLESS, scoreType, this.m_debugMenuRankingType);
		if (cacheRankingList != null && cacheRankingList.Count > 1)
		{
			RankingUtil.Ranker ranker = cacheRankingList[0];
			int num = -1;
			int num2 = -1;
			int num3 = 0;
			if (ranker != null)
			{
				text = text + "  myRank:" + (ranker.rankIndex + 1);
			}
			else
			{
				text += "  myRank:---";
			}
			for (int i = 1; i < cacheRankingList.Count; i++)
			{
				if (num == -1)
				{
					num = cacheRankingList[i].rankIndex + 1;
					num2 = -1;
					num3 = 0;
				}
				else if (num + num3 + 1 != cacheRankingList[i].rankIndex + 1)
				{
					num2 = cacheRankingList[i - 1].rankIndex + 1;
				}
				else
				{
					num3++;
				}
				if (num != -1 && num2 != -1)
				{
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"\n",
						num,
						" ～ ",
						num2
					});
					num = cacheRankingList[i].rankIndex + 1;
					num2 = -1;
					num3 = 0;
				}
			}
			if (num != -1 && num2 == -1)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\n",
					num,
					" ～ ",
					cacheRankingList[cacheRankingList.Count - 1].rankIndex + 1
				});
			}
		}
		GUI.Box(rect, text);
	}

	private void GuiMainMenuBtnRanking(Rect target)
	{
		Rect position = this.CreateGuiRectInRate(target, new Rect(0f, 0.04f, 0.44f, 0.08f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
		GUI.Box(position, string.Empty + this.m_debugMenuRankingCateg);
		int num = (int)this.m_debugMenuRankingCateg;
		int num2 = 2;
		if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(-0.01f, 0.02f, 0.24f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP), ">"))
		{
			num = (num + 1 + num2) % num2;
			this.m_debugMenuRankingCateg = (DebugGameObject.DEBUG_MENU_RANKING_CATEGORY)num;
			this.m_debugMenuRankingType = RankingUtil.RankingRankerType.COUNT;
		}
		if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0.01f, 0.02f, 0.24f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP), "<"))
		{
			num = (num - 1 + num2) % num2;
			this.m_debugMenuRankingCateg = (DebugGameObject.DEBUG_MENU_RANKING_CATEGORY)num;
			this.m_debugMenuRankingType = RankingUtil.RankingRankerType.COUNT;
		}
		DebugGameObject.DEBUG_MENU_RANKING_CATEGORY debugMenuRankingCateg = this.m_debugMenuRankingCateg;
		if (debugMenuRankingCateg != DebugGameObject.DEBUG_MENU_RANKING_CATEGORY.CACHE)
		{
			if (debugMenuRankingCateg == DebugGameObject.DEBUG_MENU_RANKING_CATEGORY.CHANGE_TEST)
			{
				this.GuiMainMenuBtnRankingChange(target);
			}
		}
		else
		{
			this.GuiMainMenuBtnRankingCache(target);
		}
	}

	private void GuiMainMenuBtnItem(Rect target)
	{
		Rect position = this.CreateGuiRectInRate(target, new Rect(0f, 0.04f, 0.44f, 0.08f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
		GUI.Box(position, string.Empty + this.m_debugMenuItemSelect);
		int num = (int)this.m_debugMenuItemSelect;
		int num2 = 3;
		if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(-0.01f, 0.02f, 0.24f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP), ">"))
		{
			num = (num + 1 + num2) % num2;
			this.m_debugMenuItemNum = 1;
			this.m_debugMenuItemPage = 0;
			this.m_debugMenuItemSelect = (DebugGameObject.DEBUG_MENU_ITEM_CATEGORY)num;
		}
		if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0.01f, 0.02f, 0.24f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP), "<"))
		{
			num = (num - 1 + num2) % num2;
			this.m_debugMenuItemNum = 1;
			this.m_debugMenuItemPage = 0;
			this.m_debugMenuItemSelect = (DebugGameObject.DEBUG_MENU_ITEM_CATEGORY)num;
		}
		if ((this.m_debugMenuItemSelect == DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.ITEM || this.m_debugMenuItemSelect == DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.OTOMO) && this.m_debugMenuItemList.ContainsKey(this.m_debugMenuItemSelect) && this.m_debugMenuItemList[this.m_debugMenuItemSelect].Count > 0)
		{
			Rect position2 = this.CreateGuiRectInRate(target, new Rect(0f, 0.15f, 0.24f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP);
			if (GUI.Button(position2, string.Empty + this.m_debugMenuItemNum))
			{
				this.m_debugMenuItemNum = 1;
			}
			if (this.m_debugMenuItemSelect == DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.ITEM)
			{
				if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(-0.01f, 0.15f, 0.144f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP), ">>"))
				{
					this.m_debugMenuItemNum += 10;
				}
				if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(-0.18f, 0.15f, 0.16f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP), ">"))
				{
					this.m_debugMenuItemNum++;
				}
				if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0.01f, 0.15f, 0.144f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP), "<<"))
				{
					this.m_debugMenuItemNum -= 10;
					if (this.m_debugMenuItemNum < 1)
					{
						this.m_debugMenuItemNum = 1;
					}
				}
				if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0.18f, 0.15f, 0.16f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP), "<"))
				{
					this.m_debugMenuItemNum--;
					if (this.m_debugMenuItemNum < 1)
					{
						this.m_debugMenuItemNum = 1;
					}
				}
			}
			else if (this.m_debugMenuItemSelect == DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.OTOMO)
			{
				if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(-0.01f, 0.15f, 0.36f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP), ">"))
				{
					this.m_debugMenuItemNum++;
					if (this.m_debugMenuItemNum > 6)
					{
						this.m_debugMenuItemNum = 6;
					}
				}
				if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0.01f, 0.15f, 0.36f, 0.12f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP), "<"))
				{
					this.m_debugMenuItemNum--;
					if (this.m_debugMenuItemNum < 1)
					{
						this.m_debugMenuItemNum = 1;
					}
				}
			}
		}
		if (this.m_debugMenuItemList.ContainsKey(this.m_debugMenuItemSelect) && this.m_debugMenuItemList[this.m_debugMenuItemSelect].Count > 0)
		{
			List<int> list = this.m_debugMenuItemList[this.m_debugMenuItemSelect];
			int num3 = 5;
			float num4 = 0.155f;
			if (this.m_debugMenuItemSelect == DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.ITEM || this.m_debugMenuItemSelect == DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.OTOMO)
			{
				num3 = 4;
				num4 = 0.275f;
				if (list.Count > num3)
				{
					if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(-0.01f, num4, 0.136f, 0.72f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP), ">"))
					{
						this.m_debugMenuItemPage++;
						if (this.m_debugMenuItemPage * num3 >= list.Count)
						{
							this.m_debugMenuItemPage--;
						}
					}
					if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0.01f, num4, 0.136f, 0.72f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP), "<"))
					{
						this.m_debugMenuItemPage--;
						if (this.m_debugMenuItemPage < 0)
						{
							this.m_debugMenuItemPage = 0;
						}
					}
				}
			}
			else if (list.Count > num3)
			{
				if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(-0.01f, num4, 0.136f, 0.84f), DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP), ">"))
				{
					this.m_debugMenuItemPage++;
					if (this.m_debugMenuItemPage * num3 >= list.Count)
					{
						this.m_debugMenuItemPage--;
					}
				}
				if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0.01f, num4, 0.136f, 0.84f), DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP), "<"))
				{
					this.m_debugMenuItemPage--;
					if (this.m_debugMenuItemPage < 0)
					{
						this.m_debugMenuItemPage = 0;
					}
				}
			}
			for (int i = 0; i < num3; i++)
			{
				int num5 = i + this.m_debugMenuItemPage * num3;
				if (num5 >= list.Count)
				{
					break;
				}
				int num6 = list[num5];
				int num7 = i;
				bool flag = true;
				string text = num6.ToString();
				if (this.m_debugMenuItemSelect == DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.ITEM)
				{
					text = string.Empty + (ServerItem.Id)num6;
				}
				else if (this.m_debugMenuItemSelect == DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.OTOMO)
				{
					if (this.m_debugMenuOtomoList != null && this.m_debugMenuOtomoList.Count > num5)
					{
						text = this.m_debugMenuOtomoList[num5].name;
						if (this.m_debugMenuOtomoList[num5].id + 400000 != num6)
						{
							flag = false;
							text += "\n Unimplemented";
						}
						else
						{
							int level = this.m_debugMenuOtomoList[num5].level;
							if (level >= 0)
							{
								text = text + "\n Lv:" + level;
							}
							else
							{
								text += "\n not have";
							}
						}
					}
				}
				else if (this.m_debugMenuItemSelect == DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.CHARACTER)
				{
					text = "[" + num6 + "]";
					if (this.m_debugMenuCharaList != null && this.m_debugMenuCharaList.Count > num5)
					{
						text = this.m_debugMenuCharaList[num5].m_name;
					}
				}
				if (flag)
				{
					if (GUI.Button(this.CreateGuiRectInRate(target, new Rect(0f, num4 + (float)num7 * 0.173f, 0.62f, 0.16f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), text))
					{
						if (this.m_debugMenuItemSelect == DebugGameObject.DEBUG_MENU_ITEM_CATEGORY.OTOMO && this.m_debugMenuItemNum > 1)
						{
							for (int j = 0; j < this.m_debugMenuItemNum - 1; j++)
							{
								this.DebugRequestGiftItem(num6, 1, null, false);
							}
							this.DebugRequestGiftItem(num6, 1, null, true);
						}
						else
						{
							this.DebugRequestGiftItem(num6, this.m_debugMenuItemNum, null, true);
						}
					}
				}
				else
				{
					GUI.Box(this.CreateGuiRectInRate(target, new Rect(0f, num4 + (float)num7 * 0.173f, 0.62f, 0.16f), DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP), text);
				}
			}
		}
	}

	private void GuiRoulette()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("title") != -1 || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("playingstage") != -1)
		{
			return;
		}
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("MainMenu") == -1)
		{
			return;
		}
	}

	private void GuiPlayerCharaList()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("title") != -1 || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("playingstage") != -1)
		{
			return;
		}
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("MainMenu") == -1)
		{
			return;
		}
	}

	private void GuiRaid()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("title") != -1 || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("playingstage") != -1)
		{
			return;
		}
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("MainMenu") == -1)
		{
			return;
		}
	}

	private void GuiRanking()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("title") != -1 || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("playingstage") != -1)
		{
			return;
		}
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("MainMenu") == -1)
		{
			return;
		}
	}

	private void GuiCurrentScoreTest()
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("title") != -1 || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("playingstage") != -1)
		{
			return;
		}
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.IndexOf("MainMenu") == -1)
		{
			return;
		}
		if (this.m_currentScore >= 0 || this.m_currentScoreEvent >= 0)
		{
			RankingManager instance = SingletonGameObject<RankingManager>.Instance;
			if (instance != null)
			{
				if (this.m_currentScore >= 0 && GUI.Button(new Rect(30f, 110f, 150f, 40f), string.Concat(new object[]
				{
					"Current ",
					this.targetRankingRankerType,
					"\n",
					this.m_currentScore
				})))
				{
					RankingUtil.DebugCurrentRanking(false, (long)this.m_currentScore);
				}
				if (this.m_currentScoreEvent >= 0 && GUI.Button(new Rect(30f, 165f, 150f, 40f), "Current Rank Event\n" + this.m_currentScoreEvent))
				{
					RankingUtil.DebugCurrentRanking(true, (long)this.m_currentScoreEvent);
				}
			}
		}
	}

	private void Update()
	{
		this.m_currentTimeScale = Time.timeScale;
		if (this.m_debugRouletteTime > 0f)
		{
			this.m_debugRouletteTime -= Time.deltaTime;
			if (this.m_debugRouletteTime <= 0f)
			{
				if (this.m_rouletteCommitMsg != null && !this.m_debugRouletteConectError)
				{
					this.m_rouletteCallback.SendMessage("ServerCommitWheelSpinGeneral_Succeeded", this.m_rouletteCommitMsg);
				}
				else
				{
					MsgServerConnctFailed value = new MsgServerConnctFailed(ServerInterface.StatusCode.AlreadyEndEvent);
					this.m_rouletteCallback.SendMessage("ServerCommitWheelSpinGeneral_Failed", value);
					this.m_debugRouletteConectError = false;
				}
				this.m_debugRouletteTime = 0f;
			}
		}
		if (this.m_debugGetRouletteTime > 0f)
		{
			this.m_debugGetRouletteTime -= Time.deltaTime;
			if (this.m_debugGetRouletteTime <= 0f)
			{
				if (this.m_rouletteGetMsg != null && !this.m_debugRouletteConectError)
				{
					this.m_rouletteCallback.SendMessage("ServerGetWheelOptionsGeneral_Succeeded", this.m_rouletteGetMsg);
				}
				else
				{
					MsgServerConnctFailed value2 = new MsgServerConnctFailed(ServerInterface.StatusCode.AlreadyEndEvent);
					this.m_rouletteCallback.SendMessage("ServerWheelOptionsGeneral_Failed", value2);
					this.m_debugRouletteConectError = false;
				}
				this.m_debugGetRouletteTime = 0f;
			}
		}
		base.enabled = false;
	}

	private void ShowUpdCost()
	{
		if (this.m_currentUpdCost != null && this.m_keys != null)
		{
			if (this.m_updateCostList == null)
			{
				this.m_updateCostList = new List<string>();
			}
			int num = this.m_keys.Count - this.m_updateCostList.Count;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					this.m_updateCostList.Add("---");
				}
			}
			int num2 = 0;
			foreach (string current in this.m_keys)
			{
				if (num2 >= this.m_updateCostList.Count)
				{
					break;
				}
				this.m_updateCostList[num2] = current + " : " + this.m_updCost[current];
				num2++;
			}
		}
	}

	private bool SetUpdCost(string name, float rate)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		bool result = false;
		if (this.m_currentUpdCost != null && this.m_updCost != null && this.m_currentUpdCost.ContainsKey(name) && this.m_updCost.ContainsKey(name))
		{
			int num = 0;
			if (this.m_currentUpdCost[name] > 0)
			{
				num = Mathf.FloorToInt((float)this.m_currentUpdCost[name] / 2f * rate);
			}
			if (name == "TOTAL_COST")
			{
				this.m_updateCost = num;
			}
			this.m_updCost[name] = num;
			this.m_currentUpdCost[name] = 0;
		}
		return result;
	}

	private bool CheckDummyRequest()
	{
		bool result = true;
		if (UnityEngine.Random.Range(0, 100) < this.m_rouletteDummyError)
		{
			result = false;
		}
		return result;
	}

	private void DummyRequestWheelOptionsItem(RouletteCategory cate, int rank, ref ServerWheelOptionsGeneral wheel, int max = 8)
	{
		if (wheel != null)
		{
			List<ServerItem.Id> list;
			switch (cate)
			{
			case RouletteCategory.PREMIUM:
				list = this.m_rouletteDataPremium;
				goto IL_76;
			case RouletteCategory.ITEM:
				list = this.m_rouletteDataItem;
				goto IL_76;
			case RouletteCategory.RAID:
				list = this.m_rouletteDataRaid;
				goto IL_76;
			case RouletteCategory.SPECIAL:
				list = this.m_rouletteDataSpecial;
				goto IL_76;
			}
			list = this.m_rouletteDataDefault;
			IL_76:
			for (int i = 0; i < max; i++)
			{
				int itemId = 120000;
				int num = 1;
				int weight = 1;
				if (list != null && list.Count > 0)
				{
					int num2 = (i + max * rank) % list.Count;
					if (num2 < 0)
					{
						num2 = 0;
					}
					itemId = (int)list[num2];
					if (list[num2] == ServerItem.Id.RING)
					{
						num = 1000 * (rank + 1);
					}
					else if (list[num2] == ServerItem.Id.RSRING)
					{
						num = 10 * (rank + 1);
					}
					else if (list[num2] == ServerItem.Id.ENERGY)
					{
						num = 2 * (rank + 1);
					}
					else if (list[num2] == ServerItem.Id.INVINCIBLE || list[num2] == ServerItem.Id.COMBO || list[num2] == ServerItem.Id.BARRIER || list[num2] == ServerItem.Id.MAGNET || list[num2] == ServerItem.Id.TRAMPOLINE || list[num2] == ServerItem.Id.DRILL)
					{
						num = 1 * (rank + 1);
					}
					if (num < 1)
					{
						num = 1;
					}
				}
				wheel.SetupItem(i, itemId, weight, num);
			}
		}
	}

	private ServerWheelOptionsGeneral DummyRequestWheelOptionsGeneral(int rouletteId, int rank, int costItemId, int costItemNum, int costItemStock)
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		list.Add(costItemId);
		list2.Add(costItemNum);
		list3.Add(costItemStock);
		return this.DummyRequestWheelOptionsGeneral(rouletteId, rank, list, list2, list3);
	}

	private ServerWheelOptionsGeneral DummyRequestWheelOptionsGeneral(int rouletteId, int rank, List<int> costItemId, List<int> costItemNum, List<int> costItemStock)
	{
		ServerWheelOptionsGeneral serverWheelOptionsGeneral = new ServerWheelOptionsGeneral();
		int remaining = 0;
		RouletteCategory rouletteCategory = (RouletteCategory)rouletteId;
		DateTime nextFree = DateTime.Now.AddDays(999.0);
		if (rank > 0)
		{
			remaining = 1;
		}
		if (rouletteCategory == RouletteCategory.ITEM)
		{
			remaining = 5;
		}
		else if (rank > 0)
		{
			remaining = 1;
		}
		if (rouletteCategory == RouletteCategory.PREMIUM || rouletteCategory == RouletteCategory.SPECIAL)
		{
			rank = 0;
		}
		if (rouletteCategory == RouletteCategory.PREMIUM && RouletteManager.Instance != null && RouletteManager.Instance.specialEgg >= 10)
		{
			rouletteCategory = RouletteCategory.SPECIAL;
			rouletteId = 8;
		}
		this.DummyRequestWheelOptionsItem(rouletteCategory, rank, ref serverWheelOptionsGeneral, 8);
		serverWheelOptionsGeneral.SetupParam(rouletteId, remaining, 9999999, rank, 5, nextFree);
		serverWheelOptionsGeneral.ResetupCostItem();
		for (int i = 0; i < costItemId.Count; i++)
		{
			int num = costItemId[i];
			int oneCost = costItemNum[i];
			int itemNum = costItemStock[i];
			if (num > 0)
			{
				serverWheelOptionsGeneral.AddCostItem(num, itemNum, oneCost);
			}
		}
		return serverWheelOptionsGeneral;
	}

	public void DummyRequestGetRouletteGeneral(int eventId, int rouletteId, GameObject callbackObject)
	{
		if (callbackObject != null)
		{
			this.m_rouletteGetMsg = new MsgGetWheelOptionsGeneralSucceed();
			int costItemId;
			switch (rouletteId)
			{
			case 1:
			case 8:
				costItemId = 900000;
				goto IL_6B;
			case 2:
				costItemId = 910000;
				goto IL_6B;
			}
			costItemId = 960000;
			IL_6B:
			this.m_rouletteCallback = callbackObject;
			this.m_rouletteGetMsg.m_wheelOptionsGeneral = this.DummyRequestWheelOptionsGeneral(rouletteId, 0, costItemId, 5, 100);
			this.m_debugGetRouletteTime = this.m_rouletteConectTime;
			this.m_debugRouletteConectError = !this.CheckDummyRequest();
			if (this.m_debugRouletteConectError)
			{
				this.m_debugGetRouletteTime = this.m_rouletteConectTime * 3f;
			}
		}
	}

	public void DummyRequestCommitRouletteGeneral(ServerWheelOptionsGeneral org, int eventId, int rouletteId, int spinCostItemId, int spinNum, GameObject callbackObject)
	{
		if (this.m_debugRouletteTime > 0f)
		{
			this.m_rouletteCommitMsg = null;
			return;
		}
		if (callbackObject != null)
		{
			bool flag = false;
			MsgCommitWheelSpinGeneralSucceed msgCommitWheelSpinGeneralSucceed = new MsgCommitWheelSpinGeneralSucceed();
			ServerSpinResultGeneral serverSpinResultGeneral = new ServerSpinResultGeneral();
			List<ServerItemState> list = new List<ServerItemState>();
			List<ServerChaoData> list2 = new List<ServerChaoData>();
			int num = 0;
			int rank = 0;
			int num2 = 0;
			List<int> list3 = new List<int>();
			if (spinNum <= 1)
			{
				for (int i = 0; i < org.itemLenght; i++)
				{
					list3.Add(i);
				}
				num = list3[UnityEngine.Random.Range(0, list3.Count)];
				int num3;
				int num4;
				float num5;
				org.GetCell(num, out num3, out num4, out num5);
				if (num3 == 200000 || num3 == 200001)
				{
					rank = (int)(org.rank + 1);
				}
				else
				{
					ServerPlayerState playerState = ServerInterface.PlayerState;
					if (num3 >= 400000 && num3 < 500000)
					{
						ServerChaoData serverChaoData = new ServerChaoData();
						serverChaoData.Id = num3;
						serverChaoData.Level = 0;
						serverChaoData.Rarity = 100;
						bool flag2 = false;
						ServerChaoState serverChaoState = playerState.ChaoStateByItemID(num3);
						if (serverChaoState != null)
						{
							switch (serverChaoState.Status)
							{
							case ServerChaoState.ChaoStatus.NotOwned:
								serverChaoData.Level = 0;
								break;
							case ServerChaoState.ChaoStatus.Owned:
								serverChaoData.Level = serverChaoState.Level + 1;
								break;
							case ServerChaoState.ChaoStatus.MaxLevel:
								serverChaoData.Level = 5;
								flag2 = true;
								break;
							}
							if (flag2)
							{
								ServerItemState serverItemState = new ServerItemState();
								serverItemState.m_itemId = 220000;
								serverItemState.m_num = 4;
								num2 += 4;
								list.Add(serverItemState);
							}
						}
						serverChaoData.Rarity = num3 / 1000 % 10;
						list2.Add(serverChaoData);
					}
					else if (num3 >= 300000 && num3 < 400000)
					{
						list2.Add(new ServerChaoData
						{
							Id = num3,
							Level = 0,
							Rarity = 100
						});
						ServerCharacterState serverCharacterState = playerState.CharacterStateByItemID(num3);
						if (serverCharacterState != null && serverCharacterState.Id >= 0 && serverCharacterState.IsUnlocked)
						{
							list.Add(new ServerItemState
							{
								m_itemId = 900000,
								m_num = 99
							});
							list.Add(new ServerItemState
							{
								m_itemId = 910000,
								m_num = 1234
							});
							ServerItemState serverItemState2 = new ServerItemState();
							serverItemState2.m_itemId = 220000;
							serverItemState2.m_num = 5;
							num2 += 5;
							list.Add(serverItemState2);
						}
					}
					else
					{
						list.Add(new ServerItemState
						{
							m_itemId = num3,
							m_num = num4
						});
					}
				}
			}
			else if (org.rank == RouletteUtility.WheelRank.Normal)
			{
				num = -1;
				List<int> list4 = new List<int>();
				List<int> list5 = new List<int>();
				list4.Add(910000);
				list4.Add(120000);
				list4.Add(120001);
				list4.Add(120003);
				list4.Add(220000);
				list4.Add(120004);
				list4.Add(120005);
				list4.Add(120006);
				list4.Add(120007);
				list4.Add(220000);
				list4.Add(900000);
				list5.Add(400000);
				list5.Add(400001);
				list5.Add(400002);
				list5.Add(300000);
				list5.Add(400003);
				list5.Add(400019);
				list5.Add(300004);
				list5.Add(401000);
				list5.Add(401001);
				list5.Add(401002);
				list5.Add(300001);
				list5.Add(401003);
				list5.Add(401004);
				list5.Add(300005);
				int num6 = UnityEngine.Random.Range(0, list4.Count);
				for (int j = 0; j < spinNum; j++)
				{
					ServerItemState serverItemState3 = new ServerItemState();
					serverItemState3.m_itemId = list4[(j + num6) % list4.Count];
					serverItemState3.m_num = 1;
					if (serverItemState3.m_itemId == 220000)
					{
						num2++;
					}
					list.Add(serverItemState3);
				}
				num6 = UnityEngine.Random.Range(0, list5.Count);
				for (int k = 0; k < spinNum; k++)
				{
					ServerChaoData serverChaoData2 = new ServerChaoData();
					serverChaoData2.Id = list5[(k + num6) % list5.Count];
					serverChaoData2.Level = 5;
					if (serverChaoData2.Id >= 300000 && serverChaoData2.Id < 400000)
					{
						serverChaoData2.Level = 0;
						serverChaoData2.Rarity = 100;
						list2.Add(serverChaoData2);
					}
					else
					{
						serverChaoData2.Rarity = serverChaoData2.Id / 1000 % 10;
						list2.Add(serverChaoData2);
						list2.Add(serverChaoData2);
						list2.Add(serverChaoData2);
						list2.Add(serverChaoData2);
						list2.Add(serverChaoData2);
						list2.Add(serverChaoData2);
					}
				}
			}
			else
			{
				flag = true;
			}
			for (int l = 0; l < list.Count; l++)
			{
				serverSpinResultGeneral.AddItemState(list[l]);
			}
			for (int m = 0; m < list2.Count; m++)
			{
				serverSpinResultGeneral.AddChaoState(list2[m]);
			}
			serverSpinResultGeneral.ItemWon = num;
			this.m_rouletteCallback = callbackObject;
			int costItemId;
			switch (org.rouletteId)
			{
			case 1:
			case 8:
				costItemId = 900000;
				goto IL_62D;
			case 2:
				costItemId = 910000;
				goto IL_62D;
			}
			costItemId = 960000;
			IL_62D:
			ServerWheelOptionsGeneral serverWheelOptionsGeneral = this.DummyRequestWheelOptionsGeneral(org.rouletteId, rank, costItemId, 5, 100);
			serverWheelOptionsGeneral.spEgg = RouletteManager.Instance.specialEgg + num2;
			if (!flag)
			{
				msgCommitWheelSpinGeneralSucceed.m_playerState = ServerInterface.PlayerState;
				msgCommitWheelSpinGeneralSucceed.m_wheelOptionsGeneral = serverWheelOptionsGeneral;
				msgCommitWheelSpinGeneralSucceed.m_resultSpinResultGeneral = serverSpinResultGeneral;
				this.m_rouletteCommitMsg = msgCommitWheelSpinGeneralSucceed;
				this.m_debugRouletteTime = this.m_rouletteConectTime;
				this.m_debugRouletteConectError = !this.CheckDummyRequest();
				if (this.m_debugRouletteConectError)
				{
					this.m_debugRouletteTime = 3f;
				}
			}
			else
			{
				this.m_rouletteCommitMsg = null;
				this.m_debugRouletteTime = 0.1f;
				this.m_debugRouletteConectError = true;
			}
		}
	}

	public ServerSpinResultGeneral DummyRouletteGeneralResult(int spinNum)
	{
		ServerSpinResultGeneral serverSpinResultGeneral = new ServerSpinResultGeneral();
		List<ServerItemState> list = new List<ServerItemState>();
		List<ServerChaoData> list2 = new List<ServerChaoData>();
		int itemWon;
		if (spinNum <= 1)
		{
			itemWon = 1;
			list.Add(new ServerItemState
			{
				m_itemId = 120001,
				m_num = 1
			});
		}
		else
		{
			itemWon = -1;
			List<int> list3 = new List<int>();
			List<int> list4 = new List<int>();
			list3.Add(910000);
			list3.Add(120000);
			list3.Add(120001);
			list3.Add(120003);
			list3.Add(220000);
			list3.Add(120004);
			list3.Add(120005);
			list3.Add(120006);
			list3.Add(120007);
			list3.Add(220000);
			list3.Add(900000);
			list4.Add(400000);
			list4.Add(400001);
			list4.Add(400002);
			list4.Add(300000);
			list4.Add(400003);
			list4.Add(400019);
			list4.Add(300004);
			list4.Add(401000);
			list4.Add(401001);
			list4.Add(401002);
			list4.Add(300001);
			list4.Add(401003);
			list4.Add(401004);
			list4.Add(300005);
			int num = UnityEngine.Random.Range(0, list3.Count);
			for (int i = 0; i < spinNum; i++)
			{
				list.Add(new ServerItemState
				{
					m_itemId = list3[(i + num) % list3.Count],
					m_num = 1
				});
			}
			num = UnityEngine.Random.Range(0, list4.Count);
			for (int j = 0; j < spinNum; j++)
			{
				ServerChaoData serverChaoData = new ServerChaoData();
				serverChaoData.Id = list4[(j + num) % list4.Count];
				serverChaoData.Level = 5;
				if (serverChaoData.Id >= 300000 && serverChaoData.Id < 400000)
				{
					serverChaoData.Level = 0;
					serverChaoData.Rarity = 100;
					list2.Add(serverChaoData);
				}
				else
				{
					serverChaoData.Rarity = serverChaoData.Id / 1000 % 10;
					list2.Add(serverChaoData);
					list2.Add(serverChaoData);
					list2.Add(serverChaoData);
					list2.Add(serverChaoData);
					list2.Add(serverChaoData);
					list2.Add(serverChaoData);
				}
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			serverSpinResultGeneral.AddItemState(list[k]);
		}
		for (int l = 0; l < list2.Count; l++)
		{
			serverSpinResultGeneral.AddChaoState(list2[l]);
		}
		serverSpinResultGeneral.ItemWon = itemWon;
		return serverSpinResultGeneral;
	}

	public Rect CreateGuiRectIn(Rect target, Rect rect, DebugGameObject.GUI_RECT_ANCHOR anchor = DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP)
	{
		Rect rect2 = new Rect(rect.x / target.width, rect.y / target.height, rect.width / target.width, rect.height / target.height);
		return this.CreateGuiRectInRate(target, rect2, anchor);
	}

	public Rect CreateGuiRectInRate(Rect target, Rect rect, DebugGameObject.GUI_RECT_ANCHOR anchor = DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP)
	{
		rect.x *= target.width;
		rect.y *= target.height;
		rect.width *= target.width;
		rect.height *= target.height;
		Rect result = new Rect(rect.x, rect.y, rect.width, rect.height);
		float num = 0f;
		float num2 = 0f;
		switch (anchor)
		{
		case DebugGameObject.GUI_RECT_ANCHOR.CENTER:
			num = target.width * 0.5f - rect.width * 0.5f;
			num2 = target.height * 0.5f - rect.height * 0.5f;
			break;
		case DebugGameObject.GUI_RECT_ANCHOR.CENTER_LEFT:
			num = 0f;
			num2 = target.height * 0.5f - rect.height * 0.5f;
			break;
		case DebugGameObject.GUI_RECT_ANCHOR.CENTER_RIGHT:
			num = target.width - rect.width;
			num2 = target.height * 0.5f - rect.height * 0.5f;
			break;
		case DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP:
			num = target.width * 0.5f - rect.width * 0.5f;
			num2 = 0f;
			break;
		case DebugGameObject.GUI_RECT_ANCHOR.CENTER_BOTTOM:
			num = target.width * 0.5f - rect.width * 0.5f;
			num2 = target.height - rect.height;
			break;
		case DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP:
			num = 0f;
			num2 = 0f;
			break;
		case DebugGameObject.GUI_RECT_ANCHOR.LEFT_BOTTOM:
			num = 0f;
			num2 = target.height - rect.height;
			break;
		case DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP:
			num = target.width - rect.width;
			num2 = 0f;
			break;
		case DebugGameObject.GUI_RECT_ANCHOR.RIGHT_BOTTOM:
			num = target.width - rect.width;
			num2 = target.height - rect.height;
			break;
		}
		num += target.x;
		num2 += target.y;
		result.x = num + rect.x;
		result.y = num2 + rect.y;
		result.width = rect.width;
		result.height = rect.height;
		return result;
	}

	public Rect CreateGuiRect(Rect rect, DebugGameObject.GUI_RECT_ANCHOR anchor = DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP)
	{
		Rect rect2 = new Rect(rect.x / 800f, rect.y / 450f, rect.width / 800f, rect.height / 450f);
		return this.CreateGuiRectRate(rect2, anchor);
	}

	public Rect CreateGuiRectRate(Rect rect, DebugGameObject.GUI_RECT_ANCHOR anchor = DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP)
	{
		if (this.m_camera == null)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width, rect.height);
			rect2.x = rect.x;
			rect2.y = rect.y;
			rect2.width = rect.width;
			rect2.height = rect.height;
			return rect;
		}
		rect.x *= this.m_camera.pixelRect.xMax;
		rect.y *= this.m_camera.pixelRect.yMax;
		rect.width *= this.m_camera.pixelRect.xMax;
		rect.height *= this.m_camera.pixelRect.yMax;
		Rect result = new Rect(rect.x, rect.y, rect.width, rect.height);
		if (this.m_camera != null)
		{
			float num = 0f;
			float num2 = 0f;
			switch (anchor)
			{
			case DebugGameObject.GUI_RECT_ANCHOR.CENTER:
				num = this.m_camera.pixelRect.xMax * 0.5f - rect.width * 0.5f;
				num2 = this.m_camera.pixelRect.yMax * 0.5f - rect.height * 0.5f;
				break;
			case DebugGameObject.GUI_RECT_ANCHOR.CENTER_LEFT:
				num = 0f;
				num2 = this.m_camera.pixelRect.yMax * 0.5f - rect.height * 0.5f;
				break;
			case DebugGameObject.GUI_RECT_ANCHOR.CENTER_RIGHT:
				num = this.m_camera.pixelRect.xMax - rect.width;
				num2 = this.m_camera.pixelRect.yMax * 0.5f - rect.height * 0.5f;
				break;
			case DebugGameObject.GUI_RECT_ANCHOR.CENTER_TOP:
				num = this.m_camera.pixelRect.xMax * 0.5f - rect.width * 0.5f;
				num2 = 0f;
				break;
			case DebugGameObject.GUI_RECT_ANCHOR.CENTER_BOTTOM:
				num = this.m_camera.pixelRect.xMax * 0.5f - rect.width * 0.5f;
				num2 = this.m_camera.pixelRect.yMax - rect.height;
				break;
			case DebugGameObject.GUI_RECT_ANCHOR.LEFT_TOP:
				num = 0f;
				num2 = 0f;
				break;
			case DebugGameObject.GUI_RECT_ANCHOR.LEFT_BOTTOM:
				num = 0f;
				num2 = this.m_camera.pixelRect.yMax - rect.height;
				break;
			case DebugGameObject.GUI_RECT_ANCHOR.RIGHT_TOP:
				num = this.m_camera.pixelRect.xMax - rect.width;
				num2 = 0f;
				break;
			case DebugGameObject.GUI_RECT_ANCHOR.RIGHT_BOTTOM:
				num = this.m_camera.pixelRect.xMax - rect.width;
				num2 = this.m_camera.pixelRect.yMax - rect.height;
				break;
			}
			result.x = num + rect.x;
			result.y = num2 + rect.y;
			result.width = rect.width;
			result.height = rect.height;
		}
		return result;
	}

	public bool IsDebugGift()
	{
		return this.m_debugGiftItemId == 0;
	}

	public bool DebugRequestUpPoint(int rsring, int ring = 0, int energy = 0)
	{
		NetDebugUpdPointData netDebugUpdPointData = new NetDebugUpdPointData(energy, 0, ring, 0, rsring, 0);
		netDebugUpdPointData.Request();
		return true;
	}

	public bool DebugRequestGiftItem(int itemId, int num, GameObject callbackObject, bool response = true)
	{
		this.PopLog(string.Concat(new object[]
		{
			"item:",
			itemId,
			"×",
			num,
			" response:",
			response
		}), 0f, 0f, DebugGameObject.GUI_RECT_ANCHOR.CENTER);
		if (response)
		{
			if (!this.IsDebugGift())
			{
				return false;
			}
			this.m_debugGiftItemId = itemId;
			this.m_debugGiftCallback = callbackObject;
		}
		NetDebugAddOpeMessage.OpeMsgInfo opeMsgInfo = new NetDebugAddOpeMessage.OpeMsgInfo();
		opeMsgInfo.userID = SystemSaveManager.GetGameID();
		opeMsgInfo.messageKind = 1;
		opeMsgInfo.infoId = 0;
		opeMsgInfo.itemId = itemId;
		opeMsgInfo.numItem = num;
		opeMsgInfo.additionalInfo1 = 0;
		opeMsgInfo.additionalInfo2 = 1;
		opeMsgInfo.msgTitle = "debug";
		opeMsgInfo.msgContent = "Debug Gift Item " + itemId;
		opeMsgInfo.msgImageId = string.Empty + opeMsgInfo.itemId;
		NetDebugAddOpeMessage netDebugAddOpeMessage = new NetDebugAddOpeMessage(opeMsgInfo);
		if (response)
		{
			base.StartCoroutine(this.NetworkRequest(netDebugAddOpeMessage, new DebugGameObject.NetworkRequestSuccessCallback(this.AddOpeMessageCallback), new DebugGameObject.NetworkRequestFailedCallback(this.NetworkFailedCallback)));
		}
		else
		{
			netDebugAddOpeMessage.Request();
		}
		return true;
	}

	private IEnumerator NetworkRequest(NetBase request, DebugGameObject.NetworkRequestSuccessCallback successCallback, DebugGameObject.NetworkRequestFailedCallback failedCallback)
	{
		DebugGameObject._NetworkRequest_c__IteratorCB _NetworkRequest_c__IteratorCB = new DebugGameObject._NetworkRequest_c__IteratorCB();
		_NetworkRequest_c__IteratorCB.request = request;
		_NetworkRequest_c__IteratorCB.successCallback = successCallback;
		_NetworkRequest_c__IteratorCB.failedCallback = failedCallback;
		_NetworkRequest_c__IteratorCB.___request = request;
		_NetworkRequest_c__IteratorCB.___successCallback = successCallback;
		_NetworkRequest_c__IteratorCB.___failedCallback = failedCallback;
		return _NetworkRequest_c__IteratorCB;
	}

	private void AddOpeMessageCallback()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetMessageList(base.gameObject);
		}
	}

	private void ServerGetMessageList_Succeeded(MsgGetMessageListSucceed msg)
	{
		if (this.m_debugGiftCallback == null)
		{
			this.m_debugGiftItemId = 0;
			HudMenuUtility.SendMsgUpdateSaveDataDisplay();
			return;
		}
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			List<ServerOperatorMessageEntry> operatorMessageList = ServerInterface.OperatorMessageList;
			if (operatorMessageList != null && operatorMessageList.Count > 0)
			{
				List<int> list = new List<int>();
				List<int> list2 = new List<int>();
				foreach (ServerOperatorMessageEntry current in operatorMessageList)
				{
					if (current.m_presentState != null && current.m_presentState.m_itemId == this.m_debugGiftItemId && current.m_messageId > 0)
					{
						list2.Add(current.m_messageId);
					}
				}
				if (list.Count > 0 || list2.Count > 0)
				{
					loggedInServerInterface.RequestServerUpdateMessage(list, list2, base.gameObject);
				}
				else
				{
					this.m_debugGiftCallback.SendMessage("DebugRequestGiftItem_Failed", SendMessageOptions.DontRequireReceiver);
					this.m_debugGiftCallback = null;
					this.m_debugGiftItemId = 0;
				}
			}
		}
	}

	private void ServerUpdateMessage_Succeeded(MsgUpdateMesseageSucceed msg)
	{
		if (this.m_debugGiftCallback != null)
		{
			this.m_debugGiftCallback.SendMessage("DebugRequestGiftItem_Succeeded", SendMessageOptions.DontRequireReceiver);
		}
		this.m_debugGiftCallback = null;
		this.m_debugGiftItemId = 0;
	}

	private void NetworkFailedCallback(ServerInterface.StatusCode statusCode)
	{
		if (this.m_debugGiftCallback != null)
		{
			this.m_debugGiftCallback.SendMessage("DebugRequestGiftItem_Failed", SendMessageOptions.DontRequireReceiver);
		}
		this.m_debugGiftCallback = null;
		this.m_debugGiftItemId = 0;
	}

	private void NetworkFailedMileCallback(ServerInterface.StatusCode statusCode)
	{
		if (this.m_debugGiftCallback != null)
		{
			this.m_debugGiftCallback.SendMessage("DebugUpdMileageData_Failed", SendMessageOptions.DontRequireReceiver);
		}
		this.m_debugGiftCallback = null;
		this.m_debugGiftItemId = 0;
	}

	public bool DebugStoryWindow()
	{
		int num = 5;
		GeneralWindow.CInfo.Event[] array = new GeneralWindow.CInfo.Event[num];
		int episode = 1;
		int pre_episode = 1;
		if (this.debugSceneLoader != null)
		{
			MileageMapText.Load(this.debugSceneLoader, episode, pre_episode);
		}
		for (int i = 0; i < num; i++)
		{
			string empty = string.Empty;
			MileageMapText.GetText(episode, empty);
			if (string.IsNullOrEmpty(empty))
			{
			}
			array[i] = new GeneralWindow.CInfo.Event
			{
				bgmCueName = string.Empty,
				seCueName = string.Empty
			};
		}
		GeneralWindow.Create(new GeneralWindow.CInfo
		{
			name = "DebugStoryWindow",
			buttonType = GeneralWindow.ButtonType.OkNextSkip,
			caption = "DebugStory",
			events = array,
			isNotPlaybackDefaultBgm = true,
			isSpecialEvent = true
		});
		return true;
	}
}
