using System;
using System.Collections.Generic;
using Text;

public class EventRankingServerInfoConverter
{
	public enum MSG_INFO
	{
		EventId,
		PlayerRank,
		ParticipationNum,
		IsPresented,
		StartDt,
		EndDt,
		NUM
	}

	private string m_messageInfo;

	private int m_playerRank = -1;

	private int m_participationNum = -1;

	private int m_eventId = -1;

	private bool m_isPresented;

	public bool isInit
	{
		get
		{
			return this.m_messageInfo != null;
		}
	}

	public int eventId
	{
		get
		{
			return this.m_eventId;
		}
	}

	public string Result
	{
		get
		{
			if (!this.isInit)
			{
				return null;
			}
			string commonText;
			if (this.m_isPresented)
			{
				commonText = TextUtility.GetCommonText("Ranking", "ranking_result_event_text_2");
			}
			else
			{
				commonText = TextUtility.GetCommonText("Ranking", "ranking_result_event_text_1");
			}
			string eventName = this.GetEventName();
			return TextUtility.Replaces(commonText, new Dictionary<string, string>
			{
				{
					"{PARAM1}",
					eventName
				},
				{
					"{PARAM2}",
					this.m_playerRank.ToString()
				},
				{
					"{PARAM3}",
					this.m_participationNum.ToString()
				}
			});
		}
	}

	public EventRankingServerInfoConverter(string serverMessageInfo)
	{
		this.Setup(serverMessageInfo);
	}

	public void Setup(string serverMessageInfo)
	{
		if (serverMessageInfo == null)
		{
			return;
		}
		this.m_messageInfo = serverMessageInfo;
		string[] array = this.m_messageInfo.Split(new char[]
		{
			','
		});
		if (array != null && array.Length > 0)
		{
			if (array.Length > 0)
			{
				this.m_eventId = int.Parse(array[0]);
			}
			if (array.Length > 1)
			{
				this.m_playerRank = int.Parse(array[1]);
			}
			else
			{
				this.m_playerRank = 0;
			}
			if (array.Length > 2)
			{
				this.m_participationNum = int.Parse(array[2]);
			}
			else
			{
				this.m_participationNum = 0;
			}
			if (array.Length > 3)
			{
				this.m_isPresented = (int.Parse(array[3]) != 0);
			}
			else
			{
				this.m_isPresented = false;
			}
		}
		else
		{
			this.m_messageInfo = null;
		}
	}

	public string GetEventName()
	{
		string result = string.Empty;
		switch (EventManager.GetType(this.m_eventId))
		{
		case EventManager.EventType.SPECIAL_STAGE:
			result = HudUtility.GetEventStageName(EventManager.GetSpecificId(this.m_eventId));
			break;
		case EventManager.EventType.RAID_BOSS:
			result = HudUtility.GetEventStageName(EventManager.GetSpecificId(this.m_eventId));
			break;
		case EventManager.EventType.COLLECT_OBJECT:
			switch (EventManager.GetCollectEventType(this.m_eventId))
			{
			case EventManager.CollectEventType.GET_ANIMALS:
				result = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_animl_get_event");
				break;
			case EventManager.CollectEventType.GET_RING:
				result = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_ring_get_event");
				break;
			case EventManager.CollectEventType.RUN_DISTANCE:
				result = TextUtility.GetText(TextManager.TextType.TEXTTYPE_EVENT_COMMON_TEXT, "Menu", "ui_Lbl_word_run_distance_event");
				break;
			}
			break;
		}
		return result;
	}
}
