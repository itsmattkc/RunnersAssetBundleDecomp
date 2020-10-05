using Message;
using SaveData;
using System;
using Text;
using UnityEngine;

public class SettingPartsSnsLogin : MonoBehaviour
{
	private enum EventSignal
	{
		PLAY_START = 100,
		SNS_LOGIN_END,
		SNS_LOGIN_FAILED,
		INCENTIVE_CONNECT_END
	}

	private TinyFsmBehavior m_fsm;

	private string m_anchorPath;

	private bool m_isEndPlay;

	private bool m_isCalceled;

	private IncentiveWindowQueue m_windowQueue;

	private SettingPartsSnsAdditional m_snsAdditional;

	private bool m_cancelWindowUseFlag = true;

	private bool m_isPlayStart;

	public bool IsEnd
	{
		get
		{
			return this.m_isEndPlay;
		}
		private set
		{
		}
	}

	public bool IsCalceled
	{
		get
		{
			return this.m_isCalceled;
		}
	}

	public void Setup(string anchorPath)
	{
		this.m_anchorPath = anchorPath;
	}

	public void PlayStart()
	{
		this.m_isEndPlay = false;
		this.m_isCalceled = false;
		UIEffectManager instance = UIEffectManager.Instance;
		if (instance != null)
		{
			instance.SetActiveEffect(HudMenuUtility.EffectPriority.UniqueWindow, false);
		}
		this.m_isPlayStart = true;
		HudMenuUtility.SetConnectAlertSimpleUI(true);
	}

	public bool IsEnableCreateCustomData()
	{
		string gameID = SystemSaveManager.GetGameID();
		return gameID != null && gameID != "0";
	}

	public void SetCancelWindowUseFlag(bool flag)
	{
		this.m_cancelWindowUseFlag = flag;
	}

	private void Start()
	{
		this.m_fsm = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
		description.initState = new TinyFsmState(new EventFunction(this.StateIdle));
		description.onFixedUpdate = true;
		this.m_fsm.SetUp(description);
	}

