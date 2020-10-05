using DataTable;
using Message;
using Mission;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StageMissionManager : MonoBehaviour
{
	private static readonly MissionTypeParam[] MISSION_TYPE_PARAM_TBL = new MissionTypeParam[]
	{
		new MissionTypeParam(EventID.ENEMYDEAD, MissionCategory.COUNT),
		new MissionTypeParam(EventID.GOLDENENEMYDEAD, MissionCategory.COUNT),
		new MissionTypeParam(EventID.TOTALDISTANCE, MissionCategory.COUNT),
		new MissionTypeParam(EventID.GET_ANIMALS, MissionCategory.COUNT),
		new MissionTypeParam(EventID.GET_SCORE, MissionCategory.COUNT),
		new MissionTypeParam(EventID.GET_RING, MissionCategory.COUNT)
	};

	private List<MissionCheck> m_missionChecks;

	private static StageMissionManager instance;

	private bool _Completed_k__BackingField;

	public bool Completed
	{
		get;
		private set;
	}

	public static StageMissionManager Instance
	{
		get
		{
			return StageMissionManager.instance;
		}
	}

	protected void Awake()
	{
		this.CheckInstance();
	}

	private void Update()
	{
		if (this.m_missionChecks == null)
		{
			return;
		}
		foreach (MissionCheck current in this.m_missionChecks)
		{
			current.Update(Time.deltaTime);
		}
	}

	private void OnDestroy()
	{
		this.DeleteAllMissionCheck();
		if (this == StageMissionManager.instance)
		{
			StageMissionManager.instance = null;
		}
	}

	public void SetupMissions()
	{
		if (this.m_missionChecks == null)
		{
			this.m_missionChecks = new List<MissionCheck>();
		}
		if (this.m_missionChecks != null && SaveDataManager.Instance)
		{
			DailyMissionData dailyMission = SaveDataManager.Instance.PlayerData.DailyMission;
			int id = dailyMission.id;
			MissionData missionData = MissionTable.GetMissionData(id);
			if (missionData != null)
			{
				bool flag = true;
				if (EventManager.Instance != null && EventManager.Instance.IsRaidBossStage() && missionData.type == MissionData.Type.RING)
				{
					flag = false;
				}
				if (dailyMission.missions_complete)
				{
					flag = false;
				}
				if (flag)
				{
					bool isSetInitialValue = missionData.save;
					long progress = dailyMission.progress;
					this.CreateMissionCheck(missionData, isSetInitialValue, progress);
				}
			}
		}
	}

	public void SaveMissions()
	{
		if (this.m_missionChecks == null)
		{
			return;
		}
		foreach (MissionCheck current in this.m_missionChecks)
		{
			MissionData data = current.GetData();
			if (data != null && SaveDataManager.Instance != null)
			{
				DailyMissionData dailyMission = SaveDataManager.Instance.PlayerData.DailyMission;
				if (data.save && dailyMission.id == data.id)
				{
					dailyMission.progress = current.GetValue();
					if (current.IsCompleted())
					{
						dailyMission.missions_complete = true;
						this.Completed = true;
					}
				}
			}
		}
	}

	private void CreateMissionCheck(MissionData data, bool isSetInitialValue = false, long initialValue = 0L)
	{
		MissionCheck missionCheck = null;
		if ((ulong)data.type < (ulong)((long)StageMissionManager.MISSION_TYPE_PARAM_TBL.Length))
		{
			MissionCategory category = StageMissionManager.MISSION_TYPE_PARAM_TBL[(int)data.type].m_category;
			if (category == MissionCategory.COUNT)
			{
				MissionCheckCount missionCheckCount = new MissionCheckCount(StageMissionManager.MISSION_TYPE_PARAM_TBL[(int)data.type].m_eventID);
				missionCheck = missionCheckCount;
			}
		}
		if (missionCheck != null)
		{
			if (isSetInitialValue)
			{
				missionCheck.SetInitialValue(initialValue);
			}
			missionCheck.SetData(data);
			this.m_missionChecks.Add(missionCheck);
		}
	}

	private void OnMissionEvent(MsgMissionEvent msg)
	{
		if (this.m_missionChecks == null)
		{
			return;
		}
		foreach (MsgMissionEvent.Data current in msg.m_missions)
		{
			MissionEvent missionEvent = new MissionEvent(current.eventid, current.num);
			this.ProcEvent(missionEvent);
		}
	}

	private void ProcEvent(MissionEvent missionEvent)
	{
		if (this.m_missionChecks == null)
		{
			return;
		}
		foreach (MissionCheck current in this.m_missionChecks)
		{
			current.ProcEvent(missionEvent);
		}
	}

	private void DeleteAllMissionCheck()
	{
		this.m_missionChecks = null;
	}

	private bool GetMissionIsCompletedAndValue(int index, ref bool? isCompleted, ref int? value)
	{
		return false;
	}

	private bool IsCompletedMission(int index)
	{
		bool? flag = new bool?(false);
		int? num = null;
		this.GetMissionIsCompletedAndValue(index, ref flag, ref num);
		return flag.Value;
	}

	private int GetMissionValue(int index)
	{
		bool? flag = null;
		int? num = new int?(0);
		if (this.GetMissionIsCompletedAndValue(index, ref flag, ref num))
		{
			return num.Value;
		}
		return 0;
	}

	private void DebugComplete(int missionNo)
	{
		if (this.m_missionChecks == null)
		{
			return;
		}
		foreach (MissionCheck current in this.m_missionChecks)
		{
			if (current.GetIndex() == missionNo)
			{
				current.DebugComplete(missionNo);
				break;
			}
		}
	}

	private void DebugComplete()
	{
		if (this.m_missionChecks == null)
		{
			return;
		}
		foreach (MissionCheck current in this.m_missionChecks)
		{
			current.DebugComplete(current.GetIndex());
		}
	}

	protected bool CheckInstance()
	{
		if (StageMissionManager.instance == null)
		{
			StageMissionManager.instance = this;
			return true;
		}
		if (this == StageMissionManager.Instance)
		{
			return true;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		return false;
	}
}
