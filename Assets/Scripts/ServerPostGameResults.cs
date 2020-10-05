using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerPostGameResults
{
	private sealed class _Process_c__Iterator8C : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal ServerGameResults results;

		internal NetServerPostGameResults _net___1;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___2;

		internal ServerPlayerState _resultPlayerState___3;

		internal ServerCharacterState[] _characterState___4;

		internal List<ServerChaoState> _chaoState___5;

		internal ServerPlayCharacterState[] _playCharacterState___6;

		internal ServerPlayCharacterState[] __s_779___7;

		internal int __s_780___8;

		internal ServerPlayCharacterState _playCharaState___9;

		internal ServerMileageMapState _interfaceMileageMapState___10;

		internal List<ServerMessageEntry> _messageEntries___11;

		internal int _resultMessageEntries___12;

		internal int _index___13;

		internal ServerMessageEntry _messageEntry___14;

		internal List<ServerOperatorMessageEntry> _operatorMessageEntries___15;

		internal int _resultOperatorMessageEntries___16;

		internal int _index___17;

		internal ServerOperatorMessageEntry _messageEntry___18;

		internal List<ServerMileageIncentive> _mileageIncentiveList___19;

		internal List<ServerItemState> _dailyIncentiveList___20;

		internal MsgPostGameResultsSucceed _msg___21;

		internal MsgServerConnctFailed _msg___22;

		internal int _PC;

		internal object _current;

		internal ServerGameResults ___results;

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
					goto IL_4F6;
				}
				this._monitor___0.PrepareConnect(ServerSessionWatcher.ValidateType.LOGIN_OR_RELOGIN);
				break;
			case 1u:
				break;
			case 2u:
				goto IL_D5;
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
				goto IL_4F6;
			}
			this._net___1 = new NetServerPostGameResults(this.results);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerPostGameResultsRetry(this.results, this.callbackObject));
			IL_D5:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._playerState___2 = ServerInterface.PlayerState;
				this._resultPlayerState___3 = this._net___1.m_resultPlayerState;
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
				this._playCharacterState___6 = this._net___1.resultPlayCharacterState;
				if (this._playCharacterState___6 != null)
				{
					this._playerState___2.ClearPlayCharacterState();
					this.__s_779___7 = this._playCharacterState___6;
					this.__s_780___8 = 0;
					while (this.__s_780___8 < this.__s_779___7.Length)
					{
						this._playCharaState___9 = this.__s_779___7[this.__s_780___8];
						if (this._playCharaState___9 != null)
						{
							this._playerState___2.SetPlayCharacterState(this._playCharaState___9);
						}
						this.__s_780___8++;
					}
				}
				this._interfaceMileageMapState___10 = ServerInterface.MileageMapState;
				this._net___1.m_resultMileageMapState.CopyTo(this._interfaceMileageMapState___10);
				if (this.results.m_chaoEggPresent && RouletteManager.Instance != null)
				{
					RouletteManager.Instance.specialEgg++;
				}
				if (this._net___1.m_messageEntryList != null)
				{
					this._messageEntries___11 = ServerInterface.MessageList;
					this._messageEntries___11.Clear();
					this._resultMessageEntries___12 = this._net___1.m_totalMessage;
					this._index___13 = 0;
					while (this._index___13 < this._resultMessageEntries___12)
					{
						this._messageEntry___14 = this._net___1.m_messageEntryList[this._index___13];
						this._messageEntries___11.Add(this._messageEntry___14);
						this._index___13++;
					}
				}
				if (this._net___1.m_operatorMessageEntryList != null)
				{
					this._operatorMessageEntries___15 = ServerInterface.OperatorMessageList;
					this._operatorMessageEntries___15.Clear();
					this._resultOperatorMessageEntries___16 = this._net___1.m_totalOperatorMessage;
					this._index___17 = 0;
					while (this._index___17 < this._resultOperatorMessageEntries___16)
					{
						this._messageEntry___18 = this._net___1.m_operatorMessageEntryList[this._index___17];
						this._operatorMessageEntries___15.Add(this._messageEntry___18);
						this._index___17++;
					}
				}
				if (this._net___1.m_resultEventIncentiveList != null)
				{
					EventUtility.SetEventIncentiveListToEventManager(this._net___1.m_resultEventIncentiveList);
				}
				if (this._net___1.m_resultEventState != null)
				{
					EventUtility.SetEventStateToEventManager(this._net___1.m_resultEventState);
				}
				this._mileageIncentiveList___19 = this._net___1.m_resultMileageIncentive;
				this._dailyIncentiveList___20 = this._net___1.m_resultDailyMissionIncentiveList;
				this._msg___21 = new MsgPostGameResultsSucceed();
				this._msg___21.m_playerState = this._playerState___2;
				this._msg___21.m_mileageMapState = this._interfaceMileageMapState___10;
				this._msg___21.m_mileageIncentive = this._mileageIncentiveList___19;
				this._msg___21.m_dailyIncentive = this._dailyIncentiveList___20;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___21, this.callbackObject, "ServerPostGameResults_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerPostGameResults_Succeeded", this._msg___21, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___22 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___22, this.callbackObject, "ServerPostGameResults_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_4F6:
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

	public static IEnumerator Process(ServerGameResults results, GameObject callbackObject)
	{
		ServerPostGameResults._Process_c__Iterator8C _Process_c__Iterator8C = new ServerPostGameResults._Process_c__Iterator8C();
		_Process_c__Iterator8C.results = results;
		_Process_c__Iterator8C.callbackObject = callbackObject;
		_Process_c__Iterator8C.___results = results;
		_Process_c__Iterator8C.___callbackObject = callbackObject;
		return _Process_c__Iterator8C;
	}
}
