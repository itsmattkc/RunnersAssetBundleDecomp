using DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventBaseInfo
{
	public enum EVENT_AGGREGATE_TARGET
	{
		SP_CRYSTAL,
		RAID_BOSS,
		ANIMAL,
		CRYSTAL,
		RING,
		DISTANCE,
		NONE
	}

	private static int s_pointSetCount;

	protected List<ChaoData> m_rewardChao;

	protected string m_eventName;

	protected bool m_init;

	protected bool m_dummyData;

	protected long m_totalPoint;

	protected EventBaseInfo.EVENT_AGGREGATE_TARGET m_totalPointTarget = EventBaseInfo.EVENT_AGGREGATE_TARGET.NONE;

	protected string m_caption;

	protected string m_leftTitle;

	protected string m_leftName;

	protected string m_leftText;

	protected string m_leftBg;

	protected string m_chaoTypeIcon;

	protected Texture m_leftTex;

	protected string m_rightTitle;

	protected string m_rightTitleIcon;

	protected List<EventMission> m_eventMission;

	public long totalPoint
	{
		get
		{
			return this.m_totalPoint;
		}
	}

	public EventBaseInfo.EVENT_AGGREGATE_TARGET totalPointTarget
	{
		get
		{
			return this.m_totalPointTarget;
		}
	}

	public string totalPointUnitsString
	{
		get
		{
			string result = string.Empty;
			switch (this.m_totalPointTarget)
			{
			case EventBaseInfo.EVENT_AGGREGATE_TARGET.SP_CRYSTAL:
				result = "ko";
				break;
			case EventBaseInfo.EVENT_AGGREGATE_TARGET.RAID_BOSS:
				result = "tai";
				break;
			case EventBaseInfo.EVENT_AGGREGATE_TARGET.ANIMAL:
				result = "hiki";
				break;
			case EventBaseInfo.EVENT_AGGREGATE_TARGET.CRYSTAL:
				result = "ko";
				break;
			case EventBaseInfo.EVENT_AGGREGATE_TARGET.RING:
				result = "ko";
				break;
			case EventBaseInfo.EVENT_AGGREGATE_TARGET.DISTANCE:
				result = "me-toru";
				break;
			}
			return result;
		}
	}

	public string eventName
	{
		get
		{
			return this.m_eventName;
		}
	}

	public string caption
	{
		get
		{
			return this.m_caption;
		}
	}

	public string leftTitle
	{
		get
		{
			return this.m_leftTitle;
		}
	}

	public string leftName
	{
		get
		{
			return this.m_leftName;
		}
	}

	public string leftText
	{
		get
		{
			return this.m_leftText;
		}
	}

	public string leftBg
	{
		get
		{
			return this.m_leftBg;
		}
	}

	public string chaoTypeIcon
	{
		get
		{
			return this.m_chaoTypeIcon;
		}
	}

	public Texture leftTex
	{
		get
		{
			return this.m_leftTex;
		}
	}

	public string rightTitle
	{
		get
		{
			return this.m_rightTitle;
		}
	}

	public string rightTitleIcon
	{
		get
		{
			return this.m_rightTitleIcon;
		}
	}

	public List<EventMission> eventMission
	{
		get
		{
			return this.m_eventMission;
		}
	}

	public abstract void Init();

	public abstract void UpdateData(MonoBehaviour obj);

	protected virtual void DebugInit()
	{
	}

	public ChaoData GetRewardChao()
	{
		ChaoData result = null;
		if (this.m_rewardChao != null && this.m_rewardChao.Count > 0)
		{
			result = this.m_rewardChao[0];
		}
		return result;
	}

	private int GetAttainmentMissionNum(long point)
	{
		int num = 0;
		if (this.m_eventMission != null && this.m_eventMission.Count > 0)
		{
			for (int i = 0; i < this.m_eventMission.Count; i++)
			{
				if (this.m_eventMission[i] != null && this.m_eventMission[i].IsAttainment(point))
				{
					num++;
				}
			}
		}
		return num;
	}

	public bool SetTotalPoint(int point, out List<EventMission> mission)
	{
		long totalPoint = this.totalPoint;
		this.m_totalPoint = (long)point;
		if (totalPoint != this.m_totalPoint)
		{
			EventBaseInfo.s_pointSetCount++;
		}
		return this.GetCurrentClearMission(totalPoint, out mission, false);
	}

	public bool SetTotalPoint(int point)
	{
		long totalPoint = this.totalPoint;
		this.m_totalPoint = (long)point;
		if (totalPoint != this.m_totalPoint)
		{
			EventBaseInfo.s_pointSetCount++;
		}
		return this.GetCurrentClearMission(totalPoint);
	}

	public bool GetCurrentClearMission(long oldTotalPoint, out List<EventMission> mission, bool nextMission = false)
	{
		bool result = false;
		mission = null;
		if (this.totalPoint >= oldTotalPoint)
		{
			int attainmentMissionNum = this.GetAttainmentMissionNum(this.totalPoint);
			int attainmentMissionNum2 = this.GetAttainmentMissionNum(oldTotalPoint);
			if (attainmentMissionNum >= attainmentMissionNum2 && this.m_eventMission != null && this.m_eventMission.Count > 0)
			{
				for (int i = 0; i < this.m_eventMission.Count; i++)
				{
					if (this.m_eventMission[i] != null)
					{
						if (!this.m_eventMission[i].IsAttainment(this.totalPoint))
						{
							if (nextMission)
							{
								if (mission == null)
								{
									mission = new List<EventMission>();
									mission.Add(this.m_eventMission[i]);
								}
								else
								{
									mission.Add(this.m_eventMission[i]);
								}
								result = true;
							}
							break;
						}
						if (!this.m_eventMission[i].IsAttainment(oldTotalPoint))
						{
							if (mission == null)
							{
								mission = new List<EventMission>();
								mission.Add(this.m_eventMission[i]);
							}
							else
							{
								mission.Add(this.m_eventMission[i]);
							}
							result = true;
						}
					}
				}
			}
		}
		return result;
	}

	public bool GetCurrentClearMission(long oldTotalPoint)
	{
		bool result = false;
		if (this.totalPoint > oldTotalPoint)
		{
			int attainmentMissionNum = this.GetAttainmentMissionNum(this.totalPoint);
			int attainmentMissionNum2 = this.GetAttainmentMissionNum(oldTotalPoint);
			if (attainmentMissionNum > attainmentMissionNum2)
			{
				result = true;
			}
		}
		return result;
	}
}
