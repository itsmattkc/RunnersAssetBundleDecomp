using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetDailyBattleStatus
{
	private sealed class _Process_c__Iterator6A : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetDailyBattleStatus _net___1;

		internal GameObject callbackObject;

		internal MsgGetDailyBattleStatusSucceed _msg___2;

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
					goto IL_1D8;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_C8;
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
				goto IL_1D8;
			}
			this._net___1 = new NetServerGetDailyBattleStatus();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetDailyBattleStatusRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgGetDailyBattleStatusSucceed();
				this._net___1.battleStatus.CopyTo(this._msg___2.battleStatus);
				this._msg___2.endTime = this._net___1.endTime;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetDailyBattleStatus_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetDailyBattleStatus_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetDailyBattleStatus_Failed", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_1D8:
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

	private const string SuccessEvent = "ServerGetDailyBattleStatus_Succeeded";

	private const string FailEvent = "ServerGetDailyBattleStatus_Failed";

	public static IEnumerator Process(GameObject callbackObject)
	{
		ServerGetDailyBattleStatus._Process_c__Iterator6A _Process_c__Iterator6A = new ServerGetDailyBattleStatus._Process_c__Iterator6A();
		_Process_c__Iterator6A.callbackObject = callbackObject;
		_Process_c__Iterator6A.___callbackObject = callbackObject;
		return _Process_c__Iterator6A;
	}
}
