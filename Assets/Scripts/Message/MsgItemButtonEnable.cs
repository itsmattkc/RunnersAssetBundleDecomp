using System;

namespace Message
{
	public class MsgItemButtonEnable : MessageBase
	{
		public bool m_enable;

		public MsgItemButtonEnable(bool enable) : base(12298)
		{
			this.m_enable = enable;
		}
	}
}
