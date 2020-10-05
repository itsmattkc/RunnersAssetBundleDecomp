using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerAtomSerial
{
	private sealed class _Process_c__IteratorA6 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal string campaignId;

		internal string serial;

		internal bool new_user;

		internal NetServerAtomSerial _net___1;

		internal GameObject callbackObject;

		internal MsgSendAtomSerialSucceed _msg___2;

		internal MsgServerConnctFailed _msg___3;

		internal int _PC;

		internal object _current;

		internal string ___campaignId;

		internal string ___serial;

		internal bool ___new_user;

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
					goto IL_227;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_EC;
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
				goto IL_227;
			}
			this._net___1 = new NetServerAtomSerial(this.campaignId, this.serial, this.new_user);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerAtomSerialRetry(this.campaignId, this.serial, this.new_user, this.callbackObject));
			IL_EC:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgSendAtomSerialSucceed();
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerAtomSerial_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerAtomSerial_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._msg___3.m_status == ServerInterface.StatusCode.InvalidSerialCode || this._msg___3.m_status == ServerInterface.StatusCode.UsedSerialCode)
				{
					if (this.callbackObject != null)
					{
						this.callbackObject.SendMessage("ServerAtomSerial_Failed", this._msg___3, SendMessageOptions.DontRequireReceiver);
					}
				}
				else if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerAtomSerial_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_227:
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

	public static IEnumerator Process(string campaignId, string serial, bool new_user, GameObject callbackObject)
	{
		ServerAtomSerial._Process_c__IteratorA6 _Process_c__IteratorA = new ServerAtomSerial._Process_c__IteratorA6();
		_Process_c__IteratorA.campaignId = campaignId;
		_Process_c__IteratorA.serial = serial;
		_Process_c__IteratorA.new_user = new_user;
		_Process_c__IteratorA.callbackObject = callbackObject;
		_Process_c__IteratorA.___campaignId = campaignId;
		_Process_c__IteratorA.___serial = serial;
		_Process_c__IteratorA.___new_user = new_user;
		_Process_c__IteratorA.___callbackObject = callbackObject;
		return _Process_c__IteratorA;
	}

	public static bool GetSerialFromScheme(string scheme, ref string campaign, ref string serial)
	{
		if (string.IsNullOrEmpty(scheme))
		{
			return false;
		}
		int num = scheme.IndexOf("cid=");
		if (num > 0)
		{
			campaign = scheme.Substring(num + "cid=".Length);
			int num2 = campaign.IndexOf("&");
			if (num2 > 0)
			{
				campaign = campaign.Remove(num2);
			}
		}
		int num3 = scheme.IndexOf("serial=");
		if (num3 > 0)
		{
			serial = scheme.Substring(num3 + "serial=".Length);
		}
		if (string.IsNullOrEmpty(serial))
		{
			int num4 = scheme.IndexOf("start_code");
			if (num4 > 0)
			{
				serial = scheme.Substring(num4 + "start_code".Length);
			}
		}
		return !string.IsNullOrEmpty(campaign) && !string.IsNullOrEmpty(serial);
	}
}
