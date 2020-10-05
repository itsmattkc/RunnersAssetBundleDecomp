using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetEventRaidBossDesiredList
{
	private sealed class _Process_c__Iterator73 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int eventId;

		internal long raidBossId;

		internal List<string> friendIdList;

		internal NetServerGetEventRaidBossDesiredList _net___1;

		internal GameObject callbackObject;

		internal MsgEventRaidBossDesiredListSucceed _msg___2;

		internal MsgServerConnctFailed _msg___3;

		internal int _PC;

		internal object _current;

		internal int ___eventId;

		internal long ___raidBossId;

		internal List<string> ___friendIdList;

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
					goto IL_1E6;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_EC;
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
				goto IL_1E6;
			}
			this._net___1 = new NetServerGetEventRaidBossDesiredList(this.eventId, this.raidBossId, this.friendIdList);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetEventRaidBossDesiredListRetry(this.eventId, this.raidBossId, this.friendIdList, this.callbackObject));
			IL_EC:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgEventRaidBossDesiredListSucceed();
				this._msg___2.m_desiredList = this._net___1.DesiredList;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetEventRaidBossDesiredList_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetEventRaidBossDesiredList_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetEventRaidBossDesiredList_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_1E6:
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

	public static IEnumerator Process(int eventId, long raidBossId, List<string> friendIdList, GameObject callbackObject)
	{
		ServerGetEventRaidBossDesiredList._Process_c__Iterator73 _Process_c__Iterator = new ServerGetEventRaidBossDesiredList._Process_c__Iterator73();
		_Process_c__Iterator.eventId = eventId;
		_Process_c__Iterator.raidBossId = raidBossId;
		_Process_c__Iterator.friendIdList = friendIdList;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___eventId = eventId;
		_Process_c__Iterator.___raidBossId = raidBossId;
		_Process_c__Iterator.___friendIdList = friendIdList;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
