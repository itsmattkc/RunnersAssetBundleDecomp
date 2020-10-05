using System;

namespace Message
{
	public class MsgDisableInput : MessageBase
	{
		public bool m_disable;

		public MsgDisableInput(bool value) : base(12319)
		{
			this.m_disable = value;
		}
	}
}
