using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerLoginBonusSelect
{
	private sealed class _Process_c__Iterator9B : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int rewardId;

		internal int rewardDays;

		internal int rewardSelect;

		internal int firstRewardDays;

		internal int firstRewardSelect;

		internal NetServerLoginBonusSelect _net___1;

		internal GameObject callbackObject;

		internal ServerLoginBonusData _loginBonusData___2;

		internal MsgLoginBonusSelectSucceed _msg___3;

		internal MsgServerConnctFailed _msg___4;

		internal int _PC;

		internal object _current;

		internal int ___rewardId;

		internal int ___rewardDays;

		internal int ___rewardSelect;

		internal int ___firstRewardDays;

		internal int ___firstRewardSelect;

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
					goto IL_28B;
				}
				this._monitor___0.PrepareConnect();
				break;
			case 1u:
				break;
			case 2u:
				goto IL_104;
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
				goto IL_28B;
			}
			this._net___1 = new NetServerLoginBonusSelect(this.rewardId, this.rewardDays, this.rewardSelect, this.firstRewardDays, this.firstRewardSelect);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerLoginBonusSelectRetry(this.rewardId, this.rewardDays, this.rewardSelect, this.firstRewardDays, this.firstRewardSelect, this.callbackObject));
			IL_104:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._loginBonusData___2 = ServerInterface.LoginBonusData;
				if (this._loginBonusData___2 != null)
				{
					this._loginBonusData___2.setLoginBonusList(this._net___1.loginBonusReward, this._net___1.firstLoginBonusReward);
				}
				if (this._net___1.loginBonusReward != null)
				{
					this._loginBonusData___2.m_loginBonusState.m_numBonus++;
					this._loginBonusData___2.m_loginBonusState.m_numLogin++;
				}
				this._msg___3 = new MsgLoginBonusSelectSucceed();
				this._msg___3.m_loginBonusReward = this._net___1.loginBonusReward;
				this._msg___3.m_firstLoginBonusReward = this._net___1.firstLoginBonusReward;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerLoginBonusSelect_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerLoginBonusSelect_Succeeded", this._msg___3, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___4 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___4, this.callbackObject, "ServerLoginBonusSelect_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_28B:
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

	public static IEnumerator Process(int rewardId, int rewardDays, int rewardSelect, int firstRewardDays, int firstRewardSelect, GameObject callbackObject)
	{
		ServerLoginBonusSelect._Process_c__Iterator9B _Process_c__Iterator9B = new ServerLoginBonusSelect._Process_c__Iterator9B();
		_Process_c__Iterator9B.rewardId = rewardId;
		_Process_c__Iterator9B.rewardDays = rewardDays;
		_Process_c__Iterator9B.rewardSelect = rewardSelect;
		_Process_c__Iterator9B.firstRewardDays = firstRewardDays;
		_Process_c__Iterator9B.firstRewardSelect = firstRewardSelect;
		_Process_c__Iterator9B.callbackObject = callbackObject;
		_Process_c__Iterator9B.___rewardId = rewardId;
		_Process_c__Iterator9B.___rewardDays = rewardDays;
		_Process_c__Iterator9B.___rewardSelect = rewardSelect;
		_Process_c__Iterator9B.___firstRewardDays = firstRewardDays;
		_Process_c__Iterator9B.___firstRewardSelect = firstRewardSelect;
		_Process_c__Iterator9B.___callbackObject = callbackObject;
		return _Process_c__Iterator9B;
	}
}
