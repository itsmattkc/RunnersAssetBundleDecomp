using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerRetrievePlayerState
{
	private sealed class _Process_c__IteratorA4 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerRetrievePlayerState _net___1;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___2;

		internal ServerPlayerState _resultPlayerState___3;

		internal MsgGetPlayerStateSucceed _msg___4;

		internal MsgServerConnctFailed _msg___5;

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
					goto IL_1EA;
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
				goto IL_1EA;
			}
			this._net___1 = new NetServerRetrievePlayerState();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerRetrievePlayerStateRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._playerState___2 = ServerInterface.PlayerState;
				this._resultPlayerState___3 = this._net___1.resultPlayerState;
				this._resultPlayerState___3.CopyTo(this._playerState___2);
				this._msg___4 = new MsgGetPlayerStateSucceed();
				this._msg___4.m_playerState = this._playerState___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___4, this.callbackObject, "ServerRetrievePlayerState_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerRetrievePlayerState_Succeeded", this._msg___4, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___5 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerRetrievePlayerState_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_1EA:
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
		ServerRetrievePlayerState._Process_c__IteratorA4 _Process_c__IteratorA = new ServerRetrievePlayerState._Process_c__IteratorA4();
		_Process_c__IteratorA.callbackObject = callbackObject;
		_Process_c__IteratorA.___callbackObject = callbackObject;
		return _Process_c__IteratorA;
	}
}
