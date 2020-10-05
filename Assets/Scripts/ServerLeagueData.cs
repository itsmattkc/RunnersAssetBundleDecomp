using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ServerLeagueData
{
	private int _mode_k__BackingField;

	private int _leagueId_k__BackingField;

	private int _groupId_k__BackingField;

	private int _numGroupMember_k__BackingField;

	private int _numLeagueMember_k__BackingField;

	private int _numUp_k__BackingField;

	private int _numDown_k__BackingField;

	private List<ServerRemainOperator> _highScoreOpe_k__BackingField;

	private List<ServerRemainOperator> _totalScoreOpe_k__BackingField;

	public int mode
	{
		get;
		set;
	}

	public RankingUtil.RankingMode rankinMode
	{
		get
		{
			RankingUtil.RankingMode result = RankingUtil.RankingMode.ENDLESS;
			if (this.mode >= 0 && this.mode < 2)
			{
				result = (RankingUtil.RankingMode)this.mode;
			}
			return result;
		}
	}

	public int leagueId
	{
		get;
		set;
	}

	public int groupId
	{
		get;
		set;
	}

	public int numGroupMember
	{
		get;
		set;
	}

	public int numLeagueMember
	{
		get;
		set;
	}

	public int numUp
	{
		get;
		set;
	}

	public int numDown
	{
		get;
		set;
	}

	public List<ServerRemainOperator> highScoreOpe
	{
		get;
		set;
	}

	public List<ServerRemainOperator> totalScoreOpe
	{
		get;
		set;
	}

	public ServerLeagueData()
	{
		this.mode = 0;
		this.leagueId = 1;
		this.groupId = 0;
		this.numGroupMember = 0;
		this.numLeagueMember = 0;
		this.numUp = 10;
		this.numDown = 10;
		this.highScoreOpe = new List<ServerRemainOperator>();
		this.totalScoreOpe = new List<ServerRemainOperator>();
	}

	public void Dump()
	{
	}

	public void CopyTo(ServerLeagueData to)
	{
		to.mode = this.mode;
		to.leagueId = this.leagueId;
		to.groupId = this.groupId;
		to.numGroupMember = this.numGroupMember;
		to.numLeagueMember = this.numLeagueMember;
		to.numUp = this.numUp;
		to.numDown = this.numDown;
		this.SetServerRemainOperator(this.highScoreOpe, to.highScoreOpe);
		this.SetServerRemainOperator(this.totalScoreOpe, to.totalScoreOpe);
	}

	public void AddHighScoreRemainOperator(ServerRemainOperator remainOperator)
	{
		this.highScoreOpe.Add(remainOperator);
	}

	public void AddTotalScoreRemainOperator(ServerRemainOperator remainOperator)
	{
		this.totalScoreOpe.Add(remainOperator);
	}

	private void SetServerRemainOperator(List<ServerRemainOperator> setData, List<ServerRemainOperator> getData)
	{
		if (setData != null && getData != null && setData.Count > 0)
		{
			getData.Clear();
			for (int i = 0; i < setData.Count; i++)
			{
				getData.Add(setData[i]);
			}
		}
	}
}
