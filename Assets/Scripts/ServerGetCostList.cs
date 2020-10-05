using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetCostList
{
	private sealed class _Process_c__Iterator85 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetCostList _net___1;

		internal GameObject callbackObject;

		internal List<ServerConsumedCostData> _costData___2;

		internal List<ServerConsumedCostData> _resultCostData___3;

		internal List<ServerConsumedCostData>.Enumerator __s_775___4;

		internal ServerConsumedCostData _data___5;

		internal MsgGetCostListSucceed _msg___6;

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
					goto IL_268;
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
				goto IL_268;
			}
			this._net___1 = new NetServerGetCostList();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetCostListRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._costData___2 = ServerInterface.CostList;
				if (this._costData___2 != null)
				{
					this._costData___2.Clear();
					this._resultCostData___3 = this._net___1.resultCostList;
					if (this._resultCostData___3 != null)
					{
						this.__s_775___4 = this._resultCostData___3.GetEnumerator();
						try
						{
							while (this.__s_775___4.MoveNext())
							{
								this._data___5 = this.__s_775___4.Current;
								if (this._data___5 != null)
								{
									this._costData___2.Add(this._data___5);
								}
							}
						}
						finally
						{
							((IDisposable)this.__s_775___4).Dispose();
						}
					}
				}
				this._msg___6 = new MsgGetCostListSucceed();
				this._msg___6.m_costList = this._costData___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "GetCostList_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("GetCostList_Succeeded", this._msg___6, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___7 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___7, this.callbackObject, "GetCostList_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_268:
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
		ServerGetCostList._Process_c__Iterator85 _Process_c__Iterator = new ServerGetCostList._Process_c__Iterator85();
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
