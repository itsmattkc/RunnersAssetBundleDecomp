using Message;
using System;
using Text;
using UnityEngine;

public class SettingUserName : SettingBase
{
	protected enum EventSignal
	{
		PLAY_START = 100
	}

	private readonly int MaxInputLength = 8;

	private TinyFsmBehavior m_fsm;

	private bool m_isEndConnect;

	private bool m_isEnd;

	private string m_anthorPath = string.Empty;

	private bool m_requestStart;

	private bool m_calcelButtonUseFlag = true;

	private bool m_isPlayStarted;

	private bool m_sendApolloFlag;

	private SendApollo m_sendApollo;

	public void SetCancelButtonUseFlag(bool useFlag)
	{
		this.m_calcelButtonUseFlag = useFlag;
	}

	private void Start()
	{
		SettingPartsUserName settingPartsUserName = base.gameObject.AddComponent<SettingPartsUserName>();
		this.m_fsm = (base.gameObject.AddComponent(typeof(TinyFsmBehavior)) as TinyFsmBehavior);
		if (this.m_fsm != null)
		{
			TinyFsmBehavior.Description description = new TinyFsmBehavior.Description(this);
			description.initState = new TinyFsmState(new EventFunction(this.StateWaitStart));
			description.onFixedUpdate = true;
			this.m_fsm.SetUp(description);
			if (this.m_requestStart)
			{
				TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
				this.m_fsm.Dispatch(signal);
				this.m_requestStart = false;
			}
		}
		this.m_sendApolloFlag = FirstLaunchUserName.IsFirstLaunch;
	}

	private void OnDestroy()
	{
		SettingPartsUserName component = base.gameObject.GetComponent<SettingPartsUserName>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		NGWordCheck.ResetData();
	}

	protected override void OnSetup(string anthorPath)
	{
		this.m_anthorPath = anthorPath;
		SettingPartsUserName component = base.gameObject.GetComponent<SettingPartsUserName>();
		if (component != null)
		{
			component.SetCancelButtonUseFlag(this.m_calcelButtonUseFlag);
			component.Setup(this.m_anthorPath);
		}
	}

