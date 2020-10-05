using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerCommitWheelSpinGeneral
{
	private sealed class _Process_c__IteratorAA : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int eventId;

		internal int spinId;

		internal int spinCostItemId;

		internal int spinNum;

		internal NetServerCommitWheelSpinGeneral _net___1;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___2;

		internal ServerPlayerState _resultPlayerState___3;

		internal ServerCharacterState[] _characterState___4;

		internal List<ServerChaoState> _chaoState___5;

		internal MsgCommitWheelSpinGeneralSucceed _msg___6;

		internal MsgServerConnctFailed _msg___7;

		internal int _PC;

		internal object _current;

		internal int ___eventId;

		internal int ___spinId;

		internal int ___spinCostItemId;

		internal int ___spinNum;

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
					goto IL_2F3;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_FE;
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
				goto IL_2F3;
			}
			this._net___1 = new NetServerCommitWheelSpinGeneral(this.eventId, this.spinId, this.spinCostItemId, this.spinNum);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerCommitWheelSpinGeneralRetry(this.eventId, this.spinId, this.spinCostItemId, this.spinNum, this.callbackObject), -1f, HudNetworkConnect.DisplayType.ONLY_ICON);
			IL_FE:
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
				this._chaoState___5 = this._net___1.resultChaoState;
				if (this._chaoState___5 != null)
				{
					this._playerState___2.SetChaoState(this._chaoState___5);
				}
				this._msg___6 = new MsgCommitWheelSpinGeneralSucceed();
				this._msg___6.m_playerState = this._playerState___2;
				this._msg___6.m_wheelOptionsGeneral = this._net___1.resultWheelOptionsGen;
				this._msg___6.m_resultSpinResultGeneral = this._net___1.resultWheelResultGen;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerCommitWheelSpinGeneral_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerCommitWheelSpinGeneral_Succeeded", this._msg___6, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___7 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._net___1.resultStCd == ServerInterface.StatusCode.RouletteBoardReset)
				{
					if (this.callbackObject != null)
					{
						this.callbackObject.SendMessage("ServerCommitWheelSpinGeneral_Failed", this._msg___7, SendMessageOptions.DontRequireReceiver);
					}
				}
				else if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___7, this.callbackObject, "ServerCommitWheelSpinGeneral_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_2F3:
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

	private const string SuccessEvent = "ServerCommitWheelSpinGeneral_Succeeded";

	private const string FailEvent = "ServerCommitWheelSpinGeneral_Failed";

	public static IEnumerator Process(int eventId, int spinId, int spinCostItemId, int spinNum, GameObject callbackObject)
	{
		ServerCommitWheelSpinGeneral._Process_c__IteratorAA _Process_c__IteratorAA = new ServerCommitWheelSpinGeneral._Process_c__IteratorAA();
		_Process_c__IteratorAA.eventId = eventId;
		_Process_c__IteratorAA.spinId = spinId;
		_Process_c__IteratorAA.spinCostItemId = spinCostItemId;
		_Process_c__IteratorAA.spinNum = spinNum;
		_Process_c__IteratorAA.callbackObject = callbackObject;
		_Process_c__IteratorAA.___eventId = eventId;
		_Process_c__IteratorAA.___spinId = spinId;
		_Process_c__IteratorAA.___spinCostItemId = spinCostItemId;
		_Process_c__IteratorAA.___spinNum = spinNum;
		_Process_c__IteratorAA.___callbackObject = callbackObject;
		return _Process_c__IteratorAA;
	}
}
