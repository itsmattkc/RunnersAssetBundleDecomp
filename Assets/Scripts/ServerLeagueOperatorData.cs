using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ServerLeagueOperatorData
{
	private int _leagueId_k__BackingField;

	private int _numUp_k__BackingField;

	private int _numDown_k__BackingField;

	private List<ServerRemainOperator> _highScoreOpe_k__BackingField;

	private List<ServerRemainOperator> _totalScoreOpe_k__BackingField;

	public int leagueId
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

	public ServerLeagueOperatorData()
	{
		this.leagueId = 1;
		this.numUp = 10;
		this.numDown = 10;
		this.highScoreOpe = new List<ServerRemainOperator>();
		this.totalScoreOpe = new List<ServerRemainOperator>();
	}

	public void Dump()
	{
	}

	public void CopyTo(ServerLeagueOperatorData to)
	{
		to.leagueId = this.leagueId;
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
