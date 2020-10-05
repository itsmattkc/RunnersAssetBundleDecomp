using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetMileageReward
{
	private sealed class _Process_c__Iterator8A : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal int episode;

		internal int chapter;

		internal NetServerGetMileageReward _net___1;

		internal GameObject callbackObject;

		internal List<ServerMileageReward> _mileageRewardList___2;

		internal List<ServerMileageReward> _allList___3;

		internal List<ServerMileageReward>.Enumerator __s_777___4;

		internal ServerMileageReward _reward___5;

		internal List<ServerMileageReward>.Enumerator __s_778___6;

		internal ServerMileageReward _reward___7;

		internal MsgGetMileageRewardSucceed _msg___8;

		internal MsgServerConnctFailed _msg___9;

		internal int _PC;

		internal object _current;

		internal int ___episode;

		internal int ___chapter;

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
				goto IL_304;
			}
			this._net___1 = new NetServerGetMileageReward(this.episode, this.chapter);
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetMileageRewardRetry(this.episode, this.chapter, this.callbackObject));
			IL_E0:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				this._mileageRewardList___2 = ServerInterface.MileageRewardList;
				this._allList___3 = new List<ServerMileageReward>();
				this.__s_777___4 = this._mileageRewardList___2.GetEnumerator();
				try
				{
					while (this.__s_777___4.MoveNext())
					{
						this._reward___5 = this.__s_777___4.Current;
						this._allList___3.Add(this._reward___5);
					}
				}
				finally
				{
					((IDisposable)this.__s_777___4).Dispose();
				}
				if (this._net___1.m_rewardList != null)
				{
					this._mileageRewardList___2.Clear();
					this.__s_778___6 = this._net___1.m_rewardList.GetEnumerator();
					try
					{
						while (this.__s_778___6.MoveNext())
						{
							this._reward___7 = this.__s_778___6.Current;
							if (!this._mileageRewardList___2.Contains(this._reward___7))
							{
								this._mileageRewardList___2.Add(this._reward___7);
							}
							if (!this._allList___3.Contains(this._reward___7))
							{
								this._allList___3.Add(this._reward___7);
							}
						}
					}
					finally
					{
						((IDisposable)this.__s_778___6).Dispose();
					}
				}
				this._msg___8 = new MsgGetMileageRewardSucceed();
				this._msg___8.m_mileageRewardList = this._allList___3;
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___8, this.callbackObject, "ServerGetMileageReward_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetMileageReward_Succeeded", this._msg___8, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___9 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___9, this.callbackObject, "ServerGetMileageReward_Failed");
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

	public static IEnumerator Process(int episode, int chapter, GameObject callbackObject)
	{
		ServerGetMileageReward._Process_c__Iterator8A _Process_c__Iterator8A = new ServerGetMileageReward._Process_c__Iterator8A();
		_Process_c__Iterator8A.episode = episode;
		_Process_c__Iterator8A.chapter = chapter;
		_Process_c__Iterator8A.callbackObject = callbackObject;
		_Process_c__Iterator8A.___episode = episode;
		_Process_c__Iterator8A.___chapter = chapter;
		_Process_c__Iterator8A.___callbackObject = callbackObject;
		return _Process_c__Iterator8A;
	}
}
