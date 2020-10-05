using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetDailyMissionData
{
	private sealed class _Process_c__Iterator86 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetDailyMissionData _net___1;

		internal GameObject callbackObject;

		internal int _numContinue___2;

		internal int _numIncentive___3;

		internal ServerDailyChallengeState _dailyIncentive___4;

		internal int _i___5;

		internal ServerDailyChallengeIncentive _incentive___6;

		internal MsgGetDailyMissionDataSucceed _msg___7;

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
					goto IL_2C6;
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
				goto IL_2C6;
			}
			this._net___1 = new NetServerGetDailyMissionData();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetDailyMissionDataRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._numContinue___2 = this._net___1.resultNumContinue;
				this._numIncentive___3 = this._net___1.resultIncentives;
				this._dailyIncentive___4 = ServerInterface.DailyChallengeState;
				this._dailyIncentive___4.m_incentiveList.Clear();
				this._i___5 = 0;
				while (this._i___5 < this._numIncentive___3)
				{
					this._incentive___6 = this._net___1.GetResultDailyMissionIncentive(this._i___5);
					this._dailyIncentive___4.m_incentiveList.Add(this._incentive___6);
					this._i___5++;
				}
				this._dailyIncentive___4.m_numIncentiveCont = this._numContinue___2;
				this._dailyIncentive___4.m_numDailyChalDay = this._net___1.resultNumDailyChalDay;
				this._dailyIncentive___4.m_maxDailyChalDay = this._net___1.resultMaxDailyChalDay;
				this._dailyIncentive___4.m_maxIncentive = this._net___1.resultMaxIncentive;
				this._dailyIncentive___4.m_chalEndTime = this._net___1.resultChalEndTime;
				NetUtil.SyncSaveDataAndDailyMission(this._dailyIncentive___4);
				this._msg___7 = new MsgGetDailyMissionDataSucceed();
				this._msg___7.m_dailyChallengeState = this._dailyIncentive___4;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___7, this.callbackObject, "ServerGetDailyMissionData_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetDailyMissionData_Succeeded", this._msg___7, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___8 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___8, this.callbackObject, "ServerGetDailyMissionData_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_2C6:
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
		ServerGetDailyMissionData._Process_c__Iterator86 _Process_c__Iterator = new ServerGetDailyMissionData._Process_c__Iterator86();
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}
}
