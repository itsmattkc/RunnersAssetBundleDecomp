using System;
using Text;

public class InfoDecoderEvent : InfoDecoder
{
	private string m_messageInfo = string.Empty;

	private EventRankingServerInfoConverter m_converter;

	public InfoDecoderEvent(string messageInfo)
	{
		this.m_messageInfo = messageInfo;
		this.m_converter = new EventRankingServerInfoConverter(this.m_messageInfo);
	}

	public override string GetCaption()
	{
		string commonText = TextUtility.GetCommonText("Ranking", "ranking_result_event_caption");
		if (!string.IsNullOrEmpty(commonText))
		{
			return commonText;
		}
		return string.Empty;
	}

	public override string GetResultString()
	{
		return this.m_converter.Result;
	}

	public override string GetMedalSpriteName()
	{
		return "ui_ranking_world_sonicmedal_blue";
	}
}
