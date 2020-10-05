using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetPrizeDailyBattle
{
	private sealed class _Process_c__Iterator6B : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetPrizeDailyBattle _net___1;

		internal GameObject callbackObject;

		internal MsgGetPrizeDailyBattleSucceed _msg___2;

		internal List<ServerDailyBattlePrizeData>.Enumerator __s_766___3;

		internal ServerDailyBattlePrizeData _prize___4;

		internal ServerDailyBattlePrizeData _setPrize___5;

		internal MsgServerConnctFailed _msg___6;

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
					goto IL_251;
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
				goto IL_251;
			}
			this._net___1 = new NetServerGetPrizeDailyBattle();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetPrizeDailyBattleRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._msg___2 = new MsgGetPrizeDailyBattleSucceed();
				if (this._net___1.battleDataPrizeList != null && this._net___1.battleDataPrizeList.Count > 0)
				{
					this.__s_766___3 = this._net___1.battleDataPrizeList.GetEnumerator();
					try
					{
						while (this.__s_766___3.MoveNext())
						{
							this._prize___4 = this.__s_766___3.Current;
							this._setPrize___5 = new ServerDailyBattlePrizeData();
							this._prize___4.CopyTo(this._setPrize___5);
							this._msg___2.battlePrizeDataList.Add(this._setPrize___5);
						}
					}
					finally
					{
						((IDisposable)this.__s_766___3).Dispose();
					}
				}
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetPrizeDailyBattle_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetPrizeDailyBattle_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___6 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetPrizeDailyBattle_Failed", this._msg___6, SendMessageOptions.DontRequireReceiver);
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_251:
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

	private const string SuccessEvent = "ServerGetPrizeDailyBattle_Succeeded";

	private const string FailEvent = "ServerGetPrizeDailyBattle_Failed";

	public static IEnumerator Process(GameObject callbackObject)
	{
		ServerGetPrizeDailyBattle._Process_c__Iterator6B _Process_c__Iterator6B = new ServerGetPrizeDailyBattle._Process_c__Iterator6B();
		_Process_c__Iterator6B.callbackObject = callbackObject;
		_Process_c__Iterator6B.___callbackObject = callbackObject;
		return _Process_c__Iterator6B;
	}
}
