using Message;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServerDayCrossWatcher : MonoBehaviour
{
	public class MsgDayCross
	{
		private bool _ServerConnect_k__BackingField;

		public bool ServerConnect
		{
			get;
			set;
		}
	}

	public delegate void UpdateInfoCallback(ServerDayCrossWatcher.MsgDayCross msg);

	private static ServerDayCrossWatcher m_instance;

	private static readonly int DayCrossHour = 15;

	private static readonly int DayCrossMinute;

	private DateTime m_nextGetInfoTime;

	private DateTime m_nextGetLoginBonusTime;

	private ServerDayCrossWatcher.UpdateInfoCallback m_callbackDayCross;

	private ServerDayCrossWatcher.UpdateInfoCallback m_callbackDailyMission;

	private ServerDayCrossWatcher.UpdateInfoCallback m_callbackDailyMissionForOneDay;

	private ServerDayCrossWatcher.UpdateInfoCallback m_callbackLoginBonus;

	public static ServerDayCrossWatcher Instance
	{
		get
		{
			return ServerDayCrossWatcher.m_instance;
		}
	}

	public bool IsDayCross()
	{
		DateTime currentTime = NetBase.GetCurrentTime();
		return DateTime.Compare(currentTime, this.m_nextGetInfoTime) >= 0;
	}

	public bool IsDaylyMissionEnd()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			DateTime currentTime = NetBase.GetCurrentTime();
			DateTime endDailyMissionDate = ServerInterface.PlayerState.m_endDailyMissionDate;
			if (DateTime.Compare(currentTime, endDailyMissionDate) >= 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsDaylyMissionChallengeEnd()
	{
		DateTime currentTime = NetBase.GetCurrentTime();
		DateTime t = currentTime.AddDays(1.0);
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			t = ServerInterface.DailyChallengeState.m_chalEndTime;
		}
		return DateTime.Compare(currentTime, t) >= 0;
	}

	public bool IsLoginBonusDayCross()
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			DateTime currentTime = NetBase.GetCurrentTime();
			if (DateTime.Compare(currentTime, this.m_nextGetLoginBonusTime) >= 0)
			{
				return true;
			}
		}
		return false;
	}

	public void UpdateClientInfosByDayCross(ServerDayCrossWatcher.UpdateInfoCallback callback)
	{
		if (callback == null)
		{
			return;
		}
		this.m_callbackDayCross = callback;
		if (!this.IsDayCross())
		{
			if (this.m_callbackDayCross != null)
			{
				this.m_callbackDayCross(new ServerDayCrossWatcher.MsgDayCross());
				this.m_callbackDayCross = null;
			}
			return;
		}
		this.CalcNextGetInfoTime();
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetWheelOptions(base.gameObject);
		}
	}

	public void UpdateDailyMissionForOneDay(ServerDayCrossWatcher.UpdateInfoCallback callback)
	{
		if (callback == null)
		{
			return;
		}
		this.m_callbackDailyMissionForOneDay = callback;
		if (!this.IsDaylyMissionEnd())
		{
			if (this.m_callbackDailyMissionForOneDay != null)
			{
				this.m_callbackDailyMissionForOneDay(new ServerDayCrossWatcher.MsgDayCross());
				this.m_callbackDailyMissionForOneDay = null;
			}
			return;
		}
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerRetrievePlayerState(base.gameObject);
		}
	}

	public void UpdateDailyMissionInfoByChallengeEnd(ServerDayCrossWatcher.UpdateInfoCallback callback)
	{
		if (callback == null)
		{
			return;
		}
		this.m_callbackDailyMission = callback;
		if (!this.IsDaylyMissionChallengeEnd())
		{
			if (this.m_callbackDailyMission != null)
			{
				this.m_callbackDailyMission(new ServerDayCrossWatcher.MsgDayCross());
				this.m_callbackDailyMission = null;
			}
			return;
		}
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetDailyMissionData(base.gameObject);
		}
	}

	public void UpdateLoginBonusEnd(ServerDayCrossWatcher.UpdateInfoCallback callback)
	{
		if (callback == null)
		{
			return;
		}
		this.m_callbackLoginBonus = callback;
		if (!this.IsLoginBonusDayCross())
		{
			if (this.m_callbackLoginBonus != null)
			{
				this.m_callbackLoginBonus(new ServerDayCrossWatcher.MsgDayCross());
				this.m_callbackLoginBonus = null;
			}
			return;
		}
		this.CalcNextGetLoginBonusTime();
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerLoginBonus(base.gameObject);
		}
	}

	private void CalcNextGetInfoTime()
	{
		DateTime dateTime = DateTime.Today.AddHours((double)ServerDayCrossWatcher.DayCrossHour).AddMinutes((double)ServerDayCrossWatcher.DayCrossMinute);
		DateTime currentTime = NetBase.GetCurrentTime();
		if (DateTime.Compare(currentTime, dateTime) < 0)
		{
			this.m_nextGetInfoTime = dateTime;
		}
		else
		{
			DateTime nextGetInfoTime = dateTime.AddDays(1.0);
			this.m_nextGetInfoTime = nextGetInfoTime;
		}
	}

	private void CalcNextGetLoginBonusTime()
	{
		DateTime dateTime = DateTime.Today.AddHours((double)ServerDayCrossWatcher.DayCrossHour).AddMinutes((double)ServerDayCrossWatcher.DayCrossMinute);
		DateTime currentTime = NetBase.GetCurrentTime();
		if (DateTime.Compare(currentTime, dateTime) < 0)
		{
			dateTime.AddMinutes((double)UnityEngine.Random.Range(1, 30));
			this.m_nextGetInfoTime = dateTime;
		}
		else
		{
			DateTime nextGetInfoTime = dateTime.AddDays(1.0);
			nextGetInfoTime.AddMinutes((double)UnityEngine.Random.Range(1, 30));
			this.m_nextGetInfoTime = nextGetInfoTime;
		}
	}

	private void Start()
	{
		if (ServerDayCrossWatcher.m_instance == null)
		{
			ServerDayCrossWatcher.m_instance = this;
			this.m_nextGetInfoTime = NetUtil.GetLocalDateTime(0L);
			this.m_nextGetLoginBonusTime = NetUtil.GetLocalDateTime(0L);
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (ServerDayCrossWatcher.m_instance == this)
		{
			ServerDayCrossWatcher.m_instance = null;
		}
	}

	private void Update()
	{
	}

	private void ServerGetWheelOptions_Succeeded(MsgGetWheelOptionsSucceed msg)
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetCampaignList(base.gameObject);
		}
	}

	private void GetCampaignList_Succeeded(MsgGetCampaignListSucceed msg)
	{
		ServerInterface loggedInServerInterface = ServerInterface.LoggedInServerInterface;
		if (loggedInServerInterface != null)
		{
			loggedInServerInterface.RequestServerGetEventList(base.gameObject);
		}
	}

	private void ServerGetEventList_Succeeded(MsgGetEventListSucceed msg)
	{
		if (this.m_callbackDayCross != null)
		{
			ServerDayCrossWatcher.MsgDayCross msgDayCross = new ServerDayCrossWatcher.MsgDayCross();
			msgDayCross.ServerConnect = true;
			this.m_callbackDayCross(msgDayCross);
			this.m_callbackDayCross = null;
		}
	}

	private void ServerGetDailyMissionData_Succeeded(MsgGetDailyMissionDataSucceed msg)
	{
		if (this.m_callbackDailyMission != null)
		{
			ServerDayCrossWatcher.MsgDayCross msgDayCross = new ServerDayCrossWatcher.MsgDayCross();
			msgDayCross.ServerConnect = true;
			this.m_callbackDailyMission(msgDayCross);
			this.m_callbackDailyMission = null;
		}
	}

	private void ServerRetrievePlayerState_Succeeded(MsgGetPlayerStateSucceed msg)
	{
		if (this.m_callbackDailyMissionForOneDay != null)
		{
			ServerDayCrossWatcher.MsgDayCross msgDayCross = new ServerDayCrossWatcher.MsgDayCross();
			msgDayCross.ServerConnect = true;
			this.m_callbackDailyMissionForOneDay(msgDayCross);
			this.m_callbackDailyMissionForOneDay = null;
		}
	}

	private void ServerLoginBonus_Succeeded(MsgLoginBonusSucceed msg)
	{
		if (this.m_callbackLoginBonus != null)
		{
			ServerDayCrossWatcher.MsgDayCross msgDayCross = new ServerDayCrossWatcher.MsgDayCross();
			msgDayCross.ServerConnect = true;
			this.m_callbackLoginBonus(msgDayCross);
			this.m_callbackLoginBonus = null;
		}
	}
}
