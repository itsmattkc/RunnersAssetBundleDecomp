using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerEventStartAct
{
	private sealed class _Process_c__Iterator70 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal float _dummyTime___1;

		internal int eventId;

		internal int energyExpend;

		internal long raidBossId;

		internal List<ItemType> modifiersItem;

		internal List<BoostItemType> modifiersBoostItem;

		internal NetServerEventStartAct _net___2;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___3;

		internal ServerPlayerState _resultPlayerState___4;

		internal ServerEventUserRaidBossState _userRaidBossState___5;

		internal MsgEventActStartSucceed _msg___6;

		internal MsgServerConnctFailed _msg___7;

		internal int _PC;

		internal object _current;

		internal int ___eventId;

		internal int ___energyExpend;

		internal long ___raidBossId;

		internal List<ItemType> ___modifiersItem;

		internal List<BoostItemType> ___modifiersBoostItem;

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
					goto IL_2AA;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				if (Time.realtimeSinceStartup - this._dummyTime___1 >= 1f)
				{
					this._net___2 = new NetServerEventStartAct(this.eventId, this.energyExpend, this.raidBossId, this.modifiersItem, this.modifiersBoostItem);
					this._net___2.Request();
					this._monitor___0.StartMonitor(new ServerEventStartActRetry(this.eventId, this.energyExpend, this.raidBossId, this.modifiersItem, this.modifiersBoostItem, this.callbackObject));
					goto IL_13C;
				}
				goto IL_93;
			case 3u:
				goto IL_13C;
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
				goto IL_2AA;
			}
			this._dummyTime___1 = Time.realtimeSinceStartup;
			IL_93:
			this._current = null;
			this._PC = 2;
			return true;
			IL_13C:
			if (this._net___2.IsExecuting())
			{
				this._current = null;
				this._PC = 3;
				return true;
			}
			if (this._net___2.IsSucceeded())
			{
				this._playerState___3 = ServerInterface.PlayerState;
				this._resultPlayerState___4 = this._net___2.resultPlayerState;
				this._resultPlayerState___4.CopyTo(this._playerState___3);
				if (EventManager.Instance != null)
				{
					this._userRaidBossState___5 = this._net___2.userRaidBossState;
					EventManager.Instance.SynchServerEventUserRaidBossState(this._userRaidBossState___5);
				}
				GeneralUtil.SetItemCount(ServerItem.Id.RAIDRING, (long)this._net___2.userRaidBossState.NumRaidbossRings);
				this._msg___6 = new MsgEventActStartSucceed();
				this._msg___6.m_playerState = this._playerState___3;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerEventStartAct_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerEventStartAct_Succeeded", this._msg___6, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___7 = new MsgServerConnctFailed(this._net___2.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___7, this.callbackObject, "ServerEventStartAct_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_2AA:
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

	public static IEnumerator Process(int eventId, int energyExpend, long raidBossId, List<ItemType> modifiersItem, List<BoostItemType> modifiersBoostItem, GameObject callbackObject)
	{
		ServerEventStartAct._Process_c__Iterator70 _Process_c__Iterator = new ServerEventStartAct._Process_c__Iterator70();
		_Process_c__Iterator.eventId = eventId;
		_Process_c__Iterator.energyExpend = energyExpend;
		_Process_c__Iterator.raidBossId = raidBossId;
		_Process_c__Iterator.modifiersItem = modifiersItem;
		_Process_c__Iterator.modifiersBoostItem = modifiersBoostItem;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___eventId = eventId;
		_Process_c__Iterator.___energyExpend = energyExpend;
		_Process_c__Iterator.___raidBossId = raidBossId;
		_Process_c__Iterator.___modifiersItem = modifiersItem;
		_Process_c__Iterator.___modifiersBoostItem = modifiersBoostItem;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
