using System;

namespace Message
{
	public class MsgTutorialQuickMode : MessageBase
	{
		public HudTutorial.Id m_id;

		public MsgTutorialQuickMode(HudTutorial.Id id) : base(12345)
		{
			this.m_id = id;
		}
	}
}
