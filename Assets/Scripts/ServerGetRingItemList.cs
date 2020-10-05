using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetRingItemList
{
	private sealed class _Process_c__Iterator8B : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetRingItemList _net___1;

		internal GameObject callbackObject;

		internal int _resultRingItemStates___2;

		internal List<ServerRingItemState> _ringItemList___3;

		internal int _i___4;

		internal ServerRingItemState _ringItemState___5;

		internal MsgGetRingItemStateSucceed _msg___6;

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
					goto IL_232;
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
				goto IL_232;
			}
			this._net___1 = new NetServerGetRingItemList();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetRingItemListRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._resultRingItemStates___2 = this._net___1.resultRingItemStates;
				this._ringItemList___3 = new List<ServerRingItemState>(this._resultRingItemStates___2);
				this._i___4 = 0;
				while (this._i___4 < this._resultRingItemStates___2)
				{
					this._ringItemState___5 = this._net___1.GetResultRingItemState(this._i___4);
					this._ringItemList___3.Add(this._ringItemState___5);
					this._i___4++;
				}
				this._msg___6 = new MsgGetRingItemStateSucceed();
				this._msg___6.m_RingStateList = this._ringItemList___3;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "GetRingItemList_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("GetRingItemList_Succeeded", this._msg___6, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___7 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___7, this.callbackObject, "GetRingItemList_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_232:
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
		ServerGetRingItemList._Process_c__Iterator8B _Process_c__Iterator8B = new ServerGetRingItemList._Process_c__Iterator8B();
		_Process_c__Iterator8B.callbackObject = callbackObject;
		_Process_c__Iterator8B.___callbackObject = callbackObject;
		return _Process_c__Iterator8B;
	}
}
