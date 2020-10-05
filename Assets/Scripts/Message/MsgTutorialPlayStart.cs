using System;
using Tutorial;

namespace Message
{
	public class MsgTutorialPlayStart : MessageBase
	{
		public readonly EventID m_eventID;

		public MsgTutorialPlayStart(EventID eventID) : base(12333)
		{
			this.m_eventID = eventID;
		}
	}
}