	protected override void OnPlayStart()
	{
		base.gameObject.SetActive(true);
		if (this.m_fsm != null)
		{
			if (this.m_isPlayStarted)
			{
				this.m_isEnd = false;
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateUserNameInput)));
			}
			else
			{
				TinyFsmEvent signal = TinyFsmEvent.CreateUserEvent(100);
				this.m_fsm.Dispatch(signal);
				this.m_isPlayStarted = true;
			}
		}
		else
		{
			this.m_isPlayStarted = true;
			this.m_requestStart = true;
		}
	}

	protected override bool OnIsEndPlay()
	{
		return this.m_isEnd;
	}

	protected override void OnUpdate()
	{
	}

	private TinyFsmState StateWaitStart(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			this.m_isEnd = false;
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			if (this.m_sendApolloFlag)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSendApolloStart)));
			}
			else
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateUserNameInput)));
			}
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
	}

	private TinyFsmState StateSendApolloStart(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_sendApollo != null)
			{
				UnityEngine.Object.Destroy(this.m_sendApollo.gameObject);
				this.m_sendApollo = null;
			}
			return TinyFsmState.End();
		case 1:
		{
			string[] value = new string[1];
			SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP2, ref value);
			this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_START, value);
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_sendApollo != null && this.m_sendApollo.IsEnd())
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateUserNameInput)));
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateUserNameInput(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			SettingPartsUserName component = base.gameObject.GetComponent<SettingPartsUserName>();
			if (component != null)
			{
				component.PlayStart();
				NGWordCheck.Load();
			}
			return TinyFsmState.End();
		}
		case 4:
		{
			SettingPartsUserName component2 = base.gameObject.GetComponent<SettingPartsUserName>();
			if (component2 != null && component2.IsEndPlay())
			{
				if (component2.IsDecided)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateCheckError)));
				}
				else if (component2.IsCanceled)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
				}
			}
			return TinyFsmState.End();
		}
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateCheckError(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			return TinyFsmState.End();
		case 4:
			if (NGWordCheck.IsLoaded())
			{
				SettingPartsUserName component = base.gameObject.GetComponent<SettingPartsUserName>();
				string inputText = component.InputText;
				if (inputText.Length > this.MaxInputLength)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateInputErrorOverFlow)));
				}
				else if (inputText.Length == 0)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateInputErrorNoInput)));
				}
				else if (NGWordCheck.Check(inputText, component.TextLabel) != null)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateInputErrorNGWord)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateAskToDecideName)));
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateInputErrorOverFlow(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				caption = TextUtility.GetCommonText("UserName", "input_error"),
				message = TextUtility.GetCommonText("UserName", "input_error_info_1"),
				buttonType = GeneralWindow.ButtonType.Ok
			});
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateUserNameInput)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateInputErrorNoInput(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				caption = TextUtility.GetCommonText("UserName", "input_error"),
				message = TextUtility.GetCommonText("UserName", "input_error_info_2"),
				buttonType = GeneralWindow.ButtonType.Ok
			});
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateUserNameInput)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateInputErrorNGWord(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
			GeneralWindow.Create(new GeneralWindow.CInfo
			{
				caption = TextUtility.GetCommonText("UserName", "input_error"),
				message = TextUtility.GetCommonText("UserName", "input_error_info_ng_word"),
				buttonType = GeneralWindow.ButtonType.Ok
			});
			return TinyFsmState.End();
		case 4:
			if (GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateUserNameInput)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateAskToDecideName(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			SettingPartsUserName component = base.gameObject.GetComponent<SettingPartsUserName>();
			string inputText = component.InputText;
			GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
			info.caption = TextUtility.GetCommonText("UserName", "entry_user");
			string tag = "{NAME}";
			info.message = TextUtility.GetCommonText("UserName", "entry_user_info", tag, inputText);
			info.buttonType = GeneralWindow.ButtonType.YesNo;
			GeneralWindow.Create(info);
			return TinyFsmState.End();
		}
		case 4:
			if (GeneralWindow.IsYesButtonPressed)
			{
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateRegisterUser)));
			}
			else if (GeneralWindow.IsNoButtonPressed)
			{
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateUserNameInput)));
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateRegisterUser(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			this.m_isEndConnect = false;
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				SettingPartsUserName component = base.gameObject.GetComponent<SettingPartsUserName>();
				string inputText = component.InputText;
				if (ServerInterface.SettingState.m_userName != inputText)
				{
					loggedInServerInterface.RequestServerSetUserName(inputText, base.gameObject);
				}
				else
				{
					this.ServerSetUserName_Succeeded(null);
				}
			}
			else
			{
				this.ServerSetUserName_Succeeded(null);
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_isEndConnect)
			{
				if (this.m_sendApolloFlag)
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateSendApolloEnd)));
				}
				else
				{
					this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateFinishedRegisterUser)));
				}
			}
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateSendApolloEnd(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			if (this.m_sendApollo != null)
			{
				UnityEngine.Object.Destroy(this.m_sendApollo.gameObject);
				this.m_sendApollo = null;
			}
			return TinyFsmState.End();
		case 1:
		{
			string[] value = new string[1];
			SendApollo.GetTutorialValue(ApolloTutorialIndex.START_STEP2, ref value);
			this.m_sendApollo = SendApollo.CreateRequest(ApolloType.TUTORIAL_END, value);
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_sendApollo != null && this.m_sendApollo.IsEnd())
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateFinishedRegisterUser)));
			}
			return TinyFsmState.End();
		case 5:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private TinyFsmState StateFinishedRegisterUser(TinyFsmEvent e)
	{
		int signal = e.Signal;
		switch (signal + 4)
		{
		case 0:
			return TinyFsmState.End();
		case 1:
		{
			SettingPartsUserName component = base.gameObject.GetComponent<SettingPartsUserName>();
			string inputText = component.InputText;
			GeneralWindow.CInfo info = default(GeneralWindow.CInfo);
			info.caption = TextUtility.GetCommonText("UserName", "end_entry_user");
			string tag = "{NAME}";
			info.message = TextUtility.GetCommonText("UserName", "end_entry_user_info", tag, inputText);
			info.buttonType = GeneralWindow.ButtonType.Ok;
			GeneralWindow.Create(info);
			ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
			if (loggedInServerInterface != null)
			{
				ServerInterface.SettingState.m_userName = inputText;
			}
			HudMenuUtility.SendMsgUpdateSaveDataDisplay();
			return TinyFsmState.End();
		}
		case 4:
			if (GeneralWindow.IsOkButtonPressed)
			{
				GeneralWindow.Close();
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
			}
			return TinyFsmState.End();
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
			this.m_isEnd = true;
			base.gameObject.SetActive(false);
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private void ServerSetUserName_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		if (msg != null)
		{
			HudMenuUtility.SetUpdateRankingFlag();
		}
		this.m_isEndConnect = true;
	}
}
