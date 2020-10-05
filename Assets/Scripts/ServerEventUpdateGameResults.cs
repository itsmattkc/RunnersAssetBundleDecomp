using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerEventUpdateGameResults
{
	private sealed class _Process_c__Iterator71 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal ServerEventGameResults results;

		internal NetServerEventUpdateGameResults _net___1;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___2;

		internal ServerPlayerState _resultPlayerState___3;

		internal ServerPlayCharacterState[] _playCharacterState___4;

		internal ServerPlayCharacterState[] __s_769___5;

		internal int __s_770___6;

		internal ServerPlayCharacterState _playCharaState___7;

		internal ServerWheelOptions _wheelOptions___8;

		internal ServerWheelOptions _resultWheelOptions___9;

		internal List<ServerMessageEntry> _messageEntries___10;

		internal List<ServerMessageEntry> _serverMessageEntries___11;

		internal int _index___12;

		internal ServerMessageEntry _messageEntry___13;

		internal List<ServerOperatorMessageEntry> _operatorMessageEntries___14;

		internal List<ServerOperatorMessageEntry> _serverOperatorMessageEntries___15;

		internal int _index___16;

		internal ServerOperatorMessageEntry _messageEntry___17;

		internal EventManager _eventManager___18;

		internal MsgEventUpdateGameResultsSucceed _msg___19;

		internal MsgServerConnctFailed _msg___20;

		internal int _PC;

		internal object _current;

		internal ServerEventGameResults ___results;

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
					goto IL_438;
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
				goto IL_438;
			}
			this._net___1 = new NetServerEventUpdateGameResults(this.results);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerEventUpdateGameResultsRetry(this.results, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._playerState___2 = ServerInterface.PlayerState;
				this._resultPlayerState___3 = this._net___1.PlayerState;
				if (this._resultPlayerState___3 != null)
				{
					this._resultPlayerState___3.CopyTo(this._playerState___2);
				}
				this._playCharacterState___4 = this._net___1.PlayerCharacterState;
				if (this._playCharacterState___4 != null)
				{
					this._playerState___2.ClearPlayCharacterState();
					this.__s_769___5 = this._playCharacterState___4;
					this.__s_770___6 = 0;
					while (this.__s_770___6 < this.__s_769___5.Length)
					{
						this._playCharaState___7 = this.__s_769___5[this.__s_770___6];
						if (this._playCharaState___7 != null)
						{
							this._playerState___2.SetPlayCharacterState(this._playCharaState___7);
						}
						this.__s_770___6++;
					}
				}
				this._wheelOptions___8 = ServerInterface.WheelOptions;
				this._resultWheelOptions___9 = this._net___1.WheelOptions;
				if (this._resultWheelOptions___9 != null)
				{
					this._resultWheelOptions___9.CopyTo(this._wheelOptions___8);
				}
				this._messageEntries___10 = this._net___1.MessageEntryList;
				if (this._messageEntries___10 != null)
				{
					this._serverMessageEntries___11 = ServerInterface.MessageList;
					this._serverMessageEntries___11.Clear();
					this._index___12 = 0;
					while (this._index___12 < this._messageEntries___10.Count)
					{
						this._messageEntry___13 = this._messageEntries___10[this._index___12];
						this._serverMessageEntries___11.Add(this._messageEntry___13);
						this._index___12++;
					}
				}
				this._operatorMessageEntries___14 = this._net___1.OperatorMessageEntryList;
				if (this._operatorMessageEntries___14 != null)
				{
					this._serverOperatorMessageEntries___15 = ServerInterface.OperatorMessageList;
					this._serverOperatorMessageEntries___15.Clear();
					this._index___16 = 0;
					while (this._index___16 < this._operatorMessageEntries___14.Count)
					{
						this._messageEntry___17 = this._operatorMessageEntries___14[this._index___16];
						this._serverOperatorMessageEntries___15.Add(this._messageEntry___17);
						this._index___16++;
					}
				}
				EventUtility.SetEventIncentiveListToEventManager(this._net___1.EventIncentiveList);
				EventUtility.SetEventStateToEventManager(this._net___1.EventState);
				this._eventManager___18 = EventManager.Instance;
				if (this._eventManager___18 != null)
				{
					this._eventManager___18.RaidBossBonus = this._net___1.RaidBossBonus;
				}
				this._msg___19 = new MsgEventUpdateGameResultsSucceed();
				this._msg___19.m_bonus = this._net___1.RaidBossBonus;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___19, this.callbackObject, "ServerUpdateGameResults_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerUpdateGameResults_Succeeded", this._msg___19, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___20 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___20, this.callbackObject, "ServerPostGameResults_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_438:
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

	public static IEnumerator Process(ServerEventGameResults results, GameObject callbackObject)
	{
		ServerEventUpdateGameResults._Process_c__Iterator71 _Process_c__Iterator = new ServerEventUpdateGameResults._Process_c__Iterator71();
		_Process_c__Iterator.results = results;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___results = results;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
