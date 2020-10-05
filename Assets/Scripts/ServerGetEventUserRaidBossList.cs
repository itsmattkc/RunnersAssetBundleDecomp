using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetEventUserRaidBossList
{
	private sealed class _Process_c__Iterator77 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int eventId;

		internal NetServerGetEventUserRaidBossList _net___1;

		internal GameObject callbackObject;

		internal MsgGetEventUserRaidBossListSucceed _msg___2;

		internal MsgServerConnctFailed _msg___3;

		internal int _PC;

		internal object _current;

		internal int ___eventId;

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
					goto IL_21D;
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
				goto IL_21D;
			}
			this._net___1 = new NetServerGetEventUserRaidBossList(this.eventId);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetEventUserRaidBossListRetry(this.eventId, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgGetEventUserRaidBossListSucceed();
				if (EventManager.Instance != null)
				{
					EventManager.Instance.SynchServerEventRaidBossList(this._net___1.UserRaidBossList);
					if (this._net___1.UserRaidBossState != null)
					{
						EventManager.Instance.SynchServerEventUserRaidBossState(this._net___1.UserRaidBossState);
						GeneralUtil.SetItemCount(ServerItem.Id.RAIDRING, (long)this._net___1.UserRaidBossState.NumRaidbossRings);
					}
				}
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetEventUserRaidBossList_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetEventUserRaidBossList_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetEventUserRaidBossList_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_21D:
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

	private const string SUCCEEDED_FUNCTION_NAME = "ServerGetEventUserRaidBossList_Succeeded";

	private const string FAILED_FUNCTION_NAME = "ServerGetEventUserRaidBossList_Failed";

	public static IEnumerator Process(int eventId, GameObject callbackObject)
	{
		ServerGetEventUserRaidBossList._Process_c__Iterator77 _Process_c__Iterator = new ServerGetEventUserRaidBossList._Process_c__Iterator77();
		_Process_c__Iterator.eventId = eventId;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___eventId = eventId;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