	private void Update()
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
		case 4:
			if (this.m_isPlayStart)
			{
				SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
				if (socialInterface != null)
				{
					if (socialInterface.IsLoggedIn)
					{
						global::Debug.Log("SettingPartsSnsLoging: Logging in");
						this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
					}
					else
					{
						global::Debug.Log("SettingPartsSnsLoging: Not Logging in");
						this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateAskSnsLogin)));
					}
				}
				return TinyFsmState.End();
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateAskSnsLogin(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			global::Debug.Log("SettingPartsSnsLoging:StateAskSnsLogin");
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FaceBook", "ui_Lbl_facebook_login").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FaceBook", "ui_Lbl_facebook_login_info").text,
				anchor_path = this.m_anchorPath,
				buttonType = GeneralWindow.ButtonType.YesNo,
				name = "FacebookLogin"
			});
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsYesButtonPressed)
			{
				global::Debug.Log("SettingPartsSnsLoging:AskSnsLogin.YesButton");
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSnsLogin)));
			}
			else if (GeneralWindow.IsNoButtonPressed)
			{
				global::Debug.Log("SettingPartsSnsLoging:AskSnsLogin.NoButton");
				GeneralWindow.Close();
				if (this.m_cancelWindowUseFlag)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSnsLoginCanceled)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSnsLoginCanceled(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			global::Debug.Log("SettingPartsSnsLoging:StateSnsLoginCanceled");
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FaceBook", "ui_Lbl_facebook_login_method").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FaceBook", "ui_Lbl_facebook_login_method_info").text,
				anchor_path = this.m_anchorPath,
				buttonType = GeneralWindow.ButtonType.Ok
			});
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
				this.m_isCalceled = true;
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSnsLogin(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			global::Debug.Log("SettingPartsSnsLoging:StateSnsLogin");
			SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
			if (socialInterface != null)
			{
				socialInterface.Login(base.gameObject);
			}
			NetMonitor instance = NetMonitor.Instance;
			if (instance != null)
			{
				instance.StartMonitor(null);
			}
			return TinyFsmState.End();
		}
		case 2:
		case 3:
		{
			IL_25:
			if (signal == 101)
			{
				NetMonitor instance2 = NetMonitor.Instance;
				if (instance2 != null)
				{
					instance2.EndMonitorForward(null, null, null);
					instance2.EndMonitorBackward();
				}
				ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
				if (loggedInServerInterface != null)
				{
					this.m_windowQueue = base.gameObject.AddComponent<IncentiveWindowQueue>();
					loggedInServerInterface.RequestServerGetFacebookIncentive(0, 0, base.gameObject);
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIncentiveConnectWait)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGetSNSInformations)));
				}
				return TinyFsmState.End();
			}
			if (signal != 102)
			{
				return TinyFsmState.End();
			}
			NetMonitor instance3 = NetMonitor.Instance;
			if (instance3 != null)
			{
				instance3.EndMonitorForward(null, null, null);
				instance3.EndMonitorBackward();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSnsLoginFailed)));
			return TinyFsmState.End();
		}
		case 4:
			return TinyFsmState.End();
		}
		goto IL_25;
	}

	private TinyFsmState StateIncentiveConnectWait(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			global::Debug.Log("SettingPartsSnsLoging:StateIncentiveConnectWait");
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 103)
			{
				return TinyFsmState.End();
			}
			global::Debug.Log("SettingPartsSnsLoging:StateIncentiveConnectWait INCENTIVE_CONNECT_END");
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSetupIncentiveQueue)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateSetupIncentiveQueue(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			global::Debug.Log("SettingPartsSnsLoging:StateSetupIncentiveQueue");
			return TinyFsmState.End();
		case 4:
			if (this.m_windowQueue && this.m_windowQueue.SetUpped)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIncentiveDisplaying)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateIncentiveDisplaying(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			global::Debug.Log("SettingPartsSnsLoging:StateIncentiveDisplaying");
			if (this.m_windowQueue != null)
			{
				global::Debug.Log("SettingPartsSnsLoging:StateIncentiveDisplaying  m_windowQueue.PlayStart()");
				this.m_windowQueue.PlayStart();
			}
			return TinyFsmState.End();
		case 4:
			global::Debug.Log("SettingPartsSnsLoging:StateIncentiveDisplaying  UPDATE(1)");
			if (this.m_windowQueue != null)
			{
				global::Debug.Log("SettingPartsSnsLoging:StateIncentiveDisplaying  UPDATE(2)");
				if (this.m_windowQueue.IsEmpty())
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateGetSNSInformations)));
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSnsLoginFailed(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			global::Debug.Log("SettingPartsSnsLoging:StateGetSNSInformations");
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				caption = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FaceBook", "ui_Lbl_network_error").text,
				message = TextManager.GetText(TextManager.TextType.TEXTTYPE_FIXATION_TEXT, "FaceBook", "ui_Lbl_network_error_info").text,
				anchor_path = this.m_anchorPath,
				buttonType = GeneralWindow.ButtonType.Ok
			});
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateAskSnsLogin)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateGetSNSInformations(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			global::Debug.Log("SettingPartsSnsLoging:StateGetSNSInformations");
			HudMenuUtility.SendMsgUpdateSaveDataDisplay();
			SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
			if (socialInterface != null && socialInterface.IsLoggedIn)
			{
				socialInterface.RequestFriendRankingInfoSet(null, null, SettingPartsSnsAdditional.Mode.BACK_GROUND_LOAD);
			}
			return TinyFsmState.End();
		}
		case 4:
		{
			SocialInterface socialInterface2 = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
			if (socialInterface2 != null && socialInterface2.IsLoggedIn)
			{
				if (socialInterface2.IsEnableFriendInfo)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
				}
			}
			else
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateEnd(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			UIEffectManager instance = UIEffectManager.Instance;
			if (instance != null)
			{
				instance.SetActiveEffect(HudMenuUtility.EffectPriority.UniqueWindow, true);
			}
			global::Debug.Log("SettingPartsSnsLoging:StateEnd");
			this.m_isPlayStart = false;
			this.m_isEndPlay = true;
			HudMenuUtility.SetConnectAlertSimpleUI(false);
			return TinyFsmState.End();
		}
		case 4:
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateIdle)));
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void ServerGetFacebookIncentive_Succeeded(MsgGetNormalIncentiveSucceed msg)
	{
		global::Debug.Log("SettingPartsSnsLoging:ServerGetFacebookIncentive_Succeeded ");
		foreach (ServerPresentState current in msg.m_incentive)
		{
			global::Debug.Log("SettingPartsSnsLoging:ServerGetFacebookIncentive_Succeeded m_incentive");
			IncentiveWindow window = new IncentiveWindow(current.m_itemId, current.m_numItem, this.m_anchorPath);
			this.m_windowQueue.AddWindow(window);
		}
		TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(103);
		if (this.m_fsm)
		{
			global::Debug.Log("SettingPartsSnsLoging:ServerGetFacebookIncentive_Succeeded INCENTIVE_CONNECT_END");
			this.m_fsm.Dispatch(signal);
		}
	}

	private void LoginEndCallback(MsgSocialNormalResponse msg)
	{
		SocialInterface socialInterface = GameObjectUtil.FindGameObjectComponent<SocialInterface>("SocialInterface");
		if (socialInterface == null)
		{
			return;
		}
		TinyFsmEvent signal;
		if (socialInterface.IsLoggedIn)
		{
			HudMenuUtility.SetUpdateRankingFlag();
			signal = TinyFsmEvent.CreateUserEvent(101);
		}
		else
		{
			signal = TinyFsmEvent.CreateUserEvent(102);
		}
		if (this.m_fsm != null)
		{
			this.m_fsm.Dispatch(signal);
		}
	}
}
