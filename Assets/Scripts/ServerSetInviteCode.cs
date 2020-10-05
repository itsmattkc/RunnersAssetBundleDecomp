using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerSetInviteCode
{
	private sealed class _Process_c__Iterator7D : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal string friendId;

		internal NetServerSetInviteCode _net___1;

		internal GameObject callbackObject;

		internal List<ServerPresentState> _incentive___2;

		internal ServerPlayerState _playerState___3;

		internal ServerPlayerState _resultPlayerState___4;

		internal MsgGetNormalIncentiveSucceed _msg___5;

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
			this._net___1 = new NetServerSetInviteCode(this.friendId);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerSetInviteCodeRetry(this.friendId, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._incentive___2 = this._net___1.incentive;
				this._playerState___3 = ServerInterface.PlayerState;
				this._resultPlayerState___4 = this._net___1.playerState;
				this._resultPlayerState___4.CopyTo(this._playerState___3);
				this._msg___5 = new MsgGetNormalIncentiveSucceed();
				this._msg___5.m_playerState = this._playerState___3;
				this._msg___5.m_incentive = this._incentive___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerSetInviteCode_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerSetInviteCode_Succeeded", this._msg___5, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___6 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerSetInviteCode_Failed");
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
		ServerSetInviteCode._Process_c__Iterator7D _Process_c__Iterator7D = new ServerSetInviteCode._Process_c__Iterator7D();
		_Process_c__Iterator7D.friendId = friendId;
		_Process_c__Iterator7D.callbackObject = callbackObject;
		_Process_c__Iterator7D.___friendId = friendId;
		_Process_c__Iterator7D.___callbackObject = callbackObject;
		return _Process_c__Iterator7D;
	}
}
