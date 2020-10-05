using Mission;
using System;
using System.Collections.Generic;

namespace Message
{
	public class MsgMissionEvent : MessageBase
	{
		public struct Data
		{
			public EventID eventid;

			public long num;
		}

		public List<MsgMissionEvent.Data> m_missions = new List<MsgMissionEvent.Data>();

		public MsgMissionEvent() : base(12318)
		{
		}

		public MsgMissionEvent(EventID id_) : base(12318)
		{
			this.Add(id_, 0L);
		}

		public MsgMissionEvent(EventID id_, long num) : base(12318)
		{
			this.Add(id_, num);
		}

		public void Add(EventID id_, long num)
		{
			MsgMissionEvent.Data item = default(MsgMissionEvent.Data);
			item.eventid = id_;
			item.num = num;
			this.m_missions.Add(item);
		}

		public void Add(EventID id_)
		{
			this.Add(id_, 0L);
		}
	}
}
