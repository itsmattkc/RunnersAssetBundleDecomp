using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetPrizeWheelSpinGeneral
{
	private sealed class _Process_c__IteratorAC : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int eventId;

		internal int spinType;

		internal NetServerGetPrizeWheelSpinGeneral _net___1;

		internal GameObject callbackObject;

		internal MsgGetPrizeWheelSpinGeneralSucceed _msg___2;

		internal MsgServerConnctFailed _msg___3;

		internal int _PC;

		internal object _current;

		internal int ___eventId;

		internal int ___spinType;

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
					goto IL_221;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_E6;
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
				goto IL_221;
			}
			this._net___1 = new NetServerGetPrizeWheelSpinGeneral(this.eventId, this.spinType);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetPrizeWheelSpinGeneralRetry(this.eventId, this.spinType, this.callbackObject), 0f, HudNetworkConnect.DisplayType.NO_BG);
			IL_E6:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				if (this._net___1.resultPrizeState != null)
				{
					this._net___1.resultPrizeState.CopyTo(ServerInterface.RaidRoulettePrizeList);
				}
				this._msg___2 = new MsgGetPrizeWheelSpinGeneralSucceed();
				this._msg___2.prizeState = ServerInterface.RaidRoulettePrizeList;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetPrizeWheelSpinGeneral_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetPrizeWheelSpinGeneral_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetPrizeWheelSpinGeneral_Failed");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetPrizeWheelSpinGeneral_Failed", SendMessageOptions.DontRequireReceiver);
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_221:
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

	public static IEnumerator Process(int eventId, int spinType, GameObject callbackObject)
	{
		ServerGetPrizeWheelSpinGeneral._Process_c__IteratorAC _Process_c__IteratorAC = new ServerGetPrizeWheelSpinGeneral._Process_c__IteratorAC();
		_Process_c__IteratorAC.eventId = eventId;
		_Process_c__IteratorAC.spinType = spinType;
		_Process_c__IteratorAC.callbackObject = callbackObject;
		_Process_c__IteratorAC.___eventId = eventId;
		_Process_c__IteratorAC.___spinType = spinType;
		_Process_c__IteratorAC.___callbackObject = callbackObject;
		return _Process_c__IteratorAC;
	}
}
