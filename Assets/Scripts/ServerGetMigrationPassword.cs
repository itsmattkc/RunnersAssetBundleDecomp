using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetMigrationPassword
{
	private sealed class _Process_c__Iterator95 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal string userPassword;

		internal NetServerGetMigrationPassword _net___1;

		internal GameObject callbackObject;

		internal MsgGetMigrationPasswordSucceed _msg___2;

		internal MsgServerConnctFailed _msg___3;

		internal int _PC;

		internal object _current;

		internal string ___userPassword;

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
					goto IL_1DE;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_D4;
			default:
				return false;
			}
			if (!this._monitor___0.IsEndPrepare())
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (!this._monitor___0.IsSuccessPrepare())
			{
				goto IL_1DE;
			}
			this._net___1 = new NetServerGetMigrationPassword(this.userPassword);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetMigrationPasswordRetry(this.userPassword, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				ServerInterface.MigrationPassword = this._net___1.paramMigrationPassword;
				this._msg___2 = new MsgGetMigrationPasswordSucceed();
				this._msg___2.m_migrationPassword = this._net___1.paramMigrationPassword;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetMigrationPassword_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetMigrationPassword_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetMigrationPassword_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_1DE:
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

	public static IEnumerator Process(string userPassword, GameObject callbackObject)
	{
		ServerGetMigrationPassword._Process_c__Iterator95 _Process_c__Iterator = new ServerGetMigrationPassword._Process_c__Iterator95();
		_Process_c__Iterator.userPassword = userPassword;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___userPassword = userPassword;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
