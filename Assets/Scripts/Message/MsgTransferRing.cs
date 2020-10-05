using System;

namespace Message
{
	public class MsgTransferRing
	{
		public int m_ring;

		public bool m_isContinue;

		public MsgTransferRing()
		{
		}

		public MsgTransferRing(int ring, bool isContinue)
		{
			this.m_ring = ring;
			this.m_isContinue = isContinue;
		}
	}
}
