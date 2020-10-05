using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerQuickModePostGameResults
{
	private sealed class _Process_c__Iterator7F : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal ServerQuickModeGameResults results;

		internal NetServerQuickModePostGameResults _net___1;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___2;

		internal ServerPlayerState _resultPlayerState___3;

		internal ServerCharacterState[] _characterState___4;

		internal List<ServerChaoState> _chaoState___5;

		internal ServerPlayCharacterState[] _playCharacterState___6;

		internal ServerPlayCharacterState[] __s_773___7;

		internal int __s_774___8;

		internal ServerPlayCharacterState _playCharaState___9;

		internal List<ServerMessageEntry> _messageEntries___10;

		internal int _resultMessageEntries___11;

		internal int _index___12;

		internal ServerMessageEntry _messageEntry___13;

		internal List<ServerOperatorMessageEntry> _operatorMessageEntries___14;

		internal int _resultOperatorMessageEntries___15;

		internal int _index___16;

		internal ServerOperatorMessageEntry _messageEntry___17;

		internal List<ServerItemState> _dailyIncentiveList___18;

		internal MsgQuickModePostGameResultsSucceed _msg___19;

		internal MsgServerConnctFailed _msg___20;

		internal int _PC;

		internal object _current;

		internal ServerQuickModeGameResults ___results;

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
					goto IL_430;
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
				goto IL_430;
			}
			this._net___1 = new NetServerQuickModePostGameResults(this.results);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerQuickModePostGameResultsRetry(this.results, this.callbackObject));
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
				this._characterState___4 = this._net___1.m_resultCharacterState;
				if (this._characterState___4 != null)
				{
					this._playerState___2.SetCharacterState(this._characterState___4);
				}
				this._chaoState___5 = this._net___1.m_resultChaoState;
				if (this._chaoState___5 != null)
				{
					this._playerState___2.SetChaoState(this._chaoState___5);
				}
				this._playCharacterState___6 = this._net___1.m_resultPlayCharacterState;
				if (this._playCharacterState___6 != null)
				{
					this._playerState___2.ClearPlayCharacterState();
					this.__s_773___7 = this._playCharacterState___6;
					this.__s_774___8 = 0;
					while (this.__s_774___8 < this.__s_773___7.Length)
					{
						this._playCharaState___9 = this.__s_773___7[this.__s_774___8];
						if (this._playCharaState___9 != null)
						{
							this._playerState___2.SetPlayCharacterState(this._playCharaState___9);
						}
						this.__s_774___8++;
					}
				}
				if (this._net___1.m_messageEntryList != null)
				{
					this._messageEntries___10 = ServerInterface.MessageList;
					this._messageEntries___10.Clear();
					this._resultMessageEntries___11 = this._net___1.totalMessage;
					this._index___12 = 0;
					while (this._index___12 < this._resultMessageEntries___11)
					{
						this._messageEntry___13 = this._net___1.m_messageEntryList[this._index___12];
						this._messageEntries___10.Add(this._messageEntry___13);
						this._index___12++;
					}
				}
				if (this._net___1.m_operatorMessageEntryList != null)
				{
					this._operatorMessageEntries___14 = ServerInterface.OperatorMessageList;
					this._operatorMessageEntries___14.Clear();
					this._resultOperatorMessageEntries___15 = this._net___1.totalOperatorMessage;
					this._index___16 = 0;
					while (this._index___16 < this._resultOperatorMessageEntries___15)
					{
						this._messageEntry___17 = this._net___1.m_operatorMessageEntryList[this._index___16];
						this._operatorMessageEntries___14.Add(this._messageEntry___17);
						this._index___16++;
					}
				}
				this._dailyIncentiveList___18 = this._net___1.m_dailyMissionIncentiveList;
				this._msg___19 = new MsgQuickModePostGameResultsSucceed();
				this._msg___19.m_playerState = this._playerState___2;
				this._msg___19.m_dailyIncentive = this._dailyIncentiveList___18;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___19, this.callbackObject, "ServerQuickModePostGameResults_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerQuickModePostGameResults_Succeeded", this._msg___19, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___20 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___20, this.callbackObject, "ServerQuickModePostGameResults_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_430:
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

	public static IEnumerator Process(ServerQuickModeGameResults results, GameObject callbackObject)
	{
		ServerQuickModePostGameResults._Process_c__Iterator7F _Process_c__Iterator7F = new ServerQuickModePostGameResults._Process_c__Iterator7F();
		_Process_c__Iterator7F.results = results;
		_Process_c__Iterator7F.callbackObject = callbackObject;
		_Process_c__Iterator7F.___results = results;
		_Process_c__Iterator7F.___callbackObject = callbackObject;
		return _Process_c__Iterator7F;
	}
}
