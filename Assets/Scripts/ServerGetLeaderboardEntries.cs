using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetLeaderboardEntries
{
	private sealed class _Process_c__Iterator8F : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int mode;

		internal int first;

		internal int count;

		internal int index;

		internal int rankingType;

		internal int eventId;

		internal string[] friendIdList;

		internal NetServerGetLeaderboardEntries _net___1;

		internal GameObject callbackObject;

		internal MsgGetLeaderboardEntriesSucceed _msg___2;

		internal MsgServerConnctFailed _msg___3;

		internal int _PC;

		internal object _current;

		internal int ___mode;

		internal int ___first;

		internal int ___count;

		internal int ___index;

		internal int ___rankingType;

		internal int ___eventId;

		internal string[] ___friendIdList;

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
				goto IL_11C;
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
			this._net___1 = new NetServerGetLeaderboardEntries(this.mode, this.first, this.count, this.index, this.rankingType, this.eventId, this.friendIdList);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetLeaderboardEntriesRetry(this.mode, this.first, this.count, this.index, this.rankingType, this.eventId, this.friendIdList, this.callbackObject));
			IL_11C:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgGetLeaderboardEntriesSucceed();
				this._msg___2.m_leaderboardEntries = ServerInterface.LeaderboardEntries;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetLeaderboardEntries_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetLeaderboardEntries_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetLeaderboardEntries_Failed");
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

	public static IEnumerator Process(int mode, int first, int count, int index, int rankingType, int eventId, string[] friendIdList, GameObject callbackObject)
	{
		ServerGetLeaderboardEntries._Process_c__Iterator8F _Process_c__Iterator8F = new ServerGetLeaderboardEntries._Process_c__Iterator8F();
		_Process_c__Iterator8F.mode = mode;
		_Process_c__Iterator8F.first = first;
		_Process_c__Iterator8F.count = count;
		_Process_c__Iterator8F.index = index;
		_Process_c__Iterator8F.rankingType = rankingType;
		_Process_c__Iterator8F.eventId = eventId;
		_Process_c__Iterator8F.friendIdList = friendIdList;
		_Process_c__Iterator8F.callbackObject = callbackObject;
		_Process_c__Iterator8F.___mode = mode;
		_Process_c__Iterator8F.___first = first;
		_Process_c__Iterator8F.___count = count;
		_Process_c__Iterator8F.___index = index;
		_Process_c__Iterator8F.___rankingType = rankingType;
		_Process_c__Iterator8F.___eventId = eventId;
		_Process_c__Iterator8F.___friendIdList = friendIdList;
		_Process_c__Iterator8F.___callbackObject = callbackObject;
		return _Process_c__Iterator8F;
	}
}
