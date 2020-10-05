using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetCountry
{
	private sealed class _Process_c__Iterator93 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetCountry _net___1;

		internal GameObject callbackObject;

		internal MsgGetCountrySucceed _msg___2;

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
					goto IL_202;
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
				goto IL_202;
			}
			this._net___1 = new NetServerGetCountry();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetCountryRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				ServerInterface.SettingState.m_countryId = this._net___1.resultCountryId;
				ServerInterface.SettingState.m_countryCode = this._net___1.resultCountryCode;
				this._msg___2 = new MsgGetCountrySucceed();
				this._msg___2.m_countryId = this._net___1.resultCountryId;
				this._msg___2.m_countryCode = this._net___1.resultCountryCode;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetCountry_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetCountry_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetCountry_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_202:
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
		ServerGetCountry._Process_c__Iterator93 _Process_c__Iterator = new ServerGetCountry._Process_c__Iterator93();
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
