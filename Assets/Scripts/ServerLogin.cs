using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerLogin
{
	private sealed class _Process_c__Iterator99 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal string userId;

		internal string password;

		internal NetServerLogin _net___0;

		internal NetMonitor _monitor___1;

		internal GameObject callbackObject;

		internal ServerLoginState _loginState___2;

		internal ServerSettingState _settingState___3;

		internal MsgLoginSucceed _msg___4;

		internal MsgServerPasswordError _msg___5;

		internal MsgServerConnctFailed _msg___6;

		internal int _PC;

		internal object _current;

		internal string ___userId;

		internal string ___password;

		internal GameObject ___callbackObject;

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
				this._net___0 = new NetServerLogin(this.userId, this.password);
				this._net___0.Request();
				this._monitor___1 = NetMonitor.Instance;
				if (this._monitor___1 != null)
				{
					this._monitor___1.StartMonitor(new ServerLoginRetry(this.userId, this.password, this.callbackObject));
				}
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._net___0.IsExecuting())
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this._net___0.IsSucceeded())
			{
				this._loginState___2 = ServerInterface.LoginState;
				this._loginState___2.m_lineId = this._net___0.paramUserId;
				this._loginState___2.m_altLineId = string.Empty;
				this._loginState___2.m_lineAuthToken = string.Empty;
				this._loginState___2.sessionId = this._net___0.resultSessionId;
				this._loginState___2.sessionTimeLimit = this._net___0.sessionTimeLimit;
				this._settingState___3 = ServerInterface.SettingState;
				this._settingState___3.m_energyRefreshTime = this._net___0.resultEnergyRefreshTime;
				this._settingState___3.m_energyRecoveryMax = this._net___0.energyRecoveryMax;
				this._settingState___3.m_invitBaseIncentive = this._net___0.resultInviteBaseIncentive;
				this._settingState___3.m_rentalBaseIncentive = this._net___0.resultRentalBaseIncentive;
				this._settingState___3.m_userName = this._net___0.userName;
				this._settingState___3.m_userId = this._net___0.userId;
				this._msg___4 = new MsgLoginSucceed();
				this._msg___4.m_userId = this._net___0.userId;
				this._msg___4.m_password = this._net___0.password;
				if (this._monitor___1 != null)
				{
					this._monitor___1.EndMonitorForward(this._msg___4, this.callbackObject, "ServerLogin_Succeeded");
				}
				ServerLogin.m_passwordErrorCount = 0;
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerLogin_Succeeded", this._msg___4, SendMessageOptions.DontRequireReceiver);
				}
			}
			else if (this._net___0.resultStCd == ServerInterface.StatusCode.PassWordError && ServerLogin.m_passwordErrorCount < ServerLogin.PasswordErrorCountMax)
			{
				ServerLogin.m_passwordErrorCount++;
				this._msg___5 = new MsgServerPasswordError();
				this._msg___5.m_key = this._net___0.key;
				this._msg___5.m_userId = this._net___0.userId;
				this._msg___5.m_password = this._net___0.password;
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerLogin_Failed", this._msg___5, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				ServerLogin.m_passwordErrorCount = 0;
				this._msg___6 = new MsgServerConnctFailed(this._net___0.resultStCd);
				if (this._monitor___1 != null)
				{
					this._monitor___1.EndMonitorForward(this._msg___6, this.callbackObject, "ServerLogin_Failed");
				}
			}
			if (this._monitor___1 != null)
			{
				this._monitor___1.EndMonitorBackward();
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

	private static int m_passwordErrorCount;

	private static readonly int PasswordErrorCountMax = 3;

	public static IEnumerator Process(string userId, string password, GameObject callbackObject)
	{
		ServerLogin._Process_c__Iterator99 _Process_c__Iterator = new ServerLogin._Process_c__Iterator99();
		_Process_c__Iterator.userId = userId;
		_Process_c__Iterator.password = password;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___userId = userId;
		_Process_c__Iterator.___password = password;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
