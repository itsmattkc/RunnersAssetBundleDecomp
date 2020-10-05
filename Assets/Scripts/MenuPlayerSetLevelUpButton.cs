using Message;
using SaveData;
using System;
using Text;
using UnityEngine;

public class MenuPlayerSetLevelUpButton : MonoBehaviour
{
	private enum EventSignal
	{
		BUTTON_PRESSED = 100,
		LEVEL_UP_END
	}

	public delegate void LevelUpCallback(AbilityType abilityType);

	private TinyFsmBehavior m_fsm;

	private CharaType m_charaType;

	private GameObject m_pageRootObject;

	private bool m_is_end_connect;

	private MenuPlayerSetLevelUpButton.LevelUpCallback m_callback;

	private GameObject m_saleIcon;

	private AbilityType m_currentLevelUpAbility;

	private void OnEnable()
	{
	}

	public void Setup(CharaType charaType, GameObject pageRootObject)
	{
		this.m_charaType = charaType;
		this.m_pageRootObject = pageRootObject;
		GameObject gameObject = GameObjectUtil.FindChildGameObject(this.m_pageRootObject, "Btn_lv_up");
		if (gameObject != null)
		{
			UIButtonMessage uIButtonMessage = gameObject.GetComponent<UIButtonMessage>();
			if (uIButtonMessage == null)
			{
				uIButtonMessage = gameObject.AddComponent<UIButtonMessage>();
			}
			uIButtonMessage.target = base.gameObject;
			uIButtonMessage.functionName = "LevelUpButtonClickedCallback";
			this.m_saleIcon = GameObjectUtil.FindChildGameObject(gameObject, "img_icon_sale");
		}
		this.InitCostLabel();
	}

	public void InitCostLabel()
	{
		ServerItem serverItem = new ServerItem(this.m_charaType);
		int id = (int)serverItem.id;
		ServerCampaignData campaignInSession = ServerInterface.CampaignState.GetCampaignInSession(Constants.Campaign.emType.CharacterUpgradeCost, id);
		if (campaignInSession != null)
		{
			if (this.m_saleIcon != null)
			{
				this.m_saleIcon.SetActive(true);
			}
		}
		else if (this.m_saleIcon != null)
		{
			this.m_saleIcon.SetActive(false);
		}
		UILabel uILabel = GameObjectUtil.FindChildGameObjectComponent<UILabel>(base.gameObject, "Lbl_price_number");
		if (uILabel != null)
		{
			int abilityCost = MenuPlayerSetUtil.GetAbilityCost(this.m_charaType);
			int num = abilityCost - MenuPlayerSetUtil.GetCurrentExp(this.m_charaType);
			num = Mathf.Max(0, num);
			uILabel.text = HudUtility.GetFormatNumString<int>(num);
		}
		UISlider uISlider = GameObjectUtil.FindChildGameObjectComponent<UISlider>(base.gameObject, "Pgb_exp");
		if (uISlider != null)
		{
			float currentExpRatio = MenuPlayerSetUtil.GetCurrentExpRatio(this.m_charaType);
			uISlider.value = currentExpRatio;
		}
	}

	public void SetCallback(MenuPlayerSetLevelUpButton.LevelUpCallback callback)
	{
		this.m_callback = callback;
	}

