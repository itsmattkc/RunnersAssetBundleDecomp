using AnimationOrTween;
using Message;
using SaveData;
using System;
using System.Collections.Generic;
using Text;
using UnityEngine;

public class MenuPlayerSetCharaButton : MonoBehaviour
{
	private enum EventSignal
	{
		CHARA_CHANGE = 100
	}

	public delegate void AnimEndCallback();

	private TinyFsmBehavior m_fsm;

	private GameObject m_pageRootObject;

	private CharaType m_charaType;

	private UISprite m_charaIcon;

	private UILabel m_charaName;

	private UILabel m_charaLevel;

	private UISprite m_ribbon;

	[SerializeField]
	private UIObjectContainer m_objectContainer;

	private PlayerSetWindowUIWithButton m_charaChangeWindow;

	private MenuPlayerSetCharaButton.AnimEndCallback m_animEndCallback;

	private int m_currentDeckSetStock;

	private List<GameObject> m_deckObjects;

	private bool m_animEnd = true;

	public bool AnimEnd
	{
		get
		{
			return this.m_animEnd;
		}
		private set
		{
		}
	}

	public void Setup(CharaType charaType, GameObject pageRootObject)
	{
		this.m_charaType = charaType;
		this.m_pageRootObject = pageRootObject;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_pageRootObject, "Btn_player_main");
		if (gameObject != null)
		{
			UIButtonMessage uIButtonMessage = gameObject.AddComponent<UIButtonMessage>();
			uIButtonMessage.target = base.gameObject;
			uIButtonMessage.functionName = "OnSelected";
		}
		UISprite uISprite = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_player_speacies");
		if (uISprite != null)
		{
			uISprite.spriteName = HudUtility.GetCharaAttributeSpriteName(this.m_charaType);
		}
		UISprite uISprite2 = GameObjectUtil.FindChildGameObjectComponent<UISprite>(gameObject, "img_player_genus");
		if (uISprite2 != null)
		{
			uISprite2.spriteName = HudUtility.GetTeamAttributeSpriteName(this.m_charaType);
		}
		this.m_currentDeckSetStock = DeckUtil.GetDeckCurrentStockIndex();
		this.SetupTabView();
	}

	private void SetupTabView()
	{
		GameObject gameObject = GameObjectUtil.FindChildGameObject(base.gameObject, "deck_tab");
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

	public void UpdateRibbon()
	{
		this.m_ribbon = GameObjectUtil.FindChildGameObjectComponent<UISprite>(this.m_pageRootObject, "img_ribbon");
		if (this.m_ribbon != null)
		{
			PlayerData playerData = SaveDataManager.Instance.PlayerData;
			if (this.m_charaType == playerData.MainChara)
			{
				this.m_ribbon.gameObject.SetActive(true);
				this.m_ribbon.spriteName = "ui_mm_player_ribbon_0";
			}
			else if (this.m_charaType == playerData.SubChara)
			{
				this.m_ribbon.gameObject.SetActive(true);
				this.m_ribbon.spriteName = "ui_mm_player_ribbon_1";
			}
			else
			{
				this.m_ribbon.gameObject.SetActive(false);
			}
		}
	}

	private void Start()
	{
		UITexture uITexture = GameObjectUtil.FindChildGameObjectComponent<UITexture>(this.m_pageRootObject, "img_player_tex");
		if (uITexture != null)
		{
			TextureRequestChara request = new TextureRequestChara(this.m_charaType, uITexture);
			TextureAsyncLoadManager.Instance.Request(request);
		}
		this.m_charaName = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_pageRootObject, "Lbl_player_name");
		if (this.m_charaName != null)
		{
			string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaName", CharaName.Name[(int)this.m_charaType]).text;
			this.m_charaName.text = text;
		}
		this.m_charaLevel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(this.m_pageRootObject, "Lbl_player_lv");
		if (this.m_charaLevel != null)
		{
			int totalLevel = MenuPlayerSetUtil.GetTotalLevel(this.m_charaType);
			this.m_charaLevel.text = TextUtility.GetTextLevel(string.Format("{0:000}", totalLevel));
		}
		this.UpdateRibbon();
		this.m_fsm = base.gameObject.AddComponent<TinyFsmBehavior>();
		TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
		description.initState = new TinyFsmState(new EventFunction(this.StateIdle));
		description.onFixedUpdate = true;
		this.m_fsm.SetUp(description);
		this.m_objectContainer = base.gameObject.AddComponent<UIObjectContainer>();
		List<GameObject> list = new List<GameObject>();
		GameObject parent = GameObjectUtil.FindChildGameObject(base.gameObject, "Btn_player_main");
		list.Add(GameObjectUtil.FindChildGameObject(parent, "eff_0"));
		list.Add(GameObjectUtil.FindChildGameObject(parent, "eff_1"));
		list.Add(GameObjectUtil.FindChildGameObject(parent, "img_player_main_sale_word"));
		this.m_objectContainer.Objects = list.ToArray();
	}

	public void LevelUp(MenuPlayerSetCharaButton.AnimEndCallback callback)
	{
		this.m_animEndCallback = callback;
		this.m_animEnd = false;
		int totalLevel = MenuPlayerSetUtil.GetTotalLevel(this.m_charaType);
		this.m_charaLevel.text = TextUtility.GetTextLevel(string.Format("{0:000}", totalLevel));
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "Btn_player_main");
		if (animation != null)
		{
			ActiveAnimation activeAnimation = ActiveAnimation.Play(animation, Direction.Forward);
			if (activeAnimation != null)
			{
				EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.LevelUpAnimEndCallback), true);
			}
			if (this.m_objectContainer != null)
			{
				this.m_objectContainer.SetActive(true);
			}
		}
		AchievementManager.RequestUpdate();
	}

	public void SkipLevelUp()
	{
		Animation animation = GameObjectUtil.FindChildGameObjectComponent<Animation>(base.gameObject, "Btn_player_main");
		if (animation != null)
		{
			foreach (AnimationState animationState in animation)
			{
				if (!(animationState == null))
				{
					animationState.time = animationState.length * 0.99f;
				}
			}
		}
	}

	public void OnSelected()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
		if (HudMenuUtility.IsTutorial_11())
		{
			HudMenuUtility.SendMsgMenuSequenceToMainMenu(MsgMenuSequence.SequeneceType.CHARA_MAIN);
		}
	}

	private void LateUpdate()
	{
	}

	private TinyFsmState StateIdle(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 2:
		case 3:
		{
			IL_25:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			int playableCharaCount = MenuPlayerSetUtil.GetPlayableCharaCount();
			if (playableCharaCount <= 1)
			{
				GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
				info.caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "recommend_chara_unlock_caption").text;
				string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "recommend_chara_unlock_text").text;
				info.message = text;
				info.anchor_path = "Camera/menu_Anim/PlayerSet_2_UI/Anchor_5_MC";
				info.buttonType = GeneralWindow.ButtonType.Ok;
				GeneralWindow.Create(info);
			}
			else
			{
				this.m_charaChangeWindow = PlayerSetWindowUIWithButton.Create(this.m_charaType);
				if (this.m_fsm != null)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateCharaChangeWindow)));
				}
			}
			return TinyFsmState.End();
		}
		case 4:
		{
			int deckCurrentStockIndex = DeckUtil.GetDeckCurrentStockIndex();
			if (this.m_currentDeckSetStock != deckCurrentStockIndex)
			{
				this.m_currentDeckSetStock = deckCurrentStockIndex;
				this.SetupTabView();
			}
			return TinyFsmState.End();
		}
		}
		goto IL_25;
	}

	private TinyFsmState StateRecommendUnlockChara(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsOkButtonPressed && this.m_fsm != null)
			{
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateCharaChangeWindow(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			if (this.m_charaChangeWindow.isClickMain)
			{
				if (this.m_fsm != null)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
				}
				GameObject playerSetRoot = MenuPlayerSetUtil.GetPlayerSetRoot();
				MenuPlayerSetContents component = playerSetRoot.GetComponent<MenuPlayerSetContents>();
				if (component != null)
				{
					component.ChangeMainChara(this.m_charaType);
				}
			}
			if (this.m_charaChangeWindow.isClickSub)
			{
				if (this.m_fsm != null)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
				}
				GameObject playerSetRoot2 = MenuPlayerSetUtil.GetPlayerSetRoot();
				MenuPlayerSetContents component2 = playerSetRoot2.GetComponent<MenuPlayerSetContents>();
				if (component2 != null)
				{
					component2.ChangeSubChara(this.m_charaType);
				}
			}
			if (this.m_charaChangeWindow.isClickCancel && this.m_fsm != null)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void LevelUpAnimEndCallback()
	{
		this.m_animEnd = true;
		if (this.m_objectContainer != null)
		{
			this.m_objectContainer.SetActive(false);
		}
		if (this.m_animEndCallback != null)
		{
			this.m_animEndCallback();
			this.m_animEndCallback = null;
		}
	}
}
