using System;

namespace Message
{
	public class MsgBossDistanceEnd : MessageBase
	{
		public bool m_end;

		public MsgBossDistanceEnd(bool end) : base(12324)
		{
			this.m_end = end;
		}
	}
}
