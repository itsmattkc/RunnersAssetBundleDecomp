using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerUpdateDailyBattleStatus
{
	private sealed class _Process_c__Iterator6E : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerUpdateDailyBattleStatus _net___1;

		internal GameObject callbackObject;

		internal MsgUpdateDailyBattleStatusSucceed _msg___2;

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
					goto IL_239;
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
				goto IL_239;
			}
			this._net___1 = new NetServerUpdateDailyBattleStatus();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerUpdateDailyBattleStatusRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgUpdateDailyBattleStatusSucceed();
				this._net___1.battleDataStatus.CopyTo(this._msg___2.battleStatus);
				this._msg___2.endTime = this._net___1.endTime;
				this._msg___2.rewardFlag = this._net___1.rewardFlag;
				if (this._msg___2.rewardFlag && this._net___1.rewardBattleDataPair != null)
				{
					this._msg___2.rewardBattleDataPair = new ServerDailyBattleDataPair();
					this._net___1.rewardBattleDataPair.CopyTo(this._msg___2.rewardBattleDataPair);
				}
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerUpdateDailyBattleStatus_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerUpdateDailyBattleStatus_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerUpdateDailyBattleStatus_Failed", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_239:
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

	private const string SuccessEvent = "ServerUpdateDailyBattleStatus_Succeeded";

	private const string FailEvent = "ServerUpdateDailyBattleStatus_Failed";

	public static IEnumerator Process(GameObject callbackObject)
	{
		ServerUpdateDailyBattleStatus._Process_c__Iterator6E _Process_c__Iterator6E = new ServerUpdateDailyBattleStatus._Process_c__Iterator6E();
		_Process_c__Iterator6E.callbackObject = callbackObject;
		_Process_c__Iterator6E.___callbackObject = callbackObject;
		return _Process_c__Iterator6E;
	}
}
