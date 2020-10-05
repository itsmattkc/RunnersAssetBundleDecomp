using System;

public class ServerEventRaidBossBonus
{
	private int m_encounterBonus;

	private int m_wrestleBonus;

	private int m_damageRateBonus;

	private int m_damageTopBonus;

	private int m_beatBonus;

	public int EncounterBonus
	{
		get
		{
			return this.m_encounterBonus;
		}
		set
		{
			this.m_encounterBonus = value;
		}
	}

	public int WrestleBonus
	{
		get
		{
			return this.m_wrestleBonus;
		}
		set
		{
			this.m_wrestleBonus = value;
		}
	}

	public int DamageRateBonus
	{
		get
		{
			return this.m_damageRateBonus;
		}
		set
		{
			this.m_damageRateBonus = value;
		}
	}

	public int DamageTopBonus
	{
		get
		{
			return this.m_damageTopBonus;
		}
		set
		{
			this.m_damageTopBonus = value;
		}
	}

	public int BeatBonus
	{
		get
		{
			return this.m_beatBonus;
		}
		set
		{
			this.m_beatBonus = value;
		}
	}

	public ServerEventRaidBossBonus()
	{
		this.m_encounterBonus = 0;
		this.m_wrestleBonus = 0;
		this.m_damageRateBonus = 0;
		this.m_damageTopBonus = 0;
		this.m_beatBonus = 0;
	}

	public void CopyTo(ServerEventRaidBossBonus to)
	{
		to.m_encounterBonus = this.m_encounterBonus;
		to.m_wrestleBonus = this.m_wrestleBonus;
		to.m_damageRateBonus = this.m_damageRateBonus;
		to.m_damageTopBonus = this.m_damageTopBonus;
		to.m_beatBonus = this.m_beatBonus;
	}
}
