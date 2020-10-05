using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetWheelOptionsGeneral
{
	private sealed class _Process_c__IteratorAE : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int eventId;

		internal int spinId;

		internal NetServerGetWheelOptionsGeneral _net___1;

		internal GameObject callbackObject;

		internal ServerWheelOptionsGeneral _resultWheelOptions___2;

		internal MsgGetWheelOptionsGeneralSucceed _msg___3;

		internal MsgServerConnctFailed _msg___4;

		internal int _PC;

		internal object _current;

		internal int ___eventId;

		internal int ___spinId;

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
					goto IL_1E6;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_E0;
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
				goto IL_1E6;
			}
			this._net___1 = new NetServerGetWheelOptionsGeneral(this.eventId, this.spinId);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetWheelOptionsGeneralRetry(this.spinId, this.eventId, this.callbackObject));
			IL_E0:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._resultWheelOptions___2 = this._net___1.resultWheelOptionsGeneral;
				this._msg___3 = new MsgGetWheelOptionsGeneralSucceed();
				this._msg___3.m_wheelOptionsGeneral = this._resultWheelOptions___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetWheelOptionsGeneral_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetWheelOptionsGeneral_Succeeded", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___4 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___4, this.callbackObject, "ServerGetWheelOptionsGeneral_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_1E6:
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

	private const string SuccessEvent = "ServerGetWheelOptionsGeneral_Succeeded";

	private const string FailEvent = "ServerGetWheelOptionsGeneral_Failed";

	public static IEnumerator Process(int eventId, int spinId, GameObject callbackObject)
	{
		ServerGetWheelOptionsGeneral._Process_c__IteratorAE _Process_c__IteratorAE = new ServerGetWheelOptionsGeneral._Process_c__IteratorAE();
		_Process_c__IteratorAE.eventId = eventId;
		_Process_c__IteratorAE.spinId = spinId;
		_Process_c__IteratorAE.callbackObject = callbackObject;
		_Process_c__IteratorAE.___eventId = eventId;
		_Process_c__IteratorAE.___spinId = spinId;
		_Process_c__IteratorAE.___callbackObject = callbackObject;
		return _Process_c__IteratorAE;
	}
}