	public void OnLevelUpEnd()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(101);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}

	private void Start()
	{
		this.m_fsm = base.gameObject.AddComponent<TinyFsmBehavior>();
		TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
		description.initState = new TinyFsmState(new EventFunction(this.StateWaitLevelUpButtonPressed));
		description.onFixedUpdate = true;
		this.m_fsm.SetUp(description);
	}

	private void Update()
	{
		this.InitCostLabel();
	}

	private TinyFsmState StateWaitLevelUpButtonPressed(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_is_end_connect = false;
			if (MenuPlayerSetUtil.IsCharacterLevelMax(this.m_charaType))
			{
				UIImageButton uIImageButton = GameObjectUtil.FindChildGameObjectComponent<UIImageButton>(this.m_pageRootObject, "Btn_lv_up");
				if (uIImageButton != null)
				{
					uIImageButton.isEnabled = false;
				}
			}
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			this.m_currentLevelUpAbility = MenuPlayerSetUtil.GetNextLevelUpAbility(this.m_charaType);
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateAskLevelUp)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateAskLevelUp(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
		{
			GeneralWindow.Close();
			SaveDataManager instance = SaveDataManager.Instance;
			int abilityCost = MenuPlayerSetUtil.GetAbilityCost(this.m_charaType);
			int num = abilityCost - MenuPlayerSetUtil.GetCurrentExp(this.m_charaType);
			num = Mathf.Max(0, num);
			int num2 = num;
			bool flag = true;
			if (instance.ItemData.RingCount - (uint)num2 < 0u)
			{
				flag = false;
			}
			if (flag)
			{
				BackKeyManager.InvalidFlag = true;
				if (ServerInterface.LoggedInServerInterface != null)
				{
					ServerInterface component = GameObject.Find("ServerInterface").GetComponent<ServerInterface>();
					ServerPlayerState playerState = ServerInterface.PlayerState;
					if (playerState != null)
					{
						ServerCharacterState serverCharacterState = playerState.CharacterState(this.m_charaType);
						if (serverCharacterState != null)
						{
							int abilityId = MenuPlayerSetUtil.TransformServerAbilityID(this.m_currentLevelUpAbility);
							component.RequestServerUpgradeCharacter(serverCharacterState.Id, abilityId, base.gameObject);
						}
					}
				}
				else
				{
					CharaData charaData = instance.CharaData;
					CharaAbility charaAbility = charaData.AbilityArray[(int)this.m_charaType];
					charaAbility.Ability[(int)this.m_currentLevelUpAbility] += 1u;
					this.ServerUpgradeCharacter_Succeeded(null);
				}
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitServerConnectEnd)));
			}
			else
			{
				GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
				string text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "MainMenu", "insufficient_ring").text;
				string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "ItemSet", "gw_buy_ring__error_text").text;
				info.caption = text;
				info.message = text2;
				info.anchor_path = "Camera/menu_Anim/PlayerSet_2_UI/Anchor_5_MC";
				info.buttonType = GeneralWindow.ButtonType.Ok;
				info.isPlayErrorSe = true;
				info.finishedCloseDelegate = new GeneralWindow.CInfo.FinishedCloseDelegate(this.GeneralWindowCloseCallback);
				GeneralWindow.Create(info);
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateFailedLevelUp)));
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateWaitServerConnectEnd(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			if (this.m_is_end_connect)
			{
				this.m_callback(this.m_currentLevelUpAbility);
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitLevelUpAnimation)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateFailedLevelUp(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateWaitLevelUpAnimation(TinyFsmEvent e)
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
			IL_23:
			if (signal != 101)
			{
				return TinyFsmState.End();
			}
			BackKeyManager.InvalidFlag = false;
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitLevelUpButtonPressed)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateLevelUpAbilityExplain(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			int level = MenuPlayerSetUtil.GetLevel(this.m_charaType, this.m_currentLevelUpAbility);
			int abilityCost = MenuPlayerSetUtil.GetAbilityCost(this.m_charaType);
			GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
			info.caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "level_up_caption").text;
			TextObject text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "WindowText", "level_up_text");
			text.ReplaceTag("{RING_COST}", abilityCost.ToString());
			string text2 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaStatus", "abilityname" + ((int)(this.m_currentLevelUpAbility + 1)).ToString()).text;
			text.ReplaceTag("{ABILITY_NAME}", text2);
			string text3 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaStatus", "abilitycaption" + ((int)(this.m_currentLevelUpAbility + 1)).ToString()).text;
			text.ReplaceTag("{ABILITY_CAPTION}", text3);
			text.ReplaceTag("{ABILITY_CAPTION2}", text3);
			TextObject text4 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaStatus", "abilitypotential" + ((int)(this.m_currentLevelUpAbility + 1)).ToString());
			TextObject text5 = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "CharaStatus", "abilitypotential" + ((int)(this.m_currentLevelUpAbility + 1)).ToString());
			float levelAbility = MenuPlayerSetUtil.GetLevelAbility(this.m_charaType, this.m_currentLevelUpAbility, level - 1);
			float levelAbility2 = MenuPlayerSetUtil.GetLevelAbility(this.m_charaType, this.m_currentLevelUpAbility, level);
			text4.ReplaceTag("{ABILITY_POTENTIAL}", levelAbility.ToString());
			text5.ReplaceTag("{ABILITY_POTENTIAL}", levelAbility2.ToString());
			text.ReplaceTag("{ABILITY_POTENTIAL}", text4.text);
			text.ReplaceTag("{ABILITY_POTENTIAL2}", text5.text);
			info.message = text.text;
			info.anchor_path = "Camera/menu_Anim/PlayerSet_2_UI/Anchor_5_MC";
			info.buttonType = GeneralWindow.ButtonType.Ok;
			info.finishedCloseDelegate = new GeneralWindow.CInfo.FinishedCloseDelegate(this.GeneralWindowCloseCallback);
			GeneralWindow.Create(info);
			return TinyFsmState.End();
		}
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void LevelUpButtonClickedCallback()
	{
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
		if (HudMenuUtility.IsTutorial_CharaLevelUp())
		{
			GameObjectUtil.SendMessageFindGameObject("PlayerSet_2_UI", "OnClickedLevelUpButton", null, SendMessageOptions.DontRequireReceiver);
		}
	}

	private void ServerUpgradeCharacter_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		this.m_is_end_connect = true;
		SoundManager.SePlay("sys_buy", "SE");
		HudMenuUtility.SendMsgUpdateSaveDataDisplay();
		StageAbilityManager instance = StageAbilityManager.Instance;
		if (instance != null)
		{
			instance.RecalcAbilityVaue();
		}
	}

	public void GeneralWindowCloseCallback()
	{
		global::Debug.Log("GeneralWindowCloseCallback IsOkButtonPressed:" + GeneralWindow.IsOkButtonPressed);
		if (this.m_fsm != null && GeneralWindow.IsOkButtonPressed)
		{
			BackKeyManager.InvalidFlag = false;
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateWaitLevelUpButtonPressed)));
		}
	}
}
