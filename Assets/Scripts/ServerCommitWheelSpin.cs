using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerCommitWheelSpin
{
	private sealed class _Process_c__IteratorA9 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int count;

		internal NetServerCommitWheelSpin _net___1;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___2;

		internal ServerPlayerState _resultPlayerState___3;

		internal ServerSpinResultGeneral _resultSpinResultGeneral___4;

		internal ServerCharacterState[] _characterState___5;

		internal List<ServerChaoState> _chaoState___6;

		internal ServerWheelOptions _wheelOptions___7;

		internal ServerWheelOptions _resultWheelOptions___8;

		internal MsgCommitWheelSpinSucceed _msg___9;

		internal ServerWheelOptionsData _currentWheelData___10;

		internal ServerSpinResultGeneral _res___11;

		internal MsgServerConnctFailed _msg___12;

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
					goto IL_3A3;
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
				goto IL_3A3;
			}
			this._net___1 = new NetServerCommitWheelSpin(this.count);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerCommitWheelSpinRetry(this.count, this.callbackObject), -1f, HudNetworkConnect.DisplayType.ONLY_ICON);
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
				this._resultSpinResultGeneral___4 = null;
				if (this._net___1.resultSpinResultGeneral != null)
				{
					this._resultSpinResultGeneral___4 = new ServerSpinResultGeneral();
					this._net___1.resultSpinResultGeneral.CopyTo(this._resultSpinResultGeneral___4);
				}
				if (this._resultPlayerState___3 != null)
				{
					this._resultPlayerState___3.CopyTo(this._playerState___2);
				}
				this._characterState___5 = this._net___1.resultCharacterState;
				if (this._characterState___5 != null)
				{
					this._playerState___2.SetCharacterState(this._characterState___5);
				}
				this._chaoState___6 = this._net___1.resultChaoState;
				if (this._chaoState___6 != null)
				{
					this._playerState___2.SetChaoState(this._chaoState___6);
				}
				this._wheelOptions___7 = ServerInterface.WheelOptions;
				this._resultWheelOptions___8 = this._net___1.resultWheelOptions;
				this._resultWheelOptions___8.CopyTo(this._wheelOptions___7);
				this._msg___9 = new MsgCommitWheelSpinSucceed();
				this._msg___9.m_playerState = this._playerState___2;
				this._msg___9.m_wheelOptions = this._wheelOptions___7;
				this._msg___9.m_resultSpinResultGeneral = this._resultSpinResultGeneral___4;
				if (this._msg___9.m_resultSpinResultGeneral == null && RouletteManager.Instance != null)
				{
					this._currentWheelData___10 = RouletteManager.Instance.GetRouletteDataOrg(RouletteCategory.ITEM);
					if (this._currentWheelData___10 != null && this._currentWheelData___10.GetOrgRankupData() != null)
					{
						this._res___11 = new ServerSpinResultGeneral(this._wheelOptions___7, this._currentWheelData___10.GetOrgRankupData());
						this._msg___9.m_resultSpinResultGeneral = this._res___11;
					}
				}
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___9, this.callbackObject, "ServerCommitWheelSpin_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerCommitWheelSpin_Succeeded", this._msg___9, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___12 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._net___1.resultStCd == ServerInterface.StatusCode.RouletteBoardReset)
				{
					if (this.callbackObject != null)
					{
						this.callbackObject.SendMessage("ServerCommitWheelSpin_Failed", this._msg___12, SendMessageOptions.DontRequireReceiver);
					}
				}
				else if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___12, this.callbackObject, "ServerCommitWheelSpin_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_3A3:
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

	private const string SuccessEvent = "ServerCommitWheelSpin_Succeeded";

	private const string FailEvent = "ServerCommitWheelSpin_Failed";

	public static IEnumerator Process(int count, GameObject callbackObject)
	{
		ServerCommitWheelSpin._Process_c__IteratorA9 _Process_c__IteratorA = new ServerCommitWheelSpin._Process_c__IteratorA9();
		_Process_c__IteratorA.count = count;
		_Process_c__IteratorA.callbackObject = callbackObject;
		_Process_c__IteratorA.___count = count;
		_Process_c__IteratorA.___callbackObject = callbackObject;
		return _Process_c__IteratorA;
	}
}
