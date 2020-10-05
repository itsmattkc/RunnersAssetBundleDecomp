using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetMessageList
{
	private sealed class _Process_c__Iterator9E : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetMessageList _net___1;

		internal GameObject callbackObject;

		internal List<ServerMessageEntry> _messageEntries___2;

		internal List<ServerOperatorMessageEntry> _operatorMessageEntries___3;

		internal int _resultMessageEntries___4;

		internal int _index___5;

		internal ServerMessageEntry _messageEntry___6;

		internal int _resultOperatorMessageEntries___7;

		internal int _index___8;

		internal ServerOperatorMessageEntry _messageEntry___9;

		internal MsgGetMessageListSucceed _msg___10;

		internal MsgServerConnctFailed _msg___11;

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
					goto IL_2A0;
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
				goto IL_2A0;
			}
			this._net___1 = new NetServerGetMessageList();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetMessageListRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._messageEntries___2 = ServerInterface.MessageList;
				this._messageEntries___2.Clear();
				this._operatorMessageEntries___3 = ServerInterface.OperatorMessageList;
				this._operatorMessageEntries___3.Clear();
				this._resultMessageEntries___4 = this._net___1.resultMessages;
				this._index___5 = 0;
				while (this._index___5 < this._resultMessageEntries___4)
				{
					this._messageEntry___6 = this._net___1.GetResultMessageEntry(this._index___5);
					this._messageEntries___2.Add(this._messageEntry___6);
					this._index___5++;
				}
				this._resultOperatorMessageEntries___7 = this._net___1.resultOperatorMessages;
				this._index___8 = 0;
				while (this._index___8 < this._resultOperatorMessageEntries___7)
				{
					this._messageEntry___9 = this._net___1.GetResultOperatorMessageEntry(this._index___8);
					this._operatorMessageEntries___3.Add(this._messageEntry___9);
					this._index___8++;
				}
				this._msg___10 = new MsgGetMessageListSucceed();
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___10, this.callbackObject, "ServerGetMessageList_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetMessageList_Succeeded", this._msg___10, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___11 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___11, this.callbackObject, "ServerGetMessageList_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_2A0:
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

	private const string SuccessEvent = "ServerGetMessageList_Succeeded";

	private const string FailEvent = "ServerGetMessageList_Failed";

	public static IEnumerator Process(GameObject callbackObject)
	{
		ServerGetMessageList._Process_c__Iterator9E _Process_c__Iterator9E = new ServerGetMessageList._Process_c__Iterator9E();
		_Process_c__Iterator9E.callbackObject = callbackObject;
		_Process_c__Iterator9E.___callbackObject = callbackObject;
		return _Process_c__Iterator9E;
	}
}
