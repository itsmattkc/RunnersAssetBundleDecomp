using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetEventRaidBossUserList
{
	private sealed class _Process_c__Iterator74 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int eventId;

		internal long raidBossId;

		internal NetServerGetEventRaidBossUserList _net___1;

		internal GameObject callbackObject;

		internal MsgGetEventRaidBossUserListSucceed _msg___2;

		internal MsgServerConnctFailed _msg___3;

		internal int _PC;

		internal object _current;

		internal int ___eventId;

		internal long ___raidBossId;

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
					goto IL_235;
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
				goto IL_235;
			}
			this._net___1 = new NetServerGetEventRaidBossUserList(this.eventId, this.raidBossId);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetEventRaidBossUserListRetry(this.eventId, this.raidBossId, this.callbackObject));
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
					if (this._net___1.RaidBossState != null)
					{
						EventManager.Instance.SynchServerEventRaidBossList(this._net___1.RaidBossState);
					}
					EventManager.Instance.SynchServerEventRaidBossUserList(this._net___1.RaidBossUserStateList, this.raidBossId, this._net___1.RaidBossBonus);
				}
				this._msg___2 = new MsgGetEventRaidBossUserListSucceed();
				this._msg___2.m_bonus = this._net___1.RaidBossBonus;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetEventRaidBossUserList_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetEventRaidBossUserList_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetEventRaidBossUserList_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_235:
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

	private const string SUCCEEDED_FUNCTION_NAME = "ServerGetEventRaidBossUserList_Succeeded";

	private const string FAILED_FUNCTION_NAME = "ServerGetEventRaidBossUserList_Failed";

	public static IEnumerator Process(int eventId, long raidBossId, GameObject callbackObject)
	{
		ServerGetEventRaidBossUserList._Process_c__Iterator74 _Process_c__Iterator = new ServerGetEventRaidBossUserList._Process_c__Iterator74();
		_Process_c__Iterator.eventId = eventId;
		_Process_c__Iterator.raidBossId = raidBossId;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___eventId = eventId;
		_Process_c__Iterator.___raidBossId = raidBossId;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
