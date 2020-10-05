using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetEventList
{
	private sealed class _Process_c__Iterator72 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetEventList _net___1;

		internal GameObject callbackObject;

		internal List<ServerEventEntry> _entries___2;

		internal int _listCount___3;

		internal int _index___4;

		internal MsgGetEventListSucceed _msg___5;

		internal MsgServerConnctFailed _msg___6;

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
					goto IL_26A;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_CE;
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
				goto IL_26A;
			}
			this._net___1 = new NetServerGetEventList();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetEventListRetry(this.callbackObject), -1f, HudNetworkConnect.DisplayType.ONLY_ICON);
			IL_CE:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._entries___2 = ServerInterface.EventEntryList;
				this._entries___2.Clear();
				if (this._net___1.resultEventList != null)
				{
					this._listCount___3 = this._net___1.resultEventList.Count;
					this._index___4 = 0;
					while (this._index___4 < this._listCount___3)
					{
						this._entries___2.Add(this._net___1.resultEventList[this._index___4]);
						this._index___4++;
					}
				}
				this._msg___5 = new MsgGetEventListSucceed();
				this._msg___5.m_eventList = this._net___1.resultEventList;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerGetEventList_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetEventList_Succeeded", this._msg___5, SendMessageOptions.DontRequireReceiver);
				}
				if (EventManager.Instance != null)
				{
					EventManager.Instance.SynchServerEntryList();
				}
			}
			else
			{
				this._msg___6 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerGetEventList_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_26A:
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

	public static IEnumerator Process(GameObject callbackObject)
	{
		ServerGetEventList._Process_c__Iterator72 _Process_c__Iterator = new ServerGetEventList._Process_c__Iterator72();
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
