using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerReLogin
{
	private sealed class _Process_c__Iterator9D : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerReLogin _net___1;

		internal GameObject callbackObject;

		internal ServerLoginState _loginState___2;

		internal MsgServerConnctFailed _msg___3;

		internal int _PC;

		internal object _current;

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
					goto IL_187;
				}
				this._net___1 = new NetServerReLogin();
				this._net___1.Request();
				this._monitor___0.StartMonitor(new ServerReLoginRetry(this.callbackObject));
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
				this._loginState___2.sessionId = this._net___1.resultSessionId;
				this._loginState___2.sessionTimeLimit = this._net___1.sessionTimeLimit;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(null, this.callbackObject, "ServerReLogin_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerReLogin_Succeeded", null, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerReLogin_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_187:
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

	private const string SuccessEvent = "ServerReLogin_Succeeded";

	private const string FailEvent = "ServerReLogin_Failed";

	public static IEnumerator Process(GameObject callbackObject)
	{
		ServerReLogin._Process_c__Iterator9D _Process_c__Iterator9D = new ServerReLogin._Process_c__Iterator9D();
		_Process_c__Iterator9D.callbackObject = callbackObject;
		_Process_c__Iterator9D.___callbackObject = callbackObject;
		return _Process_c__Iterator9D;
	}
}
