using AnimationOrTween;
using DataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Text;
using UnityEngine;

public class EventRewardWindow : EventWindowBase
{
	private enum BUTTON_ACT
	{
		CLOSE,
		ROULETTE,
		NONE
	}

	private enum Mode
	{
		Idle,
		Wait,
		End
	}

	private sealed class _ScrollInit_c__Iterator12 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal float point;

		internal UIDraggablePanel list;

		internal int _PC;

		internal object _current;

		internal float ___point;

		internal UIDraggablePanel ___list;

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
				this._current = new WaitForSeconds(0.025f);
				this._PC = 1;
				return true;
			case 1u:
				this.list.verticalScrollBar.value = this.point;
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

	private const int DRAW_LINE_MAX = 4;

	private const int DRAW_LINE_POS = 3;

	private const float GET_ICON_SOUND_DELAY = 0.25f;

	private const string LBL_CAPTION = "Lbl_caption";

	private const string LBL_LEFT_TITLE = "Lbl_word_left_title";

	private const string LBL_LEFT_TITLE_SH = "Lbl_word_left_title_sh";

	private const string LBL_LEFT_NAME = "Lbl_chao_name";

	private const string LBL_RIGHT_TITLE = "ui_Lbl_word_object_total_main";

	private const string LBL_RIGHT_NUM = "Lbl_object_total_main_num";

	private const string IMG_LEFT_BG = "img_chao_bg";

	private const string IMG_RIGHT_ICON = "img_object_total_main";

	private const string IMG_TYPE_ICON = "img_type_icon";

	private const string TEX_LEFT_TEXTURE = "texture_chao";

	private const string GO_LIST = "list";

	[SerializeField]
	private GameObject orgEventScroll;

	[SerializeField]
	private UIPanel mainPanel;

	private List<GameObject> m_listObject;

	private EventRewardWindow.Mode m_mode;

	private int m_targetChao = -1;

	[SerializeField]
	private Animation animationData;

	private bool m_close;

	private EventRewardWindow.BUTTON_ACT m_btnAct = EventRewardWindow.BUTTON_ACT.NONE;

	private bool m_first = true;

	private float m_afterTime;

	private float m_rewardTime;

	private int m_attainment;

	private int m_currentAttainment;

	private bool m_isGetEffectEnd;

	private float m_initPoint;

	private List<float> m_getSoundDelay;

	private static EventRewardWindow s_instance;

	private static EventRewardWindow Instance
	{
		get
		{
			return EventRewardWindow.s_instance;
		}
	}

	protected override void SetObject()
	{
		GeneralUtil.SetRouletteBtnIcon(base.gameObject, "Btn_roulette");
		if (this.m_isSetObject)
		{
			return;
		}
		base.gameObject.transform.localPosition = default(Vector3);
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		List<string> list4 = new List<string>();
		list.Add("Lbl_caption");
		list.Add("Lbl_word_left_title");
		list.Add("Lbl_word_left_title_sh");
		list.Add("Lbl_chao_name");
		list.Add("ui_Lbl_word_object_total_main");
		list.Add("Lbl_object_total_main_num");
		list2.Add("img_chao_bg");
		list2.Add("img_object_total_main");
		list2.Add("img_type_icon");
		list3.Add("texture_chao");
		list4.Add("list");
		this.m_objectLabels = ObjectUtility.GetObjectLabel(base.gameObject, list);
		this.m_objectSprites = ObjectUtility.GetObjectSprite(base.gameObject, list2);
		this.m_objectTextures = ObjectUtility.GetObjectTexture(base.gameObject, list3);
		this.m_objects = ObjectUtility.GetGameObject(base.gameObject, list4);
		UIPlayAnimation uIPlayAnimation = GameObjectUtil.FindChildGameObjectComponent<UIPlayAnimation>(base.gameObject, "blinder");
		UIButtonMessage uIButtonMessage = GameObjectUtil.FindChildGameObjectComponent<UIButtonMessage>(base.gameObject, "blinder");
		if (uIPlayAnimation != null && uIButtonMessage != null)
		{
			uIPlayAnimation.enabled = false;
			uIButtonMessage.enabled = false;
		}
		this.m_isSetObject = true;
	}

	private void Setup(SpecialStageInfo info)
	{
		this.mainPanel.alpha = 1f;
		if (info != null && info.GetRewardChao() != null)
		{
			this.m_targetChao = info.GetRewardChao().id;
		}
		else
		{
			this.m_targetChao = -1;
		}
		if (info != null)
		{
			this.SetupObject(info);
		}
		this.m_mode = EventRewardWindow.Mode.Wait;
	}

