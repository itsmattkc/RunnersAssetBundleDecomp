using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetEventUserRaidBossState
{
	private sealed class _Process_c__Iterator78 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int eventId;

		internal NetServerGetEventUserRaidBossState _net___1;

		internal GameObject callbackObject;

		internal MsgGetEventUserRaidBossStateSucceed _msg___2;

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
					goto IL_1FE;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_DA;
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
				goto IL_1FE;
			}
			this._net___1 = new NetServerGetEventUserRaidBossState(this.eventId);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetEventUserRaidBossStateRetry(this.eventId, this.callbackObject), -1f, HudNetworkConnect.DisplayType.ONLY_ICON);
			IL_DA:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgGetEventUserRaidBossStateSucceed();
				if (EventManager.Instance != null)
				{
					EventManager.Instance.SynchServerEventUserRaidBossState(this._net___1.UserRaidBossState);
				}
				GeneralUtil.SetItemCount(ServerItem.Id.RAIDRING, (long)this._net___1.UserRaidBossState.NumRaidbossRings);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetEventUserRaidBossState_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetEventUserRaidBossState_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetEventUserRaidBossState_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_1FE:
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

	private const string SUCCEEDED_FUNCTION_NAME = "ServerGetEventUserRaidBossState_Succeeded";

	private const string FAILED_FUNCTION_NAME = "ServerGetEventUserRaidBossState_Failed";

	public static IEnumerator Process(int eventId, GameObject callbackObject)
	{
		ServerGetEventUserRaidBossState._Process_c__Iterator78 _Process_c__Iterator = new ServerGetEventUserRaidBossState._Process_c__Iterator78();
		_Process_c__Iterator.eventId = eventId;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___eventId = eventId;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
