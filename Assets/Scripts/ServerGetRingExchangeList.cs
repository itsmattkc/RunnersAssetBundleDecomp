using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetRingExchangeList
{
	private sealed class _Process_c__IteratorB3 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetRingExchangeList _net___1;

		internal GameObject callbackObject;

		internal List<ServerRingExchangeList> _exchangeList___2;

		internal List<ServerRingExchangeList> _resultExchangeList___3;

		internal int _index___4;

		internal ServerRingExchangeList _e___5;

		internal MsgGetRingExchangeListSucceed _msg___6;

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
					goto IL_252;
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
				goto IL_252;
			}
			this._net___1 = new NetServerGetRingExchangeList();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetRingExchangeListRetry(this.callbackObject), 0f, HudNetworkConnect.DisplayType.NO_BG);
			IL_CE:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._exchangeList___2 = ServerInterface.RingExchangeList;
				this._resultExchangeList___3 = this._net___1.m_ringExchangeList;
				this._exchangeList___2.Clear();
				this._index___4 = 0;
				while (this._index___4 < this._resultExchangeList___3.Count)
				{
					this._e___5 = new ServerRingExchangeList();
					this._resultExchangeList___3[this._index___4].CopyTo(this._e___5);
					this._exchangeList___2.Add(this._e___5);
					this._index___4++;
				}
				this._msg___6 = new MsgGetRingExchangeListSucceed();
				this._msg___6.m_exchangeList = this._resultExchangeList___3;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerGetRingExchangeList_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetRingExchangeList_Succeeded", this._msg___6, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___7 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___7, this.callbackObject, "ServerGetRingExchangeList_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_252:
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
		ServerGetRingExchangeList._Process_c__IteratorB3 _Process_c__IteratorB = new ServerGetRingExchangeList._Process_c__IteratorB3();
		_Process_c__IteratorB.callbackObject = callbackObject;
		_Process_c__IteratorB.___callbackObject = callbackObject;
		return _Process_c__IteratorB;
	}
}
