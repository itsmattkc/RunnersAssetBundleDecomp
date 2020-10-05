using System;
using System.Runtime.CompilerServices;

public class ServerWeeklyLeaderboardOptions
{
	private int _mode_k__BackingField;

	private int _type_k__BackingField;

	private int _param_k__BackingField;

	private int _startTime_k__BackingField;

	private int _endTime_k__BackingField;

	public int mode
	{
		get;
		set;
	}

	public int type
	{
		get;
		set;
	}

	public int param
	{
		get;
		set;
	}

	public int startTime
	{
		get;
		set;
	}

	public int endTime
	{
		get;
		set;
	}

	public RankingUtil.RankingScoreType rankingScoreType
	{
		get
		{
			RankingUtil.RankingScoreType result = RankingUtil.RankingScoreType.HIGH_SCORE;
			if (this.type != 0)
			{
				result = RankingUtil.RankingScoreType.TOTAL_SCORE;
			}
			return result;
		}
	}

	public ServerWeeklyLeaderboardOptions()
	{
		this.mode = 0;
		this.type = 0;
		this.param = 0;
		this.startTime = 0;
		this.endTime = 0;
	}

	public void CopyTo(ServerWeeklyLeaderboardOptions to)
	{
		to.mode = this.mode;
		to.type = this.type;
		to.param = this.param;
		to.startTime = this.startTime;
		to.endTime = this.endTime;
	}
}
