using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerRequestEnergy
{
	private sealed class _Process_c__Iterator7B : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal string friendId;

		internal NetServerRequestEnergy _net___1;

		internal GameObject callbackObject;

		internal long _resultExpireTime___2;

		internal ServerPlayerState _playerState___3;

		internal ServerPlayerState _resultPlayerState___4;

		internal MsgRequestEnergySucceed _msg___5;

		internal MsgServerConnctFailed _msg___6;

		internal int _PC;

		internal object _current;

		internal string ___friendId;

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
					goto IL_218;
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
				goto IL_218;
			}
			this._net___1 = new NetServerRequestEnergy(this.friendId);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerRequestEnergyRetry(this.friendId, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._resultExpireTime___2 = this._net___1.resultExpireTime;
				this._playerState___3 = ServerInterface.PlayerState;
				this._resultPlayerState___4 = this._net___1.resultPlayerState;
				this._resultPlayerState___4.CopyTo(this._playerState___3);
				this._msg___5 = new MsgRequestEnergySucceed();
				this._msg___5.m_playerState = this._playerState___3;
				this._msg___5.m_resultExpireTime = this._resultExpireTime___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerRequestEnergy_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerRequestEnergy_Succeeded", this._msg___5, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___6 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerRequestEnergy_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_218:
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

	public static IEnumerator Process(string friendId, GameObject callbackObject)
	{
		ServerRequestEnergy._Process_c__Iterator7B _Process_c__Iterator7B = new ServerRequestEnergy._Process_c__Iterator7B();
		_Process_c__Iterator7B.friendId = friendId;
		_Process_c__Iterator7B.callbackObject = callbackObject;
		_Process_c__Iterator7B.___friendId = friendId;
		_Process_c__Iterator7B.___callbackObject = callbackObject;
		return _Process_c__Iterator7B;
	}
}
