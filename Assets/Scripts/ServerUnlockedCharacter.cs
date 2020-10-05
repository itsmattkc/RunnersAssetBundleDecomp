using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerUnlockedCharacter
{
	private sealed class _Process_c__Iterator65 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal CharaType charaType;

		internal ServerItem serverItem;

		internal NetServerUnlockedCharacter _net___1;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___2;

		internal ServerPlayerState _resultPlayerState___3;

		internal ServerCharacterState[] _characterState___4;

		internal MsgGetPlayerStateSucceed _msg___5;

		internal MsgServerConnctFailed _msg___6;

		internal int _PC;

		internal object _current;

		internal CharaType ___charaType;

		internal ServerItem ___serverItem;

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
					goto IL_23A;
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
				goto IL_23A;
			}
			this._net___1 = new NetServerUnlockedCharacter(this.charaType, this.serverItem);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerUnlockedCharacterRetry(this.charaType, this.serverItem, this.callbackObject));
			IL_E0:
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
				if (this._resultPlayerState___3 != null)
				{
					this._resultPlayerState___3.CopyTo(this._playerState___2);
				}
				this._characterState___4 = this._net___1.resultCharacterState;
				if (this._characterState___4 != null)
				{
					this._playerState___2.SetCharacterState(this._characterState___4);
				}
				this._msg___5 = new MsgGetPlayerStateSucceed();
				this._msg___5.m_playerState = this._resultPlayerState___3;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerUnlockedCharacter_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerUnlockedCharacter_Succeeded", this._msg___5, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___6 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerUnlockedCharacter_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_23A:
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

	public static IEnumerator Process(CharaType charaType, ServerItem serverItem, GameObject callbackObject)
	{
		ServerUnlockedCharacter._Process_c__Iterator65 _Process_c__Iterator = new ServerUnlockedCharacter._Process_c__Iterator65();
		_Process_c__Iterator.charaType = charaType;
		_Process_c__Iterator.serverItem = serverItem;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___charaType = charaType;
		_Process_c__Iterator.___serverItem = serverItem;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
