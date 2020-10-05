using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetWheelOptions
{
	private sealed class _Process_c__IteratorAD : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetWheelOptions _net___1;

		internal GameObject callbackObject;

		internal ServerWheelOptions _wheelOptions___2;

		internal ServerWheelOptions _resultWheelOptions___3;

		internal MsgGetWheelOptionsSucceed _msg___4;

		internal MsgServerConnctFailed _msg___5;

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
					goto IL_1EA;
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
				goto IL_1EA;
			}
			this._net___1 = new NetServerGetWheelOptions();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetWheelOptionsRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._wheelOptions___2 = ServerInterface.WheelOptions;
				this._resultWheelOptions___3 = this._net___1.resultWheelOptions;
				this._resultWheelOptions___3.CopyTo(this._wheelOptions___2);
				this._msg___4 = new MsgGetWheelOptionsSucceed();
				this._msg___4.m_wheelOptions = this._wheelOptions___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___4, this.callbackObject, "ServerGetWheelOptions_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetWheelOptions_Succeeded", this._msg___4, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___5 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerGetWheelOptions_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_1EA:
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

	private const string SuccessEvent = "ServerGetWheelOptions_Succeeded";

	private const string FailEvent = "ServerGetWheelOptions_Failed";

	public static IEnumerator Process(GameObject callbackObject)
	{
		ServerGetWheelOptions._Process_c__IteratorAD _Process_c__IteratorAD = new ServerGetWheelOptions._Process_c__IteratorAD();
		_Process_c__IteratorAD.callbackObject = callbackObject;
		_Process_c__IteratorAD.___callbackObject = callbackObject;
		return _Process_c__IteratorAD;
	}
}
