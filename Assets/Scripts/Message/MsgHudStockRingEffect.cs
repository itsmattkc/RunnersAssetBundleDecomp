using System;

namespace Message
{
	public class MsgHudStockRingEffect : MessageBase
	{
		public bool m_off;

		public MsgHudStockRingEffect(bool off) : base(49157)
		{
			this.m_off = off;
		}
	}
}
