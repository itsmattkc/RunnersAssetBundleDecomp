using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerStartAct
{
	private sealed class _Process_c__Iterator8D : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal float _dummyTime___1;

		internal List<ItemType> modifiersItem;

		internal List<BoostItemType> modifiersBoostItem;

		internal List<string> distanceFriendIdList;

		internal bool tutorial;

		internal int? eventId;

		internal NetServerStartAct _net___2;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___3;

		internal ServerPlayerState _resultPlayerState___4;

		internal List<ServerDistanceFriendEntry> _distanceFriendList___5;

		internal List<ServerDistanceFriendEntry> _resultDistanceFriendList___6;

		internal List<ServerDistanceFriendEntry>.Enumerator __s_781___7;

		internal ServerDistanceFriendEntry _entry___8;

		internal MsgActStartSucceed _msg___9;

		internal MsgServerConnctFailed _msg___10;

		internal int _PC;

		internal object _current;

		internal List<ItemType> ___modifiersItem;

		internal List<BoostItemType> ___modifiersBoostItem;

		internal List<string> ___distanceFriendIdList;

		internal bool ___tutorial;

		internal int? ___eventId;

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
					goto IL_304;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				if (Time.realtimeSinceStartup - this._dummyTime___1 >= 1f)
				{
					this._net___2 = new NetServerStartAct(this.modifiersItem, this.modifiersBoostItem, this.distanceFriendIdList, this.tutorial, this.eventId);
					this._net___2.Request();
					this._monitor___0.StartMonitor(new ServerStartActRetry(this.modifiersItem, this.modifiersBoostItem, this.distanceFriendIdList, this.tutorial, this.eventId, this.callbackObject));
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
				goto IL_304;
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
				SaveDataUtil.SetBeforeDailyMissionSaveData(this._net___2.resultPlayerState);
				this._playerState___3 = ServerInterface.PlayerState;
				this._resultPlayerState___4 = this._net___2.resultPlayerState;
				this._resultPlayerState___4.CopyTo(this._playerState___3);
				this._distanceFriendList___5 = ServerInterface.DistanceFriendEntry;
				this._resultDistanceFriendList___6 = this._net___2.resultDistanceFriendEntry;
				this._distanceFriendList___5.Clear();
				this.__s_781___7 = this._resultDistanceFriendList___6.GetEnumerator();
				try
				{
					while (this.__s_781___7.MoveNext())
					{
						this._entry___8 = this.__s_781___7.Current;
						this._distanceFriendList___5.Add(this._entry___8);
					}
				}
				finally
				{
					((IDisposable)this.__s_781___7).Dispose();
				}
				this._msg___9 = new MsgActStartSucceed();
				this._msg___9.m_playerState = this._playerState___3;
				this._msg___9.m_friendDistanceList = this._distanceFriendList___5;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___9, this.callbackObject, "ServerStartAct_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerStartAct_Succeeded", this._msg___9, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___10 = new MsgServerConnctFailed(this._net___2.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___10, this.callbackObject, "ServerStartAct_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_304:
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

	public static IEnumerator Process(List<ItemType> modifiersItem, List<BoostItemType> modifiersBoostItem, List<string> distanceFriendIdList, bool tutorial, int? eventId, GameObject callbackObject)
	{
		ServerStartAct._Process_c__Iterator8D _Process_c__Iterator8D = new ServerStartAct._Process_c__Iterator8D();
		_Process_c__Iterator8D.modifiersItem = modifiersItem;
		_Process_c__Iterator8D.modifiersBoostItem = modifiersBoostItem;
		_Process_c__Iterator8D.distanceFriendIdList = distanceFriendIdList;
		_Process_c__Iterator8D.tutorial = tutorial;
		_Process_c__Iterator8D.eventId = eventId;
		_Process_c__Iterator8D.callbackObject = callbackObject;
		_Process_c__Iterator8D.___modifiersItem = modifiersItem;
		_Process_c__Iterator8D.___modifiersBoostItem = modifiersBoostItem;
		_Process_c__Iterator8D.___distanceFriendIdList = distanceFriendIdList;
		_Process_c__Iterator8D.___tutorial = tutorial;
		_Process_c__Iterator8D.___eventId = eventId;
		_Process_c__Iterator8D.___callbackObject = callbackObject;
		return _Process_c__Iterator8D;
	}
}
