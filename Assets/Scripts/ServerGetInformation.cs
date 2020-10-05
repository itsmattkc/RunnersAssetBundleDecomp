using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetInformation
{
	private sealed class _Process_c__Iterator94 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal ServerNoticeInfo _instance___1;

		internal NetServerGetNoticeInfo _net___2;

		internal GameObject callbackObject;

		internal int _num___3;

		internal int _i___4;

		internal NetNoticeItem _info___5;

		internal MsgGetInformationSucceed _msg___6;

		internal MsgServerConnctFailed _msg___7;

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
					goto IL_2D7;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_D3;
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
				goto IL_2D7;
			}
			this._instance___1 = ServerInterface.NoticeInfo;
			this._net___2 = new NetServerGetNoticeInfo();
			this._net___2.Request();
			this._monitor___0.StartMonitor(new ServerGetInformationRetry(this.callbackObject));
			IL_D3:
			if (this._net___2.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			this._instance___1.Clear();
			if (this._net___2.IsSucceeded())
			{
				this._instance___1.m_isGetNoticeInfo = true;
				this._instance___1.LastUpdateInfoTime = NetUtil.GetCurrentTime();
				this._num___3 = this._net___2.GetInfoCount();
				this._i___4 = 0;
				while (this._i___4 < this._num___3)
				{
					this._info___5 = this._net___2.GetInfo(this._i___4);
					if (NetUtil.IsServerTimeWithinPeriod(this._info___5.Start, this._info___5.End))
					{
						if (this._info___5.WindowType == 14)
						{
							this._instance___1.m_rouletteItems.Add(this._info___5);
						}
						else if (this._info___5.WindowType == 15)
						{
							this._instance___1.m_eventItems.Add(this._info___5);
						}
						else
						{
							this._instance___1.m_noticeItems.Add(this._info___5);
						}
					}
					this._i___4++;
				}
				if (this._instance___1.m_noticeItems.Count > 1)
				{
					this._instance___1.m_noticeItems.Sort(new Comparison<NetNoticeItem>(ServerGetInformation.PriorityComparer));
				}
				this._msg___6 = new MsgGetInformationSucceed();
				this._msg___6.m_information = this._instance___1;
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetInformation_Succeeded", this._msg___6, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___7 = new MsgServerConnctFailed(this._net___2.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___7, this.callbackObject, "ServerGetInformation_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_2D7:
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
		ServerGetInformation._Process_c__Iterator94 _Process_c__Iterator = new ServerGetInformation._Process_c__Iterator94();
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}

	private static int PriorityComparer(NetNoticeItem itemA, NetNoticeItem itemB)
	{
		if (itemA == null || itemB == null)
		{
			return 0;
		}
		if (itemA.Id >= (long)NetNoticeItem.OPERATORINFO_START_ID)
		{
			if (itemB.Id >= (long)NetNoticeItem.OPERATORINFO_START_ID)
			{
				return itemA.Priority - itemB.Priority;
			}
			return 1;
		}
		else
		{
			if (itemB.Id >= (long)NetNoticeItem.OPERATORINFO_START_ID)
			{
				return -1;
			}
			return itemA.Priority - itemB.Priority;
		}
	}
}
