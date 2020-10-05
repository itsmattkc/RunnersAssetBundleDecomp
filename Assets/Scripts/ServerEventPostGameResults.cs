using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerEventPostGameResults
{
	private sealed class _Process_c__Iterator6F : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int eventId;

		internal int numRaidBossRings;

		internal NetServerEventPostGameResults _net___1;

		internal GameObject callbackObject;

		internal ServerEventUserRaidBossState _userRaidBossState___2;

		internal MsgEventPostGameResultsSucceed _msg___3;

		internal MsgServerConnctFailed _msg___4;

		internal int _PC;

		internal object _current;

		internal int ___eventId;

		internal int ___numRaidBossRings;

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
					goto IL_210;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_E0;
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
				goto IL_210;
			}
			this._net___1 = new NetServerEventPostGameResults(this.eventId, this.numRaidBossRings);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerEventPostGameResultsRetry(this.eventId, this.numRaidBossRings, this.callbackObject));
			IL_E0:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				if (EventManager.Instance != null)
				{
					this._userRaidBossState___2 = this._net___1.UserRaidBossState;
					EventManager.Instance.SynchServerEventUserRaidBossState(this._userRaidBossState___2);
				}
				GeneralUtil.SetItemCount(ServerItem.Id.RAIDRING, (long)this._net___1.UserRaidBossState.NumRaidbossRings);
				this._msg___3 = new MsgEventPostGameResultsSucceed();
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerEventPostGameResults_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerEventPostGameResults_Succeeded", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___4 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___4, this.callbackObject, "ServerEventPostGameResults_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_210:
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

	public static IEnumerator Process(int eventId, int numRaidBossRings, GameObject callbackObject)
	{
		ServerEventPostGameResults._Process_c__Iterator6F _Process_c__Iterator6F = new ServerEventPostGameResults._Process_c__Iterator6F();
		_Process_c__Iterator6F.eventId = eventId;
		_Process_c__Iterator6F.numRaidBossRings = numRaidBossRings;
		_Process_c__Iterator6F.callbackObject = callbackObject;
		_Process_c__Iterator6F.___eventId = eventId;
		_Process_c__Iterator6F.___numRaidBossRings = numRaidBossRings;
		_Process_c__Iterator6F.___callbackObject = callbackObject;
		return _Process_c__Iterator6F;
	}
}
