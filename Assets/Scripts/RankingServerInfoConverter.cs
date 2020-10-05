using System;
using System.Collections.Generic;
using Text;

public class RankingServerInfoConverter
{
	public enum MSG_INFO
	{
		ScoreRanking,
		TotalScoreRanking,
		RivalRanking,
		TotalRivalRanking,
		SendPresent,
		StartDt,
		EndDt,
		League,
		OldLeague,
		NumLeagueMember,
		SendPresentRival,
		NUM
	}

	public enum ResultType
	{
		Up,
		Stay,
		Down,
		Error,
		NUM
	}

	private static string ms_lastServerMessageInfo;

	private string m_orgServerMessageInfo;

	private int m_scoreRanking;

	private int m_totalScoreRanking;

	private int m_rivalRanking;

	private int m_totalRivalRanking;

	private int m_sendPresent;

	private DateTime m_startDt;

	private DateTime m_endDt;

	private int m_league;

	private int m_oldLeague;

	private int m_numLeagueMember;

	private int m_sendPresentRival;

	public bool isInit
	{
		get
		{
			return this.m_orgServerMessageInfo != null;
		}
	}

	public RankingServerInfoConverter.ResultType leagueResult
	{
		get
		{
			RankingServerInfoConverter.ResultType result = RankingServerInfoConverter.ResultType.Stay;
			if (!this.isInit)
			{
				return RankingServerInfoConverter.ResultType.Error;
			}
			if (this.m_league != this.m_oldLeague)
			{
				if (this.m_league > this.m_oldLeague)
				{
					result = RankingServerInfoConverter.ResultType.Up;
				}
				else
				{
					result = RankingServerInfoConverter.ResultType.Down;
				}
			}
			return result;
		}
	}

	public LeagueType currentLeague
	{
		get
		{
			return (LeagueType)this.m_league;
		}
	}

	public LeagueType oldLeague
	{
		get
		{
			return (LeagueType)this.m_oldLeague;
		}
	}

	public DateTime startDt
	{
		get
		{
			return this.m_startDt;
		}
	}

	public DateTime endDt
	{
		get
		{
			return this.m_endDt;
		}
	}

	public string rankingResultAllText
	{
		get
		{
			if (!this.isInit)
			{
				return null;
			}
			string text;
			if (this.m_sendPresent <= 0)
			{
				text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_result_text_4").text;
			}
			else
			{
				text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_result_text_3").text;
			}
			return TextUtility.Replaces(text, new Dictionary<string, string>
			{
				{
					"{PARAM1}",
					this.m_scoreRanking.ToString()
				},
				{
					"{PARAM3}",
					this.m_totalScoreRanking.ToString()
				}
			});
		}
	}

	public string rankingResultLeagueText
	{
		get
		{
			if (!this.isInit)
			{
				return null;
			}
			string text;
			if (this.m_sendPresentRival <= 0)
			{
				text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_result_text_2").text;
			}
			else
			{
				text = TextManager.GetText(TextManager.TextType.TEXTTYPE_COMMON_TEXT, "Ranking", "ranking_result_text_1").text;
			}
			return TextUtility.Replaces(text, new Dictionary<string, string>
			{
				{
					"{PARAM1}",
					this.m_rivalRanking.ToString()
				},
				{
					"{PARAM2}",
					this.m_numLeagueMember.ToString()
				},
				{
					"{PARAM3}",
					this.m_totalRivalRanking.ToString()
				},
				{
					"{PARAM4}",
					this.m_numLeagueMember.ToString()
				}
			});
		}
	}

	public static string lastServerMessageInfo
	{
		get
		{
			return RankingServerInfoConverter.ms_lastServerMessageInfo;
		}
		set
		{
			RankingServerInfoConverter.ms_lastServerMessageInfo = value;
		}
	}

	public RankingServerInfoConverter(string serverMessageInfo)
	{
		this.Setup(serverMessageInfo);
	}

	public void Setup(string serverMessageInfo)
	{
		this.m_orgServerMessageInfo = serverMessageInfo;
		RankingServerInfoConverter.ms_lastServerMessageInfo = serverMessageInfo;
		string[] array = this.m_orgServerMessageInfo.Split(new char[]
		{
			','
		});
		if (array != null && array.Length > 0)
		{
			UnityEngine.Debug.Log("orgServerMessageInfo=" + this.m_orgServerMessageInfo);
			if (array.Length > 0)
			{
				this.m_scoreRanking = int.Parse(array[0]);
			}
			if (array.Length > 1)
			{
				this.m_totalScoreRanking = int.Parse(array[1]);
			}
			else
			{
				this.m_totalScoreRanking = -1;
			}
			if (array.Length > 2)
			{
				this.m_rivalRanking = int.Parse(array[2]);
			}
			else
			{
				this.m_rivalRanking = -1;
			}
			if (array.Length > 3)
			{
				this.m_totalRivalRanking = int.Parse(array[3]);
			}
			else
			{
				this.m_totalRivalRanking = -1;
			}
			if (array.Length > 4)
			{
				this.m_sendPresent = int.Parse(array[4]);
			}
			else
			{
				this.m_sendPresent = -1;
			}
			if (array.Length > 5)
			{
				this.m_startDt = NetUtil.GetLocalDateTime(long.Parse(array[5]));
			}
			if (array.Length > 6)
			{
				this.m_endDt = NetUtil.GetLocalDateTime(long.Parse(array[6]));
			}
			if (array.Length > 7)
			{
				this.m_league = int.Parse(array[7]);
			}
			else
			{
				this.m_league = -1;
			}
			if (array.Length > 8)
			{
				this.m_oldLeague = int.Parse(array[8]);
			}
			else
			{
				this.m_oldLeague = -1;
			}
			if (array.Length > 9)
			{
				this.m_numLeagueMember = int.Parse(array[9]);
			}
			else
			{
				this.m_numLeagueMember = -1;
			}
			if (array.Length > 10)
			{
				this.m_sendPresentRival = int.Parse(array[10]);
			}
			else
			{
				this.m_sendPresentRival = -1;
			}
		}
		else
		{
			this.m_orgServerMessageInfo = null;
		}
	}

	public static RankingServerInfoConverter CreateLastServerInfo()
	{
		RankingServerInfoConverter result = null;
		if (!string.IsNullOrEmpty(RankingServerInfoConverter.ms_lastServerMessageInfo))
		{
			result = new RankingServerInfoConverter(RankingServerInfoConverter.ms_lastServerMessageInfo);
		}
		return result;
	}
}
