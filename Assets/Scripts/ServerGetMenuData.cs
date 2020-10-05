using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerGetMenuData
{
	private sealed class _Process_c__Iterator88 : IDisposable, IEnumerator, IEnumerator<object>
	{
		internal NetMonitor _monitor___0;

		internal NetServerGetMenuData _net___1;

		internal GameObject callbackObject;

		internal MsgGetMenuDataSucceed _msg___2;

		internal MsgServerConnctFailed _msg___3;

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
					goto IL_2D2;
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
				goto IL_2D2;
			}
			this._net___1 = new NetServerGetMenuData();
			this._net___1.Request();
			this._monitor___0.StartMonitor(new ServerGetMenuDataRetry(this.callbackObject));
			IL_C8:
			if (this._net___1.IsExecuting())
			{
				this._current = null;
				this._PC = 2;
				return true;
			}
			if (this._net___1.IsSucceeded())
			{
				ServerGetMenuData.SetPlayerState(this._net___1.PlayerState);
				ServerGetMenuData.SetWheelOptions(this._net___1.WheelOptions);
				ServerGetMenuData.SetChaoWheelOptions(this._net___1.ChaoWheelOptions);
				ServerGetMenuData.SetSubCharaRingPayment(this._net___1.SubCharaRingExchange);
				ServerGetMenuData.SetDailyChallenge(this._net___1.DailyChallengeState);
				ServerGetMenuData.SetMileageMap(this._net___1.MileageMapState);
				ServerGetMenuData.SetMessage(this._net___1.MessageEntryList, this._net___1.TotalMessage);
				ServerGetMenuData.SetOperatorMessage(this._net___1.OperatorMessageEntryList, this._net___1.TotalOperatorMessage);
				ServerGetMenuData.SetRedStarExchangeList(ServerInterface.RedStarItemList, this._net___1.RedstarItemStateList, this._net___1.RedstarTotalItems);
				ServerGetMenuData.SetRedStarExchangeList(ServerInterface.RedStarExchangeRingItemList, this._net___1.RingItemStateList, this._net___1.RingTotalItems);
				ServerGetMenuData.SetRedStarExchangeList(ServerInterface.RedStarExchangeEnergyItemList, this._net___1.EnergyItemStateList, this._net___1.EnergyTotalItems);
				ServerGetMenuData.SetMonthPurchase(this._net___1.MonthPurchase);
				ServerGetMenuData.SetBirthday(this._net___1.BirthDay);
				ServerGetMenuData.SetConsumedCostList(this._net___1.ConsumedCostList);
				this._msg___2 = new MsgGetMenuDataSucceed();
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___2, this.callbackObject, "ServerGetMenuData_Succeeded");
				}
				if (this.callbackObject != null)
				{
					this.callbackObject.SendMessage("ServerGetMenuData_Succeeded", this._msg___2, SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this._msg___3 = new MsgServerConnctFailed(this._net___1.resultStCd);
				if (this._monitor___0 != null)
				{
					this._monitor___0.EndMonitorForward(this._msg___3, this.callbackObject, "ServerGetMenuData_Failed");
				}
			}
			if (this._monitor___0 != null)
			{
				this._monitor___0.EndMonitorBackward();
			}
			IL_2D2:
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
		ServerGetMenuData._Process_c__Iterator88 _Process_c__Iterator = new ServerGetMenuData._Process_c__Iterator88();
		_Process_c__Iterator.callbackObject = callbackObject;
		_Process_c__Iterator.___callbackObject = callbackObject;
		return _Process_c__Iterator;
	}

	private static void SetPlayerState(ServerPlayerState resultPlayerState)
	{
		if (resultPlayerState != null)
		{
			ServerPlayerState playerState = ServerInterface.PlayerState;
			if (playerState != null)
			{
				resultPlayerState.CopyTo(playerState);
			}
		}
	}

	private static void SetWheelOptions(ServerWheelOptions resultWheelOptions)
	{
		if (resultWheelOptions != null)
		{
			ServerWheelOptions wheelOptions = ServerInterface.WheelOptions;
			if (wheelOptions != null)
			{
				resultWheelOptions.CopyTo(wheelOptions);
			}
		}
	}

	private static void SetChaoWheelOptions(ServerChaoWheelOptions resultChaoWheelOptions)
	{
		if (resultChaoWheelOptions != null)
		{
			ServerChaoWheelOptions chaoWheelOptions = ServerInterface.ChaoWheelOptions;
			if (chaoWheelOptions != null)
			{
				resultChaoWheelOptions.CopyTo(chaoWheelOptions);
			}
		}
	}

	private static void SetSubCharaRingPayment(int subCharaRingPayment)
	{
		ServerSettingState settingState = ServerInterface.SettingState;
		if (settingState != null)
		{
			settingState.m_subCharaRingPayment = subCharaRingPayment;
		}
	}

	private static void SetDailyChallenge(ServerDailyChallengeState dailyChallengeState)
	{
		if (dailyChallengeState != null)
		{
			int count = dailyChallengeState.m_incentiveList.Count;
			ServerDailyChallengeState dailyChallengeState2 = ServerInterface.DailyChallengeState;
			if (dailyChallengeState2 != null)
			{
				dailyChallengeState2.m_incentiveList.Clear();
				for (int i = 0; i < count; i++)
				{
					dailyChallengeState2.m_incentiveList.Add(dailyChallengeState.m_incentiveList[i]);
				}
				dailyChallengeState2.m_numIncentiveCont = dailyChallengeState.m_numIncentiveCont;
				dailyChallengeState2.m_numDailyChalDay = dailyChallengeState.m_numDailyChalDay;
				dailyChallengeState2.m_maxDailyChalDay = dailyChallengeState.m_maxDailyChalDay;
				dailyChallengeState2.m_maxIncentive = dailyChallengeState.m_maxIncentive;
				NetUtil.SyncSaveDataAndDailyMission(dailyChallengeState2);
			}
		}
	}

	private static void SetMileageMap(ServerMileageMapState resultMileageMapState)
	{
		if (resultMileageMapState != null)
		{
			ServerMileageMapState mileageMapState = ServerInterface.MileageMapState;
			if (mileageMapState != null)
			{
				resultMileageMapState.CopyTo(mileageMapState);
			}
		}
	}

	private static void SetMessage(List<ServerMessageEntry> resultMessageList, int totalMessage)
	{
		if (resultMessageList != null)
		{
			List<ServerMessageEntry> messageList = ServerInterface.MessageList;
			messageList.Clear();
			for (int i = 0; i < totalMessage; i++)
			{
				ServerMessageEntry item = resultMessageList[i];
				messageList.Add(item);
			}
		}
	}

	private static void SetOperatorMessage(List<ServerOperatorMessageEntry> resultMessageList, int totalMessage)
	{
		if (resultMessageList != null)
		{
			List<ServerOperatorMessageEntry> operatorMessageList = ServerInterface.OperatorMessageList;
			operatorMessageList.Clear();
			for (int i = 0; i < totalMessage; i++)
			{
				ServerOperatorMessageEntry item = resultMessageList[i];
				operatorMessageList.Add(item);
			}
		}
	}

	private static void SetRedStarExchangeList(List<ServerRedStarItemState> serverExachangeList, List<ServerRedStarItemState> exchangeList, int resultItems)
	{
		if (exchangeList != null && serverExachangeList != null)
		{
			serverExachangeList.Clear();
			for (int i = 0; i < resultItems; i++)
			{
				ServerRedStarItemState serverRedStarItemState = exchangeList[i];
				if (serverRedStarItemState != null)
				{
					ServerRedStarItemState serverRedStarItemState2 = new ServerRedStarItemState();
					serverRedStarItemState.CopyTo(serverRedStarItemState2);
					if (serverExachangeList != null)
					{
						serverExachangeList.Add(serverRedStarItemState2);
					}
				}
			}
		}
	}

	private static void SetMonthPurchase(int monthPurchase)
	{
		ServerSettingState settingState = ServerInterface.SettingState;
		if (settingState != null)
		{
			settingState.m_monthPurchase = monthPurchase;
		}
	}

	private static void SetBirthday(string birthday)
	{
		ServerSettingState settingState = ServerInterface.SettingState;
		if (settingState != null)
		{
			settingState.m_birthday = birthday;
		}
	}

	private static void SetConsumedCostList(List<ServerConsumedCostData> serverConsumedCostList)
	{
		List<ServerConsumedCostData> consumedCostList = ServerInterface.ConsumedCostList;
		if (consumedCostList != null)
		{
			consumedCostList.Clear();
			foreach (ServerConsumedCostData current in serverConsumedCostList)
			{
				if (current != null)
				{
					ServerConsumedCostData serverConsumedCostData = new ServerConsumedCostData();
					current.CopyTo(serverConsumedCostData);
					consumedCostList.Add(serverConsumedCostData);
				}
			}
		}
	}
}
