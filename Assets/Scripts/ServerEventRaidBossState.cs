using System;

public class ServerEventRaidBossState
{
	public enum StatusType
	{
		INIT,
		BOSS_ALIVE,
		BOSS_ESCAPE,
		REWARD,
		PROCESS_END
	}

	private long m_raidBossId;

	private int m_level;

	private int m_rarity;

	private int m_hitPoint;

	private int m_maxHitPoint;

	private int m_status;

	private DateTime m_escapeAt;

	private string m_encounterName;

	private bool m_encounterFlag;

	private bool m_crowdedFlag;

	private bool m_participationFlag;

	public long Id
	{
		get
		{
			return this.m_raidBossId;
		}
		set
		{
			this.m_raidBossId = value;
		}
	}

	public int Level
	{
		get
		{
			return this.m_level;
		}
		set
		{
			this.m_level = value;
		}
	}

	public int Rarity
	{
		get
		{
			return this.m_rarity;
		}
		set
		{
			this.m_rarity = value;
		}
	}

	public int HitPoint
	{
		get
		{
			return this.m_hitPoint;
		}
		set
		{
			this.m_hitPoint = value;
		}
	}

	public int MaxHitPoint
	{
		get
		{
			return this.m_maxHitPoint;
		}
		set
		{
			this.m_maxHitPoint = value;
		}
	}

	public int Status
	{
		get
		{
			return this.m_status;
		}
		set
		{
			this.m_status = value;
		}
	}

	public DateTime EscapeAt
	{
		get
		{
			return this.m_escapeAt;
		}
		set
		{
			this.m_escapeAt = value;
		}
	}

	public string EncounterName
	{
		get
		{
			return this.m_encounterName;
		}
		set
		{
			this.m_encounterName = value;
		}
	}

	public bool Encounter
	{
		get
		{
			return this.m_encounterFlag;
		}
		set
		{
			this.m_encounterFlag = value;
		}
	}

	public bool Crowded
	{
		get
		{
			return this.m_crowdedFlag;
		}
		set
		{
			this.m_crowdedFlag = value;
		}
	}

	public bool Participation
	{
		get
		{
			return this.m_participationFlag;
		}
		set
		{
			this.m_participationFlag = value;
		}
	}

	public ServerEventRaidBossState()
	{
		this.m_raidBossId = 0L;
		this.m_level = 0;
		this.m_rarity = 0;
		this.m_hitPoint = 0;
		this.m_maxHitPoint = 0;
		this.m_status = 0;
		this.m_escapeAt = DateTime.MinValue;
		this.m_encounterName = string.Empty;
		this.m_encounterFlag = false;
		this.m_crowdedFlag = false;
		this.m_participationFlag = false;
	}

	public ServerEventRaidBossState.StatusType GetStatusType()
	{
		ServerEventRaidBossState.StatusType result = ServerEventRaidBossState.StatusType.INIT;
		switch (this.m_status)
		{
		case 1:
			result = ServerEventRaidBossState.StatusType.BOSS_ALIVE;
			break;
		case 2:
			result = ServerEventRaidBossState.StatusType.BOSS_ESCAPE;
			break;
		case 3:
			result = ServerEventRaidBossState.StatusType.REWARD;
			break;
		case 4:
			result = ServerEventRaidBossState.StatusType.PROCESS_END;
			break;
		}
		return result;
	}

	public void CopyTo(ServerEventRaidBossState to)
	{
		to.m_raidBossId = this.m_raidBossId;
		to.m_level = this.m_level;
		to.m_rarity = this.m_rarity;
		to.m_hitPoint = this.m_hitPoint;
		to.m_maxHitPoint = this.m_maxHitPoint;
		to.m_status = this.m_status;
		to.m_escapeAt = this.m_escapeAt;
		to.m_encounterName = this.m_encounterName;
		to.m_encounterFlag = this.m_encounterFlag;
		to.m_crowdedFlag = this.m_crowdedFlag;
		to.m_participationFlag = this.m_participationFlag;
	}
}
