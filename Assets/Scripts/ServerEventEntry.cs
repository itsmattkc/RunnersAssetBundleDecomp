using System;

public class ServerEventEntry
{
	private int m_eventId;

	private int m_eventType;

	private DateTime m_eventStartTime;

	private DateTime m_eventEndTime;

	private DateTime m_eventCloseTime;

	public int EventId
	{
		get
		{
			return this.m_eventId;
		}
		set
		{
			this.m_eventId = value;
		}
	}

	public int EventType
	{
		get
		{
			return this.m_eventType;
		}
		set
		{
			this.m_eventType = value;
		}
	}

	public DateTime EventStartTime
	{
		get
		{
			return this.m_eventStartTime;
		}
		set
		{
			this.m_eventStartTime = value;
		}
	}

	public DateTime EventEndTime
	{
		get
		{
			return this.m_eventEndTime;
		}
		set
		{
			this.m_eventEndTime = value;
		}
	}

	public DateTime EventCloseTime
	{
		get
		{
			return this.m_eventCloseTime;
		}
		set
		{
			this.m_eventCloseTime = value;
		}
	}

	public ServerEventEntry()
	{
		this.m_eventId = 0;
		this.m_eventType = 0;
		this.m_eventStartTime = NetBase.GetCurrentTime();
		this.m_eventEndTime = NetBase.GetCurrentTime();
		this.m_eventCloseTime = NetBase.GetCurrentTime();
	}

	public void CopyTo(ServerEventEntry to)
	{
		to.m_eventId = this.m_eventId;
		to.m_eventType = this.m_eventType;
		to.m_eventStartTime = this.m_eventStartTime;
		to.m_eventEndTime = this.m_eventEndTime;
		to.m_eventCloseTime = this.m_eventCloseTime;
	}
}
