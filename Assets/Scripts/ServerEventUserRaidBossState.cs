using System;

public class ServerEventUserRaidBossState
{
	private int m_numRaidBossRings;

	private int m_raidBossEnergy;

	private int m_raidBossEnergyBuy;

	private int m_numBeatedEncounter;

	private int m_numBeatedEnterprise;

	private int m_numRaidBossEncountered;

	private DateTime m_energyRenewsAt;

	public int NumRaidbossRings
	{
		get
		{
			return this.m_numRaidBossRings;
		}
		set
		{
			this.m_numRaidBossRings = value;
		}
	}

	public int RaidBossEnergy
	{
		get
		{
			return this.m_raidBossEnergy;
		}
		set
		{
			this.m_raidBossEnergy = value;
		}
	}

	public int RaidbossEnergyBuy
	{
		get
		{
			return this.m_raidBossEnergyBuy;
		}
		set
		{
			this.m_raidBossEnergyBuy = value;
		}
	}

	public int RaidBossEnergyCount
	{
		get
		{
			return this.m_raidBossEnergy + this.m_raidBossEnergyBuy;
		}
	}

	public int NumBeatedEncounter
	{
		get
		{
			return this.m_numBeatedEncounter;
		}
		set
		{
			this.m_numBeatedEncounter = value;
		}
	}

	public int NumBeatedEnterprise
	{
		get
		{
			return this.m_numBeatedEnterprise;
		}
		set
		{
			this.m_numBeatedEnterprise = value;
		}
	}

	public int NumRaidBossEncountered
	{
		get
		{
			return this.m_numRaidBossEncountered;
		}
		set
		{
			this.m_numRaidBossEncountered = value;
		}
	}

	public DateTime EnergyRenewsAt
	{
		get
		{
			return this.m_energyRenewsAt;
		}
		set
		{
			this.m_energyRenewsAt = value;
		}
	}

	public ServerEventUserRaidBossState()
	{
		this.m_numRaidBossRings = 0;
		this.m_raidBossEnergy = 0;
		this.m_raidBossEnergyBuy = 0;
		this.m_numBeatedEncounter = 0;
		this.m_numBeatedEnterprise = 0;
		this.m_numRaidBossEncountered = 0;
		this.m_energyRenewsAt = DateTime.MinValue;
	}

	public void CopyTo(ServerEventUserRaidBossState to)
	{
		to.m_numRaidBossRings = this.m_numRaidBossRings;
		to.m_raidBossEnergy = this.m_raidBossEnergy;
		to.m_raidBossEnergyBuy = this.m_raidBossEnergyBuy;
		to.m_numBeatedEncounter = this.m_numBeatedEncounter;
		to.m_numBeatedEnterprise = this.m_numBeatedEnterprise;
		to.m_numRaidBossEncountered = this.m_numRaidBossEncountered;
		to.m_energyRenewsAt = this.m_energyRenewsAt;
	}
}
