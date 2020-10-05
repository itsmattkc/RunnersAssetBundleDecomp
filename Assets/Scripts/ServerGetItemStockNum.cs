using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetItemStockNum
{
	private sealed class _Process_c__IteratorAB : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int eventId;

		internal List<int> itemId;

		internal NetServerGetItemStockNum _net___1;

		internal GameObject callbackObject;

		internal List<ServerItemState> _itemStockList___2;

		internal List<ServerItemState>.Enumerator __s_802___3;

		internal ServerItemState _item___4;

		internal MsgGetItemStockNumSucceed _msg___5;

		internal MsgServerConnctFailed _msg___6;

		internal int _PC;

		internal object _current;

		internal int ___eventId;

		internal List<int> ___itemId;

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
					goto IL_26B;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_E0;
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
				goto IL_26B;
			}
			this._net___1 = new NetServerGetItemStockNum(this.eventId, this.itemId);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetItemStockNumRetry(this.eventId, this.itemId, this.callbackObject));
			IL_E0:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._itemStockList___2 = this._net___1.m_itemStockList;
				if (this._itemStockList___2 != null && this._itemStockList___2.Count > 0)
				{
					this.__s_802___3 = this._itemStockList___2.GetEnumerator();
					try
					{
						while (this.__s_802___3.MoveNext())
						{
							this._item___4 = this.__s_802___3.Current;
							GeneralUtil.SetItemCount((ServerItem.Id)this._item___4.m_itemId, (long)this._item___4.m_num);
						}
					}
					finally
					{
						((IDisposable)this.__s_802___3).Dispose();
					}
				}
				this._msg___5 = new MsgGetItemStockNumSucceed();
				this._msg___5.m_itemStockList = this._itemStockList___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerGetItemStockNum_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetItemStockNum_Succeeded", this._msg___5, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___6 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerGetItemStockNum_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_26B:
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

	public static IEnumerator Process(int eventId, List<int> itemId, GameObject callbackObject)
	{
		ServerGetItemStockNum._Process_c__IteratorAB _Process_c__IteratorAB = new ServerGetItemStockNum._Process_c__IteratorAB();
		_Process_c__IteratorAB.eventId = eventId;
		_Process_c__IteratorAB.itemId = itemId;
		_Process_c__IteratorAB.callbackObject = callbackObject;
		_Process_c__IteratorAB.___eventId = eventId;
		_Process_c__IteratorAB.___itemId = itemId;
		_Process_c__IteratorAB.___callbackObject = callbackObject;
		return _Process_c__IteratorAB;
	}
}
