using System;

namespace Mission
{
	public class MissionEvent
	{
		public EventID m_id;

		public long m_num;

		public MissionEvent(EventID id, long num)
		{
			this.m_id = id;
			this.m_num = num;
		}
	}
}
