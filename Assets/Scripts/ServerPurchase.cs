using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerPurchase
{
	private sealed class _Process_c__IteratorB5 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal bool purchaseResult;

		internal NetServerPurchase _net___0;

		internal ServerPlayerState _playerState___1;

		internal ServerPlayerState _resultPlayerState___2;

		internal GameObject callbackObject;

		internal MsgGetPlayerStateSucceed _msg___3;

		internal MsgServerConnctFailed _msg___4;

		internal int _PC;

		internal object _current;

		internal bool ___purchaseResult;

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
				this._net___0 = new NetServerPurchase(this.purchaseResult);
				this._net___0.Request();
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._net___0.IsExecuting())
			{
				this._current = null;
				this._PC = 1;
				return true;
			}
			if (this._net___0.IsSucceeded())
			{
				this._playerState___1 = ServerInterface.PlayerState;
				this._resultPlayerState___2 = this._net___0.resultPlayerState;
				this._resultPlayerState___2.CopyTo(this._playerState___1);
				if (this.callbackObject != null)
				{
					this._msg___3 = new MsgGetPlayerStateSucceed();
					this._msg___3.m_playerState = this._playerState___1;
					this.callbackObject.SendMessage("ServerPurchase_Succeeded", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			else if (this.callbackObject != null)
			{
				this._msg___4 = new MsgServerConnctFailed(this._net___0.resultStCd);
				this._msg___4.m_status = this._net___0.resultStCd;
				this.callbackObject.SendMessage("ServerPurchase_Failed", this._msg___4, SendMessageOptions.DontRequireReceiver);
			}
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

	public static IEnumerator Process(bool purchaseResult, GameObject callbackObject)
	{
		ServerPurchase._Process_c__IteratorB5 _Process_c__IteratorB = new ServerPurchase._Process_c__IteratorB5();
		_Process_c__IteratorB.purchaseResult = purchaseResult;
		_Process_c__IteratorB.callbackObject = callbackObject;
		_Process_c__IteratorB.___purchaseResult = purchaseResult;
		_Process_c__IteratorB.___callbackObject = callbackObject;
		return _Process_c__IteratorB;
	}
}
