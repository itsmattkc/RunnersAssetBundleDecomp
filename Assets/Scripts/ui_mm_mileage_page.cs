using AnimationOrTween;
using DataTable;
using SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class ui_mm_mileage_page : MonoBehaviour
{
	private enum WayPointEventType
	{
		NONE,
		SIMPLE,
		GORGEOUS,
		LAST
	}

	private abstract class BaseEvent
	{
		private GameObject _gameObject_k__BackingField;

		private ui_mm_mileage_page _mileage_page_k__BackingField;

		private bool _isEnd_k__BackingField;

		protected GameObject gameObject
		{
			get;
			set;
		}

		protected ui_mm_mileage_page mileage_page
		{
			get;
			set;
		}

		public bool isEnd
		{
			get;
			set;
		}

		public BaseEvent(GameObject gameObject)
		{
			this.gameObject = gameObject;
			this.mileage_page = gameObject.GetComponent<ui_mm_mileage_page>();
		}

		public virtual void Start()
		{
		}

		public abstract bool Update();

		public virtual void SkipMileageProcess()
		{
		}

		protected bool IsAskSnsFeed()
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					return systemdata.IsFacebookWindow();
				}
			}
			return true;
		}

		protected void SetDisableAskSnsFeed()
		{
			SystemSaveManager instance = SystemSaveManager.Instance;
			if (instance != null)
			{
				SystemData systemdata = instance.GetSystemdata();
				if (systemdata != null)
				{
					systemdata.SetFacebookWindow(false);
					instance.SaveSystemData();
				}
			}
		}
	}

	private class WaitEvent : ui_mm_mileage_page.BaseEvent
	{
		private float _waitTime_k__BackingField;

		private float _time_k__BackingField;

		private float waitTime
		{
			get;
			set;
		}

		private float time
		{
			get;
			set;
		}

		public WaitEvent(GameObject gameObject, float waitTime) : base(gameObject)
		{
			this.waitTime = waitTime;
		}

		public void Start()
		{
			this.time = 0f;
		}

		public bool Update()
		{
			float time = this.time;
			this.time += Time.deltaTime;
			if (time < this.waitTime && this.time >= this.waitTime)
			{
				base.isEnd = true;
			}
			return base.isEnd;
		}
	}

	private class GeneralEvent : ui_mm_mileage_page.BaseEvent
	{
		private GeneralWindow.ButtonType _buttonType_k__BackingField;

		private string _title_k__BackingField;

		private string _message_k__BackingField;

		public GeneralWindow.ButtonType buttonType
		{
			get;
			private set;
		}

		public string title
		{
			get;
			private set;
		}

		public string message
		{
			get;
			private set;
		}

		public GeneralEvent(GameObject gameObject, GeneralWindow.ButtonType buttonType, string title, string message) : base(gameObject)
		{
			this.buttonType = buttonType;
			this.title = title;
			this.message = message;
		}

		public void Start()
		{
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				buttonType = this.buttonType,
				caption = this.title,
				message = this.message
			});
		}

		public bool Update()
		{
			if (GeneralWindow.IsButtonPressed)
			{
				GeneralWindow.Close();
				base.isEnd = true;
			}
			return base.isEnd;
		}
	}

	private class SimpleEvent : ui_mm_mileage_page.BaseEvent
	{
		private enum WindowType
		{
			CHARA_GET,
			CHARA_LEVEL_UP,
			CHAO_GET,
			CHAO_GET_SRARE,
			CHAO_LEVEL_UP,
			ITEM,
			UNKNOWN
		}

		private enum StateMode
		{
			SET_WINDOW_TYPE,
			REQUEST_LOAD,
			WAIT_LOAD,
			START_WIDOW,
			WAIT_END_WIDOW,
			END
		}

		private ServerItem m_serverItem;

		private ChaoGetWindow m_charaGetWindow;

		private ChaoGetWindow m_chaoGetWindow;

		private ChaoMergeWindow m_chaoMergeWindow;

		private PlayerMergeWindow m_playerMergeWindow;

		private ButtonEventResourceLoader m_buttonEventResourceLoader;

		private ui_mm_mileage_page.SimpleEvent.WindowType m_windowType = ui_mm_mileage_page.SimpleEvent.WindowType.UNKNOWN;

		private ui_mm_mileage_page.SimpleEvent.StateMode m_stateMode;

		private RewardType _rewardType_k__BackingField;

		private int _serverItemId_k__BackingField;

		private int _count_k__BackingField;

		private string _title_k__BackingField;

		private bool _disableSe_k__BackingField;

		private RewardType rewardType
		{
			get;
			set;
		}

		private int serverItemId
		{
			get;
			set;
		}

		private int count
		{
			get;
			set;
		}

		private string title
		{
			get;
			set;
		}

		private bool disableSe
		{
			get;
			set;
		}

		public SimpleEvent(GameObject gameObject, int serverItemId, int count, string title, bool disableSe = false) : base(gameObject)
		{
			this.serverItemId = serverItemId;
			ServerItem serverItem = new ServerItem((ServerItem.Id)serverItemId);
			this.rewardType = serverItem.rewardType;
			this.count = count;
			this.title = title;
			this.disableSe = disableSe;
			this.m_serverItem = new ServerItem(this.rewardType);
		}

		public void Start()
		{
			if (this.rewardType != RewardType.NONE && this.count > 0)
			{
				MileageMapUtility.AddReward(this.rewardType, this.count);
			}
		}

		public void ResourceLoadEndCallback()
		{
			if (FontManager.Instance != null)
			{
				FontManager.Instance.ReplaceFont();
			}
			if (AtlasManager.Instance != null)
			{
				AtlasManager.Instance.ReplaceAtlasForMenu(true);
			}
			this.m_stateMode = ui_mm_mileage_page.SimpleEvent.StateMode.START_WIDOW;
		}

		public bool Update()
		{
			switch (this.m_stateMode)
			{
			case ui_mm_mileage_page.SimpleEvent.StateMode.SET_WINDOW_TYPE:
				if (this.rewardType != RewardType.NONE && this.count > 0)
				{
					this.SetWindowType();
					this.m_stateMode = ui_mm_mileage_page.SimpleEvent.StateMode.REQUEST_LOAD;
				}
				else
				{
					this.m_stateMode = ui_mm_mileage_page.SimpleEvent.StateMode.END;
				}
				break;
			case ui_mm_mileage_page.SimpleEvent.StateMode.REQUEST_LOAD:
				this.RequestLoadWindow();
				if (this.m_windowType == ui_mm_mileage_page.SimpleEvent.WindowType.UNKNOWN)
				{
					this.m_stateMode = ui_mm_mileage_page.SimpleEvent.StateMode.END;
				}
				else if (this.m_windowType == ui_mm_mileage_page.SimpleEvent.WindowType.ITEM)
				{
					this.m_stateMode = ui_mm_mileage_page.SimpleEvent.StateMode.START_WIDOW;
				}
				else
				{
					this.m_stateMode = ui_mm_mileage_page.SimpleEvent.StateMode.WAIT_LOAD;
				}
				break;
			case ui_mm_mileage_page.SimpleEvent.StateMode.WAIT_LOAD:
				if (this.m_buttonEventResourceLoader != null && this.m_buttonEventResourceLoader.IsLoaded)
				{
					this.m_stateMode = ui_mm_mileage_page.SimpleEvent.StateMode.START_WIDOW;
				}
				break;
			case ui_mm_mileage_page.SimpleEvent.StateMode.START_WIDOW:
				this.StartWindow();
				this.m_stateMode = ui_mm_mileage_page.SimpleEvent.StateMode.WAIT_END_WIDOW;
				break;
			case ui_mm_mileage_page.SimpleEvent.StateMode.WAIT_END_WIDOW:
				if (this.UpdateWindow())
				{
					this.m_stateMode = ui_mm_mileage_page.SimpleEvent.StateMode.END;
				}
				break;
			case ui_mm_mileage_page.SimpleEvent.StateMode.END:
				if (this.m_buttonEventResourceLoader != null)
				{
					UnityEngine.Object.Destroy(this.m_buttonEventResourceLoader);
					this.m_buttonEventResourceLoader = null;
				}
				base.isEnd = true;
				break;
			}
			return base.isEnd;
		}

		public void SetWindowType()
		{
			if (this.m_serverItem.idType == ServerItem.IdType.CHARA)
			{
				if (this.m_serverItem.charaType != CharaType.UNKNOWN)
				{
					ServerPlayerState playerState = ServerInterface.PlayerState;
					if (playerState != null)
					{
						ServerCharacterState serverCharacterState = playerState.CharacterState(this.m_serverItem.charaType);
						if (serverCharacterState != null && serverCharacterState.star > 0)
						{
							this.m_windowType = ui_mm_mileage_page.SimpleEvent.WindowType.CHARA_LEVEL_UP;
						}
						else
						{
							this.m_windowType = ui_mm_mileage_page.SimpleEvent.WindowType.CHARA_GET;
						}
					}
				}
			}
			else if (this.m_serverItem.idType == ServerItem.IdType.CHAO)
			{
				DataTable.ChaoData chaoData = ChaoTable.GetChaoData(this.m_serverItem.chaoId);
				if (chaoData != null)
				{
					if (chaoData.level > 0)
					{
						this.m_windowType = ui_mm_mileage_page.SimpleEvent.WindowType.CHAO_LEVEL_UP;
					}
					else if (chaoData.rarity == DataTable.ChaoData.Rarity.NORMAL || chaoData.rarity == DataTable.ChaoData.Rarity.RARE)
					{
						this.m_windowType = ui_mm_mileage_page.SimpleEvent.WindowType.CHAO_GET;
					}
					else
					{
						this.m_windowType = ui_mm_mileage_page.SimpleEvent.WindowType.CHAO_GET_SRARE;
					}
				}
			}
			else
			{
				this.m_windowType = ui_mm_mileage_page.SimpleEvent.WindowType.ITEM;
			}
		}

		public void RequestLoadWindow()
		{
			string text = string.Empty;
			switch (this.m_windowType)
			{
			case ui_mm_mileage_page.SimpleEvent.WindowType.CHARA_GET:
			case ui_mm_mileage_page.SimpleEvent.WindowType.CHARA_LEVEL_UP:
			case ui_mm_mileage_page.SimpleEvent.WindowType.CHAO_GET:
			case ui_mm_mileage_page.SimpleEvent.WindowType.CHAO_GET_SRARE:
			case ui_mm_mileage_page.SimpleEvent.WindowType.CHAO_LEVEL_UP:
				text = "ChaoWindows";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.m_buttonEventResourceLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
				this.m_buttonEventResourceLoader.LoadResourceIfNotLoadedAsync(text, new ButtonEventResourceLoader.CallbackIfNotLoaded(this.ResourceLoadEndCallback));
			}
		}

		public void StartWindow()
		{
			if (this.rewardType != RewardType.NONE && this.count > 0)
			{
				GameObject parent = GameObject.Find("UI Root (2D)");
				switch (this.m_windowType)
				{
				case ui_mm_mileage_page.SimpleEvent.WindowType.CHARA_GET:
					this.m_charaGetWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoGetWindow>(parent, "ro_PlayerGetWindowUI");
					if (this.m_charaGetWindow != null)
					{
						PlayerGetPartsOverlap playerGetPartsOverlap = this.m_charaGetWindow.gameObject.GetComponent<PlayerGetPartsOverlap>();
						if (playerGetPartsOverlap == null)
						{
							playerGetPartsOverlap = this.m_charaGetWindow.gameObject.AddComponent<PlayerGetPartsOverlap>();
						}
						playerGetPartsOverlap.Init((int)this.m_serverItem.id, 100, 0, null, PlayerGetPartsOverlap.IntroType.NO_EGG);
						ChaoGetPartsBase chaoGetParts = playerGetPartsOverlap;
						bool isTutorial = false;
						this.m_charaGetWindow.PlayStart(chaoGetParts, isTutorial, true, RouletteUtility.AchievementType.NONE);
					}
					break;
				case ui_mm_mileage_page.SimpleEvent.WindowType.CHARA_LEVEL_UP:
					this.m_playerMergeWindow = GameObjectUtil.FindChildGameObjectComponent<PlayerMergeWindow>(parent, "player_merge_Window");
					if (this.m_playerMergeWindow != null)
					{
						this.m_playerMergeWindow.PlayStart((int)this.m_serverItem.id, RouletteUtility.AchievementType.PlayerGet);
					}
					break;
				case ui_mm_mileage_page.SimpleEvent.WindowType.CHAO_GET:
				{
					DataTable.ChaoData chaoData = ChaoTable.GetChaoData(this.m_serverItem.chaoId);
					this.m_chaoGetWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoGetWindow>(parent, "chao_get_Window");
					ChaoGetPartsRare component = this.m_chaoGetWindow.gameObject.GetComponent<ChaoGetPartsRare>();
					ChaoGetPartsNormal chaoGetPartsNormal = this.m_chaoGetWindow.gameObject.GetComponent<ChaoGetPartsNormal>();
					if (component != null)
					{
						UnityEngine.Object.Destroy(component);
					}
					if (chaoGetPartsNormal == null)
					{
						chaoGetPartsNormal = this.m_chaoGetWindow.gameObject.AddComponent<ChaoGetPartsNormal>();
					}
					chaoGetPartsNormal.Init((int)this.m_serverItem.id, (int)chaoData.rarity);
					this.m_chaoGetWindow.PlayStart(chaoGetPartsNormal, false, true, RouletteUtility.AchievementType.NONE);
					break;
				}
				case ui_mm_mileage_page.SimpleEvent.WindowType.CHAO_GET_SRARE:
				{
					DataTable.ChaoData chaoData2 = ChaoTable.GetChaoData(this.m_serverItem.chaoId);
					this.m_chaoGetWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoGetWindow>(parent, "chao_rare_get_Window");
					ChaoGetPartsNormal component2 = this.m_chaoGetWindow.gameObject.GetComponent<ChaoGetPartsNormal>();
					ChaoGetPartsRare chaoGetPartsRare = this.m_chaoGetWindow.gameObject.GetComponent<ChaoGetPartsRare>();
					if (component2 != null)
					{
						UnityEngine.Object.Destroy(component2);
					}
					if (chaoGetPartsRare == null)
					{
						chaoGetPartsRare = this.m_chaoGetWindow.gameObject.AddComponent<ChaoGetPartsRare>();
					}
					chaoGetPartsRare.Init((int)this.m_serverItem.id, (int)chaoData2.rarity);
					this.m_chaoGetWindow.PlayStart(chaoGetPartsRare, false, true, RouletteUtility.AchievementType.NONE);
					break;
				}
				case ui_mm_mileage_page.SimpleEvent.WindowType.CHAO_LEVEL_UP:
				{
					DataTable.ChaoData chaoData3 = ChaoTable.GetChaoData(this.m_serverItem.chaoId);
					if (this.m_chaoMergeWindow == null)
					{
						this.m_chaoMergeWindow = GameObjectUtil.FindChildGameObjectComponent<ChaoMergeWindow>(parent, "chao_merge_Window");
					}
					this.m_chaoMergeWindow.PlayStart((int)this.m_serverItem.id, chaoData3.level, (int)chaoData3.rarity, RouletteUtility.AchievementType.NONE);
					break;
				}
				case ui_mm_mileage_page.SimpleEvent.WindowType.ITEM:
				{
					ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
					if (itemGetWindow != null)
					{
						string text = MileageMapUtility.GetText("gw_item_text", new Dictionary<string, string>
						{
							{
								"{COUNT}",
								HudUtility.GetFormatNumString<int>(this.count)
							}
						});
						itemGetWindow.Create(new ItemGetWindow.CInfo
						{
							caption = this.title,
							serverItemId = this.serverItemId,
							imageCount = text
						});
					}
					break;
				}
				}
				if (!this.disableSe)
				{
					SoundManager.SePlay("sys_roulette_itemget", "SE");
				}
			}
		}

		public bool UpdateWindow()
		{
			bool result = false;
			switch (this.m_windowType)
			{
			case ui_mm_mileage_page.SimpleEvent.WindowType.CHARA_GET:
				if (this.m_charaGetWindow != null && this.m_charaGetWindow.IsPlayEnd)
				{
					result = true;
					this.m_charaGetWindow = null;
				}
				break;
			case ui_mm_mileage_page.SimpleEvent.WindowType.CHARA_LEVEL_UP:
				if (this.m_playerMergeWindow != null && this.m_playerMergeWindow.IsPlayEnd)
				{
					result = true;
					this.m_playerMergeWindow = null;
				}
				break;
			case ui_mm_mileage_page.SimpleEvent.WindowType.CHAO_GET:
				if (this.m_chaoGetWindow != null && this.m_chaoGetWindow.IsPlayEnd)
				{
					result = true;
					this.m_chaoGetWindow = null;
				}
				break;
			case ui_mm_mileage_page.SimpleEvent.WindowType.CHAO_GET_SRARE:
				if (this.m_chaoGetWindow != null && this.m_chaoGetWindow.IsPlayEnd)
				{
					result = true;
					this.m_chaoGetWindow = null;
				}
				break;
			case ui_mm_mileage_page.SimpleEvent.WindowType.CHAO_LEVEL_UP:
				if (this.m_chaoMergeWindow != null && this.m_chaoMergeWindow.IsPlayEnd)
				{
					result = true;
					this.m_chaoMergeWindow = null;
				}
				break;
			case ui_mm_mileage_page.SimpleEvent.WindowType.ITEM:
			{
				ItemGetWindow itemGetWindow = ItemGetWindowUtil.GetItemGetWindow();
				if (itemGetWindow != null && itemGetWindow.IsEnd)
				{
					itemGetWindow.Reset();
					result = true;
				}
				break;
			}
			default:
				result = true;
				break;
			}
			return result;
		}
	}

	private class GorgeousEvent : ui_mm_mileage_page.BaseEvent
	{
		private bool m_notAllSkip;

		private int _windowId_k__BackingField;

		private bool _isNotPlaybackDefaultBgm_k__BackingField;

		public int windowId
		{
			get;
			private set;
		}

		private bool isNotPlaybackDefaultBgm
		{
			get;
			set;
		}

		public GorgeousEvent(GameObject gameObject, int windowId, bool isNotPlaybackDefaultBgm = false) : base(gameObject)
		{
			this.windowId = windowId;
			this.isNotPlaybackDefaultBgm = isNotPlaybackDefaultBgm;
			this.m_notAllSkip = false;
		}

		public GorgeousEvent(GameObject gameObject, int windowId, bool isNotPlaybackDefaultBgm, bool notAllSkip) : base(gameObject)
		{
			this.windowId = windowId;
			this.isNotPlaybackDefaultBgm = isNotPlaybackDefaultBgm;
			this.m_notAllSkip = notAllSkip;
		}

		public void Start()
		{
			MileageMapData mileageMapData = MileageMapDataManager.Instance.GetMileageMapData();
			if (mileageMapData == null)
			{
				return;
			}
			if (this.windowId >= mileageMapData.window_data.Length)
			{
				return;
			}
			WindowEventData windowEventData = mileageMapData.window_data[this.windowId];
			GeneralWindow.CInfo.Event[] array = new GeneralWindow.CInfo.Event[windowEventData.body.Length];
			for (int i = 0; i < windowEventData.body.Length; i++)
			{
				WindowBodyData windowBodyData = windowEventData.body[i];
				GeneralWindow.CInfo.Event.FaceWindow[] array2 = new GeneralWindow.CInfo.Event.FaceWindow[windowBodyData.product.Length];
				for (int j = 0; j < windowBodyData.product.Length; j++)
				{
					WindowProductData windowProductData = windowBodyData.product[j];
					array2[j] = new GeneralWindow.CInfo.Event.FaceWindow
					{
						texture = MileageMapUtility.GetFaceTexture(windowProductData.face_id),
						name = ((windowProductData.name_cell_id == null) ? null : MileageMapText.GetName(windowProductData.name_cell_id)),
						effectType = windowProductData.effect,
						animType = windowProductData.anim,
						reverseType = windowProductData.reverse,
						showingType = windowProductData.showing
					};
				}
				array[i] = new GeneralWindow.CInfo.Event
				{
					faceWindows = array2,
					arrowType = windowBodyData.arrow,
					bgmCueName = windowBodyData.bgm,
					seCueName = windowBodyData.se,
					message = MileageMapText.GetText(mileageMapData.scenario.episode, windowBodyData.text_cell_id)
				};
			}
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				buttonType = GeneralWindow.ButtonType.OkNextSkipAllSkip,
				caption = MileageMapText.GetText(mileageMapData.scenario.episode, windowEventData.title_cell_id),
				events = array,
				isNotPlaybackDefaultBgm = this.isNotPlaybackDefaultBgm
			});
		}

		public bool Update()
		{
			if (GeneralWindow.IsButtonPressed)
			{
				if (GeneralWindow.IsAllSkipButtonPressed && !this.m_notAllSkip)
				{
					base.mileage_page.OnClickAllSkipBtn();
				}
				GeneralWindow.Close();
				base.isEnd = true;
			}
			return base.isEnd;
		}
	}

	private class BalloonEffectEvent : ui_mm_mileage_page.BaseEvent
	{
		private int _waypointIndex_k__BackingField;

		private float _time_k__BackingField;

		private int waypointIndex
		{
			get;
			set;
		}

		private float time
		{
			get;
			set;
		}

		public BalloonEffectEvent(GameObject gameObject, int waypointIndex) : base(gameObject)
		{
			this.waypointIndex = waypointIndex;
		}

		public void Start()
		{
			this.SetEffectActive(false);
			this.SetEffectActive(true);
		}

		public bool Update()
		{
			float time = this.time;
			this.time += Time.deltaTime;
			if ((time < 0.5f && this.time >= 0.5f) || this.waypointIndex < 1)
			{
				this.SetEffectActive(false);
				base.isEnd = true;
			}
			return base.isEnd;
		}

		private void SetEffectActive(bool isActive)
		{
			if (this.waypointIndex >= 1)
			{
				GameObject effectGameObject = base.mileage_page.m_balloonsObjects[this.waypointIndex - 1].m_effectGameObject;
				if (effectGameObject != null)
				{
					effectGameObject.SetActive(isActive);
				}
			}
		}
	}

	private class BalloonEvent : ui_mm_mileage_page.BaseEvent
	{
		private int _eventIndex_k__BackingField;

		private int _newFaceId_k__BackingField;

		private int _oldFaceId_k__BackingField;

		private float _time_k__BackingField;

		private int eventIndex
		{
			get;
			set;
		}

		private int newFaceId
		{
			get;
			set;
		}

		private int oldFaceId
		{
			get;
			set;
		}

		private float time
		{
			get;
			set;
		}

		public BalloonEvent(GameObject gameObject, int eventIndex, int newFaceId, int oldFaceId) : base(gameObject)
		{
			this.eventIndex = eventIndex;
			this.newFaceId = newFaceId;
			this.oldFaceId = oldFaceId;
		}

		public void Start()
		{
			this.time = 0f;
		}

		public bool Update()
		{
			float time = this.time;
			this.time += Time.deltaTime;
			if (time < 0.01f && this.time >= 0.01f)
			{
				this.SkipMileageProcess();
			}
			if (this.time >= 1f)
			{
				base.isEnd = true;
			}
			return base.isEnd;
		}

		public void SkipMileageProcess()
		{
			if (base.mileage_page != null)
			{
				base.mileage_page.SetBalloonFaceTexture(this.eventIndex, this.newFaceId);
			}
		}
	}

	private class BgmEvent : ui_mm_mileage_page.BaseEvent
	{
		private string m_cueName;

		private float m_waitTime;

		private bool m_playBgmFlag;

		public BgmEvent(GameObject gameObject, string cueName) : base(gameObject)
		{
			this.m_cueName = cueName;
		}

		public void Start()
		{
			if (string.IsNullOrEmpty(this.m_cueName))
			{
				SoundManager.BgmFadeOut(0.5f);
				base.isEnd = true;
			}
			else
			{
				SoundManager.BgmFadeOut(0.5f);
				this.m_playBgmFlag = true;
			}
		}

		public bool Update()
		{
			if (this.m_playBgmFlag)
			{
				this.m_waitTime += Time.deltaTime;
				if (this.m_waitTime > 0.5f)
				{
					SoundManager.BgmStop();
					SoundManager.BgmPlay(this.m_cueName, "BGM", false);
					base.isEnd = true;
				}
			}
			return base.isEnd;
		}
	}

	private class MapEvent : ui_mm_mileage_page.BaseEvent
	{
		private int _episode_k__BackingField;

		private int _chapter_k__BackingField;

		public int episode
		{
			get;
			private set;
		}

		public int chapter
		{
			get;
			private set;
		}

		public bool isNext
		{
			get
			{
				return this.episode == -1 || this.chapter == -1;
			}
		}

		public MapEvent(GameObject gameObject, int episode, int chapter) : base(gameObject)
		{
			this.episode = episode;
			this.chapter = chapter;
		}

		public MapEvent(GameObject gameObject) : base(gameObject)
		{
			this.episode = -1;
			this.chapter = -1;
		}

		public void Start()
		{
			this.SkipMileageProcess();
			base.isEnd = true;
		}

		public bool Update()
		{
			return base.isEnd;
		}

		public void SkipMileageProcess()
		{
			SoundManager.BgmChange("bgm_sys_menu", "BGM");
			MileageMapDataManager.Instance.SetCurrentData(base.mileage_page.m_mapInfo.m_resultData.m_newMapState.m_episode, base.mileage_page.m_mapInfo.m_resultData.m_newMapState.m_chapter);
			base.mileage_page.m_mapInfo.m_resultData.m_oldMapState.m_episode = base.mileage_page.m_mapInfo.m_resultData.m_newMapState.m_episode;
			base.mileage_page.m_mapInfo.m_resultData.m_oldMapState.m_chapter = base.mileage_page.m_mapInfo.m_resultData.m_newMapState.m_chapter;
			ResultData resultData = base.mileage_page.m_mapInfo.m_resultData;
			base.mileage_page.m_mapInfo = new ui_mm_mileage_page.MapInfo();
			base.mileage_page.m_mapInfo.m_resultData = resultData;
			base.mileage_page.m_mapInfo.isNextMileage = true;
			base.mileage_page.m_mapInfo.ResetMileageIncentive();
			base.mileage_page.SetBG();
			base.mileage_page.SetAll();
			SaveDataManager.Instance.PlayerData.RankOffset = 0;
			HudMenuUtility.SendMsgUpdateSaveDataDisplay();
		}
	}

	private class HighscoreEvent : ui_mm_mileage_page.BaseEvent
	{
		private enum Phase
		{
			Init,
			InitAskWindow,
			UpdateAskWindow,
			InitSnsFeed,
			UpdateSnsFeed,
			Term
		}

		private ui_mm_mileage_page.HighscoreEvent.Phase m_phase;

		private EasySnsFeed m_easySnsFeed;

		private long _highscore_k__BackingField;

		public long highscore
		{
			get;
			private set;
		}

		public HighscoreEvent(GameObject gameObject, long highscore) : base(gameObject)
		{
			this.highscore = highscore;
		}

		private void NextPhase()
		{
			this.m_phase++;
		}

		public bool Update()
		{
			switch (this.m_phase)
			{
			case ui_mm_mileage_page.HighscoreEvent.Phase.Init:
				if (base.IsAskSnsFeed())
				{
					this.NextPhase();
				}
				else
				{
					base.isEnd = true;
				}
				break;
			case ui_mm_mileage_page.HighscoreEvent.Phase.InitAskWindow:
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					buttonType = GeneralWindow.ButtonType.TweetCancel,
					caption = MileageMapUtility.GetText("gw_highscore_caption", null),
					message = MileageMapUtility.GetText("gw_highscore_text", null)
				});
				this.NextPhase();
				break;
			case ui_mm_mileage_page.HighscoreEvent.Phase.UpdateAskWindow:
				if (GeneralWindow.IsButtonPressed)
				{
					if (GeneralWindow.IsYesButtonPressed)
					{
						this.m_phase = ui_mm_mileage_page.HighscoreEvent.Phase.InitSnsFeed;
					}
					else
					{
						this.m_phase = ui_mm_mileage_page.HighscoreEvent.Phase.Term;
						base.SetDisableAskSnsFeed();
					}
					GeneralWindow.Close();
				}
				break;
			case ui_mm_mileage_page.HighscoreEvent.Phase.InitSnsFeed:
				this.m_easySnsFeed = new EasySnsFeed(base.gameObject, "Camera/menu_Anim/ui_mm_mileage2_page/Anchor_5_MC", MileageMapUtility.GetText("feed_highscore_caption", null), MileageMapUtility.GetText("feed_highscore_text", new Dictionary<string, string>
				{
					{
						"{HIGHSCORE}",
						this.highscore.ToString()
					}
				}), null);
				this.NextPhase();
				break;
			case ui_mm_mileage_page.HighscoreEvent.Phase.UpdateSnsFeed:
			{
				EasySnsFeed.Result result = this.m_easySnsFeed.Update();
				if (result == EasySnsFeed.Result.COMPLETED || result == EasySnsFeed.Result.FAILED)
				{
					this.NextPhase();
				}
				break;
			}
			case ui_mm_mileage_page.HighscoreEvent.Phase.Term:
				base.isEnd = true;
				break;
			}
			return base.isEnd;
		}
	}

	private class RankingUPEvent : ui_mm_mileage_page.BaseEvent
	{
		private enum StateMode
		{
			WAIT_LOAD,
			START_WIDOW,
			WAIT_END_WIDOW,
			END,
			UNKNOWN
		}

		private ui_mm_mileage_page.RankingUPEvent.StateMode m_stateMode = ui_mm_mileage_page.RankingUPEvent.StateMode.UNKNOWN;

		private ButtonEventResourceLoader m_buttonEventResourceLoader;

		public RankingUPEvent(GameObject gameObject) : base(gameObject)
		{
		}

		public void Start()
		{
			this.m_buttonEventResourceLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
			this.m_buttonEventResourceLoader.LoadResourceIfNotLoadedAsync("RankingResultBitWindow", new ButtonEventResourceLoader.CallbackIfNotLoaded(this.ResourceLoadEndCallback));
			this.m_stateMode = ui_mm_mileage_page.RankingUPEvent.StateMode.WAIT_LOAD;
		}

		public void ResourceLoadEndCallback()
		{
			if (FontManager.Instance != null)
			{
				FontManager.Instance.ReplaceFont();
			}
			if (AtlasManager.Instance != null)
			{
				AtlasManager.Instance.ReplaceAtlasForMenu(true);
			}
			this.m_stateMode = ui_mm_mileage_page.RankingUPEvent.StateMode.START_WIDOW;
		}

		public bool Update()
		{
			switch (this.m_stateMode)
			{
			case ui_mm_mileage_page.RankingUPEvent.StateMode.WAIT_LOAD:
				if (this.m_buttonEventResourceLoader != null && this.m_buttonEventResourceLoader.IsLoaded)
				{
					this.m_stateMode = ui_mm_mileage_page.RankingUPEvent.StateMode.START_WIDOW;
				}
				break;
			case ui_mm_mileage_page.RankingUPEvent.StateMode.START_WIDOW:
				if (RankingUtil.ShowRankingChangeWindow(RankingUtil.RankingMode.ENDLESS))
				{
					this.m_stateMode = ui_mm_mileage_page.RankingUPEvent.StateMode.WAIT_END_WIDOW;
				}
				else
				{
					this.m_stateMode = ui_mm_mileage_page.RankingUPEvent.StateMode.END;
				}
				break;
			case ui_mm_mileage_page.RankingUPEvent.StateMode.WAIT_END_WIDOW:
				if (RankingUtil.IsEndRankingChangeWindow())
				{
					this.m_stateMode = ui_mm_mileage_page.RankingUPEvent.StateMode.END;
				}
				break;
			case ui_mm_mileage_page.RankingUPEvent.StateMode.END:
				if (this.m_buttonEventResourceLoader != null)
				{
					UnityEngine.Object.Destroy(this.m_buttonEventResourceLoader);
					this.m_buttonEventResourceLoader = null;
				}
				base.isEnd = true;
				break;
			}
			return base.isEnd;
		}
	}

	private class RankUpEvent : ui_mm_mileage_page.BaseEvent
	{
		private enum Phase
		{
			Init,
			WaitProduction,
			InitAskWindow,
			UpdateAskWindow,
			InitSnsFeed,
			UpdateSnsFeed,
			Term
		}

		private ui_mm_mileage_page.RankUpEvent.Phase m_phase;

		private EasySnsFeed m_easySnsFeed;

		private GameObject m_rankUpObj;

		private float m_waitTimer;

		private bool m_askSns;

		private int _rank_k__BackingField;

		public int rank
		{
			get;
			private set;
		}

		public RankUpEvent(GameObject gameObject) : base(gameObject)
		{
			this.rank = (int)SaveDataManager.Instance.PlayerData.Rank;
			FoxManager.SendLtvPointMap(this.rank);
		}

		private void NextPhase()
		{
			this.m_phase++;
		}

		public bool Update()
		{
			switch (this.m_phase)
			{
			case ui_mm_mileage_page.RankUpEvent.Phase.Init:
				this.m_askSns = base.IsAskSnsFeed();
				this.m_rankUpObj = GameObjectUtil.FindChildGameObject(base.gameObject.transform.root.gameObject, "Mileage_rankup");
				if (this.m_rankUpObj != null)
				{
					this.m_rankUpObj.SetActive(true);
					GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_rankUpObj, "eff_set");
					if (gameObject != null)
					{
						gameObject.SetActive(true);
					}
					Animation component = this.m_rankUpObj.GetComponent<Animation>();
					if (component != null)
					{
						ActiveAnimation.Play(component, "ui_mileage_rankup_Anim", Direction.Forward);
					}
					SoundManager.SePlay("sys_rank_up", "SE");
				}
				this.m_waitTimer = 2.3f;
				this.NextPhase();
				break;
			case ui_mm_mileage_page.RankUpEvent.Phase.WaitProduction:
				this.m_waitTimer -= Time.deltaTime;
				if (this.m_waitTimer < 0f)
				{
					this.m_phase = ui_mm_mileage_page.RankUpEvent.Phase.InitAskWindow;
				}
				break;
			case ui_mm_mileage_page.RankUpEvent.Phase.InitAskWindow:
			{
				string cellName = (!this.m_askSns) ? "gw_rankup_text_without_post" : "gw_rankup_text";
				GeneralWindow.ButtonType buttonType = (!this.m_askSns) ? GeneralWindow.ButtonType.Ok : GeneralWindow.ButtonType.TweetCancel;
				GeneralWindow.Create(new GeneralWindow.CInfo
				{
					buttonType = buttonType,
					caption = MileageMapUtility.GetText("gw_rankup_caption", null),
					message = MileageMapUtility.GetText(cellName, new Dictionary<string, string>
					{
						{
							"{RANK}",
							(this.rank + 1).ToString()
						}
					})
				});
				this.NextPhase();
				break;
			}
			case ui_mm_mileage_page.RankUpEvent.Phase.UpdateAskWindow:
				if (GeneralWindow.IsButtonPressed)
				{
					if (GeneralWindow.IsYesButtonPressed)
					{
						this.m_phase = ui_mm_mileage_page.RankUpEvent.Phase.InitSnsFeed;
					}
					else
					{
						this.m_phase = ui_mm_mileage_page.RankUpEvent.Phase.Term;
						if (this.m_askSns)
						{
							base.SetDisableAskSnsFeed();
						}
					}
					GeneralWindow.Close();
				}
				break;
			case ui_mm_mileage_page.RankUpEvent.Phase.InitSnsFeed:
				this.m_easySnsFeed = new EasySnsFeed(base.gameObject, "Camera/menu_Anim/ui_mm_mileage2_page/Anchor_5_MC", MileageMapUtility.GetText("feed_rankup_caption", null), MileageMapUtility.GetText("feed_rankup_text", new Dictionary<string, string>
				{
					{
						"{RANK}",
						(this.rank + 1).ToString()
					}
				}), null);
				this.NextPhase();
				break;
			case ui_mm_mileage_page.RankUpEvent.Phase.UpdateSnsFeed:
			{
				EasySnsFeed.Result result = this.m_easySnsFeed.Update();
				if (result == EasySnsFeed.Result.COMPLETED || result == EasySnsFeed.Result.FAILED)
				{
					this.NextPhase();
				}
				break;
			}
			case ui_mm_mileage_page.RankUpEvent.Phase.Term:
				if (this.m_rankUpObj != null)
				{
					UnityEngine.Object.Destroy(this.m_rankUpObj);
				}
				base.isEnd = true;
				break;
			}
			return base.isEnd;
		}
	}

	private class DailyMissionEvent : ui_mm_mileage_page.BaseEvent
	{
		private enum StateMode
		{
			WAIT_LOAD,
			START_WIDOW,
			WAIT_END_WIDOW,
			END,
			UNKNOWN
		}

		private ui_mm_mileage_page.DailyMissionEvent.StateMode m_stateMode = ui_mm_mileage_page.DailyMissionEvent.StateMode.UNKNOWN;

		private ButtonEventResourceLoader m_buttonEventResourceLoader;

		private DailyWindowUI m_dailyWindowUI;

		public DailyMissionEvent(GameObject gameObject) : base(gameObject)
		{
		}

		public void Start()
		{
			this.m_buttonEventResourceLoader = base.gameObject.AddComponent<ButtonEventResourceLoader>();
			this.m_buttonEventResourceLoader.LoadResourceIfNotLoadedAsync("DailyWindowUI", new ButtonEventResourceLoader.CallbackIfNotLoaded(this.ResourceLoadEndCallback));
			this.m_stateMode = ui_mm_mileage_page.DailyMissionEvent.StateMode.WAIT_LOAD;
		}

		public void ResourceLoadEndCallback()
		{
			if (FontManager.Instance != null)
			{
				FontManager.Instance.ReplaceFont();
			}
			if (AtlasManager.Instance != null)
			{
				AtlasManager.Instance.ReplaceAtlasForMenu(true);
			}
			this.m_stateMode = ui_mm_mileage_page.DailyMissionEvent.StateMode.START_WIDOW;
		}

		public bool Update()
		{
			switch (this.m_stateMode)
			{
			case ui_mm_mileage_page.DailyMissionEvent.StateMode.WAIT_LOAD:
				if (this.m_buttonEventResourceLoader != null && this.m_buttonEventResourceLoader.IsLoaded)
				{
					this.m_stateMode = ui_mm_mileage_page.DailyMissionEvent.StateMode.START_WIDOW;
				}
				break;
			case ui_mm_mileage_page.DailyMissionEvent.StateMode.START_WIDOW:
			{
				GameObject menuAnimUIObject = HudMenuUtility.GetMenuAnimUIObject();
				if (menuAnimUIObject != null)
				{
					this.m_dailyWindowUI = GameObjectUtil.FindChildGameObjectComponent<DailyWindowUI>(menuAnimUIObject, "DailyWindowUI");
					if (this.m_dailyWindowUI != null)
					{
						this.m_dailyWindowUI.gameObject.SetActive(true);
						this.m_dailyWindowUI.PlayStart();
					}
				}
				this.m_stateMode = ui_mm_mileage_page.DailyMissionEvent.StateMode.WAIT_END_WIDOW;
				break;
			}
			case ui_mm_mileage_page.DailyMissionEvent.StateMode.WAIT_END_WIDOW:
				if (this.m_dailyWindowUI != null && this.m_dailyWindowUI.IsEnd)
				{
					this.m_dailyWindowUI.gameObject.SetActive(false);
					this.m_stateMode = ui_mm_mileage_page.DailyMissionEvent.StateMode.END;
				}
				break;
			case ui_mm_mileage_page.DailyMissionEvent.StateMode.END:
				if (this.m_buttonEventResourceLoader != null)
				{
					UnityEngine.Object.Destroy(this.m_buttonEventResourceLoader);
					this.m_buttonEventResourceLoader = null;
				}
				base.isEnd = true;
				break;
			}
			return base.isEnd;
		}
	}

	private class MapInfo
	{
		public class Route
		{
			private int _routeIndex_k__BackingField;

			public int routeIndex
			{
				get;
				private set;
			}

			public Route(int routeIndex)
			{
				this.routeIndex = routeIndex;
			}
		}

		public enum TutorialPhase
		{
			NONE,
			NAME_ENTRY,
			AGE_VERIFICATION,
			BEFORE_GAME,
			FIRST_EPISODE,
			FIRST_BOSS,
			FIRST_LOSE_BOSS
		}

		public ResultData m_resultData;

		private int m_waypointIndex;

		private double m_scoreDistanceRaw;

		private double m_scoreDistance;

		private double m_targetScoreDistance;

		private bool m_isBossDestroyed;

		private ui_mm_mileage_page.MapInfo.Route[] m_routes;

		private bool _isNextMileage_k__BackingField;

		private bool _isBossStage_k__BackingField;

		private long _highscore_k__BackingField;

		public static int routeScoreDistance
		{
			get
			{
				return MileageMapDataManager.Instance.GetMileageMapData().map_data.event_interval;
			}
		}

		public static int stageScoreDistance
		{
			get
			{
				return ui_mm_mileage_page.MapInfo.routeScoreDistance * 5;
			}
		}

		public int waypointIndex
		{
			get
			{
				return this.m_waypointIndex;
			}
			set
			{
				this.m_waypointIndex = value;
			}
		}

		public double scoreDistanceRaw
		{
			get
			{
				return this.m_scoreDistanceRaw;
			}
			set
			{
				this.m_scoreDistanceRaw = value;
			}
		}

		public double scoreDistance
		{
			get
			{
				return this.m_scoreDistance;
			}
			set
			{
				this.m_scoreDistance = value;
			}
		}

		public double targetScoreDistance
		{
			get
			{
				return this.m_targetScoreDistance;
			}
			set
			{
				this.m_targetScoreDistance = value;
			}
		}

		public int nextWaypoint
		{
			get
			{
				return Mathf.Min(this.waypointIndex + 1, 5);
			}
		}

		public double waypointDistance
		{
			get
			{
				return (double)(this.waypointIndex * ui_mm_mileage_page.MapInfo.routeScoreDistance);
			}
		}

		public double nextWaypointDistance
		{
			get
			{
				return (double)(this.nextWaypoint * ui_mm_mileage_page.MapInfo.routeScoreDistance);
			}
		}

		public bool isNextMileage
		{
			get;
			set;
		}

		public bool isBossStage
		{
			get;
			set;
		}

		public bool isBossDestroyed
		{
			get
			{
				return this.m_isBossDestroyed;
			}
			set
			{
				this.m_isBossDestroyed = value;
			}
		}

		public ui_mm_mileage_page.MapInfo.Route[] routes
		{
			get
			{
				return this.m_routes;
			}
		}

		public long highscore
		{
			get;
			set;
		}

		public ui_mm_mileage_page.MapInfo.TutorialPhase tutorialPhase
		{
			get
			{
				return ui_mm_mileage_page.MapInfo.TutorialPhase.NONE;
			}
		}

		public MapInfo()
		{
			this.SetRoutesInfo();
			this.highscore = -1L;
		}

		public double GetRunDistance()
		{
			double result = 0.0;
			if (this.m_resultData != null && this.m_resultData.m_oldMapState != null)
			{
				result = this.scoreDistance - (double)((float)this.m_resultData.m_oldMapState.m_score);
			}
			return result;
		}

		public bool IsClearMileage()
		{
			return this.m_resultData != null && this.m_resultData.m_oldMapState != null && this.m_resultData.m_newMapState != null && (this.m_resultData.m_oldMapState.m_episode != this.m_resultData.m_newMapState.m_episode || this.m_resultData.m_oldMapState.m_chapter != this.m_resultData.m_newMapState.m_chapter);
		}

		public void SetRoutesInfo()
		{
			this.m_routes = new ui_mm_mileage_page.MapInfo.Route[5];
			for (int i = 0; i < 5; i++)
			{
				this.m_routes[i] = new ui_mm_mileage_page.MapInfo.Route(i);
			}
		}

		public void ResetMileageIncentive()
		{
			if (this.m_resultData != null && this.m_resultData.m_mileageIncentiveList != null)
			{
				this.m_resultData.m_mileageIncentiveList.Clear();
			}
		}

		public bool CheckMileageIncentive(int point)
		{
			if (this.m_resultData != null && this.m_resultData.m_mileageIncentiveList != null)
			{
				for (int i = 0; i < this.m_resultData.m_mileageIncentiveList.Count; i++)
				{
					ServerMileageIncentive serverMileageIncentive = this.m_resultData.m_mileageIncentiveList[i];
					if (serverMileageIncentive.m_type == ServerMileageIncentive.Type.POINT && serverMileageIncentive.m_pointId == point && serverMileageIncentive.m_itemId != 0 && serverMileageIncentive.m_num > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool UpdateFrom(ResultData resultData)
		{
			if (!resultData.m_validResult)
			{
				return false;
			}
			this.isBossStage = resultData.m_bossStage;
			this.isBossDestroyed = resultData.m_bossDestroy;
			if (resultData.m_rivalHighScore)
			{
				this.highscore = resultData.m_highScore;
			}
			this.waypointIndex = resultData.m_oldMapState.m_point;
			this.scoreDistanceRaw = (double)resultData.m_totalScore;
			this.scoreDistance = (double)Mathf.Min((float)resultData.m_oldMapState.m_score, (float)(ui_mm_mileage_page.MapInfo.routeScoreDistance * 5));
			this.targetScoreDistance = (double)Mathf.Min((float)resultData.m_newMapState.m_score, (float)(ui_mm_mileage_page.MapInfo.routeScoreDistance * 5));
			if (resultData.m_newMapState.m_episode > resultData.m_oldMapState.m_episode || (resultData.m_newMapState.m_episode == resultData.m_oldMapState.m_episode && resultData.m_newMapState.m_chapter > resultData.m_oldMapState.m_chapter))
			{
				this.targetScoreDistance = (double)(ui_mm_mileage_page.MapInfo.routeScoreDistance * 5);
			}
			this.m_resultData = resultData;
			return true;
		}

		public bool UpdateFrom(MileageMapState mileageMapState)
		{
			this.waypointIndex = mileageMapState.m_point;
			this.scoreDistance = (double)Mathf.Min((float)mileageMapState.m_score, (float)(ui_mm_mileage_page.MapInfo.routeScoreDistance * 5));
			this.targetScoreDistance = (double)Mathf.Min((float)mileageMapState.m_score, (float)(ui_mm_mileage_page.MapInfo.routeScoreDistance * 5));
			return true;
		}
	}

	private class PointTimeLimit
	{
		private ui_mm_mileage_page.BalloonObjects m_balloonObjs;

		private DateTime m_limitTime;

		private bool m_limitFlag;

		private bool m_failedFlag;

		private bool m_incentiveFlag;

		public bool LimitFlag
		{
			get
			{
				return this.m_limitFlag;
			}
		}

		public bool FailedFlag
		{
			get
			{
				return this.m_failedFlag;
			}
		}

		public void Reset()
		{
			this.m_limitFlag = false;
			this.m_incentiveFlag = false;
			this.m_failedFlag = false;
			this.m_balloonObjs = null;
		}

		public void SetupLimit(ServerMileageReward reward, ui_mm_mileage_page.BalloonObjects objs, bool incentiveFlag)
		{
			this.m_limitFlag = false;
			this.m_failedFlag = false;
			this.m_balloonObjs = objs;
			this.m_incentiveFlag = incentiveFlag;
			if (reward != null && reward.m_limitTime > 0)
			{
				TimeSpan value = new TimeSpan(0, 0, 0, reward.m_limitTime, 0);
				this.m_limitTime = reward.m_startTime;
				this.m_limitTime = this.m_limitTime.Add(value);
				this.m_limitFlag = true;
				if (this.m_balloonObjs != null)
				{
					if (this.m_balloonObjs.m_timerFrameObject != null)
					{
						UILabel component = this.m_balloonObjs.m_timerLimitObject.GetComponent<UILabel>();
						if (component != null)
						{
							component.enabled = !this.m_incentiveFlag;
						}
					}
					if (this.m_balloonObjs.m_timerWordObject != null)
					{
						UILabel component2 = this.m_balloonObjs.m_timerWordObject.GetComponent<UILabel>();
						if (component2 != null)
						{
							component2.enabled = !this.m_incentiveFlag;
						}
					}
				}
				this.Update();
			}
		}

		public void Update()
		{
			if (this.m_limitFlag && !this.m_incentiveFlag && !this.m_failedFlag && this.m_balloonObjs != null)
			{
				TimeSpan restTime = this.GetRestTime(this.m_limitTime);
				if (restTime.Seconds < 0)
				{
					if (this.m_balloonObjs.m_gameObject != null)
					{
						this.m_balloonObjs.m_gameObject.SetActive(false);
					}
					this.m_failedFlag = true;
				}
				else if (this.m_balloonObjs.m_timerFrameObject != null)
				{
					UILabel component = this.m_balloonObjs.m_timerLimitObject.GetComponent<UILabel>();
					if (component != null)
					{
						component.text = this.GetRestTimeText(restTime);
					}
				}
			}
		}

		private TimeSpan GetRestTime(DateTime limitTime)
		{
			return limitTime - NetBase.GetCurrentTime();
		}

		private string GetRestTimeText(TimeSpan restTime)
		{
			int hours = restTime.Hours;
			int minutes = restTime.Minutes;
			int seconds = restTime.Seconds;
			return string.Format("{0}:{1}:{2}", hours.ToString("D2"), minutes.ToString("D2"), seconds.ToString("D2"));
		}
	}

	private enum PlayerAnimation
	{
		RUN,
		IDLE,
		COUNT
	}

	private enum SuggestedIconType
	{
		TYPE01,
		TYPE02,
		TYPE03,
		NUM
	}

	[Serializable]
	private class RouteObjects
	{
		[SerializeField]
		public UISprite m_lineSprite;

		[SerializeField]
		public GameObject m_lineEffectGameObject;

		[SerializeField]
		public GameObject m_bonusRootGameObject;

		[SerializeField]
		public UISprite m_bonusTypeSprite;

		[SerializeField]
		public UILabel m_bonusValueLabel;

		[SerializeField]
		public TweenPosition m_bonusTweenPosition;
	}

	[Serializable]
	private class BalloonObjects
	{
		[SerializeField]
		public GameObject m_gameObject;

		[SerializeField]
		public UITexture m_texture;

		[SerializeField]
		public GameObject m_effectGameObject;

		[SerializeField]
		public GameObject m_normalFrameObject;

		[SerializeField]
		public GameObject m_timerFrameObject;

		[SerializeField]
		public GameObject m_timerLimitObject;

		[SerializeField]
		public GameObject m_timerWordObject;
	}

	private enum DisplayType
	{
		RANK,
		RSRING,
		RING,
		NUM
	}

	private enum ArraveType
	{
		POINT,
		FINISH,
		POINT_FINISH,
		RUNNIG
	}

	private enum EventSignal
	{
		CLICK_NEXT = 100,
		CLICK_ALL_SKIP
	}

	private sealed class _DelayStart_c__Iterator41 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal int _waite_frame___0;

		internal int _PC;

		internal object _current;

		internal ui_mm_mileage_page __f__this;

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
				this._waite_frame___0 = 5;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._waite_frame___0 > 0)
			{
				this._waite_frame___0--;
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this.__f__this.m_mapInfo.highscore >= 0L)
			{
				this.__f__this.ChangeState(new TinyFsmState(new EventFunction(this.__f__this.StateHighScoreEvent)));
			}
			else
			{
				this.__f__this.ChangeState(new TinyFsmState(new EventFunction(this.__f__this.StateEvent)));
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

	private const int POINT_COUNT = 6;

	private const int ROUTE_COUNT = 5;

	private const int BALLOON_COUNT = 5;

	private const int BOSS_EVENT_INDEX = 4;

	private const float BALLOON_WAIT = 0.5f;

	private static ui_mm_mileage_page instance;

	[SerializeField]
	private bool m_disabled;

	[SerializeField]
	private float m_playerRunSpeed = 1f;

	[SerializeField]
	private float m_eventWait = 0.5f;

	[SerializeField]
	private GameObject m_playerGameObject;

	[SerializeField]
	private UISprite m_playerSprite;

	[SerializeField]
	private UISpriteAnimation[] m_playerSpriteAnimations = new UISpriteAnimation[2];

	[SerializeField]
	private UISlider m_playerSlider;

	[SerializeField]
	private GameObject m_playerEffGameObject;

	[SerializeField]
	private UISprite[] m_waypointsSprite = new UISprite[6];

	[SerializeField]
	private ui_mm_mileage_page.RouteObjects[] m_routesObjects = new ui_mm_mileage_page.RouteObjects[5];

	[SerializeField]
	private ui_mm_mileage_page.BalloonObjects[] m_balloonsObjects = new ui_mm_mileage_page.BalloonObjects[5];

	[SerializeField]
	private UILabel m_scenarioNumberLabel;

	[SerializeField]
	private UILabel m_titleLabel;

	[SerializeField]
	private UILabel m_distanceLabel;

	[SerializeField]
	private UILabel m_advanceDistanceLabel;

	[SerializeField]
	private GameObject m_advanceDistanceGameObject;

	[SerializeField]
	private GameObject m_patternNextObject;

	[SerializeField]
	private GameObject m_btnNextObject;

	[SerializeField]
	private GameObject m_btnSkipObject;

	[SerializeField]
	private GameObject m_btnPlayObject;

	[SerializeField]
	private UITexture m_stageBGTex;

	private UISlider m_distanceSlider;

	private ui_mm_mileage_page.MapInfo m_mapInfo;

	private Queue<ui_mm_mileage_page.BaseEvent> m_events = new Queue<ui_mm_mileage_page.BaseEvent>();

	private ui_mm_mileage_page.BaseEvent m_event;

	private ui_mm_mileage_page.PointTimeLimit[] m_limitDatas = new ui_mm_mileage_page.PointTimeLimit[5];

	private SoundManager.PlayId m_runSePlayId;

	private UITexture m_bannerTex;

	private GameObject m_bannerObj;

	private GameObject m_eventBannerObj;

	private long m_infoId = -1L;

	private InformationWindow m_infoWindow;

	private bool m_isInit;

	private bool m_isStart;

	private bool m_isNext;

	private bool m_isSkipMileage;

	private bool m_isProduction;

	private bool m_isReachTarget;

	private int[] m_displayOffset = new int[3];

	private TinyFsmBehavior m_fsm_behavior;

	public static ui_mm_mileage_page Instance
	{
		get
		{
			return ui_mm_mileage_page.instance;
		}
	}

	private void AddEvent(ui_mm_mileage_page.BaseEvent baseEvent, int waitType = -1)
	{
		this.StopRunSe();
		if (this.m_isSkipMileage)
		{
			baseEvent.SkipMileageProcess();
		}
		else
		{
			if (waitType < 0)
			{
				this.m_events.Enqueue(new ui_mm_mileage_page.WaitEvent(base.gameObject, this.m_eventWait));
			}
			this.m_events.Enqueue(baseEvent);
			if (waitType > 0)
			{
				this.m_events.Enqueue(new ui_mm_mileage_page.WaitEvent(base.gameObject, this.m_eventWait));
			}
		}
	}

	private void AddEventPostWait(ui_mm_mileage_page.BaseEvent baseEvent)
	{
		this.AddEvent(baseEvent, 1);
	}

	private void AddEventNoWait(ui_mm_mileage_page.BaseEvent baseEvent)
	{
		this.AddEvent(baseEvent, 0);
	}

	private void AddWaypointEvents()
	{
		this.AddEventNoWait(new ui_mm_mileage_page.BalloonEffectEvent(base.gameObject, this.m_mapInfo.waypointIndex));
		MileageMapData mileageMapData = MileageMapDataManager.Instance.GetMileageMapData();
		if (mileageMapData != null)
		{
			if (this.m_mapInfo.waypointIndex < 5)
			{
				EventData event_data = mileageMapData.event_data;
				PointEventData pointEventData = event_data.point[this.m_mapInfo.waypointIndex];
				ui_mm_mileage_page.WayPointEventType wayPointEventType = (ui_mm_mileage_page.WayPointEventType)((this.m_mapInfo.waypointIndex != 0) ? pointEventData.event_type : 2);
				ui_mm_mileage_page.WayPointEventType wayPointEventType2 = wayPointEventType;
				if (wayPointEventType2 != ui_mm_mileage_page.WayPointEventType.SIMPLE)
				{
					if (wayPointEventType2 == ui_mm_mileage_page.WayPointEventType.GORGEOUS)
					{
						if (pointEventData.window_id > -1)
						{
							this.AddEventPostWait(new ui_mm_mileage_page.GorgeousEvent(base.gameObject, pointEventData.window_id, false));
						}
						if (pointEventData.reward.reward_id > -1 || pointEventData.window_id == -1)
						{
							this.AddEventPostWait(new ui_mm_mileage_page.SimpleEvent(base.gameObject, pointEventData.reward.serverId, pointEventData.reward.reward_count, MileageMapUtility.GetText("gw_item_caption", null), false));
						}
					}
				}
				else
				{
					this.AddEventPostWait(new ui_mm_mileage_page.SimpleEvent(base.gameObject, pointEventData.reward.serverId, pointEventData.reward.reward_count, MileageMapUtility.GetText("gw_item_caption", null), false));
				}
			}
			else
			{
				EventData event_data2 = mileageMapData.event_data;
				if (!event_data2.IsBossEvent())
				{
					if (event_data2.point[this.m_mapInfo.waypointIndex].balloon_on_arrival_face_id != -1)
					{
						this.AddEventPostWait(new ui_mm_mileage_page.BalloonEvent(base.gameObject, this.m_mapInfo.waypointIndex - 1, event_data2.point[this.m_mapInfo.waypointIndex].balloon_on_arrival_face_id, event_data2.point[this.m_mapInfo.waypointIndex].balloon_face_id));
					}
				}
				else if (event_data2.point[this.m_mapInfo.waypointIndex].boss.balloon_on_arrival_face_id != -1)
				{
					this.AddEventPostWait(new ui_mm_mileage_page.BalloonEvent(base.gameObject, this.m_mapInfo.waypointIndex - 1, event_data2.point[this.m_mapInfo.waypointIndex].boss.balloon_on_arrival_face_id, event_data2.point[this.m_mapInfo.waypointIndex].boss.balloon_init_face_id));
				}
				if (this.m_mapInfo.tutorialPhase != ui_mm_mileage_page.MapInfo.TutorialPhase.FIRST_EPISODE || !event_data2.IsBossEvent() || this.m_mapInfo.isBossDestroyed)
				{
					int num = event_data2.IsBossEvent() ? (this.m_mapInfo.isBossDestroyed ? event_data2.GetBossEvent().after_window_id : event_data2.GetBossEvent().before_window_id) : event_data2.point[this.m_mapInfo.waypointIndex].window_id;
					if (num > -1)
					{
						this.AddEventNoWait(new ui_mm_mileage_page.GorgeousEvent(base.gameObject, num, this.IsExistsMapClearEvents()));
					}
				}
				this.AddMapClearEvents();
			}
		}
	}

	private bool IsExistsMapClearEvents()
	{
		bool flag = MileageMapDataManager.Instance.GetMileageMapData().event_data.IsBossEvent() && !this.m_mapInfo.isBossDestroyed;
		return !flag;
	}

	private void AddMapClearEvents()
	{
		if (!this.IsExistsMapClearEvents())
		{
			return;
		}
		this.AddEvent(new ui_mm_mileage_page.BgmEvent(base.gameObject, "jingle_sys_mapclear"), -1);
		bool flag = true;
		RewardData[] reward = MileageMapDataManager.Instance.GetMileageMapData().map_data.reward;
		for (int i = 0; i < reward.Length; i++)
		{
			RewardData rewardData = reward[i];
			if (rewardData.reward_id != -1 && rewardData.reward_count > 0)
			{
				ui_mm_mileage_page.BaseEvent baseEvent = new ui_mm_mileage_page.SimpleEvent(base.gameObject, rewardData.serverId, rewardData.reward_count, MileageMapUtility.GetText("gw_map_bonus_caption", null), true);
				if (flag)
				{
					this.AddEventNoWait(baseEvent);
					flag = false;
				}
				else
				{
					this.AddEvent(baseEvent, -1);
				}
			}
		}
		if (MileageMapDataManager.Instance.GetMileageMapData().scenario.last_chapter_flag != 0)
		{
			RewardData[] reward2 = MileageMapDataManager.Instance.GetMileageMapData().scenario.reward;
			for (int j = 0; j < reward2.Length; j++)
			{
				RewardData rewardData2 = reward2[j];
				if (rewardData2.reward_id != -1 && rewardData2.reward_count > 0)
				{
					this.AddEvent(new ui_mm_mileage_page.SimpleEvent(base.gameObject, rewardData2.serverId, rewardData2.reward_count, MileageMapUtility.GetText("gw_scenario_bonus_caption", null), true), -1);
				}
			}
		}
		this.AddEvent(new ui_mm_mileage_page.RankUpEvent(base.gameObject), -1);
		this.AddEventNoWait(new ui_mm_mileage_page.BgmEvent(base.gameObject, null));
		this.AddEvent(new ui_mm_mileage_page.MapEvent(base.gameObject), -1);
		int window_id = MileageMapDataManager.Instance.GetMileageMapData().event_data.point[0].window_id;
		this.AddEvent(new ui_mm_mileage_page.GorgeousEvent(base.gameObject, window_id, false, true), -1);
	}

	private double Minimum(double a, double b)
	{
		if (a < b)
		{
			return a;
		}
		return b;
	}

	private void Awake()
	{
		if (ui_mm_mileage_page.instance == null)
		{
			ui_mm_mileage_page.instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		if (this.m_disabled)
		{
			base.enabled = false;
			return;
		}
		this.m_fsm_behavior = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		if (this.m_fsm_behavior != null)
		{
			TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
			description.initState = new TinyFsmState(new EventFunction(this.StateIdle));
			this.m_fsm_behavior.SetUp(description);
		}
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Pgb_score");
		if (gameObject != null)
		{
			this.m_distanceSlider = gameObject.GetComponent<UISlider>();
		}
		this.m_playerSpriteAnimations = this.m_playerSpriteAnimations[0].gameObject.GetComponents<UISpriteAnimation>();
		HudMenuUtility.SetTagHudMileageMap(base.gameObject);
	}

	private void OnDestroy()
	{
		if (ui_mm_mileage_page.instance == this)
		{
			if (ui_mm_mileage_page.instance.m_fsm_behavior != null)
			{
				ui_mm_mileage_page.instance.m_fsm_behavior.ShutDown();
				ui_mm_mileage_page.instance.m_fsm_behavior = null;
			}
			ui_mm_mileage_page.instance = null;
		}
	}

	private void Update()
	{
		if (!this.m_isInit)
		{
			return;
		}
		for (int i = 0; i < 5; i++)
		{
			this.m_limitDatas[i].Update();
		}
	}

	private void ChangeState(TinyFsmState nextState)
	{
		this.m_fsm_behavior.ChangeState(nextState);
	}

	private TinyFsmState StateIdle(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateHighScoreEvent(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (this.IsRankingUp())
			{
				this.AddEvent(new ui_mm_mileage_page.RankingUPEvent(base.gameObject), -1);
			}
			this.AddEvent(new ui_mm_mileage_page.HighscoreEvent(base.gameObject, this.m_mapInfo.highscore), -1);
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_27:
			if (signal != 101)
			{
				return TinyFsmState.End();
			}
			this.m_isSkipMileage = true;
			this.m_events.Clear();
			return TinyFsmState.End();
		case 4:
			if (this.m_event != null && this.m_event.Update())
			{
				this.m_event = null;
			}
			if (this.m_event == null)
			{
				if (this.m_events.Count > 0)
				{
					this.m_event = this.m_events.Dequeue();
					this.m_event.Start();
				}
				else
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateEvent)));
				}
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		goto IL_27;
	}

	private TinyFsmState StateEvent(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			if (this.m_mapInfo.isBossDestroyed)
			{
				this.SetBossClearEvent();
				this.m_isReachTarget = true;
			}
			this.SetSkipBtnEnable(false);
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_27:
			if (signal != 101)
			{
				return TinyFsmState.End();
			}
			this.m_isSkipMileage = true;
			this.m_events.Clear();
			return TinyFsmState.End();
		case 4:
			if (this.m_event != null && this.m_event.Update())
			{
				this.m_event = null;
			}
			if (this.m_event == null)
			{
				bool flag = true;
				if (this.m_events.Count > 0)
				{
					this.m_event = this.m_events.Dequeue();
					this.m_event.Start();
					flag = false;
				}
				if (flag)
				{
					if (this.m_isSkipMileage)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StageAllSkip)));
					}
					else if (this.m_isReachTarget)
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StageDailyMission)));
					}
					else
					{
						this.ChangeState(new TinyFsmState(new EventFunction(this.StageRun)));
					}
				}
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		goto IL_27;
	}

	private TinyFsmState StageRun(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			this.SetRunEffect(false);
			this.SetSkipBtnEnable(false);
			return TinyFsmState.End();
		case 1:
			this.m_runSePlayId = SoundManager.SePlay("sys_distance", "SE");
			this.SetRunEffect(true);
			this.SetSkipBtnEnable(true);
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_27:
			if (signal == 100)
			{
				this.m_isNext = true;
				return TinyFsmState.End();
			}
			if (signal != 101)
			{
				return TinyFsmState.End();
			}
			this.m_isSkipMileage = true;
			return TinyFsmState.End();
		case 4:
			if (this.m_isSkipMileage)
			{
				this.ChangeState(new TinyFsmState(new EventFunction(this.StageAllSkip)));
			}
			else
			{
				this.RunPlayer();
				switch (this.CheckRun())
				{
				case ui_mm_mileage_page.ArraveType.POINT:
					this.StopRunSe();
					this.m_mapInfo.waypointIndex++;
					this.SetBalloonsView(true);
					this.AddWaypointEvents();
					this.SetDistanceDsiplay();
					this.SetDistanceDsiplayPos();
					this.SetDisableBolloonView();
					SoundManager.SePlay("sys_arrive", "SE");
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateEvent)));
					break;
				case ui_mm_mileage_page.ArraveType.FINISH:
					this.StopRunSe();
					this.m_mapInfo.waypointIndex = ServerInterface.MileageMapState.m_point;
					this.SetDistanceDsiplay();
					this.SetDistanceDsiplayPos();
					this.m_advanceDistanceGameObject.SetActive(false);
					this.SetDisableBolloonView();
					SoundManager.SePlay("sys_arrive", "SE");
					this.m_isReachTarget = true;
					this.m_isSkipMileage = false;
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateEvent)));
					break;
				case ui_mm_mileage_page.ArraveType.POINT_FINISH:
					this.StopRunSe();
					this.m_mapInfo.waypointIndex++;
					this.SetBalloonsView(true);
					this.AddWaypointEvents();
					this.SetDistanceDsiplay();
					this.SetDistanceDsiplayPos();
					this.m_advanceDistanceGameObject.SetActive(false);
					SoundManager.SePlay("sys_arrive", "SE");
					this.SetDisableBolloonView();
					this.m_isReachTarget = true;
					this.m_isSkipMileage = false;
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateEvent)));
					break;
				}
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		goto IL_27;
	}

	private TinyFsmState StageAllSkip(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.SetAllSkip();
			return TinyFsmState.End();
		case 4:
			if (this.m_event != null && this.m_event.Update())
			{
				this.m_event = null;
			}
			if (this.m_event == null)
			{
				bool flag = true;
				if (this.m_events.Count > 0)
				{
					this.m_event = this.m_events.Dequeue();
					this.m_event.Start();
					flag = false;
				}
				if (flag)
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StageDailyMission)));
				}
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StageDailyMission(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			this.SetRunEffect(false);
			return TinyFsmState.End();
		case 1:
			this.m_isSkipMileage = false;
			if (this.CheckClearDailyMission())
			{
				this.AddEvent(new ui_mm_mileage_page.DailyMissionEvent(base.gameObject), -1);
				if (this.m_events.Count > 0)
				{
					this.m_event = this.m_events.Dequeue();
					this.m_event.Start();
				}
			}
			return TinyFsmState.End();
		case 4:
			if (this.m_event != null && this.m_event.Update())
			{
				this.m_event = null;
			}
			if (this.m_event == null)
			{
				if (this.m_events.Count > 0)
				{
					this.m_event = this.m_events.Dequeue();
					this.m_event.Start();
				}
				else
				{
					this.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
				}
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEnd(TinyFsmEvent fsm_event)
	{
		int signal = fsm_event.Signal;
		switch (signal + 4)
		{
		case 0:
			this.m_isProduction = false;
			return TinyFsmState.End();
		case 1:
			this.SetEndMileageProduction();
			return TinyFsmState.End();
		case 4:
			this.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void Initialize()
	{
		this.m_isNext = false;
		this.m_isSkipMileage = false;
		this.m_mapInfo = new ui_mm_mileage_page.MapInfo();
		for (int i = 0; i < 5; i++)
		{
			this.m_limitDatas[i] = new ui_mm_mileage_page.PointTimeLimit();
		}
		this.m_playerSpriteAnimations[0].enabled = false;
		this.m_playerSpriteAnimations[1].enabled = true;
		this.m_playerEffGameObject.SetActive(false);
		this.m_runSePlayId = SoundManager.PlayId.NONE;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "Anchor_5_MC");
		if (gameObject != null)
		{
			this.m_eventBannerObj = GameObjectUtil.FindChildGameObject(gameObject, "event_banner");
			if (this.m_eventBannerObj != null)
			{
				GameObject gameObject2 = GameObjectUtil.FindChildGameObject(this.m_eventBannerObj, "banner_slot");
				if (gameObject2 != null)
				{
					this.m_bannerObj = GameObjectUtil.FindChildGameObject(gameObject2, "img_ad_tex");
					if (this.m_bannerObj != null)
					{
						UIButtonMessage component = this.m_bannerObj.GetComponent<UIButtonMessage>();
						if (component != null)
						{
							component.enabled = true;
							component.trigger = UIButtonMessage.Trigger.OnClick;
							component.target = base.gameObject;
							component.functionName = "OnEventBannerClicked";
						}
						this.m_bannerTex = this.m_bannerObj.GetComponent<UITexture>();
					}
				}
				this.m_eventBannerObj.SetActive(false);
			}
		}
		this.m_isInit = true;
	}

	private void RunPlayer()
	{
		if (this.m_isNext)
		{
			this.m_mapInfo.scoreDistance = this.Minimum(this.m_mapInfo.nextWaypointDistance, this.m_mapInfo.targetScoreDistance);
			this.m_isNext = false;
		}
		else if (this.m_isSkipMileage)
		{
			this.m_mapInfo.scoreDistance = this.m_mapInfo.targetScoreDistance;
		}
		else
		{
			this.m_mapInfo.scoreDistance = this.Minimum(this.m_mapInfo.scoreDistance + (double)(this.m_playerRunSpeed * Time.deltaTime), this.m_mapInfo.targetScoreDistance);
			this.m_mapInfo.scoreDistance = this.Minimum(this.m_mapInfo.scoreDistance, this.m_mapInfo.nextWaypointDistance);
		}
		this.SetPlayerPosition();
		this.SetDistanceDsiplay();
	}

	private ui_mm_mileage_page.ArraveType CheckRun()
	{
		if (this.m_mapInfo.scoreDistance == this.m_mapInfo.nextWaypointDistance && this.m_mapInfo.waypointIndex < 5)
		{
			if (this.m_mapInfo.scoreDistance == this.m_mapInfo.targetScoreDistance)
			{
				return ui_mm_mileage_page.ArraveType.POINT_FINISH;
			}
			return ui_mm_mileage_page.ArraveType.POINT;
		}
		else
		{
			if (this.m_mapInfo.scoreDistance == this.m_mapInfo.targetScoreDistance)
			{
				return ui_mm_mileage_page.ArraveType.FINISH;
			}
			return ui_mm_mileage_page.ArraveType.RUNNIG;
		}
	}

	public void SetBG()
	{
		if (MileageMapDataManager.Instance != null)
		{
			int mileageStageIndex = MileageMapDataManager.Instance.MileageStageIndex;
		}
		if (this.m_stageBGTex != null)
		{
			Texture bGTexture = MileageMapUtility.GetBGTexture();
			if (bGTexture != null)
			{
				this.m_stageBGTex.mainTexture = bGTexture;
			}
		}
	}

	private void SetRunEffect(bool flag)
	{
		this.m_playerSpriteAnimations[0].enabled = flag;
		this.m_playerSpriteAnimations[1].enabled = !flag;
		this.m_playerEffGameObject.SetActive(flag);
	}

	private void StopRunSe()
	{
		SoundManager.SeStop("sys_distance", "SE");
		this.m_runSePlayId = SoundManager.PlayId.NONE;
	}

	private void SetArraivalFaceTexture()
	{
		PointEventData pointEventData = MileageMapDataManager.Instance.GetMileageMapData().event_data.point[5];
		if (pointEventData != null)
		{
			this.SetBalloonFaceTexture(4, pointEventData.balloon_on_arrival_face_id);
		}
	}

	private void SetBossClearEvent()
	{
		BossEvent bossEvent = MileageMapDataManager.Instance.GetMileageMapData().event_data.GetBossEvent();
		this.AddEvent(new ui_mm_mileage_page.BalloonEvent(base.gameObject, 4, bossEvent.balloon_clear_face_id, bossEvent.balloon_on_arrival_face_id), -1);
		this.AddEvent(new ui_mm_mileage_page.GorgeousEvent(base.gameObject, bossEvent.after_window_id, this.IsExistsMapClearEvents()), -1);
		this.AddMapClearEvents();
	}

	private bool CheckClearDailyMission()
	{
		return this.m_mapInfo.m_resultData != null && this.m_mapInfo.m_resultData.m_missionComplete;
	}

	private bool IsRankingUp()
	{
		if (SingletonGameObject<RankingManager>.Instance != null)
		{
			RankingUtil.RankChange rankingRankChange = SingletonGameObject<RankingManager>.Instance.GetRankingRankChange(RankingUtil.RankingMode.ENDLESS, RankingUtil.RankingScoreType.HIGH_SCORE, RankingUtil.RankingRankerType.RIVAL);
			return rankingRankChange == RankingUtil.RankChange.UP;
		}
		return false;
	}

	private void SetAll()
	{
		this.SetPlayerPosition();
		this.SetWaypoints();
		this.SetRoutes();
		this.SetTimeLimit();
		this.SetBalloonsView(false);
		this.SetUchanged();
		this.SetDistanceDsiplay();
		this.SetDistanceDsiplayPos();
		double a = (double)MileageMapDataManager.Instance.GetMileageMapData().map_data.event_interval;
		double b = this.m_mapInfo.targetScoreDistance - this.m_mapInfo.scoreDistance;
		this.m_playerRunSpeed = (float)this.Minimum(a, b);
	}

	private void SetPlayerPosition()
	{
		double num = this.m_mapInfo.scoreDistance / (double)(ui_mm_mileage_page.MapInfo.routeScoreDistance * 5);
		if (this.m_playerSlider != null)
		{
			this.m_playerSlider.value = (float)num;
		}
	}

	private void SetWaypoints()
	{
		this.SetWaypoint(this.m_waypointsSprite[0], 2);
		EventData event_data = MileageMapDataManager.Instance.GetMileageMapData().event_data;
		if (event_data.point.Length == 6)
		{
			this.SetWaypoint(this.m_waypointsSprite[1], event_data.point[1].event_type);
			this.SetWaypoint(this.m_waypointsSprite[2], event_data.point[2].event_type);
			this.SetWaypoint(this.m_waypointsSprite[3], event_data.point[3].event_type);
			this.SetWaypoint(this.m_waypointsSprite[4], event_data.point[4].event_type);
			this.SetWaypoint(this.m_waypointsSprite[5], event_data.point[5].event_type);
		}
	}

	private void SetWaypoint(UISprite sprite, int point_id)
	{
		if (sprite != null)
		{
			sprite.spriteName = MileageMapPointDataTable.Instance.GetTextureName(point_id);
		}
	}

	private void SetRoutes()
	{
		for (int i = 0; i < 5; i++)
		{
			if (this.m_routesObjects[i] != null)
			{
				if (this.m_routesObjects[i].m_bonusRootGameObject != null)
				{
					this.m_routesObjects[i].m_bonusRootGameObject.SetActive(false);
				}
				if (this.m_routesObjects[i].m_lineSprite != null)
				{
					this.m_routesObjects[i].m_lineSprite.spriteName = "ui_mm_mileage_route_1";
				}
				if (this.m_routesObjects[i].m_lineEffectGameObject != null)
				{
					this.m_routesObjects[i].m_lineEffectGameObject.SetActive(false);
				}
			}
		}
	}

	private void SetBalloonsView(bool disable_on_arrival = false)
	{
		for (int i = 0; i < 5; i++)
		{
			this.SetBalloonView(i, disable_on_arrival);
		}
	}

	private void SetTimeLimit()
	{
		for (int i = 0; i < 5; i++)
		{
			if (this.m_limitDatas[i] != null)
			{
				this.m_limitDatas[i].Reset();
			}
		}
		MileageMapDataManager mileageMapDataManager = MileageMapDataManager.Instance;
		int episode = mileageMapDataManager.GetMileageMapData().scenario.episode;
		int chapter = mileageMapDataManager.GetMileageMapData().scenario.chapter;
		for (int j = 0; j < 5; j++)
		{
			int point = j + 1;
			ServerMileageReward mileageReward = mileageMapDataManager.GetMileageReward(episode, chapter, point);
			if (mileageReward != null && this.m_limitDatas[j] != null)
			{
				bool incentiveFlag = this.m_mapInfo.CheckMileageIncentive(point);
				this.m_limitDatas[j].SetupLimit(mileageReward, this.m_balloonsObjects[j], incentiveFlag);
			}
		}
	}

	private void SetDisableBolloonView()
	{
		for (int i = 0; i < 4; i++)
		{
			int num = (i + 1) * ui_mm_mileage_page.MapInfo.routeScoreDistance;
			if ((double)num <= this.m_mapInfo.scoreDistance)
			{
				this.m_balloonsObjects[i].m_gameObject.SetActive(false);
			}
		}
	}

	private void SetBalloonView(int eventIndex, bool disable_on_arrival)
	{
		int num = eventIndex + 1;
		EventData event_data = MileageMapDataManager.Instance.GetMileageMapData().event_data;
		int num2 = (num >= 5) ? (event_data.IsBossEvent() ? ((this.m_mapInfo.waypointIndex >= 5 && !disable_on_arrival) ? event_data.GetBossEvent().balloon_on_arrival_face_id : event_data.GetBossEvent().balloon_init_face_id) : ((this.m_mapInfo.waypointIndex >= 5 && event_data.point[num].balloon_on_arrival_face_id != -1 && !disable_on_arrival) ? event_data.point[num].balloon_on_arrival_face_id : event_data.point[num].balloon_face_id)) : event_data.point[num].balloon_face_id;
		int num3 = eventIndex + 1;
		bool flag = num2 >= 0 && this.m_mapInfo.scoreDistance <= (double)(num3 * ui_mm_mileage_page.MapInfo.routeScoreDistance);
		if (flag && this.m_limitDatas[eventIndex] != null && this.m_limitDatas[eventIndex].FailedFlag)
		{
			flag = false;
		}
		this.m_balloonsObjects[eventIndex].m_gameObject.SetActive(flag);
		if (flag)
		{
			this.SetBalloonFrame(eventIndex);
			this.SetBalloonFaceTexture(eventIndex, num2);
		}
	}

	private void SetBalloonFrame(int eventIndex)
	{
		ui_mm_mileage_page.BalloonObjects balloonObjects = this.m_balloonsObjects[eventIndex];
		bool limitFlag = this.m_limitDatas[eventIndex].LimitFlag;
		if (balloonObjects.m_normalFrameObject != null)
		{
			balloonObjects.m_normalFrameObject.SetActive(!limitFlag);
		}
		if (balloonObjects.m_timerFrameObject != null)
		{
			balloonObjects.m_timerFrameObject.SetActive(limitFlag);
		}
	}

	private void SetBalloonFaceTexture(int eventIndex, int faceId)
	{
		ui_mm_mileage_page.BalloonObjects balloonObjects = this.m_balloonsObjects[eventIndex];
		Texture texture = MileageMapUtility.GetFaceTexture(faceId) ?? GeneralWindow.GetDummyTexture(faceId);
		if (balloonObjects.m_texture != null && balloonObjects.m_texture.mainTexture != texture)
		{
			balloonObjects.m_texture.mainTexture = texture;
		}
	}

	private void SetUchanged()
	{
		int episode = MileageMapDataManager.Instance.GetMileageMapData().scenario.episode;
		int chapter = MileageMapDataManager.Instance.GetMileageMapData().scenario.chapter;
		this.m_scenarioNumberLabel.text = episode.ToString("000") + "-" + chapter.ToString();
		string title_cell_id = MileageMapDataManager.Instance.GetMileageMapData().scenario.title_cell_id;
		this.m_titleLabel.text = MileageMapText.GetText(episode, title_cell_id);
		bool active = this.m_mapInfo.m_resultData != null;
		if (this.m_mapInfo.isBossStage)
		{
			active = false;
		}
		else if (this.m_mapInfo.isNextMileage)
		{
			active = false;
		}
		else if (this.m_mapInfo.scoreDistanceRaw == 0.0)
		{
			active = false;
		}
		this.m_advanceDistanceGameObject.SetActive(active);
		this.m_advanceDistanceLabel.text = HudUtility.GetFormatNumString<double>(this.m_mapInfo.scoreDistanceRaw);
	}

	private void SetDistanceDsiplay()
	{
		this.m_distanceLabel.text = HudUtility.GetFormatNumString<double>((double)(ui_mm_mileage_page.MapInfo.routeScoreDistance * this.m_mapInfo.nextWaypoint) - this.m_mapInfo.scoreDistance);
		this.m_advanceDistanceLabel.text = HudUtility.GetFormatNumString<double>(this.m_mapInfo.scoreDistanceRaw - this.m_mapInfo.GetRunDistance());
	}

	private void SetDistanceDsiplayPos()
	{
		if (this.m_mapInfo.waypointIndex == 5)
		{
			if (this.m_distanceSlider != null)
			{
				this.m_distanceSlider.gameObject.SetActive(false);
			}
		}
		else
		{
			float num = (float)(this.m_mapInfo.waypointIndex + 1) * 0.2f;
			if (num > 1f)
			{
				num = 1f;
			}
			if (this.m_distanceSlider != null)
			{
				this.m_distanceSlider.gameObject.SetActive(true);
				this.m_distanceSlider.value = num;
			}
		}
	}

	private void SetPlayBtnImg()
	{
		if (this.m_btnPlayObject != null)
		{
			UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_btnPlayObject, "img_word_play");
			if (uISprite != null)
			{
				if (MileageMapUtility.IsBossStage())
				{
					uISprite.spriteName = "ui_mm_btn_word_play_boss";
				}
				else
				{
					uISprite.spriteName = "ui_mm_btn_word_play";
				}
			}
			int stageIndex = 1;
			if (MileageMapDataManager.Instance != null)
			{
				stageIndex = MileageMapDataManager.Instance.MileageStageIndex;
			}
			CharacterAttribute[] characterAttribute = MileageMapUtility.GetCharacterAttribute(stageIndex);
			if (characterAttribute != null)
			{
				for (int i = 0; i < 3; i++)
				{
					string name = "img_icon_type_" + (i + 1).ToString();
					UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_btnPlayObject, name);
					if (uISprite2 != null)
					{
						if (i < characterAttribute.Length)
						{
							switch (characterAttribute[i])
							{
							case CharacterAttribute.SPEED:
								uISprite2.enabled = true;
								uISprite2.spriteName = "ui_chao_set_type_icon_speed";
								break;
							case CharacterAttribute.FLY:
								uISprite2.enabled = true;
								uISprite2.spriteName = "ui_chao_set_type_icon_fly";
								break;
							case CharacterAttribute.POWER:
								uISprite2.enabled = true;
								uISprite2.spriteName = "ui_chao_set_type_icon_power";
								break;
							default:
								uISprite2.enabled = false;
								break;
							}
						}
						else
						{
							uISprite2.enabled = false;
						}
					}
				}
			}
			UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_btnPlayObject, "img_next_map");
			if (uISprite3 != null)
			{
				uISprite3.spriteName = "ui_mm_map_thumb_w" + stageIndex.ToString("00") + "a";
			}
		}
	}

	private void SetEndMileageProduction()
	{
		this.ResetRewindOffsetToSaveData();
		if (this.m_patternNextObject != null)
		{
			this.m_patternNextObject.SetActive(false);
		}
		HudMenuUtility.SendEnableShopButton(true);
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
		this.SetBannerCollider(true);
		if (this.IsChangeDataVersion() || this.IsTutorialEvent())
		{
			HudMenuUtility.SendMenuButtonClicked(ButtonInfoTable.ButtonType.EPISODE_BACK, false);
		}
	}

	private bool IsChangeDataVersion()
	{
		return ServerInterface.LoginState != null && (ServerInterface.LoginState.IsChangeDataVersion || ServerInterface.LoginState.IsChangeAssetsVersion);
	}

	private bool IsTutorialEvent()
	{
		return HudMenuUtility.IsTutorialCharaLevelUp() || HudMenuUtility.IsRouletteTutorial() || HudMenuUtility.IsRecommendReviewTutorial();
	}

	private void SetSkipBtnEnable(bool flag)
	{
		if (this.m_btnNextObject != null)
		{
			BoxCollider component = this.m_btnNextObject.GetComponent<BoxCollider>();
			if (component != null)
			{
				component.isTrigger = !flag;
			}
		}
		if (this.m_btnSkipObject != null)
		{
			UIImageButton component2 = this.m_btnSkipObject.GetComponent<UIImageButton>();
			if (component2 != null)
			{
				component2.isEnabled = flag;
			}
		}
	}

	private void SetPlanelAlha()
	{
		UIPanel component = base.gameObject.GetComponent<UIPanel>();
		if (component != null)
		{
			component.alpha = 1f;
		}
	}

	public void StartMileageMapProduction()
	{
		base.StartCoroutine(this.DelayStart());
	}

	private void SetAllSkip()
	{
		this.StopRunSe();
		this.ResetRewindOffsetToSaveData();
		this.m_isSkipMileage = false;
		if (this.m_mapInfo.IsClearMileage())
		{
			this.SetMileageClearDisplayOffset_FromResultData(this.m_mapInfo.m_resultData);
			if (this.m_mapInfo.m_resultData != null && !this.m_mapInfo.m_resultData.m_bossDestroy)
			{
				this.m_mapInfo.scoreDistance = this.m_mapInfo.targetScoreDistance;
				this.m_mapInfo.waypointIndex = 5;
				this.SetBalloonsView(true);
				this.SetArraivalFaceTexture();
				this.SetDistanceDsiplay();
				this.SetDistanceDsiplayPos();
				this.SetPlayerPosition();
			}
			this.AddMapClearEvents();
		}
		else
		{
			SoundManager.BgmChange("bgm_sys_menu", "BGM");
			MileageMapDataManager.Instance.SetCurrentData(ServerInterface.MileageMapState.m_episode, ServerInterface.MileageMapState.m_chapter);
			MileageMapState mileageMapState = new MileageMapState();
			mileageMapState.m_episode = ServerInterface.MileageMapState.m_episode;
			mileageMapState.m_chapter = ServerInterface.MileageMapState.m_chapter;
			mileageMapState.m_point = ServerInterface.MileageMapState.m_point;
			mileageMapState.m_score = ServerInterface.MileageMapState.m_stageTotalScore;
			this.m_mapInfo.UpdateFrom(mileageMapState);
			this.SetPlayBtnImg();
			this.SetBG();
			this.SetAll();
		}
		this.m_advanceDistanceGameObject.SetActive(false);
		if (!this.m_isReachTarget)
		{
			SoundManager.SePlay("sys_arrive", "SE");
		}
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
	}

	public IEnumerator DelayStart()
	{
		ui_mm_mileage_page._DelayStart_c__Iterator41 _DelayStart_c__Iterator = new ui_mm_mileage_page._DelayStart_c__Iterator41();
		_DelayStart_c__Iterator.__f__this = this;
		return _DelayStart_c__Iterator;
	}

	private void OnStartMileage()
	{
		this.m_isStart = true;
		if (this.m_isInit && this.m_isProduction)
		{
			this.StartMileageMapProduction();
		}
		this.SetEventBanner();
	}

	private void OnEndMileage()
	{
	}

	public void OnUpdateMileageMapDisplay()
	{
		if (this.m_isInit)
		{
			return;
		}
		this.Initialize();
		MileageMapState mileageMapState = new MileageMapState();
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			mileageMapState.m_episode = ServerInterface.MileageMapState.m_episode;
			mileageMapState.m_chapter = ServerInterface.MileageMapState.m_chapter;
			mileageMapState.m_point = ServerInterface.MileageMapState.m_point;
			mileageMapState.m_score = ServerInterface.MileageMapState.m_stageTotalScore;
		}
		else
		{
			mileageMapState.m_episode = MileageMapDataManager.Instance.GetMileageMapData().scenario.episode;
			mileageMapState.m_chapter = MileageMapDataManager.Instance.GetMileageMapData().scenario.chapter;
			mileageMapState.m_point = 0;
			mileageMapState.m_score = 0L;
		}
		this.m_mapInfo.UpdateFrom(mileageMapState);
		this.SetPlayBtnImg();
		this.SetBG();
		this.SetSkipBtnEnable(false);
		this.SetAll();
		this.SetRunEffect(false);
		this.SetPlanelAlha();
		this.SetEventBanner();
	}

	public void OnPrepareMileageMapProduction(ResultData resultData)
	{
		if (this.m_isInit)
		{
			return;
		}
		if (resultData != null && resultData.m_quickMode)
		{
			this.OnUpdateMileageMapDisplay();
			return;
		}
		this.Initialize();
		this.SetDisplayOffset_FromResultData(resultData);
		this.m_mapInfo.UpdateFrom(resultData);
		this.SetPlayBtnImg();
		this.SetBG();
		this.SetAll();
		if (this.m_patternNextObject != null)
		{
			this.m_patternNextObject.SetActive(true);
		}
		this.SetRunEffect(false);
		this.SetPlanelAlha();
		HudMenuUtility.SendEnableShopButton(false);
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
		this.m_isProduction = true;
		BackKeyManager.AddMileageCallBack(base.gameObject);
		this.SetSkipBtnEnable(false);
		if (this.m_isStart)
		{
			this.StartMileageMapProduction();
		}
		this.SetEventBanner();
		this.SetBannerCollider(false);
	}

	private void OnClickNextBtn()
	{
		if (this.m_fsm_behavior != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
			this.m_fsm_behavior.Dispatch(signal);
		}
	}

	public void OnClickAllSkipBtn()
	{
		if (this.m_fsm_behavior != null)
		{
			TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(101);
			this.m_fsm_behavior.Dispatch(signal);
		}
	}

	private void OnClosedCharaGetWindow()
	{
		ui_mm_mileage_page.SimpleEvent simpleEvent = this.m_event as ui_mm_mileage_page.SimpleEvent;
		if (simpleEvent != null)
		{
			simpleEvent.isEnd = true;
		}
	}

	private void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (this.m_isProduction)
		{
			this.OnClickNextBtn();
			if (msg != null)
			{
				msg.StaySequence();
			}
		}
	}

	private void SetDisplayOffset_FromResultData(ResultData resultData)
	{
		if (this.m_displayOffset != null && this.m_displayOffset.Length == 3)
		{
			for (int i = 0; i < 3; i++)
			{
				this.m_displayOffset[i] = 0;
			}
			if (MileageMapUtility.IsRankUp_FromResultData(resultData))
			{
				this.m_displayOffset[0] = 1;
			}
			this.m_displayOffset[1] = MileageMapUtility.GetDisplayOffset_FromResultData(resultData, ServerItem.Id.RSRING);
			this.m_displayOffset[2] = MileageMapUtility.GetDisplayOffset_FromResultData(resultData, ServerItem.Id.RING);
		}
	}

	private void SetMileageClearDisplayOffset_FromResultData(ResultData resultData)
	{
		if (this.m_displayOffset != null && this.m_displayOffset.Length == 3)
		{
			for (int i = 0; i < 3; i++)
			{
				this.m_displayOffset[i] = 0;
			}
			if (MileageMapUtility.IsRankUp_FromResultData(resultData))
			{
				this.m_displayOffset[0] = 1;
			}
			this.m_displayOffset[1] = MileageMapUtility.GetMileageClearDisplayOffset_FromResultData(resultData, ServerItem.Id.RSRING);
			this.m_displayOffset[2] = MileageMapUtility.GetMileageClearDisplayOffset_FromResultData(resultData, ServerItem.Id.RING);
		}
		SaveDataManager saveDataManager = SaveDataManager.Instance;
		if (saveDataManager == null)
		{
			return;
		}
		PlayerData playerData = saveDataManager.PlayerData;
		if (playerData != null)
		{
			playerData.RankOffset = -this.m_displayOffset[0];
		}
		ItemData itemData = saveDataManager.ItemData;
		if (itemData != null)
		{
			itemData.RedRingCountOffset = -this.m_displayOffset[1];
			itemData.RingCountOffset = -this.m_displayOffset[2];
		}
	}

	private void ResetRewindOffsetToSaveData()
	{
		SaveDataManager saveDataManager = SaveDataManager.Instance;
		if (saveDataManager == null)
		{
			return;
		}
		PlayerData playerData = saveDataManager.PlayerData;
		if (playerData != null)
		{
			playerData.RankOffset = 0;
		}
		ItemData itemData = saveDataManager.ItemData;
		if (itemData != null)
		{
			itemData.RingCountOffset = 0;
			itemData.RedRingCountOffset = 0;
		}
	}

	private void SetEventBanner()
	{
		if (!this.m_isInit)
		{
			return;
		}
		bool flag = false;
		if (EventManager.Instance != null && EventManager.Instance.Type == EventManager.EventType.BGM)
		{
			EventStageData stageData = EventManager.Instance.GetStageData();
			if (stageData != null)
			{
				flag = stageData.IsEndlessModeBGM();
			}
		}
		if (flag)
		{
			if (ServerInterface.NoticeInfo != null && ServerInterface.NoticeInfo.m_eventItems != null)
			{
				foreach (NetNoticeItem current in ServerInterface.NoticeInfo.m_eventItems)
				{
					if (this.m_infoId != current.Id)
					{
						this.m_infoId = current.Id;
						if (InformationImageManager.Instance != null)
						{
							InformationImageManager.Instance.Load(current.ImageId, true, new Action<Texture2D>(this.OnLoadCallback));
						}
						if (this.m_eventBannerObj != null)
						{
							this.m_eventBannerObj.SetActive(true);
						}
						break;
					}
				}
			}
		}
		else
		{
			if (this.m_eventBannerObj != null)
			{
				this.m_eventBannerObj.SetActive(false);
			}
			if (this.m_bannerTex != null && this.m_bannerTex.mainTexture != null)
			{
				this.m_bannerTex.mainTexture = null;
			}
		}
	}

	public void OnLoadCallback(Texture2D texture)
	{
		if (this.m_bannerTex != null && texture != null)
		{
			this.m_bannerTex.mainTexture = texture;
		}
	}

	private void OnEventBannerClicked()
	{
		this.m_infoWindow = base.gameObject.GetComponent<InformationWindow>();
		if (this.m_infoWindow == null)
		{
			this.m_infoWindow = base.gameObject.AddComponent<InformationWindow>();
		}
		if (this.m_infoWindow != null && ServerInterface.NoticeInfo != null && ServerInterface.NoticeInfo.m_eventItems != null)
		{
			foreach (NetNoticeItem current in ServerInterface.NoticeInfo.m_eventItems)
			{
				if (this.m_infoId == current.Id)
				{
					InformationWindow.Information info = default(InformationWindow.Information);
					info.pattern = InformationWindow.ButtonPattern.OK;
					info.imageId = current.ImageId;
					info.caption = TextUtility.GetCommonText("Informaion", "announcement");
					GameObject cameraUIObject = HudMenuUtility.GetCameraUIObject();
					if (cameraUIObject != null)
					{
						GameObject newsWindowObj = GameObjectUtil.FindChildGameObject(cameraUIObject, "NewsWindow");
						this.m_infoWindow.Create(info, newsWindowObj);
						base.enabled = true;
						SoundManager.SePlay("sys_menu_decide", "SE");
					}
					break;
				}
			}
		}
	}

	private void SetBannerCollider(bool on)
	{
		if (this.m_bannerObj != null)
		{
			BoxCollider component = this.m_bannerObj.GetComponent<BoxCollider>();
			if (component != null)
			{
				component.isTrigger = !on;
			}
		}
	}
}
