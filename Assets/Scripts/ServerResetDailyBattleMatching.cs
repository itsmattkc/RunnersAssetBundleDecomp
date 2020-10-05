using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerResetDailyBattleMatching
{
	private sealed class _Process_c__Iterator6D : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int type;

		internal NetServerResetDailyBattleMatching _net___1;

		internal GameObject callbackObject;

		internal MsgResetDailyBattleMatchingSucceed _msg___2;

		internal MsgServerConnctFailed _msg___3;

		internal int _PC;

		internal object _current;

		internal int ___type;

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
					goto IL_214;
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
				goto IL_214;
			}
			this._net___1 = new NetServerResetDailyBattleMatching(this.type);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerResetDailyBattleMatchingRetry(this.type, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgResetDailyBattleMatchingSucceed();
				this._net___1.playerState.CopyTo(this._msg___2.playerState);
				this._net___1.playerState.CopyTo(ServerInterface.PlayerState);
				this._net___1.battleDataPair.CopyTo(this._msg___2.battleDataPair);
				this._msg___2.endTime = this._net___1.endTime;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerResetDailyBattleMatching_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerResetDailyBattleMatching_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerResetDailyBattleMatching_Failed", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_214:
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

	private const string SuccessEvent = "ServerResetDailyBattleMatching_Succeeded";

	private const string FailEvent = "ServerResetDailyBattleMatching_Failed";

	public static IEnumerator Process(int type, GameObject callbackObject)
	{
		ServerResetDailyBattleMatching._Process_c__Iterator6D _Process_c__Iterator6D = new ServerResetDailyBattleMatching._Process_c__Iterator6D();
		_Process_c__Iterator6D.type = type;
		_Process_c__Iterator6D.callbackObject = callbackObject;
		_Process_c__Iterator6D.___type = type;
		_Process_c__Iterator6D.___callbackObject = callbackObject;
		return _Process_c__Iterator6D;
	}
}
