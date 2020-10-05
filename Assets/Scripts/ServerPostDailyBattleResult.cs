using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerPostDailyBattleResult
{
	private sealed class _Process_c__Iterator6C : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerPostDailyBattleResult _net___1;

		internal GameObject callbackObject;

		internal MsgPostDailyBattleResultSucceed _msg___2;

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
					goto IL_23E;
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
				goto IL_23E;
			}
			this._net___1 = new NetServerPostDailyBattleResult();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerPostDailyBattleResultRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgPostDailyBattleResultSucceed();
				this._net___1.battleStatus.CopyTo(this._msg___2.battleStatus);
				this._net___1.battleDataPair.CopyTo(this._msg___2.battleDataPair);
				this._msg___2.rewardFlag = this._net___1.rewardFlag;
				if (this._msg___2.rewardFlag && this._net___1.rewardBattleDataPair != null)
				{
					this._msg___2.rewardBattleDataPair = new ServerDailyBattleDataPair();
					this._net___1.rewardBattleDataPair.CopyTo(this._msg___2.rewardBattleDataPair);
				}
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerPostDailyBattleResult_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerPostDailyBattleResult_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerPostDailyBattleResult_Failed", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_23E:
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

	private const string SuccessEvent = "ServerPostDailyBattleResult_Succeeded";

	private const string FailEvent = "ServerPostDailyBattleResult_Failed";

	public static IEnumerator Process(GameObject callbackObject)
	{
		ServerPostDailyBattleResult._Process_c__Iterator6C _Process_c__Iterator6C = new ServerPostDailyBattleResult._Process_c__Iterator6C();
		_Process_c__Iterator6C.callbackObject = callbackObject;
		_Process_c__Iterator6C.___callbackObject = callbackObject;
		return _Process_c__Iterator6C;
	}
}
