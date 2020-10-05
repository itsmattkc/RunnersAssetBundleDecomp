using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetWeeklyLeaderboardOptions
{
	private sealed class _Process_c__Iterator92 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int mode;

		internal NetServerGetWeeklyLeaderboardOptions _net___1;

		internal GameObject callbackObject;

		internal MsgGetWeeklyLeaderboardOptions _msg___2;

		internal MsgServerConnctFailed _msg___3;

		internal int _PC;

		internal object _current;

		internal int ___mode;

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
					goto IL_1F3;
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
				goto IL_1F3;
			}
			this._net___1 = new NetServerGetWeeklyLeaderboardOptions(this.mode);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetWeeklyLeaderboardOptionsRetry(this.mode, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgGetWeeklyLeaderboardOptions();
				this._msg___2.m_weeklyLeaderboardOptions = this._net___1.weeklyLeaderboardOptions;
				if (SingletonGameObject<RankingManager>.Instance != null)
				{
					SingletonGameObject<RankingManager>.Instance.SetRankingDataSet(this._net___1.weeklyLeaderboardOptions);
				}
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetWeeklyLeaderboardOptions_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetWeeklyLeaderboardOptions_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerWeeklyLeaderboardOptions_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_1F3:
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

	public static IEnumerator Process(int mode, GameObject callbackObject)
	{
		ServerGetWeeklyLeaderboardOptions._Process_c__Iterator92 _Process_c__Iterator = new ServerGetWeeklyLeaderboardOptions._Process_c__Iterator92();
		_Process_c__Iterator.mode = mode;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___mode = mode;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
