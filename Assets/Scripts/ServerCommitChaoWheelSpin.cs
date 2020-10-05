using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerCommitChaoWheelSpin
{
	private sealed class _Process_c__Iterator5E : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int count;

		internal NetServerCommitChaoWheelSpin _net___1;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___2;

		internal ServerPlayerState _resultPlayerState___3;

		internal ServerCharacterState[] _resultCharacterState___4;

		internal List<ServerChaoState> _resultChaoState___5;

		internal ServerChaoWheelOptions _chaoWheelOptions___6;

		internal ServerChaoWheelOptions _resultChaoWheelOptions___7;

		internal ServerSpinResultGeneral _resultSpinResultGeneral___8;

		internal MsgCommitChaoWheelSpicSucceed _msg___9;

		internal MsgServerConnctFailed _msg___10;

		internal int _PC;

		internal object _current;

		internal int ___count;

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
					goto IL_2E8;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_DA;
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
				goto IL_2E8;
			}
			this._net___1 = new NetServerCommitChaoWheelSpin(this.count);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerCommitChaoWheelSpinRetry(this.count, this.callbackObject), -1f, HudNetworkConnect.DisplayType.ONLY_ICON);
			IL_DA:
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
				this._resultChaoState___5 = this._net___1.resultChaoState;
				if (this._resultChaoState___5 != null)
				{
					this._playerState___2.SetChaoState(this._resultChaoState___5);
				}
				this._chaoWheelOptions___6 = ServerInterface.ChaoWheelOptions;
				this._resultChaoWheelOptions___7 = this._net___1.resultChaoWheelOptions;
				this._resultChaoWheelOptions___7.CopyTo(this._chaoWheelOptions___6);
				this._resultSpinResultGeneral___8 = null;
				if (this._net___1.resultSpinResultGeneral != null)
				{
					this._resultSpinResultGeneral___8 = new ServerSpinResultGeneral();
					this._net___1.resultSpinResultGeneral.CopyTo(this._resultSpinResultGeneral___8);
				}
				this._msg___9 = new MsgCommitChaoWheelSpicSucceed();
				this._msg___9.m_playerState = this._playerState___2;
				this._msg___9.m_chaoWheelOptions = this._chaoWheelOptions___6;
				this._msg___9.m_resultSpinResultGeneral = this._resultSpinResultGeneral___8;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___9, this.callbackObject, "ServerCommitChaoWheelSpin_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerCommitChaoWheelSpin_Succeeded", this._msg___9, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___10 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___10, this.callbackObject, "ServerCommitChaoWheelSpin_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_2E8:
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

	public static IEnumerator Process(int count, GameObject callbackObject)
	{
		ServerCommitChaoWheelSpin._Process_c__Iterator5E _Process_c__Iterator5E = new ServerCommitChaoWheelSpin._Process_c__Iterator5E();
		_Process_c__Iterator5E.count = count;
		_Process_c__Iterator5E.callbackObject = callbackObject;
		_Process_c__Iterator5E.___count = count;
		_Process_c__Iterator5E.___callbackObject = callbackObject;
		return _Process_c__Iterator5E;
	}
}
