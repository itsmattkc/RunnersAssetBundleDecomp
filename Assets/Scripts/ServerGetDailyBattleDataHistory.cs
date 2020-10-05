using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetDailyBattleDataHistory
{
	private sealed class _Process_c__Iterator69 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int count;

		internal NetServerGetDailyBattleDataHistory _net___1;

		internal GameObject callbackObject;

		internal MsgGetDailyBattleDataHistorySucceed _msg___2;

		internal List<ServerDailyBattleDataPair>.Enumerator __s_765___3;

		internal ServerDailyBattleDataPair _pair___4;

		internal ServerDailyBattleDataPair _setPair___5;

		internal MsgServerConnctFailed _msg___6;

		internal int _PC;

		internal object _current;

		internal int ___count;

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
					goto IL_25D;
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
				goto IL_25D;
			}
			this._net___1 = new NetServerGetDailyBattleDataHistory(this.count);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetDailyBattleDataHistoryRetry(this.count, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgGetDailyBattleDataHistorySucceed();
				if (this._net___1.battleDataPairList != null && this._net___1.battleDataPairList.Count > 0)
				{
					this.__s_765___3 = this._net___1.battleDataPairList.GetEnumerator();
					try
					{
						while (this.__s_765___3.MoveNext())
						{
							this._pair___4 = this.__s_765___3.Current;
							this._setPair___5 = new ServerDailyBattleDataPair();
							this._pair___4.CopyTo(this._setPair___5);
							this._msg___2.battleDataPairList.Add(this._setPair___5);
						}
					}
					finally
					{
						((IDisposable)this.__s_765___3).Dispose();
					}
				}
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetDailyBattleDataHistory_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetDailyBattleDataHistory_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___6 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetDailyBattleDataHistory_Failed", this._msg___6, SendMessageOptions.DontRequireReceiver);
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_25D:
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

	private const string SuccessEvent = "ServerGetDailyBattleDataHistory_Succeeded";

	private const string FailEvent = "ServerGetDailyBattleDataHistory_Failed";

	public static IEnumerator Process(int count, GameObject callbackObject)
	{
		ServerGetDailyBattleDataHistory._Process_c__Iterator69 _Process_c__Iterator = new ServerGetDailyBattleDataHistory._Process_c__Iterator69();
		_Process_c__Iterator.count = count;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___count = count;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
