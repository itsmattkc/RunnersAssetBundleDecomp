using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerDrawRaidBoss
{
	private sealed class _Process_c__Iterator83 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int eventId;

		internal long score;

		internal NetServerDrawRaidBoss _net___1;

		internal GameObject callbackObject;

		internal MsgDrawRaidBossSucceed _msg___2;

		internal MsgServerConnctFailed _msg___3;

		internal int _PC;

		internal object _current;

		internal int ___eventId;

		internal long ___score;

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
					goto IL_1FF;
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
				goto IL_1FF;
			}
			this._net___1 = new NetServerDrawRaidBoss(this.eventId, this.score);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerDrawRaidBossRetry(this.eventId, this.score, this.callbackObject));
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
					EventManager.Instance.SynchServerEventRaidBossList(this._net___1.RaidBossState);
				}
				this._msg___2 = new MsgDrawRaidBossSucceed();
				this._msg___2.m_raidBossState = this._net___1.RaidBossState;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerDrawRaidBoss_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerDrawRaidBoss_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerDrawRaidBoss_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_1FF:
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

	public static IEnumerator Process(int eventId, long score, GameObject callbackObject)
	{
		ServerDrawRaidBoss._Process_c__Iterator83 _Process_c__Iterator = new ServerDrawRaidBoss._Process_c__Iterator83();
		_Process_c__Iterator.eventId = eventId;
		_Process_c__Iterator.score = score;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___eventId = eventId;
		_Process_c__Iterator.___score = score;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
