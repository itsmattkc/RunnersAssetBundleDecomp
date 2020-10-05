using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetRedStarExchangeList
{
	private sealed class _Process_c__IteratorB2 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int itemType;

		internal NetServerGetRedStarExchangeList _net___1;

		internal GameObject callbackObject;

		internal int _resultItems___2;

		internal List<ServerRedStarItemState> _exchangeList___3;

		internal List<ServerRedStarItemState> _serverExachangeList___4;

		internal ServerSettingState _settingState___5;

		internal int _i___6;

		internal ServerRedStarItemState _result___7;

		internal ServerRedStarItemState _item___8;

		internal MsgGetRedStarExchangeListSucceed _msg___9;

		internal MsgServerConnctFailed _msg___10;

		internal int _PC;

		internal object _current;

		internal int ___itemType;

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
					goto IL_361;
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
				goto IL_361;
			}
			this._net___1 = new NetServerGetRedStarExchangeList(this.itemType);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetRedStarExchangeListRetry(this.itemType, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._resultItems___2 = this._net___1.resultItems;
				this._exchangeList___3 = new List<ServerRedStarItemState>(this._net___1.resultItems);
				this._serverExachangeList___4 = null;
				switch (this.itemType)
				{
				case 0:
					this._serverExachangeList___4 = ServerInterface.RedStarItemList;
					this._settingState___5 = ServerInterface.SettingState;
					if (this._settingState___5 != null)
					{
						this._settingState___5.m_birthday = this._net___1.resultBirthDay;
						this._settingState___5.m_monthPurchase = this._net___1.resultMonthPurchase;
					}
					break;
				case 1:
					this._serverExachangeList___4 = ServerInterface.RedStarExchangeRingItemList;
					break;
				case 2:
					this._serverExachangeList___4 = ServerInterface.RedStarExchangeEnergyItemList;
					break;
				case 4:
					this._serverExachangeList___4 = ServerInterface.RedStarExchangeRaidbossEnergyItemList;
					break;
				}
				if (this._serverExachangeList___4 != null)
				{
					this._serverExachangeList___4.Clear();
				}
				this._i___6 = 0;
				while (this._i___6 < this._resultItems___2)
				{
					this._result___7 = this._net___1.GetResultRedStarItemState(this._i___6);
					if (this._result___7 != null)
					{
						this._item___8 = new ServerRedStarItemState();
						this._result___7.CopyTo(this._item___8);
						this._exchangeList___3.Add(this._item___8);
						if (this._serverExachangeList___4 != null)
						{
							this._serverExachangeList___4.Add(this._item___8);
						}
					}
					this._i___6++;
				}
				this._msg___9 = new MsgGetRedStarExchangeListSucceed();
				this._msg___9.m_itemList = this._exchangeList___3;
				this._msg___9.m_totalItems = this._resultItems___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___9, this.callbackObject, "ServerGetRedStarExchangeList_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetRedStarExchangeList_Succeeded", this._msg___9, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___10 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___10, this.callbackObject, "ServerGetRedStarExchangeList_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_361:
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

	public static IEnumerator Process(int itemType, GameObject callbackObject)
	{
		ServerGetRedStarExchangeList._Process_c__IteratorB2 _Process_c__IteratorB = new ServerGetRedStarExchangeList._Process_c__IteratorB2();
		_Process_c__IteratorB.itemType = itemType;
		_Process_c__IteratorB.callbackObject = callbackObject;
		_Process_c__IteratorB.___itemType = itemType;
		_Process_c__IteratorB.___callbackObject = callbackObject;
		return _Process_c__IteratorB;
	}
}
