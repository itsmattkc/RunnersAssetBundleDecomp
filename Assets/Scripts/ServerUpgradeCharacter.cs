using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerUpgradeCharacter
{
	private sealed class _Process_c__Iterator66 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int characterId;

		internal int abilityId;

		internal NetServerUpgradeCharacter _net___1;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___2;

		internal ServerPlayerState _resultPlayerState___3;

		internal ServerCharacterState[] _resultCharacterState___4;

		internal MsgGetPlayerStateSucceed _msg___5;

		internal MsgServerConnctFailed _msg___6;

		internal int _PC;

		internal object _current;

		internal int ___characterId;

		internal int ___abilityId;

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
					goto IL_250;
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
				goto IL_250;
			}
			this._net___1 = new NetServerUpgradeCharacter(this.characterId, this.abilityId);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerUpgradeCharacterRetry(this.characterId, this.abilityId, this.callbackObject));
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
				this._resultCharacterState___4 = this._net___1.resultCharacterState;
				if (this._resultCharacterState___4 != null)
				{
					this._playerState___2.SetCharacterState(this._resultCharacterState___4);
				}
				this._msg___5 = new MsgGetPlayerStateSucceed();
				this._msg___5.m_playerState = this._playerState___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerUpgradeCharacter_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerUpgradeCharacter_Succeeded", this._msg___5, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___6 = new MsgServerConnctFailed(this._net___1.resultStCd);
				this._msg___6.m_status = this._net___1.resultStCd;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerUpgradeCharacter_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_250:
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

	public static IEnumerator Process(int characterId, int abilityId, GameObject callbackObject)
	{
		ServerUpgradeCharacter._Process_c__Iterator66 _Process_c__Iterator = new ServerUpgradeCharacter._Process_c__Iterator66();
		_Process_c__Iterator.characterId = characterId;
		_Process_c__Iterator.abilityId = abilityId;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___characterId = characterId;
		_Process_c__Iterator.___abilityId = abilityId;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
