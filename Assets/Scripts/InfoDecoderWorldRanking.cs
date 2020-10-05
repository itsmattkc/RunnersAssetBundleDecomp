using System;
using Text;

public class InfoDecoderWorldRanking : InfoDecoder
{
	private string m_messageInfo = string.Empty;

	private RankingServerInfoConverter m_converter;

	public InfoDecoderWorldRanking(string messageInfo)
	{
		this.m_messageInfo = messageInfo;
		this.m_converter = new RankingServerInfoConverter(this.m_messageInfo);
	}

	public override string GetCaption()
	{
		string commonText = TextUtility.GetCommonText("Ranking", "ranking_result_all_caption");
		if (!string.IsNullOrEmpty(commonText))
		{
			return commonText;
		}
		return string.Empty;
	}

	public override string GetResultString()
	{
		return this.m_converter.rankingResultAllText;
	}

	public override string GetMedalSpriteName()
	{
		return "ui_ranking_world_sonicmedal_blue";
	}
}
