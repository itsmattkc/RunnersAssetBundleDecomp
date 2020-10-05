using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetVariousParameter
{
	private sealed class _Process_c__Iterator97 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetVariousParameter _net___1;

		internal GameObject callbackObject;

		internal MsgGetVariousParameterSucceed _msg___2;

		internal MsgServerConnctFailed _msg___3;

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
					goto IL_2AF;
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
				goto IL_2AF;
			}
			this._net___1 = new NetServerGetVariousParameter();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetVariousParameterRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				ServerInterface.SettingState.m_energyRefreshTime = (long)this._net___1.resultEnergyRecveryTime;
				ServerInterface.SettingState.m_energyRecoveryMax = this._net___1.resultEnergyRecoveryMax;
				ServerInterface.SettingState.m_onePlayCmCount = this._net___1.resultOnePlayCmCount;
				ServerInterface.SettingState.m_onePlayContinueCount = this._net___1.resultOnePlayContinueCount;
				ServerInterface.SettingState.m_cmSkipCount = this._net___1.resultCmSkipCount;
				ServerInterface.SettingState.m_isPurchased = this._net___1.resultIsPurchased;
				this._msg___2 = new MsgGetVariousParameterSucceed();
				this._msg___2.m_energyRefreshTime = this._net___1.resultEnergyRecveryTime;
				this._msg___2.m_energyRecoveryMax = this._net___1.resultEnergyRecoveryMax;
				this._msg___2.m_onePlayCmCount = this._net___1.resultOnePlayCmCount;
				this._msg___2.m_onePlayContinueCount = this._net___1.resultOnePlayContinueCount;
				this._msg___2.m_cmSkipCount = this._net___1.resultCmSkipCount;
				this._msg___2.m_isPurchased = this._net___1.resultIsPurchased;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetVariousParameter_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetVariousParameter_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetVariousParameter_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_2AF:
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
		ServerGetVariousParameter._Process_c__Iterator97 _Process_c__Iterator = new ServerGetVariousParameter._Process_c__Iterator97();
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
