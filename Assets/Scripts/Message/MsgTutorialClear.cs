using System;
using System.Collections.Generic;
using Tutorial;

namespace Message
{
	public class MsgTutorialClear : MessageBase
	{
		public struct Data
		{
			public EventID eventid;
		}

		public List<MsgTutorialClear.Data> m_data = new List<MsgTutorialClear.Data>();

		public MsgTutorialClear(EventID id) : base(12336)
		{
			this.Add(id);
		}

		public void Add(EventID id)
		{
			MsgTutorialClear.Data item = default(MsgTutorialClear.Data);
			item.eventid = id;
			this.m_data.Add(item);
		}
	}
}
