using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetTicker
{
	private sealed class _Process_c__Iterator96 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal List<ServerTickerData> _instance___1;

		internal NetServerGetTicker _net___2;

		internal GameObject callbackObject;

		internal int _num___3;

		internal int _i___4;

		internal ServerTickerData _info___5;

		internal MsgGetTickerDataSucceed _msg___6;

		internal MsgServerConnctFailed _msg___7;

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
					goto IL_22F;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_D8;
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
				goto IL_22F;
			}
			this._instance___1 = ServerInterface.TickerInfo.Data;
			this._net___2 = new NetServerGetTicker();
			this._net___2.Request();
			this._monitor___0.StartMonitor(new ServerGetTickerRetry(this.callbackObject));
			IL_D8:
			if (this._net___2.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			this._instance___1.Clear();
			if (this._net___2.IsSucceeded())
			{
				this._num___3 = this._net___2.GetInfoCount();
				this._i___4 = 0;
				while (this._i___4 < this._num___3)
				{
					this._info___5 = this._net___2.GetInfo(this._i___4);
					if (NetUtil.IsServerTimeWithinPeriod(this._info___5.Start, this._info___5.End))
					{
						this._instance___1.Add(this._info___5);
					}
					this._i___4++;
				}
				this._msg___6 = new MsgGetTickerDataSucceed();
				this._msg___6.m_tickerData = this._instance___1;
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetTicker_Succeeded", this._msg___6, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___7 = new MsgServerConnctFailed(this._net___2.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___7, this.callbackObject, "ServerGetTicker_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_22F:
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
		ServerGetTicker._Process_c__Iterator96 _Process_c__Iterator = new ServerGetTicker._Process_c__Iterator96();
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
