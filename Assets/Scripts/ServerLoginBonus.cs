using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerLoginBonus
{
	private sealed class _Process_c__Iterator9A : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerLoginBonus _net___1;

		internal GameObject callbackObject;

		internal ServerLoginBonusData _loginBonusData___2;

		internal List<ServerLoginBonusReward>.Enumerator __s_787___3;

		internal ServerLoginBonusReward _data___4;

		internal List<ServerLoginBonusReward>.Enumerator __s_788___5;

		internal ServerLoginBonusReward _data___6;

		internal MsgLoginBonusSucceed _msg___7;

		internal MsgServerConnctFailed _msg___8;

		internal int _PC;

		internal object _current;

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
					goto IL_321;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_C8;
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
				goto IL_321;
			}
			this._net___1 = new NetServerLoginBonus();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerLoginBonusRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._loginBonusData___2 = ServerInterface.LoginBonusData;
				this._net___1.loginBonusState.CopyTo(this._loginBonusData___2.m_loginBonusState);
				this._loginBonusData___2.m_startTime = this._net___1.startTime;
				this._loginBonusData___2.m_endTime = this._net___1.endTime;
				this.__s_787___3 = this._net___1.loginBonusRewardList.GetEnumerator();
				try
				{
					while (this.__s_787___3.MoveNext())
					{
						this._data___4 = this.__s_787___3.Current;
						this._loginBonusData___2.m_loginBonusRewardList.Add(this._data___4);
					}
				}
				finally
				{
					((IDisposable)this.__s_787___3).Dispose();
				}
				this.__s_788___5 = this._net___1.firstLoginBonusRewardList.GetEnumerator();
				try
				{
					while (this.__s_788___5.MoveNext())
					{
						this._data___6 = this.__s_788___5.Current;
						this._loginBonusData___2.m_firstLoginBonusRewardList.Add(this._data___6);
					}
				}
				finally
				{
					((IDisposable)this.__s_788___5).Dispose();
				}
				this._loginBonusData___2.m_rewardId = this._net___1.rewardId;
				this._loginBonusData___2.m_rewardDays = this._net___1.rewardDays;
				this._loginBonusData___2.m_firstRewardDays = this._net___1.firstRewardDays;
				this._msg___7 = new MsgLoginBonusSucceed();
				this._msg___7.m_loginBonusData = this._loginBonusData___2;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___7, this.callbackObject, "ServerLoginBonus_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerLoginBonus_Succeeded", this._msg___7, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___8 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___8, this.callbackObject, "ServerLoginBonus_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_321:
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

	public static IEnumerator Process(GameObject callbackObject)
	{
		ServerLoginBonus._Process_c__Iterator9A _Process_c__Iterator9A = new ServerLoginBonus._Process_c__Iterator9A();
		_Process_c__Iterator9A.callbackObject = callbackObject;
		_Process_c__Iterator9A.___callbackObject = callbackObject;
		return _Process_c__Iterator9A;
	}
}
