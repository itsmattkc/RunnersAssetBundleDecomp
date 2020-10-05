using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetChaoRentalStates
{
	private sealed class _Process_c__Iterator60 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal string[] friendId;

		internal NetServerGetRentalState _net___1;

		internal GameObject callbackObject;

		internal int _resultChaoRentalStates___2;

		internal List<ServerChaoRentalState> _resultChaoRentalStateList___3;

		internal int _i___4;

		internal ServerChaoRentalState _chaoRentalState___5;

		internal MsgGetFriendChaoStateSucceed _msg___6;

		internal MsgServerConnctFailed _msg___7;

		internal int _PC;

		internal object _current;

		internal string[] ___friendId;

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
					goto IL_23E;
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
				goto IL_23E;
			}
			this._net___1 = new NetServerGetRentalState(this.friendId);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetChaoRentalStatesRetry(this.friendId, this.callbackObject));
			IL_D4:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._resultChaoRentalStates___2 = this._net___1.resultStates;
				this._resultChaoRentalStateList___3 = new List<ServerChaoRentalState>(this._resultChaoRentalStates___2);
				this._i___4 = 0;
				while (this._i___4 < this._resultChaoRentalStates___2)
				{
					this._chaoRentalState___5 = this._net___1.GetResultChaoRentalState(this._i___4);
					this._resultChaoRentalStateList___3.Add(this._chaoRentalState___5);
					this._i___4++;
				}
				this._msg___6 = new MsgGetFriendChaoStateSucceed();
				this._msg___6.m_chaoRentalStates = this._resultChaoRentalStateList___3;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerGetChaoRentalStates_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetChaoRentalStates_Succeeded", this._msg___6, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___7 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___7, this.callbackObject, "ServerGetChaoRentalStates_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_23E:
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

	private const string SuccessEvent = "ServerGetChaoRentalStates_Succeeded";

	private const string FailEvent = "ServerGetChaoRentalStates_Failed";

	public static IEnumerator Process(string[] friendId, GameObject callbackObject)
	{
		ServerGetChaoRentalStates._Process_c__Iterator60 _Process_c__Iterator = new ServerGetChaoRentalStates._Process_c__Iterator60();
		_Process_c__Iterator.friendId = friendId;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___friendId = friendId;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
