using Message;
using SaveData;
using System;
using System.Text.RegularExpressions;
using Text;
using UnityEngine;

public class SettingTakeoverPassword : SettingBase
{
	protected enum EventSignal
	{
		PLAY_START = 100
	}

	private readonly int MaxInputLength = 10;

	private readonly int MinInputLength = 6;

	private TinyFsmBehavior m_fsm;

	private bool m_isEndConnect;

	private bool m_isEnd;

	private bool m_isCancelEnd;

	private string m_anthorPath = string.Empty;

	private bool m_requestStart;

	private bool m_calcelButtonUseFlag = true;

	private bool m_isPlayStarted;

	public bool isCancel
	{
		get
		{
			return this.m_isCancelEnd;
		}
	}

	public void SetCancelButtonUseFlag(bool useFlag)
	{
		this.m_calcelButtonUseFlag = useFlag;
	}

	private void Start()
	{
		SettingPartsTakeoverPassword settingPartsTakeoverPassword = base.gameObject.AddComponent<SettingPartsTakeoverPassword>();
		settingPartsTakeoverPassword.SetCancelButtonUseFlag(this.m_calcelButtonUseFlag);
		settingPartsTakeoverPassword.Setup(this.m_anthorPath);
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
	}

	private void OnDestroy()
	{
		SettingPartsTakeoverPassword component = base.gameObject.GetComponent<SettingPartsTakeoverPassword>();
		if (component != null)
		{
			component.SetOkButtonEnabled(true);
			UnityEngine.Object.Destroy(component);
		}
	}

	protected override void OnSetup(string anthorPath)
	{
		this.m_anthorPath = anthorPath;
	}

	protected override void OnPlayStart()
	{
		if (this.m_fsm != null)
		{
			if (this.m_isPlayStarted)
			{
				this.m_isEnd = false;
				this.m_isCancelEnd = false;
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
			this.m_isCancelEnd = false;
			return TinyFsmState.End();
		case 2:
		case 3:
			IL_23:
			if (signal != 100)
			{
				return TinyFsmState.End();
			}
			this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateUserNameInput)));
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		goto IL_23;
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
			SettingPartsTakeoverPassword component = base.gameObject.GetComponent<SettingPartsTakeoverPassword>();
			if (component != null)
			{
				component.PlayStart();
			}
			return TinyFsmState.End();
		}
		case 4:
		{
			SettingPartsTakeoverPassword component2 = base.gameObject.GetComponent<SettingPartsTakeoverPassword>();
			if (component2 != null)
			{
				if (component2.IsEndPlay())
				{
					if (component2.IsDecided)
					{
						this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateCheckError)));
					}
					else if (component2.IsCanceled)
					{
						this.m_isCancelEnd = true;
						this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateEnd)));
					}
				}
				else
				{
					bool okButtonEnabled = this.CheckPassword(component2.InputText);
					component2.SetOkButtonEnabled(okButtonEnabled);
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
		{
			SettingPartsTakeoverPassword component = base.gameObject.GetComponent<SettingPartsTakeoverPassword>();
			string inputText = component.InputText;
			if (inputText.Length > this.MaxInputLength)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateInputErrorOverFlow)));
			}
			else if (inputText.Length == 0)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateInputErrorNoInput)));
			}
			else if (inputText.Length < this.MinInputLength)
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateInputErrorOverFlow)));
			}
			else
			{
				this.m_fsm.ChangeState(new TinyFsmState(new EventFunction(this.StateRegisterPassword)));
			}
			return TinyFsmState.End();
		}
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
				caption = TextUtility.GetCommonText("Option", "take_over_password_input_error"),
				message = TextUtility.GetCommonText("Option", "take_over_password_input_error_info"),
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
				caption = TextUtility.GetCommonText("Option", "take_over_password_input_error"),
				message = TextUtility.GetCommonText("Option", "take_over_password_input_error_info"),
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

	private TinyFsmState StateRegisterPassword(TinyFsmEvent e)
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
				SettingPartsTakeoverPassword component = base.gameObject.GetComponent<SettingPartsTakeoverPassword>();
				string inputText = component.InputText;
				string userPassword = NetUtil.CalcMD5String(inputText);
				loggedInServerInterface.RequestServerGetMigrationPassword(userPassword, base.gameObject);
				SystemSaveManager.SetTakeoverPassword(inputText);
			}
			else
			{
				this.ServerGetMigrationPassword_Succeeded(null);
			}
			return TinyFsmState.End();
		}
		case 4:
			if (this.m_isEndConnect)
			{
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
			return TinyFsmState.End();
		case 4:
			return TinyFsmState.End();
		}
		return TinyFsmState.End();
	}

	private bool CheckPassword(string password)
	{
		bool result;
		if (password.Length > this.MaxInputLength)
		{
			result = false;
		}
		else if (password.Length == 0)
		{
			result = false;
		}
		else if (password.Length < this.MinInputLength)
		{
			result = false;
		}
		else
		{
			result = false;
			string text = "[a-zA-Z0-9]";
			for (int i = 1; i < password.Length; i++)
			{
				text += "[a-zA-Z0-9]";
			}
			if (Regex.IsMatch(password, text))
			{
				result = true;
			}
		}
		return result;
	}

	private void ServerGetMigrationPassword_Succeeded(MsgGetMigrationPasswordSucceed msg)
	{
		this.m_isEndConnect = true;
		if (msg != null)
		{
			SystemSaveManager.SetTakeoverID(msg.m_migrationPassword);
			SystemSaveManager.Instance.SaveSystemData();
		}
	}
}
