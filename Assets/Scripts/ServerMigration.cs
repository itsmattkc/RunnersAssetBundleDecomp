using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerMigration
{
	private sealed class _Process_c__Iterator9C : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal string migrationId;

		internal string migrationPassword;

		internal NetServerMigration _net___1;

		internal GameObject callbackObject;

		internal ServerLoginState _loginState___2;

		internal ServerSettingState _settingState___3;

		internal MsgLoginSucceed _msg___4;

		internal MsgServerConnctFailed _msg___5;

		internal int _PC;

		internal object _current;

		internal string ___migrationId;

		internal string ___migrationPassword;

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
				this._monitor___0 = NetMonitor.Instance;
				if (!(this._monitor___0 != null))
				{
					goto IL_2C0;
				}
				this._net___1 = new NetServerMigration(this.migrationId, this.migrationPassword);
				this._net___1.Request();
				this._monitor___0.StartMonitor(new ServerMigrationRetry(this.migrationId, this.migrationPassword, this.callbackObject));
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._loginState___2 = ServerInterface.LoginState;
				this._loginState___2.m_lineId = this._net___1.paramUserId;
				this._loginState___2.m_altLineId = string.Empty;
				this._loginState___2.m_lineAuthToken = string.Empty;
				this._loginState___2.sessionId = this._net___1.resultSessionId;
				this._settingState___3 = ServerInterface.SettingState;
				this._settingState___3.m_energyRefreshTime = this._net___1.resultEnergyRefreshTime;
				this._settingState___3.m_invitBaseIncentive = this._net___1.resultInviteBaseIncentive;
				this._settingState___3.m_rentalBaseIncentive = this._net___1.resultRentalBaseIncentive;
				this._settingState___3.m_userName = this._net___1.userName;
				this._settingState___3.m_userId = this._net___1.userId;
				this._msg___4 = new MsgLoginSucceed();
				this._msg___4.m_userId = this._net___1.userId;
				this._msg___4.m_password = this._net___1.password;
				this._msg___4.m_countryCode = this._net___1.countryCode;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___4, this.callbackObject, "ServerMigration_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerMigration_Succeeded", this._msg___4, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___5 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._net___1.resultStCd == ServerInterface.StatusCode.PassWordError)
				{
					this.callbackObject.SendMessage("ServerMigration_Failed", this._msg___5, SendMessageOptions.DontRequireReceiver);
				}
				else if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerMigration_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_2C0:
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

	public static IEnumerator Process(string migrationId, string migrationPassword, GameObject callbackObject)
	{
		ServerMigration._Process_c__Iterator9C _Process_c__Iterator9C = new ServerMigration._Process_c__Iterator9C();
		_Process_c__Iterator9C.migrationId = migrationId;
		_Process_c__Iterator9C.migrationPassword = migrationPassword;
		_Process_c__Iterator9C.callbackObject = callbackObject;
		_Process_c__Iterator9C.___migrationId = migrationId;
		_Process_c__Iterator9C.___migrationPassword = migrationPassword;
		_Process_c__Iterator9C.___callbackObject = callbackObject;
		return _Process_c__Iterator9C;
	}
}
