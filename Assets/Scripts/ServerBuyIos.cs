using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerBuyIos
{
	private sealed class _Process_c__IteratorB1 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal string receiptData;

		internal NetServerBuyIos _net___1;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___2;

		internal ServerPlayerState _resultPlayerState___3;

		internal MsgGetPlayerStateSucceed _msg___4;

		internal MsgServerConnctFailed _msg___5;

		internal int _PC;

		internal object _current;

		internal string ___receiptData;

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
					goto IL_258;
				}
				this._monitor___0.PrepareConnect(ServerSessionWatcher.ValidateType.LOGIN_OR_RELOGIN);
				break;
			case 1u:
				break;
			case 2u:
				goto IL_D5;
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
				goto IL_258;
			}
			this._net___1 = new NetServerBuyIos(this.receiptData);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerBuyIosRetry(this.receiptData, this.callbackObject));
			IL_D5:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				global::Debug.Log("ServerBuyIos: connectIsSucceed");
				this._playerState___2 = ServerInterface.PlayerState;
				this._resultPlayerState___3 = this._net___1.resultPlayerState;
				this._resultPlayerState___3.CopyTo(this._playerState___2);
				ServerInterface.SettingState.m_isPurchased = true;
				this._msg___4 = new MsgGetPlayerStateSucceed();
				this._msg___4.m_playerState = this._resultPlayerState___3;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___4, this.callbackObject, "ServerBuyIos_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerBuyIos_Succeeded", this._msg___4, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				global::Debug.Log("ServerBuyIos: connectIsFailded");
				this._msg___5 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._net___1.resultStCd == ServerInterface.StatusCode.AlreadyProcessedReceipt)
				{
					if (this.callbackObject != null)
					{
						this.callbackObject.SendMessage("ServerBuyIos_Failed", this._msg___5, SendMessageOptions.DontRequireReceiver);
					}
				}
				else if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerBuyIos_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_258:
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

	public static IEnumerator Process(string receiptData, GameObject callbackObject)
	{
		ServerBuyIos._Process_c__IteratorB1 _Process_c__IteratorB = new ServerBuyIos._Process_c__IteratorB1();
		_Process_c__IteratorB.receiptData = receiptData;
		_Process_c__IteratorB.callbackObject = callbackObject;
		_Process_c__IteratorB.___receiptData = receiptData;
		_Process_c__IteratorB.___callbackObject = callbackObject;
		return _Process_c__IteratorB;
	}
}
