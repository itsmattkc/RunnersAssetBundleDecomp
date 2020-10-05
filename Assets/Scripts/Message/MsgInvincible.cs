using System;

namespace Message
{
	public class MsgInvincible : MessageBase
	{
		public enum Mode
		{
			Start,
			End
		}

		public MsgInvincible.Mode m_mode;

		public MsgInvincible(MsgInvincible.Mode mode) : base(12329)
		{
			this.m_mode = mode;
		}
	}
}
