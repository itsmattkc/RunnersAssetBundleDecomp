using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetChaoWheelOptions
{
	private sealed class _Process_c__Iterator61 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetChaoWheelOptions _net___1;

		internal GameObject callbackObject;

		internal ServerChaoWheelOptions _chaoWheelOptions___2;

		internal ServerChaoWheelOptions _resultChaoWheelOptions___3;

		internal MsgGetChaoWheelOptionsSucceed _msg___4;

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
					goto IL_201;
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
				goto IL_201;
			}
			this._net___1 = new NetServerGetChaoWheelOptions();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetChaoWheelOptionsRetry(this.callbackObject), 0f, HudNetworkConnect.DisplayType.NO_BG);
			IL_CE:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._chaoWheelOptions___2 = ServerInterface.ChaoWheelOptions;
				this._resultChaoWheelOptions___3 = this._net___1.resultChaoWheelOptions;
				this._net___1.resultChaoWheelOptions.IsConnected = true;
				this._resultChaoWheelOptions___3.CopyTo(this._chaoWheelOptions___2);
				this._msg___4 = new MsgGetChaoWheelOptionsSucceed();
				this._msg___4.m_options = this._chaoWheelOptions___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___4, this.callbackObject, "ServerGetChaoWheelOptions_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetChaoWheelOptions_Succeeded", this._msg___4, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___5 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerGetChaoWheelOptions_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_201:
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
		ServerGetChaoWheelOptions._Process_c__Iterator61 _Process_c__Iterator = new ServerGetChaoWheelOptions._Process_c__Iterator61();
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
