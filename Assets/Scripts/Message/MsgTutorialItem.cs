using System;

namespace Message
{
	public class MsgTutorialItem : MessageBase
	{
		public HudTutorial.Id m_id;

		public MsgTutorialItem(HudTutorial.Id id) : base(12341)
		{
			this.m_id = id;
		}
	}
}
