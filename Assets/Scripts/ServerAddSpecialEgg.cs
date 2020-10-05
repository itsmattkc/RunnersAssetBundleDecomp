using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerAddSpecialEgg
{
	private sealed class _Process_c__Iterator5D : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int numSpecialEgg;

		internal NetServerAddSpecialEgg _net___1;

		internal GameObject callbackObject;

		internal ServerChaoWheelOptions _serverChaoWheelOptions___2;

		internal MsgAddSpecialEggSucceed _msg___3;

		internal MsgServerConnctFailed _msg___4;

		internal int _PC;

		internal object _current;

		internal int ___numSpecialEgg;

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
					goto IL_215;
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
				goto IL_215;
			}
			this._net___1 = new NetServerAddSpecialEgg(this.numSpecialEgg);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerAddSpecialEggRetry(this.numSpecialEgg, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._serverChaoWheelOptions___2 = ServerInterface.ChaoWheelOptions;
				if (this._serverChaoWheelOptions___2 != null)
				{
					this._serverChaoWheelOptions___2.NumSpecialEggs = this._net___1.resultSpecialEgg;
				}
				if (RouletteManager.Instance != null)
				{
					RouletteManager.Instance.specialEgg = this._net___1.resultSpecialEgg;
					GeneralUtil.SetRouletteBtnIcon(null, "Btn_roulette");
				}
				this._msg___3 = new MsgAddSpecialEggSucceed();
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerAddSpecialEgg_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerAddSpecialEgg_Succeeded", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___4 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___4, this.callbackObject, "ServerAddSpecialEgg_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_215:
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

	public static IEnumerator Process(int numSpecialEgg, GameObject callbackObject)
	{
		ServerAddSpecialEgg._Process_c__Iterator5D _Process_c__Iterator5D = new ServerAddSpecialEgg._Process_c__Iterator5D();
		_Process_c__Iterator5D.numSpecialEgg = numSpecialEgg;
		_Process_c__Iterator5D.callbackObject = callbackObject;
		_Process_c__Iterator5D.___numSpecialEgg = numSpecialEgg;
		_Process_c__Iterator5D.___callbackObject = callbackObject;
		return _Process_c__Iterator5D;
	}
}
