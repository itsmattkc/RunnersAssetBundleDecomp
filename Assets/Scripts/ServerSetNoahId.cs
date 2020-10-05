using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerSetNoahId
{
	private sealed class _Process_c__IteratorA8 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal string noahId;

		internal NetServerSetNoahId _net___1;

		internal GameObject callbackObject;

		internal MsgSetNoahIdSucceed _msg___2;

		internal MsgServerConnctFailed _msg___3;

		internal int _PC;

		internal object _current;

		internal string ___noahId;

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
					goto IL_1C2;
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
				goto IL_1C2;
			}
			this._net___1 = new NetServerSetNoahId(this.noahId);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerSetNoahIdRetry(this.noahId, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgSetNoahIdSucceed();
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerSetNoahId_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerSetNoahId_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				global::Debug.Log("ServerSetNoahId: connectIsFailded");
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerSetNoahId_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_1C2:
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

	public static IEnumerator Process(string noahId, GameObject callbackObject)
	{
		ServerSetNoahId._Process_c__IteratorA8 _Process_c__IteratorA = new ServerSetNoahId._Process_c__IteratorA8();
		_Process_c__IteratorA.noahId = noahId;
		_Process_c__IteratorA.callbackObject = callbackObject;
		_Process_c__IteratorA.___noahId = noahId;
		_Process_c__IteratorA.___callbackObject = callbackObject;
		return _Process_c__IteratorA;
	}
}