	private void Setup(RaidBossInfo info)
	{
		this.mainPanel.alpha = 1f;
		if (info != null && info.GetRewardChao() != null)
		{
			this.m_targetChao = info.GetRewardChao().id;
		}
		else
		{
			this.m_targetChao = -1;
		}
		if (info != null)
		{
			this.SetupObject(info);
		}
		this.m_mode = EventRewardWindow.Mode.Wait;
	}

	private void Setup(EtcEventInfo info)
	{
		this.mainPanel.alpha = 1f;
		if (info != null && info.GetRewardChao() != null)
		{
			this.m_targetChao = info.GetRewardChao().id;
		}
		else
		{
			this.m_targetChao = -1;
		}
		if (info != null)
		{
			this.SetupObject(info);
		}
		this.m_mode = EventRewardWindow.Mode.Wait;
	}

	private void SetupObject(EventBaseInfo info)
	{
		this.m_isGetEffectEnd = true;
		this.m_afterTime = 0.3f;
		this.SetObject();
		if (this.m_getSoundDelay != null)
		{
			this.m_getSoundDelay.Clear();
		}
		this.m_mode = EventRewardWindow.Mode.Idle;
		this.m_close = false;
		this.m_btnAct = EventRewardWindow.BUTTON_ACT.NONE;
		base.enabledAnchorObjects = true;
		if (this.animationData != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.animationData, "ui_event_rewordlist_window_Anim", Direction.Forward);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
			SoundManager.SePlay("sys_window_open", "SE");
		}
		UIDraggablePanel uIDraggablePanel = null;
		if (this.m_objects != null && this.m_objects.ContainsKey("list"))
		{
			uIDraggablePanel = this.m_objects["list"].GetComponent<UIDraggablePanel>();
		}
		if (info != null)
		{
			if (this.m_objectLabels != null && this.m_objectLabels.Count > 0)
			{
				if (this.m_objectLabels.ContainsKey("Lbl_caption") && this.m_objectLabels["Lbl_caption"] != null)
				{
					this.m_objectLabels["Lbl_caption"].text = info.caption;
				}
				if (this.m_objectLabels.ContainsKey("Lbl_word_left_title") && this.m_objectLabels["Lbl_word_left_title"] != null)
				{
					this.m_objectLabels["Lbl_word_left_title"].text = info.leftTitle;
				}
				if (this.m_objectLabels.ContainsKey("Lbl_word_left_title_sh") && this.m_objectLabels["Lbl_word_left_title_sh"] != null)
				{
					this.m_objectLabels["Lbl_word_left_title_sh"].text = info.leftTitle;
				}
				if (this.m_objectLabels.ContainsKey("Lbl_chao_name") && this.m_objectLabels["Lbl_chao_name"] != null)
				{
					this.m_objectLabels["Lbl_chao_name"].text = info.leftName;
				}
				if (this.m_objectLabels.ContainsKey("ui_Lbl_word_object_total_main") && this.m_objectLabels["ui_Lbl_word_object_total_main"] != null)
				{
					this.m_objectLabels["ui_Lbl_word_object_total_main"].text = info.rightTitle;
				}
				if (this.m_objectLabels.ContainsKey("Lbl_object_total_main_num") && this.m_objectLabels["Lbl_object_total_main_num"] != null)
				{
					this.m_objectLabels["Lbl_object_total_main_num"].text = HudUtility.GetFormatNumString<long>(info.totalPoint);
				}
			}
			if (this.m_objectSprites != null && this.m_objectSprites.Count > 0)
			{
				if (this.m_objectSprites.ContainsKey("img_chao_bg"))
				{
					this.m_objectSprites["img_chao_bg"].spriteName = info.leftBg;
				}
				if (this.m_objectSprites.ContainsKey("img_object_total_main"))
				{
					this.m_objectSprites["img_object_total_main"].spriteName = info.rightTitleIcon;
				}
				if (this.m_objectSprites.ContainsKey("img_type_icon") && !string.IsNullOrEmpty(info.chaoTypeIcon))
				{
					this.m_objectSprites["img_type_icon"].spriteName = info.chaoTypeIcon;
				}
			}
			if (this.m_objectTextures != null && this.m_objectTextures.ContainsKey("texture_chao"))
			{
				ChaoData rewardChao = info.GetRewardChao();
				ChaoTextureManager.CallbackInfo info2 = new ChaoTextureManager.CallbackInfo(this.m_objectTextures["texture_chao"], null, true);
				ChaoTextureManager.Instance.GetTexture(rewardChao.id, info2);
			}
			if (uIDraggablePanel != null)
			{
				List<EventMission> eventMission = info.eventMission;
				if (eventMission != null && eventMission.Count > 0)
				{
					int count = eventMission.Count;
					int num = count;
					if (this.m_listObject == null)
					{
						this.m_rewardTime = 0f;
						this.m_attainment = 0;
						this.m_currentAttainment = 0;
						if (this.m_first)
						{
							this.m_isGetEffectEnd = false;
							this.m_afterTime = 0f;
						}
						this.m_listObject = new List<GameObject>();
						for (int i = 0; i < count; i++)
						{
							GameObject gameObject = UnityEngine.Object.Instantiate(this.orgEventScroll, Vector3.zero, Quaternion.identity) as GameObject;
							gameObject.gameObject.transform.parent = uIDraggablePanel.gameObject.transform;
							gameObject.gameObject.transform.localPosition = new Vector3(0f, (float)(-100 * i), 0f);
							gameObject.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
							UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject.gameObject, "Lbl_itemname");
							UILabel uILabel2 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject.gameObject, "Lbl_itemname_sh");
							UILabel uILabel3 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject.gameObject, "Lbl_word_event_object_total");
							UILabel uILabel4 = GameObjectUtil.FindChildGameObjectComponent<UILabel>(gameObject.gameObject, "Lbl_event_object_total_num");
							GameObject gameObject2 = GameObjectUtil.FindChildGameObject(gameObject.gameObject, "get_icon_Anim");
							UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject.gameObject, "img_item_0");
							UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject.gameObject, "texture_chao_0");
							UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(gameObject.gameObject, "texture_chao_1");
							UISprite uISprite3 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject.gameObject, "img_event_object_icon");
							if (uILabel3 != null && uILabel4 != null)
							{
								uILabel3.text = eventMission[i].name;
								uILabel4.text = HudUtility.GetFormatNumString<long>(eventMission[i].point);
							}
							if (gameObject2 != null)
							{
								gameObject2.SetActive(false);
								if (eventMission[i].IsAttainment(info.totalPoint))
								{
									this.m_attainment = i + 1;
								}
							}
							if (uISprite3 != null)
							{
								uISprite3.spriteName = info.rightTitleIcon;
							}
							if (uILabel != null && uILabel2 != null && uISprite != null && uISprite2 != null && uITexture != null)
							{
								ServerItem serverItem = new ServerItem((ServerItem.Id)eventMission[i].reward);
								string text = serverItem.serverItemName;
								int index = eventMission[i].index;
								if (index > 1)
								{
									text = text + " × " + index;
								}
								uILabel.text = text;
								uILabel2.text = text;
								if (eventMission[i].reward >= 400000 && eventMission[i].reward < 500000)
								{
									uISprite2.alpha = 0f;
									uISprite.alpha = 0f;
									uITexture.alpha = 0f;
									uITexture.gameObject.SetActive(true);
									ChaoTextureManager.CallbackInfo info3 = new ChaoTextureManager.CallbackInfo(uITexture, null, true);
									ChaoTextureManager.Instance.GetTexture(eventMission[i].reward - 400000, info3);
									uITexture.alpha = 1f;
									ChaoData chaoData = ChaoTable.GetChaoData(serverItem.chaoId);
									if (chaoData != null)
									{
										uILabel.text = chaoData.name;
										uILabel2.text = chaoData.name;
									}
								}
								else
								{
									uISprite.spriteName = PresentBoxUtility.GetItemSpriteName(serverItem);
									uISprite.alpha = 1f;
									uISprite2.alpha = 0f;
									uITexture.alpha = 0f;
									uITexture.gameObject.SetActive(false);
								}
							}
							this.m_listObject.Add(gameObject);
						}
						uIDraggablePanel.ResetPosition();
						if (this.m_attainment > 0)
						{
							for (int j = 0; j < this.m_attainment; j++)
							{
								Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(this.m_listObject[j], "get_icon_Anim");
								if (animation != null)
								{
									if (j < this.m_attainment - 5 || !this.m_first)
									{
										this.m_currentAttainment = j + 1;
										animation.enabled = false;
										animation.gameObject.SetActive(true);
									}
									else
									{
										animation.enabled = true;
									}
								}
							}
							this.m_initPoint = 0f;
							if (this.m_attainment > 3)
							{
								this.m_initPoint = 1f / (float)(num - 4) * (float)(this.m_attainment - 3);
							}
							base.StartCoroutine(this.ScrollInit(this.m_initPoint, uIDraggablePanel));
						}
					}
					else
					{
						uIDraggablePanel.ResetPosition();
						this.m_currentAttainment = this.m_attainment;
						this.m_isGetEffectEnd = true;
						this.m_afterTime = 0.3f;
						base.StartCoroutine(this.ScrollInit(this.m_initPoint, uIDraggablePanel));
					}
				}
			}
		}
		if (this.m_attainment <= 0 || this.m_currentAttainment >= this.m_attainment)
		{
			this.m_isGetEffectEnd = true;
			this.m_afterTime = 0.3f;
		}
		else
		{
			this.m_rewardTime = 0.75f;
		}
		base.enabled = true;
		this.m_first = false;
	}

	private void Update()
	{
		if (!this.m_isGetEffectEnd)
		{
			if (this.m_rewardTime <= 0f)
			{
				if (this.m_listObject != null && this.m_currentAttainment >= 0 && this.m_currentAttainment < this.m_listObject.Count)
				{
					GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_listObject[this.m_currentAttainment].gameObject, "get_icon_Anim");
					if (gameObject != null)
					{
						if (!gameObject.activeSelf)
						{
							if (this.m_getSoundDelay == null)
							{
								this.m_getSoundDelay = new List<float>();
							}
							this.m_getSoundDelay.Add(0.25f);
						}
						gameObject.SetActive(true);
					}
					this.m_currentAttainment++;
					this.m_rewardTime = 0.7f;
					if (this.m_attainment <= this.m_currentAttainment)
					{
						this.m_isGetEffectEnd = true;
						this.m_rewardTime = 0f;
					}
				}
				else
				{
					this.m_isGetEffectEnd = true;
					this.m_rewardTime = 0f;
				}
			}
			this.m_rewardTime -= Time.deltaTime;
		}
		else
		{
			this.m_afterTime += Time.deltaTime;
			this.m_rewardTime = 0f;
			if (this.m_afterTime > 3f)
			{
				base.enabled = false;
			}
		}
		if (this.m_getSoundDelay != null && this.m_getSoundDelay.Count > 0)
		{
			float deltaTime = Time.deltaTime;
			int num = -1;
			for (int i = 0; i < this.m_getSoundDelay.Count; i++)
			{
				List<float> getSoundDelay;
				List<float> expr_17C = getSoundDelay = this.m_getSoundDelay;
				int index;
				int expr_180 = index = i;
				float num2 = getSoundDelay[index];
				expr_17C[expr_180] = num2 - deltaTime;
				if (this.m_getSoundDelay[i] <= 0f)
				{
					num = i;
				}
			}
			if (num >= 0)
			{
				SoundManager.SePlay("sys_result_decide", "SE");
				this.m_getSoundDelay.RemoveAt(num);
			}
		}
	}

	private void OnSetChaoTexture(ChaoTextureManager.TextureData data)
	{
		if (data != null && data.tex != null && this.m_objectTextures != null && this.m_objectTextures.ContainsKey("texture_chao"))
		{
			this.m_objectTextures["texture_chao"].mainTexture = data.tex;
			this.m_objectTextures["texture_chao"].alpha = 1f;
		}
	}

	private IEnumerator ScrollInit(float point, UIDraggablePanel list)
	{
		EventRewardWindow._ScrollInit_c__Iterator12 _ScrollInit_c__Iterator = new EventRewardWindow._ScrollInit_c__Iterator12();
		_ScrollInit_c__Iterator.point = point;
		_ScrollInit_c__Iterator.list = list;
		_ScrollInit_c__Iterator.___point = point;
		_ScrollInit_c__Iterator.___list = list;
		return _ScrollInit_c__Iterator;
	}

	public bool IsEnd()
	{
		return this.m_mode != EventRewardWindow.Mode.Wait;
	}

	public void OnClickNoButton()
	{
		this.m_btnAct = EventRewardWindow.BUTTON_ACT.CLOSE;
		this.m_close = true;
		SoundManager.SePlay("sys_window_close", "SE");
		if (this.animationData != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.animationData, "ui_event_rewordlist_window_Anim", Direction.Reverse);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
		}
	}

	public void OnClickNoBgButton()
	{
		this.OnClickNoButton();
	}

	public void OnClickTarget()
	{
		if (this.m_targetChao >= 0)
		{
			SoundManager.SePlay("sys_menu_decide", "SE");
			ChaoSetWindowUI window = ChaoSetWindowUI.GetWindow();
			if (window != null)
			{
				ChaoData chaoData = ChaoTable.GetChaoData(this.m_targetChao);
				if (chaoData != null)
				{
					ChaoSetWindowUI.ChaoInfo chaoInfo = new ChaoSetWindowUI.ChaoInfo(chaoData);
					chaoInfo.level = ChaoTable.ChaoMaxLevel();
					chaoInfo.detail = chaoData.GetDetailsLevel(chaoInfo.level);
					if (chaoInfo.level == ChaoTable.ChaoMaxLevel())
					{
						chaoInfo.detail = chaoInfo.detail + "\n" + TextUtility.GetChaoText("Chao", "level_max");
					}
					window.OpenWindow(chaoInfo, ChaoSetWindowUI.WindowType.WINDOW_ONLY);
				}
			}
		}
	}

	public void OnClickRouletteButton()
	{
		this.m_btnAct = EventRewardWindow.BUTTON_ACT.ROULETTE;
		this.m_close = true;
		SoundManager.SePlay("sys_menu_decide", "SE");
		HudMenuUtility.SendChaoRouletteButtonClicked();
		if (this.animationData != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(this.animationData, "ui_event_rewordlist_window_Anim", Direction.Reverse);
			EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.WindowAnimationFinishCallback), true);
		}
	}

	private void WindowAnimationFinishCallback()
	{
		if (this.m_close)
		{
			this.m_mode = EventRewardWindow.Mode.End;
			if (this.m_getSoundDelay != null)
			{
				this.m_getSoundDelay.Clear();
			}
			EventRewardWindow.BUTTON_ACT btnAct = this.m_btnAct;
			if (btnAct != EventRewardWindow.BUTTON_ACT.CLOSE)
			{
				if (btnAct != EventRewardWindow.BUTTON_ACT.ROULETTE)
				{
					base.enabledAnchorObjects = false;
				}
				else
				{
					base.enabledAnchorObjects = false;
				}
			}
			else
			{
				base.enabledAnchorObjects = false;
			}
		}
	}

	public void OnClickPlatformBackButton(WindowBase.BackButtonMessage msg)
	{
		if (!base.enabledAnchorObjects || this.m_btnAct != EventRewardWindow.BUTTON_ACT.NONE)
		{
			return;
		}
		if (msg != null)
		{
			msg.StaySequence();
		}
		if (this.m_isGetEffectEnd && this.m_afterTime > 0.5f)
		{
			base.SendMessage("OnClickNoButton");
		}
	}

	public static EventRewardWindow Create(SpecialStageInfo info)
	{
		if (EventRewardWindow.s_instance != null)
		{
			EventRewardWindow.s_instance.gameObject.SetActive(true);
			EventRewardWindow.s_instance.Setup(info);
			return EventRewardWindow.s_instance;
		}
		return null;
	}

	public static EventRewardWindow Create(RaidBossInfo info)
	{
		if (EventRewardWindow.s_instance != null)
		{
			EventRewardWindow.s_instance.gameObject.SetActive(true);
			EventRewardWindow.s_instance.Setup(info);
			return EventRewardWindow.s_instance;
		}
		return null;
	}

	public static EventRewardWindow Create(EtcEventInfo info)
	{
		if (EventRewardWindow.s_instance != null)
		{
			EventRewardWindow.s_instance.gameObject.SetActive(true);
			EventRewardWindow.s_instance.Setup(info);
			return EventRewardWindow.s_instance;
		}
		return null;
	}

	public void EntryBackKeyCallBack()
	{
		BackKeyManager.AddWindowCallBack(base.gameObject);
	}

	public void RemoveBackKeyCallBack()
	{
		BackKeyManager.RemoveWindowCallBack(base.gameObject);
	}

	private void Awake()
	{
		this.SetInstance();
		this.EntryBackKeyCallBack();
		base.enabledAnchorObjects = false;
	}

	private void OnDestroy()
	{
		if (EventRewardWindow.s_instance == this)
		{
			this.RemoveBackKeyCallBack();
			EventRewardWindow.s_instance = null;
		}
	}

	private void SetInstance()
	{
		if (EventRewardWindow.s_instance == null)
		{
			EventRewardWindow.s_instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
