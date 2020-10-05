using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetDailyBattleData
{
	private sealed class _Process_c__Iterator68 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetDailyBattleData _net___1;

		internal GameObject callbackObject;

		internal MsgGetDailyBattleDataSucceed _msg___2;

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
					goto IL_1C2;
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
				goto IL_1C2;
			}
			this._net___1 = new NetServerGetDailyBattleData();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetDailyBattleDataRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgGetDailyBattleDataSucceed();
				this._net___1.battleDataPair.CopyTo(this._msg___2.battleDataPair);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetDailyBattleData_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetDailyBattleData_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetDailyBattleData_Failed", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_1C2:
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

	private const string SuccessEvent = "ServerGetDailyBattleData_Succeeded";

	private const string FailEvent = "ServerGetDailyBattleData_Failed";

	public static IEnumerator Process(GameObject callbackObject)
	{
		ServerGetDailyBattleData._Process_c__Iterator68 _Process_c__Iterator = new ServerGetDailyBattleData._Process_c__Iterator68();
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
