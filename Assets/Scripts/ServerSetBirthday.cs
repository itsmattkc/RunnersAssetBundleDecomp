using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerSetBirthday
{
	private sealed class _Process_c__IteratorB8 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal string birthday;

		internal NetServerSetBirthday _net___1;

		internal GameObject callbackObject;

		internal ServerSettingState _settingState___2;

		internal MsgSetBirthday _msg___3;

		internal MsgServerConnctFailed _msg___4;

		internal int _PC;

		internal object _current;

		internal string ___birthday;

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
					goto IL_1F4;
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
				goto IL_1F4;
			}
			this._net___1 = new NetServerSetBirthday(this.birthday);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerSetBirthdayRetry(this.birthday, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._settingState___2 = ServerInterface.SettingState;
				if (this._settingState___2 != null && string.IsNullOrEmpty(this._settingState___2.m_birthday))
				{
					this._settingState___2.m_birthday = this.birthday;
				}
				this._msg___3 = new MsgSetBirthday();
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerSetBirthday_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerSetBirthday_Succeeded", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___4 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___4, this.callbackObject, "ServerSetBirthday_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_1F4:
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

	public static IEnumerator Process(string birthday, GameObject callbackObject)
	{
		ServerSetBirthday._Process_c__IteratorB8 _Process_c__IteratorB = new ServerSetBirthday._Process_c__IteratorB8();
		_Process_c__IteratorB.birthday = birthday;
		_Process_c__IteratorB.callbackObject = callbackObject;
		_Process_c__IteratorB.___birthday = birthday;
		_Process_c__IteratorB.___callbackObject = callbackObject;
		return _Process_c__IteratorB;
	}
}
