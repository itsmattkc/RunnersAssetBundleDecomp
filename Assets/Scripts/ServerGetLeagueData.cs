using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetLeagueData
{
	private sealed class _Process_c__Iterator90 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int mode;

		internal NetServerGetLeagueData _net___1;

		internal GameObject callbackObject;

		internal ServerLeagueData _resultLeagueData___2;

		internal MsgGetLeagueDataSucceed _msg___3;

		internal MsgServerConnctFailed _msg___4;

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
					goto IL_200;
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
				goto IL_200;
			}
			this._net___1 = new NetServerGetLeagueData(this.mode);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetLeagueDataRetry(this.mode, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._resultLeagueData___2 = this._net___1.leagueData;
				if (this._resultLeagueData___2 == null)
				{
					this._resultLeagueData___2 = new ServerLeagueData();
				}
				if (SingletonGameObject<RankingManager>.Instance != null)
				{
					SingletonGameObject<RankingManager>.Instance.SetLeagueData(this._resultLeagueData___2);
				}
				this._msg___3 = new MsgGetLeagueDataSucceed();
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetLeagueData_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetLeagueData_Succeeded", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___4 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___4, this.callbackObject, "ServerGetLeagueData_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_200:
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
		ServerGetLeagueData._Process_c__Iterator90 _Process_c__Iterator = new ServerGetLeagueData._Process_c__Iterator90();
		_Process_c__Iterator.mode = mode;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___mode = mode;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
