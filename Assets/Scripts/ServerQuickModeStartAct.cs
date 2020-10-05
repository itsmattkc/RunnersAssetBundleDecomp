using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerQuickModeStartAct
{
	private sealed class _Process_c__Iterator80 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal float _dummyTime___1;

		internal List<ItemType> modifiersItem;

		internal List<BoostItemType> modifiersBoostItem;

		internal bool tutorial;

		internal NetServerQuickModeStartAct _net___2;

		internal GameObject callbackObject;

		internal ServerPlayerState _playerState___3;

		internal ServerPlayerState _resultPlayerState___4;

		internal MsgQuickModeActStartSucceed _msg___5;

		internal MsgServerConnctFailed _msg___6;

		internal int _PC;

		internal object _current;

		internal List<ItemType> ___modifiersItem;

		internal List<BoostItemType> ___modifiersBoostItem;

		internal bool ___tutorial;

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
					goto IL_256;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				if (Time.realtimeSinceStartup - this._dummyTime___1 >= 1f)
				{
					this._net___2 = new NetServerQuickModeStartAct(this.modifiersItem, this.modifiersBoostItem, this.tutorial);
					this._net___2.Request();
					this._monitor___0.StartMonitor(new ServerQuickModeStartActRetry(this.modifiersItem, this.modifiersBoostItem, this.tutorial, this.callbackObject));
					goto IL_124;
				}
				goto IL_93;
			case 3u:
				goto IL_124;
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
				goto IL_256;
			}
			this._dummyTime___1 = Time.realtimeSinceStartup;
			IL_93:
			this._current = null;
			this._PC = 2;
			return true;
			IL_124:
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
				this._msg___5 = new MsgQuickModeActStartSucceed();
				this._msg___5.m_playerState = this._playerState___3;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerQuickModeStartAct_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerQuickModeStartAct_Succeeded", this._msg___5, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___6 = new MsgServerConnctFailed(this._net___2.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerQuickModeStartAct_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_256:
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

	public static IEnumerator Process(List<ItemType> modifiersItem, List<BoostItemType> modifiersBoostItem, bool tutorial, GameObject callbackObject)
	{
		ServerQuickModeStartAct._Process_c__Iterator80 _Process_c__Iterator = new ServerQuickModeStartAct._Process_c__Iterator80();
		_Process_c__Iterator.modifiersItem = modifiersItem;
		_Process_c__Iterator.modifiersBoostItem = modifiersBoostItem;
		_Process_c__Iterator.tutorial = tutorial;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___modifiersItem = modifiersItem;
		_Process_c__Iterator.___modifiersBoostItem = modifiersBoostItem;
		_Process_c__Iterator.___tutorial = tutorial;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
