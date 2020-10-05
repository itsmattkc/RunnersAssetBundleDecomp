using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetMileageData
{
	private sealed class _Process_c__Iterator89 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal string[] distanceFriendList;

		internal NetServerGetMileageData _net___1;

		internal GameObject callbackObject;

		internal ServerMileageMapState _mileageMapState___2;

		internal ServerMileageMapState _resultMileageMapState___3;

		internal List<ServerMileageFriendEntry> _mileageFriendList___4;

		internal List<ServerMileageFriendEntry> _resultMileageFriendList___5;

		internal int _index___6;

		internal ServerMileageFriendEntry _e___7;

		internal MsgGetPlayerStateSucceed _msg___8;

		internal MsgServerConnctFailed _msg___9;

		internal int _PC;

		internal object _current;

		internal string[] ___distanceFriendList;

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
					goto IL_285;
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
				goto IL_285;
			}
			this._net___1 = new NetServerGetMileageData(this.distanceFriendList);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetMileageDataRetry(this.distanceFriendList, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._mileageMapState___2 = ServerInterface.MileageMapState;
				this._resultMileageMapState___3 = ServerGetMileageData.MakeResultMileageState(this._net___1);
				if (this._resultMileageMapState___3 != null && this._mileageMapState___2 != null)
				{
					this._resultMileageMapState___3.CopyTo(this._mileageMapState___2);
				}
				this._mileageFriendList___4 = ServerInterface.MileageFriendList;
				this._resultMileageFriendList___5 = this._net___1.m_resultMileageFriendList;
				this._mileageFriendList___4.Clear();
				this._index___6 = 0;
				while (this._index___6 < this._resultMileageFriendList___5.Count)
				{
					this._e___7 = new ServerMileageFriendEntry();
					this._resultMileageFriendList___5[this._index___6].CopyTo(this._e___7);
					this._mileageFriendList___4.Add(this._e___7);
					this._index___6++;
				}
				this._msg___8 = new MsgGetPlayerStateSucceed();
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___8, this.callbackObject, "ServerGetMileageData_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetMileageData_Succeeded", null, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___9 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___9, this.callbackObject, "ServerGetMileageData_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_285:
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

	public static IEnumerator Process(string[] distanceFriendList, GameObject callbackObject)
	{
		ServerGetMileageData._Process_c__Iterator89 _Process_c__Iterator = new ServerGetMileageData._Process_c__Iterator89();
		_Process_c__Iterator.distanceFriendList = distanceFriendList;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___distanceFriendList = distanceFriendList;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}

	private static ServerMileageMapState MakeResultMileageState(NetServerGetMileageData net)
	{
		ServerMileageMapState serverMileageMapState = new ServerMileageMapState();
		net.resultMileageMapState.CopyTo(serverMileageMapState);
		return serverMileageMapState;
	}
}
