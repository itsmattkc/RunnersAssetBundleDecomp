using System;

namespace Message
{
	public class MsgUpSpeedLevel : MessageBase
	{
		public PlayerSpeed m_level;

		public MsgUpSpeedLevel(PlayerSpeed level) : base(12303)
		{
			this.m_level = level;
		}
	}
}
