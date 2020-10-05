using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetPrizeChaoWheelSpin
{
	private sealed class _Process_c__Iterator63 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int chaoWheelSpinType;

		internal NetServerGetPrizeChaoWheelSpin _net___1;

		internal GameObject callbackObject;

		internal ServerPrizeState _prizeState___2;

		internal MsgGetPrizeChaoWheelSpinSucceed _msg___3;

		internal MsgServerConnctFailed _msg___4;

		internal int _PC;

		internal object _current;

		internal int ___chaoWheelSpinType;

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
					goto IL_27B;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_DA;
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
				goto IL_27B;
			}
			this._net___1 = new NetServerGetPrizeChaoWheelSpin(this.chaoWheelSpinType);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetPrizeChaoWheelSpinRetry(this.chaoWheelSpinType, this.callbackObject), 0f, HudNetworkConnect.DisplayType.NO_BG);
			IL_DA:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._prizeState___2 = null;
				switch (this.chaoWheelSpinType)
				{
				case 0:
					this._prizeState___2 = ServerInterface.PremiumRoulettePrizeList;
					break;
				case 1:
					this._prizeState___2 = ServerInterface.SpecialRoulettePrizeList;
					break;
				}
				if (this._net___1.resultPrizeState != null)
				{
					this._net___1.resultPrizeState.CopyTo(this._prizeState___2);
				}
				this._msg___3 = new MsgGetPrizeChaoWheelSpinSucceed();
				this._msg___3.m_prizeState = this._prizeState___2;
				this._msg___3.m_type = this.chaoWheelSpinType;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetPrizeChaoWheelSpin_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetPrizeChaoWheelSpin_Succeeded", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___4 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___4, this.callbackObject, "ServerGetPrizeChaoWheelSpin_Failed");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetPrizeChaoWheelSpin_Failed", SendMessageOptions.DontRequireReceiver);
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_27B:
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

	public static IEnumerator Process(int chaoWheelSpinType, GameObject callbackObject)
	{
		ServerGetPrizeChaoWheelSpin._Process_c__Iterator63 _Process_c__Iterator = new ServerGetPrizeChaoWheelSpin._Process_c__Iterator63();
		_Process_c__Iterator.chaoWheelSpinType = chaoWheelSpinType;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___chaoWheelSpinType = chaoWheelSpinType;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
