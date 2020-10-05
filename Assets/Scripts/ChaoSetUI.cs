using DataTable;
using Message;
using SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class ChaoSetUI : MonoBehaviour
{
	public class SaveDataInterface
	{
		public static int MainChaoId
		{
			get
			{
				SaveDataManager instance = SaveDataManager.Instance;
				if (instance != null)
				{
					return instance.PlayerData.MainChaoID;
				}
				return -1;
			}
			set
			{
				SaveDataManager instance = SaveDataManager.Instance;
				if (instance != null)
				{
					instance.PlayerData.MainChaoID = value;
				}
			}
		}

		public static int SubChaoId
		{
			get
			{
				SaveDataManager instance = SaveDataManager.Instance;
				if (instance != null)
				{
					return instance.PlayerData.SubChaoID;
				}
				return -1;
			}
			set
			{
				SaveDataManager instance = SaveDataManager.Instance;
				if (instance != null)
				{
					instance.PlayerData.SubChaoID = value;
				}
			}
		}
	}

	[Serializable]
	private class ChaoSerializeFields
	{
		[SerializeField]
		public UISprite m_chaoSprite;

		[SerializeField]
		public UITexture m_chaoTexture;

		[SerializeField]
		public UISprite m_chaoRankSprite;

		[SerializeField]
		public UILabel m_chaoNameLabel;

		[SerializeField]
		public UILabel m_chaoLevelLabel;

		[SerializeField]
		public UISprite m_chaoTypeSprite;

		[SerializeField]
		public UISprite m_bonusTypeSprite;

		[SerializeField]
		public UILabel m_bonusLabel;
	}

	[Serializable]
	private class RouletteButtonUI
	{
		[SerializeField]
		public GameObject m_alertBadgeGameObject;

		[SerializeField]
		public GameObject m_eqqBadgeGameObject;

		[SerializeField]
		public GameObject m_spinCountBadgeGameObject;

		[SerializeField]
		public UILabel m_spinCountLabel;

		public void Setup()
		{
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				this.m_alertBadgeGameObject.SetActive(ServerInterface.CampaignState.InAnyIdSession(Constants.Campaign.emType.ChaoRouletteCost));
				int num = 0;
				if (RouletteManager.Instance != null)
				{
					num = RouletteManager.Instance.specialEgg;
				}
				this.m_eqqBadgeGameObject.SetActive(num >= 10);
				this.m_spinCountBadgeGameObject.SetActive(ServerInterface.WheelOptions.m_numRemaining > 0);
				this.m_spinCountLabel.text = ServerInterface.WheelOptions.m_numRemaining.ToString();
				this.m_spinCountBadgeGameObject.SetActive(false);
			}
		}
	}

	private sealed class _InitView_c__Iterator2D : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _maxChaoCount___0;

		internal SystemSaveManager _systemSaveManager___1;

		internal SystemData _saveData___2;

		internal int _PC;

		internal object _current;

		internal ChaoSetUI __f__this;

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
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (GameObjectUtil.FindChildGameObjects(this.__f__this.m_slot, "ui_chao_set_cell(Clone)").Count == 0)
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			this.__f__this.m_currentDeckSetStock = DeckUtil.GetDeckCurrentStockIndex();
			this.__f__this.SetupTabView();
			this.__f__this.m_uiRectItemStorage.maxColumns = 4;
			this._maxChaoCount___0 = 0;
			if (ServerInterface.PlayerState != null && ServerInterface.PlayerState.ChaoStates != null)
			{
				this._maxChaoCount___0 = ServerInterface.PlayerState.ChaoStates.Count;
			}
			if (this._maxChaoCount___0 == 0)
			{
				this._maxChaoCount___0 = ChaoTable.GetDataTable().Length;
			}
			this.__f__this.m_uiRectItemStorage.maxRows = (this._maxChaoCount___0 + this.__f__this.m_uiRectItemStorage.maxColumns - 1) / this.__f__this.m_uiRectItemStorage.maxColumns;
			this.__f__this.m_uiRectItemStorage.maxItemCount = this._maxChaoCount___0;
			this.__f__this.m_uiRectItemStorage.Restart();
			this.__f__this.m_uiRectItemStorage.Strip();
			if (this.__f__this.m_initEnd)
			{
				this.__f__this.ChaoSortUpadate(this.__f__this.m_chaoSort, this.__f__this.m_descendingOrder, DataTable.ChaoData.Rarity.NONE);
			}
			else
			{
				this.__f__this.m_mask0 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.__f__this.gameObject, "img_mask_0_bg");
				this.__f__this.m_mask1 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.__f__this.gameObject, "img_mask_1_bg");
				this.__f__this.m_sortLeveUpBC = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(this.__f__this.gameObject, "sort_1");
				this.__f__this.m_sortRareUpBC = GameObjectUtil.FindChildGameObjectComponent<BoxCollider>(this.__f__this.gameObject, "sort_0");
				this.__f__this.m_mask0.alpha = 0f;
				this.__f__this.m_mask1.alpha = 1f;
				this.__f__this.m_sortLeveUp.alpha = 0f;
				this.__f__this.m_sortRareUp.alpha = 0f;
				this.__f__this.m_chaoSort = ChaoSort.RARE;
				this.__f__this.m_descendingOrder = false;
				if (this.__f__this.m_tutorial)
				{
					this.__f__this.m_mask0.alpha = 1f;
					this.__f__this.m_mask1.alpha = 0f;
					this.__f__this.m_sortLeveUp.alpha = 1f;
					this.__f__this.m_sortRareUp.alpha = 1f;
					this.__f__this.m_chaoSort = ChaoSort.LEVEL;
					this.__f__this.m_descendingOrder = true;
					this.__f__this.ChaoSortUpadate(this.__f__this.m_chaoSort, this.__f__this.m_descendingOrder, DataTable.ChaoData.Rarity.NONE);
				}
				else
				{
					this._systemSaveManager___1 = SystemSaveManager.Instance;
					if (this._systemSaveManager___1 != null)
					{
						this._saveData___2 = this._systemSaveManager___1.GetSystemdata();
						if (this._saveData___2 != null)
						{
							this.__f__this.m_chaoSort = (ChaoSort)this._saveData___2.chaoSortType01;
							this.__f__this.m_descendingOrder = (this._saveData___2.chaoSortType02 > 0);
							if (this.__f__this.m_descendingOrder)
							{
								this.__f__this.m_sortLeveUp.alpha = 1f;
								this.__f__this.m_sortRareUp.alpha = 1f;
							}
							if (this.__f__this.m_chaoSort == ChaoSort.LEVEL)
							{
								this.__f__this.m_mask0.alpha = 1f;
								this.__f__this.m_mask1.alpha = 0f;
							}
						}
					}
					this.__f__this.ChaoSortUpadate(this.__f__this.m_chaoSort, this.__f__this.m_descendingOrder, DataTable.ChaoData.Rarity.NONE);
				}
				this.__f__this.m_initEnd = true;
			}
			if (this.__f__this.m_sortLeveUpBC != null)
			{
				this.__f__this.m_sortLeveUpBC.enabled = true;
			}
			if (this.__f__this.m_sortRareUpBC != null)
			{
				this.__f__this.m_sortRareUpBC.enabled = true;
			}
			this.__f__this.m_sortDelay = 0f;
			this.__f__this.m_initDelay = 0.2f;
			this.__f__this.UpdateView();
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

	private sealed class _UpdateViewCoroutine_c__Iterator2E : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _chaoCountIndex___0;

		internal int _PC;

		internal object _current;

		internal ChaoSetUI __f__this;

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
				this.__f__this.m_cells = GameObjectUtil.FindChildGameObjects(this.__f__this.m_slot, "ui_chao_set_cell(Clone)");
				if (this.__f__this.m_cells.Count != this.__f__this.m_uiRectItemStorage.maxItemCount)
				{
					this._current = null;
					this._PC = 1;
					return true;
				}
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this.__f__this.m_stageAbilityManager != null)
			{
				this.__f__this.m_getChaoCountLabel.text = this.__f__this.m_stageAbilityManager.GetChaoCount().ToString();
				this.__f__this.m_getChaoCountShadowLabel.text = this.__f__this.m_stageAbilityManager.GetChaoCount().ToString();
				this._chaoCountIndex___0 = 0;
				while (this._chaoCountIndex___0 < this.__f__this.m_chaoCountNumber.Length)
				{
					if (this.__f__this.m_stageAbilityManager.GetChaoCount() < this.__f__this.m_chaoCountNumber[this._chaoCountIndex___0])
					{
						break;
					}
					this._chaoCountIndex___0++;
				}
				this.__f__this.m_getChaoSprite.spriteName = "ui_chao_set_dec_" + this._chaoCountIndex___0;
				this.__f__this.m_getChaoBonusLabel.text = HudUtility.GetChaoCountBonusText(this.__f__this.m_stageAbilityManager.GetChaoCountBonusValue());
			}
			this.__f__this.RegistChao(0, ChaoSetUI.SaveDataInterface.MainChaoId);
			this.__f__this.RegistChao(1, ChaoSetUI.SaveDataInterface.SubChaoId);
			this.__f__this.UpdateRegistedChaoView();
			this.__f__this.UpdateGotChaoView();
			if (this.__f__this.m_tutorial)
			{
				TutorialCursor.StartTutorialCursor(TutorialCursor.Type.CHAOSELECT_CHAO);
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

	private const int MAX_COLUMNS = 4;

	private const float SORT_DELAY = 0.4f;

	private const float INIT_DELAY = 0.2f;

	[SerializeField]
	private bool isDebugRondomSetChao;

	[SerializeField]
	private int[] m_chaoCountNumber = new int[3];

	[SerializeField]
	private UILabel m_getChaoCountLabel;

	[SerializeField]
	private UILabel m_getChaoCountShadowLabel;

	[SerializeField]
	private UISprite m_getChaoSprite;

	[SerializeField]
	private UILabel m_getChaoBonusLabel;

	[SerializeField]
	private ChaoSetUI.ChaoSerializeFields[] m_chaosSerializeFields = new ChaoSetUI.ChaoSerializeFields[2];

	[SerializeField]
	private ChaoSetUI.RouletteButtonUI m_rouletteButtonUI;

	[SerializeField]
	private UISprite m_sortLeveUp;

	[SerializeField]
	private UISprite m_sortRareUp;

	[SerializeField]
	private GameObject m_specialEggIconObj;

	[SerializeField]
	private GameObject m_freeSpinIconObj;

	[SerializeField]
	private UILabel m_freeSpinCountLabel;

	[SerializeField]
	private GameObject m_saleIconObj;

	[SerializeField]
	private GameObject m_eventIconObj;

	public static readonly string[] s_chaoBonusTypeSpriteNameSuffixs = new string[]
	{
		"score",
		"ring",
		"rsr",
		"animal",
		"range"
	};

	private GameObject m_slot;

	private UIRectItemStorage m_uiRectItemStorage;

	private UIDraggablePanel m_uiDraggablePanel;

	private List<GameObject> m_cells;

	private StageAbilityManager m_stageAbilityManager;

	private ChaoSort m_lastSort;

	private int m_currentDeckSetStock;

	private int m_sortCount;

	private List<GameObject> m_deckObjects;

	private List<DataTable.ChaoData> m_chaoDatas;

	private float m_sortDelay;

	private BoxCollider m_sortLeveUpBC;

	private BoxCollider m_sortRareUpBC;

	private UISprite m_mask0;

	private UISprite m_mask1;

	private ChaoSort m_chaoSort;

	private bool m_tutorial;

	private bool m_descendingOrder;

	private bool m_initEnd;

	private bool m_endSetUp;

	private float m_initDelay;

	public bool IsEndSetup
	{
		get
		{
			return this.m_endSetUp;
		}
	}

	private void Update()
	{
		if (this.m_tutorial && GeneralWindow.IsCreated("ChaoTutorial") && GeneralWindow.IsButtonPressed)
		{
			GeneralWindow.Close();
			TutorialCursor.StartTutorialCursor(TutorialCursor.Type.BACK);
		}
		if (GeneralWindow.IsCreated("ChaoCantSet") && GeneralWindow.IsButtonPressed)
		{
			GeneralWindow.Close();
		}
		if (this.m_sortDelay > 0f)
		{
			this.m_sortDelay -= Time.deltaTime;
			if (this.m_sortDelay <= 0f)
			{
				this.m_sortDelay = 0f;
				if (this.m_sortLeveUpBC != null)
				{
					this.m_sortLeveUpBC.enabled = true;
				}
				if (this.m_sortRareUpBC != null)
				{
					this.m_sortRareUpBC.enabled = true;
				}
			}
		}
		if (this.m_initDelay > 0f)
		{
			this.m_initDelay -= Time.deltaTime;
			if (this.m_initDelay < 0.5f)
			{
				this.m_endSetUp = true;
			}
			if (this.m_initDelay <= 0f)
			{
				this.m_endSetUp = true;
				this.m_initDelay = 0f;
				HudMenuUtility.SetConnectAlertSimpleUI(false);
			}
		}
	}

	private void OnStartChaoSet()
	{
		HudMenuUtility.SetConnectAlertSimpleUI(true);
		this.m_initDelay = 0f;
		this.m_currentDeckSetStock = DeckUtil.GetDeckCurrentStockIndex();
		this.m_endSetUp = false;
		this.m_tutorial = ChaoSetUI.IsChaoTutorial();
		if (this.m_tutorial)
		{
			ChaoSetUI.SaveDataInterface.MainChaoId = -1;
			ChaoSetUI.SaveDataInterface.SubChaoId = -1;
		}
		if (this.isDebugRondomSetChao)
		{
			SaveDataManager instance = SaveDataManager.Instance;
			if (instance != null)
			{
				DataTable.ChaoData[] dataTable = ChaoTable.GetDataTable();
				if (dataTable != null)
				{
					instance.ChaoData.Info = new SaveData.ChaoData.ChaoDataInfo[dataTable.Length];
					instance.PlayerData.MainChaoID = -1;
					instance.PlayerData.SubChaoID = -1;
					for (int i = 0; i < dataTable.Length; i++)
					{
						int id = dataTable[i].id;
						int num = UnityEngine.Random.Range(-1, 11);
						instance.ChaoData.Info[i].chao_id = id;
						instance.ChaoData.Info[i].level = num;
						if (instance.PlayerData.MainChaoID == -1 && num != -1)
						{
							instance.PlayerData.MainChaoID = id;
						}
					}
				}
			}
		}
		this.m_rouletteButtonUI.Setup();
		this.m_stageAbilityManager = GameObjectUtil.FindGameObjectComponent<StageAbilityManager>("StageAbilityManager");
		if (this.m_stageAbilityManager != null)
		{
			this.m_stageAbilityManager.RecalcAbilityVaue();
		}
		this.m_slot = GameObjectUtil.FindChildGameObject(base.gameObject, "slot");
		this.m_uiRectItemStorage = this.m_slot.GetComponent<UIRectItemStorage>();
		this.m_uiDraggablePanel = GameObjectUtil.FindChildGameObjectComponent<UIDraggablePanel>(base.gameObject, "chao_set_contents");
		this.SetRouletteButton();
		GeneralUtil.SetCharasetBtnIcon(base.gameObject, "Btn_charaset");
		GeneralUtil.SetButtonFunc(base.gameObject, "Btn_charaset", base.gameObject, "OnClickDeck");
		UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "Btn_player");
		if (uIButtonMessage != null)
		{
			uIButtonMessage.target = base.gameObject;
			uIButtonMessage.functionName = "GoToCharacterButtonClicked";
		}
		base.StartCoroutine(this.InitView());
	}

	private IEnumerator InitView()
	{
		ChaoSetUI._InitView_c__Iterator2D _InitView_c__Iterator2D = new ChaoSetUI._InitView_c__Iterator2D();
		_InitView_c__Iterator2D.__f__this = this;
		return _InitView_c__Iterator2D;
	}

	private void SetRouletteButton()
	{
		HudRouletteButtonUtil.SetSpecialEggIcon(this.m_specialEggIconObj);
		HudRouletteButtonUtil.SetFreeSpin(this.m_freeSpinIconObj, this.m_freeSpinCountLabel, false);
		HudRouletteButtonUtil.SetSaleIcon(this.m_saleIconObj);
		HudRouletteButtonUtil.SetEventIcon(this.m_eventIconObj);
	}

	private void SetupTabView()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "window_chao_tab");
		if (gameObject != null)
		{
			if (this.m_deckObjects != null)
			{
				this.m_deckObjects.Clear();
			}
			this.m_deckObjects = new List<GameObject>();
			for (int i = 0; i < 10; i++)
			{
				GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject, "tab_" + (i + 1));
				if (!(gameObject2 != null))
				{
					break;
				}
				this.m_deckObjects.Add(gameObject2);
			}
			if (this.m_deckObjects.Count > 0 && this.m_deckObjects.Count > this.m_currentDeckSetStock)
			{
				for (int j = 0; j < this.m_deckObjects.Count; j++)
				{
					this.m_deckObjects[j].SetActive(this.m_currentDeckSetStock == j);
				}
			}
		}
	}

	private void UpdateView()
	{
		base.StartCoroutine(this.UpdateViewCoroutine());
	}

	private IEnumerator UpdateViewCoroutine()
	{
		ChaoSetUI._UpdateViewCoroutine_c__Iterator2E _UpdateViewCoroutine_c__Iterator2E = new ChaoSetUI._UpdateViewCoroutine_c__Iterator2E();
		_UpdateViewCoroutine_c__Iterator2E.__f__this = this;
		return _UpdateViewCoroutine_c__Iterator2E;
	}

	private void SetCellChao(int cellIndex, DataTable.ChaoData chaoData, int mainChaoId, int subChaoId)
	{
		if (this.m_chaoDatas != null)
		{
			ui_chao_set_cell[] componentsInChildren = base.gameObject.GetComponentsInChildren<ui_chao_set_cell>(true);
			if (cellIndex >= 0 && cellIndex < componentsInChildren.Length)
			{
				if (this.m_uiDraggablePanel == null)
				{
					this.m_uiDraggablePanel = GameObjectUtil.FindChildGameObjectComponent<UIDraggablePanel>(base.gameObject, "chao_set_contents");
				}
				if (this.m_uiDraggablePanel != null)
				{
					componentsInChildren[cellIndex].UpdateView(chaoData.id, mainChaoId, subChaoId, this.m_uiDraggablePanel.panel);
				}
			}
		}
	}

	public void RegistChao(int chaoMainSubIndex, int chaoId)
	{
		base.enabled = true;
		int num = -1;
		int num2 = -1;
		bool flag = false;
		bool flag2 = false;
		if (chaoMainSubIndex == 0)
		{
			if (ChaoSetUI.SaveDataInterface.SubChaoId == chaoId)
			{
				num2 = ChaoSetUI.SaveDataInterface.MainChaoId;
				flag2 = true;
			}
			if (ChaoSetUI.SaveDataInterface.MainChaoId != chaoId)
			{
				num = chaoId;
				flag = true;
			}
		}
		else
		{
			if (ChaoSetUI.SaveDataInterface.MainChaoId == chaoId)
			{
				num = ChaoSetUI.SaveDataInterface.SubChaoId;
				flag = true;
			}
			if (ChaoSetUI.SaveDataInterface.SubChaoId != chaoId)
			{
				num2 = chaoId;
				flag2 = true;
			}
		}
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null && this.m_initDelay <= 0f)
		{
			if (flag || flag2)
			{
				loggedInServerInterface.RequestServerEquipChao((int)ServerItem.CreateFromChaoId((!flag) ? ChaoSetUI.SaveDataInterface.MainChaoId : num).id, (int)ServerItem.CreateFromChaoId((!flag2) ? ChaoSetUI.SaveDataInterface.SubChaoId : num2).id, base.gameObject);
			}
		}
		else
		{
			if (flag)
			{
				ChaoSetUI.SaveDataInterface.MainChaoId = num;
			}
			if (flag2)
			{
				ChaoSetUI.SaveDataInterface.SubChaoId = num2;
			}
			if (flag || flag2)
			{
				this.UpdateRegistedChaoView();
				this.UpdateGotChaoView();
			}
		}
	}

	private void ServerEquipChao_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		global::Debug.Log(string.Concat(new object[]
		{
			"ServerEquipChao_Succeeded  mainCharaId:",
			msg.m_playerState.m_mainCharaId,
			"  subCharaId:",
			msg.m_playerState.m_subCharaId
		}));
		NetUtil.SyncSaveDataAndDataBase(msg.m_playerState);
		this.UpdateRegistedChaoView();
		this.UpdateGotChaoView();
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	private void ServerEquipChao_Failed(MsgServerConnctFailed msg)
	{
	}

	private void ServerEquipChao_Dummy()
	{
		this.UpdateRegistedChaoView();
		this.UpdateGotChaoView();
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	private void UpdateRegistedChaoView()
	{
		GeneralUtil.SetCharasetBtnIcon(base.gameObject, "Btn_charaset");
		this.UpdateRegistedChaoView(0, ChaoTable.GetChaoData(ChaoSetUI.SaveDataInterface.MainChaoId));
		this.UpdateRegistedChaoView(1, ChaoTable.GetChaoData(ChaoSetUI.SaveDataInterface.SubChaoId));
	}

	public void UpdateRegistedChaoView(int chaoMainSubIndex, DataTable.ChaoData chaoData)
	{
		ChaoSetUI.ChaoSerializeFields chaoSerializeFields = this.m_chaosSerializeFields[chaoMainSubIndex];
		if (chaoData != null && chaoData.IsValidate)
		{
			ChaoTextureManager.CallbackInfo info = new ChaoTextureManager.CallbackInfo(chaoSerializeFields.m_chaoTexture, null, true);
			ChaoTextureManager.Instance.GetTexture(chaoData.id, info);
			chaoSerializeFields.m_chaoTexture.enabled = true;
			chaoSerializeFields.m_chaoSprite.enabled = false;
			chaoSerializeFields.m_chaoRankSprite.spriteName = "ui_chao_set_bg_l_" + (int)chaoData.rarity;
			chaoSerializeFields.m_chaoNameLabel.text = chaoData.nameTwolines;
			chaoSerializeFields.m_chaoLevelLabel.text = TextUtility.GetTextLevel(chaoData.level.ToString());
			string str = chaoData.charaAtribute.ToString().ToLower();
			chaoSerializeFields.m_chaoTypeSprite.spriteName = "ui_chao_set_type_icon_" + str;
			chaoSerializeFields.m_bonusLabel.gameObject.SetActive(false);
			chaoSerializeFields.m_bonusTypeSprite.gameObject.SetActive(false);
		}
		else
		{
			chaoSerializeFields.m_chaoTexture.enabled = false;
			chaoSerializeFields.m_chaoSprite.enabled = true;
			chaoSerializeFields.m_chaoRankSprite.spriteName = "ui_chao_set_bg_l_3";
			chaoSerializeFields.m_chaoNameLabel.text = string.Empty;
			chaoSerializeFields.m_chaoLevelLabel.text = string.Empty;
			chaoSerializeFields.m_chaoTypeSprite.spriteName = null;
			chaoSerializeFields.m_bonusTypeSprite.spriteName = null;
			chaoSerializeFields.m_bonusLabel.text = string.Empty;
		}
	}

	private void UpdateGotChaoView()
	{
		int num = 0;
		if (this.m_chaoDatas != null && this.m_chaoDatas.Count > 0 && ServerInterface.PlayerState != null && ServerInterface.PlayerState.ChaoStates != null && ServerInterface.PlayerState.ChaoStates.Count > 0)
		{
			foreach (DataTable.ChaoData current in this.m_chaoDatas)
			{
				int num2 = current.id + 400000;
				foreach (ServerChaoState current2 in ServerInterface.PlayerState.ChaoStates)
				{
					if (num2 == current2.Id)
					{
						this.SetCellChao(num, current, ChaoSetUI.SaveDataInterface.MainChaoId, ChaoSetUI.SaveDataInterface.SubChaoId);
						num++;
						break;
					}
				}
			}
		}
		else
		{
			this.ChaoSortUpadate(ChaoSort.RARE, false, DataTable.ChaoData.Rarity.NONE);
			if (this.m_chaoDatas != null)
			{
				foreach (DataTable.ChaoData current3 in this.m_chaoDatas)
				{
					this.SetCellChao(num, current3, ChaoSetUI.SaveDataInterface.MainChaoId, ChaoSetUI.SaveDataInterface.SubChaoId);
					num++;
				}
			}
		}
	}

	private void ChaoSortUpadate(ChaoSort sort, bool down = false, DataTable.ChaoData.Rarity exclusion = DataTable.ChaoData.Rarity.NONE)
	{
		DataTable.ChaoData[] dataTable = ChaoTable.GetDataTable();
		ChaoDataSorting chaoDataSorting = new ChaoDataSorting(sort);
		if (chaoDataSorting != null)
		{
			ChaoDataVisitorBase visitor = chaoDataSorting.visitor;
			if (visitor != null)
			{
				if (this.m_lastSort != sort)
				{
					this.m_lastSort = sort;
					this.m_sortCount = 0;
				}
				else
				{
					this.m_sortCount++;
				}
				DataTable.ChaoData[] array = dataTable;
				for (int i = 0; i < array.Length; i++)
				{
					DataTable.ChaoData chaoData = array[i];
					chaoData.accept(ref visitor);
				}
				switch (sort)
				{
				case ChaoSort.RARE:
				case ChaoSort.LEVEL:
					this.m_chaoDatas = chaoDataSorting.GetChaoListAll(down, exclusion);
					break;
				case ChaoSort.ATTRIBUTE:
				case ChaoSort.ABILITY:
				case ChaoSort.EVENT:
					this.m_chaoDatas = chaoDataSorting.GetChaoListAllOffset(this.m_sortCount, false, DataTable.ChaoData.Rarity.NONE);
					break;
				}
			}
		}
	}

	private void OnClickDeck()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		DeckViewWindow.Create(base.gameObject);
	}

	private void OnMsgDeckViewWindowChange()
	{
		this.OnStartChaoSet();
		global::Debug.Log("ChaoSetUI OnMsgDeckViewWindowChange!");
	}

	private void OnMsgDeckViewWindowNotChange()
	{
		this.OnStartChaoSet();
		global::Debug.Log("ChaoSetUI OnMsgDeckViewWindowNotChange!");
	}

	private void OnMsgDeckViewWindowNetworkError()
	{
		this.OnStartChaoSet();
		global::Debug.Log("ChaoSetUI OnMsgDeckViewWindowNetworkError!");
	}

	public void OnPressSortLevel()
	{
		if (this.m_sortLeveUpBC != null)
		{
			this.m_sortLeveUpBC.enabled = false;
		}
		if (this.m_sortRareUpBC != null)
		{
			this.m_sortRareUpBC.enabled = false;
		}
		this.m_sortDelay = 0.4f;
		if (this.m_chaoSort != ChaoSort.LEVEL)
		{
			this.m_descendingOrder = (this.m_sortLeveUp.alpha >= 0.9f);
			this.m_chaoSort = ChaoSort.LEVEL;
		}
		else
		{
			if (this.m_sortLeveUp.alpha >= 0.9f)
			{
				this.m_sortLeveUp.alpha = 0f;
			}
			else
			{
				this.m_sortLeveUp.alpha = 1f;
			}
			this.m_descendingOrder = !this.m_descendingOrder;
		}
		this.m_mask0.alpha = 1f;
		this.m_mask1.alpha = 0f;
		this.ChaoSortUpadate(this.m_chaoSort, this.m_descendingOrder, DataTable.ChaoData.Rarity.NONE);
		this.UpdateGotChaoView();
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	public void OnPressSortRare()
	{
		if (this.m_sortLeveUpBC != null)
		{
			this.m_sortLeveUpBC.enabled = false;
		}
		if (this.m_sortRareUpBC != null)
		{
			this.m_sortRareUpBC.enabled = false;
		}
		this.m_sortDelay = 0.4f;
		if (this.m_chaoSort != ChaoSort.RARE)
		{
			this.m_descendingOrder = (this.m_sortRareUp.alpha >= 0.9f);
			this.m_chaoSort = ChaoSort.RARE;
		}
		else
		{
			if (this.m_sortRareUp.alpha >= 0.9f)
			{
				this.m_sortRareUp.alpha = 0f;
			}
			else
			{
				this.m_sortRareUp.alpha = 1f;
			}
			this.m_descendingOrder = !this.m_descendingOrder;
		}
		this.m_mask0.alpha = 0f;
		this.m_mask1.alpha = 1f;
		this.ChaoSortUpadate(this.m_chaoSort, this.m_descendingOrder, DataTable.ChaoData.Rarity.NONE);
		this.UpdateGotChaoView();
		SoundManager.SePlay("sys_menu_decide", "SE");
	}

	private void OnSetChaoTexture(ChaoTextureManager.TextureData data)
	{
		if (ChaoSetUI.SaveDataInterface.MainChaoId == data.chao_id)
		{
			this.m_chaosSerializeFields[0].m_chaoTexture.enabled = true;
			this.m_chaosSerializeFields[0].m_chaoTexture.mainTexture = data.tex;
		}
		else if (ChaoSetUI.SaveDataInterface.SubChaoId == data.chao_id)
		{
			this.m_chaosSerializeFields[1].m_chaoTexture.enabled = true;
			this.m_chaosSerializeFields[1].m_chaoTexture.mainTexture = data.tex;
		}
	}

	private void OnClickChaoMain()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		ChaoSetWindowUI window = ChaoSetWindowUI.GetWindow();
		if (window != null)
		{
			DataTable.ChaoData chaoData = ChaoTable.GetChaoData(ChaoSetUI.SaveDataInterface.MainChaoId);
			if (chaoData != null)
			{
				ChaoSetWindowUI.ChaoInfo chaoInfo = new ChaoSetWindowUI.ChaoInfo(chaoData);
				chaoInfo.level = chaoData.level;
				chaoInfo.detail = chaoData.GetDetailLevelPlusSP(chaoInfo.level);
				window.OpenWindow(chaoInfo, ChaoSetWindowUI.WindowType.WINDOW_ONLY);
			}
		}
	}

	private void OnClickChaoSub()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		ChaoSetWindowUI window = ChaoSetWindowUI.GetWindow();
		if (window != null)
		{
			DataTable.ChaoData chaoData = ChaoTable.GetChaoData(ChaoSetUI.SaveDataInterface.SubChaoId);
			if (chaoData != null)
			{
				ChaoSetWindowUI.ChaoInfo chaoInfo = new ChaoSetWindowUI.ChaoInfo(chaoData);
				chaoInfo.level = chaoData.level;
				chaoInfo.detail = chaoData.GetDetailLevelPlusSP(chaoInfo.level);
				window.OpenWindow(chaoInfo, ChaoSetWindowUI.WindowType.WINDOW_ONLY);
			}
		}
	}

	private void GoToCharacterButtonClicked()
	{
		SoundManager.SePlay("sys_menu_decide", "SE");
		HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.CHARA_MAIN, true);
	}

	private void OnMsgMenuBack()
	{
		ui_chao_set_cell.ResetLastLoadTime();
		if (this.m_tutorial)
		{
			TutorialCursor.EndTutorialCursor(TutorialCursor.Type.BACK);
			HudMenuUtility.SendMsgUpdateSaveDataDisplay();
			this.m_tutorial = false;
		}
		ChaoTextureManager.Instance.RemoveChaoTexture();
		SystemSaveManager instance = SystemSaveManager.Instance;
		if (instance != null)
		{
			SystemData systemdata = instance.GetSystemdata();
			if (systemdata != null)
			{
				int num = (!this.m_descendingOrder) ? 0 : 1;
				if (this.m_chaoSort != (ChaoSort)systemdata.chaoSortType01 || num != systemdata.chaoSortType02)
				{
					systemdata.chaoSortType01 = (int)this.m_chaoSort;
					systemdata.chaoSortType02 = num;
					instance.SaveSystemData();
				}
			}
		}
		this.m_endSetUp = false;
	}

	public void ChaoSetLoad(int stock)
	{
	}

	public static bool IsChaoTutorial()
	{
		return false;
	}
}
