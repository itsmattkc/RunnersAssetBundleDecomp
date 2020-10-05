using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetFacebookIncentive
{
	public enum IncentiveType
	{
		LOGIN,
		REVIEW,
		FEED,
		ACHIEVEMENT,
		PUSH_NOLOGIN
	}

	private sealed class _Process_c__Iterator79 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int incentiveType;

		internal int achievementCount;

		internal NetServerGetFacebookIncentive _net___1;

		internal GameObject callbackObject;

		internal List<ServerPresentState> _incentive___2;

		internal ServerPlayerState _playerState___3;

		internal ServerPlayerState _resultPlayerState___4;

		internal MsgGetNormalIncentiveSucceed _msg___5;

		internal MsgServerConnctFailed _msg___6;

		internal int _PC;

		internal object _current;

		internal int ___incentiveType;

		internal int ___achievementCount;

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
					goto IL_224;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_E0;
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
				goto IL_224;
			}
			this._net___1 = new NetServerGetFacebookIncentive(this.incentiveType, this.achievementCount);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetFacebookIncentiveRetry(this.incentiveType, this.achievementCount, this.callbackObject));
			IL_E0:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._incentive___2 = this._net___1.incentive;
				this._playerState___3 = ServerInterface.PlayerState;
				this._resultPlayerState___4 = this._net___1.playerState;
				this._resultPlayerState___4.CopyTo(this._playerState___3);
				this._msg___5 = new MsgGetNormalIncentiveSucceed();
				this._msg___5.m_playerState = this._playerState___3;
				this._msg___5.m_incentive = this._incentive___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___5, this.callbackObject, "ServerGetFacebookIncentive_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetFacebookIncentive_Succeeded", this._msg___5, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___6 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___6, this.callbackObject, "ServerGetFacebookIncentive_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_224:
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

	public static IEnumerator Process(int incentiveType, int achievementCount, GameObject callbackObject)
	{
		ServerGetFacebookIncentive._Process_c__Iterator79 _Process_c__Iterator = new ServerGetFacebookIncentive._Process_c__Iterator79();
		_Process_c__Iterator.incentiveType = incentiveType;
		_Process_c__Iterator.achievementCount = achievementCount;
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___incentiveType = incentiveType;
		_Process_c__Iterator.___achievementCount = achievementCount;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
