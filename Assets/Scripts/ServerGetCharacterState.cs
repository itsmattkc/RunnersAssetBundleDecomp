using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetCharacterState
{
	private sealed class _Process_c__IteratorA3 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetCharacterState _net___1;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___2;

		internal ServerCharacterState[] _resultCharacterState___3;

		internal ServerCharacterState[] __s_797___4;

		internal int __s_798___5;

		internal ServerCharacterState _characterState___6;

		internal MsgGetCharacterStateSucceed _msg___7;

		internal MsgServerConnctFailed _msg___8;

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
					goto IL_251;
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
				goto IL_251;
			}
			this._net___1 = new NetServerGetCharacterState();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetCharacterStateRetry(this.callbackObject));
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
				this._resultCharacterState___3 = this._net___1.resultCharacterState;
				this._playerState___2.ClearCharacterState();
				this.__s_797___4 = this._resultCharacterState___3;
				this.__s_798___5 = 0;
				while (this.__s_798___5 < this.__s_797___4.Length)
				{
					this._characterState___6 = this.__s_797___4[this.__s_798___5];
					if (this._characterState___6 != null)
					{
						this._playerState___2.SetCharacterState(this._characterState___6);
					}
					this.__s_798___5++;
				}
				this._msg___7 = new MsgGetCharacterStateSucceed();
				this._msg___7.m_playerState = this._playerState___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___7, this.callbackObject, "ServerGetCharacterState_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetCharacterState_Succeeded", this._msg___7, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___8 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___8, this.callbackObject, "ServerGetCharacterState_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_251:
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
		ServerGetCharacterState._Process_c__IteratorA3 _Process_c__IteratorA = new ServerGetCharacterState._Process_c__IteratorA3();
		_Process_c__IteratorA.callbackObject = callbackObject;
		_Process_c__IteratorA.___callbackObject = callbackObject;
		return _Process_c__IteratorA;
	}
}
