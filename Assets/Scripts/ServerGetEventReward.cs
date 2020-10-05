using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetEventReward
{
	private sealed class _Process_c__Iterator75 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int eventId;

		internal NetServerGetEventReward _net___1;

		internal GameObject callbackObject;

		internal List<ServerEventReward> _rewards___2;

		internal int _listCount___3;

		internal int _index___4;

		internal MsgGetEventRewardSucceed _msg___5;

		internal MsgServerConnctFailed _msg___6;

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
					goto IL_276;
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
				goto IL_276;
			}
			this._net___1 = new NetServerGetEventReward(this.eventId);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetEventRewardRetry(this.eventId, this.callbackObject), -1f, HudNetworkConnect.DisplayType.ONLY_ICON);
			IL_DA:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._rewards___2 = ServerInterface.EventRewardList;
				this._rewards___2.Clear();
				if (this._net___1.resultEventRewardList != null)
				{
					this._listCount___3 = this._net___1.resultEventRewardList.Count;
					this._index___4 = 0;
					while (this._index___4 < this._listCount___3)
					{
						this._rewards___2.Add(this._net___1.resultEventRewardList[this._index___4]);
						this._index___4++;
					}
				}
				this._msg___5 = new MsgGetEventRewardSucceed();
				this._msg___5.m_eventRewardList = this._net___1.resultEventRewardList;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerGetEventReward_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetEventReward_Succeeded", this._msg___5, SendMessageOptions.DontRequireReceiver);
				}
				if (EventManager.Instance != null)
				{
					EventManager.Instance.SynchServerRewardList();
				}
			}
			else
			{
				this._msg___6 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerGetEventReward_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_276:
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

	public static IEnumerator Process(int eventId, GameObject callbackObject)
	{
		ServerGetEventReward._Process_c__Iterator75 _Process_c__Iterator = new ServerGetEventReward._Process_c__Iterator75();
		_Process_c__Iterator.eventId = eventId;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___eventId = eventId;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
